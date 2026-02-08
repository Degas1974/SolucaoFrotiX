# wwwroot/js/cadastros/motoristasitenscontrato_001.js

**Mudanca:** GRANDE | **+66** linhas | **-55** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/motoristasitenscontrato_001.js
+++ ATUAL: wwwroot/js/cadastros/motoristasitenscontrato_001.js
@@ -1,140 +1,131 @@
 var dataTableMotorista;
 
 $(document).ready(function () {
-    try {
-
+    try
+    {
         loadMotoristaList();
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'motoristasitenscontrato_001.js',
-            'document.ready',
+            "motoristasitenscontrato_<num>.js",
+            "callback@$.ready#0",
             error,
         );
     }
 });
 
 function loadMotoristaList() {
-    try {
-
-        dataTableMotorista = $('#tblMotorista').DataTable({
-
+    try
+    {
+        dataTableMotorista = $("#tblMotorista").DataTable({
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
-                    width: '6%',
+                    className: "text-center",
+                    width: "6%",
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
-                    width: '8%',
+                    className: "text-center",
+                    width: "8%",
                 },
                 {
                     targets: 5,
-                    className: 'text-left',
-                    width: '5%',
+                    className: "text-left",
+                    width: "5%",
                 },
                 {
                     targets: 6,
-                    className: 'text-left',
-                    width: '10%',
+                    className: "text-left",
+                    width: "10%",
                 },
                 {
                     targets: 7,
-                    className: 'text-center',
-                    width: '5%',
+                    className: "text-center",
+                    width: "5%",
                 },
                 {
                     targets: 8,
-                    className: 'text-center',
-                    width: '8%',
+                    className: "text-center",
+                    width: "8%",
                 },
             ],
 
             responsive: true,
-
             ajax: {
-                url: '/api/motorista',
-                type: 'GET',
-                datatype: 'json',
+                url: "/api/motorista",
+                type: "GET",
+                datatype: "json",
             },
-
             columns: [
-
-                { data: 'nome' },
-
-                { data: 'ponto' },
-
-                { data: 'cnh' },
-
-                { data: 'categoriaCNH' },
-
-                { data: 'celular01' },
-
-                { data: 'sigla' },
-
-                { data: 'contratoMotorista' },
-
+                { data: "nome" },
+                { data: "ponto" },
+                { data: "cnh" },
+                { data: "categoriaCNH" },
+                { data: "celular01" },
+                { data: "sigla" },
+                { data: "contratoMotorista" },
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
                                     '<a href="javascript:void" class="updateStatusMotorista btn btn-verde btn-xs text-white" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                     row.motoristaId +
                                     '">Ativo</a>'
                                 );
-                            } else {
-
+                            else
                                 return (
                                     '<a href="javascript:void" class="updateStatusMotorista btn btn-xs fundo-cinza text-white text-bold" data-url="/api/Motorista/updateStatusMotorista?Id=' +
                                     row.motoristaId +
                                     '">Inativo</a>'
                                 );
-                            }
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'motoristasitenscontrato_001.js',
-                                'loadList.render.status',
+                                "motoristasitenscontrato_<num>.js",
+                                "render",
                                 error,
                             );
                         }
                     },
                 },
-
                 {
-                    data: 'motoristaId',
-
+                    data: "motoristaId",
                     render: function (data) {
-                        try {
+                        try
+                        {
                             return `<div class="text-center">
                                 <a class="btn-delete btn btn-vinho btn-xs text-white" aria-label="Excluir o Motorista do Contrato!" data-microtip-position="top" role="tooltip" style="cursor:pointer;" data-id='${data}'>
                                     <i class="far fa-trash-alt"></i>
                                 </a>
-                            </div>`;
-                        } catch (error) {
+                    </div>`;
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'motoristasitenscontrato_001.js',
-                                'loadList.render.actions',
+                                "motoristasitenscontrato_<num>.js",
+                                "render",
                                 error,
                             );
                         }
@@ -143,15 +134,17 @@
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
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'motoristasitenscontrato_001.js',
-            'loadMotoristaList',
+            "motoristasitenscontrato_<num>.js",
+            "loadMotoristaList",
             error,
         );
     }
```
