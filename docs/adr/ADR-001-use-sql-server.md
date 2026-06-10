# ADR-001

## Title

Use SQL Server Instead Of PostgreSQL

---

## Status

Accepted

---

## Context

The project required a relational database.

The main options considered:

* SQL Server
* PostgreSQL

PostgreSQL offers strong AI ecosystem support through:

* pgvector
* semantic search
* vector similarity

However the primary goal of the project is learning enterprise .NET development.

---

## Decision

Use SQL Server.

---

## Rationale

Reasons:

* Used in the workplace
* Strong EF Core integration
* Mature tooling
* Reduced cognitive load
* Faster practical learning

---

## Consequences

Positive:

* Better alignment with workplace experience
* Easier onboarding
* Better tooling support

Negative:

* Less exposure to vector database workflows
* Some AI scenarios require additional infrastructure

---

## Future Revisit

This decision may be revisited during advanced AI phases if vector search becomes necessary.
