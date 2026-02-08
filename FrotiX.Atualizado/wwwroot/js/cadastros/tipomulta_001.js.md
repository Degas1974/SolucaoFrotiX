# wwwroot/js/cadastros/tipomulta_001.js

**Mudanca:** GRANDE | **+84** linhas | **-86** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/tipomulta_001.js
+++ ATUAL: wwwroot/js/cadastros/tipomulta_001.js
@@ -1,188 +1,166 @@
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
-                    'Você tem certeza que deseja apagar este tipo de multa?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar este tipo de multa?",
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
-                            var dataToPost = JSON.stringify({
-                                TipoMultaId: id,
-                            });
-
-                            var url = '/api/Multa/DeleteTipoMulta';
-
+                            var dataToPost = JSON.stringify({ TipoMultaId: id });
+                            var url = "/api/Multa/DeleteTipoMulta";
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
-                                            'tipomulta_001.js',
-                                            'btn-delete.ajax.success',
+                                            "tipomulta_<num>.js",
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
-                                            'Ocorreu um erro ao excluir o tipo de multa. Tente novamente.',
-                                        );
-                                    } catch (error) {
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'tipomulta_001.js',
-                                            'btn-delete.ajax.error',
+                                            "tipomulta_<num>.js",
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
-                            'tipomulta_001.js',
-                            'btn-delete.confirm.then',
+                            "tipomulta_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'tipomulta_001.js',
-                    'btn-delete.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("tipomulta_<num>.js", "callback@$.on#2", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'tipomulta_001.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("tipomulta_<num>.js", "callback@$.ready#0", error);
     }
 });
 
 function loadList() {
-    try {
-
-        dataTable = $('#tblTipoMulta').DataTable({
-
+    try
+    {
+        dataTable = $("#tblTipoMulta").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-left',
-                    width: '20%',
+                    className: "text-left",
+                    width: "20%",
                 },
                 {
                     targets: 1,
-                    className: 'text-left',
-                    width: '20%',
+                    className: "text-left",
+                    width: "20%",
                 },
                 {
                     targets: 2,
-                    className: 'text-left',
-                    width: '64%',
+                    className: "text-left",
+                    width: "64%",
                 },
                 {
                     targets: 3,
-                    className: 'text-center',
-                    width: '8%',
+                    className: "text-center",
+                    width: "8%",
                 },
                 {
                     targets: 4,
-                    className: 'text-center',
-                    width: '8%',
+                    className: "text-center",
+                    width: "8%",
                 },
             ],
 
             responsive: true,
-
             ajax: {
-                url: '/api/Multa/PegaTipoMulta',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/Multa/PegaTipoMulta",
+                type: "GET",
+                datatype: "json",
             },
-
             columns: [
-
-                { data: 'artigo' },
-
-                { data: 'denatran' },
-
-                { data: 'descricao' },
-
-                { data: 'infracao' },
-
+                { data: "artigo" },
+                { data: "denatran" },
+                { data: "descricao" },
+                { data: "infracao" },
                 {
-                    data: 'tipoMultaId',
-
+                    data: "tipoMultaId",
                     render: function (data) {
-                        try {
+                        try
+                        {
                             return `<div class="text-center">
-                                <a href="/Multa/UpsertTipoMulta?id=${data}" class="btn btn-azul btn-xs text-white" style="cursor:pointer;">
+                                <a href="/Multa/UpsertTipoMulta?id=${data}" class="btn btn-azul btn-xs text-white" style="cursor:pointer; ">
                                     <i class="far fa-edit"></i>
                                 </a>
-                                <a class="btn-delete btn btn-vinho btn-xs text-white" style="cursor:pointer;" data-id='${data}'>
+                                <a class="btn-delete btn btn-vinho btn-xs text-white" style="cursor:pointer; " data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>
-                            </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'tipomulta_001.js',
-                                'loadList.render.actions',
-                                error,
-                            );
+                    </div>`;
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("tipomulta_<num>.js", "render", error);
                         }
                     },
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
-        Alerta.TratamentoErroComLinha('tipomulta_001.js', 'loadList', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("tipomulta_<num>.js", "loadList", error);
     }
 }
```
