# Pages/Fornecedor/Upsert.cshtml

**Mudanca:** MEDIA | **+18** linhas | **-9** linhas

---

```diff
--- JANEIRO: Pages/Fornecedor/Upsert.cshtml
+++ ATUAL: Pages/Fornecedor/Upsert.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Fornecedor.UpsertModel
@@ -175,24 +174,35 @@
         crossorigin="anonymous"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MÁSCARA DE CNPJ EM TEMPO REAL
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Aplica máscara de formatação CNPJ(99.999.999 / 9999 - 99)
-            * durante a digitação no campo #cnpj
-                */
+        /***
+         * ⚡ FUNÇÃO: Formatação de CNPJ (Input Listener)
+         * ============================================================================
+         * 🎯 OBJETIVO : Formatar automaticamente entrada de CNPJ em padrão
+         * 00.000.000/0000-00 durante digitação
+         *
+         * 📥 ENTRADAS : Input #cnpj - valores digitados pelo usuário
+         *
+         * 📤 SAÍDAS : Input #cnpj com CNPJ formatado (XX.XXX.XXX/XXXX-XX)
+         *
+         * ⬅️ CHAMADO POR : addEventListener('input') disparado a cada digitação
+         *
+         * 📝 OBSERVAÇÕES : [VALIDACAO] Regex remove não-dígitos (/\D/g),
+         * agrupa em pattern (\d{0,2})(\d{0,3})(\d{0,3})(\d{0,4})(\d{0,2}),
+         * monta string formatada: 00.000.000/0000-00
+         ***/
         try {
             document.getElementById('cnpj').addEventListener('input', function (e) {
                 try {
+
                     var x = e.target.value.replace(/\D/g, '').match(/(\d{0,2})(\d{0,3})(\d{0,3})(\d{0,4})(\d{0,2})/);
+
                     e.target.value = !x[2] ? x[1] : x[1] + '.' + x[2] + '.' + x[3] + '/' + x[4] + (x[5] ? '-' + x[5] : '');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "cnpj.input.listener", error);
+                    Alerta.TratamentoErroComLinha("Fornecedor/Upsert.cshtml", "cnpj.input.listener", error);
                 }
             });
         } catch (error) {
-            Alerta.TratamentoErroComLinha("Upsert.cshtml", "cnpj.setup", error);
+            Alerta.TratamentoErroComLinha("Fornecedor/Upsert.cshtml", "cnpj.setup", error);
         }
     </script>
 }
```

### REMOVER do Janeiro

```html
            /**
             * ═══════════════════════════════════════════════════════════════════════════
             * MÁSCARA DE CNPJ EM TEMPO REAL
             * ═══════════════════════════════════════════════════════════════════════════
             * @@description Aplica máscara de formatação CNPJ(99.999.999 / 9999 - 99)
            * durante a digitação no campo #cnpj
                */
                    Alerta.TratamentoErroComLinha("Upsert.cshtml", "cnpj.input.listener", error);
            Alerta.TratamentoErroComLinha("Upsert.cshtml", "cnpj.setup", error);
```


### ADICIONAR ao Janeiro

```html
        /***
         * ⚡ FUNÇÃO: Formatação de CNPJ (Input Listener)
         * ============================================================================
         * 🎯 OBJETIVO : Formatar automaticamente entrada de CNPJ em padrão
         * 00.000.000/0000-00 durante digitação
         *
         * 📥 ENTRADAS : Input #cnpj - valores digitados pelo usuário
         *
         * 📤 SAÍDAS : Input #cnpj com CNPJ formatado (XX.XXX.XXX/XXXX-XX)
         *
         * ⬅️ CHAMADO POR : addEventListener('input') disparado a cada digitação
         *
         * 📝 OBSERVAÇÕES : [VALIDACAO] Regex remove não-dígitos (/\D/g),
         * agrupa em pattern (\d{0,2})(\d{0,3})(\d{0,3})(\d{0,4})(\d{0,2}),
         * monta string formatada: 00.000.000/0000-00
         ***/
                    Alerta.TratamentoErroComLinha("Fornecedor/Upsert.cshtml", "cnpj.input.listener", error);
            Alerta.TratamentoErroComLinha("Fornecedor/Upsert.cshtml", "cnpj.setup", error);
```
