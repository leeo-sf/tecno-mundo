using TecnoMundo.CartAPI.Data.ValueObjects;

namespace TecnoMundo.CartAPI.Repository
{
    public interface ICouponRepository
    {
        Task<CouponVO> GetCouponByCouponCode(Guid couponCode, string token);
    }
}
