using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TecnoMundo.Identity.Commands;
using TecnoMundo.Identity.Config;
using TecnoMundo.Identity.Model.Context;
using TecnoMundo.Identity.Repository;
using TecnoMundo.Identity.Service;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

builder.Services.AddDbContext<MySQLContext>(options =>
    options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 36)))
);

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

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddScoped<IInsertUser, InsertUser>();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
