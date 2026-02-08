# wwwroot/js/alertasfrotix/alertas_upsert.js

**Mudanca:** GRANDE | **+435** linhas | **-458** linhas

---

```diff
--- JANEIRO: wwwroot/js/alertasfrotix/alertas_upsert.js
+++ ATUAL: wwwroot/js/alertasfrotix/alertas_upsert.js
@@ -1,5 +1,7 @@
-$(document).ready(function () {
-    try {
+$(document).ready(function ()
+{
+    try
+    {
         console.log('===== ALERTAS UPSERT CARREGADO =====');
         console.log('jQuery versão:', $.fn.jquery);
         console.log('Cards encontrados:', $('.tipo-alerta-card').length);
@@ -11,137 +13,142 @@
         configurarAvisoUsuarios();
 
         console.log('===== INICIALIZAÇÃO COMPLETA =====');
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('ERRO NA INICIALIZAÇÃO:', error);
-        TratamentoErroComLinha('alertas_upsert.js', 'document.ready', error);
+        TratamentoErroComLinha("alertas_upsert.js", "document.ready", error);
     }
 });
 
-function inicializarControles() {
-    try {
-
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'inicializarControles',
-            error,
-        );
-    }
-}
-
-function configurarEventHandlers() {
-    try {
+function inicializarControles()
+{
+    try
+    {
+
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "inicializarControles", error);
+    }
+}
+
+function configurarEventHandlers()
+{
+    try
+    {
         console.log('>>> Configurando event handlers...');
 
-        $(document)
-            .off('click', '.tipo-alerta-card')
-            .on('click', '.tipo-alerta-card', function (e) {
-                try {
-                    console.log('===== CLICK DETECTADO =====');
-                    e.preventDefault();
-                    e.stopPropagation();
-
-                    $('.tipo-alerta-card').removeClass('selected');
-
-                    $(this).addClass('selected');
-
-                    var tipo = $(this).data('tipo');
-                    $('#TipoAlerta').val(tipo);
-
-                    console.log('Tipo selecionado:', tipo);
-                    console.log(
-                        'Possui classe selected:',
-                        $(this).hasClass('selected'),
-                    );
-                    console.log('Classes do card:', $(this).attr('class'));
-
-                    configurarCamposRelacionados(tipo);
-                } catch (error) {
-                    console.error('ERRO no click handler:', error);
-                    TratamentoErroComLinha(
-                        'alertas_upsert.js',
-                        'tipo-alerta-card.click',
-                        error,
-                    );
-                }
-            });
-
-        var tipoExibicaoDropdown = document.querySelector('#TipoExibicao');
-        if (tipoExibicaoDropdown && tipoExibicaoDropdown.ej2_instances) {
-            tipoExibicaoDropdown.ej2_instances[0].change = function (args) {
-                try {
+        $(document).off('click', '.tipo-alerta-card').on('click', '.tipo-alerta-card', function (e)
+        {
+            try
+            {
+                console.log('===== CLICK DETECTADO =====');
+                e.preventDefault();
+                e.stopPropagation();
+
+                $('.tipo-alerta-card').removeClass('selected');
+
+                $(this).addClass('selected');
+
+                var tipo = $(this).data('tipo');
+                $('#TipoAlerta').val(tipo);
+
+                console.log('Tipo selecionado:', tipo);
+                console.log('Possui classe selected:', $(this).hasClass('selected'));
+                console.log('Classes do card:', $(this).attr('class'));
+
+                configurarCamposRelacionados(tipo);
+            }
+            catch (error)
+            {
+                console.error('ERRO no click handler:', error);
+                TratamentoErroComLinha("alertas_upsert.js", "tipo-alerta-card.click", error);
+            }
+        });
+
+        var tipoExibicaoDropdown = document.querySelector("#TipoExibicao");
+        if (tipoExibicaoDropdown && tipoExibicaoDropdown.ej2_instances)
+        {
+            tipoExibicaoDropdown.ej2_instances[0].change = function (args)
+            {
+                try
+                {
                     configurarCamposExibicao(args.value);
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'alertas_upsert.js',
-                        'TipoExibicao.change',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("alertas_upsert.js", "TipoExibicao.change", error);
                 }
             };
         }
 
-        $('#formAlerta').on('submit', function (e) {
-            try {
+        $('#formAlerta').on('submit', function (e)
+        {
+            try
+            {
                 e.preventDefault();
                 e.stopPropagation();
                 e.stopImmediatePropagation();
 
-                if (!validarFormulario()) {
+                if (!validarFormulario())
+                {
                     return false;
                 }
 
                 var btnSubmit = $(this).find('button[type="submit"]');
-                if (btnSubmit.length) {
+                if (btnSubmit.length)
+                {
                     btnSubmit.prop('disabled', true);
                 }
 
                 salvarAlerta();
 
                 return false;
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'alertas_upsert.js',
-                    'formAlerta.submit',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("alertas_upsert.js", "formAlerta.submit", error);
                 return false;
             }
         });
 
         console.log('>>> Event handlers configurados!');
-    } catch (error) {
+    }
+    catch (error)
+    {
         console.error('ERRO em configurarEventHandlers:', error);
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'configurarEventHandlers',
-            error,
-        );
-    }
-}
-
-function configurarCamposRelacionados(tipo) {
-    try {
+        TratamentoErroComLinha("alertas_upsert.js", "configurarEventHandlers", error);
+    }
+}
+
+function configurarCamposRelacionados(tipo)
+{
+    try
+    {
 
         $('#divViagem, #divManutencao, #divMotorista, #divVeiculo').hide();
         $('#secaoVinculos').hide();
 
-        if (document.querySelector('#ViagemId')?.ej2_instances) {
-            document.querySelector('#ViagemId').ej2_instances[0].value = null;
-        }
-        if (document.querySelector('#ManutencaoId')?.ej2_instances) {
-            document.querySelector('#ManutencaoId').ej2_instances[0].value =
-                null;
-        }
-        if (document.querySelector('#MotoristaId')?.ej2_instances) {
-            document.querySelector('#MotoristaId').ej2_instances[0].value =
-                null;
-        }
-        if (document.querySelector('#VeiculoId')?.ej2_instances) {
-            document.querySelector('#VeiculoId').ej2_instances[0].value = null;
-        }
-
-        switch (parseInt(tipo)) {
+        if (document.querySelector("#ViagemId")?.ej2_instances)
+        {
+            document.querySelector("#ViagemId").ej2_instances[0].value = null;
+        }
+        if (document.querySelector("#ManutencaoId")?.ej2_instances)
+        {
+            document.querySelector("#ManutencaoId").ej2_instances[0].value = null;
+        }
+        if (document.querySelector("#MotoristaId")?.ej2_instances)
+        {
+            document.querySelector("#MotoristaId").ej2_instances[0].value = null;
+        }
+        if (document.querySelector("#VeiculoId")?.ej2_instances)
+        {
+            document.querySelector("#VeiculoId").ej2_instances[0].value = null;
+        }
+
+        switch (parseInt(tipo))
+        {
             case 1:
                 $('#divViagem').show();
                 $('#secaoVinculos').show();
@@ -163,17 +170,17 @@
 
                 break;
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'configurarCamposRelacionados',
-            error,
-        );
-    }
-}
-
-function configurarCamposExibicao(tipoExibicao) {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "configurarCamposRelacionados", error);
+    }
+}
+
+function configurarCamposExibicao(tipoExibicao)
+{
+    try
+    {
         var tipo = parseInt(tipoExibicao);
         console.log('Configurando campos para TipoExibicao:', tipo);
 
@@ -188,10 +195,10 @@
         var lblHorarioExibicao = document.getElementById('lblHorarioExibicao');
 
         if (lblDataExibicao) lblDataExibicao.textContent = 'Data de Exibição';
-        if (lblHorarioExibicao)
-            lblHorarioExibicao.textContent = 'Horário de Exibição';
-
-        switch (tipo) {
+        if (lblHorarioExibicao) lblHorarioExibicao.textContent = 'Horário de Exibição';
+
+        switch (tipo)
+        {
             case 1:
 
                 $('#divDataExpiracao').show();
@@ -212,8 +219,7 @@
 
             case 4:
 
-                if (lblDataExibicao)
-                    lblDataExibicao.textContent = 'Data Inicial';
+                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                 $('#divDataExibicao').show();
                 $('#divHorarioExibicao').show();
                 $('#divDataExpiracao').show();
@@ -221,8 +227,7 @@
 
             case 5:
 
-                if (lblDataExibicao)
-                    lblDataExibicao.textContent = 'Data Inicial';
+                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                 $('#divDataExibicao').show();
                 $('#divHorarioExibicao').show();
                 $('#divDataExpiracao').show();
@@ -231,8 +236,7 @@
 
             case 6:
 
-                if (lblDataExibicao)
-                    lblDataExibicao.textContent = 'Data Inicial';
+                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                 $('#divDataExibicao').show();
                 $('#divHorarioExibicao').show();
                 $('#divDataExpiracao').show();
@@ -241,8 +245,7 @@
 
             case 7:
 
-                if (lblDataExibicao)
-                    lblDataExibicao.textContent = 'Data Inicial';
+                if (lblDataExibicao) lblDataExibicao.textContent = 'Data Inicial';
                 $('#divDataExibicao').show();
                 $('#divHorarioExibicao').show();
                 $('#divDataExpiracao').show();
@@ -255,10 +258,7 @@
                 $('#divDataExpiracao').show();
                 $('#calendarContainerAlerta').show();
 
-                if (
-                    typeof initCalendarioAlerta === 'function' &&
-                    !window.calendarioAlertaInstance
-                ) {
+                if (typeof initCalendarioAlerta === 'function' && !window.calendarioAlertaInstance) {
                     initCalendarioAlerta();
                 }
                 break;
@@ -270,180 +270,191 @@
         }
 
         console.log('Campos configurados para tipo:', tipo);
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'configurarCamposExibicao',
-            error,
-        );
-    }
-}
-
-function aplicarSelecaoInicial() {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "configurarCamposExibicao", error);
+    }
+}
+
+function aplicarSelecaoInicial()
+{
+    try
+    {
 
         var tipoAtual = $('#TipoAlerta').val();
-        if (tipoAtual) {
-            $(`.tipo-alerta-card[data-tipo="${tipoAtual}"]`).addClass(
-                'selected',
-            );
+        if (tipoAtual)
+        {
+            $(`.tipo-alerta-card[data-tipo="${tipoAtual}"]`).addClass('selected');
             configurarCamposRelacionados(tipoAtual);
         }
 
-        var tipoExibicaoDropdown = document.querySelector('#TipoExibicao');
-        if (tipoExibicaoDropdown && tipoExibicaoDropdown.ej2_instances) {
+        var tipoExibicaoDropdown = document.querySelector("#TipoExibicao");
+        if (tipoExibicaoDropdown && tipoExibicaoDropdown.ej2_instances)
+        {
             var tipoExibicaoAtual = tipoExibicaoDropdown.ej2_instances[0].value;
-            if (tipoExibicaoAtual) {
+            if (tipoExibicaoAtual)
+            {
                 configurarCamposExibicao(tipoExibicaoAtual);
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'aplicarSelecaoInicial',
-            error,
-        );
-    }
-}
-
-function configurarValidacao() {
-    try {
-
-        var tituloInput = document.querySelector('#Titulo');
-        if (tituloInput && tituloInput.ej2_instances) {
-            tituloInput.ej2_instances[0].blur = function () {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "aplicarSelecaoInicial", error);
+    }
+}
+
+function configurarValidacao()
+{
+    try
+    {
+
+        var tituloInput = document.querySelector("#Titulo");
+        if (tituloInput && tituloInput.ej2_instances)
+        {
+            tituloInput.ej2_instances[0].blur = function ()
+            {
                 validarCampo('Titulo', 'Título é obrigatório');
             };
         }
 
-        var descricaoInput = document.querySelector('#Descricao');
-        if (descricaoInput && descricaoInput.ej2_instances) {
-            descricaoInput.ej2_instances[0].blur = function () {
+        var descricaoInput = document.querySelector("#Descricao");
+        if (descricaoInput && descricaoInput.ej2_instances)
+        {
+            descricaoInput.ej2_instances[0].blur = function ()
+            {
                 validarCampo('Descricao', 'Descrição é obrigatória');
             };
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'configurarValidacao',
-            error,
-        );
-    }
-}
-
-function configurarAvisoUsuarios() {
-    try {
-        var usuariosSelect = document.querySelector('#UsuariosIds');
-        if (usuariosSelect && usuariosSelect.ej2_instances) {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "configurarValidacao", error);
+    }
+}
+
+function configurarAvisoUsuarios()
+{
+    try
+    {
+        var usuariosSelect = document.querySelector("#UsuariosIds");
+        if (usuariosSelect && usuariosSelect.ej2_instances)
+        {
             var multiselect = usuariosSelect.ej2_instances[0];
 
-            if (!$('#avisoTodosUsuarios').length) {
-                var avisoHtml =
-                    '<div id="avisoTodosUsuarios" style="display:none; margin-top: 8px; padding: 8px 12px; background-color: #e0f2fe; border-left: 3px solid #0ea5e9; border-radius: 4px; font-size: 0.85rem; color: #0c4a6e;"><i class="fa-duotone fa-info-circle" style="margin-right: 6px;"></i>Nenhum usuário selecionado. O alerta será exibido para <strong>todos os usuários</strong>.</div>';
+            if (!$('#avisoTodosUsuarios').length)
+            {
+                var avisoHtml = '<div id="avisoTodosUsuarios" style="display:none; margin-top: 8px; padding: 8px 12px; background-color: #e0f2fe; border-left: 3px solid #0ea5e9; border-radius: 4px; font-size: 0.85rem; color: #0c4a6e;"><i class="fa-duotone fa-info-circle" style="margin-right: 6px;"></i>Nenhum usuário selecionado. O alerta será exibido para <strong>todos os usuários</strong>.</div>';
                 $(usuariosSelect).closest('.col-md-12').append(avisoHtml);
             }
 
-            multiselect.change = function (args) {
+            multiselect.change = function (args)
+            {
                 var usuarios = multiselect.value;
-                if (!usuarios || usuarios.length === 0) {
+                if (!usuarios || usuarios.length === 0)
+                {
                     $('#avisoTodosUsuarios').slideDown(200);
                     $('[data-valmsg-for="UsuariosIds"]').text('').hide();
-                } else {
+                }
+                else
+                {
                     $('#avisoTodosUsuarios').slideUp(200);
                 }
             };
 
             var valoresIniciais = multiselect.value;
-            if (!valoresIniciais || valoresIniciais.length === 0) {
+            if (!valoresIniciais || valoresIniciais.length === 0)
+            {
                 $('#avisoTodosUsuarios').show();
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'configurarAvisoUsuarios',
-            error,
-        );
-    }
-}
-
-function validarCampo(campoId, mensagemErro) {
-    try {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "configurarAvisoUsuarios", error);
+    }
+}
+
+function validarCampo(campoId, mensagemErro)
+{
+    try
+    {
         var campo = document.querySelector(`#${campoId}`);
         var spanErro = $(`[data-valmsg-for="${campoId}"]`);
 
-        if (campo && campo.ej2_instances) {
+        if (campo && campo.ej2_instances)
+        {
             var valor = campo.ej2_instances[0].value;
 
-            if (!valor || valor.trim() === '') {
+            if (!valor || valor.trim() === '')
+            {
                 spanErro.text(mensagemErro).show();
                 return false;
-            } else {
+            }
+            else
+            {
                 spanErro.text('').hide();
                 return true;
             }
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha('alertas_upsert.js', 'validarCampo', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "validarCampo", error);
         return false;
     }
 }
 
-function validarFormulario() {
-    try {
+function validarFormulario()
+{
+    try
+    {
         var valido = true;
 
-        if (!validarCampo('Titulo', 'O título é obrigatório')) {
+        if (!validarCampo('Titulo', 'O título é obrigatório'))
+        {
             valido = false;
         }
 
-        if (!validarCampo('Descricao', 'A descrição é obrigatória')) {
+        if (!validarCampo('Descricao', 'A descrição é obrigatória'))
+        {
             valido = false;
         }
 
         var tipoAlerta = $('#TipoAlerta').val();
-        if (!tipoAlerta || tipoAlerta == '0') {
-            AppToast.show('Amarelo', 'Selecione um tipo de alerta', 2000);
+        if (!tipoAlerta || tipoAlerta == '0')
+        {
+            AppToast.show("Amarelo", "Selecione um tipo de alerta", 2000);
             valido = false;
         }
 
-        var usuariosSelect = document.querySelector('#UsuariosIds');
-        if (usuariosSelect && usuariosSelect.ej2_instances) {
+        var usuariosSelect = document.querySelector("#UsuariosIds");
+        if (usuariosSelect && usuariosSelect.ej2_instances)
+        {
             $('[data-valmsg-for="UsuariosIds"]').text('').hide();
         }
 
-        var tipoExibicao = parseInt(
-            document.querySelector('#TipoExibicao')?.ej2_instances?.[0]
-                ?.value || 1,
-        );
-
-        switch (tipoExibicao) {
+        var tipoExibicao = parseInt(document.querySelector("#TipoExibicao")?.ej2_instances?.[0]?.value || 1);
+
+        switch (tipoExibicao)
+        {
             case 2:
-                var horario =
-                    document.querySelector('#HorarioExibicao')
-                        ?.ej2_instances?.[0]?.value;
-                if (!horario) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione o horário de exibição',
-                        2000,
-                    );
+                var horario = document.querySelector("#HorarioExibicao")?.ej2_instances?.[0]?.value;
+                if (!horario)
+                {
+                    AppToast.show("Amarelo", "Selecione o horário de exibição", 2000);
                     valido = false;
                 }
                 break;
 
             case 3:
-                var dataExib =
-                    document.querySelector('#DataExibicao')?.ej2_instances?.[0]
-                        ?.value;
-                if (!dataExib) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione a data de exibição',
-                        2000,
-                    );
+                var dataExib = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
+                if (!dataExib)
+                {
+                    AppToast.show("Amarelo", "Selecione a data de exibição", 2000);
                     valido = false;
                 }
                 break;
@@ -452,53 +463,35 @@
             case 5:
             case 6:
             case 7:
-                var dataInicial =
-                    document.querySelector('#DataExibicao')?.ej2_instances?.[0]
-                        ?.value;
-                var dataFinal =
-                    document.querySelector('#DataExpiracao')?.ej2_instances?.[0]
-                        ?.value;
-                if (!dataInicial) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione a data inicial da recorrência',
-                        2000,
-                    );
+                var dataInicial = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
+                var dataFinal = document.querySelector("#DataExpiracao")?.ej2_instances?.[0]?.value;
+                if (!dataInicial)
+                {
+                    AppToast.show("Amarelo", "Selecione a data inicial da recorrência", 2000);
                     valido = false;
                 }
-                if (!dataFinal) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione a data final da recorrência',
-                        2000,
-                    );
+                if (!dataFinal)
+                {
+                    AppToast.show("Amarelo", "Selecione a data final da recorrência", 2000);
                     valido = false;
                 }
 
-                if (tipoExibicao === 5 || tipoExibicao === 6) {
-                    var diasSemana =
-                        document.querySelector('#lstDiasAlerta')
-                            ?.ej2_instances?.[0]?.value;
-                    if (!diasSemana || diasSemana.length === 0) {
-                        AppToast.show(
-                            'Amarelo',
-                            'Selecione pelo menos um dia da semana',
-                            2000,
-                        );
+                if (tipoExibicao === 5 || tipoExibicao === 6)
+                {
+                    var diasSemana = document.querySelector("#lstDiasAlerta")?.ej2_instances?.[0]?.value;
+                    if (!diasSemana || diasSemana.length === 0)
+                    {
+                        AppToast.show("Amarelo", "Selecione pelo menos um dia da semana", 2000);
                         valido = false;
                     }
                 }
 
-                if (tipoExibicao === 7) {
-                    var diaMes =
-                        document.querySelector('#lstDiasMesAlerta')
-                            ?.ej2_instances?.[0]?.value;
-                    if (!diaMes) {
-                        AppToast.show(
-                            'Amarelo',
-                            'Selecione o dia do mês',
-                            2000,
-                        );
+                if (tipoExibicao === 7)
+                {
+                    var diaMes = document.querySelector("#lstDiasMesAlerta")?.ej2_instances?.[0]?.value;
+                    if (!diaMes)
+                    {
+                        AppToast.show("Amarelo", "Selecione o dia do mês", 2000);
                         valido = false;
                     }
                 }
@@ -506,37 +499,40 @@
 
             case 8:
                 var datasSelecionadas = window.datasAlertaSelecionadas || [];
-                if (datasSelecionadas.length === 0) {
-                    AppToast.show(
-                        'Amarelo',
-                        'Selecione pelo menos uma data no calendário',
-                        2000,
-                    );
+                if (datasSelecionadas.length === 0)
+                {
+                    AppToast.show("Amarelo", "Selecione pelo menos uma data no calendário", 2000);
                     valido = false;
                 }
                 break;
         }
 
         return valido;
-    } catch (error) {
-        TratamentoErroComLinha('alertas_upsert.js', 'validarFormulario', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "validarFormulario", error);
         return false;
     }
 }
 
-function salvarAlerta() {
-
-    if (window.salvandoAlerta) {
+function salvarAlerta()
+{
+
+    if (window.salvandoAlerta)
+    {
         console.log('Já existe um salvamento em andamento, ignorando...');
         return;
     }
 
-    try {
+    try
+    {
         window.salvandoAlerta = true;
 
         var dados = obterDadosFormulario();
 
-        if (!dados) {
+        if (!dados)
+        {
             console.error('Dados do formulário inválidos');
             window.salvandoAlerta = false;
             return;
@@ -546,9 +542,10 @@
             title: 'Salvando...',
             text: 'Aguarde enquanto o alerta é salvo',
             allowOutsideClick: false,
-            didOpen: () => {
+            didOpen: () =>
+            {
                 Swal.showLoading();
-            },
+            }
         });
 
         $.ajax({
@@ -556,184 +553,164 @@
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify(dados),
-            success: function (response) {
-                try {
+            success: function (response)
+            {
+                try
+                {
                     window.salvandoAlerta = false;
                     Swal.close();
 
-                    if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            response.message || 'Alerta salvo com sucesso!',
-                            2000,
-                        );
-
-                        setTimeout(function () {
+                    if (response.success)
+                    {
+                        AppToast.show("Verde", response.message || "Alerta salvo com sucesso!", 2000);
+
+                        setTimeout(function ()
+                        {
                             window.location.href = '/AlertasFrotiX';
                         }, 1500);
-                    } else {
-                        Swal.fire(
-                            'Erro',
-                            response.message || 'Erro ao salvar alerta',
-                            'error',
-                        );
                     }
-                } catch (error) {
+                    else
+                    {
+                        Swal.fire('Erro', response.message || 'Erro ao salvar alerta', 'error');
+                    }
+                }
+                catch (error)
+                {
                     window.salvandoAlerta = false;
-                    TratamentoErroComLinha(
-                        'alertas_upsert.js',
-                        'salvarAlerta.success',
-                        error,
-                    );
+                    TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 window.salvandoAlerta = false;
                 Swal.close();
-                TratamentoErroComLinha(
-                    'alertas_upsert.js',
-                    'salvarAlerta.error',
-                    error,
-                );
+                TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta.error", error);
 
                 var mensagem = 'Erro ao salvar alerta';
-                if (xhr.responseJSON && xhr.responseJSON.message) {
+                if (xhr.responseJSON && xhr.responseJSON.message)
+                {
                     mensagem = xhr.responseJSON.message;
-                } else if (xhr.status === 404) {
-                    mensagem =
-                        'Rota não encontrada (404). Verifique se a URL /AlertasFrotiX/Salvar está correta.';
-                } else if (xhr.status === 500) {
-                    mensagem =
-                        'Erro no servidor. Verifique os logs do backend.';
+                }
+                else if (xhr.status === 404)
+                {
+                    mensagem = 'Rota não encontrada (404). Verifique se a URL /AlertasFrotiX/Salvar está correta.';
+                }
+                else if (xhr.status === 500)
+                {
+                    mensagem = 'Erro no servidor. Verifique os logs do backend.';
                 }
 
                 Swal.fire('Erro', mensagem, 'error');
 
                 $('#formAlerta button[type="submit"]').prop('disabled', false);
-            },
+            }
         });
-    } catch (error) {
+    }
+    catch (error)
+    {
         window.salvandoAlerta = false;
         Swal.close();
-        TratamentoErroComLinha('alertas_upsert.js', 'salvarAlerta', error);
+        TratamentoErroComLinha("alertas_upsert.js", "salvarAlerta", error);
 
         $('#formAlerta button[type="submit"]').prop('disabled', false);
     }
 }
 
-function obterDadosFormulario() {
-    try {
-        var tipoExibicao = parseInt(
-            document.querySelector('#TipoExibicao')?.ej2_instances?.[0]
-                ?.value || 1,
-        );
+function obterDadosFormulario()
+{
+    try
+    {
+        var tipoExibicao = parseInt(document.querySelector("#TipoExibicao")?.ej2_instances?.[0]?.value || 1);
 
         var dados = {
             AlertasFrotiXId: $('#AlertasFrotiXId').val(),
-            Titulo:
-                document.querySelector('#Titulo')?.ej2_instances?.[0]?.value ||
-                '',
-            Descricao:
-                document.querySelector('#Descricao')?.ej2_instances?.[0]
-                    ?.value || '',
+            Titulo: document.querySelector("#Titulo")?.ej2_instances?.[0]?.value || '',
+            Descricao: document.querySelector("#Descricao")?.ej2_instances?.[0]?.value || '',
             TipoAlerta: parseInt($('#TipoAlerta').val()),
-            Prioridade: parseInt(
-                document.querySelector('#Prioridade')?.ej2_instances?.[0]
-                    ?.value || 1,
-            ),
+            Prioridade: parseInt(document.querySelector("#Prioridade")?.ej2_instances?.[0]?.value || 1),
             TipoExibicao: tipoExibicao,
-            UsuariosIds:
-                document.querySelector('#UsuariosIds')?.ej2_instances?.[0]
-                    ?.value || [],
+            UsuariosIds: document.querySelector("#UsuariosIds")?.ej2_instances?.[0]?.value || []
         };
 
         var tipoAlerta = dados.TipoAlerta;
 
         if (tipoAlerta === 1)
         {
-            var viagemId =
-                document.querySelector('#ViagemId')?.ej2_instances?.[0]?.value;
-            if (viagemId) {
-                viagemId = String(viagemId)
-                    .trim()
-                    .replace(/[^a-f0-9\-]/gi, '');
+            var viagemId = document.querySelector("#ViagemId")?.ej2_instances?.[0]?.value;
+            if (viagemId)
+            {
+                viagemId = String(viagemId).trim().replace(/[^a-f0-9\-]/gi, '');
                 if (viagemId.length > 0) dados.ViagemId = viagemId;
             }
-        } else if (tipoAlerta === 2)
-        {
-            var manutencaoId =
-                document.querySelector('#ManutencaoId')?.ej2_instances?.[0]
-                    ?.value;
-            if (manutencaoId) {
-                manutencaoId = String(manutencaoId)
-                    .trim()
-                    .replace(/[^a-f0-9\-]/gi, '');
+        }
+        else if (tipoAlerta === 2)
+        {
+            var manutencaoId = document.querySelector("#ManutencaoId")?.ej2_instances?.[0]?.value;
+            if (manutencaoId)
+            {
+                manutencaoId = String(manutencaoId).trim().replace(/[^a-f0-9\-]/gi, '');
                 if (manutencaoId.length > 0) dados.ManutencaoId = manutencaoId;
             }
-        } else if (tipoAlerta === 3)
-        {
-            var motoristaId =
-                document.querySelector('#MotoristaId')?.ej2_instances?.[0]
-                    ?.value;
-            if (motoristaId) {
-                motoristaId = String(motoristaId)
-                    .trim()
-                    .replace(/[^a-f0-9\-]/gi, '');
+        }
+        else if (tipoAlerta === 3)
+        {
+            var motoristaId = document.querySelector("#MotoristaId")?.ej2_instances?.[0]?.value;
+            if (motoristaId)
+            {
+                motoristaId = String(motoristaId).trim().replace(/[^a-f0-9\-]/gi, '');
                 if (motoristaId.length > 0) dados.MotoristaId = motoristaId;
             }
-        } else if (tipoAlerta === 4)
-        {
-            var veiculoId =
-                document.querySelector('#VeiculoId')?.ej2_instances?.[0]?.value;
-            if (veiculoId) {
-                veiculoId = String(veiculoId)
-                    .trim()
-                    .replace(/[^a-f0-9\-]/gi, '');
+        }
+        else if (tipoAlerta === 4)
+        {
+            var veiculoId = document.querySelector("#VeiculoId")?.ej2_instances?.[0]?.value;
+            if (veiculoId)
+            {
+                veiculoId = String(veiculoId).trim().replace(/[^a-f0-9\-]/gi, '');
                 if (veiculoId.length > 0) dados.VeiculoId = veiculoId;
             }
         }
 
-        if (tipoExibicao >= 3 && tipoExibicao <= 7) {
-            var dataExibicao =
-                document.querySelector('#DataExibicao')?.ej2_instances?.[0]
-                    ?.value;
+        if (tipoExibicao >= 3 && tipoExibicao <= 7)
+        {
+            var dataExibicao = document.querySelector("#DataExibicao")?.ej2_instances?.[0]?.value;
             if (dataExibicao) dados.DataExibicao = dataExibicao;
         }
 
-        if (tipoExibicao >= 2) {
-            var horario =
-                document.querySelector('#HorarioExibicao')?.ej2_instances?.[0]
-                    ?.value;
+        if (tipoExibicao >= 2)
+        {
+            var horario = document.querySelector("#HorarioExibicao")?.ej2_instances?.[0]?.value;
             if (horario) dados.HorarioExibicao = horario;
         }
 
-        var dataExpiracao =
-            document.querySelector('#DataExpiracao')?.ej2_instances?.[0]?.value;
+        var dataExpiracao = document.querySelector("#DataExpiracao")?.ej2_instances?.[0]?.value;
         if (dataExpiracao) dados.DataExpiracao = dataExpiracao;
 
-        if (tipoExibicao === 5 || tipoExibicao === 6) {
-            var diasSemana =
-                document.querySelector('#lstDiasAlerta')?.ej2_instances?.[0]
-                    ?.value;
-            if (diasSemana && diasSemana.length > 0) {
+        if (tipoExibicao === 5 || tipoExibicao === 6)
+        {
+            var diasSemana = document.querySelector("#lstDiasAlerta")?.ej2_instances?.[0]?.value;
+            if (diasSemana && diasSemana.length > 0)
+            {
                 dados.DiasSemana = diasSemana;
             }
         }
 
-        if (tipoExibicao === 7) {
-            var diaMes =
-                document.querySelector('#lstDiasMesAlerta')?.ej2_instances?.[0]
-                    ?.value;
-            if (diaMes) {
+        if (tipoExibicao === 7)
+        {
+            var diaMes = document.querySelector("#lstDiasMesAlerta")?.ej2_instances?.[0]?.value;
+            if (diaMes)
+            {
                 dados.DiaMesRecorrencia = parseInt(diaMes);
             }
         }
 
-        if (tipoExibicao === 8) {
+        if (tipoExibicao === 8)
+        {
             var datasSelecionadas = window.datasAlertaSelecionadas || [];
-            if (datasSelecionadas.length > 0) {
-
-                var datasFormatadas = datasSelecionadas.map(function (d) {
+            if (datasSelecionadas.length > 0)
+            {
+
+                var datasFormatadas = datasSelecionadas.map(function(d) {
                     var data = new Date(d);
                     var mes = ('0' + (data.getMonth() + 1)).slice(-2);
                     var dia = ('0' + data.getDate()).slice(-2);
@@ -745,27 +722,29 @@
 
         console.log('Dados do formulário preparados:', dados);
         return dados;
-    } catch (error) {
-        TratamentoErroComLinha(
-            'alertas_upsert.js',
-            'obterDadosFormulario',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("alertas_upsert.js", "obterDadosFormulario", error);
         return null;
     }
 }
 
-function configurarDropdownMotoristaComFoto() {
-    try {
+function configurarDropdownMotoristaComFoto()
+{
+    try
+    {
         const motoristaDropdown = document.getElementById('MotoristaId');
-        if (!motoristaDropdown?.ej2_instances?.[0]) {
+        if (!motoristaDropdown?.ej2_instances?.[0])
+        {
             console.log('Dropdown de motoristas não encontrado');
             return;
         }
 
         const dropdown = motoristaDropdown.ej2_instances[0];
 
-        dropdown.itemTemplate = function (data) {
+        dropdown.itemTemplate = function (data)
+        {
             if (!data) return '';
 
             const foto = data.Group?.Name || '/images/placeholder-user.png';
@@ -781,7 +760,8 @@
                 </div>`;
         };
 
-        dropdown.valueTemplate = function (data) {
+        dropdown.valueTemplate = function (data)
+        {
             if (!data) return '';
 
             const foto = data.Group?.Name || '/images/placeholder-user.png';
@@ -800,33 +780,36 @@
         dropdown.dataBind();
 
         console.log('Dropdown de motoristas configurada com foto');
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao configurar dropdown motorista:', error);
-        if (typeof Alerta !== 'undefined') {
-            Alerta.TratamentoErroComLinha(
-                'alertas_upsert.js',
-                'configurarDropdownMotoristaComFoto',
-                error,
-            );
-        }
-    }
-}
-
-document.addEventListener('DOMContentLoaded', function () {
+        if (typeof Alerta !== 'undefined')
+        {
+            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownMotoristaComFoto", error);
+        }
+    }
+}
+
+document.addEventListener('DOMContentLoaded', function ()
+{
     setTimeout(configurarDropdownMotoristaComFoto, 300);
 });
 
-function configurarDropdownAgendamentoRico() {
-    try {
+function configurarDropdownAgendamentoRico()
+{
+    try
+    {
         const viagemDropdown = document.getElementById('ViagemId');
-        if (!viagemDropdown?.ej2_instances?.[0]) {
+        if (!viagemDropdown?.ej2_instances?.[0])
+        {
             console.log('Dropdown de viagens não encontrado');
             return;
         }
 
         const dropdown = viagemDropdown.ej2_instances[0];
 
-        dropdown.itemTemplate = function (data) {
+        dropdown.itemTemplate = function (data)
+        {
             if (!data) return '';
 
             return `
@@ -840,7 +823,7 @@
                               <strong>${data.HoraInicio || ''}</strong>
                             </span>
                         </div>
-                        <span class="agendamento-badge">${data.Finalidade || 'Aniversário'}</span>
+                        <span class="agendamento-badge">${data.Finalidade || 'Diversos'}</span>
                     </div>
 
                     <div class="agendamento-card-body">
@@ -864,7 +847,8 @@
                 </div>`;
         };
 
-        dropdown.valueTemplate = function (data) {
+        dropdown.valueTemplate = function (data)
+        {
             if (!data) return '';
 
             return `
@@ -877,22 +861,19 @@
                 </div>`;
         };
 
-        dropdown.filtering = function (e) {
+        dropdown.filtering = function (e)
+        {
             if (!e.text) return;
 
             const query = e.text.toLowerCase();
-            const filtered = dropdown.dataSource.filter((item) => {
+            const filtered = dropdown.dataSource.filter(item =>
+            {
                 return (
-                    (item.DataInicial &&
-                        item.DataInicial.toLowerCase().includes(query)) ||
-                    (item.Origem &&
-                        item.Origem.toLowerCase().includes(query)) ||
-                    (item.Destino &&
-                        item.Destino.toLowerCase().includes(query)) ||
-                    (item.Requisitante &&
-                        item.Requisitante.toLowerCase().includes(query)) ||
-                    (item.Finalidade &&
-                        item.Finalidade.toLowerCase().includes(query))
+                    (item.DataInicial && item.DataInicial.toLowerCase().includes(query)) ||
+                    (item.Origem && item.Origem.toLowerCase().includes(query)) ||
+                    (item.Destino && item.Destino.toLowerCase().includes(query)) ||
+                    (item.Requisitante && item.Requisitante.toLowerCase().includes(query)) ||
+                    (item.Finalidade && item.Finalidade.toLowerCase().includes(query))
                 );
             });
 
@@ -902,39 +883,38 @@
         dropdown.dataBind();
 
         console.log('Dropdown de agendamentos configurada com cards ricos');
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao configurar dropdown agendamento:', error);
-        if (typeof Alerta !== 'undefined') {
-            Alerta.TratamentoErroComLinha(
-                'alertas_upsert.js',
-                'configurarDropdownAgendamentoRico',
-                error,
-            );
-        }
-    }
-}
-
-document.addEventListener('DOMContentLoaded', function () {
+        if (typeof Alerta !== 'undefined')
+        {
+            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownAgendamentoRico", error);
+        }
+    }
+}
+
+document.addEventListener('DOMContentLoaded', function ()
+{
     setTimeout(configurarDropdownAgendamentoRico, 300);
 });
 
-function configurarDropdownManutencaoRico() {
-    try {
+function configurarDropdownManutencaoRico()
+{
+    try
+    {
         const el = document.getElementById('ManutencaoId');
         const ddl = el?.ej2_instances?.[0];
         if (!ddl) return;
 
-        if (
-            ddl.dataSource?.length &&
-            ddl.dataSource[0].Text !== undefined &&
-            window.__manutencoesDS
-        ) {
+        if (ddl.dataSource?.length && ddl.dataSource[0].Text !== undefined && window.__manutencoesDS)
+        {
             ddl.dataSource = window.__manutencoesDS;
             ddl.fields = { text: 'NumOS', value: 'ManutencaoId' };
             ddl.dataBind();
         }
 
-        ddl.itemTemplate = function (data) {
+        ddl.itemTemplate = function (data)
+        {
             if (!data) return '';
 
             const linha = (icon, val) =>
@@ -947,10 +927,9 @@
        <span class="manutencao-valor">${val || '—'}</span>
      </span>`;
 
-            const reservaTxt =
-                data.ReservaEnviado === 'Sim'
-                    ? data.CarroReserva || 'Reserva enviada'
-                    : 'Reserva não enviada';
+            const reservaTxt = (data.ReservaEnviado === 'Sim')
+                ? (data.CarroReserva || 'Reserva enviada')
+                : 'Reserva não enviada';
 
             return `
                     <div class="manutencao-card-item">
@@ -978,7 +957,8 @@
                     </div>`;
         };
 
-        ddl.valueTemplate = function (data) {
+        ddl.valueTemplate = function (data)
+        {
             if (!data) return '';
             return `
 <div class="manutencao-selected">
@@ -987,34 +967,31 @@
 </div>`;
         };
 
-        ddl.filtering = function (e) {
+        ddl.filtering = function (e)
+        {
             const q = (e.text || '').toLowerCase();
             if (!q) return;
             const src = ddl.dataSource || [];
-            e.updateData(
-                src.filter(
-                    (d) =>
-                        (d.NumOS || '').toLowerCase().includes(q) ||
-                        (d.Veiculo || '').toLowerCase().includes(q) ||
-                        (d.CarroReserva || '').toLowerCase().includes(q),
-                ),
-            );
+            e.updateData(src.filter(d =>
+                (d.NumOS || '').toLowerCase().includes(q) ||
+                (d.Veiculo || '').toLowerCase().includes(q) ||
+                (d.CarroReserva || '').toLowerCase().includes(q)
+            ));
         };
 
         ddl.dataBind();
         console.log('ManutencaoId com cards ricos ✅');
-    } catch (err) {
+    } catch (err)
+    {
         console.error('Erro ao configurar dropdown manutenção:', err);
-        if (typeof Alerta !== 'undefined') {
-            Alerta.TratamentoErroComLinha(
-                'alertas_upsert.js',
-                'configurarDropdownManutencaoRico',
-                err,
-            );
-        }
-    }
-}
-
-document.addEventListener('DOMContentLoaded', () => {
+        if (typeof Alerta !== 'undefined')
+        {
+            Alerta.TratamentoErroComLinha("alertas_upsert.js", "configurarDropdownManutencaoRico", err);
+        }
+    }
+}
+
+document.addEventListener('DOMContentLoaded', () =>
+{
     setTimeout(configurarDropdownManutencaoRico, 300);
 });
```
