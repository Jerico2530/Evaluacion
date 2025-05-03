using BiblotecaClase.Datos;
using Evaluacion.Mapping;
using Evaluacion.Repositorio;
using Evaluacion.Repositorio.IRepositorio;
using Evaluacion.Service;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Agregar servicios
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de DbContext
builder.Services.AddDbContext<BackendContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string cadenaConexion = configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Cadena de conexión no configurada.");
    options.UseSqlServer(cadenaConexion);
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

// Repositorios y servicios
builder.Services.AddScoped<IEstudianteRepositorio, EstudianteRepositorio>();
builder.Services.AddScoped<ICursoRepositorio, CursoRepositorio>();
builder.Services.AddScoped<IMatriculaRepositorio, MatriculaRepositorio>();
builder.Services.AddScoped<MatriculaService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
