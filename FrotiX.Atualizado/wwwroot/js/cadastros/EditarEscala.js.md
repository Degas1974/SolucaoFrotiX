# wwwroot/js/cadastros/EditarEscala.js

**Mudanca:** GRANDE | **+123** linhas | **-317** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/EditarEscala.js
+++ ATUAL: wwwroot/js/cadastros/EditarEscala.js
@@ -3,128 +3,73 @@
         inicializarEventosEditarEscala();
         inicializarSubmitEscala();
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'EditarEscala.js',
-            'document.ready',
-            error,
-        );
+        Alerta.TratamentoErroComLinha('EditarEscala.js', 'document.ready', error);
     }
 });
 
 function inicializarSubmitEscala() {
     try {
-        console.log('inicializarSubmitEscala: Iniciando...');
-
-        var form = $('#formEditarEscala');
-        console.log('Formulário encontrado:', form.length > 0);
+        console.log("inicializarSubmitEscala: Iniciando...");
+
+        var form = $("#formEditarEscala");
+        console.log("Formulário encontrado:", form.length > 0);
 
         if (form.length === 0) {
-            console.error('ERRO: Formulário #formEditarEscala não encontrado!');
+            console.error("ERRO: Formulário #formEditarEscala não encontrado!");
             return;
         }
 
-        form.on('submit', function (e) {
+        form.on("submit", function (e) {
             try {
                 e.preventDefault();
-                console.log('=== SUBMIT INTERCEPTADO ===');
-
-                var escalaDiaId = $('#hiddenEscalaDiaId').val();
-                console.log('EscalaDiaId:', escalaDiaId);
-
-                if (
-                    !escalaDiaId ||
-                    escalaDiaId === '00000000-0000-0000-0000-000000000000'
-                ) {
-                    AppToast.show(
-                        'Vermelho',
-                        'ID da escala inválido.',
-                        3000000,
-                    );
+                console.log("=== SUBMIT INTERCEPTADO ===");
+
+                var escalaDiaId = $("#hiddenEscalaDiaId").val();
+                console.log("EscalaDiaId:", escalaDiaId);
+
+                if (!escalaDiaId || escalaDiaId === '00000000-0000-0000-0000-000000000000') {
+                    AppToast.show("Vermelho", "ID da escala inválido.", 3000000);
                     return false;
                 }
 
-                AppToast.show('Amarelo', 'Salvando escala...', 2000);
-
-                var dataEscalaPicker =
-                    document.getElementById('dataEscala')?.ej2_instances?.[0];
-                var horaInicioPicker =
-                    document.getElementById('horaInicio')?.ej2_instances?.[0];
-                var horaFimPicker =
-                    document.getElementById('horaFim')?.ej2_instances?.[0];
-                var turnoDropdown =
-                    document.getElementById('turnoId')?.ej2_instances?.[0];
-                var veiculoDropdown =
-                    document.getElementById('veiculoId')?.ej2_instances?.[0];
-                var tipoServicoDropdown =
-                    document.getElementById('tipoServicoId')
-                        ?.ej2_instances?.[0];
-                var lotacaoDropdown =
-                    document.getElementById('lotacao')?.ej2_instances?.[0];
-                var requisitanteDropdown =
-                    document.getElementById('requisitanteId')
-                        ?.ej2_instances?.[0];
-                var observacoesTextbox =
-                    document.getElementById('observacoes')?.ej2_instances?.[0];
-
-                var categoriaDropdown = document.getElementById(
-                    'categoriaIndisponibilidade',
-                )?.ej2_instances?.[0];
-                var dataInicioIndispPicker = document.getElementById(
-                    'dataInicioIndisponibilidade',
-                )?.ej2_instances?.[0];
-                var dataFimIndispPicker = document.getElementById(
-                    'dataFimIndisponibilidade',
-                )?.ej2_instances?.[0];
-                var motoristaCobertorDropdown = document.getElementById(
-                    'motoristaCobertorId',
-                )?.ej2_instances?.[0];
-
-                console.log('Componentes Syncfusion:');
-                console.log(
-                    '- dataEscalaPicker:',
-                    dataEscalaPicker ? 'OK' : 'NULL',
-                );
-                console.log(
-                    '- horaInicioPicker:',
-                    horaInicioPicker ? 'OK' : 'NULL',
-                );
-                console.log('- horaFimPicker:', horaFimPicker ? 'OK' : 'NULL');
-                console.log('- turnoDropdown:', turnoDropdown ? 'OK' : 'NULL');
-                console.log(
-                    '- tipoServicoDropdown:',
-                    tipoServicoDropdown ? 'OK' : 'NULL',
-                );
-                console.log(
-                    '- categoriaDropdown:',
-                    categoriaDropdown ? 'OK' : 'NULL',
-                );
-                console.log(
-                    '- dataInicioIndispPicker:',
-                    dataInicioIndispPicker ? 'OK' : 'NULL',
-                );
-                console.log(
-                    '- dataFimIndispPicker:',
-                    dataFimIndispPicker ? 'OK' : 'NULL',
-                );
-                console.log(
-                    '- motoristaCobertorDropdown:',
-                    motoristaCobertorDropdown ? 'OK' : 'NULL',
-                );
-
-                var isIndisponivel = $('#MotoristaIndisponivel').is(':checked');
-
-                var veiculoNaoDefinido = $('#veiculoNaoDefinido').is(
-                    ':checked',
-                );
+                AppToast.show("Amarelo", "Salvando escala...", 2000);
+
+                var dataEscalaPicker = document.getElementById('dataEscala')?.ej2_instances?.[0];
+                var horaInicioPicker = document.getElementById('horaInicio')?.ej2_instances?.[0];
+                var horaFimPicker = document.getElementById('horaFim')?.ej2_instances?.[0];
+                var turnoDropdown = document.getElementById('turnoId')?.ej2_instances?.[0];
+                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
+                var tipoServicoDropdown = document.getElementById('tipoServicoId')?.ej2_instances?.[0];
+                var lotacaoDropdown = document.getElementById('lotacao')?.ej2_instances?.[0];
+                var requisitanteDropdown = document.getElementById('requisitanteId')?.ej2_instances?.[0];
+                var observacoesTextbox = document.getElementById('observacoes')?.ej2_instances?.[0];
+
+                var categoriaDropdown = document.getElementById('categoriaIndisponibilidade')?.ej2_instances?.[0];
+                var dataInicioIndispPicker = document.getElementById('dataInicioIndisponibilidade')?.ej2_instances?.[0];
+                var dataFimIndispPicker = document.getElementById('dataFimIndisponibilidade')?.ej2_instances?.[0];
+                var motoristaCobertorDropdown = document.getElementById('motoristaCobertorId')?.ej2_instances?.[0];
+
+                console.log("Componentes Syncfusion:");
+                console.log("- dataEscalaPicker:", dataEscalaPicker ? "OK" : "NULL");
+                console.log("- horaInicioPicker:", horaInicioPicker ? "OK" : "NULL");
+                console.log("- horaFimPicker:", horaFimPicker ? "OK" : "NULL");
+                console.log("- turnoDropdown:", turnoDropdown ? "OK" : "NULL");
+                console.log("- tipoServicoDropdown:", tipoServicoDropdown ? "OK" : "NULL");
+                console.log("- categoriaDropdown:", categoriaDropdown ? "OK" : "NULL");
+                console.log("- dataInicioIndispPicker:", dataInicioIndispPicker ? "OK" : "NULL");
+                console.log("- dataFimIndispPicker:", dataFimIndispPicker ? "OK" : "NULL");
+                console.log("- motoristaCobertorDropdown:", motoristaCobertorDropdown ? "OK" : "NULL");
+
+                var isIndisponivel = $("#MotoristaIndisponivel").is(":checked");
+
+                var veiculoNaoDefinido = $("#veiculoNaoDefinido").is(":checked");
 
                 var dados = {
                     EscalaDiaId: escalaDiaId,
-                    MotoristaId: $('#hiddenMotoristaId').val(),
-                    VeiculoId: veiculoNaoDefinido
-                        ? ''
-                        : veiculoDropdown?.value || '',
+                    MotoristaId: $("#hiddenMotoristaId").val(),
+                    VeiculoId: veiculoNaoDefinido ? '' : (veiculoDropdown?.value || ''),
                     VeiculoNaoDefinido: veiculoNaoDefinido,
-                    NumeroSaidas: $('#hiddenNumeroSaidas').val() || 0,
+                    NumeroSaidas: $("#hiddenNumeroSaidas").val() || 0,
                     DataEscala: formatarData(dataEscalaPicker),
                     HoraInicio: formatarHora(horaInicioPicker),
                     HoraFim: formatarHora(horaFimPicker),
@@ -134,111 +79,74 @@
                     RequisitanteId: requisitanteDropdown?.value || null,
                     Observacoes: observacoesTextbox?.value || '',
                     MotoristaIndisponivel: isIndisponivel,
-                    MotoristaEconomildo: $('#MotoristaEconomildo').is(
-                        ':checked',
-                    ),
-                    MotoristaEmServico: $('#MotoristaEmServico').is(':checked'),
-                    MotoristaReservado: $('#MotoristaReservado').is(':checked'),
-
-                    CategoriaIndisponibilidade: isIndisponivel
-                        ? categoriaDropdown?.value || ''
-                        : '',
-                    DataInicioIndisponibilidade: isIndisponivel
-                        ? formatarData(dataInicioIndispPicker)
-                        : '',
-                    DataFimIndisponibilidade: isIndisponivel
-                        ? formatarData(dataFimIndispPicker)
-                        : '',
-                    MotoristaCobertorId: isIndisponivel
-                        ? motoristaCobertorDropdown?.value || ''
-                        : '',
-
-                    Segunda: $('#segunda').is(':checked'),
-                    Terca: $('#terca').is(':checked'),
-                    Quarta: $('#quarta').is(':checked'),
-                    Quinta: $('#quinta').is(':checked'),
-                    Sexta: $('#sexta').is(':checked'),
-                    Sabado: $('#sabado').is(':checked'),
-                    Domingo: $('#domingo').is(':checked'),
+                    MotoristaEconomildo: $("#MotoristaEconomildo").is(":checked"),
+                    MotoristaEmServico: $("#MotoristaEmServico").is(":checked"),
+                    MotoristaReservado: $("#MotoristaReservado").is(":checked"),
+
+                    CategoriaIndisponibilidade: isIndisponivel ? (categoriaDropdown?.value || '') : '',
+                    DataInicioIndisponibilidade: isIndisponivel ? formatarData(dataInicioIndispPicker) : '',
+                    DataFimIndisponibilidade: isIndisponivel ? formatarData(dataFimIndispPicker) : '',
+                    MotoristaCobertorId: isIndisponivel ? (motoristaCobertorDropdown?.value || '') : '',
+
+                    Segunda: $("#segunda").is(":checked"),
+                    Terca: $("#terca").is(":checked"),
+                    Quarta: $("#quarta").is(":checked"),
+                    Quinta: $("#quinta").is(":checked"),
+                    Sexta: $("#sexta").is(":checked"),
+                    Sabado: $("#sabado").is(":checked"),
+                    Domingo: $("#domingo").is(":checked")
                 };
 
-                console.log('=== DADOS A ENVIAR ===');
+                console.log("=== DADOS A ENVIAR ===");
                 console.log(JSON.stringify(dados, null, 2));
 
                 if (isIndisponivel) {
                     if (!dados.DataInicioIndisponibilidade) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Data de início da indisponibilidade é obrigatória.',
-                            3000000,
-                        );
+                        AppToast.show("Vermelho", "Data de início da indisponibilidade é obrigatória.", 3000000);
                         return false;
                     }
                     if (!dados.DataFimIndisponibilidade) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Data de fim da indisponibilidade é obrigatória.',
-                            3000000,
-                        );
+                        AppToast.show("Vermelho", "Data de fim da indisponibilidade é obrigatória.", 3000000);
                         return false;
                     }
                     if (!dados.CategoriaIndisponibilidade) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Categoria da indisponibilidade é obrigatória.',
-                            3000000,
-                        );
+                        AppToast.show("Vermelho", "Categoria da indisponibilidade é obrigatória.", 3000000);
                         return false;
                     }
                 }
 
                 var token = $('input[name="__RequestVerificationToken"]').val();
-                console.log('Token:', token ? 'Presente' : 'AUSENTE');
-
-                console.log('Enviando para: /api/Escala/EditEscala');
+                console.log("Token:", token ? "Presente" : "AUSENTE");
+
+                console.log("Enviando para: /api/Escala/EditEscala");
 
                 $.ajax({
                     url: '/api/Escala/EditEscala',
                     type: 'POST',
                     data: dados,
                     headers: {
-                        RequestVerificationToken: token,
+                        'RequestVerificationToken': token
                     },
                     success: function (response) {
                         try {
-                            console.log('=== RESPOSTA DO SERVIDOR ===');
+                            console.log("=== RESPOSTA DO SERVIDOR ===");
                             console.log(response);
 
                             if (response.debugLog) {
-                                console.log('=== DEBUG LOG DO SERVIDOR ===');
+                                console.log("=== DEBUG LOG DO SERVIDOR ===");
                                 console.log(response.debugLog);
                             }
 
                             if (response.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    response.message ||
-                                        'Escala atualizada com sucesso!',
-                                    3000,
-                                );
+                                AppToast.show("Verde", response.message || "Escala atualizada com sucesso!", 3000);
                                 setTimeout(function () {
-                                    window.location.href =
-                                        '/Escalas/ListaEscala';
+                                    window.location.href = '/Escalas/ListaEscala';
                                 }, 2000);
                             } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    response.message ||
-                                        'Erro ao atualizar escala.',
-                                    3000000,
-                                );
+                                AppToast.show("Vermelho", response.message || "Erro ao atualizar escala.", 3000000);
                             }
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'EditarEscala.js',
-                                'ajax.success',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'ajax.success', error);
                         }
                     },
                     error: function (xhr, status, error) {
@@ -249,8 +157,7 @@
                             console.error('Response:', xhr.responseText);
                             console.error('Status Code:', xhr.status);
 
-                            var mensagemErro =
-                                'Erro ao salvar escala. Tente novamente.';
+                            var mensagemErro = "Erro ao salvar escala. Tente novamente.";
                             try {
                                 var resp = JSON.parse(xhr.responseText);
                                 if (resp.message) mensagemErro = resp.message;
@@ -258,39 +165,25 @@
 
                             }
 
-                            AppToast.show('Vermelho', mensagemErro, 3000000);
+                            AppToast.show("Vermelho", mensagemErro, 3000000);
                         } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'EditarEscala.js',
-                                'ajax.error',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'ajax.error', error);
                         }
-                    },
+                    }
                 });
 
                 return false;
             } catch (error) {
-                console.error('ERRO no submit:', error);
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'formEditarEscala.submit',
-                    error,
-                );
+                console.error("ERRO no submit:", error);
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'formEditarEscala.submit', error);
                 return false;
             }
         });
 
-        console.log(
-            'inicializarSubmitEscala: Evento de submit registrado com sucesso!',
-        );
-    } catch (error) {
-        console.error('ERRO em inicializarSubmitEscala:', error);
-        Alerta.TratamentoErroComLinha(
-            'EditarEscala.js',
-            'inicializarSubmitEscala',
-            error,
-        );
+        console.log("inicializarSubmitEscala: Evento de submit registrado com sucesso!");
+    } catch (error) {
+        console.error("ERRO em inicializarSubmitEscala:", error);
+        Alerta.TratamentoErroComLinha('EditarEscala.js', 'inicializarSubmitEscala', error);
     }
 }
 
@@ -298,13 +191,9 @@
     try {
         if (picker && picker.value) {
             var data = new Date(picker.value);
-            return (
-                data.getFullYear() +
-                '-' +
-                String(data.getMonth() + 1).padStart(2, '0') +
-                '-' +
-                String(data.getDate()).padStart(2, '0')
-            );
+            return data.getFullYear() + '-' +
+                String(data.getMonth() + 1).padStart(2, '0') + '-' +
+                String(data.getDate()).padStart(2, '0');
         }
         return '';
     } catch (error) {
@@ -317,11 +206,8 @@
     try {
         if (picker && picker.value) {
             var hora = new Date(picker.value);
-            return (
-                String(hora.getHours()).padStart(2, '0') +
-                ':' +
-                String(hora.getMinutes()).padStart(2, '0')
-            );
+            return String(hora.getHours()).padStart(2, '0') + ':' +
+                String(hora.getMinutes()).padStart(2, '0');
         }
         return '';
     } catch (error) {
@@ -333,11 +219,10 @@
 function inicializarEventosEditarEscala() {
     try {
 
-        setTimeout(function () {
+        setTimeout(function() {
             try {
                 var checkbox = document.getElementById('veiculoNaoDefinido');
-                var veiculoDropdown =
-                    document.getElementById('veiculoId')?.ej2_instances?.[0];
+                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
 
                 if (checkbox && veiculoDropdown) {
 
@@ -347,18 +232,13 @@
                     }
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'veiculoNaoDefinido.init',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'veiculoNaoDefinido.init', error);
             }
         }, 600);
 
-        $('#veiculoNaoDefinido').change(function () {
-            try {
-                var veiculoDropdown =
-                    document.getElementById('veiculoId')?.ej2_instances?.[0];
+        $('#veiculoNaoDefinido').change(function() {
+            try {
+                var veiculoDropdown = document.getElementById('veiculoId')?.ej2_instances?.[0];
                 if (veiculoDropdown) {
                     if (this.checked) {
 
@@ -371,52 +251,34 @@
                     }
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
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
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'veiculoNaoDefinido.change', error);
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
-                                'EditarEscala.js',
-                                'tipoServicoDropdown.change',
-                                error,
-                            );
+                            Alerta.TratamentoErroComLinha('EditarEscala.js', 'tipoServicoDropdown.change', error);
                         }
                     };
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'tipoServico.setTimeout',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'tipoServico.setTimeout', error);
             }
         }, 500);
 
@@ -439,11 +301,7 @@
                     $('#statusIndisponivel').removeClass('active-indisponivel');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'MotoristaIndisponivel.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaIndisponivel.change', error);
             }
         });
 
@@ -466,19 +324,13 @@
                     $('#statusServico').removeClass('active-servico');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'MotoristaEmServico.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaEmServico.change', error);
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
@@ -497,10 +349,7 @@
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
@@ -512,11 +361,7 @@
                     $('#statusEconomildo').removeClass('active-economildo');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'MotoristaEconomildo.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaEconomildo.change', error);
             }
         });
 
@@ -538,41 +383,26 @@
                     $('#statusReservado').removeClass('active-reservado');
                 }
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'EditarEscala.js',
-                    'MotoristaReservado.change',
-                    error,
-                );
-            }
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'EditarEscala.js',
-            'inicializarEventosEditarEscala',
-            error,
-        );
+                Alerta.TratamentoErroComLinha('EditarEscala.js', 'MotoristaReservado.change', error);
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('EditarEscala.js', 'inicializarEventosEditarEscala', error);
     }
 }
 
 function excluirEscala(escalaDiaId) {
     try {
-        if (
-            !escalaDiaId ||
-            escalaDiaId === '00000000-0000-0000-0000-000000000000'
-        ) {
-            AppToast.show('Vermelho', 'ID da escala inválido.', 3000);
+        if (!escalaDiaId || escalaDiaId === '00000000-0000-0000-0000-000000000000') {
+            AppToast.show("Vermelho", "ID da escala inválido.", 3000);
             return;
         }
 
-        if (
-            !confirm(
-                'Tem certeza que deseja excluir esta escala? Esta ação não pode ser desfeita.',
-            )
-        ) {
+        if (!confirm('Tem certeza que deseja excluir esta escala? Esta ação não pode ser desfeita.')) {
             return;
         }
 
-        AppToast.show('Amarelo', 'Excluindo escala...', 2000);
+        AppToast.show("Amarelo", "Excluindo escala...", 2000);
 
         $.ajax({
             url: '/api/Escala/DeleteEscala/' + escalaDiaId,
@@ -580,51 +410,27 @@
             success: function (response) {
                 try {
                     if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            response.message || 'Escala excluída com sucesso!',
-                            3000,
-                        );
+                        AppToast.show("Verde", response.message || "Escala excluída com sucesso!", 3000);
                         setTimeout(function () {
                             window.location.href = '/Escalas/ListaEscala';
                         }, 1500);
                     } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao excluir escala.',
-                            3000,
-                        );
+                        AppToast.show("Vermelho", response.message || "Erro ao excluir escala.", 3000);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'EditarEscala.js',
-                        'excluirEscala.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala.success', error);
                 }
             },
             error: function (xhr, status, error) {
                 try {
                     console.error('Erro ao excluir escala:', error);
-                    AppToast.show(
-                        'Vermelho',
-                        'Erro ao excluir escala. Tente novamente.',
-                        3000,
-                    );
+                    AppToast.show("Vermelho", "Erro ao excluir escala. Tente novamente.", 3000);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'EditarEscala.js',
-                        'excluirEscala.error',
-                        error,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'EditarEscala.js',
-            'excluirEscala',
-            error,
-        );
+                    Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala.error', error);
+                }
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('EditarEscala.js', 'excluirEscala', error);
     }
 }
```
