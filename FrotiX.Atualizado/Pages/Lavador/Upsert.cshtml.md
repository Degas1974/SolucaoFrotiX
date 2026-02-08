# Pages/Lavador/Upsert.cshtml

**Mudanca:** GRANDE | **+9** linhas | **-31** linhas

---

```diff
--- JANEIRO: Pages/Lavador/Upsert.cshtml
+++ ATUAL: Pages/Lavador/Upsert.cshtml
@@ -542,17 +542,7 @@
 
 @section ScriptsBlock {
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE MÁSCARA E VALIDAÇÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Aplica máscara de CPF (999.999.999-99)
-             * @@description Formata o valor digitado no padrão brasileiro de CPF
-            * @@param { HTMLInputElement } i - Elemento input que recebe a máscara
-                */
+
         function mascara(i) {
             try {
                 var v = i.value || "";
@@ -574,29 +564,17 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * PREVIEW DE FOTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Exibe preview da foto selecionada antes do upload
-            */
-            $("#txtFile").on("change", function (event) {
-                try {
-                    var files = event.target.files;
-                    if (files && files[0]) {
-                        $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "txtFile.change", error);
+        $("#txtFile").on("change", function (event) {
+            try {
+                var files = event.target.files;
+                if (files && files[0]) {
+                    $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
                 }
-            });
-
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DO DOCUMENTO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Carrega foto do lavador via AJAX ou define foto padrão
-            */
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Upsert.cshtml", "txtFile.change", error);
+            }
+        });
+
         $(document).ready(function () {
             try {
                 const lavadorId = '@Model.LavadorObj.Lavador.LavadorId';
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * FUNÇÕES DE MÁSCARA E VALIDAÇÃO
             * ═══════════════════════════════════════════════════════════════════════════
             */
            /**
             * Aplica máscara de CPF (999.999.999-99)
             * @@description Formata o valor digitado no padrão brasileiro de CPF
            * @@param { HTMLInputElement } i - Elemento input que recebe a máscara
                */
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * PREVIEW DE FOTO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Exibe preview da foto selecionada antes do upload
            */
            $("#txtFile").on("change", function (event) {
                try {
                    var files = event.target.files;
                    if (files && files[0]) {
                        $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "txtFile.change", error);
            });
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * INICIALIZAÇÃO DO DOCUMENTO
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Carrega foto do lavador via AJAX ou define foto padrão
            */
```


### ADICIONAR ao Janeiro

```html
        $("#txtFile").on("change", function (event) {
            try {
                var files = event.target.files;
                if (files && files[0]) {
                    $("#imgViewer").attr("src", window.URL.createObjectURL(files[0]));
            } catch (error) {
                Alerta.TratamentoErroComLinha("Upsert.cshtml", "txtFile.change", error);
            }
        });
```
