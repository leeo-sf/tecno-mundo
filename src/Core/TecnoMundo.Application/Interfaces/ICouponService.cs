using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Interfaces
{
    public interface ICouponService
    {
        Task<CouponVO?> GetCouponByCouponCode(string couponCode);
        Task<CouponVO> CreateCoupon(CreateCouponVO vo);
    }
}
