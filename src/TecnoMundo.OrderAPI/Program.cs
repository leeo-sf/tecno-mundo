using GeekShopping.OrderAPI.MessageConsumer;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);
var structure = new DependencyInjectionOrder(builder.Services, builder.Configuration);

structure.AddDbContext();

structure.AddCorsPolicy();

structure.AddScopedAndDependencies();
builder.Services.AddHostedService<RabbitMQCheckoutConsumer>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
structure.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

structure.AddInfrastructureSwagger(apiName: "Order");

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
