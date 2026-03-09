# 🏠 NestIQ — Device Registry Service

NestIQ is a smart home IoT platform. **Device Registry** is the foundational service responsible for managing smart devices within homes.

---

## 📐 Architecture

```
┌─────────────────────────────────────────┐
│              API Layer                  │
├─────────────────────────────────────────┤
│           Application Layer             │
├─────────────────────────────────────────┤
│             Domain Layer                │
├─────────────────────────────────────────┤
│          Infrastructure Layer           │
└─────────────────────────────────────────┘
```

| Layer | Purpose |
|---|---|
| **API** | HTTP entry point. Wires everything together |
| **Application** | Orchestrates use cases. Depends only on Domain |
| **Domain** | Business rules and entities. No external dependencies |
| **Infrastructure** | Data persistence. Implements Application interfaces |

---

## 🧪 Testing Strategy

### Test Pyramid

```
              [E2E Tests]
           Cross-service flows
         ─────────────────────
           [API Smoke Tests]
         Deployed service health
       ───────────────────────────
          [Integration Tests]
         Data layer + real DB
     ───────────────────────────────
          [Component Tests]
       Full HTTP pipeline, fake DB
   ───────────────────────────────────
              [Unit Tests]
         Business logic in isolation
```

### Test Types

| Test Type | Purpose | Mocking | Speed |
|---|---|---|---|
| **Unit** | Business logic and domain rules | Everything external | Very fast |
| **Component** | Full HTTP pipeline with faked infrastructure | Repository | Fast |
| **Integration** | Data layer communicates correctly with real DB | None | Medium |
| **API Smoke** | Deployed service is alive and full stack is wired | None | Medium |

---

## 🚀 Quality Gates & CI/CD Flow

```
  Write code on feature branch
         │
         ▼
  Open Pull Request → main
         │
         ▼
  ┌──────────────────────────────┐
  │       CI QUALITY GATE        │
  │                              │
  │  ✅ Build                    │
  │  ✅ Unit Tests               │
  │  ✅ Component Tests          │
  │  ✅ Integration Tests        │
  │                              │
  │  ❌ Any fail → PR BLOCKED    │
  │  ✅ All pass → PR can merge  │
  └──────────────────────────────┘
         │
         ▼
  Code Review + Merge to main
         │
         ▼
  Deploy to Staging
         │
         ▼
  ┌──────────────────────────────┐
  │       API SMOKE TESTS        │
  │                              │
  │  ❌ Fail → deployment alert  │
  │  ✅ Pass → notify QA         │
  └──────────────────────────────┘
         │
         ▼
  QA Manual Testing on Staging
         │
         ▼
  Deploy to Production
         │
         ▼
  ┌──────────────────────────────┐
  │          E2E TESTS           │
  │  Cross-service full flows    │
  └──────────────────────────────┘
```

### When Each Test Runs

| Test Type | Pull Request | Staging | Production |
|---|---|---|---|
| Unit | ✅ | | |
| Component | ✅ | | |
| Integration | ✅ | | |
| API Smoke | | ✅ | |
| E2E | | | ✅ |
| Performance (k6) | | ✅ Scheduled | |

---

## 📋 Branch & Commit Conventions

**Branch naming:**
```
feature/   fix/   chore/   refactor/   test/   docs/
```

**Commit format (Conventional Commits):**
```
type(scope): description

feat(device): add register device endpoint
fix(device): trim device name on creation
ci: add quality gate pipeline
```

| Type | When |
|---|---|
| `feat` | New feature |
| `fix` | Bug fix |
| `chore` | Maintenance |
| `refactor` | Code restructuring |
| `test` | Adding tests |
| `docs` | Documentation |
| `ci` | CI/CD changes |
| `perf` | Performance improvement |

---

## 🗄️ Database

**Engine:** PostgreSQL 16 — each service owns its own database.

```bash
# Create migration
dotnet ef migrations add MigrationName \
  --project src/NestIQ.DeviceRegistry.Infrastructure \
  --startup-project src/NestIQ.DeviceRegistry.Api

# Apply migration
dotnet ef database update \
  --project src/NestIQ.DeviceRegistry.Infrastructure \
  --startup-project src/NestIQ.DeviceRegistry.Api
```

---

## 🛠️ Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL 16
- Docker (for integration tests)

### Setup

```bash
# Clone
git clone https://github.com/YOUR_USERNAME/nestiq-device-registry.git
cd nestiq-device-registry

# Restore tools
dotnet tool restore

# Restore dependencies
dotnet restore

# Apply migrations
dotnet ef database update \
  --project src/NestIQ.DeviceRegistry.Infrastructure \
  --startup-project src/NestIQ.DeviceRegistry.Api

# Run the API
dotnet run --project src/NestIQ.DeviceRegistry.Api

# Swagger
http://localhost:5283/swagger
```