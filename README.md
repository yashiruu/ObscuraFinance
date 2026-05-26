# Obscura.FinanceTracker

Personal finance tracker application built with .NET ecosystem for learning enterprise software engineering concepts and clean architecture.

---

# Goals

* Learn enterprise .NET architecture
* Improve understanding of backend/frontend separation
* Practice clean architecture principles
* Simulate real-world software engineering workflow
* Build long-term portfolio project
* Reduce contribution friction in professional environment
* Understand real enterprise request flow end-to-end

---

# Tech Stack

| Area           | Technology                    |
| -------------- | ----------------------------- |
| Backend        | ASP.NET Core Web API          |
| Frontend       | Blazor Server                 |
| ORM            | Entity Framework Core 8       |
| Database       | SQL Server                    |
| Architecture   | Simplified Clean Architecture |
| SDK            | .NET 8 LTS                    |
| IDE            | Visual Studio 2022            |
| Source Control | Git                           |

---

# Architecture

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

# Development Workflow

## Branch Strategy

```text
main
develop
feature/*
```

## Workflow

### Main Branch

Used for:

* stable project foundation
* architecture baseline
* initial setup
* stable milestone

### Develop Branch

Used for:

* integration branch
* active development
* feature merge target

### Feature Branch

Used for isolated feature development.

Example:

```text
feature/create-transaction
feature/category-crud
feature/dashboard-summary
```

---

# Commit Convention

This project uses simplified Conventional Commits.

Format:

```text
<type>: <description>
```

## Commit Types

### feat

Used for new features.

Example:

```text
feat: add transaction crud endpoint
```

---

### fix

Used for bug fixes.

Example:

```text
fix: resolve transaction date parsing issue
```

---

### refactor

Used for internal code improvements without changing behavior.

Example:

```text
refactor: simplify transaction validation flow
```

---

### docs

Used for documentation updates.

Example:

```text
docs: update architecture explanation
```

---

### chore

Used for setup, tooling, configuration, dependency, and repository maintenance.

Example:

```text
chore: initialize solution structure and repository setup
```

---

# Development Principles

* Focus on understanding software engineering flow deeply
* Prioritize stability over complexity
* Learn architecture incrementally
* Avoid premature abstraction and overengineering
* Build features gradually and refactor later
* Mimic enterprise workflow without unnecessary complexity

---

# Current Scope (V1)

## Entities

* Transaction
* Category
* Account

## Features

* Transaction CRUD
* Category CRUD
* Account CRUD
* Monthly Summary Dashboard

---

# Current Status

Project initialization phase.
