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

# ----------------------------------------------------------------------
# Tree Model
# ----------------------------------------------------------------------

$Tree = [System.Collections.Generic.List[PSObject]]::new()

function Add-TreeNode {

    param(
        [string]$Name,

        [ValidateSet("Root", "Directory", "File")]
        [string]$Type,

        [string]$Indent,

        [int]$Level
    )

    $script:Tree.Add(
        [PSCustomObject]@{
            Name   = $Name
            Type   = $Type
            Indent = $Indent
            Level  = $Level
        }
    )
}

# ----------------------------------------------------------------------
# Scanner
# ----------------------------------------------------------------------

function Scan-Tree {

    param(
        [System.IO.DirectoryInfo]$Directory,

        [string]$Indent = "",

        [int]$Level = 1
    )

    Add-TreeNode `
        -Name $Directory.Name `
        -Type Directory `
        -Indent $Indent `
        -Level $Level

    if ($Depth -ge 0 -and $Level -ge $Depth) {
        return
    }

    Get-ChildItem $Directory.FullName -Directory |
        Where-Object { $_.Name -notin $Exclude } |
        Sort-Object Name |
        ForEach-Object {

            Scan-Tree `
                -Directory $_ `
                -Indent "$Indent|   " `
                -Level ($Level + 1)
        }

    if ($IncludeFiles) {

        Get-ChildItem $Directory.FullName -File |
            Sort-Object Name |
            ForEach-Object {

                Add-TreeNode `
                    -Name $_.Name `
                    -Type File `
                    -Indent "$Indent|   " `
                    -Level ($Level + 1)
            }
    }
}

# ----------------------------------------------------------------------
# Renderer
# ----------------------------------------------------------------------

function Render-Text {

    param(
        [System.Collections.Generic.List[PSObject]]$Tree
    )

    $Lines = [System.Collections.Generic.List[string]]::new()

    foreach ($Node in $Tree) {

        switch ($Node.Type) {

            "Root" {
                $Lines.Add($Node.Name)
            }

            "Directory" {
                $Lines.Add("$($Node.Indent)+-- $($Node.Name)")
            }

            "File" {
                $Lines.Add("$($Node.Indent)+-- $($Node.Name)")
            }
        }
    }

    return $Lines
}

# ----------------------------------------------------------------------
# Validation
# ----------------------------------------------------------------------

if (-not (Test-Path $Path)) {
    throw "Directory '$Path' does not exist."
}

$Root = Get-Item $Path

# ----------------------------------------------------------------------
# Scan
# ----------------------------------------------------------------------

Add-TreeNode `
    -Name $Root.Name `
    -Type Root `
    -Indent "" `
    -Level 0

Get-ChildItem $Root.FullName -Directory |
    Where-Object { $_.Name -notin $Exclude } |
    Sort-Object Name |
    ForEach-Object {

        Scan-Tree $_ "" 1
    }

if ($IncludeFiles) {

    Get-ChildItem $Root.FullName -File |
        Sort-Object Name |
        ForEach-Object {

            Add-TreeNode `
                -Name $_.Name `
                -Type File `
                -Indent "" `
                -Level 1
        }
}

# ----------------------------------------------------------------------
# Render
# ----------------------------------------------------------------------

$Lines = Render-Text $Tree

# ----------------------------------------------------------------------
# Output
# ----------------------------------------------------------------------

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