using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.RabbitMQServer;
using TecnoMundo.CartAPI.Service;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _service;
        private readonly IServiceCoupon _couponService;
        private readonly IServiceProduct _productService;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;
        private readonly IConfiguration _configuration;
        private readonly DistributedCacheEntryOptions _options;
        private readonly string _keyCache;

        public CartController(
            ICartService service,
            IServiceCoupon couponRepository,
            IServiceProduct productRepository,
            IRabbitMQMessageSender rabbitMQMessageSender,
            IConfiguration configuration
        )
        {
            _service = service;
            _couponService = couponRepository;
            _productService = productRepository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
            _configuration = configuration;
            _keyCache = _configuration.GetSection("Redis").GetSection("Key_Cache").Value ?? "cart";
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Absolute_Expire").Value
                            ?? "3600"
                    )
                ),
                SlidingExpiration = TimeSpan.FromSeconds(
                    double.Parse(
                        _configuration.GetSection("Redis").GetSection("Sliding_Expire").Value
                            ?? "600"
                    )
                )
            };
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(Guid userId)
        {
            var cart = await _service.FindCartByUserId(userId, $"{_keyCache}-{userId}", _options);
            if (cart == null)
                return NotFound(new { errorMessage = "No cart found for this account." });

            try
            {
                if (cart?.CartDetails?.FirstOrDefault()?.Product == null)
                {
                    cart = await _productService.GetProductsByListCart(cart);
                    await _service.AddCartVOInCache(
                        cart,
                        $"{_keyCache}-{cart.CartHeader.UserId}",
                        _options
                    );
                }
            }
            catch (ApplicationException ex)
            {
                return StatusCode(404, new { erroMessage = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { erroMessage = ex.Message });
            }

            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            try
            {
                var productVO = await _productService.GetProductById(
                    vo.CartDetails.FirstOrDefault().ProductId
                );

                if (productVO.Id == Guid.Empty)
                {
                    throw new ArgumentException("Product id invalid.");
                }

                var cart = await _service.SaveOrUpdate(
                    vo,
                    productVO,
                    $"{_keyCache}-{vo.CartHeader.UserId}",
                    _options
                );
                /*if (cart == null)
                    return NotFound();*/
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            try
            {
                var productVO = await _productService.GetProductById(
                    vo.CartDetails.FirstOrDefault().ProductId
                );

                if (productVO.Id == Guid.Empty)
                {
                    throw new ArgumentException("Product id invalid.");
                }

                var cart = await _service.SaveOrUpdate(
                    vo,
                    productVO,
                    $"{_keyCache}-{vo.CartHeader.UserId}",
                    _options
                );
                if (cart == null)
                    return NotFound();
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [HttpDelete("remove-cart/{cartDetailsId}")]
        public async Task<ActionResult<CartVO>> RemoveCart(Guid cartDetailsId)
        {
            var status = await _service.RemoveFromCart(cartDetailsId, _keyCache, _options);
            if (!status)
                return NotFound(new { Status = status });
            return Ok(new { Status = status });
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult<bool>> ClearCart(Guid userId)
        {
            var status = await _service.ClearCart(userId, $"{_keyCache}-{userId}");

            return Ok(new { Status = status });
        }

        [HttpPost("apply-coupon/{userId}")]
        public async Task<ActionResult<bool>> ApplyCouponToCart(
            [FromRoute] Guid userId,
            [FromHeader(Name = "Coupon-Code")] string couponCode
        )
        {
            string token = Request.Headers["Authorization"].ToString();
            CouponVO coupon = await _couponService.GetCouponByCouponCode(
                couponCode,
                token.Replace("Bearer ", "")
            );
            if (coupon.Id == Guid.Empty)
                return BadRequest(new { errorMessage = "Error when searching for coupon." });

            var status = await _service.ApplyCoupon(
                userId,
                couponCode,
                $"{_keyCache}-{userId}",
                _options
            );

            return Ok(new { Status = status });
        }

        [HttpPost("remove-coupon")]
        public async Task<ActionResult<bool>> RemoveCouponToCart([FromHeader] Guid userId)
        {
            var status = await _service.RemoveCoupon(userId, $"{_keyCache}-{userId}", _options);

            return Ok(new { Status = status });
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"].ToString();
            if (vo?.UserId == null)
                return BadRequest();
            var cart = await _service.FindCartByUserId(
                vo.UserId,
                $"{_keyCache}-{vo.UserId}",
                _options
            );
            if (cart == null)
                return NotFound(new { errorMessage = "Cart not found." });
            if (!string.IsNullOrEmpty(vo.CouponCode))
            {
                CouponVO coupon = await _couponService.GetCouponByCouponCode(
                    vo.CouponCode,
                    token.Replace("Bearer ", "")
                );
                if (vo.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(
                        412,
                        new
                        {
                            errorMessage = "The coupon discount has been changed, we recommend applying again."
                        }
                    );
                }
            }
            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            var dataSendToRabbitMQ = new DataServerRabbitMQ<CheckoutHeaderVO>(
                hostName: _configuration.GetSection("RabbitMQServer:HostName").Value ?? "",
                password: _configuration.GetSection("RabbitMQServer:Password").Value ?? "",
                userName: _configuration.GetSection("RabbitMQServer:Username").Value ?? "",
                virtualHost: _configuration.GetSection("RabbitMQServer:VirtualHost").Value ?? "",
                queueName: "checkoutqueue",
                baseMessage: vo
            );
            _rabbitMQMessageSender.SendMessage(dataSendToRabbitMQ);

            await _service.ClearCart(vo.UserId, $"{_keyCache}-{vo.UserId}");

            return Ok(vo);
        }
    }
}
