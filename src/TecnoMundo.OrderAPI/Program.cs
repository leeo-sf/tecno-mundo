using GeekShopping.OrderAPI.MessageConsumer;
using GeekShopping.OrderAPI.RabbitMQSender;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

DependencyInjectionOrder.AddDbContext(builder.Services, builder.Configuration);

DependencyInjection.AddCorsPolicy(builder.Services, builder.Configuration);

DependencyInjectionOrder.AddInfrastructureDbContext(builder.Services);
builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();
builder.Services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
DependencyInjection.AddAuthentication(builder.Services, builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

DependencyInjection.AddInfrastructureSwagger(builder.Services, "Order");

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
