# Auditoria Banco SQL vs Modelos C#
# Versao: 2.0 (Simplificada)

$SchemaCsv = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts\schema_banco_real.csv"
$ModelsPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Models"
$OutputPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site.OLD\Scripts"

Write-Host "Iniciando auditoria..." -ForegroundColor Cyan

# Carregar schema do banco
$schema = Import-Csv $SchemaCsv -Header "TABLE","COLUMN","TYPE","MAXLEN","NULLABLE","DEFAULT"
$tables = $schema | Group-Object -Property TABLE

Write-Host "Total de tabelas: $($tables.Count)" -ForegroundColor Green

# Mapear tipos SQL para C#
function Get-CSharpType {
    param($SqlType, $IsNullable)

    $map = @{
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
        'float' = 'double'
        'decimal' = 'decimal'
        'varbinary' = 'byte[]'
        'image' = 'byte[]'
        'timestamp' = 'byte[]'
        'char' = 'string'
        'datetimeoffset' = 'DateTimeOffset'
    }

    $type = $map[$SqlType]
    if (-not $type) { $type = "object" }

    if ($IsNullable -eq 'YES' -and $type -ne 'string' -and $type -ne 'byte[]') {
        $type += '?'
    }

    return $type
}

# Parsear modelos C#
function Parse-Model {
    param($Path)

    $content = Get-Content $Path -Raw
    $props = @{}

    # Regex simples para propriedades
    $regex = 'public\s+(\S+\??)\s+(\w+)\s*\{\s*get;\s*set;\s*\}'
    $matches = [regex]::Matches($content, $regex)

    foreach ($m in $matches) {
        $props[$m.Groups[2].Value] = $m.Groups[1].Value
    }

    return $props
}

# Carregar modelos C#
$models = @{}
$csFiles = Get-ChildItem -Path $ModelsPath -Filter "*.cs" -Recurse

foreach ($file in $csFiles) {
    $name = [IO.Path]::GetFileNameWithoutExtension($file.Name)

    # Ignorar DTOs, ViewModels, etc
    if ($name -match 'Dto$|ViewModel$|Model$|Extensions$') { continue }

    $models[$name] = @{
        Path = $file.FullName
        Props = Parse-Model -Path $file.FullName
    }
}

Write-Host "Total de modelos: $($models.Count)" -ForegroundColor Green

# Analise
$issues = @()

foreach ($tblGroup in $tables) {
    $tblName = $tblGroup.Name
    $columns = $tblGroup.Group

    # Ignorar tabelas de sistema
    if ($tblName -match '^AspNet|^tr_|^sysdiagrams|^Perfis|^Usuarios') { continue }

    # Verificar se modelo existe
    if (-not $models.ContainsKey($tblName)) {
        $issues += [PSCustomObject]@{
            Tabela = $tblName
            Coluna = "N/A"
            Problema = "TABELA_SEM_MODELO"
            Severidade = "BAIXO"
            TipoBanco = "N/A"
            TipoModelo = "N/A"
        }
        continue
    }

    $model = $models[$tblName]

    # Comparar colunas
    foreach ($col in $columns) {
        $colName = $col.COLUMN
        $sqlType = $col.TYPE
        $nullable = $col.NULLABLE

        $expectedType = Get-CSharpType -SqlType $sqlType -IsNullable $nullable

        # Verificar se propriedade existe
        if (-not $model.Props.ContainsKey($colName)) {
            $issues += [PSCustomObject]@{
                Tabela = $tblName
                Coluna = $colName
                Problema = "COLUNA_NAO_MAPEADA"
                Severidade = "ALTO"
                TipoBanco = $expectedType
                TipoModelo = "N/A"
            }
        }
        else {
            $propType = $model.Props[$colName]

            # Comparar tipos (simplificado)
            $baseExpected = $expectedType -replace '\?', ''
            $baseProp = $propType -replace '\?', ''

            if ($baseExpected -ne $baseProp) {
                $issues += [PSCustomObject]@{
                    Tabela = $tblName
                    Coluna = $colName
                    Problema = "TIPO_INCOMPATIVEL"
                    Severidade = "CRITICO"
                    TipoBanco = $expectedType
                    TipoModelo = $propType
                }
            }
        }
    }

    # Verificar propriedades extras
    foreach ($propName in $model.Props.Keys) {
        $exists = $columns | Where-Object { $_.COLUMN -eq $propName }

        if (-not $exists) {
            $issues += [PSCustomObject]@{
                Tabela = $tblName
                Coluna = $propName
                Problema = "PROPRIEDADE_EXTRA"
                Severidade = "MEDIO"
                TipoBanco = "N/A"
                TipoModelo = $model.Props[$propName]
            }
        }
    }
}

Write-Host "Total de problemas: $($issues.Count)" -ForegroundColor Yellow

# Exportar para CSV
$csvPath = Join-Path $OutputPath "analise_discrepancias.csv"
$issues | Export-Csv -Path $csvPath -NoTypeInformation -Encoding UTF8

Write-Host "Arquivo gerado: $csvPath" -ForegroundColor Green

# Estatisticas
$criticos = ($issues | Where-Object { $_.Severidade -eq 'CRITICO' }).Count
$altos = ($issues | Where-Object { $_.Severidade -eq 'ALTO' }).Count
$medios = ($issues | Where-Object { $_.Severidade -eq 'MEDIO' }).Count

Write-Host "`nEstatisticas:" -ForegroundColor Cyan
Write-Host "  CRITICO: $criticos" -ForegroundColor Red
Write-Host "  ALTO: $altos" -ForegroundColor Yellow
Write-Host "  MEDIO: $medios" -ForegroundColor Yellow
