# wwwroot/js/cadastros/setor_patrimonial.js

**Mudanca:** GRANDE | **+273** linhas | **-294** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/setor_patrimonial.js
+++ ATUAL: wwwroot/js/cadastros/setor_patrimonial.js
@@ -1,258 +1,219 @@
 var path = window.location.pathname.toLowerCase();
 console.log(path);
 
-if (path == '/setorpatrimonial/index' || path == '/setorpatrimonial') {
-
+if (path == "/setorpatrimonial/index" || path == "/setorpatrimonial")
+{
     loadGrid();
-    console.log('Entrou na listagem de setores');
-
-    $(document).ready(function () {
-        try {
-
-            $(document).on('click', '.btn-delete', function () {
-                try {
-
-                    var id = $(this).data('id');
+    console.log("Entrou na listagem de setores");
+
+    $(document).ready(function ()
+    {
+        try
+        {
+            $(document).on("click", ".btn-delete", function ()
+            {
+                try
+                {
+                    var id = $(this).data("id");
                     console.log(id);
 
                     Alerta.Confirmar(
-                        'Confirmar Exclusão',
-                        'Você tem certeza que deseja apagar este setor? Não será possível recuperar os dados eliminados!',
-                        'Sim, excluir',
-                        'Cancelar',
-                    ).then((willDelete) => {
-                        try {
-
-                            if (willDelete) {
-
+                        "Confirmar Exclusão",
+                        "Você tem certeza que deseja apagar este setor? Não será possível recuperar os dados eliminados!",
+                        "Sim, excluir",
+                        "Cancelar"
+                    ).then((willDelete) =>
+                    {
+                        try
+                        {
+                            if (willDelete)
+                            {
                                 $.ajax({
-                                    url: '/api/Setor/Delete',
-                                    type: 'POST',
-                                    contentType: 'application/json',
+                                    url: "/api/Setor/Delete",
+                                    type: "POST",
+                                    contentType: "application/json",
                                     data: JSON.stringify(id),
-
-                                    success: function (data) {
-                                        try {
-                                            if (data.success) {
-
-                                                AppToast.show(
-                                                    'Verde',
-                                                    data.message,
-                                                    2000,
-                                                );
-
+                                    success: function (data)
+                                    {
+                                        try
+                                        {
+                                            if (data.success)
+                                            {
+                                                AppToast.show("Verde", data.message, 2000);
                                                 dataTable.ajax.reload();
-                                            } else {
-
-                                                AppToast.show(
-                                                    'Vermelho',
-                                                    data.message,
-                                                    2000,
-                                                );
+                                            } else
+                                            {
+                                                AppToast.show("Vermelho", data.message, 2000);
                                             }
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'setor_patrimonial.js',
-                                                'btn-delete.ajax.success',
-                                                error,
-                                            );
+                                        }
+                                        catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "ajax.Delete.success", error);
                                         }
                                     },
-
-                                    error: function (err) {
-                                        try {
+                                    error: function (err)
+                                    {
+                                        try
+                                        {
                                             console.error(err);
-                                            Alerta.Erro(
-                                                'Erro ao Excluir',
-                                                'Ocorreu um erro ao tentar excluir o setor patrimonial. Tente novamente.',
-                                                'OK',
-                                            );
-                                        } catch (error) {
-                                            Alerta.TratamentoErroComLinha(
-                                                'setor_patrimonial.js',
-                                                'btn-delete.ajax.error',
-                                                error,
-                                            );
+                                            Alerta.Erro("Erro ao Excluir", "Ocorreu um erro ao tentar excluir o setor patrimonial. Tente novamente.", "OK");
+                                        }
+                                        catch (error)
+                                        {
+                                            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "ajax.Delete.error", error);
                                         }
                                     },
                                 });
                             }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'setor_patrimonial.js',
-                                'btn-delete.confirm.then',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "btn-delete.Confirmar.then", error);
                         }
                     });
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'setor_patrimonial.js',
-                        'btn-delete.click',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'document.ready',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "btn-delete.click", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "document.ready", error);
         }
     });
 
-    $(document).on('click', '.updateStatusSetor', function () {
-        try {
-
-            var url = $(this).data('url');
-
+    $(document).on("click", ".updateStatusSetor", function ()
+    {
+        try
+        {
+            var url = $(this).data("url");
             var currentElement = $(this);
 
-            $.get(url, function (data) {
-                try {
-                    if (data.success) {
-
+            $.get(url, function (data)
+            {
+                try
+                {
+                    if (data.success)
+                    {
                         var text;
-
-                        if (data.type == 1) {
-                            text = 'Inativo';
-
-                            currentElement
-                                .removeClass('btn-verde')
-                                .addClass('fundo-cinza');
-                        } else {
-                            text = 'Ativo';
-
-                            currentElement
-                                .removeClass('fundo-cinza')
-                                .addClass('btn-verde');
-                        }
-
-                        AppToast.show('Verde', data.message, 2000);
+                        if (data.type == 1)
+                        {
+                            text = "Inativo";
+                            currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                        } else
+                        {
+                            text = "Ativo";
+                            currentElement.removeClass("fundo-cinza").addClass("btn-verde");
+                        }
+
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
-                        'setor_patrimonial.js',
-                        'updateStatus.get.callback',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'updateStatus.click',
-                error,
-            );
+                    } else
+                    {
+                        Alerta.Erro("Erro ao Alterar Status", "Ocorreu um erro ao tentar alterar o status. Tente novamente.", "OK");
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "updateStatusSetor.get.callback", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "updateStatusSetor.click", error);
         }
     });
 
-    function loadGrid() {
-        try {
-            console.log('Entrou na loadGrid setor');
-
-            dataTable = $('#tblSetor').DataTable({
-
+    function loadGrid()
+    {
+        try
+        {
+            console.log("Entrou na loadGrid setor");
+            dataTable = $("#tblSetor").DataTable({
                 columnDefs: [
                     {
                         targets: 0,
-                        className: 'text-left',
-                        width: '25%',
+                        className: "text-left",
+                        width: "25%",
                     },
                     {
                         targets: 1,
-                        className: 'text-left',
-                        width: '35%',
+                        className: "text-left",
+                        width: "35%",
                     },
                     {
                         targets: 2,
-                        className: 'text-center',
-                        width: '15%',
+                        className: "text-center",
+                        width: "15%",
                     },
                     {
                         targets: 3,
-                        className: 'text-center',
-                        width: '15%',
+                        className: "text-center",
+                        width: "15%",
                     },
                 ],
 
                 responsive: true,
-
                 ajax: {
-                    url: '/api/setor/ListaSetores',
-                    type: 'GET',
-                    datatype: 'json',
-
-                    error: function (xhr, status, error) {
-                        try {
-                            console.error('Erro ao carregar os dados: ', error);
-                            Alerta.Erro(
-                                'Erro ao Carregar Dados',
-                                'Não foi possível carregar a lista de setores patrimoniais.',
-                                'OK',
-                            );
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'setor_patrimonial.js',
-                                'loadGrid.ajax.error',
-                                error,
-                            );
+                    url: "/api/setor/ListaSetores",
+                    type: "GET",
+                    datatype: "json",
+                    error: function (xhr, status, error)
+                    {
+                        try
+                        {
+                            console.error("Erro ao carregar os dados: ", error);
+                            Alerta.Erro("Erro ao Carregar Dados", "Não foi possível carregar a lista de setores patrimoniais.", "OK");
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "ajax.GetGrid.error", error);
                         }
                     },
                 },
-
                 columns: [
-
-                    { data: 'nomeSetor' },
-
-                    { data: 'nomeCompleto' },
-
-                    {
-                        data: 'status',
-
-                        render: function (data, type, row, meta) {
-                            try {
-                                if (data) {
-
+                    { data: "nomeSetor" },
+                    { data: "nomeCompleto" },
+                    {
+                        data: "status",
+                        render: function (data, type, row, meta)
+                        {
+                            try
+                            {
+                                if (data)
+                                {
                                     return (
                                         '<a href="javascript:void(0)" class="updateStatusSetor btn btn-verde btn-xs text-white" data-url="/api/Setor/updateStatusSetor?Id=' +
                                         row.setorId +
                                         '" data-ejtip="Setor ativo - clique para inativar">Ativo</a>'
                                     );
-                                } else {
-
+                                } else
+                                {
                                     return (
                                         '<a href="javascript:void(0)" class="updateStatusSetor btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Setor/updateStatusSetor?Id=' +
                                         row.setorId +
                                         '" data-ejtip="Setor inativo - clique para ativar">Inativo</a>'
                                     );
                                 }
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'setor_patrimonial.js',
-                                    'loadGrid.render.status',
-                                    error,
-                                );
-                                return '';
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "DataTable.render.status", error);
+                                return "";
                             }
                         },
-                        width: '6%',
-                    },
-
-                    {
-                        data: 'setorId',
-
-                        render: function (data) {
-                            try {
+                        width: "6%",
+                    },
+                    {
+                        data: "setorId",
+                        render: function (data)
+                        {
+                            try
+                            {
                                 return `<div class="text-center">
                                     <a href="/Setorpatrimonial/Upsert?id=${data}"
                                        class="btn btn-azul text-white"
@@ -267,199 +228,183 @@
                                         <i class="far fa-trash-alt"></i>
                                     </a>
                                 </div>`;
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'setor_patrimonial.js',
-                                    'loadGrid.render.actions',
-                                    error,
-                                );
-                                return '';
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "DataTable.render.acoes", error);
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
-                'setor_patrimonial.js',
-                'loadGrid',
-                error,
-            );
+                width: "100%",
+            });
+            console.log("Saiu da LoadGrid");
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "loadGrid", error);
         }
     }
-} else if (path === '/setorpatrimonial/upsert') {
-
-    console.log('Entrou no setor upsert');
-
-    document.addEventListener('DOMContentLoaded', function () {
-        try {
-
+} else if (path === "/setorpatrimonial/upsert")
+{
+    console.log("Entrou no setor upsert");
+
+    document.addEventListener("DOMContentLoaded", function ()
+    {
+        try
+        {
             loadListaUsuarios();
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'upsert.DOMContentLoaded',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "DOMContentLoaded.loadListaUsuarios", error);
         }
     });
 
-    function validaNome() {
-        try {
-            $(FormsSetor).on('submit', function (event) {
-                try {
-
-                    var nomeSetor =
-                        document.getElementsByName('SetorObj.NomeSetor')[0]
-                            .value;
-
-                    if (nomeSetor === '') {
+    function validaNome()
+    {
+        try
+        {
+            $(FormsSetor).on("submit", function (event)
+            {
+                try
+                {
+
+                    var nomeSetor = document.getElementsByName("SetorObj.NomeSetor")[0].value;
+
+                    if (nomeSetor === "")
+                    {
                         event.preventDefault();
                         Alerta.Erro(
-                            'Erro no Nome do Setor',
-                            'O nome do setor não pode estar em branco!',
-                            'OK',
+                            "Erro no Nome do Setor",
+                            "O nome do setor não pode estar em branco!",
+                            "OK"
                         );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'setor_patrimonial.js',
-                        'validaNome.submit',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'validaNome',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "validaNome.submit", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "validaNome", error);
         }
     }
 
-    function validaDetentor() {
-        try {
-            $(FormsSetor).on('submit', function (event) {
-                try {
-
-                    var detentorId = document.getElementsByName(
-                        'SetorObj.DetentorId',
-                    )[0];
-
-                    if (detentorId === '' || detentorId == null) {
+    function validaDetentor()
+    {
+        try
+        {
+            $(FormsSetor).on("submit", function (event)
+            {
+                try
+                {
+
+                    var detentorId = document.getElementsByName("SetorObj.DetentorId")[0];
+
+                    if (detentorId === "" || detentorId == null)
+                    {
                         event.preventDefault();
                         Alerta.Erro(
-                            'Erro no Detentor',
-                            'O detentor da seção não pode estar em branco!',
-                            'OK',
+                            "Erro no Detentor",
+                            "O detentor da seção não pode estar em branco!",
+                            "OK"
                         );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'setor_patrimonial.js',
-                        'validaDetentor.submit',
-                        error,
-                    );
-                }
-            });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'validaDetentor',
-                error,
-            );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "validaDetentor.submit", error);
+                }
+            });
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "validaDetentor", error);
         }
     }
 
-    function loadListaUsuarios() {
-        try {
-
-            var comboBox =
-                document.getElementById('cmbDetentores').ej2_instances[0];
+    function loadListaUsuarios()
+    {
+        try
+        {
+            var comboBox = document.getElementById("cmbDetentores").ej2_instances[0];
 
             $.ajax({
-                type: 'get',
-                url: '/api/usuario/listaUsuariosDetentores',
-                dataType: 'json',
-
-                success: function (res) {
-                    try {
-                        if (res != null && res.data.length) {
+                type: "get",
+                url: "/api/usuario/listaUsuariosDetentores",
+                dataType: "json",
+                success: function (res)
+                {
+                    try
+                    {
+                        if (res != null && res.data.length)
+                        {
 
                             comboBox.dataSource = res.data;
 
-                            comboBox.fields = {
-                                text: 'nomeCompleto',
-                                value: 'usuarioId',
-                            };
-                        } else {
-                            console.log('Nenhum setor encontrado.');
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'setor_patrimonial.js',
-                            'loadListaUsuarios.success',
-                            error,
-                        );
+                            comboBox.fields = { text: "nomeCompleto", value: "usuarioId" };
+                        } else
+                        {
+                            console.log("Nenhum setor encontrado.");
+                        }
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "ajax.listaUsuariosDetentores.success", error);
                     }
                 },
-
-                error: function (error) {
-                    try {
-                        console.error('Erro ao carregar detentores: ', error);
-                        Alerta.Erro(
-                            'Erro ao Carregar Detentores',
-                            'Não foi possível carregar a lista de detentores. Tente novamente.',
-                            'OK',
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'setor_patrimonial.js',
-                            'loadListaUsuarios.error',
-                            error,
-                        );
+                error: function (error)
+                {
+                    try
+                    {
+                        console.error("Erro ao carregar detentores: ", error);
+                        Alerta.Erro("Erro ao Carregar Detentores", "Não foi possível carregar a lista de detentores. Tente novamente.", "OK");
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "ajax.listaUsuariosDetentores.error", error);
                     }
                 },
             });
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'loadListaUsuarios',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "loadListaUsuarios", error);
         }
     }
 
-    document.addEventListener('DOMContentLoaded', function () {
-        try {
-
-            const infoDiv = document.getElementById('divSetorIdEmpty');
+    document.addEventListener("DOMContentLoaded", function ()
+    {
+        try
+        {
+
+            const infoDiv = document.getElementById("divSetorIdEmpty");
 
             const setorId = infoDiv.dataset.setorid;
-            console.log('Guid do Setor:', setorId);
-
-            const isEmptyGuid =
-                setorId === '00000000-0000-0000-0000-000000000000';
-
-            const checkbox = document.getElementById('chkStatus');
-
-            if (isEmptyGuid && checkbox) {
+            console.log("Guid do Setor:", setorId);
+
+            const isEmptyGuid = setorId === "00000000-0000-0000-0000-000000000000";
+
+            const checkbox = document.getElementById("chkStatus");
+
+            if (isEmptyGuid && checkbox)
+            {
                 checkbox.checked = true;
             }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha(
-                'setor_patrimonial.js',
-                'upsert.initCheckbox',
-                error,
-            );
+        }
+        catch (error)
+        {
+            Alerta.TratamentoErroComLinha("setor_patrimonial_001.js", "DOMContentLoaded.checkbox", error);
         }
     });
 }
```
