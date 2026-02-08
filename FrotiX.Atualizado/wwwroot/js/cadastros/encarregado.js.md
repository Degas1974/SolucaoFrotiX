# wwwroot/js/cadastros/encarregado.js

**Mudanca:** GRANDE | **+60** linhas | **-134** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/encarregado.js
+++ ATUAL: wwwroot/js/cadastros/encarregado.js
@@ -4,189 +4,125 @@
     try {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
+        $(document).on("click", ".btn-delete", function () {
             try {
-                var id = $(this).data('id');
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Confirmar Exclusão',
-                    'Você tem certeza que deseja apagar este encarregado?',
-                    'Sim, excluir',
-                    'Cancelar',
+                    "Confirmar Exclusão",
+                    "Você tem certeza que deseja apagar este encarregado?",
+                    "Sim, excluir",
+                    "Cancelar"
                 ).then((willDelete) => {
                     try {
                         if (willDelete) {
-                            var dataToPost = JSON.stringify({
-                                EncarregadoId: id,
-                            });
+                            var dataToPost = JSON.stringify({ EncarregadoId: id });
                             $.ajax({
-                                url: '/api/Encarregado/Delete',
-                                type: 'POST',
+                                url: "/api/Encarregado/Delete",
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
-                                            'encarregado.js',
-                                            'Delete.success',
-                                            error,
-                                        );
+                                        Alerta.TratamentoErroComLinha("encarregado.js", "Delete.success", error);
                                     }
                                 },
                                 error: function (err) {
                                     try {
-                                        console.error('Erro ao excluir:', err);
-                                        AppToast.show(
-                                            'Vermelho',
-                                            'Erro ao excluir encarregado',
-                                            3000,
-                                        );
+                                        console.error("Erro ao excluir:", err);
+                                        AppToast.show("Vermelho", "Erro ao excluir encarregado", 3000);
                                     } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'encarregado.js',
-                                            'Delete.error',
-                                            error,
-                                        );
+                                        Alerta.TratamentoErroComLinha("encarregado.js", "Delete.error", error);
                                     }
-                                },
+                                }
                             });
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'encarregado.js',
-                            'Delete.confirmar',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("encarregado.js", "Delete.confirmar", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'encarregado.js',
-                    'btn-delete.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("encarregado.js", "btn-delete.click", error);
             }
         });
 
-        $(document).on('click', '.updateStatusEncarregado', function () {
+        $(document).on("click", ".updateStatusEncarregado", function () {
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
-                                    '<i class="fa-duotone fa-circle-xmark me-1"></i> Inativo',
-                                );
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
+                                currentElement.html('<i class="fa-duotone fa-circle-xmark me-1"></i> Inativo');
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                             } else {
 
-                                currentElement.html(
-                                    '<i class="fa-duotone fa-circle-check me-1"></i> Ativo',
-                                );
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                                currentElement.html('<i class="fa-duotone fa-circle-check me-1"></i> Ativo');
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
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
-                            'encarregado.js',
-                            'updateStatus.callback',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("encarregado.js", "updateStatus.callback", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'encarregado.js',
-                    'updateStatusEncarregado.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("encarregado.js", "updateStatusEncarregado.click", error);
             }
         });
+
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'encarregado.js',
-            'document.ready',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("encarregado.js", "document.ready", error);
     }
 });
 
 function loadList() {
     try {
-        dataTable = $('#tblEncarregado').DataTable({
+        dataTable = $("#tblEncarregado").DataTable({
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
-                { targets: 3, className: 'text-left', width: '29%' },
-                { targets: 4, className: 'text-center', width: '10%' },
-                { targets: 5, className: 'text-center', width: '16%' },
+                { targets: 0, className: "text-left", width: "25%" },
+                { targets: 1, className: "text-center", width: "8%" },
+                { targets: 2, className: "text-center", width: "12%" },
+                { targets: 3, className: "text-left", width: "29%" },
+                { targets: 4, className: "text-center", width: "10%" },
+                { targets: 5, className: "text-center", width: "16%" }
             ],
             responsive: true,
+
             ajax: {
-                url: '/api/encarregado',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/encarregado",
+                type: "GET",
+                datatype: "json"
             },
             columns: [
-                { data: 'nome' },
-                { data: 'ponto' },
-                { data: 'celular01' },
-                { data: 'contratoEncarregado' },
+                { data: "nome" },
+                { data: "ponto" },
+                { data: "celular01" },
+                { data: "contratoEncarregado" },
                 {
-                    data: 'status',
+                    data: "status",
                     render: function (data, type, row) {
                         try {
                             if (data) {
@@ -205,17 +141,13 @@
                                         </a>`;
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'encarregado.js',
-                                'render.status',
-                                error,
-                            );
-                            return '';
+                            Alerta.TratamentoErroComLinha("encarregado.js", "render.status", error);
+                            return "";
                         }
-                    },
+                    }
                 },
                 {
-                    data: 'encarregadoId',
+                    data: "encarregadoId",
                     render: function (data) {
                         try {
                             return `<div class="ftx-btn-acoes">
@@ -235,23 +167,19 @@
                                         </a>
                                     </div>`;
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'encarregado.js',
-                                'render.acoes',
-                                error,
-                            );
-                            return '';
+                            Alerta.TratamentoErroComLinha("encarregado.js", "render.acoes", error);
+                            return "";
                         }
-                    },
-                },
+                    }
+                }
             ],
             language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Nenhum encarregado encontrado',
+                url: "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                emptyTable: "Nenhum encarregado encontrado"
             },
-            width: '100%',
+            width: "100%"
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha('encarregado.js', 'loadList', error);
+        Alerta.TratamentoErroComLinha("encarregado.js", "loadList", error);
     }
 }
```
