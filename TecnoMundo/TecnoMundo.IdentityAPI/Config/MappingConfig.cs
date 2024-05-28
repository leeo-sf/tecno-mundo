using AutoMapper;
using GeekShopping.Identity.Data.ValueObjects;
using GeekShopping.Identity.Model;

namespace GeekShopping.Identity.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<UserVO, User>();
            });

            return config;
        }
    }
}
