# Panduan Penulisan ADR

## Apa Itu ADR?

ADR adalah singkatan dari:

```text
Architecture Decision Record
```

ADR adalah dokumen ringan yang digunakan untuk mencatat keputusan teknis penting yang dibuat selama pengembangan proyek.

Tujuan ADR bukan untuk mendokumentasikan kode.

Tujuan ADR adalah mendokumentasikan:

* mengapa suatu keputusan dibuat
* opsi apa saja yang dipertimbangkan
* mengapa opsi tertentu dipilih
* konsekuensi dari keputusan tersebut

ADR memungkinkan developer di masa depan (termasuk diri kita sendiri) memahami alasan di balik keputusan arsitektur yang pernah diambil.

Tanpa ADR:

```text
Keputusan
    ↓
Waktu Berlalu
    ↓
Alasan Terlupakan
```

Dengan ADR:

```text
Keputusan
    ↓
ADR
    ↓
Alasan Tetap Tersimpan
```

---

# Mengapa ADR Dibutuhkan?

Kode hanya menunjukkan:

```text
Apa yang Dibangun
```

ADR menjelaskan:

```text
Mengapa Dibangun Dengan Cara Tersebut
```

Contoh:

Kode dapat menunjukkan:

```text
Proyek menggunakan SQL Server
```

ADR menjelaskan:

```text
Mengapa SQL Server dipilih dibanding PostgreSQL
```

Semakin besar proyek, semakin penting informasi tersebut.

---

# Template ADR

Setiap ADR sebaiknya mengikuti struktur berikut:

```md
# ADR-XXX

## Judul

Deskripsi singkat keputusan.

---

## Status

Accepted

atau

Proposed

atau

Superseded

---

## Context

Masalah apa yang sedang diselesaikan?

Batasan apa yang ada?

Pilihan apa saja yang dipertimbangkan?

---

## Decision

Keputusan apa yang diambil?

---

## Rationale

Mengapa keputusan tersebut dipilih?

---

## Consequences

Dampak positif.

Dampak negatif.

Trade-off.

---

## Future Revisit

Dalam kondisi apa keputusan ini perlu dievaluasi kembali?
```

---

# Status ADR

## Proposed

Keputusan masih dalam tahap diskusi.

Contoh:

```text
Apakah kita akan menggunakan MediatR?
```

---

## Accepted

Keputusan sudah disetujui dan diterapkan.

Contoh:

```text
Menggunakan SQL Server
```

---

## Superseded

Keputusan sebelumnya pernah diterima namun kemudian diganti.

Contoh:

Awalnya:

```text
Menggunakan Blazor
```

Kemudian:

```text
Menggunakan React
```

Maka ADR lama berubah menjadi:

```text
Superseded
```

dan dibuat ADR baru.

---

# Aturan Menulis ADR

## Aturan 1

Dokumentasikan keputusan, bukan implementasi.

Baik:

```text
Menggunakan SQL Server dibanding PostgreSQL
```

Kurang tepat:

```text
Menambahkan konfigurasi DbContext
```

Contoh kedua adalah perubahan implementasi, bukan keputusan arsitektur.

---

## Aturan 2

Fokus pada keputusan yang penting.

Contoh yang layak menjadi ADR:

* keputusan arsitektur
* pemilihan teknologi
* perubahan strategi proyek
* perubahan workflow utama

Hindari membuat ADR untuk keputusan sepele.

Contoh:

```text
Mengganti nama properti
```

tidak memerlukan ADR.

---

## Aturan 3

Selalu tuliskan trade-off.

Setiap keputusan memiliki:

* keuntungan
* kerugian

ADR yang baik menjelaskan keduanya.

Contoh:

```text
Positif:
- Arsitektur lebih sederhana

Negatif:
- Fleksibilitas berkurang
```

---

## Aturan 4

Jelaskan mengapa alternatif lain ditolak.

Pembaca di masa depan harus memahami:

```text
Mengapa Memilih Opsi A

dan bukan

Opsi B
```

---

## Aturan 5

Jaga ADR tetap ringkas.

Idealnya:

```text
1–2 halaman
```

Bukan:

```text
10 halaman
```

Tujuan ADR adalah kejelasan, bukan kelengkapan absolut.

---

# Kapan ADR Harus Dibuat?

Buat ADR jika jawaban pertanyaan berikut adalah:

```text
Apakah saya akan lupa alasan keputusan ini dalam beberapa bulan?
```

Jika jawabannya:

```text
Ya
```

maka buat ADR.

---

# Pemicu ADR Pada Proyek Ini

Situasi berikut biasanya layak dibuat ADR.

## Pemilihan Teknologi

Contoh:

```text
SQL Server vs PostgreSQL

Blazor vs React

OpenAI vs Ollama
```

---

## Keputusan Arsitektur

Contoh:

```text
Menggunakan Clean Architecture

Menggunakan Repository Pattern

Mengadopsi CQRS
```

---

## Keputusan Strategi Proyek

Contoh:

```text
Dashboard sebelum Refactor Enterprise

AI setelah CQRS

Agentic AI setelah AI Integration
```

---

## Keputusan Workflow

Contoh:

```text
Feature Driven Learning

Adopsi Conventional Commit

Perubahan Struktur Dokumentasi
```

---

# Kapan Tidak Perlu Membuat ADR?

Jangan membuat ADR untuk:

* bug fix
* refactor kecil
* perubahan UI
* perubahan nama
* detail implementasi
* code cleanup ringan

Hal-hal tersebut lebih cocok dicatat dalam:

```text
Git Commit
Pull Request
Dokumentasi Teknis
```

dan bukan ADR.

---

# Filosofi ADR Pada Obscura Finance Tracker

Proyek ini dibuat untuk belajar pengembangan software enterprise.

Karena itu ADR tidak hanya dianggap sebagai artefak teknis, tetapi juga artefak pembelajaran.

Sebuah ADR dianggap berhasil jika enam bulan kemudian kita dapat membacanya kembali dan langsung memahami:

```text
Keputusan apa yang dibuat?

Mengapa keputusan tersebut dibuat?

Alternatif apa yang tersedia?

Apakah keputusan yang sama masih layak diambil hari ini?
```

Jika keempat pertanyaan tersebut dapat dijawab dengan cepat, maka ADR telah menjalankan fungsinya dengan baik.
