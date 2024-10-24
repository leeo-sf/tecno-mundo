using AutoMapper;
using TecnoMundo.Application.DTOs;
using TecnoMundo.Domain.Entities;

namespace TecnoMundo.Application.Mappings
{
    public class DomainToDTOMappingCart
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductVO, Product>().ReverseMap();
                config.CreateMap<CategoryVO, ProductCategory>().ReverseMap();
                config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
                config.CreateMap<CartVO, Cart>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
