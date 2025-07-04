using EducationalCoursesAPI.Infrastructure.Data;
using EducationalCoursesAPI.Infrastructure.Repositories;
using EducationalCoursesAPI.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración de DbContext y PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();

// Servicios de negocio
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();

// CORS (permitir todos los orígenes en desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EducationalCoursesAPI", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer.\r\n\r\n Escribe 'Bearer' [espacio] y luego tu token en la caja de texto de abajo.\r\n\r\nEjemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Eliminada la llamada a DataSeeder.SeedAsync, ya no se hace seeding de datos falsos.

// Middleware de CORS
app.UseCors("AllowAll");

// Middleware para restringir acceso por IP
var allowedIps = new[]
{
    "187.155.101.200", // IP del usuario (escuela)
    "35.230.45.39",    // IP de Render
    "34.82.41.62",     // Otra IP de Render detectada
    "200.94.113.148",  // IP de casa del usuario
    "127.0.0.1",       // localhost IPv4
    "::1"              // localhost IPv6
    // Puedes agregar aquí la IP de Render cuando la identifiques en los logs
};

app.Use(async (context, next) =>
{
    string? remoteIp = context.Connection.RemoteIpAddress?.ToString();

    // Intenta obtener la IP real desde el header si existe (Render suele usar X-Forwarded-For)
    if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
    {
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            // Puede haber varias IPs separadas por coma, toma la primera
            remoteIp = forwardedFor.Split(',')[0].Trim();
        }
    }

    // Permitir acceso si la IP está en la lista
    if (!allowedIps.Contains(remoteIp))
    {
        // Loguea la IP para identificar la de Render en los logs
        Console.WriteLine($"Intento de acceso denegado desde IP: {remoteIp}");
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Acceso denegado: IP no permitida.");
        return;
    }

    await next();
});

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

// Aquí se mapearán los endpoints y controladores
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
