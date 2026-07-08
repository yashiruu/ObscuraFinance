<#
.SYNOPSIS
    Display a project directory tree.

.DESCRIPTION
    Displays the directory structure of a project and optionally includes files.
    Common build folders (bin, obj, etc.) can be excluded.

.PARAMETER Path
    Root directory to scan.

.PARAMETER Exclude
    Directory names to exclude.

.PARAMETER IncludeFiles
    Include files in the output.

.PARAMETER Depth
    Maximum directory depth.
    Use -1 for unlimited depth.

.PARAMETER Clipboard
    Copy the generated tree to the Windows clipboard.

.PARAMETER Output
    Save the generated tree to a file.

.EXAMPLE
    .\Show-Tree.ps1

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles

.EXAMPLE
    .\Show-Tree.ps1 -Depth 2

.EXAMPLE
    .\Show-Tree.ps1 -Clipboard

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles -Output tree.txt
#>

[CmdletBinding()]
param(
    [string]$Path = ".",

    [string[]]$Exclude = @(
        "bin",
        "obj",
        ".git",
        ".vs",
        "node_modules"
    ),

    [switch]$IncludeFiles,

    [int]$Depth = -1,

    [switch]$Clipboard,

    [string]$Output
)

$Lines = [System.Collections.Generic.List[string]]::new()

function Add-Line {
    param([string]$Text)

    $script:Lines.Add($Text)
}

function Show-Node {

    param(
        [System.IO.DirectoryInfo]$Directory,

        [string]$Indent = "",

        [int]$Level = 1
    )

    Add-Line "$Indent+-- $($Directory.Name)"

    if ($Depth -ge 0 -and $Level -ge $Depth) {
        return
    }

    Get-ChildItem $Directory.FullName -Directory |
        Where-Object { $_.Name -notin $Exclude } |
        Sort-Object Name |
        ForEach-Object {

            Show-Node $_ "$Indent|   " ($Level + 1)
        }

    if ($IncludeFiles) {

        Get-ChildItem $Directory.FullName -File |
            Sort-Object Name |
            ForEach-Object {

                Add-Line "$Indent|   +-- $($_.Name)"
            }
    }
}

if (-not (Test-Path $Path)) {
    throw "Directory '$Path' does not exist."
}

$Root = Get-Item $Path

Add-Line $Root.Name

Get-ChildItem $Root.FullName -Directory |
    Where-Object { $_.Name -notin $Exclude } |
    Sort-Object Name |
    ForEach-Object {

        Show-Node $_ "" 1
    }

if ($IncludeFiles) {

    Get-ChildItem $Root.FullName -File |
        Sort-Object Name |
        ForEach-Object {

            Add-Line "+-- $($_.Name)"
        }
}

if ($Output) {

    $Lines | Set-Content -Path $Output -Encoding UTF8

    Write-Host "Tree exported to '$Output'."

    return
}

if ($Clipboard) {

    $Lines -join "`r`n" | Set-Clipboard

    Write-Host "Tree copied to clipboard."

    return
}

$Lines