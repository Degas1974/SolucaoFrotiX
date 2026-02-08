# wwwroot/js/cadastros/itenscontrato.js

**Mudanca:** GRANDE | **+524** linhas | **-1392** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/itenscontrato.js
+++ ATUAL: wwwroot/js/cadastros/itenscontrato.js
@@ -35,11 +35,7 @@
                 }
                 ocultarTudo();
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'ddlStatus.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "ddlStatus.change", error);
             }
         });
 
@@ -52,11 +48,7 @@
                     ocultarTudo();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'ddlContrato.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "ddlContrato.change", error);
             }
         });
 
@@ -69,320 +61,177 @@
                     ocultarTudo();
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'ddlAta.change',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusVeiculo', function () {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "ddlAta.change", error);
+            }
+        });
+
+        $(document).on("click", ".updateStatusVeiculo", function () {
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
+                            AppToast.show('Verde', "Status alterado com sucesso!", 2000);
 
                             if (data.type == 1) {
 
-                                currentElement
-                                    .removeClass('btn-verde')
-                                    .addClass('fundo-cinza');
-                                currentElement.html(
-                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo',
-                                );
+                                currentElement.removeClass("btn-verde").addClass("fundo-cinza");
+                                currentElement.html('<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo');
                             } else {
 
-                                currentElement
-                                    .removeClass('fundo-cinza')
-                                    .addClass('btn-verde');
-                                currentElement.html(
-                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo',
-                                );
+                                currentElement.removeClass("fundo-cinza").addClass("btn-verde");
+                                currentElement.html('<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo');
                             }
 
                             atualizarContadoresStatus();
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Não foi possível alterar o status.',
-                                2000,
-                            );
+                            AppToast.show('Vermelho', "Não foi possível alterar o status.", 2000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusVeiculo.get.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusVeiculo.get.success", error);
                     }
                 }).fail(function () {
                     try {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status do veículo.',
-                            2000,
-                        );
+                        AppToast.show('Vermelho', "Erro ao alterar o status do veículo.", 2000);
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusVeiculo.get.fail',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusVeiculo.get.fail", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'updateStatusVeiculo.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusEncarregado', function () {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusVeiculo.click", error);
+            }
+        });
+
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
+                            AppToast.show('Verde', "Status alterado com sucesso!", 2000);
                             atualizarBadgeStatus(currentElement, data.type);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Não foi possível alterar o status.',
-                                2000,
-                            );
+                            AppToast.show('Vermelho', "Não foi possível alterar o status.", 2000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusEncarregado.get.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusEncarregado.get.success", error);
                     }
                 }).fail(function () {
                     try {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status do encarregado.',
-                            2000,
-                        );
+                        AppToast.show('Vermelho', "Erro ao alterar o status do encarregado.", 2000);
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusEncarregado.get.fail',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusEncarregado.get.fail", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'updateStatusEncarregado.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusOperador', function () {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusEncarregado.click", error);
+            }
+        });
+
+        $(document).on("click", ".updateStatusOperador", function () {
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
+                            AppToast.show('Verde', "Status alterado com sucesso!", 2000);
                             atualizarBadgeStatus(currentElement, data.type);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Não foi possível alterar o status.',
-                                2000,
-                            );
+                            AppToast.show('Vermelho', "Não foi possível alterar o status.", 2000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusOperador.get.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusOperador.get.success", error);
                     }
                 }).fail(function () {
                     try {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status do operador.',
-                            2000,
-                        );
+                        AppToast.show('Vermelho', "Erro ao alterar o status do operador.", 2000);
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusOperador.get.fail',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusOperador.get.fail", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'updateStatusOperador.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusMotorista', function () {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusOperador.click", error);
+            }
+        });
+
+        $(document).on("click", ".updateStatusMotorista", function () {
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
+                            AppToast.show('Verde', "Status alterado com sucesso!", 2000);
                             atualizarBadgeStatus(currentElement, data.type);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Não foi possível alterar o status.',
-                                2000,
-                            );
+                            AppToast.show('Vermelho', "Não foi possível alterar o status.", 2000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusMotorista.get.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusMotorista.get.success", error);
                     }
                 }).fail(function () {
                     try {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status do motorista.',
-                            2000,
-                        );
+                        AppToast.show('Vermelho', "Erro ao alterar o status do motorista.", 2000);
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusMotorista.get.fail',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusMotorista.get.fail", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'updateStatusMotorista.click',
-                    error,
-                );
-            }
-        });
-
-        $(document).on('click', '.updateStatusLavador', function () {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusMotorista.click", error);
+            }
+        });
+
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
+                            AppToast.show('Verde', "Status alterado com sucesso!", 2000);
                             atualizarBadgeStatus(currentElement, data.type);
                         } else {
-                            AppToast.show(
-                                'Vermelho',
-                                'Não foi possível alterar o status.',
-                                2000,
-                            );
+                            AppToast.show('Vermelho', "Não foi possível alterar o status.", 2000);
                         }
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusLavador.get.success',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusLavador.get.success", error);
                     }
                 }).fail(function () {
                     try {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao alterar o status do lavador.',
-                            2000,
-                        );
+                        AppToast.show('Vermelho', "Erro ao alterar o status do lavador.", 2000);
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'updateStatusLavador.get.fail',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusLavador.get.fail", error);
                     }
                 });
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'updateStatusLavador.click',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'document.ready',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "updateStatusLavador.click", error);
+            }
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "document.ready", error);
     }
 });
 
 function atualizarBadgeStatus(currentElement, type) {
     try {
         if (type == 1) {
-            currentElement.html(
-                '<i class="fa-duotone fa-circle-xmark me-1"></i> Inativo',
-            );
-            currentElement.removeClass('btn-verde').addClass('fundo-cinza');
+            currentElement.html('<i class="fa-duotone fa-circle-xmark me-1"></i> Inativo');
+            currentElement.removeClass("btn-verde").addClass("fundo-cinza");
         } else {
-            currentElement.html(
-                '<i class="fa-duotone fa-circle-check me-1"></i> Ativo',
-            );
-            currentElement.removeClass('fundo-cinza').addClass('btn-verde');
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'atualizarBadgeStatus',
-            error,
-        );
+            currentElement.html('<i class="fa-duotone fa-circle-check me-1"></i> Ativo');
+            currentElement.removeClass("fundo-cinza").addClass("btn-verde");
+        }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "atualizarBadgeStatus", error);
     }
 }
 
@@ -405,8 +254,9 @@
         }
 
         ocultarTudo();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('itenscontrato.js', 'switchTipo', error);
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "switchTipo", error);
     }
 }
 
@@ -418,29 +268,18 @@
         $('#tabstripContainer').removeClass('show');
         $('#cardContadoresVeiculos').removeClass('show');
 
-        if (dtVeiculos) {
-            dtVeiculos.clear().draw();
-        }
-        if (dtEncarregados) {
-            dtEncarregados.clear().draw();
-        }
-        if (dtOperadores) {
-            dtOperadores.clear().draw();
-        }
-        if (dtMotoristas) {
-            dtMotoristas.clear().draw();
-        }
-        if (dtLavadores) {
-            dtLavadores.clear().draw();
-        }
-
-        $(
-            '#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores',
-        ).hide();
+        if (dtVeiculos) { dtVeiculos.clear().draw(); }
+        if (dtEncarregados) { dtEncarregados.clear().draw(); }
+        if (dtOperadores) { dtOperadores.clear().draw(); }
+        if (dtMotoristas) { dtMotoristas.clear().draw(); }
+        if (dtLavadores) { dtLavadores.clear().draw(); }
+
+        $('#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores').hide();
         $('.nav-tabs-custom .nav-link').removeClass('active');
         $('.tab-pane').removeClass('show active');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('itenscontrato.js', 'ocultarTudo', error);
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "ocultarTudo", error);
     }
 }
 
@@ -448,11 +287,7 @@
     try {
         $('#shimmerContainer').addClass('show');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'mostrarShimmer',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "mostrarShimmer", error);
     }
 }
 
@@ -460,11 +295,7 @@
     try {
         $('#shimmerContainer').removeClass('show');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'ocultarShimmer',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "ocultarShimmer", error);
     }
 }
 
@@ -472,11 +303,7 @@
     try {
         $('#loadingOverlayContrato').css('display', 'flex');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'mostrarLoading',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "mostrarLoading", error);
     }
 }
 
@@ -484,89 +311,57 @@
     try {
         $('#loadingOverlayContrato').css('display', 'none');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'esconderLoading',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "esconderLoading", error);
     }
 }
 
 function loadContratos(status, contratoIdParaSelecionar) {
     try {
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/ListaContratos',
+            type: "GET",
+            url: "/api/ItensContrato/ListaContratos",
             data: { status: status },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione um Contrato --</option>';
+                    var options = '<option value="">-- Selecione um Contrato --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '" data-tipo="' +
-                                item.tipoContrato +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '" data-tipo="' + item.tipoContrato + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlContrato').html(options);
 
                     if (contratoIdParaSelecionar) {
                         $('#ddlContrato').val(contratoIdParaSelecionar);
-                        if (
-                            $('#ddlContrato').val() === contratoIdParaSelecionar
-                        ) {
+                        if ($('#ddlContrato').val() === contratoIdParaSelecionar) {
                             $('#ddlContrato').trigger('change');
                         }
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadContratos.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratos.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'loadContratos.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadContratos',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratos.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratos", error);
     }
 }
 
 function loadAtas(status, ataIdParaSelecionar) {
     try {
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/ListaAtas',
+            type: "GET",
+            url: "/api/ItensContrato/ListaAtas",
             data: { status: status },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione uma Ata --</option>';
+                    var options = '<option value="">-- Selecione uma Ata --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlAta').html(options);
@@ -578,23 +373,15 @@
                         }
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadAtas.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtas.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'loadAtas.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('itenscontrato.js', 'loadAtas', error);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtas.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtas", error);
     }
 }
 
@@ -604,8 +391,8 @@
         ocultarTudo();
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetContratoDetalhes',
+            type: "GET",
+            url: "/api/ItensContrato/GetContratoDetalhes",
             data: { id: id },
             success: function (res) {
                 try {
@@ -616,35 +403,19 @@
                         exibirResumoContrato(res.data);
                         configurarAbas(res.data);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar detalhes do contrato',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", "Erro ao carregar detalhes do contrato", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadContratoDetalhes.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratoDetalhes.success", error);
                 }
             },
             error: function (err) {
                 ocultarShimmer();
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'loadContratoDetalhes.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadContratoDetalhes',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratoDetalhes.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadContratoDetalhes", error);
     }
 }
 
@@ -668,20 +439,15 @@
         }
 
         $('#resumoContrato').addClass('show');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'exibirResumoContrato',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "exibirResumoContrato", error);
     }
 }
 
 function configurarAbas(data) {
     try {
-        $(
-            '#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores',
-        ).hide();
+        $('#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores').hide();
         $('#resumoServico').removeClass('show');
         $('#tabstripContainer').removeClass('show');
         $('#cardContadoresVeiculos').removeClass('show');
@@ -706,18 +472,15 @@
 
             loadItensContrato(data.contratoId);
             loadTblVeiculos(data.contratoId);
+
         } else if (tipoContrato === 'Terceirização') {
             var primeiraAba = null;
             var primeiroPane = null;
 
             if (data.contratoEncarregados) {
                 $('#navItemEncarregados').show();
-                $('#qtdContratadaEncarregados').text(
-                    data.quantidadeEncarregado || 0,
-                );
-                $('#custoMensalEncarregados').text(
-                    formatarMoeda(data.custoMensalEncarregado),
-                );
+                $('#qtdContratadaEncarregados').text(data.quantidadeEncarregado || 0);
+                $('#custoMensalEncarregados').text(formatarMoeda(data.custoMensalEncarregado));
                 if (!primeiraAba) {
                     primeiraAba = 'tabEncarregados';
                     primeiroPane = 'paneEncarregados';
@@ -726,12 +489,8 @@
 
             if (data.contratoOperadores) {
                 $('#navItemOperadores').show();
-                $('#qtdContratadaOperadores').text(
-                    data.quantidadeOperador || 0,
-                );
-                $('#custoMensalOperadores').text(
-                    formatarMoeda(data.custoMensalOperador),
-                );
+                $('#qtdContratadaOperadores').text(data.quantidadeOperador || 0);
+                $('#custoMensalOperadores').text(formatarMoeda(data.custoMensalOperador));
                 if (!primeiraAba) {
                     primeiraAba = 'tabOperadores';
                     primeiroPane = 'paneOperadores';
@@ -740,12 +499,8 @@
 
             if (data.contratoMotoristas) {
                 $('#navItemMotoristas').show();
-                $('#qtdContratadaMotoristas').text(
-                    data.quantidadeMotorista || 0,
-                );
-                $('#custoMensalMotoristas').text(
-                    formatarMoeda(data.custoMensalMotorista),
-                );
+                $('#qtdContratadaMotoristas').text(data.quantidadeMotorista || 0);
+                $('#custoMensalMotoristas').text(formatarMoeda(data.custoMensalMotorista));
                 if (!primeiraAba) {
                     primeiraAba = 'tabMotoristas';
                     primeiroPane = 'paneMotoristas';
@@ -755,9 +510,7 @@
             if (data.contratoLavadores) {
                 $('#navItemLavadores').show();
                 $('#qtdContratadaLavadores').text(data.quantidadeLavador || 0);
-                $('#custoMensalLavadores').text(
-                    formatarMoeda(data.custoMensalLavador),
-                );
+                $('#custoMensalLavadores').text(formatarMoeda(data.custoMensalLavador));
                 if (!primeiraAba) {
                     primeiraAba = 'tabLavadores';
                     primeiroPane = 'paneLavadores';
@@ -768,46 +521,33 @@
                 $('#' + primeiraAba).addClass('active');
                 $('#' + primeiroPane).addClass('show active');
 
-                if (primeiraAba === 'tabEncarregados')
-                    loadTblEncarregados(data.contratoId);
-                else if (primeiraAba === 'tabOperadores')
-                    loadTblOperadores(data.contratoId);
-                else if (primeiraAba === 'tabMotoristas')
-                    loadTblMotoristas(data.contratoId);
-                else if (primeiraAba === 'tabLavadores')
-                    loadTblLavadores(data.contratoId);
-            }
-        }
-
-        $('.nav-tabs-custom .nav-link')
-            .off('shown.bs.tab')
-            .on('shown.bs.tab', function (e) {
-                try {
-                    var target = $(e.target).attr('id');
-                    var id = contratoAtual ? contratoAtual.contratoId : null;
-
-                    if (!id) return;
-
-                    if (target === 'tabVeiculos') loadTblVeiculos(id);
-                    else if (target === 'tabEncarregados')
-                        loadTblEncarregados(id);
-                    else if (target === 'tabOperadores') loadTblOperadores(id);
-                    else if (target === 'tabMotoristas') loadTblMotoristas(id);
-                    else if (target === 'tabLavadores') loadTblLavadores(id);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'nav-link.shown',
-                        error,
-                    );
-                }
-            });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'configurarAbas',
-            error,
-        );
+                if (primeiraAba === 'tabEncarregados') loadTblEncarregados(data.contratoId);
+                else if (primeiraAba === 'tabOperadores') loadTblOperadores(data.contratoId);
+                else if (primeiraAba === 'tabMotoristas') loadTblMotoristas(data.contratoId);
+                else if (primeiraAba === 'tabLavadores') loadTblLavadores(data.contratoId);
+            }
+        }
+
+        $('.nav-tabs-custom .nav-link').off('shown.bs.tab').on('shown.bs.tab', function (e) {
+            try {
+                var target = $(e.target).attr('id');
+                var id = contratoAtual ? contratoAtual.contratoId : null;
+
+                if (!id) return;
+
+                if (target === 'tabVeiculos') loadTblVeiculos(id);
+                else if (target === 'tabEncarregados') loadTblEncarregados(id);
+                else if (target === 'tabOperadores') loadTblOperadores(id);
+                else if (target === 'tabMotoristas') loadTblMotoristas(id);
+                else if (target === 'tabLavadores') loadTblLavadores(id);
+
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "nav-link.shown", error);
+            }
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "configurarAbas", error);
     }
 }
 
@@ -817,8 +557,8 @@
         ocultarTudo();
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetAtaDetalhes',
+            type: "GET",
+            url: "/api/ItensContrato/GetAtaDetalhes",
             data: { id: id },
             success: function (res) {
                 try {
@@ -829,35 +569,19 @@
                         exibirResumoAta(res.data);
                         configurarAbasAta(res.data);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao carregar detalhes da ata',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", "Erro ao carregar detalhes da ata", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadAtaDetalhes.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtaDetalhes.success", error);
                 }
             },
             error: function (err) {
                 ocultarShimmer();
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'loadAtaDetalhes.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadAtaDetalhes',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtaDetalhes.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadAtaDetalhes", error);
     }
 }
 
@@ -870,20 +594,15 @@
         $('#resumoAtaValor').text(formatarMoeda(data.valor));
 
         $('#resumoAta').addClass('show');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'exibirResumoAta',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "exibirResumoAta", error);
     }
 }
 
 function configurarAbasAta(data) {
     try {
-        $(
-            '#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores',
-        ).hide();
+        $('#navItemVeiculos, #navItemEncarregados, #navItemOperadores, #navItemMotoristas, #navItemLavadores').hide();
         $('#resumoServico').removeClass('show');
 
         $('.nav-tabs-custom .nav-link').removeClass('active');
@@ -898,40 +617,29 @@
         $('#spanCustoVeiculos').hide();
 
         loadTblVeiculosAta(data.ataId);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'configurarAbasAta',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "configurarAbasAta", error);
     }
 }
 
 function loadItensContrato(contratoId) {
     try {
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetItensContrato',
+            type: "GET",
+            url: "/api/ItensContrato/GetItensContrato",
             data: { contratoId: contratoId },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Item --</option>';
+                    var options = '<option value="">-- Selecione o Item --</option>';
                     var qtdTotal = 0;
                     var custoTotal = 0;
 
                     if (res && res.success && res.data && res.data.length > 0) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
-                            qtdTotal += item.quantidade || 0;
-                            custoTotal +=
-                                (item.quantidade || 0) *
-                                (item.valorUnitario || 0);
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
+                            qtdTotal += (item.quantidade || 0);
+                            custoTotal += (item.quantidade || 0) * (item.valorUnitario || 0);
                         });
                         $('#divItemVeiculo').show();
                     } else {
@@ -941,28 +649,17 @@
                     $('#ddlItemVeiculo').html(options);
                     $('#qtdContratadaVeiculos').text(qtdTotal);
                     $('#custoMensalVeiculos').text(formatarMoeda(custoTotal));
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadItensContrato.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadItensContrato.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'loadItensContrato.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadItensContrato',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "loadItensContrato.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadItensContrato", error);
     }
 }
 
@@ -982,9 +679,9 @@
 
         dtVeiculos = $('#tblVeiculos').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetVeiculosContrato',
+                url: "/api/ItensContrato/GetVeiculosContrato",
                 data: { contratoId: contratoId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
@@ -996,29 +693,21 @@
 
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblVeiculos.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculos.dataSrc", error);
                         return [];
                     }
                 },
                 error: function (xhr, error, thrown) {
                     esconderLoading();
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadTblVeiculos.ajax.error',
-                        error,
-                    );
-                },
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculos.ajax.error", error);
+                }
             },
             columns: [
                 {
                     data: 'descricaoItem',
                     render: function (data) {
                         return data || '-';
-                    },
+                    }
                 },
                 { data: 'placa' },
                 { data: 'marcaModelo' },
@@ -1027,40 +716,24 @@
                     className: 'text-center',
                     render: function (data, type, row) {
                         if (data) {
-                            return (
-                                '<a href="javascript:void(0)" class="updateStatusVeiculo btn btn-verde text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
-                                row.veiculoId +
-                                '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                            );
+                            return '<a href="javascript:void(0)" class="updateStatusVeiculo btn btn-verde text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' + row.veiculoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                   '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                         } else {
-                            return (
-                                '<a href="javascript:void(0)" class="updateStatusVeiculo btn fundo-cinza text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
-                                row.veiculoId +
-                                '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo</a>'
-                            );
-                        }
-                    },
+                            return '<a href="javascript:void(0)" class="updateStatusVeiculo btn fundo-cinza text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' + row.veiculoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                   '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo</a>';
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-action btn-danger" onclick="removerVeiculo(\'' +
-                            row.veiculoId +
-                            "', '" +
-                            row.contratoId +
-                            '\')" title="Remover"><i class="fad fa-trash-alt"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-action btn-danger" onclick="removerVeiculo(\'' + row.veiculoId + '\', \'' + row.contratoId + '\')" title="Remover"><i class="fad fa-trash-alt"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[1, 'asc']],
             responsive: true,
             dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
@@ -1069,15 +742,12 @@
             },
             drawCallback: function () {
                 esconderLoading();
-            },
-        });
+            }
+        });
+
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblVeiculos',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculos", error);
     }
 }
 
@@ -1092,9 +762,9 @@
 
         dtVeiculos = $('#tblVeiculos').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetVeiculosAta',
+                url: "/api/ItensContrato/GetVeiculosAta",
                 data: { ataId: ataId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
@@ -1107,29 +777,19 @@
 
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblVeiculosAta.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculosAta.dataSrc", error);
                         return [];
                     }
                 },
                 error: function (xhr, error, thrown) {
                     esconderLoading();
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'loadTblVeiculosAta.ajax.error',
-                        error,
-                    );
-                },
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculosAta.ajax.error", error);
+                }
             },
             columns: [
                 {
                     data: 'descricaoItem',
-                    render: function (data) {
-                        return data || '-';
-                    },
+                    render: function (data) { return data || '-'; }
                 },
                 { data: 'placa' },
                 { data: 'marcaModelo' },
@@ -1138,40 +798,24 @@
                     className: 'text-center',
                     render: function (data, type, row) {
                         if (data) {
-                            return (
-                                '<a href="javascript:void(0)" class="updateStatusVeiculo btn btn-verde text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
-                                row.veiculoId +
-                                '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                            );
+                            return '<a href="javascript:void(0)" class="updateStatusVeiculo btn btn-verde text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' + row.veiculoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                   '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                         } else {
-                            return (
-                                '<a href="javascript:void(0)" class="updateStatusVeiculo btn fundo-cinza text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' +
-                                row.veiculoId +
-                                '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo</a>'
-                            );
-                        }
-                    },
+                            return '<a href="javascript:void(0)" class="updateStatusVeiculo btn fundo-cinza text-white" data-url="/api/Veiculo/updateStatusVeiculo?Id=' + row.veiculoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                   '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ffcdd2;"></i> Inativo</a>';
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-action btn-danger" onclick="removerVeiculoAta(\'' +
-                            row.veiculoId +
-                            "', '" +
-                            row.ataId +
-                            '\')" title="Remover"><i class="fad fa-trash-alt"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-action btn-danger" onclick="removerVeiculoAta(\'' + row.veiculoId + '\', \'' + row.ataId + '\')" title="Remover"><i class="fad fa-trash-alt"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[1, 'asc']],
             responsive: true,
             dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
@@ -1180,15 +824,12 @@
             },
             drawCallback: function () {
                 esconderLoading();
-            },
-        });
+            }
+        });
+
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblVeiculosAta',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblVeiculosAta", error);
     }
 }
 
@@ -1198,44 +839,26 @@
         $ddl.empty().append('<option value="">-- Todos os Itens --</option>');
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetItensContrato',
+            type: "GET",
+            url: "/api/ItensContrato/GetItensContrato",
             data: { contratoId: contratoId },
             success: function (res) {
                 try {
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            $ddl.append(
-                                '<option value="' +
-                                    item.text +
-                                    '">' +
-                                    item.text +
-                                    '</option>',
-                            );
+                            $ddl.append('<option value="' + item.text + '">' + item.text + '</option>');
                         });
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'carregarFiltroItens.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "carregarFiltroItens.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'carregarFiltroItens.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'carregarFiltroItens',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "carregarFiltroItens.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "carregarFiltroItens", error);
     }
 }
 
@@ -1251,11 +874,7 @@
         }
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'filtrarPorItem',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "filtrarPorItem", error);
     }
 }
 
@@ -1270,11 +889,7 @@
         }
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'limparFiltroItem',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "limparFiltroItem", error);
     }
 }
 
@@ -1302,11 +917,7 @@
             $('#qtdVeiculosInativos').text(inativos);
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'atualizarContadoresStatus',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "atualizarContadoresStatus", error);
     }
 }
 
@@ -1319,23 +930,19 @@
 
         dtEncarregados = $('#tblEncarregados').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetEncarregadosContrato',
+                url: "/api/ItensContrato/GetEncarregadosContrato",
                 data: { contratoId: contratoId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
                         $('#badgeEncarregados').text(data.length);
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblEncarregados.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblEncarregados.dataSrc", error);
                         return [];
                     }
-                },
+                }
             },
             columns: [
                 { data: 'nome' },
@@ -1347,58 +954,35 @@
                     render: function (data, type, row) {
                         try {
                             if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusEncarregado btn btn-verde text-white" data-url="/api/Encarregado/UpdateStatusEncarregado?Id=' +
-                                    row.encarregadoId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusEncarregado btn btn-verde text-white" data-url="/api/Encarregado/UpdateStatusEncarregado?Id=' + row.encarregadoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                             } else {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusEncarregado btn fundo-cinza text-white" data-url="/api/Encarregado/UpdateStatusEncarregado?Id=' +
-                                    row.encarregadoId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusEncarregado btn fundo-cinza text-white" data-url="/api/Encarregado/UpdateStatusEncarregado?Id=' + row.encarregadoId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>';
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'loadTblEncarregados.render.status',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblEncarregados.render.status", error);
+                            return "";
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center ftx-actions',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularEncarregado(\'' +
-                            row.encarregadoId +
-                            "', '" +
-                            row.contratoId +
-                            '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularEncarregado(\'' + row.encarregadoId + '\', \'' + row.contratoId + '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[0, 'asc']],
             responsive: true,
-            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblEncarregados',
-            error,
-        );
+            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip'
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblEncarregados", error);
     }
 }
 
@@ -1411,23 +995,19 @@
 
         dtOperadores = $('#tblOperadores').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetOperadoresContrato',
+                url: "/api/ItensContrato/GetOperadoresContrato",
                 data: { contratoId: contratoId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
                         $('#badgeOperadores').text(data.length);
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblOperadores.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblOperadores.dataSrc", error);
                         return [];
                     }
-                },
+                }
             },
             columns: [
                 { data: 'nome' },
@@ -1439,58 +1019,35 @@
                     render: function (data, type, row) {
                         try {
                             if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusOperador btn btn-verde text-white" data-url="/api/Operador/UpdateStatusOperador?Id=' +
-                                    row.operadorId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusOperador btn btn-verde text-white" data-url="/api/Operador/UpdateStatusOperador?Id=' + row.operadorId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                             } else {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusOperador btn fundo-cinza text-white" data-url="/api/Operador/UpdateStatusOperador?Id=' +
-                                    row.operadorId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusOperador btn fundo-cinza text-white" data-url="/api/Operador/UpdateStatusOperador?Id=' + row.operadorId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>';
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'loadTblOperadores.render.status',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblOperadores.render.status", error);
+                            return "";
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center ftx-actions',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularOperador(\'' +
-                            row.operadorId +
-                            "', '" +
-                            row.contratoId +
-                            '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularOperador(\'' + row.operadorId + '\', \'' + row.contratoId + '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[0, 'asc']],
             responsive: true,
-            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblOperadores',
-            error,
-        );
+            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip'
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblOperadores", error);
     }
 }
 
@@ -1503,23 +1060,19 @@
 
         dtMotoristas = $('#tblMotoristas').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetMotoristasContrato',
+                url: "/api/ItensContrato/GetMotoristasContrato",
                 data: { contratoId: contratoId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
                         $('#badgeMotoristas').text(data.length);
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblMotoristas.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblMotoristas.dataSrc", error);
                         return [];
                     }
-                },
+                }
             },
             columns: [
                 { data: 'nome' },
@@ -1532,58 +1085,35 @@
                     render: function (data, type, row) {
                         try {
                             if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusMotorista btn btn-verde text-white" data-url="/api/Motorista/UpdateStatusMotorista?Id=' +
-                                    row.motoristaId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusMotorista btn btn-verde text-white" data-url="/api/Motorista/UpdateStatusMotorista?Id=' + row.motoristaId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                             } else {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusMotorista btn fundo-cinza text-white" data-url="/api/Motorista/UpdateStatusMotorista?Id=' +
-                                    row.motoristaId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusMotorista btn fundo-cinza text-white" data-url="/api/Motorista/UpdateStatusMotorista?Id=' + row.motoristaId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>';
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'loadTblMotoristas.render.status',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblMotoristas.render.status", error);
+                            return "";
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center ftx-actions',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularMotorista(\'' +
-                            row.motoristaId +
-                            "', '" +
-                            row.contratoId +
-                            '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularMotorista(\'' + row.motoristaId + '\', \'' + row.contratoId + '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[0, 'asc']],
             responsive: true,
-            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblMotoristas',
-            error,
-        );
+            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip'
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblMotoristas", error);
     }
 }
 
@@ -1596,23 +1126,19 @@
 
         dtLavadores = $('#tblLavadores').DataTable({
             ajax: {
-                url: '/api/ItensContrato/GetLavadoresContrato',
+                url: "/api/ItensContrato/GetLavadoresContrato",
                 data: { contratoId: contratoId },
-                type: 'GET',
+                type: "GET",
                 dataSrc: function (json) {
                     try {
                         var data = json.data || [];
                         $('#badgeLavadores').text(data.length);
                         return data;
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'loadTblLavadores.dataSrc',
-                            error,
-                        );
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblLavadores.dataSrc", error);
                         return [];
                     }
-                },
+                }
             },
             columns: [
                 { data: 'nome' },
@@ -1624,348 +1150,214 @@
                     render: function (data, type, row) {
                         try {
                             if (data) {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusLavador btn btn-verde text-white" data-url="/api/Lavador/UpdateStatusLavador?Id=' +
-                                    row.lavadorId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusLavador btn btn-verde text-white" data-url="/api/Lavador/UpdateStatusLavador?Id=' + row.lavadorId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-check me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#c8e6c9;"></i> Ativo</a>';
                             } else {
-                                return (
-                                    '<a href="javascript:void(0)" class="updateStatusLavador btn fundo-cinza text-white" data-url="/api/Lavador/UpdateStatusLavador?Id=' +
-                                    row.lavadorId +
-                                    '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
-                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>'
-                                );
+                                return '<a href="javascript:void(0)" class="updateStatusLavador btn fundo-cinza text-white" data-url="/api/Lavador/UpdateStatusLavador?Id=' + row.lavadorId + '" style="cursor:pointer; padding: 4px 10px; font-size: 12px; border-radius: 6px;">' +
+                                    '<i class="fa-duotone fa-circle-xmark me-1" style="--fa-primary-color:#fff; --fa-secondary-color:#ccc;"></i> Inativo</a>';
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'loadTblLavadores.render.status',
-                                error,
-                            );
-                            return '';
-                        }
-                    },
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblLavadores.render.status", error);
+                            return "";
+                        }
+                    }
                 },
                 {
                     data: null,
                     orderable: false,
                     className: 'text-center ftx-actions',
                     render: function (data, type, row) {
-                        return (
-                            '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularLavador(\'' +
-                            row.lavadorId +
-                            "', '" +
-                            row.contratoId +
-                            '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>'
-                        );
-                    },
-                },
+                        return '<button type="button" class="btn-icon-28 btn-terracota" onclick="desvincularLavador(\'' + row.lavadorId + '\', \'' + row.contratoId + '\')" data-ejtip="Desvincular do Contrato"><i class="fa-duotone fa-link-slash"></i></button>';
+                    }
+                }
             ],
-            language: {
-                url: '
-            },
+            language: { url: '
             order: [[0, 'asc']],
             responsive: true,
-            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'loadTblLavadores',
-            error,
-        );
+            dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip'
+        });
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "loadTblLavadores", error);
     }
 }
 
 function abrirModalVeiculo() {
     try {
-        var id =
-            tipoSelecionado === 'contrato'
-                ? contratoAtual?.contratoId
-                : ataAtual?.ataId;
+        var id = tipoSelecionado === 'contrato' ? contratoAtual?.contratoId : ataAtual?.ataId;
         if (!id) {
-            AppToast.show(
-                'Amarelo',
-                'Selecione um contrato ou ata primeiro',
-                3000,
-            );
+            AppToast.show("Amarelo", "Selecione um contrato ou ata primeiro", 3000);
             return;
         }
 
-        var url =
-            tipoSelecionado === 'contrato'
-                ? '/api/ItensContrato/GetVeiculosDisponiveis'
-                : '/api/ItensContrato/GetVeiculosDisponiveisAta';
-        var param =
-            tipoSelecionado === 'contrato' ? { contratoId: id } : { ataId: id };
+        var url = tipoSelecionado === 'contrato' ? '/api/ItensContrato/GetVeiculosDisponiveis' : '/api/ItensContrato/GetVeiculosDisponiveisAta';
+        var param = tipoSelecionado === 'contrato' ? { contratoId: id } : { ataId: id };
 
         $.ajax({
-            type: 'GET',
+            type: "GET",
             url: url,
             data: param,
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Veículo --</option>';
+                    var options = '<option value="">-- Selecione o Veículo --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlVeiculo').html(options);
-                    $('#btnSalvarVeiculo')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarVeiculo').removeClass('loading').prop('disabled', false);
                     $('#modalVeiculo').modal('show');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'abrirModalVeiculo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalVeiculo.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'abrirModalVeiculo.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'abrirModalVeiculo',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalVeiculo.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalVeiculo", error);
     }
 }
 
 function abrirModalEncarregado() {
     try {
         if (!contratoAtual?.contratoId) {
-            AppToast.show('Amarelo', 'Selecione um contrato primeiro', 3000);
+            AppToast.show("Amarelo", "Selecione um contrato primeiro", 3000);
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetEncarregadosDisponiveis',
+            type: "GET",
+            url: "/api/ItensContrato/GetEncarregadosDisponiveis",
             data: { contratoId: contratoAtual.contratoId },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Encarregado --</option>';
+                    var options = '<option value="">-- Selecione o Encarregado --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlEncarregado').html(options);
-                    $('#btnSalvarEncarregado')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarEncarregado').removeClass('loading').prop('disabled', false);
                     $('#modalEncarregado').modal('show');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'abrirModalEncarregado.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalEncarregado.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'abrirModalEncarregado.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'abrirModalEncarregado',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalEncarregado.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalEncarregado", error);
     }
 }
 
 function abrirModalOperador() {
     try {
         if (!contratoAtual?.contratoId) {
-            AppToast.show('Amarelo', 'Selecione um contrato primeiro', 3000);
+            AppToast.show("Amarelo", "Selecione um contrato primeiro", 3000);
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetOperadoresDisponiveis',
+            type: "GET",
+            url: "/api/ItensContrato/GetOperadoresDisponiveis",
             data: { contratoId: contratoAtual.contratoId },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Operador --</option>';
+                    var options = '<option value="">-- Selecione o Operador --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlOperador').html(options);
-                    $('#btnSalvarOperador')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarOperador').removeClass('loading').prop('disabled', false);
                     $('#modalOperador').modal('show');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'abrirModalOperador.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalOperador.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'abrirModalOperador.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'abrirModalOperador',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalOperador.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalOperador", error);
     }
 }
 
 function abrirModalMotorista() {
     try {
         if (!contratoAtual?.contratoId) {
-            AppToast.show('Amarelo', 'Selecione um contrato primeiro', 3000);
+            AppToast.show("Amarelo", "Selecione um contrato primeiro", 3000);
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetMotoristasDisponiveis',
+            type: "GET",
+            url: "/api/ItensContrato/GetMotoristasDisponiveis",
             data: { contratoId: contratoAtual.contratoId },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Motorista --</option>';
+                    var options = '<option value="">-- Selecione o Motorista --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlMotorista').html(options);
-                    $('#btnSalvarMotorista')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarMotorista').removeClass('loading').prop('disabled', false);
                     $('#modalMotorista').modal('show');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'abrirModalMotorista.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalMotorista.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'abrirModalMotorista.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'abrirModalMotorista',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalMotorista.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalMotorista", error);
     }
 }
 
 function abrirModalLavador() {
     try {
         if (!contratoAtual?.contratoId) {
-            AppToast.show('Amarelo', 'Selecione um contrato primeiro', 3000);
+            AppToast.show("Amarelo", "Selecione um contrato primeiro", 3000);
             return;
         }
 
         $.ajax({
-            type: 'GET',
-            url: '/api/ItensContrato/GetLavadoresDisponiveis',
+            type: "GET",
+            url: "/api/ItensContrato/GetLavadoresDisponiveis",
             data: { contratoId: contratoAtual.contratoId },
             success: function (res) {
                 try {
-                    var options =
-                        '<option value="">-- Selecione o Lavador --</option>';
+                    var options = '<option value="">-- Selecione o Lavador --</option>';
                     if (res && res.success && res.data) {
                         res.data.forEach(function (item) {
-                            options +=
-                                '<option value="' +
-                                item.value +
-                                '">' +
-                                item.text +
-                                '</option>';
+                            options += '<option value="' + item.value + '">' + item.text + '</option>';
                         });
                     }
                     $('#ddlLavador').html(options);
-                    $('#btnSalvarLavador')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarLavador').removeClass('loading').prop('disabled', false);
                     $('#modalLavador').modal('show');
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'abrirModalLavador.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalLavador.success", error);
                 }
             },
             error: function (err) {
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'abrirModalLavador.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'abrirModalLavador',
-            error,
-        );
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalLavador.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "abrirModalLavador", error);
     }
 }
 
@@ -1973,7 +1365,7 @@
     try {
         var veiculoId = $('#ddlVeiculo').val();
         if (!veiculoId) {
-            AppToast.show('Amarelo', 'Selecione um veículo', 3000);
+            AppToast.show("Amarelo", "Selecione um veículo", 3000);
             return;
         }
 
@@ -1986,28 +1378,26 @@
             data = {
                 veiculoId: veiculoId,
                 contratoId: contratoAtual.contratoId,
-                itemVeiculoId: $('#ddlItemVeiculo').val() || null,
+                itemVeiculoId: $('#ddlItemVeiculo').val() || null
             };
         } else {
             url = '/api/ItensContrato/IncluirVeiculoAta';
             data = {
                 veiculoId: veiculoId,
-                ataId: ataAtual.ataId,
+                ataId: ataAtual.ataId
             };
         }
 
         $.ajax({
-            type: 'POST',
+            type: "POST",
             url: url,
-            contentType: 'application/json',
+            contentType: "application/json",
             data: JSON.stringify(data),
             success: function (res) {
                 try {
-                    $('#btnSalvarVeiculo')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarVeiculo').removeClass('loading').prop('disabled', false);
                     if (res && res.success) {
-                        AppToast.show('Verde', res.message, 3000);
+                        AppToast.show("Verde", res.message, 3000);
                         $('#modalVeiculo').modal('hide');
 
                         if (tipoSelecionado === 'contrato') {
@@ -2016,37 +1406,19 @@
                             loadTblVeiculosAta(ataAtual.ataId);
                         }
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao incluir veículo',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", res.message || "Erro ao incluir veículo", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'salvarVeiculo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarVeiculo.success", error);
                 }
             },
             error: function (err) {
-                $('#btnSalvarVeiculo')
-                    .removeClass('loading')
-                    .prop('disabled', false);
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'salvarVeiculo.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'salvarVeiculo',
-            error,
-        );
+                $('#btnSalvarVeiculo').removeClass('loading').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarVeiculo.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarVeiculo", error);
     }
 }
 
@@ -2054,61 +1426,41 @@
     try {
         var encarregadoId = $('#ddlEncarregado').val();
         if (!encarregadoId) {
-            AppToast.show('Amarelo', 'Selecione um encarregado', 3000);
+            AppToast.show("Amarelo", "Selecione um encarregado", 3000);
             return;
         }
 
         $('#btnSalvarEncarregado').addClass('loading').prop('disabled', true);
 
         $.ajax({
-            type: 'POST',
-            url: '/api/ItensContrato/IncluirEncarregadoContrato',
-            contentType: 'application/json',
+            type: "POST",
+            url: "/api/ItensContrato/IncluirEncarregadoContrato",
+            contentType: "application/json",
             data: JSON.stringify({
                 encarregadoId: encarregadoId,
-                contratoId: contratoAtual.contratoId,
+                contratoId: contratoAtual.contratoId
             }),
             success: function (res) {
                 try {
-                    $('#btnSalvarEncarregado')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarEncarregado').removeClass('loading').prop('disabled', false);
                     if (res && res.success) {
-                        AppToast.show('Verde', res.message, 3000);
+                        AppToast.show("Verde", res.message, 3000);
                         $('#modalEncarregado').modal('hide');
                         loadTblEncarregados(contratoAtual.contratoId);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao incluir encarregado',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", res.message || "Erro ao incluir encarregado", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'salvarEncarregado.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarEncarregado.success", error);
                 }
             },
             error: function (err) {
-                $('#btnSalvarEncarregado')
-                    .removeClass('loading')
-                    .prop('disabled', false);
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'salvarEncarregado.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'salvarEncarregado',
-            error,
-        );
+                $('#btnSalvarEncarregado').removeClass('loading').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarEncarregado.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarEncarregado", error);
     }
 }
 
@@ -2116,61 +1468,41 @@
     try {
         var operadorId = $('#ddlOperador').val();
         if (!operadorId) {
-            AppToast.show('Amarelo', 'Selecione um operador', 3000);
+            AppToast.show("Amarelo", "Selecione um operador", 3000);
             return;
         }
 
         $('#btnSalvarOperador').addClass('loading').prop('disabled', true);
 
         $.ajax({
-            type: 'POST',
-            url: '/api/ItensContrato/IncluirOperadorContrato',
-            contentType: 'application/json',
+            type: "POST",
+            url: "/api/ItensContrato/IncluirOperadorContrato",
+            contentType: "application/json",
             data: JSON.stringify({
                 operadorId: operadorId,
-                contratoId: contratoAtual.contratoId,
+                contratoId: contratoAtual.contratoId
             }),
             success: function (res) {
                 try {
-                    $('#btnSalvarOperador')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarOperador').removeClass('loading').prop('disabled', false);
                     if (res && res.success) {
-                        AppToast.show('Verde', res.message, 3000);
+                        AppToast.show("Verde", res.message, 3000);
                         $('#modalOperador').modal('hide');
                         loadTblOperadores(contratoAtual.contratoId);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao incluir operador',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", res.message || "Erro ao incluir operador", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'salvarOperador.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarOperador.success", error);
                 }
             },
             error: function (err) {
-                $('#btnSalvarOperador')
-                    .removeClass('loading')
-                    .prop('disabled', false);
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'salvarOperador.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'salvarOperador',
-            error,
-        );
+                $('#btnSalvarOperador').removeClass('loading').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarOperador.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarOperador", error);
     }
 }
 
@@ -2178,61 +1510,41 @@
     try {
         var motoristaId = $('#ddlMotorista').val();
         if (!motoristaId) {
-            AppToast.show('Amarelo', 'Selecione um motorista', 3000);
+            AppToast.show("Amarelo", "Selecione um motorista", 3000);
             return;
         }
 
         $('#btnSalvarMotorista').addClass('loading').prop('disabled', true);
 
         $.ajax({
-            type: 'POST',
-            url: '/api/ItensContrato/IncluirMotoristaContrato',
-            contentType: 'application/json',
+            type: "POST",
+            url: "/api/ItensContrato/IncluirMotoristaContrato",
+            contentType: "application/json",
             data: JSON.stringify({
                 motoristaId: motoristaId,
-                contratoId: contratoAtual.contratoId,
+                contratoId: contratoAtual.contratoId
             }),
             success: function (res) {
                 try {
-                    $('#btnSalvarMotorista')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarMotorista').removeClass('loading').prop('disabled', false);
                     if (res && res.success) {
-                        AppToast.show('Verde', res.message, 3000);
+                        AppToast.show("Verde", res.message, 3000);
                         $('#modalMotorista').modal('hide');
                         loadTblMotoristas(contratoAtual.contratoId);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao incluir motorista',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", res.message || "Erro ao incluir motorista", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'salvarMotorista.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarMotorista.success", error);
                 }
             },
             error: function (err) {
-                $('#btnSalvarMotorista')
-                    .removeClass('loading')
-                    .prop('disabled', false);
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'salvarMotorista.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'salvarMotorista',
-            error,
-        );
+                $('#btnSalvarMotorista').removeClass('loading').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarMotorista.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarMotorista", error);
     }
 }
 
@@ -2240,436 +1552,272 @@
     try {
         var lavadorId = $('#ddlLavador').val();
         if (!lavadorId) {
-            AppToast.show('Amarelo', 'Selecione um lavador', 3000);
+            AppToast.show("Amarelo", "Selecione um lavador", 3000);
             return;
         }
 
         $('#btnSalvarLavador').addClass('loading').prop('disabled', true);
 
         $.ajax({
-            type: 'POST',
-            url: '/api/ItensContrato/IncluirLavadorContrato',
-            contentType: 'application/json',
+            type: "POST",
+            url: "/api/ItensContrato/IncluirLavadorContrato",
+            contentType: "application/json",
             data: JSON.stringify({
                 lavadorId: lavadorId,
-                contratoId: contratoAtual.contratoId,
+                contratoId: contratoAtual.contratoId
             }),
             success: function (res) {
                 try {
-                    $('#btnSalvarLavador')
-                        .removeClass('loading')
-                        .prop('disabled', false);
+                    $('#btnSalvarLavador').removeClass('loading').prop('disabled', false);
                     if (res && res.success) {
-                        AppToast.show('Verde', res.message, 3000);
+                        AppToast.show("Verde", res.message, 3000);
                         $('#modalLavador').modal('hide');
                         loadTblLavadores(contratoAtual.contratoId);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao incluir lavador',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", res.message || "Erro ao incluir lavador", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'itenscontrato.js',
-                        'salvarLavador.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarLavador.success", error);
                 }
             },
             error: function (err) {
-                $('#btnSalvarLavador')
-                    .removeClass('loading')
-                    .prop('disabled', false);
-                Alerta.TratamentoErroComLinha(
-                    'itenscontrato.js',
-                    'salvarLavador.error',
-                    err,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'salvarLavador',
-            error,
-        );
+                $('#btnSalvarLavador').removeClass('loading').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarLavador.error", err);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "salvarLavador", error);
     }
 }
 
 function removerVeiculo(veiculoId, contratoId) {
     try {
         Alerta.Confirmar(
-            'Deseja realmente remover este veículo do contrato?',
-            'Esta ação não poderá ser desfeita!',
-            'Sim, remover',
-            'Cancelar',
+            "Deseja realmente remover este veículo do contrato?",
+            "Esta ação não poderá ser desfeita!",
+            "Sim, remover",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverVeiculoContrato',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        veiculoId: veiculoId,
-                        contratoId: contratoId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverVeiculoContrato",
+                    contentType: "application/json",
+                    data: JSON.stringify({ veiculoId: veiculoId, contratoId: contratoId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show('Verde', res.message, 3000);
+                                AppToast.show("Verde", res.message, 3000);
                                 loadTblVeiculos(contratoId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message || 'Erro ao remover veículo',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao remover veículo", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'removerVeiculo.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculo.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'removerVeiculo.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculo.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'removerVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculo", error);
     }
 }
 
 function removerVeiculoAta(veiculoId, ataId) {
     try {
         Alerta.Confirmar(
-            'Deseja realmente remover este veículo da ata?',
-            'Esta ação não poderá ser desfeita!',
-            'Sim, remover',
-            'Cancelar',
+            "Deseja realmente remover este veículo da ata?",
+            "Esta ação não poderá ser desfeita!",
+            "Sim, remover",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverVeiculoAta',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        veiculoId: veiculoId,
-                        ataId: ataId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverVeiculoAta",
+                    contentType: "application/json",
+                    data: JSON.stringify({ veiculoId: veiculoId, ataId: ataId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show('Verde', res.message, 3000);
+                                AppToast.show("Verde", res.message, 3000);
                                 loadTblVeiculosAta(ataId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message || 'Erro ao remover veículo',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao remover veículo", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'removerVeiculoAta.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculoAta.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'removerVeiculoAta.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculoAta.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'removerVeiculoAta',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "removerVeiculoAta", error);
     }
 }
 
 function desvincularEncarregado(encarregadoId, contratoId) {
     try {
         Alerta.Confirmar(
-            'Deseja desvincular este encarregado do contrato?',
-            'O encarregado ficará disponível para outros contratos.',
-            'Sim, desvincular',
-            'Cancelar',
+            "Deseja desvincular este encarregado do contrato?",
+            "O encarregado ficará disponível para outros contratos.",
+            "Sim, desvincular",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverEncarregadoContrato',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        encarregadoId: encarregadoId,
-                        contratoId: contratoId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverEncarregadoContrato",
+                    contentType: "application/json",
+                    data: JSON.stringify({ encarregadoId: encarregadoId, contratoId: contratoId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    'Encarregado desvinculado com sucesso!',
-                                    3000,
-                                );
+                                AppToast.show("Verde", "Encarregado desvinculado com sucesso!", 3000);
                                 loadTblEncarregados(contratoId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message ||
-                                        'Erro ao desvincular encarregado',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao desvincular encarregado", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'desvincularEncarregado.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularEncarregado.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'desvincularEncarregado.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularEncarregado.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'desvincularEncarregado',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularEncarregado", error);
     }
 }
 
 function desvincularOperador(operadorId, contratoId) {
     try {
         Alerta.Confirmar(
-            'Deseja desvincular este operador do contrato?',
-            'O operador ficará disponível para outros contratos.',
-            'Sim, desvincular',
-            'Cancelar',
+            "Deseja desvincular este operador do contrato?",
+            "O operador ficará disponível para outros contratos.",
+            "Sim, desvincular",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverOperadorContrato',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        operadorId: operadorId,
-                        contratoId: contratoId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverOperadorContrato",
+                    contentType: "application/json",
+                    data: JSON.stringify({ operadorId: operadorId, contratoId: contratoId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    'Operador desvinculado com sucesso!',
-                                    3000,
-                                );
+                                AppToast.show("Verde", "Operador desvinculado com sucesso!", 3000);
                                 loadTblOperadores(contratoId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message ||
-                                        'Erro ao desvincular operador',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao desvincular operador", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'desvincularOperador.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularOperador.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'desvincularOperador.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularOperador.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'desvincularOperador',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularOperador", error);
     }
 }
 
 function desvincularMotorista(motoristaId, contratoId) {
     try {
         Alerta.Confirmar(
-            'Deseja desvincular este motorista do contrato?',
-            'O motorista ficará disponível para outros contratos.',
-            'Sim, desvincular',
-            'Cancelar',
+            "Deseja desvincular este motorista do contrato?",
+            "O motorista ficará disponível para outros contratos.",
+            "Sim, desvincular",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverMotoristaContrato',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        motoristaId: motoristaId,
-                        contratoId: contratoId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverMotoristaContrato",
+                    contentType: "application/json",
+                    data: JSON.stringify({ motoristaId: motoristaId, contratoId: contratoId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    'Motorista desvinculado com sucesso!',
-                                    3000,
-                                );
+                                AppToast.show("Verde", "Motorista desvinculado com sucesso!", 3000);
                                 loadTblMotoristas(contratoId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message ||
-                                        'Erro ao desvincular motorista',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao desvincular motorista", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'desvincularMotorista.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularMotorista.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'desvincularMotorista.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularMotorista.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'desvincularMotorista',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularMotorista", error);
     }
 }
 
 function desvincularLavador(lavadorId, contratoId) {
     try {
         Alerta.Confirmar(
-            'Deseja desvincular este lavador do contrato?',
-            'O lavador ficará disponível para outros contratos.',
-            'Sim, desvincular',
-            'Cancelar',
+            "Deseja desvincular este lavador do contrato?",
+            "O lavador ficará disponível para outros contratos.",
+            "Sim, desvincular",
+            "Cancelar"
         ).then((confirmado) => {
             if (confirmado) {
                 $.ajax({
-                    type: 'POST',
-                    url: '/api/ItensContrato/RemoverLavadorContrato',
-                    contentType: 'application/json',
-                    data: JSON.stringify({
-                        lavadorId: lavadorId,
-                        contratoId: contratoId,
-                    }),
+                    type: "POST",
+                    url: "/api/ItensContrato/RemoverLavadorContrato",
+                    contentType: "application/json",
+                    data: JSON.stringify({ lavadorId: lavadorId, contratoId: contratoId }),
                     success: function (res) {
                         try {
                             if (res && res.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    'Lavador desvinculado com sucesso!',
-                                    3000,
-                                );
+                                AppToast.show("Verde", "Lavador desvinculado com sucesso!", 3000);
                                 loadTblLavadores(contratoId);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    res.message ||
-                                        'Erro ao desvincular lavador',
-                                    3000,
-                                );
+                                AppToast.show("Vermelho", res.message || "Erro ao desvincular lavador", 3000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'itenscontrato.js',
-                                'desvincularLavador.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularLavador.success", error);
                         }
                     },
                     error: function (err) {
-                        Alerta.TratamentoErroComLinha(
-                            'itenscontrato.js',
-                            'desvincularLavador.error',
-                            err,
-                        );
-                    },
+                        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularLavador.error", err);
+                    }
                 });
             }
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'desvincularLavador',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "desvincularLavador", error);
     }
 }
 
 function formatarMoeda(valor) {
     try {
         if (valor === null || valor === undefined) return 'R$ 0,00';
-        return (
-            'R$ ' +
-            parseFloat(valor).toLocaleString('pt-BR', {
-                minimumFractionDigits: 2,
-                maximumFractionDigits: 2,
-            })
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'itenscontrato.js',
-            'formatarMoeda',
-            error,
-        );
+        return 'R$ ' + parseFloat(valor).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("itenscontrato.js", "formatarMoeda", error);
         return 'R$ 0,00';
     }
 }
```
