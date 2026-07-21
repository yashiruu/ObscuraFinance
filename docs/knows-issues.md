# Known Issues

This document records known technical limitations, architectural observations, and implementation decisions.

Its purpose is to preserve context for future improvements while avoiding premature optimization.

---

# Current Release

Version:

v1.3.0

Release:

Data Access & Application Patterns

Status:

✅ Stable

The current release is considered stable for continued development.

No release-blocking defects are currently known.

Next Target:

```text
v1.4.0
CQRS Architecture
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
* FluentValidation completed.
* AutoMapper completed.
* Automated unit testing implemented.

Current Confidence Level:

High

Reason:

The application has been verified through both manual testing and automated unit tests for core business services and request validators. The project now has a stable testing foundation, providing high confidence for future architectural refactoring.

---

# Current Implementation Status

| Area | Status |
|------|--------|
| Foundation | ✅ Stable |
| Category Management | ✅ Stable |
| Account Management | ✅ Stable |
| Transaction Management | ✅ Stable |
| Dashboard V1 | ✅ Stable |
| Enterprise Foundation | ✅ Stable |
| Repository Pattern | ✅ Stable |
| Unit Of Work | ✅ Stable |
| Validation | ✅ Stable |
| AutoMapper | ✅ Stable |
| Testing | ✅ Stable |

---

# Testing Status

Completed:

* Manual CRUD testing
* Manual API verification
* Manual UI verification
* Dashboard verification
* Repository regression verification
* Unit Of Work verification
* FluentValidation verification
* AccountService unit tests
* CategoryService unit tests
* TransactionService unit tests
* DashboardService unit tests
* Request validator unit tests

Current Test Coverage:

* Business Services
* Request Validation
* Repository Integration (through service tests)

Planned:

* Integration Testing
* End-to-End Testing
* Performance Testing

Reason:

The project now has comprehensive unit testing. Remaining verification focuses on cross-component behavior and production-scale scenarios that are better introduced after the CQRS architecture has stabilized.

---

# Release Summary

Version v1.3.0 focuses on architectural maturity rather than user-facing features.

Highlights:

* Repository Pattern completed.
* Unit Of Work completed.
* FluentValidation integrated.
* AutoMapper integrated.
* Comprehensive unit testing introduced.
* Shared testing infrastructure established.
* Core services protected by automated regression tests.

The application is now considered ready for architectural evolution toward CQRS.

---

# Architectural Observations

Architectural Observations (AO) capture implementation decisions, trade-offs, and future improvements.

These are intentionally retained even when they are not release blockers.

---

## AO-001 — Generic Repository Query Flexibility

Status:

🟡 Deferred

Priority:

Medium

Target:

Post v1.3.0

Observation:

The current generic repository exposes only the operations required by existing features.

As more advanced querying scenarios emerge, additional abstractions such as specifications or query objects may become beneficial.

Reason for Deferral:

The existing implementation remains simple, readable, and sufficient for the current application size.

Premature abstraction would introduce unnecessary complexity before CQRS.

---

## AO-002 — Unit Of Work Growth

Status:

🟡 Deferred

Priority:

Medium

Target:

Post v1.3.0

Observation:

As additional repositories are introduced, the Unit Of Work interface may continue to grow.

Possible future improvements include:

* Feature-based repositories
* Aggregate-oriented organization
* CQRS handlers reducing repository exposure

Reason for Deferral:

Current repository count remains manageable.

CQRS will likely reshape repository usage, making early refactoring unnecessary.

---

## AO-003 — Integration Testing

Status:

🟡 Planned

Priority:

Medium

Target:

Future Release

Observation:

Current automated testing focuses on unit testing.

Cross-layer verification has intentionally been postponed.

Planned Coverage:

* Controller → Service → Repository
* Database Integration
* Middleware Pipeline
* API Contract Verification

Reason:

Unit testing provides the highest return during the current architectural phase.

Integration testing becomes more valuable once CQRS stabilizes.

---

## AO-004 — Performance Optimization

Status:

🟢 Not Required

Priority:

Low

Observation:

No performance bottlenecks have been identified.

Database size remains small.

API response times are acceptable.

Decision:

Avoid optimization until supported by measurable evidence.

---

## AO-005 — CQRS Readiness

Status:

🟢 Ready

Priority:

High

Observation:

The application has successfully completed the architectural prerequisites for CQRS.

Completed Prerequisites:

* Interface abstraction
* Service layer
* Repository Pattern
* Unit Of Work
* FluentValidation
* AutoMapper
* Centralized exception handling
* Standardized API responses
* Automated unit testing

Conclusion:

The application is architecturally prepared to begin CQRS implementation in Phase 4.

---

## AO-006 — ApiResponse Factory Method Bypass Pattern

Status:

🟡 Open — On Hold

Priority:

Medium

Target:

Post v1.3.0

Observation:

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

Constrain construction to the factory methods only, e.g. make the constructor `private`
(or `internal`), forcing all response creation through `SuccessResponse()` / `ErrorResponse()`.

Note:

Originally tracked in the v1.2.0 known-issues document. Confirmed still unresolved — held
intentionally, not yet scheduled. Will remain open until deliberately picked up.

---

## AO-007 — Cross-Service Data Access Gaps (Account, Category, Transaction)

Status:

🟡 Open — On Hold

Priority:

Medium

Target:

Post v1.3.0

Observation:

A cross-service review of `AccountService`, `CategoryService`, and `TransactionService`
identified three recurring gaps that follow the same pattern across all three services.
Documented together because the fix should be designed once and applied consistently,
rather than patched per-service.

### Gap 1 — CancellationToken Not Propagated to Queries

All three services accept `CancellationToken` on every method, but only forward it to
`SaveChangesAsync()`. Repository calls such as `GetByIdAsync`, `IsNameTakenAsync`,
`GetAllAsync`, `ExistsAsync`, `GetAllByTypeAsync`, and `GetAllDeletedAsync` do not receive
the token at all.

Impact: A cancelled request still executes all reads before cancellation is ever observed.

Root Cause: Likely originates at `IRepository<TEntity>` (and entity-specific interfaces)
not accepting `CancellationToken` on query methods, not a per-service mistake.

Affected: AccountService, CategoryService, TransactionService — all query calls.

### Gap 2 — Redundant Duplicate-Name Check on Update When Name Is Unchanged

`AccountService.UpdateAsync` and `CategoryService.UpdateAsync` always call
`IsNameTakenAsync(request.Name, excludeId: id)`, even when `request.Name` is identical to
the existing entity's name.

Impact: Minor — one avoidable query per update when name is unchanged. Not a correctness bug.

Suggested Fix:

```csharp
if (!string.Equals(entity.Name, request.Name, StringComparison.Ordinal))
{
    var takenName = await _unitOfWork.X.IsNameTakenAsync(request.Name, excludeId: id);
    ...
}
```

Affected: AccountService.UpdateAsync, CategoryService.UpdateAsync
(Not applicable to TransactionService — Transaction has no Name uniqueness constraint.)

### Gap 3 — DeleteAsync Missing Referential Integrity Checks

`DeleteAsync` in all three services soft-deletes the entity without checking whether it is
referenced elsewhere.

| Entity | Risk Level | Reason |
|--------|-----------|--------|
| Category | High | `Transaction.CategoryId` is a required FK. Soft-deleting a Category still referenced by active Transactions can produce dashboard/dropdown inconsistencies. |
| Account | Medium | Same FK relationship via `Transaction.AccountId`, slightly lower visible impact. |
| Transaction | Low (currently) | No entity references Transaction yet. Becomes relevant once `ObligationTransactions` (Module 3.5) is implemented. |

Suggested Fix:

Add a referential check (e.g. `_unitOfWork.Transactions.ExistsAsync(t => t.CategoryId == id)`)
before allowing delete, or document soft-delete-with-orphan-reference as accepted behavior
if intentional.

Priority order for fixing: Category → Account → Transaction (defer Transaction until
Module 3.5 introduces the referencing entity).

Note:

Originally tracked in the v1.2.0 known-issues document as AO-004. Confirmed still
unresolved — held intentionally, not yet scheduled. If any Module 15 tests document this
current (gap-exposing) behavior, they should follow the `_AO007` naming suffix per the unit
test naming convention.

---

# AI-Assisted Test Authoring

Status:

⚠️ Pending Self-Review

Release Context:

This note documents technical debt related to knowledge ownership rather than software correctness. It does not affect the stability of v1.3.0.

Background:

A significant portion of the unit test suite was initially authored with AI assistance.

Although every generated test has been executed successfully, not every implementation has yet been manually reconstructed from first principles.

Risk:

Future modifications may become difficult if the underlying testing patterns are not fully internalized.

Mitigation Plan:

* Re-read every test class.
* Rewrite selected tests without AI assistance.
* Ensure complete understanding of mocking strategy.
* Ensure complete understanding of Arrange-Act-Assert flow.
* Ensure confidence in extending the test suite independently.

Success Criteria:

This observation can be closed once new tests can be designed and implemented without relying on AI-generated examples.

---

# Overall Assessment

Current Stability:

🟢 High

Release Readiness:

✅ Approved

Technical Debt:

Low to Medium — AO-006 and AO-007 remain open and on hold by deliberate choice

Architectural Readiness:

✅ Ready for Phase 4

The project has reached a stable architectural baseline.

Future work should prioritize new architecture (CQRS) rather than additional refactoring of the existing layered design. AO-006 and AO-007 are intentionally held rather than scheduled; since both touch service classes that CQRS will restructure (Commands/Queries/Handlers), it may be more efficient to address them as part of that refactor rather than fixing them twice.

Current technical debt is intentional, documented, and considered acceptable for the next phase of development.