# Pages/Multa/UpsertAutuacao.cshtml

**Mudanca:** GRANDE | **+11** linhas | **-27** linhas

---

```diff
--- JANEIRO: Pages/Multa/UpsertAutuacao.cshtml
+++ ATUAL: Pages/Multa/UpsertAutuacao.cshtml
@@ -794,24 +794,22 @@
     <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.common.min.css" rel="stylesheet" />
     <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.default.min.css" rel="stylesheet" />
     <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/cultures/kendo.culture.pt-BR.min.js"></script>
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/messages/kendo.messages.pt-BR.min.js"></script>
+    <script>
+        try {
+            if (window.kendo && kendo.culture) {
+                kendo.culture("pt-BR");
+            }
+        } catch (error) {
+            console.warn("Kendo pt-BR: falha ao aplicar cultura.", error);
+        }
+    </script>
 
     <script src="~/js/viagens/kendo-editor-upsert.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * UTILITÁRIOS DE FORMATAÇÃO DE MOEDA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções para conversão entre formatos brasileiro e numérico
-         * para campos de valor monetário em autuações.
-         */
-
-        /**
-         * Converte valor em formato brasileiro para número.
-         * @@param {string} str - Valor no formato "R$ 10.000,50" ou "10.000,50".
-         * @@returns {number} Valor numérico (ex: 10000.50).
-         * @@example parseCurrencyBR("R$ 1.234,56")
-         */
+
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
@@ -827,15 +825,6 @@
             }
         }
 
-        /**
-         * Máscara de moeda em tempo real para campos input.
-         * @@param {HTMLInputElement} input - Campo de entrada.
-         * @@param {string} sep - Separador de milhar (padrão ".").
-         * @@param {string} dec - Separador decimal (padrão ",").
-         * @@param {KeyboardEvent} event - Evento de teclado.
-         * @@returns {boolean} False para prevenir input padrão.
-         * @@description Para R$ 1.000,00 digite 100000 (últimos 2 = centavos).
-         */
         function moeda(input, sep, dec, event) {
             try {
                 let digitado = "",
@@ -898,11 +887,6 @@
             }
         }
 
-        /**
-         * Formata número para exibição em formato brasileiro.
-         * @@param {number} valor - Valor numérico.
-         * @@returns {string} Valor formatado "R$ 10.000,50".
-         */
         function formatCurrencyBR(valor) {
             try {
                 if (!valor && valor !== 0) return "";
```

### REMOVER do Janeiro

```html
        /**
         * ═══════════════════════════════════════════════════════════════════════════
         * UTILITÁRIOS DE FORMATAÇÃO DE MOEDA
         * ═══════════════════════════════════════════════════════════════════════════
         * @@description Funções para conversão entre formatos brasileiro e numérico
         * para campos de valor monetário em autuações.
         */
        /**
         * Converte valor em formato brasileiro para número.
         * @@param {string} str - Valor no formato "R$ 10.000,50" ou "10.000,50".
         * @@returns {number} Valor numérico (ex: 10000.50).
         * @@example parseCurrencyBR("R$ 1.234,56")
         */
        /**
         * Máscara de moeda em tempo real para campos input.
         * @@param {HTMLInputElement} input - Campo de entrada.
         * @@param {string} sep - Separador de milhar (padrão ".").
         * @@param {string} dec - Separador decimal (padrão ",").
         * @@param {KeyboardEvent} event - Evento de teclado.
         * @@returns {boolean} False para prevenir input padrão.
         * @@description Para R$ 1.000,00 digite 100000 (últimos 2 = centavos).
         */
        /**
         * Formata número para exibição em formato brasileiro.
         * @@param {number} valor - Valor numérico.
         * @@returns {string} Valor formatado "R$ 10.000,50".
         */
```


### ADICIONAR ao Janeiro

```html
    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/cultures/kendo.culture.pt-BR.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/messages/kendo.messages.pt-BR.min.js"></script>
    <script>
        try {
            if (window.kendo && kendo.culture) {
                kendo.culture("pt-BR");
            }
        } catch (error) {
            console.warn("Kendo pt-BR: falha ao aplicar cultura.", error);
        }
    </script>
```
