/* ****************************************************************************************
 * ⚡ ARQUIVO: formatters.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Funções utilitárias para formatação de dados e cálculos de viagem.
 *                   Inclui parsing seguro (parseInt), formatação de datas (pt-BR),
 *                   detecção de script atual, remoção de datas de listas multi-select,
 *                   cálculos automáticos (distância km, duração horas), sincronização
 *                   de listboxes/badges, e atualização de UI com datas formatadas.
 * 📥 ENTRADAS     : valores variados (number, string, Date, timestamp), arrays de datas,
 *                   valores de campos DOM (#txtKmInicial, #txtKmFinal, #txtDataInicial,
 *                   etc.), window.selectedDates (global array)
 * 📤 SAÍDAS       : valores parseados (number/null), strings formatadas (DD/MM/YYYY,
 *                   "ViagemUpsert_NNN"), campos calculados atualizados (#txtQuilometragem,
 *                   #txtDuracao), listboxes sincronizadas (lstDiasCalendario,
 *                   lstDiasCalendarioHTML), badges atualizadas (#itensBadge, #itensBadgeHTML),
 *                   classes CSS aplicadas (.distancia-alerta)
 * 🔗 CHAMADA POR  : Event handlers de campos (change eventos), exibe-viagem.js,
 *                   components de calendário, botões de UI
 * 🔄 CHAMA        : parseInt, Number.isFinite, Intl.DateTimeFormat, parseFloat, Math.round,
 *                   moment.js (format), jQuery (val, addClass, removeClass), document
 *                   methods (getElementById, createElement, getElementsByTagName),
 *                   Syncfusion ej2_instances (dataBind), Alerta.TratamentoErroComLinha
 * 📦 DEPENDÊNCIAS : moment.js, jQuery, Syncfusion EJ2 (ListBox, Calendar), Alerta.js,
 *                   window.selectedDates (global array), window.formatDate (external fn),
 *                   window.datasSelecionadas (global array)
 * 📝 OBSERVAÇÕES  : Exporta 9 funções window.* + 1 variável global (__scriptName). Todas
 *                   têm try-catch completo. calcularDistanciaViagem adiciona classe
 *                   .distancia-alerta se >100km (visual warning). calcularDuracaoViagem
 *                   aceita formatos DD/MM/YYYY e YYYY-MM-DD. removeDate sincroniza 3 UI
 *                   elements (window.selectedDates array, lstDiasCalendario listbox,
 *                   calDatasSelecionadas calendar). __getScriptName usa regex para detectar
 *                   ViagemUpsert_NNN.js ou .min.js.
 *
 * 📋 ÍNDICE DE FUNÇÕES (9 funções window.* + 1 variável global):
 *
 * ┌─ PARSING E FORMATAÇÃO ───────────────────────────────────────────────────┐
 * │ 1. window.parseIntSafe(v)                                                │
 * │    → param {*} v - Valor qualquer                                        │
 * │    → returns {number|null} parseInt(v,10) se isFinite, senão null        │
 * │    → Safe wrapper: evita NaN, retorna null em erro                       │
 * │    → try-catch: Alerta.TratamentoErroComLinha + retorna null             │
 * │                                                                            │
 * │ 2. window.formatDateLocal(d)                                              │
 * │    → param {Date} d - Data                                               │
 * │    → returns {string} Data formatada pt-BR                               │
 * │    → Tenta window.formatDate(d) primeiro (função externa)                │
 * │    → Fallback: new Intl.DateTimeFormat("pt-BR", {timeZone: "America/Sao_Paulo"})│
 * │    → try-catch: Alerta.TratamentoErroComLinha + fallback Intl            │
 * └───────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ DETECÇÃO DE SCRIPT ──────────────────────────────────────────────────────┐
 * │ 3. window.__getScriptName()                                               │
 * │    → returns {string} Nome do script atual (ex: "ViagemUpsert_001")      │
 * │    → Obtém document.currentScript ou scripts[last]                       │
 * │    → Regex: /(ViagemUpsert_\d+)(?:\.min)?\.js$/i                         │
 * │    → Fallback: "ViagemUpsert" se regex não match                         │
 * │    → try-catch: Alerta.TratamentoErroComLinha + retorna "ViagemUpsert"   │
 * │                                                                            │
 * │ window.__scriptName (variável global)                                    │
 * │    → Executa __getScriptName() e armazena resultado (line 67)            │
 * │    → Usado para identificação de script em logs                          │
 * └───────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ GERENCIAMENTO DE DATAS SELECIONADAS ────────────────────────────────────┐
 * │ 4. window.removeDate(timestamp)                                           │
 * │    → param {number} timestamp - Timestamp da data a remover              │
 * │    → Remove de 3 locais:                                                 │
 * │      1. window.selectedDates array (filter por timestamp)                │
 * │      2. lstDiasCalendario ListBox (Syncfusion): dataSource + dataBind    │
 * │      3. calDatasSelecionadas Calendar (Syncfusion): remove de cal.values │
 * │    → Normaliza datas para timestamp (setHours 0,0,0,0) antes comparar    │
 * │    → try-catch: Alerta.TratamentoErroComLinha                            │
 * │                                                                            │
 * │ 5. window.syncListBoxAndBadges()                                          │
 * │    → Sincroniza lstDiasCalendario ListBox com window.selectedDates       │
 * │    → Atualiza badges: #itensBadge e #itensBadgeHTML com total de itens   │
 * │    → Se listBox existe: dataSource = selectedDates + dataBind()          │
 * │    → try-catch: Alerta.TratamentoErroComLinha                            │
 * │                                                                            │
 * │ 6. window.atualizarListBoxHTMLComDatas(datas)                             │
 * │    → param {Array} datas - Array de datas (Date objects ou strings)     │
 * │    → Copia datas para window.datasSelecionadas = [...datas]              │
 * │    → Limpa lstDiasCalendarioHTML: innerHTML = ""                         │
 * │    → Para cada data: cria <li> com moment(data).format("DD/MM/YYYY")     │
 * │    → Atualiza #itensBadgeHTML com contDatas                              │
 * │    → Atualiza #diasSelecionadosTexto: "Dias Selecionados (N)"           │
 * │    → try-catch: Alerta.TratamentoErroComLinha (outer + forEach inner)    │
 * └───────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ CÁLCULOS DE VIAGEM ──────────────────────────────────────────────────────┐
 * │ 7. window.calcularDistanciaViagem()                                       │
 * │    → Calcula kmPercorrido = kmFinal - kmInicial                          │
 * │    → Lê #txtKmInicial.val() e #txtKmFinal.val()                          │
 * │    → Se ambos preenchidos:                                               │
 * │      • parseFloat com replace(",", ".") para aceitar formato pt-BR       │
 * │      • kmPercorrido = Math.round(kmFinal - kmInicial)                    │
 * │      • Atualiza #txtQuilometragem.val(kmPercorrido)                      │
 * │      • Se kmPercorrido > 100: addClass("distancia-alerta") (visual alert)│
 * │      • Senão: removeClass("distancia-alerta")                            │
 * │    → Se faltam campos ou isNaN: limpa #txtQuilometragem + remove class   │
 * │    → try-catch: Alerta.TratamentoErroComLinha                            │
 * │                                                                            │
 * │ 8. window.calcularDuracaoViagem()                                         │
 * │    → Calcula duração em horas = (dtFinal - dtInicial) / (1000*60*60)     │
 * │    → Lê 4 campos: #txtDataInicial, #txtHoraInicial, #txtDataFinal,       │
 * │                   #txtHoraFinal                                           │
 * │    → parseDataHora(data, hora) helper interno:                           │
 * │      • Se data.includes("/"): formato DD/MM/YYYY → YYYY-MM-DD            │
 * │      • Se data.includes("-"): já YYYY-MM-DD                              │
 * │      • Cria Date object: `${data}T${hora}`                               │
 * │    → Se dtFinal <= dtInicial: limpa #txtDuracao                          │
 * │    → Senão: diffHoras.toFixed(2) → Math.round() → atualiza #txtDuracao   │
 * │    → try-catch: Alerta.TratamentoErroComLinha (outer + parseDataHora)    │
 * └───────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE CÁLCULO DISTÂNCIA:
 * 1. Usuário preenche #txtKmInicial (ex: "1000")
 * 2. Usuário preenche #txtKmFinal (ex: "1150,5")
 * 3. Event handler chama calcularDistanciaViagem()
 * 4. parseFloat("1000".replace(",",".")) → 1000
 * 5. parseFloat("1150,5".replace(",",".")) → 1150.5
 * 6. kmPercorrido = Math.round(1150.5 - 1000) → 151
 * 7. #txtQuilometragem.val(151)
 * 8. 151 > 100 → addClass("distancia-alerta") → texto fica vermelho/negrito
 *
 * 🔄 FLUXO DE CÁLCULO DURAÇÃO:
 * 1. Usuário preenche #txtDataInicial ("15/01/2026")
 * 2. Usuário preenche #txtHoraInicial ("08:00")
 * 3. Usuário preenche #txtDataFinal ("15/01/2026")
 * 4. Usuário preenche #txtHoraFinal ("12:30")
 * 5. Event handler chama calcularDuracaoViagem()
 * 6. parseDataHora("15/01/2026", "08:00"):
 *    a. data.includes("/") → true
 *    b. split("/") → ["15","01","2026"]
 *    c. new Date("2026-01-15T08:00") → dtInicial
 * 7. parseDataHora("15/01/2026", "12:30") → dtFinal
 * 8. diffMs = dtFinal - dtInicial (milissegundos)
 * 9. diffHoras = (diffMs / (1000*60*60)).toFixed(2) → 4.50
 * 10. Math.round(4.50) → 5 (arredonda para cima se >=.5)
 * 11. #txtDuracao.val(5) → "5 horas"
 *
 * 🔄 FLUXO DE REMOÇÃO DE DATA:
 * 1. Usuário clica botão remover em date listbox
 * 2. Button callback chama removeDate(timestamp)
 * 3. window.selectedDates = selectedDates.filter(d => d.Timestamp !== timestamp)
 * 4. lstDiasCalendario.dataSource = selectedDates + dataBind()
 * 5. calDatasSelecionadas.values = values.filter(date => normalized !== timestamp)
 * 6. Data removida de array, listbox, e calendário simultaneamente
 *
 * 🔄 FLUXO DE SINCRONIZAÇÃO LISTBOX:
 * 1. window.selectedDates é modificado programaticamente
 * 2. Código chama syncListBoxAndBadges()
 * 3. lstDiasCalendario.dataSource = selectedDates
 * 4. lstDiasCalendario.dataBind() (Syncfusion atualiza UI)
 * 5. badge1.textContent = selectedDates.length
 * 6. badge2.textContent = selectedDates.length
 * 7. UI reflete array atualizado
 *
 * 📌 FORMATO DE DATAS:
 * - Input aceito: DD/MM/YYYY (pt-BR) ou YYYY-MM-DD (ISO)
 * - Output: DD/MM/YYYY (moment format) para listboxes
 * - Timestamp: setHours(0,0,0,0) para normalização (comparação de datas)
 *
 * 📌 CÁLCULOS AUTOMÁTICOS:
 * - calcularDistanciaViagem: kmFinal - kmInicial, arredondado
 * - calcularDuracaoViagem: (dtFinal - dtInicial) / 1h em ms, arredondado
 * - Ambos chamados por event handlers de campos (change/blur)
 *
 * 📌 CLASSES CSS:
 * - .distancia-alerta: aplicada se kmPercorrido > 100 (vermelho/negrito)
 * - Removida se kmPercorrido <= 100
 *
 * 📌 LISTBOXES SINCRONIZADAS:
 * - lstDiasCalendario (Syncfusion ListBox): dataSource binding
 * - lstDiasCalendarioHTML (HTML <ul>): innerHTML manual
 * - Ambas refletem window.selectedDates / window.datasSelecionadas
 *
 * 📌 BADGES:
 * - #itensBadge: Syncfusion listbox badge
 * - #itensBadgeHTML: HTML listbox badge
 * - Ambos exibem contagem de datas selecionadas
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - parseFloat usa replace(",", ".") para aceitar vírgula decimal pt-BR
 * - parseDataHora helper interno em calcularDuracaoViagem (não exportada)
 * - __getScriptName detecta scripts minificados (.min.js) e não-minificados (.js)
 * - removeDate normaliza timestamps com setHours(0,0,0,0) para comparação correta
 * - atualizarListBoxHTMLComDatas cria <li> com class "list-group-item" (Bootstrap)
 * - syncListBoxAndBadges depende de window.selectedDates existir globalmente
 * - Todos os cálculos silenciosamente falham (limpa campos) se dados inválidos
 * - try-catch aninhados em atualizarListBoxHTMLComDatas (outer + forEach inner)
 * - formatDateLocal usa Intl.DateTimeFormat como fallback (timeZone América/São Paulo)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */

/**
 * Parse int seguro
 * param {*} v - Valor
 * returns {number|null} Número ou null
 */
window.parseIntSafe = function (v)
{
    try
    {
        const n = parseInt(v, 10);
        return Number.isFinite(n) ? n : null;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "parseIntSafe", error);
        return null;
    }
};

/**
 * Formata data para exibição local
 * param {Date} d - Data
 * returns {string} Data formatada
 */
window.formatDateLocal = function (d)
{
    try
    {
        return window.formatDate(d);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "formatDateLocal", error);
        return new Intl.DateTimeFormat("pt-BR", {
            timeZone: "America/Sao_Paulo"
        }).format(d);
    }
};

/**
 * Obtém nome do script atual
 * returns {string} Nome do script
 */
window.__getScriptName = function ()
{
    try
    {
        let script = document.currentScript;
        if (!script)
        {
            const scripts = document.getElementsByTagName("script");
            script = scripts[scripts.length - 1];
        }
        const src = script.src || "";
        const m = src.match(/(ViagemUpsert_\d+)(?:\.min)?\.js$/i);
        return m ? m[1] : "ViagemUpsert";
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "__getScriptName", error);
        return "ViagemUpsert";
    }
};

// Executar e armazenar
window.__scriptName = window.__getScriptName();

/**
 * Remove data da lista (callback para template)
 * param {number} timestamp - Timestamp da data
 */
window.removeDate = function (timestamp)
{
    try
    {
        window.selectedDates = window.selectedDates.filter(d => d.Timestamp !== timestamp);

        // [LOGICA] Atualizar ListBox com datas filtradas (via bridge ej2_instances ou widget Kendo)
        var listBoxWidget = window.getSyncfusionInstance("lstDiasCalendario");
        if (listBoxWidget) {
            if (typeof listBoxWidget.dataSource === 'function') {
                // Kendo widget
                listBoxWidget.dataSource(window.selectedDates);
            } else if (listBoxWidget.dataSource !== undefined) {
                // Syncfusion/bridge compat
                listBoxWidget.dataSource = window.selectedDates;
                if (typeof listBoxWidget.dataBind === 'function') listBoxWidget.dataBind();
            }
        }

        // [LOGICA] Remover data do calendário multi-select
        var calWidget = window.getSyncfusionInstance("calDatasSelecionadas");
        if (calWidget) {
            var currentSelectedDates = calWidget.values || [];
            currentSelectedDates = currentSelectedDates.filter(function(date) {
                var normalizedDate = new Date(date).setHours(0, 0, 0, 0);
                return normalizedDate !== timestamp;
            });
            calWidget.values = currentSelectedDates;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "removeDate", error);
    }
};

/**
 * Calcula distância da viagem
 */
window.calcularDistanciaViagem = function ()
{
    try
    {
        const kmInicialStr = $("#txtKmInicial").val();
        const kmFinalStr = $("#txtKmFinal").val();
        const txtQuilometragem = $("#txtQuilometragem");

        if (!kmInicialStr || !kmFinalStr)
        {
            txtQuilometragem.val("");
            txtQuilometragem.removeClass("distancia-alerta");
            return;
        }

        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
        const kmFinal = parseFloat(kmFinalStr.replace(",", "."));

        if (isNaN(kmInicial) || isNaN(kmFinal))
        {
            txtQuilometragem.val("");
            txtQuilometragem.removeClass("distancia-alerta");
            return;
        }

        const kmPercorrido = Math.round(kmFinal - kmInicial);
        txtQuilometragem.val(kmPercorrido);

        // Aplicar estilo vermelho negrito se > 100km
        if (kmPercorrido > 100)
        {
            txtQuilometragem.addClass("distancia-alerta");
        }
        else
        {
            txtQuilometragem.removeClass("distancia-alerta");
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "calcularDistanciaViagem", error);
    }
};

/**
 * Calcula duração da viagem
 */
window.calcularDuracaoViagem = function ()
{
    try
    {
        const dataInicialStr = $("#txtDataInicial").val();
        const horaInicialStr = $("#txtHoraInicial").val();
        const dataFinalStr = $("#txtDataFinal").val();
        const horaFinalStr = $("#txtHoraFinal").val();

        if (!dataInicialStr || !horaInicialStr || !dataFinalStr || !horaFinalStr)
        {
            $("#txtDuracao").val("");
            return;
        }

        const parseDataHora = (data, hora) =>
        {
            try
            {
                if (data.includes("/"))
                {
                    const [dia, mes, ano] = data.split("/");
                    return new Date(`${ano}-${mes}-${dia}T${hora}`);
                } else if (data.includes("-"))
                {
                    return new Date(`${data}T${hora}`);
                } else
                {
                    return null;
                }
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("formatters.js", "parseDataHora", error);
                return null;
            }
        };

        const dtInicial = parseDataHora(dataInicialStr, horaInicialStr);
        const dtFinal = parseDataHora(dataFinalStr, horaFinalStr);

        if (!dtInicial || !dtFinal || dtFinal <= dtInicial)
        {
            $("#txtDuracao").val("");
            return;
        }

        const diffMs = dtFinal - dtInicial;
        const diffHoras = (diffMs / (1000 * 60 * 60)).toFixed(2);
        $("#txtDuracao").val(Math.round(diffHoras));
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "calcularDuracaoViagem", error);
    }
};

/**
 * Sincroniza listbox e badges com datas
 */
window.syncListBoxAndBadges = function ()
{
    try
    {
        // [LOGICA] Atualizar ListBox com datas selecionadas (via bridge)
        var listBoxWidget = window.getSyncfusionInstance("lstDiasCalendario");
        if (listBoxWidget && window.selectedDates) {
            if (typeof listBoxWidget.dataSource === 'function') {
                listBoxWidget.dataSource(window.selectedDates);
            } else if (listBoxWidget.dataSource !== undefined) {
                listBoxWidget.dataSource = window.selectedDates;
                if (typeof listBoxWidget.dataBind === 'function') listBoxWidget.dataBind();
            }
        }

        const totalItems = window.selectedDates ? window.selectedDates.length : 0;
        const badge1 = document.getElementById("itensBadge");
        const badge2 = document.getElementById("itensBadgeHTML");

        if (badge1) badge1.textContent = totalItems;
        if (badge2) badge2.textContent = totalItems;
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "syncListBoxAndBadges", error);
    }
};

/**
 * Atualiza listbox HTML com datas
 * param {Array} datas - Array de datas
 */
window.atualizarListBoxHTMLComDatas = function (datas)
{
    try
    {
        window.datasSelecionadas = [...datas];

        const listBoxHTML = document.getElementById("lstDiasCalendarioHTML");
        const divDiasSelecionados = document.getElementById("diasSelecionadosTexto");

        if (!listBoxHTML) return;

        listBoxHTML.innerHTML = "";
        let contDatas = 0;

        datas.forEach(data =>
        {
            try
            {
                const dataFormatada = moment(data).format("DD/MM/YYYY");
                const li = document.createElement("li");
                li.className = "list-group-item d-flex justify-content-between align-items-center";
                li.innerHTML = `<span>${dataFormatada}</span>`;
                listBoxHTML.appendChild(li);
                contDatas += 1;
            } catch (error)
            {
                Alerta.TratamentoErroComLinha("formatters.js", "atualizarListBoxHTMLComDatas_forEach", error);
            }
        });

        const badge2 = document.getElementById("itensBadgeHTML");
        if (badge2) badge2.textContent = contDatas;

        if (divDiasSelecionados)
        {
            divDiasSelecionados.textContent = `Dias Selecionados (${datas.length})`;
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha("formatters.js", "atualizarListBoxHTMLComDatas", error);
    }
};
