# Show-Tree.bat

`Show-Tree.bat` adalah port dari `Show-Tree.ps1` ke Batch/CMD, untuk yang tidak mau atau tidak bisa menjalankan PowerShell (misalnya karena Execution Policy yang dikunci).

Fungsinya sama: menampilkan struktur folder proyek, dengan opsi exclude, filter file, filter ekstensi, batas depth, export ke file/Markdown, dan copy ke clipboard.

> **Catatan:** ini bukan sekadar alias — sintaks parameternya berbeda dari versi PowerShell. Lihat tabel perbandingan di bawah.

---

## Perbedaan Sintaks vs Versi PowerShell

PowerShell pakai `-Nama Value` (spasi sebagai pemisah), Batch pakai `/Nama:Value` (titik dua sebagai pemisah), gaya klasik command-line Windows.

| Fitur                  | PowerShell (`Show-Tree.ps1`)            | Batch (`Show-Tree.bat`)             |
| ----------------------- | ---------------------------------------- | ------------------------------------- |
| Root directory          | `-Path .\src`                            | `/Path:src`                           |
| Exclude folder          | `-Exclude bin,obj`                       | `/Exclude:bin,obj`                    |
| Include files           | `-IncludeFiles`                          | `/IncludeFiles`                       |
| Filter ekstensi         | `-IncludeExtensions cs,csproj`           | `/Extensions:cs,csproj`               |
| Hidden/system items     | `-Force`                                 | **Tidak didukung** (lihat Keterbatasan) |
| Batas depth             | `-Depth 2`                               | `/Depth:2`                            |
| Export ke file          | `-Output tree.txt`                       | `/Output:tree.txt`                    |
| Export sebagai Markdown | `-AsMarkdown`                            | `/AsMarkdown`                         |
| Copy ke clipboard       | `-Clipboard`                             | `/Clipboard`                          |
| Ringkasan jumlah        | `-Stats`                                 | `/Stats`                              |
| Multi-path via pipeline | `"src","tests" \| .\Show-Tree.ps1`       | **Tidak didukung** — jalankan satu per satu |
| Bantuan                 | `Get-Help .\Show-Tree.ps1`               | `Show-Tree.bat /?`                    |

---

## Penggunaan

### Tampilkan folder saja

```bat
./Show-Tree.bat
```

---

### Sertakan file

```bat
./Show-Tree.bat /IncludeFiles
```

---

### Scan folder lain

```bat
./Show-Tree.bat /Path:src
```

---

### Batasi kedalaman rekursi

```bat
./Show-Tree.bat /Depth:2
```

Gunakan `-1` (default) untuk kedalaman tanpa batas.

---

### Filter file berdasarkan ekstensi

```bat
./Show-Tree.bat /IncludeFiles /Extensions:cs,csproj,json
```

Hanya berlaku bila `/IncludeFiles` digunakan. Jika tidak diisi, semua file ditampilkan.

---

### Tampilkan ringkasan jumlah

```bat
./Show-Tree.bat /IncludeFiles /Stats
```

Menambahkan baris seperti `42 directories, 187 files` di akhir output.

---

### Export ke file

```bat
./Show-Tree.bat /Output:tree.txt
```

Kalau hanya nama file yang diberikan (tanpa folder), file akan disimpan ke folder `export` di sebelah script itu sendiri — folder ini dibuat otomatis kalau belum ada:

```
tools\powershell\
./├── Show-Tree.bat
└── export\
    └── tree.txt
```

Kalau kamu sertakan path folder, path itu dipakai apa adanya (foldernya juga dibuat otomatis kalau belum ada):

```bat
./Show-Tree.bat /Output:docs\project-structure.txt
```

---

### Export sebagai Markdown

```bat
./Show-Tree.bat /IncludeFiles /AsMarkdown /Output:\project-structure.md
```

Membungkus hasil dalam code fence ` ```text `, siap ditempel ke README.

---

### Copy ke clipboard

```bat
./Show-Tree.bat /Clipboard
```

---

### Exclude folder tambahan

```bat
./Show-Tree.bat /Exclude:bin,obj,.git,.vs,node_modules,artifacts
```

---

## Parameter

| Parameter        | Deskripsi                                                             |
| ----------------- | ------------------------------------------------------------------------ |
| `/Path:dir`        | Folder root yang di-scan (default: folder saat ini)                    |
| `/Exclude:list`    | Nama folder yang di-exclude, pisahkan dengan koma                      |
| `/IncludeFiles`    | Sertakan file dalam output                                             |
| `/Extensions:list` | Hanya sertakan file dengan ekstensi ini (dipakai bersama `/IncludeFiles`) |
| `/Depth:n`         | Batas kedalaman rekursi (`-1` = tanpa batas)                            |
| `/Output:file`     | Simpan output ke file (default folder `export` di sebelah script bila hanya nama file yang diberikan) |
| `/Clipboard`       | Copy output ke clipboard                                               |
| `/AsMarkdown`      | Bungkus output dalam code fence Markdown                               |
| `/Stats`           | Tambahkan baris ringkasan `X directories, Y files`                     |
| `/?` atau `/help`  | Tampilkan bantuan penggunaan                                           |

---

## Contoh

Tampilkan tree lengkap termasuk file:

```bat
./Show-Tree.bat /IncludeFiles
```

Tampilkan hanya 3 level teratas:

```bat
./Show-Tree.bat /Depth:3
```

Scan hanya file C#, dengan ringkasan:

```bat
./Show-Tree.bat /IncludeFiles /Extensions:cs,csproj /Stats
```

Simpan sebagai dokumentasi Markdown:

```bat
./Show-Tree.bat /IncludeFiles /AsMarkdown /Output:\project-structure.md
```

---

## Contoh Output

```text
ObscuraFinance
├── 1.Core
│   ├── ObscuraFinance.Domain
│   └── ObscuraFinance.Application
├── 2.Infrastructure
└── 3.Presentation
    ├── ObscuraFinance.Api
    └── ObscuraFinance.UI

5 directories, 0 files
```

Simbol tree sekarang identik dengan versi PowerShell (`├──`, `└──`, `│`). Script otomatis menjalankan `chcp 65001` (UTF-8) di awal supaya karakter ini tampil dengan benar. Kalau karakternya tampil sebagai kotak/tanda tanya di Command Prompt lama, coba jalankan lewat **Windows Terminal** atau pastikan font terminal mendukung karakter box-drawing (misalnya Cascadia Mono, Consolas).

---

## Keterbatasan dibanding versi PowerShell

Batch/CMD jauh lebih terbatas dibanding PowerShell untuk tugas seperti ini, jadi beberapa fitur di `Show-Tree.ps1` sengaja tidak diporting:

* **Tidak ada `-Force`** untuk menampilkan file/folder hidden atau system — `dir` di Batch selalu skip item hidden secara default.
* **Tidak mendukung multi-path via pipeline** — jalankan script terpisah untuk tiap folder.
* **Karakter khusus pada nama file/folder** (seperti `%`, `^`, `&`, `!`) berpotensi menyebabkan hasil yang tidak terduga, karena keterbatasan parsing Batch. Untuk proyek dengan penamaan yang lebih kompleks, versi PowerShell lebih andal.
* **Kedalaman folder sangat dalam** (>~30 level nested) berpotensi terbentur batas nested `setlocal` di CMD.

Untuk kebutuhan yang lebih kompleks atau proyek dengan struktur besar, tetap disarankan pakai `Show-Tree.ps1`.

---

## Version

Current version: **v1.1.0** (port dari `Show-Tree.ps1` v2.1.0)

### Changelog

**v1.1.0**
* Simbol tree diganti ke Unicode (`├──`, `└──`, `│`) agar identik dengan versi PowerShell; script otomatis set code page ke UTF-8
* Perbaikan resolusi folder `export` default agar selalu konsisten mengikuti lokasi script, bukan direktori kerja saat ini

**v1.0.0**
* Port awal dari `Show-Tree.ps1` v2.1.0, menggunakan karakter ASCII klasik (`+---`, `\---`)