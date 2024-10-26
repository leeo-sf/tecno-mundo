using TecnoMundo.Application.DTOs;

namespace TecnoMundo.CartAPI.Service
{
    public interface IServiceCoupon
    {
        Task<CouponVO> GetCouponByCouponCode(string couponCode, string token);
    }
}
