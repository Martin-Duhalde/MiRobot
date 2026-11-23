using MiRobot;

using Microsoft.OpenApi;

using System.Security.Cryptography;
using System.Text;
using System.Threading.RateLimiting;

//// Estado simulado del robot

Robot robot = new(id: 1, name: "Robot-T800");

//// Minimal API 

#region builder

var builder = WebApplication.CreateBuilder(args);

// Configuration
var apiKey       /**/ = builder.Configuration["MiRobot:ApiKey"];
var omitirApiKey /**/ = builder.Configuration.GetValue<bool>("MiRobot:OmitirApiKey");

if (string.IsNullOrWhiteSpace(apiKey) || apiKey.Length < 32)
{
    /// Para pruebas locales en desarrollo escribir esto en powershell:
    ///   [Environment]::SetEnvironmentVariable("MiRobot__ApiKey", "RobotDemo-2026-MiClave-Segura-123456789", "User")
    throw new InvalidOperationException("MiRobot:ApiKey must contain at least 32 characters.");
}

// Swagger
builder.Services.AddEndpointsApiExplorer();
// Autorizacion para usar la api con swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "API Key",
        Description = "Ingresá tu API Key."
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
});

// Error handling
builder.Services.AddProblemDetails();

// Rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter
        .Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 60,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                }));
});

#endregion

#region app

var app = builder.Build();

// Production middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRateLimiter();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy"
}));

#endregion

#region robotApi 

var robotApi = app.MapGroup("/Robot");

if (!omitirApiKey) // para pruebas en desarrollo sin Api Key 
{
    // Endpoint Filter: intercepta todas las solicitudes del grupo /Robot
    // antes de ejecutar el endpoint. Valida la API Key enviada en el
    // header X-Api-Key y rechaza con 401 Unauthorized si no es válida.
    robotApi.AddEndpointFilter(async (context, next) =>
        {
            var authorization = context.HttpContext
                .Request.Headers.Authorization.ToString();

            var suppliedKey = authorization.StartsWith(
                "Bearer ", StringComparison.OrdinalIgnoreCase)
                    ? authorization["Bearer ".Length..].Trim()
                    : string.Empty;

            var expected = Encoding.UTF8.GetBytes(apiKey);
            var supplied = Encoding.UTF8.GetBytes(suppliedKey);

            var valid = supplied.Length == expected.Length &&
                CryptographicOperations.FixedTimeEquals(
                    supplied, expected);

            if (!valid)
                return Results.Unauthorized();

            return await next(context);
        });
}

#endregion

#region Web

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@picocss/pico@2/css/pico.min.css">
    <meta charset="UTF-8">
    <title>Robot API</title>
</head>
<body>
    <main class="container">
        <h1>🤖 Robot API</h1>
        <h2>Robot - 3 Axis Simulator</h2>

        <h3>Servicios</h3>
        <ul>
            <li><a href="/Robot">Robot</a></li>
            <li><a href="/Robot/Posicion">Posición</a></li>
            <li><a href="/Robot/Estado">Estado</a></li>
        </ul>
        
        <h3>Documentación</h3>
        <a href="/swagger" role="button">Abrir Swagger UI</a>
    </main>
</body>
</html>
""", "text/html; charset=utf-8"));

#endregion

#region Api

// Robot
robotApi.MapGet("/", () => Results.Ok(new
{
    robot.Id,
    robot.Name,
    robot.Status,
    Axes = 3,
    Message = "Robot ready"
}));

// Posición actual
robotApi.MapGet("/Posicion", () => Results.Ok(robot.Position));

// Mover un eje o los tres
robotApi.MapPost("/Posicion", (Position request) =>
{
    if (!request.IsValid())
        return Results.BadRequest("Posición inválida.");

    robot.Position = request;
    robot.Status = "Moved";

    return Results.Ok(new
    {
        robot.Status,
        robot.Position
    });
});

// Estado
robotApi.MapGet("/Estado", () => Results.Ok(new
{
    robot.Status,
    IsMoving = false,
    Temperature = 35.5,
    Battery = 98
}));

// Home: volver al origen
robotApi.MapPost("/Home", () =>
{
    robot.Position = new Position(0, 0, 0);

    return Results.Ok(new
    {
        Status = "Homed",
        robot.Position
    });
});

#endregion

app.Run();