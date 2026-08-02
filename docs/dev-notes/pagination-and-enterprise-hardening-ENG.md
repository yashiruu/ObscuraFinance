# Pagination & Enterprise Hardening

Companion notes to Module 16 — Pagination (see `04-project-status.md`, Current Priority). Covers the full review-and-fix cycle: bugs found in the initial pagination implementation, the resulting middleware hardening, an API contract audit, and a documentation sweep. Written after the fact so the reasoning survives past the commit messages.

Language: English. Indonesian version: `pagination-and-enterprise-hardening-IND.md`.

---

# Context

Pagination (`PagedRequest` / `PagedResult<T>`) had already been added to `GetAll` on `AccountController`, `CategoryController`, and `TransactionController` before this review started. The review's job was to check whether pagination had been applied everywhere it was needed, and it surfaced four separate problems that grew into four commits.

---

# 1. Pagination bug fixes and completeness — `feat(pagination)`

## What changed

* `PagedRequest.PageNumber` / `PageSize` now clamp to a minimum of 1 (`PageSize` already had an upper clamp of 50; the lower bound was missing).
* `Repository<TEntity>.GetAllAsync` now orders by `CreatedAt` then `Id` before `Skip`/`Take`.
* `PagedResult<T>.TotalPages` returns `0` instead of dividing by zero when `PageSize` is not positive.
* `CategoryController.GetDeleted` is now paginated end-to-end (`ICategoryRepository.GetAllDeletedAsync(pageNumber, pageSize)` → `ICategoryService.GetDeletedAsync(PagedRequest, ...)` → controller).
* `AccountClient`, `CategoryClient`, `TransactionClient`, and the affected Blazor pages were updated to consume `PagedResult<T>` instead of a raw list.
* Unit tests updated to assert against `PagedResult<T>.Items` instead of a raw collection.

## Why

* **Unclamped `PageNumber`/`PageSize`**: a request like `?PageNumber=0` or `?PageSize=-5` reached EF Core's `Skip()`/`Take()` unchanged. `Skip()` with a negative offset throws `ArgumentOutOfRangeException` — an unvalidated query parameter could 500 the endpoint.
* **Missing `OrderBy`**: without a deterministic sort, the database is free to return rows in a different order on every query. Two consecutive page requests could return duplicate rows, or silently skip rows, depending on how the query planner decided to satisfy `Skip`/`Take` that time. This is a well-known pagination pitfall and is the highest-severity fix in this batch.
* **`TotalPages` division by zero**: `PageSize` reaching 0 (before the clamp fix) meant `TotalCount / (double)0` → `NaN`, which casts to an undefined `int` value. Defensive guard added regardless, since `PagedResult<T>` can in principle be constructed manually.
* **`GetDeleted` left unpaginated**: every other list endpoint had been paginated; this one was missed. Same growth risk as any unpaginated list endpoint — it will eventually return every soft-deleted row in one response.
* **Clients broke silently**: once `GetAll` started returning `ApiResponse<PagedResult<T>>` instead of `ApiResponse<List<T>>`, the client deserialization target no longer matched the response shape. `System.Net.Http.Json` doesn't throw on a shape mismatch here — it just returns default/empty data, so the Blazor UI would have silently shown empty lists with no error. This was caught by rebuilding the whole solution, not by inspection.

## Known follow-up (not done, out of scope for this batch)

The Blazor list pages (`Accounts.razor`, `Categories.razor`, `Transactions.razor`, and the account/category pickers in the transaction create/edit forms) now request `pageSize: 50` as a stopgap so they keep showing "effectively everything" for the current small dataset. They do **not** have real pagination UI (no next/previous page controls) yet. If any list grows past 50 rows, the UI will silently truncate. This should be picked up as its own UI task before the entity counts get large.

## Before / After

`PagedRequest` only clamped the upper bound of `PageSize`; `PageNumber` had no clamp at all (`src/03.Shared/Models/PagedRequest.cs` — this file was still uncommitted at the start of the review, so this is the state it was reviewed in, not a prior commit):

```csharp
// Before
public class PagedRequest
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}

// After
public class PagedRequest
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;
    private int _pageNumber = 1;
    private int _pageSize = DefaultPageSize;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < 1) ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value < 1) ? DefaultPageSize : (value > MaxPageSize) ? MaxPageSize : value;
    }
}
```

`PagedResult<T>.TotalPages` before/after (same uncommitted-at-review-time note applies):

```csharp
// Before
public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

// After
public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
```

`Repository<TEntity>.GetAllAsync` — no ordering before `Skip`/`Take` (real diff, `src/05.Infrastructure/Persistence/Repositories/Repository.cs`):

```diff
-        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
+        public async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
         {
-            return await _dbSet.AsNoTracking().ToListAsync();
+            var totalCount = await _dbSet.CountAsync(cancellationToken);
+
+            // Apply Skip/Take pattern for efficient querying. A deterministic order is required:
+            // without it the database may return rows in a different order per query, causing
+            // duplicated or skipped rows across pages.
+            var items = await _dbSet
+                .AsNoTracking() // Optimization for read-only queries
+                .OrderBy(x => x.CreatedAt)
+                .ThenBy(x => x.Id)
+                .Skip((pageNumber - 1) * pageSize)
+                .Take(pageSize)
+                .ToListAsync(cancellationToken);
+
+            return (items, totalCount);
         }
```

`CategoryController.GetDeleted` — extending pagination to the last unpaginated list endpoint (`src/06.WebApi/Controllers/CategoryController.cs`):

```diff
         [HttpGet("deleted")]
-        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryResponse>>), StatusCodes.Status200OK)]
+        [ProducesResponseType(typeof(ApiResponse<PagedResult<CategoryResponse>>), StatusCodes.Status200OK)]
+        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
         [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
-        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetDeleted(CancellationToken cancellationToken)
+        public async Task<ActionResult<PagedResult<CategoryResponse>>> GetDeleted([FromQuery] PagedRequest request, CancellationToken cancellationToken)
         {
-            var categories = await _categoryService.GetDeletedAsync(cancellationToken);
+            var categories = await _categoryService.GetDeletedAsync(request, cancellationToken);

-            return Ok(ApiResponse<IEnumerable<CategoryResponse>>.SuccessResponse(categories, "Deleted categories retrieved successfully"));
+            return Ok(ApiResponse<PagedResult<CategoryResponse>>.SuccessResponse(categories, "Deleted categories retrieved successfully"));
         }
```

`AccountClient.GetAllAsync` — representative of the fix applied to all three list clients, whose return type silently stopped matching the API response shape (`src/07.Client/Features/AccountClient.cs`):

```diff
-        public async Task<List<AccountListResponse>> GetAllAsync()
+        public async Task<PagedResult<AccountListResponse>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
         {
-            var response = await _httpClient.GetAsync(ApiRoutes.Accounts);
+            var response = await _httpClient.GetAsync($"{ApiRoutes.Accounts}?pageNumber={pageNumber}&pageSize={pageSize}");

             response.EnsureSuccessStatusCode();

-            var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AccountListResponse>>>();
+            var result = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<AccountListResponse>>>();

-            return result?.Data ?? [];
+            return result?.Data ?? new PagedResult<AccountListResponse>();
         }
```

---

# 2. Global exception middleware hardening — `feat(middleware)`

## What changed

* `ApiResponse<T>` gained a `TraceId` property.
* `ExceptionMiddleware` now sets `response.TraceId = context.TraceIdentifier` on every error response.
* Added explicit handling for `DbUpdateConcurrencyException` and `DbUpdateException` → HTTP 409, instead of falling through to the generic 500 branch.
* Added a dedicated `catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)` branch that only logs, and does not attempt to write a response (the client is already gone).
* The 500 branch now includes `exception.ToString()` in the response body, but only when `IHostEnvironment.IsDevelopment()` is true.
* `Program.cs` now configures `ConfigureApiBehaviorOptions().InvalidModelStateResponseFactory` so that ASP.NET's automatic model-binding validation (triggered by `[ApiController]`) returns the same `ApiResponse<object>` envelope as `ExceptionMiddleware`, instead of the framework's default `ValidationProblemDetails` shape.

## Why

* **`TraceId`**: once a user reports "I got an error," there was no way to correlate their report with a specific log line without timestamps and guesswork. `HttpContext.TraceIdentifier` is already generated per-request by ASP.NET Core; surfacing it in the error response is a standard, low-cost way to close that gap.
* **`DbUpdateConcurrencyException` / `DbUpdateException` → 409**: these are the natural EF Core exceptions for "someone else changed this row" or "this write violates a constraint." Previously they fell into the generic 500 branch, which is both misleading (409 is the correct semantic code for a conflict) and less actionable for a client trying to decide whether to retry.
* **Client cancellation handling**: `OperationCanceledException` from an aborted request was previously caught by the generic `catch (Exception ex)` block, which would try to write an error response to a connection the client had already closed — at best wasted work, at worst an exception during exception handling. It's now recognized and treated as expected behavior, not a fault.
* **Dev-only exception detail**: the existing generic message ("An unexpected error occurred...") is correct for production but unhelpful while developing locally. Gating the detail behind `IsDevelopment()` keeps both properties true at once.
* **`InvalidModelStateResponseFactory`**: this was found while auditing pagination's own failure modes. A malformed `PageSize` query value (e.g. `?PageSize=abc`) fails model binding before the controller action runs, which ASP.NET Core's `[ApiController]` attribute already turns into an automatic 400 — but in the framework's own `ValidationProblemDetails` shape, not the project's `ApiResponse<T>` envelope. Every other error path in this API already returns `ApiResponse<T>`; this was the one gap.

## Before / After

`ExceptionMiddleware` — new exception types and correlation id (`src/06.WebApi/Middleware/ExceptionMiddleware.cs`):

```diff
         private readonly RequestDelegate _next;
         private readonly ILogger<ExceptionMiddleware> _logger;
+        private readonly IHostEnvironment _environment;

-        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
+        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
         {
             _next = next;
             _logger = logger;
+            _environment = environment;
         }

         public async Task InvokeAsync(HttpContext context)
         {
             try
             {
                 await _next(context);
             }
+            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
+            {
+                // The client disconnected or cancelled the request. There is no one left to respond to,
+                // so we only log it (at a low severity, since this is expected client behavior, not a fault).
+                _logger.LogInformation("Request was aborted by the client. Path: {Path}", context.Request.Path);
+            }
             catch (Exception ex)
             {
-                _logger.LogError(ex, "An exception occurred during request processing. Message: {Message}", ex.Message);
+                _logger.LogError(ex, "An exception occurred during request processing. TraceId: {TraceId}, Message: {Message}",
+                    context.TraceIdentifier, ex.Message);

                 await HandleExceptionAsync(context, ex);
             }
         }
```

```diff
                 UnauthorizedAccessException _ => (
                     StatusCodes.Status401Unauthorized,
                     ApiResponse<object>.ErrorResponse("You are not authorized to perform this action.")
                 ),

+                // Optimistic concurrency conflict (row changed/deleted between read and save)
+                DbUpdateConcurrencyException _ => (
+                    StatusCodes.Status409Conflict,
+                    ApiResponse<object>.ErrorResponse("The record was modified or deleted by another process. Please reload and try again.")
+                ),
+
+                // Other persistence failures (e.g. constraint violations). The raw provider error is
+                // never exposed to the client; it is only logged (above) for diagnostics.
+                DbUpdateException _ => (
+                    StatusCodes.Status409Conflict,
+                    ApiResponse<object>.ErrorResponse("The request could not be saved because it conflicts with existing data.")
+                ),
+
                 _ => (
                     StatusCodes.Status500InternalServerError,
-                    ApiResponse<object>.ErrorResponse("An unexpected error occurred. Please contact support if the issue persists.")
+                    ApiResponse<object>.ErrorResponse(
+                        "An unexpected error occurred. Please contact support if the issue persists.",
+                        _environment.IsDevelopment() ? [exception.ToString()] : null)
                 )
             };

+            // Attach the correlation id so the client can reference this specific request when reporting issues.
+            response.TraceId = context.TraceIdentifier;
+
             context.Response.StatusCode = statusCode;
             await context.Response.WriteAsJsonAsync(response);
```

`Program.cs` — consistent envelope for automatic model-state 400s (`src/06.WebApi/Program.cs`):

```diff
-builder.Services.AddControllers();
+builder.Services.AddControllers()
+    .ConfigureApiBehaviorOptions(options =>
+    {
+        options.InvalidModelStateResponseFactory = context =>
+        {
+            var errors = context.ModelState
+                .Where(entry => entry.Value?.Errors.Count > 0)
+                .SelectMany(entry => entry.Value!.Errors.Select(e => e.ErrorMessage))
+                .ToList();
+
+            var response = ApiResponse<object>.ErrorResponse("Validation Failed", errors);
+            response.TraceId = context.HttpContext.TraceIdentifier;
+
+            return new BadRequestObjectResult(response);
+        };
+    });
```

---

# 3. API contract accuracy fix — `fix(webapi)`

## What changed

`DashboardController.GetDashboardSummary` no longer declares `[ProducesResponseType(..., StatusCodes.Status401Unauthorized)]`.

## Why

This surfaced while cross-checking every controller's declared `ProducesResponseType` attributes against what `ExceptionMiddleware` can actually produce. `401` is only ever emitted by the middleware in response to `UnauthorizedAccessException` — and nothing in the codebase throws it, because there is no authentication middleware configured anywhere in `Program.cs` (`Module 17 — Authentication` is still on the roadmap, not yet started). A `401` in the OpenAPI/Swagger contract that can never actually be returned is worse than no documentation at all — it tells API consumers to handle a case that doesn't exist, and hides the fact that the endpoint is currently open to anyone. This will need to be re-added, correctly, once Module 17 ships.

## Before / After

`src/06.WebApi/Controllers/DashboardController.cs`:

```diff
         /// <response code="200">Returns the dashboard summary successfully.</response>
-        /// <response code="401">If the user is not authenticated or authorized.</response>
         /// <response code="500">If an unexpected internal server error occurs.</response>
         [HttpGet("summary")]
         [ProducesResponseType(typeof(ApiResponse<DashboardSummaryResponse>), StatusCodes.Status200OK)]
-        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
         [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
```

---

# 4. XML documentation sweep — `docs`

## What changed

Added concise English XML doc comments across:

* Domain layer: `BaseEntity`, `Account`, `Category`, `Transaction`, `AccountType`, `TransactionType`.
* All Account/Category/Transaction/Dashboard request and response DTOs (14 files).
* `IDashboardService` / `DashboardService`.
* Remaining Client project files: `ApiRoutes`, `DependencyInjection`, `DashboardClient`.
* (Controllers, service interfaces/implementations, and the other Client classes were documented in the same pass as items 1–3 above, since those files were already being touched for the fixes.)

No behavior changes in this commit — pure documentation.

## Why

The project's `06.WebApi.csproj` already has `GenerateDocumentationFile` enabled (with warning `1591` suppressed), meaning the intent to document the public API was already there, just not finished. Since Swagger/OpenAPI generation reads these XML comments directly into the generated spec, undocumented DTOs and domain types show up as unlabeled fields in the API docs consumers see — this is the same "enterprise API" goal already tracked as complete in `04-project-status.md`, extended to the layers that had been left out.

## Before / After

One representative example out of ~25 files touched in this commit — `src/01.Base/Entities/BaseEntity.cs`, the base class every domain entity inherits from:

```diff
     public abstract class BaseEntity
     {
+        /// <summary>Unique identifier of the entity.</summary>
         public Guid Id { get; set; }
+
+        /// <summary>UTC timestamp at which the entity was created.</summary>
         public DateTime CreatedAt { get; set; }
+
+        /// <summary>Id of the user who created the entity, if known.</summary>
         public Guid? CreatedBy { get; set; }
+
+        /// <summary>UTC timestamp of the last update, if any.</summary>
         public DateTime? UpdatedAt { get; set; }
+
+        /// <summary>Id of the user who last updated the entity, if known.</summary>
         public Guid? UpdatedBy { get; set; }
+
+        /// <summary>Whether the entity is soft-deleted. Soft-deleted entities are excluded by default query filters.</summary>
         public bool IsDeleted { get; set; } = false;
+
+        /// <summary>UTC timestamp at which the entity was soft-deleted, if any.</summary>
         public DateTime? DeletedAt { get; set; }
+
+        /// <summary>Id of the user who soft-deleted the entity, if known.</summary>
         public Guid? DeletedBy { get; set; }
     }
```

The other ~24 files (domain enums, DTOs, `IDashboardService`/`DashboardService`, `ApiRoutes`, `DependencyInjection`, `DashboardClient`) follow the same pattern: a class-level `<summary>` plus one `<summary>` per public member. Full diffs are in commit `32bd2f7`.

---

# Summary of commits

| Commit | Type | Scope |
|---|---|---|
| `870881e` | `feat(pagination)` | Page param validation, deterministic ordering, `GetDeleted` pagination, client/UI fixes |
| `ea7967b` | `feat(middleware)` | Correlation id, DB conflict handling, cancellation handling, consistent model-state errors |
| `5adc049` | `fix(webapi)` | Removed a `ProducesResponseType` that could never occur |
| `32bd2f7` | `docs` | XML documentation for domain, DTOs, and remaining client/service files |
