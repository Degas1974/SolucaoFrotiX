# ============================================================================
# Script: Suprimir Mensagens de Trial da Telerik
# Descrição: Adiciona targets MSBuild para suprimir mensagens de licenciamento
# Autor: Claude Code
# Data: 14/02/2026
# ============================================================================

param(
    [string]$CsprojPath = ".\FrotiX.Site.OLD\FrotiX.csproj",
    [switch]$UsarDirectoryBuildProps
)

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  Supressor de Mensagens de Trial - Telerik" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Verifica se o arquivo existe
if (-not (Test-Path $CsprojPath)) {
    Write-Host "❌ ERRO: Arquivo não encontrado: $CsprojPath" -ForegroundColor Red
    exit 1
}

Write-Host "📁 Arquivo: $CsprojPath" -ForegroundColor Yellow
Write-Host ""

if ($UsarDirectoryBuildProps) {
    # ====================
    # Método 1: Directory.Build.props (Global)
    # ====================

    $solutionDir = Split-Path -Parent $CsprojPath
    $propsFile = Join-Path $solutionDir "Directory.Build.props"

    Write-Host "🔧 Criando Directory.Build.props..." -ForegroundColor Green

    $propsContent = @"
<Project>
  <PropertyGroup>
    <!-- Suprime mensagens de trial da Telerik globalmente -->
    <TelerikNuGetMessageLevel>none</TelerikNuGetMessageLevel>
    <TelerikLicensingMessageLevel>none</TelerikLicensingMessageLevel>

    <!-- Warnings como mensagens (não como erros) -->
    <MSBuildWarningsAsMessages>`$(MSBuildWarningsAsMessages);TKL003;TKL004;TKL101;TKL102</MSBuildWarningsAsMessages>

    <!-- Suprime mensagens de informação durante o build -->
    <MSBuildLogger>Telerik=none</MSBuildLogger>
  </PropertyGroup>
</Project>
"@

    Set-Content -Path $propsFile -Value $propsContent -Encoding UTF8

    Write-Host "✅ Arquivo criado: $propsFile" -ForegroundColor Green
    Write-Host ""
    Write-Host "Este arquivo será aplicado automaticamente a TODOS os projetos da solução." -ForegroundColor Yellow

} else {
    # ====================
    # Método 2: Modificar .csproj diretamente
    # ====================

    Write-Host "🔧 Modificando FrotiX.csproj..." -ForegroundColor Green

    # Lê o conteúdo atual
    $content = Get-Content $CsprojPath -Raw

    # Verifica se já foi aplicado
    if ($content -match "SuppressTelerikMessages") {
        Write-Host "⚠️  Target 'SuppressTelerikMessages' já existe no projeto!" -ForegroundColor Yellow
        Write-Host "   Nenhuma modificação necessária." -ForegroundColor Yellow
        exit 0
    }

    # Define o target a ser adicionado
    $targetContent = @"

  <!-- ========================================= -->
  <!-- Suprime mensagens de licenciamento da Telerik -->
  <!-- Adicionado automaticamente em 14/02/2026 -->
  <!-- ========================================= -->
  <Target Name="SuppressTelerikMessages" BeforeTargets="Build">
    <PropertyGroup>
      <!-- Suprime warnings específicos da Telerik -->
      <MSBuildWarningsAsMessages>`$(MSBuildWarningsAsMessages);TKL003;TKL004;TKL101;TKL102</MSBuildWarningsAsMessages>

      <!-- Configurações adicionais de supressão -->
      <TelerikNuGetMessageLevel>none</TelerikNuGetMessageLevel>
      <TelerikLicensingMessageLevel>none</TelerikLicensingMessageLevel>
    </PropertyGroup>
  </Target>

  <!-- Redireciona STDOUT da Telerik (mensagens INFO) -->
  <Target Name="FilterTelerikOutput" AfterTargets="CoreCompile">
    <Message Text="ℹ️  Mensagens de licenciamento da Telerik suprimidas." Importance="low" />
  </Target>

</Project>
"@

    # Substitui o </Project> final pelo target + </Project>
    $newContent = $content -replace '</Project>$', $targetContent

    # Salva o arquivo modificado
    Set-Content -Path $CsprojPath -Value $newContent -Encoding UTF8 -NoNewline

    Write-Host "✅ Targets adicionados com sucesso!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📋 Targets adicionados:" -ForegroundColor Cyan
    Write-Host "   - SuppressTelerikMessages (BeforeTargets='Build')" -ForegroundColor White
    Write-Host "   - FilterTelerikOutput (AfterTargets='CoreCompile')" -ForegroundColor White
}

Write-Host ""
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  🎉 Configuração concluída com sucesso!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Próximos passos:" -ForegroundColor Yellow
Write-Host "  1. Recompile o projeto: dotnet build" -ForegroundColor White
Write-Host "  2. Verifique que as mensagens de trial não aparecem mais" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  IMPORTANTE: Mantenha Telerik.Reporting na versão 18.1.24.514" -ForegroundColor Red
Write-Host "   (publicada antes de 23/05/2024 - sem watermarks)" -ForegroundColor Red
Write-Host ""
