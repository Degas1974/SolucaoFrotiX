# wwwroot/js/cadastros/secao_patrimonial.js

**Mudanca:** GRANDE | **+292** linhas | **-309** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/secao_patrimonial.js
+++ ATUAL: wwwroot/js/cadastros/secao_patrimonial.js
@@ -1,230 +1,220 @@
 var path = window.location.pathname.toLowerCase();
 console.log(path);
 
-if (path == '/secaopatrimonial/index' || path == '/secaopatrimonial') {
-    console.log('Entrou na seção index');
-
-    $(document).ready(function () {
-        try {
+if (path == "/secaopatrimonial/index" || path == "/secaopatrimonial")
+{
+    console.log("Entrou na seção index");
+
+    $(document).ready(function ()
+    {
+        try
+        {
             loadGrid();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'document.ready',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "document.ready", error);
         }
     });
 
-    $(document).on('click', '.btn-delete', function () {
-        try {
-            var id = $(this).data('id');
+    $(document).on("click", ".btn-delete", function ()
+    {
+        try
+        {
+            var id = $(this).data("id");
             console.log(id);
 
             Alerta.Confirmar(
-                'Confirmar Exclusão',
-                'Você tem certeza que deseja apagar esta Seção Patrimonial? Não será possível recuperar os dados eliminados!',
-                'Sim, excluir',
-                'Cancelar',
-            ).then((willDelete) => {
-                try {
-                    if (willDelete) {
+                "Confirmar Exclusão",
+                "Você tem certeza que deseja apagar esta Seção Patrimonial? Não será possível recuperar os dados eliminados!",
+                "Sim, excluir",
+                "Cancelar"
+            ).then((willDelete) =>
+            {
+                try
+                {
+                    if (willDelete)
+                    {
                         $.ajax({
-                            url: '/api/Secao/Delete',
-                            type: 'POST',
-                            contentType: 'application/json',
+                            url: "/api/Secao/Delete",
+                            type: "POST",
+                            contentType: "application/json",
                             data: JSON.stringify(id),
-                            success: function (data) {
-                                try {
-                                    if (data.success) {
-                                        AppToast.show(
-                                            'Verde',
-                                            data.message,
-                                            2000,
-                                        );
+                            success: function (data)
+                            {
+                                try
+                                {
+                                    if (data.success)
+                                    {
+                                        AppToast.show("Verde", data.message, 2000);
                                         dataTable.ajax.reload();
-                                    } else {
-                                        AppToast.show(
-                                            'Vermelho',
-                                            data.message,
-                                            2000,
-                                        );
+                                    } else
+                                    {
+                                        AppToast.show("Vermelho", data.message, 2000);
                                     }
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'secao_patrimonial.js',
-                                        'ajax.Delete.success',
-                                        error,
-                                    );
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.Delete.success", error);
                                 }
                             },
-                            error: function (err) {
-                                try {
+                            error: function (err)
+                            {
+                                try
+                                {
                                     console.error(err);
-                                    Alerta.Erro(
-                                        'Erro ao Excluir',
-                                        'Ocorreu um erro ao tentar excluir a seção patrimonial. Tente novamente.',
-                                        'OK',
-                                    );
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'secao_patrimonial.js',
-                                        'ajax.Delete.error',
-                                        error,
-                                    );
+                                    Alerta.Erro("Erro ao Excluir", "Ocorreu um erro ao tentar excluir a seção patrimonial. Tente novamente.", "OK");
+                                }
+                                catch (error)
+                                {
+                                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.Delete.error", error);
                                 }
                             },
                         });
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'secao_patrimonial.js',
-                        'btn-delete.Confirmar.then',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'btn-delete.click',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "btn-delete.Confirmar.then", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "btn-delete.click", error);
         }
     });
 
-    $(document).on('click', '.updateStatusSecao', function () {
-        try {
-            var url = $(this).data('url');
+    $(document).on("click", ".updateStatusSecao", function ()
+    {
+        try
+        {
+            var url = $(this).data("url");
             var currentElement = $(this);
 
-            $.get(url, function (data) {
-                try {
-                    if (data.success) {
+            $.get(url, function (data)
+            {
+                try
+                {
+                    if (data.success)
+                    {
                         var text;
-                        if (data.type == 1) {
-                            text = 'Inativa';
-                            currentElement
-                                .removeClass('btn-verde')
-                                .addClass('fundo-cinza');
-                        } else {
-                            text = 'Ativa';
-                            currentElement
-                                .removeClass('fundo-cinza')
-                                .addClass('btn-verde');
+                        if (data.type == 1)
+                        {
+                            text = "Inativa";
+                            currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                        } else
+                        {
+                            text = "Ativa";
+                            currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                         }
 
-                        AppToast.show('Verde', data.message, 2000);
+                        AppToast.show("Verde", data.message, 2000);
 
                         currentElement.text(text);
-                    } else {
-                        Alerta.Erro(
-                            'Erro ao Alterar Status',
-                            'Ocorreu um erro ao tentar alterar o status. Tente novamente.',
-                            'OK',
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'secao_patrimonial.js',
-                        'updateStatusSecao.get.callback',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'updateStatusSecao.click',
-                error,
-            );
+                    } else
+                    {
+                        Alerta.Erro("Erro ao Alterar Status", "Ocorreu um erro ao tentar alterar o status. Tente novamente.", "OK");
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "updateStatusSecao.get.callback", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "updateStatusSecao.click", error);
         }
     });
 
-    function loadGrid() {
-        try {
-            console.log('Entrou na loadGrid secao');
-            dataTable = $('#tblSecao').DataTable({
+    function loadGrid()
+    {
+        try
+        {
+            console.log("Entrou na loadGrid secao");
+            dataTable = $("#tblSecao").DataTable({
                 columnDefs: [
                     {
                         targets: 0,
-                        className: 'text-left',
-                        width: '15%',
+                        className: "text-left",
+                        width: "15%",
                     },
                     {
                         targets: 1,
-                        className: 'text-left',
-                        width: '15%',
+                        className: "text-left",
+                        width: "15%",
                     },
                     {
                         targets: 2,
-                        className: 'text-center',
-                        width: '10%',
+                        className: "text-center",
+                        width: "10%",
                     },
                     {
                         targets: 3,
-                        className: 'text-center',
-                        width: '10%',
+                        className: "text-center",
+                        width: "10%",
                     },
                 ],
 
                 responsive: true,
                 ajax: {
-                    url: '/api/secao/ListaSecoes',
-                    type: 'GET',
-                    datatype: 'json',
-                    error: function (xhr, status, error) {
-                        try {
-                            console.error('Erro ao carregar os dados: ', error);
-                            Alerta.Erro(
-                                'Erro ao Carregar Dados',
-                                'Não foi possível carregar a lista de seções patrimoniais.',
-                                'OK',
-                            );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'secao_patrimonial.js',
-                                'ajax.GetGrid.error',
-                                error,
-                            );
+                    url: "/api/secao/ListaSecoes",
+                    type: "GET",
+                    datatype: "json",
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
+                            console.error("Erro ao carregar os dados: ", error);
+                            Alerta.Erro("Erro ao Carregar Dados", "Não foi possível carregar a lista de seções patrimoniais.", "OK");
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.GetGrid.error", error);
                         }
                     },
                 },
                 columns: [
-                    { data: 'nomeSecao' },
-                    { data: 'nomeSetor' },
-                    {
-                        data: 'status',
-                        render: function (data, type, row, meta) {
-                            try {
-                                if (data) {
+                    { data: "nomeSecao" },
+                    { data: "nomeSetor" },
+                    {
+                        data: "status",
+                        render: function (data, type, row, meta)
+                        {
+                            try
+                            {
+                                if (data)
+                                {
                                     return (
                                         '<a href="javascript:void(0)" class="updateStatusSecao btn btn-verde btn-xs text-white" data-url="/api/Secao/updateStatusSecao?Id=' +
                                         row.secaoId +
                                         '" data-ejtip="Seção ativa - clique para inativar">Ativa</a>'
                                     );
-                                } else {
+                                } else
+                                {
                                     return (
                                         '<a href="javascript:void(0)" class="updateStatusSecao btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Secao/updateStatusSecao?Id=' +
                                         row.secaoId +
                                         '" data-ejtip="Seção inativa - clique para ativar">Inativa</a>'
                                     );
                                 }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'secao_patrimonial.js',
-                                    'DataTable.render.status',
-                                    error,
-                                );
-                                return '';
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DataTable.render.status", error);
+                                return "";
                             }
                         },
-                        width: '6%',
-                    },
-                    {
-                        data: 'secaoId',
-                        render: function (data) {
-                            try {
+                        width: "6%",
+                    },
+                    {
+                        data: "secaoId",
+                        render: function (data)
+                        {
+                            try
+                            {
                                 return `<div class="text-center">
                                     <a href="/SecaoPatrimonial/Upsert?id=${data}"
                                        class="btn btn-azul text-white"
@@ -239,221 +229,214 @@
                                         <i class="far fa-trash-alt"></i>
                                     </a>
                                 </div>`;
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'secao_patrimonial.js',
-                                    'DataTable.render.acoes',
-                                    error,
-                                );
-                                return '';
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DataTable.render.acoes", error);
+                                return "";
                             }
                         },
                     },
                 ],
 
                 language: {
-                    url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                    emptyTable: 'Sem Dados para Exibição',
+                    url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                    emptyTable: "Sem Dados para Exibição",
                 },
-                width: '100%',
-            });
-            console.log('Saiu da LoadGrid');
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'loadGrid',
-                error,
-            );
+                width: "100%",
+            });
+            console.log("Saiu da LoadGrid");
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadGrid", error);
         }
     }
-} else if (path === '/secaopatrimonial/upsert') {
-    console.log('Upsert seção');
-    $(document).ready(function () {
-        try {
+} else if (path === "/secaopatrimonial/upsert")
+{
+    console.log("Upsert seção");
+    $(document).ready(function ()
+    {
+        try
+        {
             loadListaSetores();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'document.ready.upsert',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "document.ready.upsert", error);
         }
     });
 
-    function validaNome() {
-        try {
-            $(FormsSecao).on('submit', function (event) {
-                try {
-
-                    var nomeSecao =
-                        document.getElementsByName('SecaoObj.NomeSecao')[0]
-                            .value;
-
-                    if (nomeSecao === '') {
+    function validaNome()
+    {
+        try
+        {
+            $(FormsSecao).on("submit", function (event)
+            {
+                try
+                {
+
+                    var nomeSecao = document.getElementsByName("SecaoObj.NomeSecao")[0].value;
+
+                    if (nomeSecao === "")
+                    {
                         event.preventDefault();
                         Alerta.Erro(
-                            'Erro no Nome da Seção',
-                            'O nome da seção não pode estar em branco!',
-                            'OK',
+                            "Erro no Nome da Seção",
+                            "O nome da seção não pode estar em branco!",
+                            "OK"
                         );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'secao_patrimonial.js',
-                        'validaNome.submit',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'validaNome',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaNome.submit", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaNome", error);
         }
     }
 
-    function validaSetor() {
-        try {
-            $(FormsSecao).on('submit', function (event) {
-                try {
-
-                    var setorId =
-                        document.getElementById('cmbSetor').ej2_instances[0]
-                            .value;
-
-                    if (setorId === '' || setorId == null) {
+    function validaSetor()
+    {
+        try
+        {
+            $(FormsSecao).on("submit", function (event)
+            {
+                try
+                {
+
+                    var setorId = document.getElementById("cmbSetor").ej2_instances[0].value;
+
+                    if (setorId === "" || setorId == null)
+                    {
                         event.preventDefault();
                         Alerta.Erro(
-                            'Erro no Setor',
-                            'O Setor da seção não pode estar em branco!',
-                            'OK',
+                            "Erro no Setor",
+                            "O Setor da seção não pode estar em branco!",
+                            "OK"
                         );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'secao_patrimonial.js',
-                        'validaSetor.submit',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'validaSetor',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaSetor.submit", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "validaSetor", error);
         }
     }
 
-    function loadListaSetores() {
-        try {
-            var comboBox = document.getElementById('cmbSetor').ej2_instances[0];
-            var secaoId = document.getElementsByName('SecaoObj.SecaoId');
+    function loadListaSetores()
+    {
+        try
+        {
+            var comboBox = document.getElementById("cmbSetor").ej2_instances[0];
+            var secaoId = document.getElementsByName("SecaoObj.SecaoId");
             var setorId;
-            if (secaoId.length <= 0) {
-                comboBox.value = '';
+            if (secaoId.length <= 0)
+            {
+                comboBox.value = "";
                 console.log(secaoId > 0);
-            } else {
-                setorId =
-                    document.getElementById('cmbSetor').ej2_instances[0].value;
+            } else
+            {
+                setorId = document.getElementById("cmbSetor").ej2_instances[0].value;
             }
 
             $.ajax({
-                type: 'get',
-                url: '/api/Setor/ListaSetoresCombo',
-                datatype: 'json',
-                success: function (res) {
-                    try {
-                        if (res != null && res.data.length) {
-
-                            comboBox.fields = { text: 'text', value: 'value' };
+                type: "get",
+                url: "/api/Setor/ListaSetoresCombo",
+                datatype: "json",
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res != null && res.data.length)
+                        {
+
+                            comboBox.fields = { text: "text", value: "value" };
 
                             comboBox.dataSource = res.data;
 
-                            if (setorId) {
-
-                                var item = comboBox.dataSource.find((item) => {
-                                    try {
-                                        return (
-                                            item.value.toLowerCase() ==
-                                            setorId.toString()
-                                        );
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'secao_patrimonial.js',
-                                            'loadListaSetores.find',
-                                            error,
-                                        );
+                            if (setorId)
+                            {
+
+                                var item = comboBox.dataSource.find((item) =>
+                                {
+                                    try
+                                    {
+                                        return item.value.toLowerCase() == setorId.toString();
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadListaSetores.find", error);
                                         return false;
                                     }
                                 });
-                                console.log('item: ', item);
-                                if (item) {
+                                console.log("item: ", item);
+                                if (item)
+                                {
                                     comboBox.value = item.value;
                                 }
                             }
-                        } else {
-                            console.log('Nenhum setor encontrado.');
+                        } else
+                        {
+                            console.log("Nenhum setor encontrado.");
                         }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'secao_patrimonial.js',
-                            'ajax.ListaSetores.success',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.ListaSetores.success", error);
                     }
                 },
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar setores: ', error);
-                        Alerta.Erro(
-                            'Erro ao Carregar Setores',
-                            'Não foi possível carregar a lista de setores. Tente novamente.',
-                            'OK',
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'secao_patrimonial.js',
-                            'ajax.ListaSetores.error',
-                            error,
-                        );
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar setores: ", error);
+                        Alerta.Erro("Erro ao Carregar Setores", "Não foi possível carregar a lista de setores. Tente novamente.", "OK");
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("secao_patrimonial.js", "ajax.ListaSetores.error", error);
                     }
                 },
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'loadListaSetores',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "loadListaSetores", error);
         }
     }
 
-    document.addEventListener('DOMContentLoaded', function () {
-        try {
-
-            const infoDiv = document.getElementById('divSecaoIdEmpty');
+    document.addEventListener("DOMContentLoaded", function ()
+    {
+        try
+        {
+
+            const infoDiv = document.getElementById("divSecaoIdEmpty");
 
             const secaoId = infoDiv.dataset.secaoid;
-            console.log('Guid da Seção:', secaoId);
-
-            const isEmptyGuid =
-                secaoId === '00000000-0000-0000-0000-000000000000';
-
-            const checkbox = document.getElementById('chkStatus');
-
-            if (isEmptyGuid && checkbox) {
+            console.log("Guid da Seção:", secaoId);
+
+            const isEmptyGuid = secaoId === "00000000-0000-0000-0000-000000000000";
+
+            const checkbox = document.getElementById("chkStatus");
+
+            if (isEmptyGuid && checkbox)
+            {
                 checkbox.checked = true;
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'secao_patrimonial.js',
-                'DOMContentLoaded',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("secao_patrimonial.js", "DOMContentLoaded", error);
         }
     });
 }
```
