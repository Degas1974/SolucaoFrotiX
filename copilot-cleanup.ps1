# =============================================================================
# SCRIPT DE LIMPEZA DO GITHUB COPILOT
# =============================================================================
# Este script realiza limpeza completa do cache e dados do GitHub Copilot
# IMPORTANTE: Feche o VS Code antes de executar este script!
# =============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  LIMPEZA DO GITHUB COPILOT - INICIO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se VS Code está rodando
$vscodeProcess = Get-Process -Name "Code" -ErrorAction SilentlyContinue
if ($vscodeProcess) {
    Write-Host "AVISO: VS Code está em execução!" -ForegroundColor Red
    Write-Host "Feche o VS Code antes de continuar." -ForegroundColor Yellow
    $continue = Read-Host "Deseja continuar mesmo assim? (S/N)"
    if ($continue -ne "S" -and $continue -ne "s") {
        Write-Host "Script cancelado." -ForegroundColor Yellow
        exit
    }
}

# Criar backup antes de limpar
$backupDir = "$env:USERPROFILE\Desktop\copilot-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
Write-Host "Criando backup em: $backupDir" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

# Caminhos a serem limpos
$copilotPaths = @(
    "$env:APPDATA\Code\User\globalStorage\github.copilot",
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat",
    "$env:APPDATA\Code\User\workspaceStorage\*\github.copilot*",
    "$env:APPDATA\Code\CachedExtensionVSIXs\github.copilot*",
    "$env:APPDATA\Code\logs\*\exthost\GitHub.copilot*"
)

$itemsRemoved = 0
$totalSize = 0

foreach ($path in $copilotPaths) {
    $items = Get-Item $path -ErrorAction SilentlyContinue

    if ($items) {
        foreach ($item in $items) {
            # Calcular tamanho
            if ($item.PSIsContainer) {
                $size = (Get-ChildItem -Path $item.FullName -Recurse -File -ErrorAction SilentlyContinue |
                        Measure-Object -Property Length -Sum).Sum
            } else {
                $size = $item.Length
            }

            $totalSize += $size
            $sizeMB = [math]::Round($size / 1MB, 2)

            Write-Host "Removendo: $($item.FullName) ($sizeMB MB)" -ForegroundColor Yellow

            # Fazer backup
            $relativePath = $item.FullName.Replace("$env:APPDATA\Code\", "")
            $backupPath = Join-Path $backupDir $relativePath
            $backupParent = Split-Path $backupPath -Parent

            if (-not (Test-Path $backupParent)) {
                New-Item -ItemType Directory -Path $backupParent -Force | Out-Null
            }

            try {
                Copy-Item -Path $item.FullName -Destination $backupPath -Recurse -Force -ErrorAction SilentlyContinue
                Remove-Item -Path $item.FullName -Recurse -Force -ErrorAction SilentlyContinue
                $itemsRemoved++
                Write-Host "  ✓ Removido com sucesso" -ForegroundColor Green
            } catch {
                Write-Host "  ✗ Erro ao remover: $_" -ForegroundColor Red
            }
        }
    }
}

# Limpar logs antigos do Copilot (manter apenas últimos 2 dias)
Write-Host "`nLimpando logs antigos do Copilot..." -ForegroundColor Yellow
$logsPath = "$env:APPDATA\Code\logs"
$cutoffDate = (Get-Date).AddDays(-2)

Get-ChildItem -Path $logsPath -Directory | Where-Object {
    $_.Name -match '^\d{8}T\d{6}$' -and $_.CreationTime -lt $cutoffDate
} | ForEach-Object {
    $copilotLogs = Get-ChildItem -Path $_.FullName -Recurse -Filter "*copilot*" -ErrorAction SilentlyContinue
    if ($copilotLogs) {
        Write-Host "Removendo logs antigos de: $($_.Name)" -ForegroundColor Yellow
        $copilotLogs | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    }
}

# Resumo
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  LIMPEZA CONCLUÍDA" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Itens removidos: $itemsRemoved" -ForegroundColor Green
Write-Host "Espaço liberado: $([math]::Round($totalSize / 1MB, 2)) MB" -ForegroundColor Green
Write-Host "Backup salvo em: $backupDir" -ForegroundColor Green
Write-Host "`nPróximos passos:" -ForegroundColor Yellow
Write-Host "1. Reinicie o VS Code" -ForegroundColor White
Write-Host "2. Aguarde o Copilot reinicializar" -ForegroundColor White
Write-Host "3. Teste novamente o Copilot Editor" -ForegroundColor White
Write-Host ""
