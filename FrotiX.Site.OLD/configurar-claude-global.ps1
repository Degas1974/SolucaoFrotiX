# Script para configurar Claude Code com auto-aprovação global no VS Code
# Execução: .\configurar-claude-global.ps1

$settingsPath = "$env:APPDATA\Code\User\settings.json"

Write-Host "Configurando Claude Code para auto-aprovação global..." -ForegroundColor Cyan

# Verifica se o arquivo existe
if (-not (Test-Path $settingsPath)) {
    Write-Host "Arquivo de configurações não encontrado. Criando novo..." -ForegroundColor Yellow
    New-Item -Path $settingsPath -Force | Out-Null
    Set-Content -Path $settingsPath -Value "{}"
}

# Lê o conteúdo atual
$jsonContent = Get-Content $settingsPath -Raw | ConvertFrom-Json

# Adiciona ou atualiza as configurações do Claude Code
$jsonContent | Add-Member -MemberType NoteProperty -Name "claude.code.autoApproveTerminalCommands" -Value $true -Force
$jsonContent | Add-Member -MemberType NoteProperty -Name "claude.code.autoApproveFileEdits" -Value $true -Force
$jsonContent | Add-Member -MemberType NoteProperty -Name "claude.code.autoApproveFileOperations" -Value $true -Force
$jsonContent | Add-Member -MemberType NoteProperty -Name "claude.code.autoApproveActions" -Value $true -Force

# Salva de volta
$jsonContent | ConvertTo-Json -Depth 100 | Set-Content $settingsPath

Write-Host "✅ Configurações aplicadas com sucesso!" -ForegroundColor Green
Write-Host ""
Write-Host "As seguintes configurações foram adicionadas/atualizadas:" -ForegroundColor Cyan
Write-Host "  • claude.code.autoApproveTerminalCommands: true" -ForegroundColor White
Write-Host "  • claude.code.autoApproveFileEdits: true" -ForegroundColor White
Write-Host "  • claude.code.autoApproveFileOperations: true" -ForegroundColor White
Write-Host "  • claude.code.autoApproveActions: true" -ForegroundColor White
Write-Host ""
Write-Host "Arquivo modificado: $settingsPath" -ForegroundColor Yellow
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Reinicie o VS Code para as alterações terem efeito!" -ForegroundColor Red
