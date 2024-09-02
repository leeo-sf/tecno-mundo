using AutoMapper;
using TecnoMundo.Identity.Data.ValueObjects;
using TecnoMundo.Identity.Model;
using TecnoMundo.IdentityAPI.Data.ValueObjects;

namespace TecnoMundo.Identity.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<UserVO, User>();
                config.CreateMap<AuthenticateVO, Authenticate>();
            });

            return config;
        }
    }
}
