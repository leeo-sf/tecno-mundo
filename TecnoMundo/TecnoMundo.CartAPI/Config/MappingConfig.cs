using AutoMapper;
using TecnoMundo.CartAPI.Data.ValueObjects;
using TecnoMundo.CartAPI.Model;

namespace TecnoMundo.CartAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>()
                    .ReverseMap();
                config.CreateMap<CategoryVO, ProductCategory>()
                    .ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>()
                    .ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>()
                    .ReverseMap();
                config.CreateMap<CartVO, Cart>()
                    .ReverseMap();
            });

            return mappingConfig;
        }
    }
}
