# ADR-004

## Title

Build Dashboard Before Enterprise Refactoring

---

## Status

Accepted

---

## Context

After completing transaction management, two possible paths existed.

Option A:

```text id="c65v0g"
Transaction Workflow
        ↓
Large Refactor
        ↓
Dashboard
```

Option B:

```text id="ejd0g5"
Transaction Workflow
        ↓
Dashboard
        ↓
Enterprise Refactor
```

---

## Decision

Choose Option B.

Build Dashboard V1 before introducing enterprise patterns.

---

## Rationale

Reasons:

* Faster visible progress
* Higher motivation
* Immediate value from existing data
* Better understanding of reporting requirements

---

## Expected Benefits

The dashboard provides:

* Aggregate Queries
* Summary DTOs
* Reporting Experience
* Visual Feedback

before architectural complexity increases.

---

## Consequences

Positive:

* Faster product validation
* Earlier usable application
* Better user feedback

Negative:

* Some dashboard code may later be refactored

---

## Principle

Deliver visible value before introducing architectural complexity.
