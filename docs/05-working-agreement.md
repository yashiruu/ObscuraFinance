# Working Agreement

## Purpose

This document defines how development decisions are made and how collaboration is performed throughout the project.

---

# Learning Strategy

We use:

```text id="vh2l6w"
Feature Driven Learning
```

Not:

```text id="um0m2v"
Study Theory For Months
    ↓
Start Coding
```

Instead:

```text id="l4o2tb"
Build Feature
    ↓
Understand Flow
    ↓
Refactor
    ↓
Build Next Feature
```

---

# Development Workflow

The project follows an iterative development workflow.

Rather than implementing architectural patterns immediately, each feature follows the same engineering cycle.

```text
Understand
    ↓
Discuss
    ↓
Implement
    ↓
Review
    ↓
Refactor
    ↓
Document
```

Understanding always comes before implementation.

Documentation is considered part of the implementation rather than a separate activity.

---

# Commit Philosophy

Commits are used as both:

* Source control checkpoints
* Learning documentation

The project follows Conventional Commit whenever possible.

Examples:

```text id="hj8v9q"
feat(account): add account management workflow

feat(transaction): add transaction dto requests and responses

fix(transaction): resolve account and category dropdown binding issue

refactor(transaction): simplify transaction form state handling
```

---

# Commit Categories

```text id="mh2z8p"
feat     → new feature
fix      → bug fix
refactor → internal restructuring
docs     → documentation changes
style    → formatting changes
test     → tests
chore    → maintenance
```

---

# Commit Rule

Every commit should represent one logical change.

Avoid:

```text id="tb4z8n"
Huge mixed commits
```

Prefer:

```text id="aq2g6e"
Small focused commits
```

---

# Collaboration Rule 1

## Documentation and Code Comments Must Use English

Applies to:

* Generated Code
* Reviewed Code
* XML Documentation
* Markdown Documentation
* README
* Architecture Notes
* Code Comments

Good:

```csharp
// Calculate monthly expense summary
```

Avoid:

```csharp
// Menghitung ringkasan pengeluaran bulanan
```

---

# Collaboration Rule 2

## Always Check Commit Readiness

After finishing a feature or refactor:

Ask:

```text id="gf7e1j"
Is this ready to commit?
```

If yes:

* Verify the implementation has been tested.
* Suggest a Conventional Commit message.
* Review whether any documentation requires updating.
* Confirm the working tree is clean before committing.

---

# Collaboration Rule 3

## Always Suggest The Next Step

After preparing a commit:

Always identify the next learning objective.

Example:

```text
Ready To Commit

feat(repository): introduce generic repository abstraction

Next Step

Implement Unit Of Work.
```

Learning should continue incrementally after every completed milestone.

---

# Collaboration Rule 4

## Keep Documentation Synchronized

Documentation evolves together with the implementation.

Whenever a significant architectural milestone is completed, review and update the relevant documentation.

This typically includes:

* README
* Learning Roadmap
* Project Status
* Architecture Guide
* Known Issues

Documentation is treated as part of the implementation rather than an optional task.

---

# Collaboration Rule 5

## Prefer Understanding Before Abstraction

Architectural patterns should never be introduced simply because they are commonly used.

Before implementing a new pattern:

1. Understand the problem.
2. Discuss the architectural trade-offs.
3. Implement the solution.
4. Review the implementation.
5. Refactor when understanding improves.

Learning why a pattern exists is more important than memorizing how to implement it.

---

# Architecture Principles

## Principle 1

Mirror enterprise structure.

---

## Principle 2

Avoid unnecessary complexity.

---

## Principle 3

Understand before abstracting.

---

## Principle 4

Refactor incrementally.

---

## Principle 5

Patterns are introduced because they solve a problem or teach a concept.

Never introduce complexity solely because a pattern is popular.

---

# Decision Making Rule

When multiple solutions exist:

Priority order:

```text id="i5l4tk"
Learning Value
    ↓
Maintainability
    ↓
Simplicity
    ↓
Performance
```

The project is primarily a learning platform.

Therefore learning value is considered first unless a technical constraint requires otherwise.

---

# Definition of Done

A module is considered complete when:

* The implementation is finished.
* Manual verification has been completed.
* Related documentation has been reviewed and updated.
* Project status reflects the latest progress.
* A Conventional Commit message has been prepared.
* The next learning objective has been identified.

Completion is defined by both working software and accurate documentation.

---

# Documentation Philosophy

Documentation is treated as a first-class artifact of the project.

Architecture, roadmap, and project status should accurately reflect the current implementation.

Outdated documentation should be corrected as part of normal development rather than postponed indefinitely.

Every architectural milestone should leave the codebase and its documentation in a consistent state.