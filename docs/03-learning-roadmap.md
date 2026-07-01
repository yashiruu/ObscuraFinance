# Learning Roadmap

## Overview

This roadmap represents the learning journey of the Obscura Finance Tracker project.

The goal is not only to build a finance application but also to progressively learn enterprise software development concepts.

---

## Current Position

Current Position:

Phase 3 — Data Access & Application Patterns

Completed

✔ Module 11 — Repository Pattern

✔ Module 12 — Unit Of Work

✔ Module 13 — Validation

Current

🚧 Module 14 — AutoMapper

Next

Module 15 — Testing

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

```text id="fx4kzi"
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

```text id="qlkg1l"
v1.0.0
Usable Finance Tracker
```

---

# Phase 2 — Enterprise Foundation

## Goal

Learn foundational enterprise patterns.

## Status:

✅ Completed

## Why This Phase

With the core application complete, the next step is improving the overall architecture rather than adding new business features.

This phase introduces foundational enterprise concepts that improve maintainability, consistency, and separation of concerns while preserving the existing functionality.

The application evolves from a working CRUD application into a structured enterprise application by introducing:

```
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

```text id="kk3sdc"
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

```text id="5kz31z"
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

```text id="qgsv1e"
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

```text id="p84k6z"
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

```text id="l4w7if"
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

```text id="i1q36u"
HasQueryFilter()
IgnoreQueryFilters()
```

---

## Milestone

```text id="xar5pd"
v1.2.0
Enterprise Foundation Ready
```

---

# Phase 3 — Data Access & Application Patterns

## Goal

Understand enterprise data access patterns.

Status:

🚧 Current Phase

## Why This Phase

Before introducing CQRS, the application first establishes a solid data access foundation.

This phase introduces enterprise patterns incrementally:

```
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

## Exit Criteria

The module is considered complete when:

- All requests use FluentValidation.
- Validation rules are centralized.
- Validation errors are returned consistently.
- Middleware handles validation exceptions.

---

## Module 14 — AutoMapper

Status:

🚧 Next

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

⏳ Planned

Learning Objectives:

* Unit Testing
* Integration Testing
* Validation Testing

Topics:

```text
Unit Testing
Integration Testing
Mocking
Test Isolation
```

### Exit Criteria

The module is considered complete when:

- Core business services have unit tests.
- Critical workflows have integration tests.
- Validation behavior is verified through automated tests.

---

## Milestone

```text
v1.3.0
Enterprise Finance Tracker

🎯 Current Target
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

Learn modern enterprise application architecture.

## Why This Phase

Once the application has a stable data access layer, responsibilities can be separated between commands and queries.

CQRS is intentionally postponed until this stage because introducing it earlier would increase complexity without sufficient architectural benefit.

```
Repository
    ↓
Unit Of Work
    ↓
Validation
    ↓
AutoMapper
    ↓
CQRS
    ↓
MediatR
```

At this point, the project shifts from layered CRUD architecture toward modern enterprise application architecture.

The goal is to improve scalability, maintainability, and separation of responsibilities while preserving existing business behavior.

---

## Module 16 — CQRS

Learning Objectives:

* Command Responsibility
* Query Responsibility

---

## Module 17 — MediatR

Learning Objectives:

* Mediator Pattern
* Decoupled Architecture

---

## Milestone

```text id="o0x2el"
v1.8.0
CQRS Based Finance Tracker
```

---

# Phase 5 — AI Integration

## Goal

Learn practical LLM integration.

## Why This Phase

Artificial Intelligence is introduced only after the application has a stable architecture and reliable business logic.

Rather than treating AI as an isolated feature, the application uses existing domain knowledge and financial data to provide meaningful assistance.

```
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

```text id="k2qj3j"
v2.0.0
AI Powered Finance Tracker
```

---

# Phase 6 — Agentic AI

## Goal

Learn tool calling and agent workflows.

## Why This Phase

After learning basic AI integration, the next step is allowing the model to reason over application data instead of simply generating text.

The application evolves from responding to questions into performing multi-step financial analysis through controlled tool execution.

```
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

```text id="2s9m7u"
v2.5.0
Agentic Finance Assistant
```

---

# Phase 7 — DevOps & Deployment

## Goal

Deploy and operate the application.

## Why This Phase

Enterprise software development extends beyond writing application code.

The final phase focuses on deployment, automation, monitoring, and operational readiness.

```
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

```text id="ks9u5o"
v3.0.0
Production Ready Finance Platform
```
