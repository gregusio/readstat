# Readstat - Setup Guide

## Quick Start

```bash
git clone https://github.com/gregusio/readstat.git
cd readstat
docker compose up -d
```

Done! Access the app at **http://localhost:3000**

## Services

| Service | URL | User | Password |
|---------|-----|------|----------|
| Frontend | http://localhost:3000 | - | - |
| Backend API | http://localhost:5027 | - | - |
| SQL Server | localhost:1433 | sa | 1qazXSW@ |

## Common Commands

```bash
# Check status
docker compose ps

# View logs
docker compose logs -f backend        # backend only
docker compose logs -f frontend       # frontend only
docker compose logs -f sqlserver      # database only

# Restart everything
docker compose restart

# Rebuild and restart (after code changes)
docker compose up -d --build backend   # rebuild backend
docker compose up -d --build frontend  # rebuild frontend

# Stop everything
docker compose down

# Stop and delete database
docker compose down -v
```
