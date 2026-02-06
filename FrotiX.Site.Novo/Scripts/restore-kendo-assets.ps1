$ErrorActionPreference = 'Stop'

$sourceRoot = 'C:\Program Files (x86)\Progress\Telerik UI for ASP.NET Core 2025 Q4'
$targetRoot = 'wwwroot\lib\kendo\2025.2.520'
$targetThemes = 'wwwroot\lib\kendo\themes\12.0.1\bootstrap'

if (-not (Test-Path $sourceRoot)) {
    throw "Pasta de origem não encontrada: $sourceRoot"
}

New-Item -ItemType Directory -Force -Path $targetRoot | Out-Null
New-Item -ItemType Directory -Force -Path $targetThemes | Out-Null

$sourceJsRoot = Join-Path $sourceRoot 'js'
$sourceJs = Join-Path $sourceJsRoot 'js'
$sourceStyles = Join-Path $sourceRoot 'styles'

if (Test-Path $sourceJs) {
    Copy-Item -Path (Join-Path $sourceJs '*') -Destination (Join-Path $targetRoot 'js') -Recurse -Force
    Write-Host "OK: JS -> $targetRoot\js"
} elseif (Test-Path $sourceJsRoot) {
    Copy-Item -Path (Join-Path $sourceJsRoot '*') -Destination (Join-Path $targetRoot 'js') -Recurse -Force
    Write-Host "OK: JS (raiz) -> $targetRoot\js"
} else {
    Write-Warning "Pasta JS não encontrada em: $sourceJsRoot"
}

if (Test-Path $sourceStyles) {
    Copy-Item -Path $sourceStyles -Destination (Join-Path $targetRoot 'styles') -Recurse -Force
    Write-Host "OK: styles -> $targetRoot\styles"
} else {
    Write-Warning "Pasta styles não encontrada em: $sourceStyles"
}

$themeCandidates = @(
    'bootstrap-main.css',
    'kendo.bootstrap-main.min.css',
    'kendo.bootstrap-main.css'
)

$themeFile = $null
foreach ($candidate in $themeCandidates) {
    $themeFile = Get-ChildItem -Path $sourceRoot -Recurse -Filter $candidate -File -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($themeFile) { break }
}

if ($themeFile) {
    Copy-Item -Path $themeFile.FullName -Destination (Join-Path $targetThemes 'bootstrap-main.css') -Force
    Write-Host "OK: Tema -> $targetThemes\bootstrap-main.css"
} else {
    Write-Warning "Arquivo de tema bootstrap não encontrado. Copie manualmente para $targetThemes\bootstrap-main.css"
}
