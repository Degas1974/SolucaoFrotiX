# wwwroot/js/syncfusion_tooltips.js

**Mudanca:** GRANDE | **+106** linhas | **-100** linhas

---

```diff
--- JANEIRO: wwwroot/js/syncfusion_tooltips.js
+++ ATUAL: wwwroot/js/syncfusion_tooltips.js
@@ -2,13 +2,15 @@
 {
     function initializeTooltip()
     {
-
-        if (typeof ej === 'undefined' || !ej.popups || !ej.popups.Tooltip)
-        {
-            console.warn('Syncfusion não carregado. Tentando novamente em 500ms...');
-            setTimeout(initializeTooltip, 500);
-            return;
-        }
+        try
+        {
+
+            if (typeof ej === 'undefined' || !ej.popups || !ej.popups.Tooltip)
+            {
+                console.warn('Syncfusion não carregado. Tentando novamente em 500ms...');
+                setTimeout(initializeTooltip, 500);
+                return;
+            }
 
         document.querySelectorAll('[data-ejtip]').forEach(function (el)
         {
@@ -96,73 +98,131 @@
 
             content: function (args)
             {
-                try {
-
-                    const targetElement = args && args.target ? args.target : args;
-
-                    if (!targetElement || typeof targetElement.getAttribute !== 'function') {
-                        return 'Sem descrição';
-                    }
-
-                    let tooltipText = targetElement.getAttribute('data-ejtip');
+                try
+                {
+                    let tooltipText = args.getAttribute('data-ejtip');
+                    console.log('Tooltip text:', tooltipText);
 
                     if (tooltipText) {
                         tooltipText = tooltipText.replace(/\n/g, '<br>');
                     }
                     return tooltipText || 'Sem descrição';
-                } catch (error) {
-                    console.warn('Erro ao obter tooltip:', error);
-                    return 'Sem descrição';
+                }
+                catch (erro)
+                {
+                    console.error('Erro em content callback:', erro);
+                    return 'Erro ao carregar tooltip';
                 }
             },
             beforeOpen: function (args)
             {
-
-                const target = args.target;
-                let tooltipText = target.getAttribute('data-ejtip');
-
-                if (tooltipText)
-                {
-
-                    tooltipText = tooltipText.replace(/\n/g, '<br>');
-                    this.content = tooltipText;
-                    console.log('Tooltip configurado com:', tooltipText);
-                } else
-                {
-                    console.warn('Elemento sem data-ejtip:', target);
-                    this.content = 'Sem descrição';
+                try
+                {
+
+                    const target = args.target;
+                    let tooltipText = target.getAttribute('data-ejtip');
+
+                    if (tooltipText)
+                    {
+
+                        tooltipText = tooltipText.replace(/\n/g, '<br>');
+                        this.content = tooltipText;
+                        console.log('Tooltip configurado com:', tooltipText);
+                    } else
+                    {
+                        console.warn('Elemento sem data-ejtip:', target);
+                        this.content = 'Sem descrição';
+                    }
+                }
+                catch (erro)
+                {
+                    console.error('Erro em beforeOpen callback:', erro);
+                    this.content = 'Erro ao carregar tooltip';
                 }
             },
             afterOpen: function (args)
             {
-
-                const tooltipElement = args.element;
-                const closeTimeout = setTimeout(() =>
-                {
-                    this.close();
-                }, 2000);
-
-                tooltipElement.setAttribute('data-close-timeout', closeTimeout);
+                try
+                {
+
+                    const tooltipElement = args.element;
+                    const closeTimeout = setTimeout(() =>
+                    {
+                        this.close();
+                    }, 2000);
+
+                    tooltipElement.setAttribute('data-close-timeout', closeTimeout);
+                }
+                catch (erro)
+                {
+                    console.error('Erro em afterOpen callback:', erro);
+                }
             },
             beforeClose: function (args)
             {
-
-                if (!args || !args.element) return;
-
-                const closeTimeout = args.element.getAttribute('data-close-timeout');
-                if (closeTimeout)
-                {
-                    clearTimeout(parseInt(closeTimeout));
-                    args.element.removeAttribute('data-close-timeout');
-                }
-            }
-        });
-
-        window.ejTooltip.appendTo('body');
-        console.log('✓ Tooltip GLOBAL Syncfusion inicializado (sem setas)');
+                try
+                {
+                    const closeTimeout = args.element.getAttribute('data-close-timeout');
+                    if (closeTimeout)
+                    {
+                        clearTimeout(parseInt(closeTimeout));
+                        args.element.removeAttribute('data-close-timeout');
+                    }
+                }
+                catch (erro)
+                {
+                    console.error('Erro em beforeClose callback:', erro);
+                }
+            }
+        });
+
+            window.ejTooltip.appendTo('body');
+            console.log('✓ Tooltip GLOBAL Syncfusion inicializado (sem setas)');
+        }
+        catch (erro)
+        {
+            console.error('Erro em initializeTooltip:', erro);
+            Alerta.TratamentoErroComLinha('syncfusion_tooltips.js', 'initializeTooltip', erro);
+        }
     }
 
     window.refreshTooltips = function ()
+    {
+        try
+        {
+            document.querySelectorAll('[data-ejtip]').forEach(function (el)
+            {
+                el.removeAttribute('data-bs-toggle');
+                el.removeAttribute('data-bs-original-title');
+                el.removeAttribute('title');
+            });
+
+            if (window.ejTooltip)
+            {
+                window.ejTooltip.refresh();
+                console.log('✓ Tooltips atualizados');
+            } else
+            {
+                console.warn('⚠ ejTooltip não está inicializado. Inicializando...');
+                initializeTooltip();
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em refreshTooltips:', erro);
+            Alerta.TratamentoErroComLinha('syncfusion_tooltips.js', 'refreshTooltips', erro);
+        }
+    };
+
+    if (document.readyState === 'loading')
+    {
+        document.addEventListener('DOMContentLoaded', initializeTooltip);
+    } else
+    {
+        initializeTooltip();
+    }
+
+    const observer = new MutationObserver(() =>
     {
         document.querySelectorAll('[data-ejtip]').forEach(function (el)
         {
@@ -174,73 +234,7 @@
         if (window.ejTooltip)
         {
             window.ejTooltip.refresh();
-            console.log('✓ Tooltips atualizados');
-        } else
-        {
-            console.warn('⚠ ejTooltip não está inicializado. Inicializando...');
-            initializeTooltip();
-        }
-    };
-
-    if (document.readyState === 'loading')
-    {
-        document.addEventListener('DOMContentLoaded', initializeTooltip);
-    } else
-    {
-        initializeTooltip();
-    }
-
-    let tooltipRefreshTimer = null;
-    let isRefreshing = false;
-
-    const observer = new MutationObserver((mutations) =>
-    {
-
-        if (isRefreshing) return;
-
-        let hasNewTooltipElements = false;
-        for (const mutation of mutations) {
-            for (const node of mutation.addedNodes) {
-                if (node.nodeType === Node.ELEMENT_NODE) {
-                    if (node.hasAttribute && node.hasAttribute('data-ejtip')) {
-                        hasNewTooltipElements = true;
-                        break;
-                    }
-                    if (node.querySelector && node.querySelector('[data-ejtip]')) {
-                        hasNewTooltipElements = true;
-                        break;
-                    }
-                }
-            }
-            if (hasNewTooltipElements) break;
-        }
-
-        if (!hasNewTooltipElements) return;
-
-        if (tooltipRefreshTimer) {
-            clearTimeout(tooltipRefreshTimer);
-        }
-
-        tooltipRefreshTimer = setTimeout(() =>
-        {
-            isRefreshing = true;
-            try {
-                document.querySelectorAll('[data-ejtip]').forEach(function (el)
-                {
-                    el.removeAttribute('data-bs-toggle');
-                    el.removeAttribute('data-bs-original-title');
-                    el.removeAttribute('title');
-                });
-
-                if (window.ejTooltip)
-                {
-                    window.ejTooltip.refresh();
-                }
-            } finally {
-
-                setTimeout(() => { isRefreshing = false; }, 100);
-            }
-        }, 500);
+        }
     });
 
     if (document.readyState === 'loading')
```
