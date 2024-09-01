using AutoMapper;
using TecnoMundo.ProductAPI.Data.ValueObjects;
using TecnoMundo.ProductAPI.Model;
using TecnoMundo.ProductAPI.Data.ValueObjects;

namespace TecnoMundo.ProductAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                //entra um productvo e vira product
                config.CreateMap<ProductVO, Product>()
                    .ForMember(x => x.Category, opt => opt.Ignore());
                config.CreateMap<Product, ProductVO>();

                config.CreateMap<ProductCategory, CategoryVO>();

                config.CreateMap<CreateProductVO, Product>();
            });

            return config;
        }
    }
}
