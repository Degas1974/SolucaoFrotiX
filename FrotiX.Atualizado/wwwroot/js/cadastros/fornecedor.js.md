# wwwroot/js/cadastros/fornecedor.js

**Mudanca:** GRANDE | **+138** linhas | **-128** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/fornecedor.js
+++ ATUAL: wwwroot/js/cadastros/fornecedor.js
@@ -1,180 +1,193 @@
 var dataTable;
 
-$(document).ready(function () {
-    try {
+$(document).ready(function ()
+{
+    try
+    {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
-            try {
-                var id = $(this).data('id');
+        $(document).on("click", ".btn-delete", function ()
+        {
+            try
+            {
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este fornecedor?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
-                ).then((willDelete) => {
-                    try {
-                        if (willDelete) {
-                            var dataToPost = JSON.stringify({
-                                FornecedorId: id,
-                            });
-                            var url = '/api/Fornecedor/Delete';
+                    "Você tem certeza que deseja apagar este fornecedor?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
+
+                ).then((willDelete) =>
+                {
+                    try
+                    {
+                        if (willDelete)
+                        {
+                            var dataToPost = JSON.stringify({ FornecedorId: id });
+                            var url = "/api/Fornecedor/Delete";
+
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
-                                success: function (data) {
-                                    try {
-                                        if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
+                                success: function (data)
+                                {
+                                    try
+                                    {
+                                        if (data.success)
+                                        {
+                                            AppToast.show('Verde', data.message);
                                             dataTable.ajax.reload();
-                                        } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                        } else
+                                        {
+                                            AppToast.show('Vermelho', data.message);
                                         }
-                                    } catch (error) {
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'fornecedor_<num>.js',
-                                            'success',
+                                            "fornecedor_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
-                                error: function (err) {
-                                    try {
+                                error: function (err)
+                                {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'fornecedor_<num>.js',
-                                            'error',
+                                            "fornecedor_<num>.js",
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
-                            'fornecedor_<num>.js',
-                            'callback@swal.then#0',
+                            "fornecedor_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'fornecedor_<num>.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "callback@$.on#2", error);
             }
         });
 
-        $(document).on('click', '.updateStatusFornecedor', function () {
-            try {
-                var url = $(this).data('url');
+        $(document).on("click", ".updateStatusFornecedor", function ()
+        {
+            try
+            {
+                var url = $(this).data("url");
                 var currentElement = $(this);
 
-                $.get(url, function (data) {
-                    try {
-                        if (data.success) {
-                            AppToast.show(
-                                'Verde',
-                                'Status alterado com sucesso!',
-                            );
-                            var text = 'Ativo';
-
-                            if (data.type == 1) {
-                                text = 'Inativo';
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
+                $.get(url, function (data)
+                {
+                    try
+                    {
+                        if (data.success)
+                        {
+                            AppToast.show('Verde', "Status alterado com sucesso!");
+                            var text = "Ativo";
+
+                            if (data.type == 1)
+                            {
+                                text = "Inativo";
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                             } else
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
 
                             currentElement.text(text);
-                        } else alert('Something went wrong!');
-                    } catch (error) {
+                        } else alert("Something went wrong!");
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'fornecedor_<num>.js',
-                            'callback@$.get#1',
+                            "fornecedor_<num>.js",
+                            "callback@$.get#1",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'fornecedor_<num>.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "callback@$.on#2", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'fornecedor_<num>.js',
-            'callback@$.ready#0',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "callback@$.ready#0", error);
     }
 });
 
-function loadList() {
-    try {
-        dataTable = $('#tblFornecedor').DataTable({
+function loadList()
+{
+    try
+    {
+
+        dataTable = $("#tblFornecedor").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-left',
+                    className: "text-left",
                 },
                 {
                     targets: 1,
-                    className: 'text-left',
+                    className: "text-left",
                 },
                 {
                     targets: 2,
-                    className: 'text-left',
+                    className: "text-left",
                 },
                 {
                     targets: 3,
-                    className: 'text-center',
+                    className: "text-center",
                 },
                 {
                     targets: 4,
-                    className: 'text-center',
+                    className: "text-center",
                 },
                 {
                     targets: 5,
-                    className: 'text-center',
+                    className: "text-center",
                 },
             ],
 
             responsive: true,
             ajax: {
-                url: '/api/fornecedor',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/fornecedor",
+                type: "GET",
+                datatype: "json",
             },
             columns: [
-                { data: 'cnpj', width: '10%' },
-                { data: 'descricaoFornecedor', width: '25%' },
-                { data: 'contato01', width: '20%' },
-                { data: 'telefone01', width: '8%' },
-                {
-                    data: 'status',
-                    render: function (data, type, row, meta) {
-                        try {
+                { data: "cnpj", width: "10%" },
+                { data: "descricaoFornecedor", width: "25%" },
+                { data: "contato01", width: "20%" },
+                { data: "telefone01", width: "8%" },
+                {
+                    data: "status",
+                    render: function (data, type, row, meta)
+                    {
+                        try
+                        {
                             if (data)
                                 return (
                                     '<a href="javascript:void" class="updateStatusFornecedor btn btn-verde btn-xs text-white" data-url="/api/Fornecedor/updateStatusFornecedor?Id=' +
@@ -187,20 +200,20 @@
                                     row.fornecedorId +
                                     '">Inativo</a>'
                                 );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'fornecedor_<num>.js',
-                                'render',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "render", error);
                         }
                     },
-                    width: '6%',
-                },
-                {
-                    data: 'fornecedorId',
-                    render: function (data) {
-                        try {
+                    width: "6%",
+                },
+                {
+                    data: "fornecedorId",
+                    render: function (data)
+                    {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a href="/Fornecedor/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" aria-label="Editar o Fornecedor!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
                                     <i class="far fa-edit"></i>
@@ -209,25 +222,25 @@
                                     <i class="far fa-trash-alt"></i>
                                 </a>
                     </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'fornecedor_<num>.js',
-                                'render',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "render", error);
                         }
                     },
-                    width: '8%',
+                    width: "8%",
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
-        Alerta.TratamentoErroComLinha('fornecedor_<num>.js', 'loadList', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("fornecedor_<num>.js", "loadList", error);
     }
 }
```
