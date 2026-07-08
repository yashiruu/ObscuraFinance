# PowerShell Utilities

This directory contains PowerShell utilities used during the development of **Obscura Finance**.

---

# Show-Tree

`Show-Tree.ps1` generates a readable directory tree for the project.

Unlike the built-in Windows `tree` command, it can exclude folders such as `bin` and `obj`, include files when needed, limit recursion depth, export the result to a file, or copy it directly to the clipboard.

---

## Features

* Display project directory structure
* Exclude unwanted directories
* Include files in the output
* Limit directory depth
* Export output to a text file
* Copy output directly to the clipboard
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

### Limit recursion depth

```powershell
.\tools\powershell\Show-Tree.ps1 -Depth 2
```

Use `-1` (default) for unlimited depth.

---

### Export to a file

```powershell
.\tools\powershell\Show-Tree.ps1 -Output tree.txt
```

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

| Parameter      | Description                                |
| -------------- | ------------------------------------------ |
| `Path`         | Root directory to scan                     |
| `Exclude`      | Directory names to ignore                  |
| `IncludeFiles` | Include files in the output                |
| `Depth`        | Maximum recursion depth (`-1` = unlimited) |
| `Clipboard`    | Copy output to the Windows clipboard       |
| `Output`       | Save output to a text file                 |

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

Generate a tree and save it as documentation:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Output docs\project-structure.txt
```

Generate a tree and copy it directly to the clipboard:

```powershell
.\tools\powershell\Show-Tree.ps1 -IncludeFiles -Clipboard
```

---

## Version

Current version: **v1.1.0**
