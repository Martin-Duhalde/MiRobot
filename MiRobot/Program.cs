// Estado simulado del robot
var robot = new
{
    Id = 1,
    Name = "Robot-T800"
};
var posicion = new Position(0, 0, 0);
var status = "Active";


// Minimal API 

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


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


// Robot
app.MapGet("/Robot", () => Results.Ok(new
{
    robot.Id,
    robot.Name,
    Status = status,
    Axes = 3,
    Message = "Robot ready"
}));

// Posición actual
app.MapGet("/Robot/Posicion", () => Results.Ok(posicion));

// Mover un eje o los tres
app.MapPost("/Robot/Posicion", (Position request) =>
{
    posicion = request;

    return Results.Ok(new
    {
        Status = "Moved",
        Position = posicion
    });
});

// Estado
app.MapGet("/Robot/Estado", () => Results.Ok(new
{
    Status = status,
    IsMoving = false,
    Temperature = 35.5,
    Battery = 98
}));

// Home: volver al origen
app.MapPost("/Robot/Home", () =>
{
    posicion = new Position(0, 0, 0);

    return Results.Ok(new
    {
        Status = "Homed",
        Position = posicion
    });
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();

// Modelo de posición
record Position(double X, double Y, double Z);