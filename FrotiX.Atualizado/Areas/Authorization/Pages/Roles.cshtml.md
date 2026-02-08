# Areas/Authorization/Pages/Roles.cshtml

**Mudanca:** GRANDE | **+79** linhas | **-33** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Roles.cshtml
+++ ATUAL: Areas/Authorization/Pages/Roles.cshtml
@@ -3,28 +3,15 @@
 @{
     ViewData["Title"] = "Role";
     ViewData["PageName"] = "authorization_roles";
-    ViewData["Heading"] = "<i class='fa-duotone fa-shield-halved'></i> Authorization: <span class='fw-300'>Roles</span>";
+    ViewData["Heading"] = "<i class='fal fa-shield-alt'></i> Authorization: <span class='fw-300'>Roles</span>";
     ViewData["Category1"] = "Authorization";
-    ViewData["PageIcon"] = "fa-shield-halved";
+    ViewData["PageIcon"] = "fa-shield-alt";
 }
 
 @section HeadBlock {
     <link rel="stylesheet" media="screen, print" href="~/css/datagrid/datatables/datatables.bundle.css">
     <link rel="stylesheet" media="screen, print" href="~/css/formplugins/select2/select2.bundle.css">
-    <style>
-        :root {
-            --fa-primary-color: #ff6b35;
-            --fa-secondary-color: #6c757d;
-        }
-    </style>
 }
-
-<div id="loading-overlay" class="ftx-spin-overlay" style="display:none;">
-    <div class="ftx-spin-box">
-        <img src="/images/logo_gota_frotix_transparente.png" class="ftx-loading-logo" />
-        <div class="ftx-loading-text">Processando...</div>
-    </div>
-</div>
 
 <div class="row">
     <div class="col-xl-12">
@@ -42,26 +29,88 @@
     <script src="~/js/datagrid/datatables/datatables.bundle.js"></script>
 
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: Inicialização DataTable de Roles (Perfis de Acesso)
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Gerenciar perfis de acesso (Roles) do sistema com CRUD completo
+         * via DataTable editável (adicionar, editar, deletar roles).
+         * 📥 ENTRADAS : Nenhuma (função auto-executável no document.ready).
+         * 📤 SAÍDAS : DataTable renderizada na div #dt-list com operações CRUD.
+         * 🔗 CHAMADA POR : Evento de UI (document.ready do jQuery).
+         * 🔄 CHAMA : DataTableEdit() (plugin customizado), $.ajax() para API /api/roles.
+         * 📦 DEPENDÊNCIAS : jQuery, DataTables, datatables.bundle.js, plugin DataTableEdit.
+         ****************************************************************************************/
         $(function () {
-            const endpoint = "/api/roles";
-            $('#dt-list').DataTableEdit({
-                ajax: endpoint,
-                columns: [
-                    { title: "Id", data: "id", type: "readonly", visible: false, searchable: false },
-                    { title: "ConcurrencyStamp", data: "concurrencyStamp", type: "readonly", visible: false, searchable: false },
-                    { title: "Name", data: "name", type: "text" },
-                    { title: "Normalized", data: "normalizedName", type: "readonly" }
-                ],
-                onAddRow: function (table, rowdata, success, error) {
-                    $.ajax({ url: endpoint, type: 'POST', data: rowdata, success: success, error: error });
-                },
-                onDeleteRow: function (table, rowdata, success, error) {
-                    $.ajax({ url: endpoint, type: 'DELETE', data: rowdata, success: success, error: error });
-                },
-                onEditRow: function (table, rowdata, success, error) {
-                    $.ajax({ url: endpoint, type: 'PUT', data: rowdata, success: success, error: error });
-                }
-            });
+            try {
+                const endpoint = "/api/roles";
+
+                $('#dt-list').DataTableEdit({
+                    ajax: endpoint,
+                    columns: [
+                        { title: "Id", data: "id", type: "readonly", visible: false, searchable: false },
+                        { title: "ConcurrencyStamp", data: "concurrencyStamp", type: "readonly", visible: false, searchable: false },
+                        { title: "Name", data: "name", type: "text" },
+                        { title: "Normalized", data: "normalizedName", type: "readonly" }
+                    ],
+
+                    onAddRow: function (table, rowdata, success, error) {
+                        try {
+                            $.ajax({
+                                url: endpoint,
+                                type: 'POST',
+                                data: rowdata,
+                                success: success,
+                                error: function(xhr, status, errorMsg) {
+                                    alerta.erro('Erro ao adicionar role: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar adição de role: ' + ex.message);
+                            console.error('Erro onAddRow:', ex);
+                        }
+                    },
+
+                    onDeleteRow: function (table, rowdata, success, error) {
+                        try {
+                            $.ajax({
+                                url: endpoint,
+                                type: 'DELETE',
+                                data: rowdata,
+                                success: success,
+                                error: function(xhr, status, errorMsg) {
+                                    alerta.erro('Erro ao deletar role: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar exclusão de role: ' + ex.message);
+                            console.error('Erro onDeleteRow:', ex);
+                        }
+                    },
+
+                    onEditRow: function (table, rowdata, success, error) {
+                        try {
+                            $.ajax({
+                                url: endpoint,
+                                type: 'PUT',
+                                data: rowdata,
+                                success: success,
+                                error: function(xhr, status, errorMsg) {
+                                    alerta.erro('Erro ao editar role: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar edição de role: ' + ex.message);
+                            console.error('Erro onEditRow:', ex);
+                        }
+                    }
+                });
+            } catch (erro) {
+                alerta.erro('Erro ao inicializar DataTable de Roles: ' + erro.message);
+                console.error('Erro na inicialização:', erro);
+            }
         });
     </script>
 }
```
