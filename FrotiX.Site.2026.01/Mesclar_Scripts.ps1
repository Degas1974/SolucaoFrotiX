# Script para mesclar os arquivos SQL em um único arquivo integrado

$baseFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\ATUALIZACAO_COMPLETA_INTEGRADA.sql"
$objetosFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\SCRIPT_ATUALIZACAO_PRODUCAO.sql"
$outputFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\ATUALIZACAO_COMPLETA_FINAL.sql"

Write-Host "Mesclando scripts SQL..." -ForegroundColor Cyan

# Ler arquivo base
$baseContent = Get-Content $baseFile -Raw -Encoding UTF8

# Ler arquivo de objetos (pular cabeçalho até a Seção 1)
$objetosContent = Get-Content $objetosFile -Raw -Encoding UTF8

# Extrair apenas as seções de criação de objetos (após o cabeçalho)
$startMarker = "-- ======================================================================================`r`n-- SE"
$objetosStartIndex = $objetosContent.IndexOf($startMarker)

if ($objetosStartIndex -gt 0) {
    $objetosSections = $objetosContent.Substring($objetosStartIndex)
    
    # Inserir o conteúdo de objetos no local marcado
    $markerToReplace = "-- Incluir aqui todo o conteúdo de SCRIPT_ATUALIZACAO_PRODUCAO.sql"
    
    if ($baseContent.Contains($markerToReplace)) {
        $finalContent = $baseContent.Replace($markerToReplace, $objetosSections)
        
        # Salvar arquivo final
        $finalContent | Out-File $outputFile -Encoding UTF8
        
        $fileSize = (Get-Item $outputFile).Length / 1MB
        $lineCount = (Get-Content $outputFile | Measure-Object -Line).Lines
        
        Write-Host ""
        Write-Host "Sucesso! Script integrado criado:" -ForegroundColor Green
        Write-Host "Arquivo: $outputFile" -ForegroundColor Cyan
        Write-Host "Tamanho: $("{0:N2}" -f $fileSize) MB" -ForegroundColor Gray
        Write-Host "Linhas: $("{0:N0}" -f $lineCount)" -ForegroundColor Gray
        Write-Host ""
        Write-Host "IMPORTANTE:" -ForegroundColor Yellow
        Write-Host "  1. FACA BACKUP do banco antes de executar" -ForegroundColor Yellow
        Write-Host "  2. Execute em horario de baixo movimento" -ForegroundColor Yellow
        Write-Host "  3. Tempo estimado: 5-10 minutos" -ForegroundColor Yellow
        Write-Host ""
    }
    else {
        Write-Host "ERRO: Marcador nao encontrado no arquivo base!" -ForegroundColor Red
    }
}
else {
    Write-Host "ERRO: Nao foi possivel localizar as secoes de objetos!" -ForegroundColor Red
}
