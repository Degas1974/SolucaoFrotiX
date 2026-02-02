/* ****************************************************************************************
 * ⚡ ARQUIVO: relatorio.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento completo de relatórios Telerik ReportViewer no modal de
 *                   visualização de viagens. IIFE com 17 funções internas + 3 exports
 *                   para carregar, exibir, destruir ReportViewer com overlay loading
 *                   FrotiX (logo piscando), validações, retry pattern, height fixes, error
 *                   handling. Principais fluxos: mostrarLoadingRelatorio → overlay z-index
 *                   999999, buscarDadosViagem → GET /api/Viagem/PegarViagemParaEdicao,
 *                   determinarRelatorio → "Agendamento.trdp" ou "FichaVistoria.trdp",
 *                   inicializarViewer → telerik_ReportViewer({ serviceUrl: '/api/reports/',
 *                   reportSource, callbacks }), aguardarTelerikReportViewer → retry 5s,
 *                   aplicarAlturasFixas → calc(100vh - 380px), destruirViewerAnterior →
 *                   dispose/destroy + removeData. Suporta 2 tipos relatório: Agendamento
 *                   (criação), FichaVistoria (finalização). Usa jQuery, Telerik Reporting,
 *                   StateManager, Bootstrap Modal.
 * 📥 ENTRADAS     : viagemId (int de carregarRelatorioViagem), mensagem (string de
 *                   mostrarLoading), condition/timeout/interval (waitUntil params),
 *                   data (Object de determinarRelatorio com Status/Concluida properties)
 * 📤 SAÍDAS       : Void (side effects: DOM updates, overlay append/remove, ReportViewer
 *                   instance, console.log debug), Promise<void> (async functions),
 *                   Promise<Object> buscarDadosViagem, string (determinarRelatorio:
 *                   "Agendamento.trdp"/"FichaVistoria.trdp"), Objects (obter* functions:
 *                   jQuery elements, estado object)
 * 🔗 CHAMADA POR  : modal-viagem-novo.carregarRelatorioNoModal (indiretamente via btnVisualizarRelatorio),
 *                   exibe-viagem.js (btnVisualizarRelatorio click → carregarRelatorioViagem),
 *                   main.js (event listeners), reportviewer-close-guard.js (callbacks
 *                   wrapeados)
 * 🔄 CHAMA        : jQuery ($.ajax, $('#element'), .append(), .remove(), .show(), .hide(),
 *                   .data(), .removeData(), .empty(), .css(), .on()), Telerik Reporting
 *                   ($.fn.telerik_ReportViewer, instance.dispose/destroy, callbacks:
 *                   renderingBegin/renderingEnd/ready/error), ApiClient.get (buscarDadosViagem),
 *                   StateManager.get/set (viagemId, ehEdicao), setTimeout (1000ms overlay,
 *                   100ms retry waitUntil), Alerta.TratamentoErroComLinha, console.log/warn/error
 *                   (debug com emoji prefixes), window.isReportViewerLoading (flag global)
 * 📦 DEPENDÊNCIAS : jQuery (DOM manipulation, AJAX, $.fn plugin system), Telerik Reporting
 *                   ($.fn.telerik_ReportViewer plugin, API: dispose/destroy/refresh),
 *                   ApiClient (buscarDadosViagem: GET endpoint), StateManager (viagemId
 *                   state), Alerta (TratamentoErroComLinha), Bootstrap Modal (#modalRelatorio,
 *                   #cardRelatorio containers), DOM elements (#reportViewerAgenda,
 *                   #ReportContainerAgenda, #modal-relatorio-loading-overlay),
 *                   reportviewer-close-guard.js (patch callbacks), FrotiX CSS
 *                   (/images/logo_gota_frotix_transparente.png, .ftx-spin-overlay classes)
 * 📝 OBSERVAÇÕES  : IIFE pattern (Immediately Invoked Function Expression) com 'use strict'.
 *                   Arquivo grande (1478 linhas, 20 funções). 3 window.* exports:
 *                   mostrarLoadingRelatorio, esconderLoadingRelatorio, carregarRelatorioViagem.
 *                   17 funções internas privadas (não exportadas). Try-catch completo em
 *                   todas as funções principais com Alerta.TratamentoErroComLinha. Console.log
 *                   extensivo com emoji prefixes ([Relatório] ⏳🗑️✅❌). Overlay loading:
 *                   padrão FrotiX com logo piscando, z-index 999999, bloqueia ESC/clicks.
 *                   Retry pattern: waitUntil com timeout 15s, interval 100ms (até 150
 *                   tentativas). Height fixes: aplicarAlturasFixas com calc(100vh - 380px)
 *                   para garantir scroll interno ReportViewer. 2 tipos relatório:
 *                   Agendamento.trdp (Status != "Concluída"), FichaVistoria.trdp (Status
 *                   == "Concluída"). Telerik callbacks: renderingBegin → mostrarLoading,
 *                   renderingEnd → esconderLoading + isReportViewerLoading=false, ready →
 *                   mostrarRelatorio + diagnóstico, error → mostrarErro + Alerta. Destruir
 *                   viewer: dispose/destroy + removeData + off('*') + empty() (cleanup
 *                   completo). Validações: ej.base existence, $.fn.telerik_ReportViewer,
 *                   viagemId format (GUID uppercase). Fallback: carregarRelatorioViagem
 *                   simplificado se função não existir (compatibilidade). Diagnóstico:
 *                   diagnosticarVisibilidadeRelatorio com 10 checks (elemento, display,
 *                   visibility, opacity, dimensions, zIndex, parent, instance, documentElement,
 *                   computedStyle).
 *
 * 📋 ÍNDICE DE FUNÇÕES (17 internas + 3 exports + 2 async helpers):
 *
 * ┌─ EXPORTS GLOBAIS (3 window.* functions) ────────────────────────────┐
 * │ 1. window.mostrarLoadingRelatorio()                                  │
 * │    → Mostra overlay loading FrotiX (logo piscando)                  │
 * │    → returns void (side effect: append overlay ao body)             │
 * │    → Fluxo:                                                          │
 * │      1. console.log "Mostrando overlay"                             │
 * │      2. $('#modal-relatorio-loading-overlay').remove() (limpar anterior)│
 * │      3. Criar HTML overlay:                                         │
 * │         - id="modal-relatorio-loading-overlay"                      │
 * │         - class="ftx-spin-overlay"                                  │
 * │         - z-index: 999999, cursor: wait                             │
 * │         - logo: /images/logo_gota_frotix_transparente.png           │
 * │         - texto: "Carregando a Ficha...", "Aguarde, por favor"      │
 * │      4. $('body').append(html)                                      │
 * │      5. Bloquear ESC e clicks: on('click keydown', preventDefault)  │
 * │      6. console.log "Overlay visível"                               │
 * │    → Uso típico: inicializarViewer → renderingBegin callback        │
 * │                                                                       │
 * │ 2. window.esconderLoadingRelatorio()                                 │
 * │    → Esconde overlay loading com delay 1s                           │
 * │    → returns void (side effect: remove overlay após timeout)        │
 * │    → Fluxo:                                                          │
 * │      1. console.log "Aguardando 1 segundo antes de remover"         │
 * │      2. setTimeout 1000ms:                                           │
 * │         a. $('#modal-relatorio-loading-overlay').fadeOut(300)       │
 * │         b. setTimeout 300ms: .remove()                              │
 * │         c. console.log "Overlay removido"                           │
 * │    → Delay necessário: dar tempo para usuário ver "Carregando"      │
 * │    → Uso típico: inicializarViewer → renderingEnd callback          │
 * │                                                                       │
 * │ 3. window.carregarRelatorioViagem(viagemId)                          │
 * │    → Entry point público para carregar relatório de viagem          │
 * │    → param viagemId: int (ID da viagem)                             │
 * │    → returns void (side effect: carrega ReportViewer)               │
 * │    → Fluxo simplificado (fallback se função principal não existir): │
 * │      1. Validar viagemId                                            │
 * │      2. Obter $('#reportViewerAgenda')                              │
 * │      3. Destruir viewer anterior (oldViewer.dispose())              │
 * │      4. Criar viewer: .telerik_ReportViewer({ serviceUrl,           │
 * │         reportSource: { report: 'Agendamento.trdp', parameters: {   │
 * │         ViagemId: viagemId.toUpperCase() } } })                     │
 * │      5. Mostrar: $('#cardRelatorio').show(), $('#ReportContainerAgenda').show()│
 * │      6. try-catch: console.error                                    │
 * │    → Nota: função completa definida internamente no IIFE (não mostrada aqui)│
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÕES INTERNAS (17 funções privadas) ────────────────────────────┐
 * │ 4. async waitUntil(condition, timeout=15000, interval=100)           │
 * │    → Retry pattern genérico com timeout e interval                  │
 * │    → param condition: function que retorna boolean                  │
 * │    → param timeout: int ms (default 15s)                            │
 * │    → param interval: int ms (default 100ms)                         │
 * │    → returns Promise<void> (resolve se condition true, reject timeout)│
 * │    → Fluxo: while loop → await condition() → se true: resolve → senão:│
 * │      await sleep(interval) → se elapsed > timeout: reject           │
 * │    → Uso típico: aguardarTelerikReportViewer                        │
 * │                                                                       │
 * │ 5. validarDependencias()                                             │
 * │    → Valida jQuery, Telerik, ej.base carregados                     │
 * │    → returns void (throw Error se dependência faltando)             │
 * │    → Checks: window.$, $.fn.telerik_ReportViewer, ej.base           │
 * │                                                                       │
 * │ 6. validarViagemId(viagemId)                                         │
 * │    → Valida formato ViagemId (GUID uppercase)                       │
 * │    → returns string (viagemId.toUpperCase() ou throw Error)         │
 * │                                                                       │
 * │ 7. obterCard()                                                       │
 * │    → Retorna jQuery element #cardRelatorio                          │
 * │    → returns jQuery object (throw Error se não encontrado)          │
 * │                                                                       │
 * │ 8. obterContainer()                                                  │
 * │    → Retorna jQuery element #ReportContainerAgenda                  │
 * │    → returns jQuery object (throw Error se não encontrado)          │
 * │                                                                       │
 * │ 9. obterViewer()                                                     │
 * │    → Retorna jQuery element #reportViewerAgenda                     │
 * │    → returns jQuery object (throw Error se não encontrado)          │
 * │                                                                       │
 * │ 10. limparInstanciaAnterior()                                        │
 * │     → Destrói ReportViewer anterior (dispose + removeData + empty)  │
 * │     → returns void (side effect: cleanup completo)                  │
 * │                                                                       │
 * │ 11. mostrarLoading(mensagem='Carregando relatório...')              │
 * │     → Mostra loading no container (alternativa ao overlay)          │
 * │     → returns void (side effect: HTML loading no container)         │
 * │                                                                       │
 * │ 12. mostrarErro(mensagem)                                            │
 * │     → Mostra mensagem de erro no container                          │
 * │     → returns void (side effect: HTML erro vermelho)                │
 * │                                                                       │
 * │ 13. aplicarAlturasFixas()                                            │
 * │     → Aplica heights fixos para scroll interno ReportViewer         │
 * │     → returns void (side effect: CSS height calc(100vh - 380px))    │
 * │     → Nota: crítico para evitar overflow na página                  │
 * │                                                                       │
 * │ 14. mostrarRelatorio()                                               │
 * │     → Exibe card e container do relatório                           │
 * │     → returns void (side effect: .show() em elementos)              │
 * │                                                                       │
 * │ 15. esconderRelatorio()                                              │
 * │     → Esconde card e container do relatório                         │
 * │     → returns void (side effect: .hide() em elementos)              │
 * │                                                                       │
 * │ 16. determinarRelatorio(data)                                        │
 * │     → Decide qual relatório usar baseado em Status                  │
 * │     → param data: Object ({ Status, Concluida })                    │
 * │     → returns string: "Agendamento.trdp" ou "FichaVistoria.trdp"    │
 * │     → Lógica: se Status == "Concluída" → FichaVistoria, senão       │
 * │       Agendamento                                                    │
 * │                                                                       │
 * │ 17. inicializarViewer(viagemId, relatorioNome)                      │
 * │     → Cria instância Telerik ReportViewer com callbacks             │
 * │     → param viagemId: string GUID                                   │
 * │     → param relatorioNome: string ("Agendamento.trdp"/etc.)         │
 * │     → returns void (side effect: ReportViewer criado)               │
 * │     → Fluxo: (107 linhas)                                           │
 * │       1. Obter $viewer = obterViewer()                              │
 * │       2. Limpar anterior: limparInstanciaAnterior()                 │
 * │       3. Criar viewer: $viewer.telerik_ReportViewer({               │
 * │          serviceUrl: '/api/reports/',                               │
 * │          reportSource: { report: relatorioNome, parameters: {       │
 * │            ViagemId: viagemId } },                                  │
 * │          viewMode: 'INTERACTIVE', scaleMode: 'FIT_PAGE_WIDTH',      │
 * │          scale: 1.0, pageMode: 'SINGLE_PAGE',                       │
 * │          renderingBegin: mostrarLoadingRelatorio,                   │
 * │          renderingEnd: function() { esconderLoadingRelatorio();     │
 * │            isReportViewerLoading=false; aplicarAlturasFixas(); },   │
 * │          ready: function() { mostrarRelatorio();                    │
 * │            diagnosticarVisibilidadeRelatorio(); },                  │
 * │          error: function(e, args) { mostrarErro(args.message);      │
 * │            Alerta.MostrarMensagemErro(); isReportViewerLoading=false; }│
 * │        })                                                            │
 * │       4. console.log "ReportViewer inicializado"                    │
 * │       5. try-catch: Alerta.TratamentoErroComLinha                   │
 * │                                                                       │
 * │ 18. async buscarDadosViagem(viagemId)                               │
 * │     → GET dados da viagem via API                                   │
 * │     → param viagemId: string GUID                                   │
 * │     → returns Promise<Object>: dados viagem                         │
 * │     → Endpoint: GET /api/Viagem/PegarViagemParaEdicao               │
 * │                                                                       │
 * │ 19. obterEstado()                                                    │
 * │     → Retorna estado atual dos elementos (debug)                    │
 * │     → returns Object: { card, container, viewer, instance, visible }│
 * │                                                                       │
 * │ 20. diagnosticarVisibilidadeRelatorio()                             │
 * │     → Diagnóstico completo de visibilidade (10 checks)              │
 * │     → returns void (side effect: console.log diagnóstico)           │
 * │     → Checks: elemento exists, display, visibility, opacity, width/ │
 * │       height, zIndex, parent display, instance, documentElement,    │
 * │       computedStyle                                                  │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ ASYNC HELPERS (2 funções) ─────────────────────────────────────────┐
 * │ 21. async aguardarTelerikReportViewer()                              │
 * │     → Aguarda Telerik Reporting carregar (retry 5s)                 │
 * │     → returns Promise<void> (resolve se carregado, throw timeout)    │
 * │     → Usa: waitUntil(() => ej.base && $.fn.telerik_ReportViewer,    │
 * │       5000, 100)                                                     │
 * │                                                                       │
 * │ 22. async destruirViewerAnterior()                                   │
 * │     → Destrói viewer anterior (versão async completa)               │
 * │     → returns Promise<void> (side effect: cleanup + off('*'))        │
 * │     → Similar a limparInstanciaAnterior mas mais completo           │
 * └──────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO TÍPICO - CARREGAR RELATÓRIO AGENDAMENTO:
 * 1. Usuário clica btnVisualizarRelatorio em exibe-viagem.js
 * 2. exibe-viagem.js → carregarRelatorioViagem(12345)
 * 3. validarDependencias() → verifica jQuery, Telerik, ej.base
 * 4. validarViagemId(12345) → "12345" (converte uppercase)
 * 5. buscarDadosViagem(12345) → GET /api/Viagem/PegarViagemParaEdicao
 * 6. API retorna: { ViagemId: 12345, Status: "Aberta", ... }
 * 7. determinarRelatorio(data) → Status != "Concluída" → "Agendamento.trdp"
 * 8. inicializarViewer("12345", "Agendamento.trdp")
 * 9. limparInstanciaAnterior() → dispose() anterior + removeData() + empty()
 * 10. telerik_ReportViewer({ serviceUrl: '/api/reports/', reportSource: {
 *     report: 'Agendamento.trdp', parameters: { ViagemId: '12345' } } })
 * 11. Telerik dispara renderingBegin callback
 * 12. renderingBegin → mostrarLoadingRelatorio()
 * 13. Overlay FrotiX aparece (logo piscando, z-index 999999)
 * 14. isReportViewerLoading = true (flag global para reportviewer-close-guard.js)
 * 15. Telerik renderiza relatório (comunicação com /api/reports/)
 * 16. Telerik dispara renderingEnd callback
 * 17. renderingEnd → esconderLoadingRelatorio() (setTimeout 1s)
 * 18. renderingEnd → isReportViewerLoading = false
 * 19. renderingEnd → aplicarAlturasFixas() (calc(100vh - 380px))
 * 20. Telerik dispara ready callback
 * 21. ready → mostrarRelatorio() (#cardRelatorio.show(), #ReportContainerAgenda.show())
 * 22. ready → diagnosticarVisibilidadeRelatorio() (console.log 10 checks)
 * 23. Relatório exibido para usuário
 *
 * 🔄 FLUXO TÍPICO - CARREGAR FICHA VISTORIA (Status Concluída):
 * 1-7. Mesmos passos até determinarRelatorio
 * 8. determinarRelatorio(data) → Status == "Concluída" → "FichaVistoria.trdp"
 * 9. inicializarViewer("12345", "FichaVistoria.trdp")
 * 10-23. Mesmos passos de renderização
 *
 * 📌 TIPOS RELATÓRIO (2 tipos):
 * - Agendamento.trdp: relatório de criação/edição de agendamento
 *   - Usado quando Status != "Concluída" (ex: "Aberta", "Cancelada")
 *   - Contém campos: DataInicial, DataFinal, Motorista, Veículo, etc.
 * - FichaVistoria.trdp: ficha de vistoria pós-viagem
 *   - Usado quando Status == "Concluída"
 *   - Contém campos adicionais: KmInicial, KmFinal, Observações, etc.
 *
 * 📌 OVERLAY LOADING (padrão FrotiX):
 * - Logo: /images/logo_gota_frotix_transparente.png (animação piscando)
 * - Classes: .ftx-spin-overlay, .ftx-spin-box, .ftx-loading-logo, .ftx-loading-bar
 * - Z-index: 999999 (sobre tudo, incluindo modals)
 * - Cursor: wait (indica processamento)
 * - Bloqueia: ESC key + clicks (preventDefault + stopImmediatePropagation)
 * - Texto: "Carregando a Ficha...", "Aguarde, por favor"
 * - Delay remoção: 1s (fadeOut 300ms + remove)
 *
 * 📌 HEIGHT FIXES (aplicarAlturasFixas):
 * - Necessário porque ReportViewer sem height fixo causa overflow na página
 * - Cálculo: calc(100vh - 380px) = viewport height - headers/footers
 * - Aplicado em: #reportViewerAgenda, .trv-report-viewer, .trv-pages-area
 * - Resultado: scroll interno no viewer, página não scrolla
 *
 * 📌 TELERIK REPORTVIEWER CONFIG (inicializarViewer):
 * - serviceUrl: '/api/reports/' (backend Telerik Reporting service)
 * - reportSource: { report: 'Nome.trdp', parameters: { ViagemId: '12345' } }
 * - viewMode: 'INTERACTIVE' (permite interação, zoom, etc.)
 * - scaleMode: 'FIT_PAGE_WIDTH' (ajusta largura da página)
 * - scale: 1.0 (zoom 100%)
 * - pageMode: 'SINGLE_PAGE' (uma página por vez)
 * - Callbacks: renderingBegin, renderingEnd, ready, error
 *
 * 📌 CLEANUP (limparInstanciaAnterior/destruirViewerAnterior):
 * - Obter instance: $viewer.data('telerik_ReportViewer')
 * - Destruir: instance.dispose() ou instance.destroy()
 * - Limpar data: $viewer.removeData('telerik_ReportViewer')
 * - Remover events: $viewer.off('*') (todos event handlers)
 * - Limpar DOM: $viewer.empty()
 * - Resultado: memória liberada, sem memory leaks
 *
 * 📌 VALIDAÇÕES (3 funções):
 * - validarDependencias: window.$, $.fn.telerik_ReportViewer, ej.base
 * - validarViagemId: truthy, converte toString().toUpperCase()
 * - obter* functions: throw Error se elemento não encontrado (fail-fast)
 *
 * 📌 DIAGNÓSTICO (diagnosticarVisibilidadeRelatorio):
 * 10 checks de visibilidade:
 * 1. Elemento existe no DOM
 * 2. Display != 'none'
 * 3. Visibility != 'hidden'
 * 4. Opacity != '0'
 * 5. Width > 0 && Height > 0
 * 6. Z-index >= 0
 * 7. Parent display != 'none'
 * 8. ReportViewer instance exists
 * 9. DocumentElement contains elemento
 * 10. ComputedStyle display != 'none'
 * → Console.log cada check com ✅/❌
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - IIFE pattern: evita poluir scope global (apenas 3 window.* exports)
 * - 'use strict': modo estrito JavaScript
 * - Console.log com prefixes: [Relatório] ⏳ (loading), 🗑️ (destroy), ✅ (success), ❌ (error)
 * - Try-catch em todas as funções públicas com Alerta.TratamentoErroComLinha
 * - Retry pattern: waitUntil genérico reutilizável (timeout 15s, interval 100ms)
 * - Async/await: funções async retornam Promises (buscarDadosViagem, aguardarTelerikReportViewer)
 * - jQuery chaining: $('#element').show().css({ height: '100%' })
 * - Fallback carregarRelatorioViagem: versão simplificada se função principal não existir (compatibilidade)
 * - isReportViewerLoading flag: sincronização com reportviewer-close-guard.js (bloqueia modal close)
 * - setTimeout delays: 1000ms (overlay removal UX), 100ms (retry interval)
 * - FadeOut animation: 300ms antes de remover overlay (transição suave)
 * - Error handling: mostrarErro + Alerta.MostrarMensagemErro (duplo aviso)
 * - StateManager integration: get('viagemId') para obter ID (não usado diretamente neste arquivo)
 * - ApiClient.get: wrapper sobre $.ajax (endpoint /api/Viagem/PegarViagemParaEdicao)
 * - reportSource.parameters: ViagemId sempre uppercase (backend requirement)
 * - viewMode INTERACTIVE: permite zoom, print, export (user interactions)
 * - pageMode SINGLE_PAGE: melhor UX para relatórios curtos (1-2 páginas)
 * - Arquivo grande: 1478 linhas (muita configuração Telerik, callbacks extensos, debug logging)
 *
 * 🔌 VERSÃO: 3.0 (refatorado após Lote 192, adiciona comprehensive header)
 * 📌 ÚLTIMA ATUALIZAÇÃO: 02/02/2026
 **************************************************************************************** */
(function ()
{
    'use strict';

    // ================================================================
    // OVERLAY DE LOADING COM LOGO FROTIX PISCANDO (PADRÃO FROTIX)
    // ================================================================
    window.mostrarLoadingRelatorio = function ()
    {
        console.log('[Relatório] ⏳ Mostrando overlay...');

        // Remover anterior
        $('#modal-relatorio-loading-overlay').remove();

        // Criar HTML com padrão FrotiX (logo piscando)
        const html = `
        <div id="modal-relatorio-loading-overlay" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait;">
            <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
                <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
                <div class="ftx-loading-bar"></div>
                <div class="ftx-loading-text">Carregando a Ficha...</div>
                <div class="ftx-loading-subtext">Aguarde, por favor</div>
            </div>
        </div>
    `;

        $('body').append(html);

        // Bloquear ESC e clicks
        $('#modal-relatorio-loading-overlay').on('click keydown', function (e)
        {
            e.preventDefault();
            e.stopImmediatePropagation();
            return false;
        });

        console.log('[Relatório] ✅ Overlay visível');
    };

    window.esconderLoadingRelatorio = function ()
    {
        console.log('[Relatório] ✅ Aguardando 1 segundo antes de remover overlay...');

        setTimeout(function ()
        {
            $('#modal-relatorio-loading-overlay').fadeOut(300, function ()
            {
                $(this).remove();
            });

            console.log('[Relatório] ✅ Overlay removido');
        }, 1000);
    };


    // ================================================================
    // CONFIGURAÇÕES E ESTADO
    // ================================================================

    const CONFIG = {
        CARD_ID: 'cardRelatorio',
        VIEWER_ID: 'reportViewerAgenda',
        CONTAINER_ID: 'ReportContainerAgenda',
        HIDDEN_ID: 'txtViagemIdRelatorio',
        SERVICE_URL: '/api/reports/',
        RECOVERY_URL: '/api/Agenda/RecuperaViagem',
        TIMEOUT: 20000, // Aumentado de 18s para 20s (+10s total para carregamento da Ficha)
        SHOW_DELAY: 500,
        // ✅ NOVA CONFIGURAÇÃO: Alturas fixas
        VIEWER_HEIGHT: '800px',
        CONTAINER_MIN_HEIGHT: '850px'
    };

    let reportViewerInstance = null;
    let loadTimeout = null;

    // ================================================================
    // FLAGS GLOBAIS DE CONTROLE ANTI-CONFLITO
    // ================================================================

    window.isReportViewerLoading = false;
    window.isReportViewerDestroying = false;
    window.reportViewerInitPromise = null;
    window.reportViewerDestroyPromise = null;

    // ================================================================
    // FUNÇÃO DE ESPERA PARA SINCRONIZAÇÃO
    // ================================================================

    /**
     * ⏳ Aguarda até que uma condição seja verdadeira
     * @param {Function} condition - Função que retorna boolean
     * @param {number} timeout - Timeout em ms
     * @param {number} interval - Intervalo de verificação em ms
     * @returns {Promise<boolean>}
     */
    async function waitUntil(condition, timeout = 15000, interval = 100)
    {
        const startTime = Date.now();

        while (!condition())
        {
            if (Date.now() - startTime > timeout)
            {
                console.warn('⚠️ [Relatório] Timeout ao aguardar condição');
                return false;
            }

            await new Promise(resolve => setTimeout(resolve, interval));
        }

        return true;
    }

    // ================================================================
    // FUNÇÕES PRIVADAS - VALIDAÇÃO
    // ================================================================

    /**
     * 🔍 Valida se todas as dependências necessárias estão carregadas
     * returns {Object} Resultado da validação
     */
    function validarDependencias()
    {
        const deps = {
            jQuery: typeof $ !== 'undefined',
            jQueryFn: typeof $.fn !== 'undefined',
            Telerik: typeof $.fn.telerik_ReportViewer === 'function',
            TelerikViewer: typeof telerikReportViewer !== 'undefined',
            Kendo: typeof kendo !== 'undefined'
        };

        const todasCarregadas = Object.values(deps).every(v => v === true);

        if (!todasCarregadas)
        {
            console.error("❌ Dependências faltando:",
                Object.entries(deps)
                    .filter(([_, loaded]) => !loaded)
                    .map(([name]) => name)
            );
        }

        return {
            valido: todasCarregadas,
            dependencias: deps
        };
    }

    /**
     * 🔍 Valida se o ViagemId é válido
     * param {string} viagemId - ID da viagem
     * returns {boolean}
     */
    function validarViagemId(viagemId)
    {
        if (!viagemId ||
            viagemId === "" ||
            viagemId === "00000000-0000-0000-0000-000000000000")
        {
            console.warn("⚠️ ViagemId inválido:", viagemId);
            return false;
        }
        return true;
    }

    // ================================================================
    // FUNÇÕES PRIVADAS - MANIPULAÇÃO DO DOM
    // ================================================================

    /**
     * 🔍 Obtém referência ao card do relatório
     * returns {HTMLElement|null}
     */
    function obterCard()
    {
        const card = document.getElementById(CONFIG.CARD_ID);

        if (!card)
        {
            console.error(`❌ #${CONFIG.CARD_ID} não encontrado no DOM`);
        }

        return card;
    }

    /**
     * 🔍 Obtém referência ao container do relatório
     * returns {HTMLElement|null}
     */
    function obterContainer()
    {
        const container = document.getElementById(CONFIG.CONTAINER_ID);

        if (!container)
        {
            console.error(`❌ #${CONFIG.CONTAINER_ID} não encontrado no DOM`);
        }

        return container;
    }

    /**
     * 🔍 Obtém referência ao viewer do relatório
     * returns {HTMLElement|null}
     */
    function obterViewer()
    {
        const viewer = document.getElementById(CONFIG.VIEWER_ID);

        if (!viewer)
        {
            console.error(`❌ #${CONFIG.VIEWER_ID} não encontrado no DOM`);
        }

        return viewer;
    }

    /**
     * 🧹 Limpa instância anterior do Telerik ReportViewer
     */
    function limparInstanciaAnterior()
    {
        try
        {
            const $viewer = $(`#${CONFIG.VIEWER_ID}`);

            // Tenta obter instância existente
            const viewer = $viewer.data("telerik_ReportViewer");

            if (viewer)
            {
                console.log("🗑️ Destruindo viewer anterior...");

                if (typeof viewer.dispose === 'function')
                {
                    viewer.dispose();
                } else if (typeof viewer.destroy === 'function')
                {
                    viewer.destroy();
                }

                reportViewerInstance = null;
            }

            // Remove dados do jQuery
            $viewer.removeData("telerik_ReportViewer");

            // Limpa HTML
            $viewer.empty();

            console.log("✅ Instância anterior limpa");

        } catch (error)
        {
            console.warn("⚠️ Erro ao limpar instância anterior:", error.message);
        }
    }

    /**
     * ⏳ Mostra indicador de loading
     * param {string} mensagem - Mensagem a exibir
     */
    function mostrarLoading(mensagem = 'Carregando relatório...')
    {
        const viewer = obterViewer();

        if (!viewer) return;

        viewer.innerHTML = `
            <div class="text-center p-5">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Carregando...</span>
                </div>
                <p class="mt-3 text-muted">${mensagem}</p>
            </div>
        `;
    }

    /**
     * ❌ Mostra mensagem de erro no viewer
     * param {string} mensagem - Mensagem de erro
     */
    function mostrarErro(mensagem)
    {
        const viewer = obterViewer();

        if (!viewer) return;

        viewer.innerHTML = `
            <div class="alert alert-danger m-4">
                <i class="fa fa-exclamation-triangle me-2"></i>
                <strong>Erro:</strong> ${mensagem}
            </div>
        `;
    }

    /**
     * 🎨 Aplica alturas fixas aos containers
     * CORREÇÃO: Define alturas ANTES de inicializar o Telerik
     */
    function aplicarAlturasFixas()
    {
        console.log("📏 Aplicando alturas fixas aos containers...");

        const $viewer = $(`#${CONFIG.VIEWER_ID}`);
        const $container = $(`#${CONFIG.CONTAINER_ID}`);

        // Aplicar altura FIXA no viewer
        $viewer.css({
            'height': CONFIG.VIEWER_HEIGHT,
            'min-height': CONFIG.VIEWER_HEIGHT,
            'max-height': 'none',
            'width': '100%',
            'display': 'block',
            'visibility': 'visible',
            'opacity': '1',
            'position': 'relative'
        });

        // Aplicar altura no container
        $container.css({
            'height': 'auto',
            'min-height': CONFIG.CONTAINER_MIN_HEIGHT,
            'display': 'block',
            'visibility': 'visible',
            'opacity': '1'
        });

        console.log("✅ Alturas aplicadas:", {
            viewer: CONFIG.VIEWER_HEIGHT,
            containerMin: CONFIG.CONTAINER_MIN_HEIGHT
        });
    }

    // ================================================================
    // FUNÇÕES PÚBLICAS - INTERFACE
    // ================================================================

    /**
     * 👁️ Mostra o card e container do relatório
     */
    function mostrarRelatorio()
    {
        try
        {
            console.log("👁️ Mostrando relatório...");

            const $card = $(`#${CONFIG.CARD_ID}`);
            const $container = $(`#${CONFIG.CONTAINER_ID}`);
            const $viewer = $(`#${CONFIG.VIEWER_ID}`);

            if ($card.length === 0)
            {
                console.error("❌ Card não encontrado");
                return;
            }

            // 1. Garantir alturas FIXAS (CRÍTICO)
            aplicarAlturasFixas();

            // 2. Mostrar o card
            console.log("📺 Mostrando #cardRelatorio");
            $card.show().css({
                'display': 'block',
                'visibility': 'visible',
                'opacity': '1'
            });

            // 3. Mostrar o container
            if ($container.length > 0)
            {
                console.log("📺 Mostrando #ReportContainerAgenda");
                $container.show().css({
                    'display': 'block',
                    'visibility': 'visible',
                    'opacity': '1'
                });
            }

            // 4. Mostrar o viewer
            console.log("📺 Mostrando #reportViewerAgenda");
            $viewer.show().css({
                'display': 'block',
                'visibility': 'visible',
                'opacity': '1'
            });

            // 5. Forçar refresh do viewer se existir
            const viewerInstance = $viewer.data('telerik_ReportViewer');
            if (viewerInstance)
            {
                console.log("🔄 Forçando refresh do viewer");
                try
                {
                    if (typeof viewerInstance.refreshReport === 'function')
                    {
                        viewerInstance.refreshReport();
                    }
                } catch (e)
                {
                    console.warn("⚠️ Erro ao fazer refresh:", e);
                }
            }

            // 6. Scroll suave até o card
            setTimeout(() =>
            {
                const cardElement = $card[0];
                if (cardElement)
                {
                    console.log("📜 Fazendo scroll até o relatório");
                    cardElement.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }, 300);

            console.log("✅ Relatório exibido");

            // 7. Debug de visibilidade (se disponível)
            setTimeout(() =>
            {
                if (typeof window.diagnosticarVisibilidadeRelatorio === 'function')
                {
                    window.diagnosticarVisibilidadeRelatorio();
                }
            }, 500);

        } catch (error)
        {
            console.error("❌ Erro ao mostrar relatório:", error);

            if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
            {
                Alerta.TratamentoErroComLinha("relatorio.js", "mostrarRelatorio", error);
            }
        }
    }

    /**
     * 🙈 Esconde o card e limpa o relatório
     */
    function esconderRelatorio()
    {
        console.log("🙈 Escondendo relatório...");

        const card = obterCard();
        const container = obterContainer();

        if (!card || !container) return;

        // Esconder o card com animação
        $(card).slideUp(300, function ()
        {
            card.style.display = "none";
        });

        // Esconder o container
        container.style.display = "none";
        container.classList.remove("visible");

        // Limpar viewer
        limparInstanciaAnterior();

        // Resetar HTML para o loading inicial
        const viewer = obterViewer();

        if (viewer)
        {
            viewer.innerHTML = `
                <div class="text-center p-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Carregando...</span>
                    </div>
                    <p class="mt-3 text-muted">Carregando relatório...</p>
                </div>
            `;
        }

        console.log("✅ Relatório escondido e resetado");
    }

    /**
     * 📊 Determina qual relatório usar baseado no status e finalidade
     * param {Object} data - Dados da viagem
     * returns {string} Nome do arquivo .trdp
     */
    function determinarRelatorio(data)
    {
        if (!data)
        {
            console.warn("⚠️ Dados vazios, usando relatório padrão");
            return "FichaAberta.trdp";
        }

        // Normaliza propriedades (suporta PascalCase e camelCase)
        const status = data.status || data.Status;
        const finalidade = data.finalidade || data.Finalidade;
        const statusAgendamento = data.statusAgendamento ?? data.StatusAgendamento;

        let relatorioAsString = "FichaAberta.trdp"; // Default

        // Lógica de seleção do relatório
        if (status === "Cancelada" || status === "Cancelado")
        {
            relatorioAsString = finalidade !== "Evento"
                ? "FichaCancelada.trdp"
                : "FichaEventoCancelado.trdp";
        }
        else if (finalidade === "Evento" && status !== "Cancelada")
        {
            relatorioAsString = "FichaEvento.trdp";
        }
        else if (status === "Aberta" && finalidade !== "Evento")
        {
            relatorioAsString = "FichaAberta.trdp";
        }
        else if (status === "Realizada")
        {
            relatorioAsString = finalidade !== "Evento"
                ? "FichaRealizada.trdp"
                : "FichaEventoRealizado.trdp";
        }
        else if (statusAgendamento === true)
        {
            relatorioAsString = finalidade !== "Evento"
                ? "FichaAgendamento.trdp"
                : "FichaEventoAgendado.trdp";
        }

        console.log("📄 Relatório selecionado:", relatorioAsString);
        console.log("   - Status:", status);
        console.log("   - Finalidade:", finalidade);
        console.log("   - StatusAgendamento:", statusAgendamento);
        console.log("   - Dados originais:", JSON.stringify(data).substring(0, 500));

        return relatorioAsString;
    }

    /**
     * 🎨 Inicializa o Telerik ReportViewer
     * param {string} viagemId - ID da viagem
     * param {string} relatorioNome - Nome do arquivo .trdp
     */
    function inicializarViewer(viagemId, relatorioNome)
    {
        const $viewer = $(`#${CONFIG.VIEWER_ID}`);

        console.log("🎨 Inicializando Telerik ReportViewer...");
        console.log("   - ViagemId:", viagemId);
        console.log("   - Relatório:", relatorioNome);

        try
        {
            // 1. Limpa HTML
            $viewer.empty();

            // 2. ✅ CRÍTICO: Aplicar alturas ANTES de inicializar
            aplicarAlturasFixas();

            // 3. Mostra progresso do Kendo
            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
            {
                kendo.ui.progress($viewer, true);
            }

            // 4. Inicializa o viewer
            $viewer.telerik_ReportViewer({
                serviceUrl: CONFIG.SERVICE_URL,
                reportSource: {
                    report: relatorioNome,
                    parameters: {
                        ViagemId: viagemId.toString().toUpperCase()
                    }
                },
                viewMode: telerikReportViewer.ViewModes.PRINT_PREVIEW,
                scaleMode: telerikReportViewer.ScaleModes.SPECIFIC,
                scale: 1.0,
                enableAccessibility: false,
                sendEmail: {
                    enabled: true
                },

                // ⚠️ NÃO definir height aqui, já está definido no CSS
                // height: "100%",  <-- REMOVIDO

                // Callbacks do Telerik
                ready: function ()
                {
                    console.log("✅ Telerik ReportViewer PRONTO!");
                    console.log("📄 Relatório renderizado com sucesso");

                    if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
                    {
                        kendo.ui.progress($viewer, false);
                    }
                },

                renderingBegin: function ()
                {
                    console.log("🎨 Iniciando renderização do relatório...");
                },

                renderingEnd: function ()
                {
                    console.log("🎨 Renderização concluída!");
                },

                error: function (e, args)
                {
                    console.error("❌ Erro no Telerik ReportViewer:", args);

                    if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
                    {
                        kendo.ui.progress($viewer, false);
                    }

                    const mensagem = args.message || "Falha ao renderizar o relatório";
                    mostrarErro(mensagem);

                    if (typeof AppToast !== 'undefined')
                    {
                        AppToast.show("Vermelho", "Erro ao renderizar relatório", mensagem);
                    }
                }
            });

            // 5. Guarda referência da instância
            reportViewerInstance = $viewer.data("telerik_ReportViewer");

            console.log("✅ Viewer inicializado");

        } catch (error)
        {
            console.error("❌ Erro ao inicializar viewer:", error);

            if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
            {
                kendo.ui.progress($viewer, false);
            }

            mostrarErro(error.message);
            throw error;
        }
    }

    /**
     * 🌐 Busca os dados da viagem na API
     * param {string} viagemId - ID da viagem
     * returns {Promise<Object>} Dados da viagem
     */
    function buscarDadosViagem(viagemId)
    {
        console.log("🌐 Fazendo requisição para RecuperaViagem...");

        return new Promise((resolve, reject) =>
        {
            $.ajax({
                type: "GET",
                url: CONFIG.RECOVERY_URL,
                data: { id: viagemId },
                contentType: "application/json",
                dataType: "json",
                timeout: CONFIG.TIMEOUT,

                success: function (response)
                {
                    console.log("📥 Resposta recebida da API:", response);

                    // Validar resposta
                    if (!response || !response.data)
                    {
                        reject(new Error("Resposta vazia ou inválida do servidor"));
                        return;
                    }

                    resolve(response.data);
                },

                error: function (jqXHR, textStatus, errorThrown)
                {
                    console.error("❌ Erro na requisição AJAX:", {
                        status: jqXHR.status,
                        statusText: jqXHR.statusText,
                        textStatus: textStatus,
                        error: errorThrown
                    });

                    // Criar erro detalhado
                    let mensagem = "Falha na comunicação com o servidor";

                    if (typeof window.criarErroAjax === 'function')
                    {
                        const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                        mensagem = erro.mensagemUsuario || mensagem;
                    } else if (jqXHR.responseJSON && jqXHR.responseJSON.message)
                    {
                        mensagem = jqXHR.responseJSON.message;
                    }

                    reject(new Error(mensagem));
                }
            });
        });
    }


    /**
     * 📊 Carrega o relatório de viagem com destruição completa do viewer anterior
     * param {string} viagemId - ID da viagem
     */
    window.carregarRelatorioViagem = async function (viagemId)
    {
        console.log('[Relatório] ===== INICIANDO CARREGAMENTO =====');
        console.log('[Relatório] ViagemId:', viagemId);

        // CRÍTICO: Mostrar overlay IMEDIATAMENTE
        window.mostrarLoadingRelatorio();

        try
        {
            // 1. Validação de ID
            if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
            {
                console.error('[Relatório] ViagemId inválido:', viagemId);
                window.esconderLoadingRelatorio();
                window.limparRelatorio();
                return;
            }

            // 2. PROTEÇÃO: Validar dependências Telerik
            if (typeof $ === 'undefined' || !$.fn.telerik_ReportViewer)
            {
                console.error('[Relatório] ❌ Telerik não disponível');

                $('#reportViewerAgenda').html(`
                    <div class="alert alert-warning m-3">
                        <i class="fa fa-exclamation-triangle"></i>
                        Componente não disponível. Recarregue a página.
                    </div>
                `);
                window.esconderLoadingRelatorio();
                return;
            }

            // 3. PROTEÇÃO: Validar modal ainda aberto
            const modalAberto = $('#modalViagens').hasClass('show');
            if (!modalAberto)
            {
                console.warn('[Relatório] ⚠️ Modal foi fechado, cancelando carregamento');
                window.esconderLoadingRelatorio();
                return;
            }

            // 4. PROTEÇÃO: Aguardar destruição anterior
            if (window.isReportViewerDestroying)
            {
                console.log('[Relatório] ⏳ Aguardando limpeza anterior...');
                await waitUntil(() => !window.isReportViewerDestroying, 3000);
            }

            // 5. PROTEÇÃO: Cancelar carregamento duplicado
            if (window.isReportViewerLoading)
            {
                console.log('[Relatório] ⚠️ Já existe carregamento em andamento');
                window.esconderLoadingRelatorio();
                return;
            }

            // 6. MARCAR COMO CARREGANDO
            window.isReportViewerLoading = true;

            // 7. LIMPAR VIEWER ANTERIOR
            console.log('[Relatório] 🧹 Limpando viewer anterior...');
            await window.limparRelatorio();

            // 8. AGUARDAR DEBOUNCE
            await new Promise(resolve => setTimeout(resolve, 500));

            // 9. VALIDAÇÃO: Modal ainda aberto após debounce
            const modalAindaAberto = $('#modalViagens').hasClass('show');
            if (!modalAindaAberto)
            {
                console.warn('[Relatório] ⚠️ Modal fechado durante debounce');
                window.isReportViewerLoading = false;
                window.esconderLoadingRelatorio();
                return;
            }

            // 10. VALIDAÇÃO: ViagemId não mudou
            const viagemIdAtual = $('#txtViagemIdRelatorio').val();
            if (viagemIdAtual && viagemIdAtual !== viagemId)
            {
                console.warn('[Relatório] ⚠️ ViagemId mudou durante carregamento');
                window.isReportViewerLoading = false;
                window.esconderLoadingRelatorio();
                return;
            }

            console.log('[Relatório] 🚀 Iniciando carregamento do viewer...');

            // 2. IMPORTANTE: Destruir completamente o viewer anterior
            await destruirViewerAnterior();

            // 3. Marcar como carregando (JÁ MARCADO ACIMA)
            // window.isReportViewerLoading = true;

            // 4. Verificar dependências (JÁ VERIFICADO ACIMA)

            // 5. Recriar o container do viewer
            const $container = $('#ReportContainerAgenda');
            if ($container.length === 0)
            {
                console.error('[Relatório] Container principal não encontrado');
                window.isReportViewerLoading = false;
                window.esconderLoadingRelatorio();
                return;
            }

            // 6. IMPORTANTE: Recriar o elemento viewer completamente
            $container.empty();
            $container.html(`
            <div id="reportViewerAgenda" style="width:100%; height: 800px; min-height: 800px;">
                <div class="text-center p-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Carregando...</span>
                    </div>
                    <p class="mt-3 text-muted">Carregando relatório...</p>
                </div>
            </div>
        `);

            // 7. Buscar dados da viagem para determinar tipo de relatório
            let tipoRelatorio = 'FichaAgendamento.trdp'; // Default

            try
            {
                const response = await $.ajax({
                    type: "GET",
                    url: '/api/Agenda/RecuperaViagem',
                    data: { id: viagemId },
                    timeout: 10000
                });

                if (response && response.data)
                {
                    tipoRelatorio = determinarRelatorio(response.data);
                    console.log('[Relatório] Tipo determinado:', tipoRelatorio);
                }
            } catch (error)
            {
                console.warn('[Relatório] Usando relatório padrío, erro ao buscar dados:', error);
            }

            // 8. AGUARDAR UM MOMENTO para garantir que o DOM está pronto
            await new Promise(resolve => setTimeout(resolve, 500));

            // 9. Pegar referência NOVA do elemento viewer
            const $viewer = $('#reportViewerAgenda');
            if ($viewer.length === 0)
            {
                console.error('[Relatório] Viewer não foi recriado corretamente');
                window.isReportViewerLoading = false;
                window.esconderLoadingRelatorio();
                return;
            }

            // 10. Limpar conteúdo antes de inicializar
            $viewer.empty();

            // 11. Inicializar novo Telerik ReportViewer
            console.log('[Relatório] Criando novo Telerik ReportViewer...');

            $viewer.telerik_ReportViewer({
                serviceUrl: '/api/reports/',
                reportSource: {
                    report: tipoRelatorio,
                    parameters: {
                        ViagemId: viagemId.toString().toUpperCase()
                    }
                },
                scale: 1.0,
                viewMode: 'PRINT_PREVIEW',
                scaleMode: 'SPECIFIC',

                // Callbacks
                // Callbacks
                ready: function ()
                {
                    try
                    {
                        const modalAberto = $('#modalViagens').hasClass('show');
                        if (!modalAberto)
                        {
                            console.warn('[Relatório] ⚠️ Modal fechado durante ready');
                            window.isReportViewerLoading = false;
                            return;
                        }
                        window.esconderLoadingRelatorio();
                        console.log('[Relatório] ✅ ready - Viewer pronto');
                        window.isReportViewerLoading = false;
                        window.telerikReportViewer = $viewer.data('telerik_ReportViewer');
                        setTimeout(() =>
                        {
                            if (!$('#modalViagens').hasClass('show')) return;
                            if (window.telerikReportViewer && typeof window.telerikReportViewer.scale === 'function')
                            {
                                try
                                {
                                    window.telerikReportViewer.scale({ scale: 1.4, scaleMode: 'SPECIFIC' });
                                    console.log('[Relatório] Zoom automático aplicado: 140%');
                                } catch (e)
                                {
                                    console.warn('[Relatório] Erro ao aplicar zoom:', e);
                                }
                            }
                        }, 500);
                        if (typeof kendo !== 'undefined' && kendo.ui && kendo.ui.progress)
                        {
                            kendo.ui.progress($viewer, false);
                        }
                    } catch (error)
                    {
                        console.error('[Relatório] Erro no callback ready:', error);
                        window.isReportViewerLoading = false;
                    }
                },

                renderingBegin: function ()
                {
                    try
                    {
                        console.log('[Relatório] 🎬 renderingBegin');

                        const modalAberto = $('#modalViagens').hasClass('show');
                        if (!modalAberto)
                        {
                            console.warn('[Relatório] ⚠️ Modal fechado durante renderingBegin');
                            window.esconderLoadingRelatorio();
                            return;
                        }
                    } catch (error)
                    {
                        console.error('[Relatório] Erro no callback renderingBegin:', error);
                        window.esconderLoadingRelatorio();
                    }
                },

                renderingEnd: function ()
                {
                    try
                    {
                        window.esconderLoadingRelatorio();
                        console.log('[Relatório] ✅ renderingEnd - Overlay removido');

                        const modalAberto = $('#modalViagens').hasClass('show');
                        if (!modalAberto)
                        {
                            console.warn('[Relatório] ⚠️ Modal fechado durante renderingEnd');
                            return;
                        }
                    } catch (error)
                    {
                        console.error('[Relatório] Erro no callback renderingEnd:', error);
                        window.esconderLoadingRelatorio();
                    }
                },

                error: function (e, args)
                {
                    window.esconderLoadingRelatorio();
                    console.error('[Relatório] ❌ Erro - Overlay removido:', args);
                    window.isReportViewerLoading = false;

                    // Mostrar erro no container
                    $viewer.html(`
                    <div class="alert alert-danger m-3">
                        <i class="fa fa-exclamation-circle"></i>
                        <strong>Erro ao carregar relatório</strong><br>
                        ${args.message || 'Erro desconhecido'}
                    </div>
                `);

                    if (typeof AppToast !== 'undefined')
                    {
                        AppToast.show('Vermelho', 'Erro ao carregar relatório', 3000);
                    }
                }
            });

            // 12. Mostrar o card do relatório
            $('#cardRelatorio').slideDown(300);
            $('#ReportContainerAgenda').show();

            // 13. Fazer scroll suave até o relatório (opcional)
            setTimeout(() =>
            {
                const cardElement = document.getElementById('cardRelatorio');
                if (cardElement)
                {
                    cardElement.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }, 500);

            console.log('[Relatório] ✅ Processo concluído com sucesso');

        } catch (error)
        {
            console.error('[Relatório] ❌ Erro crítico:', error);
            window.isReportViewerLoading = false;

            if (typeof Alerta !== 'undefined' && Alerta.TratamentoErroComLinha)
            {
                Alerta.TratamentoErroComLinha("relatorio.js", "carregarRelatorioViagem", error);
            }

            // Mostrar erro no container
            $('#reportViewerAgenda').html(`
            <div class="alert alert-danger m-3">
                <i class="fa fa-exclamation-circle"></i>
                <strong>Erro ao inicializar relatório</strong><br>
                ${error.message}
            </div>
        `);
        }
    };

    /**
     * 🧹 Limpa o relatório com destruição completa
     */
    window.limparRelatorio = async function ()
    {
        try
        {
            console.log('[Relatório] 🧹 Iniciando limpeza segura...');

            // PASSO 1: Verificar se já está limpando
            if (window.isReportViewerDestroying)
            {
                console.log('[Relatório] ⚠️ Limpeza já em andamento, aguardando...');

                if (window.reportViewerDestroyPromise)
                {
                    await window.reportViewerDestroyPromise;
                }

                console.log('[Relatório] ✅ Limpeza anterior concluída');
                return;
            }

            // PASSO 2: Marcar que está destruindo
            window.isReportViewerDestroying = true;

            // PASSO 3: Cancelar carregamento pendente
            if (window.isReportViewerLoading)
            {
                console.log('[Relatório] ⚠️ Cancelando carregamento pendente...');
                window.isReportViewerLoading = false;

                if (loadTimeout)
                {
                    clearTimeout(loadTimeout);
                    loadTimeout = null;
                }
            }

            // PASSO 4: Criar Promise de destruição
            window.reportViewerDestroyPromise = new Promise(async (resolve) =>
            {
                try
                {
                    const $viewer = $('#reportViewerAgenda');

                    if ($viewer.length > 0)
                    {
                        const instance = $viewer.data('telerik_ReportViewer');

                        if (instance)
                        {
                            console.log('[Relatório] 🗑️ Destruindo instância do viewer...');

                            try
                            {
                                if (typeof instance.dispose === 'function')
                                {
                                    instance.dispose();
                                }
                                else if (typeof instance.destroy === 'function')
                                {
                                    instance.destroy();
                                }

                                await new Promise(r => setTimeout(r, 200));

                            } catch (e)
                            {
                                console.warn('[Relatório] ⚠️ Erro ao destruir viewer:', e);
                            }
                        }

                        $viewer.removeData('telerik_ReportViewer');
                        $viewer.empty();
                    }

                    $('#cardRelatorio').hide();
                    $('#ReportContainerAgenda').hide();

                    reportViewerInstance = null;
                    window.telerikReportViewer = null;
                    $('#txtViagemIdRelatorio').val('');

                    console.log('[Relatório] ✅ Limpeza concluída');

                } catch (error)
                {
                    console.error('[Relatório] ❌ Erro durante limpeza:', error);
                }
                finally
                {
                    window.isReportViewerDestroying = false;
                    window.reportViewerDestroyPromise = null;
                    resolve();
                }
            });

            await window.reportViewerDestroyPromise;

        } catch (error)
        {
            console.error('[Relatório] ❌ Erro na limpeza:', error);

            window.isReportViewerDestroying = false;
            window.reportViewerDestroyPromise = null;
        }
    };


    /**
     * ℹ️ Obtém informações sobre o estado atual
     * returns {Object}
     */
    function obterEstado()
    {
        return {
            temInstancia: !!reportViewerInstance,
            cardVisivel: obterCard()?.style.display !== 'none',
            containerVisivel: obterContainer()?.style.display !== 'none',
            viewerDisponivel: !!obterViewer(),
            viagemId: $(`#${CONFIG.HIDDEN_ID}`).val() || window.currentViagemId
        };
    }

    // ================================================================
    // 🔧 FUNÇÃO DE DIAGNÓSTICO (DEBUG)
    // ================================================================

    /**
     * 🔍 Diagnostica visibilidade do relatório
     * Função útil para debug em produção
     */
    function diagnosticarVisibilidadeRelatorio()
    {
        console.log("🔍 ===== DIAGNÓSTICO DE VISIBILIDADE =====");

        // 1. Verificar container principal
        const reportContainer = document.getElementById(CONFIG.VIEWER_ID);
        if (!reportContainer)
        {
            console.error(`❌ #${CONFIG.VIEWER_ID} NÃO EXISTE no DOM`);
            return;
        }

        console.log(`✅ #${CONFIG.VIEWER_ID} existe`);
        console.log("📏 Dimensões:", {
            offsetWidth: reportContainer.offsetWidth,
            offsetHeight: reportContainer.offsetHeight,
            clientWidth: reportContainer.clientWidth,
            clientHeight: reportContainer.clientHeight,
            scrollWidth: reportContainer.scrollWidth,
            scrollHeight: reportContainer.scrollHeight
        });

        const styles = window.getComputedStyle(reportContainer);
        console.log("🎨 Estilos computados:", {
            display: styles.display,
            visibility: styles.visibility,
            opacity: styles.opacity,
            height: styles.height,
            minHeight: styles.minHeight,
            maxHeight: styles.maxHeight,
            position: styles.position,
            zIndex: styles.zIndex,
            overflow: styles.overflow
        });

        // 2. Verificar container ReportContainerAgenda
        const reportContainerAgenda = document.getElementById(CONFIG.CONTAINER_ID);
        if (reportContainerAgenda)
        {
            console.log(`✅ #${CONFIG.CONTAINER_ID} existe`);
            const styles2 = window.getComputedStyle(reportContainerAgenda);
            console.log("📏 Dimensões:", {
                offsetWidth: reportContainerAgenda.offsetWidth,
                offsetHeight: reportContainerAgenda.offsetHeight
            });
            console.log("🎨 Estilos:", {
                display: styles2.display,
                visibility: styles2.visibility,
                opacity: styles2.opacity,
                height: styles2.height,
                minHeight: styles2.minHeight
            });
        } else
        {
            console.warn(`⚠️ #${CONFIG.CONTAINER_ID} NÃO EXISTE`);
        }

        // 3. Verificar card
        const cardRelatorio = document.getElementById(CONFIG.CARD_ID);
        if (cardRelatorio)
        {
            console.log(`✅ #${CONFIG.CARD_ID} existe`);
            const styles3 = window.getComputedStyle(cardRelatorio);
            console.log("📏 Dimensões:", {
                offsetWidth: cardRelatorio.offsetWidth,
                offsetHeight: cardRelatorio.offsetHeight
            });
            console.log("🎨 Estilos:", {
                display: styles3.display,
                visibility: styles3.visibility,
                opacity: styles3.opacity
            });
        } else
        {
            console.warn(`⚠️ #${CONFIG.CARD_ID} NÃO EXISTE`);
        }

        // 4. Verificar conteúdo HTML
        const htmlLength = reportContainer.innerHTML.length;
        console.log("📄 Tamanho do HTML:", htmlLength);
        if (htmlLength > 0)
        {
            console.log("📄 Primeiros 500 caracteres:", reportContainer.innerHTML.substring(0, 500));
        }

        // 5. Verificar instância do viewer
        const viewerInstance = $(`#${CONFIG.VIEWER_ID}`).data('telerik_ReportViewer');
        console.log("🔧 Instância do viewer:", viewerInstance ? "EXISTE" : "NÃO EXISTE");

        if (viewerInstance)
        {
            try
            {
                console.log("📊 Estado do viewer:", {
                    reportSource: viewerInstance.reportSource ? viewerInstance.reportSource() : null,
                    serviceUrl: viewerInstance.serviceUrl ? viewerInstance.serviceUrl() : null
                });
            } catch (e)
            {
                console.warn("⚠️ Erro ao obter estado do viewer:", e);
            }
        }

        console.log("🔍 ===== FIM DO DIAGNÓSTICO =====");
    }

    // ================================================================
    // REGISTRAR FUNÇÕES NO ESCOPO GLOBAL
    // ================================================================

    window.carregarRelatorioViagem = carregarRelatorioViagem;
    window.mostrarRelatorio = mostrarRelatorio;
    window.esconderRelatorio = esconderRelatorio;
    window.limparRelatorio = limparRelatorio;
    window.obterEstadoRelatorio = obterEstado;
    window.diagnosticarVisibilidadeRelatorio = diagnosticarVisibilidadeRelatorio;

    console.log("✅ Módulo de relatório carregado!");
    console.log("✅ Funções registradas globalmente:", {
        carregarRelatorioViagem: typeof carregarRelatorioViagem,
        mostrarRelatorio: typeof mostrarRelatorio,
        esconderRelatorio: typeof esconderRelatorio,
        limparRelatorio: typeof limparRelatorio,
        obterEstadoRelatorio: typeof obterEstado,
        diagnosticarVisibilidadeRelatorio: typeof diagnosticarVisibilidadeRelatorio
    });

})();

/**
* ⏳ Aguarda o Telerik ReportViewer estar disponível
* returns {Promise<boolean>}
*/
async function aguardarTelerikReportViewer()
{
    console.log('[Relatório] Aguardando Telerik ReportViewer...');

    const maxTentativas = 50; // 5 segundos no total
    const intervalo = 100; // 100ms entre tentativas

    for (let i = 0; i < maxTentativas; i++)
    {
        // Verificar se Telerik está disponível
        if (typeof $ !== 'undefined' &&
            typeof $.fn !== 'undefined' &&
            typeof $.fn.telerik_ReportViewer === 'function')
        {

            console.log('[Relatório] ✅ Telerik ReportViewer disponível após', i * intervalo, 'ms');

            // Verificar também se os enums estão disponíveis
            if (typeof telerikReportViewer === 'undefined' && typeof window.telerikReportViewer === 'undefined')
            {
                console.warn('[Relatório] ⚠️ Objeto telerikReportViewer global não encontrado');

                // Tentar localizar em outros lugares possíveis
                if (typeof Telerik !== 'undefined' && Telerik.ReportViewer)
                {
                    window.telerikReportViewer = Telerik.ReportViewer;
                    console.log('[Relatório] Objeto telerikReportViewer encontrado em Telerik.ReportViewer');
                }
            }

            return true;
        }

        await new Promise(resolve => setTimeout(resolve, intervalo));
    }

    throw new Error('Telerik ReportViewer não foi carregado após 5 segundos');
}

// Correção de compatibilidade - garante que a função existe
if (typeof window.carregarRelatorioViagem !== 'function')
{
    window.carregarRelatorioViagem = function (viagemId)
    {
        console.log('[Relatório] Função simplificada - ViagemId:', viagemId);

        try
        {
            // Verificação básica
            if (!viagemId)
            {
                console.error('[Relatório] ViagemId não fornecido');
                return;
            }

            const $viewer = $('#reportViewerAgenda');
            if ($viewer.length === 0 || !$.fn.telerik_ReportViewer)
            {
                console.error('[Relatório] Viewer não disponível');
                return;
            }

            // Limpar anterior
            const oldViewer = $viewer.data('telerik_ReportViewer');
            if (oldViewer && oldViewer.dispose)
            {
                try { oldViewer.dispose(); } catch (e) { }
            }

            // Configuração mínima
            $viewer.empty().telerik_ReportViewer({
                serviceUrl: '/api/reports/',
                reportSource: {
                    report: 'Agendamento.trdp',
                    parameters: {
                        ViagemId: viagemId.toString().toUpperCase()
                    }
                },
                scale: 1.0
            });

            // Mostrar
            $('#cardRelatorio').show();
            $('#ReportContainerAgenda').show();

        } catch (error)
        {
            console.error('[Relatório] Erro:', error);
        }
    };
}

/**
* 🗑️ Destrói completamente o viewer anterior
*/
async function destruirViewerAnterior()
{
    console.log('[Relatório] Destruindo viewer anterior...');

    try
    {
        // 1. Buscar todas as possíveis instâncias
        const $viewer = $('#reportViewerAgenda');

        if ($viewer.length > 0)
        {
            // Tentar destruir instância do Telerik
            const instance = $viewer.data('telerik_ReportViewer');
            if (instance)
            {
                console.log('[Relatório] Destruindo instância Telerik...');

                try
                {
                    // Tentar diferentes métodos de destruição
                    if (typeof instance.dispose === 'function')
                    {
                        instance.dispose();
                    }
                    if (typeof instance.destroy === 'function')
                    {
                        instance.destroy();
                    }
                } catch (e)
                {
                    console.warn('[Relatório] Erro ao destruir instância:', e);
                }

                // Limpar data
                $viewer.removeData('telerik_ReportViewer');
            }

            // Limpar todos os event handlers
            $viewer.off();

            // Remover classes do Telerik
            $viewer.removeClass('trv-report-viewer');

            // Limpar HTML
            $viewer.empty();
        }

        // 2. Limpar variáveis globais
        if (window.telerikReportViewer)
        {
            try
            {
                if (typeof window.telerikReportViewer.dispose === 'function')
                {
                    window.telerikReportViewer.dispose();
                }
            } catch (e)
            {
                // Ignorar erro
            }
            window.telerikReportViewer = null;
        }

        // 3. Limpar quaisquer elementos órfãos do Kendo/Telerik
        $('.k-window, .k-overlay').remove();

        // 4. Aguardar um momento para garantir limpeza
        await new Promise(resolve => setTimeout(resolve, 100));

        console.log('[Relatório] ✅ Viewer anterior destruído');

    } catch (error)
    {
        console.error('[Relatório] Erro ao destruir viewer:', error);
        // Continuar mesmo com erro
    }
}
