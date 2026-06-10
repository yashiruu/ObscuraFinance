# ADR-003

## Title

Use Clean Architecture From The Beginning

---

## Status

Accepted

---

## Context

The project could be built using a simple monolithic structure.

However the primary learning objective includes understanding enterprise architecture.

---

## Decision

Adopt a Clean Architecture inspired solution structure from the beginning.

---

## Rationale

Reasons:

* Mirrors workplace architecture
* Provides long-term scalability
* Encourages separation of concerns
* Builds familiarity with enterprise codebases

---

## Architecture

```text id="5zhl0o"
01.Base
02.Domain
03.Shared
04.Application
05.Infrastructure
06.WebApi
07.Client
08.Bsui
```

---

## Consequences

Positive:

* Better architectural understanding
* Easier future evolution
* Consistent project organization

Negative:

* Additional complexity compared to a simple CRUD project
* More files and projects to maintain

---

## Principle

Mirror enterprise structure.

Avoid enterprise complexity whenever possible.
