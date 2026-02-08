# wwwroot/js/cadastros/aporte_001.js

**Mudanca:** GRANDE | **+52** linhas | **-57** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/aporte_001.js
+++ ATUAL: wwwroot/js/cadastros/aporte_001.js
@@ -1,98 +1,94 @@
 var AporteTable;
 
 $(document).ready(function () {
-    try {
-        $(document).on('click', '.btn-deleteaporte', function () {
-            try {
-                var id = $(this).data('id');
-                var context = $(this).data('context');
+    try
+    {
 
-                var idEmpenho = context === 'empenho' ? id : null;
-                var idEmpenhoMulta = context === 'empenhoMulta' ? id : null;
+        $(document).on("click", ".btn-deleteaporte", function () {
+            try
+            {
+                var id = $(this).data("id");
+                var context = $(this).data("context");
+
+                var idEmpenho = context === "empenho" ? id : null;
+                var idEmpenhoMulta = context === "empenhoMulta" ? id : null;
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este aporte?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar este aporte?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar",
                 ).then((willDelete) => {
-                    try {
+                    try
+                    {
                         if (willDelete) {
                             var dataToPost = JSON.stringify({
-                                mEmpenho: idEmpenho
-                                    ? { MovimentacaoId: idEmpenho }
-                                    : null,
-                                mEmpenhoMulta: idEmpenhoMulta
-                                    ? { MovimentacaoId: idEmpenhoMulta }
-                                    : null,
+                                mEmpenho: idEmpenho ? { MovimentacaoId: idEmpenho } : null,
+                                mEmpenhoMulta: idEmpenhoMulta ? { MovimentacaoId: idEmpenhoMulta } : null
                             });
-                            var url = '/api/Empenho/DeleteMovimentacao';
+                            var url = "/api/Empenho/DeleteMovimentacao";
                             $.ajax({
                                 url: url,
-                                type: 'POST',
+                                type: "POST",
                                 data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
+                                contentType: "application/json; charset=utf-8",
+                                dataType: "json",
                                 success: function (data) {
-                                    try {
+                                    try
+                                    {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-                                            $('#tblAporte')
-                                                .DataTable()
-                                                .ajax.reload(null, false);
+                                            AppToast.show('Verde', data.message);
+                                            $("#tblAporte").DataTable().ajax.reload(null, false);
                                             location.reload();
                                         } else {
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
-                                            'aporte_<num>.js',
-                                            'success',
+                                            "aporte_<num>.js",
+                                            "success",
                                             error,
                                         );
                                     }
                                 },
                                 error: function (err) {
-                                    try {
+                                    try
+                                    {
                                         console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
+                                        alert("something went wrong");
+                                    }
+                                    catch (error)
+                                    {
                                         Alerta.TratamentoErroComLinha(
-                                            'aporte_<num>.js',
-                                            'error',
+                                            "aporte_<num>.js",
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
-                            'aporte_<num>.js',
-                            'callback@swal.then#0',
+                            "aporte_<num>.js",
+                            "callback@swal.then#0",
                             error,
                         );
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'aporte_<num>.js',
-                    'callback@$.on#2',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("aporte_<num>.js", "callback@$.on#2", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'aporte_<num>.js',
-            'callback@$.ready#0',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("aporte_<num>.js", "callback@$.ready#0", error);
     }
 });
```
