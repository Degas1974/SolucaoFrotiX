/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está completamente documentado em:                         ║
 * ║  📄 Documentacao/JavaScript/agendamento/components/calendario.js.md      ║
 * ║                                                                          ║
 * ║  Última atualização: 17/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
