using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Enums;

namespace TecnoMundo.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _repository;

        public CouponController(ICouponService repository)
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
        [Authorize(Roles = nameof(Role.Admin))]
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
