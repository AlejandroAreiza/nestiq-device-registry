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

FieldValueServer name / 
Host localhost 
Port 5432 
Database nestiq_device_registry
Username output of whoami
Password leave empty
Connection NestIQ Local