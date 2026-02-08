# wwwroot/js/dt-better-errors.js

**Mudanca:** GRANDE | **+248** linhas | **-146** linhas

---

```diff
--- JANEIRO: wwwroot/js/dt-better-errors.js
+++ ATUAL: wwwroot/js/dt-better-errors.js
@@ -23,153 +23,214 @@
     const alertaQ = queue();
     const modalQ = queue();
 
-    function setGlobalOptions(opts) { GLOBAL_OPTS = Object.assign({}, GLOBAL_OPTS, opts || {}); }
-    function getLogs() { return _logs.slice(); }
-    function clearLogs() { _logs.length = 0; }
+    function setGlobalOptions(opts) {
+        try {
+            GLOBAL_OPTS = Object.assign({}, GLOBAL_OPTS, opts || {});
+        } catch (erro) {
+            console.error('Erro em setGlobalOptions:', erro);
+        }
+    }
+    function getLogs() {
+        try {
+            return _logs.slice();
+        } catch (erro) {
+            console.error('Erro em getLogs:', erro);
+            return [];
+        }
+    }
+    function clearLogs() {
+        try {
+            _logs.length = 0;
+        } catch (erro) {
+            console.error('Erro em clearLogs:', erro);
+        }
+    }
 
     function queue()
     {
-        let current = Promise.resolve();
-        return { enqueue(task) { current = current.then(() => task()).catch(e => console.warn('[DTBetterErrors][Queue]', e)); return current; } };
+        try {
+            let current = Promise.resolve();
+            return { enqueue(task) { current = current.then(() => task()).catch(e => console.warn('[DTBetterErrors][Queue]', e)); return current; } };
+        } catch (erro) {
+            console.error('Erro em queue:', erro);
+            return { enqueue: () => Promise.resolve() };
+        }
     }
 
     function ensureErrModeDisabled()
     {
-        if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none';
-        else $(function () { if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none'; });
+        try {
+            if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none';
+            else $(function () { if ($.fn?.dataTable?.ext) $.fn.dataTable.ext.errMode = 'none'; });
+        } catch (erro) {
+            console.error('Erro em ensureErrModeDisabled:', erro);
+        }
     }
 
     function enable(selector, opts = {})
     {
-        ensureErrModeDisabled();
-
-        const options = Object.assign({}, DEFAULTS, GLOBAL_OPTS, opts);
-        const $table = $(selector);
-        if (!$table.length) { console.warn('[DTBetterErrors] Tabela não encontrada:', selector); return; }
-
-        const state = $table.data('dtBetterState') || {
-            lastPreXhrAt: 0, lastPreXhrUrl: '',
-            lastXhrErrAt: 0, lastXhrSig: '', lastXhrPayload: null
-        };
-        $table.data('dtBetterState', state);
-
-        $table.off('.dtBetterErrors');
-        $table.data('dtBetterErrorsOptions', options);
-
-        $table.on('preXhr.dt.dtBetterErrors', function (e, settings, data)
-        {
-            const url = settings.ajax && (settings.ajax.url || settings.ajax);
-            state.lastPreXhrUrl = typeof url === 'function' ? '[function]' : (url || '');
-            state.lastPreXhrAt = Date.now();
+        try {
+            ensureErrModeDisabled();
+
+            const options = Object.assign({}, DEFAULTS, GLOBAL_OPTS, opts);
+            const $table = $(selector);
+            if (!$table.length) { console.warn('[DTBetterErrors] Tabela não encontrada:', selector); return; }
+
+            const state = $table.data('dtBetterState') || {
+                lastPreXhrAt: 0, lastPreXhrUrl: '',
+                lastXhrErrAt: 0, lastXhrSig: '', lastXhrPayload: null
+            };
             $table.data('dtBetterState', state);
-        });
-
-        $table.on('xhr.dt.dtBetterErrors', function (e, settings, json, xhr)
-        {
-            if (!xhr) return;
-            const ok = xhr.status >= 200 && xhr.status < 300;
-            if (ok) return;
-
-            try { xhr.__dtBetterHandled = true; } catch (_) { }
-
-            const url = settings.ajax && (settings.ajax.url || settings.ajax);
-            const body = (xhr.responseText || '');
-            const payload = {
-                title: 'Erro ao carregar dados (AJAX)',
-                summary: `HTTP ${xhr.status} ${xhr.statusText || ''} em ${url}`,
-                suggestions: [
-                    'Verifique a URL do endpoint e se ele está acessível.',
-                    'Confirme que o servidor retorna JSON válido (Content-Type: application/json).',
-                    'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
-                    'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".',
-                    'Confira CORS, autenticação/token e logs do servidor.'
-                ],
-                raw: { status: xhr.status, statusText: xhr.statusText || '', url, responsePreview: body.slice(0, options.previewLimit) },
-                dedupeKey: `XHR|${settings.sTableId || ''}|${url || ''}|${xhr.status}`
-            };
-
-            state.lastXhrErrAt = Date.now();
-            state.lastXhrSig = payload.dedupeKey;
-            state.lastXhrPayload = payload;
-            $table.data('dtBetterState', state);
-
-            handleError($table, options, payload);
-        });
-
-        $table.on('error.dt.dtBetterErrors', function (e, settings, techNote, message)
-        {
-            e.preventDefault();
-            const msg = (message || '').toString();
-            const isAjaxErrorMsg = /Ajax error/i.test(msg) || /Erro ao carregar dados/i.test(msg) || /datatables\.net\/tn\/7/i.test(msg);
-
-            if (options.suppressAjaxErrorDt && isAjaxErrorMsg)
-            {
-                const windowMs = options.coalesceWindowMs || 2000;
-                if (state.lastXhrErrAt && (Date.now() - state.lastXhrErrAt) < windowMs)
-                {
-                    log(options, 'info', 'Coalescido: error.dt Ajax error suprimido (já houve xhr.dt)', { message, state });
-                    return;
+
+            $table.off('.dtBetterErrors');
+            $table.data('dtBetterErrorsOptions', options);
+
+            $table.on('preXhr.dt.dtBetterErrors', function (e, settings, data)
+            {
+                try {
+                    const url = settings.ajax && (settings.ajax.url || settings.ajax);
+                    state.lastPreXhrUrl = typeof url === 'function' ? '[function]' : (url || '');
+                    state.lastPreXhrAt = Date.now();
+                    $table.data('dtBetterState', state);
+                } catch (erro) {
+                    console.error('Erro em preXhr.dt handler:', erro);
                 }
-            }
-
-            if (isAjaxErrorMsg)
-            {
-                const url = state.lastPreXhrUrl || (settings.ajax && (settings.ajax.url || settings.ajax)) || '';
-                const payload = {
-                    title: 'Erro ao carregar dados (AJAX)',
-                    summary: `Falha ao carregar via AJAX em ${url || '(URL não disponível)'}`,
-                    suggestions: [
-                        'Cheque a aba Network do DevTools (status HTTP, corpo e Content-Type).',
-                        'Se o servidor retorna HTML (ex.: página 404), ajuste a rota ou autenticação.',
-                        'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
-                        'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".'
-                    ],
-                    raw: { message: msg, techNote, url },
-
-                    dedupeKey: `ERR|${settings.sTableId || ''}|${url || ''}|AJAX`
-                };
-                handleError($table, options, payload);
-                return;
-            }
-
-            const dtApi = $(settings.nTable).DataTable();
-            const human = explainErrorMessage(msg, settings, dtApi);
-            const payload = {
-                title: 'Erro no DataTables',
-                summary: human.summary,
-                suggestions: human.suggestions,
-                raw: {
-                    message: msg, techNote, tableId: settings.sTableId,
-                    columns: settings.aoColumns?.map(c => getDataSrc(dtApi, c)) || []
+            });
+
+            $table.on('xhr.dt.dtBetterErrors', function (e, settings, json, xhr)
+            {
+                try {
+                    if (!xhr) return;
+                    const ok = xhr.status >= 200 && xhr.status < 300;
+                    if (ok) return;
+
+                    try { xhr.__dtBetterHandled = true; } catch (_) { }
+
+                    const url = settings.ajax && (settings.ajax.url || settings.ajax);
+                    const body = (xhr.responseText || '');
+                    const payload = {
+                        title: 'Erro ao carregar dados (AJAX)',
+                        summary: `HTTP ${xhr.status} ${xhr.statusText || ''} em ${url}`,
+                        suggestions: [
+                            'Verifique a URL do endpoint e se ele está acessível.',
+                            'Confirme que o servidor retorna JSON válido (Content-Type: application/json).',
+                            'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
+                            'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".',
+                            'Confira CORS, autenticação/token e logs do servidor.'
+                        ],
+                        raw: { status: xhr.status, statusText: xhr.statusText || '', url, responsePreview: body.slice(0, options.previewLimit) },
+                        dedupeKey: `XHR|${settings.sTableId || ''}|${url || ''}|${xhr.status}`
+                    };
+
+                    state.lastXhrErrAt = Date.now();
+                    state.lastXhrSig = payload.dedupeKey;
+                    state.lastXhrPayload = payload;
+                    $table.data('dtBetterState', state);
+
+                    handleError($table, options, payload);
+                } catch (erro) {
+                    console.error('Erro em xhr.dt handler:', erro);
                 }
-            };
-            handleError($table, options, payload);
-        });
-    }
-
-    function disable(selector) { const $t = $(selector); $t.off('.dtBetterErrors'); $t.removeData('dtBetterErrorsOptions'); }
+            });
+
+            $table.on('error.dt.dtBetterErrors', function (e, settings, techNote, message)
+            {
+                try {
+                    e.preventDefault();
+                    const msg = (message || '').toString();
+                    const isAjaxErrorMsg = /Ajax error/i.test(msg) || /Erro ao carregar dados/i.test(msg) || /datatables\.net\/tn\/7/i.test(msg);
+
+                    if (options.suppressAjaxErrorDt && isAjaxErrorMsg)
+                    {
+                        const windowMs = options.coalesceWindowMs || 2000;
+                        if (state.lastXhrErrAt && (Date.now() - state.lastXhrErrAt) < windowMs)
+                        {
+                            log(options, 'info', 'Coalescido: error.dt Ajax error suprimido (já houve xhr.dt)', { message, state });
+                            return;
+                        }
+                    }
+
+                    if (isAjaxErrorMsg)
+                    {
+                        const url = state.lastPreXhrUrl || (settings.ajax && (settings.ajax.url || settings.ajax)) || '';
+                        const payload = {
+                            title: 'Erro ao carregar dados (AJAX)',
+                            summary: `Falha ao carregar via AJAX em ${url || '(URL não disponível)'}`,
+                            suggestions: [
+                                'Cheque a aba Network do DevTools (status HTTP, corpo e Content-Type).',
+                                'Se o servidor retorna HTML (ex.: página 404), ajuste a rota ou autenticação.',
+                                'Se o JSON é um array na raiz, use `ajax: { dataSrc: "" }`.',
+                                'Se o JSON vem como { "data": [...] }, ajuste `ajax.dataSrc` para "data".'
+                            ],
+                            raw: { message: msg, techNote, url },
+
+                            dedupeKey: `ERR|${settings.sTableId || ''}|${url || ''}|AJAX`
+                        };
+                        handleError($table, options, payload);
+                        return;
+                    }
+
+                    const dtApi = $(settings.nTable).DataTable();
+                    const human = explainErrorMessage(msg, settings, dtApi);
+                    const payload = {
+                        title: 'Erro no DataTables',
+                        summary: human.summary,
+                        suggestions: human.suggestions,
+                        raw: {
+                            message: msg, techNote, tableId: settings.sTableId,
+                            columns: settings.aoColumns?.map(c => getDataSrc(dtApi, c)) || []
+                        }
+                    };
+                    handleError($table, options, payload);
+                } catch (erro) {
+                    console.error('Erro em error.dt handler:', erro);
+                }
+            });
+        } catch (erro) {
+            console.error('Erro em enable:', erro);
+        }
+    }
+
+    function disable(selector) {
+        try {
+            const $t = $(selector);
+            $t.off('.dtBetterErrors');
+            $t.removeData('dtBetterErrorsOptions');
+        } catch (erro) {
+            console.error('Erro em disable:', erro);
+        }
+    }
 
     function handleError($table, options, payload)
     {
-        const contexto = options.contexto || ($table.attr('id') || 'DataTable');
-        const origem = String(options.origem || 'AJAX.DataTable');
-
-        const key = (payload.dedupeKey || makeKey(contexto, origem, payload));
-        if (seen(key, options.dedupeWindowMs)) { log(options, 'info', 'Erro duplicado suprimido', { contexto, origem, payload }); return; }
-        stamp(key);
-
-        if (options.encaminharParaAlerta && hasAlerta()) alertaQ.enqueue(() => sendToAlerta(contexto, origem, payload, options));
-        if (options.showModal) modalQ.enqueue(() => showModal(payload, options));
-
-        log(options, 'error', 'Erro DataTables', { contexto, origem, payload });
-        global.__DT_LAST_ERROR__ = { contexto, origem, payload, ts: Date.now() };
+        try {
+            const contexto = options.contexto || ($table.attr('id') || 'DataTable');
+            const origem = String(options.origem || 'AJAX.DataTable');
+
+            const key = (payload.dedupeKey || makeKey(contexto, origem, payload));
+            if (seen(key, options.dedupeWindowMs)) { log(options, 'info', 'Erro duplicado suprimido', { contexto, origem, payload }); return; }
+            stamp(key);
+
+            if (options.encaminharParaAlerta && hasAlerta()) alertaQ.enqueue(() => sendToAlerta(contexto, origem, payload, options));
+            if (options.showModal) modalQ.enqueue(() => showModal(payload, options));
+
+            log(options, 'error', 'Erro DataTables', { contexto, origem, payload });
+            global.__DT_LAST_ERROR__ = { contexto, origem, payload, ts: Date.now() };
+        } catch (erro) {
+            console.error('Erro em handleError:', erro);
+        }
     }
 
     function hasAlerta()
     {
-        return global.Alerta &&
-            (typeof global.Alerta.TratamentoErroComLinha === 'function' ||
-                typeof global.Alerta.TratamentoErroComLinhaEnriquecido === 'function');
+        try {
+            return global.Alerta &&
+                (typeof global.Alerta.TratamentoErroComLinha === 'function' ||
+                    typeof global.Alerta.TratamentoErroComLinhaEnriquecido === 'function');
+        } catch (erro) {
+            console.error('Erro em hasAlerta:', erro);
+            return false;
+        }
     }
 
     function sendToAlerta(contexto, origem, payload, options)
@@ -218,7 +279,8 @@
 
     function render({ title, summary, suggestions = [], raw = {} })
     {
-        return `
+        try {
+            return `
       <div class="mb-2"><strong>${esc(title || 'Erro')}</strong></div>
       <div class="mb-2">${esc(summary || '')}</div>
       ${suggestions.length ? `<ul class="mb-2">${suggestions.map(s => `<li>${esc(s)}</li>`).join('')}</ul>` : ''}
@@ -226,45 +288,55 @@
         <pre class="mt-2 p-2 bg-light border rounded small">${esc(JSON.stringify(raw, null, 2))}</pre>
       </details>
     `;
+        } catch (erro) {
+            console.error('Erro em render:', erro);
+            return '<div>Erro ao renderizar mensagem de erro</div>';
+        }
     }
 
     function explainErrorMessage(message, settings, dtApi)
     {
-        let summary = message; const suggestions = [];
-        if (/Requested unknown parameter/i.test(message))
-        {
-            const m = message.match(/column\s+(\d+)/i); const idx = m ? parseInt(m[1], 10) : null;
-            const dataSrc = (idx != null && dtApi?.column) ? dtApi.column(idx).dataSrc() : undefined;
-            summary = 'Coluna configurada com um campo que não existe no dataset.';
-            if (idx != null) suggestions.push(`A coluna #${idx} usa \`data\` = "${dataSrc}". Confirme que cada registro do JSON possui essa chave.`);
-            else suggestions.push('Uma coluna usa `data` inexistente em algumas linhas do JSON.');
-            suggestions.push('Se algumas linhas não tiverem o campo, defina `columns[n].defaultContent = ""`.',
-                'Se o servidor retorna array (posicional), use `columns[n].data = n` (índice).',
-                'Compare as chaves reais do payload com `columns[].data`.');
-        }
-        if (/Invalid JSON response/i.test(message))
-        {
-            summary = 'A resposta do servidor não é um JSON válido para o DataTables.';
-            suggestions.push('O servidor pode estar retornando HTML de erro (500) ou JSON malformado.',
-                'Garanta `Content-Type: application/json` e JSON bem formado.',
-                'Se o array está na raiz, use `ajax: { dataSrc: "" }`.',
-                'Se vier `{ "data": [...] }`, ajuste `ajax.dataSrc` para "data".');
-        }
-        if (suggestions.length === 0)
-        {
-            suggestions.push('Abra o console para ver o erro completo e o objeto `settings`.',
-                'Revise o mapeamento `columns[].data` e o `ajax.dataSrc`.',
-                'Em `render`, trate campos ausentes com `row?.campo ?? ""`.');
-        }
-        return { summary, suggestions };
+        try {
+            let summary = message; const suggestions = [];
+            if (/Requested unknown parameter/i.test(message))
+            {
+                const m = message.match(/column\s+(\d+)/i); const idx = m ? parseInt(m[1], 10) : null;
+                const dataSrc = (idx != null && dtApi?.column) ? dtApi.column(idx).dataSrc() : undefined;
+                summary = 'Coluna configurada com um campo que não existe no dataset.';
+                if (idx != null) suggestions.push(`A coluna #${idx} usa \`data\` = "${dataSrc}". Confirme que cada registro do JSON possui essa chave.`);
+                else suggestions.push('Uma coluna usa `data` inexistente em algumas linhas do JSON.');
+                suggestions.push('Se algumas linhas não tiverem o campo, defina `columns[n].defaultContent = ""`.',
+                    'Se o servidor retorna array (posicional), use `columns[n].data = n` (índice).',
+                    'Compare as chaves reais do payload com `columns[].data`.');
+            }
+            if (/Invalid JSON response/i.test(message))
+            {
+                summary = 'A resposta do servidor não é um JSON válido para o DataTables.';
+                suggestions.push('O servidor pode estar retornando HTML de erro (500) ou JSON malformado.',
+                    'Garanta `Content-Type: application/json` e JSON bem formado.',
+                    'Se o array está na raiz, use `ajax: { dataSrc: "" }`.',
+                    'Se vier `{ "data": [...] }`, ajuste `ajax.dataSrc` para "data".');
+            }
+            if (suggestions.length === 0)
+            {
+                suggestions.push('Abra o console para ver o erro completo e o objeto `settings`.',
+                    'Revise o mapeamento `columns[].data` e o `ajax.dataSrc`.',
+                    'Em `render`, trate campos ausentes com `row?.campo ?? ""`.');
+            }
+            return { summary, suggestions };
+        } catch (erro) {
+            console.error('Erro em explainErrorMessage:', erro);
+            return { summary: message, suggestions: ['Erro ao processar mensagem de erro'] };
+        }
     }
 
     function getDataSrc(dtApi, colSettings) { try { return dtApi.column(colSettings.idx).dataSrc(); } catch { return colSettings?.mData ?? colSettings?.data; } }
 
     function ensureModal()
     {
-        if (document.getElementById('dtErrorModal')) return;
-        const tpl = `
+        try {
+            if (document.getElementById('dtErrorModal')) return;
+            const tpl = `
     <div class="modal fade" id="dtErrorModal" tabindex="-1" aria-labelledby="dtErrorModalLabel" aria-hidden="true">
       <div class="modal-dialog modal-lg modal-dialog-scrollable"><div class="modal-content">
         <div class="modal-header"><h5 class="modal-title" id="dtErrorModalLabel">Detalhes do erro</h5>
@@ -273,10 +345,20 @@
         <div class="modal-footer"><button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Fechar</button></div>
       </div></div>
     </div>`;
-        document.body.insertAdjacentHTML('beforeend', tpl);
-    }
-
-    function esc(s) { return String(s).replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m])); }
+            document.body.insertAdjacentHTML('beforeend', tpl);
+        } catch (erro) {
+            console.error('Erro em ensureModal:', erro);
+        }
+    }
+
+    function esc(s) {
+        try {
+            return String(s).replace(/[&<>"']/g, m => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[m]));
+        } catch (erro) {
+            console.error('Erro em esc:', erro);
+            return String(s);
+        }
+    }
 
     function log(options, level, msg, extra)
     {
@@ -288,11 +370,31 @@
 
     function makeKey(ctx, orig, payload)
     {
-        const base = `${ctx}|${orig}|${payload.title}|${payload.summary}`;
-        return base.replace(/\s+/g, ' ').trim().slice(0, 500);
-    }
-    function seen(key, windowMs) { const last = _dedupe.get(key); if (!last) return false; return (Date.now() - last) < windowMs; }
-    function stamp(key) { _dedupe.set(key, Date.now()); }
+        try {
+            const base = `${ctx}|${orig}|${payload.title}|${payload.summary}`;
+            return base.replace(/\s+/g, ' ').trim().slice(0, 500);
+        } catch (erro) {
+            console.error('Erro em makeKey:', erro);
+            return `${ctx}|${orig}|ERROR`;
+        }
+    }
+    function seen(key, windowMs) {
+        try {
+            const last = _dedupe.get(key);
+            if (!last) return false;
+            return (Date.now() - last) < windowMs;
+        } catch (erro) {
+            console.error('Erro em seen:', erro);
+            return false;
+        }
+    }
+    function stamp(key) {
+        try {
+            _dedupe.set(key, Date.now());
+        } catch (erro) {
+            console.error('Erro em stamp:', erro);
+        }
+    }
 
     global.DTBetterErrors = {
         enable, disable, setGlobalOptions, getLogs, clearLogs, version: '1.4.1'
```
