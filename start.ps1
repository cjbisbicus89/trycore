# Script de arranque para Windows (PowerShell)
# Levanta toda la pila (Postgres + API + Frontend) y abre el navegador

$ErrorActionPreference = "Stop"
$ComposeFile = "backend/docker-compose.yml"

Write-Host ""
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host " EVM Project Management - Arranque local" -ForegroundColor Cyan
Write-Host "==============================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "[1/3] Construyendo y levantando contenedores..." -ForegroundColor Yellow
docker compose -f $ComposeFile up -d --build
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error al levantar los contenedores." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "[2/3] Esperando a que la API este lista..." -ForegroundColor Yellow
$maxRetries = 30
$apiReady = $false
for ($i = 1; $i -le $maxRetries; $i++) {
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5000/api/projects" -TimeoutSec 2 -UseBasicParsing -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            $apiReady = $true
            break
        }
    } catch {
        Start-Sleep -Seconds 2
    }
}

if (-not $apiReady) {
    Write-Host "La API no respondio en 60 segundos. Revisa los logs con:" -ForegroundColor Red
    Write-Host "  docker compose -f $ComposeFile logs" -ForegroundColor Red
    exit 1
}

Write-Host "API lista." -ForegroundColor Green

Write-Host ""
Write-Host "[3/3] Abriendo URLs en el navegador..." -ForegroundColor Yellow
Start-Process "http://localhost:4200"
Start-Process "http://localhost:5000/swagger-ui"
Start-Process "http://localhost:5000/api-docs/v1/swagger.json"

Write-Host ""
Write-Host "==============================================" -ForegroundColor Green
Write-Host " Servicios disponibles:" -ForegroundColor Green
Write-Host "==============================================" -ForegroundColor Green
Write-Host "  Frontend:     http://localhost:4200"
Write-Host "  API:          http://localhost:5000"
Write-Host "  Swagger UI:   http://localhost:5000/swagger-ui"
Write-Host "  OpenAPI spec: http://localhost:5000/api-docs/v1/swagger.json"
Write-Host ""
Write-Host "Para detener todo: docker compose -f $ComposeFile down" -ForegroundColor Cyan
Write-Host ""
Write-Host "Mostrando logs (Ctrl+C para salir; los contenedores seguiran corriendo):" -ForegroundColor Yellow
Write-Host ""

docker compose -f $ComposeFile logs -f
