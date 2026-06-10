# ADR Writing Guidelines

## What Is An ADR?

ADR stands for:

```text
Architecture Decision Record
```

An ADR is a lightweight document used to record important technical decisions made during a project.

The purpose of an ADR is not to document code.

The purpose is to document:

* why a decision was made
* what alternatives were considered
* why the chosen option was selected
* what consequences the decision creates

An ADR allows future developers (including your future self) to understand the reasoning behind architectural decisions.

Without ADRs:

```text
Decision
    ↓
Time Passes
    ↓
Reason Forgotten
```

With ADRs:

```text
Decision
    ↓
ADR
    ↓
Reason Preserved
```

---

# Why ADRs Exist

Code shows:

```text
What Was Built
```

ADR shows:

```text
Why It Was Built That Way
```

Example:

Code can tell us:

```text
The project uses SQL Server.
```

ADR explains:

```text
Why SQL Server was chosen instead of PostgreSQL.
```

This distinction becomes increasingly valuable as a project grows.

---

# ADR Template

Every ADR should follow this structure:

```md
# ADR-XXX

## Title

Short description of the decision.

---

## Status

Accepted

or

Proposed

or

Superseded

---

## Context

What problem are we trying to solve?

What constraints exist?

What options were considered?

---

## Decision

What decision was made?

---

## Rationale

Why was this decision selected?

---

## Consequences

Positive outcomes.

Negative outcomes.

Trade-offs.

---

## Future Revisit

Under what conditions should this decision be re-evaluated?
```

---

# ADR Status Values

## Proposed

The decision is being discussed.

Example:

```text
Should we use MediatR?
```

---

## Accepted

The decision has been approved and adopted.

Example:

```text
Use SQL Server.
```

---

## Superseded

The decision was previously accepted but later replaced.

Example:

```text
Use Blazor
```

Later:

```text
Use React
```

The original ADR becomes:

```text
Superseded
```

and a new ADR is created.

---

# ADR Writing Rules

## Rule 1

Document decisions, not implementations.

Good:

```text
Use SQL Server instead of PostgreSQL.
```

Avoid:

```text
Add DbContext configuration.
```

The second example belongs in a commit, not an ADR.

---

## Rule 2

Focus on important decisions.

Good candidates:

* architecture decisions
* technology choices
* major workflow changes
* project strategy changes

Avoid documenting trivial decisions.

Example:

```text
Rename a property
```

does not need an ADR.

---

## Rule 3

Always include trade-offs.

Every decision has advantages and disadvantages.

Good ADRs acknowledge both.

Example:

```text
Positive:
- Simpler architecture

Negative:
- Less flexibility
```

---

## Rule 4

Explain why alternatives were rejected.

A future reader should understand:

```text
Why Option A
instead of
Option B
```

---

## Rule 5

Keep ADRs small.

An ADR should usually be:

```text
1–2 pages
```

not:

```text
10 pages
```

The goal is clarity, not completeness.

---

# When Should An ADR Be Created?

Create an ADR when the answer to this question is "yes":

```text
Will future-me forget why this decision was made?
```

If yes:

Create an ADR.

---

# ADR Triggers For This Project

The following situations should normally create a new ADR.

## Technology Selection

Examples:

```text
SQL Server vs PostgreSQL

Blazor vs React

OpenAI vs Ollama
```

---

## Architecture Decisions

Examples:

```text
Use Clean Architecture

Adopt Repository Pattern

Introduce CQRS
```

---

## Project Strategy Decisions

Examples:

```text
Dashboard before Enterprise Refactor

AI after CQRS

Agentic AI after AI Integration
```

---

## Workflow Decisions

Examples:

```text
Feature Driven Learning

Conventional Commit Adoption

Documentation Structure Changes
```

---

# When Not To Create An ADR

Do not create ADRs for:

* bug fixes
* small refactors
* UI adjustments
* naming changes
* implementation details
* minor code cleanup

These belong in:

```text
Git Commits
Pull Requests
Documentation
```

not ADRs.

---

# ADR Philosophy For Obscura Finance Tracker

This project exists to learn enterprise development.

Therefore ADRs are considered educational artifacts as much as technical artifacts.

An ADR is successful when future-you can read it six months later and immediately understand:

```text
What decision was made?

Why was it made?

What alternatives existed?

Would we still make the same decision today?
```

If those questions can be answered quickly, the ADR has achieved its purpose.
