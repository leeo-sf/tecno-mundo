using GeekShopping.CartAPI.Command;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CartAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepoository _cartRepostory;
        private readonly ISaveOrUpdateCart _saveOrUpdate;
        private readonly ICouponRepository _couponRepostory;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        public CartController(
            ICartRepoository repository,
            ISaveOrUpdateCart insertCart,
            ICouponRepository couponRepository,
            IRabbitMQMessageSender rabbitMQMessageSender
        )
        {
            _cartRepostory = repository;
            _couponRepostory = couponRepository;
            _saveOrUpdate = insertCart;
            _rabbitMQMessageSender = rabbitMQMessageSender;
        }

        [HttpGet("find-cart/{userId}")]
        public async Task<ActionResult<CartVO>> FindById(Guid userId)
        {
            var cart = await _cartRepostory.FindCartByUserId(userId);
            if (cart == null)
                return NotFound();
            return Ok(cart);
        }

        [HttpPost("add-cart")]
        public async Task<ActionResult<CartVO>> AddCart(CartVO vo)
        {
            try
            {
                var cart = await _saveOrUpdate.Execute(vo);
                if (cart == null)
                    return NotFound();
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-cart")]
        public async Task<ActionResult<CartVO>> UpdateCart(CartVO vo)
        {
            try
            {
                var cart = await _saveOrUpdate.Execute(vo);
                if (cart == null)
                    return NotFound();
                return Ok(cart);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove-cart/{id}")]
        public async Task<ActionResult<CartVO>> RemoveCart(Guid id)
        {
            var status = await _cartRepostory.RemoveFromCart(id);
            if (status == null)
                return BadRequest();
            return Ok(status);
        }

        [HttpDelete("clear/{userId}")]
        public async Task<ActionResult<bool>> ClearCart(Guid userId)
        {
            var status = await _cartRepostory.ClearCart(userId);

            return Ok(status);
        }

        [HttpPost("apply-coupon/{userId}")]
        public async Task<ActionResult<bool>> ApplyCouponToCart(
            [FromRoute] Guid userId,
            [FromHeader(Name = "Coupon-Code")] string couponCode
        )
        {
            string token = Request.Headers["Authorization"];
            CouponVO coupon = await _couponRepostory.GetCouponByCouponCode(
                couponCode,
                token.Replace("Bearer ", "")
            );
            if (coupon.Id == Guid.Empty)
                return BadRequest();

            var status = await _cartRepostory.ApplyCoupon(userId, couponCode);

            return Ok(status);
        }

        [HttpPost("remove-coupon")]
        public async Task<ActionResult<bool>> RemoveCouponToCart([FromHeader] Guid userId)
        {
            var status = await _cartRepostory.RemoveCoupon(userId);

            return Ok(status);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
        {
            string token = Request.Headers["Authorization"];
            if (vo?.UserId == null)
                return BadRequest();
            var cart = await _cartRepostory.FindCartByUserId(vo.UserId);
            if (cart == null)
                return NotFound();
            if (!string.IsNullOrEmpty(vo.CouponCode))
            {
                CouponVO coupon = await _couponRepostory.GetCouponByCouponCode(
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

            await _cartRepostory.ClearCart(vo.UserId);

            return Ok(vo);
        }
    }
}
