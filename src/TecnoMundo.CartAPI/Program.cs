using TecnoMundo.CartAPI.Service;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);
var structure = new DependencyInjectionCart(builder.Services, builder.Configuration);

structure.AddDbContext();

structure.AddCorsPolicy();

structure.AddScopedAndDependencies();
builder.Services.AddScoped<IServiceCoupon, ServiceCoupon>();
builder.Services.AddScoped<IServiceProduct, ServiceProduct>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<IServiceCoupon, ServiceCoupon>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"] ?? "")
);
builder.Services.AddHttpClient<IServiceProduct, ServiceProduct>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"] ?? "")
);

//Adicionando configura��es de seguran�a
structure.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
structure.AddInfrastructureSwagger(apiName: "Cart");

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
