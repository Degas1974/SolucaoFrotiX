# wwwroot/js/cadastros/ata.js

**Mudanca:** GRANDE | **+73** linhas | **-145** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/ata.js
+++ ATUAL: wwwroot/js/cadastros/ata.js
@@ -4,100 +4,69 @@
     try {
         loadList();
 
-        $(document).on('click', '.btn-delete', function () {
+        $(document).on("click", ".btn-delete", function () {
             try {
 
                 if ($(this).hasClass('disabled')) {
                     return;
                 }
 
-                var id = $(this).data('id');
+                var id = $(this).data("id");
 
                 $.ajax({
-                    url:
-                        '/api/AtaRegistroPrecos/VerificarDependencias?id=' + id,
-                    type: 'GET',
-                    dataType: 'json',
+                    url: "/api/AtaRegistroPrecos/VerificarDependencias?id=" + id,
+                    type: "GET",
+                    dataType: "json",
                     success: function (result) {
                         try {
                             if (result.success && result.possuiDependencias) {
-                                var mensagem =
-                                    'Esta ata não pode ser excluída pois possui:\n\n';
-                                if (result.itens > 0)
-                                    mensagem +=
-                                        '• ' +
-                                        result.itens +
-                                        ' item(ns) vinculado(s)\n';
-                                if (result.veiculos > 0)
-                                    mensagem +=
-                                        '• ' +
-                                        result.veiculos +
-                                        ' veículo(s) associado(s)\n';
-
-                                mensagem +=
-                                    '\nRemova as associações antes de excluir a ata.';
-                                Alerta.Warning(
-                                    'Exclusão não permitida',
-                                    mensagem,
-                                );
+                                var mensagem = "Esta ata não pode ser excluída pois possui:\n\n";
+                                if (result.itens > 0) mensagem += "• " + result.itens + " item(ns) vinculado(s)\n";
+                                if (result.veiculos > 0) mensagem += "• " + result.veiculos + " veículo(s) associado(s)\n";
+
+                                mensagem += "\nRemova as associações antes de excluir a ata.";
+                                Alerta.Warning("Exclusão não permitida", mensagem);
                             } else {
 
                                 Alerta.Confirmar(
-                                    'Você tem certeza que deseja apagar?',
-                                    'Você não será capaz de restaurar os dados!',
-                                    'Sim, apague!',
-                                    'Não, cancele!',
+                                    "Você tem certeza que deseja apagar?",
+                                    "Você não será capaz de restaurar os dados!",
+                                    "Sim, apague!",
+                                    "Não, cancele!"
                                 ).then((willDelete) => {
                                     if (willDelete) {
-                                        var dataToPost = JSON.stringify({
-                                            AtaId: id,
-                                        });
+                                        var dataToPost = JSON.stringify({ AtaId: id });
                                         $.ajax({
-                                            url: 'api/AtaRegistroPrecos/Delete',
-                                            type: 'POST',
+                                            url: "api/AtaRegistroPrecos/Delete",
+                                            type: "POST",
                                             data: dataToPost,
-                                            contentType:
-                                                'application/json; charset=utf-8',
-                                            dataType: 'json',
+                                            contentType: "application/json; charset=utf-8",
+                                            dataType: "json",
                                             success: function (data) {
                                                 if (data.success) {
-                                                    AppToast.show(
-                                                        'Verde',
-                                                        data.message,
-                                                    );
+                                                    AppToast.show('Verde', data.message);
                                                     dataTable.ajax.reload();
                                                 } else {
-                                                    AppToast.show(
-                                                        'Vermelho',
-                                                        data.message,
-                                                    );
+                                                    AppToast.show('Vermelho', data.message);
                                                 }
-                                            },
+                                            }
                                         });
                                     }
                                 });
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ata.js',
-                                'btn-delete.ajax.success',
-                                error,
-                            );
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("ata.js", "btn-delete.ajax.success", error);
+                        }
+                    }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ata.js',
-                    'btn-delete.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("ata.js", "btn-delete.click", error);
             }
         });
 
-        $(document).on('click', '.updateStatusAta', function () {
+        $(document).on("click", ".updateStatusAta", function () {
             try {
-                var url = $(this).data('url');
+                var url = $(this).data("url");
                 var currentElement = $(this);
 
                 $.post(url, function (data) {
@@ -106,52 +75,42 @@
                             AppToast.show('Verde', data.message);
                             dataTable.ajax.reload();
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao alterar status!',
-                            );
+                            AppToast.show('Vermelho', 'Erro ao alterar status!');
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'ata.js',
-                            'updateStatusAta.post.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("ata.js", "updateStatusAta.post.success", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ata.js',
-                    'updateStatusAta.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("ata.js", "updateStatusAta.click", error);
             }
         });
+
     } catch (error) {
-        Alerta.TratamentoErroComLinha('ata.js', 'document.ready', error);
+        Alerta.TratamentoErroComLinha("ata.js", "document.ready", error);
     }
 });
 
 function loadList() {
     try {
-        dataTable = $('#tblAta').DataTable({
-            order: [[0, 'desc']],
-            responsive: true,
-            ajax: {
-                url: 'api/AtaRegistroPrecos',
-                type: 'GET',
-                datatype: 'json',
+        dataTable = $("#tblAta").DataTable({
+            "order": [[0, "desc"]],
+            "responsive": true,
+            "ajax": {
+                "url": "api/AtaRegistroPrecos",
+                "type": "GET",
+                "datatype": "json"
             },
-            columns: [
-                { data: 'ataCompleta', width: '10%' },
-                { data: 'processoCompleto', width: '10%' },
-                { data: 'objeto', width: '20%' },
-                { data: 'descricaoFornecedor', width: '20%' },
-                { data: 'periodo', width: '10%' },
-                { data: 'valorFormatado', width: '10%' },
+            "columns": [
+                { "data": "ataCompleta", "width": "10%" },
+                { "data": "processoCompleto", "width": "10%" },
+                { "data": "objeto", "width": "20%" },
+                { "data": "descricaoFornecedor", "width": "20%" },
+                { "data": "periodo", "width": "10%" },
+                { "data": "valorFormatado", "width": "10%" },
                 {
-                    data: 'status',
-                    render: function (data, type, row) {
+                    "data": "status",
+                    "render": function (data, type, row) {
                         try {
                             if (data) {
                                 return `<a href="javascript:void(0)"
@@ -167,42 +126,24 @@
                                         </a>`;
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ata.js',
-                                'render.status',
-                                error,
-                            );
-                        }
-                    },
-                    width: '10%',
+                            Alerta.TratamentoErroComLinha("ata.js", "render.status", error);
+                        }
+                    }, "width": "10%"
                 },
                 {
-                    data: 'ataId',
-                    render: function (data, type, row) {
+                    "data": "ataId",
+                    "render": function (data, type, row) {
                         try {
 
                             var dependencias = [];
-                            if (row.depItens > 0)
-                                dependencias.push(
-                                    ' - ' +
-                                        row.depItens +
-                                        ' item(ns) vinculado(s)',
-                                );
-                            if (row.depVeiculos > 0)
-                                dependencias.push(
-                                    ' - ' +
-                                        row.depVeiculos +
-                                        ' veículo(s) associado(s)',
-                                );
+                            if (row.depItens > 0) dependencias.push(' - ' + row.depItens + ' item(ns) vinculado(s)');
+                            if (row.depVeiculos > 0) dependencias.push(' - ' + row.depVeiculos + ' veículo(s) associado(s)');
 
                             var possuiDependencias = dependencias.length > 0;
-                            var disabledExcluir = possuiDependencias
-                                ? 'disabled'
-                                : '';
+                            var disabledExcluir = possuiDependencias ? 'disabled' : '';
 
                             var tooltipExcluirHtml = possuiDependencias
-                                ? 'Exclusão bloqueada:<br>' +
-                                  dependencias.join('<br>')
+                                ? 'Exclusão bloqueada:<br>' + dependencias.join('<br>')
                                 : '';
 
                             var tooltipExcluirAttr = possuiDependencias
@@ -223,55 +164,43 @@
                                         </a>
                                     </div>`;
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ata.js',
-                                'render.actions',
-                                error,
-                            );
-                        }
-                    },
-                    width: '10%',
-                },
+                            Alerta.TratamentoErroComLinha("ata.js", "render.actions", error);
+                        }
+                    }, "width": "10%"
+                }
             ],
-            language: {
-                url: 'https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json',
-                emptyTable: 'Nenhum registro encontrado',
+            "language": {
+                "url": "https://cdn.datatables.net/plug-ins/1.10.25/i18n/Portuguese-Brasil.json",
+                "emptyTable": "Nenhum registro encontrado"
             },
-            width: '100%',
-
-            drawCallback: function () {
+            "width": "100%",
+
+            drawCallback: function() {
                 try {
 
                     if (window.ejTooltip) {
                         window.ejTooltip.refresh();
                     }
 
-                    var tooltipTriggerList = document.querySelectorAll(
-                        '#tblAta [data-bs-toggle="tooltip"]',
-                    );
-                    tooltipTriggerList.forEach(function (tooltipTriggerEl) {
-
-                        var existingTooltip =
-                            bootstrap.Tooltip.getInstance(tooltipTriggerEl);
+                    var tooltipTriggerList = document.querySelectorAll('#tblAta [data-bs-toggle="tooltip"]');
+                    tooltipTriggerList.forEach(function(tooltipTriggerEl) {
+
+                        var existingTooltip = bootstrap.Tooltip.getInstance(tooltipTriggerEl);
                         if (existingTooltip) {
                             existingTooltip.dispose();
                         }
 
                         new bootstrap.Tooltip(tooltipTriggerEl, {
                             customClass: 'tooltip-ftx-syncfusion',
-                            trigger: 'hover',
+                            trigger: 'hover'
                         });
                     });
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ata.js',
-                        'drawCallback',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("ata.js", "drawCallback", error);
                 }
-            },
+            }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha('ata.js', 'loadList', error);
+        Alerta.TratamentoErroComLinha("ata.js", "loadList", error);
     }
 }
```
