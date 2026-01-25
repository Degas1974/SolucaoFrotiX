# Script para corrigir problema de "Loading..." no FrotiX Mobile
# Execute este script no diretório raiz do projeto

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CORREÇÃO FROTIX MOBILE - LOADING..." -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está no diretório correto
if (-not (Test-Path "FrotiX.Mobile.Economildo\MauiProgram.cs")) {
    Write-Host "ERRO: Execute este script no diretório raiz do projeto FrotiX.Mobile!" -ForegroundColor Red
    Write-Host "Diretório atual: $(Get-Location)" -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ Diretório do projeto encontrado" -ForegroundColor Green
Write-Host ""

# Fazer backup dos arquivos originais
Write-Host "1. Criando backup dos arquivos originais..." -ForegroundColor Yellow
$backupDir = "Backup_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

# Lista de arquivos para backup
$filesToBackup = @(
    "FrotiX.Mobile.Economildo\wwwroot\index.html",
    "FrotiX.Mobile.Economildo\MauiProgram.cs",
    "FrotiX.Mobile.Economildo\Components\App.razor",
    "FrotiX.Mobile.Economildo\Components\Pages\Home.razor",
    "FrotiX.Mobile.Economildo\Components\Layout\MainLayout.razor"
)

foreach ($file in $filesToBackup) {
    if (Test-Path $file) {
        Copy-Item $file -Destination "$backupDir\" -Force
        Write-Host "  ✓ Backup: $(Split-Path $file -Leaf)" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "2. Aplicando correções..." -ForegroundColor Yellow

# Aplicar arquivos de correção
try {
    # Index com debug
    if (Test-Path "index_DEBUG.html") {
        Copy-Item "index_DEBUG.html" -Destination "FrotiX.Mobile.Economildo\wwwroot\index.html" -Force
        Write-Host "  ✓ index.html (com debug)" -ForegroundColor Green
    }

    # MauiProgram simplificado
    if (Test-Path "MauiProgram_SIMPLES.cs") {
        Copy-Item "MauiProgram_SIMPLES.cs" -Destination "FrotiX.Mobile.Economildo\MauiProgram.cs" -Force
        Write-Host "  ✓ MauiProgram.cs (simplificado)" -ForegroundColor Green
    }

    # App.razor mínimo
    if (Test-Path "App_MINIMO.razor") {
        Copy-Item "App_MINIMO.razor" -Destination "FrotiX.Mobile.Economildo\Components\App.razor" -Force
        Write-Host "  ✓ App.razor (mínimo)" -ForegroundColor Green
    }

    # Home.razor mínimo
    if (Test-Path "Home_MINIMO.razor") {
        Copy-Item "Home_MINIMO.razor" -Destination "FrotiX.Mobile.Economildo\Components\Pages\Home.razor" -Force
        Write-Host "  ✓ Home.razor (mínimo)" -ForegroundColor Green
    }

    # MainLayout mínimo
    if (Test-Path "MainLayout_MINIMO.razor") {
        Copy-Item "MainLayout_MINIMO.razor" -Destination "FrotiX.Mobile.Economildo\Components\Layout\MainLayout.razor" -Force
        Write-Host "  ✓ MainLayout.razor (mínimo)" -ForegroundColor Green
    }

    # Página de teste
    if (Test-Path "Teste.razor") {
        Copy-Item "Teste.razor" -Destination "FrotiX.Mobile.Economildo\Components\Pages\Teste.razor" -Force
        Write-Host "  ✓ Teste.razor (página de teste)" -ForegroundColor Green
    }
}
catch {
    Write-Host "ERRO ao copiar arquivos: $_" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "3. Limpando cache de compilação..." -ForegroundColor Yellow

# Limpar bin e obj
$foldersToClean = @(
    "FrotiX.Mobile.Economildo\bin",
    "FrotiX.Mobile.Economildo\obj",
    "FrotiX.Mobile.Shared\bin",
    "FrotiX.Mobile.Shared\obj"
)

foreach ($folder in $foldersToClean) {
    if (Test-Path $folder) {
        Remove-Item -Path $folder -Recurse -Force
        Write-Host "  ✓ Removido: $folder" -ForegroundColor Gray
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  ✅ CORREÇÕES APLICADAS COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "1. Abra o projeto no Visual Studio" -ForegroundColor White
Write-Host "2. Faça 'Build > Rebuild Solution'" -ForegroundColor White
Write-Host "3. Execute no emulador Android" -ForegroundColor White
Write-Host "4. Abra o Chrome DevTools (chrome://inspect)" -ForegroundColor White
Write-Host "5. Verifique o console para mensagens de debug" -ForegroundColor White
Write-Host ""
Write-Host "ESPERADO:" -ForegroundColor Cyan
Write-Host "- Caixa amarela com debug no canto inferior direito" -ForegroundColor Yellow
Write-Host "- Mensagem '✅ BLAZOR ESTÁ FUNCIONANDO!'" -ForegroundColor Green
Write-Host "- Botões de teste funcionais" -ForegroundColor Green
Write-Host ""
Write-Host "Backup salvo em: $backupDir" -ForegroundColor Gray
Write-Host ""
Write-Host "Se precisar reverter:" -ForegroundColor Yellow
Write-Host "Copy-Item '$backupDir\*' -Destination 'FrotiX.Mobile.Economildo\' -Recurse -Force" -ForegroundColor Gray
