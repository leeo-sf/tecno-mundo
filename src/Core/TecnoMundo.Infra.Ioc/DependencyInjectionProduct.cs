using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.Mappings;
using TecnoMundo.Application.Services;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;
using TecnoMundo.Infra.Data.Repositories;

namespace TecnoMundo.Infra.Ioc
{
    public class DependencyInjectionProduct : DependencyInjection
    {
        public DependencyInjectionProduct(IServiceCollection services,
            IConfiguration configuration) : base(services, configuration) { }

        public override IServiceCollection AddDbContext()
        {
            var connection = _config.GetSection("MySQLConnection").GetSection("MySQLConnectionString").Value;

            _service.AddDbContext<ApplicationDbContextProduct>(options =>
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
            );

            return _service;
        }

        public override void AddScopedAndDependencies()
        {
            IMapper mapper = DomainToDTOMappingProduct.RegisterMaps().CreateMapper();
            _service.AddSingleton(mapper);
            _service.AddScoped<IProductRepository, ProductRepository>();
            _service.AddScoped<IProductService, ProductService>();
        }
    }
}
