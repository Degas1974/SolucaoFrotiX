# wwwroot/js/cadastros/usuario_001.js

**Mudanca:** GRANDE | **+166** linhas | **-206** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/usuario_001.js
+++ ATUAL: wwwroot/js/cadastros/usuario_001.js
@@ -1,386 +1,298 @@
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
-                var Id = $(this).data('id');
+        $(document).on("click", ".btn-delete", function () {
+            try
+            {
+                var Id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este Usuário?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar este Usuário?",
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
                             var dataToPost = JSON.stringify({ Id: Id });
-
-                            var url = '/api/Usuario/Delete';
-
+                            var url = "/api/Usuario/Delete";
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
-                                            'usuario_001.js',
-                                            'btn-delete.ajax.success',
+                                            "usuario_<num>.js",
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
-                                            'Ocorreu um erro ao excluir o usuário. Tente novamente.',
-                                        );
-                                    } catch (error) {
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'usuario_001.js',
-                                            'btn-delete.ajax.error',
+                                            "usuario_<num>.js",
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
-                            'usuario_001.js',
-                            'btn-delete.confirm.then',
+                            "usuario_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'usuario_001.js',
-                    'btn-delete.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusUsuario', function () {
-            try {
-
-                var url = $(this).data('url');
-
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.on#2", error);
+            }
+        });
+
+        $(document).on("click", ".updateStatusUsuario", function () {
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
-
-                            var iconHtml =
-                                '<i class="fa-solid fa-circle-check me-1"></i>';
+                            AppToast.show('Verde', "Status alterado com sucesso!");
+                            var text = "Ativo";
+                            var iconHtml = '<i class="fa-solid fa-circle-check me-1"></i>';
 
                             if (data.type == 1) {
-                                text = 'Inativo';
-                                iconHtml =
-                                    '<i class="fa-solid fa-circle-xmark me-1"></i>';
-
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
+                                text = "Inativo";
+                                iconHtml = '<i class="fa-solid fa-circle-xmark me-1"></i>';
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                             } else {
-
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                             }
 
                             currentElement.html(iconHtml + text);
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
-                            'usuario_001.js',
-                            'updateStatus.get.callback',
+                            "usuario_<num>.js",
+                            "callback@$.get#1",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'usuario_001.js',
-                    'updateStatus.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateCargaPatrimonial', function () {
-            try {
-
-                var url = $(this).data('url');
-
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.on#2", error);
+            }
+        });
+
+        $(document).on("click", ".updateCargaPatrimonial", function () {
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
-                                'Carga Patrimonial alterada com sucesso!',
-                            );
-
-                            var text = 'Sim';
-
-                            var iconHtml =
-                                '<i class="fa-duotone fa-badge-check me-1"></i>';
+                            AppToast.show('Verde', "Carga Patrimonial alterada com sucesso!");
+                            var text = "Sim";
+                            var iconHtml = '<i class="fa-duotone fa-badge-check me-1"></i>';
 
                             if (data.type == 1) {
-                                text = 'Não';
-                                iconHtml =
-                                    '<i class="fa-duotone fa-circle-xmark me-1"></i>';
-
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
+                                text = "Não";
+                                iconHtml = '<i class="fa-duotone fa-circle-xmark me-1"></i>';
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                             } else {
-
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                             }
 
                             currentElement.html(iconHtml + text);
-                        } else {
-                            Alerta.Erro(
-                                'Erro',
-                                'Não foi possível alterar a carga patrimonial. Tente novamente.',
-                            );
-                        }
-                    } catch (error) {
+                        } else alert("Something went wrong!");
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'usuario_001.js',
-                            'updateCargaPatrimonial.get.callback',
+                            "usuario_<num>.js",
+                            "callback@$.get#1",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'usuario_001.js',
-                    'updateCargaPatrimonial.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.btn-modal-senha', function (e) {
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.on#2", error);
+            }
+        });
+
+        $(document).on("click", ".btn-modal-senha", function (e) {
             try {
                 e.preventDefault();
-
-                var usuarioId = $(this).data('id');
-
+                var usuarioId = $(this).data("id");
                 $('#txtUsuarioIdSenha').val(usuarioId);
-
                 var modalElement = document.getElementById('modalSenha');
-
                 if (modalElement) {
-
                     var modal = new bootstrap.Modal(modalElement);
                     modal.show();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'usuario_001.js',
-                    'btn-modal-senha.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.btn-modal-acesso', function (e) {
+                Alerta.TratamentoErroComLinha("usuario_001.js", "btn-modal-senha.click", error);
+            }
+        });
+
+        $(document).on("click", ".btn-modal-acesso", function (e) {
             try {
                 e.preventDefault();
-
-                var usuarioId = $(this).data('id');
-
+                var usuarioId = $(this).data("id");
                 $('#txtUsuarioIdAcesso').val(usuarioId);
-
-                var modalElement = document.getElementById(
-                    'modalControleAcesso',
-                );
-
+                var modalElement = document.getElementById('modalControleAcesso');
                 if (modalElement) {
-
                     var modal = new bootstrap.Modal(modalElement);
                     modal.show();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'usuario_001.js',
-                    'btn-modal-acesso.click',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'usuario_001.js',
-            'document.ready',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("usuario_001.js", "btn-modal-acesso.click", error);
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("usuario_<num>.js", "callback@$.ready#0", error);
     }
 });
 
 function loadList() {
-    try {
-
-        dataTable = $('#tblUsuario').DataTable({
-
+    try
+    {
+        dataTable = $("#tblUsuario").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-left',
-                    width: '15%',
+                    className: "text-left",
+                    width: "15%",
                 },
                 {
                     targets: 1,
-                    className: 'text-center',
-                    width: '6%',
+                    className: "text-center",
+                    width: "6%",
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
-                url: '/api/usuario',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/usuario",
+                type: "GET",
+                datatype: "json",
             },
-
             columns: [
-
-                { data: 'nomeCompleto' },
-
-                { data: 'ponto' },
-
-                {
-                    data: 'detentorCargaPatrimonial',
-
+                { data: "nomeCompleto" },
+                { data: "ponto" },
+                {
+                    data: "detentorCargaPatrimonial",
                     render: function (data, type, row, meta) {
-                        try {
-                            if (data) {
-
+                        try
+                        {
+                            if (data)
                                 return (
                                     '<a href="javascript:void" class="updateCargaPatrimonial btn btn-verde btn-xs text-white" data-url="/api/Usuario/updateCargaPatrimonial?Id=' +
                                     row.usuarioId +
                                     '">Sim</a>'
                                 );
-                            } else {
-
+                            else
                                 return (
                                     '<a href="javascript:void" class="updateCargaPatrimonial btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Usuario/updateCargaPatrimonial?Id=' +
                                     row.usuarioId +
                                     '">Não</a>'
                                 );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'usuario_001.js',
-                                'loadList.render.cargaPatrimonial',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("usuario_<num>.js", "render", error);
                         }
                     },
                 },
-
-                {
-                    data: 'status',
-
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
                                     '<a href="javascript:void" class="updateStatusUsuario btn btn-verde btn-xs text-white" data-url="/api/Usuario/updateStatusUsuario?Id=' +
                                     row.usuarioId +
                                     '">Ativo</a>'
                                 );
-                            } else {
-
+                            else
                                 return (
                                     '<a href="javascript:void" class="updateStatusUsuario btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Usuario/updateStatusUsuario?Id=' +
                                     row.usuarioId +
                                     '">Inativo</a>'
                                 );
-                            }
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'usuario_001.js',
-                                'loadList.render.status',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("usuario_<num>.js", "render", error);
                         }
                     },
                 },
-
-                {
-                    data: 'usuarioId',
-
+                {
+                    data: "usuarioId",
                     render: function (data, type, row) {
-                        try {
+                        try
+                        {
 
                             var btnEditar = `<a href="/Usuarios/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" aria-label="Editar o Usuário!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
                                 <i class="far fa-edit"></i>
@@ -388,12 +300,10 @@
 
                             var btnExcluir = '';
                             if (row.podeExcluir) {
-
                                 btnExcluir = `<a class="btn-delete btn btn-vinho btn-xs text-white" aria-label="Excluir o Usuário!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>`;
                             } else {
-
                                 btnExcluir = `<button class="btn btn-secondary btn-xs text-white"
                                     data-bs-toggle="tooltip"
                                     data-bs-custom-class="tooltip-ftx-azul"
@@ -418,34 +328,31 @@
                                 ${btnSenha}
                                 ${btnAcesso}
                             </div>`;
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'usuario_001.js',
-                                'render',
-                                error,
-                            );
+                        }
+                        catch (error)
+                        {
+                            Alerta.TratamentoErroComLinha("usuario_001.js", "render", error);
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
-
-            drawCallback: function () {
-
-                var tooltipTriggerList = [].slice.call(
-                    document.querySelectorAll('[data-bs-toggle="tooltip"]'),
-                );
+            width: "100%",
+            drawCallback: function() {
+
+                var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
                 tooltipTriggerList.map(function (tooltipTriggerEl) {
                     return new bootstrap.Tooltip(tooltipTriggerEl);
                 });
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('usuario_001.js', 'loadList', error);
+            }
+        });
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("usuario_<num>.js", "loadList", error);
     }
 }
```
