# Pagination & Enterprise Hardening

Catatan pendamping untuk Module 16 — Pagination (lihat `04-project-status.md`, bagian Current Priority). Mencakup satu siklus penuh review-dan-perbaikan: bug yang ditemukan di implementasi pagination awal, hasil pengerasan (hardening) middleware, audit kontrak API, dan pembersihan dokumentasi. Ditulis setelah eksekusi supaya alasan di balik tiap perubahan tidak hilang begitu saja setelah pesan commit.

Bahasa: Indonesia. Versi Inggris: `pagination-and-enterprise-hardening-ENG.md`.

---

# Konteks

Pagination (`PagedRequest` / `PagedResult<T>`) sudah lebih dulu ditambahkan ke `GetAll` pada `AccountController`, `CategoryController`, dan `TransactionController` sebelum review ini dimulai. Tugas review adalah memastikan pagination sudah diterapkan di semua tempat yang membutuhkannya — dan dari situ ditemukan empat masalah terpisah yang akhirnya menjadi empat commit.

---

# 1. Perbaikan bug pagination & kelengkapannya — `feat(pagination)`

## Apa yang berubah

* `PagedRequest.PageNumber` / `PageSize` sekarang di-clamp minimal 1 (`PageSize` sebelumnya sudah punya batas atas 50; batas bawahnya yang belum ada).
* `Repository<TEntity>.GetAllAsync` sekarang mengurutkan berdasarkan `CreatedAt` lalu `Id` sebelum `Skip`/`Take`.
* `PagedResult<T>.TotalPages` mengembalikan `0`, bukan hasil pembagian dengan nol, ketika `PageSize` tidak positif.
* `CategoryController.GetDeleted` sekarang paginated end-to-end (`ICategoryRepository.GetAllDeletedAsync(pageNumber, pageSize)` → `ICategoryService.GetDeletedAsync(PagedRequest, ...)` → controller).
* `AccountClient`, `CategoryClient`, `TransactionClient`, dan halaman Blazor yang terdampak diperbarui agar membaca `PagedResult<T>`, bukan list mentah.
* Unit test diperbarui agar mengecek `PagedResult<T>.Items`, bukan koleksi mentah.

## Alasan

* **`PageNumber`/`PageSize` tidak di-clamp**: request seperti `?PageNumber=0` atau `?PageSize=-5` sampai ke `Skip()`/`Take()` milik EF Core tanpa validasi. `Skip()` dengan offset negatif melempar `ArgumentOutOfRangeException` — artinya satu query parameter yang tidak divalidasi bisa bikin endpoint balikin 500.
* **`OrderBy` yang hilang**: tanpa urutan yang deterministik, database bebas mengembalikan baris dalam urutan berbeda di setiap query. Dua request halaman berurutan bisa saja mengembalikan baris duplikat, atau malah melewatkan baris, tergantung bagaimana query planner memutuskan mengeksekusi `Skip`/`Take` saat itu. Ini jebakan pagination yang sudah dikenal luas, dan jadi perbaikan dengan tingkat keparahan tertinggi di batch ini.
* **`TotalPages` pembagian dengan nol**: `PageSize` yang bisa mencapai 0 (sebelum perbaikan clamp) berarti `TotalCount / (double)0` → `NaN`, yang saat di-cast ke `int` hasilnya tidak terdefinisi. Guard tetap ditambahkan sebagai jaga-jaga, karena `PagedResult<T>` pada dasarnya bisa saja dibuat secara manual di tempat lain.
* **`GetDeleted` belum paginated**: semua endpoint list lainnya sudah paginated; endpoint ini terlewat. Risikonya sama seperti endpoint list mana pun yang belum paginated — cepat atau lambat akan mengembalikan seluruh baris yang di-soft-delete dalam satu response.
* **Client rusak diam-diam**: begitu `GetAll` mulai mengembalikan `ApiResponse<PagedResult<T>>` alih-alih `ApiResponse<List<T>>`, target deserialisasi di client tidak lagi cocok dengan bentuk response-nya. `System.Net.Http.Json` tidak melempar error untuk ketidakcocokan bentuk semacam ini — dia cuma mengembalikan data default/kosong, jadi UI Blazor akan diam-diam menampilkan list kosong tanpa error apa pun. Ini ketahuan saat build ulang seluruh solution, bukan dari membaca kode.

## Tindak lanjut yang belum dikerjakan (di luar scope batch ini)

Halaman list Blazor (`Accounts.razor`, `Categories.razor`, `Transactions.razor`, serta picker akun/kategori di form create/edit transaksi) sekarang meminta `pageSize: 50` sebagai solusi sementara supaya tetap terlihat menampilkan "semua data" untuk dataset kecil saat ini. Halaman-halaman itu **belum** punya UI pagination sungguhan (belum ada tombol next/previous). Kalau ada list yang tumbuh lebih dari 50 baris, UI-nya akan diam-diam terpotong. Ini sebaiknya diambil sebagai task UI tersendiri sebelum jumlah data jadi besar.

## Sebelum / Sesudah

`PagedRequest` sebelumnya cuma meng-clamp batas atas `PageSize`; `PageNumber` sama sekali belum di-clamp (`src/03.Shared/Models/PagedRequest.cs` — file ini masih belum ter-commit saat review dimulai, jadi ini kondisi saat direview, bukan commit sebelumnya):

```csharp
// Sebelum
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

// Sesudah
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

`PagedResult<T>.TotalPages` sebelum/sesudah (catatan yang sama berlaku — belum ter-commit saat direview):

```csharp
// Sebelum
public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

// Sesudah
public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
```

`Repository<TEntity>.GetAllAsync` — belum ada pengurutan sebelum `Skip`/`Take` (diff asli, `src/05.Infrastructure/Persistence/Repositories/Repository.cs`):

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

`CategoryController.GetDeleted` — memperluas pagination ke satu-satunya endpoint list yang belum paginated (`src/06.WebApi/Controllers/CategoryController.cs`):

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

`AccountClient.GetAllAsync` — mewakili perbaikan yang diterapkan ke ketiga client list, yang return type-nya diam-diam sudah tidak cocok lagi dengan bentuk response API (`src/07.Client/Features/AccountClient.cs`):

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

# 2. Pengerasan global exception middleware — `feat(middleware)`

## Apa yang berubah

* `ApiResponse<T>` dapat properti baru `TraceId`.
* `ExceptionMiddleware` sekarang mengisi `response.TraceId = context.TraceIdentifier` di setiap response error.
* Ditambah penanganan eksplisit untuk `DbUpdateConcurrencyException` dan `DbUpdateException` → HTTP 409, alih-alih jatuh ke cabang 500 generik.
* Ditambah cabang khusus `catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)` yang hanya mencatat log, tanpa mencoba menulis response (karena client-nya sudah terputus).
* Cabang 500 sekarang menyertakan `exception.ToString()` di body response, tapi hanya kalau `IHostEnvironment.IsDevelopment()` bernilai true.
* `Program.cs` sekarang mengonfigurasi `ConfigureApiBehaviorOptions().InvalidModelStateResponseFactory` supaya validasi model-binding otomatis dari ASP.NET (dipicu oleh `[ApiController]`) mengembalikan envelope `ApiResponse<object>` yang sama seperti `ExceptionMiddleware`, bukan bentuk `ValidationProblemDetails` bawaan framework.

## Alasan

* **`TraceId`**: begitu user melapor "saya dapat error," sebelumnya tidak ada cara mengaitkan laporan itu dengan baris log tertentu tanpa modal timestamp dan tebak-tebakan. `HttpContext.TraceIdentifier` sebenarnya sudah dibuat otomatis oleh ASP.NET Core per-request; menampilkannya di response error adalah cara standar dan murah untuk menutup celah itu.
* **`DbUpdateConcurrencyException` / `DbUpdateException` → 409**: ini exception wajar dari EF Core untuk kasus "ada orang lain yang sudah mengubah baris ini" atau "penulisan ini melanggar constraint." Sebelumnya keduanya jatuh ke cabang 500 generik, yang menyesatkan (409 adalah kode semantik yang benar untuk konflik) dan kurang bisa ditindaklanjuti oleh client yang mau memutuskan apakah perlu retry atau tidak.
* **Penanganan cancellation dari client**: `OperationCanceledException` akibat request yang dibatalkan sebelumnya tertangkap oleh blok `catch (Exception ex)` generik, yang lalu mencoba menulis response error ke koneksi yang sudah ditutup client — paling ringan cuma kerja sia-sia, paling parah exception di tengah penanganan exception. Sekarang kasus ini dikenali dan diperlakukan sebagai perilaku yang memang diharapkan, bukan kegagalan.
* **Detail exception khusus dev**: pesan generik yang sudah ada ("An unexpected error occurred...") sudah benar untuk production tapi kurang membantu saat development lokal. Membatasi detail itu hanya muncul saat `IsDevelopment()` membuat kedua kebutuhan itu terpenuhi sekaligus.
* **`InvalidModelStateResponseFactory`**: ini ditemukan saat mengaudit mode kegagalan pagination sendiri. Nilai `PageSize` yang salah format (misalnya `?PageSize=abc`) gagal di tahap model binding sebelum action controller sempat jalan, yang oleh atribut `[ApiController]` milik ASP.NET Core sudah otomatis diubah jadi 400 — tapi dalam bentuk `ValidationProblemDetails` bawaan framework, bukan envelope `ApiResponse<T>` milik project ini. Semua jalur error lain di API ini sudah mengembalikan `ApiResponse<T>`; ini satu-satunya celah yang tersisa.

## Sebelum / Sesudah

`ExceptionMiddleware` — jenis exception baru dan correlation id (`src/06.WebApi/Middleware/ExceptionMiddleware.cs`):

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

`Program.cs` — envelope yang konsisten untuk 400 otomatis dari model-state (`src/06.WebApi/Program.cs`):

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

# 3. Perbaikan akurasi kontrak API — `fix(webapi)`

## Apa yang berubah

`DashboardController.GetDashboardSummary` tidak lagi mendeklarasikan `[ProducesResponseType(..., StatusCodes.Status401Unauthorized)]`.

## Alasan

Ini ketahuan saat mencocokkan atribut `ProducesResponseType` di setiap controller dengan apa yang benar-benar bisa dihasilkan `ExceptionMiddleware`. `401` hanya pernah dikeluarkan middleware sebagai respons terhadap `UnauthorizedAccessException` — dan tidak ada satu pun kode di project ini yang melempar exception itu, karena memang belum ada middleware autentikasi yang dikonfigurasi di `Program.cs` mana pun (`Module 17 — Authentication` masih di roadmap, belum dimulai). `401` di kontrak OpenAPI/Swagger yang sebenarnya tidak akan pernah terjadi itu lebih buruk daripada tidak ada dokumentasi sama sekali — itu memberi tahu konsumen API untuk menangani kasus yang tidak ada, sekaligus menyembunyikan fakta bahwa endpoint ini saat ini masih terbuka untuk siapa saja. Ini perlu ditambahkan kembali, dengan benar, begitu Module 17 selesai dikerjakan.

## Sebelum / Sesudah

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

# 4. Pembersihan dokumentasi XML — `docs`

## Apa yang berubah

Menambahkan komentar XML doc ringkas berbahasa Inggris di:

* Domain layer: `BaseEntity`, `Account`, `Category`, `Transaction`, `AccountType`, `TransactionType`.
* Semua DTO request dan response Account/Category/Transaction/Dashboard (14 file).
* `IDashboardService` / `DashboardService`.
* Sisa file di Client project: `ApiRoutes`, `DependencyInjection`, `DashboardClient`.
* (Controller, interface/implementasi service, dan class Client lainnya sudah didokumentasikan dalam pass yang sama dengan poin 1–3 di atas, karena file-file itu memang sudah disentuh untuk perbaikan tersebut.)

Tidak ada perubahan perilaku di commit ini — murni dokumentasi.

## Alasan

`06.WebApi.csproj` milik project ini sudah mengaktifkan `GenerateDocumentationFile` (dengan warning `1591` di-suppress), artinya niat untuk mendokumentasikan API publik memang sudah ada, cuma belum tuntas. Karena Swagger/OpenAPI generation membaca komentar XML ini langsung ke spesifikasi yang dihasilkan, DTO dan tipe domain yang belum terdokumentasi akan muncul sebagai field tanpa label di dokumentasi API yang dilihat konsumen — ini tujuan "enterprise API" yang sama dengan yang sudah ditandai selesai di `04-project-status.md`, sekarang diperluas ke layer-layer yang sebelumnya terlewat.

## Sebelum / Sesudah

Satu contoh representatif dari ~25 file yang tersentuh di commit ini — `src/01.Base/Entities/BaseEntity.cs`, base class yang diturunkan oleh semua entity domain:

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

~24 file lainnya (enum domain, DTO, `IDashboardService`/`DashboardService`, `ApiRoutes`, `DependencyInjection`, `DashboardClient`) mengikuti pola yang sama: satu `<summary>` di level class, ditambah satu `<summary>` per member publik. Diff lengkapnya ada di commit `32bd2f7`.

---

# Ringkasan commit

| Commit | Tipe | Cakupan |
|---|---|---|
| `870881e` | `feat(pagination)` | Validasi parameter halaman, urutan deterministik, pagination `GetDeleted`, perbaikan client/UI |
| `ea7967b` | `feat(middleware)` | Correlation id, penanganan konflik DB, penanganan cancellation, error model-state yang konsisten |
| `5adc049` | `fix(webapi)` | Menghapus `ProducesResponseType` yang tidak mungkin terjadi |
| `32bd2f7` | `docs` | Dokumentasi XML untuk domain, DTO, dan sisa file client/service |
