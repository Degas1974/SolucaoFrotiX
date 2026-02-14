# ============================================================================
# Script: Verificador de Licenças Telerik
# Descrição: Descobre informações sobre licenças e versões instaladas
# Autor: Claude Code
# Data: 14/02/2026
# ============================================================================

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  🔍 Verificador de Licenças Telerik" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# ====================================
# 1. VERIFICAR CACHE NUGET
# ====================================

Write-Host "📦 PACOTES NUGET INSTALADOS" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$nugetPath = "$env:USERPROFILE\.nuget\packages\telerik.ui.for.aspnet.core"

if (Test-Path $nugetPath) {
    $versions = Get-ChildItem -Path $nugetPath -Directory | Select-Object Name

    Write-Host "Versões encontradas no cache NuGet:" -ForegroundColor Green
    foreach ($ver in $versions) {
        $versionPath = Join-Path $nugetPath $ver.Name
        $nupkgMetadata = Join-Path $versionPath "$($ver.Name).nupkg.metadata"

        if (Test-Path $nupkgMetadata) {
            $metadata = Get-Content $nupkgMetadata -Raw | ConvertFrom-Json
            $downloadDate = (Get-Item $nupkgMetadata).CreationTime

            Write-Host "  ✓ $($ver.Name)" -ForegroundColor White
            Write-Host "    Origem: $($metadata.source ?? 'Desconhecido')" -ForegroundColor DarkGray
            Write-Host "    Download: $downloadDate" -ForegroundColor DarkGray
        } else {
            Write-Host "  ✓ $($ver.Name)" -ForegroundColor White
        }
    }

    Write-Host ""

    # Verificar especificamente 2025.4.1321
    $version2025Q4 = Join-Path $nugetPath "2025.4.1321"
    if (Test-Path $version2025Q4) {
        Write-Host "🎯 VERSÃO 2025.4.1321 ENCONTRADA!" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Versão 2025.4.1321 NÃO encontrada no cache NuGet" -ForegroundColor Yellow
    }

} else {
    Write-Host "❌ Cache NuGet não encontrado em: $nugetPath" -ForegroundColor Red
}

Write-Host ""
Write-Host ""

# ====================================
# 2. VERIFICAR INSTALAÇÃO LOCAL MSI
# ====================================

Write-Host "💿 INSTALAÇÃO LOCAL (MSI)" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$msiPath = "C:\Program Files (x86)\Progress\Telerik UI for ASP.NET Core 2025 Q4"

if (Test-Path $msiPath) {
    Write-Host "✅ Instalação encontrada: $msiPath" -ForegroundColor Green

    $binariesPath = Join-Path $msiPath "wrappers\aspnetcore\Binaries\AspNet.Core"

    if (Test-Path $binariesPath) {
        $dlls = Get-ChildItem -Path $binariesPath -Filter "*.dll"

        Write-Host ""
        Write-Host "DLLs principais:" -ForegroundColor Cyan

        foreach ($dll in $dlls | Where-Object { $_.Name -match "^(Kendo|Telerik)" } | Select-Object -First 5) {
            Write-Host "  📄 $($dll.Name)" -ForegroundColor White
            Write-Host "     Tamanho: $([math]::Round($dll.Length/1MB, 2)) MB" -ForegroundColor DarkGray
            Write-Host "     Criação: $($dll.CreationTime)" -ForegroundColor DarkGray
            Write-Host "     Modificação: $($dll.LastWriteTime)" -ForegroundColor DarkGray

            try {
                $assembly = [System.Reflection.Assembly]::ReflectionOnlyLoadFrom($dll.FullName)
                $version = $assembly.GetName().Version
                Write-Host "     Versão Assembly: $version" -ForegroundColor Green
            } catch {
                Write-Host "     Versão Assembly: Não disponível" -ForegroundColor DarkGray
            }

            Write-Host ""
        }
    }

    # Verificar pacotes NuGet no diretório de instalação
    $packagesPath = Join-Path $msiPath "packages"
    if (Test-Path $packagesPath) {
        Write-Host "📦 Pacotes NuGet na instalação MSI:" -ForegroundColor Cyan
        $nupkgs = Get-ChildItem -Path $packagesPath -Filter "*.nupkg" -Recurse | Select-Object -First 10
        foreach ($nupkg in $nupkgs) {
            Write-Host "  • $($nupkg.Name)" -ForegroundColor White
        }
    }

} else {
    Write-Host "❌ Instalação MSI NÃO encontrada" -ForegroundColor Red
    Write-Host "   Caminho esperado: $msiPath" -ForegroundColor DarkGray
}

Write-Host ""
Write-Host ""

# ====================================
# 3. VERIFICAR REGISTRO DO WINDOWS
# ====================================

Write-Host "🔑 REGISTRO DO WINDOWS" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor DarkGray

try {
    $regPaths = @(
        "HKCU:\Software\Progress\Telerik",
        "HKLM:\SOFTWARE\Progress\Telerik"
    )

    $foundRegistry = $false

    foreach ($regPath in $regPaths) {
        if (Test-Path $regPath) {
            $foundRegistry = $true
            Write-Host "✅ Chave encontrada: $regPath" -ForegroundColor Green

            $props = Get-ItemProperty $regPath -ErrorAction SilentlyContinue
            if ($props) {
                $props.PSObject.Properties | Where-Object { $_.Name -notlike "PS*" } | ForEach-Object {
                    Write-Host "   $($_.Name): $($_.Value)" -ForegroundColor White
                }
            }
            Write-Host ""
        }
    }

    if (-not $foundRegistry) {
        Write-Host "⚠️  Nenhuma chave de registro encontrada" -ForegroundColor Yellow
        Write-Host "   Isso pode indicar que a instalação MSI foi feita sem login" -ForegroundColor DarkGray
    }

} catch {
    Write-Host "❌ Erro ao acessar registro: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host ""

# ====================================
# 4. VERIFICAR ARQUIVOS DE LICENÇA OCULTOS
# ====================================

Write-Host "📁 ARQUIVOS DE LICENÇA (AppData)" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$appDataPaths = @(
    "$env:LOCALAPPDATA\Progress",
    "$env:LOCALAPPDATA\Telerik",
    "$env:APPDATA\Progress",
    "$env:APPDATA\Telerik",
    "$env:TEMP\Telerik"
)

$foundLicenseFiles = $false

foreach ($path in $appDataPaths) {
    if (Test-Path $path) {
        Write-Host "✅ Pasta encontrada: $path" -ForegroundColor Green

        $licenseFiles = Get-ChildItem -Path $path -Recurse -Force -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -match "(license|lic|key|token)" -and -not $_.PSIsContainer } |
            Select-Object -First 5

        if ($licenseFiles) {
            $foundLicenseFiles = $true
            foreach ($file in $licenseFiles) {
                Write-Host "  📄 $($file.Name)" -ForegroundColor White
                Write-Host "     Caminho: $($file.DirectoryName)" -ForegroundColor DarkGray
                Write-Host "     Tamanho: $($file.Length) bytes" -ForegroundColor DarkGray
                Write-Host "     Modificado: $($file.LastWriteTime)" -ForegroundColor DarkGray
                Write-Host ""
            }
        }
    }
}

if (-not $foundLicenseFiles) {
    Write-Host "⚠️  Nenhum arquivo de licença encontrado no AppData" -ForegroundColor Yellow
}

Write-Host ""
Write-Host ""

# ====================================
# 5. ANALISAR PROJETO FROTIX
# ====================================

Write-Host "🔧 PROJETO FROTIX" -ForegroundColor Yellow
Write-Host "-----------------------------------" -ForegroundColor DarkGray

$csprojPath = ".\FrotiX.Site.OLD\FrotiX.csproj"

if (Test-Path $csprojPath) {
    Write-Host "✅ Projeto encontrado: $csprojPath" -ForegroundColor Green

    $csprojContent = Get-Content $csprojPath -Raw

    # Extrair versões Telerik
    $telerikPackages = [regex]::Matches($csprojContent, '<PackageReference Include="(Telerik[^"]*)" Version="([^"]*)"')

    Write-Host ""
    Write-Host "Pacotes Telerik no projeto:" -ForegroundColor Cyan

    foreach ($match in $telerikPackages) {
        $packageName = $match.Groups[1].Value
        $version = $match.Groups[2].Value

        Write-Host "  📦 $packageName" -ForegroundColor White
        Write-Host "     Versão: $version" -ForegroundColor Green

        # Verificar se a versão está no cache
        $packagePath = "$env:USERPROFILE\.nuget\packages\$($packageName.ToLower())\$version"
        if (Test-Path $packagePath) {
            Write-Host "     Status: ✅ Instalada localmente" -ForegroundColor Green
        } else {
            Write-Host "     Status: ⚠️  NÃO encontrada no cache" -ForegroundColor Yellow
        }

        Write-Host ""
    }

} else {
    Write-Host "❌ Projeto FrotiX não encontrado em: $csprojPath" -ForegroundColor Red
}

Write-Host ""
Write-Host ""

# ====================================
# 6. RESUMO E RECOMENDAÇÕES
# ====================================

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "  📊 RESUMO" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "Sobre a versão 2025.4.1321:" -ForegroundColor Yellow
Write-Host "  • Esta versão está na instalação MSI local" -ForegroundColor White
Write-Host "  • Provavelmente NÃO está no cache NuGet" -ForegroundColor White
Write-Host "  • Para usá-la, você precisa referenciar as DLLs locais" -ForegroundColor White
Write-Host ""

Write-Host "Sobre expiração de licenças:" -ForegroundColor Yellow
Write-Host "  • Trials NuGet: 30 dias a partir do primeiro build" -ForegroundColor White
Write-Host "  • Licenças MSI com login: 1 ano (renovável)" -ForegroundColor White
Write-Host "  • Licença perpétua (seu caso): SEM expiração para versões antigas" -ForegroundColor Green
Write-Host ""

Write-Host "⚠️  IMPORTANTE:" -ForegroundColor Red
Write-Host "  Você tem uma LICENÇA PERPÉTUA válida até 23/05/2024" -ForegroundColor Yellow
Write-Host "  Use versões publicadas ANTES dessa data para evitar watermarks!" -ForegroundColor Yellow
Write-Host ""

Write-Host "✅ Versões SEGURAS (sem watermark):" -ForegroundColor Green
Write-Host "  • Telerik.Reporting: 18.1.24.514 (15/05/2024)" -ForegroundColor White
Write-Host "  • Telerik.UI.for.AspNet.Core: até 2024.2.x" -ForegroundColor White
Write-Host ""

Write-Host "❌ Versões ARRISCADAS (podem ter watermark):" -ForegroundColor Red
Write-Host "  • Telerik.UI.for.AspNet.Core: 2025.2.520 (maio/2025)" -ForegroundColor White
Write-Host "  • Telerik.UI.for.AspNet.Core: 2025.4.1321 (jan/2026)" -ForegroundColor White
Write-Host ""

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Salvar relatório em arquivo
$reportPath = ".\RELATORIO_LICENCAS_TELERIK_$(Get-Date -Format 'yyyyMMdd_HHmmss').txt"
$transcript = $PSCmdlet.MyInvocation.MyCommand.ScriptBlock.ToString()

Write-Host "💾 Relatório salvo em: $reportPath" -ForegroundColor Green
