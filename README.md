# Obscura Finance Tracker

Personal Finance Tracker built with ASP.NET Core, Blazor, EF Core, and SQL Server as a long-term learning project focused on enterprise software engineering.

![Version](https://img.shields.io/badge/Version-v1.3.0-blue)
![Release](https://img.shields.io/badge/Release-v1.3.0-success)

![Architecture](https://img.shields.io/badge/Architecture-Clean_Architecture-success)
![Learning](https://img.shields.io/badge/Learning-Enterprise_.NET-blue)
![Roadmap](https://img.shields.io/badge/Roadmap-CQRS_→_AI_→_Agentic-purple)

![.NET](https://img.shields.io/badge/.NET-8_LTS-512BD4?logo=dotnet&logoColor=white)
![Blazor Server](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor&logoColor=white)
![MudBlazor](https://img.shields.io/badge/MudBlazor-9.5-594AE2)
![EF Core](https://img.shields.io/badge/EF_Core-8-512BD4)
![SQL Server](https://img.shields.io/badge/SQL_Server-2025-CC2927?logo=microsoftsqlserver&logoColor=white)
![License](https://img.shields.io/badge/License-MIT-2EA043)

---

## Latest Release

**v1.3.0 — Data Access & Application Patterns**

Completed:

* ✅ Repository Pattern
* ✅ Unit Of Work
* ✅ FluentValidation
* ✅ AutoMapper
* ✅ Testing Infrastructure (xUnit, FluentAssertions, Moq)
* ✅ AccountService, CategoryService, TransactionService, DashboardService Unit Tests
* ✅ Request Validator Unit Tests

This release establishes a stable data access and application layer, backed by a comprehensive automated unit test suite, and prepares the codebase for CQRS.

Current Development

* 🚧 Phase 4 — CQRS Architecture (extended scope)

Just Completed:
* ✅ Module 16 — Pagination

Next Modules:
* ⏳ Module 17 — CQRS
* ⏳ Module 18 — MediatR
* ⏳ Module 19 — Authentication + Authorization
* ⏳ Module 20 — Caching

---

## Release History

| Version | Status   | Description                        |
| ------- | -------- | ----------------------------------- |
| v1.3.0  | Latest   | Data Access & Application Patterns |
| v1.2.0  | Previous | Enterprise Foundation               |
| v1.1.0  | Previous | Release Stabilization               |
| v1.0.0  | Previous | Usable Finance Tracker              |

---

## 📖 Overview

Obscura Finance Tracker exists for two reasons:

1. Build a usable personal finance application.
2. Learn enterprise software engineering through a real project.

The application is the vehicle.

The learning journey is the destination.

---

## 🎯 Learning Objectives

This project is used to learn and practice:

* Clean Architecture
* Dependency Injection
* EF Core
* REST API Design
* Service Layer
* Repository Pattern
* Unit Of Work
* Validation
* AutoMapper
* Testing
* CQRS
* MediatR
* AI Integration
* Agentic Systems

---

## 🚀 Current Status

### 🏁 Completed

* Foundation
* Category Management
* Account Management
* Transaction Management
* Dashboard V1
* Enterprise Foundation
* Data Access & Application Patterns (Repository Pattern, Unit Of Work, Validation, AutoMapper, Testing)
* Module 16 — Pagination

### 🔄️ In Progress / Current Focus

* Preparing for Module 17 — CQRS

### 💡 Planned

* Module 17 — CQRS
* Module 18 — MediatR
* Module 19 — Authentication + Authorization
* Module 20 — Caching
* AI Integration
* Agentic AI
* DevOps & Deployment

---

## ✨ Features

| Feature                 | Status |
| ------------------------ | ------ |
| Category Management      | ✅    |
| Account Management       | ✅    |
| Transaction Management   | ✅    |
| Dashboard V1              | ✅    |
| Enterprise Foundation     | ✅    |
| Data Access Patterns      | ✅    |
| Automated Unit Testing    | ✅    |
| Pagination                | ✅    |
| CQRS                      | ⏳    |
| Authentication & Authorization | ⏳    |
| AI Integration            | ⏳    |

---

## 🛠 Tech Stack

| Area           | Technology                    |
| -------------- | ------------------------------ |
| Backend        | ASP.NET Core Web API          |
| Frontend       | Blazor Server                 |
| ORM            | Entity Framework Core 8       |
| Database       | SQL Server                    |
| Architecture   | Simplified Clean Architecture |
| Testing        | xUnit, Moq, FluentAssertions  |
| SDK            | .NET 8 LTS                    |
| IDE            | Visual Studio 2022            |
| Source Control | Git                            |

---

## 🏗 Architecture

```text
08.Bsui
    ↓
07.Client
    ↓ ← HTTP/API
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

### 🏗️ Solution Structure

```text
src/
 ├── 01.Base            → Shared abstractions
 ├── 02.Domain          → Business entities
 ├── 03.Shared          → Common utilities
 ├── 04.Application     → Use cases and DTOs
 ├── 05.Infrastructure  → EF Core and persistence
 ├── 06.WebApi          → API endpoints
 ├── 07.Client          → API communication
 └── 08.Bsui            → User interface

tests/
 └── ObscuraFinance.Application.UnitTest
```

---

## 🗺 Roadmap

| Phase                                       | Status      |
| -------------------------------------------- | ----------- |
| Phase 1 — Core Finance Application           | ✅ Completed |
| Phase 2 — Enterprise Foundation              | ✅ Completed |
| Phase 3 — Data Access & Application Patterns | ✅ Completed |
| Phase 4 — CQRS Architecture                  | 🚧 Current   |
| Phase 5 — AI Integration                     | ⏳ Planned   |
| Phase 6 — Agentic AI                         | ⏳ Planned   |
| Phase 7 — DevOps & Deployment                | ⏳ Planned   |

---

## 📚 Documentation

```text
docs/
├── 01-project-vision.md
├── 02-architecture.md
├── 03-learning-roadmap.md
├── 04-project-status.md
├── 05-working-agreement.md
├── 06-ai-roadmap.md
├── 07-git-workflow.md
├── known-issues.md
├── RoadmapGraph.md
├── adr/
├── changelog/
└── dev-notes/
```

---

## 🌱 Development Principles

* Focus on understanding software engineering deeply
* Learn before abstracting.
* Learn architecture incrementally
* Avoid premature abstraction
* Prioritize maintainability over complexity
* Refactor when understanding improves
* Mimic enterprise practices without unnecessary overhead

---

## 🔄 Git Workflow

```text
main
 └── develop
      └── feature/*
```

This project follows a simplified feature-branch workflow with Conventional Commits.

---

## 📈 Long-Term Evolution

```text
Finance Tracker
    ↓
Enterprise Patterns
    ↓
Data Access Patterns
    ↓
CQRS
    ↓
AI Integration
    ↓
Agentic AI
    ↓
Production Platform
```

---

## 🔮 Future Direction

### v1.4.0 — CQRS Architecture

* CQRS (Commands / Queries)
* MediatR
* Request Handlers
* Pipeline Behaviors

### v1.5.0 — AI Integration

* AI Service Abstraction
* AI Playground
* Finance Assistant
* Context Injection
* Finance Insights Engine

### Recently Completed — v1.3.0 Data Access & Application Patterns

* Repository Pattern
* Unit Of Work
* FluentValidation
* AutoMapper
* Automated Unit Testing (Services + Validators)

---

## 🎯 Final Objective

The ultimate goal of this project is not to build the perfect finance tracker.

The ultimate goal is to become a better enterprise .NET developer while building a useful real-world application.

---

## Screenshots

### Dashboard

![Dashboard](docs/screenshots/v1.0.0/dashboard-home.png)

### Categories

![Categories](docs/screenshots/v1.0.0/category-list.png)

### Accounts

![Accounts](docs/screenshots/v1.0.0/account-list.png)

### Transactions

![Transactions](docs/screenshots/v1.0.0/transaction-list.png)