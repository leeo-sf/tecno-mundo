using TecnoMundo.Identity.Service;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

// connection DB
DependencyInjectionIdentity.AddDbContext(builder.Services, builder.Configuration);

// Adicionando CORS
DependencyInjection.AddCorsPolicy(builder.Services, builder.Configuration);

// Adicionar scopeds
builder.Services.AddScoped<ITokenService, TokenService>();
DependencyInjectionIdentity.AddInfrastructureDbContext(builder.Services);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
