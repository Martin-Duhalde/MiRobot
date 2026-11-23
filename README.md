# 🤖 MiRobot

API REST desarrollada con **.NET 10 Minimal API** para simular el control de un robot.

El proyecto es una demostración práctica de cómo construir, proteger, probar y versionar una API utilizando herramientas del ecosistema .NET.

## Características

- **Minimal API:** endpoints HTTP con una estructura simple y directa.
- **Control del robot:** consulta y modificación de su posición.
- **Autenticación:** protección mediante API Key enviada en el encabezado `Authorization: Bearer`.
- **Rate Limiting:** limitación de solicitudes para evitar un uso excesivo de la API.
- **Swagger / OpenAPI:** documentación y pruebas interactivas.
- **Git y GitHub:** control de versiones y sincronización del código.

> El robot es simulado: no requiere hardware ni controla un dispositivo físico.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Visual Studio 2026 o Visual Studio Code (opcional).

## Ejecutar el proyecto

Clonar el repositorio:

```bash
git clone https://github.com/Martin-Duhalde/MiRobot.git
cd MiRobot
```

Ejecutar la aplicación:

```bash
dotnet run
```

La dirección local y el puerto se muestran en la consola al iniciar la aplicación.

## Autenticación

En los entornos donde está habilitada la autenticación, las solicitudes deben incluir la API Key:

```http
Authorization: Bearer TU_API_KEY
```

La clave se configura localmente y no debe publicarse en el repositorio.

## Tutoriales

Este proyecto acompaña una serie de videos donde se construye y evoluciona la API paso a paso.

1. **[Parte 1 – Creación de una API con .NET 10 Minimal API](https://www.youtube.com/watch?v=wj29GvhJ6oE&t=1021s)**
2. **[Parte 2 – Seguridad, API Key y Rate Limiting](https://www.youtube.com/watch?v=5nbVbyIiVyU&t=1316s)**
3. **[Parte 3 – Git y control de versiones desde Visual Studio](https://www.youtube.com/watch?v=IyswEc1DJVY)**
4. **[Parte 4 – GitHub, clonado de repositorios y sincronización de cambios](https://www.youtube.com/watch?v=BNMbZ_1z3w4&t=316s)**

## Tecnologías

- C#
- .NET 10
- ASP.NET Core Minimal API
- OpenAPI / Swagger
- Git
- GitHub

## Autor

**Martín Duhalde**

[GitHub – Martin-Duhalde](https://github.com/Martin-Duhalde)