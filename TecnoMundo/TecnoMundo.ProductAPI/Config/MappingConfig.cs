using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using TecnoMundo.ProductAPI.Data.ValueObjects;

namespace GeekShopping.ProductAPI.Config
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
