using AutoMapper;
using TecnoMundo.CartAPI.Command;
using TecnoMundo.CartAPI.Config;
using TecnoMundo.CartAPI.Model.Context;
using TecnoMundo.CartAPI.RabbitMQSender;
using TecnoMundo.CartAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options => options
    .UseMySql(connection,
        new MySqlServerVersion(
            new Version(8, 0, 36))));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "CorsPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration["CorsPolicy:TecnoMundo-Web-Http"],
            builder.Configuration["CorsPolicy:TecnoMundo-Web-Https"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddScoped<ICartRepoository, CartRepository>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ISaveOrUpdateCart, SaveOrUpdateCart>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<ICouponRepository, CouponRepository>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"]));
builder.Services.AddHttpClient<IProductRepository, ProductRepository>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]));


//Adicionando configurações de segurança
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Authentication:Key").Value);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("Authentication:UrlAuthentication").Value,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Authentication:Scope").Value,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        //garantir que o usuário esteja autenticado
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", $"{builder.Configuration["Authentication:Scope"]}");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TecnoMundo.Cart", Version = "v1" });
    c.EnableAnnotations();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        //Passa no cabeçalho da request o token
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
            new List<string> ()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
