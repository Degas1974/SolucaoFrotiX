# Areas/Authorization/Pages/Users.cshtml

**Mudanca:** GRANDE | **+96** linhas | **-37** linhas

---

```diff
--- JANEIRO: Areas/Authorization/Pages/Users.cshtml
+++ ATUAL: Areas/Authorization/Pages/Users.cshtml
@@ -29,48 +29,113 @@
     <script src="~/js/datagrid/datatables/datatables.bundle.js"></script>
 
     <script>
+        /****************************************************************************************
+         * ⚡ FUNÇÃO: Inicialização DataTable de Users (Usuários do Sistema)
+         * --------------------------------------------------------------------------------------
+         * 🎯 OBJETIVO : Gerenciar usuários do sistema (ASP.NET Identity) com CRUD completo
+         * via DataTable editável, incluindo controle de email confirmado,
+         * telefone e status de bloqueio (LockOut).
+         * 📥 ENTRADAS : Nenhuma (função auto-executável no document.ready).
+         * 📤 SAÍDAS : DataTable renderizada na div #dt-list com operações CRUD de usuários.
+         * 🔗 CHAMADA POR : Evento de UI (document.ready do jQuery).
+         * 🔄 CHAMA : DataTableEdit() (plugin customizado), $.ajax() para API /api/users.
+         * 📦 DEPENDÊNCIAS : jQuery, DataTables, datatables.bundle.js, plugin DataTableEdit.
+         ****************************************************************************************/
         $(function () {
-            const endpoint = "/api/users";
-            const trueFalseOptions = ["false", "true"];
-            $('#dt-list').DataTableEdit({
-                ajax: endpoint,
-                columns: [
-                    { title: "Id", data: "id", type: "readonly", visible: false, searchable: false },
-                    { title: "ConcurrencyStamp", data: "concurrencyStamp", type: "readonly", visible: false, searchable: false },
-                    { title: "Nome do Usuário", data: "userName", type: "readonly", searchable: true },
-                    { title: "Email", data: "email", searchable: true },
-                    {
-                        title: "Confirmed",
-                        data: "emailConfirmed",
-                        className: "text-center col-1",
-                        type: "select",
-                        options: trueFalseOptions,
-                        render: function (data) {
-                            return `<input type="checkbox" name="lockoutEnabled" disabled ${data ? "checked" : ""}>`;
+            try {
+                const endpoint = "/api/users";
+                const trueFalseOptions = ["false", "true"];
+
+                $('#dt-list').DataTableEdit({
+                    ajax: endpoint,
+                    columns: [
+                        { title: "Id", data: "id", type: "readonly", visible: false, searchable: false },
+                        { title: "ConcurrencyStamp", data: "concurrencyStamp", type: "readonly", visible: false, searchable: false },
+                        { title: "Nome do Usuário", data: "userName", type: "readonly", searchable: true },
+                        { title: "Email", data: "email", searchable: true },
+                        {
+                            title: "Confirmed",
+                            data: "emailConfirmed",
+                            className: "text-center col-1",
+                            type: "select",
+                            options: trueFalseOptions,
+
+                            render: function (data) {
+                                return `<input type="checkbox" name="emailConfirmed" disabled ${data ? "checked" : ""}>`;
+                            }
+                        },
+                        { title: "PhoneNumber", data: "phoneNumber", searchable: true },
+                        {
+                            title: "LockOut",
+                            data: "lockoutEnabled",
+                            className: "text-center col-1",
+                            type: "select",
+                            options: trueFalseOptions,
+
+                            render: function (data) {
+                                return `<input type="checkbox" name="lockoutEnabled" disabled ${data ? "checked" : ""}>`;
+                            }
+                        }
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
+                                    alerta.erro('Erro ao adicionar usuário: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar adição de usuário: ' + ex.message);
+                            console.error('Erro onAddRow:', ex);
                         }
                     },
-                    { title: "PhoneNumber", data: "phoneNumber", searchable: true },
-                    {
-                        title: "LockOut",
-                        data: "lockoutEnabled",
-                        className: "text-center col-1",
-                        type: "select",
-                        options: trueFalseOptions,
-                        render: function (data) {
-                            return `<input type="checkbox" name="lockoutEnabled" disabled ${data ? "checked" : ""}>`;
+
+                    onDeleteRow: function (table, rowdata, success, error) {
+                        try {
+                            $.ajax({
+                                url: endpoint,
+                                type: 'DELETE',
+                                data: rowdata,
+                                success: success,
+                                error: function(xhr, status, errorMsg) {
+                                    alerta.erro('Erro ao deletar usuário: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar exclusão de usuário: ' + ex.message);
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
+                                    alerta.erro('Erro ao editar usuário: ' + (xhr.responseJSON?.message || errorMsg));
+                                    if (error) error(xhr, status, errorMsg);
+                                }
+                            });
+                        } catch (ex) {
+                            alerta.erro('Erro ao processar edição de usuário: ' + ex.message);
+                            console.error('Erro onEditRow:', ex);
                         }
                     }
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
+                });
+            } catch (erro) {
+                alerta.erro('Erro ao inicializar DataTable de Usuários: ' + erro.message);
+                console.error('Erro na inicialização:', erro);
+            }
         });
     </script>
 }
```
