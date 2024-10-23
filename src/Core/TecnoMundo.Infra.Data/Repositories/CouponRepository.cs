using Microsoft.EntityFrameworkCore;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Data.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContextCoupon _context;

        public CouponRepository(ApplicationDbContextCoupon context)
        {
            _context = context;
        }

        public async Task<Coupon?> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _context.Coupon.FirstOrDefaultAsync(c => c.CouponCode == couponCode);

            return coupon;
        }

        public async Task<Coupon> CreateCoupon(Coupon coupon)
        {
            var couponExists = _context
                .Coupon.AsNoTracking()
                .Any(x => x.CouponCode == coupon.CouponCode);

            if (couponExists)
                throw new ApplicationException("Coupon code already exists");

            _context.Coupon.Add(coupon);
            await _context.SaveChangesAsync();
            return coupon;
        }
    }
}
