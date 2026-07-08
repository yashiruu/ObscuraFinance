<#
.SYNOPSIS
    Display project directory structure.

.DESCRIPTION
    Prints a tree view of a directory and optionally includes files.
    Common build folders such as bin and obj can be excluded.

.PARAMETER Path
    Root directory.

.PARAMETER Exclude
    Folder names to ignore.

.PARAMETER IncludeFiles
    Include files in the output.

.PARAMETER Output
    Save the result to a file.

.EXAMPLE
    .\Show-Tree.ps1

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles

.EXAMPLE
    .\Show-Tree.ps1 -Output tree.txt

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles -Output docs\structure.txt
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
        [string]$Indent = ""
    )

    Add-Line "$Indent+-- $($Directory.Name)"

    Get-ChildItem $Directory.FullName -Directory |
        Where-Object { $_.Name -notin $Exclude } |
        Sort-Object Name |
        ForEach-Object {

            Show-Node $_ "$Indent|   "
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

        Show-Node $_
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

$Lines