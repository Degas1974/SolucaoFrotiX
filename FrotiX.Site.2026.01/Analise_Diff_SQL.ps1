# ====================================================================
# SCRIPT DE ANÁLISE E DIFF - FrotixProducao vs FrotixDesenvolvimento
# ====================================================================

param(
    [string]$ProducaoFile = "FrotixProducao.sql",
    [string]$DesenvolvimentoFile = "FrotixDesenvolvimento.sql"
)

Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     ANÁLISE DIFF - PRODUÇÃO vs DESENVOLVIMENTO                 ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# Função para extrair objetos de um arquivo SQL
function Get-SqlObjects {
    param([string]$FilePath)
    
    $content = Get-Content $FilePath -Raw
    $objects = @{
        Tables = @{}
        Views = @{}
        Procedures = @{}
        Functions = @{}
        Triggers = @{}
        Indexes = @{}
        ForeignKeys = @{}
        Defaults = @{}
        UniqueConstraints = @{}
    }
    
    # Padrões de regex para identificar objetos
    $patterns = @{
        Table = 'CREATE TABLE\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?'
        View = 'CREATE\s+(?:OR\s+ALTER\s+)?VIEW\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?'
        Procedure = 'CREATE\s+(?:OR\s+ALTER\s+)?PROC(?:EDURE)?\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?'
        Function = 'CREATE\s+(?:OR\s+ALTER\s+)?FUNCTION\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?'
        Trigger = 'CREATE\s+(?:OR\s+ALTER\s+)?TRIGGER\s+(?:\[?dbo\]?\.)?\[?(\w+)\]?'
        Index = 'CREATE\s+(?:UNIQUE\s+)?(?:CLUSTERED\s+)?(?:NONCLUSTERED\s+)?INDEX\s+\[?(\w+)\]?'
        ForeignKey = 'ALTER TABLE.*?ADD\s+CONSTRAINT\s+\[?(\w+)\]?\s+FOREIGN KEY'
        Default = 'ALTER TABLE.*?ADD\s+CONSTRAINT\s+\[?(\w+)\]?\s+DEFAULT'
        UniqueConstraint = 'ALTER TABLE.*?ADD\s+CONSTRAINT\s+\[?(\w+)\]?\s+UNIQUE'
    }
    
    foreach ($type in $patterns.Keys) {
        $matches = [regex]::Matches($content, $patterns[$type], [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
        $typePlural = switch($type) {
            'Procedure' { 'Procedures' }
            'Index' { 'Indexes' }
            'ForeignKey' { 'ForeignKeys' }
            'UniqueConstraint' { 'UniqueConstraints' }
            default { $type + 's' }
        }
        
        foreach ($match in $matches) {
            $objectName = $match.Groups[1].Value
            if ($objectName -and -not $objects[$typePlural].ContainsKey($objectName)) {
                # Tentar capturar o código completo do objeto
                $startIndex = $match.Index
                $endIndex = $content.IndexOf('GO', $startIndex)
                if ($endIndex -eq -1) { $endIndex = $content.Length }
                
                $objectCode = $content.Substring($startIndex, $endIndex - $startIndex).Trim()
                $objects[$typePlural][$objectName] = $objectCode
            }
        }
    }
    
    return $objects
}

# Carregar arquivos
$prodPath = Join-Path $PSScriptRoot $ProducaoFile
$devPath = Join-Path $PSScriptRoot $DesenvolvimentoFile

if (-not (Test-Path $prodPath)) {
    Write-Host "❌ ERRO: Arquivo não encontrado: $prodPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $devPath)) {
    Write-Host "❌ ERRO: Arquivo não encontrado: $devPath" -ForegroundColor Red
    exit 1
}

Write-Host "📂 Produção: $ProducaoFile" -ForegroundColor Gray
Write-Host "📂 Desenvolvimento: $DesenvolvimentoFile" -ForegroundColor Gray
Write-Host ""

Write-Host "⏳ Extraindo objetos de PRODUÇÃO..." -ForegroundColor Yellow
$prodObjects = Get-SqlObjects -FilePath $prodPath

Write-Host "⏳ Extraindo objetos de DESENVOLVIMENTO..." -ForegroundColor Yellow
$devObjects = Get-SqlObjects -FilePath $devPath

Write-Host ""
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║                    RESUMO DOS OBJETOS                          ║" -ForegroundColor Green
Write-Host "╠════════════════════════════════════════════════════════════════╣" -ForegroundColor Green

$summary = @()
foreach ($type in @('Tables', 'Views', 'Procedures', 'Functions', 'Triggers', 'Indexes', 'ForeignKeys', 'Defaults', 'UniqueConstraints')) {
    $prodCount = $prodObjects[$type].Count
    $devCount = $devObjects[$type].Count
    $diff = $devCount - $prodCount
    $diffStr = if ($diff -gt 0) { "+$diff" } elseif ($diff -lt 0) { "$diff" } else { "0" }
    
    $summary += [PSCustomObject]@{
        Tipo = $type
        Producao = $prodCount
        Desenvolvimento = $devCount
        Diferenca = $diffStr
    }
}

$summary | Format-Table -AutoSize | Out-String | ForEach-Object { Write-Host $_ -ForegroundColor White }

Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""

# Análise detalhada
$report = @{
    NovosObjetos = @{}
    ObjetosModificados = @{}
    ObjetosRemovidos = @{}
}

foreach ($type in @('Tables', 'Views', 'Procedures', 'Functions', 'Triggers', 'Indexes', 'ForeignKeys', 'Defaults', 'UniqueConstraints')) {
    $report.NovosObjetos[$type] = @()
    $report.ObjetosModificados[$type] = @()
    $report.ObjetosRemovidos[$type] = @()
    
    # Novos objetos (existem em DEV mas não em PROD)
    foreach ($objName in $devObjects[$type].Keys) {
        if (-not $prodObjects[$type].ContainsKey($objName)) {
            $report.NovosObjetos[$type] += $objName
        }
    }
    
    # Objetos removidos (existem em PROD mas não em DEV)
    foreach ($objName in $prodObjects[$type].Keys) {
        if (-not $devObjects[$type].ContainsKey($objName)) {
            $report.ObjetosRemovidos[$type] += $objName
        }
    }
    
    # Objetos modificados (existem em ambos mas com código diferente)
    foreach ($objName in $devObjects[$type].Keys) {
        if ($prodObjects[$type].ContainsKey($objName)) {
            $devCode = $devObjects[$type][$objName] -replace '\s+', ' '
            $prodCode = $prodObjects[$type][$objName] -replace '\s+', ' '
            
            if ($devCode -ne $prodCode) {
                $report.ObjetosModificados[$type] += $objName
            }
        }
    }
}

# Exibir relatório detalhado
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║              OBJETOS NOVOS EM DESENVOLVIMENTO                  ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

$totalNovos = 0
foreach ($type in @('Tables', 'Views', 'Procedures', 'Functions', 'Triggers', 'Indexes', 'ForeignKeys', 'Defaults', 'UniqueConstraints')) {
    if ($report.NovosObjetos[$type].Count -gt 0) {
        Write-Host "[$type] ($($report.NovosObjetos[$type].Count) novo(s)):" -ForegroundColor Yellow
        $report.NovosObjetos[$type] | Sort-Object | ForEach-Object {
            Write-Host "  ✨ $_" -ForegroundColor Green
        }
        Write-Host ""
        $totalNovos += $report.NovosObjetos[$type].Count
    }
}

if ($totalNovos -eq 0) {
    Write-Host "  ✅ Nenhum objeto novo." -ForegroundColor Gray
    Write-Host ""
}

Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Magenta
Write-Host "║                  OBJETOS MODIFICADOS                           ║" -ForegroundColor Magenta
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Magenta
Write-Host ""

$totalModificados = 0
foreach ($type in @('Tables', 'Views', 'Procedures', 'Functions', 'Triggers', 'Indexes', 'ForeignKeys', 'Defaults', 'UniqueConstraints')) {
    if ($report.ObjetosModificados[$type].Count -gt 0) {
        Write-Host "[$type] ($($report.ObjetosModificados[$type].Count) modificado(s)):" -ForegroundColor Yellow
        $report.ObjetosModificados[$type] | Sort-Object | ForEach-Object {
            Write-Host "  🔧 $_" -ForegroundColor Cyan
        }
        Write-Host ""
        $totalModificados += $report.ObjetosModificados[$type].Count
    }
}

if ($totalModificados -eq 0) {
    Write-Host "  ✅ Nenhum objeto modificado." -ForegroundColor Gray
    Write-Host ""
}

Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Red
Write-Host "║              OBJETOS REMOVIDOS EM DESENVOLVIMENTO              ║" -ForegroundColor Red
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Red
Write-Host ""

$totalRemovidos = 0
foreach ($type in @('Tables', 'Views', 'Procedures', 'Functions', 'Triggers', 'Indexes', 'ForeignKeys', 'Defaults', 'UniqueConstraints')) {
    if ($report.ObjetosRemovidos[$type].Count -gt 0) {
        Write-Host "[$type] ($($report.ObjetosRemovidos[$type].Count) removido(s)):" -ForegroundColor Yellow
        $report.ObjetosRemovidos[$type] | Sort-Object | ForEach-Object {
            Write-Host "  ❌ $_" -ForegroundColor Red
        }
        Write-Host ""
        $totalRemovidos += $report.ObjetosRemovidos[$type].Count
    }
}

if ($totalRemovidos -eq 0) {
    Write-Host "  ✅ Nenhum objeto removido." -ForegroundColor Gray
    Write-Host ""
}

# Resumo final
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor White
Write-Host "║                      RESUMO FINAL                              ║" -ForegroundColor White
Write-Host "╠════════════════════════════════════════════════════════════════╣" -ForegroundColor White
Write-Host "║  ✨ Objetos novos: $totalNovos" -ForegroundColor Green
Write-Host "║  🔧 Objetos modificados: $totalModificados" -ForegroundColor Cyan
Write-Host "║  ❌ Objetos removidos: $totalRemovidos" -ForegroundColor Red
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor White
Write-Host ""

# Salvar relatório detalhado em JSON
$reportPath = Join-Path $PSScriptRoot "DiffReport.json"
$report | ConvertTo-Json -Depth 10 | Out-File $reportPath -Encoding UTF8
Write-Host "📄 Relatório detalhado salvo em: DiffReport.json" -ForegroundColor Gray

# Salvar objetos extraídos
$objectsPath = Join-Path $PSScriptRoot "ObjectsExtracted.json"
@{
    Producao = $prodObjects
    Desenvolvimento = $devObjects
} | ConvertTo-Json -Depth 10 | Out-File $objectsPath -Encoding UTF8
Write-Host "📄 Objetos extraídos salvos em: ObjectsExtracted.json" -ForegroundColor Gray
Write-Host ""

Write-Host "✅ Análise concluída!" -ForegroundColor Green
Write-Host ""
Write-Host "📌 Próximo passo: Gerar script de atualização" -ForegroundColor Yellow
Write-Host ""
