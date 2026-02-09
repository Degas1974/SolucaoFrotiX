# ============================================
# Script de Limpeza de Cache - FrotiX
# ============================================

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  LIMPEZA DE CACHE - FrotiX" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Passo 1: Matar processos IIS Express
Write-Host "[1/4] Finalizando processos IIS Express..." -ForegroundColor Yellow
$iisProcesses = Get-Process -Name "iisexpress" -ErrorAction SilentlyContinue
if ($iisProcesses) {
    $iisProcesses | Stop-Process -Force
    Write-Host "  OK - $($iisProcesses.Count) processo(s) IIS Express finalizado(s)" -ForegroundColor Green
}
else {
    Write-Host "  OK - Nenhum processo IIS Express em execucao" -ForegroundColor Gray
}

# Passo 2: Deletar pasta .vs (cache do Visual Studio e IIS Express)
Write-Host "[2/4] Removendo pasta .vs (cache do VS)..." -ForegroundColor Yellow
$vsFolder = Join-Path $PSScriptRoot ".vs"
if (Test-Path $vsFolder) {
    Remove-Item $vsFolder -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Pasta .vs removida" -ForegroundColor Green
}
else {
    Write-Host "  OK - Pasta .vs nao existe" -ForegroundColor Gray
}

# Passo 3: Deletar pastas bin e obj
Write-Host "[3/4] Removendo pastas bin e obj..." -ForegroundColor Yellow
$projectFolder = Join-Path $PSScriptRoot "FrotiX.Site"

$binFolder = Join-Path $projectFolder "bin"
if (Test-Path $binFolder) {
    Remove-Item $binFolder -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Pasta bin removida" -ForegroundColor Green
}
else {
    Write-Host "  OK - Pasta bin nao existe" -ForegroundColor Gray
}

$objFolder = Join-Path $projectFolder "obj"
if (Test-Path $objFolder) {
    Remove-Item $objFolder -Recurse -Force -ErrorAction SilentlyContinue
    Write-Host "  OK - Pasta obj removida" -ForegroundColor Green
}
else {
    Write-Host "  OK - Pasta obj nao existe" -ForegroundColor Gray
}

# Passo 4: Limpar cache do navegador (instruções)
Write-Host "[4/4] Cache do navegador..." -ForegroundColor Yellow
Write-Host "  LEMBRE-SE: Abra o DevTools (F12) e marque 'Disable cache'" -ForegroundColor Magenta

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  LIMPEZA CONCLUIDA!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "PROXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "1. Abra o Visual Studio" -ForegroundColor White
Write-Host "2. Build > Rebuild Solution" -ForegroundColor White
Write-Host "3. Pressione F5 para executar" -ForegroundColor White
Write-Host "4. No navegador, abra DevTools (F12) e marque 'Disable cache'" -ForegroundColor White
Write-Host ""
Write-Host "Pressione qualquer tecla para fechar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
