using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace TecnoMundo.Infra.Ioc
{
    public abstract class DependencyInjection
    {
        protected readonly IServiceCollection _service;
        protected readonly IConfiguration _config;

        public DependencyInjection(IServiceCollection service, IConfiguration configuration)
        {
            _service = service;
            _config = configuration;
        }

        public IServiceCollection AddCorsPolicy()
        {
            _service.AddCors(opt =>
            {
                opt.AddPolicy(
                    name: "CorsPolicy",
                    policy =>
                    {
                        policy
                            .WithOrigins(
                                _config.GetSection("CorsPolicy:TecnoMundo-Web-Http").Value ?? "",
                                _config.GetSection("CorsPolicy:TecnoMundo-Web-Https").Value ?? ""
                            )
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            });
            return _service;
        }

        public IServiceCollection AddAuthentication()
        {
            var key = Encoding.ASCII.GetBytes(_config.GetSection("Authentication:Key").Value ?? "");
            _service
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _config.GetSection("Authentication:UrlAuthentication").Value,
                        ValidateAudience = true,
                        ValidAudience = _config.GetSection("Authentication:Scope").Value,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true
                    };
                });

            _service.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ApiScope",
                    policy =>
                    {
                        //garantir que o usu�rio esteja autenticado
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("scope", $"{_config["Authentication:Scope"]}");
                    }
                );
            });

            return _service;
        }

        public IServiceCollection AddInfrastructureSwagger(string apiName)
        {
            _service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo { Title = $"TecnoMundo.{apiName}", Version = "v1" }
                );
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
            return _service;
        }

        public abstract IServiceCollection AddDbContext();

        public abstract void AddScopedAndDependencies();
    }
}
