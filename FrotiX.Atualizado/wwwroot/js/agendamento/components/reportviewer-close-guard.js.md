# wwwroot/js/agendamento/components/reportviewer-close-guard.js

**Mudanca:** GRANDE | **+85** linhas | **-72** linhas

---

```diff
--- JANEIRO: wwwroot/js/agendamento/components/reportviewer-close-guard.js
+++ ATUAL: wwwroot/js/agendamento/components/reportviewer-close-guard.js
@@ -1,120 +1,127 @@
-(function () {
+(function ()
+{
     'use strict';
 
-    window.isReportViewerLoading =
-        window.isReportViewerLoading === true ? true : false;
-    window._pendingCloseAfterRender =
-        window._pendingCloseAfterRender === true ? true : false;
-
-    function closeModalViagens() {
-        try {
+    window.isReportViewerLoading = window.isReportViewerLoading === true ? true : false;
+    window._pendingCloseAfterRender = window._pendingCloseAfterRender === true ? true : false;
+
+    function closeModalViagens()
+    {
+        try
+        {
             const el = document.getElementById('modalViagens');
             if (!el) return;
 
             console.log('[CloseGuard] 🚪 Fechando modal programaticamente...');
 
-            if (
-                window.bootstrap &&
-                bootstrap.Modal &&
-                bootstrap.Modal.getOrCreateInstance
-            ) {
+            if (window.bootstrap && bootstrap.Modal && bootstrap.Modal.getOrCreateInstance)
+            {
                 bootstrap.Modal.getOrCreateInstance(el).hide();
             }
 
-            else if (window.$ && $('#modalViagens').modal) {
+            else if (window.$ && $('#modalViagens').modal)
+            {
                 $('#modalViagens').modal('hide');
             }
 
-            else {
+            else
+            {
                 el.classList.remove('show');
                 el.setAttribute('aria-hidden', 'true');
             }
 
             console.log('[CloseGuard] ✅ Modal fechado');
-        } catch (e) {
+        } catch (e)
+        {
             console.warn('[CloseGuard] ❌ Erro ao fechar modal:', e);
         }
     }
 
-    function patchReportViewerPlugin($) {
-        if (!$ || !$.fn || !$.fn.telerik_ReportViewer || $.fn._trv_patched) {
+    function patchReportViewerPlugin($)
+    {
+        if (!$ || !$.fn || !$.fn.telerik_ReportViewer || $.fn._trv_patched)
+        {
             return;
         }
 
-        console.log(
-            '[CloseGuard] 🔧 Aplicando patch no Telerik ReportViewer...',
-        );
+        console.log('[CloseGuard] 🔧 Aplicando patch no Telerik ReportViewer...');
 
         const original = $.fn.telerik_ReportViewer;
 
-        $.fn.telerik_ReportViewer = function (options) {
-            try {
-                if (options && typeof options === 'object') {
+        $.fn.telerik_ReportViewer = function (options)
+        {
+            try
+            {
+                if (options && typeof options === 'object')
+                {
 
                     const originalRenderingBegin = options.renderingBegin;
                     const originalRenderingEnd = options.renderingEnd;
                     const originalReady = options.ready;
                     const originalError = options.error;
 
-                    options.renderingBegin = function () {
-                        console.log(
-                            '[CloseGuard] 🎬 renderingBegin - BLOQUEANDO fechamento',
-                        );
+                    options.renderingBegin = function ()
+                    {
+                        console.log('[CloseGuard] 🎬 renderingBegin - BLOQUEANDO fechamento');
                         window.isReportViewerLoading = true;
                         window._pendingCloseAfterRender = false;
 
-                        if (typeof originalRenderingBegin === 'function') {
+                        if (typeof originalRenderingBegin === 'function')
+                        {
                             originalRenderingBegin.apply(this, arguments);
                         }
                     };
 
-                    options.renderingEnd = function () {
-                        console.log(
-                            '[CloseGuard] 🎬 renderingEnd - DESBLOQUEANDO',
-                        );
+                    options.renderingEnd = function ()
+                    {
+                        console.log('[CloseGuard] 🎬 renderingEnd - DESBLOQUEANDO');
                         window.isReportViewerLoading = false;
 
-                        if (window._pendingCloseAfterRender) {
-                            console.log(
-                                '[CloseGuard] 🚪 Executando fechamento pendente...',
-                            );
+                        if (window._pendingCloseAfterRender)
+                        {
+                            console.log('[CloseGuard] 🚪 Executando fechamento pendente...');
                             window._pendingCloseAfterRender = false;
                             setTimeout(() => closeModalViagens(), 100);
                         }
 
-                        if (typeof originalRenderingEnd === 'function') {
+                        if (typeof originalRenderingEnd === 'function')
+                        {
                             originalRenderingEnd.apply(this, arguments);
                         }
                     };
 
-                    options.ready = function () {
+                    options.ready = function ()
+                    {
                         console.log('[CloseGuard] ✅ ready - DESBLOQUEANDO');
                         window.isReportViewerLoading = false;
 
-                        if (window._pendingCloseAfterRender) {
-                            console.log(
-                                '[CloseGuard] 🚪 Executando fechamento pendente...',
-                            );
+                        if (window._pendingCloseAfterRender)
+                        {
+                            console.log('[CloseGuard] 🚪 Executando fechamento pendente...');
                             window._pendingCloseAfterRender = false;
                             setTimeout(() => closeModalViagens(), 100);
                         }
 
-                        if (typeof originalReady === 'function') {
+                        if (typeof originalReady === 'function')
+                        {
                             originalReady.apply(this, arguments);
                         }
                     };
 
-                    options.error = function () {
+                    options.error = function ()
+                    {
                         console.log('[CloseGuard] ❌ error - DESBLOQUEANDO');
                         window.isReportViewerLoading = false;
                         window._pendingCloseAfterRender = false;
 
-                        if (typeof originalError === 'function') {
+                        if (typeof originalError === 'function')
+                        {
                             originalError.apply(this, arguments);
                         }
                     };
                 }
-            } catch (e) {
+            } catch (e)
+            {
                 console.warn('[CloseGuard] ❌ Erro ao aplicar patch:', e);
             }
 
@@ -125,73 +132,79 @@
         console.log('[CloseGuard] ✅ Patch aplicado com sucesso');
     }
 
-    function bindModalGuard() {
-        function onAttemptClose(e) {
-            try {
-                if (window.isReportViewerLoading) {
-                    console.log(
-                        '[CloseGuard] 🛑 Tentativa de fechar BLOQUEADA - relatório renderizando',
-                    );
+    function bindModalGuard()
+    {
+        function onAttemptClose(e)
+        {
+            try
+            {
+                if (window.isReportViewerLoading)
+                {
+                    console.log('[CloseGuard] 🛑 Tentativa de fechar BLOQUEADA - relatório renderizando');
 
                     if (e && e.preventDefault) e.preventDefault();
-                    if (e && e.stopImmediatePropagation)
-                        e.stopImmediatePropagation();
+                    if (e && e.stopImmediatePropagation) e.stopImmediatePropagation();
 
                     window._pendingCloseAfterRender = true;
 
-                    if (
-                        window.Alerta &&
-                        typeof window.Alerta.Alerta === 'function'
-                    ) {
+                    if (window.Alerta && typeof window.Alerta.Alerta === 'function')
+                    {
                         window.Alerta.Alerta(
                             'Relatório em processamento',
-                            'Aguarde a renderização completa da Ficha de Viagem. O modal fechará automaticamente ao terminar.',
+                            'Aguarde a renderização completa da Ficha de Viagem. O modal fechará automaticamente ao terminar.'
                         );
                     }
 
                     return false;
                 }
-            } catch (err) {
+            } catch (err)
+            {
                 console.warn('[CloseGuard] ❌ Erro em onAttemptClose:', err);
             }
         }
 
-        if (window.$ && $(document).on) {
+        if (window.$ && $(document).on)
+        {
             $(document).off('hide.bs.modal', '#modalViagens', onAttemptClose);
             $(document).on('hide.bs.modal', '#modalViagens', onAttemptClose);
             console.log('[CloseGuard] ✅ Event listener jQuery registrado');
         }
 
         const el = document.getElementById('modalViagens');
-        if (el && el.addEventListener) {
-            el.addEventListener('hide.bs.modal', onAttemptClose, {
-                capture: true,
-            });
+        if (el && el.addEventListener)
+        {
+            el.addEventListener('hide.bs.modal', onAttemptClose, { capture: true });
             console.log('[CloseGuard] ✅ Event listener nativo registrado');
         }
 
         console.log('[CloseGuard] 🛡️ Modal guard ativo');
     }
 
-    function init() {
-        try {
+    function init()
+    {
+        try
+        {
             console.log('[CloseGuard] 🚀 Inicializando proteção...');
 
-            if (window.$) {
+            if (window.$)
+            {
                 patchReportViewerPlugin(window.$);
             }
 
             bindModalGuard();
 
             console.log('[CloseGuard] ✅ Proteção ativa');
-        } catch (e) {
+        } catch (e)
+        {
             console.warn('[CloseGuard] ❌ Erro na inicialização:', e);
         }
     }
 
-    if (document.readyState === 'loading') {
+    if (document.readyState === 'loading')
+    {
         document.addEventListener('DOMContentLoaded', init);
-    } else {
+    } else
+    {
         init();
     }
 })();
```
