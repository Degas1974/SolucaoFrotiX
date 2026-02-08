# wwwroot/js/cadastros/anulacao_001.js

**Mudanca:** GRANDE | **+84** linhas | **-89** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/anulacao_001.js
+++ ATUAL: wwwroot/js/cadastros/anulacao_001.js
@@ -1,98 +1,104 @@
 var GlosaTable;
 
 $(document).ready(function () {
-    try {
-        $(document).on('click', '.btn-deleteanulacao', function () {
-            try {
-                var id = $(this).data('id');
-                var context = $(this).data('context');
+    try
+    {
 
-                var idEmpenho = context === 'empenho' ? id : null;
-                var idEmpenhoMulta = context === 'empenhoMulta' ? id : null;
+                $(document).on("click", ".btn-deleteanulacao", function () {
+                    try
+                    {
 
-                Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar esta anulação?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
-                ).then((willDelete) => {
-                    try {
-                        if (willDelete) {
-                            var dataToPost = JSON.stringify({
-                                mEmpenho: idEmpenho
-                                    ? { MovimentacaoId: idEmpenho }
-                                    : null,
-                                mEmpenhoMulta: idEmpenhoMulta
-                                    ? { MovimentacaoId: idEmpenhoMulta }
-                                    : null,
-                            });
-                            var url = '/api/Empenho/DeleteMovimentacao';
-                            $.ajax({
-                                url: url,
-                                type: 'POST',
-                                data: dataToPost,
-                                contentType: 'application/json; charset=utf-8',
-                                dataType: 'json',
-                                success: function (data) {
-                                    try {
-                                        if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-                                            $('#tblGlosa')
-                                                .DataTable()
-                                                .ajax.reload(null, false);
-                                            location.reload();
-                                        } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
-                                        }
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'anulacao_<num>.js',
-                                            'success',
-                                            error,
-                                        );
-                                    }
-                                },
-                                error: function (err) {
-                                    try {
-                                        console.log(err);
-                                        alert('something went wrong');
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'anulacao_<num>.js',
-                                            'error',
-                                            error,
-                                        );
-                                    }
-                                },
-                            });
-                        }
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'anulacao_<num>.js',
-                            'callback@swal.then#0',
-                            error,
-                        );
+                        var id = $(this).data("id");
+                        var context = $(this).data("context");
+
+                        var idEmpenho = context === "empenho" ? id : null;
+                        var idEmpenhoMulta = context === "empenhoMulta" ? id : null;
+
+                        Alerta.Confirmar(
+                            "Você tem certeza que deseja apagar esta anulação?",
+                            "Não será possível recuperar os dados eliminados!",
+                            "Excluir",
+                            "Cancelar",
+                        ).then((willDelete) => {
+                            try
+                            {
+                                if (willDelete) {
+
+                                    var dataToPost = JSON.stringify({
+                                        mEmpenho: idEmpenho ? { MovimentacaoId: idEmpenho } : null,
+                                        mEmpenhoMulta: idEmpenhoMulta ? { MovimentacaoId: idEmpenhoMulta } : null
+                                    });
+                                    var url = "/api/Empenho/DeleteMovimentacao";
+
+                                    $.ajax({
+                                        url: url,
+                                        type: "POST",
+                                        data: dataToPost,
+                                        contentType: "application/json; charset=utf-8",
+                                        dataType: "json",
+                                        success: function (data) {
+                                            try
+                                            {
+
+                                                if (data.success) {
+
+                                                    AppToast.show('Verde', data.message);
+
+                                                    $("#tblGlosa").DataTable().ajax.reload(null, false);
+
+                                                    location.reload();
+                                                } else {
+
+                                                    AppToast.show('Vermelho', data.message);
+                                                }
+                                            }
+                                            catch (error)
+                                            {
+                                                Alerta.TratamentoErroComLinha(
+                                                    "anulacao_001.js",
+                                                    "$.ajax.success",
+                                                    error,
+                                                );
+                                            }
+                                        },
+                                        error: function (err) {
+                                            try
+                                            {
+
+                                                console.log(err);
+
+                                                alert("something went wrong");
+                                            }
+                                            catch (error)
+                                            {
+                                                Alerta.TratamentoErroComLinha(
+                                                    "anulacao_001.js",
+                                                    "$.ajax.error",
+                                                    error,
+                                                );
+                                            }
+                                        },
+                                    });
+                                }
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha(
+                                    "anulacao_001.js",
+                                    "Alerta.Confirmar.then",
+                                    error,
+                                );
+                            }
+                        });
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha("anulacao_001.js", ".btn-deleteanulacao.click", error);
                     }
                 });
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'anulacao_<num>.js',
-                    'callback@$.on#2',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'anulacao_<num>.js',
-            'callback@$.ready#0',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("anulacao_001.js", "DOMContentLoaded", error);
     }
 });
```
