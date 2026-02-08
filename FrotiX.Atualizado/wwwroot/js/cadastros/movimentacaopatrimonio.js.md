# wwwroot/js/cadastros/movimentacaopatrimonio.js

**Mudanca:** GRANDE | **+952** linhas | **-1007** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/movimentacaopatrimonio.js
+++ ATUAL: wwwroot/js/cadastros/movimentacaopatrimonio.js
@@ -1,399 +1,428 @@
 var dataTable;
 
-function mostrarLoading() {
-    try {
+function mostrarLoading()
+{
+    try
+    {
         var overlay = document.getElementById('loadingOverlayMovPatrimonio');
-        if (overlay) {
+        if (overlay)
+        {
             overlay.style.display = 'flex';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'mostrarLoading',
-            error,
-        );
-    }
-}
-
-function esconderLoading() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "mostrarLoading", error);
+    }
+}
+
+function esconderLoading()
+{
+    try
+    {
         var overlay = document.getElementById('loadingOverlayMovPatrimonio');
-        if (overlay) {
+        if (overlay)
+        {
             overlay.style.display = 'none';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'esconderLoading',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "esconderLoading", error);
+    }
+}
+
+$(document).ready(function ()
+{
+    try
+    {
         var path = window.location.pathname.toLowerCase();
 
-        if (
-            path == '/movimentacaopatrimonio/index' ||
-            path == '/movimentacaopatrimonio'
-        ) {
-            try {
-                console.log('Entrou no index');
-                setTimeout(function () {
-                    carregarFiltrosMovimentacoes();
-                }, 500);
+        if (path == "/movimentacaopatrimonio/index" || path == "/movimentacaopatrimonio")
+        {
+            try
+            {
+                console.log("Entrou no index");
+                setTimeout(function () { carregarFiltrosMovimentacoes(); }, 500);
                 loadList();
 
-                $(document).on('click', '.btn-delete', function () {
-                    try {
-                        var id = $(this).data('id');
+                $(document).on("click", ".btn-delete", function ()
+                {
+                    try
+                    {
+                        var id = $(this).data("id");
 
                         Alerta.Confirmar(
-                            'Confirmar Exclusão',
-                            'Você tem certeza que deseja apagar esta movimentação? Não será possível recuperar os dados eliminados!',
-                            'Sim, excluir',
-                            'Cancelar',
-                        ).then(function (confirmed) {
-                            try {
-                                if (confirmed) {
-                                    var dataToPost = JSON.stringify({
-                                        MovimentacaoPatrimonioId: id,
-                                    });
-                                    var url =
-                                        '/api/Patrimonio/DeleteMovimentacaoPatrimonio';
+                            "Confirmar Exclusão",
+                            "Você tem certeza que deseja apagar esta movimentação? Não será possível recuperar os dados eliminados!",
+                            "Sim, excluir",
+                            "Cancelar"
+                        ).then(function (confirmed)
+                        {
+                            try
+                            {
+                                if (confirmed)
+                                {
+                                    var dataToPost = JSON.stringify({ MovimentacaoPatrimonioId: id });
+                                    var url = "/api/Patrimonio/DeleteMovimentacaoPatrimonio";
 
                                     $.ajax({
                                         url: url,
-                                        type: 'POST',
+                                        type: "POST",
                                         data: dataToPost,
-                                        contentType:
-                                            'application/json; charset=utf-8',
-                                        dataType: 'json',
-                                        success: function (data) {
-                                            try {
-                                                if (data.success) {
-                                                    AppToast.show(
-                                                        'Verde',
-                                                        data.message,
-                                                        3000,
-                                                    );
+                                        contentType: "application/json; charset=utf-8",
+                                        dataType: "json",
+                                        success: function (data)
+                                        {
+                                            try
+                                            {
+                                                if (data.success)
+                                                {
+                                                    AppToast.show("Verde", data.message, 3000);
                                                     dataTable.ajax.reload();
-                                                } else {
-                                                    AppToast.show(
-                                                        'Vermelho',
-                                                        data.message,
-                                                        3000,
-                                                    );
                                                 }
-                                            } catch (error) {
+                                                else
+                                                {
+                                                    AppToast.show("Vermelho", data.message, 3000);
+                                                }
+                                            }
+                                            catch (error)
+                                            {
                                                 Alerta.TratamentoErroComLinha(
-                                                    'movimentacaopatrimonio.js',
-                                                    'index.btnDelete.success',
-                                                    error,
+                                                    "movimentacaopatrimonio.js",
+                                                    "index.btnDelete.success",
+                                                    error
                                                 );
                                             }
                                         },
-                                        error: function (err) {
-                                            try {
+                                        error: function (err)
+                                        {
+                                            try
+                                            {
                                                 console.log(err);
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    'Erro ao excluir movimentação',
-                                                    3000,
-                                                );
-                                            } catch (error) {
+                                                AppToast.show("Vermelho", "Erro ao excluir movimentação", 3000);
+                                            }
+                                            catch (error)
+                                            {
                                                 Alerta.TratamentoErroComLinha(
-                                                    'movimentacaopatrimonio.js',
-                                                    'index.btnDelete.error',
-                                                    error,
+                                                    "movimentacaopatrimonio.js",
+                                                    "index.btnDelete.error",
+                                                    error
                                                 );
                                             }
-                                        },
+                                        }
                                     });
                                 }
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 Alerta.TratamentoErroComLinha(
-                                    'movimentacaopatrimonio.js',
-                                    'index.btnDelete.then',
-                                    error,
+                                    "movimentacaopatrimonio.js",
+                                    "index.btnDelete.then",
+                                    error
                                 );
                             }
                         });
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'index.btnDelete.handler',
-                            error,
+                            "movimentacaopatrimonio.js",
+                            "index.btnDelete.handler",
+                            error
                         );
                     }
                 });
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'index.branch',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "index.branch",
+                    error
                 );
             }
-        } else if (path == '/movimentacaopatrimonio/upsert') {
-            try {
-                console.log('Ta na movimentacaopatrmonio/upsert');
-
-                setTimeout(function () {
-                    try {
+        }
+        else if (path == "/movimentacaopatrimonio/upsert")
+        {
+            try
+            {
+                console.log("Ta na movimentacaopatrmonio/upsert");
+
+                setTimeout(function ()
+                {
+                    try
+                    {
                         inicializarFormularioUpsert();
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'upsert.setTimeout.init',
-                            error,
+                            "movimentacaopatrimonio.js",
+                            "upsert.setTimeout.init",
+                            error
                         );
                     }
                 }, 500);
 
                 configurarHandlersFormulario();
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'upsert.branch',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "upsert.branch",
+                    error
                 );
             }
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'document.ready',
-            error,
+            "movimentacaopatrimonio.js",
+            "document.ready",
+            error
         );
     }
 });
 
-function loadList() {
-    try {
-        console.log('Entrou na loadList');
+function loadList()
+{
+    try
+    {
+        console.log("Entrou na loadList");
         mostrarLoading();
-        dataTable = $('#tblMovimentacaoPatrimonio').DataTable({
+        dataTable = $("#tblMovimentacaoPatrimonio").DataTable({
             columnDefs: [
-                { targets: 0, className: 'text-center', width: '8%' },
-                { targets: 1, className: 'text-left', width: '8%' },
-                { targets: 2, className: 'text-left', width: '20%' },
-                {
-                    targets: 3,
-                    className: 'text-left',
-                    width: '10%',
-                    defaultContent: '',
-                },
-                {
-                    targets: 4,
-                    className: 'text-left',
-                    width: '10%',
-                    defaultContent: '',
-                },
-                {
-                    targets: 5,
-                    className: 'text-left',
-                    width: '10%',
-                    defaultContent: '',
-                },
-                { targets: 6, className: 'text-center', width: '10%' },
-                { targets: 7, className: 'text-right', width: '10%' },
-                { targets: 8, className: 'text-center', width: '8%' },
+                { targets: 0, className: "text-center", width: "8%" },
+                { targets: 1, className: "text-left", width: "8%" },
+                { targets: 2, className: "text-left", width: "20%" },
+                { targets: 3, className: "text-left", width: "10%", defaultContent: "" },
+                { targets: 4, className: "text-left", width: "10%", defaultContent: "" },
+                { targets: 5, className: "text-left", width: "10%", defaultContent: "" },
+                { targets: 6, className: "text-center", width: "10%" },
+                { targets: 7, className: "text-right", width: "10%" },
+                { targets: 8, className: "text-center", width: "8%" }
             ],
             responsive: true,
             ajax: {
-                url: '/api/Patrimonio/MovimentacaoPatrimonioGrid',
-                type: 'GET',
-                dataType: 'json',
-                error: function (xhr, status, error) {
-                    try {
+                url: "/api/Patrimonio/MovimentacaoPatrimonioGrid",
+                type: "GET",
+                dataType: "json",
+                error: function (xhr, status, error)
+                {
+                    try
+                    {
                         esconderLoading();
-                        console.error('Erro ao carregar os dados:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar dados da tabela',
-                            3000,
-                        );
-                    } catch (error) {
+                        console.error("Erro ao carregar os dados:", error);
+                        AppToast.show("Vermelho", "Erro ao carregar dados da tabela", 3000);
+                    }
+                    catch (error)
+                    {
                         esconderLoading();
                         Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'loadList.ajax.error',
-                            error,
+                            "movimentacaopatrimonio.js",
+                            "loadList.ajax.error",
+                            error
                         );
                     }
-                },
+                }
             },
-            initComplete: function () {
+            initComplete: function ()
+            {
                 esconderLoading();
             },
             columns: [
                 {
-                    data: 'dataMovimentacao',
-                    type: 'date',
-                    render: function (data, type, row) {
-                        try {
-                            if (type === 'display' && data) {
+                    data: "dataMovimentacao",
+                    type: "date",
+                    render: function (data, type, row)
+                    {
+                        try
+                        {
+                            if (type === "display" && data)
+                            {
                                 var date = new Date(data);
-                                var day = String(date.getDate()).padStart(
-                                    2,
-                                    '0',
-                                );
-                                var month = String(
-                                    date.getMonth() + 1,
-                                ).padStart(2, '0');
+                                var day = String(date.getDate()).padStart(2, "0");
+                                var month = String(date.getMonth() + 1).padStart(2, "0");
                                 var year = date.getFullYear();
-                                return day + '/' + month + '/' + year;
+                                return day + "/" + month + "/" + year;
                             }
                             return data;
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadList.columns[0].render',
-                                error,
+                                "movimentacaopatrimonio.js",
+                                "loadList.columns[0].render",
+                                error
                             );
                             return data;
                         }
-                    },
+                    }
                 },
-                { data: 'npr' },
-                { data: 'descricao' },
-                { data: 'setorOrigemNome' },
-                { data: 'secaoOrigemNome' },
-                { data: 'setorDestinoNome' },
-                { data: 'secaoDestinoNome' },
-                { data: 'responsavelMovimentacao' },
-                {
-                    data: 'movimentacaoPatrimonioId',
-                    render: function (data) {
-                        try {
+                { data: "npr" },
+                { data: "descricao" },
+                { data: "setorOrigemNome" },
+                { data: "secaoOrigemNome" },
+                { data: "setorDestinoNome" },
+                { data: "secaoDestinoNome" },
+                { data: "responsavelMovimentacao" },
+                {
+                    data: "movimentacaoPatrimonioId",
+                    render: function (data)
+                    {
+                        try
+                        {
                             return (
                                 '<div class="text-center">' +
                                 '<a class="btn-delete btn btn-vinho text-white" ' +
                                 'data-ejtip="Excluir movimentação" ' +
                                 'style="cursor:pointer; padding: 2px 6px !important; font-size: 12px !important; margin: 1px !important;" ' +
-                                "data-id='" +
-                                data +
-                                "'>" +
+                                "data-id='" + data + "'>" +
                                 '<i class="far fa-trash-alt"></i>' +
-                                '</a>' +
-                                '</div>'
+                                "</a>" +
+                                "</div>"
                             );
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadList.columns[8].render',
-                                error,
+                                "movimentacaopatrimonio.js",
+                                "loadList.columns[8].render",
+                                error
                             );
-                            return '';
-                        }
-                    },
-                },
+                            return "";
+                        }
+                    }
+                }
             ],
-            order: [[0, 'desc']],
+            order: [[0, "desc"]],
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Sem Dados para Exibição',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Sem Dados para Exibição"
             },
-            width: '100%',
+            width: "100%"
         });
-        console.log('Saiu da loadList');
-    } catch (error) {
+        console.log("Saiu da loadList");
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'loadList',
-            error,
+            "movimentacaopatrimonio.js",
+            "loadList",
+            error
         );
     }
 }
 
-function inicializarFormularioUpsert() {
-    try {
+function inicializarFormularioUpsert()
+{
+    try
+    {
 
         var movimentacaoId = $('#MovimentacaoPatrimonioId').val();
-        var isEdicao =
-            movimentacaoId &&
-            movimentacaoId !== '00000000-0000-0000-0000-000000000000';
-
-        if (isEdicao) {
+        var isEdicao = movimentacaoId && movimentacaoId !== '00000000-0000-0000-0000-000000000000';
+
+        if (isEdicao)
+        {
             $('#tituloFormulario').text('Atualizar Movimentação');
             $('#textoBotaoSalvar').text('Atualizar Movimentação');
             carregarDadosMovimentacao(movimentacaoId);
-        } else {
-            console.log('Não é edição');
-            var divSecao = document.getElementById('divSecaoDestino');
-            if (divSecao) divSecao.style.display = 'none';
+        }
+        else
+        {
+            console.log("Não é edição");
+            var divSecao = document.getElementById("divSecaoDestino");
+            if (divSecao) divSecao.style.display = "none";
         }
 
         loadListaPatrimonios();
         loadListaSetoresDestino();
 
         setStatusUI(false);
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'inicializarFormularioUpsert',
-            error,
+            "movimentacaopatrimonio.js",
+            "inicializarFormularioUpsert",
+            error
         );
     }
 }
 
-function configurarHandlersFormulario() {
-    try {
-
-        $('#btnSalvar')
-            .off('click')
-            .on('click', function (e) {
-                try {
-                    e.preventDefault();
-                    salvarMovimentacao();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'configurarHandlersFormulario.btnSalvar.click',
-                        error,
-                    );
-                }
-            });
-
-        $('#formsMovimentacaoPatrimonio').on('keypress', function (e) {
-            try {
-                if (e.key === 'Enter' || e.keyCode === 13) {
+function configurarHandlersFormulario()
+{
+    try
+    {
+
+        $('#btnSalvar').off('click').on('click', function (e)
+        {
+            try
+            {
+                e.preventDefault();
+                salvarMovimentacao();
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha(
+                    "movimentacaopatrimonio.js",
+                    "configurarHandlersFormulario.btnSalvar.click",
+                    error
+                );
+            }
+        });
+
+        $('#formsMovimentacaoPatrimonio').on('keypress', function (e)
+        {
+            try
+            {
+                if (e.key === 'Enter' || e.keyCode === 13)
+                {
                     var src = e.target || e.srcElement;
-                    if (
-                        src.tagName.toLowerCase() !== 'textarea' &&
-                        src.tagName.toLowerCase() !== 'button'
-                    ) {
+                    if (src.tagName.toLowerCase() !== 'textarea' && src.tagName.toLowerCase() !== 'button')
+                    {
                         e.preventDefault();
                         return false;
                     }
                 }
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'configurarHandlersFormulario.keypress',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "configurarHandlersFormulario.keypress",
+                    error
                 );
             }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'configurarHandlersFormulario',
-            error,
+            "movimentacaopatrimonio.js",
+            "configurarHandlersFormulario",
+            error
         );
     }
 }
 
-function carregarDadosMovimentacao(id) {
-    try {
+function carregarDadosMovimentacao(id)
+{
+    try
+    {
         $.ajax({
             url: '/api/Patrimonio/GetMovimentacao',
             type: 'GET',
             data: { id: id },
-            success: function (res) {
-                try {
-                    if (res && res.success) {
+            success: function (res)
+            {
+                try
+                {
+                    if (res && res.success)
+                    {
                         var data = res.data;
 
                         $('#SetorOrigemId').val(data.setorOrigemId || '');
@@ -405,208 +434,154 @@
 
                         setStatusUI(data.status || false);
 
-                        setTimeout(function () {
-                            try {
-                                var cmbPatrimonio =
-                                    document.getElementById('cmbPatrimonio')
-                                        ?.ej2_instances?.[0];
-                                if (cmbPatrimonio && data.patrimonioId) {
+                        setTimeout(function ()
+                        {
+                            try
+                            {
+                                var cmbPatrimonio = document.getElementById('cmbPatrimonio')?.ej2_instances?.[0];
+                                if (cmbPatrimonio && data.patrimonioId)
+                                {
                                     cmbPatrimonio.value = data.patrimonioId;
                                 }
 
-                                var cmbSetorDestino =
-                                    document.getElementById('cmbSetorDestino')
-                                        ?.ej2_instances?.[0];
-                                if (cmbSetorDestino && data.setorDestinoId) {
+                                var cmbSetorDestino = document.getElementById('cmbSetorDestino')?.ej2_instances?.[0];
+                                if (cmbSetorDestino && data.setorDestinoId)
+                                {
                                     cmbSetorDestino.value = data.setorDestinoId;
 
-                                    if (data.setorDestinoId) {
-                                        loadListaSecoes(
-                                            data.setorDestinoId,
-                                            data.secaoDestinoId,
-                                        );
+                                    if (data.setorDestinoId)
+                                    {
+                                        loadListaSecoes(data.setorDestinoId, data.secaoDestinoId);
                                     }
                                 }
 
-                                var datePicker =
-                                    document.getElementById('dataMov')
-                                        ?.ej2_instances?.[0];
-                                if (datePicker && data.dataMovimentacao) {
-                                    datePicker.value = new Date(
-                                        data.dataMovimentacao,
-                                    );
+                                var datePicker = document.getElementById('dataMov')?.ej2_instances?.[0];
+                                if (datePicker && data.dataMovimentacao)
+                                {
+                                    datePicker.value = new Date(data.dataMovimentacao);
                                 }
-                            } catch (error) {
+                            }
+                            catch (error)
+                            {
                                 Alerta.TratamentoErroComLinha(
-                                    'movimentacaopatrimonio.js',
-                                    'carregarDadosMovimentacao.setTimeout',
-                                    error,
+                                    "movimentacaopatrimonio.js",
+                                    "carregarDadosMovimentacao.setTimeout",
+                                    error
                                 );
                             }
                         }, 700);
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'carregarDadosMovimentacao.success',
-                        error,
+                        "movimentacaopatrimonio.js",
+                        "carregarDadosMovimentacao.success",
+                        error
                     );
                 }
             },
-            error: function (err) {
-                try {
+            error: function (err)
+            {
+                try
+                {
                     console.error('Erro ao carregar movimentação:', err);
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao carregar dados da movimentação',
-                        3000,
+                    AppToast.show("Vermelho", "Erro ao carregar dados da movimentação", 3000);
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha(
+                        "movimentacaopatrimonio.js",
+                        "carregarDadosMovimentacao.error",
+                        error
                     );
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'carregarDadosMovimentacao.error',
-                        error,
-                    );
-                }
-            },
+                }
+            }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'carregarDadosMovimentacao',
-            error,
+            "movimentacaopatrimonio.js",
+            "carregarDadosMovimentacao",
+            error
         );
     }
 }
 
-function salvarMovimentacao() {
-    try {
-        console.log('=== Iniciando salvamento de movimentação ===');
-
-        var cmbPatrimonio =
-            document.getElementById('cmbPatrimonio')?.ej2_instances?.[0];
-        var cmbSetorDestino =
-            document.getElementById('cmbSetorDestino')?.ej2_instances?.[0];
-        var cmbSecaoDestino =
-            document.getElementById('cmbSecoesDestino')?.ej2_instances?.[0];
+function salvarMovimentacao()
+{
+    try
+    {
+        console.log("=== Iniciando salvamento de movimentação ===");
+
+        var cmbPatrimonio = document.getElementById('cmbPatrimonio')?.ej2_instances?.[0];
+        var cmbSetorDestino = document.getElementById('cmbSetorDestino')?.ej2_instances?.[0];
+        var cmbSecaoDestino = document.getElementById('cmbSecoesDestino')?.ej2_instances?.[0];
         var datePicker = document.getElementById('dataMov')?.ej2_instances?.[0];
-        var statusCheckbox =
-            document.getElementById('StatusCheckbox')?.ej2_instances?.[0];
+        var statusCheckbox = document.getElementById('StatusCheckbox')?.ej2_instances?.[0];
 
         var dados = {
             movimentacaoPatrimonioId: $('#MovimentacaoPatrimonioId').val(),
             patrimonioId: cmbPatrimonio?.value || $('#PatrimonioId').val(),
-            dataMovimentacao: datePicker?.value
-                ? datePicker.value.toISOString()
-                : null,
+            dataMovimentacao: datePicker?.value ? datePicker.value.toISOString() : null,
             setorOrigemId: $('#SetorOrigemId').val(),
             secaoOrigemId: $('#SecaoOrigemId').val(),
             setorDestinoId: cmbSetorDestino?.value,
             secaoDestinoId: cmbSecaoDestino?.value,
-            statusPatrimonio: statusCheckbox?.checked || false,
+            statusPatrimonio: statusCheckbox?.checked || false
         };
 
-        if (
-            !dados.patrimonioId ||
-            dados.patrimonioId === '' ||
-            dados.patrimonioId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'O Patrimônio não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.patrimonioId || dados.patrimonioId === '' || dados.patrimonioId === '00000000-0000-0000-0000-000000000000')
+        {
+            Alerta.Erro('Erro no formulário', 'O Patrimônio não pode estar em branco!', 'OK');
             return;
         }
 
-        if (!dados.dataMovimentacao) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'A data não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.dataMovimentacao)
+        {
+            Alerta.Erro('Erro no formulário', 'A data não pode estar em branco!', 'OK');
             return;
         }
 
-        if (
-            !dados.setorOrigemId ||
-            dados.setorOrigemId === '' ||
-            dados.setorOrigemId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'O setor de origem não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.setorOrigemId || dados.setorOrigemId === '' || dados.setorOrigemId === '00000000-0000-0000-0000-000000000000')
+        {
+            Alerta.Erro('Erro no formulário', 'O setor de origem não pode estar em branco!', 'OK');
             return;
         }
 
-        if (
-            !dados.secaoOrigemId ||
-            dados.secaoOrigemId === '' ||
-            dados.secaoOrigemId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'A seção de origem não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.secaoOrigemId || dados.secaoOrigemId === '' || dados.secaoOrigemId === '00000000-0000-0000-0000-000000000000')
+        {
+            Alerta.Erro('Erro no formulário', 'A seção de origem não pode estar em branco!', 'OK');
             return;
         }
 
-        if (
-            !dados.setorDestinoId ||
-            dados.setorDestinoId === '' ||
-            dados.setorDestinoId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'O setor de destino não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.setorDestinoId || dados.setorDestinoId === '' || dados.setorDestinoId === '00000000-0000-0000-0000-000000000000')
+        {
+            Alerta.Erro('Erro no formulário', 'O setor de destino não pode estar em branco!', 'OK');
             return;
         }
 
-        if (
-            !dados.secaoDestinoId ||
-            dados.secaoDestinoId === '' ||
-            dados.secaoDestinoId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'A seção de destino não pode estar em branco!',
-                'OK',
-            );
+        if (!dados.secaoDestinoId || dados.secaoDestinoId === '' || dados.secaoDestinoId === '00000000-0000-0000-0000-000000000000')
+        {
+            Alerta.Erro('Erro no formulário', 'A seção de destino não pode estar em branco!', 'OK');
             return;
         }
 
-        if (
-            dados.secaoDestinoId &&
-            dados.secaoOrigemId &&
-            dados.secaoDestinoId === dados.secaoOrigemId
-        ) {
-            Alerta.Erro(
-                'Erro no formulário',
-                'A seção de destino não pode ser a mesma que a seção de origem!',
-                'OK',
-            );
+        if (dados.secaoDestinoId && dados.secaoOrigemId && dados.secaoDestinoId === dados.secaoOrigemId)
+        {
+            Alerta.Erro('Erro no formulário', 'A seção de destino não pode ser a mesma que a seção de origem!', 'OK');
             return;
         }
 
-        console.log('Validações passaram com sucesso');
-
-        var isEdicao =
-            dados.movimentacaoPatrimonioId &&
-            dados.movimentacaoPatrimonioId !==
-                '00000000-0000-0000-0000-000000000000';
-
-        var url = isEdicao
-            ? '/api/Patrimonio/UpdateMovimentacao'
-            : '/api/Patrimonio/CreateMovimentacao';
-
-        $('#btnSalvar')
-            .prop('disabled', true)
-            .html('<i class="fa fa-spinner fa-spin"></i> Salvando...');
+        console.log("Validações passaram com sucesso");
+
+        var isEdicao = dados.movimentacaoPatrimonioId &&
+            dados.movimentacaoPatrimonioId !== '00000000-0000-0000-0000-000000000000';
+
+        var url = isEdicao ? '/api/Patrimonio/UpdateMovimentacao' : '/api/Patrimonio/CreateMovimentacao';
+
+        $('#btnSalvar').prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> Salvando...');
 
         $.ajax({
             url: url,
@@ -614,548 +589,583 @@
             data: JSON.stringify(dados),
             contentType: 'application/json; charset=utf-8',
             dataType: 'json',
-            success: function (res) {
-                try {
-                    if (res.success) {
-                        AppToast.show(
-                            res.message ||
-                                (isEdicao
-                                    ? 'Movimentação atualizada com sucesso!'
-                                    : 'Verde',
-                                'Movimentação registrada com sucesso!'),
-                            3000,
-                        );
-
-                        setTimeout(function () {
-                            try {
-                                window.location.href =
-                                    '/MovimentacaoPatrimonio/Index';
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'movimentacaopatrimonio.js',
-                                    'salvarMovimentacao.success.setTimeout',
-                                    error,
-                                );
+            success: function (res)
+            {
+                try
+                {
+                    if (res.success)
+                    {
+                        AppToast.show(res.message || (isEdicao ? 'Movimentação atualizada com sucesso!' : "Verde", 'Movimentação registrada com sucesso!'), 3000);
+
+                        setTimeout(function ()
+                        {
+                            try
+                            {
+                                window.location.href = '/MovimentacaoPatrimonio/Index';
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "salvarMovimentacao.success.setTimeout", error);
                             }
                         }, 1500);
-                    } else {
-                        AppToast.show(
-                            res.message || 'Vermelho',
-                            'Erro ao salvar movimentação',
-                            5000,
-                        );
-                        $('#btnSalvar')
-                            .prop('disabled', false)
-                            .html(
-                                '<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' +
-                                    (isEdicao
-                                        ? 'Atualizar Movimentação'
-                                        : 'Registrar Movimentação'),
-                            );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'salvarMovimentacao.success',
-                        error,
-                    );
-                    $('#btnSalvar')
-                        .prop('disabled', false)
-                        .html(
-                            '<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' +
-                                (isEdicao
-                                    ? 'Atualizar Movimentação'
-                                    : 'Registrar Movimentação'),
-                        );
+                    }
+                    else
+                    {
+                        AppToast.show(res.message || "Vermelho", 'Erro ao salvar movimentação', 5000);
+                        $('#btnSalvar').prop('disabled', false).html('<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' + (isEdicao ? 'Atualizar Movimentação' : 'Registrar Movimentação'));
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "salvarMovimentacao.success", error);
+                    $('#btnSalvar').prop('disabled', false).html('<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' + (isEdicao ? 'Atualizar Movimentação' : 'Registrar Movimentação'));
                 }
             },
-            error: function (xhr, status, error) {
-                try {
+            error: function (xhr, status, error)
+            {
+                try
+                {
                     console.error('Status HTTP:', xhr.status);
                     console.error('Error:', error);
 
-                    var mensagemErro = 'Erro ao salvar movimentação.';
-
-                    if (xhr.responseText) {
-                        try {
+                    var mensagemErro = "Erro ao salvar movimentação.";
+
+                    if (xhr.responseText)
+                    {
+                        try
+                        {
                             var responseObj = JSON.parse(xhr.responseText);
-                            if (responseObj.message) {
+                            if (responseObj.message)
+                            {
                                 mensagemErro = responseObj.message;
                             }
-                        } catch (e) {
-                            if (xhr.status === 500) {
-                                mensagemErro =
-                                    'Erro interno do servidor. Verifique os logs.';
-                            } else if (xhr.status === 400) {
-                                mensagemErro =
-                                    'Dados inválidos. Verifique o formulário.';
-                            }
-                        }
-                    }
-
-                    AppToast.show('Vermelho', mensagemErro, 3000);
-                    $('#btnSalvar')
-                        .prop('disabled', false)
-                        .html(
-                            '<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' +
-                                (isEdicao
-                                    ? 'Atualizar Movimentação'
-                                    : 'Registrar Movimentação'),
-                        );
-                } catch (e) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'salvarMovimentacao.error',
-                        e,
-                    );
-                }
-            },
+                        }
+                        catch (e)
+                        {
+                            if (xhr.status === 500)
+                            {
+                                mensagemErro = "Erro interno do servidor. Verifique os logs.";
+                            }
+                            else if (xhr.status === 400)
+                            {
+                                mensagemErro = "Dados inválidos. Verifique o formulário.";
+                            }
+                        }
+                    }
+
+                    AppToast.show("Vermelho", mensagemErro, 3000);
+                    $('#btnSalvar').prop('disabled', false).html('<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;' + (isEdicao ? 'Atualizar Movimentação' : 'Registrar Movimentação'));
+                }
+                catch (e)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "salvarMovimentacao.error", e);
+                }
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'salvarMovimentacao',
-            error,
-        );
-        $('#btnSalvar')
-            .prop('disabled', false)
-            .html(
-                '<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;Registrar Movimentação',
-            );
-    }
-}
-
-function getComboBoxInstance(elementId, callback) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "salvarMovimentacao", error);
+        $('#btnSalvar').prop('disabled', false).html('<i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>&nbsp;&nbsp;Registrar Movimentação');
+    }
+}
+
+function getComboBoxInstance(elementId, callback)
+{
+    try
+    {
         var attempts = 0;
         var maxAttempts = 10;
 
-        var checkInterval = setInterval(function () {
-            try {
+        var checkInterval = setInterval(function ()
+        {
+            try
+            {
                 var element = document.getElementById(elementId);
-                if (
-                    element &&
-                    element.ej2_instances &&
-                    element.ej2_instances[0]
-                ) {
+                if (element && element.ej2_instances && element.ej2_instances[0])
+                {
                     clearInterval(checkInterval);
-                    try {
+                    try
+                    {
                         callback(element.ej2_instances[0]);
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'getComboBoxInstance.callback',
-                            error,
+                            "movimentacaopatrimonio.js",
+                            "getComboBoxInstance.callback",
+                            error
                         );
                     }
-                } else {
+                }
+                else
+                {
                     attempts++;
-                    if (attempts >= maxAttempts) {
+                    if (attempts >= maxAttempts)
+                    {
                         clearInterval(checkInterval);
-                        try {
-                            console.error(
-                                'ComboBox ' +
-                                    elementId +
-                                    ' não foi inicializada após ' +
-                                    maxAttempts +
-                                    ' tentativas',
+                        try
+                        {
+                            console.error("ComboBox " + elementId + " não foi inicializada após " + maxAttempts + " tentativas");
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha(
+                                "movimentacaopatrimonio.js",
+                                "getComboBoxInstance.consoleError",
+                                error
                             );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'getComboBoxInstance.consoleError',
-                                error,
-                            );
-                        }
-                    }
-                }
-            } catch (error) {
+                        }
+                    }
+                }
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'getComboBoxInstance.interval',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "getComboBoxInstance.interval",
+                    error
                 );
             }
         }, 200);
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'getComboBoxInstance',
-            error,
+            "movimentacaopatrimonio.js",
+            "getComboBoxInstance",
+            error
         );
     }
 }
 
-function loadListaPatrimonios() {
-    try {
-        getComboBoxInstance('cmbPatrimonio', function (comboBox) {
-            try {
+function loadListaPatrimonios()
+{
+    try
+    {
+        getComboBoxInstance("cmbPatrimonio", function (comboBox)
+        {
+            try
+            {
                 comboBox.value = null;
-                comboBox.text = '';
+                comboBox.text = "";
 
                 $.ajax({
-                    type: 'get',
-                    url: '/api/Patrimonio/ListaPatrimonios',
-                    success: function (res) {
-                        try {
-                            if (res != null && res.data && res.data.length) {
+                    type: "get",
+                    url: "/api/Patrimonio/ListaPatrimonios",
+                    success: function (res)
+                    {
+                        try
+                        {
+                            if (res != null && res.data && res.data.length)
+                            {
                                 comboBox.dataSource = res.data;
 
-                                if (!comboBox.hasPatrimonioListener) {
+                                if (!comboBox.hasPatrimonioListener)
+                                {
                                     comboBox.change = onPatrimonioChange;
                                     comboBox.hasPatrimonioListener = true;
                                 }
-                            } else {
-                                console.log('Nenhum patrimônio encontrado.');
+                            }
+                            else
+                            {
+                                console.log("Nenhum patrimônio encontrado.");
                                 comboBox.dataSource = [];
                             }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaPatrimonios.success',
-                                error,
+                                "movimentacaopatrimonio.js",
+                                "loadListaPatrimonios.success",
+                                error
                             );
                         }
                     },
-                    error: function (error) {
-                        try {
-                            console.log(
-                                'Erro ao carregar patrimônios: ',
-                                error,
+                    error: function (error)
+                    {
+                        try
+                        {
+                            console.log("Erro ao carregar patrimônios: ", error);
+                            AppToast.show("Vermelho", "Erro ao carregar lista de patrimônios", 3000);
+                        }
+                        catch (e)
+                        {
+                            Alerta.TratamentoErroComLinha(
+                                "movimentacaopatrimonio.js",
+                                "loadListaPatrimonios.error",
+                                e
                             );
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar lista de patrimônios',
-                                3000,
-                            );
-                        } catch (e) {
-                            Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaPatrimonios.error',
-                                e,
-                            );
-                        }
-                    },
+                        }
+                    }
                 });
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'loadListaPatrimonios.comboInit',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "loadListaPatrimonios.comboInit",
+                    error
                 );
             }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'loadListaPatrimonios',
-            error,
+            "movimentacaopatrimonio.js",
+            "loadListaPatrimonios",
+            error
         );
     }
 }
 
-function loadListaSetoresDestino() {
-    try {
-        getComboBoxInstance('cmbSetorDestino', function (comboBox) {
-            try {
+function loadListaSetoresDestino()
+{
+    try
+    {
+        getComboBoxInstance("cmbSetorDestino", function (comboBox)
+        {
+            try
+            {
                 comboBox.value = null;
-                comboBox.text = '';
+                comboBox.text = "";
 
                 $.ajax({
-                    type: 'get',
-                    url: '/api/Setor/ListaSetores',
-                    success: function (res) {
-                        try {
-                            if (res != null && res.data && res.data.length) {
+                    type: "get",
+                    url: "/api/Setor/ListaSetores",
+                    success: function (res)
+                    {
+                        try
+                        {
+                            if (res != null && res.data && res.data.length)
+                            {
                                 comboBox.dataSource = res.data;
 
-                                if (!comboBox.hasSetorListener) {
+                                if (!comboBox.hasSetorListener)
+                                {
                                     comboBox.change = onSetorChangeDestino;
                                     comboBox.hasSetorListener = true;
                                 }
-                            } else {
-                                console.log('Nenhum setor encontrado.');
+                            }
+                            else
+                            {
+                                console.log("Nenhum setor encontrado.");
                                 comboBox.dataSource = [];
                             }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaSetoresDestino.success',
-                                error,
+                                "movimentacaopatrimonio.js",
+                                "loadListaSetoresDestino.success",
+                                error
                             );
                         }
                     },
-                    error: function (error) {
-                        try {
-                            console.log('Erro ao carregar setores: ' + error);
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar lista de setores',
-                                5000,
+                    error: function (error)
+                    {
+                        try
+                        {
+                            console.log("Erro ao carregar setores: " + error);
+                            AppToast.show("Vermelho", "Erro ao carregar lista de setores", 5000);
+                        }
+                        catch (e)
+                        {
+                            Alerta.TratamentoErroComLinha(
+                                "movimentacaopatrimonio.js",
+                                "loadListaSetoresDestino.error",
+                                e
                             );
-                        } catch (e) {
-                            Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaSetoresDestino.error',
-                                e,
-                            );
-                        }
-                    },
+                        }
+                    }
                 });
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'loadListaSetoresDestino.comboInit',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "loadListaSetoresDestino.comboInit",
+                    error
                 );
             }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'loadListaSetoresDestino',
-            error,
+            "movimentacaopatrimonio.js",
+            "loadListaSetoresDestino",
+            error
         );
     }
 }
 
-function loadListaSecoes(setorSelecionado, secaoIdToSelect) {
-    try {
-        getComboBoxInstance('cmbSecoesDestino', function (comboBox) {
-            try {
+function loadListaSecoes(setorSelecionado, secaoIdToSelect)
+{
+    try
+    {
+        getComboBoxInstance("cmbSecoesDestino", function (comboBox)
+        {
+            try
+            {
                 comboBox.value = null;
-                comboBox.text = '';
+                comboBox.text = "";
 
                 $.ajax({
-                    type: 'get',
-                    url: '/api/Secao/ListaSecoes',
+                    type: "get",
+                    url: "/api/Secao/ListaSecoes",
                     data: { setorSelecionado: setorSelecionado },
-                    success: function (res) {
-                        try {
-                            if (res != null && res.data && res.data.length) {
-                                var processedData = res.data.map(
-                                    function (item) {
-                                        try {
-                                            var nome = item.text
-                                                .split('/')[0]
-                                                .trim();
-                                            return {
-                                                text: nome,
-                                                value: item.value,
-                                            };
-                                        } catch (error) {
+                    success: function (res)
+                    {
+                        try
+                        {
+                            if (res != null && res.data && res.data.length)
+                            {
+                                var processedData = res.data.map(function (item)
+                                {
+                                    try
+                                    {
+                                        var nome = item.text.split("/")[0].trim();
+                                        return { text: nome, value: item.value };
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "movimentacaopatrimonio.js",
+                                            "loadListaSecoes.map",
+                                            error
+                                        );
+                                        return { text: item && item.text ? item.text : "", value: item ? item.value : "" };
+                                    }
+                                });
+
+                                comboBox.dataSource = processedData;
+
+                                if (secaoIdToSelect)
+                                {
+                                    setTimeout(function ()
+                                    {
+                                        try
+                                        {
+                                            comboBox.value = secaoIdToSelect;
+                                        }
+                                        catch (error)
+                                        {
                                             Alerta.TratamentoErroComLinha(
-                                                'movimentacaopatrimonio.js',
-                                                'loadListaSecoes.map',
-                                                error,
-                                            );
-                                            return {
-                                                text:
-                                                    item && item.text
-                                                        ? item.text
-                                                        : '',
-                                                value: item ? item.value : '',
-                                            };
-                                        }
-                                    },
-                                );
-
-                                comboBox.dataSource = processedData;
-
-                                if (secaoIdToSelect) {
-                                    setTimeout(function () {
-                                        try {
-                                            comboBox.value = secaoIdToSelect;
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'movimentacaopatrimonio.js',
-                                                'loadListaSecoes.setTimeout',
-                                                error,
+                                                "movimentacaopatrimonio.js",
+                                                "loadListaSecoes.setTimeout",
+                                                error
                                             );
                                         }
                                     }, 100);
                                 }
-                            } else {
-                                console.log('Nenhuma seção encontrada.');
+                            }
+                            else
+                            {
+                                console.log("Nenhuma seção encontrada.");
                                 comboBox.dataSource = [];
                             }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaSecoes.success',
-                                error,
+                                "movimentacaopatrimonio.js",
+                                "loadListaSecoes.success",
+                                error
                             );
                         }
                     },
-                    error: function (error) {
-                        try {
-                            console.log('Erro na requisição: ', error);
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao carregar lista de seções',
-                                3000,
+                    error: function (error)
+                    {
+                        try
+                        {
+                            console.log("Erro na requisição: ", error);
+                            AppToast.show("Vermelho", "Erro ao carregar lista de seções", 3000);
+                        }
+                        catch (e)
+                        {
+                            Alerta.TratamentoErroComLinha(
+                                "movimentacaopatrimonio.js",
+                                "loadListaSecoes.error",
+                                e
                             );
-                        } catch (e) {
-                            Alerta.TratamentoErroComLinha(
-                                'movimentacaopatrimonio.js',
-                                'loadListaSecoes.error',
-                                e,
-                            );
-                        }
-                    },
+                        }
+                    }
                 });
-            } catch (error) {
+            }
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha(
-                    'movimentacaopatrimonio.js',
-                    'loadListaSecoes.comboInit',
-                    error,
+                    "movimentacaopatrimonio.js",
+                    "loadListaSecoes.comboInit",
+                    error
                 );
             }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'loadListaSecoes',
-            error,
+            "movimentacaopatrimonio.js",
+            "loadListaSecoes",
+            error
         );
     }
 }
 
-function onPatrimonioChange(args) {
-    try {
+function onPatrimonioChange(args)
+{
+    try
+    {
         var patrimonioId = args.value;
-        console.log('Patrimônio selecionado:', patrimonioId);
-
-        if (
-            patrimonioId &&
-            patrimonioId != '00000000-0000-0000-0000-000000000000'
-        ) {
+        console.log("Patrimônio selecionado:", patrimonioId);
+
+        if (patrimonioId && patrimonioId != "00000000-0000-0000-0000-000000000000")
+        {
             $.ajax({
-                type: 'get',
-                url: '/api/Patrimonio/GetSingle',
+                type: "get",
+                url: "/api/Patrimonio/GetSingle",
                 data: { Id: patrimonioId },
-                success: function (res) {
-                    try {
-                        if (res != null && res.success == true) {
-                            console.log('SetorId: ' + res.data.setorOrigemId);
-                            console.log('SecaoId: ' + res.data.secaoOrigemId);
-
-                            $('#SetorOrigemId').val(
-                                res.data.setorOrigemId || '',
-                            );
-                            $('#SecaoOrigemId').val(
-                                res.data.secaoOrigemId || '',
-                            );
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res != null && res.success == true)
+                        {
+                            console.log("SetorId: " + res.data.setorOrigemId);
+                            console.log("SecaoId: " + res.data.secaoOrigemId);
+
+                            $('#SetorOrigemId').val(res.data.setorOrigemId || "");
+                            $('#SecaoOrigemId').val(res.data.secaoOrigemId || "");
                             $('#PatrimonioId').val(patrimonioId);
 
-                            $('#SetorOrigem').val(
-                                res.data.setorOrigemNome || '',
-                            );
-                            $('#SecaoOrigem').val(
-                                res.data.secaoOrigemNome || '',
-                            );
+                            $('#SetorOrigem').val(res.data.setorOrigemNome || "");
+                            $('#SecaoOrigem').val(res.data.secaoOrigemNome || "");
 
                             setStatusUI(res.data.status || false);
                         }
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'onPatrimonioChange.success',
-                            error,
+                            "movimentacaopatrimonio.js",
+                            "onPatrimonioChange.success",
+                            error
                         );
                     }
                 },
-                error: function (xhr, status, error) {
-                    try {
-                        console.log('Erro ao buscar patrimônio:', error);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar dados do patrimônio',
-                            3000,
+                error: function (xhr, status, error)
+                {
+                    try
+                    {
+                        console.log("Erro ao buscar patrimônio:", error);
+                        AppToast.show("Vermelho", "Erro ao carregar dados do patrimônio", 3000);
+                    }
+                    catch (e)
+                    {
+                        Alerta.TratamentoErroComLinha(
+                            "movimentacaopatrimonio.js",
+                            "onPatrimonioChange.error",
+                            e
                         );
-                    } catch (e) {
-                        Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'onPatrimonioChange.error',
-                            e,
-                        );
-                    }
-                },
+                    }
+                }
             });
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'onPatrimonioChange',
-            error,
+            "movimentacaopatrimonio.js",
+            "onPatrimonioChange",
+            error
         );
     }
 }
 
-function onSetorChangeDestino(args) {
-    try {
+function onSetorChangeDestino(args)
+{
+    try
+    {
         var setorSelecionado = args.value;
-        console.log('Setor selecionado:', setorSelecionado);
-
-        if (setorSelecionado) {
-            var divSecao = document.getElementById('divSecaoDestino');
-            if (divSecao) divSecao.style.display = 'block';
+        console.log("Setor selecionado:", setorSelecionado);
+
+        if (setorSelecionado)
+        {
+            var divSecao = document.getElementById("divSecaoDestino");
+            if (divSecao) divSecao.style.display = "block";
             loadListaSecoes(setorSelecionado);
         }
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'onSetorChangeDestino',
-            error,
+            "movimentacaopatrimonio.js",
+            "onSetorChangeDestino",
+            error
         );
     }
 }
 
-function setStatusUI(isActive) {
-    try {
-        var host = document.getElementById('StatusCheckbox');
-        var lbl = document.getElementById('StatusCheckboxLabel');
-
-        if (host) {
+function setStatusUI(isActive)
+{
+    try
+    {
+        var host = document.getElementById("StatusCheckbox");
+        var lbl = document.getElementById("StatusCheckboxLabel");
+
+        if (host)
+        {
             var ej2 = host.ej2_instances && host.ej2_instances[0];
-            if (ej2 && typeof ej2.setProperties === 'function') {
+            if (ej2 && typeof ej2.setProperties === "function")
+            {
                 ej2.setProperties({ checked: !!isActive });
-                if (typeof ej2.dataBind === 'function') ej2.dataBind();
-            } else {
+                if (typeof ej2.dataBind === "function") ej2.dataBind();
+            }
+            else
+            {
                 host.checked = !!isActive;
             }
         }
 
-        if (lbl) {
-            lbl.textContent = isActive ? 'Ativo' : 'Baixado';
-            lbl.className =
-                'status-label ' +
-                (isActive ? 'ativo text-success' : 'baixado text-danger');
-        }
-    } catch (error) {
+        if (lbl)
+        {
+            lbl.textContent = isActive ? "Ativo" : "Baixado";
+            lbl.className = "status-label " + (isActive ? "ativo text-success" : "baixado text-danger");
+        }
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'setStatusUI',
-            error,
+            "movimentacaopatrimonio.js",
+            "setStatusUI",
+            error
         );
     }
 }
 
-window.onEJ2CheckboxChange = function (args) {
-    try {
+window.onEJ2CheckboxChange = function (args)
+{
+    try
+    {
         setStatusUI(!!args.checked);
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'onEJ2CheckboxChange',
-            error,
+            "movimentacaopatrimonio.js",
+            "onEJ2CheckboxChange",
+            error
         );
     }
 };
 
-function carregarFiltrosMovimentacoes() {
-    try {
-        console.log('🔧 Carregando filtros de movimentações...');
+function carregarFiltrosMovimentacoes()
+{
+    try
+    {
+        console.log("🔧 Carregando filtros de movimentações...");
 
         carregarSetoresSecoesMovimentacoes('ddtSetorSecaoOrigem');
 
@@ -1164,305 +1174,240 @@
         carregarResponsaveisMovimentacoes();
 
         var btnFiltrar = document.getElementById('btnFiltrarMovimentacoes');
-        if (btnFiltrar) {
-            btnFiltrar.addEventListener('click', function () {
-                try {
-                    console.log('🔍 Aplicando filtros de movimentações...');
+        if (btnFiltrar)
+        {
+            btnFiltrar.addEventListener('click', function ()
+            {
+                try
+                {
+                    console.log("🔍 Aplicando filtros de movimentações...");
                     aplicarFiltrosMovimentacoes();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'btnFiltrarMovimentacoes.click',
-                        error,
-                    );
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "btnFiltrarMovimentacoes.click", error);
                 }
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'carregarFiltrosMovimentacoes',
-            error,
-        );
-    }
-}
-
-function carregarSetoresSecoesMovimentacoes(elementId) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "carregarFiltrosMovimentacoes", error);
+    }
+}
+
+function carregarSetoresSecoesMovimentacoes(elementId)
+{
+    try
+    {
         console.log(`🔄 Carregando ${elementId}...`);
         $.ajax({
             url: '/api/Patrimonio/GetSetoresSecoesHierarquicos',
             type: 'GET',
             dataType: 'json',
-            success: function (response) {
-                try {
+            success: function (response)
+            {
+                try
+                {
                     console.log(`📦 Resposta para ${elementId}:`, response);
-                    if (response.success && response.data) {
+                    if (response.success && response.data)
+                    {
                         var element = document.getElementById(elementId);
-                        if (
-                            element &&
-                            element.ej2_instances &&
-                            element.ej2_instances[0]
-                        ) {
+                        if (element && element.ej2_instances && element.ej2_instances[0])
+                        {
                             var ddtInstance = element.ej2_instances[0];
                             ddtInstance.fields = {
                                 dataSource: response.data,
                                 value: 'id',
                                 text: 'name',
                                 child: 'children',
-                                hasChildren: 'hasChildren',
+                                hasChildren: 'hasChildren'
                             };
                             ddtInstance.dataBind();
-                            console.log(
-                                `✅ ${elementId} carregado com ${response.data.length} setores`,
-                            );
-                        } else {
-                            console.error(
-                                `❌ Elemento ${elementId} ou instância EJ2 não encontrado`,
-                            );
-                        }
-                    } else {
-                        console.error(
-                            `❌ Resposta inválida para ${elementId}:`,
-                            response,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        `carregarSetoresSecoesMovimentacoes.${elementId}.success`,
-                        error,
-                    );
+                            console.log(`✅ ${elementId} carregado com ${response.data.length} setores`);
+                        }
+                        else
+                        {
+                            console.error(`❌ Elemento ${elementId} ou instância EJ2 não encontrado`);
+                        }
+                    }
+                    else
+                    {
+                        console.error(`❌ Resposta inválida para ${elementId}:`, response);
+                    }
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", `carregarSetoresSecoesMovimentacoes.${elementId}.success`, error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
-                    console.error(`❌ Erro AJAX ao carregar ${elementId}:`, {
-                        xhr: xhr,
-                        status: status,
-                        error: error,
-                    });
-                    AppToast.show(
-                        'Vermelho',
-                        `Erro ao carregar ${elementId}`,
-                        3000,
-                    );
-                } catch (err) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        `carregarSetoresSecoesMovimentacoes.${elementId}.error`,
-                        err,
-                    );
-                }
-            },
+            error: function (xhr, status, error)
+            {
+                try
+                {
+                    console.error(`❌ Erro AJAX ao carregar ${elementId}:`, { xhr: xhr, status: status, error: error });
+                    AppToast.show('Vermelho', `Erro ao carregar ${elementId}`, 3000);
+                } catch (err)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", `carregarSetoresSecoesMovimentacoes.${elementId}.error`, err);
+                }
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            `carregarSetoresSecoesMovimentacoes.${elementId}`,
-            error,
-        );
-    }
-}
-
-function carregarResponsaveisMovimentacoes() {
-    try {
-        console.log('🔄 Carregando responsáveis...');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", `carregarSetoresSecoesMovimentacoes.${elementId}`, error);
+    }
+}
+
+function carregarResponsaveisMovimentacoes()
+{
+    try
+    {
+        console.log("🔄 Carregando responsáveis...");
         $.ajax({
             url: '/api/Patrimonio/GetResponsaveisMovimentacoes',
             type: 'GET',
             dataType: 'json',
-            success: function (response) {
-                try {
-                    console.log('📦 Resposta responsáveis:', response);
-                    if (response.success && response.data) {
+            success: function (response)
+            {
+                try
+                {
+                    console.log("📦 Resposta responsáveis:", response);
+                    if (response.success && response.data)
+                    {
                         var element = document.getElementById('cmbResponsavel');
-                        if (
-                            element &&
-                            element.ej2_instances &&
-                            element.ej2_instances[0]
-                        ) {
+                        if (element && element.ej2_instances && element.ej2_instances[0])
+                        {
                             var cmbInstance = element.ej2_instances[0];
                             cmbInstance.dataSource = response.data;
-                            cmbInstance.fields = {
-                                text: 'text',
-                                value: 'value',
-                            };
+                            cmbInstance.fields = { text: 'text', value: 'value' };
                             cmbInstance.dataBind();
-                            console.log(
-                                `✅ Responsáveis carregados: ${response.data.length} registros`,
-                            );
-                        } else {
-                            console.error(
-                                '❌ Elemento cmbResponsavel ou instância EJ2 não encontrado',
-                            );
-                        }
-                    } else {
-                        console.error(
-                            '❌ Resposta inválida para responsáveis:',
-                            response,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'carregarResponsaveisMovimentacoes.success',
-                        error,
-                    );
+                            console.log(`✅ Responsáveis carregados: ${response.data.length} registros`);
+                        }
+                        else
+                        {
+                            console.error("❌ Elemento cmbResponsavel ou instância EJ2 não encontrado");
+                        }
+                    }
+                    else
+                    {
+                        console.error("❌ Resposta inválida para responsáveis:", response);
+                    }
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "carregarResponsaveisMovimentacoes.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                try {
-                    console.error('❌ Erro AJAX ao carregar responsáveis:', {
-                        xhr: xhr,
-                        status: status,
-                        error: error,
-                    });
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao carregar responsáveis',
-                        3000,
-                    );
-                } catch (err) {
-                    Alerta.TratamentoErroComLinha(
-                        'movimentacaopatrimonio.js',
-                        'carregarResponsaveisMovimentacoes.error',
-                        err,
-                    );
-                }
-            },
+            error: function (xhr, status, error)
+            {
+                try
+                {
+                    console.error("❌ Erro AJAX ao carregar responsáveis:", { xhr: xhr, status: status, error: error });
+                    AppToast.show('Vermelho', 'Erro ao carregar responsáveis', 3000);
+                } catch (err)
+                {
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "carregarResponsaveisMovimentacoes.error", err);
+                }
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'carregarResponsaveisMovimentacoes',
-            error,
-        );
-    }
-}
-
-function aplicarFiltrosMovimentacoes() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "carregarResponsaveisMovimentacoes", error);
+    }
+}
+
+function aplicarFiltrosMovimentacoes()
+{
+    try
+    {
         mostrarLoading();
 
-        var drpDataMovimentacao = document.getElementById(
-            'drpDataMovimentacao',
-        );
-        var ddtSetorSecaoOrigem = document.getElementById(
-            'ddtSetorSecaoOrigem',
-        );
-        var ddtSetorSecaoDestino = document.getElementById(
-            'ddtSetorSecaoDestino',
-        );
+        var drpDataMovimentacao = document.getElementById('drpDataMovimentacao');
+        var ddtSetorSecaoOrigem = document.getElementById('ddtSetorSecaoOrigem');
+        var ddtSetorSecaoDestino = document.getElementById('ddtSetorSecaoDestino');
         var cmbResponsavel = document.getElementById('cmbResponsavel');
 
-        var dataInicio = '';
-        var dataFim = '';
+        var dataInicio = "";
+        var dataFim = "";
         var setoresSecoesOrigem = [];
         var setoresSecoesDestino = [];
-        var responsavel = '';
-
-        if (
-            drpDataMovimentacao &&
-            drpDataMovimentacao.ej2_instances &&
-            drpDataMovimentacao.ej2_instances[0]
-        ) {
+        var responsavel = "";
+
+        if (drpDataMovimentacao && drpDataMovimentacao.ej2_instances && drpDataMovimentacao.ej2_instances[0])
+        {
             var dateRange = drpDataMovimentacao.ej2_instances[0];
-            if (dateRange.startDate && dateRange.endDate) {
+            if (dateRange.startDate && dateRange.endDate)
+            {
                 dataInicio = dateRange.startDate.toISOString();
                 dataFim = dateRange.endDate.toISOString();
             }
         }
 
-        if (
-            ddtSetorSecaoOrigem &&
-            ddtSetorSecaoOrigem.ej2_instances &&
-            ddtSetorSecaoOrigem.ej2_instances[0]
-        ) {
-            var valoresSelecionados =
-                ddtSetorSecaoOrigem.ej2_instances[0].value;
-            if (valoresSelecionados && valoresSelecionados.length > 0) {
+        if (ddtSetorSecaoOrigem && ddtSetorSecaoOrigem.ej2_instances && ddtSetorSecaoOrigem.ej2_instances[0])
+        {
+            var valoresSelecionados = ddtSetorSecaoOrigem.ej2_instances[0].value;
+            if (valoresSelecionados && valoresSelecionados.length > 0)
+            {
                 setoresSecoesOrigem = valoresSelecionados;
             }
         }
 
-        if (
-            ddtSetorSecaoDestino &&
-            ddtSetorSecaoDestino.ej2_instances &&
-            ddtSetorSecaoDestino.ej2_instances[0]
-        ) {
-            var valoresSelecionados =
-                ddtSetorSecaoDestino.ej2_instances[0].value;
-            if (valoresSelecionados && valoresSelecionados.length > 0) {
+        if (ddtSetorSecaoDestino && ddtSetorSecaoDestino.ej2_instances && ddtSetorSecaoDestino.ej2_instances[0])
+        {
+            var valoresSelecionados = ddtSetorSecaoDestino.ej2_instances[0].value;
+            if (valoresSelecionados && valoresSelecionados.length > 0)
+            {
                 setoresSecoesDestino = valoresSelecionados;
             }
         }
 
-        if (
-            cmbResponsavel &&
-            cmbResponsavel.ej2_instances &&
-            cmbResponsavel.ej2_instances[0]
-        ) {
-            responsavel = cmbResponsavel.ej2_instances[0].value || '';
-        }
-
-        console.log('Filtros de movimentações aplicados:', {
+        if (cmbResponsavel && cmbResponsavel.ej2_instances && cmbResponsavel.ej2_instances[0])
+        {
+            responsavel = cmbResponsavel.ej2_instances[0].value || "";
+        }
+
+        console.log("Filtros de movimentações aplicados:", {
             dataInicio: dataInicio,
             dataFim: dataFim,
             setoresSecoesOrigem: setoresSecoesOrigem,
             setoresSecoesDestino: setoresSecoesDestino,
-            responsavel: responsavel,
+            responsavel: responsavel
         });
 
-        if ($.fn.DataTable.isDataTable('#tblMovimentacaoPatrimonio')) {
+        if ($.fn.DataTable.isDataTable('#tblMovimentacaoPatrimonio'))
+        {
             var table = $('#tblMovimentacaoPatrimonio').DataTable();
 
-            var origemParam =
-                setoresSecoesOrigem.length > 0
-                    ? setoresSecoesOrigem.join(',')
-                    : '';
-            var destinoParam =
-                setoresSecoesDestino.length > 0
-                    ? setoresSecoesDestino.join(',')
-                    : '';
-
-            table.ajax
-                .url(
-                    `/api/Patrimonio/MovimentacaoPatrimonioGrid?dataInicio=${encodeURIComponent(dataInicio)}&dataFim=${encodeURIComponent(dataFim)}&setorSecaoOrigem=${encodeURIComponent(origemParam)}&setorSecaoDestino=${encodeURIComponent(destinoParam)}&responsavel=${encodeURIComponent(responsavel)}`,
-                )
-                .load(function (json) {
-                    try {
-                        esconderLoading();
-
-                        if (json && json.data && json.data.length === 0) {
-                            AppToast.show(
-                                'Amarelo',
-                                'Nenhuma movimentacao encontrada com os filtros selecionados.',
-                                3000,
-                            );
-                        } else if (json && json.data && json.data.length > 0) {
-                            console.log(
-                                `${json.data.length} movimentacao(oes) encontrada(s)`,
-                            );
-                        }
-                    } catch (error) {
-                        esconderLoading();
-                        Alerta.TratamentoErroComLinha(
-                            'movimentacaopatrimonio.js',
-                            'aplicarFiltrosMovimentacoes.load.callback',
-                            error,
-                        );
-                    }
-                });
-        } else {
+            var origemParam = setoresSecoesOrigem.length > 0 ? setoresSecoesOrigem.join(',') : '';
+            var destinoParam = setoresSecoesDestino.length > 0 ? setoresSecoesDestino.join(',') : '';
+
+            table.ajax.url(
+                `/api/Patrimonio/MovimentacaoPatrimonioGrid?dataInicio=${encodeURIComponent(dataInicio)}&dataFim=${encodeURIComponent(dataFim)}&setorSecaoOrigem=${encodeURIComponent(origemParam)}&setorSecaoDestino=${encodeURIComponent(destinoParam)}&responsavel=${encodeURIComponent(responsavel)}`
+            ).load(function (json)
+            {
+                try
+                {
+                    esconderLoading();
+
+                    if (json && json.data && json.data.length === 0)
+                    {
+                        AppToast.show('Amarelo', 'Nenhuma movimentacao encontrada com os filtros selecionados.', 3000);
+                    }
+                    else if (json && json.data && json.data.length > 0)
+                    {
+                        console.log(`${json.data.length} movimentacao(oes) encontrada(s)`);
+                    }
+                } catch (error)
+                {
+                    esconderLoading();
+                    Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "aplicarFiltrosMovimentacoes.load.callback", error);
+                }
+            });
+        }
+        else
+        {
             esconderLoading();
         }
-    } catch (error) {
+    } catch (error)
+    {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'movimentacaopatrimonio.js',
-            'aplicarFiltrosMovimentacoes',
-            error,
-        );
-    }
-}
+        Alerta.TratamentoErroComLinha("movimentacaopatrimonio.js", "aplicarFiltrosMovimentacoes", error);
+    }
+}
```
