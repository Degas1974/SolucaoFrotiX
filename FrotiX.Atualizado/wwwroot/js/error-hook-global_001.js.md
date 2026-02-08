# wwwroot/js/error-hook-global_001.js

**Mudanca:** GRANDE | **+32** linhas | **-17** linhas

---

```diff
--- JANEIRO: wwwroot/js/error-hook-global_001.js
+++ ATUAL: wwwroot/js/error-hook-global_001.js
@@ -4,19 +4,24 @@
     var seen = new Set();
 
     function extractProblem(raw) {
-        var obj = null;
-        if (raw && typeof raw === 'object') {
-            obj = raw;
-        } else if (typeof raw === 'string') {
-            try { obj = JSON.parse(raw); } catch { obj = {}; }
-        } else {
-            obj = raw || {};
+        try {
+            var obj = null;
+            if (raw && typeof raw === 'object') {
+                obj = raw;
+            } else if (typeof raw === 'string') {
+                try { obj = JSON.parse(raw); } catch { obj = {}; }
+            } else {
+                obj = raw || {};
+            }
+
+            var detail = (obj && (obj.detail || obj.title)) || 'Erro inesperado.';
+            var ext = (obj && (obj.extensions || obj)) || {};
+            var corr = obj.correlationId || ext.correlationId || null;
+            return { detail: detail, corr: corr, problem: obj };
+        } catch (erro) {
+            console.error('[FrotiX ErrorHook] Erro em extractProblem:', erro);
+            return { detail: 'Erro inesperado.', corr: null, problem: {} };
         }
-
-        var detail = (obj && (obj.detail || obj.title)) || 'Erro inesperado.';
-        var ext = (obj && (obj.extensions || obj)) || {};
-        var corr = obj.correlationId || ext.correlationId || null;
-        return { detail: detail, corr: corr, problem: obj };
     }
 
     function showProblem(detail, corr, source) {
@@ -35,15 +40,25 @@
     }
 
     function shouldHandle(status) {
-        return Number(status) >= 500;
+        try {
+            return Number(status) >= 500;
+        } catch (erro) {
+            console.error('[FrotiX ErrorHook] Erro em shouldHandle:', erro);
+            return false;
+        }
     }
 
     function stamp(corr) {
-        if (!corr) return true;
-        if (seen.has(corr)) return false;
-        seen.add(corr);
-        setTimeout(function () { try { seen.delete(corr); } catch {} }, 10000);
-        return true;
+        try {
+            if (!corr) return true;
+            if (seen.has(corr)) return false;
+            seen.add(corr);
+            setTimeout(function () { try { seen.delete(corr); } catch {} }, 10000);
+            return true;
+        } catch (erro) {
+            console.error('[FrotiX ErrorHook] Erro em stamp:', erro);
+            return true;
+        }
     }
 
     if (window.jQuery) {
```

### REMOVER do Janeiro

```javascript
        var obj = null;
        if (raw && typeof raw === 'object') {
            obj = raw;
        } else if (typeof raw === 'string') {
            try { obj = JSON.parse(raw); } catch { obj = {}; }
        } else {
            obj = raw || {};
        var detail = (obj && (obj.detail || obj.title)) || 'Erro inesperado.';
        var ext = (obj && (obj.extensions || obj)) || {};
        var corr = obj.correlationId || ext.correlationId || null;
        return { detail: detail, corr: corr, problem: obj };
        return Number(status) >= 500;
        if (!corr) return true;
        if (seen.has(corr)) return false;
        seen.add(corr);
        setTimeout(function () { try { seen.delete(corr); } catch {} }, 10000);
        return true;
```


### ADICIONAR ao Janeiro

```javascript
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
        try {
            return Number(status) >= 500;
        } catch (erro) {
            console.error('[FrotiX ErrorHook] Erro em shouldHandle:', erro);
            return false;
        }
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
```
