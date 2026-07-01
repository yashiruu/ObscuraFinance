# Architecture Guide

## Architecture Goal

The project adopts enterprise architecture principles without adopting unnecessary enterprise complexity.

The objective is:

```text
Learn Enterprise Thinking
```

Not:

```text
Create Enterprise Complexity
```

---

# Solution Structure

```text
src/
 ├── 01.Base
 ├── 02.Domain
 ├── 03.Shared
 ├── 04.Application
 ├── 05.Infrastructure
 ├── 06.WebApi
 ├── 07.Client
 └── 08.Bsui
```

---

# Layer Responsibilities

## 01.Base

Contains foundational abstractions.

Examples:

```text
BaseEntity
BaseRequest
BaseResponse
BaseAuditableEntity
```

Responsibilities:

* Common abstractions
* Shared base models
* Reduce duplication

---

## 02.Domain

Contains core business concepts.

Examples:

```text
Account
Category
Transaction
```

Characteristics:

* No database knowledge
* No API knowledge
* No UI knowledge

Domain is the center of the system.

---

## 03.Shared

Contains reusable shared components.

Examples:

```text
Constants
Extensions
Utilities
Exceptions
Pagination Models
```

Responsibilities:

* Cross-project reuse
* Common helpers
* Shared models

---

## 04.Application

Contains application business logic and use cases.

Current structure:

```text
Services
DTOs
Validators
```

Future structure (CQRS):

```text
Features/
 ├── Commands
 ├── Queries
 ├── DTOs
 ├── Validators
 ├── Mappings
 └── Handlers
```

Responsibilities:

* Business rules
* Application workflows
* Request validation
* DTO mapping
* Use case orchestration

---

## 05.Infrastructure

Contains technical implementations.

Examples:

```text
EF Core
Repository
Authentication
External Services
File Storage
```

Responsibilities:

* Database access
* External integrations
* Technical concerns

---

## 06.WebApi

Backend entry point.

Examples:

```text
Controllers
Middleware
Swagger
Dependency Injection
```

Responsibilities:

* HTTP endpoints
* Request handling
* Response generation

---

## 07.Client

Frontend communication layer.

Examples:

```text
AccountClient
CategoryClient
TransactionClient
```

Responsibilities:

* API communication
* Serialization
* Deserialization
* Endpoint abstraction

---

## 08.Bsui

User interface layer.

Examples:

```text
Pages
Components
Layouts
Dashboard
Forms
```

Responsibilities:

* User interaction
* Data presentation
* UI state

---

# Dependency Direction

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
05.Infrastructure
    ↓
04.Application
    ↓
02.Domain
    ↓
01.Base
```

Dependencies should always point inward.

---

# Dependency Rules

Dependencies must always point inward.

Allowed:

```text
Application
    ↓
Domain
```

```text
Infrastructure
    ↓
Application
```

```text
WebApi
    ↓
Application
```

Avoid:

```text
Domain
    ↓
Infrastructure
```

```text
Domain
    ↓
WebApi
```

```text
Application
    ↓
WebApi
```

The Domain layer must remain independent of infrastructure, presentation, and delivery concerns.

---

# Request Flow Evolution

The request flow evolves as the project adopts additional architectural patterns.

## Phase 1 — Direct Data Access

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
DbContext
    ↓
SQL Server
```

---

## Phase 2 — Service Layer

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
Application Service
    ↓
DbContext
    ↓
SQL Server
```

---

## Phase 3 — Repository Pattern (Current)

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
Application Service
    ↓
Repository
    ↓
Unit Of Work
    ↓
DbContext
    ↓
SQL Server
```

This architecture separates business logic from data access while centralizing transaction management through the Unit Of Work pattern.

---

## Phase 4 — CQRS (Planned)

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
Command / Query
    ↓
Handler
    ↓
Repository
    ↓
Unit Of Work
    ↓
DbContext
    ↓
SQL Server
```

CQRS separates read and write operations while preserving the existing data access layer.

---

# Architecture Principles

1. Mirror enterprise structure.
2. Avoid premature complexity.
3. Learn architecture incrementally.
4. Refactor when understanding improves.
5. Introduce patterns only when they solve a real problem or teach an important concept.
6. Keep documentation aligned with the implemented architecture.

---

# Final Architecture Objective

```text
Clean Architecture
    ↓
Service Layer
    ↓
Repository Pattern
    ↓
Unit Of Work
    ↓
CQRS
    ↓
MediatR
    ↓
AI Integration
    ↓
Agentic Architecture
```

Every stage exists to deepen understanding of enterprise software development.
