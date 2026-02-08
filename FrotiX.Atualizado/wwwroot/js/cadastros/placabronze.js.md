# wwwroot/js/cadastros/placabronze.js

**Mudanca:** GRANDE | **+286** linhas | **-310** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/placabronze.js
+++ ATUAL: wwwroot/js/cadastros/placabronze.js
@@ -1,10 +1,13 @@
 var dataTable;
 var placaBronzeInitialized = false;
 
-$(document).ready(function () {
-    try {
-
-        if (placaBronzeInitialized) {
+$(document).ready(function ()
+{
+    try
+    {
+
+        if (placaBronzeInitialized)
+        {
             console.warn('placabronze.js já foi inicializado - ignorando');
             return;
         }
@@ -14,401 +17,374 @@
 
         loadList();
 
-        $(document).off('click', '.btn-delete');
-        $(document).off('click', '.btn-desvincular');
-        $(document).off('click', '.updateStatusPlacaBronze');
-
-        $(document).on('click', '.btn-delete', function (e) {
+        $(document).off("click", ".btn-delete");
+        $(document).off("click", ".btn-desvincular");
+        $(document).off("click", ".updateStatusPlacaBronze");
+
+        $(document).on("click", ".btn-delete", function (e)
+        {
             e.preventDefault();
             e.stopImmediatePropagation();
 
-            try {
-                var id = $(this).data('id');
+            try
+            {
+                var id = $(this).data("id");
                 console.log('Delete clicado - ID:', id);
 
                 Alerta.Confirmar(
-                    'Confirmar Exclusão',
-                    'Você tem certeza que deseja apagar esta placa? Não será possível recuperar os dados eliminados!',
-                    'Sim, excluir',
-                    'Cancelar',
-                ).then((willDelete) => {
-                    try {
-                        if (willDelete) {
-                            var dataToPost = JSON.stringify({
-                                PlacaBronzeId: id,
-                            });
-                            var url = '/api/PlacaBronze/Delete';
+                    "Confirmar Exclusão",
+                    "Você tem certeza que deseja apagar esta placa? Não será possível recuperar os dados eliminados!",
+                    "Sim, excluir",
+                    "Cancelar"
+                ).then((willDelete) =>
+                {
+                    try
+                    {
+                        if (willDelete)
+                        {
+                            var dataToPost = JSON.stringify({ PlacaBronzeId: id });
+                            var url = "/api/PlacaBronze/Delete";
 
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
-                                success: function (data) {
-                                    try {
-                                        console.log(
-                                            'Resposta do servidor:',
-                                            data,
-                                        );
-
-                                        if (data.success) {
-                                            if (
-                                                typeof AppToast !== 'undefined'
-                                            ) {
-                                                AppToast.show(
-                                                    'Verde',
-                                                    data.message,
-                                                    2000,
-                                                );
-                                            } else {
-                                                console.warn(
-                                                    'AppToast não disponível',
-                                                );
-                                                alert(data.message);
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
+                                success: function (data)
+                                {
+                                    try
+                                    {
+                                        console.log('Resposta do servidor:', data);
+
+                                        if (data.success)
+                                        {
+                                            if (typeof AppToast !== 'undefined')
+                                            {
+                                                AppToast.show("Verde", data.message, 2000);
+                                            }
+                                            else
+                                            {
+                                                console.error('[placabronze.js] Sucesso -', data.message);
                                             }
                                             dataTable.ajax.reload();
-                                        } else {
-                                            if (
-                                                typeof AppToast !== 'undefined'
-                                            ) {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    2000,
-                                                );
-                                            } else {
-                                                alert(data.message);
-                                            }
-                                        }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'placabronze.js',
-                                            'ajax.Delete.success',
-                                            error,
-                                        );
+                                        }
+                                        else
+                                        {
+                                            if (typeof AppToast !== 'undefined')
+                                            {
+                                                AppToast.show("Vermelho", data.message, 2000);
+                                            }
+                                            else
+                                            {
+                                                console.error('[placabronze.js] Delete error -', data.message);
+                                            }
+                                        }
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("placabronze.js", "ajax.Delete.success", error);
                                     }
                                 },
-                                error: function (err) {
-                                    try {
-                                        console.error(
-                                            'Erro na requisição:',
-                                            err,
-                                        );
-                                        if (typeof AppToast !== 'undefined') {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                'Erro ao excluir a placa de bronze. Tente novamente.',
-                                                2000,
-                                            );
-                                        } else {
-                                            alert(
-                                                'Erro ao excluir a placa de bronze. Tente novamente.',
-                                            );
-                                        }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'placabronze.js',
-                                            'ajax.Delete.error',
-                                            error,
-                                        );
+                                error: function (err)
+                                {
+                                    try
+                                    {
+                                        console.error('Erro na requisição:', err);
+                                        if (typeof AppToast !== 'undefined')
+                                        {
+                                            AppToast.show("Vermelho", "Erro ao excluir a placa de bronze. Tente novamente.", 2000);
+                                        }
+                                        else
+                                        {
+                                            console.error('[placabronze.js] Erro ao excluir a placa de bronze.');
+                                        }
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("placabronze.js", "ajax.Delete.error", error);
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'placabronze.js',
-                            'btn-delete.Confirmar.then',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("placabronze.js", "btn-delete.Confirmar.then", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'placabronze.js',
-                    'btn-delete.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("placabronze.js", "btn-delete.click", error);
             }
         });
 
-        $(document).on('click', '.btn-desvincular', function (e) {
+        $(document).on("click", ".btn-desvincular", function (e)
+        {
             e.preventDefault();
             e.stopImmediatePropagation();
 
-            try {
-                var id = $(this).data('id');
+            try
+            {
+                var id = $(this).data("id");
                 console.log('Desvincular clicado - ID:', id);
 
                 Alerta.Confirmar(
-                    'Confirmar Desvinculação',
-                    'Você tem certeza que deseja desvincular esse veículo? Você precisará reassociá-lo se for o caso!',
-                    'Sim, desvincular',
-                    'Cancelar',
-                ).then((willDelete) => {
-                    try {
-                        if (willDelete) {
-                            var dataToPost = JSON.stringify({
-                                PlacaBronzeId: id,
-                            });
-                            var url = '/api/PlacaBronze/Desvincula';
+                    "Confirmar Desvinculação",
+                    "Você tem certeza que deseja desvincular esse veículo? Você precisará reassociá-lo se for o caso!",
+                    "Sim, desvincular",
+                    "Cancelar"
+                ).then((willDelete) =>
+                {
+                    try
+                    {
+                        if (willDelete)
+                        {
+                            var dataToPost = JSON.stringify({ PlacaBronzeId: id });
+                            var url = "/api/PlacaBronze/Desvincula";
 
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
-                                            if (
-                                                typeof AppToast !== 'undefined'
-                                            ) {
-                                                AppToast.show(
-                                                    'Verde',
-                                                    data.message,
-                                                    2000,
-                                                );
-                                            } else {
-                                                alert(data.message);
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
+                                success: function (data)
+                                {
+                                    try
+                                    {
+                                        if (data.success)
+                                        {
+                                            if (typeof AppToast !== 'undefined')
+                                            {
+                                                AppToast.show("Verde", data.message, 2000);
+                                            }
+                                            else
+                                            {
+                                                console.error('[placabronze.js] Sucesso -', data.message);
                                             }
                                             dataTable.ajax.reload();
-                                        } else {
-                                            if (
-                                                typeof AppToast !== 'undefined'
-                                            ) {
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    2000,
-                                                );
-                                            } else {
-                                                alert(data.message);
-                                            }
-                                        }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'placabronze.js',
-                                            'ajax.Desvincula.success',
-                                            error,
-                                        );
+                                        }
+                                        else
+                                        {
+                                            if (typeof AppToast !== 'undefined')
+                                            {
+                                                AppToast.show("Vermelho", data.message, 2000);
+                                            }
+                                            else
+                                            {
+                                                console.error('[placabronze.js] Desvincula error -', data.message);
+                                            }
+                                        }
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("placabronze.js", "ajax.Desvincula.success", error);
                                     }
                                 },
-                                error: function (err) {
-                                    try {
+                                error: function (err)
+                                {
+                                    try
+                                    {
                                         console.error(err);
-                                        if (typeof AppToast !== 'undefined') {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                'Erro ao desvincular o veículo. Tente novamente.',
-                                                2000,
-                                            );
-                                        } else {
-                                            alert(
-                                                'Erro ao desvincular o veículo. Tente novamente.',
-                                            );
-                                        }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'placabronze.js',
-                                            'ajax.Desvincula.error',
-                                            error,
-                                        );
+                                        if (typeof AppToast !== 'undefined')
+                                        {
+                                            AppToast.show("Vermelho", "Erro ao desvincular o veículo. Tente novamente.", 2000);
+                                        }
+                                        else
+                                        {
+                                            console.error('[placabronze.js] Erro ao desvincular o veículo.');
+                                        }
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("placabronze.js", "ajax.Desvincula.error", error);
                                     }
                                 },
                             });
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'placabronze.js',
-                            'btn-desvincular.Confirmar.then',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("placabronze.js", "btn-desvincular.Confirmar.then", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'placabronze.js',
-                    'btn-desvincular.click',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("placabronze.js", "btn-desvincular.click", error);
             }
         });
 
-        $(document).on('click', '.updateStatusPlacaBronze', function (e) {
+        $(document).on("click", ".updateStatusPlacaBronze", function (e)
+        {
             e.preventDefault();
             e.stopImmediatePropagation();
 
-            try {
-                var url = $(this).data('url');
+            try
+            {
+                var url = $(this).data("url");
                 var currentElement = $(this);
 
-                $.get(url, function (data) {
-                    try {
-                        if (data.success) {
-                            if (typeof AppToast !== 'undefined') {
-                                AppToast.show('Verde', data.message, 2000);
-                            }
-
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
+                            if (typeof AppToast !== 'undefined')
+                            {
+                                AppToast.show("Verde", data.message, 2000);
+                            }
+
+                            var text = "Ativo";
+
+                            if (data.type == 1)
+                            {
+                                text = "Inativo";
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                            }
+                            else
+                            {
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                             }
 
                             currentElement.text(text);
-                        } else {
-                            if (typeof AppToast !== 'undefined') {
-                                AppToast.show(
-                                    'Vermelho',
-                                    'Erro ao alterar o status. Tente novamente.',
-                                    2000,
-                                );
-                            } else {
-                                alert(
-                                    'Erro ao alterar o status. Tente novamente.',
-                                );
-                            }
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'placabronze.js',
-                            'updateStatusPlacaBronze.get.callback',
-                            error,
-                        );
-                    }
-                }).fail(function (jqXHR) {
-                    try {
-                        console.error(jqXHR);
-                        if (typeof AppToast !== 'undefined') {
-                            AppToast.show(
-                                'Vermelho',
-                                'Ocorreu um erro ao alterar o status da placa',
-                                2000,
-                            );
-                        } else {
-                            alert(
-                                'Ocorreu um erro ao alterar o status da placa',
-                            );
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'placabronze.js',
-                            'updateStatusPlacaBronze.get.fail',
-                            error,
-                        );
-                    }
-                });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'placabronze.js',
-                    'updateStatusPlacaBronze.click',
-                    error,
-                );
+                        }
+                        else
+                        {
+                            if (typeof AppToast !== 'undefined')
+                            {
+                                AppToast.show("Vermelho", "Erro ao alterar o status. Tente novamente.", 2000);
+                            }
+                            else
+                            {
+                                console.error('[placabronze.js] Erro ao alterar o status.');
+                            }
+                        }
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("placabronze.js", "updateStatusPlacaBronze.get.callback", error);
+                    }
+                })
+                    .fail(function (jqXHR)
+                    {
+                        try
+                        {
+                            console.error(jqXHR);
+                            if (typeof AppToast !== 'undefined')
+                            {
+                                AppToast.show("Vermelho", "Ocorreu um erro ao alterar o status da placa", 2000);
+                            }
+                            else
+                            {
+                                console.error('[placabronze.js] Ocorreu um erro ao alterar o status da placa');
+                            }
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("placabronze.js", "updateStatusPlacaBronze.get.fail", error);
+                        }
+                    });
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("placabronze.js", "updateStatusPlacaBronze.click", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'placabronze.js',
-            'document.ready',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("placabronze.js", "document.ready", error);
     }
 });
 
-function loadList() {
-    try {
-
-        if ($.fn.DataTable.isDataTable('#tblPlacaBronze')) {
+function loadList()
+{
+    try
+    {
+
+        if ($.fn.DataTable.isDataTable('#tblPlacaBronze'))
+        {
             console.log('Destruindo DataTable anterior');
             $('#tblPlacaBronze').DataTable().destroy();
         }
 
         console.log('Inicializando DataTable');
 
-        dataTable = $('#tblPlacaBronze').DataTable({
+        dataTable = $("#tblPlacaBronze").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-left',
-                    width: '40%',
+                    className: "text-left",
+                    width: "40%",
                 },
                 {
                     targets: 1,
-                    className: 'text-center',
-                    width: '15%',
+                    className: "text-center",
+                    width: "15%",
                 },
                 {
                     targets: 2,
-                    className: 'text-center',
-                    width: '10%',
+                    className: "text-center",
+                    width: "10%",
                 },
                 {
                     targets: 3,
-                    className: 'text-center',
-                    width: '15%',
+                    className: "text-center",
+                    width: "15%",
                 },
             ],
 
             responsive: true,
             ajax: {
-                url: '/api/placaBronze',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/placaBronze",
+                type: "GET",
+                datatype: "json",
             },
             columns: [
-                { data: 'descricaoPlaca' },
-                { data: 'placaVeiculo' },
-                {
-                    data: 'status',
-                    render: function (data, type, row, meta) {
-                        try {
-                            if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" ' +
+                { data: "descricaoPlaca" },
+                { data: "placaVeiculo" },
+                {
+                    data: "status",
+                    render: function (data, type, row, meta)
+                    {
+                        try
+                        {
+                            if (data)
+                            {
+                                return '<a href="javascript:void(0)" ' +
                                     'class="updateStatusPlacaBronze btn btn-verde text-white" ' +
-                                    'data-url="/api/PlacaBronze/updateStatusPlacaBronze?Id=' +
-                                    row.placaBronzeId +
-                                    '" ' +
+                                    'data-url="/api/PlacaBronze/updateStatusPlacaBronze?Id=' + row.placaBronzeId + '" ' +
                                     'data-ejtip="Placa ativa - clique para inativar" ' +
                                     'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">' +
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
                                     'class="updateStatusPlacaBronze btn fundo-cinza text-white text-bold" ' +
-                                    'data-url="/api/PlacaBronze/updateStatusPlacaBronze?Id=' +
-                                    row.placaBronzeId +
-                                    '" ' +
+                                    'data-url="/api/PlacaBronze/updateStatusPlacaBronze?Id=' + row.placaBronzeId + '" ' +
                                     'data-ejtip="Placa inativa - clique para ativar" ' +
                                     'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;">' +
-                                    'Inativo</a>'
-                                );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'placabronze.js',
-                                'DataTable.render.status',
-                                error,
-                            );
-                            return '';
+                                    'Inativo</a>';
+                            }
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("placabronze.js", "DataTable.render.status", error);
+                            return "";
                         }
                     },
                 },
                 {
-                    data: 'placaBronzeId',
-                    render: function (data) {
-                        try {
+                    data: "placaBronzeId",
+                    render: function (data)
+                    {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a href="/PlacaBronze/Upsert?id=${data}"
                                    class="btn btn-azul text-white"
@@ -432,27 +408,27 @@
                                     <i class="far fa-unlink"></i>
                                 </a>
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'placabronze.js',
-                                'DataTable.render.acoes',
-                                error,
-                            );
-                            return '';
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("placabronze.js", "DataTable.render.acoes", error);
+                            return "";
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
 
         console.log('✓ DataTable inicializado com sucesso');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('placabronze.js', 'loadList', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("placabronze.js", "loadList", error);
     }
 }
```
