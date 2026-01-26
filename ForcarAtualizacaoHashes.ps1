# ============================================
# Script de Forçar Atualização de Hashes
# ============================================
# Este script força o ASP.NET Core a recalcular os hashes
# dos arquivos JS/CSS usando asp-append-version="true"

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  FORCAR ATUALIZACAO DE HASHES - FrotiX" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = Join-Path $PSScriptRoot "FrotiX.Site"

# Passo 1: Parar IIS Express
Write-Host "[1/6] Finalizando IIS Express..." -ForegroundColor Yellow
$iisProcesses = Get-Process -Name "iisexpress" -ErrorAction SilentlyContinue
if ($iisProcesses) {
    $iisProcesses | Stop-Process -Force
    Write-Host "  OK - IIS Express finalizado" -ForegroundColor Green
}
else {
    Write-Host "  OK - IIS Express nao estava rodando" -ForegroundColor Gray
}

# Passo 2: Limpar cache do TagHelper
Write-Host "[2/6] Limpando cache do TagHelper..." -ForegroundColor Yellow
$tagHelperCache = Join-Path $projectPath "obj\Debug\net8.0\*.TagHelpers.*"
if (Test-Path $tagHelperCache) {
    Remove-Item $tagHelperCache -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Cache do TagHelper limpo" -ForegroundColor Green
}
else {
    Write-Host "  OK - Cache do TagHelper nao existe" -ForegroundColor Gray
}

# Passo 3: "Tocar" nos arquivos JS modificados (atualizar timestamp)
Write-Host "[3/6] Atualizando timestamp dos arquivos JS modificados..." -ForegroundColor Yellow

$arquivosJS = @(
    "wwwroot\js\agendamento\components\exibe-viagem.js",
    "wwwroot\js\agendamento\main.js",
    "wwwroot\js\cadastros\modal_agenda.js",
    "wwwroot\js\cadastros\agendamento_viagem.js"
)

foreach ($arquivo in $arquivosJS) {
    $caminhoCompleto = Join-Path $projectPath $arquivo
    if (Test-Path $caminhoCompleto) {
        # Atualizar timestamp do arquivo
        (Get-Item $caminhoCompleto).LastWriteTime = Get-Date
        Write-Host "  OK - $arquivo atualizado" -ForegroundColor Green
    }
}

# Passo 4: "Tocar" no arquivo CSS
Write-Host "[4/6] Atualizando timestamp do CSS..." -ForegroundColor Yellow
$cssFile = Join-Path $projectPath "wwwroot\css\modal-viagens-consolidado.css"
if (Test-Path $cssFile) {
    (Get-Item $cssFile).LastWriteTime = Get-Date
    Write-Host "  OK - CSS atualizado" -ForegroundColor Green
}

# Passo 5: "Tocar" no Index.cshtml para forçar recompilação
Write-Host "[5/6] Atualizando Index.cshtml..." -ForegroundColor Yellow
$indexFile = Join-Path $projectPath "Pages\Agenda\Index.cshtml"
if (Test-Path $indexFile) {
    (Get-Item $indexFile).LastWriteTime = Get-Date
    Write-Host "  OK - Index.cshtml atualizado" -ForegroundColor Green
}

# Passo 6: Limpar bin e obj
Write-Host "[6/6] Limpando bin e obj..." -ForegroundColor Yellow

$binFolder = Join-Path $projectPath "bin"
if (Test-Path $binFolder) {
    Remove-Item $binFolder -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Pasta bin removida" -ForegroundColor Green
}

$objFolder = Join-Path $projectPath "obj"
if (Test-Path $objFolder) {
    Remove-Item $objFolder -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Pasta obj removida" -ForegroundColor Green
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ATUALIZACAO CONCLUIDA!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "PROXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "1. Abra o Visual Studio" -ForegroundColor White
Write-Host "2. Build > Rebuild Solution" -ForegroundColor White
Write-Host "3. Pressione F5" -ForegroundColor White
Write-Host "4. No navegador: Ctrl+Shift+Delete > Limpar cache" -ForegroundColor White
Write-Host "5. OU simplesmente: Ctrl+F5 (hard refresh)" -ForegroundColor White
Write-Host ""
Write-Host "IMPORTANTE: Os hashes dos arquivos JS/CSS serao recalculados!" -ForegroundColor Yellow
Write-Host ""
Write-Host "Pressione qualquer tecla para fechar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
