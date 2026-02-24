# Readstat - Setup Guide

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/) and [Docker Compose](https://docs.docker.com/compose/install/)
- [Git](https://git-scm.com/)

## Quick Start

```bash
git clone https://github.com/gregusio/readstat.git
cd readstat
```

### 1. Configure environment variables

Copy the example environment file and fill in your own secrets:

```bash
cp .env.example .env
```

Open `.env` in your editor and update the values:

| Variable | Description | Required |
|----------|-------------|----------|
| `SA_PASSWORD` | SQL Server admin password (must meet [complexity requirements](https://learn.microsoft.com/en-us/sql/relational-databases/security/password-policy)) | ✅ |
| `JWT_KEY` | Secret key for signing JWT tokens (min 32 chars). Generate one with `openssl rand -base64 48` | ✅ |
| `JWT_ISSUER` | JWT issuer claim | Optional (default: `readstat`) |
| `JWT_AUDIENCE` | JWT audience claim | Optional (default: `readstat`) |
| `ALLOWED_ORIGINS` | Comma-separated list of allowed frontend URLs for CORS | Optional (default: `http://localhost:3000`) |
| `VITE_API_BASE_URL` | Backend API URL as seen from the browser | Optional (default: `http://localhost:5027/api`) |
| `BACKEND_PORT` | Port the backend listens on | Optional (default: `5027`) |
| `FRONTEND_PORT` | Port the frontend is served on | Optional (default: `3000`) |
| `ASPNETCORE_ENVIRONMENT` | ASP.NET environment (`Development` / `Production`) | Optional (default: `Development`) |

> **⚠️ Important:** The `.env` file contains sensitive values and is excluded from version control via `.gitignore`. **Never commit it to the repository.**

### 2. Start the application

```bash
docker compose up -d
```

Wait for all services to become healthy (this may take a minute on first run while SQL Server initializes):

```bash
docker compose ps
```

### 3. Access the app

| Service | URL |
|---------|-----|
| Frontend | http://localhost:3000 |
| Backend API | http://localhost:5027/api |
| Swagger (dev only) | http://localhost:5027/swagger |

## Common Commands

```bash
# Check status of all services
docker compose ps

# View logs
docker compose logs -f              # all services
docker compose logs -f backend      # backend only
docker compose logs -f frontend     # frontend only
docker compose logs -f sqlserver    # database only

# Restart all services
docker compose restart

# Rebuild and restart after code changes
docker compose up -d --build backend   # rebuild backend
docker compose up -d --build frontend  # rebuild frontend

# Stop all services
docker compose down

# Stop all services and delete database data
docker compose down -v
```

## Local Development (without Docker)

If you prefer running services directly on your machine:

### Backend

Requirements: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
cd Backend
dotnet restore
dotnet run
```

The backend reads configuration from `appsettings.json` and environment variables. You can create an `appsettings.Development.json` (git-ignored) to override settings locally.

### Frontend

Requirements: [Node.js 22+](https://nodejs.org/)

```bash
cd frontend
npm install
npm run dev
```

The frontend uses `VITE_API_BASE_URL` to locate the backend. You can create a `.env.local` file in the `frontend/` directory to override it:

```
VITE_API_BASE_URL=http://localhost:5027/api
```

### Both at once

From the project root:

```bash
npm install
npm run dev
```

This uses `concurrently` to start both the frontend and backend in development mode.

## Production Deployment

For production deployments, make sure to:

1. Set `ASPNETCORE_ENVIRONMENT=Production`
2. Use a strong, unique `SA_PASSWORD` and `JWT_KEY`
3. Set `ALLOWED_ORIGINS` to your actual deployed frontend URL
4. Set `VITE_API_BASE_URL` to your actual deployed backend URL
5. Disable Swagger (automatically disabled in Production)

Refer to the [README](README.md) for more information about the project.