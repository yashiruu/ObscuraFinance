# Known Issues

## Purpose

This document tracks known issues, limitations, and technical observations discovered during development.

The purpose is not to record every minor bug.

The purpose is to maintain visibility of issues that may affect functionality, usability, maintainability, or future development.

This document should be updated whenever a meaningful issue is discovered.

---

# Current Status

Status:

⚠️ Known Architectural Observations

At the completion of the v1.2.0 Enterprise Foundation release, no unresolved release-blocking defects have been identified.

The following modules have been implemented and verified:

* Category Management
* Account Management
* Transaction Management
* Dashboard V1
* Enterprise Foundation

Previously identified functional issues have been resolved during development.

Examples include:

* Transaction account dropdown binding issue
* Transaction category dropdown binding issue
* ApiResponse client deserialization issue
* Validation exception middleware mapping issue

No active critical defects are currently being tracked.

Several architectural observations have been identified and are documented in this file for future improvement.

---

# Testing Status

Current testing has primarily consisted of:

* Feature development testing
* Manual workflow validation
* CRUD verification
* API verification
* UI interaction testing
* Development-time debugging
* Enterprise Foundation regression testing

The project has not yet undergone a dedicated full-system testing phase or automated testing implementation.

As a result, the absence of critical known issues should not be interpreted as proof that no defects exist.

It only indicates that no release-blocking defects are currently known.

---

# Deferred Validation

A comprehensive testing phase is intentionally postponed until after Phase 3.

Reasoning:

* Current focus is architectural learning and implementation.
* Phase 3 introduces significant data access refactoring.
* Performing a dedicated testing phase after Repository Pattern, Unit Of Work, Validation, and AutoMapper provides greater value.

The future validation phase should include:

* End-to-End Workflow Testing
* Cross-Module Integration Testing
* Soft Delete Verification
* API Validation Testing
* UI Consistency Review
* Data Integrity Verification
* Edge Case Testing
* Regression Testing

---

# Areas Requiring Future Verification

The following areas should receive special attention during the future testing phase.

## Category Management

Verify:

* Create category
* Update category
* Delete category
* Restore category
* Duplicate name scenarios
* Empty input validation

---

## Account Management

Verify:

* Create account
* Update account
* Delete account
* Restore account
* Initial balance handling
* Account type selection
* Active and inactive account behavior

---

## Transaction Management

Verify:

* Create transaction
* Update transaction
* Delete transaction
* Restore transaction
* Category relation integrity
* Account relation integrity
* Amount validation
* Date validation
* Transaction type handling

---

## Dashboard V1

Verify:

* Summary card calculations
* Recent transaction accuracy
* Expense aggregation accuracy
* Account summary accuracy
* Empty data scenarios
* Large dataset scenarios

---

# Technical Debt

No significant technical debt has been formally identified.

The following items are planned roadmap objectives and should not be considered technical debt:

* Repository Pattern
* Unit Of Work
* Validation
* AutoMapper
* Testing
* CQRS
* MediatR

These items represent planned architectural evolution rather than corrective actions.

---

# Architectural Observations

The following items are not considered release-blocking defects but have been identified as areas for future improvement.

## AO-001 — ApiResponse Coupling Between API and Client

Description:

The introduction of standardized API responses requires client applications to understand and deserialize `ApiResponse<T>`.

Current flow:

```text
WebApi
    ↓
ApiResponse<T>

Client
    ↓
Needs to understand ApiResponse<T>
```

Impact:

* Increased coupling between API and Client layers.
* Response deserialization logic is repeated across multiple client classes.
* Future response contract changes may require updates across all clients.

Future Improvement:

Consider introducing a shared helper method:

```csharp
ReadApiResponseAsync<T>()
```

to centralize response deserialization and reduce duplication.

Target Phase:

* Phase 3 — Data Access & Application Patterns

---

## AO-002 — Client Error Handling Is Still Primitive

Description:

Most client implementations currently rely on:

```csharp
response.EnsureSuccessStatusCode();
```

When the API returns a structured business error response:

```json
{
  "success": false,
  "message": "Category already exists"
}
```

the client receives only:

```text
HttpRequestException
```

and loses the business message provided by the API.

Impact:

* Reduced user feedback quality.
* Business error messages are not fully utilized.
* UI cannot consistently display meaningful validation messages.

Future Improvement:

Introduce a centralized response handling mechanism:

```csharp
ReadApiResponseAsync<T>()
```

Expected flow:

```text
Success = false
    ↓
Throw Business Exception
    ↓
UI displays meaningful message
```

Target Phase:

* Phase 3 — Data Access & Application Patterns
* Module 13 — Validation

---

# Risk Assessment

Current Risk Level:

Low

Reasoning:

* Core CRUD workflows are operational.
* Enterprise Foundation implementation has been completed.
* No release-blocking defects are known.
* Remaining issues are architectural and manageable.

Primary risk areas moving forward:

* Dashboard aggregation accuracy
* Data consistency after Repository Pattern implementation
* Soft delete behavior across future modules
* Client-side error handling consistency
* Regression issues introduced during architectural refactoring

---

# Release Assessment

Version:

v1.2.0

Assessment:

The application is considered stable enough to serve as the Enterprise Foundation release and the baseline for Phase 3 development.

No release-blocking issues are currently known.

---

# Next Review

The next known issues review should be performed:

* Before v1.5.0 release
* After Data Access Patterns implementation
* Before the first dedicated testing phase

At that time this document should be updated with:

* Newly discovered defects
* Resolved defects
* Deferred issues
* Testing results