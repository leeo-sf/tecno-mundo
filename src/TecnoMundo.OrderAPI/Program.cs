using System.Text;
using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.Model.Context;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
);

var builderDbContext = new DbContextOptionsBuilder<MySQLContext>();
builderDbContext.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)));

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(
        name: "CorsPolicy",
        policy =>
        {
            policy
                .WithOrigins(
                    builder.Configuration["CorsPolicy:TecnoMundo-Web-Http"],
                    builder.Configuration["CorsPolicy:TecnoMundo-Web-Https"]
                )
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    );
});

builder.Services.AddSingleton(new OrderRepository(builderDbContext.Options));
builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Authentication:Key").Value);
builder
    .Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder
                .Configuration.GetSection("Authentication:UrlAuthentication")
                .Value,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Authentication:Scope").Value,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "ApiScope",
        policy =>
        {
            //garantir que o usu�rio esteja autenticado
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", $"{builder.Configuration["Authentication:Scope"]}");
        }
    );
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TecnoMundo.Order", Version = "v1" });
    c.EnableAnnotations();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
