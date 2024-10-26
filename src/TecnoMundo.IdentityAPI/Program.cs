using TecnoMundo.Identity.Service;
using TecnoMundo.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);
var structure = new DependencyInjectionIdentity(builder.Services, builder.Configuration);

// connection DB
structure.AddDbContext();

// Adicionando CORS
structure.AddCorsPolicy();

// Adicionar scopeds
builder.Services.AddScoped<ITokenService, TokenService>();
structure.AddScopedAndDependencies();

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
