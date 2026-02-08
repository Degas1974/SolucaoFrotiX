# Pages/Combustivel/Upsert.cshtml

**Mudanca:** MEDIA | **+28** linhas | **-0** linhas

---

```diff
--- JANEIRO: Pages/Combustivel/Upsert.cshtml
+++ ATUAL: Pages/Combustivel/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Combustivel.UpsertModel
@@ -111,4 +110,37 @@
         crossorigin="anonymous" />
     <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
         crossorigin="anonymous"></script>
+
+    <script>
+        /***
+         * ⚡ ARQUIVO: Pages/Combustivel/Upsert.cshtml - JavaScript Inicialização
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Configurar eventos de submit do formulário com spinner
+         *
+         * 📥 ENTRADAS : Formulário #form (asp-action="Upsert"), botão #btnSalvarCombustivel
+         *
+         * 📤 SAÍDAS : Spinner ativado no botão, POST enviado para servidor
+         *
+         * ➡️ CHAMA : UpsertModel.OnPostSubmit (code-behind)
+         ***/
+        $(document).ready(function() {
+            try {
+
+                $('form').on('submit', function(e) {
+                    try {
+                        const btn = $('button[type="submit"]');
+
+                        btn.prop('disabled', true);
+                        btn.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Salvando...');
+
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Combustivel/Upsert.cshtml", "formSubmit", error);
+                    }
+                });
+
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Combustivel/Upsert.cshtml", "document.ready", error);
+            }
+        });
+    </script>
 }
```

### ADICIONAR ao Janeiro

```html
    <script>
        /***
         * ⚡ ARQUIVO: Pages/Combustivel/Upsert.cshtml - JavaScript Inicialização
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Configurar eventos de submit do formulário com spinner
         *
         * 📥 ENTRADAS : Formulário #form (asp-action="Upsert"), botão #btnSalvarCombustivel
         *
         * 📤 SAÍDAS : Spinner ativado no botão, POST enviado para servidor
         *
         * ➡️ CHAMA : UpsertModel.OnPostSubmit (code-behind)
         ***/
        $(document).ready(function() {
            try {
                $('form').on('submit', function(e) {
                    try {
                        const btn = $('button[type="submit"]');
                        btn.prop('disabled', true);
                        btn.html('<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>Salvando...');
                    } catch (error) {
                        Alerta.TratamentoErroComLinha("Combustivel/Upsert.cshtml", "formSubmit", error);
                    }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("Combustivel/Upsert.cshtml", "document.ready", error);
            }
        });
    </script>
```
