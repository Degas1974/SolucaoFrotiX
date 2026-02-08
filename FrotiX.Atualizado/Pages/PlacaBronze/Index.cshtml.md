# Pages/PlacaBronze/Index.cshtml

**Mudanca:** MEDIA | **+19** linhas | **-1** linhas

---

```diff
--- JANEIRO: Pages/PlacaBronze/Index.cshtml
+++ ATUAL: Pages/PlacaBronze/Index.cshtml
@@ -60,7 +60,7 @@
                                     <th>Descrição da Placa</th>
                                     <th>Veículo Associado</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody></tbody>
@@ -73,6 +73,24 @@
 </div>
 
 @section ScriptsBlock {
+    <script>
+        /***
+         * ⚡ ARQUIVO: placabronze.js (Externo - Arquivo dedicado)
+         * ============================================================================
+         * 📥 RESPONSABILIDADES:
+         * • Flag anti-reinit: placaBronzeInitialized (evita double binding)
+         * • loadList() - Busca placas via AJAX para DataTable
+         * • Delete handlers - Validação de dependências antes de excluir
+         * • Status toggle - Ativar/inativar placa via AJAX
+         * • Event cleanup - $.off() + stopImmediatePropagation
+         * • Event delegation - Botões .btn-editar, .btn-delete
+         *
+         * 📤 SAÍDAS: DataTable #tblPlacaBronze inicializado com dados dinâmicos
+         *
+         * ⚠️ IMPORTANTE: Não incluir global-toast.js se já estiver no _Layout
+         * 🔄 DOCUMENTAÇÃO: Ver arquivo completo /wwwroot/js/cadastros/placabronze.js (439 linhas)
+         ***/
+    </script>
 
     <script src="~/js/cadastros/placabronze.js" asp-append-version="true"></script>
 }
```

### REMOVER do Janeiro

```html
                                    <th>Ações</th>
```


### ADICIONAR ao Janeiro

```html
                                    <th>Ação</th>
    <script>
        /***
         * ⚡ ARQUIVO: placabronze.js (Externo - Arquivo dedicado)
         * ============================================================================
         * 📥 RESPONSABILIDADES:
         * • Flag anti-reinit: placaBronzeInitialized (evita double binding)
         * • loadList() - Busca placas via AJAX para DataTable
         * • Delete handlers - Validação de dependências antes de excluir
         * • Status toggle - Ativar/inativar placa via AJAX
         * • Event cleanup - $.off() + stopImmediatePropagation
         * • Event delegation - Botões .btn-editar, .btn-delete
         *
         * 📤 SAÍDAS: DataTable #tblPlacaBronze inicializado com dados dinâmicos
         *
         * ⚠️ IMPORTANTE: Não incluir global-toast.js se já estiver no _Layout
         * 🔄 DOCUMENTAÇÃO: Ver arquivo completo /wwwroot/js/cadastros/placabronze.js (439 linhas)
         ***/
    </script>
```
