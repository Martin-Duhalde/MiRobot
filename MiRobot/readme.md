# Configurar ApiKey la en el entorno del proceso
$env:MiRobot__ApiKey = "RobotDemo-2026-MiClave-Segura-123456789"


# Configurarla en powershell para el usuario wiindos
[Environment]::SetEnvironmentVariable("MiRobot__ApiKey","RobotDemo-2026-MiClave-Segura-123456789","User")