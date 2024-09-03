using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Repository;

namespace TecnoMundo.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _repository;

        public CouponController(ICouponRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{couponCode}")]
        public async Task<ActionResult<CouponVO>> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _repository.GetCouponByCouponCode(couponCode);
            if (coupon == null)
                return NotFound("Invalid coupon. Try again!");
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<ActionResult<CouponVO>> CreateCoupon([FromBody] CreateCouponVO couponVO)
        {
            try
            {
                var coupon = await _repository.CreateCoupon(couponVO);
                return Ok(coupon);
            }
            catch (Exception ex) when (ex is ApplicationException || ex is DbUpdateException)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
