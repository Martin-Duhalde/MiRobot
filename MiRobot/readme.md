# 🤖 MiRobot — Robot API

API REST desarrollada con **ASP.NET Core Minimal API (.NET 10)** que simula el funcionamiento de un robot de tres ejes.

El proyecto demuestra cómo construir una API pequeña y funcional incorporando mecanismos básicos de seguridad, control de solicitudes y documentación interactiva.

## Características

- **Minimal API:** endpoints definidos directamente con ASP.NET Core.
- **Robot simulado:** estado, posición y movimientos de tres ejes.
- **API Key:** autenticación mediante `Authorization: Bearer`.
- **Swagger / OpenAPI:** documentación y pruebas interactivas con soporte para Bearer Token.
- **Rate Limiting:** límite global de 60 solicitudes por minuto por dirección IP.
- **Problem Details:** infraestructura para respuestas de errores estandarizadas.
- **HTTPS:** redirección a HTTPS y HSTS fuera del entorno de desarrollo.
- **Configuración externa:** API Key almacenada en variables de entorno, no en el código fuente.

## Requisitos

- Windows / PowerShell.
- .NET 10 SDK.
- Visual Studio 2026 o CLI de .NET.

## Configuración

La API requiere una clave de al menos 32 caracteres.

La configuración utiliza la variable de entorno:

`MiRobot__ApiKey`

El doble guion bajo (`__`) representa el separador jerárquico de configuración de .NET.

### Opción 1: Configurar para la sesión actual

En PowerShell:

```powershell
$env:MiRobot__ApiKey = "RobotDemo-2026-MiClave-Segura-123456789"
```

La variable estará disponible únicamente durante esa sesión y para los procesos iniciados desde ella.

### Opción 2: Configurar para el usuario de Windows

En PowerShell:

```powershell
[Environment]::SetEnvironmentVariable(
    "MiRobot__ApiKey",
    "RobotDemo-2026-MiClave-Segura-123456789",
    "User"
)
```

Cerrar y volver a abrir la terminal o Visual Studio para que los nuevos procesos reciban la variable.

Para eliminarla:

```powershell
[Environment]::SetEnvironmentVariable(
    "MiRobot__ApiKey",
    $null,
    "User"
)
```

**Importante:** la clave de ejemplo es exclusivamente para pruebas. En otros entornos, utilizar una clave aleatoria, privada y diferente. No subir claves reales al repositorio.

## Ejecución

Desde la carpeta del proyecto:

```powershell
dotnet restore
dotnet run
```

La dirección HTTPS y el puerto dependerán de la configuración de lanzamiento del proyecto.

## Swagger

En entorno de desarrollo, Swagger UI permite consultar y ejecutar los endpoints.

Abrir:

`https://localhost:<puerto>/swagger`

Para autenticar las solicitudes:

1. Seleccionar **Authorize**.
2. Ingresar la API Key.
3. Confirmar.

El esquema configurado es HTTP Bearer, por lo que Swagger envía:

```http
Authorization: Bearer TU_API_KEY
```

No es necesario escribir manualmente el prefijo `Bearer` en el cuadro de autorización de Swagger.

## API Endpoints

Todos los endpoints bajo `/Robot` requieren autenticación Bearer, salvo que se habilite explícitamente la omisión de API Key.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/health` | Verifica disponibilidad de la API. |
| GET | `/` | Página de inicio. |
| GET | `/Robot` | Información general del robot. |
| GET | `/Robot/Posicion` | Consulta la posición actual. |
| POST | `/Robot/Posicion` | Modifica la posición de los tres ejes. |
| GET | `/Robot/Estado` | Consulta el estado del robot. |
| POST | `/Robot/Home` | Retorna el robot al origen. |

### Ejemplo: consultar el robot

```http
GET /Robot
Authorization: Bearer TU_API_KEY
```

### Ejemplo: consultar posición

```http
GET /Robot/Posicion
Authorization: Bearer TU_API_KEY
```

### Ejemplo: mover el robot

```http
POST /Robot/Posicion
Authorization: Bearer TU_API_KEY
Content-Type: application/json
```

```json
{
    "x": 10,
    "y": 20,
    "z": 30
}
```

Los valores deben cumplir las validaciones definidas por el modelo `Position`.

### Ejemplo: volver al origen

```http
POST /Robot/Home
Authorization: Bearer TU_API_KEY
```

## Seguridad

### Autenticación Bearer

La API utiliza una API Key enviada mediante el encabezado HTTP estándar `Authorization: Bearer`.

La validación se implementa mediante un **Endpoint Filter** aplicado al grupo `/Robot`.

Características:

- La clave no está almacenada en el código fuente.
- Se valida la longitud y se compara mediante `CryptographicOperations.FixedTimeEquals`.
- Las solicitudes sin credenciales válidas reciben `401 Unauthorized`.
- La API Key se obtiene desde la configuración de .NET.

### Rate Limiting

Se configura un limitador global de ventana fija:

| Parámetro | Valor |
|---|---|
| Estrategia | Fixed Window |
| Límite | 60 solicitudes |
| Ventana | 1 minuto |
| Partición | Dirección IP |
| Cola | Deshabilitada |
| Rechazo | HTTP 429 |

Cuando se supera el límite, la API responde con `429 Too Many Requests`.

### Entornos

- **Development:** Swagger habilitado y posibilidad de omitir la autenticación mediante configuración.
- **Production:** Swagger deshabilitado, manejo de excepciones, HSTS y autenticación obligatoria.

Para omitir la API Key durante pruebas locales se puede configurar:

```json
{
    "MiRobot": {
        "OmitirApiKey": true
    }
}
```

No habilitar esta opción en producción.

## Estructura conceptual

```text
MiRobot/
├── Program.cs
├── Robot.cs
├── Position.cs
├── MiRobot.csproj
└── README.md
```

La implementación mantiene una arquitectura intencionalmente sencilla: un único proceso, estado simulado en memoria y endpoints agrupados por funcionalidad.

## Alcance

Este proyecto es una demostración educativa de construcción de APIs con ASP.NET Core.

No representa un sistema de control industrial ni implementa comunicación con hardware real, persistencia de datos o autorización basada en usuarios y roles.

---

**Tecnologías:** C# · .NET 10 · ASP.NET Core Minimal API · OpenAPI · Swagger · HTTP Bearer Authentication · Rate Limiting