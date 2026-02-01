# =============================================================================
# SCRIPT DE RESET COMPLETO DO GITHUB COPILOT
# =============================================================================
# Este script realiza um reset TOTAL do GitHub Copilot, incluindo:
# - Desinstalação das extensões
# - Limpeza de cache e dados
# - Reinstalação das extensões
# IMPORTANTE: Feche o VS Code antes de executar!
# =============================================================================

Write-Host "========================================" -ForegroundColor Red
Write-Host "  RESET COMPLETO DO GITHUB COPILOT" -ForegroundColor Red
Write-Host "========================================" -ForegroundColor Red
Write-Host ""
Write-Host "ATENÇÃO: Este script irá:" -ForegroundColor Yellow
Write-Host "  1. Desinstalar as extensões do Copilot" -ForegroundColor White
Write-Host "  2. Remover todos os dados e cache" -ForegroundColor White
Write-Host "  3. Reinstalar as extensões" -ForegroundColor White
Write-Host ""

$confirm = Read-Host "Deseja continuar? (S/N)"
if ($confirm -ne "S" -and $confirm -ne "s") {
    Write-Host "Script cancelado." -ForegroundColor Yellow
    exit
}

# Verificar se VS Code está rodando
$vscodeProcess = Get-Process -Name "Code" -ErrorAction SilentlyContinue
if ($vscodeProcess) {
    Write-Host "`nERRO: VS Code está em execução!" -ForegroundColor Red
    Write-Host "Feche o VS Code antes de continuar." -ForegroundColor Yellow
    exit
}

# Criar backup
$backupDir = "$env:USERPROFILE\Desktop\copilot-full-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
Write-Host "`nCriando backup em: $backupDir" -ForegroundColor Yellow
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

# Backup das configurações do Copilot no settings.json
$settingsPath = "$env:APPDATA\Code\User\settings.json"
if (Test-Path $settingsPath) {
    Copy-Item -Path $settingsPath -Destination "$backupDir\settings.json.backup" -Force
    Write-Host "✓ Backup do settings.json criado" -ForegroundColor Green
}

# Passo 1: Desinstalar extensões do Copilot
Write-Host "`n[1/4] Desinstalando extensões do Copilot..." -ForegroundColor Cyan
$extensions = @("github.copilot", "github.copilot-chat")

foreach ($ext in $extensions) {
    Write-Host "  Desinstalando: $ext" -ForegroundColor Yellow
    & code --uninstall-extension $ext --force 2>&1 | Out-Null
    Start-Sleep -Seconds 2
}

# Passo 2: Limpar todos os dados
Write-Host "`n[2/4] Removendo todos os dados do Copilot..." -ForegroundColor Cyan

$pathsToClean = @(
    "$env:APPDATA\Code\User\globalStorage\github.copilot",
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat",
    "$env:APPDATA\Code\CachedExtensionVSIXs",
    "$env:USERPROFILE\.vscode\extensions\github.copilot*"
)

foreach ($path in $pathsToClean) {
    $items = Get-Item $path -ErrorAction SilentlyContinue
    if ($items) {
        foreach ($item in $items) {
            Write-Host "  Removendo: $($item.FullName)" -ForegroundColor Yellow
            Remove-Item -Path $item.FullName -Recurse -Force -ErrorAction SilentlyContinue
        }
    }
}

# Limpar workspaceStorage
Get-ChildItem -Path "$env:APPDATA\Code\User\workspaceStorage" -Directory | ForEach-Object {
    $copilotData = Get-ChildItem -Path $_.FullName -Recurse -Filter "*copilot*" -ErrorAction SilentlyContinue
    if ($copilotData) {
        Write-Host "  Removendo dados de workspace: $($_.Name)" -ForegroundColor Yellow
        $copilotData | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    }
}

# Limpar todos os logs
Write-Host "  Removendo logs do Copilot..." -ForegroundColor Yellow
Get-ChildItem -Path "$env:APPDATA\Code\logs" -Recurse -Filter "*copilot*" -ErrorAction SilentlyContinue |
    Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

# Passo 3: Limpar configurações específicas do Copilot no settings.json
Write-Host "`n[3/4] Limpando configurações do Copilot..." -ForegroundColor Cyan

if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath -Raw | ConvertFrom-Json

    # Remover configurações específicas do Copilot
    $copilotSettings = $settings.PSObject.Properties | Where-Object {
        $_.Name -like "github.copilot*" -or $_.Name -like "*copilot*"
    }

    foreach ($setting in $copilotSettings) {
        $settings.PSObject.Properties.Remove($setting.Name)
        Write-Host "  Removida configuração: $($setting.Name)" -ForegroundColor Yellow
    }

    # Salvar settings.json limpo
    $settings | ConvertTo-Json -Depth 100 | Set-Content $settingsPath -Force
    Write-Host "  ✓ Configurações limpas" -ForegroundColor Green
}

# Passo 4: Reinstalar extensões
Write-Host "`n[4/4] Reinstalando extensões do Copilot..." -ForegroundColor Cyan

foreach ($ext in $extensions) {
    Write-Host "  Instalando: $ext" -ForegroundColor Yellow
    & code --install-extension $ext --force 2>&1 | Out-Null
    Start-Sleep -Seconds 3
}

# Resumo final
Write-Host "`n========================================" -ForegroundColor Green
Write-Host "  RESET COMPLETO CONCLUÍDO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Backup salvo em: $backupDir" -ForegroundColor Cyan
Write-Host ""
Write-Host "PRÓXIMOS PASSOS:" -ForegroundColor Yellow
Write-Host "1. Abra o VS Code" -ForegroundColor White
Write-Host "2. Faça login no GitHub quando solicitado" -ForegroundColor White
Write-Host "3. Autorize o GitHub Copilot" -ForegroundColor White
Write-Host "4. Aguarde a sincronização completa" -ForegroundColor White
Write-Host "5. Teste o Copilot Editor novamente" -ForegroundColor White
Write-Host ""
Write-Host "Se o problema persistir, execute o script 'copilot-diagnostics.ps1'" -ForegroundColor Yellow
Write-Host ""
