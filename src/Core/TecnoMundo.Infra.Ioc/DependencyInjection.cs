using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TecnoMundo.Infra.Data.Context;

namespace TecnoMundo.Infra.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCorsPolicy(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(
                    name: "CorsPolicy",
                    policy =>
                    {
                        policy
                            .WithOrigins(
                                configuration.GetSection("CorsPolicy:TecnoMundo-Web-Http").Value ?? "",
                                configuration.GetSection("CorsPolicy:TecnoMundo-Web-Https").Value ?? ""
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });
            return services;
        }

        public static IServiceCollection AddAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("Authentication:Key").Value ?? "");
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetSection("Authentication:UrlAuthentication")
                            .Value,
                        ValidateAudience = true,
                        ValidAudience = configuration.GetSection("Authentication:Scope").Value,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ApiScope",
                    policy =>
                    {
                        //garantir que o usu�rio esteja autenticado
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", $"{configuration["Authentication:Scope"]}");
                    }
                );
            });

            return services;
        }

        public static IServiceCollection AddInfrastructureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetSection("MySQLConnection").GetSection("MySQLConnectionString").Value;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
            );

            return services;
        }

        public static IServiceCollection AddInfrastructureSwagger(IServiceCollection services)
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
