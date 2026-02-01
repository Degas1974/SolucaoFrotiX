/* ****************************************************************************************
 * ⚡ ARQUIVO: dt-better-errors.js
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Sistema avançado de tratamento de erros para DataTables com filas,
 *                   dedupe, coalescência e integração com Alerta.* do FrotiX. Suprime
 *                   alertas nativos, captura error.dt/xhr.dt/preXhr.dt, exibe modals.
 * 📥 ENTRADAS     : Eventos DataTables (error.dt, xhr.dt, preXhr.dt), opções de config
 * 📤 SAÍDAS       : Bootstrap modals com sugestões, logs no console, Alerta.* calls
 * 🔗 CHAMADA POR  : DTBetterErrors.enable(selector, opts) em páginas com DataTables
 * 🔄 CHAMA        : Alerta.TratamentoErroComLinhaEnriquecido, Bootstrap modals, DataTables API
 * 📦 DEPENDÊNCIAS : jQuery, DataTables ($.fn.dataTable), Bootstrap 5 (optional), Alerta.js
 * 📝 OBSERVAÇÕES  : IIFE com global exports (window.DTBetterErrors), Promise queues para
 *                   execução sequencial de alertas/modals, dedupe window de 3s padrão
 *
 * 📋 ÍNDICE DE FUNÇÕES (20 funções principais):
 *
 * ┌─ API PÚBLICA (window.DTBetterErrors.*) ────────────────────────────────────┐
 * │ 1. enable(selector, opts)                                                   │
 * │    → Ativa better errors para DataTable específica                         │
 * │    → Binds: error.dt, xhr.dt, preXhr.dt (namespaced .dtBetterErrors)       │
 * │    → State management: lastPreXhrUrl, lastXhrErrAt, lastXhrSig             │
 * │                                                                             │
 * │ 2. disable(selector)                                                        │
 * │    → Remove event handlers .dtBetterErrors de uma tabela                   │
 * │                                                                             │
 * │ 3. setGlobalOptions(opts)                                                   │
 * │    → Define opções globais para todas as tabelas                           │
 * │    → Merge com DEFAULTS                                                    │
 * │                                                                             │
 * │ 4. getLogs()                                                                │
 * │    → Retorna cópia do array de logs internos (_logs.slice())               │
 * │                                                                             │
 * │ 5. clearLogs()                                                              │
 * │    → Limpa array de logs (_logs.length = 0)                                │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ INFRAESTRUTURA ───────────────────────────────────────────────────────────┐
 * │ 6. queue()                                                                  │
 * │    → Factory para fila de Promises (execução sequencial)                   │
 * │    → Retorna { enqueue(task) } com task = () => Promise                    │
 * │    → Usado em alertaQ e modalQ para evitar sobreposição                    │
 * │                                                                             │
 * │ 7. ensureErrModeDisabled()                                                  │
 * │    → Define $.fn.dataTable.ext.errMode = 'none'                            │
 * │    → Suprime alertas nativos do DataTables                                 │
 * │    → Retry com $(function) se DT não carregou ainda                        │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MANIPULADORES DE ERROS ───────────────────────────────────────────────────┐
 * │ 8. handleError($table, options, payload)                                    │
 * │    → Handler central para todos os erros                                   │
 * │    → Dedupe via makeKey/seen/stamp (window de 3s padrão)                   │
 * │    → Enfileira sendToAlerta e showModal (Promise queues)                   │
 * │    → Armazena em global.__DT_LAST_ERROR__ para debug                       │
 * │                                                                             │
 * │ 9. sendToAlerta(contexto, origem, payload, options)                         │
 * │    → Envia erro para sistema Alerta.* do FrotiX                            │
 * │    → Tenta TratamentoErroComLinhaEnriquecido (preferido)                   │
 * │    → Fallback para TratamentoErroComLinha                                  │
 * │    → Retorna Promise para fila sequencial                                  │
 * │                                                                             │
 * │ 10. hasAlerta()                                                             │
 * │     → Verifica se global.Alerta.* está disponível                          │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ MODAL E RENDERIZAÇÃO ─────────────────────────────────────────────────────┐
 * │ 11. showModal(payload, options)                                             │
 * │     → Exibe Bootstrap modal com erro formatado                             │
 * │     → Fallback para alert() se Bootstrap não disponível                    │
 * │     → Promise resolve em hidden.bs.modal (para fila sequencial)            │
 * │                                                                             │
 * │ 12. render({ title, summary, suggestions, raw })                            │
 * │     → Renderiza HTML do modal com título, resumo, lista de sugestões       │
 * │     → <details> com JSON.stringify(raw) em <pre>                           │
 * │     → Escapa HTML via esc()                                                │
 * │                                                                             │
 * │ 13. ensureModal()                                                           │
 * │     → Cria elemento #dtErrorModal no DOM se não existir                    │
 * │     → Template Bootstrap 5 (modal-lg, modal-dialog-scrollable)             │
 * │                                                                             │
 * │ 14. esc(s)                                                                  │
 * │     → Escapa HTML entities (&, <, >, ", ')                                 │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ EXPLICADORES DE ERROS ────────────────────────────────────────────────────┐
 * │ 15. explainErrorMessage(message, settings, dtApi)                           │
 * │     → Traduz mensagens técnicas do DataTables para texto amigável         │
 * │     → "Requested unknown parameter" → sugestões de defaultContent          │
 * │     → "Invalid JSON response" → sugestões de dataSrc                       │
 * │     → Retorna { summary, suggestions[] }                                   │
 * │                                                                             │
 * │ 16. getDataSrc(dtApi, colSettings)                                          │
 * │     → Obtém data source de uma coluna (tenta dtApi.column().dataSrc())     │
 * │     → Fallback para colSettings.mData ou colSettings.data                  │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * ┌─ LOGGING E DEDUPE ─────────────────────────────────────────────────────────┐
 * │ 17. log(options, level, msg, extra)                                         │
 * │     → Adiciona entry em _logs[] com timestamp                              │
 * │     → Console.log se options.logToConsole = true                           │
 * │     → Prefixo "[DTBetterErrors]"                                           │
 * │                                                                             │
 * │ 18. makeKey(ctx, orig, payload)                                             │
 * │     → Cria chave de dedupe (500 chars max)                                 │
 * │     → Formato: "contexto|origem|title|summary"                             │
 * │                                                                             │
 * │ 19. seen(key, windowMs)                                                     │
 * │     → Verifica se key foi vista no windowMs (Map _dedupe)                  │
 * │     → Retorna true se duplicada                                            │
 * │                                                                             │
 * │ 20. stamp(key)                                                              │
 * │     → Marca key como vista com timestamp atual (_dedupe.set)               │
 * └─────────────────────────────────────────────────────────────────────────────┘
 *
 * 🔄 EVENTOS DATATABLES CAPTURADOS:
 * - preXhr.dt → Captura URL do endpoint antes da request (state.lastPreXhrUrl)
 * - xhr.dt → Captura erros HTTP 4xx/5xx (marca xhr.__dtBetterHandled)
 * - error.dt → Captura erros internos DT (Ajax error, unknown parameter, invalid JSON)
 *
 * 🎯 COALESCÊNCIA (Coalescing):
 * - Quando xhr.dt dispara (HTTP erro), marca state.lastXhrErrAt
 * - Se error.dt "Ajax error" dispara em <2s após xhr.dt, suprime (deduplica)
 * - Evita alertas duplicados para o mesmo erro HTTP
 *
 * 📌 PAYLOAD STRUCTURE:
 * {
 *   title: string,        // "Erro ao carregar dados (AJAX)"
 *   summary: string,      // Descrição resumida
 *   suggestions: string[],// Lista de sugestões ao desenvolvedor
 *   raw: object,          // Dados técnicos (status, url, responsePreview, etc)
 *   dedupeKey?: string    // Chave customizada para dedupe (opcional)
 * }
 *
 * 📌 DEFAULTS CONFIG:
 * {
 *   contexto: 'DataTable',
 *   origem: 'AJAX.DataTable',
 *   encaminharParaAlerta: true,
 *   preferEnriquecido: true,
 *   showModal: true,
 *   logToConsole: true,
 *   previewLimit: 1200,          // Max chars de responseText
 *   dedupeWindowMs: 3000,        // Janela de dedupe (3s)
 *   suppressAjaxErrorDt: true,   // Suprimir error.dt "Ajax error" se xhr.dt veio
 *   coalesceWindowMs: 2000       // Janela de coalescência (2s)
 * }
 *
 * 📌 VARIÁVEIS GLOBAIS:
 * - GLOBAL_OPTS (config global), _logs[] (log entries), _dedupe (Map timestamp)
 * - alertaQ, modalQ (Promise queues), window.__DT_LAST_ERROR__ (debug)
 *
 * 🔌 VERSÃO: 1.4.1
 **************************************************************************************** */
(function (global, $)
{
    'use strict';
    if (!$) { console.error('[DTBetterErrors] jQuery é obrigatório.'); return; }

    const DEFAULTS = {
        contexto: 'DataTable',
        origem: 'AJAX.DataTable',
        encaminharParaAlerta: true,
        preferEnriquecido: true,
        showModal: true,
        logToConsole: true,
        previewLimit: 1200,
        dedupeWindowMs: 3000,

        // coalescer error.dt “Ajax error / Erro ao carregar dados”
        suppressAjaxErrorDt: true,
        coalesceWindowMs: 2000
    };

    let GLOBAL_OPTS = {};
    const _logs = [];
    const _dedupe = new Map();
    const alertaQ = queue();
    const modalQ = queue();

    function setGlobalOptions(opts) {
        try {
            GLOBAL_OPTS = Object.assign({}, GLOBAL_OPTS, opts || {});
        } catch (erro) {
            console.error('Erro em setGlobalOptions:', erro);
        }
    }
    function getLogs() {
        try {
            return _logs.slice();
        } catch (erro) {
            console.error('Erro em getLogs:', erro);
            return [];
        }
    }
    function clearLogs() {
        try {
            _logs.length = 0;
        } catch (erro) {
            console.error('Erro em clearLogs:', erro);
        }
    }

    function queue()
    {
        try {
            let current = Promise.resolve();
            return { enqueue(task) { current = current.then(() => task()).catch(e => console.warn('[DTBetterErrors][Queue]', e)); return current; } };
        } catch (erro) {
            console.error('Erro em queue:', erro);
            return { enqueue: () => Promise.resolve() };
        }
    }

    function ensureErrModeDisabled()
    {
        try {
            if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none';
            else $(function () { if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none'; });
        } catch (erro) {
            console.error('Erro em ensureErrModeDisabled:', erro);
        }
    }

    function enable(selector, opts = {})
    {
        try {
            ensureErrModeDisabled();

            const options = Object.assign({}, DEFAULTS, GLOBAL_OPTS, opts);
            const $table = $(selector);
            if (!$table.length) { console.warn('[DTBetterErrors] Tabela não encontrada:', selector); return; }

            // Estado local por tabela
            const state = $table.data('dtBetterState') || {
                lastPreXhrAt: 0, lastPreXhrUrl: '',
                lastXhrErrAt: 0, lastXhrSig: '', lastXhrPayload: null
            };
            $table.data('dtBetterState', state);

            // Evita binds duplicados
            $table.off('.dtBetterErrors');
            $table.data('dtBetterErrorsOptions', options);

            // Guardar URL da requisição (para enriquecer/suprimir error.dt)
            $table.on('preXhr.dt.dtBetterErrors', function (e, settings, data)
            {
                try {
                    const url = settings.ajax && (settings.ajax.url || settings.ajax);
                    state.lastPreXhrUrl = typeof url === 'function' ? '[function]' : (url || '');
                    state.lastPreXhrAt = Date.now();
                    $table.data('dtBetterState', state);
                } catch (erro) {
                    console.error('Erro em preXhr.dt handler:', erro);
                }
            });

            // XHR.DT — HTTP 4xx/5xx etc.
            $table.on('xhr.dt.dtBetterErrors', function (e, settings, json, xhr)
            {
                try {
                    if (!xhr) return;
                    const ok = xhr.status >= 200 && xhr.status < 300;
                    if (ok) return;

                    try { xhr.__dtBetterHandled = true; } catch (_) { }

                    const url = settings.ajax && (settings.ajax.url || settings.ajax);
                    const body = (xhr.responseText || '');
                    const payload = {
                        title: 'Erro ao carregar dados (AJAX)',
                        summary: `HTTP ${xhr.status} ${xhr.statusText || ''} em ${url}`,
                        suggestions: [
                            'Verifique a URL do endpoint e se ele está acessível.',
                            'Confirme que o servidor retorna JSON válido (Content-Type: application/json).',
                            'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
                            'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".',
                            'Confira CORS, autenticação/token e logs do servidor.'
                        ],
                        raw: { status: xhr.status, statusText: xhr.statusText || '', url, responsePreview: body.slice(0, options.previewLimit) },
                        dedupeKey: `XHR|${settings.sTableId || ''}|${url || ''}|${xhr.status}`
                    };

                    // marca estado p/ coalescer o error.dt subsequente
                    state.lastXhrErrAt = Date.now();
                    state.lastXhrSig = payload.dedupeKey;
                    state.lastXhrPayload = payload;
                    $table.data('dtBetterState', state);

                    handleError($table, options, payload);
                } catch (erro) {
                    console.error('Erro em xhr.dt handler:', erro);
                }
            });

            // ERROR.DT — mensageria interna (ex.: "Ajax error / tn/7")
            $table.on('error.dt.dtBetterErrors', function (e, settings, techNote, message)
            {
                try {
                    e.preventDefault(); // suprime alerta nativo
                    const msg = (message || '').toString();
                    const isAjaxErrorMsg = /Ajax error/i.test(msg) || /Erro ao carregar dados/i.test(msg) || /datatables\.net\/tn\/7/i.test(msg);

                    // Coalescer com xhr.dt (se veio logo antes)
                    if (options.suppressAjaxErrorDt && isAjaxErrorMsg)
                    {
                        const windowMs = options.coalesceWindowMs || 2000;
                        if (state.lastXhrErrAt && (Date.now() - state.lastXhrErrAt) < windowMs)
                        {
                            log(options, 'info', 'Coalescido: error.dt Ajax error suprimido (já houve xhr.dt)', { message, state });
                            return;
                        }
                    }

                    // Sem xhr.dt recente, montamos um fallback útil com a URL do preXhr
                    if (isAjaxErrorMsg)
                    {
                        const url = state.lastPreXhrUrl || (settings.ajax && (settings.ajax.url || settings.ajax)) || '';
                        const payload = {
                            title: 'Erro ao carregar dados (AJAX)',
                            summary: `Falha ao carregar via AJAX em ${url || '(URL não disponível)'}`,
                            suggestions: [
                                'Cheque a aba Network do DevTools (status HTTP, corpo e Content-Type).',
                                'Se o servidor retorna HTML (ex.: página 404), ajuste a rota ou autenticação.',
                                'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
                                'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".'
                            ],
                            raw: { message: msg, techNote, url },
                            // chave compatível com XHR para dedupe/coalescência
                            dedupeKey: `ERR|${settings.sTableId || ''}|${url || ''}|AJAX`
                        };
                        handleError($table, options, payload);
                        return;
                    }

                    // Demais erros (unknown parameter, invalid JSON etc.)
                    const dtApi = $(settings.nTable).DataTable();
                    const human = explainErrorMessage(msg, settings, dtApi);
                    const payload = {
                        title: 'Erro no DataTables',
                        summary: human.summary,
                        suggestions: human.suggestions,
                        raw: {
                            message: msg, techNote, tableId: settings.sTableId,
                            columns: settings.aoColumns?.map(c => getDataSrc(dtApi, c)) || []
                        }
                    };
                    handleError($table, options, payload);
                } catch (erro) {
                    console.error('Erro em error.dt handler:', erro);
                }
            });
        } catch (erro) {
            console.error('Erro em enable:', erro);
        }
    }

    function disable(selector) {
        try {
            const $t = $(selector);
            $t.off('.dtBetterErrors');
            $t.removeData('dtBetterErrorsOptions');
        } catch (erro) {
            console.error('Erro em disable:', erro);
        }
    }

    function handleError($table, options, payload)
    {
        try {
            const contexto = options.contexto || ($table.attr('id') || 'DataTable');
            const origem = String(options.origem || 'AJAX.DataTable');

            // Dedupe: usa chave específica se fornecida (xhr/err coalescidos)
            const key = (payload.dedupeKey || makeKey(contexto, origem, payload));
            if (seen(key, options.dedupeWindowMs)) { log(options, 'info', 'Erro duplicado suprimido', { contexto, origem, payload }); return; }
            stamp(key);

            if (options.encaminharParaAlerta && hasAlerta()) alertaQ.enqueue(() => sendToAlerta(contexto, origem, payload, options));
            if (options.showModal) modalQ.enqueue(() => showModal(payload, options));

            log(options, 'error', 'Erro DataTables', { contexto, origem, payload });
            global.__DT_LAST_ERROR__ = { contexto, origem, payload, ts: Date.now() };
        } catch (erro) {
            console.error('Erro em handleError:', erro);
        }
    }

    function hasAlerta()
    {
        try {
            return global.Alerta &&
                (typeof global.Alerta.TratamentoErroComLinha === 'function' ||
                    typeof global.Alerta.TratamentoErroComLinhaEnriquecido === 'function');
        } catch (erro) {
            console.error('Erro em hasAlerta:', erro);
            return false;
        }
    }

    function sendToAlerta(contexto, origem, payload, options)
    {
        return new Promise((resolve) =>
        {
            try
            {
                const err = new Error(payload.summary || payload.title || 'Erro DataTables');
                err.name = 'DataTablesError';
                err.details = payload.raw || {};
                err.__dtBetterPayload = payload;
                let ret;
                if (options.preferEnriquecido && typeof global.Alerta.TratamentoErroComLinhaEnriquecido === 'function')
                {
                    ret = global.Alerta.TratamentoErroComLinhaEnriquecido(contexto, origem, err, { fonte: 'DTBetterErrors' });
                } else if (typeof global.Alerta.TratamentoErroComLinha === 'function')
                {
                    ret = global.Alerta.TratamentoErroComLinha(contexto, origem, err);
                }
                if (ret && typeof ret.then === 'function') ret.then(() => resolve()).catch(() => resolve());
                else resolve();
            } catch (ex) { console.warn('[DTBetterErrors] Falha ao chamar Alerta.*:', ex); resolve(); }
        });
    }

    function showModal(payload, options)
    {
        return new Promise((resolve) =>
        {
            const hasBS = !!global.bootstrap || !!$.fn.modal;
            if (!hasBS) { try { alert(`${payload.title}\n\n${payload.summary}\n\n- ${(payload.suggestions || []).join('\n- ')}`); } catch (_) { } resolve(); return; }
            ensureModal(); $('#dtErrorModal .modal-body').html(render(payload));
            const el = document.getElementById('dtErrorModal');
            if (global.bootstrap)
            {
                const m = new bootstrap.Modal(el);
                el.addEventListener('hidden.bs.modal', () => resolve(), { once: true });
                m.show();
            } else
            {
                $('#dtErrorModal').one('hidden.bs.modal', () => resolve()).modal('show');
            }
        });
    }

    function render({ title, summary, suggestions = [], raw = {} })
    {
        try {
            return `
      <div class="mb-2"><strong>${esc(title || 'Erro')}</strong></div>
      <div class="mb-2">${esc(summary || '')}</div>
      ${suggestions.length ? `<ul class="mb-2">${suggestions.map(s => `<li>${esc(s)}</li>`).join('')}</ul>` : ''}
      <details open><summary>Detalhes técnicos</summary>
        <pre class="mt-2 p-2 bg-light border rounded small">${esc(JSON.stringify(raw, null, 2))}</pre>
      </details>
    `;
        } catch (erro) {
            console.error('Erro em render:', erro);
            return '<div>Erro ao renderizar mensagem de erro</div>';
        }
    }

    function explainErrorMessage(message, settings, dtApi)
    {
        try {
            let summary = message; const suggestions = [];
            if (/Requested unknown parameter/i.test(message))
            {
                const m = message.match(/column\s+(\d+)/i); const idx = m ? parseInt(m[1], 10) : null;
                const dataSrc = (idx != null && dtApi?.column) ? dtApi.column(idx).dataSrc() : undefined;
                summary = 'Coluna configurada com um campo que não existe no dataset.';
                if (idx != null) suggestions.push(`A coluna #${idx} usa \`data\` = "${dataSrc}". Confirme que cada registro do JSON possui essa chave.`);
                else suggestions.push('Uma coluna usa `data` inexistente em algumas linhas do JSON.');
                suggestions.push('Se algumas linhas não tiverem o campo, defina `columns[n].defaultContent = ""`.',
                    'Se o servidor retorna array (posicional), use `columns[n].data = n` (índice).',
                    'Compare as chaves reais do payload com `columns[].data`.');
            }
            if (/Invalid JSON response/i.test(message))
            {
                summary = 'A resposta do servidor não é um JSON válido para o DataTables.';
                suggestions.push('O servidor pode estar retornando HTML de erro (500) ou JSON malformado.',
                    'Garanta `Content-Type: application/json` e JSON bem formado.',
                    'Se o array está na raiz, use `ajax: { dataSrc: "" }`.',
                    'Se vier `{ "data": [...] }`, ajuste `ajax.dataSrc` para "data".');
            }
            if (suggestions.length === 0)
            {
                suggestions.push('Abra o console para ver o erro completo e o objeto `settings`.',
                    'Revise o mapeamento `columns[].data` e o `ajax.dataSrc`.',
                    'Em `render`, trate campos ausentes com `row?.campo ?? ""`.');
            }
            return { summary, suggestions };
        } catch (erro) {
            console.error('Erro em explainErrorMessage:', erro);
            return { summary: message, suggestions: ['Erro ao processar mensagem de erro'] };
        }
    }

    function getDataSrc(dtApi, colSettings) { try { return dtApi.column(colSettings.idx).dataSrc(); } catch { return colSettings?.mData ?? colSettings?.data; } }

    function ensureModal()
    {
        try {
            if (document.getElementById('dtErrorModal')) return;
            const tpl = `
    <div class="modal fade" id="dtErrorModal" tabindex="-1" aria-labelledby="dtErrorModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg modal-dialog-scrollable"><div class="modal-content">
        <div class="modal-header"><h5 class="modal-title" id="dtErrorModalLabel">Detalhes do erro</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Fechar"></button></div>
        <div class="modal-body"></div>
        <div class="modal-footer"><button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button></div>
      </div></div>
    </div>`;
            document.body.insertAdjacentHTML('beforeend', tpl);
        } catch (erro) {
            console.error('Erro em ensureModal:', erro);
        }
    }

    function esc(s) {
        try {
            return String(s).replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]));
        } catch (erro) {
            console.error('Erro em esc:', erro);
            return String(s);
        }
    }

    function log(options, level, msg, extra)
    {
        const e = { level, msg, extra, ts: Date.now() }; _logs.push(e);
        if (!options.logToConsole) return;
        const p = '[DTBetterErrors]';
        try { (console[level] || console.log).call(console, p, msg, extra); } catch (_) { }
    }

    function makeKey(ctx, orig, payload)
    {
        try {
            const base = `${ctx}|${orig}|${payload.title}|${payload.summary}`;
            return base.replace(/\s+/g, ' ').trim().slice(0, 500);
        } catch (erro) {
            console.error('Erro em makeKey:', erro);
            return `${ctx}|${orig}|ERROR`;
        }
    }
    function seen(key, windowMs) {
        try {
            const last = _dedupe.get(key);
            if (!last) return false;
            return (Date.now() - last) < windowMs;
        } catch (erro) {
            console.error('Erro em seen:', erro);
            return false;
        }
    }
    function stamp(key) {
        try {
            _dedupe.set(key, Date.now());
        } catch (erro) {
            console.error('Erro em stamp:', erro);
        }
    }

    // API pública
    global.DTBetterErrors = {
        enable, disable, setGlobalOptions, getLogs, clearLogs, version: '1.4.1'
    };
})(window, window.jQuery);
