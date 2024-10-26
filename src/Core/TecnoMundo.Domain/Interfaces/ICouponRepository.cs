using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Domain.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetCouponByCouponCode(string couponCode);
        Task<Coupon> CreateCoupon(Coupon createCouponVO);
    }
}
