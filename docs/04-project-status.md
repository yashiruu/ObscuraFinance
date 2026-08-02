# Project Status

## Purpose

This document tracks the current implementation status of the project.

Unlike the roadmap, this file changes frequently as development progresses.

---

# Current Status

## Foundation

Status:

✅ Completed

Completed:

* Git Workflow
* Feature Branch Workflow
* Clean Architecture Setup
* SQL Server Integration
* EF Core Setup
* Migration Flow
* Base Entities
* Audit Fields

---

# Category Management

Status:

✅ Completed

Completed:

* Domain
* Persistence
* API
* Client
* UI
* Soft Delete

---

# Account Management

Status:

✅ Completed

Completed:

* Domain
* Persistence
* API
* Client
* UI
* Soft Delete

---

# Transaction Management

Status:

✅ Completed

Completed:

* Domain
* Account Relation
* Category Relation
* API
* Client
* UI
* Soft Delete

Notes:

* CRUD workflow completed
* Dropdown issues resolved
* Transaction flow stabilized

---

# Dashboard V1

Status:

✅ Completed

Completed Features:

* Summary Cards
* Recent Transactions
* Expense By Category
* Account Summary

Notes:

* Dashboard summary endpoints implemented
* Dashboard DTOs implemented
* Dashboard UI completed
* Dashboard V1 released as part of v1.0.0

---

# Enterprise Foundation

Status:

✅ Completed

Modules:

* Interface Layer
* Service Layer
* Structured Logging
* Global Exception Middleware
* Response Standardization
* Global Query Filter
* OpenAPI/Swagger Documentation Setup
* API Controller Metadata ([ProducesResponseType])

Goal:

* Introduce foundational enterprise patterns
* Improve separation of concerns
* Improve API consistency
* Centralize error handling
* Complete soft delete implementation
* Prepare for Repository Pattern and CQRS

Notes:

* Service layer introduced for business logic separation.
* API responses standardized using `ApiResponse<T>`.
* Global exception handling implemented through middleware.
* Soft delete completed using EF Core global query filters.
* Client applications updated to support standardized API responses.
* Request validation standardized using FluentValidation.
* Validation pipeline implemented across Category, Account, and Transaction.
* API controllers are fully documented with XML comments and enterprise-level OpenAPI specifications.
* CancellationToken propagation has been implemented across all endpoints.

Follow-up hardening (alongside Module 16 — Pagination):

* A cross-controller audit of `[ProducesResponseType]` attributes against what `ExceptionMiddleware` actually returns found and fixed real gaps: a `401` on `DashboardController` that could never occur (no auth middleware exists yet), and missing `400`/`404` declarations on a few endpoints.
* `ExceptionMiddleware` extended with a `TraceId` correlation id on every error response, explicit `DbUpdateConcurrencyException`/`DbUpdateException` → 409 handling, safe handling of client-cancelled requests, and environment-aware exception detail.
* `InvalidModelStateResponseFactory` wired up so automatic model-binding failures (e.g. a malformed `PageSize` query value) return the same `ApiResponse<T>` envelope as every other error path, instead of the framework's default shape.
* XML documentation completed across the domain layer, all DTOs, and the remaining service/client files that were still undocumented.
* Full reasoning and before/after code for all of the above: `docs/dev-notes/pagination-and-enterprise-hardening-ENG.md` (also available in Indonesian).

---

# Data Access & Application Patterns

Status:

✅ Completed

Modules:

* Repository Pattern
* Unit Of Work
* FluentValidation
* AutoMapper
* Testing

Goal:

* Abstract data access behind repositories.
* Coordinate persistence through Unit Of Work.
* Centralize request validation.
* Simplify object mapping.
* Establish automated testing before CQRS.

Notes:

* Generic Repository implemented across the application.
* Unit Of Work coordinates repository access and SaveChanges.
* FluentValidation integrated for all request DTOs.
* AutoMapper replaces repetitive manual mapping.
* Comprehensive unit test suite implemented for core services.
* Request validators covered by automated tests.
* Testing infrastructure established using xUnit, FluentAssertions, and Moq.

---

# Long-Term Roadmap Status

```text
Phase 1 ██████████ 100%

Phase 2 ██████████ 100%

Phase 3 ██████████ 100%

Phase 4 ██░░░░░░░░ 20%

Phase 5 ░░░░░░░░░░ 0%

Phase 6 ░░░░░░░░░░ 0%

Phase 7 ░░░░░░░░░░ 0%
```

---

# Current Priority

```text
Module 16 — Pagination (✅ Completed)
↓
Module 17 — CQRS
↓
Module 18 — MediatR
↓
Module 19 — Authentication + Authorization
↓
Module 20 — Caching
```

---

# Latest Release

```text
v1.3.0
Data Access & Application Patterns
```

Delivered:

* Repository Pattern
* Unit Of Work
* FluentValidation
* AutoMapper
* Testing Infrastructure
* AccountService Unit Tests
* CategoryService Unit Tests
* TransactionService Unit Tests
* DashboardService Unit Tests
* Request Validator Unit Tests

---

# Next Major Milestone

```text
v1.4.0
CQRS Architecture
```

Objectives:

* Pagination
* Authentication (Identity/JWT)
* CQRS
* MediatR
* Authorization Policy Enforcement
* Caching
* Command / Query Separation
* Request Handlers
* Pipeline Behaviors

Current Progress:

🚧 Not Started

---

# ✅ Completed Modules

## Module 11 — Repository Pattern

Status:

✅ COMPLETED

Objectives:

* Introduce data access abstraction
* Reduce direct dependency on DbContext
* Prepare foundation for Unit Of Work
* Learn generic repositories and constraints

Completed:

* Generic Repository
* Entity-specific repositories
* Repository abstraction
* Service migration from DbContext to repositories

---

## Module 12 — Unit Of Work

Status:

✅ COMPLETED

Objectives:

* Coordinate repositories
* Manage transactions consistently
* Centralize SaveChanges operations

Completed:

* IUnitOfWork abstraction
* UnitOfWork implementation
* Repository coordination
* Centralized persistence

---

## Module 13 — Validation

Status:

✅ COMPLETED

Objectives:

* Introduce request validation
* Improve API error handling
* Standardize validation responses

Completed:

* FluentValidation
* Category Validators
* Account Validators
* Transaction Validators
* Validation Constraints
* Validation Pipeline
* Centralized Validation Responses

Notes:

Three-layer validation adopted:

* Request Validation
* Business Validation
* Database Constraints

---

## Module 14 — AutoMapper

Status:

✅ COMPLETED

Objectives:

* Reduce manual mapping
* Improve DTO maintainability
* Prepare for CQRS architecture

Completed:

* Mapping Profiles
* Entity-to-DTO mapping
* DTO-to-Entity mapping
* Service refactoring to AutoMapper

---

## Module 15 — Testing

Status:

✅ COMPLETED

Objectives:

* Unit Testing
* Testing Infrastructure
* Validator Testing
* Improve confidence during refactoring

Completed:

* xUnit test project
* Shared testing infrastructure
* Test builders
* Mock factories
* AccountService unit tests
* CategoryService unit tests
* TransactionService unit tests
* DashboardService unit tests
* Request validator unit tests

Notes:

* Unit tests provide a regression safety net for future refactoring.
* Integration testing has been intentionally deferred to a later milestone after the architecture stabilizes.
* AI-assisted tests are documented for future self-review in `known-issues.md`.

---

## Module 16 — Pagination

Status:

✅ COMPLETED

Objectives:

* Introduce reusable PagedRequest/PagedResult models.
* Apply pagination to GetAllAsync across Account, Category, Transaction.
* Prepare data access layer for query-heavy CQRS Query handlers.

Completed:

* `PagedRequest` / `PagedResult<T>` in `03.Shared`, with page number/size validation.
* Deterministic ordering added to the generic repository (previously missing — pages could return duplicate or skipped rows).
* Pagination applied to `GetAllAsync` on Account, Category, and Transaction, plus `CategoryController.GetDeleted`.
* Client project and Blazor UI updated to consume the new `PagedResult<T>` response shape.

Notes:

* This module also produced unplanned but valuable follow-up work: global exception middleware hardening and a `ProducesResponseType` accuracy audit (see the Enterprise Foundation notes above), and completion of XML documentation across layers that had been left undocumented.
* Full write-up: `docs/dev-notes/pagination-and-enterprise-hardening-ENG.md` / `-IND.md`.

---

# Next Module

## Module 17 — CQRS

Status:

⏳ Not Started

Objectives:

* Express Account/Category/Transaction business logic as Command/Query + Handler pairs.
* Controllers invoke Handlers directly (no mediator yet — that's Module 18).
* Prepare the application for MediatR without introducing it prematurely.

---