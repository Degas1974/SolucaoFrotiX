# Script para corrigir arquivos Razor com problema de espaço antes de *@
# Execute este script no PowerShell do Windows como Administrador

$projectPath = "D:\FrotiX\Solucao FrotiX 2026\FrotiX.Site"

Write-Host "Limpando cache do Visual Studio e .NET..." -ForegroundColor Yellow

# Parar processos do Visual Studio se estiverem rodando
Get-Process | Where-Object {$_.ProcessName -like "devenv*"} | ForEach-Object {
    Write-Host "Encerrando Visual Studio (PID: $($_.Id))..." -ForegroundColor Red
    Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
}

Start-Sleep -Seconds 2

# Limpar pastas bin e obj
Write-Host "Removendo pastas bin e obj..." -ForegroundColor Yellow
Remove-Item "$projectPath\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "$projectPath\obj" -Recurse -Force -ErrorAction SilentlyContinue

# Limpar cache do NuGet
Write-Host "Limpando cache local do projeto..." -ForegroundColor Yellow
dotnet clean "$projectPath\FrotiX.csproj" --verbosity quiet

# Verificar e corrigir arquivos com problema
Write-Host "`nVerificando arquivos Razor..." -ForegroundColor Cyan

$files = @(
    "$projectPath\Pages\Shared\_LeftPanel.cshtml",
    "$projectPath\Pages\Shared\_Logo.cshtml",
    "$projectPath\Pages\Shared\_NavFilterMsg.cshtml",
    "$projectPath\Pages\Shared\_NavFooter.cshtml",
    "$projectPath\Pages\Temp\Index.cshtml",
    "$projectPath\Pages\Agenda\Index.cshtml",
    "$projectPath\Pages\Shared\_Head.cshtml",
    "$projectPath\Pages\Shared\_Layout.cshtml",
    "$projectPath\Pages\Shared\_PageContentOverlay.cshtml",
    "$projectPath\Pages\Shared\_ScriptsBasePlugins.cshtml",
    "$projectPath\Pages\Viagens\ItensPendentes.cshtml",
    "$projectPath\Pages\Viagens\Upsert.cshtml"
)

$fixedCount = 0

foreach ($file in $files) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw -Encoding UTF8
        $originalContent = $content

        # Remover espaço antes de *@ no final de comentários
        $content = $content -replace ' \*@\r?\n', '*@' + "`r`n"
        $content = $content -replace ' \*@$', '*@'

        if ($content -ne $originalContent) {
            Set-Content $file -Value $content -Encoding UTF8 -NoNewline
            Write-Host "  ✓ Corrigido: $file" -ForegroundColor Green
            $fixedCount++
        } else {
            Write-Host "  OK: $file" -ForegroundColor Gray
        }
    } else {
        Write-Host "  ✗ Não encontrado: $file" -ForegroundColor Red
    }
}

Write-Host "`n$fixedCount arquivo(s) corrigido(s)." -ForegroundColor Cyan

# Verificar arquivo Temp/Index.cshtml especificamente
$tempFile = "$projectPath\Pages\Temp\Index.cshtml"
if (Test-Path $tempFile) {
    $lineCount = (Get-Content $tempFile).Count
    Write-Host "`nArquivo Temp/Index.cshtml tem $lineCount linhas" -ForegroundColor Yellow

    if ($lineCount -ne 103) {
        Write-Host "  AVISO: Deveria ter 103 linhas! Arquivo pode estar desatualizado." -ForegroundColor Red
        Write-Host "  Linha 46 deveria ser: '*****************************************************************************************@'" -ForegroundColor Yellow
    }
}

Write-Host "`nReconstruindo projeto..." -ForegroundColor Yellow
dotnet build "$projectPath\FrotiX.csproj" --no-incremental

Write-Host "`nConcluído! Agora você pode abrir o Visual Studio." -ForegroundColor Green
Write-Host "Pressione qualquer tecla para continuar..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
