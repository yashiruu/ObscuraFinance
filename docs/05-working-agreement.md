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

* State that the change is ready to commit
* Suggest a Conventional Commit message

---

# Collaboration Rule 3

## Always Suggest The Next Step

After suggesting a commit:

Always provide:

```text id="y7j8pr"
Next Step
```

Example:

```text id="fh5l2v"
Ready To Commit:
feat(transaction): add transaction summary endpoint

Next Step:
Implement Dashboard V1 summary cards.
```

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
