# nestiq-device-registry

| Project | Responsibility |
|---|---|
| `Domain` | Entities, enums, domain rules — no dependencies |
| `Application` | Use cases, interfaces, DTOs — depends on Domain only |
| `Infrastructure` | EF Core, repositories, DB — implements Application interfaces |
| `Api` | Controllers, middleware, DI wiring — entry point |
| `UnitTests` | Tests Domain + Application logic in isolation |
| `ComponentTests` | Tests full HTTP pipeline with faked infrastructure |
| `IntegrationTests` | Tests Infrastructure layer with real PostgreSQL |

---

Feature Branch
      ↓
git push
      ↓
Open Pull Request to main
      ↓ GitHub Actions triggers (quality gate):
      ✅ Build
      ✅ Unit Tests
      ✅ Component Tests
      ✅ Integration Tests
      ↓
      ❌ Any test fails → PR blocked, cannot merge
      ✅ All pass → PR can be merged
      ↓
Code Review + Merge to main
      ↓
Deploy to Staging
      ✅ API Smoke Tests (quality gate)
      ✅ E2E Tests (UI and subcutaneous)
      ↓
Deploy to Production

echo "# NestIQ Device Registry"