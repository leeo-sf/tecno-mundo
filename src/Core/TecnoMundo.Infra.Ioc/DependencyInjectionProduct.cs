using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.Mappings;
using TecnoMundo.Application.Services;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Repositories;

namespace TecnoMundo.Infra.Ioc
{
    public static class DependencyInjectionProduct
    {
        public static void AddInfrastructureDbContext(this IServiceCollection services)
        {
            IMapper mapper = DomainToDTOMappingProduct.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
        }
    }
}
