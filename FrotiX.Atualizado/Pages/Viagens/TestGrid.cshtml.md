# Pages/Viagens/TestGrid.cshtml

**Mudanca:** GRANDE | **+26** linhas | **-9** linhas

---

```diff
--- JANEIRO: Pages/Viagens/TestGrid.cshtml
+++ ATUAL: Pages/Viagens/TestGrid.cshtml
@@ -120,7 +120,7 @@
                                                     <th>Celular</th>
                                                     <th>Unidade</th>
                                                     <th>Status</th>
-                                                    <th>Ações</th>
+                                                    <th>Ação</th>
                                                 </tr>
                                             </thead>
                                             <tbody>
@@ -160,17 +160,33 @@
 
 @section ScriptsBlock
 {
+    <script>
+        /***
+         * ⚡ FUNÇÃO: Inicialização do TabStrip Kendo
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Inicializar componente TabStrip Kendo com animação de fadeIn
+         *
+         * 📥 ENTRADAS : #tabstrip elemento HTML
+         *
+         * 📤 SAÍDAS : TabStrip ativado com animações
+         *
+         * ⬅️ CHAMADO POR : document.ready event
+         *
+         * ➡️ CHAMA : kendoTabStrip() [Kendo UI widget]
+         ***/
+        $(document).ready(function () {
+            try {
 
-    <script>
-        $(document).ready(function () {
-            $("#tabstrip").kendoTabStrip({
-                animation: {
-                    open: {
-                        effects: "fadeIn"
+                $("#tabstrip").kendoTabStrip({
+                    animation: {
+                        open: {
+                            effects: "fadeIn"
+                        }
                     }
-                }
-            });
+                });
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("TestGrid.cshtml", "TabStripInit", error);
+            }
         });
     </script>
-
 }
```

### REMOVER do Janeiro

```html
                                                    <th>Ações</th>
    <script>
        $(document).ready(function () {
            $("#tabstrip").kendoTabStrip({
                animation: {
                    open: {
                        effects: "fadeIn"
                }
            });
```


### ADICIONAR ao Janeiro

```html
                                                    <th>Ação</th>
    <script>
        /***
         * ⚡ FUNÇÃO: Inicialização do TabStrip Kendo
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO : Inicializar componente TabStrip Kendo com animação de fadeIn
         *
         * 📥 ENTRADAS : #tabstrip elemento HTML
         *
         * 📤 SAÍDAS : TabStrip ativado com animações
         *
         * ⬅️ CHAMADO POR : document.ready event
         *
         * ➡️ CHAMA : kendoTabStrip() [Kendo UI widget]
         ***/
        $(document).ready(function () {
            try {
                $("#tabstrip").kendoTabStrip({
                    animation: {
                        open: {
                            effects: "fadeIn"
                        }
                });
            } catch (error) {
                Alerta.TratamentoErroComLinha("TestGrid.cshtml", "TabStripInit", error);
            }
```
