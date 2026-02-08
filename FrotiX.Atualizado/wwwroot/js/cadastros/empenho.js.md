# wwwroot/js/cadastros/empenho.js

**Mudanca:** GRANDE | **+98** linhas | **-168** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/empenho.js
+++ ATUAL: wwwroot/js/cadastros/empenho.js
@@ -3,254 +3,193 @@
 $(document).ready(function () {
     try {
 
-        $(document).on('click', '.btn-delete', function () {
+        $(document).on("click", ".btn-delete", function () {
             try {
-                var id = $(this).data('id');
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar este empenho?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar este empenho?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
                 ).then((willDelete) => {
                     try {
                         if (willDelete) {
                             var dataToPost = JSON.stringify({ EmpenhoId: id });
-                            var url = '/api/Empenho/Delete';
+                            var url = "/api/Empenho/Delete";
 
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
                                     try {
                                         if (data.success) {
-                                            AppToast.show(
-                                                'Verde',
-                                                data.message,
-                                            );
-                                            $('#tblEmpenho')
-                                                .DataTable()
-                                                .ajax.reload(null, false);
+                                            AppToast.show('Verde', data.message);
+                                            $("#tblEmpenho").DataTable().ajax.reload(null, false);
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
                                     } catch (error) {
                                         Alerta.TratamentoErroComLinha(
-                                            'empenho.js',
-                                            'btn-delete.ajax.success',
-                                            error,
+                                            "empenho.js",
+                                            "btn-delete.ajax.success",
+                                            error
                                         );
                                     }
                                 },
                                 error: function (err) {
                                     try {
-                                        console.error(
-                                            'Erro ao excluir empenho:',
-                                            err,
-                                        );
-                                        AppToast.show(
-                                            'Vermelho',
-                                            'Erro ao excluir o empenho. Tente novamente.',
-                                        );
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'empenho.js',
-                                            'btn-delete.ajax.error',
-                                            error,
-                                        );
-                                    }
-                                },
+                                        console.error("Erro ao excluir empenho:", err);
+                                        AppToast.show('Vermelho', 'Erro ao excluir o empenho. Tente novamente.');
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha(
+                                            "empenho.js",
+                                            "btn-delete.ajax.error",
+                                            error
+                                        );
+                                    }
+                                }
                             });
                         }
                     } catch (error) {
                         Alerta.TratamentoErroComLinha(
-                            'empenho.js',
-                            'btn-delete.swal.then',
-                            error,
+                            "empenho.js",
+                            "btn-delete.swal.then",
+                            error
                         );
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'empenho.js',
-                    'btn-delete.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("empenho.js", "btn-delete.click", error);
             }
         });
 
-        $(document).on('click', '.updateStatusEmpenho', function () {
+        $(document).on("click", ".updateStatusEmpenho", function () {
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
-                            );
-                            var text = 'Ativo';
+                            AppToast.show('Verde', "Status alterado com sucesso!");
+                            var text = "Ativo";
 
                             if (data.type == 1) {
-                                text = 'Inativo';
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
+                                text = "Inativo";
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
                             } else {
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
                             }
 
                             currentElement.text(text);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Erro ao alterar o status.',
-                            );
+                            AppToast.show('Vermelho', 'Erro ao alterar o status.');
                         }
                     } catch (error) {
                         Alerta.TratamentoErroComLinha(
-                            'empenho.js',
-                            'updateStatusEmpenho.get.success',
-                            error,
+                            "empenho.js",
+                            "updateStatusEmpenho.get.success",
+                            error
                         );
                     }
                 }).fail(function (err) {
                     try {
-                        console.error('Erro ao alterar status:', err);
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status. Tente novamente.',
-                        );
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'empenho.js',
-                            'updateStatusEmpenho.get.fail',
-                            error,
+                        console.error("Erro ao alterar status:", err);
+                        AppToast.show('Vermelho', 'Erro ao alterar o status. Tente novamente.');
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha(
+                            "empenho.js",
+                            "updateStatusEmpenho.get.fail",
+                            error
                         );
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'empenho.js',
-                    'updateStatusEmpenho.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("empenho.js", "updateStatusEmpenho.click", error);
             }
         });
 
-        $(document).on('click', '.btn-delete-nf', function () {
+        $(document).on("click", ".btn-delete-nf", function () {
             try {
-                var id = $(this).data('id');
+                var id = $(this).data("id");
 
                 Alerta.Confirmar(
-                    'Você tem certeza que deseja apagar esta Nota Fiscal?',
-                    'Não será possível recuperar os dados eliminados!',
-                    'Excluir',
-                    'Cancelar',
+                    "Você tem certeza que deseja apagar esta Nota Fiscal?",
+                    "Não será possível recuperar os dados eliminados!",
+                    "Excluir",
+                    "Cancelar"
                 ).then((willDelete) => {
                     try {
                         if (willDelete) {
                             $.ajax({
-                                url: '/api/NotaFiscal/Delete',
-                                type: 'POST',
+                                url: "/api/NotaFiscal/Delete",
+                                type: "POST",
                                 data: JSON.stringify({ NotaFiscalId: id }),
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
-                                            );
-                                            $('#tblNotaFiscal')
-                                                .DataTable()
-                                                .ajax.reload(null, false);
-
-                                            if (
-                                                $.fn.DataTable.isDataTable(
-                                                    '#tblEmpenho',
-                                                )
-                                            ) {
-                                                $('#tblEmpenho')
-                                                    .DataTable()
-                                                    .ajax.reload(null, false);
+                                            AppToast.show('Verde', data.message);
+                                            $("#tblNotaFiscal").DataTable().ajax.reload(null, false);
+
+                                            if ($.fn.DataTable.isDataTable('#tblEmpenho')) {
+                                                $("#tblEmpenho").DataTable().ajax.reload(null, false);
                                             }
                                         } else {
-                                            AppToast.show(
-                                                'Vermelho',
-                                                data.message,
-                                            );
+                                            AppToast.show('Vermelho', data.message);
                                         }
                                     } catch (error) {
                                         Alerta.TratamentoErroComLinha(
-                                            'empenho.js',
-                                            'btn-delete-nf.ajax.success',
-                                            error,
+                                            "empenho.js",
+                                            "btn-delete-nf.ajax.success",
+                                            error
                                         );
                                     }
                                 },
                                 error: function (err) {
                                     try {
-                                        console.error(
-                                            'Erro ao excluir nota fiscal:',
-                                            err,
-                                        );
-                                        AppToast.show(
-                                            'Vermelho',
-                                            'Erro ao excluir a nota fiscal. Tente novamente.',
-                                        );
-                                    } catch (error) {
-                                        Alerta.TratamentoErroComLinha(
-                                            'empenho.js',
-                                            'btn-delete-nf.ajax.error',
-                                            error,
-                                        );
-                                    }
-                                },
+                                        console.error("Erro ao excluir nota fiscal:", err);
+                                        AppToast.show('Vermelho', 'Erro ao excluir a nota fiscal. Tente novamente.');
+                                    } catch (error) {
+                                        Alerta.TratamentoErroComLinha(
+                                            "empenho.js",
+                                            "btn-delete-nf.ajax.error",
+                                            error
+                                        );
+                                    }
+                                }
                             });
                         }
                     } catch (error) {
                         Alerta.TratamentoErroComLinha(
-                            'empenho.js',
-                            'btn-delete-nf.swal.then',
-                            error,
+                            "empenho.js",
+                            "btn-delete-nf.swal.then",
+                            error
                         );
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'empenho.js',
-                    'btn-delete-nf.click',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("empenho.js", "btn-delete-nf.click", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('empenho.js', 'document.ready', error);
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("empenho.js", "document.ready", error);
     }
 });
 
 function formatarMoeda(valor) {
     try {
-        if (valor === null || valor === undefined) return 'R$ 0,00';
-        return valor.toLocaleString('pt-BR', {
-            style: 'currency',
-            currency: 'BRL',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('empenho.js', 'formatarMoeda', error);
-        return 'R$ 0,00';
+        if (valor === null || valor === undefined) return "R$ 0,00";
+        return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("empenho.js", "formatarMoeda", error);
+        return "R$ 0,00";
     }
 }
 
@@ -263,23 +202,23 @@
                 .replace(/\./g, '')
                 .replace(',', '.')
                 .replace('R$', '')
-                .replace('&nbsp;', ''),
+                .replace('&nbsp;', '')
         );
     } catch (error) {
-        Alerta.TratamentoErroComLinha('empenho.js', 'moedaParaNumero', error);
+        Alerta.TratamentoErroComLinha("empenho.js", "moedaParaNumero", error);
         return 0;
     }
 }
 
 function formatarData(data) {
     try {
-        if (!data) return '';
+        if (!data) return "";
         const d = new Date(data);
-        if (isNaN(d.getTime())) return '';
+        if (isNaN(d.getTime())) return "";
         return d.toLocaleDateString('pt-BR');
     } catch (error) {
-        Alerta.TratamentoErroComLinha('empenho.js', 'formatarData', error);
-        return '';
+        Alerta.TratamentoErroComLinha("empenho.js", "formatarData", error);
+        return "";
     }
 }
 
@@ -289,11 +228,7 @@
             $('#tblEmpenho').DataTable().ajax.reload(null, false);
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'empenho.js',
-            'recarregarTabelaEmpenhos',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("empenho.js", "recarregarTabelaEmpenhos", error);
     }
 }
 
@@ -303,10 +238,6 @@
             $('#tblNotaFiscal').DataTable().ajax.reload(null, false);
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'empenho.js',
-            'recarregarTabelaNotasFiscais',
-            error,
-        );
-    }
-}
+        Alerta.TratamentoErroComLinha("empenho.js", "recarregarTabelaNotasFiscais", error);
+    }
+}
```
