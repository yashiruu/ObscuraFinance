# PowerShell Utilities

This directory contains PowerShell utilities used during the development of **Obscura Finance**.

---

# Show-Tree

`Show-Tree.ps1` generates a readable directory tree for the project.

Unlike the built-in Windows `tree` command, it can exclude folders such as `bin` and `obj`, filter files by extension, include hidden items, limit recursion depth, export the result to a file or Markdown, or copy it directly to the clipboard.

---

## Features

* Display project directory structure with proper tree branch characters (`├──` / `└──`)
* Exclude unwanted directories
* Include files in the output, optionally filtered by extension
* Include hidden and system files/directories
* Limit directory depth
* Show a summary of total folders and files
* Export output to a text file or a ready-to-paste Markdown code block
* Copy output directly to the clipboard
* Accept multiple root paths via the pipeline
* No external dependencies

---

## Default Excluded Directories

The following directories are excluded by default:

* `bin`
* `obj`
* `.git`
* `.vs`
* `node_modules`

---

## Usage

### Display directories only

```powershell
.\tools\powershell\Show-Tree.ps1
```

---

### Include files

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles
```

---

### Scan another directory

```powershell
.\tools\powershell\Show-Tree.ps1 -Path .\src
```

---

### Scan multiple directories at once

```powershell
"src", "tests" | .\tools\powershell\Show-Tree.ps1 -IncludeFiles
```

---

### Limit recursion depth

```powershell
.\tools\powershell\Show-Tree.ps1 -Depth 2
```

Use `-1` (default) for unlimited depth. Values below `-1` are rejected.

---

### Filter files by extension

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -IncludeExtensions cs,csproj,json
```

Only applies when `-IncludeFiles` is used. If omitted, all files are shown.

---

### Include hidden and system items

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Force
```

---

### Show a summary count

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Stats
```

Appends a line such as `42 directories, 187 files` at the end of the output.

---

### Export to a file

```powershell
.\tools\powershell\Show-Tree.ps1 -Output tree.txt
```

---

### Export as a Markdown code block

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -AsMarkdown -Output docs\project-structure.md
```

Wraps the output in a fenced ` ```text ` code block, ready to paste directly into a README or any Markdown document.

---

### Copy to clipboard

```powershell
.\tools\powershell\Show-Tree.ps1 -Clipboard
```

---

### Include files and export

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Output docs\project-structure.txt
```

---

### Include files and copy to clipboard

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Clipboard
```

---

### Exclude additional directories

```powershell
.\tools\powershell\Show-Tree.ps1 -Exclude bin,obj,.git,.vs,node_modules,artifacts
```

---

## Parameters

| Parameter           | Description                                                          |
| -------------------- | ---------------------------------------------------------------------- |
| `Path`               | Root directory (or directories) to scan. Accepts pipeline input.     |
| `Exclude`             | Directory names to ignore                                            |
| `IncludeFiles`        | Include files in the output                                          |
| `IncludeExtensions`   | Restrict included files to these extensions (used with `IncludeFiles`) |
| `Force`               | Include hidden and system files/directories                          |
| `Depth`               | Maximum recursion depth (`-1` = unlimited)                           |
| `Stats`               | Append a `X directories, Y files` summary line                       |
| `Clipboard`           | Copy output to the Windows clipboard                                 |
| `Output`              | Save output to a file                                                 |
| `AsMarkdown`          | Wrap output in a fenced Markdown code block                          |

---

## Examples

Generate a complete project tree including files:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles
```

Generate only the top three levels:

```powershell
.\tools\powershell\Show-Tree.ps1 -Depth 3
```

Generate a tree of only C# source files, with a summary:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -IncludeExtensions cs,csproj -Stats
```

Generate a tree and save it as Markdown documentation:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -AsMarkdown -Output docs\project-structure.md
```

Generate a tree and copy it directly to the clipboard:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Clipboard
```

Scan several project folders in one pass:

```powershell
"1.Core", "2.Infrastructure", "3.Presentation" | .\tools\powershell\Show-Tree.ps1 -IncludeFiles -Depth 2
```

---

## Sample Output

```text
MyApp
├── 1.Core
│   ├── MyApp.Domain
│   └── MyApp.Application
├── 2.Infrastructure
└── 3.Presentation
    ├── MyApp.Api
    └── MyApp.UI

5 directories, 0 files
```

---

## Version

Current version: **v2.0.0**

### Changelog

**v2.0.0**
* Proper tree branch characters (`├──` / `└──`) instead of a flat `+--` prefix
* Added `-Force` to include hidden/system items
* Added `-IncludeExtensions` to filter files by type
* Added `-Stats` for a folder/file count summary
* Added `-AsMarkdown` to export a ready-to-paste Markdown code block
* `Path` now accepts pipeline input for scanning multiple roots in one call
* Added `-ErrorAction SilentlyContinue` to gracefully skip inaccessible folders
* Renamed internal functions to use approved PowerShell verbs
* Added validation for the `-Depth` parameter
* Output now defaults to an auto-created `export` folder next to the script when only a filename is provided
* Renamed internal functions to use approved PowerShell verbs

**v1.1.0**
* Initial public version with exclude list, depth limiting, clipboard and file export
