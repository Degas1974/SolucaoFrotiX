# wwwroot/js/cadastros/lavador.js

**Mudanca:** GRANDE | **+61** linhas | **-135** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/lavador.js
+++ ATUAL: wwwroot/js/cadastros/lavador.js
@@ -4,191 +4,126 @@
     try {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
+        $(document).on("click", ".btn-delete", function () {
             try {
-                var id = $(this).data('id');
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Confirmar Exclusão',
-                    'Você tem certeza que deseja apagar este lavador?',
-                    'Sim, excluir',
-                    'Cancelar',
+                    "Confirmar Exclusão",
+                    "Você tem certeza que deseja apagar este lavador?",
+                    "Sim, excluir",
+                    "Cancelar"
                 ).then((willDelete) => {
                     try {
                         if (willDelete) {
                             var dataToPost = JSON.stringify({ LavadorId: id });
                             $.ajax({
-                                url: '/api/Lavador/Delete',
-                                type: 'POST',
+                                url: "/api/Lavador/Delete",
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
                                     try {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                                2000,
-                                            );
+                                            AppToast.show("Verde", data.message, 2000);
                                             dataTable.ajax.reload();
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                                3000,
-                                            );
+                                            AppToast.show("Vermelho", data.message, 3000);
                                         }
                                     } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'lavador.js',
-                                            'Delete.success',
-                                            error,
-                                        );
+                                        Alerta.TratamentoErroComLinha("lavador.js", "Delete.success", error);
                                     }
                                 },
                                 error: function (err) {
                                     try {
-                                        console.error('Erro ao excluir:', err);
-                                        AppToast.show(
-                                            'Vermelho',
-                                            'Erro ao excluir lavador',
-                                            3000,
-                                        );
+                                        console.error("Erro ao excluir:", err);
+                                        AppToast.show("Vermelho", "Erro ao excluir lavador", 3000);
                                     } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'lavador.js',
-                                            'Delete.error',
-                                            error,
-                                        );
+                                        Alerta.TratamentoErroComLinha("lavador.js", "Delete.error", error);
                                     }
-                                },
+                                }
                             });
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'lavador.js',
-                            'Delete.confirmar',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("lavador.js", "Delete.confirmar", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'lavador.js',
-                    'btn-delete.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("lavador.js", "btn-delete.click", error);
             }
         });
 
-        $(document).on('click', '.updateStatusLavador', function () {
+        $(document).on("click", ".updateStatusLavador", function () {
             try {
-                var url = $(this).data('url');
+                var url = $(this).data("url");
                 var currentElement = $(this);
 
                 $.get(url, function (data) {
                     try {
                         if (data.success) {
-                            AppToast.show(
-                                'Verde',
-                                'Status alterado com sucesso!',
-                                2000,
-                            );
+                            AppToast.show("Verde", "Status alterado com sucesso!", 2000);
 
                             if (data.type == 1) {
 
-                                currentElement.html(
-                                    '<i class="fa-duotone fa-circle-xmark"></i> Inativo',
-                                );
-                                currentElement
-                                    .removeClass('badge-ativo')
-                                    .addClass('badge-inativo');
-                                currentElement.attr(
-                                    'data-ejtip',
-                                    'Lavador inativo - clique para ativar',
-                                );
+                                currentElement.html('<i class="fa-duotone fa-circle-xmark"></i> Inativo');
+                                currentElement.removeClass("badge-ativo").addClass("badge-inativo");
+                                currentElement.attr("data-ejtip", "Lavador inativo - clique para ativar");
                             } else {
 
-                                currentElement.html(
-                                    '<i class="fa-duotone fa-circle-check"></i> Ativo',
-                                );
-                                currentElement
-                                    .removeClass('badge-inativo')
-                                    .addClass('badge-ativo');
-                                currentElement.attr(
-                                    'data-ejtip',
-                                    'Lavador ativo - clique para inativar',
-                                );
+                                currentElement.html('<i class="fa-duotone fa-circle-check"></i> Ativo');
+                                currentElement.removeClass("badge-inativo").addClass("badge-ativo");
+                                currentElement.attr("data-ejtip", "Lavador ativo - clique para inativar");
                             }
                         } else {
-                            Alerta.Erro(
-                                'Erro ao Alterar Status',
-                                'Ocorreu um erro ao tentar alterar o status.',
-                                'OK',
-                            );
+                            Alerta.Erro("Erro ao Alterar Status", "Ocorreu um erro ao tentar alterar o status.", "OK");
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'lavador.js',
-                            'updateStatus.callback',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("lavador.js", "updateStatus.callback", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'lavador.js',
-                    'updateStatusLavador.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("lavador.js", "updateStatusLavador.click", error);
             }
         });
+
     } catch (error) {
-        Alerta.TratamentoErroComLinha('lavador.js', 'document.ready', error);
+        Alerta.TratamentoErroComLinha("lavador.js", "document.ready", error);
     }
 });
 
 function loadList() {
     try {
-        dataTable = $('#tblLavador').DataTable({
+        dataTable = $("#tblLavador").DataTable({
             autoWidth: false,
             dom: 'Bfrtip',
             lengthMenu: [
                 [10, 25, 50, -1],
-                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas'],
+                ['10 linhas', '25 linhas', '50 linhas', 'Todas as Linhas']
             ],
-            buttons: [
-                'pageLength',
-                'excel',
-                {
-                    extend: 'pdfHtml5',
-                    orientation: 'landscape',
-                    pageSize: 'LEGAL',
-                },
-            ],
+            buttons: ['pageLength', 'excel', { extend: 'pdfHtml5', orientation: 'landscape', pageSize: 'LEGAL' }],
             order: [[0, 'asc']],
             columnDefs: [
-                { targets: 0, className: 'text-left', width: '25%' },
-                { targets: 1, className: 'text-center', width: '8%' },
-                { targets: 2, className: 'text-center', width: '12%' },
-                { targets: 3, className: 'text-left', width: '25%' },
-                { targets: 4, className: 'text-center', width: '10%' },
-                { targets: 5, className: 'text-center', width: '20%' },
+                { targets: 0, className: "text-left", width: "25%" },
+                { targets: 1, className: "text-center", width: "8%" },
+                { targets: 2, className: "text-center", width: "12%" },
+                { targets: 3, className: "text-left", width: "25%" },
+                { targets: 4, className: "text-center", width: "10%" },
+                { targets: 5, className: "text-center", width: "20%" }
             ],
             responsive: true,
             ajax: {
-                url: '/api/lavador',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/lavador",
+                type: "GET",
+                datatype: "json"
             },
             columns: [
-                { data: 'nome' },
-                { data: 'ponto' },
-                { data: 'celular01' },
-                { data: 'contratoLavador' },
+                { data: "nome" },
+                { data: "ponto" },
+                { data: "celular01" },
+                { data: "contratoLavador" },
                 {
-                    data: 'status',
+                    data: "status",
                     render: function (data, type, row) {
                         try {
                             if (data) {
@@ -207,17 +142,13 @@
                                         </a>`;
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'lavador.js',
-                                'render.status',
-                                error,
-                            );
-                            return '';
+                            Alerta.TratamentoErroComLinha("lavador.js", "render.status", error);
+                            return "";
                         }
-                    },
+                    }
                 },
                 {
-                    data: 'lavadorId',
+                    data: "lavadorId",
                     render: function (data) {
                         try {
                             return `<div class="ftx-btn-acoes">
@@ -240,23 +171,19 @@
                                         </a>
                                     </div>`;
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'lavador.js',
-                                'render.acoes',
-                                error,
-                            );
-                            return '';
+                            Alerta.TratamentoErroComLinha("lavador.js", "render.acoes", error);
+                            return "";
                         }
-                    },
-                },
+                    }
+                }
             ],
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Nenhum lavador encontrado',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Nenhum lavador encontrado"
             },
-            width: '100%',
+            width: "100%"
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha('lavador.js', 'loadList', error);
+        Alerta.TratamentoErroComLinha("lavador.js", "loadList", error);
     }
 }
```
