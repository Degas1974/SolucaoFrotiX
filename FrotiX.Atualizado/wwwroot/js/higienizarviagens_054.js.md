# wwwroot/js/higienizarviagens_054.js

**Mudanca:** GRANDE | **+55** linhas | **-30** linhas

---

```diff
--- JANEIRO: wwwroot/js/higienizarviagens_054.js
+++ ATUAL: wwwroot/js/higienizarviagens_054.js
@@ -1,43 +1,69 @@
 function mostrarLoading(texto) {
-    const overlay = document.getElementById('loadingOverlayHigienizar');
-    if (overlay) {
-        if (texto) {
-            const loadingText = overlay.querySelector('.ftx-loading-text');
-            if (loadingText) loadingText.textContent = texto;
-        }
-        overlay.style.display = 'flex';
-    }
-
-    document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
-        btn.disabled = true;
-        btn.classList.add('btn-disabled-loading');
-    });
+    try {
+        const overlay = document.getElementById('loadingOverlayHigienizar');
+        if (overlay) {
+            if (texto) {
+                const loadingText = overlay.querySelector('.ftx-loading-text');
+                if (loadingText) loadingText.textContent = texto;
+            }
+            overlay.style.display = 'flex';
+        }
+
+        document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
+            btn.disabled = true;
+            btn.classList.add('btn-disabled-loading');
+        });
+    } catch (erro) {
+        console.error('[FrotiX] Erro em mostrarLoading:', erro);
+    }
 }
 
 function esconderLoading() {
-    const overlay = document.getElementById('loadingOverlayHigienizar');
-    if (overlay) {
-        overlay.style.display = 'none';
-    }
-
-    document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
-        btn.disabled = false;
-        btn.classList.remove('btn-disabled-loading');
-    });
-}
-
-function showLoading() { mostrarLoading(); }
-function hideLoading() { esconderLoading(); }
+    try {
+        const overlay = document.getElementById('loadingOverlayHigienizar');
+        if (overlay) {
+            overlay.style.display = 'none';
+        }
+
+        document.querySelectorAll('button, input[type="button"], input[type="submit"]').forEach(btn => {
+            btn.disabled = false;
+            btn.classList.remove('btn-disabled-loading');
+        });
+    } catch (erro) {
+        console.error('[FrotiX] Erro em esconderLoading:', erro);
+    }
+}
+
+function showLoading() {
+    try {
+        mostrarLoading();
+    } catch (erro) {
+        console.error('[FrotiX] Erro em showLoading:', erro);
+    }
+}
+
+function hideLoading() {
+    try {
+        esconderLoading();
+    } catch (erro) {
+        console.error('[FrotiX] Erro em hideLoading:', erro);
+    }
+}
 
 function normalizarTexto(texto)
 {
-    return texto
-        .normalize('NFD')
-        .replace(/[\u0300-\u036f]/g, '')
-        .replace(/\s+/g, ' ')
-        .replace(/\
-        .trim()
-        .toLowerCase();
+    try {
+        return texto
+            .normalize('NFD')
+            .replace(/[\u0300-\u036f]/g, '')
+            .replace(/\s+/g, ' ')
+            .replace(/\
+            .trim()
+            .toLowerCase();
+    } catch (erro) {
+        console.error('[FrotiX] Erro em normalizarTexto:', erro);
+        return String(texto || '').toLowerCase();
+    }
 }
 
 function moverSelecionados(origemId, destinoId)
```
