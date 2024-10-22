using System.Text;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

DependencyInjection.AddInfrastructureDbContext(builder.Services, builder.Configuration);

// Adicionando scope
DependencyInjection.AddCorsPolicy(builder.Services, builder.Configuration);

DependencyInjectionProduct.AddInfrastructureDbContext(builder.Services);

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
DependencyInjection.AddAuthentication(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Adicionando defini��o de seguran�a
DependencyInjection.AddInfrastructureSwagger(builder.Services);

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
