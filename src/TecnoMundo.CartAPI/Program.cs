using TecnoMundo.CartAPI.RabbitMQSender;
using TecnoMundo.CartAPI.Service;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

DependencyInjectionCart.AddDbContext(builder.Services, builder.Configuration);

DependencyInjection.AddCorsPolicy(builder.Services, builder.Configuration);

DependencyInjectionCart.AddInfrastructureDbContext(builder.Services);

builder.Services.AddScoped<IServiceCoupon, ServiceCoupon>();
builder.Services.AddScoped<IServiceProduct, ServiceProduct>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient<IServiceCoupon, ServiceCoupon>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"] ?? "")
);
builder.Services.AddHttpClient<IServiceProduct, ServiceProduct>(s =>
    s.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"] ?? "")
);

//Adicionando configura��es de seguran�a
DependencyInjection.AddAuthentication(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
DependencyInjection.AddInfrastructureSwagger(builder.Services, "Cart");

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
