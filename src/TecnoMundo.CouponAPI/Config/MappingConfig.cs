using AutoMapper;
using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Data.ValueObjects;
using TecnoMundo.CouponAPI.Model;

namespace TecnoMundo.CouponAPI.Config
{
    public class MappingConfig
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
