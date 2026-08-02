# Learning Roadmap

## Overview

This roadmap represents the learning journey of the Obscura Finance Tracker project.

The goal is not only to build a finance application but also to progressively learn enterprise software development concepts.

---

## Current Position

Current Position:

Phase 4 — CQRS Architecture

Completed

✔ Module 11 — Repository Pattern

✔ Module 12 — Unit Of Work

✔ Module 13 — Validation

✔ Module 14 — AutoMapper

✔ Module 15 — Testing

✔ Module 16 — Pagination

Next

⏳ Module 17 — CQRS

⏳ Module 18 — MediatR

⏳ Module 19 — Authentication + Authorization

⏳ Module 20 — Caching

---

# Release Roadmap

The project evolves through a series of architectural milestones.

Each release represents a significant step in the application's evolution rather than simply a collection of completed features.

| Version | Milestone | Phase |
|----------|-----------|-------|
| v1.0.0 | Core Finance Application | Phase 1 |
| v1.1.0 | Release Stabilization | Post Phase 1 |
| v1.2.0 | Enterprise Foundation | Phase 2 |
| v1.3.0 | Data Access Patterns | Phase 3 |
| v1.4.0 | CQRS Architecture | Phase 4 |
| v1.5.0 | AI Integration | Phase 5 |
| v2.0.0 | Agentic Finance Platform | Phase 6 |
| v3.0.0 | Production Platform | Phase 7 |

---

# Architecture Evolution

```text
Foundation
    ↓
Category Management
    ↓
Account Management
    ↓
Transaction Management
    ↓
Dashboard V1
    ↓
Enterprise Foundation
    ↓
Data Access Patterns
    ↓
CQRS Architecture
    ↓
AI Integration
    ↓
Agentic AI
    ↓
DevOps & Deployment
```

---

# Phase 1 — Core Finance Application

## Goal

Build a usable finance application.

## Why This Phase

Before learning enterprise architecture, the project first establishes a complete, working application.

Building real features provides practical experience with application flow, data modeling, API design, and user interaction.

Without a functional application, introducing enterprise patterns would add complexity without meaningful context.

This phase focuses on understanding how the system works before learning how to improve its architecture.

---

## Module 1 — Category Management

Status:

✅ Completed

Learning Objectives:

* CRUD Operations
* Entity Design
* API Design
* Blazor Forms

---

## Module 2 — Account Management

Status:

✅ Completed

Learning Objectives:

* Account Modeling
* CRUD Workflow
* UI Integration

---

## Module 3 — Transaction Management

Status:

✅ Completed

Learning Objectives:

* Entity Relationships
* DTO Design
* Form Handling
* Data Validation

---

## Module 4 — Dashboard V1

Status:

✅ Completed

Learning Objectives:

* Aggregate Queries
* Data Projection
* Summary DTOs
* Reporting Fundamentals

Features:

* Summary Cards
* Recent Transactions
* Expense By Category
* Account Summary

---

## Milestone

```text
v1.0.0
Usable Finance Tracker
```

---

# Phase 2 — Enterprise Foundation

## Goal

Learn foundational enterprise patterns.

## Status

✅ Completed

## Why This Phase

With the core application complete, the next step is improving the overall architecture rather than adding new business features.

This phase introduces foundational enterprise concepts that improve maintainability, consistency, and separation of concerns while preserving the existing functionality.

The application evolves from a working CRUD application into a structured enterprise application by introducing:

```text
Interface Abstraction
    ↓
Service Layer
    ↓
Structured Logging
    ↓
Global Exception Handling
    ↓
Response Standardization
    ↓
Global Query Filters
```

These patterns establish a stable architectural foundation before refactoring the data access layer.

---

## Module 5 — Interface

Status:

✅ Completed

Learning Objectives:

* Abstraction
* Dependency Inversion
* Loose Coupling

Topics:

```text
IAccountService
ITransactionService
ICategoryService
IDashboardService
```

---

## Module 6 — Service Layer

Status:

✅ Completed

Learning Objectives:

* Business Logic Separation
* Orchestration Layer

Architecture Evolution:

```text
Controller
    ↓
Service
    ↓
DbContext
```

---

## Module 7 — Logging

Status:

✅ Completed

Learning Objectives:

* Diagnostics
* Observability

Topics:

```text
ILogger
Structured Logging
Log Levels
```

---

## Module 8 — Middleware

Status:

✅ Completed

Learning Objectives:

* Request Pipeline
* Exception Handling

Topics:

```text
Global Exception Handling
Request Processing
Response Processing
```

---

## Module 9 — Response Standardization

Status:

✅ Completed

Learning Objectives:

* API Contracts
* Consistent Responses

Topics:

```text
ApiResponse<T>
Success Response
Error Response
```

---

## Module 10 — Global Query Filter

Status:

✅ Completed

Learning Objectives:

* Soft Delete Completion
* Automatic Filtering

Topics:

```text
HasQueryFilter()
IgnoreQueryFilters()
```

---

## Milestone

```text
v1.2.0
Enterprise Foundation Ready
```

---

# Phase 3 — Data Access & Application Patterns

## Goal

Understand enterprise data access patterns.

## Status

✅ Completed

## Why This Phase

Before introducing CQRS, the application first establishes a solid data access foundation.

This phase introduces enterprise patterns incrementally:

```text
Repository Pattern
    ↓
Unit Of Work
    ↓
Validation
    ↓
AutoMapper
    ↓
Testing
```

These patterns reduce coupling, improve maintainability, and prepare the codebase for CQRS without introducing unnecessary complexity too early.

---

## Module 11 — Repository Pattern

Status:

✅ Completed

Learning Objectives:

* Data Access Abstraction
* Generic Repository
* Generic Constraints
* Repository Design

Topics:

```text
IRepository<TEntity>
Generic Constraints
Repository Abstraction
Data Access Separation
```

### Exit Criteria

The module is considered complete when:

- Generic repository abstraction has been introduced.
- Services no longer access DbContext directly.
- Data access responsibilities are centralized.
- Existing features continue to function correctly.

---

## Module 12 — Unit Of Work

Status:

✅ Completed

Learning Objectives:

* Transaction Management
* Consistency

Topics:

```text
Unit Of Work
Transaction Coordination
Repository Aggregation
SaveChanges Management
```

### Exit Criteria

The module is considered complete when:

- SaveChanges() is coordinated through Unit Of Work.
- Repositories are managed consistently.
- Multiple repository operations can participate in a single transaction.

---

## Module 13 — Validation

Status:

✅ Completed

Learning Objectives:

* FluentValidation
* Fail Fast Principle

Topics:

```text
FluentValidation
Validation Pipeline
Business Validation
Error Handling
```

### Exit Criteria

The module is considered complete when:

- All requests use FluentValidation.
- Validation rules are centralized.
- Validation errors are returned consistently.
- Middleware handles validation exceptions.

---

## Module 14 — AutoMapper

Status:

✅ Completed

Learning Objectives:

* Reduce repetitive mapping code
* Centralize mapping configuration
* Improve DTO maintainability
* Prepare the application for CQRS

Topics:

```text
AutoMapper
Mapping Profiles
Reverse Mapping
Projection
DTO Mapping
Entity Mapping
```

### Exit Criteria

The module is considered complete when:

- Manual entity-to-DTO mapping has been removed where appropriate.
- Mapping profiles are centralized.
- DTO creation logic is simplified.
- Existing API behavior remains unchanged.

---

## Module 15 — Testing

Status:

✅ Completed

Learning Objectives:

* Unit Testing
* Testing Infrastructure
* Mocking
* Validation Testing

Topics:

```text
xUnit
FluentAssertions
Moq
Unit Testing
Testing Infrastructure
Service Testing
Validator Testing
Mocking
Test Isolation
```

Completed:

* Unit Testing Infrastructure
* AccountService Unit Tests
* CategoryService Unit Tests
* TransactionService Unit Tests
* DashboardService Unit Tests
* Request Validator Unit Tests

### Exit Criteria

The module is considered complete when:

- Core business services have unit tests.
- Request validators have automated tests.
- Testing infrastructure has been established.
- The application is ready for safe refactoring.

---

## Milestone

```text
v1.3.0
Enterprise Finance Tracker

✅ Released
```

## Architecture Evolution After Phase 3

```text
Controller
        ↓
Application Service
        ↓
Repository
        ↓
Unit Of Work
        ↓
DbContext
```

The application now has a stable enterprise data access layer and is ready to evolve toward CQRS.

---

# Phase 4 — CQRS Architecture

## Goal

Complete cross-cutting application concerns (pagination, authentication,
authorization, caching) and learn modern enterprise application architecture
through CQRS and MediatR.

## Status

🚧 Current Phase

## Why This Phase

Pagination is addressed first as an infrastructure-level concern independent
of CQRS. CQRS and MediatR then form the core architectural shift: Commands,
Queries, and Handlers are introduced first (Module 17), with MediatR added
afterward (Module 18) as the dispatch mechanism and the foundation for
Pipeline Behaviors. Authentication and Authorization are deliberately
sequenced after MediatR and combined into one module — Authorization
enforcement is implemented as a Pipeline Behavior, which only becomes
natural once that infrastructure exists, and Authorization has nothing to
enforce without Authentication already in place. Caching closes out the
phase for the same reason: it is naturally a Pipeline Behavior on the
Query side, and depends on Commands/Queries already being separated.

```text
Pagination
    ↓
CQRS
    ↓
MediatR
    ↓
Authentication + Authorization
    ↓
Caching
```

At this point, the project shifts from layered CRUD architecture toward
modern enterprise application architecture. The goal is to improve
scalability, security, performance, and separation of responsibilities
while preserving existing business behavior.

---

## Module 16 — Pagination

Status:

✅ Completed

Learning Objectives:

* Query Projection
* Skip/Take Patterns
* Total Count Strategies
* Paged Response Design

Topics:

```text
PagedRequest
PagedResult<T>
Skip() / Take()
Total Count Query
```

Completed:

* Reusable `PagedRequest` / `PagedResult<T>` models in `03.Shared`, with `PageNumber`/`PageSize` clamped to valid ranges.
* `GetAllAsync` paginated across Account, Category, and Transaction.
* Deterministic ordering (`CreatedAt`, then `Id`) added to the generic repository so pages stay stable across requests.
* `CategoryController.GetDeleted` paginated as well, for consistency across every list endpoint.
* `AccountClient`, `CategoryClient`, `TransactionClient`, and the Blazor list pages updated to consume `PagedResult<T>`.
* Global exception middleware hardened alongside this module (correlation id, DB conflict handling, consistent model-state error responses) and a `ProducesResponseType` accuracy audit performed across all controllers — see `docs/dev-notes/pagination-and-enterprise-hardening-ENG.md` for the full review and reasoning.

Known Gap:

* Blazor list pages request a fixed `pageSize: 50` as a stopgap; they do not yet have next/previous page navigation. Tracked as AO-008 in `known-issues.md`.

### Exit Criteria

The module is considered complete when:

- ✅ A reusable `PagedRequest` / `PagedResult<T>` model exists in `03.Shared`.
- ✅ `GetAllAsync` endpoints across Account, Category, and Transaction support pagination.
- ✅ Existing non-paged consumers (if any) are updated or intentionally deprecated.

---

## Module 17 — CQRS

Status:

⏳ Next

Learning Objectives:

* Command Responsibility
* Query Responsibility
* Read/Write Separation

Topics:

```text
Command Objects
Query Objects
Command Handlers
Query Handlers
```

MediatR is deliberately not introduced yet — Controllers invoke Handlers
directly (constructor-injected), the same way they currently call Services.
The mediator/dispatch layer is Module 18's concern.

### Exit Criteria

The module is considered complete when:

- Business logic for Account, Category, and Transaction is expressed as discrete Command/Query + Handler pairs.
- Controllers invoke Handlers directly, without a mediator.
- Existing API behavior remains functionally unchanged.

---

## Module 18 — MediatR

Status:

⏳ Planned

Learning Objectives:

* Mediator Pattern
* Decoupled Architecture
* Pipeline Behaviors

Topics:

```text
IRequest / IRequestHandler
Pipeline Behavior
```

### Exit Criteria

The module is considered complete when:

- Controllers dispatch Commands/Queries through MediatR instead of invoking Handlers directly.
- Pipeline Behavior infrastructure exists and is proven with at least one cross-cutting behavior (e.g. logging or validation).
- Existing API behavior remains functionally unchanged.

---

## Module 19 — Authentication + Authorization

Status:

⏳ Planned

Learning Objectives:

* Identity Fundamentals
* Token-Based Authentication
* Login / Registration Flow
* Securing API Endpoints
* Policy-Based Authorization

Topics:

```text
ASP.NET Core Identity
JWT Issuance & Validation
[Authorize] Attribute
Refresh Tokens (optional)
Authorization Pipeline Behavior
Policy-Based Authorization
```

Why Combined:

Authentication (who the user is) and Authorization (what the user can do)
are a natural pair — Authorization enforcement has nothing to enforce until
Authentication exists. Sequencing them together, on top of the Pipeline
Behavior infrastructure Module 18 already established, avoids building a
temporary authorization mechanism that would need to be rebuilt later.

### Exit Criteria

The module is considered complete when:

- Users can register and log in.
- API endpoints require a valid token by default.
- Client application handles token storage and attachment to requests.
- An Authorization Pipeline Behavior enforces policies before a handler executes.

---

## Module 20 — Caching

Status:

⏳ Planned

Learning Objectives:

* Cache-Aside Pattern
* Query-Side Caching
* Cache Invalidation Strategy

Topics:

```text
IMemoryCache / Distributed Cache
Caching Pipeline Behavior (Query-side)
Cache Invalidation on Command Execution
```

Why After CQRS/MediatR:

Caching is naturally a Query-side concern. Implementing it before Commands
and Queries are separated risks mixing cache logic into services that also
handle writes, leading to unclear invalidation triggers.

### Exit Criteria

The module is considered complete when:

- Read-heavy Queries (e.g. Dashboard, GetAll) are cached via a Pipeline Behavior.
- Cache entries are invalidated correctly on related Command execution.

---

## Milestone

```text
v1.4.0
Secure, Paginated, CQRS-Based Finance Tracker
```

---

# Phase 5 — AI Integration

## Goal

Learn practical LLM integration.

## Why This Phase

Artificial Intelligence is introduced only after the application has a stable architecture and reliable business logic.

Rather than treating AI as an isolated feature, the application uses existing domain knowledge and financial data to provide meaningful assistance.

```text
AI Service Abstraction
    ↓
AI Playground
    ↓
Finance Assistant
    ↓
Context Injection
    ↓
Finance Insights Engine
```

The objective is to learn how enterprise applications integrate Large Language Models while maintaining clean architecture and provider independence.

Modules:

* AI Service Abstraction
* AI Playground
* Finance Assistant
* Context Injection
* Finance Insights Engine

Milestone:

```text
v1.5.0
AI Powered Finance Tracker
```

---

# Phase 6 — Agentic AI

## Goal

Learn tool calling and agent workflows.

## Why This Phase

After learning basic AI integration, the next step is allowing the model to reason over application data instead of simply generating text.

The application evolves from responding to questions into performing multi-step financial analysis through controlled tool execution.

```text
Tool Calling
    ↓
Planning
    ↓
Reasoning
    ↓
Finance Agent
    ↓
Multi-Step Analysis
```

This phase introduces agentic system design while keeping business logic inside the application rather than inside the language model.

Modules:

* Tool Calling
* Agent Workflow
* Finance Agent
* Multi-Step Analysis

Milestone:

```text
v2.0.0
Agentic Finance Assistant
```

---

# Phase 7 — DevOps & Deployment

## Goal

Deploy and operate the application.

## Why This Phase

Enterprise software development extends beyond writing application code.

The final phase focuses on deployment, automation, monitoring, and operational readiness.

```text
Docker
    ↓
Container Orchestration
    ↓
CI/CD
    ↓
Monitoring
    ↓
Secrets Management
```

The objective is to understand how enterprise applications are built, deployed, monitored, and maintained throughout their entire lifecycle.

At the completion of this phase, the project evolves from a learning application into a production-oriented software platform.

Modules:

* Docker
* Docker Compose
* CI/CD
* Monitoring
* Secrets Management

Milestone:

```text
v3.0.0
Production Ready Finance Platform
```