# wwwroot/js/kendo-error-suppressor.js

**Mudanca:** GRANDE | **+29** linhas | **-48** linhas

---

```diff
--- JANEIRO: wwwroot/js/kendo-error-suppressor.js
+++ ATUAL: wwwroot/js/kendo-error-suppressor.js
@@ -4,70 +4,47 @@
     const originalError = console.error;
     const originalOnError = window.onerror;
 
-    const syncfusionFormatErrors = [
-        'percentsign',
-        'currencysign',
-        'decimalseparator',
-        'groupseparator',
-        'format options',
-        'type given must be invalid'
-    ];
+    console.error = function(...args) {
+        try {
+            const msg = args.join(' ').toLowerCase();
 
-    function isSyncfusionFormatError(msg) {
-        const lowerMsg = msg.toLowerCase();
-        return syncfusionFormatErrors.some(err => lowerMsg.includes(err));
-    }
+            if (msg.includes('collapsible') ||
+                (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
+                console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
+                return;
+            }
 
-    console.error = function(...args) {
-        const msg = args.join(' ').toLowerCase();
+            originalError.apply(console, args);
+        } catch (erro) {
 
-        if (msg.includes('collapsible') ||
-            (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
-            console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
-            return;
+            originalError.apply(console, args);
         }
-
-        if (isSyncfusionFormatError(msg)) {
-            console.warn('[SUPRIMIDO] Erro de formatação Syncfusion ignorado:', ...args);
-            return;
-        }
-
-        originalError.apply(console, args);
     };
 
     window.onerror = function(message, source, lineno, colno, error) {
-        const msg = (message || '').toString().toLowerCase();
-        const src = (source || '').toString().toLowerCase();
+        try {
+            const msg = (message || '').toString().toLowerCase();
+            const src = (source || '').toString().toLowerCase();
 
-        if (msg.includes('collapsible') ||
-            (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
-            console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
-            return true;
+            if (msg.includes('collapsible') ||
+                (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
+                console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
+                return true;
+            }
+
+            if (originalOnError) {
+                return originalOnError.apply(this, arguments);
+            }
+
+            return false;
+        } catch (erro) {
+
+            if (originalOnError) {
+                return originalOnError.apply(this, arguments);
+            }
+            return false;
         }
-
-        if (isSyncfusionFormatError(msg) ||
-            (src.includes('syncfusion') && msg.includes('cannot read properties of undefined')) ||
-            (src.includes('ej2') && msg.includes('cannot read properties of undefined'))) {
-            console.warn('[SUPRIMIDO] Erro global Syncfusion ignorado:', message);
-            return true;
-        }
-
-        if (originalOnError) {
-            return originalOnError.apply(this, arguments);
-        }
-
-        return false;
     };
 
-    window.addEventListener('unhandledrejection', function(event) {
-        const reason = (event.reason || '').toString().toLowerCase();
-
-        if (isSyncfusionFormatError(reason)) {
-            console.warn('[SUPRIMIDO] Promise rejection Syncfusion ignorada:', event.reason);
-            event.preventDefault();
-            return;
-        }
-    });
-
-    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo e Syncfusion serão suprimidos');
+    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo serão suprimidos');
 })();
```

### REMOVER do Janeiro

```javascript
    const syncfusionFormatErrors = [
        'percentsign',
        'currencysign',
        'decimalseparator',
        'groupseparator',
        'format options',
        'type given must be invalid'
    ];
    function isSyncfusionFormatError(msg) {
        const lowerMsg = msg.toLowerCase();
        return syncfusionFormatErrors.some(err => lowerMsg.includes(err));
    }
    console.error = function(...args) {
        const msg = args.join(' ').toLowerCase();
        if (msg.includes('collapsible') ||
            (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
            console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
            return;
        if (isSyncfusionFormatError(msg)) {
            console.warn('[SUPRIMIDO] Erro de formatação Syncfusion ignorado:', ...args);
            return;
        }
        originalError.apply(console, args);
        const msg = (message || '').toString().toLowerCase();
        const src = (source || '').toString().toLowerCase();
        if (msg.includes('collapsible') ||
            (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
            console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
            return true;
        if (isSyncfusionFormatError(msg) ||
            (src.includes('syncfusion') && msg.includes('cannot read properties of undefined')) ||
            (src.includes('ej2') && msg.includes('cannot read properties of undefined'))) {
            console.warn('[SUPRIMIDO] Erro global Syncfusion ignorado:', message);
            return true;
        }
        if (originalOnError) {
            return originalOnError.apply(this, arguments);
        }
        return false;
    window.addEventListener('unhandledrejection', function(event) {
        const reason = (event.reason || '').toString().toLowerCase();
        if (isSyncfusionFormatError(reason)) {
            console.warn('[SUPRIMIDO] Promise rejection Syncfusion ignorada:', event.reason);
            event.preventDefault();
            return;
        }
    });
    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo e Syncfusion serão suprimidos');
```


### ADICIONAR ao Janeiro

```javascript
    console.error = function(...args) {
        try {
            const msg = args.join(' ').toLowerCase();
            if (msg.includes('collapsible') ||
                (msg.includes('cannot read properties of undefined') && msg.includes('toggle'))) {
                console.warn('[SUPRIMIDO] Erro do Kendo ignorado:', ...args);
                return;
            }
            originalError.apply(console, args);
        } catch (erro) {
            originalError.apply(console, args);
        try {
            const msg = (message || '').toString().toLowerCase();
            const src = (source || '').toString().toLowerCase();
            if (msg.includes('collapsible') ||
                (msg.includes('cannot read properties of undefined') && src.includes('kendo'))) {
                console.warn('[SUPRIMIDO] Erro global do Kendo ignorado:', message);
                return true;
            }
            if (originalOnError) {
                return originalOnError.apply(this, arguments);
            }
            return false;
        } catch (erro) {
            if (originalOnError) {
                return originalOnError.apply(this, arguments);
            }
            return false;
    console.log('[SUPRESSOR] ✅ Ativo - erros do Kendo serão suprimidos');
```
