# Script de Monitoramento de Progresso - Documentação FrotiX
# Atualiza a cada 10 segundos
# Pressione Ctrl+C para sair

$docFile = "FrotiX.Site/DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md"
$totalArquivos = 924

function Get-ArquivosDocumentados {
    if (Test-Path $docFile) {
        $content = Get-Content $docFile -Raw
        $matches = ([regex]::Matches($content, "^\- \[x\]", [System.Text.RegularExpressions.RegexOptions]::Multiline)).Count
        return $matches
    }
    return 0
}

function Show-TabelaProgresso {
    param($atual)

    Clear-Host

    $percentualAtual = [math]::Round(($atual / $totalArquivos) * 100, 2)

    Write-Host ""
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host "    PROGRESSO DOCUMENTAÇÃO FROTIX" -ForegroundColor Yellow
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host ""

    # Tabela de marcos
    $marcos = @(
        @{Nome="5%";   Arquivos=46;  Percentual=5},
        @{Nome="10%";  Arquivos=92;  Percentual=10},
        @{Nome="15%";  Arquivos=138; Percentual=15},
        @{Nome="20%";  Arquivos=185; Percentual=20},
        @{Nome="25%";  Arquivos=231; Percentual=25},
        @{Nome="50%";  Arquivos=462; Percentual=50},
        @{Nome="75%";  Arquivos=693; Percentual=75},
        @{Nome="100%"; Arquivos=924; Percentual=100}
    )

    Write-Host " Marco      Arquivos     Status" -ForegroundColor White
    Write-Host "───────────────────────────────────────────────────" -ForegroundColor DarkGray

    foreach ($marco in $marcos) {
        $nomeFormatado = $marco.Nome.PadRight(8)
        $arquivosFormatado = "$($marco.Arquivos)/$totalArquivos".PadRight(12)

        if ($atual -ge $marco.Arquivos) {
            Write-Host " $nomeFormatado $arquivosFormatado " -NoNewline -ForegroundColor White
            Write-Host "✅ Concluído" -ForegroundColor Green
        } elseif ($percentualAtual -ge ($marco.Percentual - 5) -and $percentualAtual -lt $marco.Percentual) {
            Write-Host " $nomeFormatado $arquivosFormatado " -NoNewline -ForegroundColor Yellow
            Write-Host "⏳ Em progresso" -ForegroundColor Yellow
        } else {
            Write-Host " $nomeFormatado $arquivosFormatado " -NoNewline -ForegroundColor DarkGray
            Write-Host "⚪ Pendente" -ForegroundColor DarkGray
        }
    }

    Write-Host ""
    Write-Host "───────────────────────────────────────────────────" -ForegroundColor DarkGray
    Write-Host " ATUAL      $atual/$totalArquivos".PadRight(22) -NoNewline -ForegroundColor Cyan
    Write-Host "🎯 $percentualAtual%" -ForegroundColor Cyan
    Write-Host "═══════════════════════════════════════════════════════════════" -ForegroundColor Cyan
    Write-Host ""
    Write-Host " Atualização a cada 10 segundos..." -ForegroundColor DarkGray
    Write-Host " Pressione Ctrl+C para sair" -ForegroundColor DarkGray
    Write-Host ""
}

# Loop principal
try {
    while ($true) {
        $arquivosDocumentados = Get-ArquivosDocumentados
        Show-TabelaProgresso -atual $arquivosDocumentados
        Start-Sleep -Seconds 10
    }
} catch {
    Write-Host ""
    Write-Host "Monitoramento encerrado." -ForegroundColor Yellow
    Write-Host ""
}
