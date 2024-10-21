using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("MySQLConnection").GetSection("MySQLConnectionString").Value;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
            );

            return services;
        }

        public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TecnoMundo.Products", Version = "v1" });
                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = @"Enter 'Bearer' [space] and your token!",
                        Name = "Authorization",
                        //Passa no cabe�alho da request o token
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    }
                );

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
                    }
                );
            });
            return services;
        }
    }
}
