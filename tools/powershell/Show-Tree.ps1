<#
.SYNOPSIS
    Display a project directory tree.

.DESCRIPTION
    Displays the directory structure of a project and optionally includes files.
    Common build folders (bin, obj, etc.) can be excluded.

.PARAMETER Path
    Root directory to scan. Accepts pipeline input, so multiple paths can be
    scanned in one call, e.g. "src","tests" | .\Show-Tree.ps1

.PARAMETER Exclude
    Directory names to exclude.

.PARAMETER IncludeFiles
    Include files in the output.

.PARAMETER IncludeExtensions
    When used together with -IncludeFiles, only files matching these
    extensions are shown (e.g. -IncludeExtensions cs,csproj,json).
    If omitted, all files are included.

.PARAMETER Force
    Include hidden and system files/directories in the output.

.PARAMETER Depth
    Maximum directory depth.
    Use -1 for unlimited depth.

.PARAMETER Clipboard
    Copy the generated tree to the Windows clipboard.

.PARAMETER Output
    Save the generated tree to a file.
    If only a filename is given (no folder), the file is saved into an
    "export" folder next to the script itself; that folder is created
    automatically if it doesn't exist yet. If a folder path is included,
    it is used as-is (and also created automatically if missing).

.PARAMETER AsMarkdown
    Wrap the generated tree in a fenced ```text code block, ready to paste
    into a README or other Markdown document.

.PARAMETER Stats
    Append a summary line with the total folder and file count.

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles -IncludeExtensions cs,csproj -Stats

.EXAMPLE
    .\Show-Tree.ps1 -IncludeFiles -AsMarkdown -Output docs\structure.md
#>

[CmdletBinding()]
param(
    [Parameter(ValueFromPipeline = $true)]
    [string[]]$Path = ".",

    [string[]]$Exclude = @(
        "bin",
        "obj",
        ".git",
        ".vs",
        "node_modules"
    ),

    [switch]$IncludeFiles,

    [string[]]$IncludeExtensions,

    [switch]$Force,

    [ValidateScript({
        if ($_ -lt -1) { throw "Depth must be -1 (unlimited) or a non-negative integer." }
        $true
    })]
    [int]$Depth = -1,

    [switch]$Clipboard,

    [string]$Output,

    [switch]$AsMarkdown,

    [switch]$Stats
)

begin {

    # ----------------------------------------------------------------------
    # Tree Model
    # ----------------------------------------------------------------------

    function New-TreeNode {

        param(
            [string]$Name,

            [ValidateSet("Root", "Directory", "File")]
            [string]$Type,

            [string]$Prefix,

            [int]$Level
        )

        [PSCustomObject]@{
            Name   = $Name
            Type   = $Type
            Prefix = $Prefix
            Level  = $Level
        }
    }

    # ----------------------------------------------------------------------
    # Scanner
    # ----------------------------------------------------------------------

    # $DirectoryCount / $FileCount are reset per invocation (per Path) in process{}
    function Get-DirectoryTree {

        param(
            [System.IO.DirectoryInfo]$Directory,

            [string]$IndentPrefix = "",

            [int]$Level = 1
        )

        $Nodes = [System.Collections.Generic.List[PSObject]]::new()

        $ForceParam = @{}
        if ($Force) { $ForceParam["Force"] = $true }

        # Box-drawing characters (defined via [char] cast for Windows PowerShell
        # 5.1 compatibility, since `u{XXXX} escapes require PowerShell 6+)
        $CharVertical  = [char]0x2502   # │
        $CharTee       = [char]0x251C   # ├
        $CharCorner    = [char]0x2514   # └
        $CharHorizontal = [char]0x2500  # ─

        $ChildDirectories = @(
            Get-ChildItem $Directory.FullName -Directory -ErrorAction SilentlyContinue @ForceParam |
                Where-Object { $_.Name -notin $Exclude } |
                Sort-Object Name
        )

        $ChildFiles = @()

        if ($IncludeFiles) {

            $ChildFiles = @(
                Get-ChildItem $Directory.FullName -File -ErrorAction SilentlyContinue @ForceParam |
                    Where-Object {
                        -not $IncludeExtensions -or
                        ($_.Extension.TrimStart(".") -in $IncludeExtensions)
                    } |
                    Sort-Object Name
            )
        }

        $AtDepthLimit = ($Depth -ge 0 -and $Level -gt $Depth)

        if ($AtDepthLimit) {
            return $Nodes
        }

        $TotalChildren = $ChildDirectories.Count + $ChildFiles.Count
        $Index = 0

        foreach ($ChildDir in $ChildDirectories) {

            $Index++
            $IsLast = ($Index -eq $TotalChildren)
            $Branch = if ($IsLast) { "$CharCorner$CharHorizontal$CharHorizontal " } else { "$CharTee$CharHorizontal$CharHorizontal " }
            $ChildPrefix = if ($IsLast) { "$IndentPrefix    " } else { "$IndentPrefix$CharVertical   " }

            $Nodes.Add((New-TreeNode -Name $ChildDir.Name -Type Directory -Prefix "$IndentPrefix$Branch" -Level $Level))
            $script:DirectoryCount++

            foreach ($ChildNode in (Get-DirectoryTree -Directory $ChildDir -IndentPrefix $ChildPrefix -Level ($Level + 1))) {
                $Nodes.Add($ChildNode)
            }
        }

        foreach ($ChildFile in $ChildFiles) {

            $Index++
            $IsLast = ($Index -eq $TotalChildren)
            $Branch = if ($IsLast) { "$CharCorner$CharHorizontal$CharHorizontal " } else { "$CharTee$CharHorizontal$CharHorizontal " }

            $Nodes.Add((New-TreeNode -Name $ChildFile.Name -Type File -Prefix "$IndentPrefix$Branch" -Level $Level))
            $script:FileCount++
        }

        return $Nodes
    }

    # ----------------------------------------------------------------------
    # Renderer
    # ----------------------------------------------------------------------

    function ConvertTo-TreeText {

        param(
            [System.Collections.Generic.List[PSObject]]$Tree
        )

        $Lines = [System.Collections.Generic.List[string]]::new()

        foreach ($Node in $Tree) {

            if ($Node.Type -eq "Root") {
                $Lines.Add($Node.Name)
            }
            else {
                $Lines.Add("$($Node.Prefix)$($Node.Name)")
            }
        }

        return $Lines
    }
}

process {

    foreach ($SinglePath in $Path) {

        # ----------------------------------------------------------------------
        # Validation
        # ----------------------------------------------------------------------

        if (-not (Test-Path $SinglePath)) {
            Write-Error "Directory '$SinglePath' does not exist."
            continue
        }

        $Root = Get-Item $SinglePath

        if (-not $Root.PSIsContainer) {
            Write-Error "'$SinglePath' is not a directory."
            continue
        }

        # ----------------------------------------------------------------------
        # Scan
        # ----------------------------------------------------------------------

        $script:DirectoryCount = 0
        $script:FileCount = 0

        $Tree = [System.Collections.Generic.List[PSObject]]::new()
        $Tree.Add((New-TreeNode -Name $Root.Name -Type Root -Prefix "" -Level 0))

        foreach ($ChildNode in (Get-DirectoryTree -Directory $Root -IndentPrefix "" -Level 1)) {
            $Tree.Add($ChildNode)
        }

        # ----------------------------------------------------------------------
        # Render
        # ----------------------------------------------------------------------

        $Lines = [System.Collections.Generic.List[string]]::new([string[]](ConvertTo-TreeText $Tree))

        if ($Stats) {
            $Lines.Add("")
            $Lines.Add("$($script:DirectoryCount) directories, $($script:FileCount) files")
        }

        if ($AsMarkdown) {
            $Lines.Insert(0, '```text')
            $Lines.Add('```')
        }

        # ----------------------------------------------------------------------
        # Output
        # ----------------------------------------------------------------------

        if ($Output) {

            $HasDirectoryComponent = [bool]([System.IO.Path]::GetDirectoryName($Output))

            if ($HasDirectoryComponent) {
                # User supplied a path (relative or absolute) — respect it as-is.
                $ResolvedOutput = $Output
            }
            else {
                # User supplied only a filename — default to an "export" folder
                # next to the script itself.
                $ExportRoot = Join-Path $PSScriptRoot "export"
                $ResolvedOutput = Join-Path $ExportRoot $Output
            }

            $OutputDirectory = [System.IO.Path]::GetDirectoryName($ResolvedOutput)

            if ($OutputDirectory -and -not (Test-Path $OutputDirectory)) {
                New-Item -ItemType Directory -Path $OutputDirectory -Force | Out-Null
            }

            $Lines | Set-Content -Path $ResolvedOutput -Encoding UTF8
            Write-Host "Tree exported to '$ResolvedOutput'."
        }
        elseif ($Clipboard) {

            $Lines -join "`r`n" | Set-Clipboard
            Write-Host "Tree copied to clipboard."
        }
        else {
            $Lines
        }
    }
}