# Git Workflow

## Purpose

This document defines the Git workflow used in the Obscura Finance Tracker project.

The goal is to maintain a clean development history while preserving release milestones.

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
v1.5.0 → Data Access Patterns
v1.8.0 → CQRS Architecture
v2.0.0 → AI Integration
```
