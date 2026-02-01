/* ****************************************************************************************
 * ⚡ ARQUIVO: error-hook-global_001.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Hook global para capturar erros HTTP 5xx (ProblemDetails JSON) de
 *                   AJAX/fetch/axios e exibir alertas usando SweetAlert. Intercepta
 *                   chamadas HTTP, extrai correlationId, deduplica e mostra erros.
 * 📥 ENTRADAS     : Respostas HTTP de jQuery.ajax, fetch(), axios com status >= 500
 * 📤 SAÍDAS       : SweetAlert modals, console logs, chamadas Alerta.TratamentoErroComLinha
 * 🔗 CHAMADA POR  : Auto-execução IIFE, interceptores globais (jQuery, fetch, axios)
 * 🔄 CHAMA        : SweetAlertInterop.ShowError, Alerta.TratamentoErroComLinha, console.*
 * 📦 DEPENDÊNCIAS : jQuery (opcional), SweetAlertInterop.js, Alerta.js
 * 📝 OBSERVAÇÕES  : IIFE auto-executável, dedupe via correlationId (10s window), hooks
 *                   globais para jQuery.ajaxError, window.fetch override, axios interceptor
 *
 * 📋 ÍNDICE DE FUNÇÕES (4 funções + 3 interceptores):
 *
 * ┌─ FUNÇÕES UTILITÁRIAS ──────────────────────────────────────────────────┐
 * │ 1. extractProblem(raw)                                                  │
 * │    → Extrai detalhes de ProblemDetails JSON de resposta HTTP           │
 * │    → Retorna { detail, corr, problem }                                 │
 * │    → Parse de obj.detail, obj.title, obj.correlationId                 │
 * │                                                                         │
 * │ 2. showProblem(detail, corr, source)                                   │
 * │    → Exibe erro usando SweetAlert ou Alerta.TratamentoErroComLinha    │
 * │    → Prioridade: SweetAlertInterop > Alerta > console.error           │
 * │    → Formato: "mensagem\nCódigo de correlação: {corr}"                │
 * │                                                                         │
 * │ 3. shouldHandle(status)                                                │
 * │    → Verifica se status HTTP deve ser tratado (>= 500)                │
 * │    → Retorna boolean                                                   │
 * │                                                                         │
 * │ 4. stamp(corr)                                                         │
 * │    → Marca correlationId como visto (dedupe)                           │
 * │    → Retorna false se duplicado, true se novo                          │
 * │    → Auto-limpa após 10 segundos via setTimeout                        │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INTERCEPTORES GLOBAIS ────────────────────────────────────────────────┐
 * │ 5. jQuery ajaxError handler                                            │
 * │    → Intercepta todos os erros jQuery.ajax globalmente                │
 * │    → Captura xhr.responseJSON ou xhr.responseText                     │
 * │    → Chama extractProblem, stamp, showProblem                         │
 * │                                                                         │
 * │ 6. window.fetch override                                               │
 * │    → Sobrescreve fetch() nativo para interceptar respostas            │
 * │    → Clona response para ler body sem consumir stream                 │
 * │    → Marca como hooked com window.__fxFetchHooked flag                │
 * │                                                                         │
 * │ 7. axios.interceptors.response                                         │
 * │    → Adiciona interceptor de erro no axios (se disponível)            │
 * │    → Captura err.response.data                                        │
 * │    → Marca como hooked com window.__fxAxiosHooked flag                │
 * └─────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 FLUXO DE CAPTURA:
 * 1. Request HTTP (jQuery/fetch/axios)
 * 2. Response com status >= 500
 * 3. Interceptor captura response
 * 4. extractProblem() extrai detail + correlationId do JSON
 * 5. shouldHandle() verifica se status >= 500
 * 6. stamp() verifica dedupe (retorna false se duplicado)
 * 7. showProblem() exibe SweetAlert ou Alerta
 *
 * 📌 PROBLEMDETAILS JSON STRUCTURE (ASP.NET Core):
 * {
 *   "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
 *   "title": "Erro interno do servidor",
 *   "status": 500,
 *   "detail": "Mensagem de erro amigável",
 *   "correlationId": "abc123xyz",
 *   "extensions": { ... }
 * }
 *
 * 📝 OBSERVAÇÕES ADICIONAIS:
 * - Dedupe window: 10 segundos (Set seen, setTimeout clear)
 * - fetch() intercept: clona response para não consumir stream
 * - axios: rejeita Promise.reject(err) após processar
 * - jQuery: usa document.ajaxError (global)
 * - Flags de hook: __fxFetchHooked, __fxAxiosHooked (previne double-hook)
 *
 * 🔌 VERSÃO: 1.0
 * 📌 ÚLTIMA ATUALIZAÇÃO: 01/02/2026
 **************************************************************************************** */
(function () {
    'use strict';

    var seen = new Set();

    function extractProblem(raw) {
        try {
            var obj = null;
            if (raw && typeof raw === 'object') {
                obj = raw;
            } else if (typeof raw === 'string') {
                try { obj = JSON.parse(raw); } catch { obj = {}; }
            } else {
                obj = raw || {};
            }

            var detail = (obj && (obj.detail || obj.title)) || 'Erro inesperado.';
            var ext = (obj && (obj.extensions || obj)) || {};
            var corr = obj.correlationId || ext.correlationId || null;
            return { detail: detail, corr: corr, problem: obj };
        } catch (erro) {
            console.error('[FrotiX ErrorHook] Erro em extractProblem:', erro);
            return { detail: 'Erro inesperado.', corr: null, problem: {} };
        }
    }

    function showProblem(detail, corr, source) {
        try {
            var msg = detail + (corr ? "\nCódigo de correlação: " + corr : "");
            if (window.SweetAlertInterop && typeof SweetAlertInterop.ShowError === "function") {
                SweetAlertInterop.ShowError("Falha na operação", msg);
            } else if (window.Alerta && typeof Alerta.TratamentoErroComLinha === "function") {
                Alerta.TratamentoErroComLinha(source || "ajax", "global", new Error(detail + (corr ? " | corr=" + corr : "")));
            } else {
                console.error("[FrotiX ErrorHook]", msg);
            }
        } catch (e) {
            console.error("[FrotiX ErrorHook] showProblem falhou:", e);
        }
    }

    function shouldHandle(status) {
        try {
            return Number(status) >= 500;
        } catch (erro) {
            console.error('[FrotiX ErrorHook] Erro em shouldHandle:', erro);
            return false;
        }
    }

    function stamp(corr) {
        try {
            if (!corr) return true;
            if (seen.has(corr)) return false;
            seen.add(corr);
            setTimeout(function () { try { seen.delete(corr); } catch {} }, 10000);
            return true;
        } catch (erro) {
            console.error('[FrotiX ErrorHook] Erro em stamp:', erro);
            return true;
        }
    }

    // jQuery global (cobre $.ajax, DataTables, etc.)
    if (window.jQuery) {
        jQuery(document).ajaxError(function (_evt, xhr) {
            try {
                if (!shouldHandle(xhr && xhr.status)) return;
                var raw = (xhr && (xhr.responseJSON || xhr.responseText)) || {};
                var meta = extractProblem(raw);
                if (stamp(meta.corr)) showProblem(meta.detail, meta.corr, "ajax");
            } catch (e) {
                console.error("[FrotiX ErrorHook] ajaxError:", e);
            }
        });
    }

    // fetch() intercept
    if (window.fetch && !window.__fxFetchHooked) {
        var originalFetch = window.fetch;
        window.fetch = async function (input, init) {
            var resp = await originalFetch(input, init);
            try {
                if (!resp.ok && shouldHandle(resp.status)) {
                    var clone = resp.clone();
                    var text = "";
                    try { text = await clone.text(); } catch {}
                    var meta = extractProblem(text);
                    if (stamp(meta.corr)) showProblem(meta.detail, meta.corr, "fetch");
                }
            } catch (e) {
                console.error("[FrotiX ErrorHook] fetch intercept:", e);
            }
            return resp;
        };
        window.__fxFetchHooked = true;
    }

    // axios intercept (se presente)
    if (window.axios && axios.interceptors && !window.__fxAxiosHooked) {
        axios.interceptors.response.use(
            function (r) { return r; },
            function (err) {
                try {
                    var status = (err && err.response && err.response.status) || 0;
                    if (shouldHandle(status)) {
                        var data = err.response && err.response.data;
                        var meta = extractProblem(data);
                        if (stamp(meta.corr)) showProblem(meta.detail, meta.corr, "axios");
                    }
                } catch (e) {
                    console.error("[FrotiX ErrorHook] axios intercept:", e);
                }
                return Promise.reject(err);
            }
        );
        window.__fxAxiosHooked = true;
    }
})();
