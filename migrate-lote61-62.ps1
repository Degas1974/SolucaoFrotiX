# MIGRAÇÃO LOTES 61-62: 22 arquivos
$source = "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site"
$target = "c:/FrotiX/Solucao FrotiX 2026/FrotiX.Site.Backup"

$arquivos = @(
    "Tools/DocGenerator/Program.cs",
    "wwwroot/js/agendamento/components/calendario.js",
    "wwwroot/js/agendamento/components/controls-init.js", 
    "wwwroot/js/agendamento/components/dialogs.js",
    "wwwroot/js/agendamento/components/event-handlers.js",
    "wwwroot/js/agendamento/components/evento.js",
    "wwwroot/js/agendamento/components/exibe-viagem.js",
    "wwwroot/js/agendamento/components/modal-config.js",
    "wwwroot/js/agendamento/components/modal-viagem-novo.js",
    "wwwroot/js/agendamento/components/recorrencia-init.js",
    "wwwroot/js/agendamento/components/recorrencia-logic.js",
    "wwwroot/js/agendamento/components/recorrencia.js",
    "wwwroot/js/agendamento/components/relatorio.js",
    "wwwroot/js/agendamento/components/reportviewer-close-guard.js",
    "wwwroot/js/agendamento/components/sweetalert_interop.patch.js",
    "wwwroot/js/agendamento/components/validacao.js",
    "wwwroot/js/agendamento/utils/calendario-config.js",
    "wwwroot/js/agendamento/utils/date.utils.js",
    "wwwroot/js/agendamento/utils/formatters.js",
    "wwwroot/js/agendamento/utils/kendo-datetime.js",
    "wwwroot/js/agendamento/utils/kendo-editor-helper.js",
    "wwwroot/js/agendamento/utils/syncfusion.utils.js",
    "wwwroot/js/agendamento/main.js",
    "wwwroot/js/agendamento/core/ajax-helper.js",
    "wwwroot/js/agendamento/core/api-client.js",
    "wwwroot/js/agendamento/core/state.js",
    "wwwroot/js/agendamento/services/agendamento.service.js",
    "wwwroot/js/agendamento/services/evento.service.js",
    "wwwroot/js/agendamento/services/requisitante.service.js",
    "wwwroot/js/agendamento/services/viagem.service.js"
)

$pulados = 0
$migrados = 0
$erros = 0

Write-Host "`n📋 MIGRAÇÃO LOTES 61-62 ($($arquivos.Count) arquivos)" -ForegroundColor Cyan
Write-Host "=" * 70 -ForegroundColor Cyan

foreach ($arq in $arquivos) {
    try {
        $srcPath = Join-Path $source $arq
        $tgtPath = Join-Path $target $arq
        
        if (-not (Test-Path $srcPath)) {
            Write-Host "  ⚠️ Pulado (SOURCE inexistente): $arq" -ForegroundColor Yellow
            $pulados++
            continue
        }
        
        # Garantir diretório TARGET
        $tgtDir = Split-Path -Parent $tgtPath
        if (-not (Test-Path $tgtDir)) {
            New-Item -ItemType Directory -Path $tgtDir -Force | Out-Null
        }
        
        # Verificar se é necessário copiar
        if (Test-Path $tgtPath) {
            $srcHash = (Get-FileHash -Path $srcPath -Algorithm MD5).Hash
            $tgtHash = (Get-FileHash -Path $tgtPath -Algorithm MD5).Hash
            
            if ($srcHash -eq $tgtHash) {
                Write-Host "  ⏭️ Pulado (idêntico): $arq" -ForegroundColor DarkGray
                $pulados++
                continue
            }
        }
        
        # Copiar arquivo
        Copy-Item -Path $srcPath -Destination $tgtPath -Force
        Write-Host "  ✅ Migrado: $arq" -ForegroundColor Green
        $migrados++
        
    } catch {
        Write-Host "  ❌ ERRO: $arq - $($_.Exception.Message)" -ForegroundColor Red
        $erros++
    }
}

Write-Host "`n" + ("=" * 70) -ForegroundColor Cyan
$cor = if ($erros -eq 0) {"Green"} else {"Red"}
Write-Host "📊 LOTES 61-62: Pulados: $pulados | Migrados: $migrados | Preservados: 0 | Erros: $erros" -ForegroundColor $cor
Write-Host "=" * 70 -ForegroundColor Cyan
