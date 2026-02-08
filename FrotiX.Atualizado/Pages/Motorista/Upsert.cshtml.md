# Pages/Motorista/Upsert.cshtml

**Mudanca:** MEDIA | **+0** linhas | **-11** linhas

---

```diff
--- JANEIRO: Pages/Motorista/Upsert.cshtml
+++ ATUAL: Pages/Motorista/Upsert.cshtml
@@ -732,18 +732,6 @@
     <script src="~/js/cadastros/motorista_upsert.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VALIDAÇÃO E SUBMISSÃO DO FORMULÁRIO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Valida formulário e prepara submissão
-             * @@description Chama validarFormulario() do arquivo externo e ativa loading
-            * @@param { HTMLButtonElement } btn - Botão de submit para exibir estado de loading
-                * @@returns { boolean } true se formulário válido, false caso contrário
-                    */
         function validarESubmeter(btn) {
             try {
                 if (!validarFormulario()) {
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * VALIDAÇÃO E SUBMISSÃO DO FORMULÁRIO
             * ═══════════════════════════════════════════════════════════════════════════
             */
            /**
             * Valida formulário e prepara submissão
             * @@description Chama validarFormulario() do arquivo externo e ativa loading
            * @@param { HTMLButtonElement } btn - Botão de submit para exibir estado de loading
                * @@returns { boolean } true se formulário válido, false caso contrário
                    */
```

