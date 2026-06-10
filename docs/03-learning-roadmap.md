# Learning Roadmap

## Overview

This roadmap represents the learning journey of the Obscura Finance Tracker project.

The goal is not only to build a finance application but also to progressively learn enterprise software development concepts.

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

🎯 Current Focus

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

---

## Module 5 — Interface

Learning Objectives:

* Abstraction
* Dependency Inversion
* Loose Coupling

Topics:

```text id="kk3sdc"
IAccountService
ITransactionService
ICategoryService
ISummaryService
```

---

## Module 6 — Service Layer

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

---

## Module 11 — Repository Pattern

Learning Objectives:

* Data Access Abstraction

---

## Module 12 — Unit Of Work

Learning Objectives:

* Transaction Management
* Consistency

---

## Module 13 — Validation

Learning Objectives:

* FluentValidation
* Fail Fast Principle

---

## Module 14 — AutoMapper

Learning Objectives:

* DTO Mapping
* Entity Mapping

---

## Module 15 — Testing

Learning Objectives:

* Unit Testing
* Integration Testing
* Validation Testing

---

## Milestone

```text id="gq6b4i"
v1.5.0
Enterprise Finance Tracker
```

---

# Phase 4 — CQRS Architecture

## Goal

Learn modern enterprise application architecture.

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
