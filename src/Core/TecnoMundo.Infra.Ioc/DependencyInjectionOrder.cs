using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TecnoMundo.Application.Interfaces;
using TecnoMundo.Application.RabbitMQServer;
using TecnoMundo.Application.Services;
using TecnoMundo.Domain.Interfaces;
using TecnoMundo.Infra.Data.Context;
using TecnoMundo.Infra.Data.Repositories;
using TecnoMundo.ProductAPI.Caching;

namespace TecnoMundo.Infra.Ioc
{
    public class DependencyInjectionOrder : DependencyInjection
    {
        public DependencyInjectionOrder(IServiceCollection services,
            IConfiguration configuration) : base(services, configuration) { }

        public override IServiceCollection AddDbContext()
        {
            var connection = _config.GetSection("MySQLConnection").GetSection("MySQLConnectionString").Value;

            _service.AddDbContext<ApplicationDbContextOrder>(options =>
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
            );

            var builderDbContext = new DbContextOptionsBuilder<ApplicationDbContextOrder>();
            builderDbContext.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)));

            _service.AddSingleton(new OrderRepository(builderDbContext.Options));

            return _service;
        }

        public override void AddScopedAndDependencies()
        {
            _service.AddScoped<IOrderRepository, OrderRepository>();
            _service.AddScoped<IOrderService, OrderService>();
            _service.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            _service.AddScoped<ICachingService, CachingService>();
            _service.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _config.GetSection("Redis").GetSection("Host").Value;
            });
        }
    }
}
