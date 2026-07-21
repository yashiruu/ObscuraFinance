# Git Workflow

# Purpose

This document defines the Git workflow, release process, and versioning strategy used throughout the Obscura Finance Tracker project.

The objective is to maintain a clean development history while ensuring every architectural milestone is fully documented, tested, and reproducible.

---

# Branch Strategy

```text
main
 └── develop
      └── feature/*
```

## Branch Responsibilities

### main

Contains release history.

Examples:

```text
v1.0.0
v1.1.0
v1.2.0
```

Only release-ready code should be merged into main.

---

### develop

Primary development branch.

All completed features are integrated into develop before becoming part of a release.

---

### feature/*

Used for isolated development work.

Examples:

```text
feature/dashboard-v1
feature/interface-layer
feature/release-stabilization
```

---

# Feature Development Workflow

## Step 1 — Create Feature Branch

Create a feature branch from develop.

```bash
git checkout develop
git pull origin develop

git checkout -b feature/my-feature
```

---

## Step 2 — Implement Changes

Create focused commits.

Examples:

```text
feat(account): add account management workflow

fix(transaction): resolve dropdown binding issue

docs(release): freeze v1.0.0 project documentation
```

---

## Step 3 — Push Feature Branch

```bash
git push origin feature/my-feature
```

---

# Development Workflow

Every feature follows the same engineering process.

```text
Understand
    ↓
Discuss
    ↓
Implement
    ↓
Review
    ↓
Commit
    ↓
Update Documentation
    ↓
Push
```

Implementation is only considered complete after the related documentation has been reviewed.

---
# Merge Feature Into Develop

After the feature is completed:

```bash
git checkout develop
git pull origin develop

git merge feature/my-feature
```

Push develop:

```bash
git push origin develop
```

Result:

```text
feature commits
      │
      │
develop
```

Feature commits become part of develop history.

No merge commit is required.

---

# Feature Branch Cleanup

After a successful merge into develop:

```bash
git branch -d feature/my-feature
git push origin --delete feature/my-feature
```

Feature branches should be removed after they are no longer needed to keep the repository clean.

---

# Release Workflow

A release begins when all planned work for the milestone is completed.

Example:

```text
v1.1.0
Release Stabilization
```

---

## Step 1 — Verify Develop

Ensure develop is clean.

```bash
git status
```

Expected:

```text
working tree clean
```

---

## Step 2 — Update Main

```bash
git checkout main
git pull origin main
```

---

## Step 3 — Merge Develop Into Main

Always create a release merge commit.

```bash
git merge --no-ff develop
```

Example:

```text
release(v1.1.0): release stabilization milestone
```

---

## Step 4 — Push Main

```bash
git push origin main
```

---

## Step 5 — Create Tag

Create the release tag after the merge into main.

Example:

```bash
git tag -a v1.1.0 -m "Release v1.1.0 - Release Stabilization"
```

Push tag:

```bash
git push origin v1.1.0
```

---

## Step 6 — Create GitHub Release

Create a GitHub Release using the tag.

Example:

```text
Tag:
v1.1.0

Title:
v1.1.0 - Release Stabilization
```

---

# Release Checklist

Before creating a release:

- [ ] Working tree is clean.
- [ ] All planned modules are completed.
- [ ] Documentation has been reviewed.
- [ ] README reflects the current implementation.
- [ ] Project Status has been updated.
- [ ] Known Issues has been reviewed.
- [ ] Manual regression testing has been completed.
- [ ] Develop has been pushed.
- [ ] Release notes have been prepared.

---

# Versioning Strategy

The project follows Semantic Versioning (SemVer).

Version numbers communicate the significance of a release rather than the amount of work completed.

## Major Release (X.0.0)

Represents a major evolution of the application architecture or platform.

Examples:

* Agentic Finance Platform
* Production Platform

---

## Minor Release (1.X.0)

Represents a completed architectural milestone.

Examples:

* Core Finance Application
* Enterprise Foundation
* Data Access Patterns
* CQRS Architecture
* AI Integration

---

## Patch Release (1.2.X)

Represents maintenance work that does not introduce new architectural milestones.

Examples:

* Bug fixes
* Documentation updates
* Performance improvements
* Small UI improvements
* Internal refactoring

Every release should include:

* Updated documentation
* Release notes
* Git tag
* GitHub Release
---
# Expected History

## Feature Development

```text
feature/interface-layer
        │
        │
develop
```

Feature commits become part of develop.

---

## Release

```text
feature commits
       │
       │
develop
       │
       │
       M  ← release(v1.1.0)
       │
main
```

The release boundary is represented by the merge commit on main.

---

# Important Rules

## Rule 1

Always commit before merging.

Verify:

```bash
git status
```

Expected:

```text
working tree clean
```

---

## Rule 2

Always push develop before creating a release.

```bash
git push origin develop
```

This provides a recovery point.

---

## Rule 3

Do not use --no-ff when merging feature branches into develop.

Use:

```bash
git merge feature/my-feature
```

Not:

```bash
git merge --no-ff feature/my-feature
```

The develop branch should remain the primary development timeline.

---

## Rule 4

Always use --no-ff when merging develop into main.

Use:

```bash
git merge --no-ff develop
```

This creates a visible release milestone.

---

## Rule 5

Create tags only after the release merge into main.

Correct:

```text
Develop
    ↓
Main
    ↓
Tag
```

Incorrect:

```text
Tag
    ↓
Main
```

---

## Rule 6

Avoid history rewriting after a successful merge.

Avoid:

```bash
git reset --hard
git reset --keep
```

unless intentionally rewriting history and fully understanding the consequences.

---

# Release History Convention

```text
v1.0.0 → Core Finance Application
v1.1.0 → Release Stabilization
v1.2.0 → Enterprise Foundation
v1.3.0 → Data Access Patterns
v1.4.0 → CQRS Architecture
v1.5.0 → AI Integration
v2.0.0 → Agentic Finance Platform
v3.0.0 → Production Platform
```

---

# Release Philosophy

A release represents an architectural milestone rather than simply a collection of commits.

Every release should:

* Deliver a meaningful improvement.
* Preserve application stability.
* Update all relevant documentation.
* Leave the repository in a releasable state.

Releases should communicate the evolution of the project as clearly as the code itself.