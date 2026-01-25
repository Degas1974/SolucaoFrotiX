# =====================================================================================
# Script: AddAsTrackingToControllers.ps1
# Descrição: Adiciona .AsTracking() em queries do Entity Framework que usam
#            Update/Remove/SaveChanges nos Controllers
# Data: 19/01/2026
# =====================================================================================

# Caminho para a pasta de Controllers
$controllersPath = "C:\FrotiX\Solucao FrotiX 2026\FrotiX.Site\Controllers"

# Contador de arquivos modificados
$filesModified = 0
$totalCorrections = 0

# Lista para rastrear modificações
$modifications = @()

# Função para adicionar .AsTracking() antes de GetFirstOrDefault
function Add-AsTracking {
    param (
        [string]$FilePath
    )

    $content = Get-Content $FilePath -Raw -Encoding UTF8
    $originalContent = $content
    $fileModified = $false
    $correctionsInFile = 0

    # PADRÃO 1: _unitOfWork.Entity.GetFirstOrDefault seguido de Update/Remove
    # Regex: captura _unitOfWork.<NomeEntidade>.GetFirstOrDefault e adiciona .AsTracking() antes
    $pattern1 = '(_unitOfWork\.\w+)\.GetFirstOrDefault'

    # Processa cada match individualmente
    $matches = [regex]::Matches($content, $pattern1)

    foreach ($match in $matches) {
        $fullMatch = $match.Value
        $entityAccess = $match.Groups[1].Value

        # Verifica se já tem .AsTracking()
        $beforeMatch = $content.Substring(0, $match.Index)
        if ($beforeMatch -notmatch '\.AsTracking\(\)\s*$') {
            # Busca o contexto após o GetFirstOrDefault para verificar se há Update/Remove/SaveChanges
            $afterMatch = $content.Substring($match.Index + $match.Length, [Math]::Min(500, $content.Length - $match.Index - $match.Length))

            if ($afterMatch -match '(\.Update\(|\.Remove\(|_unitOfWork\.Save\(|_context\.SaveChanges\()') {
                # Adiciona .AsTracking() antes do GetFirstOrDefault
                $replacement = "$entityAccess.AsTracking().GetFirstOrDefault"
                $content = $content.Replace($fullMatch, $replacement)
                $fileModified = $true
                $correctionsInFile++
            }
        }
    }

    # PADRÃO 2: _context.Entity.Where seguido de RemoveRange/Update
    $pattern2 = '(_context\.\w+)\s*\n\s*\.Where'
    $matches2 = [regex]::Matches($content, $pattern2)

    foreach ($match in $matches2) {
        $fullMatch = $match.Value
        $contextAccess = $match.Groups[1].Value

        # Verifica se já tem .AsTracking()
        $beforeMatch = $content.Substring(0, $match.Index)
        if ($beforeMatch -notmatch '\.AsTracking\(\)\s*$') {
            # Busca o contexto após para verificar se há RemoveRange/SaveChanges
            $afterMatch = $content.Substring($match.Index + $match.Length, [Math]::Min(500, $content.Length - $match.Index - $match.Length))

            if ($afterMatch -match '(RemoveRange\(|\.SaveChanges\()') {
                # Adiciona .AsTracking() logo após _context.Entity
                $replacement = "$contextAccess`n                    .AsTracking()`n                    .Where"
                $content = $content.Replace($fullMatch, $replacement)
                $fileModified = $true
                $correctionsInFile++
            }
        }
    }

    # Se houve modificação, salva o arquivo
    if ($fileModified) {
        Set-Content -Path $FilePath -Value $content -Encoding UTF8 -NoNewline

        $script:filesModified++
        $script:totalCorrections += $correctionsInFile

        $fileName = Split-Path $FilePath -Leaf
        $script:modifications += [PSCustomObject]@{
            File = $fileName
            Corrections = $correctionsInFile
        }

        Write-Host "✓ $fileName - $correctionsInFile correção(ões)" -ForegroundColor Green
        return $true
    }

    return $false
}

# Processa todos os arquivos .cs na pasta Controllers
Write-Host "`n===================================================================" -ForegroundColor Cyan
Write-Host "  Iniciando correções - Adicionando .AsTracking()" -ForegroundColor Cyan
Write-Host "===================================================================" -ForegroundColor Cyan

Get-ChildItem -Path $controllersPath -Filter "*.cs" -Recurse | ForEach-Object {
    Add-AsTracking -FilePath $_.FullName
}

# Relatório final
Write-Host "`n===================================================================" -ForegroundColor Cyan
Write-Host "  RELATÓRIO FINAL" -ForegroundColor Cyan
Write-Host "===================================================================" -ForegroundColor Cyan
Write-Host "Arquivos modificados: $filesModified" -ForegroundColor Yellow
Write-Host "Total de correções: $totalCorrections" -ForegroundColor Yellow

if ($modifications.Count -gt 0) {
    Write-Host "`nDetalhamento por arquivo:" -ForegroundColor Cyan
    $modifications | ForEach-Object {
        Write-Host "  - $($_.File): $($_.Corrections) correção(ões)" -ForegroundColor White
    }
}

Write-Host "`n✓ Script concluído!" -ForegroundColor Green
