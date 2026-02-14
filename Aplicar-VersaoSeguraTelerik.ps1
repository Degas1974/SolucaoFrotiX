# ============================================================================
# Script: Aplicar Versão Segura da Telerik
# Descrição: Faz downgrade para versão coberta pela licença perpétua
# Autor: Claude Code
# Data: 14/02/2026
# ============================================================================

param(
    [string]$VersaoAlvo = "2024.2.514",
    [string]$CsprojPath = ".\FrotiX.Site.OLD\FrotiX.csproj",
    [switch]$Backup = $true
)

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  🔧 Aplicar Versão Segura da Telerik" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Validar arquivo
if (-not (Test-Path $CsprojPath)) {
    Write-Host "❌ ERRO: Arquivo não encontrado: $CsprojPath" -ForegroundColor Red
    exit 1
}

Write-Host "📁 Arquivo: $CsprojPath" -ForegroundColor Yellow
Write-Host "🎯 Versão alvo: $VersaoAlvo" -ForegroundColor Yellow
Write-Host ""

# Validar versão
$versoesSeguras = @("2023.3.1010", "2024.1.130", "2024.2.514")

if ($VersaoAlvo -notin $versoesSeguras) {
    Write-Host "⚠️  AVISO: Versão $VersaoAlvo não está na lista de versões recomendadas!" -ForegroundColor Yellow
    Write-Host "   Versões seguras (cobertas pela licença perpétua):" -ForegroundColor Yellow
    foreach ($v in $versoesSeguras) {
        Write-Host "     • $v" -ForegroundColor White
    }
    Write-Host ""

    $continuar = Read-Host "Deseja continuar mesmo assim? (S/N)"
    if ($continuar -ne "S" -and $continuar -ne "s") {
        Write-Host "Operação cancelada." -ForegroundColor Yellow
        exit 0
    }
}

# Fazer backup
if ($Backup) {
    $backupPath = "$CsprojPath.bak_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    Copy-Item $CsprojPath $backupPath
    Write-Host "💾 Backup criado: $backupPath" -ForegroundColor Green
    Write-Host ""
}

# Ler conteúdo
$content = Get-Content $CsprojPath -Raw

# Extrair versões atuais
Write-Host "🔍 VERSÕES ATUAIS:" -ForegroundColor Cyan
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$matches = [regex]::Matches($content, '<PackageReference Include="(Telerik[^"]*)" Version="([^"]*)"')

$pacotesAfetados = @(
    "Telerik.UI.for.AspNet.Core",
    "Telerik.Web.PDF"
)

foreach ($match in $matches) {
    $packageName = $match.Groups[1].Value
    $currentVersion = $match.Groups[2].Value

    if ($packageName -in $pacotesAfetados) {
        Write-Host "  📦 $packageName" -ForegroundColor Yellow
        Write-Host "     Atual: $currentVersion" -ForegroundColor Red
        Write-Host "     Nova:  $VersaoAlvo" -ForegroundColor Green
        Write-Host ""
    }
}

# Confirmação
Write-Host "⚠️  Esta operação irá alterar o arquivo .csproj" -ForegroundColor Yellow
$confirmar = Read-Host "Deseja continuar? (S/N)"

if ($confirmar -ne "S" -and $confirmar -ne "s") {
    Write-Host "Operação cancelada." -ForegroundColor Yellow
    exit 0
}

Write-Host ""
Write-Host "🔧 Aplicando alterações..." -ForegroundColor Cyan

# Substituir versões
foreach ($package in $pacotesAfetados) {
    $pattern = "(<PackageReference Include=`"$package`" Version=`")[^`"]+(`")"
    $replacement = "`${1}$VersaoAlvo`${2}"

    $newContent = $content -replace $pattern, $replacement

    if ($newContent -ne $content) {
        Write-Host "  ✅ Atualizado: $package → $VersaoAlvo" -ForegroundColor Green
        $content = $newContent
    } else {
        Write-Host "  ⚠️  Não encontrado: $package" -ForegroundColor Yellow
    }
}

# Salvar arquivo
Set-Content -Path $CsprojPath -Value $content -Encoding UTF8 -NoNewline

Write-Host ""
Write-Host "✅ Arquivo atualizado com sucesso!" -ForegroundColor Green
Write-Host ""

# Restaurar pacotes
Write-Host "📦 Restaurando pacotes NuGet..." -ForegroundColor Cyan

try {
    Push-Location (Split-Path $CsprojPath -Parent)

    $restoreOutput = dotnet restore 2>&1

    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✅ Pacotes restaurados com sucesso!" -ForegroundColor Green
    } else {
        Write-Host "  ⚠️  Erro ao restaurar pacotes:" -ForegroundColor Yellow
        Write-Host $restoreOutput -ForegroundColor DarkGray
    }

    Pop-Location
} catch {
    Write-Host "  ❌ Erro: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  🎉 CONCLUÍDO!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 PRÓXIMOS PASSOS:" -ForegroundColor Yellow
Write-Host "  1. Compile o projeto: dotnet build" -ForegroundColor White
Write-Host "  2. Execute os testes" -ForegroundColor White
Write-Host "  3. Verifique se NÃO há mensagens de trial" -ForegroundColor White
Write-Host ""

Write-Host "✅ BENEFÍCIOS:" -ForegroundColor Green
Write-Host "  • Sem watermarks" -ForegroundColor White
Write-Host "  • Sem expiração" -ForegroundColor White
Write-Host "  • 100% legal (licença perpétua)" -ForegroundColor White
Write-Host "  • Sem mensagens de trial" -ForegroundColor White
Write-Host ""

Write-Host "⚠️  IMPORTANTE:" -ForegroundColor Red
Write-Host "  NUNCA atualize Telerik.Reporting além de 18.1.24.514!" -ForegroundColor Yellow
Write-Host "  (Esta versão foi publicada em 15/05/2024 - ANTES da expiração)" -ForegroundColor Yellow
Write-Host ""

# Exibir arquivo modificado
Write-Host "📄 ALTERAÇÕES NO ARQUIVO:" -ForegroundColor Cyan
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$newContent = Get-Content $CsprojPath -Raw
$telerikLines = [regex]::Matches($newContent, '<PackageReference Include="(Telerik[^"]*)" Version="([^"]*)"')

foreach ($match in $telerikLines) {
    $packageName = $match.Groups[1].Value
    $version = $match.Groups[2].Value

    if ($packageName -in $pacotesAfetados) {
        Write-Host "  ✅ $packageName = $version" -ForegroundColor Green
    } else {
        Write-Host "  • $packageName = $version" -ForegroundColor White
    }
}

Write-Host ""
