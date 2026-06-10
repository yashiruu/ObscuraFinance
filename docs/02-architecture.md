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

Contains business flow.

Current structure:

```text
Services
DTOs
```

Future structure:

```text
Features/
 ├── Commands
 ├── Queries
 ├── DTOs
 ├── Validators
 └── Mappings
```

Responsibilities:

* Business rules
* Use cases
* Application workflows

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

# Request Flow Evolution

## Current State

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

## Service Layer Stage

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
Service
    ↓
DbContext
    ↓
SQL Server
```

---

## Repository Stage

```text
08.Bsui
    ↓
07.Client
    ↓ HTTP/API
06.WebApi
    ↓
Service
    ↓
Repository
    ↓
DbContext
    ↓
SQL Server
```

---

## CQRS Stage

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
DbContext
    ↓
SQL Server
```

---

# Architecture Principles

1. Principle 1 - Mirror enterprise structure.
2. Principle 2 - Avoid premature complexity.
3. Principle 3 - Learn architecture incrementally.
4. Principle 4 - Refactor when understanding improves.
5. Principle 5 :
   - Prioritize understanding over patterns.
   - A pattern should be introduced because it solves a problem or teaches an important concept.
   - Never introduce a pattern solely because it is popular.

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
