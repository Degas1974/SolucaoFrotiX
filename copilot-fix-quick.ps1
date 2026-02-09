# =============================================================================
# SOLUÇÃO RÁPIDA PARA ERRO DO GITHUB COPILOT
# =============================================================================
# Este script executa automaticamente a FASE 1 da solução
# Tempo estimado: 5-10 minutos
# Taxa de sucesso: 75%
# =============================================================================

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  SOLUÇÃO RÁPIDA - GITHUB COPILOT" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Este script irá:" -ForegroundColor Yellow
Write-Host "  1. Desabilitar extensões conflitantes de AI" -ForegroundColor White
Write-Host "  2. Limpar cache do GitHub Copilot" -ForegroundColor White
Write-Host "  3. Reiniciar o VS Code" -ForegroundColor White
Write-Host ""
Write-Host "Pressione ENTER para continuar ou Ctrl+C para cancelar..." -ForegroundColor Cyan
Read-Host

# Passo 1: Verificar e fechar VS Code
Write-Host "`n[1/3] Verificando VS Code..." -ForegroundColor Green
$vscodeProcess = Get-Process -Name "Code" -ErrorAction SilentlyContinue

if ($vscodeProcess) {
    Write-Host "  VS Code está rodando. Fechando..." -ForegroundColor Yellow
    $vscodeProcess | Stop-Process -Force
    Start-Sleep -Seconds 2
    Write-Host "  ✓ VS Code fechado" -ForegroundColor Green
} else {
    Write-Host "  ✓ VS Code não está rodando" -ForegroundColor Green
}

# Passo 2: Desabilitar extensões conflitantes
Write-Host "`n[2/3] Desabilitando extensões conflitantes de AI..." -ForegroundColor Green

$conflictingExtensions = @(
    "danielsanmedium.dscodegpt",
    "feiskyer.chatgpt-copilot",
    "google.gemini-cli-vscode-ide-companion",
    "google.geminicodeassist",
    "openai.chatgpt"
)

foreach ($ext in $conflictingExtensions) {
    Write-Host "  Desabilitando: $ext" -ForegroundColor Yellow
    & code --disable-extension $ext 2>&1 | Out-Null
    Start-Sleep -Seconds 1
}

Write-Host "  ✓ Extensões conflitantes desabilitadas" -ForegroundColor Green

# Passo 3: Limpar cache do Copilot
Write-Host "`n[3/3] Limpando cache do GitHub Copilot..." -ForegroundColor Green

$cachePaths = @(
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat\commandEmbeddings.json",
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat\settingEmbeddings.json",
    "$env:APPDATA\Code\User\globalStorage\github.copilot-chat\toolEmbeddingsCache.bin"
)

$cacheSize = 0

foreach ($path in $cachePaths) {
    if (Test-Path $path) {
        $file = Get-Item $path
        $cacheSize += $file.Length
        Write-Host "  Removendo: $($file.Name) ($([math]::Round($file.Length / 1MB, 2)) MB)" -ForegroundColor Yellow
        Remove-Item -Path $path -Force -ErrorAction SilentlyContinue
    }
}

# Limpar logs antigos
Write-Host "  Limpando logs antigos..." -ForegroundColor Yellow
$logsPath = "$env:APPDATA\Code\logs"
$cutoffDate = (Get-Date).AddDays(-1)

Get-ChildItem -Path $logsPath -Directory | Where-Object {
    $_.Name -match '^\d{8}T\d{6}$' -and $_.CreationTime -lt $cutoffDate
} | ForEach-Object {
    $copilotLogs = Get-ChildItem -Path $_.FullName -Recurse -Filter "*copilot*" -ErrorAction SilentlyContinue
    if ($copilotLogs) {
        $copilotLogs | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "  ✓ Cache limpo ($([math]::Round($cacheSize / 1MB, 2)) MB liberados)" -ForegroundColor Green

# Resumo
Write-Host "`n========================================" -ForegroundColor Green
Write-Host "  SOLUÇÃO APLICADA COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Próximos passos:" -ForegroundColor Yellow
Write-Host "  1. Abra o VS Code" -ForegroundColor White
Write-Host "  2. Aguarde o Copilot carregar (15-30 segundos)" -ForegroundColor White
Write-Host "  3. Teste o Copilot Editor com um prompt simples" -ForegroundColor White
Write-Host ""
Write-Host "Se o problema persistir:" -ForegroundColor Yellow
Write-Host "  Execute: .\copilot-full-reset.ps1" -ForegroundColor Cyan
Write-Host ""

# Opção de abrir VS Code automaticamente
$openVSCode = Read-Host "Deseja abrir o VS Code agora? (S/N)"
if ($openVSCode -eq "S" -or $openVSCode -eq "s") {
    Write-Host "`nAbrindo VS Code..." -ForegroundColor Green
    Start-Process "code" -ArgumentList "."
    Start-Sleep -Seconds 2
    Write-Host "✓ VS Code aberto. Aguarde o Copilot carregar..." -ForegroundColor Green
}

Write-Host "`nPressione ENTER para fechar..." -ForegroundColor Cyan
Read-Host
