# EVM Project Management

Aplicación fullstack para gestión de proyectos con cálculo automático de los indicadores de **Earned Value Management (EVM)**: PV, EV, CV, SV, CPI, SPI, EAC y VAC, con su interpretación automática (bajo presupuesto / sobre presupuesto / adelantado / atrasado).

---

## Tabla de contenidos

- [Stack tecnológico](#stack-tecnológico)
- [Arquitectura](#arquitectura)
- [Requisitos previos](#requisitos-previos)
- [Cómo correr el proyecto localmente](#cómo-correr-el-proyecto-localmente)
  - [Opción 1 — Un solo comando (recomendada)](#opción-1--un-solo-comando-recomendada)
  - [Opción 2 — Docker Compose manual](#opción-2--docker-compose-manual)
  - [Opción 3 — Ejecución sin Docker](#opción-3--ejecución-sin-docker)
- [Inicialización de la base de datos](#inicialización-de-la-base-de-datos)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Cómo correr los tests](#cómo-correr-los-tests)
- [Documentación de la API (OpenAPI / Swagger)](#documentación-de-la-api-openapi--swagger)
- [Gitflow y convenciones](#gitflow-y-convenciones)

---

## Stack tecnológico

**Backend**
- .NET 8 (ASP.NET Core)
- Entity Framework Core 8 + Npgsql
- PostgreSQL 16
- xUnit + FluentAssertions + Moq (tests)
- Swashbuckle / OpenAPI

**Frontend**
- Angular 21 (standalone components)
- TailwindCSS
- ng2-charts (Chart.js)
- Signals + Reactive Forms
- TypeScript estricto (sin `any`)

**Infraestructura**
- Docker / Docker Compose
- Nginx (frontend)

---

## Arquitectura

El backend sigue **Clean Architecture** en cuatro capas:

```
EVM.ProjectManagement.Domain         → Entidades, Value Objects, lógica EVM pura
EVM.ProjectManagement.Application    → Casos de uso, DTOs, validadores
EVM.ProjectManagement.Infrastructure → EF Core, repositorios, migraciones
EVM.ProjectManagement.API            → Controllers, middleware, Swagger
```

La lógica de cálculo EVM vive en el dominio (`EVMCalculationService`) y es 100% pura, sin dependencias externas — esto la hace trivialmente testeable.

---

## Requisitos previos

| Para usar | Necesitás |
|---|---|
| Opción 1 y 2 (Docker) | [Docker Desktop](https://www.docker.com/products/docker-desktop/) 24+ con Docker Compose v2 |
| Opción 3 (sin Docker) | [.NET 8 SDK](https://dotnet.microsoft.com/download), [Node.js 20+](https://nodejs.org), [PostgreSQL 16](https://www.postgresql.org/download/) |

---

## Cómo correr el proyecto localmente

### Opción 1 — Un solo comando (recomendada)

Levanta toda la pila (Postgres + API + Frontend), espera al healthcheck y abre las URLs en el navegador automáticamente.

**Windows (PowerShell):**
```powershell
.\start.ps1
```

**Linux / macOS:**
```bash
chmod +x start.sh
./start.sh
```

El script:
1. Construye y levanta los contenedores en segundo plano
2. Espera hasta que la API responda en `http://localhost:5000`
3. Abre automáticamente en el navegador:
   - **Frontend** → http://localhost:4200
   - **Swagger UI** → http://localhost:5000/swagger-ui
   - **OpenAPI spec** → http://localhost:5000/api-docs/v1/swagger.json
4. Muestra los logs en vivo (`Ctrl+C` para cerrar el seguimiento; los contenedores siguen corriendo)

Para detener y limpiar:
```bash
docker compose -f backend/docker-compose.yml down       # mantiene los datos
docker compose -f backend/docker-compose.yml down -v    # borra el volumen de Postgres
```

---

### Opción 2 — Docker Compose manual

Si preferís controlar Docker Compose directamente, sin el script wrapper:

```bash
docker compose -f backend/docker-compose.yml up --build
```

Una vez levantado, abrí manualmente en el navegador:
- http://localhost:4200 — Frontend
- http://localhost:5000/swagger-ui — Swagger UI
- http://localhost:5000/api-docs/v1/swagger.json — OpenAPI spec

---

### Opción 3 — Ejecución sin Docker

Útil para desarrollo activo (debugging, hot reload, etc.).

**1. Levantar PostgreSQL** (cualquiera de estas formas):

```bash
# Opción A: solo la BD vía Docker
docker compose -f backend/docker-compose.yml up -d db

# Opción B: PostgreSQL local
psql -U postgres -c "CREATE USER evm_user WITH PASSWORD 'evm_pass';"
psql -U postgres -c "CREATE DATABASE evm_db OWNER evm_user;"
psql -U evm_user -d evm_db -f db/init.sql
```

**2. Backend** (puerto 5001):
```bash
cd backend
dotnet restore
dotnet run --project src/EVM.ProjectManagement.API
```

**3. Frontend** (puerto 4200, en otra terminal):
```bash
cd frontend
npm install
npm start
```

> **Nota:** en modo desarrollo el frontend apunta a `http://localhost:5000/api` (ver `src/environments/environment.ts`). Si corrés el backend en `5001`, ajustá esa URL o iniciá la API con `ASPNETCORE_URLS=http://+:5000`.

---

## Inicialización de la base de datos

Hay **dos formas equivalentes** de inicializar el esquema:

### Automática (recomendada)
La API ejecuta `Database.Migrate()` al arrancar (ver `Program.cs:RunMigrations`). Aplica todas las migraciones de EF Core en orden y es idempotente — si la BD ya está al día, no hace nada.

### Manual con `db/init.sql`
Para entornos donde no se quiere que la app aplique migraciones (por ejemplo, ambientes con DBA separado), se incluye el script consolidado e idempotente:

```bash
psql -U evm_user -d evm_db -f db/init.sql
```

El script `db/init.sql` se genera con:
```bash
cd backend
dotnet ef migrations script --idempotent \
  --project src/EVM.ProjectManagement.Infrastructure \
  --startup-project src/EVM.ProjectManagement.API \
  --output ../db/init.sql
```

---

## Estructura del proyecto

```
trycore/
├── backend/
│   ├── src/
│   │   ├── EVM.ProjectManagement.Domain/         # Entidades, Value Objects, EVMCalculationService
│   │   ├── EVM.ProjectManagement.Application/    # Servicios de aplicación, DTOs, validadores
│   │   ├── EVM.ProjectManagement.Infrastructure/ # EF Core, repositorios, migraciones
│   │   └── EVM.ProjectManagement.API/            # Controllers, Swagger, middleware
│   ├── tests/
│   │   ├── EVM.ProjectManagement.UnitTests/        # Tests de dominio y aplicación
│   │   └── EVM.ProjectManagement.IntegrationTests/ # Tests end-to-end de los endpoints
│   ├── docker-compose.yml
│   ├── Dockerfile
│   └── EVM.ProjectManagement.sln
├── frontend/
│   ├── src/app/
│   │   ├── core/         # Servicios HTTP, modelos, interceptores, constantes
│   │   ├── shared/       # Componentes y pipes reutilizables
│   │   ├── features/     # Lista de proyectos
│   │   └── dashboard/    # Dashboard EVM (tabla, summary, chart, modal)
│   ├── Dockerfile
│   └── nginx.conf
├── db/
│   └── init.sql          # Script SQL idempotente generado desde EF Core migrations
├── start.ps1             # Script de arranque para Windows
├── start.sh              # Script de arranque para Linux/macOS
└── README.md
```

---

## Cómo correr los tests

**Tests unitarios + de integración (backend):**
```bash
cd backend
dotnet test
```

**Tests por separado:**
```bash
dotnet test tests/EVM.ProjectManagement.UnitTests
dotnet test tests/EVM.ProjectManagement.IntegrationTests
```

Los tests cubren:
- Cálculo EVM (todos los indicadores) con casos borde: `AC = 0`, `PV = 0`, `BAC = 0`, sin actividades, avance real cero
- Invariantes de dominio (validaciones en `Activity` y `Project`)
- Servicios de aplicación (mocks de repositorios)
- Contrato HTTP de cada endpoint (status codes, schemas de response)

---

## Documentación de la API (OpenAPI / Swagger)

Con la pila levantada, los endpoints quedan documentados en:

- **Swagger UI:** http://localhost:5000/swagger-ui
- **OpenAPI spec (JSON):** http://localhost:5000/api-docs/v1/swagger.json

Cada endpoint incluye descripción, esquemas de request/response y todos los códigos de error posibles (200/201/204/400/404/500).

---

## Gitflow y convenciones

El repositorio sigue **Gitflow estricto**:

- `main` → producción. Solo recibe merges desde `release/*`.
- `develop` → integración. Todas las features se mergean acá vía Pull Request.
- `feature/*` → una rama por funcionalidad.
- `release/v1.0.0` → estabilización antes del merge final a `main`.

**Mensajes de commit:** descriptivos, en imperativo, con prefijo de tipo (`feat:`, `fix:`, `refactor:`, `test:`, `docs:`, `chore:`).

Ejemplos:
- `feat: Add EVM calculation service`
- `fix: Fix CPI edge case when AC is zero`
- `test: Add integration tests for activities endpoints`

El historial de Pull Requests fusionados puede consultarse en el repositorio para ver la evolución del proyecto.
