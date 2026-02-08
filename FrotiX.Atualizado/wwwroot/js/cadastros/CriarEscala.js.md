# wwwroot/js/cadastros/CriarEscala.js

**Mudanca:** GRANDE | **+50** linhas | **-156** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/CriarEscala.js
+++ ATUAL: wwwroot/js/cadastros/CriarEscala.js
@@ -2,21 +2,16 @@
     try {
         inicializarEventosEscala();
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'CriarEscala.js',
-            'document.ready',
-            error,
-        );
+        Alerta.TratamentoErroComLinha('CriarEscala.js', 'document.ready', error);
     }
 });
 
 function inicializarEventosEscala() {
     try {
 
-        $('#veiculoNaoDefinido').change(function () {
-            try {
-                var veiculoDropdown =
-                    document.getElementById('veiculoId')?.ej2_instances?.[0];
+        $('#veiculoNaoDefinido').change(function() {
+            try {
+                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
                 if (veiculoDropdown) {
                     if (this.checked) {
 
@@ -29,52 +24,34 @@
                     }
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'veiculoNaoDefinido.change',
-                    error,
-                );
-            }
-        });
-
-        setTimeout(function () {
-            try {
-                var tipoServicoDropdown =
-                    document.getElementById('tipoServicoId')
-                        ?.ej2_instances?.[0];
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'veiculoNaoDefinido.change', error);
+            }
+        });
+
+        setTimeout(function() {
+            try {
+                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
 
                 if (tipoServicoDropdown) {
 
-                    tipoServicoDropdown.change = function (args) {
+                    tipoServicoDropdown.change = function(args) {
                         try {
                             if (!args.itemData) return;
 
                             var textoSelecionado = args.itemData.Text || '';
 
-                            if (
-                                textoSelecionado.toLowerCase() === 'economildo'
-                            ) {
+                            if (textoSelecionado.toLowerCase() === 'economildo') {
                                 if (!$('#MotoristaEconomildo').is(':checked')) {
-                                    $('#MotoristaEconomildo')
-                                        .prop('checked', true)
-                                        .trigger('change');
+                                    $('#MotoristaEconomildo').prop('checked', true).trigger('change');
                                 }
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'CriarEscala.js',
-                                'tipoServicoDropdown.change',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha('CriarEscala.js', 'tipoServicoDropdown.change', error);
                         }
                     };
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'tipoServico.setTimeout',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'tipoServico.setTimeout', error);
             }
         }, 500);
 
@@ -97,11 +74,7 @@
                     $('#statusIndisponivel').removeClass('active-indisponivel');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'MotoristaIndisponivel.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaIndisponivel.change', error);
             }
         });
 
@@ -124,19 +97,13 @@
                     $('#statusServico').removeClass('active-servico');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'MotoristaEmServico.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaEmServico.change', error);
             }
         });
 
         $('#MotoristaEconomildo').change(function () {
             try {
-                var tipoServicoDropdown =
-                    document.getElementById('tipoServicoId')
-                        ?.ej2_instances?.[0];
+                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
 
                 if (this.checked) {
                     $('#economildoSection').slideDown();
@@ -155,10 +122,7 @@
                         var items = tipoServicoDropdown.dataSource;
                         if (items && items.length > 0) {
                             for (var i = 0; i < items.length; i++) {
-                                if (
-                                    items[i].Text &&
-                                    items[i].Text.toLowerCase() === 'economildo'
-                                ) {
+                                if (items[i].Text && items[i].Text.toLowerCase() === 'economildo') {
                                     tipoServicoDropdown.value = items[i].Value;
                                     break;
                                 }
@@ -170,11 +134,7 @@
                     $('#statusEconomildo').removeClass('active-economildo');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'MotoristaEconomildo.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaEconomildo.change', error);
             }
         });
 
@@ -196,11 +156,7 @@
                     $('#statusReservado').removeClass('active-reservado');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'MotoristaReservado.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'MotoristaReservado.change', error);
             }
         });
 
@@ -208,12 +164,9 @@
             inicializarEventosTurno();
             inicializarEventosMotorista();
         }, 500);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'CriarEscala.js',
-            'inicializarEventosEscala',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosEscala', error);
     }
 }
 
@@ -221,14 +174,8 @@
     try {
         const turnoElement = document.getElementById('turnoId');
 
-        if (
-            !turnoElement ||
-            !turnoElement.ej2_instances ||
-            turnoElement.ej2_instances.length === 0
-        ) {
-            console.warn(
-                '[CriarEscala.js] Controle turnoId não encontrado ou não inicializado',
-            );
+        if (!turnoElement || !turnoElement.ej2_instances || turnoElement.ej2_instances.length === 0) {
+            console.warn('[CriarEscala.js] Controle turnoId não encontrado ou não inicializado');
             return;
         }
 
@@ -245,25 +192,13 @@
                 const horaInicioElement = document.getElementById('horaInicio');
                 const horaFimElement = document.getElementById('horaFim');
 
-                if (
-                    !horaInicioElement ||
-                    !horaInicioElement.ej2_instances ||
-                    horaInicioElement.ej2_instances.length === 0
-                ) {
-                    console.warn(
-                        '[CriarEscala.js] Controle horaInicio não encontrado',
-                    );
-                    return;
-                }
-
-                if (
-                    !horaFimElement ||
-                    !horaFimElement.ej2_instances ||
-                    horaFimElement.ej2_instances.length === 0
-                ) {
-                    console.warn(
-                        '[CriarEscala.js] Controle horaFim não encontrado',
-                    );
+                if (!horaInicioElement || !horaInicioElement.ej2_instances || horaInicioElement.ej2_instances.length === 0) {
+                    console.warn('[CriarEscala.js] Controle horaInicio não encontrado');
+                    return;
+                }
+
+                if (!horaFimElement || !horaFimElement.ej2_instances || horaFimElement.ej2_instances.length === 0) {
+                    console.warn('[CriarEscala.js] Controle horaFim não encontrado');
                     return;
                 }
 
@@ -281,24 +216,15 @@
                     horaFim.value = new Date(2024, 0, 2, 6, 0);
                 }
 
-                console.log(
-                    '[CriarEscala.js] Horários ajustados para turno:',
-                    turnoText,
-                );
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'turnoDropdown.change',
-                    error,
-                );
+                console.log('[CriarEscala.js] Horários ajustados para turno:', turnoText);
+
+            } catch (error) {
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'turnoDropdown.change', error);
             }
         };
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'CriarEscala.js',
-            'inicializarEventosTurno',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosTurno', error);
     }
 }
 
@@ -306,14 +232,8 @@
     try {
         const motoristaElement = document.getElementById('motoristaId');
 
-        if (
-            !motoristaElement ||
-            !motoristaElement.ej2_instances ||
-            motoristaElement.ej2_instances.length === 0
-        ) {
-            console.warn(
-                '[CriarEscala.js] Controle motoristaId não encontrado ou não inicializado',
-            );
+        if (!motoristaElement || !motoristaElement.ej2_instances || motoristaElement.ej2_instances.length === 0) {
+            console.warn('[CriarEscala.js] Controle motoristaId não encontrado ou não inicializado');
             return;
         }
 
@@ -329,14 +249,8 @@
 
                 const dataInicioElement = document.getElementById('dataInicio');
 
-                if (
-                    !dataInicioElement ||
-                    !dataInicioElement.ej2_instances ||
-                    dataInicioElement.ej2_instances.length === 0
-                ) {
-                    console.warn(
-                        '[CriarEscala.js] Controle dataInicio não encontrado ou não inicializado',
-                    );
+                if (!dataInicioElement || !dataInicioElement.ej2_instances || dataInicioElement.ej2_instances.length === 0) {
+                    console.warn('[CriarEscala.js] Controle dataInicio não encontrado ou não inicializado');
                     return;
                 }
 
@@ -350,46 +264,31 @@
                         type: 'GET',
                         data: {
                             motoristaId: motoristaId,
-                            data: dataInicio,
+                            data: dataInicio
                         },
                         success: function (response) {
                             try {
                                 if (response && !response.disponivel) {
-                                    AppToast.show(
-                                        'Amarelo',
+                                    AppToast.show('Amarelo',
                                         'Atenção: Este motorista já possui escala nesta data!',
-                                        5000,
-                                    );
+                                        5000);
                                 }
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    'CriarEscala.js',
-                                    'verificarDisponibilidade.success',
-                                    error,
-                                );
+                                Alerta.TratamentoErroComLinha('CriarEscala.js', 'verificarDisponibilidade.success', error);
                             }
                         },
                         error: function (xhr, status, error) {
-                            console.warn(
-                                '[CriarEscala.js] Erro ao verificar disponibilidade:',
-                                error,
-                            );
-                        },
+                            console.warn('[CriarEscala.js] Erro ao verificar disponibilidade:', error);
+                        }
                     });
                 }
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'CriarEscala.js',
-                    'motoristaDropdown.change',
-                    error,
-                );
+
+            } catch (error) {
+                Alerta.TratamentoErroComLinha('CriarEscala.js', 'motoristaDropdown.change', error);
             }
         };
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'CriarEscala.js',
-            'inicializarEventosMotorista',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('CriarEscala.js', 'inicializarEventosMotorista', error);
     }
 }
```
