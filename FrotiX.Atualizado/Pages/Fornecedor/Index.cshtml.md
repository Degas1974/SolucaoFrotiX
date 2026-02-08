# Pages/Fornecedor/Index.cshtml

**Mudanca:** MEDIA | **+19** linhas | **-5** linhas

---

```diff
--- JANEIRO: Pages/Fornecedor/Index.cshtml
+++ ATUAL: Pages/Fornecedor/Index.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Models.Fornecedor
 
 @{
@@ -45,7 +44,7 @@
                                     <th>Contato</th>
                                     <th>Telefone</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody>
@@ -59,10 +58,25 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
+
+    <script>
+        /***
+         * ⚡ ARQUIVO: fornecedor.js (Externo - Arquivo dedicado)
+         * ============================================================================
+         * 📥 RESPONSABILIDADES:
+         * • loadList() - Carrega fornecedores via AJAX para DataTable
+         * • Delete handlers - Validação de dependências antes de excluir
+         * • Status toggle - Ativar/inativar fornecedor via AJAX
+         * • Event delegation - Botões .btn-editar, .btn-delete
+         *
+         * 📤 EVENTOS DISPARADOS:
+         * • DataTable #tblFornecedor inicializado com dados dinâmicos
+         *
+         * 🔗 DOCUMENTAÇÃO COMPLETA: Ver arquivo /wwwroot/js/cadastros/fornecedor.js (152 linhas)
+         ***/
+    </script>
 
     <script src="~/js/cadastros/fornecedor.js" asp-append-version="true"></script>
 }
```

### REMOVER do Janeiro

```html
                                    <th>Ações</th>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
        crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
        crossorigin="anonymous"></script>
```


### ADICIONAR ao Janeiro

```html
                                    <th>Ação</th>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
    <script>
        /***
         * ⚡ ARQUIVO: fornecedor.js (Externo - Arquivo dedicado)
         * ============================================================================
         * 📥 RESPONSABILIDADES:
         * • loadList() - Carrega fornecedores via AJAX para DataTable
         * • Delete handlers - Validação de dependências antes de excluir
         * • Status toggle - Ativar/inativar fornecedor via AJAX
         * • Event delegation - Botões .btn-editar, .btn-delete
         *
         * 📤 EVENTOS DISPARADOS:
         * • DataTable #tblFornecedor inicializado com dados dinâmicos
         *
         * 🔗 DOCUMENTAÇÃO COMPLETA: Ver arquivo /wwwroot/js/cadastros/fornecedor.js (152 linhas)
         ***/
    </script>
```
