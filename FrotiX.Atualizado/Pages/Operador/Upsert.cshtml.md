# Pages/Operador/Upsert.cshtml

**Mudanca:** MEDIA | **+0** linhas | **-15** linhas

---

```diff
--- JANEIRO: Pages/Operador/Upsert.cshtml
+++ ATUAL: Pages/Operador/Upsert.cshtml
@@ -542,18 +542,7 @@
 
 @section ScriptsBlock {
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * OPERADOR - SCRIPTS DE FORMULÁRIO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Máscara de CPF e preview de foto para cadastro de operadores.
-             */
-
-            /**
-             * Aplica máscara de CPF no campo.
-             * @@param { HTMLInputElement } i - Campo de input do CPF.
-             * @@description Formata como 000.000.000-00.
-             */
+
         function mascara(i) {
             try {
                 var v = i.value || "";
@@ -575,10 +564,6 @@
             }
         }
 
-            /**
-             * Preview de foto selecionada.
-             * @@description Atualiza img#imgViewer com arquivo selecionado.
-             */
         $("#txtFile").on("change", function (event) {
             try {
                 var files = event.target.files;
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * OPERADOR - SCRIPTS DE FORMULÁRIO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Máscara de CPF e preview de foto para cadastro de operadores.
             */
            /**
             * Aplica máscara de CPF no campo.
             * @@param { HTMLInputElement } i - Campo de input do CPF.
             * @@description Formata como 000.000.000-00.
             */
            /**
             * Preview de foto selecionada.
             * @@description Atualiza img#imgViewer com arquivo selecionado.
             */
```

