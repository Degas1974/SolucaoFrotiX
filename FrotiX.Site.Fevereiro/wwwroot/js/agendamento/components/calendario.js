/* ****************************************************************************************
 * ⚡ ARQUIVO: calendario.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciamento completo do calendário FullCalendar para agendamento
 *                   de viagens. Renderiza eventos com cores dinâmicas, tooltips Bootstrap
 *                   customizadas, seleção de datas para criar novos agendamentos, e
 *                   integração com sistema de estado global (window.AppState).
 * 📥 ENTRADAS     : URL endpoint (string) para carregar eventos via AJAX, fetchInfo
 *                   (Object FullCalendar com startStr/endStr), event objects
 *                   (FullCalendar event com id/title/backgroundColor/extendedProps)
 * 📤 SAÍDAS       : Calendário FullCalendar renderizado no DOM (#agenda), tooltips
 *                   Bootstrap com cores dinâmicas, modais de viagem abertas via
 *                   window.ExibeViagem, eventos carregados e exibidos
 * 🔗 CHAMADA POR  : Pages/Agenda/Index.cshtml (InitializeCalendar), outros módulos
 *                   (calendarEvents)
 * 🔄 CHAMA        : $.ajax (jQuery), FullCalendar API (new Calendar, render), Bootstrap
 *                   Tooltip API, moment.js, window.AppState.update, window.ExibeViagem,
 *                   window.AgendamentoService.carregarEventos, window.arredondarHora,
 *                   window.limparCamposModalViagens, window.criarErroAjax,
 *                   Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : jQuery ($.ajax), FullCalendar v5+ (Calendar class), Bootstrap 5
 *                   (Tooltip), moment.js (format, set), Alerta.js, window.AppState,
 *                   window.ExibeViagem, window.AgendamentoService, window.FtxSpin,
 *                   criarErroAjax, Font Awesome icons
 * 📝 OBSERVAÇÕES  : Todas as funções têm try-catch completo. Cores são clareadas em 20%
 *                   para tooltips (lightenColor). Detecção de cor escura usa fórmula W3C
 *                   (isColorDark). Tooltips exibem ícones Font Awesome (fa-duotone).
 *                   Eventos carregados via AJAX com mapping manual de propriedades.
 *                   Click em evento abre modal com ExibeViagem. Select em data cria
 *                   novo agendamento com hora arredondada (10min). Spinners escondidos
 *                   após carregamento (hideAgendaSpinners). FtxSpin.hide() no firstHide.
 *
 * 📋 ÍNDICE DE FUNÇÕES (5 funções principais + 2 nested + 8 callbacks FullCalendar):
 *
 * ┌─ FUNÇÕES UTILITÁRIAS ───────────────────────────────────────────────────┐
 * │ 1. lightenColor(color, percent)                                         │
 * │    → param {string} color - Cor hex (#RRGGBB)                          │
 * │    → param {number} percent - Porcentagem clarear (0-100)              │
 * │    → returns {string} Cor clareada em hex                              │
 * │    → Converte hex→RGB, aplica clarear (r + (255-r)*amount),           │
 * │      converte RGB→hex com padding zero                                 │
 * │    → try-catch: Alerta.TratamentoErroComLinha, retorna cor original   │
 * │                                                                          │
 * │ 2. isColorDark(color)                                                   │
 * │    → param {string} color - Cor hex (#RRGGBB)                          │
 * │    → returns {boolean} True se luminância < 0.5                        │
 * │    → Converte hex→RGB, calcula luminância W3C:                        │
 * │      (0.299*r + 0.587*g + 0.114*b) / 255                              │
 * │    → Considera escura se luminância < 0.5                              │
 * │    → try-catch: retorna false (assume clara)                           │
 * │                                                                          │
 * │ 3. gerarTooltipHTML(event)                                              │
 * │    → param {object} event - FullCalendar event com extendedProps       │
 * │    → returns {string} HTML com ícones Font Awesome e <br>              │
 * │    → Ordem: Evento (fa-tent) → Motorista (fa-user-tie) →              │
 * │                Veículo (fa-car) → Descrição (fa-memo-pad)             │
 * │    → Se finalidade='Evento': exibe evento, senão omite                 │
 * │    → Se motorista vazio: "(Motorista Não Informado)"                   │
 * │    → Se placa vazia: "(Veículo não Informado)"                         │
 * │    → Remove <br> final se existir                                      │
 * │    → try-catch: retorna "Sem informações"                              │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÃO PRINCIPAL FULLCALENDAR ─────────────────────────────────────────┐
 * │ 4. window.InitializeCalendar(URL)                                       │
 * │    → param {string} URL - Endpoint para carregar eventos               │
 * │    → Cria FullCalendar no #agenda com config:                          │
 * │      • timeZone: "local", lazyFetching: true                           │
 * │      • headerToolbar: prev/next/today | title | views                  │
 * │      • buttonText: pt-BR (Hoje, mensal, semanal, diário)              │
 * │      • initialView: "diaSemana" (custom timeGridDay)                   │
 * │      • views custom: diaSemana, listDay, weekends                      │
 * │      • locale: "pt", selectable: true, editable: true                  │
 * │    → Nested function: hideAgendaSpinners()                             │
 * │      Esconde .fc-spinner e remove .e-spinner-pane (Syncfusion)         │
 * │    → Nested function: firstHide()                                      │
 * │      Executa FtxSpin.hide() e esconde #agenda-loading-overlay          │
 * │      uma vez (flag firstPaintDone)                                     │
 * │    → Renderiza calendar (window.calendar.render())                     │
 * │    → setTimeout(firstHide, 10000) como fallback                        │
 * │    → try-catch: Alerta.TratamentoErroComLinha                          │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CALLBACKS FULLCALENDAR (8 callbacks) ──────────────────────────────────┐
 * │ 4.1. events(fetchInfo, successCallback, failureCallback)               │
 * │      → AJAX GET para URL com start/end (fetchInfo.startStr/endStr)     │
 * │      → Mapeia data Array ou data.data para eventos FullCalendar        │
 * │      → Usa datas diretamente (sem +1 dia), allDay: false               │
 * │      → extendedProps: {descricao, placa, motorista, evento, finalidade}│
 * │      → success: successCallback(events)                                │
 * │      → fail: criarErroAjax + failureCallback + successCallback([])     │
 * │      → finally: setTimeout(hideAgendaSpinners, 0)                      │
 * │      → try-catch aninhados (outer, done, mapItem, fail, fail_outer)    │
 * │                                                                          │
 * │ 4.2. eventClick(info)                                                   │
 * │      → Previne default (info.jsEvent.preventDefault())                 │
 * │      → AJAX GET /api/Agenda/RecuperaViagem?id={event.id}               │
 * │      → success: AppState.update (viagem.id, viagem.recorrenciaId, etc) │
 * │                 window.dataInicial (pt-BR format)                      │
 * │                 window.ExibeViagem(response.data)                      │
 * │      → error: criarErroAjax + Alerta.TratamentoErroComLinha            │
 * │      → Modal aberto por ExibeViagem                                    │
 * │      → try-catch: eventClick, eventClick_success, eventClick_error     │
 * │                                                                          │
 * │ 4.3. eventDidMount(info)                                                │
 * │      → Gera HTML tooltip: gerarTooltipHTML(info.event)                 │
 * │      → Obtém bgColor (backgroundColor ou #808080)                      │
 * │      → Clareia cor em 20%: lightenColor(bgColor, 20)                   │
 * │      → Define textColor: isColorDark(lightColor) ? '#FFF' : '#000'     │
 * │      → Destrói tooltip existente: bootstrap.Tooltip.getInstance        │
 * │      → Cria Bootstrap Tooltip: { html: true, customClass, trigger }    │
 * │      → Listener 'shown.bs.tooltip': aplica backgroundColor/color       │
 * │      → try-catch: eventDidMount, shown.bs.tooltip                      │
 * │                                                                          │
 * │ 4.4. loading(isLoading)                                                 │
 * │      → Se !isLoading: firstHide() + setTimeout(hideAgendaSpinners, 0)  │
 * │      → try-catch: loading, firstHide (silencioso)                      │
 * │                                                                          │
 * │ 4.5. viewDidMount()                                                     │
 * │      → Executa firstHide() + setTimeout(hideAgendaSpinners, 0)         │
 * │      → try-catch: viewDidMount, firstHide (silencioso)                 │
 * │                                                                          │
 * │ 4.6. eventSourceFailure()                                               │
 * │      → Executa firstHide() + setTimeout(hideAgendaSpinners, 0)         │
 * │      → try-catch: eventSourceFailure, firstHide (silencioso)           │
 * │                                                                          │
 * │ 4.7. select(info)                                                       │
 * │      → Pega info.start (data/hora clicada)                             │
 * │      → Arredonda hora: window.arredondarHora(start, 10) intervalos 10min│
 * │      → Cria startArredondado com moment.set({hour, minute, second=0})  │
 * │      → dataStr: moment.format("YYYY-MM-DD")                            │
 * │      → window.CarregandoAgendamento = true                             │
 * │      → Chama: window.limparCamposModalViagens()                        │
 * │               window.ExibeViagem("", startArredondado, horaArredondada)│
 * │               $("#modalViagens").modal("show")                         │
 * │      → window.CarregandoAgendamento = false                            │
 * │      → try-catch: select                                               │
 * │                                                                          │
 * │ 4.8. selectOverlap(event)                                               │
 * │      → returns !event.block (permite overlap se não for bloqueio)      │
 * │      → try-catch: retorna true                                         │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ FUNÇÃO ALTERNATIVA ASYNC ──────────────────────────────────────────────┐
 * │ 5. window.calendarEvents(fetchInfo, successCallback, failureCallback)  │
 * │    → async function: await AgendamentoService.carregarEventos(fetchInfo)│
 * │    → Se result.success: successCallback(result.data)                   │
 * │    → Senão: failureCallback(result.error)                              │
 * │    → try-catch: failureCallback(error)                                 │
 * └──────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE INICIALIZAÇÃO DO CALENDÁRIO:
 * 1. InitializeCalendar(URL) chamado de Index.cshtml
 * 2. Define hideAgendaSpinners() e firstHide() (nested functions)
 * 3. Cria FullCalendar com config completa (views, locale, callbacks)
 * 4. Renderiza: window.calendar.render()
 * 5. setTimeout(firstHide, 10000) como fallback
 * 6. FullCalendar dispara events() callback automaticamente
 * 7. events() → AJAX GET → mapeia data → successCallback(events)
 * 8. Para cada evento: eventDidMount() → gera tooltip Bootstrap dinâmica
 * 9. loading(false) → firstHide() → esconde overlays e spinners
 *
 * 🔄 FLUXO DE CLIQUE EM EVENTO (eventClick):
 * 1. Usuário clica em evento no calendário
 * 2. eventClick(info) → preventDefault
 * 3. AJAX GET /api/Agenda/RecuperaViagem?id={event.id}
 * 4. success: AppState.update({viagem.id, viagem.recorrenciaId, ...})
 * 5. window.dataInicial = formato pt-BR
 * 6. window.ExibeViagem(response.data) → preenche e abre modal
 *
 * 🔄 FLUXO DE SELEÇÃO DE DATA (select):
 * 1. Usuário clica em data/hora vazia no calendário
 * 2. select(info) → pega info.start
 * 3. Arredonda hora para intervalos de 10 minutos (arredondarHora)
 * 4. Cria startArredondado com moment.set
 * 5. limparCamposModalViagens() → limpa formulário
 * 6. ExibeViagem("", startArredondado, horaArredondada) → preenche data/hora
 * 7. $("#modalViagens").modal("show") → abre modal para novo agendamento
 *
 * 🔄 FLUXO DE GERAÇÃO DE TOOLTIP (eventDidMount):
 * 1. FullCalendar renderiza evento → dispara eventDidMount(info)
 * 2. gerarTooltipHTML(event) → monta HTML com ícones Font Awesome
 * 3. lightenColor(bgColor, 20) → clareia cor de fundo em 20%
 * 4. isColorDark(lightColor) → define textColor (#FFF ou #000)
 * 5. bootstrap.Tooltip.getInstance → destrói tooltip existente
 * 6. new bootstrap.Tooltip → cria tooltip com HTML customizado
 * 7. 'shown.bs.tooltip' listener → aplica backgroundColor e color dinamicamente
 *
 * 🎨 FLUXO DE MANIPULAÇÃO DE CORES:
 * 1. Evento tem backgroundColor (#RRGGBB)
 * 2. lightenColor(bgColor, 20):
 *    a. Remove # → converte hex para RGB (parseInt base 16)
 *    b. Clareia cada componente: newR = r + (255-r) * 0.20
 *    c. Limita a 255 (Math.min)
 *    d. Converte RGB→hex com padding zero (toHex helper)
 *    e. Retorna #RRGGBB clareado
 * 3. isColorDark(lightColor):
 *    a. Converte hex→RGB
 *    b. Calcula luminância W3C: (0.299*r + 0.587*g + 0.114*b) / 255
 *    c. Retorna luminance < 0.5 (true = escura)
 * 4. Define textColor baseado em isColorDark (#FFFFFF ou #000000)
 *
 * 📌 ESTRUTURA DE DADOS (extendedProps):
 * - descricao: string (descrição pura da viagem, sem motorista/placa)
 * - placa: string (placa do veículo)
 * - motorista: string (nome do motorista)
 * - evento: string (nome do evento se finalidade='Evento')
 * - finalidade: string (tipo de finalidade, ex: 'Evento')
 *
 * 📌 VIEWS CUSTOMIZADAS:
 * - diaSemana: type=timeGridDay, weekends=true, buttonText="Dia"
 * - listDay: weekends=true, buttonText="Lista do dia"
 * - weekends: type=timeGridWeek, weekends=true, hiddenDays=[1-5], buttonText="Fins de Semana"
 *
 * 📌 TRATAMENTO DE SPINNERS:
 * - hideAgendaSpinners(): esconde .fc-spinner (FullCalendar) e remove .e-spinner-pane (Syncfusion)
 * - firstHide(): executa FtxSpin.hide() e esconde #agenda-loading-overlay (uma vez)
 * - Executado em: loading, viewDidMount, eventSourceFailure, timeout 10s
 *
 * 📌 INTEGRAÇÃO COM WINDOW.APPSTATE:
 * - viagem.id: ID da viagem (viagemId)
 * - viagem.idAJAX: ID da viagem (alias)
 * - viagem.recorrenciaId: ID da recorrência (recorrenciaViagemId)
 * - viagem.recorrenciaIdAJAX: ID da recorrência (alias)
 * - viagem.dataInicialList: data inicial ISO (dataInicial)
 *
 * 📌 DIFERENÇAS ENTRE events E calendarEvents:
 * - events (callback inline): usa $.ajax diretamente, mapeia manualmente
 * - calendarEvents (função window): usa AgendamentoService.carregarEventos (async/await)
 * - Ambas retornam array de eventos para FullCalendar via successCallback
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Todas as datas usadas diretamente (SEM adicionar +1 dia, backend retorna correto)
 * - allDay sempre false (eventos têm hora específica)
 * - Tooltips Bootstrap 5 com customClass: 'tooltip-ftx-agenda-dinamica'
 * - Cores dinâmicas aplicadas após tooltip ser criada (evento 'shown.bs.tooltip')
 * - Arredondamento de hora em intervalos de 10 minutos (window.arredondarHora)
 * - Modal aberto/preenchido por window.ExibeViagem (função externa)
 * - window.CarregandoAgendamento flag para controle de carregamento
 * - Try-catch aninhados em events callback (outer, done, mapItem, fail, fail_outer)
 * - Tooltips existentes destruídas antes de criar novas (evita duplicação)
 * - Font Awesome icons: fa-tent, fa-user-tie, fa-car, fa-memo-pad (fa-duotone)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

// ====================================================================
// CALENDÁRIO - Gerenciamento do calendário FullCalendar
// ====================================================================

/**
 * Clareia uma cor em uma porcentagem específica
 * @param {string} color - Cor em formato hex (#RRGGBB)
 * @param {number} percent - Porcentagem para clarear (0-100)
 * @returns {string} Cor clareada em formato hex
 */
function lightenColor(color, percent) {
    try {
        // Remove # se houver
        const hex = color.replace('#', '');

        // Converte hex para RGB
        const r = parseInt(hex.substring(0, 2), 16);
        const g = parseInt(hex.substring(2, 4), 16);
        const b = parseInt(hex.substring(4, 6), 16);

        // Clareia cada componente
        const amount = percent / 100;
        const newR = Math.min(255, Math.round(r + (255 - r) * amount));
        const newG = Math.min(255, Math.round(g + (255 - g) * amount));
        const newB = Math.min(255, Math.round(b + (255 - b) * amount));

        // Converte de volta para hex
        const toHex = (n) => {
            const hex = n.toString(16);
            return hex.length === 1 ? '0' + hex : hex;
        };

        return `#${toHex(newR)}${toHex(newG)}${toHex(newB)}`;
    } catch (error) {
        Alerta.TratamentoErroComLinha("calendario.js", "lightenColor", error);
        return color; // Retorna cor original em caso de erro
    }
}

/**
 * Detecta se uma cor é escura
 * @param {string} color - Cor em formato hex (#RRGGBB)
 * @returns {boolean} True se a cor for escura
 */
function isColorDark(color) {
    try {
        // Remove # se houver
        const hex = color.replace('#', '');

        // Converte hex para RGB
        const r = parseInt(hex.substring(0, 2), 16);
        const g = parseInt(hex.substring(2, 4), 16);
        const b = parseInt(hex.substring(4, 6), 16);

        // Calcula luminância relativa (fórmula W3C)
        const luminance = (0.299 * r + 0.587 * g + 0.114 * b) / 255;

        // Considera escura se luminância < 0.5
        return luminance < 0.5;
    } catch (error) {
        Alerta.TratamentoErroComLinha("calendario.js", "isColorDark", error);
        return false; // Assume clara em caso de erro
    }
}

/**
 * Gera HTML da tooltip com ícones e quebras de linha
 * @param {object} event - Objeto do evento com propriedades placa, motorista, evento, descricao
 * @returns {string} HTML da tooltip
 */
function gerarTooltipHTML(event) {
    try {
        const props = event.extendedProps || {};
        let html = '';

        // ORDEM: Evento → Motorista → Veículo → Descrição

        // 1. Evento (se for finalidade = Evento)
        const evento = props.evento || '';
        const finalidade = props.finalidade || '';
        if (finalidade === 'Evento' && evento && evento.trim() !== '') {
            html += '<i class="fa-duotone fa-tent"></i>: ' + evento + '<br>';
        }

        // 2. Motorista
        const motorista = props.motorista || '';
        if (motorista && motorista.trim() !== '') {
            html += '<i class="fa-duotone fa-user-tie"></i>: ' + motorista + '<br>';
        } else {
            html += '<i class="fa-duotone fa-user-tie"></i>: (Motorista Não Informado)<br>';
        }

        // 3. Veículo (Placa)
        const placa = props.placa || '';
        if (placa && placa.trim() !== '') {
            html += '<i class="fa-duotone fa-car"></i>: ' + placa + '<br>';
        } else {
            html += '<i class="fa-duotone fa-car"></i>: (Veículo não Informado)<br>';
        }

        // 4. Descrição (campo DescricaoPura - apenas a descrição da viagem, sem motorista/placa)
        const descricao = props.descricao || '';
        if (descricao && descricao.trim() !== '') {
            html += '<i class="fa-duotone fa-memo-pad"></i>: ' + descricao.trim();
        }

        // Remove <br> do final se existir
        if (html.endsWith('<br>')) {
            html = html.substring(0, html.length - 4);
        }

        return html;
    } catch (error) {
        Alerta.TratamentoErroComLinha("calendario.js", "gerarTooltipHTML", error);
        return "Sem informações";
    }
}

/**
 * Inicializa o calendário FullCalendar
 * param {string} URL - URL para carregar eventos
 */
window.InitializeCalendar = function (URL)
{
    try
    {
        function hideAgendaSpinners()
        {
            try
            {
                var root = document.getElementById('agenda');
                if (!root) return;

                root.querySelectorAll('.fc-spinner').forEach(function (el)
                {
                    try
                    {
                        el.style.setProperty('display', 'none', 'important');
                        el.style.setProperty('visibility', 'hidden', 'important');
                    } catch (error)
                    {
                        Alerta.TratamentoErroComLinha("calendario.js", "hideAgendaSpinners_forEach", error);
                    }
                });

                root.querySelectorAll('.e-spinner-pane, .e-spin-overlay, .e-spin-show').forEach(function (el)
                {
                    try
                    {
                        el.remove();
                    } catch (error)
                    {
                        el.style.setProperty('display', 'none', 'important');
                        el.style.setProperty('visibility', 'hidden', 'important');
                    }
                });
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("calendario.js", "hideAgendaSpinners", error);
            }
        }

        let firstPaintDone = false;
        const firstHide = () =>
        {
            try
            {
                if (!firstPaintDone)
                {
                    firstPaintDone = true;
                    if (window.FtxSpin) window.FtxSpin.hide();

                    // Esconder modal de espera da agenda
                    const loadingOverlay = document.getElementById('agenda-loading-overlay');
                    if (loadingOverlay) {
                        loadingOverlay.style.display = 'none';
                    }
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("calendario.js", "firstHide", error);
            }
        };

        var calendarEl = document.getElementById("agenda");
        window.calendar = new FullCalendar.Calendar(calendarEl, {
            timeZone: "local",
            lazyFetching: true,
            headerToolbar: {
                left: "prev,next today",
                center: "title",
                right: "dayGridMonth,timeGridWeek,timeGridDay"
            },
            buttonText: {
                today: "Hoje",
                dayGridMonth: "mensal",
                timeGridWeek: "semanal",
                timeGridDay: "diário"
            },
            initialView: "diaSemana",
            views: {
                diaSemana: {
                    buttonText: "Dia",
                    type: "timeGridDay",
                    weekends: true
                },
                listDay: {
                    buttonText: "Lista do dia",
                    weekends: true
                },
                weekends: {
                    buttonText: "Fins de Semana",
                    type: "timeGridWeek",
                    weekends: true,
                    hiddenDays: [1, 2, 3, 4, 5]
                }
            },
            locale: "pt",
            selectable: true,
            editable: true,
            navLinks: true,
            events: function (fetchInfo, successCallback, failureCallback)
            {
                try
                {
                    $.ajax({
                        url: URL,
                        type: "GET",
                        dataType: "json",
                        data: {
                            start: fetchInfo.startStr,
                            end: fetchInfo.endStr
                        }
                    }).done(function (data)
                    {
                        try
                        {
                            var raw = Array.isArray(data) ? data : data && data.data || [];
                            var events = [];

                            for (var i = 0; i < raw.length; i++)
                            {
                                var item = raw[i];
                                try
                                {
                                    // CORREÇÃO: Usar as datas diretamente sem adicionar 1 dia
                                    // O backend já retorna as datas corretas
                                    var start = item.start;
                                    var end = item.end;

                                    if (!start) continue;

                                    events.push({
                                        id: item.id,
                                        title: item.title,
                                        start: start,
                                        end: end,
                                        backgroundColor: item.backgroundColor,
                                        textColor: item.textColor,
                                        allDay: false,
                                        extendedProps: {
                                            descricao: item.descricao,
                                            placa: item.placa,
                                            motorista: item.motorista,
                                            evento: item.evento,
                                            finalidade: item.finalidade
                                        }
                                    });
                                } catch (e)
                                {
                                    Alerta.TratamentoErroComLinha("calendario.js", "events_mapItem", e);
                                }
                            }

                            successCallback(events);
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("calendario.js", "events_done", error);
                            if (typeof failureCallback === 'function') failureCallback(error);
                            successCallback([]);
                        } finally
                        {
                            setTimeout(hideAgendaSpinners, 0);
                        }
                    }).fail(function (jqXHR, textStatus, errorThrown)
                    {
                        try
                        {
                            const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                            Alerta.TratamentoErroComLinha("calendario.js", "events_fail", erro);
                            if (typeof failureCallback === 'function') failureCallback(erro);
                        } catch (error)
                        {
                            Alerta.TratamentoErroComLinha("calendario.js", "events_fail_outer", error);
                        } finally
                        {
                            successCallback([]);
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "events", error);
                }
            },
            eventClick: function (info)
            {
                try
                {
                    var idViagem = info.event.id;
                    info.jsEvent.preventDefault();

                    $.ajax({
                        type: "GET",
                        url: "/api/Agenda/RecuperaViagem",
                        data: { id: idViagem },
                        contentType: "application/json",
                        dataType: "json",
                        success: function (response)
                        {
                            try
                            {
                                window.AppState.update({
                                    'viagem.id': response.data.viagemId,
                                    'viagem.idAJAX': response.data.viagemId,
                                    'viagem.recorrenciaId': response.data.recorrenciaViagemId,
                                    'viagem.recorrenciaIdAJAX': response.data.recorrenciaViagemId,
                                    'viagem.dataInicialList': response.data.dataInicial
                                });

                                const dataInicialISO = response.data.dataInicial;
                                const dataTemp = new Date(dataInicialISO);
                                window.dataInicial = new Intl.DateTimeFormat("pt-BR").format(dataTemp);

                                if (typeof window.ExibeViagem === 'function') window.ExibeViagem(response.data);
                            } catch (error)
                            {
                                Alerta.TratamentoErroComLinha("calendario.js", "eventClick_success", error);
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown)
                        {
                            const erro = window.criarErroAjax(jqXHR, textStatus, errorThrown, this);
                            Alerta.TratamentoErroComLinha("calendario.js", "eventClick_error", erro);
                        }
                    });

                    // Modal sera aberto por ExibeViagem apos carregar dados
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "eventClick", error);
                }
            },
            eventDidMount: function (info)
            {
                try
                {
                    // Gera HTML da tooltip com ícones e quebras de linha
                    const tooltipHTML = gerarTooltipHTML(info.event);

                    // Obtém cor de fundo do evento
                    const bgColor = info.event.backgroundColor || '#808080';

                    // Clareia a cor em 20%
                    const lightColor = lightenColor(bgColor, 20);

                    // Detecta se a cor é escura para definir cor do texto
                    const textColor = isColorDark(lightColor) ? '#FFFFFF' : '#000000';

                    // Destrói tooltip existente se houver
                    const existingTooltip = bootstrap.Tooltip.getInstance(info.el);
                    if (existingTooltip) {
                        existingTooltip.dispose();
                    }

                    // Cria tooltip com HTML customizado e cor dinâmica
                    new bootstrap.Tooltip(info.el, {
                        html: true,
                        title: tooltipHTML,
                        customClass: 'tooltip-ftx-agenda-dinamica',
                        trigger: 'hover'
                    });

                    // Aplica cor de fundo e texto dinamicamente após tooltip ser criada
                    info.el.addEventListener('shown.bs.tooltip', function() {
                        try {
                            const tooltipElement = document.querySelector('.tooltip-ftx-agenda-dinamica .tooltip-inner');
                            if (tooltipElement) {
                                tooltipElement.style.backgroundColor = lightColor;
                                tooltipElement.style.color = textColor;
                            }
                        } catch (err) {
                            Alerta.TratamentoErroComLinha("calendario.js", "shown.bs.tooltip", err);
                        }
                    });
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "eventDidMount", error);
                }
            },
            loading: function (isLoading)
            {
                try
                {
                    if (!isLoading)
                    {
                        try
                        {
                            firstHide && firstHide();
                        } catch (_) { }
                        setTimeout(hideAgendaSpinners, 0);
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "loading", error);
                }
            },
            viewDidMount: function ()
            {
                try
                {
                    try
                    {
                        firstHide && firstHide();
                    } catch (_) { }
                    setTimeout(hideAgendaSpinners, 0);
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "viewDidMount", error);
                }
            },
            eventSourceFailure: function ()
            {
                try
                {
                    try
                    {
                        firstHide && firstHide();
                    } catch (_) { }
                    setTimeout(hideAgendaSpinners, 0);
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "eventSourceFailure", error);
                }
            },
            select: function (info)
            {
                try
                {
                    // Pega a data/hora clicada
                    const start = info.start ? new Date(info.start) : new Date();

                    // Arredonda a hora para intervalos de 10 minutos
                    const horaArredondada = window.arredondarHora(start, 10);

                    // Cria um novo Date com a hora arredondada
                    const startArredondado = moment(start).set({
                        'hour': parseInt(horaArredondada.split(':')[0]),
                        'minute': parseInt(horaArredondada.split(':')[1]),
                        'second': 0,
                        'millisecond': 0
                    }).toDate();

                    const dataStr = moment(startArredondado).format("YYYY-MM-DD");

                    window.CarregandoAgendamento = true;

                    // 1. Limpar campos primeiro
                    if (typeof window.limparCamposModalViagens === 'function')
                    {
                        window.limparCamposModalViagens();
                    }

                    // 2. Configurar novo agendamento (ExibeViagem já preenche data/hora)
                    if (typeof window.ExibeViagem === 'function')
                    {
                        window.ExibeViagem("", startArredondado, horaArredondada);
                    }

                    // 3. Abrir modal
                    $("#modalViagens").modal("show");

                    window.CarregandoAgendamento = false;
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "select", error);
                }
            },
            selectOverlap: function (event)
            {
                try
                {
                    return !event.block;
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha("calendario.js", "selectOverlap", error);
                    return true;
                }
            }
        });

        window.calendar.render();
        setTimeout(firstHide, 10000);

    } catch (error)
    {
        Alerta.TratamentoErroComLinha("calendario.js", "InitializeCalendar", error);
    }
};

/**
 * Carrega eventos do calendário (callback alternativo)
 * param {Object} fetchInfo - Informações de fetch
 * param {Function} successCallback - Callback de sucesso
 * param {Function} failureCallback - Callback de falha
 */
window.calendarEvents = async function (fetchInfo, successCallback, failureCallback)
{
    try
    {
        const result = await window.AgendamentoService.carregarEventos(fetchInfo);

        if (result.success)
        {
            successCallback(result.data);
        } else
        {
            failureCallback(result.error);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("calendario.js", "calendarEvents", error);
        failureCallback(error);
    }
};
