# =============================================================================
# SCRIPT DE COMPARAÇÃO AVANÇADA - SQL Server Database Schema Diff
# =============================================================================

$ErrorActionPreference = 'Stop'

$prodFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\FrotixProducao.sql"
$devFile = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\FrotixDesenvolvimento.sql"

Write-Host "`n==============================================================================" -ForegroundColor Cyan
Write-Host "ANÁLISE DETALHADA: PRODUÇÃO vs DESENVOLVIMENTO" -ForegroundColor Cyan
Write-Host "==============================================================================`n" -ForegroundColor Cyan

# Ler arquivos
$prodContent = Get-Content $prodFile -Raw -Encoding UTF8
$devContent = Get-Content $devFile -Raw -Encoding UTF8

# Função para extrair tabelas
function Get-Tables {
    param([string]$Content)
    $tables = @{}
    $pattern = '(?s)CREATE TABLE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?.*?(?=GO\s*$|\z)'
    $matches = [regex]::Matches($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $matches) {
        $tableName = $match.Groups[1].Value
        $tableCode = $match.Value.Trim()
        $tables[$tableName] = $tableCode
    }
    return $tables
}

# Função para extrair views
function Get-Views {
    param([string]$Content)
    $views = @{}
    $pattern = '(?s)CREATE\s+(?:OR\s+ALTER\s+)?VIEW\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?.*?(?=GO\s*$|\z)'
    $matches = [regex]::Matches($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $matches) {
        $viewName = $match.Groups[1].Value
        $viewCode = $match.Value.Trim()
        $views[$viewName] = $viewCode
    }
    return $views
}

# Função para extrair procedures
function Get-Procedures {
    param([string]$Content)
    $procedures = @{}
    $pattern = '(?s)CREATE\s+(?:OR\s+ALTER\s+)?PROC(?:EDURE)?\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?.*?(?=GO\s*$|\z)'
    $matches = [regex]::Matches($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $matches) {
        $procName = $match.Groups[1].Value
        $procCode = $match.Value.Trim()
        $procedures[$procName] = $procCode
    }
    return $procedures
}

# Função para extrair functions
function Get-Functions {
    param([string]$Content)
    $functions = @{}
    $pattern = '(?s)CREATE\s+(?:OR\s+ALTER\s+)?FUNCTION\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?.*?(?=GO\s*$|\z)'
    $matches = [regex]::Matches($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $matches) {
        $funcName = $match.Groups[1].Value
        $funcCode = $match.Value.Trim()
        $functions[$funcName] = $funcCode
    }
    return $functions
}

# Função para extrair triggers
function Get-Triggers {
    param([string]$Content)
    $triggers = @{}
    $pattern = '(?s)CREATE\s+(?:OR\s+ALTER\s+)?TRIGGER\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?.*?(?=GO\s*$|\z)'
    $matches = [regex]::Matches($Content, $pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    foreach ($match in $matches) {
        $trigName = $match.Groups[1].Value
        $trigCode = $match.Value.Trim()
        $triggers[$trigName] = $trigCode
    }
    return $triggers
}

Write-Host "Carregando objetos de PRODUÇÃO..." -ForegroundColor Yellow
$prodTables = Get-Tables $prodContent
$prodViews = Get-Views $prodContent
$prodProcedures = Get-Procedures $prodContent
$prodFunctions = Get-Functions $prodContent
$prodTriggers = Get-Triggers $prodContent

Write-Host "Carregando objetos de DESENVOLVIMENTO..." -ForegroundColor Yellow
$devTables = Get-Tables $devContent
$devViews = Get-Views $devContent
$devProcedures = Get-Procedures $devContent
$devFunctions = Get-Functions $devContent
$devTriggers = Get-Triggers $devContent

Write-Host "`n==============================================================================`n" -ForegroundColor Green

# Resumo
Write-Host "RESUMO DOS OBJETOS:`n" -ForegroundColor White
Write-Host ("  Tabelas       - Prod: {0,3} | Dev: {1,3} | Diff: {2}" -f $prodTables.Count, $devTables.Count, ($devTables.Count - $prodTables.Count))
Write-Host ("  Views         - Prod: {0,3} | Dev: {1,3} | Diff: {2}" -f $prodViews.Count, $devViews.Count, ($devViews.Count - $prodViews.Count))
Write-Host ("  Procedures    - Prod: {0,3} | Dev: {1,3} | Diff: {2}" -f $prodProcedures.Count, $devProcedures.Count, ($devProcedures.Count - $prodProcedures.Count))
Write-Host ("  Functions     - Prod: {0,3} | Dev: {1,3} | Diff: {2}" -f $prodFunctions.Count, $devFunctions.Count, ($devFunctions.Count - $prodFunctions.Count))
Write-Host ("  Triggers      - Prod: {0,3} | Dev: {1,3} | Diff: {2}" -f $prodTriggers.Count, $devTriggers.Count, ($devTriggers.Count - $prodTriggers.Count))

Write-Host "`n==============================================================================`n" -ForegroundColor Cyan

# Análise detalhada de TABELAS
Write-Host "TABELAS - NOVOS EM DESENVOLVIMENTO:" -ForegroundColor Green
$novasTables = $devTables.Keys | Where-Object { -not $prodTables.ContainsKey($_) } | Sort-Object
if ($novasTables) {
    $novasTables | ForEach-Object { Write-Host "  + $_" -ForegroundColor Green }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nTABELAS - REMOVIDAS EM DESENVOLVIMENTO:" -ForegroundColor Red
$removidasTables = $prodTables.Keys | Where-Object { -not $devTables.ContainsKey($_) } | Sort-Object
if ($removidasTables) {
    $removidasTables | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nTABELAS - MODIFICADAS:" -ForegroundColor Yellow
$modificadasTables = @()
foreach ($tableName in $devTables.Keys) {
    if ($prodTables.ContainsKey($tableName)) {
        $devCode = $devTables[$tableName] -replace '\s+', ' '
        $prodCode = $prodTables[$tableName] -replace '\s+', ' '
        if ($devCode -ne $prodCode) {
            $modificadasTables += $tableName
            Write-Host "  * $tableName" -ForegroundColor Yellow
        }
    }
}
if ($modificadasTables.Count -eq 0) {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`n==============================================================================`n" -ForegroundColor Cyan

# Análise detalhada de VIEWS
Write-Host "VIEWS - NOVOS EM DESENVOLVIMENTO:" -ForegroundColor Green
$novasViews = $devViews.Keys | Where-Object { -not $prodViews.ContainsKey($_) } | Sort-Object
if ($novasViews) {
    $novasViews | ForEach-Object { Write-Host "  + $_" -ForegroundColor Green }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nVIEWS - REMOVIDAS:" -ForegroundColor Red
$removidasViews = $prodViews.Keys | Where-Object { -not $devViews.ContainsKey($_) } | Sort-Object
if ($removidasViews) {
    $removidasViews | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nVIEWS - MODIFICADAS:" -ForegroundColor Yellow
$modificadasViews = @()
foreach ($viewName in $devViews.Keys) {
    if ($prodViews.ContainsKey($viewName)) {
        $devCode = $devViews[$viewName] -replace '\s+', ' '
        $prodCode = $prodViews[$viewName] -replace '\s+', ' '
        if ($devCode -ne $prodCode) {
            $modificadasViews += $viewName
            Write-Host "  * $viewName" -ForegroundColor Yellow
        }
    }
}
if ($modificadasViews.Count -eq 0) {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`n==============================================================================`n" -ForegroundColor Cyan

# Análise detalhada de PROCEDURES
Write-Host "PROCEDURES - NOVOS EM DESENVOLVIMENTO:" -ForegroundColor Green
$novasProcedures = $devProcedures.Keys | Where-Object { -not $prodProcedures.ContainsKey($_) } | Sort-Object
if ($novasProcedures) {
    $novasProcedures | ForEach-Object { Write-Host "  + $_" -ForegroundColor Green }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nPROCEDURES - REMOVIDAS:" -ForegroundColor Red
$removidasProcedures = $prodProcedures.Keys | Where-Object { -not $devProcedures.ContainsKey($_) } | Sort-Object
if ($removidasProcedures) {
    $removidasProcedures | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
} else {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`nPROCEDURES - MODIFICADAS:" -ForegroundColor Yellow
$modificadasProcedures = @()
foreach ($procName in $devProcedures.Keys) {
    if ($prodProcedures.ContainsKey($procName)) {
        $devCode = $devProcedures[$procName] -replace '\s+', ' '
        $prodCode = $prodProcedures[$procName] -replace '\s+', ' '
        if ($devCode -ne $prodCode) {
            $modificadasProcedures += $procName
            Write-Host "  * $procName" -ForegroundColor Yellow
        }
    }
}
if ($modificadasProcedures.Count -eq 0) {
    Write-Host "  (nenhuma)" -ForegroundColor Gray
}

Write-Host "`n==============================================================================`n" -ForegroundColor White

# Resumo Final
$totalNovos = $novasTables.Count + $novasViews.Count + $novasProcedures.Count
$totalModificados = $modificadasTables.Count + $modificadasViews.Count + $modificadasProcedures.Count
$totalRemovidos = $removidasTables.Count + $removidasViews.Count + $removidasProcedures.Count

Write-Host "RESUMO FINAL:" -ForegroundColor White
Write-Host ("  Novos objetos:       {0}" -f $totalNovos) -ForegroundColor Green
Write-Host ("  Objetos modificados: {0}" -f $totalModificados) -ForegroundColor Yellow
Write-Host ("  Objetos removidos:   {0}" -f $totalRemovidos) -ForegroundColor Red

# Salvar dados para o próximo script
$diffData = @{
    NovasTables = @($novasTables)
    ModificadasTables = @($modificadasTables)
    RemovidasTables = @($removidasTables)
    NovasViews = @($novasViews)
    ModificadasViews = @($modificadasViews)
    RemovidasViews = @($removidasViews)
    NovasProcedures = @($novasProcedures)
    ModificadasProcedures = @($modificadasProcedures)
    RemovidasProcedures = @($removidasProcedures)
    DevTables = $devTables
    DevViews = $devViews
    DevProcedures = $devProcedures
    ProdTables = $prodTables
    ProdViews = $prodViews
    ProdProcedures = $prodProcedures
}

$diffDataPath = "d:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.2026.01\DiffData.json"
$diffData | ConvertTo-Json -Depth 10 | Out-File $diffDataPath -Encoding UTF8

Write-Host "`nDados salvos em: DiffData.json" -ForegroundColor Gray
Write-Host "==============================================================================`n" -ForegroundColor Cyan
