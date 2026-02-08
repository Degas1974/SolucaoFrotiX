# wwwroot/js/site.js

**Mudanca:** GRANDE | **+72** linhas | **-49** linhas

---

```diff
--- JANEIRO: wwwroot/js/site.js
+++ ATUAL: wwwroot/js/site.js
@@ -1,27 +1,37 @@
 $(document).ready(function () {
+
     jQueryModalGet = (url, title) => {
         try {
+
             $.ajax({
                 type: 'GET',
                 url: url,
                 contentType: false,
                 processData: false,
                 success: function (res) {
+
                     $('#form-modal .modal-body').html(res.html);
+
                     $('#form-modal .modal-title').html(title);
+
                     $('#form-modal').modal('show');
                 },
                 error: function (err) {
-                    console.log(err);
-                },
-            });
+                    console.error('Erro em AJAX GET:', err);
+                    Alerta.TratamentoErroComLinha('site.js', 'jQueryModalGet.ajax.error', err);
+                }
+            })
             return false;
-        } catch (ex) {
-            console.log(ex);
+        } catch (erro) {
+            console.error('Erro em jQueryModalGet:', erro);
+            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalGet', erro);
+            return false;
         }
-    };
-    jQueryModalPost = (form) => {
+    }
+
+    jQueryModalPost = form => {
         try {
+
             $.ajax({
                 type: 'POST',
                 url: form.action,
@@ -29,23 +39,32 @@
                 contentType: false,
                 processData: false,
                 success: function (res) {
+
                     if (res.isValid) {
-                        $('#viewAll').html(res.html);
+
+                        $('#viewAll').html(res.html)
+
                         $('#form-modal').hide();
                     }
                 },
                 error: function (err) {
-                    console.log(err);
-                },
-            });
+                    console.error('Erro em AJAX POST:', err);
+                    Alerta.TratamentoErroComLinha('site.js', 'jQueryModalPost.ajax.error', err);
+                }
+            })
             return false;
-        } catch (ex) {
-            console.log(ex);
+        } catch (erro) {
+            console.error('Erro em jQueryModalPost:', erro);
+            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalPost', erro);
+            return false;
         }
-    };
-    jQueryModalDelete = (form) => {
-        if (confirm('Are you sure to delete this record ?')) {
-            try {
+    }
+
+    jQueryModalDelete = form => {
+        try {
+
+            if (confirm('Are you sure to delete this record ?')) {
+
                 $.ajax({
                     type: 'POST',
                     url: form.action,
@@ -53,77 +72,101 @@
                     contentType: false,
                     processData: false,
                     success: function (res) {
+
                         $('#viewAll').html(res.html);
                     },
                     error: function (err) {
-                        console.log(err);
-                    },
-                });
-            } catch (ex) {
-                console.log(ex);
+                        console.error('Erro em AJAX DELETE:', err);
+                        Alerta.TratamentoErroComLinha('site.js', 'jQueryModalDelete.ajax.error', err);
+                    }
+                })
             }
+            return false;
+        } catch (erro) {
+            console.error('Erro em jQueryModalDelete:', erro);
+            Alerta.TratamentoErroComLinha('site.js', 'jQueryModalDelete', erro);
+            return false;
         }
-        return false;
-    };
+    }
 });
 
 (function ($) {
 
     $(document).keydown(function (event) {
+        try {
 
-        if (event.ctrlKey && event.which === 81)
-            $('a[title*=Apps]').trigger('click');
+            if (event.ctrlKey && event.which === 81)
+
+                $("a[title*=Apps]").trigger("click");
+        } catch (erro) {
+            console.error('Erro em keydown handler:', erro);
+            Alerta.TratamentoErroComLinha('site.js', 'keydown.CTRL+Q', erro);
+        }
     });
 
     $.fn.DataTableEdit = function ($options) {
-        var options = $.extend(
-            {
-                dom:
-                    "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
+        try {
+
+            var options = $.extend({
+
+                dom: "<'row mb-3'<'col-sm-12 col-md-6 d-flex align-items-center justify-content-start'f><'col-sm-12 col-md-6 d-flex align-items-center justify-content-end'B>>" +
                     "<'row'<'col-sm-12'tr>>" +
                     "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
                 responsive: true,
                 serverSide: true,
                 altEditor: true,
                 pageLength: 10,
-                select: { style: 'single' },
+                select: { style: "single" },
+
                 buttons: [
                     {
                         extend: 'selected',
                         text: '<i class="fal fa-times mr-1"></i> Excluir',
                         name: 'delete',
-                        className: 'btn-vinho btn-sm mr-1',
+                        className: 'btn-vinho btn-sm mr-1'
                     },
                     {
                         extend: 'selected',
                         text: '<i class="fal fa-edit mr-1"></i> Editar',
                         name: 'edit',
-                        className: 'btn-warning btn-sm mr-1',
+                        className: 'btn-warning btn-sm mr-1'
                     },
                     {
                         text: '<i class="fal fa-plus mr-1"></i> Adicionar',
                         name: 'add',
-                        className: 'btn-info btn-sm mr-1',
+                        className: 'btn-info btn-sm mr-1'
                     },
                     {
                         text: '<i class="fal fa-sync mr-1"></i> Synchronize',
                         name: 'refresh',
-                        className: 'btn-azul btn-sm',
-                    },
-                ],
-            },
-            $options,
-        );
+                        className: 'btn-azul btn-sm'
+                    }
+                ]
+            }, $options);
 
-        return $(this)
-            .DataTable(options)
-            .on('init.dt', function () {
-                $('span[data-role=filter]')
-                    .off()
-                    .on('click', function () {
-                        const search = $(this).data('filter');
-                        if (table) table.search(search).draw();
+            return $(this).DataTable(options).on('init.dt', function () {
+                try {
+
+                    $("span[data-role=filter]").off().on("click", function () {
+                        try {
+
+                            const search = $(this).data("filter");
+                            if (table)
+                                table.search(search).draw();
+                        } catch (erro) {
+                            console.error('Erro em filter click:', erro);
+                            Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit.filter.click', erro);
+                        }
                     });
+                } catch (erro) {
+                    console.error('Erro em init.dt:', erro);
+                    Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit.init.dt', erro);
+                }
             });
+        } catch (erro) {
+            console.error('Erro em DataTableEdit:', erro);
+            Alerta.TratamentoErroComLinha('site.js', 'DataTableEdit', erro);
+            return null;
+        }
     };
-})(jQuery);
+}(jQuery));
```
