# wwwroot/js/alertasfrotix/alertas_recorrencia.js

**Mudanca:** GRANDE | **+40** linhas | **-154** linhas

---

```diff
--- JANEIRO: wwwroot/js/alertasfrotix/alertas_recorrencia.js
+++ ATUAL: wwwroot/js/alertasfrotix/alertas_recorrencia.js
@@ -14,10 +14,7 @@
 
         console.log('✅ Controles de recorrência inicializados');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            error,
-            'inicializarControlesRecorrenciaAlerta',
-        );
+        Alerta.TratamentoErroComLinha(error, 'inicializarControlesRecorrenciaAlerta');
     }
 }
 
@@ -26,61 +23,42 @@
 
         var tipoExibicaoElement = document.getElementById('TipoExibicao');
 
-        if (
-            tipoExibicaoElement &&
-            tipoExibicaoElement.ej2_instances &&
-            tipoExibicaoElement.ej2_instances[0]
-        ) {
+        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
             var dropdown = tipoExibicaoElement.ej2_instances[0];
 
             var originalChangeHandler = dropdown.change;
 
-            dropdown.change = function (args) {
+            dropdown.change = function(args) {
                 try {
 
-                    if (
-                        originalChangeHandler &&
-                        typeof originalChangeHandler === 'function'
-                    ) {
+                    if (originalChangeHandler && typeof originalChangeHandler === 'function') {
                         originalChangeHandler.call(this, args);
                     }
 
                     var tipoExibicao = parseInt(args.value);
                     mostrarCamposPorTipoExibicao(tipoExibicao);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        error,
-                        'TipoExibicao.change.recorrencia',
-                    );
+                    Alerta.TratamentoErroComLinha(error, 'TipoExibicao.change.recorrencia');
                 }
             };
 
-            console.log(
-                '✅ Event handler de recorrência configurado para TipoExibicao',
-            );
+            console.log('✅ Event handler de recorrência configurado para TipoExibicao');
         } else {
-            console.warn(
-                '⚠️ Dropdown TipoExibicao não encontrado ou não inicializado',
-            );
-
-            setTimeout(function () {
+            console.warn('⚠️ Dropdown TipoExibicao não encontrado ou não inicializado');
+
+            setTimeout(function() {
                 try {
                     var el = document.getElementById('TipoExibicao');
                     if (el && el.ej2_instances && el.ej2_instances[0]) {
-                        el.ej2_instances[0].change = function (args) {
+                        el.ej2_instances[0].change = function(args) {
                             try {
                                 var tipoExibicao = parseInt(args.value);
                                 mostrarCamposPorTipoExibicao(tipoExibicao);
                             } catch (error) {
-                                Alerta.TratamentoErroComLinha(
-                                    error,
-                                    'TipoExibicao.change.fallback',
-                                );
+                                Alerta.TratamentoErroComLinha(error, 'TipoExibicao.change.fallback');
                             }
                         };
-                        console.log(
-                            '✅ Event handler de recorrência configurado (fallback)',
-                        );
+                        console.log('✅ Event handler de recorrência configurado (fallback)');
                     }
                 } catch (err) {
                     console.error('Erro no fallback:', err);
@@ -88,10 +66,7 @@
             }, 500);
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            error,
-            'configurarEventosRecorrenciaAlerta',
-        );
+        Alerta.TratamentoErroComLinha(error, 'configurarEventosRecorrenciaAlerta');
     }
 }
 
@@ -99,11 +74,7 @@
     try {
         var tipoExibicaoElement = document.getElementById('TipoExibicao');
 
-        if (
-            tipoExibicaoElement &&
-            tipoExibicaoElement.ej2_instances &&
-            tipoExibicaoElement.ej2_instances[0]
-        ) {
+        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
             var valor = tipoExibicaoElement.ej2_instances[0].value;
             if (valor) {
                 var tipoExibicao = parseInt(valor);
@@ -112,10 +83,7 @@
             }
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            error,
-            'verificarEstadoRecorrenciaAlerta',
-        );
+        Alerta.TratamentoErroComLinha(error, 'verificarEstadoRecorrenciaAlerta');
     }
 }
 
@@ -148,11 +116,7 @@
 
             case 5:
             case 6:
-                console.log(
-                    'Tipo ' +
-                        tipoExibicao +
-                        ' - Semanal/Quinzenal: mostrando dias da semana',
-                );
+                console.log('Tipo ' + tipoExibicao + ' - Semanal/Quinzenal: mostrando dias da semana');
                 mostrarElemento('divDiasAlerta');
                 break;
 
@@ -185,74 +149,30 @@
         if (container.ej2_instances && container.ej2_instances[0]) {
             calendarioAlertaInstance = container.ej2_instances[0];
             window.calendarioAlertaInstance = calendarioAlertaInstance;
-            console.log(
-                'Calendário já inicializado, usando instância existente',
-            );
+            console.log('Calendário já inicializado, usando instância existente');
             return;
-        }
-
-        if (
-            typeof ej === 'undefined' ||
-            typeof ej.calendars === 'undefined' ||
-            typeof ej.calendars.Calendar === 'undefined'
-        ) {
-            console.warn(
-                '⚠️ Syncfusion Calendar não está disponível ainda. Tentando novamente em 500ms...',
-            );
-            setTimeout(function () {
-                initCalendarioAlerta();
-            }, 500);
-            return;
-        }
-
-        var localeToUse = 'en';
-        try {
-            if (
-                typeof ej.base !== 'undefined' &&
-                typeof ej.base.L10n !== 'undefined'
-            ) {
-
-                var cultures = ej.base.L10n.Locale || {};
-                if (cultures['pt-BR'] || cultures['pt']) {
-                    localeToUse = cultures['pt-BR'] ? 'pt-BR' : 'pt';
-                }
-            }
-        } catch (e) {
-            console.warn('Não foi possível verificar locale, usando en:', e);
         }
 
         calendarioAlertaInstance = new ej.calendars.Calendar({
             isMultiSelection: true,
-            locale: localeToUse,
             change: function (args) {
                 try {
                     datasAlertaSelecionadas = args.values || [];
                     window.datasAlertaSelecionadas = datasAlertaSelecionadas;
                     atualizarBadgeContador();
                     atualizarCampoHidden();
-                    console.log(
-                        'Datas selecionadas:',
-                        datasAlertaSelecionadas.length,
-                    );
+                    console.log('Datas selecionadas:', datasAlertaSelecionadas.length);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'alertas_recorrencia.js',
-                        'calendarioAlerta.change',
-                        error,
-                    );
-                }
-            },
+                    Alerta.TratamentoErroComLinha(error, 'calendarioAlerta.change');
+                }
+            }
         });
         calendarioAlertaInstance.appendTo('#calDatasSelecionadasAlerta');
         window.calendarioAlertaInstance = calendarioAlertaInstance;
 
         console.log('✅ Calendário inicializado');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'alertas_recorrencia.js',
-            'initCalendarioAlerta',
-            error,
-        );
+        Alerta.TratamentoErroComLinha(error, 'initCalendarioAlerta');
     }
 }
 
@@ -289,17 +209,12 @@
         var tipoExibicaoElement = document.getElementById('TipoExibicao');
         var tipoExibicao = 1;
 
-        if (
-            tipoExibicaoElement &&
-            tipoExibicaoElement.ej2_instances &&
-            tipoExibicaoElement.ej2_instances[0]
-        ) {
-            tipoExibicao =
-                parseInt(tipoExibicaoElement.ej2_instances[0].value) || 1;
+        if (tipoExibicaoElement && tipoExibicaoElement.ej2_instances && tipoExibicaoElement.ej2_instances[0]) {
+            tipoExibicao = parseInt(tipoExibicaoElement.ej2_instances[0].value) || 1;
         }
 
         var dados = {
-            TipoExibicao: tipoExibicao,
+            TipoExibicao: tipoExibicao
         };
 
         if (tipoExibicao < 4) {
@@ -313,25 +228,15 @@
             case 5:
             case 6:
                 var lstDias = document.getElementById('lstDiasAlerta');
-                if (
-                    lstDias &&
-                    lstDias.ej2_instances &&
-                    lstDias.ej2_instances[0]
-                ) {
+                if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
                     dados.DiasSemana = lstDias.ej2_instances[0].value || [];
                 }
                 break;
 
             case 7:
                 var lstDiasMes = document.getElementById('lstDiasMesAlerta');
-                if (
-                    lstDiasMes &&
-                    lstDiasMes.ej2_instances &&
-                    lstDiasMes.ej2_instances[0]
-                ) {
-                    dados.DiaMesRecorrencia = parseInt(
-                        lstDiasMes.ej2_instances[0].value,
-                    );
+                if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0]) {
+                    dados.DiaMesRecorrencia = parseInt(lstDiasMes.ej2_instances[0].value);
                 }
                 break;
 
@@ -364,11 +269,7 @@
             case 6:
                 if (alerta.DiasSemana && alerta.DiasSemana.length > 0) {
                     var lstDias = document.getElementById('lstDiasAlerta');
-                    if (
-                        lstDias &&
-                        lstDias.ej2_instances &&
-                        lstDias.ej2_instances[0]
-                    ) {
+                    if (lstDias && lstDias.ej2_instances && lstDias.ej2_instances[0]) {
                         lstDias.ej2_instances[0].value = alerta.DiasSemana;
                         lstDias.ej2_instances[0].dataBind();
                     }
@@ -377,15 +278,9 @@
 
             case 7:
                 if (alerta.DiaMesRecorrencia) {
-                    var lstDiasMes =
-                        document.getElementById('lstDiasMesAlerta');
-                    if (
-                        lstDiasMes &&
-                        lstDiasMes.ej2_instances &&
-                        lstDiasMes.ej2_instances[0]
-                    ) {
-                        lstDiasMes.ej2_instances[0].value =
-                            alerta.DiaMesRecorrencia;
+                    var lstDiasMes = document.getElementById('lstDiasMesAlerta');
+                    if (lstDiasMes && lstDiasMes.ej2_instances && lstDiasMes.ej2_instances[0]) {
+                        lstDiasMes.ej2_instances[0].value = alerta.DiaMesRecorrencia;
                         lstDiasMes.ej2_instances[0].dataBind();
                     }
                 }
@@ -393,10 +288,9 @@
 
             case 8:
                 if (alerta.DatasSelecionadas) {
-                    var datasStr =
-                        typeof alerta.DatasSelecionadas === 'string'
-                            ? alerta.DatasSelecionadas.split(',')
-                            : alerta.DatasSelecionadas;
+                    var datasStr = typeof alerta.DatasSelecionadas === 'string'
+                        ? alerta.DatasSelecionadas.split(',')
+                        : alerta.DatasSelecionadas;
 
                     datasAlertaSelecionadas = datasStr.map(function (d) {
                         return new Date(d.trim ? d.trim() : d);
@@ -404,8 +298,7 @@
                     window.datasAlertaSelecionadas = datasAlertaSelecionadas;
 
                     if (calendarioAlertaInstance) {
-                        calendarioAlertaInstance.values =
-                            datasAlertaSelecionadas;
+                        calendarioAlertaInstance.values = datasAlertaSelecionadas;
                         calendarioAlertaInstance.dataBind();
                     }
 
@@ -414,10 +307,7 @@
                 break;
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            error,
-            'preencherCamposRecorrenciaAlerta',
-        );
+        Alerta.TratamentoErroComLinha(error, 'preencherCamposRecorrenciaAlerta');
     }
 }
 
@@ -461,26 +351,19 @@
 document.addEventListener('DOMContentLoaded', function () {
     try {
 
-        setTimeout(function () {
+        setTimeout(function() {
             try {
                 inicializarControlesRecorrenciaAlerta();
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    error,
-                    'DOMContentLoaded.setTimeout.alertas_recorrencia',
-                );
+                Alerta.TratamentoErroComLinha(error, 'DOMContentLoaded.setTimeout.alertas_recorrencia');
             }
         }, 300);
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            error,
-            'DOMContentLoaded.alertas_recorrencia',
-        );
+        Alerta.TratamentoErroComLinha(error, 'DOMContentLoaded.alertas_recorrencia');
     }
 });
 
-window.inicializarControlesRecorrenciaAlerta =
-    inicializarControlesRecorrenciaAlerta;
+window.inicializarControlesRecorrenciaAlerta = inicializarControlesRecorrenciaAlerta;
 window.verificarEstadoRecorrenciaAlerta = verificarEstadoRecorrenciaAlerta;
 window.mostrarCamposPorTipoExibicao = mostrarCamposPorTipoExibicao;
 window.initCalendarioAlerta = initCalendarioAlerta;
```
