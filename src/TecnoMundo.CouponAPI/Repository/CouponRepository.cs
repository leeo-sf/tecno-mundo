using AutoMapper;
using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Model;
using TecnoMundo.CouponAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.CouponAPI.Data.ValueObjects;

namespace TecnoMundo.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly MySQLContext _context;
        private readonly IMapper _mapper;

        public CouponRepository(MySQLContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(c => c.CouponCode == couponCode);

            return _mapper.Map<CouponVO>(coupon);
        }

        public async Task<CouponVO> CreateCoupon(CreateCouponVO couponVO)
        {
            var coupon = _mapper.Map<Coupon>(couponVO);

            var couponExists = _context.Coupon
                .AsNoTracking()
                .Any(x => x.CouponCode == coupon.CouponCode);

            if (couponExists) throw new ApplicationException("Coupon code already exists");

            _context.Coupon.Add(coupon);
            await _context.SaveChangesAsync();
            return _mapper.Map<CouponVO>(coupon);
        }
    }
}
