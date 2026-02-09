# =============================================================================
# ANALISADOR DE DIFERENÇAS EM TABELAS - Comparação Detalhada
# =============================================================================

$ErrorActionPreference = 'Stop'

$prodFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\FrotixProducao.sql"
$devFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\FrotixDesenvolvimento.sql"

$tabelasModificadas = @('Viagem', 'Manutencao', 'Recurso', 'Lavagem', 'Fornecedor')

Write-Host "`n======================================================================================" -ForegroundColor Cyan
Write-Host "ANÁLISE DETALHADA DE TABELAS MODIFICADAS" -ForegroundColor Cyan
Write-Host "======================================================================================`n" -ForegroundColor Cyan

$prodContent = Get-Content $prodFile -Raw -Encoding UTF8
$devContent = Get-Content $devFile -Raw -Encoding UTF8

function Get-TableDefinition {
    param(
        [string]$Content,
        [string]$TableName
    )
    
    # Capturar CREATE TABLE até o próximo GO
    $pattern = "(?s)CREATE TABLE\s+(?:\[?dbo\]?\.)?\[?$TableName\]?.*?(?=^GO\s*$)"
    $match = [regex]::Match($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    
    if ($match.Success) {
        return $match.Value
    }
    return $null
}

function Get-Columns {
    param([string]$TableDefinition)
    
    if (-not $TableDefinition) { return @() }
    
    $columns = @()
    
    # Extrair cada linha de coluna
    $lines = $TableDefinition -split "`n"
    $inColumns = $false
    
    foreach ($line in $lines) {
        $line = $line.Trim()
        
        if ($line -match "CREATE TABLE") {
            $inColumns = $true
            continue
        }
        
        if ($line -match "^\)") {
            break
        }
        
        if ($inColumns -and $line -and -not ($line -match "^PRIMARY KEY") -and -not ($line -match "^CONSTRAINT") -and -not ($line -match "^UNIQUE")) {
            # Extrair nome da coluna
            if ($line -match "^\[?(\w+)\]?\s+(.+?)(?:,\s*$|$)") {
                $columns += @{
                    Name = $matches[1]
                    Definition = $line.TrimEnd(',')
                }
            }
        }
    }
    
    return $columns
}

function Compare-Columns {
    param(
        [array]$ProdColumns,
        [array]$DevColumns,
        [string]$TableName
    )
    
    $prodColNames = $ProdColumns | ForEach-Object { $_.Name }
    $devColNames = $DevColumns | ForEach-Object { $_.Name }
    
    $novas = $devColNames | Where-Object { $_ -notin $prodColNames }
    $removidas = $prodColNames | Where-Object { $_ -notin $devColNames }
    $comuns = $devColNames | Where-Object { $_ -in $prodColNames }
    
    $alteradas = @()
    foreach ($colName in $comuns) {
        $prodDef = ($ProdColumns | Where-Object { $_.Name -eq $colName }).Definition
        $devDef = ($DevColumns | Where-Object { $_.Name -eq $colName }).Definition
        
        # Normalizar para comparação
        $prodDefNorm = $prodDef -replace '\s+', ' ' -replace '\[', '' -replace '\]', ''
        $devDefNorm = $devDef -replace '\s+', ' ' -replace '\[', '' -replace '\]', ''
        
        if ($prodDefNorm -ne $devDefNorm) {
            $alteradas += @{
                Name = $colName
                ProdDef = $prodDef
                DevDef = $devDef
            }
        }
    }
    
    return @{
        Novas = $novas
        Removidas = $removidas
        Alteradas = $alteradas
    }
}

$alterScripts = @()

foreach ($tableName in $tabelasModificadas) {
    Write-Host "==================================================================================" -ForegroundColor White
    Write-Host "TABELA: $tableName" -ForegroundColor Yellow
    Write-Host "==================================================================================" -ForegroundColor White
    
    $prodDef = Get-TableDefinition -Content $prodContent -TableName $tableName
    $devDef = Get-TableDefinition -Content $devContent -TableName $tableName
    
    if (-not $prodDef) {
        Write-Host "  Tabela NAO encontrada em PRODUCAO!" -ForegroundColor Red
        continue
    }
    
    if (-not $devDef) {
        Write-Host "  Tabela NAO encontrada em DESENVOLVIMENTO!" -ForegroundColor Red
        continue
    }
    
    $prodCols = Get-Columns -TableDefinition $prodDef
    $devCols = Get-Columns -TableDefinition $devDef
    
    Write-Host "`nColunas em PRODUCAO: $($prodCols.Count)" -ForegroundColor Gray
    Write-Host "Colunas em DESENVOLVIMENTO: $($devCols.Count)" -ForegroundColor Gray
    
    $diff = Compare-Columns -ProdColumns $prodCols -DevColumns $devCols -TableName $tableName
    
    if ($diff.Novas.Count -gt 0) {
        Write-Host "`nCOLUNAS NOVAS ($($diff.Novas.Count)):" -ForegroundColor Green
        foreach ($colName in $diff.Novas) {
            $colDef = ($devCols | Where-Object { $_.Name -eq $colName }).Definition
            Write-Host "  + $colName" -ForegroundColor Green
            Write-Host "    $colDef" -ForegroundColor Gray
            
            # Gerar ALTER TABLE
            $alterCmd = "ALTER TABLE [dbo].[$tableName] ADD $colDef"
            $alterScripts += @{
                Table = $tableName
                Type = "ADD"
                Column = $colName
                Script = $alterCmd
            }
        }
    }
    
    if ($diff.Removidas.Count -gt 0) {
        Write-Host "`nCOLUNAS REMOVIDAS ($($diff.Removidas.Count)):" -ForegroundColor Red
        foreach ($colName in $diff.Removidas) {
            Write-Host "  - $colName" -ForegroundColor Red
            
            # Gerar ALTER TABLE (comentado por segurança)
            $alterCmd = "-- ATENCAO: Remover coluna com dados! Verificar antes de executar`n-- ALTER TABLE [dbo].[$tableName] DROP COLUMN [$colName]"
            $alterScripts += @{
                Table = $tableName
                Type = "DROP"
                Column = $colName
                Script = $alterCmd
            }
        }
    }
    
    if ($diff.Alteradas.Count -gt 0) {
        Write-Host "`nCOLUNAS ALTERADAS ($($diff.Alteradas.Count)):" -ForegroundColor Yellow
        foreach ($col in $diff.Alteradas) {
            Write-Host "  * $($col.Name)" -ForegroundColor Yellow
            Write-Host "    PRODUCAO:       $($col.ProdDef)" -ForegroundColor Gray
            Write-Host "    DESENVOLVIMENTO: $($col.DevDef)" -ForegroundColor Cyan
            
            # Gerar ALTER TABLE (comentado, precisa revisão)
            $alterCmd = "-- ATENCAO: Alterar tipo/definicao de coluna! Verificar compatibilidade`n-- ALTER TABLE [dbo].[$tableName] ALTER COLUMN $($col.DevDef)"
            $alterScripts += @{
                Table = $tableName
                Type = "ALTER"
                Column = $col.Name
                Script = $alterCmd
            }
        }
    }
    
    if ($diff.Novas.Count -eq 0 -and $diff.Removidas.Count -eq 0 -and $diff.Alteradas.Count -eq 0) {
        Write-Host "`nNenhuma diferenca encontrada em colunas!" -ForegroundColor Gray
        Write-Host "A diferenca pode estar em indices, constraints ou outras propriedades." -ForegroundColor Gray
    }
    
    Write-Host ""
}

Write-Host "`n======================================================================================" -ForegroundColor Cyan
Write-Host "RESUMO DOS SCRIPTS ALTER TABLE" -ForegroundColor Cyan
Write-Host "======================================================================================`n" -ForegroundColor Cyan

if ($alterScripts.Count -eq 0) {
    Write-Host "Nenhum script ALTER TABLE gerado." -ForegroundColor Gray
} else {
    Write-Host "Total de scripts: $($alterScripts.Count)`n" -ForegroundColor White
    
    # Salvar scripts em arquivo
    $outputFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\ALTER_TABLE_SCRIPTS.sql"
    
    $sqlOutput = @"
/* ======================================================================================
   SCRIPTS ALTER TABLE - ATUALIZACAO DE TABELAS MODIFICADAS
   ======================================================================================
   
   DATA GERACAO: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
   
   IMPORTANTE:
   - Revise TODOS os scripts antes de executar
   - Scripts comentados (--) requerem ATENCAO ESPECIAL
   - Faca BACKUP antes de alterar estrutura de tabelas com dados
   
   ======================================================================================
*/

USE Frotix
GO

"@
    
    $currentTable = ""
    foreach ($script in $alterScripts) {
        if ($currentTable -ne $script.Table) {
            $currentTable = $script.Table
            $sqlOutput += "`n-- ========================================`n"
            $sqlOutput += "-- TABELA: $currentTable`n"
            $sqlOutput += "-- ========================================`n`n"
        }
        
        $sqlOutput += "PRINT 'Processando: $($script.Table).$($script.Column)...';`n"
        $sqlOutput += "GO`n`n"
        $sqlOutput += "$($script.Script);`n"
        $sqlOutput += "GO`n`n"
    }
    
    $sqlOutput | Out-File $outputFile -Encoding UTF8
    
    Write-Host "Scripts salvos em: ALTER_TABLE_SCRIPTS.sql" -ForegroundColor Green
    Write-Host ""
    
    # Exibir resumo por tabela
    $byTable = $alterScripts | Group-Object -Property Table
    foreach ($group in $byTable) {
        Write-Host "$($group.Name):" -ForegroundColor Yellow
        $adds = ($group.Group | Where-Object { $_.Type -eq "ADD" }).Count
        $drops = ($group.Group | Where-Object { $_.Type -eq "DROP" }).Count
        $alters = ($group.Group | Where-Object { $_.Type -eq "ALTER" }).Count
        
        if ($adds -gt 0) { Write-Host "  + $adds coluna(s) nova(s)" -ForegroundColor Green }
        if ($drops -gt 0) { Write-Host "  - $drops coluna(s) removida(s)" -ForegroundColor Red }
        if ($alters -gt 0) { Write-Host "  * $alters coluna(s) alterada(s)" -ForegroundColor Yellow }
    }
}

Write-Host "`n======================================================================================`n" -ForegroundColor Cyan
