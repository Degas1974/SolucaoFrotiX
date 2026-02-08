# wwwroot/js/frotix-error-logger.js

**Mudanca:** GRANDE | **+142** linhas | **-97** linhas

---

```diff
--- JANEIRO: wwwroot/js/frotix-error-logger.js
+++ ATUAL: wwwroot/js/frotix-error-logger.js
@@ -73,22 +73,27 @@
     }
 
     function isDuplicateError(errorKey) {
-        const now = Date.now();
-        const lastTime = recentErrors.get(errorKey);
-
-        if (lastTime && (now - lastTime) < CONFIG.debounceTime) {
-            return true;
-        }
-
-        recentErrors.set(errorKey, now);
-
-        if (recentErrors.size > 100) {
-            const oldEntries = Array.from(recentErrors.entries())
-                .filter(([, time]) => (now - time) > 5000);
-            oldEntries.forEach(([key]) => recentErrors.delete(key));
-        }
-
-        return false;
+        try {
+            const now = Date.now();
+            const lastTime = recentErrors.get(errorKey);
+
+            if (lastTime && (now - lastTime) < CONFIG.debounceTime) {
+                return true;
+            }
+
+            recentErrors.set(errorKey, now);
+
+            if (recentErrors.size > 100) {
+                const oldEntries = Array.from(recentErrors.entries())
+                    .filter(([, time]) => (now - time) > 5000);
+                oldEntries.forEach(([key]) => recentErrors.delete(key));
+            }
+
+            return false;
+        } catch (erro) {
+            console.error('Erro em isDuplicateError:', erro);
+            return false;
+        }
     }
 
     async function sendErrorToServer(errorData) {
@@ -117,89 +122,109 @@
     }
 
     function globalErrorHandler(message, source, lineno, colno, error) {
-        const stackInfo = error ? parseStackTrace(error.stack) : {};
-
-        const errorData = {
-            mensagem: message || 'Erro JavaScript desconhecido',
-            arquivo: extractFileName(source) || stackInfo.arquivo,
-            metodo: stackInfo.funcao,
-            linha: lineno || stackInfo.linha,
-            coluna: colno || stackInfo.coluna,
-            stack: error?.stack || null,
-            url: window.location.href,
-            userAgent: navigator.userAgent,
-            timestamp: new Date().toISOString()
-        };
-
-        sendErrorToServer(errorData);
+        try {
+            const stackInfo = error ? parseStackTrace(error.stack) : {};
+
+            const errorData = {
+                mensagem: message || 'Erro JavaScript desconhecido',
+                arquivo: extractFileName(source) || stackInfo.arquivo,
+                metodo: stackInfo.funcao,
+                linha: lineno || stackInfo.linha,
+                coluna: colno || stackInfo.coluna,
+                stack: error?.stack || null,
+                url: window.location.href,
+                userAgent: navigator.userAgent,
+                timestamp: new Date().toISOString()
+            };
+
+            sendErrorToServer(errorData);
+        } catch (erro) {
+            console.error('Erro em globalErrorHandler:', erro);
+        }
 
         return false;
     }
 
     function unhandledRejectionHandler(event) {
-        const error = event.reason;
-        const stackInfo = error instanceof Error ? parseStackTrace(error.stack) : {};
-
-        const errorData = {
-            mensagem: error instanceof Error ? error.message : String(error),
-            arquivo: stackInfo.arquivo || 'Promise',
-            metodo: stackInfo.funcao || 'unhandledRejection',
-            linha: stackInfo.linha,
-            coluna: stackInfo.coluna,
-            stack: error instanceof Error ? error.stack : null,
-            url: window.location.href,
-            userAgent: navigator.userAgent,
-            timestamp: new Date().toISOString()
-        };
-
-        sendErrorToServer(errorData);
+        try {
+            const error = event.reason;
+            const stackInfo = error instanceof Error ? parseStackTrace(error.stack) : {};
+
+            const errorData = {
+                mensagem: error instanceof Error ? error.message : String(error),
+                arquivo: stackInfo.arquivo || 'Promise',
+                metodo: stackInfo.funcao || 'unhandledRejection',
+                linha: stackInfo.linha,
+                coluna: stackInfo.coluna,
+                stack: error instanceof Error ? error.stack : null,
+                url: window.location.href,
+                userAgent: navigator.userAgent,
+                timestamp: new Date().toISOString()
+            };
+
+            sendErrorToServer(errorData);
+        } catch (erro) {
+            console.error('Erro em unhandledRejectionHandler:', erro);
+        }
     }
 
     function TratamentoErroComLinha(classe, metodo, erro) {
-        const stackInfo = erro instanceof Error ? parseStackTrace(erro.stack) : {};
-
-        const errorData = {
-            mensagem: erro instanceof Error ? erro.message : String(erro),
-            arquivo: stackInfo.arquivo || classe,
-            metodo: metodo || stackInfo.funcao,
-            linha: stackInfo.linha,
-            coluna: stackInfo.coluna,
-            stack: erro instanceof Error ? erro.stack : null,
-            url: window.location.href,
-            userAgent: navigator.userAgent,
-            timestamp: new Date().toISOString()
-        };
-
-        sendErrorToServer(errorData);
-
-        if (window.Alerta && window.Alerta.TratamentoErroComLinha) {
-            window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
-        } else if (window.SweetAlertInterop && window.SweetAlertInterop.ShowErrorUnexpected) {
-            window.SweetAlertInterop.ShowErrorUnexpected(classe, metodo, erro);
-        } else {
-            console.error(`[FrotiX Error] ${classe}.${metodo}:`, erro);
+        try {
+            const stackInfo = erro instanceof Error ? parseStackTrace(erro.stack) : {};
+
+            const errorData = {
+                mensagem: erro instanceof Error ? erro.message : String(erro),
+                arquivo: stackInfo.arquivo || classe,
+                metodo: metodo || stackInfo.funcao,
+                linha: stackInfo.linha,
+                coluna: stackInfo.coluna,
+                stack: erro instanceof Error ? erro.stack : null,
+                url: window.location.href,
+                userAgent: navigator.userAgent,
+                timestamp: new Date().toISOString()
+            };
+
+            sendErrorToServer(errorData);
+
+            if (window.Alerta && window.Alerta.TratamentoErroComLinha) {
+                window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
+            } else if (window.SweetAlertInterop && window.SweetAlertInterop.ShowErrorUnexpected) {
+                window.SweetAlertInterop.ShowErrorUnexpected(classe, metodo, erro);
+            } else {
+                console.error(`[FrotiX Error] ${classe}.${metodo}:`, erro);
+            }
+        } catch (erroInterno) {
+            console.error('Erro em TratamentoErroComLinha:', erroInterno);
         }
     }
 
     function logError(message, arquivo, metodo, details) {
-        const errorData = {
-            mensagem: message,
-            arquivo: arquivo || extractFileName(window.location.pathname),
-            metodo: metodo,
-            linha: null,
-            coluna: null,
-            stack: details?.stack || null,
-            url: window.location.href,
-            userAgent: navigator.userAgent,
-            timestamp: new Date().toISOString()
-        };
-
-        sendErrorToServer(errorData);
+        try {
+            const errorData = {
+                mensagem: message,
+                arquivo: arquivo || extractFileName(window.location.pathname),
+                metodo: metodo,
+                linha: null,
+                coluna: null,
+                stack: details?.stack || null,
+                url: window.location.href,
+                userAgent: navigator.userAgent,
+                timestamp: new Date().toISOString()
+            };
+
+            sendErrorToServer(errorData);
+        } catch (erro) {
+            console.error('Erro em logError:', erro);
+        }
     }
 
     function logWarning(message, arquivo, metodo) {
-        console.warn(`[FrotiX Warning] ${arquivo || ''}.${metodo || ''}: ${message}`);
-
+        try {
+            console.warn(`[FrotiX Warning] ${arquivo || ''}.${metodo || ''}: ${message}`);
+
+        } catch (erro) {
+            console.error('Erro em logWarning:', erro);
+        }
     }
 
     async function safeAsync(fn, classe, metodo) {
@@ -221,19 +246,31 @@
     }
 
     function setEnabled(enabled) {
-        CONFIG.enabled = enabled;
+        try {
+            CONFIG.enabled = enabled;
+        } catch (erro) {
+            console.error('Erro em setEnabled:', erro);
+        }
     }
 
     function setApiEndpoint(endpoint) {
-        CONFIG.apiEndpoint = endpoint;
+        try {
+            CONFIG.apiEndpoint = endpoint;
+        } catch (erro) {
+            console.error('Erro em setApiEndpoint:', erro);
+        }
     }
 
     function init() {
-
-        window.onerror = globalErrorHandler;
-        window.addEventListener('unhandledrejection', unhandledRejectionHandler);
-
-        console.log('[FrotiXErrorLogger] Inicializado com sucesso');
+        try {
+
+            window.onerror = globalErrorHandler;
+            window.addEventListener('unhandledrejection', unhandledRejectionHandler);
+
+            console.log('[FrotiXErrorLogger] Inicializado com sucesso');
+        } catch (erro) {
+            console.error('Erro em init:', erro);
+        }
     }
 
     if (document.readyState === 'loading') {
@@ -256,32 +293,40 @@
 
 window.Alerta = window.Alerta || {};
 window.Alerta.TratamentoErroComLinha = window.Alerta.TratamentoErroComLinha || function (classe, metodo, erro) {
-    FrotiXErrorLogger.TratamentoErroComLinha(classe, metodo, erro);
-
-    if (typeof Swal !== 'undefined') {
-        const stackInfo = erro instanceof Error && erro.stack ?
-            erro.stack.split('\n').slice(0, 5).join('<br>') : '';
-
-        Swal.fire({
-            icon: 'error',
-            title: 'Erro Sem Tratamento',
-            html: `
-                <div style="text-align: left; font-size: 14px;">
-                    <p><b>📚 Classe:</b> ${classe}</p>
-                    <p><b>🖋️ Método:</b> ${metodo}</p>
-                    <p><b>❌ Erro:</b> ${erro?.message || erro}</p>
-                    ${stackInfo ? `<details><summary>Stack Trace</summary><pre style="font-size: 11px; overflow-x: auto;">${stackInfo}</pre></details>` : ''}
-                </div>
-            `,
-            confirmButtonText: 'OK',
-            confirmButtonColor: '#dc3545'
-        });
-    } else {
-
-        alert(`Erro em ${classe}.${metodo}: ${erro?.message || erro}`);
+    try {
+        FrotiXErrorLogger.TratamentoErroComLinha(classe, metodo, erro);
+
+        if (typeof Swal !== 'undefined') {
+            const stackInfo = erro instanceof Error && erro.stack ?
+                erro.stack.split('\n').slice(0, 5).join('<br>') : '';
+
+            Swal.fire({
+                icon: 'error',
+                title: 'Erro Sem Tratamento',
+                html: `
+                    <div style="text-align: left; font-size: 14px;">
+                        <p><b>📚 Classe:</b> ${classe}</p>
+                        <p><b>🖋️ Método:</b> ${metodo}</p>
+                        <p><b>❌ Erro:</b> ${erro?.message || erro}</p>
+                        ${stackInfo ? `<details><summary>Stack Trace</summary><pre style="font-size: 11px; overflow-x: auto;">${stackInfo}</pre></details>` : ''}
+                    </div>
+                `,
+                confirmButtonText: 'OK',
+                confirmButtonColor: '#dc3545'
+            });
+        } else {
+
+            alert(`Erro em ${classe}.${metodo}: ${erro?.message || erro}`);
+        }
+    } catch (erroInterno) {
+        console.error('Erro em window.Alerta.TratamentoErroComLinha:', erroInterno);
     }
 };
 
 function TratamentoErroComLinha(classe, metodo, erro) {
-    window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
+    try {
+        window.Alerta.TratamentoErroComLinha(classe, metodo, erro);
+    } catch (erroInterno) {
+        console.error('Erro em TratamentoErroComLinha (global):', erroInterno);
+    }
 }
```
