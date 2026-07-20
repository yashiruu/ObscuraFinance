# Known Issues

## Purpose

This document records known defects, architectural observations, quality risks, and verification activities throughout the project.

Its purpose is not only to track unresolved issues but also to document implementation quality and areas that require future validation.

The document evolves alongside the application and should be reviewed before every architectural release.

---

# Current Release

Version:

v1.2.0

Release:

Enterprise Foundation

Status:

✅ Stable

The current release is considered stable for continued development.

No release-blocking defects are currently known.

Development is proceeding toward:

```text
v1.3.0
Data Access Patterns
```

---

# Quality Assessment

Overall Status:

🟢 Stable

Assessment:

* Core CRUD workflows verified.
* Dashboard functionality verified.
* Enterprise Foundation completed.
* Repository Pattern completed.
* Unit Of Work completed.
* Validation completed.
* AutoMapper pending.
* Automated testing not yet implemented.

Current Confidence Level:

Medium

Reason:

The application has undergone extensive manual verification but has not yet entered the automated testing phase.

---

# Current Implementation Status

| Area | Status |
|-------|--------|
| Foundation | ✅ Stable |
| Category Management | ✅ Stable |
| Account Management | ✅ Stable |
| Transaction Management | ✅ Stable |
| Dashboard V1 | ✅ Stable |
| Enterprise Foundation | ✅ Stable |
| Repository Pattern | ✅ Stable |
| Unit Of Work | ✅ Stable |
| Validation | ✅ Stable |
| AutoMapper | 🚧 In Progress |
| Testing | ⏳ Planned |

---

# Testing Status

Completed:

* Manual CRUD testing
* Manual API verification
* Manual UI verification
* Dashboard verification
* Repository regression verification
* Unit Of Work verification
* Validation verification

Planned:

* Unit Testing
* Integration Testing
* End-to-End Testing
* Performance Testing

---

# Deferred Verification

The following validations are intentionally postponed until Phase 3 is completed.

Reason:

Testing becomes significantly more valuable once the application architecture has stabilized.

Planned verification includes:

* Cross-module regression testing
* Data integrity verification
* Large dataset verification
* Performance verification
* UI consistency review

---

---

# AI-Assisted Test Authoring — Review Status

Status:

⚠️ Pending Self-Review

Context:

The unit test suite for Module 15 (AccountServiceTest and subsequent service
tests) was authored with AI assistance (Claude) through a guided, concept-first
process. The developer actively participated in reasoning through each test's
Arrange/Act/Assert structure and understands the underlying testing concepts
(Mock/Base/Builder roles, orchestration verification, exception-testing pattern).
However, the majority of the actual test code was written by the AI based on
that shared understanding, not typed independently by the developer line-by-line.

Required Follow-Up:

1. **Self-Review Pass**
   Once time permits, revisit each test file written during this phase and
   verify the developer can explain every Arrange/Act/Assert line without
   referring back to the AI conversation — not just recognize it as correct.

2. **Re-Validate Against Business Logic Changes**
   Whenever `AccountService`, `CategoryService`, `TransactionService`, or
   `DashboardService` business logic is modified going forward, the
   corresponding AI-authored tests must be re-reviewed — not assumed to
   still be accurate. AI-authored tests reflect the business logic AS IT
   EXISTED at the time of writing; they do not automatically track future
   changes.

3. **Coverage Gap Check**
   Test scenarios not yet covered (or covered thinly) should be identified
   and extended once the developer is comfortable designing test cases
   independently, rather than only extending patterns already demonstrated
   by the AI.

Priority:

Medium — does not block Module 15 progress, but must be completed before
these tests are treated as a reliable regression safety net for future
refactoring (e.g. before AO-004 fixes are implemented).

---

# Functional Verification

## Category Management

Verify:

* Create
* Update
* Delete
* Restore
* Duplicate names
* Empty input

---

## Account Management

Verify:

* Create
* Update
* Delete
* Restore
* Initial Balance
* Account Type
* Active Status

---

## Transaction Management

Verify:

* Create
* Update
* Delete
* Restore
* Account Relation
* Category Relation
* Amount Validation
* Date Validation

---

## Dashboard

Verify:

* Summary Cards
* Recent Transactions
* Expense Aggregation
* Account Summary
* Empty Dataset
* Large Dataset

---

# Architectural Observations

Architectural observations are not considered defects.

They identify areas that can be improved in future milestones.

---

## AO-001

### ApiResponse Coupling

Status:

Open

Target Release:

v1.3.0

Description:

Client applications currently deserialize ApiResponse<T> individually.

Potential Improvement:

Introduce:

```csharp
ReadApiResponseAsync<T>()
```

to centralize response handling.

Priority:

Medium

---

## AO-002

### Client Error Handling

Status:

Open

Target Release:

v1.3.0

Description:

Business errors are not consistently propagated to the UI.

Potential Improvement:

Centralize client response handling.

Priority:

Medium

---

## AO-003

### ApiResponse Factory Method Bypass Pattern

Status:

Open

Target Release:

Post v1.3.0 (after Module 15 — Testing)

Description:

`ApiResponse<T>` is designed to be constructed exclusively through its factory methods —
`SuccessResponse()` and `ErrorResponse()` — which populate fields such as `Timestamp`
automatically. However, nothing currently prevents `ApiResponse<T>` from being constructed
directly via an object initializer instead of the factory methods.

```csharp
// Intended
return ApiResponse<T>.SuccessResponse(data);

// Bypass — compiles fine, but skips factory-populated fields
return new ApiResponse<T> { Data = data, Success = true };
```

Impact:

An object initializer bypasses `Timestamp` population (and any other logic the factory
methods are responsible for). Currently the codebase follows the factory convention
consistently by discipline, but the compiler does not enforce it — nothing stops a future
addition (including AI-generated or copy-pasted code) from silently constructing an
inconsistent response.

Root Cause:

`ApiResponse<T>` likely exposes a public parameterless constructor and public settable
properties alongside its factory methods, so both construction paths remain available.

Suggested Fix:

Constrain construction to the factory methods only, e.g.:

* Make the constructor `private` (or `internal`), forcing all

---

## AO-004

### Cross-Service Data Access Gaps (Account, Category, Transaction)

Status:

Open

Target Release:

Post v1.3.0 (after Module 15 — Testing)

Description:

A cross-service review of `AccountService`, `CategoryService`, and `TransactionService`
identified three recurring gaps that follow the same pattern across all three services.
Documented together because the fix should be designed once and applied consistently,
rather than patched per-service.

---

#### Gap 1 — CancellationToken Not Propagated to Queries

All three services accept `CancellationToken` on every method, but only forward it to
`SaveChangesAsync()`. Repository calls such as `GetByIdAsync`, `IsNameTakenAsync`,
`GetAllAsync`, `ExistsAsync`, `GetAllByTypeAsync`, and `GetAllDeletedAsync` do not receive
the token at all.

Impact:

A cancelled request still executes all reads before cancellation is ever observed.

Root Cause:

Likely originates at `IRepository<TEntity>` (and entity-specific interfaces) not accepting
`CancellationToken` on query methods, not a per-service mistake.

Affected:

* AccountService — all query calls
* CategoryService — all query calls
* TransactionService — all query calls

---

#### Gap 2 — Redundant Duplicate-Name Check on Update When Name Is Unchanged

`AccountService.UpdateAsync` and `CategoryService.UpdateAsync` always call
`IsNameTakenAsync(request.Name, excludeId: id)`, even when `request.Name` is identical to
the existing entity's name. This is an unnecessary query on the common case where a user
updates other fields without renaming.

Impact:

Minor — one avoidable query per update when name is unchanged. Not a correctness bug.

Suggested Fix:

Short-circuit the check:

```csharp
if (!string.Equals(entity.Name, request.Name, StringComparison.Ordinal))
{
    var takenName = await _unitOfWork.X.IsNameTakenAsync(request.Name, excludeId: id);
    ...
}
```

Affected:

* AccountService.UpdateAsync
* CategoryService.UpdateAsync
* (Not applicable to TransactionService — Transaction has no Name uniqueness constraint)

---

#### Gap 3 — DeleteAsync Missing Referential Integrity Checks

`DeleteAsync` in all three services soft-deletes the entity without checking whether it is
referenced elsewhere.

Risk by entity:

| Entity | Risk Level | Reason |
|--------|-----------|--------|
| Category | High | `Transaction.CategoryId` is a required FK. Soft-deleting a Category still referenced by active Transactions can produce dashboard/dropdown inconsistencies (e.g. "Expense by Category" pointing to a category no longer listed). |
| Account | Medium | Same FK relationship via `Transaction.AccountId`, slightly lower visible impact than Category. |
| Transaction | Low (currently) | No entity references Transaction yet. Will become relevant once `ObligationTransactions` (Module 3.5) is implemented and starts referencing Transaction. |

Suggested Fix:

Add a referential check (e.g. `_unitOfWork.Transactions.ExistsAsync(t => t.CategoryId == id)`)
before allowing delete, or document soft-delete-with-orphan-reference as accepted behavior
if intentional.

Priority order for fixing: Category → Account → Transaction (defer Transaction until
Module 3.5 introduces the referencing entity).

---

Priority:

Medium

Rationale for Deferral:

Per working agreement, business logic should stabilize before Module 15 (Testing) writes
tests around it. However, per Collaboration Rule 5 (Understand Before Abstraction) and the
existing plan to complete Module 15 first, these three gaps are deferred until after Testing
is complete so that:

1. Tests can first be written against current (known-gap) behavior.
2. The CancellationToken and referential-integrity fixes can be designed once at the
   interface/repository level and verified by the new test suite immediately after.

Next Review:

After Module 15 — Testing is complete, before v1.3.0 release.

---

# Technical Debt

Current Status:

🟢 Low

The following items are planned architectural evolution and should not be considered technical debt:

* AutoMapper
* Testing
* CQRS
* MediatR
* AI Integration

---

# Risk Register

| Risk | Impact | Mitigation |
|------|--------|------------|
| Regression after AutoMapper | Medium | Automated Testing |
| CQRS Refactoring | Medium | Incremental Migration |
| Dashboard Aggregation Accuracy | Low | Integration Testing |
| Client Error Consistency | Medium | Centralized Response Handling |

---

# Release Assessment

Current Release:

v1.2.0

Assessment:

✅ Approved as the Enterprise Foundation baseline.

The project is ready to continue toward Data Access Patterns.

---

# Next Review

Perform the next review:

* Before v1.3.0
* After AutoMapper
* After Testing
* Before CQRS implementation

Review:

* Known Issues
* Risk Register
* Technical Debt
* Testing Status
* Architectural Observations