# wwwroot/js/cadastros/orgaoautuante.js

**Mudanca:** GRANDE | **+93** linhas | **-100** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/orgaoautuante.js
+++ ATUAL: wwwroot/js/cadastros/orgaoautuante.js
@@ -1,127 +1,112 @@
 var dataTable;
 
-$(document).ready(function () {
-    try {
-
+$(document).ready(function ()
+{
+    try
+    {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
-            try {
-
-                var id = $(this).data('id');
+        $(document).on("click", ".btn-delete", function ()
+        {
+            try
+            {
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este órgão?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
-                ).then(function (confirmed) {
-                    try {
-
+                    "Você tem certeza que deseja apagar este órgão?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
+                ).then(function (confirmed)
+                {
+                    try
+                    {
                         if (!confirmed) return;
 
-                        var dataToPost = JSON.stringify({
-                            OrgaoAutuanteId: id,
-                        });
-                        var url = '/api/Multa/DeleteOrgaoAutuante';
+                        var dataToPost = JSON.stringify({ OrgaoAutuanteId: id });
+                        var url = "/api/Multa/DeleteOrgaoAutuante";
 
                         $.ajax({
                             url: url,
-                            type: 'POST',
+                            type: "POST",
                             data: dataToPost,
-                            contentType: 'application/json; charset=utf-8',
-                            dataType: 'json',
-                            success: function (data) {
-                                try {
-
-                                    if (data.success) {
-                                        AppToast.show(
-                                            'Verde',
-                                            data.message,
-                                            2000,
-                                        );
-
+                            contentType: "application/json; charset=utf-8",
+                            dataType: "json",
+                            success: function (data)
+                            {
+                                try
+                                {
+                                    if (data.success)
+                                    {
+                                        AppToast.show('Verde', data.message, 2000);
                                         dataTable.ajax.reload(null, false);
-                                    } else {
-                                        AppToast.show(
-                                            'Vermelho',
-                                            data.message,
-                                            3000,
-                                        );
                                     }
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'orgaoautuante.js',
-                                        'btn-delete.success',
-                                        error,
-                                    );
+                                    else
+                                    {
+                                        AppToast.show('Vermelho', data.message, 3000);
+                                    }
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha("orgaoautuante.js", "btn-delete.success", error);
                                 }
                             },
-                            error: function (err) {
-                                try {
+                            error: function (err)
+                            {
+                                try
+                                {
                                     console.error('Erro ao excluir:', err);
-                                    Alerta.Erro(
-                                        'Erro',
-                                        'Ocorreu um erro ao excluir o órgão',
-                                        'OK',
-                                    );
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'orgaoautuante.js',
-                                        'btn-delete.error',
-                                        error,
-                                    );
+                                    Alerta.Erro('Erro', 'Ocorreu um erro ao excluir o órgão', 'OK');
                                 }
-                            },
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha("orgaoautuante.js", "btn-delete.error", error);
+                                }
+                            }
                         });
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'orgaoautuante.js',
-                            'btn-delete.confirmar',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("orgaoautuante.js", "btn-delete.confirmar", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'orgaoautuante.js',
-                    'btn-delete.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("orgaoautuante.js", "btn-delete.click", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'orgaoautuante.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("orgaoautuante.js", "document.ready", error);
     }
 });
 
-function loadList() {
-    try {
-
-        dataTable = $('#tblOrgaoAutuante').DataTable({
-
+function loadList()
+{
+    try
+    {
+        dataTable = $("#tblOrgaoAutuante").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-left',
-                    width: '15%',
+                    className: "text-left",
+                    width: "15%"
                 },
                 {
                     targets: 1,
-                    className: 'text-left',
-                    width: '70%',
+                    className: "text-left",
+                    width: "70%"
                 },
                 {
                     targets: 2,
-                    className: 'text-center',
-                    width: '15%',
-                    render: function (data, type, full) {
-                        try {
-
+                    className: "text-center",
+                    width: "15%",
+                    render: function (data, type, full)
+                    {
+                        try
+                        {
                             return `<div class="text-center" style="white-space: nowrap;">
                                 <a href="/Multa/UpsertOrgaoAutuante?id=${data}"
                                    class="ftx-btn-icon ftx-btn-editar"
@@ -134,39 +119,37 @@
                                     <i class="fa-duotone fa-trash-can"></i>
                                 </a>
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'orgaoautuante.js',
-                                'render.acoes',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("orgaoautuante.js", "render.acoes", error);
                             return '';
                         }
-                    },
-                },
+                    }
+                }
             ],
 
             responsive: true,
-
             ajax: {
-                url: '/api/Multa/PegaOrgaoAutuante',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/Multa/PegaOrgaoAutuante",
+                type: "GET",
+                datatype: "json"
             },
-
             columns: [
-                { data: 'sigla' },
-                { data: 'nome' },
-                { data: 'orgaoAutuanteId' },
+                { data: "sigla" },
+                { data: "nome" },
+                { data: "orgaoAutuanteId" }
             ],
 
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Nenhum órgão autuante cadastrado',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Nenhum órgão autuante cadastrado"
             },
-            width: '100%',
+            width: "100%"
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('orgaoautuante.js', 'loadList', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("orgaoautuante.js", "loadList", error);
     }
 }
```
