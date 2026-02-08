# wwwroot/js/cadastros/modeloveiculo.js

**Mudanca:** GRANDE | **+139** linhas | **-164** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/modeloveiculo.js
+++ ATUAL: wwwroot/js/cadastros/modeloveiculo.js
@@ -1,212 +1,189 @@
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
-                    'Confirmar Exclusão',
-                    'Você tem certeza que deseja apagar este modelo de veículo? Não será possível recuperar os dados eliminados!',
-                    'Sim, excluir',
-                    'Cancelar',
-                ).then(function (confirmed) {
-                    try {
-                        if (confirmed) {
+                    "Confirmar Exclusão",
+                    "Você tem certeza que deseja apagar este modelo de veículo? Não será possível recuperar os dados eliminados!",
+                    "Sim, excluir",
+                    "Cancelar"
+                ).then(function (confirmed)
+                {
+                    try
+                    {
+                        if (confirmed)
+                        {
                             var dataToPost = JSON.stringify({ ModeloId: id });
-                            var url = '/api/ModeloVeiculo/Delete';
+                            var url = "/api/ModeloVeiculo/Delete";
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
-                                                2000,
-                                            );
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
+                                success: function (data)
+                                {
+                                    try
+                                    {
+                                        if (data.success)
+                                        {
+                                            AppToast.show("Verde", data.message, 2000);
                                             dataTable.ajax.reload();
-                                        } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                                2000,
-                                            );
+                                        } else
+                                        {
+                                            AppToast.show("Vermelho", data.message, 2000);
                                         }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'modeloveiculo.js',
-                                            'ajax.success',
-                                            error,
-                                        );
+                                    } catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("modeloveiculo.js", "ajax.success", error);
                                     }
                                 },
-                                error: function (err) {
-                                    try {
+                                error: function (err)
+                                {
+                                    try
+                                    {
                                         console.log(err);
-                                        Alerta.Erro(
-                                            'Erro',
-                                            'Ocorreu um erro ao tentar excluir o modelo de veículo.',
-                                        );
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'modeloveiculo.js',
-                                            'ajax.error',
-                                            error,
-                                        );
+                                        Alerta.Erro("Erro", "Ocorreu um erro ao tentar excluir o modelo de veículo.");
+                                    } catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("modeloveiculo.js", "ajax.error", error);
                                     }
-                                },
+                                }
                             });
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'modeloveiculo.js',
-                            'confirmar.then',
-                            error,
-                        );
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("modeloveiculo.js", "confirmar.then", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modeloveiculo.js',
-                    'click.btn-delete',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modeloveiculo.js", "click.btn-delete", error);
             }
         });
 
-        $(document).on('click', '.updateStatusModeloVeiculo', function () {
-            try {
-                var url = $(this).data('url');
+        $(document).on("click", ".updateStatusModeloVeiculo", function ()
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
-                                2000,
-                            );
-                            var text = 'Ativo';
-
-                            if (data.type == 1) {
-                                text = 'Inativo';
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
-                            } else {
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                $.get(url, function (data)
+                {
+                    try
+                    {
+                        if (data.success)
+                        {
+                            AppToast.show("Verde", "Status alterado com sucesso!", 2000);
+                            var text = "Ativo";
+
+                            if (data.type == 1)
+                            {
+                                text = "Inativo";
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                            } else
+                            {
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                             }
 
                             currentElement.text(text);
-                        } else {
-                            Alerta.Erro(
-                                'Erro',
-                                'Ocorreu um erro ao alterar o status do modelo.',
-                            );
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'modeloveiculo.js',
-                            'get.success',
-                            error,
-                        );
+                        } else
+                        {
+                            Alerta.Erro("Erro", "Ocorreu um erro ao alterar o status do modelo.");
+                        }
+                    } catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("modeloveiculo.js", "get.success", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'modeloveiculo.js',
-                    'click.updateStatusModeloVeiculo',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha("modeloveiculo.js", "click.updateStatusModeloVeiculo", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'modeloveiculo.js',
-            'document.ready',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modeloveiculo.js", "document.ready", error);
     }
 });
 
-function loadList() {
-    try {
-        dataTable = $('#tblModeloVeiculo').DataTable({
+function loadList()
+{
+    try
+    {
+        dataTable = $("#tblModeloVeiculo").DataTable({
             columnDefs: [
                 {
                     targets: 2,
-                    className: 'text-center',
-                    width: '20%',
+                    className: "text-center",
+                    width: "20%"
                 },
                 {
                     targets: 3,
-                    className: 'text-center',
-                    width: '20%',
-                },
+                    className: "text-center",
+                    width: "20%"
+                }
             ],
             responsive: true,
             ajax: {
-                url: '/api/modeloVeiculo',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/modeloVeiculo",
+                type: "GET",
+                datatype: "json"
             },
             columns: [
-                { data: 'marcaVeiculo.descricaoMarca', width: '30%' },
-                { data: 'descricaoModelo', width: '30%' },
-                {
-                    data: 'status',
-                    render: function (data, type, row, meta) {
-                        try {
-                            if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" ' +
+                { data: "marcaVeiculo.descricaoMarca", width: "30%" },
+                { data: "descricaoModelo", width: "30%" },
+                {
+                    data: "status",
+                    render: function (data, type, row, meta)
+                    {
+                        try
+                        {
+                            if (data)
+                            {
+                                return '<a href="javascript:void(0)" ' +
                                     'class="updateStatusModeloVeiculo btn btn-verde text-white" ' +
-                                    'data-url="/api/ModeloVeiculo/updateStatusModeloVeiculo?Id=' +
-                                    row.modeloId +
-                                    '" ' +
+                                    'data-url="/api/ModeloVeiculo/updateStatusModeloVeiculo?Id=' + row.modeloId + '" ' +
                                     'data-ejtip="Modelo ativo - clique para inativar" ' +
                                     'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 0px !important; display: inline-block !important;">' +
-                                    'Ativo</a>'
-                                );
-                            } else {
-                                return (
-                                    '<a href="javascript:void(0)" ' +
+                                    'Ativo</a>';
+                            }
+                            else
+                            {
+                                return '<a href="javascript:void(0)" ' +
                                     'class="updateStatusModeloVeiculo btn fundo-cinza text-white text-bold" ' +
-                                    'data-url="/api/ModeloVeiculo/updateStatusModeloVeiculo?Id=' +
-                                    row.modeloId +
-                                    '" ' +
+                                    'data-url="/api/ModeloVeiculo/updateStatusModeloVeiculo?Id=' + row.modeloId + '" ' +
                                     'data-ejtip="Modelo inativo - clique para ativar" ' +
                                     'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 0px !important; display: inline-block !important;">' +
-                                    'Inativo</a>'
-                                );
+                                    'Inativo</a>';
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'modeloveiculo.js',
-                                'render.status',
-                                error,
-                            );
-                            return '';
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("modeloveiculo.js", "render.status", error);
+                            return "";
                         }
                     },
-                    className: 'text-center',
-                    width: '10%',
+                    className: "text-center",
+                    width: "10%"
                 },
                 {
-                    data: 'modeloId',
-                    render: function (data) {
-                        try {
+                    data: "modeloId",
+                    render: function (data)
+                    {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a href="/ModeloVeiculo/Upsert?id=${data}" class="btn btn-azul text-white" data-ejtip="Editar modelo de veículo" style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">
                                     <i class="far fa-edit"></i>
@@ -215,24 +192,22 @@
                                     <i class="far fa-trash-alt"></i>
                                 </a>
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'modeloveiculo.js',
-                                'render.acoes',
-                                error,
-                            );
+                        } catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("modeloveiculo.js", "render.acoes", error);
                         }
                     },
-                    width: '20%',
-                },
+                    width: "20%"
+                }
             ],
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Sem Dados para Exibição',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Sem Dados para Exibição"
             },
-            width: '100%',
+            width: "100%"
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('modeloveiculo.js', 'loadList', error);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("modeloveiculo.js", "loadList", error);
     }
 }
```
