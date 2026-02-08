# wwwroot/js/frotix.js

**Mudanca:** GRANDE | **+432** linhas | **-224** linhas

---

```diff
--- JANEIRO: wwwroot/js/frotix.js
+++ ATUAL: wwwroot/js/frotix.js
@@ -1,51 +1,60 @@
 function trimTransparentPNG(img, targetWidth, targetHeight)
 {
-    const tempCanvas = document.createElement('canvas');
-    const ctx = tempCanvas.getContext('2d');
-
-    tempCanvas.width = img.width;
-    tempCanvas.height = img.height;
-    ctx.drawImage(img, 0, 0);
-
-    const imageData = ctx.getImageData(0, 0, tempCanvas.width, tempCanvas.height);
-    const pixels = imageData.data;
-    const w = imageData.width;
-    const h = imageData.height;
-
-    let top = null, bottom = null, left = null, right = null;
-
-    for (let y = 0; y < h; y++)
-    {
-        for (let x = 0; x < w; x++)
-        {
-            const alpha = pixels[(y * w + x) * 4 + 3];
-            if (alpha > 0)
+    try
+    {
+        const tempCanvas = document.createElement('canvas');
+        const ctx = tempCanvas.getContext('2d');
+
+        tempCanvas.width = img.width;
+        tempCanvas.height = img.height;
+        ctx.drawImage(img, 0, 0);
+
+        const imageData = ctx.getImageData(0, 0, tempCanvas.width, tempCanvas.height);
+        const pixels = imageData.data;
+        const w = imageData.width;
+        const h = imageData.height;
+
+        let top = null, bottom = null, left = null, right = null;
+
+        for (let y = 0; y < h; y++)
+        {
+            for (let x = 0; x < w; x++)
             {
-                if (top === null) top = y;
-                bottom = y;
-                if (left === null || x < left) left = x;
-                if (right === null || x > right) right = x;
-            }
-        }
-    }
-
-    const trimmedWidth = right - left + 1;
-    const trimmedHeight = bottom - top + 1;
-    const trimmedData = ctx.getImageData(left, top, trimmedWidth, trimmedHeight);
-
-    const resultCanvas = document.createElement('canvas');
-    resultCanvas.width = targetWidth;
-    resultCanvas.height = targetHeight;
-    const resultCtx = resultCanvas.getContext('2d');
-
-    const trimmedCanvas = document.createElement('canvas');
-    trimmedCanvas.width = trimmedWidth;
-    trimmedCanvas.height = trimmedHeight;
-    trimmedCanvas.getContext('2d').putImageData(trimmedData, 0, 0);
-
-    resultCtx.drawImage(trimmedCanvas, 0, 0, targetWidth, targetHeight);
-
-    return resultCanvas;
+                const alpha = pixels[(y * w + x) * 4 + 3];
+                if (alpha > 0)
+                {
+                    if (top === null) top = y;
+                    bottom = y;
+                    if (left === null || x < left) left = x;
+                    if (right === null || x > right) right = x;
+                }
+            }
+        }
+
+        const trimmedWidth = right - left + 1;
+        const trimmedHeight = bottom - top + 1;
+        const trimmedData = ctx.getImageData(left, top, trimmedWidth, trimmedHeight);
+
+        const resultCanvas = document.createElement('canvas');
+        resultCanvas.width = targetWidth;
+        resultCanvas.height = targetHeight;
+        const resultCtx = resultCanvas.getContext('2d');
+
+        const trimmedCanvas = document.createElement('canvas');
+        trimmedCanvas.width = trimmedWidth;
+        trimmedCanvas.height = trimmedHeight;
+        trimmedCanvas.getContext('2d').putImageData(trimmedData, 0, 0);
+
+        resultCtx.drawImage(trimmedCanvas, 0, 0, targetWidth, targetHeight);
+
+        return resultCanvas;
+    }
+    catch (erro)
+    {
+        console.error('Erro em trimTransparentPNG:', erro);
+        Alerta.TratamentoErroComLinha('frotix.js', 'trimTransparentPNG', erro);
+        return null;
+    }
 }
 
 (function ()
@@ -112,83 +121,130 @@
 
     function isFormEnabled(form)
     {
-
-        return (form.dataset.autoSpinner || "on").toLowerCase() !== "off";
+        try
+        {
+
+            return (form.dataset.autoSpinner || "on").toLowerCase() !== "off";
+        }
+        catch (erro)
+        {
+            console.error('Erro em isFormEnabled:', erro);
+            return true;
+        }
     }
 
     function getSubmitter(evt, form)
     {
-        if (evt && evt.submitter) return evt.submitter;
-        if (form._lastClickedSubmit && form.contains(form._lastClickedSubmit)) return form._lastClickedSubmit;
-
-        var explicitDefault = form.querySelector("[data-default-submit]");
-        if (explicitDefault) return explicitDefault;
-
-        return form.querySelector(SUBMIT_SELECTOR);
+        try
+        {
+            if (evt && evt.submitter) return evt.submitter;
+            if (form._lastClickedSubmit && form.contains(form._lastClickedSubmit)) return form._lastClickedSubmit;
+
+            var explicitDefault = form.querySelector("[data-default-submit]");
+            if (explicitDefault) return explicitDefault;
+
+            return form.querySelector(SUBMIT_SELECTOR);
+        }
+        catch (erro)
+        {
+            console.error('Erro em getSubmitter:', erro);
+            return null;
+        }
     }
 
     function htmlEscape(s)
     {
-        return String(s).replace(/[&<>"']/g, function (ch)
-        {
-            return ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[ch];
-        });
+        try
+        {
+            return String(s).replace(/[&<>"']/g, function (ch)
+            {
+                return ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" })[ch];
+            });
+        }
+        catch (erro)
+        {
+            console.error('Erro em htmlEscape:', erro);
+            return s;
+        }
     }
 
     function lockButton(btn)
     {
-        if (!btn || btn.dataset.busy === "true" || btn.hasAttribute("data-no-busy")) return false;
-
-        if (btn.dataset.busyKeepWidth === "true")
-        {
-            var rect = btn.getBoundingClientRect();
-            btn.style.minWidth = rect.width + "px";
-        }
-
-        btn.dataset.busy = "true";
-        btn.setAttribute("disabled", "disabled");
-        btn.setAttribute("aria-busy", "true");
-
-        var text = btn.dataset.spinnerText || "Salvando...";
-        var iconClass = btn.dataset.spinnerIcon || "fa-solid fa-spinner fa-spin me-2";
-
-        if (btn.tagName === "BUTTON")
-        {
-            if (!btn.dataset.originalHtml) btn.dataset.originalHtml = btn.innerHTML;
-
-            btn.innerHTML = '<i class="' + htmlEscape(iconClass) + '"></i>' + htmlEscape(text);
-        } else if (btn.tagName === "INPUT")
-        {
-
-            if (!btn.dataset.originalValue) btn.dataset.originalValue = btn.value;
-            btn.value = text;
-
-        }
-        return true;
+        try
+        {
+            if (!btn || btn.dataset.busy === "true" || btn.hasAttribute("data-no-busy")) return false;
+
+            if (btn.dataset.busyKeepWidth === "true")
+            {
+                var rect = btn.getBoundingClientRect();
+                btn.style.minWidth = rect.width + "px";
+            }
+
+            btn.dataset.busy = "true";
+            btn.setAttribute("disabled", "disabled");
+            btn.setAttribute("aria-busy", "true");
+
+            var text = btn.dataset.spinnerText || "Salvando...";
+            var iconClass = btn.dataset.spinnerIcon || "fa-solid fa-spinner fa-spin me-2";
+
+            if (btn.tagName === "BUTTON")
+            {
+                if (!btn.dataset.originalHtml) btn.dataset.originalHtml = btn.innerHTML;
+
+                btn.innerHTML = '<i class="' + htmlEscape(iconClass) + '"></i>' + htmlEscape(text);
+            } else if (btn.tagName === "INPUT")
+            {
+
+                if (!btn.dataset.originalValue) btn.dataset.originalValue = btn.value;
+                btn.value = text;
+
+            }
+            return true;
+        }
+        catch (erro)
+        {
+            console.error('Erro em lockButton:', erro);
+            return false;
+        }
     }
 
     function maybeValid(form)
     {
-
-        if (typeof form.checkValidity === "function" && !form.checkValidity()) return false;
-
-        var $ = window.jQuery;
-        if ($ && typeof $.fn.valid === "function")
-        {
-            var $form = $(form);
-            if ($form.data("validator") && !$form.valid()) return false;
-        }
-        return true;
+        try
+        {
+
+            if (typeof form.checkValidity === "function" && !form.checkValidity()) return false;
+
+            var $ = window.jQuery;
+            if ($ && typeof $.fn.valid === "function")
+            {
+                var $form = $(form);
+                if ($form.data("validator") && !$form.valid()) return false;
+            }
+            return true;
+        }
+        catch (erro)
+        {
+            console.error('Erro em maybeValid:', erro);
+            return true;
+        }
     }
 
     function onReady(fn)
     {
-        if (document.readyState === "loading")
-        {
-            document.addEventListener("DOMContentLoaded", fn);
-        } else
-        {
-            fn();
+        try
+        {
+            if (document.readyState === "loading")
+            {
+                document.addEventListener("DOMContentLoaded", fn);
+            } else
+            {
+                fn();
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em onReady:', erro);
         }
     }
 
@@ -221,52 +277,108 @@
 
 function formatarDataBR(raw)
 {
-    const s = (raw ?? '').toString().trim();
-    if (!s) return '';
-
-    const m = moment(s,
-        [
+    try
+    {
+        const s = (raw ?? '').toString().trim();
+        if (!s) return '';
+
+        const m = moment(s,
+            [
+                moment.ISO_8601,
+                "DD/MM/YYYY",
+                "D/M/YYYY",
+                "DD/MM/YYYY HH:mm",
+                "D/M/YYYY H:mm",
+                "YYYY-MM-DD",
+                "x"
+            ],
+            true
+        );
+
+        return m.isValid() ? m.format("DD/MM/YYYY") : '';
+    }
+    catch (erro)
+    {
+        console.error('Erro em formatarDataBR:', erro);
+        return '';
+    }
+}
+
+function formatarHora(raw, preferVazioSeSemHora = false)
+{
+    try
+    {
+        const s = (raw ?? '').toString().trim();
+        if (!s) return '';
+
+        const ticks = +((s.match(/\d+/) || [])[0]);
+        const candidato = s.startsWith('/Date(') ? ticks : s;
+
+        const m = moment(candidato, [
             moment.ISO_8601,
-            "DD/MM/YYYY",
-            "D/M/YYYY",
+            "DD/MM/YYYY HH:mm:ss",
+            "DD/MM/YYYY H:mm:ss",
             "DD/MM/YYYY HH:mm",
-            "D/M/YYYY H:mm",
+            "DD/MM/YYYY H:mm",
+            "YYYY-MM-DD HH:mm:ss",
+            "YYYY-MM-DD HH:mm",
             "YYYY-MM-DD",
             "x"
-        ],
-        true
-    );
-
-    return m.isValid() ? m.format("DD/MM/YYYY") : '';
+        ], true);
+
+        if (!m.isValid()) return '';
+
+        const temHora = /[T\s]\d{1,2}:\d{2}/.test(s);
+        if (preferVazioSeSemHora && !temHora) return '';
+
+        return m.format("HH:mm");
+    }
+    catch (erro)
+    {
+        console.error('Erro em formatarHora:', erro);
+        return '';
+    }
 }
 
-function formatarHora(raw, preferVazioSeSemHora = false)
-{
-    const s = (raw ?? '').toString().trim();
-    if (!s) return '';
-
-    const ticks = +((s.match(/\d+/) || [])[0]);
-    const candidato = s.startsWith('/Date(') ? ticks : s;
-
-    const m = moment(candidato, [
-        moment.ISO_8601,
-        "DD/MM/YYYY HH:mm:ss",
-        "DD/MM/YYYY H:mm:ss",
-        "DD/MM/YYYY HH:mm",
-        "DD/MM/YYYY H:mm",
-        "YYYY-MM-DD HH:mm:ss",
-        "YYYY-MM-DD HH:mm",
-        "YYYY-MM-DD",
-        "x"
-    ], true);
-
-    if (!m.isValid()) return '';
-
-    const temHora = /[T\s]\d{1,2}:\d{2}/.test(s);
-    if (preferVazioSeSemHora && !temHora) return '';
-
-    return m.format("HH:mm");
-}
+let ejTooltip = new ej.popups.Tooltip({
+    target: '[data-ejtip]',
+    opensOn: 'Hover',
+    beforeRender(args)
+    {
+        try
+        {
+            this.content = args.target.getAttribute('data-ejtip') || '';
+        }
+        catch (erro)
+        {
+            console.error('Erro em beforeRender (ejTooltip):', erro);
+        }
+    },
+    afterOpen(args)
+    {
+        try
+        {
+
+            setTimeout(() =>
+            {
+                try
+                {
+                    this.close();
+                }
+                catch (erro)
+                {
+                    console.error('Erro ao fechar tooltip:', erro);
+                }
+            }, 2000);
+        }
+        catch (erro)
+        {
+            console.error('Erro em afterOpen (ejTooltip):', erro);
+        }
+    }
+});
+
+ejTooltip.appendTo(document.body);
 
 function tiraAcento(texto)
 {
@@ -419,43 +531,57 @@
 
 function addRippleToElement(element, variant = 'light')
 {
-    if (!element) return;
-
-    element.classList.add('ftx-ripple');
-
-    if (variant === 'dark')
-    {
-        element.classList.add('ftx-ripple-dark');
-    }
-    else if (variant === 'subtle')
-    {
-        element.classList.add('ftx-ripple-subtle');
-    }
-    else if (variant === 'intense')
-    {
-        element.classList.add('ftx-ripple-intense');
-    }
-    else
-    {
-        element.classList.add('ftx-ripple-light');
-    }
-
-    const computedStyle = window.getComputedStyle(element);
-    if (computedStyle.position === 'static')
-    {
-        element.style.position = 'relative';
-    }
-    element.style.overflow = 'hidden';
+    try
+    {
+        if (!element) return;
+
+        element.classList.add('ftx-ripple');
+
+        if (variant === 'dark')
+        {
+            element.classList.add('ftx-ripple-dark');
+        }
+        else if (variant === 'subtle')
+        {
+            element.classList.add('ftx-ripple-subtle');
+        }
+        else if (variant === 'intense')
+        {
+            element.classList.add('ftx-ripple-intense');
+        }
+        else
+        {
+            element.classList.add('ftx-ripple-light');
+        }
+
+        const computedStyle = window.getComputedStyle(element);
+        if (computedStyle.position === 'static')
+        {
+            element.style.position = 'relative';
+        }
+        element.style.overflow = 'hidden';
+    }
+    catch (erro)
+    {
+        console.error('Erro em addRippleToElement:', erro);
+    }
 }
 
 function removeRippleFromElement(element)
 {
-    if (!element) return;
-
-    element.classList.remove('ftx-ripple', 'ftx-ripple-light', 'ftx-ripple-dark', 'ftx-ripple-subtle', 'ftx-ripple-intense');
-
-    const ripples = element.querySelectorAll('.ftx-ripple-circle');
-    ripples.forEach(r => r.remove());
+    try
+    {
+        if (!element) return;
+
+        element.classList.remove('ftx-ripple', 'ftx-ripple-light', 'ftx-ripple-dark', 'ftx-ripple-subtle', 'ftx-ripple-intense');
+
+        const ripples = element.querySelectorAll('.ftx-ripple-circle');
+        ripples.forEach(r => r.remove());
+    }
+    catch (erro)
+    {
+        console.error('Erro em removeRippleFromElement:', erro);
+    }
 }
 
 window.addRippleToElement = addRippleToElement;
@@ -474,68 +600,90 @@
     const LOADING_SELECTOR = '[data-ftx-loading]';
 
     function applyLoading(element) {
-        if (!element || element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
+        try
+        {
+            if (!element || element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
+                return false;
+            }
+
+            const icon = element.querySelector('i[class*="fa-"]');
+            if (icon) {
+                element.dataset.ftxOriginalIcon = icon.className;
+                icon.className = FTX_LOADING_CONFIG.spinnerClass;
+            } else {
+
+                const spinner = document.createElement('i');
+                spinner.className = FTX_LOADING_CONFIG.spinnerClass + ' me-1';
+                spinner.dataset.ftxTempSpinner = 'true';
+                element.insertBefore(spinner, element.firstChild);
+            }
+
+            const rect = element.getBoundingClientRect();
+            element.style.minWidth = rect.width + 'px';
+
+            element.classList.add(FTX_LOADING_CONFIG.loadingClass);
+
+            if (FTX_LOADING_CONFIG.disableOnClick) {
+                if (element.tagName === 'BUTTON' || element.tagName === 'INPUT') {
+                    element.disabled = true;
+                }
+                element.style.pointerEvents = 'none';
+            }
+
+            element._ftxLoadingTimeout = setTimeout(() => {
+                try
+                {
+                    resetLoading(element);
+                }
+                catch (erro)
+                {
+                    console.error('Erro no setTimeout de applyLoading:', erro);
+                }
+            }, FTX_LOADING_CONFIG.timeout);
+
+            return true;
+        }
+        catch (erro)
+        {
+            console.error('Erro em applyLoading:', erro);
             return false;
         }
-
-        const icon = element.querySelector('i[class*="fa-"]');
-        if (icon) {
-            element.dataset.ftxOriginalIcon = icon.className;
-            icon.className = FTX_LOADING_CONFIG.spinnerClass;
-        } else {
-
-            const spinner = document.createElement('i');
-            spinner.className = FTX_LOADING_CONFIG.spinnerClass + ' me-1';
-            spinner.dataset.ftxTempSpinner = 'true';
-            element.insertBefore(spinner, element.firstChild);
-        }
-
-        const rect = element.getBoundingClientRect();
-        element.style.minWidth = rect.width + 'px';
-
-        element.classList.add(FTX_LOADING_CONFIG.loadingClass);
-
-        if (FTX_LOADING_CONFIG.disableOnClick) {
+    }
+
+    function resetLoading(element) {
+        try
+        {
+            if (!element || !element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
+                return;
+            }
+
+            if (element._ftxLoadingTimeout) {
+                clearTimeout(element._ftxLoadingTimeout);
+                element._ftxLoadingTimeout = null;
+            }
+
+            const tempSpinner = element.querySelector('[data-ftx-temp-spinner]');
+            if (tempSpinner) {
+                tempSpinner.remove();
+            }
+
+            const icon = element.querySelector('i[class*="fa-spin"]');
+            if (icon && element.dataset.ftxOriginalIcon) {
+                icon.className = element.dataset.ftxOriginalIcon;
+                delete element.dataset.ftxOriginalIcon;
+            }
+
+            element.classList.remove(FTX_LOADING_CONFIG.loadingClass);
+            element.style.minWidth = '';
+            element.style.pointerEvents = '';
+
             if (element.tagName === 'BUTTON' || element.tagName === 'INPUT') {
-                element.disabled = true;
-            }
-            element.style.pointerEvents = 'none';
-        }
-
-        element._ftxLoadingTimeout = setTimeout(() => {
-            resetLoading(element);
-        }, FTX_LOADING_CONFIG.timeout);
-
-        return true;
-    }
-
-    function resetLoading(element) {
-        if (!element || !element.classList.contains(FTX_LOADING_CONFIG.loadingClass)) {
-            return;
-        }
-
-        if (element._ftxLoadingTimeout) {
-            clearTimeout(element._ftxLoadingTimeout);
-            element._ftxLoadingTimeout = null;
-        }
-
-        const tempSpinner = element.querySelector('[data-ftx-temp-spinner]');
-        if (tempSpinner) {
-            tempSpinner.remove();
-        }
-
-        const icon = element.querySelector('i[class*="fa-spin"]');
-        if (icon && element.dataset.ftxOriginalIcon) {
-            icon.className = element.dataset.ftxOriginalIcon;
-            delete element.dataset.ftxOriginalIcon;
-        }
-
-        element.classList.remove(FTX_LOADING_CONFIG.loadingClass);
-        element.style.minWidth = '';
-        element.style.pointerEvents = '';
-
-        if (element.tagName === 'BUTTON' || element.tagName === 'INPUT') {
-            element.disabled = false;
+                element.disabled = false;
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em resetLoading:', erro);
         }
     }
 
@@ -615,10 +763,14 @@
     });
 
     function initNavMenuClickEnhancement() {
-        var navMenu = document.getElementById('js-nav-menu');
-        if (!navMenu) return;
-
-        navMenu.querySelectorAll('li').forEach(function(li) {
+        try
+        {
+            var navMenu = document.getElementById('js-nav-menu');
+            if (!navMenu) return;
+
+            navMenu.querySelectorAll('li').forEach(function(li) {
+                try
+                {
             var submenu = li.querySelector(':scope > ul');
             var link = li.querySelector(':scope > a');
 
@@ -677,9 +829,19 @@
                     }
                 }
             });
-        });
-
-        console.log('FrotiX Nav Menu Click Enhancement inicializado');
+                }
+                catch (erroLi)
+                {
+                    console.error('Erro ao processar item do menu:', erroLi);
+                }
+            });
+
+            console.log('FrotiX Nav Menu Click Enhancement inicializado');
+        }
+        catch (erro)
+        {
+            console.error('Erro em initNavMenuClickEnhancement:', erro);
+        }
     }
 })();
 
@@ -727,47 +889,96 @@
 
 (function() {
     function aplicarValidacaoAntiEspacos() {
-        const seletores = 'input[type="text"], input[type="search"], input[type="email"], textarea';
-        const campos = document.querySelectorAll(seletores);
-        let contadorAplicado = 0;
-
-        campos.forEach(function(campo) {
-
-            if (campo.dataset.validacaoEspacosAplicada) return;
-            campo.dataset.validacaoEspacosAplicada = 'true';
-
-            campo.addEventListener('keypress', function(e) {
-                if ((!campo.value || campo.value.length === 0) && e.charCode === 32) {
-                    e.preventDefault();
-                    return false;
+        try
+        {
+            const seletores = 'input[type="text"], input[type="search"], input[type="email"], textarea';
+            const campos = document.querySelectorAll(seletores);
+            let contadorAplicado = 0;
+
+            campos.forEach(function(campo) {
+                try
+                {
+
+                    if (campo.dataset.validacaoEspacosAplicada) return;
+                    campo.dataset.validacaoEspacosAplicada = 'true';
+
+                    campo.addEventListener('keypress', function(e) {
+                        try
+                        {
+                            if ((!campo.value || campo.value.length === 0) && e.charCode === 32) {
+                                e.preventDefault();
+                                return false;
+                            }
+                        }
+                        catch (erro)
+                        {
+                            console.error('Erro no keypress listener:', erro);
+                        }
+                    });
+
+                    campo.addEventListener('input', function(e) {
+                        try
+                        {
+                            if (campo.value && campo.value[0] === ' ') {
+                                campo.value = campo.value.trimStart();
+                            }
+                        }
+                        catch (erro)
+                        {
+                            console.error('Erro no input listener:', erro);
+                        }
+                    });
+
+                    campo.addEventListener('paste', function(e) {
+                        try
+                        {
+                            setTimeout(() => {
+                                try
+                                {
+                                    if (campo.value && campo.value[0] === ' ') {
+                                        campo.value = campo.value.trimStart();
+                                    }
+                                }
+                                catch (erro)
+                                {
+                                    console.error('Erro no setTimeout do paste:', erro);
+                                }
+                            }, 10);
+                        }
+                        catch (erro)
+                        {
+                            console.error('Erro no paste listener:', erro);
+                        }
+                    });
+
+                    campo.addEventListener('blur', function(e) {
+                        try
+                        {
+                            if (campo.value && campo.value !== campo.value.trim()) {
+                                campo.value = campo.value.trim();
+                            }
+                        }
+                        catch (erro)
+                        {
+                            console.error('Erro no blur listener:', erro);
+                        }
+                    });
+
+                    contadorAplicado++;
+                }
+                catch (erroCampo)
+                {
+                    console.error('Erro ao processar campo:', erroCampo);
                 }
             });
 
-            campo.addEventListener('input', function(e) {
-                if (campo.value && campo.value[0] === ' ') {
-                    campo.value = campo.value.trimStart();
-                }
-            });
-
-            campo.addEventListener('paste', function(e) {
-                setTimeout(() => {
-                    if (campo.value && campo.value[0] === ' ') {
-                        campo.value = campo.value.trimStart();
-                    }
-                }, 10);
-            });
-
-            campo.addEventListener('blur', function(e) {
-                if (campo.value && campo.value !== campo.value.trim()) {
-                    campo.value = campo.value.trim();
-                }
-            });
-
-            contadorAplicado++;
-        });
-
-        if (contadorAplicado > 0) {
-            console.log(`✅ Validação anti-espaços aplicada em ${contadorAplicado} campos`);
+            if (contadorAplicado > 0) {
+                console.log(`✅ Validação anti-espaços aplicada em ${contadorAplicado} campos`);
+            }
+        }
+        catch (erro)
+        {
+            console.error('Erro em aplicarValidacaoAntiEspacos:', erro);
         }
     }
 
```
