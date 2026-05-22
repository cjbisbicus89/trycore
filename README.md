# EVM Project Management

Sistema para gestionar proyectos y calcular automáticamente los indicadores de **Earned Value Management (EVM)** — una técnica de gestión de proyectos que te dice si estás por encima o por debajo del presupuesto y del cronograma.

---

## 🚀 Cómo correr el proyecto (lo más simple)

Si tenés Docker Desktop instalado, esto es todo lo que necesitás:

**Windows:**
```powershell
.\start.ps1
```

**Linux / macOS:**
```bash
chmod +x start.sh
./start.sh
```

### ¿Qué hace ese comando?

1. **Limpia puertos** — Cierra cualquier programa que esté usando los puertos 4200 y 5000
2. **Levanta la base de datos** — PostgreSQL en un contenedor Docker
3. **Levanta la API** — El backend en .NET 8
4. **Levanta el frontend** — La aplicación en Angular
5. **Abre tu navegador** — Automáticamente abre:
   - La aplicación principal: http://localhost:4200
   - La documentación de la API: http://localhost:5000/swagger-ui
   - El archivo de especificación de la API: http://localhost:5000/api-docs/v1/swagger.json

### ¿Cómo detenerlo?

Abre una terminal y ejecutá:
```bash
docker compose -f backend/docker-compose.yml down
```

---

## 📋 ¿Qué necesitas?

| Opción | Requisito |
|---|---|
| La opción rápida (Docker) | [Docker Desktop](https://www.docker.com/products/docker-desktop/) instalado |
| Desarrollo manual | .NET 8 SDK, Node.js 20+, PostgreSQL 16 |

---

## 🏗️ Otras formas de correr el proyecto

### Opción 2 — Docker sin el script

Si preferís usar Docker Compose directamente, abrí **dos terminales**:

**Terminal 1 — levantar la pila:**
```bash
docker compose -f backend/docker-compose.yml up --build
```

**Terminal 2 — abrir el navegador** (una vez que veas `Application started` en los logs):

```powershell
# Windows
Start-Process "http://localhost:4200"; Start-Process "http://localhost:5000/swagger-ui"
```
```bash
# Linux / macOS
open "http://localhost:4200" && open "http://localhost:5000/swagger-ui"
```

### Opción 3 — Sin Docker (para desarrollo)

Si querés modificar el código y ver los cambios en tiempo real:

**Paso 1 — Base de datos:**
```bash
docker compose -f backend/docker-compose.yml up -d db
```

**Paso 2 — Backend** (en una terminal):
```bash
cd backend
dotnet run --project src/EVM.ProjectManagement.API
```

**Paso 3 — Frontend** (en otra terminal):
```bash
cd frontend
npm install
npm start
```

**Paso 4 — Abrir el navegador** (una vez que veas `Application bundle generation complete` en la terminal del frontend):

```powershell
# Windows
Start-Process "http://localhost:4200"; Start-Process "http://localhost:5000/swagger-ui"
```
```bash
# Linux / macOS
open "http://localhost:4200" && open "http://localhost:5000/swagger-ui"
```

---

## 🗄️ Inicialización de la base de datos

La base de datos se inicializa automáticamente cuando levantas el proyecto con Docker — no tenés que hacer nada manual.

Si necesitás inicializarla manualmente (por ejemplo, en un servidor de producción):

```bash
psql -U evm_user -d evm_db -f db/init.sql
```

El archivo `db/init.sql` está en la raíz del proyecto y contiene todo el esquema de la base de datos.

---

## 🧪 Cómo correr los tests

```bash
cd backend
dotnet test
```

Esto ejecuta todos los tests unitarios y de integración del backend.

---

## 📁 Estructura del proyecto

```
trycore/
├── backend/          # API en .NET 8
│   ├── src/          # Código de la aplicación
│   ├── tests/        # Tests
│   └── docker-compose.yml
├── frontend/         # Aplicación en Angular
│   ├── src/app/      # Código de la aplicación
│   └── Dockerfile
├── db/
│   └── init.sql      # Script de inicialización de la BD
├── start.ps1         # Script de arranque (Windows)
├── start.sh          # Script de arranque (Linux/macOS)
└── README.md         # Este archivo
```

---

## 📚 Documentación de la API

Con el proyecto corriendo, la documentación interactiva está en:
- http://localhost:5000/swagger-ui

Ahí podés ver todos los endpoints, probarlos directamente desde el navegador, y ver los ejemplos de request/response.

---

## 🔧 Tecnologías usadas

**Backend:**
- .NET 8 (ASP.NET Core)
- Entity Framework Core
- PostgreSQL
- xUnit (tests)

**Frontend:**
- Angular 21
- TailwindCSS
- ng2-charts (gráficos)

**Infraestructura:**
- Docker
- Nginx

---

## 📝 Historial de commits

El repositorio usa Gitflow. El historial completo de commits y Pull Requests está visible en GitHub para ver la evolución del proyecto.
