# Run in VS Code PowerShell:
# powershell -NoProfile -ExecutionPolicy Bypass -File .\Build-And-Run.ps1
# No .NET SDK, Visual Studio IDE, NuGet, or network downloads required.
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$folder = Join-Path $root 'LibraryDesk'
$candidates = @(
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'),
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe')
)
$compiler = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $compiler) {
    Write-Host 'No built-in C# compiler found. Send the full error to me.' -ForegroundColor Red
    exit 1
}
$sourceFiles = @(Get-ChildItem -Path $folder -Recurse -Filter '*.cs' -File |
    Sort-Object FullName | ForEach-Object { $_.FullName })
if ($sourceFiles.Count -eq 0) { throw 'No source files found.' }
$outDir = Join-Path $folder 'bin\Debug'
New-Item -ItemType Directory -Path $outDir -Force | Out-Null
$exe = Join-Path $outDir 'LibraryDesk.exe'
$compileArgs = @(
    '/nologo', '/target:winexe', '/debug+',
    "/out:$exe",
    '/reference:System.dll',
    '/reference:System.Core.dll',
    '/reference:System.Drawing.dll',
    '/reference:System.Windows.Forms.dll',
    '/reference:System.Xml.dll'
) + $sourceFiles
Write-Host 'Compiling LibraryDesk with built-in Windows C# compiler...' -ForegroundColor Cyan
& $compiler @compileArgs
if ($LASTEXITCODE -ne 0) {
    throw 'Compilation failed. Copy the compiler errors here so we can fix them.'
}
Copy-Item -Path (Join-Path $folder 'App.config') -Destination ($exe + '.config') -Force
Write-Host 'Build completed. Opening LibraryDesk...' -ForegroundColor Green
& $exe
