# wwwroot/js/cadastros/condutorapoio_001.js

**Mudanca:** GRANDE | **+111** linhas | **-120** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/condutorapoio_001.js
+++ ATUAL: wwwroot/js/cadastros/condutorapoio_001.js
@@ -1,225 +1,190 @@
 var dataTable;
 
 $(document).ready(function () {
-    try {
-
+    try
+    {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
-            try {
-
-                var id = $(this).data('id');
+        $(document).on("click", ".btn-delete", function () {
+            try
+            {
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este condutor?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar este condutor?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
+
                 ).then((willDelete) => {
-                    try {
-
+                    try
+                    {
                         if (willDelete) {
-
                             var dataToPost = JSON.stringify({ CondutorId: id });
-
-                            var url = '/api/CondutorApoio/Delete';
-
+                            var url = "/api/CondutorApoio/Delete";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
-
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
+                                    try
+                                    {
                                         if (data.success) {
-
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-
+                                            AppToast.show('Verde', data.message);
                                             dataTable.ajax.reload();
                                         } else {
-
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'condutorapoio_001.js',
-                                            'btn-delete.ajax.success',
+                                            "condutorapoio_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
-
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        Alerta.Erro(
-                                            'Erro',
-                                            'Ocorreu um erro ao excluir o condutor. Tente novamente.',
-                                        );
-                                    } catch (error) {
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'condutorapoio_001.js',
-                                            'btn-delete.ajax.error',
+                                            "condutorapoio_<num>.js",
+                                            "error",
                                             error,
                                         );
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'condutorapoio_001.js',
-                            'btn-delete.confirm.then',
+                            "condutorapoio_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'condutorapoio_001.js',
-                    'btn-delete.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("condutorapoio_<num>.js", "callback@$.on#2", error);
             }
         });
 
-        $(document).on('click', '.updateStatusCondutor', function () {
-            try {
-
-                var url = $(this).data('url');
-
+        $(document).on("click", ".updateStatusCondutor", function () {
+            try
+            {
+                var url = $(this).data("url");
                 var currentElement = $(this);
 
                 $.get(url, function (data) {
-                    try {
+                    try
+                    {
                         if (data.success) {
-
-                            AppToast.show(
-                                'Verde',
-                                'Status alterado com sucesso!',
-                            );
-
-                            var text = 'Ativo';
+                            AppToast.show('Verde', "Status alterado com sucesso!");
+                            var text = "Ativo";
 
                             if (data.type == 1) {
-                                text = 'Inativo';
-
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
-                            } else {
-
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
-                            }
+                                text = "Inativo";
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                            } else
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
 
                             currentElement.text(text);
-                        } else {
-                            Alerta.Erro(
-                                'Erro',
-                                'Não foi possível alterar o status. Tente novamente.',
-                            );
-                        }
-                    } catch (error) {
+                        } else alert("Something went wrong!");
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'condutorapoio_001.js',
-                            'updateStatus.get.callback',
+                            "condutorapoio_<num>.js",
+                            "callback@$.get#1",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'condutorapoio_001.js',
-                    'updateStatus.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("condutorapoio_<num>.js", "callback@$.on#2", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'condutorapoio_001.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("condutorapoio_<num>.js", "callback@$.ready#0", error);
     }
 });
 
 function loadList() {
-    try {
-
-        dataTable = $('#tblCondutor').DataTable({
-
+    try
+    {
+
+        dataTable = $("#tblCondutor").DataTable({
             columnDefs: [
                 {
                     targets: 1,
-                    className: 'text-center',
-                    width: '20%',
+                    className: "text-center",
+                    width: "20%",
                 },
                 {
                     targets: 2,
-                    className: 'text-center',
-                    width: '20%',
+                    className: "text-center",
+                    width: "20%",
                 },
             ],
 
             responsive: true,
-
             ajax: {
-                url: '/api/condutorapoio',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/condutorapoio",
+                type: "GET",
+                datatype: "json",
             },
-
             columns: [
-
-                { data: 'descricao', width: '30%' },
-
-                {
-                    data: 'status',
-
+                { data: "descricao", width: "30%" },
+                {
+                    data: "status",
                     render: function (data, type, row, meta) {
-                        try {
-                            if (data) {
-
+                        try
+                        {
+                            if (data)
                                 return (
                                     '<a href="javascript:void" class="updateStatusCondutor btn btn-verde btn-xs text-white" data-url="/api/CondutorApoio/UpdateStatusCondutor?Id=' +
                                     row.condutorId +
                                     '">Ativo</a>'
                                 );
-                            } else {
-
+                            else
                                 return (
-                                    '<a href="javascript:void" class="updateStatusCondutor btn btn-xs fundo-cinza text-white text-bold" data-url="/api/CondutorApoio/UpdateStatusCondutor?Id=' +
+                                    '<a href="javascript:void" class="updateStatusCondutor btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Combustivel/UpdateStatusCondutor?Id=' +
                                     row.condutorId +
                                     '">Inativo</a>'
                                 );
-                            }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'condutorapoio_001.js',
-                                'loadList.render.status',
+                                "condutorapoio_<num>.js",
+                                "render",
                                 error,
                             );
                         }
                     },
-                    width: '10%',
-                },
-
-                {
-                    data: 'condutorId',
-
+                    width: "10%",
+                },
+                {
+                    data: "condutorId",
                     render: function (data) {
-                        try {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a href="/CondutorApoio/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" style="cursor:pointer; width:75px;">
                                     <i class="far fa-edit"></i> Editar
@@ -227,30 +192,30 @@
                                 <a class="btn-delete btn btn-vinho btn-xs text-white" style="cursor:pointer; width:80px;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i> Excluir
                                 </a>
-                            </div>`;
-                        } catch (error) {
+                    </div>`;
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'condutorapoio_001.js',
-                                'loadList.render.actions',
+                                "condutorapoio_<num>.js",
+                                "render",
                                 error,
                             );
                         }
                     },
-                    width: '20%',
+                    width: "20%",
                 },
             ],
 
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Sem Dados para Exibição',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Sem Dados para Exibição",
             },
-            width: '100%',
+            width: "100%",
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'condutorapoio_001.js',
-            'loadList',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("condutorapoio_<num>.js", "loadList", error);
     }
 }
```
