# ========================================
# Script de Auditoria: Banco SQL Server REAL vs Modelos C#
# Versão: 1.0
# Data: 13/02/2026
# ========================================

param(
    [string]$SchemaCsvPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts\schema_banco_real.csv",
    [string]$ModelsPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models",
    [string]$OutputPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts"
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "AUDITORIA: Banco SQL Server REAL vs Modelos C#" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# ========================================
# FUNÇÕES AUXILIARES
# ========================================

function ConvertTo-CSharpType {
    param([string]$SqlType, [string]$IsNullable, [int]$MaxLength)

    $typeMap = @{
        'uniqueidentifier' = 'Guid'
        'nvarchar' = 'string'
        'varchar' = 'string'
        'int' = 'int'
        'bigint' = 'long'
        'bit' = 'bool'
        'datetime' = 'DateTime'
        'datetime2' = 'DateTime'
        'date' = 'DateTime'
        'time' = 'TimeSpan'
        'datetimeoffset' = 'DateTimeOffset'
        'float' = 'double'
        'decimal' = 'decimal'
        'varbinary' = 'byte[]'
        'image' = 'byte[]'
        'timestamp' = 'byte[]'
        'char' = 'string'
    }

    $csharpType = $typeMap[$SqlType]
    if (-not $csharpType) { $csharpType = "object" }

    # Tipos nullable em C#
    if ($IsNullable -eq 'YES' -and $csharpType -ne 'string' -and $csharpType -ne 'byte[]') {
        $csharpType += '?'
    }

    return $csharpType
}

function Parse-CSharpModel {
    param([string]$FilePath)

    $content = Get-Content $FilePath -Raw
    $properties = @{}

    # Regex para capturar propriedades C#
    $regex = '(?ms)(?:\[.*?\]\s*)*public\s+(\S+(?:\?)?)\s+(\w+)\s*\{\s*get;\s*set;\s*\}'

    $matches = [regex]::Matches($content, $regex)

    foreach ($match in $matches) {
        $propType = $match.Groups[1].Value
        $propName = $match.Groups[2].Value

        $properties[$propName] = @{
            Type = $propType
            IsNullable = $propType.Contains('?') -or $propType -eq 'string' -or $propType -eq 'byte[]'
        }
    }

    return $properties
}

# ========================================
# CARREGAR SCHEMA DO BANCO
# ========================================

Write-Host "[1/4] Carregando schema do banco SQL Server..." -ForegroundColor Yellow

$bancoSchema = Import-Csv $SchemaCsvPath -Header "TABLE_NAME","COLUMN_NAME","DATA_TYPE","MAX_LENGTH","IS_NULLABLE","DEFAULT_VALUE"
$bancoTables = $bancoSchema | Group-Object -Property TABLE_NAME

Write-Host "      Total de tabelas no banco: $($bancoTables.Count)" -ForegroundColor Green

# ========================================
# CARREGAR MODELOS C#
# ========================================

Write-Host "`n[2/4] Carregando modelos C#..." -ForegroundColor Yellow

$csFiles = Get-ChildItem -Path $ModelsPath -Filter "*.cs" -Recurse
$modelosC = @{}

foreach ($file in $csFiles) {
    $className = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)

    # Ignorar classes que não são entidades (DTO, ViewModel, etc)
    if ($className -match 'Dto$|ViewModel$|Model$|Extensions$') {
        continue
    }

    $modelosC[$className] = @{
        Path = $file.FullName
        Properties = Parse-CSharpModel -FilePath $file.FullName
    }
}

Write-Host "      Total de modelos C# encontrados: $($modelosC.Count)" -ForegroundColor Green

# ========================================
# ANÁLISE COMPARATIVA
# ========================================

Write-Host "`n[3/4] Executando análise comparativa..." -ForegroundColor Yellow

$discrepancias = @()
$tabelasOrfas = @()
$modelosOrfaos = @()

# Verificar cada tabela do banco
foreach ($tableGroup in $bancoTables) {
    $tableName = $tableGroup.Name
    $columns = $tableGroup.Group

    # Ignorar tabelas de sistema
    if ($tableName -match '^AspNet|^tr_|^sysdiagrams$|^Perfis$|^Usuarios$') {
        continue
    }

    # Verificar se existe modelo C# correspondente
    if (-not $modelosC.ContainsKey($tableName)) {
        $tabelasOrfas += [PSCustomObject]@{
            Tabela = $tableName
            TotalColunas = $columns.Count
        }
        continue
    }

    $modelo = $modelosC[$tableName]

    # Comparar cada coluna
    foreach ($column in $columns) {
        $columnName = $column.COLUMN_NAME
        $sqlType = $column.DATA_TYPE
        $isNullable = $column.IS_NULLABLE
        $maxLength = [int]$column.MAX_LENGTH

        # Ignorar colunas de sistema
        if ($columnName -match '^__') {
            continue
        }

        $expectedCSharpType = ConvertTo-CSharpType -SqlType $sqlType -IsNullable $isNullable -MaxLength $maxLength

        # Verificar se propriedade existe no modelo
        if (-not $modelo.Properties.ContainsKey($columnName)) {
            $discrepancias += [PSCustomObject]@{
                Tabela = $tableName
                Coluna = $columnName
                TipoProblema = "COLUNA_NAO_EXISTE_NO_MODELO"
                TipoBanco = "$sqlType (NULL=$isNullable)"
                TipoModelo = "N/A"
                Severidade = "ALTO"
                Acao = "Adicionar propriedade: public $expectedCSharpType $columnName { get; set; }"
            }
        }
        else {
            $propType = $modelo.Properties[$columnName].Type

            # Comparação de tipos (simplificada)
            $tipoCompativel = $true

            if ($expectedCSharpType -ne $propType) {
                # Verificar se é apenas diferença de nullable
                $baseExpected = $expectedCSharpType -replace '\?', ''
                $baseProp = $propType -replace '\?', ''

                if ($baseExpected -ne $baseProp) {
                    $tipoCompativel = $false
                }
            }

            if (-not $tipoCompativel) {
                $discrepancias += [PSCustomObject]@{
                    Tabela = $tableName
                    Coluna = $columnName
                    TipoProblema = "TIPO_INCOMPATIVEL"
                    TipoBanco = "$expectedCSharpType (SQL: $sqlType)"
                    TipoModelo = $propType
                    Severidade = "CRITICO"
                    Acao = "Alterar tipo para: $expectedCSharpType"
                }
            }
        }
    }

    # Verificar propriedades do modelo que não existem no banco
    foreach ($propName in $modelo.Properties.Keys) {
        $existeNoBanco = $columns | Where-Object { $_.COLUMN_NAME -eq $propName }

        if (-not $existeNoBanco) {
            $discrepancias += [PSCustomObject]@{
                Tabela = $tableName
                Coluna = $propName
                TipoProblema = "PROPRIEDADE_NAO_EXISTE_NO_BANCO"
                TipoBanco = "N/A"
                TipoModelo = $modelo.Properties[$propName].Type
                Severidade = "MEDIO"
                Acao = "Adicionar atributo [NotMapped] se for propriedade calculada"
            }
        }
    }
}

# Verificar modelos órfãos (sem tabela correspondente)
foreach ($modelName in $modelosC.Keys) {
    $existeNoBanco = $bancoTables | Where-Object { $_.Name -eq $modelName }

    if (-not $existeNoBanco) {
        $modelosOrfaos += [PSCustomObject]@{
            Modelo = $modelName
            Path = $modelosC[$modelName].Path
            TotalPropriedades = $modelosC[$modelName].Properties.Count
        }
    }
}

Write-Host "      Discrepâncias encontradas: $($discrepancias.Count)" -ForegroundColor $(if ($discrepancias.Count -gt 0) { "Red" } else { "Green" })
Write-Host "      Tabelas órfãs (sem modelo): $($tabelasOrfas.Count)" -ForegroundColor Yellow
Write-Host "      Modelos órfãos (sem tabela): $($modelosOrfaos.Count)" -ForegroundColor Yellow

# ========================================
# GERAR RELATÓRIOS
# ========================================

Write-Host "`n[4/4] Gerando relatórios..." -ForegroundColor Yellow

# ========================================
# RELATÓRIO 1: AUDITORIA COMPLETA
# ========================================

$auditoria = @"
# AUDITORIA BANCO REAL VS MODELOS C#
**Data:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")
**Banco de Dados:** SQL Server - Frotix
**Total de Tabelas no Banco:** $($bancoTables.Count)
**Total de Modelos C# Encontrados:** $($modelosC.Count)

---

## RESUMO EXECUTIVO

| Métrica | Valor |
|---------|-------|
| **Total de Discrepâncias** | $($discrepancias.Count) |
| **Discrepâncias CRÍTICAS** | $(($discrepancias | Where-Object { $_.Severidade -eq 'CRITICO' }).Count) |
| **Discrepâncias ALTAS** | $(($discrepancias | Where-Object { $_.Severidade -eq 'ALTO' }).Count) |
| **Discrepâncias MÉDIAS** | $(($discrepancias | Where-Object { $_.Severidade -eq 'MEDIO' }).Count) |
| **Tabelas Órfãs (sem modelo)** | $($tabelasOrfas.Count) |
| **Modelos Órfãos (sem tabela)** | $($modelosOrfaos.Count) |

---

## DISCREPÂNCIAS POR TABELA

"@

# Agrupar discrepâncias por tabela
$discrepanciasPorTabela = $discrepancias | Group-Object -Property Tabela

foreach ($grupo in $discrepanciasPorTabela) {
    $tabela = $grupo.Name
    $problemas = $grupo.Group

    $auditoria += @"

### $tabela
**Total de Problemas:** $($problemas.Count)

| Coluna | Problema | Tipo Banco | Tipo Modelo | Severidade | Ação |
|--------|----------|------------|-------------|------------|------|
"@

    foreach ($p in $problemas) {
        $auditoria += "`n| $($p.Coluna) | $($p.TipoProblema) | $($p.TipoBanco) | $($p.TipoModelo) | **$($p.Severidade)** | $($p.Acao) |"
    }

    $auditoria += "`n"
}

$auditoria += @"

---

## TABELAS SEM MODELO C# CORRESPONDENTE

| Tabela | Total de Colunas |
|--------|------------------|
"@

foreach ($tabela in $tabelasOrfas) {
    $auditoria += "`n| $($tabela.Tabela) | $($tabela.TotalColunas) |"
}

$auditoria += @"


---

## MODELOS C# SEM TABELA NO BANCO

| Modelo | Total de Propriedades | Caminho |
|--------|----------------------|---------|
"@

foreach ($modelo in $modelosOrfaos) {
    $auditoria += "`n| $($modelo.Modelo) | $($modelo.TotalPropriedades) | $($modelo.Path) |"
}

$auditoria += @"


---

## OBSERVAÇÕES

- **CRÍTICO:** Incompatibilidade de tipos - pode causar erros em runtime
- **ALTO:** Coluna no banco mas não no modelo - dados não serão carregados
- **MÉDIO:** Propriedade no modelo mas não no banco - pode causar erros ao salvar
- Tabelas prefixadas com `AspNet`, `tr_`, `sysdiagrams`, `Perfis`, `Usuarios` foram ignoradas (tabelas de sistema)

---

**Gerado automaticamente em:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")
"@

$auditoriaPath = Join-Path $OutputPath "AUDITORIA_BANCO_REAL_VS_MODELOS.md"
$auditoria | Out-File -FilePath $auditoriaPath -Encoding UTF8
Write-Host "      ✓ Gerado: AUDITORIA_BANCO_REAL_VS_MODELOS.md" -ForegroundColor Green

# ========================================
# RELATÓRIO 2: CORREÇÕES PRÁTICAS
# ========================================

$correcoes = @"
# CORREÇÕES MODELOS C# - SINCRONIZAÇÃO COM BANCO REAL
**Data:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")

---

## PRIORIDADE CRÍTICA - TIPOS INCOMPATÍVEIS

"@

$criticosCount = 0
foreach ($d in ($discrepancias | Where-Object { $_.Severidade -eq 'CRITICO' } | Sort-Object Tabela, Coluna)) {
    $criticosCount++
    $correcoes += @"

### $criticosCount. $($d.Tabela).$($d.Coluna)
**Problema:** $($d.TipoProblema)
**Tipo no Banco:** ``$($d.TipoBanco)``
**Tipo no Modelo:** ``$($d.TipoModelo)``

**Ação:**
``````csharp
// ANTES
public $($d.TipoModelo) $($d.Coluna) { get; set; }

// DEPOIS
public $($d.TipoBanco.Split(' ')[0]) $($d.Coluna) { get; set; }
``````

---
"@
}

$correcoes += @"

## PRIORIDADE ALTA - COLUNAS NÃO MAPEADAS

"@

$altosCount = 0
foreach ($d in ($discrepancias | Where-Object { $_.Severidade -eq 'ALTO' } | Sort-Object Tabela, Coluna)) {
    $altosCount++
    $correcoes += @"

### $altosCount. $($d.Tabela).$($d.Coluna)
**Problema:** $($d.TipoProblema)
**Tipo no Banco:** ``$($d.TipoBanco)``

**Ação:**
``````csharp
$($d.Acao)
``````

---
"@
}

$correcoes += @"

## PRIORIDADE MÉDIA - PROPRIEDADES EXTRAS NO MODELO

"@

$mediosCount = 0
foreach ($d in ($discrepancias | Where-Object { $_.Severidade -eq 'MEDIO' } | Sort-Object Tabela, Coluna)) {
    $mediosCount++
    $correcoes += @"

### $mediosCount. $($d.Tabela).$($d.Coluna)
**Problema:** $($d.TipoProblema)
**Tipo no Modelo:** ``$($d.TipoModelo)``

**Ação:**
``````csharp
[NotMapped]
public $($d.TipoModelo) $($d.Coluna) { get; set; }
``````

---
"@
}

$correcoesPath = Join-Path $OutputPath "CORRECOES_MODELOS_CSHARP_BANCO_REAL.md"
$correcoes | Out-File -FilePath $correcoesPath -Encoding UTF8
Write-Host "      ✓ Gerado: CORRECOES_MODELOS_CSHARP_BANCO_REAL.md" -ForegroundColor Green

# ========================================
# RELATÓRIO 3: RESUMO EXECUTIVO
# ========================================

$resumo = @"
# RELATÓRIO DE SINCRONIZAÇÃO - BANCO REAL vs MODELOS C#
**Data:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")

---

## VISÃO GERAL

| Métrica | Valor | Status |
|---------|-------|--------|
| **Tabelas no Banco** | $($bancoTables.Count) | ✓ |
| **Modelos C# Encontrados** | $($modelosC.Count) | $(if ($modelosC.Count -ge 50) { '✓' } else { '⚠' }) |
| **Discrepâncias Totais** | $($discrepancias.Count) | $(if ($discrepancias.Count -eq 0) { '✓' } else { '❌' }) |
| **Tabelas Órfãs** | $($tabelasOrfas.Count) | $(if ($tabelasOrfas.Count -eq 0) { '✓' } else { '⚠' }) |
| **Modelos Órfãos** | $($modelosOrfaos.Count) | $(if ($modelosOrfaos.Count -eq 0) { '✓' } else { '⚠' }) |

---

## DISTRIBUIÇÃO DE SEVERIDADE

``````
CRÍTICO (Tipos Incompatíveis): $(($discrepancias | Where-Object { $_.Severidade -eq 'CRITICO' }).Count)
ALTO (Colunas Não Mapeadas): $(($discrepancias | Where-Object { $_.Severidade -eq 'ALTO' }).Count)
MÉDIO (Propriedades Extras): $(($discrepancias | Where-Object { $_.Severidade -eq 'MEDIO' }).Count)
``````

---

## TOP 10 TABELAS COM MAIS PROBLEMAS

| Posição | Tabela | Total de Problemas |
|---------|--------|--------------------|
"@

$top10 = $discrepanciasPorTabela | Sort-Object Count -Descending | Select-Object -First 10
$pos = 1
foreach ($t in $top10) {
    $resumo += "`n| $pos | $($t.Name) | $($t.Count) |"
    $pos++
}

$resumo += @"


---

## RECOMENDAÇÕES

"@

if (($discrepancias | Where-Object { $_.Severidade -eq 'CRITICO' }).Count -gt 0) {
    $resumo += "`n- **[URGENTE]** Corrigir incompatibilidades de tipo (CRITICO) imediatamente"
}

if (($discrepancias | Where-Object { $_.Severidade -eq 'ALTO' }).Count -gt 0) {
    $resumo += "`n- **[IMPORTANTE]** Mapear colunas faltantes nos modelos C#"
}

if (($discrepancias | Where-Object { $_.Severidade -eq 'MEDIO' }).Count -gt 0) {
    $resumo += "`n- **[REVISAR]** Adicionar [NotMapped] em propriedades calculadas"
}

if ($tabelasOrfas.Count -gt 0) {
    $resumo += "`n- **[ANALISE]** Verificar se tabelas orfas precisam de modelos C#"
}

if ($modelosOrfaos.Count -gt 0) {
    $resumo += "`n- **[LIMPEZA]** Modelos orfaos devem ter [NotMapped] ou ser removidos"
}

$resumo += @"


---

## PRÓXIMOS PASSOS

1. Revisar arquivo ``AUDITORIA_BANCO_REAL_VS_MODELOS.md`` para detalhes completos
2. Aplicar correções do arquivo ``CORRECOES_MODELOS_CSHARP_BANCO_REAL.md``
3. Testar aplicação após correções
4. Re-executar este script para validar

---

**Gerado automaticamente em:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")
"@

$resumoPath = Join-Path $OutputPath "RELATORIO_SINCRONIZACAO_BANCO_REAL.md"
$resumo | Out-File -FilePath $resumoPath -Encoding UTF8
Write-Host "      ✓ Gerado: RELATORIO_SINCRONIZACAO_BANCO_REAL.md" -ForegroundColor Green

# ========================================
# FINALIZAÇÃO
# ========================================

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "AUDITORIA CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Arquivos gerados em: $OutputPath" -ForegroundColor White
Write-Host "  - AUDITORIA_BANCO_REAL_VS_MODELOS.md" -ForegroundColor White
Write-Host "  - CORRECOES_MODELOS_CSHARP_BANCO_REAL.md" -ForegroundColor White
Write-Host "  - RELATORIO_SINCRONIZACAO_BANCO_REAL.md" -ForegroundColor White
Write-Host ""
