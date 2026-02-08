# wwwroot/js/cadastros/veiculositenscontrato_001.js

**Mudanca:** GRANDE | **+59** linhas | **-52** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/veiculositenscontrato_001.js
+++ ATUAL: wwwroot/js/cadastros/veiculositenscontrato_001.js
@@ -1,117 +1,108 @@
 var dataTableVeiculo;
 
 $(document).ready(function () {
-    try {
-
+    try
+    {
         loadList();
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'veiculositenscontrato_001.js',
-            'document.ready',
+            "veiculositenscontrato_<num>.js",
+            "callback@$.ready#0",
             error,
         );
     }
 });
 
 function loadList() {
-    try {
-
-        dataTableVeiculo = $('#tblVeiculo').DataTable({
-
+    try
+    {
+        dataTableVeiculo = $("#tblVeiculo").DataTable({
             columnDefs: [
                 {
                     targets: 0,
-                    className: 'text-center',
-                    width: '9%',
+                    className: "text-center",
+                    width: "9%",
                 },
                 {
                     targets: 1,
-                    className: 'text-left',
-                    width: '17%',
+                    className: "text-left",
+                    width: "17%",
                 },
                 {
                     targets: 2,
-                    className: 'text-left',
-                    width: '35%',
+                    className: "text-left",
+                    width: "35%",
                 },
                 {
                     targets: 3,
-                    className: 'text-center',
-                    width: '5%',
-                    defaultContent: '',
+                    className: "text-center",
+                    width: "5%",
+                    defaultContent: "",
                 },
                 {
                     targets: 4,
-                    className: 'text-center',
-                    width: '5%',
+                    className: "text-center",
+                    width: "5%",
                 },
                 {
                     targets: 5,
-                    className: 'text-center',
-                    width: '7%',
+                    className: "text-center",
+                    width: "7%",
                 },
                 {
                     targets: 6,
-                    className: 'text-center',
-                    width: '8%',
+                    className: "text-center",
+                    width: "8%",
                 },
             ],
 
             responsive: true,
-
             ajax: {
-                url: '/api/veiculo',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/veiculo",
+                type: "GET",
+                datatype: "json",
             },
-
             columns: [
-
-                { data: 'placa' },
-
-                { data: 'marcaModelo' },
-
-                { data: 'contratoVeiculo' },
-
-                { data: 'sigla' },
-
-                { data: 'combustivelDescricao' },
-
+                { data: "placa" },
+                { data: "marcaModelo" },
+                { data: "contratoVeiculo" },
+                { data: "sigla" },
+                { data: "combustivelDescricao" },
                 {
-                    data: 'status',
-
+                    data: "status",
                     render: function (data, type, row, meta) {
-                        try {
-                            if (data) {
-
+                        try
+                        {
+                            if (data)
                                 return (
                                     '<a href="javascript:void" class="updateStatusVeiculo btn btn-verde btn-xs text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
                                     row.veiculoId +
                                     '">Ativo</a>'
                                 );
-                            } else {
-
+                            else
                                 return (
                                     '<a href="javascript:void" class="updateStatusVeiculo btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
                                     row.veiculoId +
                                     '">Inativo</a>'
                                 );
-                            }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'veiculositenscontrato_001.js',
-                                'loadList.render.status',
+                                "veiculositenscontrato_<num>.js",
+                                "render",
                                 error,
                             );
                         }
                     },
                 },
-
                 {
-                    data: 'veiculoId',
-
+                    data: "veiculoId",
                     render: function (data) {
-                        try {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a href="/Veiculo/Upsert?id=${data}" class="btn btn-azul btn-xs text-white" aria-label="Editar o Veículo!" data-microtip-position="top" role="tooltip" style="cursor:pointer;">
                                     <i class="far fa-edit"></i>
@@ -119,11 +110,13 @@
                                 <a class="btn-delete btn btn-vinho btn-xs text-white" aria-label="Excluir o Veículo!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>
-                            </div>`;
-                        } catch (error) {
+                    </div>`;
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'veiculositenscontrato_001.js',
-                                'loadList.render.actions',
+                                "veiculositenscontrato_<num>.js",
+                                "render",
                                 error,
                             );
                         }
@@ -132,16 +125,14 @@
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
-        Alerta.TratamentoErroComLinha(
-            'veiculositenscontrato_001.js',
-            'loadList',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("veiculositenscontrato_<num>.js", "loadList", error);
     }
 }
```
