# ============================================================================
# SCRIPT DE ANÁLISE COMPLETA DO BANCO DE DADOS FROTIX
# ============================================================================
# Analisa o arquivo Frotix.sql para identificar:
# 1. Todas as tabelas e seus campos
# 2. Foreign Keys existentes
# 3. Foreign Keys faltantes (baseado em convenções de nomes)
# 4. Índices faltantes (campos frequentemente usados em WHERE/JOIN)
# ============================================================================

param(
    [string]$SqlFile = "D:\FrotiX\Solucao FrotiX 2026\FrotiX.Site\Frotix.sql",
    [string]$OutputDir = "D:\FrotiX\Solucao FrotiX 2026\FrotiX.Site\Analises"
)

$ErrorActionPreference = "Stop"

# Criar diretório de saída se não existir
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "ANÁLISE COMPLETA DO BANCO DE DADOS FROTIX" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host ""

# Ler arquivo SQL
Write-Host "[1/6] Lendo arquivo SQL..." -ForegroundColor Yellow
$sqlContent = Get-Content $SqlFile -Raw
Write-Host "✓ Arquivo lido: $((Get-Item $SqlFile).Length / 1MB | Out-String) MB" -ForegroundColor Green
Write-Host ""

# ============================================================================
# EXTRAIR TABELAS
# ============================================================================
Write-Host "[2/6] Extraindo tabelas..." -ForegroundColor Yellow

$tabelas = @{}
$tabelasMatches = [regex]::Matches($sqlContent, 'CREATE TABLE dbo\.(\w+)\s*\(((?:[^()]|\((?:[^()]|\([^()]*\))*\))*)\)', [System.Text.RegularExpressions.RegexOptions]::Singleline)

foreach ($match in $tabelasMatches) {
    $nomeTabela = $match.Groups[1].Value
    $camposBloco = $match.Groups[2].Value

    # Extrair campos individuais
    $campos = @()
    $lines = $camposBloco -split "`n" | Where-Object { $_ -match '^\s*\w+\s+\w+' }

    foreach ($line in $lines) {
        if ($line -match '^\s*(\w+)\s+([\w()]+).*?(NULL|NOT NULL)?') {
            $nomeCampo = $Matches[1]
            $tipoCampo = $Matches[2]

            # Identificar possíveis FKs por convenção
            $isPossibleFK = $false
            $tabelaReferenciada = $null

            if ($nomeCampo -match '(\w+)Id$' -and $nomeCampo -ne 'Id') {
                $isPossibleFK = $true
                $tabelaReferenciada = $Matches[1]
            }

            $campos += [PSCustomObject]@{
                Nome = $nomeCampo
                Tipo = $tipoCampo
                PossivelFK = $isPossibleFK
                TabelaReferenciada = $tabelaReferenciada
            }
        }
    }

    $tabelas[$nomeTabela] = $campos
}

Write-Host "✓ Extraídas $($tabelas.Count) tabelas" -ForegroundColor Green
Write-Host ""

# ============================================================================
# EXTRAIR FOREIGN KEYS EXISTENTES
# ============================================================================
Write-Host "[3/6] Extraindo Foreign Keys existentes..." -ForegroundColor Yellow

$fksExistentes = @()
$fkMatches = [regex]::Matches($sqlContent, 'ALTER TABLE dbo\.(\w+).*?ADD CONSTRAINT (\S+) FOREIGN KEY \((\w+)\) REFERENCES dbo\.(\w+)')

foreach ($match in $fkMatches) {
    $fksExistentes += [PSCustomObject]@{
        TabelaOrigem = $match.Groups[1].Value
        NomeConstraint = $match.Groups[2].Value
        CampoOrigem = $match.Groups[3].Value
        TabelaDestino = $match.Groups[4].Value
    }
}

Write-Host "✓ Encontradas $($fksExistentes.Count) Foreign Keys" -ForegroundColor Green
Write-Host ""

# ============================================================================
# IDENTIFICAR FKs FALTANTES
# ============================================================================
Write-Host "[4/6] Identificando Foreign Keys faltantes..." -ForegroundColor Yellow

$fksFaltantes = @()

foreach ($tabela in $tabelas.Keys) {
    $campos = $tabelas[$tabela]

    foreach ($campo in $campos | Where-Object { $_.PossivelFK }) {
        # Verificar se já existe FK para este campo
        $fkExiste = $fksExistentes | Where-Object {
            $_.TabelaOrigem -eq $tabela -and $_.CampoOrigem -eq $campo.Nome
        }

        if (-not $fkExiste) {
            # Verificar se tabela referenciada existe
            $tabelaRefExiste = $tabelas.ContainsKey($campo.TabelaReferenciada)

            $fksFaltantes += [PSCustomObject]@{
                TabelaOrigem = $tabela
                CampoOrigem = $campo.Nome
                TabelaDestino = $campo.TabelaReferenciada
                TabelaDestinoExiste = $tabelaRefExiste
                Prioridade = if ($tabelaRefExiste) { "Alta" } else { "Baixa" }
            }
        }
    }
}

Write-Host "✓ Identificadas $($fksFaltantes.Count) Foreign Keys faltantes" -ForegroundColor Green
Write-Host "  - Alta prioridade (tabela destino existe): $(($fksFaltantes | Where-Object { $_.Prioridade -eq 'Alta' }).Count)" -ForegroundColor Yellow
Write-Host "  - Baixa prioridade (tabela destino NÃO existe): $(($fksFaltantes | Where-Object { $_.Prioridade -eq 'Baixa' }).Count)" -ForegroundColor DarkGray
Write-Host ""

# ============================================================================
# ANALISAR ÍNDICES
# ============================================================================
Write-Host "[5/6] Analisando índices existentes..." -ForegroundColor Yellow

$indicesExistentes = @()
$indexMatches = [regex]::Matches($sqlContent, 'CREATE.*?INDEX (\S+)\s+ON dbo\.(\w+)\s*\(([^)]+)\)')

foreach ($match in $indexMatches) {
    $indicesExistentes += [PSCustomObject]@{
        NomeIndice = $match.Groups[1].Value
        Tabela = $match.Groups[2].Value
        Campos = $match.Groups[3].Value -replace '\s+', ''
    }
}

Write-Host "✓ Encontrados $($indicesExistentes.Count) índices" -ForegroundColor Green
Write-Host ""

# ============================================================================
# GERAR RELATÓRIOS
# ============================================================================
Write-Host "[6/6] Gerando relatórios..." -ForegroundColor Yellow

# Relatório de FKs Faltantes (Alta Prioridade)
$reportFKsAltaPrioridade = $OutputDir + "\FKs_Faltantes_AltaPrioridade.csv"
$fksFaltantes | Where-Object { $_.Prioridade -eq "Alta" } |
    Export-Csv -Path $reportFKsAltaPrioridade -NoTypeInformation -Encoding UTF8
Write-Host "✓ Relatório gerado: FKs_Faltantes_AltaPrioridade.csv" -ForegroundColor Green

# Relatório de todas as FKs Faltantes
$reportFKsTodasas = $OutputDir + "\FKs_Faltantes_Todas.csv"
$fksFaltantes | Export-Csv -Path $reportFKsTodasas -NoTypeInformation -Encoding UTF8
Write-Host "✓ Relatório gerado: FKs_Faltantes_Todas.csv" -ForegroundColor Green

# Relatório de Tabelas
$reportTabelas = $OutputDir + "\Tabelas_Resumo.txt"
$tabelasResumo = foreach ($tabela in $tabelas.Keys | Sort-Object) {
    $campos = $tabelas[$tabela]
    "$tabela - $($campos.Count) campos"
}
$tabelasResumo | Out-File -FilePath $reportTabelas -Encoding UTF8
Write-Host "✓ Relatório gerado: Tabelas_Resumo.txt" -ForegroundColor Green

Write-Host ""
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "RESUMO DA ANÁLISE" -ForegroundColor Cyan
Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host "Tabelas encontradas: $($tabelas.Count)" -ForegroundColor White
Write-Host "Foreign Keys existentes: $($fksExistentes.Count)" -ForegroundColor White
Write-Host "Foreign Keys faltantes (alta prioridade): $(($fksFaltantes | Where-Object { $_.Prioridade -eq 'Alta' }).Count)" -ForegroundColor Yellow
Write-Host "Índices existentes: $($indicesExistentes.Count)" -ForegroundColor White
Write-Host ""
Write-Host "Relatórios salvos em: $OutputDir" -ForegroundColor Green
Write-Host "============================================================================" -ForegroundColor Cyan
