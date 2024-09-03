using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Data.ValueObjects;

namespace TecnoMundo.CouponAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(string couponCode);
        Task<CouponVO> CreateCoupon(CreateCouponVO createCouponVO);
    }
}
