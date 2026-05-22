#!/usr/bin/env bash
# Script de arranque para Linux/macOS
# Levanta toda la pila (Postgres + API + Frontend) y abre el navegador

set -e
COMPOSE_FILE="backend/docker-compose.yml"

echo ""
echo "=============================================="
echo " EVM Project Management - Arranque local"
echo "=============================================="
echo ""

echo "[1/4] Liberando puertos..."
lsof -ti:4200 | xargs kill -9 2>/dev/null || true
lsof -ti:5000 | xargs kill -9 2>/dev/null || true
pkill -f "node" 2>/dev/null || true
pkill -f "dotnet" 2>/dev/null || true
echo "Puertos 4200 y 5000 liberados."

echo ""
echo "[2/4] Construyendo y levantando contenedores..."
docker compose -f "$COMPOSE_FILE" up -d --build

echo ""
echo "[3/4] Esperando a que la API este lista..."
MAX_RETRIES=30
API_READY=false
for i in $(seq 1 $MAX_RETRIES); do
    if curl -sf -o /dev/null http://localhost:5000/api/projects 2>/dev/null; then
        API_READY=true
        break
    fi
    sleep 2
done

if [ "$API_READY" = false ]; then
    echo "La API no respondio en 60 segundos. Revisa los logs con:"
    echo "  docker compose -f $COMPOSE_FILE logs"
    exit 1
fi

echo "API lista."

echo ""
echo "[4/4] Abriendo URLs en el navegador..."

if command -v xdg-open >/dev/null 2>&1; then
    OPEN_CMD=xdg-open
elif command -v open >/dev/null 2>&1; then
    OPEN_CMD=open
else
    OPEN_CMD=""
fi

URLS=(
    "http://localhost:4200"
    "http://localhost:5000/swagger-ui"
    "http://localhost:5000/api-docs/v1/swagger.json"
)

if [ -n "$OPEN_CMD" ]; then
    for url in "${URLS[@]}"; do
        $OPEN_CMD "$url" >/dev/null 2>&1 || true
    done
else
    echo "(No se detecto xdg-open/open; abre manualmente las URLs)"
fi

echo ""
echo "=============================================="
echo " Servicios disponibles:"
echo "=============================================="
echo "  Frontend:     http://localhost:4200"
echo "  API:          http://localhost:5000"
echo "  Swagger UI:   http://localhost:5000/swagger-ui"
echo "  OpenAPI spec: http://localhost:5000/api-docs/v1/swagger.json"
echo ""
echo "Para detener todo: docker compose -f $COMPOSE_FILE down"
echo ""
echo "Mostrando logs (Ctrl+C para salir; los contenedores seguiran corriendo):"
echo ""

docker compose -f "$COMPOSE_FILE" logs -f
