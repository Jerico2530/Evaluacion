using BiblotecaClase.Datos;
using Evaluacion.Mapping;
using Evaluacion.Repository;
using Evaluacion.Repository.IRepository;
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

// Repositorys y servicios
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITuitionRepository, TuitionRepository>();
builder.Services.AddScoped<IStateTuitionRepository, StateTuitionRepository>();

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
