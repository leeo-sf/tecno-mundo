using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

DependencyInjectionCoupon.AddDbContext(builder.Services, builder.Configuration);

DependencyInjection.AddCorsPolicy(builder.Services, builder.Configuration);

DependencyInjectionCoupon.AddInfrastructureDbContext(builder.Services);

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
DependencyInjection.AddAuthentication(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
DependencyInjection.AddInfrastructureSwagger(builder.Services, "Coupon");
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
