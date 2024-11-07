using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Domain.Entities;
using TecnoMundo.Domain.Interfaces;

namespace TecnoMundo.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CouponVO> CreateCoupon(CreateCouponVO vo)
        {
            var coupon = _mapper.Map<Coupon>(vo);
            var couponCreated = await _repository.CreateCoupon(coupon);
            return _mapper.Map<CouponVO>(couponCreated);
        }

        public async Task<CouponVO?> GetCouponByCouponCode(string couponCode)
        {
            var coupon = await _repository.GetCouponByCouponCode(couponCode);
            return _mapper.Map<CouponVO>(coupon);
        }
    }
}
