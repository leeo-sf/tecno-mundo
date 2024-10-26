using TecnoMundo.CartAPI.RabbitMQSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.CartAPI.Service;

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

        public CartController(
            ICartService service,
            IServiceCoupon couponRepository,
            IServiceProduct productRepository,
            IRabbitMQMessageSender rabbitMQMessageSender
        )
        {
            _service = service;
            _couponService = couponRepository;
            _productService = productRepository;
            _rabbitMQMessageSender = rabbitMQMessageSender;
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(Guid userId)
        {
            var cart = await _service.FindCartByUserId(userId);
            if (cart.CartHeader is null)
                return NotFound(new { errorMessage = "No cart found for this account." });

            try
            {
                if (!cart.CartDetails.IsNullOrEmpty())
                    cart = await _productService.GetProductsByListCart(cart);
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

                var cart = await _service.SaveOrUpdate(vo, productVO);
                if (cart == null)
                    return NotFound();
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

                var cart = await _service.SaveOrUpdate(vo, productVO);
                if (cart == null)
                    return NotFound();
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(Guid id)
        {
            var status = await _service.RemoveFromCart(id);
            if (!status)
                return NotFound(status);
            return Ok(status);
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult<bool>> ClearCart(Guid userId)
        {
            var status = await _service.ClearCart(userId);

            return Ok(status);
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
                return BadRequest();

            var status = await _service.ApplyCoupon(userId, couponCode);

            return Ok(status);
        }

        [HttpPost("remove-coupon")]
        public async Task<ActionResult<bool>> RemoveCouponToCart([FromHeader] Guid userId)
        {
            var status = await _service.RemoveCoupon(userId);

            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];
            if (vo?.UserId == null)
                return BadRequest();
            var cart = await _service.FindCartByUserId(vo.UserId);
            if (cart == null)
                return NotFound();
            if (!string.IsNullOrEmpty(vo.CouponCode))
            {
                CouponVO coupon = await _couponService.GetCouponByCouponCode(
                    vo.CouponCode,
                    token.Replace("Bearer ", "")
                );
                if (vo.DiscountAmount != coupon.DiscountAmount)
                {
                    return StatusCode(412);
                }
            }
            vo.CartDetails = cart.CartDetails;
            vo.DateTime = DateTime.Now;

            _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

            await _service.ClearCart(vo.UserId);

            return Ok(vo);
        }
    }
}
