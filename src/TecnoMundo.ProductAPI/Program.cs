using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);
var structure = new DependencyInjectionProduct(builder.Services, builder.Configuration);

structure.AddDbContext();

// Adicionando scope
structure.AddCorsPolicy();

structure.AddScopedAndDependencies();

// Add services to the container.

builder.Services.AddControllers();

//Adicionando configura��es de seguran�a
structure.AddAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Adicionando defini��o de seguran�a
structure.AddInfrastructureSwagger(apiName: "Products");

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
