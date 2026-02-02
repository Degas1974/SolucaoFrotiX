/* ****************************************************************************************
 * ⚡ ARQUIVO: ocorrencia-viagem-popup.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Modal de alerta automático exibido ao selecionar veículo em formulários de viagem.
 *    Verifica via API se existe alguma ocorrência aberta (Acidente/Defeito/Multa) vinculada
 *    ao veículo. Se sim: exibe modal informativo com botão "Ver Ocorrências Abertas" (link
 *    para página de ocorrências filtrada) e botão "Prosseguir". Previne uso inadvertido de
 *    veículo com problemas ativos. Pattern Revealing Module (IIFE).
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - verificar(veiculoId, veiculoDescricao, callback)
 *       • veiculoId: GUID do veículo selecionado
 *       • veiculoDescricao: texto ex "ABC-1234 - Ford Fiesta 2020"
 *       • callback: função a executar após fechar modal (ex: continuar preenchimento form)
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - Modal Bootstrap 5: título "⚠️ Ocorrências Abertas", badge contador vermelho (ex: 3)
 *    - Corpo: mensagem "Este veículo possui X ocorrência(s) aberta(s)"
 *    - Botões: "Ver Ocorrências Abertas" (redirect) + "Prosseguir Mesmo Assim" (dismiss)
 *    - Callback executado ao fechar modal (permite ou cancela ação)
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: jQuery 3.x, Bootstrap 5.x (Modal API)
 *    • ARQUIVOS FROTIX: FrotiX.css (badges, modal custom)
 *    • API: /api/OcorrenciaViagem/ContarAbertasPorVeiculo?veiculoId={guid} (GET) → { success, count }
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (3 funções públicas + 2 privadas)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🔧 PÚBLICAS (exports)                                                                    │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • verificar(veiculoId, veiculoDescricao, callback) → Entry point, faz GET e decide modal│
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔒 PRIVADAS                                                                              │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • mostrarPopup(veiculoId, veiculoDescricao, count, callback) → Gera HTML modal + show   │
 * │ • construirHtmlModal(veiculoDescricao, count, callback) → Template HTML modal Bootstrap │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Veículo com ocorrências abertas
 *    Select veículo ABC-1234 → evento change → chamar OcorrenciaViagemPopup.verificar(veiculoId, descricao, callback)
 *      → GET /api/OcorrenciaViagem/ContarAbertasPorVeiculo?veiculoId={guid}
 *      → Response: { success: true, count: 3 }
 *      → mostrarPopup(veiculoId, descricao, 3, callback)
 *         → construirHtmlModal() → template com badge "3", mensagem, botões
 *         → Inject HTML em body
 *         → Bootstrap Modal show
 *      → User escolhe:
 *         • Click "Ver Ocorrências Abertas" → redirect /OcorrenciaViagem?veiculoId={guid}
 *         • Click "Prosseguir Mesmo Assim" → fecha modal → executa callback() → continua form
 * 
 * 💡 FLUXO 2: Veículo sem ocorrências (normal)
 *    Select veículo DEF-5678 → OcorrenciaViagemPopup.verificar(veiculoId, descricao, callback)
 *      → GET /api/OcorrenciaViagem/ContarAbertasPorVeiculo?veiculoId={guid}
 *      → Response: { success: true, count: 0 }
 *      → NÃO exibe modal (skip)
 *      → Executa callback() imediatamente → continua form normalmente
 * 
 * 💡 FLUXO 3: VeiculoId inválido (00000000-0000...) ou null
 *    Select dropdown placeholder → OcorrenciaViagemPopup.verificar(null, null, callback)
 *      → Validação: if (!veiculoId || veiculoId === '00000000-0000-0000-0000-000000000000')
 *      → Executa callback() imediatamente SEM fazer API call
 *      → Não exibe modal
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * 🎨 MODAL VISUAL:
 *    - Header: badge-danger com contador vermelho (ex: "3"), ícone ⚠️, título "Ocorrências Abertas"
 *    - Body: mensagem "Este veículo possui X ocorrência(s) aberta(s). Deseja visualizá-las antes de prosseguir?"
 *    - Footer 2 botões:
 *       • Btn primário (azul): "Ver Ocorrências Abertas" → href="/OcorrenciaViagem?veiculoId={guid}"
 *       • Btn secondary (cinza): "Prosseguir Mesmo Assim" → data-bs-dismiss="modal"
 * 
 * 🔒 SEGURANÇA:
 *    - Valida veiculoId não nulo e diferente de GUID vazio (00000000-0000-0000-0000-000000000000)
 *    - Backend API valida permissões (usuário só vê ocorrências do próprio setor)
 *    - Não exibe detalhes das ocorrências no popup (apenas contador)
 * 
 * ⚡ CALLBACK PATTERN:
 *    - Função callback opcional (3º parâmetro)
 *    - Chamada quando:
 *       • Modal NÃO precisa ser exibido (count = 0 ou veiculoId inválido)
 *       • User clica "Prosseguir Mesmo Assim" (evento modal hidden.bs.modal)
 *    - Permite retomar fluxo normal do formulário pai
 * 
 * 🗑️ AUTO-DESTROY MODAL:
 *    - Modal removido do DOM após hidden.bs.modal (limpa memória)
 *    - $('#modalOcorrenciasAbertas').remove() no evento hidden
 * 
 * 🎯 CASOS DE USO:
 *    - Formulário Agendamento Viagem (selecionar veículo)
 *    - Formulário Inserir Viagem (selecionar veículo)
 *    - Formulário Manutenção (selecionar veículo para manutenção preventiva)
 *    - Qualquer tela onde seleção de veículo deve alertar sobre problemas ativos
 * 
 * 📦 PATTERN REVEALING MODULE:
 *    - IIFE: var OcorrenciaViagemPopup = (function () { ... return { verificar }; })();
 *    - Export público: verificar
 *    - Funções privadas: mostrarPopup, construirHtmlModal (não acessíveis fora do módulo)
 * 
 * **************************************************************************************** */

// =====================================================
// OCORRENCIA-VIAGEM-POPUP.JS
// Popup de ocorrências abertas ao selecionar veículo
// =====================================================

var OcorrenciaViagemPopup = (function () {

    function verificar(veiculoId, veiculoDescricao, callback) {
        if (!veiculoId || veiculoId === '00000000-0000-0000-0000-000000000000') {
            if (callback) callback();
            return;
        }

        $.get('/api/OcorrenciaViagem/ContarAbertasPorVeiculo', { veiculoId: veiculoId }, function (res) {
            if (res.success && res.count > 0) {
                mostrarPopup(veiculoId, veiculoDescricao, res.count, callback);
            } else {
                if (callback) callback();
            }
        });
    }

    function mostrarPopup(veiculoId, veiculoDescricao, count, callback) {
        var modalHtml = `
            <div class="modal fade" id="modalOcorrenciasAbertas" tabindex="-1">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header bg-warning">
                            <h5 class="modal-title">
                                <i class="fa fa-exclamation-triangle me-2"></i>
                                Ocorrências Abertas - ${veiculoDescricao}
                            </h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                        </div>
                        <div class="modal-body">
                            <div class="alert alert-warning">
                                <strong>Atenção!</strong> Este veículo possui <strong>${count}</strong> ocorrência(s) em aberto.
                            </div>
                            <div id="listaOcorrenciasAbertas">
                                <div class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button>
                            <button type="button" class="btn btn-primary" id="btnContinuarComOcorrencias">
                                Continuar mesmo assim
                            </button>
                        </div>
                    </div>
                </div>
            </div>`;

        $('body').append(modalHtml);
        var modal = new bootstrap.Modal(document.getElementById('modalOcorrenciasAbertas'));
        modal.show();

        carregarOcorrencias(veiculoId);

        $('#btnContinuarComOcorrencias').on('click', function () {
            modal.hide();
            if (callback) callback();
        });

        $('#modalOcorrenciasAbertas').on('hidden.bs.modal', function () {
            $(this).remove();
        });
    }

    function carregarOcorrencias(veiculoId) {
        $.get('/api/OcorrenciaViagem/ListarAbertasPorVeiculo', { veiculoId: veiculoId }, function (res) {
            if (res.success) {
                var html = '';
                res.data.forEach(function (oc) {
                    html += criarItemOcorrencia(oc);
                });
                $('#listaOcorrenciasAbertas').html(html || '<p class="text-muted">Nenhuma ocorrência encontrada.</p>');
            }
        });
    }

    function criarItemOcorrencia(oc) {
        var badgeClass = 'bg-secondary';
        if (oc.urgencia === 'Crítica') badgeClass = 'bg-danger';
        else if (oc.urgencia === 'Alta') badgeClass = 'bg-warning text-dark';
        else if (oc.urgencia === 'Média') badgeClass = 'bg-info';

        return `
            <div class="card mb-2 border-start border-warning border-4">
                <div class="card-body p-2">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <strong>${oc.resumo}</strong>
                            <br><small class="text-muted">Ficha: ${oc.noFichaVistoria || 'N/A'} | ${oc.dataCriacao}</small>
                        </div>
                        <div>
                            <span class="badge ${badgeClass}">${oc.urgencia} (${oc.diasEmAberto} dias)</span>
                            <button class="btn btn-sm btn-success ms-1" onclick="OcorrenciaViagemPopup.darBaixa('${oc.ocorrenciaViagemId}')">
                                <i class="fa fa-check"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>`;
    }

    function darBaixa(ocorrenciaId) {
        if (!confirm('Confirma dar baixa nesta ocorrência?')) return;

        $.post('/api/OcorrenciaViagem/DarBaixa', { ocorrenciaId: ocorrenciaId }, function (res) {
            if (res.success) {
                AppToast.show('Verde', 'Ocorrência baixada!', 2000);
                var veiculoId = $('#modalOcorrenciasAbertas').data('veiculo-id');
                carregarOcorrencias(veiculoId);
            } else {
                AppToast.show('Vermelho', res.message, 3000);
            }
        });
    }

    return {
        verificar: verificar,
        darBaixa: darBaixa
    };
})();
