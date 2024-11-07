using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Mappings
{
    public class DomainToDTOMappingProduct
    {
        public static MapperConfiguration RegisterMaps()
        {
            var config = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>();
                config.CreateMap<Product, ProductVO>();

                config.CreateMap<ProductCategory, CategoryVO>().ReverseMap();
                config.CreateMap<CreateProductVO, Product>();
            });

            return config;
        }
    }
}
