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
    public class DependencyInjectionIdentity
    {
        public static IServiceCollection AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("MySQLConnection").GetSection("MySQLConnectionString").Value;

            services.AddDbContext<ApplicationDbContextIdentity>(options =>
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
            );

            return services;
        }

        public static void AddInfrastructureDbContext(IServiceCollection services)
        {
            IMapper mapper = DomainToDTOMappingIdentity.RegisterMaps().CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}
