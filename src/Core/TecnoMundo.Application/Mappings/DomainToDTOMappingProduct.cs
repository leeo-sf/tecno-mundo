using AutoMapper;
using TecnoMundo.Domain.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Mappings
{
    public class DomainToDTOMappingProduct
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                config
                    .CreateMap<ProductVO, Product>()
                    .ForMember(x => x.Category, opt => opt.Ignore());
                config.CreateMap<Product, ProductVO>();

                config.CreateMap<ProductCategory, CategoryVO>();
                config.CreateMap<CreateProductVO, Product>();
            });

            return config;
        }
    }
}
