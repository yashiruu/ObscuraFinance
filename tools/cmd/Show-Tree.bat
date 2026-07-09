@echo off
chcp 65001 >nul
setlocal EnableExtensions EnableDelayedExpansion

:: ============================================================================
:: Show-Tree.bat
:: Batch port of Show-Tree.ps1 - displays a project directory tree.
:: Run "Show-Tree.bat /?" for usage.
:: ============================================================================

set "SCRIPTDIR=%~dp0"

set "ROOTPATH=."
set "EXCLUDE=bin,obj,.git,.vs,node_modules"
set "INCLUDEFILES=0"
set "EXTENSIONS="
set "DEPTH=-1"
set "OUTPUT="
set "CLIPBOARD=0"
set "ASMARKDOWN=0"
set "STATS=0"

:: ----------------------------------------------------------------------
:: Parse arguments
:: ----------------------------------------------------------------------
:ParseArgs
if "%~1"=="" goto ArgsDone
set "ARG=%~1"
if /i "%ARG%"=="/?" goto ShowHelp
if /i "%ARG%"=="/help" goto ShowHelp
if /i "%ARG:~0,6%"=="/path:" (set "ROOTPATH=%ARG:~6%" & goto NextArg)
if /i "%ARG:~0,9%"=="/exclude:" (set "EXCLUDE=%ARG:~9%" & goto NextArg)
if /i "%ARG%"=="/includefiles" (set "INCLUDEFILES=1" & goto NextArg)
if /i "%ARG:~0,12%"=="/extensions:" (set "EXTENSIONS=%ARG:~12%" & goto NextArg)
if /i "%ARG:~0,7%"=="/depth:" (set "DEPTH=%ARG:~7%" & goto NextArg)
if /i "%ARG:~0,8%"=="/output:" (set "OUTPUT=%ARG:~8%" & goto NextArg)
if /i "%ARG%"=="/clipboard" (set "CLIPBOARD=1" & goto NextArg)
if /i "%ARG%"=="/asmarkdown" (set "ASMARKDOWN=1" & goto NextArg)
if /i "%ARG%"=="/stats" (set "STATS=1" & goto NextArg)
echo Unknown parameter: %ARG%
goto ShowHelp

:NextArg
shift
goto ParseArgs

:ArgsDone

:: ----------------------------------------------------------------------
:: Validation
:: ----------------------------------------------------------------------
if not exist "%ROOTPATH%\" (
    echo Error: Directory "%ROOTPATH%" does not exist.
    exit /b 1
)

for %%R in ("%ROOTPATH%") do set "ROOTFULL=%%~fR"
for %%N in ("%ROOTFULL%") do set "ROOTNAME=%%~nxN"
if "%ROOTNAME%"=="" set "ROOTNAME=%ROOTFULL%"

:: ----------------------------------------------------------------------
:: Scan (tree lines go to TREEFILE, one "D"/"F" marker per node to STATFILE)
:: ----------------------------------------------------------------------
set "TREEFILE=%TEMP%\showtree_tree_%RANDOM%%RANDOM%.txt"
set "STATFILE=%TEMP%\showtree_stat_%RANDOM%%RANDOM%.txt"
type nul > "%TREEFILE%"
type nul > "%STATFILE%"

echo(%ROOTNAME%>>"%TREEFILE%"

call :ScanDir "%ROOTFULL%" "" 1

set "DIRCOUNT=0"
set "FILECOUNT=0"
for /f %%A in ('findstr /b "D" "%STATFILE%" ^| find /c /v ""') do set "DIRCOUNT=%%A"
for /f %%A in ('findstr /b "F" "%STATFILE%" ^| find /c /v ""') do set "FILECOUNT=%%A"

:: ----------------------------------------------------------------------
:: Stats
:: ----------------------------------------------------------------------
if "%STATS%"=="1" (
    echo(>>"%TREEFILE%"
    echo(%DIRCOUNT% directories, %FILECOUNT% files>>"%TREEFILE%"
)

:: ----------------------------------------------------------------------
:: Markdown wrap
:: ----------------------------------------------------------------------
if "%ASMARKDOWN%"=="1" (
    set "MDFILE=%TEMP%\showtree_md_%RANDOM%%RANDOM%.txt"
    echo(```text>"!MDFILE!"
    type "%TREEFILE%">>"!MDFILE!"
    echo(```>>"!MDFILE!"
    del "%TREEFILE%" >nul 2>&1
    set "TREEFILE=!MDFILE!"
)

:: ----------------------------------------------------------------------
:: Output
:: ----------------------------------------------------------------------
if not "%OUTPUT%"=="" (

    echo(%OUTPUT%| findstr /c:"\" >nul
    if errorlevel 1 (
        set "RESOLVED=!SCRIPTDIR!export\%OUTPUT%"
        set "OUTDIR=!SCRIPTDIR!export"
    ) else (
        set "RESOLVED=%OUTPUT%"
        for %%F in ("%OUTPUT%") do set "OUTDIR=%%~dpF"
    )

    if not exist "!OUTDIR!\" mkdir "!OUTDIR!" >nul 2>&1

    copy /y "!TREEFILE!" "!RESOLVED!" >nul
    echo Tree exported to '!RESOLVED!'.

) else if "%CLIPBOARD%"=="1" (
    type "%TREEFILE%" | clip
    echo Tree copied to clipboard.
) else (
    type "%TREEFILE%"
)

del "%TREEFILE%" >nul 2>&1
del "%STATFILE%" >nul 2>&1

endlocal
exit /b 0

:: ============================================================================
:: :ScanDir  -  recursive directory scanner
::   %1 = full directory path
::   %2 = current line prefix (accumulated "|   " / "    " indent)
::   %3 = level (1 = root's direct children)
:: ============================================================================
:ScanDir
setlocal EnableDelayedExpansion
set "CURDIR=%~1"
set "PREFIX=%~2"
set "LEVEL=%~3"

if not "%DEPTH%"=="-1" if %LEVEL% GTR %DEPTH% (
    endlocal
    exit /b
)

set "DIRTOTAL=0"
for /f "delims=" %%D in ('dir "!CURDIR!" /b /ad /on 2^>nul') do (
    call :CheckExcluded "%%D"
    if "!ISEXCLUDED!"=="0" set /a DIRTOTAL+=1
)

set "FILETOTAL=0"
if "%INCLUDEFILES%"=="1" (
    for /f "delims=" %%F in ('dir "!CURDIR!" /b /a-d /on 2^>nul') do (
        call :CheckExtension "%%F"
        if "!EXTMATCH!"=="1" set /a FILETOTAL+=1
    )
)

set /a TOTALCHILDREN=DIRTOTAL+FILETOTAL
set /a NEXTLEVEL=LEVEL+1
set "IDX=0"

for /f "delims=" %%D in ('dir "!CURDIR!" /b /ad /on 2^>nul') do (
    call :CheckExcluded "%%D"
    if "!ISEXCLUDED!"=="0" (
        set /a IDX+=1
        if !IDX! EQU !TOTALCHILDREN! (
            set "BRANCH=└── "
            set "CHILDPREFIX=!PREFIX!    "
        ) else (
            set "BRANCH=├── "
            set "CHILDPREFIX=!PREFIX!│   "
        )
        echo(!PREFIX!!BRANCH!%%D>>"%TREEFILE%"
        echo(D>>"%STATFILE%"
        call :ScanDir "!CURDIR!\%%D" "!CHILDPREFIX!" !NEXTLEVEL!
    )
)

if "%INCLUDEFILES%"=="1" (
    for /f "delims=" %%F in ('dir "!CURDIR!" /b /a-d /on 2^>nul') do (
        call :CheckExtension "%%F"
        if "!EXTMATCH!"=="1" (
            set /a IDX+=1
            if !IDX! EQU !TOTALCHILDREN! (
                set "BRANCH=└── "
            ) else (
                set "BRANCH=├── "
            )
            echo(!PREFIX!!BRANCH!%%F>>"%TREEFILE%"
            echo(F>>"%STATFILE%"
        )
    )
)

endlocal
exit /b

:: ============================================================================
:: :CheckExcluded  -  sets ISEXCLUDED to 1 if %1 matches the /Exclude list
:: ============================================================================
:CheckExcluded
set "NAME=%~1"
set "ISEXCLUDED=0"
for %%E in (%EXCLUDE%) do (
    if /i "%%E"=="%NAME%" set "ISEXCLUDED=1"
)
exit /b

:: ============================================================================
:: :CheckExtension  -  sets EXTMATCH to 1 if %1's extension matches /Extensions
:: (always 1 if /Extensions was not supplied)
:: ============================================================================
:CheckExtension
set "EXTMATCH=1"
if not "%EXTENSIONS%"=="" (
    set "EXTMATCH=0"
    set "FEXT=%~x1"
    set "FEXT=!FEXT:~1!"
    for %%X in (%EXTENSIONS%) do (
        if /i "%%X"=="!FEXT!" set "EXTMATCH=1"
    )
)
exit /b

:: ============================================================================
:: :ShowHelp
:: ============================================================================
:ShowHelp
echo.
echo Show-Tree.bat - Display a project directory tree
echo.
echo USAGE:
echo   Show-Tree.bat [/Path:dir] [/Exclude:name1,name2,...] [/IncludeFiles]
echo                 [/Extensions:ext1,ext2,...] [/Depth:n] [/Output:file]
echo                 [/Clipboard] [/AsMarkdown] [/Stats]
echo.
echo PARAMETERS:
echo   /Path:dir           Root directory to scan (default: current directory)
echo   /Exclude:list       Comma-separated directory names to exclude
echo                       (default: bin,obj,.git,.vs,node_modules)
echo   /IncludeFiles       Include files in the output
echo   /Extensions:list    Only include these file extensions (used with /IncludeFiles)
echo   /Depth:n            Maximum recursion depth (-1 = unlimited, default)
echo   /Output:file        Save output to a file. If only a filename is given,
echo                       it is saved into an "export" folder next to this
echo                       script (created automatically if missing).
echo   /Clipboard          Copy output to the clipboard
echo   /AsMarkdown         Wrap output in a fenced ```text code block
echo   /Stats              Append a "X directories, Y files" summary line
echo   /?  /help           Show this help
echo.
echo EXAMPLES:
echo   Show-Tree.bat /IncludeFiles /Stats
echo   Show-Tree.bat /Path:src /Depth:2 /Output:tree.txt
echo   Show-Tree.bat /IncludeFiles /Extensions:cs,csproj /AsMarkdown /Output:docs\structure.md
echo.
exit /b 0