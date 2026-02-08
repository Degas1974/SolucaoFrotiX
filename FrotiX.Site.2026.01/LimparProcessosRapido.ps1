# ============================================
#   FrotiX - Limpeza Rápida (SEM reiniciar SQL)
#   Executar como Administrador
# ============================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  FrotiX - Limpeza Rapida de Processos" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Lista de processos para matar
$processos = @(
    "iisexpress",
    "iisexpresstray",
    "VBCSCompiler",
    "ServiceHub.Host.dotnet.x64",
    "ServiceHub.Host.CLR.x64",
    "ServiceHub.IdentityHost",
    "ServiceHub.VSDetouredHost"
)

Write-Host "[1/2] Encerrando processos orfaos..." -ForegroundColor Yellow

foreach ($proc in $processos) {
    $running = Get-Process -Name $proc -ErrorAction SilentlyContinue
    if ($running) {
        Write-Host "       Matando $proc..." -ForegroundColor Gray
        Stop-Process -Name $proc -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "       Processos encerrados." -ForegroundColor Green
Write-Host ""

Write-Host "[2/2] Aguardando liberacao de recursos..." -ForegroundColor Yellow
Start-Sleep -Seconds 2
Write-Host "       Recursos liberados." -ForegroundColor Green
Write-Host ""

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Limpeza concluida!" -ForegroundColor Green
Write-Host "  Pode compilar no Visual Studio agora." -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
