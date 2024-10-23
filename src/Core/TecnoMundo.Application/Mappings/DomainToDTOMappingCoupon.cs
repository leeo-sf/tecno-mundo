using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Mappings
{
    public class DomainToDTOMappingCoupon
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponVO, Coupon>().ReverseMap();

                config.CreateMap<CreateCouponVO, Coupon>();
            });

            return mappingConfig;
        }
    }
}
