using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);
var structure = new DependencyInjectionCoupon(builder.Services, builder.Configuration);

structure.AddDbContext();

structure.AddCorsPolicy();

structure.AddScopedAndDependencies();

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
structure.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
structure.AddInfrastructureSwagger(apiName: "Coupon");
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
