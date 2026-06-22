# ADR-001: Phase 2 Implementation Order

## Status

Accepted

## Context

The original roadmap places Global Query Filter at the end of Phase 2.

However, the application already implements soft delete using:

- IsDeleted
- DeletedAt
- DeletedBy

Global Query Filter provides immediate value and introduces minimal architectural risk.

## Decision

Phase 2 will be implemented in the following order:

1. Global Query Filter
2. Interface
3. Service Layer
4. Response Standardization
5. Middleware
6. Logging

## Consequences

Benefits:

- Immediate improvement to soft delete behavior.
- Smaller initial refactoring scope.
- Service Layer naturally builds on interfaces.
- Middleware benefits from standardized responses.
- Logging becomes more meaningful after the request pipeline is established.

Drawbacks:

- Execution order differs from the original roadmap.
- Documentation must clarify the distinction between roadmap sequence and implementation sequence.