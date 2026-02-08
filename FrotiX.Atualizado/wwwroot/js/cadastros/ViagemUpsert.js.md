# wwwroot/js/cadastros/ViagemUpsert.js

**Mudanca:** GRANDE | **+2897** linhas | **-2932** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/ViagemUpsert.js
+++ ATUAL: wwwroot/js/cadastros/ViagemUpsert.js
@@ -1,164 +1,183 @@
-(function () {
-    try {
-        var scriptTag =
-            document.currentScript ||
-            document.scripts[document.scripts.length - 1];
-        var __scriptName = scriptTag.src.split('/').pop();
+(function ()
+{
+    try
+    {
+        var scriptTag = document.currentScript || document.scripts[document.scripts.length - 1];
+        var __scriptName = scriptTag.src.split("/").pop();
         window.__scriptName = __scriptName;
-    } catch (error) {
-
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'IIFE_ObterScriptName',
-            error,
-        );
+    }
+    catch (error)
+    {
+
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "IIFE_ObterScriptName", error);
     }
 })();
 
-function mostrarModalSalvando() {
-    try {
+function mostrarModalSalvando()
+{
+    try
+    {
         const el = document.getElementById('loadingOverlaySalvando');
-        if (el) {
+        if (el)
+        {
             el.style.display = 'flex';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'mostrarModalSalvando',
-            error,
-        );
-    }
-}
-
-function esconderModalSalvando() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "mostrarModalSalvando", error);
+    }
+}
+
+function esconderModalSalvando()
+{
+    try
+    {
         const el = document.getElementById('loadingOverlaySalvando');
-        if (el) {
+        if (el)
+        {
             el.style.display = 'none';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'esconderModalSalvando',
-            error,
-        );
-    }
-}
-
-function enviarFormularioViaAjax(handler, id) {
-
-    AppToast.show('Amarelo', 'Salvando dados...', 2000);
-
-    const form = document.querySelector('form');
-    const formData = new FormData(form);
-
-    const base64 = $('#hiddenFoto').val();
-    if (base64 && base64.length > 0) {
-        console.log('Incluindo imagem:', base64.length, 'caracteres');
-        formData.append('FotoBase64', base64);
-
-        $('#hiddenFoto').val('');
-    }
-
-    const fichaExistente = $('#hiddenFichaExistente').val();
-    if (fichaExistente) {
-        formData.append('FichaVistoriaExistente', fichaExistente);
-    }
-
-    let url = `/Viagens/Upsert?handler=${handler}`;
-    if (id) {
-        url += `&id=${id}`;
-    }
-
-    const token = $('input[name="__RequestVerificationToken"]').val();
-
-    $.ajax({
-        url: url,
-        type: 'POST',
-        data: formData,
-        processData: false,
-        contentType: false,
-        headers: {
-            RequestVerificationToken: token,
-        },
-        success: function (response) {
-            if (response.success) {
-                AppToast.show(
-                    'Verde',
-                    handler === 'Edit'
-                        ? 'Viagem atualizada com sucesso!'
-                        : 'Viagem criada com sucesso!',
-                    2000,
-                );
-
-                setTimeout(function () {
-                    window.location.href = '/Viagens';
-                }, 2000);
-            } else {
-                AppToast.show(
-                    'Vermelho',
-                    response.message || 'Erro ao salvar',
-                    3000,
-                );
-            }
-        },
-        error: function (xhr, status, error) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "esconderModalSalvando", error);
+    }
+}
+
+function enviarFormularioViaAjax(handler, id)
+{
+    try
+    {
+
+        AppToast.show("Amarelo", "Salvando dados...", 2000);
+
+        const form = document.querySelector('form');
+        const formData = new FormData(form);
+
+        const base64 = $("#hiddenFoto").val();
+        if (base64 && base64.length > 0)
+        {
+            console.log("Incluindo imagem:", base64.length, "caracteres");
+            formData.append("FotoBase64", base64);
+
+            $("#hiddenFoto").val("");
+        }
+
+        const fichaExistente = $("#hiddenFichaExistente").val();
+        if (fichaExistente)
+        {
+            formData.append("FichaVistoriaExistente", fichaExistente);
+        }
+
+        let url = `/Viagens/Upsert?handler=${handler}`;
+        if (id)
+        {
+            url += `&id=${id}`;
+        }
+
+        const token = $('input[name="__RequestVerificationToken"]').val();
+
+        $.ajax({
+            url: url,
+            type: 'POST',
+            data: formData,
+            processData: false,
+            contentType: false,
+            headers: {
+                'RequestVerificationToken': token
+            },
+            success: function (response)
+            {
+                try
+                {
+
+                    if (response.success)
+                    {
+
+                        AppToast.show("Verde", handler === "Edit" ? "Viagem atualizada com sucesso!" : "Viagem criada com sucesso!", 2000);
+
+                        setTimeout(function ()
+                        {
+                            window.location.href = '/Viagens';
+                        }, 2000);
+                    } else
+                    {
+
+                        AppToast.show("Vermelho", response.message || "Erro ao salvar", 3000);
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "enviarFormularioViaAjax.ajax.success", error);
+                }
+            },
+        error: function (xhr, status, error)
+        {
             console.error('Erro AJAX:', status, error);
             console.error('Response:', xhr.responseText);
 
-            let mensagemErro = 'Erro ao salvar. Tente novamente.';
-            try {
+            let mensagemErro = "Erro ao salvar. Tente novamente.";
+            try
+            {
                 const resp = JSON.parse(xhr.responseText);
                 if (resp.message) mensagemErro = resp.message;
-            } catch (e) {
-
-            }
-
-            AppToast.show('Vermelho', mensagemErro, 3000);
-        },
+            } catch (e)
+            {
+
+            }
+
+            AppToast.show("Vermelho", mensagemErro, 3000);
+        }
     });
-}
-
-$(document).ready(function () {
-    try {
-
-        $('#btnEscondido').click(function (event) {
-            try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "enviarFormularioViaAjax", error);
+    }
+}
+
+$(document).ready(function ()
+{
+    try
+    {
+
+        $("#btnEscondido").click(function (event)
+        {
+            try
+            {
                 event.preventDefault();
 
-                const handler = $(this).data('handler');
-                const id = $(this).data('id');
+                const handler = $(this).data("handler");
+                const id = $(this).data("id");
 
                 mostrarModalSalvando();
 
                 const form = document.querySelector('form');
                 const formData = new FormData(form);
 
-                const base64 = $('#hiddenFoto').val();
-                if (base64 && base64.length > 0) {
-                    console.log(
-                        'Incluindo imagem com',
-                        base64.length,
-                        'caracteres',
-                    );
-                    formData.append('FotoBase64', base64);
-
-                    $('#hiddenFoto').val('');
-                }
-
-                const fichaExistente = $('#hiddenFichaExistente').val();
-                if (fichaExistente) {
-                    formData.append('FichaVistoriaExistente', fichaExistente);
+                const base64 = $("#hiddenFoto").val();
+                if (base64 && base64.length > 0)
+                {
+                    console.log("Incluindo imagem com", base64.length, "caracteres");
+                    formData.append("FotoBase64", base64);
+
+                    $("#hiddenFoto").val("");
+                }
+
+                const fichaExistente = $("#hiddenFichaExistente").val();
+                if (fichaExistente)
+                {
+                    formData.append("FichaVistoriaExistente", fichaExistente);
                 }
 
                 let url = `/Viagens/Upsert?handler=${handler}`;
-                if (id) {
+                if (id)
+                {
                     url += `&id=${id}`;
                 }
 
-                const token = $(
-                    'input[name="__RequestVerificationToken"]',
-                ).val();
+                const token = $('input[name="__RequestVerificationToken"]').val();
 
                 $.ajax({
                     url: url,
@@ -167,52 +186,41 @@
                     processData: false,
                     contentType: false,
                     headers: {
-                        RequestVerificationToken: token,
+                        'RequestVerificationToken': token
                     },
-                    success: function (response) {
+                    success: function (response)
+                    {
 
                         esconderModalSalvando();
 
-                        if (
-                            typeof response === 'object' &&
-                            response.success !== undefined
-                        ) {
-
-                            if (response.success) {
-                                AppToast.show(
-                                    'Verde',
-                                    handler === 'Edit'
-                                        ? 'Viagem atualizada com sucesso!'
-                                        : 'Viagem criada com sucesso!',
-                                    2000,
-                                );
-
-                                setTimeout(function () {
-                                    window.location.href =
-                                        response.redirectUrl || '/Viagens';
+                        if (typeof response === 'object' && response.success !== undefined)
+                        {
+
+                            if (response.success)
+                            {
+                                AppToast.show("Verde",
+                                    handler === "Edit" ? "Viagem atualizada com sucesso!" : "Viagem criada com sucesso!",
+                                    2000);
+
+                                setTimeout(function ()
+                                {
+                                    window.location.href = response.redirectUrl || '/Viagens';
                                 }, 2000);
-                            } else {
-                                AppToast.show(
-                                    'Vermelho',
-                                    response.message || 'Erro ao salvar',
-                                    3000,
-                                );
-                                $('#btnSubmit').prop('disabled', false);
+                            } else
+                            {
+                                AppToast.show("Vermelho", response.message || "Erro ao salvar", 3000);
+                                $("#btnSubmit").prop("disabled", false);
                             }
-                        } else {
-
-                            AppToast.show(
-                                'Verde',
-                                'Viagem salva com sucesso!',
-                                2000,
-                            );
-
-                            if (
-                                response.includes('window.location') ||
-                                response.includes('/Viagens')
-                            ) {
+                        } else
+                        {
+
+                            AppToast.show("Verde", "Viagem salva com sucesso!", 2000);
+
+                            if (response.includes('window.location') || response.includes('/Viagens'))
+                            {
                                 window.location.href = '/Viagens';
-                            } else {
+                            } else
+                            {
 
                                 document.open();
                                 document.write(response);
@@ -220,29 +228,23 @@
                             }
                         }
                     },
-                    error: function (xhr, status, error) {
+                    error: function (xhr, status, error)
+                    {
 
                         esconderModalSalvando();
 
                         console.error('Erro AJAX:', status, error);
                         console.error('Response:', xhr.responseText);
 
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro ao salvar. Tente novamente.',
-                            3000,
-                        );
-                        $('#btnSubmit').prop('disabled', false);
-                    },
+                        AppToast.show("Vermelho", "Erro ao salvar. Tente novamente.", 3000);
+                        $("#btnSubmit").prop("disabled", false);
+                    }
                 });
-            } catch (error) {
+            } catch (error)
+            {
                 esconderModalSalvando();
-                Alerta.TratamentoErroComLinha(
-                    'ViagemUpsert.js',
-                    'click.btnEscondido',
-                    error,
-                );
-                $('#btnSubmit').prop('disabled', false);
+                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnEscondido", error);
+                $("#btnSubmit").prop("disabled", false);
             }
         });
 
@@ -251,396 +253,418 @@
             position: { X: 'Right', Y: 'Top' },
             animation: {
                 show: { effect: 'SlideRightIn', duration: 600, easing: 'ease' },
-                hide: {
-                    effect: 'SlideRightOut',
-                    duration: 600,
-                    easing: 'ease',
-                },
+                hide: { effect: 'SlideRightOut', duration: 600, easing: 'ease' }
             },
             showProgressBar: true,
             progressDirection: 'Ltr',
             timeOut: 2000,
             extendedTimeout: 0,
             showCloseButton: true,
-            newestOnTop: true,
+            newestOnTop: true
         });
         toastObj.appendTo('#toast_container');
 
-        $('#modalEvento')
+        $("#modalEvento")
             .modal({
                 keyboard: true,
                 backdrop: false,
                 show: false,
             })
-            .on('hide.bs.modal', function () {
-                try {
-                    let setores = document.getElementById(
-                        'ddtSetorRequisitanteEvento',
-                    ).ej2_instances[0];
-                    setores.value = '';
-                    let requisitantes = document.getElementById(
-                        'lstRequisitanteEvento',
-                    ).ej2_instances[0];
-                    requisitantes.value = '';
-                    $('#txtNome').val('');
-                    $('#txtDescricao').val('');
-                    $('#txtDataInicial').val('');
-                    $('#txtDataFinal').val('');
-                    $('.modal-backdrop').remove();
-                    $(document.body).removeClass('modal-open');
-                } catch (error) {
+            .on("hide.bs.modal", function ()
+            {
+                try
+                {
+                    let setores = document.getElementById("ddtSetorRequisitanteEvento")
+                        .ej2_instances[0];
+                    setores.value = "";
+                    let requisitantes =
+                        document.getElementById("lstRequisitanteEvento").ej2_instances[0];
+                    requisitantes.value = "";
+                    $("#txtNome").val("");
+                    $("#txtDescricao").val("");
+                    $("#txtDataInicial").val("");
+                    $("#txtDataFinal").val("");
+                    $(".modal-backdrop").remove();
+                    $(document.body).removeClass("modal-open");
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("ViagemUpsert.js", "hide.modalEvento", error);
+                }
+            });
+
+        $("#modalRequisitante")
+            .modal({
+                keyboard: true,
+                backdrop: "static",
+                show: false,
+            })
+            .on("hide.bs.modal", function ()
+            {
+                try
+                {
+                    let setores = document.getElementById("ddtSetorRequisitante").ej2_instances[0];
+                    setores.value = "";
+                    $("#txtPonto").val("");
+                    $("#txtNome").val("");
+                    $("#txtRamal").val("");
+                    $("#txtEmail").val("");
+                    $(".modal-backdrop").remove();
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'hide.modalEvento',
+                        "ViagemUpsert.js",
+                        "hide.modalRequisitante",
                         error,
                     );
                 }
             });
 
-        $('#modalRequisitante')
+        $("#modalSetor")
             .modal({
                 keyboard: true,
-                backdrop: 'static',
+                backdrop: "static",
                 show: false,
             })
-            .on('hide.bs.modal', function () {
-                try {
-                    let setores = document.getElementById(
-                        'ddtSetorRequisitante',
-                    ).ej2_instances[0];
-                    setores.value = '';
-                    $('#txtPonto').val('');
-                    $('#txtNome').val('');
-                    $('#txtRamal').val('');
-                    $('#txtEmail').val('');
-                    $('.modal-backdrop').remove();
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'hide.modalRequisitante',
-                        error,
-                    );
+            .on("hide.bs.modal", function ()
+            {
+                try
+                {
+                    let setores = document.getElementById("ddtSetorPai").ej2_instances[0];
+                    setores.value = "";
+                    $("#txtSigla").val("");
+                    $("#txtNomeSetor").val("");
+                    $("#txtRamalSetor").val("");
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha("ViagemUpsert.js", "hide.modalSetor", error);
                 }
             });
 
-        $('#modalSetor')
-            .modal({
-                keyboard: true,
-                backdrop: 'static',
-                show: false,
-            })
-            .on('hide.bs.modal', function () {
-                try {
-                    let setores =
-                        document.getElementById('ddtSetorPai').ej2_instances[0];
-                    setores.value = '';
-                    $('#txtSigla').val('');
-                    $('#txtNomeSetor').val('');
-                    $('#txtRamalSetor').val('');
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'hide.modalSetor',
-                        error,
-                    );
-                }
-            });
-
-        $('#txtFile').change(function (event) {
-            try {
+        $("#txtFile").change(function (event)
+        {
+            try
+            {
                 let files = event.target.files;
                 if (files.length === 0) return;
                 let file = files[0];
-                if (!file.type.startsWith('image/')) {
+                if (!file.type.startsWith("image/"))
+                {
                     Alerta.Erro(
-                        'Arquivo inválido',
-                        'Por favor, selecione um arquivo de imagem válido.',
+                        "Arquivo inválido",
+                        "Por favor, selecione um arquivo de imagem válido.",
                     );
                     return;
                 }
-                $('#imgViewer').attr('src', window.URL.createObjectURL(file));
-                $('#painelfundo').css({ 'padding-bottom': '200px' });
-            } catch (error) {
-                TratamentoErroComLinha(
-                    'ViagemUpsert.js',
-                    'change.txtFile',
-                    error,
-                );
+                $("#imgViewer").attr("src", window.URL.createObjectURL(file));
+                $("#painelfundo").css({ "padding-bottom": "200px" });
+            }
+            catch (error)
+            {
+                TratamentoErroComLinha("ViagemUpsert.js", "change.txtFile", error);
             }
         });
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'document.ready', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "document.ready", error);
     }
 });
 
 var CarregandoViagemBloqueada = false;
 
-$('#txtKmInicial').focusout(function () {
-    try {
-        const kmInicialStr = $('#txtKmInicial').val();
-        const kmAtualStr = $('#txtKmAtual').val();
-
-        if (!kmInicialStr || !kmAtualStr) {
-            $('#txtKmPercorrido').val('');
-            if (!kmAtualStr || kmAtualStr === '0' || kmAtualStr === 0) {
-                $('#txtKmInicial').val('');
-                $('#txtKmFinal').val('');
-                $('#txtKmPercorrido').val('');
+$("#txtKmInicial").focusout(function ()
+{
+    try
+    {
+        const kmInicialStr = $("#txtKmInicial").val();
+        const kmAtualStr = $("#txtKmAtual").val();
+
+        if (!kmInicialStr || !kmAtualStr)
+        {
+            $("#txtKmPercorrido").val("");
+            if (!kmAtualStr || kmAtualStr === "0" || kmAtualStr === 0)
+            {
+                $("#txtKmInicial").val("");
+                $("#txtKmFinal").val("");
+                $("#txtKmPercorrido").val("");
                 Alerta.Erro(
-                    'Erro na Quilometragem',
+                    "Erro na Quilometragem",
                     'A quilometragem <strong class="destaque-erro">Atual</strong> deve estar preenchida e ser maior que <strong class="destaque-erro">Zero</strong>!',
                 );
             }
             return;
         }
 
-        const kmInicial = parseFloat(kmInicialStr.replace(',', '.'));
-        const kmAtual = parseFloat(kmAtualStr.replace(',', '.'));
-
-        if (isNaN(kmInicial) || isNaN(kmAtual)) {
-            $('#txtKmPercorrido').val('');
-            return;
-        }
-
-        if (kmInicial < 0) {
-            $('#txtKmInicial').val('');
-            $('#txtKmPercorrido').val('');
+        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
+        const kmAtual = parseFloat(kmAtualStr.replace(",", "."));
+
+        if (isNaN(kmInicial) || isNaN(kmAtual))
+        {
+            $("#txtKmPercorrido").val("");
+            return;
+        }
+
+        if (kmInicial < 0)
+        {
+            $("#txtKmInicial").val("");
+            $("#txtKmPercorrido").val("");
             Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!',
+                "Erro na Quilometragem",
+                "A quilometragem <strong>inicial</strong> deve ser maior que <strong>zero</strong>!",
             );
             return;
         }
 
-        if (kmInicial < kmAtual) {
-            $('#txtKmInicial').val('');
-            $('#txtKmPercorrido').val('');
+        if (kmInicial < kmAtual)
+        {
+            $("#txtKmInicial").val("");
+            $("#txtKmPercorrido").val("");
             Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!',
+                "Erro na Quilometragem",
+                "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>!",
             );
             return;
         }
 
         validarKmAtualInicial();
 
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'focusout.txtKmInicial',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtKmInicial", error);
     }
 });
 
-$('#txtKmFinal').focusout(async function () {
-    try {
-        const kmInicialStr = $('#txtKmInicial').val();
-        const kmFinalStr = $('#txtKmFinal').val();
+$("#txtKmFinal").focusout(async function ()
+{
+    try
+    {
+        const kmInicialStr = $("#txtKmInicial").val();
+        const kmFinalStr = $("#txtKmFinal").val();
 
         if (
-            (kmInicialStr === '' || kmInicialStr === null) &&
-            kmFinalStr != '' &&
+            (kmInicialStr === "" || kmInicialStr === null) &&
+            kmFinalStr != "" &&
             kmFinalStr != null
-        ) {
-            $('#txtKmFinal').val('');
-            $('#txtKmPercorrido').val('');
+        )
+        {
+            $("#txtKmFinal").val("");
+            $("#txtKmPercorrido").val("");
             Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem <strong>Final</strong> deve ser preenchida somente após a <strong>Inicial</strong>!',
+                "Erro na Quilometragem",
+                "A quilometragem <strong>Final</strong> deve ser preenchida somente após a <strong>Inicial</strong>!",
             );
             return;
         }
 
-        if (!kmInicialStr || !kmFinalStr) {
-            $('#txtKmPercorrido').val('');
-            return;
-        }
-
-        const kmInicial = parseFloat(kmInicialStr.replace(',', '.'));
-        const kmFinal = parseFloat(kmFinalStr.replace(',', '.'));
-
-        if (isNaN(kmInicial) || isNaN(kmFinal)) {
-            $('#txtKmPercorrido').val('');
-            return;
-        }
-
-        if (kmFinal < kmInicial) {
-            $('#txtKmFinal').val('');
-            $('#txtKmPercorrido').val('');
+        if (!kmInicialStr || !kmFinalStr)
+        {
+            $("#txtKmPercorrido").val("");
+            return;
+        }
+
+        const kmInicial = parseFloat(kmInicialStr.replace(",", "."));
+        const kmFinal = parseFloat(kmFinalStr.replace(",", "."));
+
+        if (isNaN(kmInicial) || isNaN(kmFinal))
+        {
+            $("#txtKmPercorrido").val("");
+            return;
+        }
+
+        if (kmFinal < kmInicial)
+        {
+            $("#txtKmFinal").val("");
+            $("#txtKmPercorrido").val("");
             Alerta.Erro(
-                'Erro na Quilometragem',
-                'A quilometragem final deve ser maior que a inicial!',
+                "Erro na Quilometragem",
+                "A quilometragem final deve ser maior que a inicial!",
             );
             return;
         }
 
         const kmPercorrido = Math.round(kmFinal - kmInicial);
-        $('#txtKmPercorrido').val(kmPercorrido);
+        $("#txtKmPercorrido").val(kmPercorrido);
 
         calcularKmPercorrido();
 
-        if (typeof ValidadorFinalizacaoIA !== 'undefined') {
-            const veiculoId =
-                document.getElementById('cmbVeiculo')?.ej2_instances?.[0]
-                    ?.value || '';
-
-            if (veiculoId && kmInicial > 0 && kmFinal > 0) {
+        if (typeof ValidadorFinalizacaoIA !== 'undefined')
+        {
+            const veiculoId = document.getElementById("cmbVeiculo")?.ej2_instances?.[0]?.value || '';
+
+            if (veiculoId && kmInicial > 0 && kmFinal > 0)
+            {
                 const validador = ValidadorFinalizacaoIA.obterInstancia();
                 const dadosKm = {
                     kmInicial: kmInicial,
                     kmFinal: kmFinal,
-                    veiculoId: veiculoId,
+                    veiculoId: veiculoId
                 };
 
                 const resultadoKm = await validador.analisarKm(dadosKm);
-                if (!resultadoKm.valido) {
-                    if (resultadoKm.nivel === 'erro') {
-                        await Alerta.Erro(
-                            resultadoKm.titulo,
-                            resultadoKm.mensagem,
-                        );
-                        $('#txtKmFinal').val('');
-                        $('#txtKmPercorrido').val('');
+                if (!resultadoKm.valido)
+                {
+                    if (resultadoKm.nivel === 'erro')
+                    {
+                        await Alerta.Erro(resultadoKm.titulo, resultadoKm.mensagem);
+                        $("#txtKmFinal").val("");
+                        $("#txtKmPercorrido").val("");
                         return;
-                    } else if (resultadoKm.nivel === 'aviso') {
+                    }
+                    else if (resultadoKm.nivel === 'aviso')
+                    {
                         const confirma = await Alerta.ValidacaoIAConfirmar(
                             resultadoKm.titulo,
                             resultadoKm.mensagem,
-                            'Manter KM',
-                            'Corrigir',
+                            "Manter KM",
+                            "Corrigir"
                         );
-                        if (!confirma) {
-                            $('#txtKmFinal').val('');
-                            $('#txtKmPercorrido').val('');
+                        if (!confirma)
+                        {
+                            $("#txtKmFinal").val("");
+                            $("#txtKmPercorrido").val("");
                             return;
                         }
                     }
                 }
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'focusout.txtKmFinal', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtKmFinal", error);
     }
 });
 
-$('#txtDataInicial').focusout(function () {
-    try {
-        const rawDataFinal = document.getElementById('txtDataFinal')?.value;
-        const rawDataInicial = document.getElementById('txtDataInicial')?.value;
+$("#txtDataInicial").focusout(function ()
+{
+    try
+    {
+        const rawDataFinal = document.getElementById("txtDataFinal")?.value;
+        const rawDataInicial = document.getElementById("txtDataInicial")?.value;
         const data = new Date(rawDataInicial);
         const anoAtual = new Date().getFullYear();
         const anoInformado = data.getFullYear();
 
         const ehValida =
             !isNaN(data.getTime()) &&
-            rawDataInicial === data.toISOString().split('T')[0] &&
+            rawDataInicial === data.toISOString().split("T")[0] &&
             anoInformado >= anoAtual - 1 &&
             anoInformado <= anoAtual + 1;
 
-        if (!ehValida) {
+        if (!ehValida)
+        {
             Alerta.Erro(
-                'Erro na Data',
-                'A data deve ser válida e o ano deve estar entre o ano anterior e o próximo!',
+                "Erro na Data",
+                "A data deve ser válida e o ano deve estar entre o ano anterior e o próximo!",
             );
-            document.getElementById('txtDataFinal').value = '';
-            document.getElementById('txtDataFinal').focus();
-            return;
-        }
-
-        const dataInicial = rawDataInicial.replace(/-/g, '/');
-        const dataFinal = rawDataFinal.replace(/-/g, '/');
-
-        var inicio = moment(`${dataInicial}`, 'DD/MM/YYYY HH:mm');
-        var fim = moment(`${dataFinal}`, 'DD/MM/YYYY HH:mm');
+            document.getElementById("txtDataFinal").value = "";
+            document.getElementById("txtDataFinal").focus();
+            return;
+        }
+
+        const dataInicial = rawDataInicial.replace(/-/g, "/");
+        const dataFinal = rawDataFinal.replace(/-/g, "/");
+
+        var inicio = moment(`${dataInicial}`, "DD/MM/YYYY HH:mm");
+        var fim = moment(`${dataFinal}`, "DD/MM/YYYY HH:mm");
 
         if (!inicio.isValid() || !fim.isValid()) return;
 
-        if (dataFinal < dataInicial) {
-            $('#txtDataInicial').val('');
-            $('#txtDuracao').val('');
-            Alerta.Erro(
-                'Erro na Data',
-                'A data inicial deve ser menor que a final!',
-            );
+        if (dataFinal < dataInicial)
+        {
+            $("#txtDataInicial").val("");
+            $("#txtDuracao").val("");
+            Alerta.Erro("Erro na Data", "A data inicial deve ser menor que a final!");
             return;
         }
 
         validarDatasInicialFinal(dataInicial, dataFinal);
 
-        if (dataFinal === dataInicial) {
-            const horaInicial = $('#txtHoraInicial').val();
-            const horaFinal = $('#txtHoraFinal').val();
+        if (dataFinal === dataInicial)
+        {
+            const horaInicial = $("#txtHoraInicial").val();
+            const horaFinal = $("#txtHoraFinal").val();
 
             if (!horaInicial || !horaFinal) return;
 
-            const [hI, mI] = horaInicial.split(':').map(Number);
-            const [hF, mF] = horaFinal.split(':').map(Number);
+            const [hI, mI] = horaInicial.split(":").map(Number);
+            const [hF, mF] = horaFinal.split(":").map(Number);
             const minIni = hI * 60 + mI;
             const minFin = hF * 60 + mF;
 
-            if (minFin <= minIni) {
-                $('#txtHoraFinal').val('');
-                $('#txtDuracao').val('');
+            if (minFin <= minIni)
+            {
+                $("#txtHoraFinal").val("");
+                $("#txtDuracao").val("");
                 Alerta.Erro(
-                    'Erro na Hora',
-                    'A hora inicial deve ser menor que a final quando as datas forem iguais!',
+                    "Erro na Hora",
+                    "A hora inicial deve ser menor que a final quando as datas forem iguais!",
                 );
                 return;
             }
         }
 
         calcularDuracaoViagem();
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'focusout.txtDataFinal',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtDataFinal", error);
     }
 });
 
 let evitandoLoop = false;
 
-$('#txtDataFinal').focusout(async function () {
-    try {
+$("#txtDataFinal").focusout(async function ()
+{
+    try
+    {
         if (evitandoLoop) return;
 
-        try {
-            const rawDataFinal = document.getElementById('txtDataFinal')?.value;
-            const rawDataInicial =
-                document.getElementById('txtDataInicial')?.value;
+        try
+        {
+            const rawDataFinal = document.getElementById("txtDataFinal")?.value;
+            const rawDataInicial = document.getElementById("txtDataInicial")?.value;
             const data = new Date(rawDataFinal);
             const anoAtual = new Date().getFullYear();
             const anoInformado = data.getFullYear();
 
-            if (rawDataFinal === '' || rawDataFinal === null) {
+            if (rawDataFinal === "" || rawDataFinal === null)
+            {
                 return;
             }
 
             const ehValida =
                 !isNaN(data.getTime()) &&
-                rawDataFinal === data.toISOString().split('T')[0] &&
+                rawDataFinal === data.toISOString().split("T")[0] &&
                 anoInformado >= anoAtual - 1 &&
                 anoInformado <= anoAtual + 1;
 
-            if (!ehValida) {
+            if (!ehValida)
+            {
                 evitandoLoop = true;
 
                 Alerta.Erro(
-                    'Erro na Data',
-                    'A data deve ser válida e o ano deve estar entre o ano anterior e o próximo!',
+                    "Erro na Data",
+                    "A data deve ser válida e o ano deve estar entre o ano anterior e o próximo!",
                 );
 
-                setTimeout(() => {
-                    try {
-                        document.getElementById('txtDataFinal').value = '';
-                        document.getElementById('txtDataFinal').focus();
+                setTimeout(() =>
+                {
+                    try
+                    {
+                        document.getElementById("txtDataFinal").value = "";
+                        document.getElementById("txtDataFinal").focus();
                         evitandoLoop = false;
-                    } catch (error) {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'ViagemUpsert.js',
-                            'callback@setTimeout#0',
+                            "ViagemUpsert.js",
+                            "callback@setTimeout#0",
                             error,
                         );
                     }
@@ -649,161 +673,158 @@
                 return;
             }
 
-            if (typeof ValidadorFinalizacaoIA !== 'undefined') {
+            if (typeof ValidadorFinalizacaoIA !== 'undefined')
+            {
                 const validador = ValidadorFinalizacaoIA.obterInstancia();
-                const resultadoDataFutura =
-                    await validador.validarDataNaoFutura(rawDataFinal);
-                if (!resultadoDataFutura.valido) {
-                    await Alerta.Erro(
-                        resultadoDataFutura.titulo,
-                        resultadoDataFutura.mensagem,
-                    );
-                    document.getElementById('txtDataFinal').value = '';
+                const resultadoDataFutura = await validador.validarDataNaoFutura(rawDataFinal);
+                if (!resultadoDataFutura.valido)
+                {
+                    await Alerta.Erro(resultadoDataFutura.titulo, resultadoDataFutura.mensagem);
+                    document.getElementById("txtDataFinal").value = "";
                     return;
                 }
-            } else {
+            }
+            else
+            {
 
                 const hoje = new Date();
                 hoje.setHours(0, 0, 0, 0);
-                if (data > hoje) {
-                    document.getElementById('txtDataFinal').value = '';
-                    document.getElementById('txtDataFinal').focus();
-                    AppToast.show(
-                        'Amarelo',
-                        'A Data Final não pode ser superior à data atual.',
-                        4000,
+                if (data > hoje)
+                {
+                    document.getElementById("txtDataFinal").value = "";
+                    document.getElementById("txtDataFinal").focus();
+                    AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
+                    return;
+                }
+            }
+
+            const dataInicial = rawDataInicial.replace(/-/g, "/");
+            const dataFinal = rawDataFinal.replace(/-/g, "/");
+
+            var inicio = moment(`${dataInicial}`, "DD/MM/YYYY HH:mm");
+            var fim = moment(`${dataFinal}`, "DD/MM/YYYY HH:mm");
+
+            if (!inicio.isValid() || !fim.isValid()) return;
+
+            if (dataFinal < dataInicial)
+            {
+                $("#txtDataInicial").val("");
+                $("#txtDuracao").val("");
+                Alerta.Erro("Erro na Data", "A data final deve ser maior ou igual que a inicial!");
+                return;
+            }
+
+            validarDatasInicialFinal(dataInicial, dataFinal);
+
+            if (dataFinal === dataInicial)
+            {
+                const horaInicial = $("#txtHoraInicial").val();
+                const horaFinal = $("#txtHoraFinal").val();
+
+                if (!horaInicial || !horaFinal) return;
+
+                const [hI, mI] = horaInicial.split(":").map(Number);
+                const [hF, mF] = horaFinal.split(":").map(Number);
+                const minIni = hI * 60 + mI;
+                const minFin = hF * 60 + mF;
+
+                if (minFin <= minIni)
+                {
+                    $("#txtHoraFinal").val("");
+                    $("#txtDuracao").val("");
+                    Alerta.Erro(
+                        "Erro na Hora",
+                        "A hora final deve ser maior ou igual que a inicial quando as datas forem iguais!",
                     );
                     return;
                 }
             }
 
-            const dataInicial = rawDataInicial.replace(/-/g, '/');
-            const dataFinal = rawDataFinal.replace(/-/g, '/');
-
-            var inicio = moment(`${dataInicial}`, 'DD/MM/YYYY HH:mm');
-            var fim = moment(`${dataFinal}`, 'DD/MM/YYYY HH:mm');
-
-            if (!inicio.isValid() || !fim.isValid()) return;
-
-            if (dataFinal < dataInicial) {
-                $('#txtDataInicial').val('');
-                $('#txtDuracao').val('');
-                Alerta.Erro(
-                    'Erro na Data',
-                    'A data final deve ser maior ou igual que a inicial!',
-                );
-                return;
-            }
-
-            validarDatasInicialFinal(dataInicial, dataFinal);
-
-            if (dataFinal === dataInicial) {
-                const horaInicial = $('#txtHoraInicial').val();
-                const horaFinal = $('#txtHoraFinal').val();
-
-                if (!horaInicial || !horaFinal) return;
-
-                const [hI, mI] = horaInicial.split(':').map(Number);
-                const [hF, mF] = horaFinal.split(':').map(Number);
-                const minIni = hI * 60 + mI;
-                const minFin = hF * 60 + mF;
-
-                if (minFin <= minIni) {
-                    $('#txtHoraFinal').val('');
-                    $('#txtDuracao').val('');
-                    Alerta.Erro(
-                        'Erro na Hora',
-                        'A hora final deve ser maior ou igual que a inicial quando as datas forem iguais!',
-                    );
-                    return;
-                }
-            }
-
             calcularDuracaoViagem();
 
-            if (typeof ValidadorFinalizacaoIA !== 'undefined') {
-                const horaInicial = $('#txtHoraInicial').val();
-                const horaFinal = $('#txtHoraFinal').val();
-
-                if (rawDataInicial && horaInicial && horaFinal) {
+            if (typeof ValidadorFinalizacaoIA !== 'undefined')
+            {
+                const horaInicial = $("#txtHoraInicial").val();
+                const horaFinal = $("#txtHoraFinal").val();
+
+                if (rawDataInicial && horaInicial && horaFinal)
+                {
                     const validador = ValidadorFinalizacaoIA.obterInstancia();
                     const dadosDatas = {
                         dataInicial: rawDataInicial,
                         horaInicial: horaInicial,
                         dataFinal: rawDataFinal,
-                        horaFinal: horaFinal,
+                        horaFinal: horaFinal
                     };
 
-                    const resultadoDatas =
-                        await validador.analisarDatasHoras(dadosDatas);
-                    if (
-                        !resultadoDatas.valido &&
-                        resultadoDatas.nivel === 'aviso'
-                    ) {
+                    const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
+                    if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
+                    {
                         const confirma = await Alerta.ValidacaoIAConfirmar(
                             resultadoDatas.titulo,
                             resultadoDatas.mensagem,
-                            'Manter Data',
-                            'Corrigir',
+                            "Manter Data",
+                            "Corrigir"
                         );
-                        if (!confirma) {
-                            document.getElementById('txtDataFinal').value = '';
+                        if (!confirma)
+                        {
+                            document.getElementById("txtDataFinal").value = "";
                             return;
                         }
                     }
                 }
             }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'focusout.txtDataFinal',
-                error,
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtDataFinal", error);
+        }
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "callback@$.focusout#0", error);
+    }
+});
+
+$("#txtHoraFinal").focusout(async function ()
+{
+    try
+    {
+        if ($("#txtDataFinal").val() === "" && $("#txtHoraFinal").val() != "")
+        {
+            Alerta.Erro(
+                "Erro na Hora",
+                "A hora final só pode ser preenchida depois de Data Final!",
             );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'callback@$.focusout#0',
-            error,
-        );
-    }
-});
-
-$('#txtHoraFinal').focusout(async function () {
-    try {
-        if ($('#txtDataFinal').val() === '' && $('#txtHoraFinal').val() != '') {
-            Alerta.Erro(
-                'Erro na Hora',
-                'A hora final só pode ser preenchida depois de Data Final!',
-            );
-            $('#txtHoraFinal').val('');
-            $('#txtDuracao').val('');
-            return;
-        }
-
-        const dataInicialStr = $('#txtDataInicial').val();
-        const dataFinalStr = $('#txtDataFinal').val();
-        const horaInicial = $('#txtHoraInicial').val();
-        const horaFinal = $('#txtHoraFinal').val();
-
-        if (!dataInicialStr || !dataFinalStr || !horaInicial || !horaFinal)
-            return;
-
-        const [dia, mes, ano] = dataInicialStr.split('/');
+            $("#txtHoraFinal").val("");
+            $("#txtDuracao").val("");
+            return;
+        }
+
+        const dataInicialStr = $("#txtDataInicial").val();
+        const dataFinalStr = $("#txtDataFinal").val();
+        const horaInicial = $("#txtHoraInicial").val();
+        const horaFinal = $("#txtHoraFinal").val();
+
+        if (!dataInicialStr || !dataFinalStr || !horaInicial || !horaFinal) return;
+
+        const [dia, mes, ano] = dataInicialStr.split("/");
         const dataInicial = `${ano}-${mes}-${dia}`;
 
-        if (dataInicial === dataFinalStr) {
-            const [hI, mI] = horaInicial.split(':').map(Number);
-            const [hF, mF] = horaFinal.split(':').map(Number);
+        if (dataInicial === dataFinalStr)
+        {
+            const [hI, mI] = horaInicial.split(":").map(Number);
+            const [hF, mF] = horaFinal.split(":").map(Number);
             const minIni = hI * 60 + mI;
             const minFin = hF * 60 + mF;
 
-            if (minFin <= minIni) {
-                $('#txtHoraFinal').val('');
-                $('#txtDuracao').val('');
+            if (minFin <= minIni)
+            {
+                $("#txtHoraFinal").val("");
+                $("#txtDuracao").val("");
                 Alerta.Erro(
-                    'Erro na Hora',
-                    'A hora final deve ser maior que a inicial quando as datas forem iguais!',
+                    "Erro na Hora",
+                    "A hora final deve ser maior que a inicial quando as datas forem iguais!",
                 );
                 return;
             }
@@ -811,248 +832,268 @@
 
         calcularDuracaoViagem();
 
-        if (typeof ValidadorFinalizacaoIA !== 'undefined') {
+        if (typeof ValidadorFinalizacaoIA !== 'undefined')
+        {
             const validador = ValidadorFinalizacaoIA.obterInstancia();
             const dadosDatas = {
                 dataInicial: dataInicialStr,
                 horaInicial: horaInicial,
                 dataFinal: dataFinalStr,
-                horaFinal: horaFinal,
+                horaFinal: horaFinal
             };
 
-            const resultadoDatas =
-                await validador.analisarDatasHoras(dadosDatas);
-            if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso') {
+            const resultadoDatas = await validador.analisarDatasHoras(dadosDatas);
+            if (!resultadoDatas.valido && resultadoDatas.nivel === 'aviso')
+            {
                 const confirma = await Alerta.ValidacaoIAConfirmar(
                     resultadoDatas.titulo,
                     resultadoDatas.mensagem,
-                    'Manter Hora',
-                    'Corrigir',
+                    "Manter Hora",
+                    "Corrigir"
                 );
-                if (!confirma) {
-                    $('#txtHoraFinal').val('');
-                    $('#txtDuracao').val('');
+                if (!confirma)
+                {
+                    $("#txtHoraFinal").val("");
+                    $("#txtDuracao").val("");
                     return;
                 }
             }
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'focusout.txtHoraFinal',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtHoraFinal", error);
     }
 });
 
-function PreencheListaEventos() {
-    try {
-        const eventos = document.getElementById('ddtEventos');
-        if (
-            eventos &&
-            eventos.ej2_instances &&
-            eventos.ej2_instances.length > 0
-        ) {
+function PreencheListaEventos()
+{
+    try
+    {
+        const eventos = document.getElementById("ddtEventos");
+        if (eventos && eventos.ej2_instances && eventos.ej2_instances.length > 0)
+        {
             eventos.ej2_instances[0].dataSource = [];
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'PreencheListaEventos',
-            error,
-        );
-    }
-}
-
-function PreencheListaRequisitantes() {
-    try {
-        const requisitantes = document.getElementById('cmbRequisitante');
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaEventos", error);
+    }
+}
+
+function PreencheListaRequisitantes()
+{
+    try
+    {
+        const requisitantes = document.getElementById("cmbRequisitante");
         if (
             requisitantes &&
             requisitantes.ej2_instances &&
             requisitantes.ej2_instances.length > 0
-        ) {
+        )
+        {
             requisitantes.ej2_instances[0].dataSource = [];
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'PreencheListaRequisitantes',
-            error,
-        );
-    }
-}
-
-function PreencheListaSetores(SetorSolicitanteId) {
-    try {
-        const setor = document.getElementById('cmbSetor');
-        if (setor && setor.ej2_instances && setor.ej2_instances.length > 0) {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaRequisitantes", error);
+    }
+}
+
+function PreencheListaSetores(SetorSolicitanteId)
+{
+    try
+    {
+        const setor = document.getElementById("cmbSetor");
+        if (setor && setor.ej2_instances && setor.ej2_instances.length > 0)
+        {
             setor.ej2_instances[0].dataSource = [];
             setor.ej2_instances[0].enabled = true;
         }
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'PreencheListaSetores',
-            error,
-        );
-    }
-}
-
-function upload(args) {
-    try {
-        console.log('Arquivo enviado:', args);
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'upload', error);
-    }
-}
-
-function toolbarClick(e) {
-    try {
-        console.log('Toolbar click:', e);
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'toolbarClick', error);
-    }
-}
-
-async function validarKmAtualInicial() {
-    try {
-        if (CarregandoViagemBloqueada) {
-            return;
-        }
-
-        const kmInicial = $('#txtKmInicial').val();
-        const kmAtual = $('#txtKmAtual').val();
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaSetores", error);
+    }
+}
+
+function upload(args)
+{
+    try
+    {
+        console.log("Arquivo enviado:", args);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "upload", error);
+    }
+}
+
+function toolbarClick(e)
+{
+    try
+    {
+        console.log("Toolbar click:", e);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "toolbarClick", error);
+    }
+}
+
+async function validarKmAtualInicial()
+{
+    try
+    {
+        if (CarregandoViagemBloqueada)
+        {
+            return;
+        }
+
+        const kmInicial = $("#txtKmInicial").val();
+        const kmAtual = $("#txtKmAtual").val();
 
         if (!kmInicial || !kmAtual) return true;
 
-        const ini = parseFloat(kmAtual.replace(',', '.'));
-        const fim = parseFloat(kmInicial.replace(',', '.'));
-
-        if (fim < ini) {
+        const ini = parseFloat(kmAtual.replace(",", "."));
+        const fim = parseFloat(kmInicial.replace(",", "."));
+
+        if (fim < ini)
+        {
             Alerta.Erro(
-                'Erro',
-                'A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>.',
+                "Erro",
+                "A quilometragem <strong>inicial</strong> deve ser maior que a <strong>atual</strong>.",
             );
             return false;
         }
 
         const diff = fim - ini;
-        if (diff > 100) {
+        if (diff > 100)
+        {
             const confirmado = await Alerta.Confirmar(
-                'Quilometragem Alta',
-                'A quilometragem <strong>inicial</strong> excede em 100km a <strong>atual</strong>. Tem certeza?',
-                'Tenho certeza! 💪🏼',
+                "Quilometragem Alta",
+                "A quilometragem <strong>inicial</strong> excede em 100km a <strong>atual</strong>. Tem certeza?",
+                "Tenho certeza! 💪🏼",
                 "Me enganei! 😟'",
             );
 
-            if (!confirmado) {
-                const txtKmInicialElement =
-                    document.getElementById('txtKmInicial');
+            if (!confirmado)
+            {
+                const txtKmInicialElement = document.getElementById("txtKmInicial");
                 txtKmInicialElement.value = null;
                 txtKmInicialElement.focus();
                 return false;
-            } else {
+            } else
+            {
                 calcularKmPercorrido();
             }
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'validarKmAtualInicial',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "validarKmAtualInicial", error);
         return false;
     }
 }
 
-async function validarKmInicialFinal() {
-    try {
-        if ($('#btnSubmit').is(':hidden')) {
-            return;
-        }
-
-        const kmInicial = $('#txtKmInicial').val();
-        const kmFinal = $('#txtKmFinal').val();
+async function validarKmInicialFinal()
+{
+    try
+    {
+        if ($("#btnSubmit").is(":hidden"))
+        {
+            return;
+        }
+
+        const kmInicial = $("#txtKmInicial").val();
+        const kmFinal = $("#txtKmFinal").val();
 
         if (!kmInicial || !kmFinal) return true;
 
-        const ini = parseFloat(kmInicial.replace(',', '.'));
-        const fim = parseFloat(kmFinal.replace(',', '.'));
-
-        if (fim < ini) {
-            Alerta.Erro(
-                'Erro',
-                'A quilometragem final deve ser maior que a inicial.',
-            );
+        const ini = parseFloat(kmInicial.replace(",", "."));
+        const fim = parseFloat(kmFinal.replace(",", "."));
+
+        if (fim < ini)
+        {
+            Alerta.Erro("Erro", "A quilometragem final deve ser maior que a inicial.");
             return false;
         }
 
         const diff = fim - ini;
-        if (diff > 100) {
+        if (diff > 100)
+        {
             const confirmado = await Alerta.Confirmar(
-                'Quilometragem Alta',
-                'A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?',
-                'Tenho certeza! 💪🏼',
+                "Quilometragem Alta",
+                "A quilometragem <strong>final</strong> excede em 100km a <strong>inicial</strong>. Tem certeza?",
+                "Tenho certeza! 💪🏼",
                 "Me enganei! 😟'",
             );
 
-            if (!confirmado) {
-                const txtKmFinalElement = document.getElementById('txtKmFinal');
+            if (!confirmado)
+            {
+                const txtKmFinalElement = document.getElementById("txtKmFinal");
                 txtKmFinalElement.value = null;
                 txtKmFinalElement.focus();
                 return false;
-            } else {
+            } else
+            {
                 calcularKmPercorrido();
             }
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'validarKmInicialFinal',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "validarKmInicialFinal", error);
         return false;
     }
 }
 
-async function validarDatasInicialFinal(DataInicial, DataFinal) {
-    try {
-        if (CarregandoViagemBloqueada) {
-            return;
-        }
-
-        function parseData(data) {
-            try {
+async function validarDatasInicialFinal(DataInicial, DataFinal)
+{
+    try
+    {
+        if (CarregandoViagemBloqueada)
+        {
+            return;
+        }
+
+        function parseData(data)
+        {
+            try
+            {
                 if (!data) return null;
                 if (data instanceof Date) return new Date(data.getTime());
 
-                if (typeof data === 'string') {
-                    if (data.match(/^\d{4}\/\d{2}\/\d{2}$/)) {
-                        const [ano, mes, dia] = data.split('/');
+                if (typeof data === "string")
+                {
+                    if (data.match(/^\d{4}\/\d{2}\/\d{2}$/))
+                    {
+                        const [ano, mes, dia] = data.split("/");
                         return new Date(ano, mes - 1, dia);
                     }
-                    if (data.match(/^\d{4}-\d{2}-\d{2}$/)) {
-                        const [ano, mes, dia] = data.split('-');
+                    if (data.match(/^\d{4}-\d{2}-\d{2}$/))
+                    {
+                        const [ano, mes, dia] = data.split("-");
                         return new Date(ano, mes - 1, dia);
                     }
-                    if (data.match(/^\d{2}\/\d{2}\/\d{4}$/)) {
-                        const [dia, mes, ano] = data.split('/');
+                    if (data.match(/^\d{2}\/\d{2}\/\d{4}$/))
+                    {
+                        const [dia, mes, ano] = data.split("/");
                         return new Date(ano, mes - 1, dia);
                     }
                 }
 
                 return null;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'ViagemUpsert.js',
-                    'parseData',
-                    error,
-                );
+            }
+            catch (error)
+            {
+                Alerta.TratamentoErroComLinha("ViagemUpsert.js", "parseData", error);
             }
         }
 
@@ -1063,29 +1104,23 @@
 
         const diff = (dtFim - dtIni) / (1000 * 60 * 60 * 24);
 
-        if (diff >= 5) {
-            const mensagem =
-                'A Data Final está 5 dias ou mais após a Data Inicial. Tem certeza?';
-            const confirmado =
-                await window.SweetAlertInterop.ShowPreventionAlert(mensagem);
-
-            if (confirmado) {
-                showSyncfusionToast(
-                    'Confirmação feita pelo usuário!',
-                    'success',
-                    '💪🏼',
-                );
-                document.getElementById('txtHoraFinal').focus();
-            } else {
-                showSyncfusionToast(
-                    'Ação cancelada pelo usuário',
-                    'danger',
-                    '😟',
-                );
-
-                const campo = document.getElementById('txtDataFinal');
-                if (campo) {
-                    campo.value = '';
+        if (diff >= 5)
+        {
+            const mensagem = "A Data Final está 5 dias ou mais após a Data Inicial. Tem certeza?";
+            const confirmado = await window.SweetAlertInterop.ShowPreventionAlert(mensagem);
+
+            if (confirmado)
+            {
+                showSyncfusionToast("Confirmação feita pelo usuário!", "success", "💪🏼");
+                document.getElementById("txtHoraFinal").focus();
+            } else
+            {
+                showSyncfusionToast("Ação cancelada pelo usuário", "danger", "😟");
+
+                const campo = document.getElementById("txtDataFinal");
+                if (campo)
+                {
+                    campo.value = "";
                     campo.focus();
                     return false;
                 }
@@ -1093,1627 +1128,1600 @@
         }
 
         return true;
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'validarDatasInicialFinal',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "validarDatasInicialFinal", error);
         return false;
     }
 }
 
-function calcularDuracaoViagem() {
-    try {
-        var rawDataInicial =
-            document.getElementById('txtDataInicial') &&
-            document.getElementById('txtDataInicial').value;
-        var horaInicial =
-            document.getElementById('txtHoraInicial') &&
-            document.getElementById('txtHoraInicial').value;
-        var rawDataFinal =
-            document.getElementById('txtDataFinal') &&
-            document.getElementById('txtDataFinal').value;
-        var horaFinal =
-            document.getElementById('txtHoraFinal') &&
-            document.getElementById('txtHoraFinal').value;
-        var elDuracao = document.getElementById('txtDuracao');
-
-        console.log(
-            'calcularDuracaoViagem - DataInicial:',
-            rawDataInicial,
-            'HoraInicial:',
-            horaInicial,
-            'DataFinal:',
-            rawDataFinal,
-            'HoraFinal:',
-            horaFinal,
-        );
+function calcularDuracaoViagem()
+{
+    try
+    {
+        var rawDataInicial = document.getElementById("txtDataInicial") && document.getElementById("txtDataInicial").value;
+        var horaInicial = document.getElementById("txtHoraInicial") && document.getElementById("txtHoraInicial").value;
+        var rawDataFinal = document.getElementById("txtDataFinal") && document.getElementById("txtDataFinal").value;
+        var horaFinal = document.getElementById("txtHoraFinal") && document.getElementById("txtHoraFinal").value;
+        var elDuracao = document.getElementById("txtDuracao");
+
+        console.log("calcularDuracaoViagem - DataInicial:", rawDataInicial, "HoraInicial:", horaInicial, "DataFinal:", rawDataFinal, "HoraFinal:", horaFinal);
 
         if (!elDuracao) return;
 
         var LIMIAR_MINUTOS = 120;
 
-        if (!rawDataInicial || !horaInicial || !rawDataFinal || !horaFinal) {
-            elDuracao.value = '';
+        if (!rawDataInicial || !horaInicial || !rawDataFinal || !horaFinal)
+        {
+            elDuracao.value = "";
             if (typeof FieldUX !== 'undefined') {
                 FieldUX.setInvalid(elDuracao, false);
-                FieldUX.tooltipOnTransition(
-                    elDuracao,
-                    false,
-                    1000,
-                    'tooltipDuracao',
-                );
-            }
-            return;
-        }
-
-        var inicio = moment(
-            rawDataInicial + 'T' + horaInicial,
-            'YYYY-MM-DDTHH:mm',
-        );
-        var fim = moment(rawDataFinal + 'T' + horaFinal, 'YYYY-MM-DDTHH:mm');
-        if (!inicio.isValid() || !fim.isValid()) {
-            elDuracao.value = '';
+                FieldUX.tooltipOnTransition(elDuracao, false, 1000, 'tooltipDuracao');
+            }
+            return;
+        }
+
+        var inicio = moment(rawDataInicial + "T" + horaInicial, "YYYY-MM-DDTHH:mm");
+        var fim = moment(rawDataFinal + "T" + horaFinal, "YYYY-MM-DDTHH:mm");
+        if (!inicio.isValid() || !fim.isValid())
+        {
+            elDuracao.value = "";
             if (typeof FieldUX !== 'undefined') {
                 FieldUX.setInvalid(elDuracao, false);
-                FieldUX.tooltipOnTransition(
-                    elDuracao,
-                    false,
-                    1000,
-                    'tooltipDuracao',
-                );
-            }
-            return;
-        }
-
-        var duracaoMinutos = fim.diff(inicio, 'minutes');
+                FieldUX.tooltipOnTransition(elDuracao, false, 1000, 'tooltipDuracao');
+            }
+            return;
+        }
+
+        var duracaoMinutos = fim.diff(inicio, "minutes");
         var dias = Math.floor(duracaoMinutos / 1440);
         var horas = Math.floor((duracaoMinutos % 1440) / 60);
-        var textoDuracao =
-            dias +
-            ' dia' +
-            (dias !== 1 ? 's' : '') +
-            ' e ' +
-            horas +
-            ' hora' +
-            (horas !== 1 ? 's' : '');
+        var textoDuracao = dias + " dia" + (dias !== 1 ? "s" : "") +
+            " e " + horas + " hora" + (horas !== 1 ? "s" : "");
         elDuracao.value = textoDuracao;
 
-        var invalid = duracaoMinutos > LIMIAR_MINUTOS;
+        var invalid = (duracaoMinutos > LIMIAR_MINUTOS);
         if (typeof FieldUX !== 'undefined') {
             FieldUX.setInvalid(elDuracao, invalid);
 
-            FieldUX.tooltipOnTransition(
-                elDuracao,
-                duracaoMinutos > LIMIAR_MINUTOS,
-                1000,
-                'tooltipDuracao',
-            );
-        }
-    } catch (error) {
-        if (typeof TratamentoErroComLinha === 'function') {
-            TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'calcularDuracaoViagem',
-                error,
-            );
-        } else {
+            FieldUX.tooltipOnTransition(elDuracao, duracaoMinutos > LIMIAR_MINUTOS, 1000, 'tooltipDuracao');
+        }
+    } catch (error)
+    {
+        if (typeof TratamentoErroComLinha === 'function')
+        {
+            TratamentoErroComLinha("ViagemUpsert.js", "calcularDuracaoViagem", error);
+        } else
+        {
             console.error(error);
         }
     }
 }
 
-$(document).ready(function () {
-    try {
-        $('.esconde-diveventos').hide();
-
-        if (
-            ViagemId !== '00000000-0000-0000-0000-000000000000' &&
-            ViagemId != null
-        ) {
+$(document).ready(function ()
+{
+    try
+    {
+        $(".esconde-diveventos").hide();
+
+        if (ViagemId !== "00000000-0000-0000-0000-000000000000" && ViagemId != null)
+        {
             $.ajax({
-                type: 'GET',
-                url: '/api/Viagem/PegaFicha?id=' + ViagemId,
-                success: function (data) {
-                    try {
-                        if (
-                            data.fichaVistoria !== null &&
-                            data.fichaVistoria !== undefined
-                        ) {
-                            $('#imgViewer').attr(
-                                'src',
-                                'data:image/jpg;base64,' + data.fichaVistoria,
+                type: "GET",
+                url: "/api/Viagem/PegaFicha?id=" + ViagemId,
+                success: function (data)
+                {
+                    try
+                    {
+                        if (data.fichaVistoria !== null && data.fichaVistoria !== undefined)
+                        {
+                            $("#imgViewer").attr(
+                                "src",
+                                "data:image/jpg;base64," + data.fichaVistoria,
                             );
-                        } else {
-                            $('#imgViewer').attr(
-                                'src',
-                                '/Images/FichaAmarelaNova.jpg',
-                            );
+                        } else
+                        {
+                            $("#imgViewer").attr("src", "/Images/FichaAmarelaNova.jpg");
                         }
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            __scriptName,
-                            'ajax.PegaFicha.success',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha(__scriptName, "ajax.PegaFicha.success", error);
                     }
                 },
-                error: function (data) {
-                    try {
-                        console.log('Error:', data);
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            __scriptName,
-                            'ajax.PegaFicha.error',
-                            error,
-                        );
+                error: function (data)
+                {
+                    try
+                    {
+                        console.log("Error:", data);
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha(__scriptName, "ajax.PegaFicha.error", error);
                     }
                 },
             });
-        } else {
+        } else
+        {
             const origin = window.location.origin;
-            $('#imgViewer').attr('src', '/Images/FichaAmarelaNova.jpg');
+            $("#imgViewer").attr("src", "/Images/FichaAmarelaNova.jpg");
 
             let list = new DataTransfer();
-            let file = new File(
-                ['content'],
-                origin + '/Images/FichaAmarelaNova.jpg',
-            );
+            let file = new File(["content"], origin + "/Images/FichaAmarelaNova.jpg");
             list.items.add(file);
         }
 
-        const viagemId = document.getElementById('txtViagemId').value;
-        if (viagemId && viagemId !== '00000000-0000-0000-0000-000000000000') {
+        const viagemId = document.getElementById("txtViagemId").value;
+        if (viagemId && viagemId !== "00000000-0000-0000-0000-000000000000")
+        {
             $.ajax({
-                type: 'GET',
-                url: '/api/Agenda/RecuperaViagem',
+                type: "GET",
+                url: "/api/Agenda/RecuperaViagem",
                 data: { id: viagemId },
-                contentType: 'application/json',
-                dataType: 'json',
-                success: function (response) {
-                    try {
+                contentType: "application/json",
+                dataType: "json",
+                success: function (response)
+                {
+                    try
+                    {
                         ExibeViagem(response.data);
-                    } catch (error) {
-                        TratamentoErroComLinha(
-                            __scriptName,
-                            'ajax.RecuperaViagem.success',
-                            error,
-                        );
+                    }
+                    catch (error)
+                    {
+                        TratamentoErroComLinha(__scriptName, "ajax.RecuperaViagem.success", error);
                     }
                 },
             });
-        } else {
+        } else
+        {
             const agora = new Date();
-            const dataAtual = moment().format('YYYY-MM-DD');
-            const horaAtual = agora
-                .toTimeString()
-                .split(':')
-                .slice(0, 2)
-                .join(':');
-
-            $('#txtDataInicial').val(dataAtual);
-            $('#txtHoraInicial').val(horaAtual);
-        }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'document.ready', error);
+            const dataAtual = moment().format("YYYY-MM-DD");
+            const horaAtual = agora.toTimeString().split(":").slice(0, 2).join(":");
+
+            $("#txtDataInicial").val(dataAtual);
+            $("#txtHoraInicial").val(horaAtual);
+        }
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "document.ready", error);
     }
 });
 
-function ExibeViagem(viagem) {
-    try {
-        console.log('ExibeViagem - status:', viagem.status, 'viagem:', viagem);
-
-        $('#btnSubmit').hide();
-
-        document.getElementById('ddtFinalidade').ej2_instances[0].value =
-            viagem.finalidade;
-        document.getElementById('ddtFinalidade').ej2_instances[0].text =
-            viagem.finalidade;
-
-        if (viagem.eventoId != null) {
-            const ddtEventos =
-                document.getElementById('ddtEventos').ej2_instances[0];
+function ExibeViagem(viagem)
+{
+    try
+    {
+        console.log("ExibeViagem - status:", viagem.status, "viagem:", viagem);
+
+        $("#btnSubmit").hide();
+
+        document.getElementById("ddtFinalidade").ej2_instances[0].value = viagem.finalidade;
+        document.getElementById("ddtFinalidade").ej2_instances[0].text = viagem.finalidade;
+
+        if (viagem.eventoId != null)
+        {
+            const ddtEventos = document.getElementById("ddtEventos").ej2_instances[0];
             ddtEventos.enabled = true;
             ddtEventos.value = [viagem.eventoId];
-            document.getElementById('btnEvento').style.display = 'block';
-            $('.esconde-diveventos').show();
-        } else {
-            const ddtEventos =
-                document.getElementById('ddtEventos').ej2_instances[0];
+            document.getElementById("btnEvento").style.display = "block";
+            $(".esconde-diveventos").show();
+        } else
+        {
+            const ddtEventos = document.getElementById("ddtEventos").ej2_instances[0];
             ddtEventos.enabled = false;
-            document.getElementById('btnEvento').style.display = 'none';
-            $('.esconde-diveventos').hide();
+            document.getElementById("btnEvento").style.display = "none";
+            $(".esconde-diveventos").hide();
         }
 
         if (viagem.setorSolicitanteId)
-            document.getElementById('ddtSetor').ej2_instances[0].value = [
+            document.getElementById("ddtSetor").ej2_instances[0].value = [
                 viagem.setorSolicitanteId,
             ];
 
         if (viagem.combustivelInicial)
-            document.getElementById(
-                'ddtCombustivelInicial',
-            ).ej2_instances[0].value = [viagem.combustivelInicial];
+            document.getElementById("ddtCombustivelInicial").ej2_instances[0].value = [
+                viagem.combustivelInicial,
+            ];
 
         if (viagem.combustivelFinal)
-            document.getElementById(
-                'ddtCombustivelFinal',
-            ).ej2_instances[0].value = [viagem.combustivelFinal];
-
-        $('#txtKmInicial').val(viagem.kmInicial);
-
-        if (viagem.status === 'Realizada' || viagem.status === 'Cancelada') {
+            document.getElementById("ddtCombustivelFinal").ej2_instances[0].value = [
+                viagem.combustivelFinal,
+            ];
+
+        $("#txtKmInicial").val(viagem.kmInicial);
+
+        if (viagem.status === "Realizada" || viagem.status === "Cancelada")
+        {
             CarregandoViagemBloqueada = true;
 
-            $('#divPainel :input').each(function () {
-                try {
-                    $(this).prop('disabled', true);
-                } catch (error) {
+            $("#divPainel :input").each(function ()
+            {
+                try
+                {
+                    $(this).prop("disabled", true);
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'callback@$.each#0',
+                        "ViagemUpsert.js",
+                        "callback@$.each#0",
                         error,
                     );
                 }
             });
 
-            setTimeout(function () {
+            setTimeout(function() {
                 try {
-                    const rteElement = document.getElementById('rte');
-                    if (
-                        rteElement &&
-                        rteElement.ej2_instances &&
-                        rteElement.ej2_instances[0]
-                    ) {
+                    const rteElement = document.getElementById("rte");
+                    if (rteElement && rteElement.ej2_instances && rteElement.ej2_instances[0]) {
                         rteElement.ej2_instances[0].enabled = false;
                     }
                     if (typeof disableEditorUpsert === 'function') {
                         disableEditorUpsert();
                     }
 
-                    if (
-                        typeof _kendoEditorUpsert !== 'undefined' &&
-                        _kendoEditorUpsert
-                    ) {
+                    if (typeof _kendoEditorUpsert !== 'undefined' && _kendoEditorUpsert) {
                         _kendoEditorUpsert.body.contentEditable = false;
                         $('#rte').closest('.k-editor').addClass('k-disabled');
                     }
                 } catch (e) {
-                    console.log('Erro ao desabilitar editor:', e);
+                    console.log("Erro ao desabilitar editor:", e);
                 }
             }, 500);
 
-            [
-                'cmbMotorista',
-                'cmbVeiculo',
-                'cmbRequisitante',
-                'cmbOrigem',
-                'cmbDestino',
-            ].forEach((id) => {
-                try {
-                    const control =
-                        document.getElementById(id).ej2_instances[0];
+            ["cmbMotorista", "cmbVeiculo", "cmbRequisitante", "cmbOrigem", "cmbDestino"].forEach(
+                (id) =>
+                {
+                    try
+                    {
+                        const control = document.getElementById(id).ej2_instances[0];
+                        if (control) control.enabled = false;
+                    }
+                    catch (error)
+                    {
+                        Alerta.TratamentoErroComLinha(
+                            "ViagemUpsert.js",
+                            'callback@["cmbMotorista", "cmbVeiculo", "cmbRequi.forEach#0',
+                            error,
+                        );
+                    }
+                },
+            );
+
+            ["ddtSetor", "ddtCombustivelInicial", "ddtCombustivelFinal"].forEach((id) =>
+            {
+                try
+                {
+                    const control = document.getElementById(id).ej2_instances[0];
                     if (control) control.enabled = false;
-                } catch (error) {
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'callback@["cmbMotorista", "cmbVeiculo", "cmbRequi.forEach#0',
-                        error,
-                    );
-                }
-            });
-
-            [
-                'ddtSetor',
-                'ddtCombustivelInicial',
-                'ddtCombustivelFinal',
-            ].forEach((id) => {
-                try {
-                    const control =
-                        document.getElementById(id).ej2_instances[0];
-                    if (control) control.enabled = false;
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
+                        "ViagemUpsert.js",
                         'callback@["ddtSetor", "ddtCombustivelInicial", "d.forEach#0',
                         error,
                     );
                 }
             });
 
-            document.getElementById('ddtFinalidade').ej2_instances[0].enabled =
-                false;
-            document.getElementById('ddtEventos').ej2_instances[0].enabled =
-                false;
-
-            ['btnRequisitante', 'btnSetor', 'btnEvento'].forEach((id) => {
-                try {
+            document.getElementById("ddtFinalidade").ej2_instances[0].enabled = false;
+            document.getElementById("ddtEventos").ej2_instances[0].enabled = false;
+
+            ["btnRequisitante", "btnSetor", "btnEvento"].forEach((id) =>
+            {
+                try
+                {
                     const button = document.getElementById(id);
                     if (button) button.disabled = true;
-                } catch (error) {
+                }
+                catch (error)
+                {
                     Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
+                        "ViagemUpsert.js",
                         'callback@["btnRequisitante", "btnSetor", "btnEven.forEach#0',
                         error,
                     );
                 }
             });
 
-            document.getElementById('divSubmit').style.display = 'none';
-        } else {
-            $('#btnSubmit').show();
-        }
-
-        setTimeout(function () {
+            document.getElementById("divSubmit").style.display = "none";
+        } else
+        {
+            $("#btnSubmit").show();
+        }
+
+        setTimeout(function() {
             calcularDuracaoViagem();
             calcularKmPercorrido();
         }, 600);
 
         const isGuidValido = (guid) => {
-            return (
-                guid &&
-                guid !== '00000000-0000-0000-0000-000000000000' &&
-                guid !== ''
-            );
+            return guid && guid !== "00000000-0000-0000-0000-000000000000" && guid !== "";
         };
 
-        const lblAgendamento = document.getElementById('lblUsuarioAgendamento');
-        if (lblAgendamento) {
-            const temUsuarioAgendamento = isGuidValido(
-                viagem.usuarioIdAgendamento,
-            );
-            const dataAgendamentoValida =
-                viagem.dataAgendamento &&
-                moment(viagem.dataAgendamento).isValid();
-
-            if (
-                (viagem.statusAgendamento || viagem.foiAgendamento) &&
-                temUsuarioAgendamento &&
-                dataAgendamentoValida
-            ) {
-                const DataAgendamento = moment(viagem.dataAgendamento).format(
-                    'DD/MM/YYYY',
-                );
-                const HoraAgendamento = moment(viagem.dataAgendamento).format(
-                    'HH:mm',
-                );
-                $.ajax({
-                    url: '/api/Agenda/RecuperaUsuario',
-                    type: 'Get',
-                    data: { id: viagem.usuarioIdAgendamento },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioAgendamento;
-                            $.each(data, function (key, val) {
-                                try {
-                                    usuarioAgendamento = val;
-                                } catch (error) {
+        const lblAgendamento = document.getElementById("lblUsuarioAgendamento");
+            if (lblAgendamento)
+            {
+                const temUsuarioAgendamento = isGuidValido(viagem.usuarioIdAgendamento);
+                const dataAgendamentoValida = viagem.dataAgendamento && moment(viagem.dataAgendamento).isValid();
+
+                if ((viagem.statusAgendamento || viagem.foiAgendamento) && temUsuarioAgendamento && dataAgendamentoValida)
+                {
+                    const DataAgendamento = moment(viagem.dataAgendamento).format("DD/MM/YYYY");
+                    const HoraAgendamento = moment(viagem.dataAgendamento).format("HH:mm");
+                    $.ajax({
+                        url: "/api/Agenda/RecuperaUsuario",
+                        type: "Get",
+                        data: { id: viagem.usuarioIdAgendamento },
+                        contentType: "application/json; charset=utf-8",
+                        dataType: "json",
+                        success: function (data)
+                        {
+                            try
+                            {
+                                let usuarioAgendamento;
+                                $.each(data, function (key, val)
+                                {
+                                    try
+                                    {
+                                        usuarioAgendamento = val;
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "ViagemUpsert.js",
+                                            "callback@$.each#1",
+                                            error,
+                                        );
+                                    }
+                                });
+                                const lbl = document.getElementById("lblUsuarioAgendamento");
+                                if (lbl && usuarioAgendamento)
+                                {
+                                    lbl.innerHTML =
+                                        '<i class="fa-duotone fa-solid fa-user-clock"></i> ' +
+                                        "<span>Agendado por:</span> " +
+                                        usuarioAgendamento +
+                                        " em " +
+                                        DataAgendamento +
+                                        " às " +
+                                        HoraAgendamento;
+                                    lbl.style.display = "";
+                                    }
+                                }
+                                catch (error)
+                                {
+
                                     Alerta.TratamentoErroComLinha(
-                                        'ViagemUpsert.js',
-                                        'callback@$.each#1',
+                                        "agendamento_viagem.js",
+                                        "success",
                                         error,
                                     );
                                 }
-                            });
-                            const lbl = document.getElementById(
-                                'lblUsuarioAgendamento',
-                            );
-                            if (lbl && usuarioAgendamento) {
-                                lbl.innerHTML =
-                                    '<i class="fa-duotone fa-solid fa-user-clock"></i> ' +
-                                    '<span>Agendado por:</span> ' +
-                                    usuarioAgendamento +
-                                    ' em ' +
-                                    DataAgendamento +
-                                    ' às ' +
-                                    HoraAgendamento;
-                                lbl.style.display = '';
-                            }
-                        } catch (error) {
-
-                            Alerta.TratamentoErroComLinha(
-                                'agendamento_viagem.js',
-                                'success',
-                                error,
-                            );
-                        }
-                    },
-                    error: function (err) {
-                        try {
-                            console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemUpsert.js',
-                                'error',
-                                error,
-                            );
-                        }
-                    },
-                });
-            } else {
-                lblAgendamento.innerHTML = '';
-            }
-        }
-
-        const lblCriacao = document.getElementById('lblUsuarioCriacao');
-        if (lblCriacao) {
-            const temUsuarioCriacao = isGuidValido(viagem.usuarioIdCriacao);
-            const dataCriacaoValida =
-                viagem.dataCriacao && moment(viagem.dataCriacao).isValid();
-
-            if (
-                viagem.statusAgendamento === false &&
-                temUsuarioCriacao &&
-                dataCriacaoValida
-            ) {
-                const DataCriacao = moment(viagem.dataCriacao).format(
-                    'DD/MM/YYYY',
-                );
-                const HoraCriacao = moment(viagem.dataCriacao).format('HH:mm');
-                $.ajax({
-                    url: '/api/Agenda/RecuperaUsuario',
-                    type: 'Get',
-                    data: { id: viagem.usuarioIdCriacao },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioCriacao;
-                            $.each(data, function (key, val) {
-                                try {
-                                    usuarioCriacao = val;
-                                } catch (error) {
+                            },
+                            error: function (err)
+                            {
+                                try
+                                {
+                                    console.log(err);
+                                    AppToast.show("Vermelho", "Erro ao buscar dados de agendamento", 3000);
+                                }
+                                catch (error)
+                                {
                                     Alerta.TratamentoErroComLinha(
-                                        'ViagemUpsert.js',
-                                        'callback@$.each#1',
+                                        "ViagemUpsert.js",
+                                        "error",
                                         error,
                                     );
                                 }
-                            });
-                            const lbl =
-                                document.getElementById('lblUsuarioCriacao');
-                            if (lbl && usuarioCriacao) {
-                                lbl.innerHTML =
-                                    '<i class="fa-sharp-duotone fa-solid fa-user-plus"></i> ' +
-                                    '<span>Criado/Alterado por:</span> ' +
-                                    usuarioCriacao +
-                                    ' em ' +
-                                    DataCriacao +
-                                    ' às ' +
-                                    HoraCriacao;
-                                lbl.style.display = '';
+                            },
+                        });
+                    } else
+                    {
+                        lblAgendamento.innerHTML = "";
+                    }
+            }
+
+            const lblCriacao = document.getElementById("lblUsuarioCriacao");
+            if (lblCriacao)
+            {
+                const temUsuarioCriacao = isGuidValido(viagem.usuarioIdCriacao);
+                const dataCriacaoValida = viagem.dataCriacao && moment(viagem.dataCriacao).isValid();
+
+                if (viagem.statusAgendamento === false && temUsuarioCriacao && dataCriacaoValida)
+                {
+                    const DataCriacao = moment(viagem.dataCriacao).format("DD/MM/YYYY");
+                    const HoraCriacao = moment(viagem.dataCriacao).format("HH:mm");
+                    $.ajax({
+                        url: "/api/Agenda/RecuperaUsuario",
+                        type: "Get",
+                        data: { id: viagem.usuarioIdCriacao },
+                        contentType: "application/json; charset=utf-8",
+                        dataType: "json",
+                        success: function (data)
+                        {
+                            try
+                            {
+                                let usuarioCriacao;
+                                $.each(data, function (key, val)
+                                {
+                                    try
+                                    {
+                                        usuarioCriacao = val;
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "ViagemUpsert.js",
+                                            "callback@$.each#1",
+                                            error,
+                                        );
+                                    }
+                                });
+                                const lbl = document.getElementById("lblUsuarioCriacao");
+                                if (lbl && usuarioCriacao)
+                                {
+                                    lbl.innerHTML =
+                                        '<i class="fa-sharp-duotone fa-solid fa-user-plus"></i> ' +
+                                        "<span>Criado/Alterado por:</span> " +
+                                        usuarioCriacao +
+                                        " em " +
+                                        DataCriacao +
+                                        " às " +
+                                        HoraCriacao;
+                                    lbl.style.display = "";
+                                }
                             }
-                        } catch (error) {
-
-                            Alerta.TratamentoErroComLinha(
-                                'agendamento_viagem.js',
-                                'success',
-                                error,
-                            );
-                        }
-                    },
-                    error: function (err) {
-                        try {
-                            console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemUpsert.js',
-                                'error',
-                                error,
-                            );
-                        }
-                    },
-                });
-            } else {
-                lblCriacao.innerHTML = '';
-            }
-        }
-
-        const lblFinalizacao = document.getElementById('lblUsuarioFinalizacao');
-        if (lblFinalizacao) {
-            const temUsuarioFinalizacao = isGuidValido(
-                viagem.usuarioIdFinalizacao,
-            );
-            const dataFinalizacaoValida =
-                viagem.dataFinalizacao &&
-                moment(viagem.dataFinalizacao).isValid();
-
-            if (
-                viagem.horaFim != null &&
-                temUsuarioFinalizacao &&
-                dataFinalizacaoValida
-            ) {
-                const DataFinalizacao = moment(viagem.dataFinalizacao).format(
-                    'DD/MM/YYYY',
-                );
-                const HoraFinalizacao = moment(viagem.dataFinalizacao).format(
-                    'HH:mm',
-                );
-                $.ajax({
-                    url: '/api/Agenda/RecuperaUsuario',
-                    type: 'Get',
-                    data: { id: viagem.usuarioIdFinalizacao },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioFinalizacao;
-                            $.each(data, function (key, val) {
-                                try {
-                                    usuarioFinalizacao = val;
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'ViagemUpsert.js',
-                                        'callback@$.each#1',
-                                        error,
-                                    );
+                            catch (error)
+                            {
+
+                                Alerta.TratamentoErroComLinha(
+                                    "agendamento_viagem.js",
+                                    "success",
+                                    error,
+                                );
+                            }
+                        },
+                        error: function (err)
+                        {
+                            try
+                            {
+                                console.log(err);
+                                AppToast.show("Vermelho", "Erro ao buscar dados de criação", 3000);
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha(
+                                    "ViagemUpsert.js",
+                                    "error",
+                                    error,
+                                );
+                            }
+                        },
+                    });
+                } else
+                {
+                    lblCriacao.innerHTML = "";
+                }
+            }
+
+            const lblFinalizacao = document.getElementById("lblUsuarioFinalizacao");
+            if (lblFinalizacao)
+            {
+                const temUsuarioFinalizacao = isGuidValido(viagem.usuarioIdFinalizacao);
+                const dataFinalizacaoValida = viagem.dataFinalizacao && moment(viagem.dataFinalizacao).isValid();
+
+                if (viagem.horaFim != null && temUsuarioFinalizacao && dataFinalizacaoValida)
+                {
+                    const DataFinalizacao = moment(viagem.dataFinalizacao).format("DD/MM/YYYY");
+                    const HoraFinalizacao = moment(viagem.dataFinalizacao).format("HH:mm");
+                    $.ajax({
+                        url: "/api/Agenda/RecuperaUsuario",
+                        type: "Get",
+                        data: { id: viagem.usuarioIdFinalizacao },
+                        contentType: "application/json; charset=utf-8",
+                        dataType: "json",
+                        success: function (data)
+                        {
+                            try
+                            {
+                                let usuarioFinalizacao;
+                                $.each(data, function (key, val)
+                                {
+                                    try
+                                    {
+                                        usuarioFinalizacao = val;
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "ViagemUpsert.js",
+                                            "callback@$.each#1",
+                                            error,
+                                        );
+                                    }
+                                });
+                                const lbl = document.getElementById("lblUsuarioFinalizacao");
+                                if (lbl && usuarioFinalizacao)
+                                {
+                                    lbl.innerHTML =
+                                        '<i class="fa-duotone fa-solid fa-user-check"></i> ' +
+                                        "<span>Finalizado por:</span> " +
+                                        usuarioFinalizacao +
+                                        " em " +
+                                        DataFinalizacao +
+                                        " às " +
+                                        HoraFinalizacao;
+                                    lbl.style.display = "";
                                 }
-                            });
-                            const lbl = document.getElementById(
-                                'lblUsuarioFinalizacao',
-                            );
-                            if (lbl && usuarioFinalizacao) {
-                                lbl.innerHTML =
-                                    '<i class="fa-duotone fa-solid fa-user-check"></i> ' +
-                                    '<span>Finalizado por:</span> ' +
-                                    usuarioFinalizacao +
-                                    ' em ' +
-                                    DataFinalizacao +
-                                    ' às ' +
-                                    HoraFinalizacao;
-                                lbl.style.display = '';
                             }
-                        } catch (error) {
-
-                            Alerta.TratamentoErroComLinha(
-                                'agendamento_viagem.js',
-                                'success',
-                                error,
-                            );
-                        }
-                    },
-                    error: function (err) {
-                        try {
-                            console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemUpsert.js',
-                                'error',
-                                error,
-                            );
-                        }
-                    },
-                });
-            } else {
-                lblFinalizacao.innerHTML = '';
-            }
-        }
-
-        const lblCancelamento = document.getElementById(
-            'lblUsuarioCancelamento',
-        );
-        if (lblCancelamento) {
-            const temUsuarioCancelamento = isGuidValido(
-                viagem.usuarioIdCancelamento,
-            );
-            const dataCancelamentoValida =
-                viagem.dataCancelamento &&
-                moment(viagem.dataCancelamento).isValid();
-
-            if (temUsuarioCancelamento && dataCancelamentoValida) {
-                const DataCancelamento = moment(viagem.dataCancelamento).format(
-                    'DD/MM/YYYY',
-                );
-                $.ajax({
-                    url: '/api/Agenda/RecuperaUsuario',
-                    type: 'Get',
-                    data: { id: viagem.usuarioIdCancelamento },
-                    contentType: 'application/json; charset=utf-8',
-                    dataType: 'json',
-                    success: function (data) {
-                        try {
-                            let usuarioCancelamento;
-                            $.each(data, function (key, val) {
-                                try {
-                                    usuarioCancelamento = val;
-                                } catch (error) {
-                                    Alerta.TratamentoErroComLinha(
-                                        'ViagemUpsert.js',
-                                        'callback@$.each#1',
-                                        error,
-                                    );
+                            catch (error)
+                            {
+
+                                Alerta.TratamentoErroComLinha(
+                                    "agendamento_viagem.js",
+                                    "success",
+                                    error,
+                                );
+                            }
+                        },
+                        error: function (err)
+                        {
+                            try
+                            {
+                                console.log(err);
+                                AppToast.show("Vermelho", "Erro ao buscar dados de finalização", 3000);
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha(
+                                    "ViagemUpsert.js",
+                                    "error",
+                                    error,
+                                );
+                            }
+                        },
+                    });
+                } else
+                {
+                    lblFinalizacao.innerHTML = "";
+                }
+            }
+
+            const lblCancelamento = document.getElementById("lblUsuarioCancelamento");
+            if (lblCancelamento)
+            {
+                const temUsuarioCancelamento = isGuidValido(viagem.usuarioIdCancelamento);
+                const dataCancelamentoValida = viagem.dataCancelamento && moment(viagem.dataCancelamento).isValid();
+
+                if (temUsuarioCancelamento && dataCancelamentoValida)
+                {
+                    const DataCancelamento = moment(viagem.dataCancelamento).format("DD/MM/YYYY");
+                    $.ajax({
+                        url: "/api/Agenda/RecuperaUsuario",
+                        type: "Get",
+                        data: { id: viagem.usuarioIdCancelamento },
+                        contentType: "application/json; charset=utf-8",
+                        dataType: "json",
+                        success: function (data)
+                        {
+                            try
+                            {
+                                let usuarioCancelamento;
+                                $.each(data, function (key, val)
+                                {
+                                    try
+                                    {
+                                        usuarioCancelamento = val;
+                                    }
+                                    catch (error)
+                                    {
+                                        Alerta.TratamentoErroComLinha(
+                                            "ViagemUpsert.js",
+                                            "callback@$.each#1",
+                                            error,
+                                        );
+                                    }
+                                });
+                                const lbl = document.getElementById("lblUsuarioCancelamento");
+                                if (lbl && usuarioCancelamento)
+                                {
+                                    lbl.innerHTML =
+                                        '<i class="fa-duotone fa-regular fa-trash-can-xmark"></i> ' +
+                                        "<span>Cancelado por:</span> " +
+                                        usuarioCancelamento +
+                                        " em " +
+                                        DataCancelamento;
+                                    lbl.style.display = "";
                                 }
-                            });
-                            const lbl = document.getElementById(
-                                'lblUsuarioCancelamento',
-                            );
-                            if (lbl && usuarioCancelamento) {
-                                lbl.innerHTML =
-                                    '<i class="fa-duotone fa-regular fa-trash-can-xmark"></i> ' +
-                                    '<span>Cancelado por:</span> ' +
-                                    usuarioCancelamento +
-                                    ' em ' +
-                                    DataCancelamento;
-                                lbl.style.display = '';
                             }
-                        } catch (error) {
-
-                            Alerta.TratamentoErroComLinha(
-                                'agendamento_viagem.js',
-                                'success',
-                                error,
-                            );
-                        }
-                    },
-                    error: function (err) {
-                        try {
-                            console.log(err);
-                            alert('something went wrong');
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha(
-                                'ViagemUpsert.js',
-                                'error',
-                                error,
-                            );
-                        }
-                    },
-                });
-            } else {
-                lblCancelamento.innerHTML = '';
-            }
-        }
-    } catch (error) {
-
-        Alerta.TratamentoErroComLinha(
-            'agendamento_viagem.js',
-            'ExibeViagem',
-            error,
-        );
-    }
-}
-
-function BuscarSetoresPorMotorista(motoristaId) {
-    try {
+                            catch (error)
+                            {
+
+                                Alerta.TratamentoErroComLinha(
+                                    "agendamento_viagem.js",
+                                    "success",
+                                    error,
+                                );
+                            }
+                        },
+                        error: function (err)
+                        {
+                            try
+                            {
+                                console.log(err);
+                                AppToast.show("Vermelho", "Erro ao buscar dados de cancelamento", 3000);
+                            }
+                            catch (error)
+                            {
+                                Alerta.TratamentoErroComLinha(
+                                    "ViagemUpsert.js",
+                                    "error",
+                                    error,
+                                );
+                            }
+                        },
+                    });
+                } else
+                {
+                    lblCancelamento.innerHTML = "";
+                }
+            }
+    }
+    catch (error)
+    {
+
+        Alerta.TratamentoErroComLinha("agendamento_viagem.js", "ExibeViagem", error);
+    }
+}
+
+function BuscarSetoresPorMotorista(motoristaId)
+{
+    try
+    {
         if (!motoristaId) return;
 
         $.ajax({
-            url: '/Setores/BuscarSetoresPorMotorista',
+            url: "/Setores/BuscarSetoresPorMotorista",
             data: { motoristaId: motoristaId },
-            success: function (data) {
-                try {
-                    const ddtSetor =
-                        document.getElementById('ddtSetor').ej2_instances[0];
+            success: function (data)
+            {
+                try
+                {
+                    const ddtSetor = document.getElementById("ddtSetor").ej2_instances[0];
                     ddtSetor.dataSource = data;
                     ddtSetor.refresh();
-                } catch (error) {
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.BuscarSetoresPorMotorista.success',
+                        "ajax.BuscarSetoresPorMotorista.success",
                         error,
                     );
                 }
             },
-            error: function (xhr) {
-                try {
+            error: function (xhr)
+            {
+                try
+                {
                     TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'BuscarSetoresPorMotorista',
+                        "ViagemUpsert.js",
+                        "BuscarSetoresPorMotorista",
                         xhr,
                     );
-                } catch (error) {
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.BuscarSetoresPorMotorista.error',
+                        "ajax.BuscarSetoresPorMotorista.error",
                         error,
                     );
                 }
             },
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'BuscarSetoresPorMotorista',
-            error,
-        );
-    }
-}
-
-function InserirNovoRequisitante() {
-    try {
-        const nome = $('#txtNomeRequisitante').val();
-        if (!nome) {
-            Alerta.Info('Atenção', 'Informe o nome do novo requisitante.');
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "BuscarSetoresPorMotorista", error);
+    }
+}
+
+function InserirNovoRequisitante()
+{
+    try
+    {
+        const nome = $("#txtNomeRequisitante").val();
+        if (!nome)
+        {
+            Alerta.Info("Atenção", "Informe o nome do novo requisitante.");
             return;
         }
 
         $.ajax({
-            url: '/Requisitantes/CriarNovoRequisitante',
-            type: 'POST',
+            url: "/Requisitantes/CriarNovoRequisitante",
+            type: "POST",
             data: { nome: nome },
-            success: function (requisitante) {
-                try {
-                    const cmb =
-                        document.getElementById('cmbRequisitante')
-                            .ej2_instances[0];
+            success: function (requisitante)
+            {
+                try
+                {
+                    const cmb = document.getElementById("cmbRequisitante").ej2_instances[0];
                     cmb.dataSource.push(requisitante);
                     cmb.value = requisitante.id;
                     cmb.dataBind();
-                    $('#modalNovoRequisitante').modal('hide');
-                } catch (error) {
+                    $("#modalNovoRequisitante").modal("hide");
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.CriarNovoRequisitante.success',
+                        "ajax.CriarNovoRequisitante.success",
                         error,
                     );
                 }
             },
-            error: function (xhr) {
-                try {
-                    Alerta.Erro(
-                        'Erro',
-                        'Erro ao criar novo requisitante: ' + xhr.statusText,
-                    );
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.CriarNovoRequisitante.error',
-                        error,
-                    );
+            error: function (xhr)
+            {
+                try
+                {
+                    Alerta.Erro("Erro", "Erro ao criar novo requisitante: " + xhr.statusText);
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.CriarNovoRequisitante.error", error);
                 }
             },
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'InserirNovoRequisitante',
-            error,
-        );
-    }
-}
-
-function VisualizaImagem(input) {
-    try {
-        if (input.files && input.files[0]) {
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "InserirNovoRequisitante", error);
+    }
+}
+
+function VisualizaImagem(input)
+{
+    try
+    {
+        if (input.files && input.files[0])
+        {
             const file = input.files[0];
 
             const maxSize = 5 * 1024 * 1024;
-            if (file.size > maxSize) {
-                AppToast.show(
-                    'Amarelo',
-                    'Arquivo muito grande! Máximo: 5MB',
-                    3000,
-                );
-                input.value = '';
+            if (file.size > maxSize)
+            {
+                AppToast.show("Amarelo", "Arquivo muito grande! Máximo: 5MB", 3000);
+                input.value = "";
                 return;
             }
 
-            const allowedTypes = [
-                'image/jpeg',
-                'image/jpg',
-                'image/png',
-                'image/gif',
-            ];
-            if (!allowedTypes.includes(file.type)) {
-                AppToast.show(
-                    'Amarelo',
-                    'Tipo de arquivo não permitido! Use JPG, PNG ou GIF.',
-                    3000,
-                );
-                input.value = '';
+            const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
+            if (!allowedTypes.includes(file.type))
+            {
+                AppToast.show("Amarelo", "Tipo de arquivo não permitido! Use JPG, PNG ou GIF.", 3000);
+                input.value = "";
                 return;
             }
 
             const reader = new FileReader();
 
-            reader.onload = function (e) {
-                try {
+            reader.onload = function (e)
+            {
+                try
+                {
                     const base64String = e.target.result;
 
-                    $('#imgViewerItem').attr('src', base64String);
-
-                    $('#hiddenFoto').val(base64String);
-
-                    $('#hiddenFichaExistente').val('');
-
-                    console.log('Imagem carregada com sucesso!');
-                    console.log('Base64 length:', base64String.length);
-                    console.log(
-                        'Campo hidden preenchido?',
-                        $('#hiddenFoto').val().length > 0,
-                    );
-
-                    AppToast.show(
-                        'Verde',
-                        'Imagem carregada com sucesso',
-                        2000,
-                    );
-                } catch (error) {
-                    console.error('Erro no onload:', error);
-                    const fallbackImg = '/images/FichaAmarelaNova.jpg';
-                    $('#imgViewerItem').attr('src', fallbackImg);
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'reader.onload',
-                        error,
-                    );
+                    $("#imgViewerItem").attr("src", base64String);
+
+                    $("#hiddenFoto").val(base64String);
+
+                    $("#hiddenFichaExistente").val("");
+
+                    console.log("Imagem carregada com sucesso!");
+                    console.log("Base64 length:", base64String.length);
+                    console.log("Campo hidden preenchido?", $("#hiddenFoto").val().length > 0);
+
+                    AppToast.show("Verde", "Imagem carregada com sucesso", 2000);
+                }
+                catch (error)
+                {
+                    console.error("Erro no onload:", error);
+                    const fallbackImg = "/images/FichaAmarelaNova.jpg";
+                    $("#imgViewerItem").attr("src", fallbackImg);
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "reader.onload", error);
                 }
             };
 
-            reader.onerror = function () {
-                const fallbackImg = '/images/FichaAmarelaNova.jpg';
-                $('#imgViewerItem').attr('src', fallbackImg);
-                AppToast.show('Vermelho', 'Erro ao ler arquivo!', 3000);
+            reader.onerror = function ()
+            {
+                const fallbackImg = "/images/FichaAmarelaNova.jpg";
+                $("#imgViewerItem").attr("src", fallbackImg);
+                AppToast.show("Vermelho", "Erro ao ler arquivo!", 3000);
             };
 
             reader.readAsDataURL(file);
-        } else {
-
-            $('#hiddenFoto').val('');
-            console.log('Input file limpo');
-        }
-    } catch (error) {
-        const fallbackImg = '/images/FichaAmarelaNova.jpg';
-        $('#imgViewerItem').attr('src', fallbackImg);
-        TratamentoErroComLinha('ViagemUpsert.js', 'VisualizaImagem', error);
-    }
-}
-
-function PreencheListaSetores(SetorSolicitanteId) {
-    try {
+        }
+        else
+        {
+
+            $("#hiddenFoto").val("");
+            console.log("Input file limpo");
+        }
+    }
+    catch (error)
+    {
+        const fallbackImg = "/images/FichaAmarelaNova.jpg";
+        $("#imgViewerItem").attr("src", fallbackImg);
+        TratamentoErroComLinha("ViagemUpsert.js", "VisualizaImagem", error);
+    }
+}
+
+function PreencheListaSetores(SetorSolicitanteId)
+{
+    try
+    {
         $.ajax({
-            url: '/Viagens/Upsert?handler=AJAXPreencheListaSetores',
-            method: 'GET',
-            datatype: 'json',
-            success: function (res) {
-                try {
+            url: "/Viagens/Upsert?handler=AJAXPreencheListaSetores",
+            method: "GET",
+            datatype: "json",
+            success: function (res)
+            {
+                try
+                {
                     let SetorList = [];
 
-                    res.data.forEach((item) => {
-                        try {
+                    res.data.forEach((item) =>
+                    {
+                        try
+                        {
                             SetorList.push({
                                 SetorSolicitanteId: item.setorSolicitanteId,
                                 SetorPaiId: item.setorPaiId,
                                 Nome: item.nome,
                                 HasChild: item.hasChild,
                             });
-                        } catch (error) {
+                        }
+                        catch (error)
+                        {
                             Alerta.TratamentoErroComLinha(
-                                'ViagemUpsert.js',
-                                'callback@res.data.forEach#0',
+                                "ViagemUpsert.js",
+                                "callback@res.data.forEach#0",
                                 error,
                             );
                         }
                     });
 
-                    document.getElementById(
-                        'ddtSetor',
-                    ).ej2_instances[0].fields.dataSource = SetorList;
-                } catch (error) {
+                    document.getElementById("ddtSetor").ej2_instances[0].fields.dataSource =
+                        SetorList;
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerAJAXPreencheListaSetores.success',
+                        "ajax.UpserthandlerAJAXPreencheListaSetores.success",
                         error,
                     );
                 }
             },
         });
 
-        document.getElementById('ddtSetor').ej2_instances[0].refresh();
+        document.getElementById("ddtSetor").ej2_instances[0].refresh();
         var strSetor = String(SetorSolicitanteId);
-        document.getElementById('ddtSetor').ej2_instances[0].value = [strSetor];
-    } catch (error) {
+        document.getElementById("ddtSetor").ej2_instances[0].value = [strSetor];
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "PreencheListaSetores", error);
+    }
+}
+
+function RequisitanteValueChange()
+{
+    try
+    {
+        var ddTreeObj = document.getElementById("cmbRequisitante").ej2_instances[0];
+        if (ddTreeObj.value === null) return;
+        var requisitanteid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaSetor",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res)
+            {
+                try
+                {
+                    document.getElementById("ddtSetor").ej2_instances[0].value = [res.data];
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(
+                        __scriptName,
+                        "ajax.UpserthandlerPegaSetor.success",
+                        error,
+                    );
+                }
+            },
+        });
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaRamal",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res)
+            {
+                try
+                {
+                    document.getElementById("txtRamalRequisitante").value = res.data;
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(
+                        __scriptName,
+                        "ajax.UpserthandlerPegaRamal.success",
+                        error,
+                    );
+                }
+            },
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "RequisitanteValueChange", error);
+    }
+}
+
+function RequisitanteEventoValueChange()
+{
+    try
+    {
+        var ddTreeObj = document.getElementById("lstRequisitanteEvento").ej2_instances[0];
+        if (ddTreeObj.value === null) return;
+        var requisitanteid = String(ddTreeObj.value);
+
+        $.ajax({
+            url: "/Viagens/Upsert?handler=PegaSetor",
+            method: "GET",
+            datatype: "json",
+            data: { id: requisitanteid },
+            success: function (res)
+            {
+                try
+                {
+                    document.getElementById("ddtSetorRequisitanteEvento").ej2_instances[0].value = [
+                        res.data,
+                    ];
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(
+                        __scriptName,
+                        "ajax.UpserthandlerPegaSetor.success",
+                        error,
+                    );
+                }
+            },
+        });
+    }
+    catch (error)
+    {
         TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'PreencheListaSetores',
+            "ViagemUpsert.js",
+            "RequisitanteEventoValueChange",
             error,
         );
     }
 }
 
-function RequisitanteValueChange() {
-    try {
-        var ddTreeObj =
-            document.getElementById('cmbRequisitante').ej2_instances[0];
+function MotoristaValueChange()
+{
+    try
+    {
+        var ddTreeObj = document.getElementById("cmbMotorista").ej2_instances[0];
+        console.log("Objeto Motorista:", ddTreeObj);
+
         if (ddTreeObj.value === null) return;
-        var requisitanteid = String(ddTreeObj.value);
+
+        var motoristaid = String(ddTreeObj.value);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: requisitanteid },
-            success: function (res) {
-                try {
-                    document.getElementById('ddtSetor').ej2_instances[0].value =
-                        [res.data];
-                } catch (error) {
+            url: "/Viagens/Upsert?handler=VerificaMotoristaViagem",
+            method: "GET",
+            datatype: "json",
+            data: { id: motoristaid },
+            success: function (res)
+            {
+                try
+                {
+                    var viajando = res.data;
+                    console.log("Motorista Viajando:", viajando);
+
+                    if (viajando)
+                    {
+                        AppToast.show(
+                            "amarelo",
+                            "Este motorista encontra-se em uma viagem não terminada!",
+                            5000
+                        );
+                    }
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerPegaSetor.success',
+                        "ajax.UpserthandlerVerificaMotoristaViagem.success",
                         error,
                     );
                 }
             },
         });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "MotoristaValueChange", error);
+    }
+}
+
+function VeiculoValueChange()
+{
+    try
+    {
+        var ddTreeObj = document.getElementById("cmbVeiculo").ej2_instances[0];
+        console.log("Objeto Veículo:", ddTreeObj);
+
+        if (ddTreeObj.value === null)
+        {
+
+            desabilitarBotaoOcorrenciasVeiculo();
+
+            controlarSecaoOcorrencias(null);
+            return;
+        }
+
+        var veiculoid = String(ddTreeObj.value);
+
+        controlarSecaoOcorrencias(veiculoid);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaRamal',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: requisitanteid },
-            success: function (res) {
-                try {
-                    document.getElementById('txtRamalRequisitante').value =
-                        res.data;
-                } catch (error) {
+            url: "/Viagens/Upsert?handler=VerificaVeiculoViagem",
+            method: "GET",
+            datatype: "json",
+            data: { id: veiculoid },
+            success: function (res)
+            {
+                try
+                {
+                    var viajando = res.data;
+                    console.log("Veículo Viajando:", viajando);
+
+                    if (viajando)
+                    {
+                        AppToast.show(
+                            "amarelo",
+                            "Este veículo encontra-se em uma viagem não terminada!",
+                            5000
+                        );
+                    }
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerPegaRamal.success',
+                        "ajax.UpserthandlerVerificaVeiculoViagem.success",
                         error,
                     );
                 }
             },
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'RequisitanteValueChange',
-            error,
-        );
-    }
-}
-
-function RequisitanteEventoValueChange() {
-    try {
-        var ddTreeObj = document.getElementById('lstRequisitanteEvento')
-            .ej2_instances[0];
-        if (ddTreeObj.value === null) return;
-        var requisitanteid = String(ddTreeObj.value);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaSetor',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: requisitanteid },
-            success: function (res) {
-                try {
-                    document.getElementById(
-                        'ddtSetorRequisitanteEvento',
-                    ).ej2_instances[0].value = [res.data];
-                } catch (error) {
+            url: "/Viagens/Upsert?handler=PegaKmAtualVeiculo",
+            method: "GET",
+            datatype: "json",
+            data: { id: veiculoid },
+            success: function (res)
+            {
+                try
+                {
+                    var km = res.data;
+                    document.getElementById("txtKmAtual").value = km;
+                    document.getElementById("txtKmInicial").value = km;
+                    if (km === 0 || km === "0" || km === null)
+                    {
+                        AppToast.show(
+                            "amarelo",
+                            "Este veículo está sem Quilometragem Atual!",
+                            5000
+                        );
+                        document.getElementById("txtKmAtual").value = "";
+                        document.getElementById("txtKmInicial").value = "";
+                        document.getElementById("txtKmFinal").value = "";
+                        var combo = document.getElementById("cmbVeiculo").ej2_instances[0];
+
+                        combo.focusIn();
+                    }
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerPegaSetor.success',
+                        "ajax.UpserthandlerPegaKmAtualVeiculo.success",
                         error,
                     );
                 }
             },
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'RequisitanteEventoValueChange',
-            error,
-        );
-    }
-}
-
-function MotoristaValueChange() {
-    try {
-        var ddTreeObj =
-            document.getElementById('cmbMotorista').ej2_instances[0];
-        console.log('Objeto Motorista:', ddTreeObj);
-
-        if (ddTreeObj.value === null) return;
-
-        var motoristaid = String(ddTreeObj.value);
+
+        verificarOcorrenciasVeiculo(veiculoid);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "VeiculoValueChange", error);
+    }
+}
+
+$("#btnInserirEvento").click(function (e)
+{
+    try
+    {
+        e.preventDefault();
+
+        if (
+            $("#txtNomeDoEvento").val() === "" ||
+            $("#txtDescricao").val() === "" ||
+            $("#txtDataInicialEvento").val() === "" ||
+            $("#txtDataFinalEvento").val() === "" ||
+            $("#txtQtdPessoas").val() === ""
+        )
+        {
+            AppToast.show("amarelo", "Todos os campos são obrigatórios!", 5000);
+            return;
+        }
+
+        let setores = document.getElementById("ddtSetorRequisitanteEvento").ej2_instances[0];
+        let requisitantes = document.getElementById("lstRequisitanteEvento").ej2_instances[0];
+
+        if (!setores.value || !requisitantes.value)
+        {
+            AppToast.show("amarelo", "Setor e Requisitante são obrigatórios!", 5000);
+            return;
+        }
+
+        let objEvento = JSON.stringify({
+            Nome: $("#txtNomeDoEvento").val(),
+            Descricao: $("#txtDescricaoEvento").val(),
+            SetorSolicitanteId: setores.value.toString(),
+            RequisitanteId: requisitantes.value.toString(),
+            QtdParticipantes: $("#txtQtdPessoas").val(),
+            DataInicial: moment($("#txtDataInicialEvento").val()).format("MM-DD-YYYY"),
+            DataFinal: moment($("#txtDataFinalEvento").val()).format("MM-DD-YYYY"),
+            Status: "1",
+        });
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=VerificaMotoristaViagem',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: motoristaid },
-            success: function (res) {
-                try {
-                    var viajando = res.data;
-                    console.log('Motorista Viajando:', viajando);
-
-                    if (viajando) {
-                        AppToast.show(
-                            'amarelo',
-                            'Este motorista encontra-se em uma viagem não terminada!',
-                            5000,
+            type: "POST",
+            url: "/api/Viagem/AdicionarEvento",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objEvento,
+            success: function (data)
+            {
+                try
+                {
+                    AppToast.show('Verde', data.message);
+                    PreencheListaEventos(data.eventoId);
+                    $("#modalEvento").hide();
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarEvento.success", error);
+                }
+            },
+            error: function (data)
+            {
+                try
+                {
+                    AppToast.show("Vermelho", "Erro ao adicionar evento", 3000);
+                    console.log(data);
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarEvento.error", error);
+                }
+            },
+        });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirEvento", error);
+    }
+});
+
+$("#btnInserirRequisitante").click(function (e)
+{
+    try
+    {
+        e.preventDefault();
+
+        if (
+            $("#txtPonto").val() === "" ||
+            $("#txtNome").val() === "" ||
+            $("#txtRamal").val() === ""
+        )
+        {
+            AppToast.show("amarelo", "Ponto, Nome e Ramal são obrigatórios!", 5000);
+            return;
+        }
+
+        let setores = document.getElementById("ddtSetorRequisitante").ej2_instances[0];
+        if (!setores.value)
+        {
+            AppToast.show("amarelo", "O Setor do Requisitante é obrigatório!", 5000);
+            return;
+        }
+
+        let objRequisitante = JSON.stringify({
+            Nome: $("#txtNome").val(),
+            Ponto: $("#txtPonto").val(),
+            Ramal: $("#txtRamal").val(),
+            Email: $("#txtEmail").val(),
+            SetorSolicitanteId: setores.value.toString(),
+        });
+
+        $.ajax({
+            type: "POST",
+            url: "/api/Viagem/AdicionarRequisitante",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objRequisitante,
+            success: function (data)
+            {
+                try
+                {
+                    if (data.success)
+                    {
+                        AppToast.show('Verde', data.message);
+                        document.getElementById("cmbRequisitante").ej2_instances[0].addItem(
+                            {
+                                RequisitanteId: data.requisitanteid,
+                                Requisitante: $("#txtNome").val() + " - " + $("#txtPonto").val(),
+                            },
+                            0,
                         );
+                        $("#modalRequisitante").hide();
+                        $(".modal-backdrop").remove();
+                        $("body").removeClass("modal-open").css("overflow", "auto");
+                        $("#btnFecharRequisitante").click();
+                    } else
+                    {
+                        AppToast.show('Vermelho', data.message);
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerVerificaMotoristaViagem.success',
+                        "ajax.AdicionarRequisitante.success",
                         error,
                     );
                 }
             },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'MotoristaValueChange',
-            error,
-        );
-    }
-}
-
-function VeiculoValueChange() {
-    try {
-        var ddTreeObj = document.getElementById('cmbVeiculo').ej2_instances[0];
-        console.log('Objeto Veículo:', ddTreeObj);
-
-        if (ddTreeObj.value === null) {
-
-            desabilitarBotaoOcorrenciasVeiculo();
-
-            controlarSecaoOcorrencias(null);
-            return;
-        }
-
-        var veiculoid = String(ddTreeObj.value);
-
-        controlarSecaoOcorrencias(veiculoid);
-
-        $.ajax({
-            url: '/Viagens/Upsert?handler=VerificaVeiculoViagem',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: veiculoid },
-            success: function (res) {
-                try {
-                    var viajando = res.data;
-                    console.log('Veículo Viajando:', viajando);
-
-                    if (viajando) {
-                        AppToast.show(
-                            'amarelo',
-                            'Este veículo encontra-se em uma viagem não terminada!',
-                            5000,
-                        );
-                    }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.UpserthandlerVerificaVeiculoViagem.success',
-                        error,
-                    );
+            error: function (data)
+            {
+                try
+                {
+                    Alerta.Erro("Atenção", "Já existe um requisitante com este ponto/nome!");
+                    console.log(data);
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarRequisitante.error", error);
                 }
             },
         });
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirRequisitante", error);
+    }
+});
+
+$("#btnInserirSetor").click(function (e)
+{
+    try
+    {
+        e.preventDefault();
+
+        if ($("#txtNomeSetor").val() === "" || $("#txtRamalSetor").val() === "")
+        {
+            AppToast.show("amarelo", "Nome e Ramal do Setor são obrigatórios!", 5000);
+            return;
+        }
+
+        let setorPaiId = null;
+        let setorPai = document.getElementById("ddtSetorPai").ej2_instances[0].value;
+        if (setorPai !== "" && setorPai !== null)
+        {
+            setorPaiId = setorPai.toString();
+        }
+
+        let objSetorData = {
+            Nome: $("#txtNomeSetor").val(),
+            Ramal: $("#txtRamalSetor").val(),
+            Sigla: $("#txtSigla").val(),
+        };
+
+        if (setorPaiId)
+        {
+            objSetorData["SetorPaiId"] = setorPaiId;
+        }
+
+        let objSetor = JSON.stringify(objSetorData);
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=PegaKmAtualVeiculo',
-            method: 'GET',
-            datatype: 'json',
-            data: { id: veiculoid },
-            success: function (res) {
-                try {
-                    var km = res.data;
-                    document.getElementById('txtKmAtual').value = km;
-                    document.getElementById('txtKmInicial').value = km;
-                    if (km === 0 || km === '0' || km === null) {
-                        AppToast.show(
-                            'amarelo',
-                            'Este veículo está sem Quilometragem Atual!',
-                            5000,
-                        );
-                        document.getElementById('txtKmAtual').value = '';
-                        document.getElementById('txtKmInicial').value = '';
-                        document.getElementById('txtKmFinal').value = '';
-                        var combo =
-                            document.getElementById('cmbVeiculo')
-                                .ej2_instances[0];
-
-                        combo.focusIn();
-                    }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.UpserthandlerPegaKmAtualVeiculo.success',
-                        error,
-                    );
+            type: "POST",
+            url: "/api/Viagem/AdicionarSetor",
+            contentType: "application/json; charset=utf-8",
+            dataType: "json",
+            data: objSetor,
+            success: function (data)
+            {
+                try
+                {
+                    AppToast.show('Verde', data.message);
+                    PreencheListaSetores(data.setorId);
+                    $("#modalSetor").hide();
+                    $(".modal-backdrop").remove();
+                    $("body").removeClass("modal-open");
+                    $("body").css("overflow", "auto");
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarSetor.success", error);
+                }
+            },
+            error: function (data)
+            {
+                try
+                {
+                    AppToast.show("Vermelho", "Erro ao adicionar setor", 3000);
+                    console.log(data);
+                }
+                catch (error)
+                {
+                    TratamentoErroComLinha(__scriptName, "ajax.AdicionarSetor.error", error);
                 }
             },
         });
-
-        verificarOcorrenciasVeiculo(veiculoid);
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'VeiculoValueChange', error);
-    }
-}
-
-$('#btnInserirEvento').click(function (e) {
-    try {
-        e.preventDefault();
-
-        if (
-            $('#txtNomeDoEvento').val() === '' ||
-            $('#txtDescricao').val() === '' ||
-            $('#txtDataInicialEvento').val() === '' ||
-            $('#txtDataFinalEvento').val() === '' ||
-            $('#txtQtdPessoas').val() === ''
-        ) {
-            AppToast.show('amarelo', 'Todos os campos são obrigatórios!', 5000);
-            return;
-        }
-
-        let setores = document.getElementById('ddtSetorRequisitanteEvento')
-            .ej2_instances[0];
-        let requisitantes = document.getElementById('lstRequisitanteEvento')
-            .ej2_instances[0];
-
-        if (!setores.value || !requisitantes.value) {
-            AppToast.show(
-                'amarelo',
-                'Setor e Requisitante são obrigatórios!',
-                5000,
-            );
-            return;
-        }
-
-        let objEvento = JSON.stringify({
-            Nome: $('#txtNomeDoEvento').val(),
-            Descricao: $('#txtDescricaoEvento').val(),
-            SetorSolicitanteId: setores.value.toString(),
-            RequisitanteId: requisitantes.value.toString(),
-            QtdParticipantes: $('#txtQtdPessoas').val(),
-            DataInicial: moment($('#txtDataInicialEvento').val()).format(
-                'MM-DD-YYYY',
-            ),
-            DataFinal: moment($('#txtDataFinalEvento').val()).format(
-                'MM-DD-YYYY',
-            ),
-            Status: '1',
-        });
-
-        $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AdicionarEvento',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
-            data: objEvento,
-            success: function (data) {
-                try {
-                    AppToast.show('Verde', data.message);
-                    PreencheListaEventos(data.eventoId);
-                    $('#modalEvento').hide();
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarEvento.success',
-                        error,
-                    );
-                }
-            },
-            error: function (data) {
-                try {
-                    alert('error');
-                    console.log(data);
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarEvento.error',
-                        error,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btnInserirEvento',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "click.btnInserirSetor", error);
     }
 });
 
-$('#btnInserirRequisitante').click(function (e) {
-    try {
-        e.preventDefault();
-
-        if (
-            $('#txtPonto').val() === '' ||
-            $('#txtNome').val() === '' ||
-            $('#txtRamal').val() === ''
-        ) {
-            AppToast.show(
-                'amarelo',
-                'Ponto, Nome e Ramal são obrigatórios!',
-                5000,
-            );
-            return;
-        }
-
-        let setores = document.getElementById('ddtSetorRequisitante')
-            .ej2_instances[0];
-        if (!setores.value) {
-            AppToast.show(
-                'amarelo',
-                'O Setor do Requisitante é obrigatório!',
-                5000,
-            );
-            return;
-        }
-
-        let objRequisitante = JSON.stringify({
-            Nome: $('#txtNome').val(),
-            Ponto: $('#txtPonto').val(),
-            Ramal: $('#txtRamal').val(),
-            Email: $('#txtEmail').val(),
-            SetorSolicitanteId: setores.value.toString(),
-        });
-
-        $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AdicionarRequisitante',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
-            data: objRequisitante,
-            success: function (data) {
-                try {
-                    if (data.success) {
-                        AppToast.show('Verde', data.message);
-                        document
-                            .getElementById('cmbRequisitante')
-                            .ej2_instances[0].addItem(
-                                {
-                                    RequisitanteId: data.requisitanteid,
-                                    Requisitante:
-                                        $('#txtNome').val() +
-                                        ' - ' +
-                                        $('#txtPonto').val(),
-                                },
-                                0,
-                            );
-                        $('#modalRequisitante').hide();
-                        $('.modal-backdrop').remove();
-                        $('body')
-                            .removeClass('modal-open')
-                            .css('overflow', 'auto');
-                        $('#btnFecharRequisitante').click();
-                    } else {
-                        AppToast.show('Vermelho', data.message);
-                    }
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarRequisitante.success',
-                        error,
-                    );
-                }
-            },
-            error: function (data) {
-                try {
-                    Alerta.Erro(
-                        'Atenção',
-                        'Já existe um requisitante com este ponto/nome!',
-                    );
-                    console.log(data);
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarRequisitante.error',
-                        error,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btnInserirRequisitante',
-            error,
-        );
-    }
-});
-
-$('#btnInserirSetor').click(function (e) {
-    try {
-        e.preventDefault();
-
-        if (
-            $('#txtNomeSetor').val() === '' ||
-            $('#txtRamalSetor').val() === ''
-        ) {
-            AppToast.show(
-                'amarelo',
-                'Nome e Ramal do Setor são obrigatórios!',
-                5000,
-            );
-            return;
-        }
-
-        let setorPaiId = null;
-        let setorPai =
-            document.getElementById('ddtSetorPai').ej2_instances[0].value;
-        if (setorPai !== '' && setorPai !== null) {
-            setorPaiId = setorPai.toString();
-        }
-
-        let objSetorData = {
-            Nome: $('#txtNomeSetor').val(),
-            Ramal: $('#txtRamalSetor').val(),
-            Sigla: $('#txtSigla').val(),
-        };
-
-        if (setorPaiId) {
-            objSetorData['SetorPaiId'] = setorPaiId;
-        }
-
-        let objSetor = JSON.stringify(objSetorData);
-
-        $.ajax({
-            type: 'POST',
-            url: '/api/Viagem/AdicionarSetor',
-            contentType: 'application/json; charset=utf-8',
-            dataType: 'json',
-            data: objSetor,
-            success: function (data) {
-                try {
-                    AppToast.show('Verde', data.message);
-                    PreencheListaSetores(data.setorId);
-                    $('#modalSetor').hide();
-                    $('.modal-backdrop').remove();
-                    $('body').removeClass('modal-open');
-                    $('body').css('overflow', 'auto');
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarSetor.success',
-                        error,
-                    );
-                }
-            },
-            error: function (data) {
-                try {
-                    alert('error');
-                    console.log(data);
-                } catch (error) {
-                    TratamentoErroComLinha(
-                        __scriptName,
-                        'ajax.AdicionarSetor.error',
-                        error,
-                    );
-                }
-            },
-        });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btnInserirSetor',
-            error,
-        );
-    }
-});
-
-$('#btnSubmit').click(async function (event) {
-    try {
+$("#btnSubmit").click(async function (event)
+{
+    try
+    {
         event.preventDefault();
 
-        if ($('#txtDataInicial').val() === '') {
-            Alerta.Erro('Informação Ausente', 'A Data Inicial é obrigatória');
-            return;
-        }
-
-        if ($('#txtHoraInicial').val() === '') {
-            Alerta.Erro('Informação Ausente', 'A Hora Inicial é obrigatória');
-            return;
-        }
-
-        const finalidade =
-            document.getElementById('ddtFinalidade').ej2_instances[0];
-        if (!finalidade.value || finalidade.value[0] === null) {
-            Alerta.Erro('Informação Ausente', 'A Finalidade é obrigatória');
-            return;
-        }
-
-        const origem = document.getElementById('cmbOrigem').ej2_instances[0];
-        if (origem.value === null) {
-            Alerta.Erro('Informação Ausente', 'A Origem é obrigatória');
-            return;
-        }
-
-        const motorista =
-            document.getElementById('cmbMotorista').ej2_instances[0];
-        if (motorista.value === null) {
-            Alerta.Erro('Informação Ausente', 'O Motorista é obrigatório');
-            return;
-        }
-
-        const veiculo = document.getElementById('cmbVeiculo').ej2_instances[0];
-        if (veiculo.value === null) {
-            Alerta.Erro('Informação Ausente', 'O Veículo é obrigatório');
-            return;
-        }
-
-        if ($('#txtKmInicial').val() === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'A Quilometragem Inicial é obrigatória',
-            );
-            return;
-        }
-
-        const combustivel = document.getElementById('ddtCombustivelInicial')
-            .ej2_instances[0];
-        if (!combustivel.value) {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O Nível de Combustível Inicial é obrigatório',
-            );
-            return;
-        }
-
-        const requisitante =
-            document.getElementById('cmbRequisitante').ej2_instances[0];
-        if (!requisitante.value || requisitante.value[0] === null) {
-            Alerta.Erro('Informação Ausente', 'O Requisitante é obrigatório');
-            return;
-        }
-
-        if ($('#txtRamalRequisitante').val() === '') {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O Ramal do Requisitante é obrigatório',
-            );
-            return;
-        }
-
-        const setor = document.getElementById('ddtSetor').ej2_instances[0];
-        if (!setor.value) {
-            Alerta.Erro(
-                'Informação Ausente',
-                'O Setor Solicitante é obrigatório',
-            );
-            return;
-        }
-
-        const dataFinal = $('#txtDataFinal').val();
-        const horaFinal = $('#txtHoraFinal').val();
-        const combustivelFinal = document.getElementById('ddtCombustivelFinal')
-            .ej2_instances[0].value;
-        const kmFinal = $('#txtKmFinal').val();
-
-        if (dataFinal) {
-            const dataFinalDate = new Date(dataFinal + 'T00:00:00');
+        if ($("#txtDataInicial").val() === "")
+        {
+            Alerta.Erro("Informação Ausente", "A Data Inicial é obrigatória");
+            return;
+        }
+
+        if ($("#txtHoraInicial").val() === "")
+        {
+            Alerta.Erro("Informação Ausente", "A Hora Inicial é obrigatória");
+            return;
+        }
+
+        const finalidade = document.getElementById("ddtFinalidade").ej2_instances[0];
+        if (!finalidade.value || finalidade.value[0] === null)
+        {
+            Alerta.Erro("Informação Ausente", "A Finalidade é obrigatória");
+            return;
+        }
+
+        const origem = document.getElementById("cmbOrigem").ej2_instances[0];
+        if (origem.value === null)
+        {
+            Alerta.Erro("Informação Ausente", "A Origem é obrigatória");
+            return;
+        }
+
+        const motorista = document.getElementById("cmbMotorista").ej2_instances[0];
+        if (motorista.value === null)
+        {
+            Alerta.Erro("Informação Ausente", "O Motorista é obrigatório");
+            return;
+        }
+
+        const veiculo = document.getElementById("cmbVeiculo").ej2_instances[0];
+        if (veiculo.value === null)
+        {
+            Alerta.Erro("Informação Ausente", "O Veículo é obrigatório");
+            return;
+        }
+
+        if ($("#txtKmInicial").val() === "")
+        {
+            Alerta.Erro("Informação Ausente", "A Quilometragem Inicial é obrigatória");
+            return;
+        }
+
+        const combustivel = document.getElementById("ddtCombustivelInicial").ej2_instances[0];
+        if (!combustivel.value)
+        {
+            Alerta.Erro("Informação Ausente", "O Nível de Combustível Inicial é obrigatório");
+            return;
+        }
+
+        const requisitante = document.getElementById("cmbRequisitante").ej2_instances[0];
+        if (!requisitante.value || requisitante.value[0] === null)
+        {
+            Alerta.Erro("Informação Ausente", "O Requisitante é obrigatório");
+            return;
+        }
+
+        if ($("#txtRamalRequisitante").val() === "")
+        {
+            Alerta.Erro("Informação Ausente", "O Ramal do Requisitante é obrigatório");
+            return;
+        }
+
+        const setor = document.getElementById("ddtSetor").ej2_instances[0];
+        if (!setor.value)
+        {
+            Alerta.Erro("Informação Ausente", "O Setor Solicitante é obrigatório");
+            return;
+        }
+
+        const dataFinal = $("#txtDataFinal").val();
+        const horaFinal = $("#txtHoraFinal").val();
+        const combustivelFinal =
+            document.getElementById("ddtCombustivelFinal").ej2_instances[0].value;
+        const kmFinal = $("#txtKmFinal").val();
+
+        if (dataFinal)
+        {
+            const dataFinalDate = new Date(dataFinal + "T00:00:00");
             const hoje = new Date();
             hoje.setHours(0, 0, 0, 0);
-            if (dataFinalDate > hoje) {
-                $('#txtDataFinal').val('');
-                $('#txtDataFinal').focus();
-                AppToast.show(
-                    'Amarelo',
-                    'A Data Final não pode ser superior à data atual.',
-                    4000,
-                );
+            if (dataFinalDate > hoje)
+            {
+                $("#txtDataFinal").val("");
+                $("#txtDataFinal").focus();
+                AppToast.show("Amarelo", "A Data Final não pode ser superior à data atual.", 4000);
                 return;
             }
         }
 
-        const algumFinalPreenchido =
-            dataFinal || horaFinal || combustivelFinal || kmFinal;
-        const todosFinalPreenchidos =
-            dataFinal && horaFinal && combustivelFinal && kmFinal;
-
-        if (kmFinal && parseFloat(kmFinal) <= 0) {
+        const algumFinalPreenchido = dataFinal || horaFinal || combustivelFinal || kmFinal;
+        const todosFinalPreenchidos = dataFinal && horaFinal && combustivelFinal && kmFinal;
+
+        if (kmFinal && parseFloat(kmFinal) <= 0)
+        {
+            Alerta.Erro("Informação Incorreta", "A Quilometragem Final deve ser maior que zero");
+            return;
+        }
+
+        if (algumFinalPreenchido && !todosFinalPreenchidos)
+        {
             Alerta.Erro(
-                'Informação Incorreta',
-                'A Quilometragem Final deve ser maior que zero',
+                "Informação Incompleta",
+                "Todos os campos de Finalização devem ser preenchidos para encerrar a viagem",
             );
             return;
         }
 
-        if (algumFinalPreenchido && !todosFinalPreenchidos) {
-            Alerta.Erro(
-                'Informação Incompleta',
-                'Todos os campos de Finalização devem ser preenchidos para encerrar a viagem',
+        if (todosFinalPreenchidos)
+        {
+            const confirmacao = await Alerta.Confirmar(
+                "Confirmar Fechamento",
+                'Você está criando a viagem como "Realizada". Deseja continuar?',
+                "Sim, criar!",
+                "Cancelar",
             );
-            return;
-        }
-
-        if (todosFinalPreenchidos) {
-            const confirmacao = await Alerta.Confirmar(
-                'Confirmar Fechamento',
-                'Você está criando a viagem como "Realizada". Deseja continuar?',
-                'Sim, criar!',
-                'Cancelar',
-            );
-
-            if (!confirmacao) {
+
+            if (!confirmacao)
+            {
                 return;
             }
         }
 
         const datasOk = await validarDatasInicialFinal(
-            $('#txtDataInicial').val(),
-            $('#txtDataFinal').val(),
+            $("#txtDataInicial").val(),
+            $("#txtDataFinal").val(),
         );
-        if (!datasOk) {
+        if (!datasOk)
+        {
             return;
         }
 
         const kmOk = await validarKmInicialFinal();
-        if (!kmOk) {
-            return;
-        }
-
-        if (
-            todosFinalPreenchidos &&
-            typeof window.validarFinalizacaoConsolidadaIA === 'function'
-        ) {
-            const veiculoId =
-                document.getElementById('cmbVeiculo')?.ej2_instances?.[0]
-                    ?.value || '';
+        if (!kmOk)
+        {
+            return;
+        }
+
+        if (todosFinalPreenchidos && typeof window.validarFinalizacaoConsolidadaIA === 'function')
+        {
+            const veiculoId = document.getElementById("cmbVeiculo")?.ej2_instances?.[0]?.value || '';
 
             const iaValida = await window.validarFinalizacaoConsolidadaIA({
-                dataInicial: $('#txtDataInicial').val(),
-                horaInicial: $('#txtHoraInicial').val(),
+                dataInicial: $("#txtDataInicial").val(),
+                horaInicial: $("#txtHoraInicial").val(),
                 dataFinal: dataFinal,
                 horaFinal: horaFinal,
-                kmInicial: parseInt($('#txtKmInicial').val()) || 0,
+                kmInicial: parseInt($("#txtKmInicial").val()) || 0,
                 kmFinal: parseInt(kmFinal) || 0,
-                veiculoId: veiculoId,
+                veiculoId: veiculoId
             });
 
-            if (!iaValida) {
+            if (!iaValida)
+            {
                 return;
             }
         }
 
-        $('#btnSubmit').prop('disabled', true);
-        $('#btnEscondido').click();
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'click.btnSubmit', error);
+        $("#btnSubmit").prop("disabled", true);
+        $("#btnEscondido").click();
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "click.btnSubmit", error);
     }
 });
 
-$('#txtNoFichaVistoria').focusout(async function () {
-    try {
-        let noFicha = $('#txtNoFichaVistoria').val();
-        if (noFicha === '') return;
+$("#txtNoFichaVistoria").focusout(async function ()
+{
+    try
+    {
+        let noFicha = $("#txtNoFichaVistoria").val();
+        if (noFicha === "") return;
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=VerificaFicha',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Viagens/Upsert?handler=VerificaFicha",
+            method: "GET",
+            datatype: "json",
             data: { id: noFicha },
-            success: async function (res) {
-                try {
+            success: async function (res)
+            {
+                try
+                {
                     let maxFicha = parseInt(res.data);
-                    if (noFicha > maxFicha + 100 || noFicha < maxFicha - 100) {
+                    if (noFicha > maxFicha + 100 || noFicha < maxFicha - 100)
+                    {
                         const confirmado = await Alerta.Confirmar(
-                            'Ficha Divergente',
-                            'O número inserido difere em ±100 da última Ficha inserida! Tem certeza?',
-                            'Tenho certeza! 💪🏼',
+                            "Ficha Divergente",
+                            "O número inserido difere em ±100 da última Ficha inserida! Tem certeza?",
+                            "Tenho certeza! 💪🏼",
                             "Me enganei! 😟'",
                         );
 
-                        if (!confirmado) {
-                            document.getElementById(
-                                'txtNoFichaVistoria',
-                            ).value = '';
-                            document
-                                .getElementById('txtNoFichaVistoria')
-                                .focus();
+                        if (!confirmado)
+                        {
+                            document.getElementById("txtNoFichaVistoria").value = "";
+                            document.getElementById("txtNoFichaVistoria").focus();
                             return;
                         }
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerVerificaFicha.success',
+                        "ajax.UpserthandlerVerificaFicha.success",
                         error,
                     );
                 }
@@ -2721,80 +2729,70 @@
         });
 
         $.ajax({
-            url: '/Viagens/Upsert?handler=FichaExistente',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Viagens/Upsert?handler=FichaExistente",
+            method: "GET",
+            datatype: "json",
             data: { id: noFicha },
-            success: async function (res) {
-                try {
-                    if (res.data === true) {
+            success: async function (res)
+            {
+                try
+                {
+                    if (res.data === true)
+                    {
                         await window.SweetAlertInterop.ShowPreventionAlert(
-                            'Já existe uma Ficha inserida com esta numeração!',
+                            "Já existe uma Ficha inserida com esta numeração!",
                         );
                     }
-                } catch (error) {
+                }
+                catch (error)
+                {
                     TratamentoErroComLinha(
                         __scriptName,
-                        'ajax.UpserthandlerFichaExistente.success',
+                        "ajax.UpserthandlerFichaExistente.success",
                         error,
                     );
                 }
             },
         });
-    } catch (error) {
-        TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'focusout.txtNoFichaVistoria',
-            error,
-        );
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "focusout.txtNoFichaVistoria", error);
     }
 });
 
-function calcularKmPercorrido() {
-    try {
-        var elKmInicial = document.getElementById('txtKmInicial');
-        var elKmFinal = document.getElementById('txtKmFinal');
-        var elKmPercorrido = document.getElementById('txtKmPercorrido');
-
-        console.log(
-            'calcularKmPercorrido - KmInicial:',
-            elKmInicial?.value,
-            'KmFinal:',
-            elKmFinal?.value,
-            'Status:',
-            window.viagemStatus,
-        );
+function calcularKmPercorrido()
+{
+    try
+    {
+        var elKmInicial = document.getElementById("txtKmInicial");
+        var elKmFinal = document.getElementById("txtKmFinal");
+        var elKmPercorrido = document.getElementById("txtKmPercorrido");
+
+        console.log("calcularKmPercorrido - KmInicial:", elKmInicial?.value, "KmFinal:", elKmFinal?.value, "Status:", window.viagemStatus);
 
         if (!elKmInicial || !elKmFinal || !elKmPercorrido) return;
 
-        if (window.viagemStatus !== 'Realizada') {
-            elKmPercorrido.value = '';
+        if (window.viagemStatus !== "Realizada")
+        {
+            elKmPercorrido.value = "";
             if (typeof FieldUX !== 'undefined') {
                 FieldUX.setInvalid(elKmPercorrido, false);
                 FieldUX.setHigh(elKmPercorrido, false);
-                FieldUX.tooltipOnTransition(
-                    elKmPercorrido,
-                    false,
-                    1000,
-                    'tooltipKm',
-                );
-            }
-            return;
-        }
-
-        var kmInicial = parseFloat((elKmInicial.value || '').replace(',', '.'));
-        var kmFinal = parseFloat((elKmFinal.value || '').replace(',', '.'));
-        if (isNaN(kmInicial) || isNaN(kmFinal)) {
-            elKmPercorrido.value = '';
+                FieldUX.tooltipOnTransition(elKmPercorrido, false, 1000, 'tooltipKm');
+            }
+            return;
+        }
+
+        var kmInicial = parseFloat((elKmInicial.value || '').replace(",", "."));
+        var kmFinal = parseFloat((elKmFinal.value || '').replace(",", "."));
+        if (isNaN(kmInicial) || isNaN(kmFinal))
+        {
+            elKmPercorrido.value = "";
             if (typeof FieldUX !== 'undefined') {
                 FieldUX.setInvalid(elKmPercorrido, false);
                 FieldUX.setHigh(elKmPercorrido, false);
-                FieldUX.tooltipOnTransition(
-                    elKmPercorrido,
-                    false,
-                    1000,
-                    'tooltipKm',
-                );
+                FieldUX.tooltipOnTransition(elKmPercorrido, false, 1000, 'tooltipKm');
             }
             return;
         }
@@ -2802,86 +2800,97 @@
         var diff = kmFinal - kmInicial;
         elKmPercorrido.value = diff;
 
-        var invalid = diff < 0 || diff > 100;
-        var high = diff >= 50 && diff < 100;
+        var invalid = (diff < 0 || diff > 100);
+        var high = (diff >= 50 && diff < 100);
         if (typeof FieldUX !== 'undefined') {
             FieldUX.setInvalid(elKmPercorrido, invalid);
             FieldUX.setHigh(elKmPercorrido, high);
 
-            FieldUX.tooltipOnTransition(
-                elKmPercorrido,
-                diff > 100,
-                1000,
-                'tooltipKm',
-            );
-        }
-    } catch (error) {
-        if (typeof TratamentoErroComLinha === 'function') {
-            TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'calcularKmPercorrido',
-                error,
-            );
-        } else {
+            FieldUX.tooltipOnTransition(elKmPercorrido, diff > 100, 1000, 'tooltipKm');
+        }
+    } catch (error)
+    {
+        if (typeof TratamentoErroComLinha === 'function')
+        {
+            TratamentoErroComLinha("ViagemUpsert.js", "calcularKmPercorrido", error);
+        } else
+        {
             console.error(error);
         }
     }
 }
 
-['input', 'focusout', 'change'].forEach((evt) => {
-    try {
-        return document
-            .getElementById('txtKmFinal')
-            ?.addEventListener(evt, calcularKmPercorrido);
-    } catch (error) {
+["input", "focusout", "change"].forEach((evt) =>
+{
+    try
+    {
+        return document.getElementById("txtKmFinal")?.addEventListener(evt, calcularKmPercorrido);
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
+            "ViagemUpsert.js",
             'callback@["input", "focusout", "change"].forEach#0',
             error,
         );
     }
 });
 
-['input', 'focusout', 'change'].forEach((evt) => {
-    try {
+["input", "focusout", "change"].forEach((evt) =>
+{
+    try
+    {
         return document
-            .getElementById('txtHoraFinal')
+            .getElementById("txtHoraFinal")
             ?.addEventListener(evt, calcularDuracaoViagem);
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
+            "ViagemUpsert.js",
             'callback@["input", "focusout", "change"].forEach#0',
             error,
         );
     }
 });
 
-['input', 'focusout', 'change'].forEach((evt) => {
-    try {
+["input", "focusout", "change"].forEach((evt) =>
+{
+    try
+    {
         return document
-            .getElementById('txtKmPercorrido')
+            .getElementById("txtKmPercorrido")
             ?.addEventListener(evt, calcularKmPercorrido);
-    } catch (error) {
+    }
+    catch (error)
+    {
         Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
+            "ViagemUpsert.js",
             'callback@["input", "focusout", "change"].forEach#0',
             error,
         );
     }
 });
 
-window.addEventListener('load', () => {
-    try {
-        const duracaoInput = document.getElementById('txtDuracao');
-        if (duracaoInput) {
+window.addEventListener("load", () =>
+{
+    try
+    {
+        const duracaoInput = document.getElementById("txtDuracao");
+        if (duracaoInput)
+        {
             duracaoInput.addEventListener(
-                'focus',
-                () => {
-                    try {
-                    } catch (error) {
+                "focus",
+                () =>
+                {
+                    try
+                    {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'ViagemUpsert.js',
-                            'callback@duracaoInput.addEventListener#1',
+                            "ViagemUpsert.js",
+                            "callback@duracaoInput.addEventListener#1",
                             error,
                         );
                     }
@@ -2889,16 +2898,21 @@
 
             );
         }
-        const percorridoInput = document.getElementById('txtKmPercorrido');
-        if (percorridoInput) {
+        const percorridoInput = document.getElementById("txtKmPercorrido");
+        if (percorridoInput)
+        {
             percorridoInput.addEventListener(
-                'focus',
-                () => {
-                    try {
-                    } catch (error) {
+                "focus",
+                () =>
+                {
+                    try
+                    {
+                    }
+                    catch (error)
+                    {
                         Alerta.TratamentoErroComLinha(
-                            'ViagemUpsert.js',
-                            'callback@percorridoInput.addEventListener#1',
+                            "ViagemUpsert.js",
+                            "callback@percorridoInput.addEventListener#1",
                             error,
                         );
                     }
@@ -2906,144 +2920,151 @@
 
             );
         }
-    } catch (error) {
-        TratamentoErroComLinha('ViagemUpsert.js', 'load.window', error);
+    }
+    catch (error)
+    {
+        TratamentoErroComLinha("ViagemUpsert.js", "load.window", error);
     }
 });
 
 var textBoxNoFichaVistoria = new ej.inputs.TextBox({
-    input: function (args) {
-        try {
+    input: function (args)
+    {
+        try
+        {
             const value = args.event.target.value;
 
-            args.event.target.value = value.replace(/[^\d-]/g, '');
-
-            if (
-                (value.match(/-/g) || []).length > 1 ||
-                value.indexOf('-') > 0
-            ) {
-                args.event.target.value = value.replace(/-/g, '');
+            args.event.target.value = value.replace(/[^\d-]/g, "");
+
+            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
+            {
+                args.event.target.value = value.replace(/-/g, "");
             }
 
             const num = parseInt(args.event.target.value, 10);
-            if (!isNaN(num)) {
-                if (num > 2147483647) {
-                    args.event.target.value = '2147483647';
-                } else if (num < -2147483648) {
-                    args.event.target.value = '-2147483648';
-                }
-            }
-        } catch (error) {
+            if (!isNaN(num))
+            {
+                if (num > 2147483647)
+                {
+                    args.event.target.value = "2147483647";
+                } else if (num < -2147483648)
+                {
+                    args.event.target.value = "-2147483648";
+                }
+            }
+        }
+        catch (error)
+        {
             TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'textBoxNoFichaVistoria.input',
+                "ViagemUpsert.js",
+                "textBoxNoFichaVistoria.input",
                 error,
             );
         }
     },
 });
-textBoxNoFichaVistoria.appendTo('#txtNoFichaVistoria');
+textBoxNoFichaVistoria.appendTo("#txtNoFichaVistoria");
 
 var textBoxKmInicial = new ej.inputs.TextBox({
-    input: function (args) {
-        try {
+    input: function (args)
+    {
+        try
+        {
             const value = args.event.target.value;
 
-            args.event.target.value = value.replace(/[^\d-]/g, '');
-
-            if (
-                (value.match(/-/g) || []).length > 1 ||
-                value.indexOf('-') > 0
-            ) {
-                args.event.target.value = value.replace(/-/g, '');
+            args.event.target.value = value.replace(/[^\d-]/g, "");
+
+            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
+            {
+                args.event.target.value = value.replace(/-/g, "");
             }
 
             const num = parseInt(args.event.target.value, 10);
-            if (!isNaN(num)) {
-                if (num > 2147483647) {
-                    args.event.target.value = '2147483647';
-                } else if (num < -2147483648) {
-                    args.event.target.value = '-2147483648';
-                }
-            }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'textBoxKmInicial.input',
-                error,
-            );
+            if (!isNaN(num))
+            {
+                if (num > 2147483647)
+                {
+                    args.event.target.value = "2147483647";
+                } else if (num < -2147483648)
+                {
+                    args.event.target.value = "-2147483648";
+                }
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("ViagemUpsert.js", "textBoxKmInicial.input", error);
         }
     },
 });
-textBoxKmInicial.appendTo('#txtKmInicial');
+textBoxKmInicial.appendTo("#txtKmInicial");
 
 var textBoxKmFinal = new ej.inputs.TextBox({
-    input: function (args) {
-        try {
+    input: function (args)
+    {
+        try
+        {
             const value = args.event.target.value;
 
-            args.event.target.value = value.replace(/[^\d-]/g, '');
-
-            if (
-                (value.match(/-/g) || []).length > 1 ||
-                value.indexOf('-') > 0
-            ) {
-                args.event.target.value = value.replace(/-/g, '');
+            args.event.target.value = value.replace(/[^\d-]/g, "");
+
+            if ((value.match(/-/g) || []).length > 1 || value.indexOf("-") > 0)
+            {
+                args.event.target.value = value.replace(/-/g, "");
             }
 
             const num = parseInt(args.event.target.value, 10);
-            if (!isNaN(num)) {
-                if (num > 2147483647) {
-                    args.event.target.value = '2147483647';
-                } else if (num < -2147483648) {
-                    args.event.target.value = '-2147483648';
-                }
-            }
-        } catch (error) {
-            TratamentoErroComLinha(
-                'ViagemUpsert.js',
-                'textBoxKmFinal.input',
-                error,
-            );
+            if (!isNaN(num))
+            {
+                if (num > 2147483647)
+                {
+                    args.event.target.value = "2147483647";
+                } else if (num < -2147483648)
+                {
+                    args.event.target.value = "-2147483648";
+                }
+            }
+        }
+        catch (error)
+        {
+            TratamentoErroComLinha("ViagemUpsert.js", "textBoxKmFinal.input", error);
         }
     },
 });
-textBoxKmFinal.appendTo('#txtKmFinal');
-
-(function () {
-
-    function ensureTooltip(el, globalName) {
-
-        if (
-            globalName &&
-            window[globalName] &&
-            typeof window[globalName].open === 'function'
-        ) {
+textBoxKmFinal.appendTo("#txtKmFinal");
+
+(function ()
+{
+
+    function ensureTooltip(el, globalName)
+    {
+
+        if (globalName && window[globalName] && typeof window[globalName].open === 'function')
+        {
             return window[globalName];
         }
 
-        if (el && el.ej2_instances && el.ej2_instances.length) {
-            for (var i = 0; i < el.ej2_instances.length; i++) {
+        if (el && el.ej2_instances && el.ej2_instances.length)
+        {
+            for (var i = 0; i < el.ej2_instances.length; i++)
+            {
                 var inst = el.ej2_instances[i];
-                if (
-                    inst &&
-                    typeof inst.open === 'function' &&
-                    typeof inst.close === 'function'
-                ) {
+                if (inst && typeof inst.open === 'function' && typeof inst.close === 'function')
+                {
                     if (globalName) window[globalName] = inst;
                     return inst;
                 }
             }
         }
 
-        if (window.ej && ej.popups && typeof ej.popups.Tooltip === 'function') {
+        if (window.ej && ej.popups && typeof ej.popups.Tooltip === 'function')
+        {
             var Tooltip = ej.popups.Tooltip;
-            var content =
-                el.getAttribute('data-ejtip') || 'Valor acima do limite.';
+            var content = el.getAttribute('data-ejtip') || 'Valor acima do limite.';
             var inst = new Tooltip({
                 content: content,
                 opensOn: 'Custom',
-                position: 'TopCenter',
+                position: 'TopCenter'
             });
             inst.appendTo(el);
             if (globalName) window[globalName] = inst;
@@ -3052,69 +3073,65 @@
         return null;
     }
 
-    function setInvalid(el, invalid) {
+    function setInvalid(el, invalid)
+    {
         if (!el) return;
 
-        if (el.classList) {
+        if (el.classList)
+        {
             el.classList.toggle('is-invalid', !!invalid);
-        } else {
+        } else
+        {
             var cls = el.className || '';
             var has = /\bis-invalid\b/.test(cls);
             if (invalid && !has) el.className = (cls + ' is-invalid').trim();
-            if (!invalid && has)
-                el.className = cls
-                    .replace(/\bis-invalid\b/, '')
-                    .replace(/\s{2,}/g, ' ')
-                    .trim();
-        }
-        try {
-            el.setAttribute('aria-invalid', String(!!invalid));
-        } catch (e) {}
-
-        try {
-            el.style.color = invalid ? 'var(--ftx-invalid, #dc3545)' : 'black';
-        } catch (e) {}
-
-        try {
-            var wrapper = el.closest(
-                '.e-input-group, .e-float-input, .e-control-wrapper',
-            );
-            if (wrapper && wrapper.classList) {
+            if (!invalid && has) el.className = cls.replace(/\bis-invalid\b/, '').replace(/\s{2,}/g, ' ').trim();
+        }
+        try { el.setAttribute('aria-invalid', String(!!invalid)); } catch (e) { }
+
+        try { el.style.color = invalid ? 'var(--ftx-invalid, #dc3545)' : 'black'; } catch (e) { }
+
+        try
+        {
+            var wrapper = el.closest('.e-input-group, .e-float-input, .e-control-wrapper');
+            if (wrapper && wrapper.classList)
+            {
                 wrapper.classList.toggle('is-invalid', !!invalid);
             }
-        } catch (e) {
-
-        }
-    }
-
-    function setHigh(el, high) {
+        } catch (e) { }
+    }
+
+    function setHigh(el, high)
+    {
         if (!el) return;
-        if (el.classList) {
+        if (el.classList)
+        {
             el.classList.toggle('is-high', !!high);
-        } else {
+        } else
+        {
             var cls = el.className || '';
             var has = /\bis-high\b/.test(cls);
             if (high && !has) el.className = (cls + ' is-high').trim();
-            if (!high && has)
-                el.className = cls
-                    .replace(/\bis-high\b/, '')
-                    .replace(/\s{2,}/g, ' ')
-                    .trim();
-        }
-    }
-
-    function tooltipOnTransition(el, condition, ms, globalName) {
+            if (!high && has) el.className = cls.replace(/\bis-high\b/, '').replace(/\s{2,}/g, ' ').trim();
+        }
+    }
+
+    function tooltipOnTransition(el, condition, ms, globalName)
+    {
         if (!el) return;
         var key = '_prevCond_' + (globalName || 'tt');
         var prev = !!el[key];
         var now = !!condition;
 
-        if (now && !prev) {
+        if (now && !prev)
+        {
             var tip = ensureTooltip(el, globalName);
-            if (tip && typeof tip.open === 'function') {
+            if (tip && typeof tip.open === 'function')
+            {
                 tip.open(el);
                 clearTimeout(el._tipTimer);
-                el._tipTimer = setTimeout(function () {
+                el._tipTimer = setTimeout(function ()
+                {
                     if (tip && typeof tip.close === 'function') tip.close();
                 }, ms || 1000);
             }
@@ -3126,29 +3143,34 @@
         ensureTooltip: ensureTooltip,
         setInvalid: setInvalid,
         setHigh: setHigh,
-        tooltipOnTransition: tooltipOnTransition,
+        tooltipOnTransition: tooltipOnTransition
     };
 })();
 
-(function () {
+(function ()
+{
     const modalEl = document.getElementById('modalZoom');
     const viewer = document.getElementById('imgViewer');
     const zoomed = document.getElementById('imgZoomed');
 
     if (!modalEl) return;
 
-    modalEl.addEventListener('show.bs.modal', function () {
+    modalEl.addEventListener('show.bs.modal', function ()
+    {
         if (viewer && zoomed) zoomed.src = viewer.getAttribute('src') || '';
     });
 })();
 
-function salvarFormulario() {
+function salvarFormulario()
+{
     const fileInput = document.getElementById('txtFile');
 
-    if (fileInput.files && fileInput.files[0]) {
+    if (fileInput.files && fileInput.files[0])
+    {
         const reader = new FileReader();
 
-        reader.onload = function (e) {
+        reader.onload = function (e)
+        {
 
             $('#hiddenFoto').val(e.target.result);
 
@@ -3156,7 +3178,8 @@
         };
 
         reader.readAsDataURL(fileInput.files[0]);
-    } else {
+    } else
+    {
 
         $('form').submit();
     }
@@ -3167,221 +3190,217 @@
 var _veiculoIdSelecionado = null;
 var _qtdOcorrenciasVeiculo = 0;
 
-function verificarOcorrenciasVeiculo(veiculoId) {
-    try {
+function verificarOcorrenciasVeiculo(veiculoId)
+{
+    try
+    {
         _veiculoIdSelecionado = veiculoId;
 
         $.ajax({
             url: '/api/OcorrenciaViagem/VerificarOcorrenciasVeiculo',
             type: 'GET',
             data: { veiculoId: veiculoId },
-            success: function (response) {
-                try {
-                    if (response.success) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
                         _qtdOcorrenciasVeiculo = response.quantidade || 0;
 
-                        if (response.temOcorrencias) {
-                            habilitarBotaoOcorrenciasVeiculo(
-                                response.quantidade,
-                            );
-                        } else {
+                        if (response.temOcorrencias)
+                        {
+                            habilitarBotaoOcorrenciasVeiculo(response.quantidade);
+                        }
+                        else
+                        {
                             desabilitarBotaoOcorrenciasVeiculo();
                         }
-                    } else {
+                    }
+                    else
+                    {
                         desabilitarBotaoOcorrenciasVeiculo();
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'verificarOcorrenciasVeiculo.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarOcorrenciasVeiculo.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao verificar ocorrências:', error);
                 desabilitarBotaoOcorrenciasVeiculo();
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'verificarOcorrenciasVeiculo',
-            error,
-        );
-    }
-}
-
-function habilitarBotaoOcorrenciasVeiculo(quantidade) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarOcorrenciasVeiculo", error);
+    }
+}
+
+function habilitarBotaoOcorrenciasVeiculo(quantidade)
+{
+    try
+    {
         const $btn = $('#btnOcorrenciasVeiculo');
-        if ($btn.length) {
+        if ($btn.length)
+        {
             $btn.removeClass('disabled').prop('disabled', false);
             $btn.attr('title', `${quantidade} ocorrência(s) em aberto`);
 
             const $badge = $('#badgeOcorrenciasVeiculo');
-            if ($badge.length) {
+            if ($badge.length)
+            {
                 $badge.text(quantidade).show();
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'habilitarBotaoOcorrenciasVeiculo',
-            error,
-        );
-    }
-}
-
-function desabilitarBotaoOcorrenciasVeiculo() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "habilitarBotaoOcorrenciasVeiculo", error);
+    }
+}
+
+function desabilitarBotaoOcorrenciasVeiculo()
+{
+    try
+    {
         _qtdOcorrenciasVeiculo = 0;
 
         const $btn = $('#btnOcorrenciasVeiculo');
-        if ($btn.length) {
+        if ($btn.length)
+        {
             $btn.addClass('disabled').prop('disabled', true);
             $btn.attr('title', 'Nenhuma ocorrência em aberto');
 
             const $badge = $('#badgeOcorrenciasVeiculo');
-            if ($badge.length) {
+            if ($badge.length)
+            {
                 $badge.hide();
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'desabilitarBotaoOcorrenciasVeiculo',
-            error,
-        );
-    }
-}
-
-$(document).on('click', '#btnOcorrenciasVeiculo:not(.disabled)', function (e) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "desabilitarBotaoOcorrenciasVeiculo", error);
+    }
+}
+
+$(document).on('click', '#btnOcorrenciasVeiculo:not(.disabled)', function (e)
+{
+    try
+    {
         e.preventDefault();
 
-        if (!_veiculoIdSelecionado) {
+        if (!_veiculoIdSelecionado)
+        {
             AppToast.show('Amarelo', 'Selecione um veículo primeiro', 3000);
             return;
         }
 
-        const modalEl = document.getElementById(
-            'modalOcorrenciasVeiculoUpsert',
-        );
+        const modalEl = document.getElementById('modalOcorrenciasVeiculoUpsert');
         if (!modalEl) return;
 
         modalEl.setAttribute('data-veiculo-id', String(_veiculoIdSelecionado));
 
-        const ddTreeObj =
-            document.getElementById('cmbVeiculo').ej2_instances[0];
+        const ddTreeObj = document.getElementById("cmbVeiculo").ej2_instances[0];
         const textoVeiculo = ddTreeObj.text || 'Veículo';
 
-        const tituloSpan = modalEl.querySelector(
-            '#modalOcorrenciasVeiculoUpsertLabel span',
-        );
-        if (tituloSpan) {
+        const tituloSpan = modalEl.querySelector('#modalOcorrenciasVeiculoUpsertLabel span');
+        if (tituloSpan)
+        {
             tituloSpan.textContent = `Ocorrências em Aberto - ${textoVeiculo}`;
         }
 
         const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btnOcorrenciasVeiculo',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnOcorrenciasVeiculo", error);
     }
 });
 
-$('#modalOcorrenciasVeiculoUpsert').on('shown.bs.modal', function (e) {
-    try {
+$('#modalOcorrenciasVeiculoUpsert').on('shown.bs.modal', function (e)
+{
+    try
+    {
         const modalEl = this;
         const veiculoId = modalEl.getAttribute('data-veiculo-id');
 
-        if (!veiculoId) {
+        if (!veiculoId)
+        {
             console.error('VeiculoId não encontrado');
             return;
         }
 
-        $('#tblOcorrenciasVeiculoUpsert tbody').html(
-            '<tr><td colspan="6" class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</td></tr>',
-        );
+        $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center"><i class="fa fa-spinner fa-spin"></i> Carregando...</td></tr>');
 
         carregarOcorrenciasVeiculoUpsert(veiculoId);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'modalOcorrenciasVeiculoUpsert.shown',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalOcorrenciasVeiculoUpsert.shown", error);
     }
 });
 
-function carregarOcorrenciasVeiculoUpsert(veiculoId) {
-    try {
+function carregarOcorrenciasVeiculoUpsert(veiculoId)
+{
+    try
+    {
         $.ajax({
             url: '/api/OcorrenciaViagem/ListarOcorrenciasVeiculo',
             type: 'GET',
             data: { veiculoId: veiculoId },
-            success: function (response) {
-                try {
-                    if (
-                        response.success &&
-                        response.data &&
-                        response.data.length > 0
-                    ) {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success && response.data && response.data.length > 0)
+                    {
                         renderizarTabelaOcorrenciasVeiculoUpsert(response.data);
-                    } else {
-                        $('#tblOcorrenciasVeiculoUpsert tbody').html(
-                            '<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>',
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'carregarOcorrenciasVeiculoUpsert.success',
-                        error,
-                    );
+                    else
+                    {
+                        $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasVeiculoUpsert.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao carregar ocorrências:', error);
-                $('#tblOcorrenciasVeiculoUpsert tbody').html(
-                    '<tr><td colspan="6" class="text-center text-danger">Erro ao carregar ocorrências</td></tr>',
-                );
-                AppToast.show(
-                    'Vermelho',
-                    'Erro ao carregar ocorrências do veículo',
-                    4000,
-                );
-            },
+                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-danger">Erro ao carregar ocorrências</td></tr>');
+                AppToast.show('Vermelho', 'Erro ao carregar ocorrências do veículo', 4000);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarOcorrenciasVeiculoUpsert',
-            error,
-        );
-    }
-}
-
-function renderizarTabelaOcorrenciasVeiculoUpsert(ocorrencias) {
-    try {
-        if (!ocorrencias || ocorrencias.length === 0) {
-            $('#tblOcorrenciasVeiculoUpsert tbody').html(
-                '<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>',
-            );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasVeiculoUpsert", error);
+    }
+}
+
+function renderizarTabelaOcorrenciasVeiculoUpsert(ocorrencias)
+{
+    try
+    {
+        if (!ocorrencias || ocorrencias.length === 0)
+        {
+            $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
             return;
         }
 
         let html = '';
-        ocorrencias.forEach(function (oc, index) {
-            const dataFormatada = oc.dataCriacao
-                ? new Date(oc.dataCriacao).toLocaleDateString('pt-BR')
-                : '-';
-            const temImagem =
-                oc.imagemOcorrencia && oc.imagemOcorrencia.trim() !== '';
+        ocorrencias.forEach(function (oc, index)
+        {
+            const dataFormatada = oc.dataCriacao ? new Date(oc.dataCriacao).toLocaleDateString('pt-BR') : '-';
+            const temImagem = oc.imagemOcorrencia && oc.imagemOcorrencia.trim() !== '';
             const statusOc = oc.statusOcorrencia;
             const statusStr = oc.status || '';
             const itemManutId = oc.itemManutencaoId;
@@ -3389,16 +3408,18 @@
             let statusFinal = 'Aberta';
             let badgeClass = 'ftx-ocorrencia-badge-aberta';
 
-            if (statusStr === 'Pendente') {
+            if (statusStr === 'Pendente')
+            {
                 statusFinal = 'Pendente';
                 badgeClass = 'ftx-ocorrencia-badge-pendente';
-            } else if (statusStr === 'Baixada' || statusOc === false) {
+            }
+            else if (statusStr === 'Baixada' || statusOc === false)
+            {
                 statusFinal = 'Baixada';
                 badgeClass = 'ftx-ocorrencia-badge-baixada';
-            } else if (
-                itemManutId &&
-                itemManutId !== '00000000-0000-0000-0000-000000000000'
-            ) {
+            }
+            else if (itemManutId && itemManutId !== '00000000-0000-0000-0000-000000000000')
+            {
                 statusFinal = 'Manutenção';
                 badgeClass = 'ftx-ocorrencia-badge-manutencao';
             }
@@ -3439,107 +3460,102 @@
         });
 
         $('#tblOcorrenciasVeiculoUpsert tbody').html(html);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'renderizarTabelaOcorrenciasVeiculoUpsert',
-            error,
-        );
-    }
-}
-
-$(document).on('click', '.btn-excluir-ocorrencia-upsert', function (e) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "renderizarTabelaOcorrenciasVeiculoUpsert", error);
+    }
+}
+
+$(document).on('click', '.btn-excluir-ocorrencia-upsert', function (e)
+{
+    try
+    {
         e.preventDefault();
         const ocorrenciaId = $(this).data('id');
         const $btn = $(this);
         const $row = $btn.closest('tr');
 
         Alerta.Confirmar(
-            'Deseja realmente excluir esta ocorrência?',
-            'Esta ação não poderá ser desfeita!',
-            'Sim, excluir',
-            'Cancelar',
-        ).then((confirmado) => {
-            if (confirmado) {
+            "Deseja realmente excluir esta ocorrência?",
+            "Esta ação não poderá ser desfeita!",
+            "Sim, excluir",
+            "Cancelar"
+        ).then((confirmado) =>
+        {
+            if (confirmado)
+            {
                 excluirOcorrenciaVeiculoUpsert(ocorrenciaId, $row);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btn-excluir-ocorrencia-upsert',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btn-excluir-ocorrencia-upsert", error);
     }
 });
 
-function excluirOcorrenciaVeiculoUpsert(ocorrenciaId, $row) {
-    try {
+function excluirOcorrenciaVeiculoUpsert(ocorrenciaId, $row)
+{
+    try
+    {
         $.ajax({
             url: '/api/OcorrenciaViagem/ExcluirOcorrencia',
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify({ ocorrenciaViagemId: ocorrenciaId }),
-            success: function (response) {
-                try {
-                    if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            'Ocorrência excluída com sucesso',
-                            3000,
-                        );
-                        $row.fadeOut(300, function () {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+                        AppToast.show('Verde', 'Ocorrência excluída com sucesso', 3000);
+                        $row.fadeOut(300, function ()
+                        {
                             $(this).remove();
 
                             _qtdOcorrenciasVeiculo--;
 
-                            if (
-                                $('#tblOcorrenciasVeiculoUpsert tbody tr')
-                                    .length === 0
-                            ) {
-                                $('#tblOcorrenciasVeiculoUpsert tbody').html(
-                                    '<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>',
-                                );
+                            if ($('#tblOcorrenciasVeiculoUpsert tbody tr').length === 0)
+                            {
+                                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
                                 desabilitarBotaoOcorrenciasVeiculo();
-                            } else {
-
-                                habilitarBotaoOcorrenciasVeiculo(
-                                    _qtdOcorrenciasVeiculo,
-                                );
+                            }
+                            else
+                            {
+
+                                habilitarBotaoOcorrenciasVeiculo(_qtdOcorrenciasVeiculo);
                             }
                         });
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao excluir ocorrência',
-                            4000,
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'excluirOcorrenciaVeiculoUpsert.success',
-                        error,
-                    );
+                    else
+                    {
+                        AppToast.show('Vermelho', response.message || 'Erro ao excluir ocorrência', 4000);
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "excluirOcorrenciaVeiculoUpsert.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao excluir ocorrência:', error);
                 AppToast.show('Vermelho', 'Erro ao excluir ocorrência', 4000);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'excluirOcorrenciaVeiculoUpsert',
-            error,
-        );
-    }
-}
-
-$(document).on('click', '.btn-baixar-ocorrencia-upsert', async function (e) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "excluirOcorrenciaVeiculoUpsert", error);
+    }
+}
+
+$(document).on('click', '.btn-baixar-ocorrencia-upsert', async function (e)
+{
+    try
+    {
         e.preventDefault();
         const ocorrenciaId = $(this).data('id');
         const resumo = $(this).data('resumo') || 'esta ocorrência';
@@ -3547,96 +3563,92 @@
         const $row = $btn.closest('tr');
 
         const confirmaBaixa = await Alerta.Confirmar(
-            'Dar Baixa na Ocorrência?',
+            "Dar Baixa na Ocorrência?",
             `Deseja dar baixa em: "${resumo}"?`,
-            'Sim, dar baixa',
-            'Cancelar',
+            "Sim, dar baixa",
+            "Cancelar"
         );
 
-        if (!confirmaBaixa) {
+        if (!confirmaBaixa)
+        {
             return;
         }
 
         const querSolucao = await Alerta.Confirmar(
-            'Adicionar Solução?',
-            'Deseja informar a solução aplicada para esta ocorrência?',
-            'Sim, informar',
-            'Não, baixar sem solução',
+            "Adicionar Solução?",
+            "Deseja informar a solução aplicada para esta ocorrência?",
+            "Sim, informar",
+            "Não, baixar sem solução"
         );
 
-        if (querSolucao) {
+        if (querSolucao)
+        {
 
             $('#hiddenOcorrenciaIdSolucaoUpsert').val(ocorrenciaId);
             $('#txtSolucaoOcorrenciaUpsert').val('');
 
-            const modalSolucao = new bootstrap.Modal(
-                document.getElementById('modalSolucaoOcorrenciaUpsert'),
-            );
+            const modalSolucao = new bootstrap.Modal(document.getElementById('modalSolucaoOcorrenciaUpsert'));
             modalSolucao.show();
 
-            $('#modalSolucaoOcorrenciaUpsert').one(
-                'shown.bs.modal',
-                function () {
-                    $('#txtSolucaoOcorrenciaUpsert').focus();
-                },
-            );
-        } else {
+            $('#modalSolucaoOcorrenciaUpsert').one('shown.bs.modal', function () {
+                $('#txtSolucaoOcorrenciaUpsert').focus();
+            });
+        }
+        else
+        {
 
             baixarOcorrenciaVeiculoUpsert(ocorrenciaId, null, $row);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btn-baixar-ocorrencia-upsert',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btn-baixar-ocorrencia-upsert", error);
     }
 });
 
-$(document).on('click', '#btnConfirmarSolucaoUpsert', function (e) {
-    try {
+$(document).on('click', '#btnConfirmarSolucaoUpsert', function (e)
+{
+    try
+    {
         e.preventDefault();
 
         const ocorrenciaId = $('#hiddenOcorrenciaIdSolucaoUpsert').val();
         const solucao = $('#txtSolucaoOcorrenciaUpsert').val().trim();
 
-        if (!solucao) {
-            AppToast.show(
-                'Amarelo',
-                'Por favor, informe a solução aplicada',
-                3000,
-            );
+        if (!solucao)
+        {
+            AppToast.show('Amarelo', 'Por favor, informe a solução aplicada', 3000);
             $('#txtSolucaoOcorrenciaUpsert').focus();
             return;
         }
 
         const modalEl = document.getElementById('modalSolucaoOcorrenciaUpsert');
         const modalInstance = bootstrap.Modal.getInstance(modalEl);
-        if (modalInstance) {
+        if (modalInstance)
+        {
             modalInstance.hide();
         }
 
-        const $row = $(
-            `.btn-baixar-ocorrencia-upsert[data-id="${ocorrenciaId}"]`,
-        ).closest('tr');
+        const $row = $(`.btn-baixar-ocorrencia-upsert[data-id="${ocorrenciaId}"]`).closest('tr');
 
         baixarOcorrenciaVeiculoUpsert(ocorrenciaId, solucao, $row);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'click.btnConfirmarSolucaoUpsert',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "click.btnConfirmarSolucaoUpsert", error);
     }
 });
 
-function baixarOcorrenciaVeiculoUpsert(ocorrenciaId, solucao, $row) {
-    try {
+function baixarOcorrenciaVeiculoUpsert(ocorrenciaId, solucao, $row)
+{
+    try
+    {
         const payload = {
-            OcorrenciaViagemId: ocorrenciaId,
+            OcorrenciaViagemId: ocorrenciaId
         };
 
-        if (solucao) {
+        if (solucao)
+        {
             payload.SolucaoOcorrencia = solucao;
         }
 
@@ -3645,147 +3657,147 @@
             type: 'POST',
             contentType: 'application/json',
             data: JSON.stringify(payload),
-            success: function (response) {
-                try {
-                    if (response.success) {
-                        AppToast.show(
-                            'Verde',
-                            'Ocorrência baixada com sucesso',
-                            3000,
-                        );
-                        $row.fadeOut(300, function () {
+            success: function (response)
+            {
+                try
+                {
+                    if (response.success)
+                    {
+                        AppToast.show('Verde', 'Ocorrência baixada com sucesso', 3000);
+                        $row.fadeOut(300, function ()
+                        {
                             $(this).remove();
 
                             _qtdOcorrenciasVeiculo--;
 
-                            if (
-                                $('#tblOcorrenciasVeiculoUpsert tbody tr')
-                                    .length === 0
-                            ) {
-                                $('#tblOcorrenciasVeiculoUpsert tbody').html(
-                                    '<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>',
-                                );
+                            if ($('#tblOcorrenciasVeiculoUpsert tbody tr').length === 0)
+                            {
+                                $('#tblOcorrenciasVeiculoUpsert tbody').html('<tr><td colspan="6" class="text-center text-muted">Nenhuma ocorrência em aberto</td></tr>');
                                 desabilitarBotaoOcorrenciasVeiculo();
-                            } else {
-
-                                habilitarBotaoOcorrenciasVeiculo(
-                                    _qtdOcorrenciasVeiculo,
-                                );
+                            }
+                            else
+                            {
+
+                                habilitarBotaoOcorrenciasVeiculo(_qtdOcorrenciasVeiculo);
                             }
                         });
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            response.message || 'Erro ao baixar ocorrência',
-                            4000,
-                        );
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'baixarOcorrenciaVeiculoUpsert.success',
-                        error,
-                    );
+                    else
+                    {
+                        AppToast.show('Vermelho', response.message || 'Erro ao baixar ocorrência', 4000);
+                    }
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "baixarOcorrenciaVeiculoUpsert.success", error);
                 }
             },
-            error: function (xhr, status, error) {
+            error: function (xhr, status, error)
+            {
                 console.error('Erro ao baixar ocorrência:', error);
                 AppToast.show('Vermelho', 'Erro ao baixar ocorrência', 4000);
-            },
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'baixarOcorrenciaVeiculoUpsert',
-            error,
-        );
-    }
-}
-
-$('#modalSolucaoOcorrenciaUpsert').on('hidden.bs.modal', function () {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "baixarOcorrenciaVeiculoUpsert", error);
+    }
+}
+
+$('#modalSolucaoOcorrenciaUpsert').on('hidden.bs.modal', function ()
+{
+    try
+    {
         $('#hiddenOcorrenciaIdSolucaoUpsert').val('');
         $('#txtSolucaoOcorrenciaUpsert').val('');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'modalSolucaoOcorrenciaUpsert.hidden',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalSolucaoOcorrenciaUpsert.hidden", error);
     }
 });
 
-$('#modalOcorrenciasVeiculoUpsert').on('hidden.bs.modal', function () {
-    try {
+$('#modalOcorrenciasVeiculoUpsert').on('hidden.bs.modal', function ()
+{
+    try
+    {
         this.removeAttribute('data-veiculo-id');
         $('#tblOcorrenciasVeiculoUpsert tbody').html('');
 
-        const tituloSpan = this.querySelector(
-            '#modalOcorrenciasVeiculoUpsertLabel span',
-        );
-        if (tituloSpan) {
+        const tituloSpan = this.querySelector('#modalOcorrenciasVeiculoUpsertLabel span');
+        if (tituloSpan)
+        {
             tituloSpan.textContent = 'Ocorrências em Aberto do Veículo';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'modalOcorrenciasVeiculoUpsert.hidden',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "modalOcorrenciasVeiculoUpsert.hidden", error);
     }
 });
 
 let _dadosMobile = null;
 let _ocorrenciasViagem = [];
 
-function formatarHora(valor) {
-    try {
+function formatarHora(valor)
+{
+    try
+    {
         if (!valor) return '';
 
         if (/^\d{2}:\d{2}$/.test(valor)) return valor;
 
-        if (/^\d{2}:\d{2}:\d{2}$/.test(valor)) {
+        if (/^\d{2}:\d{2}:\d{2}$/.test(valor))
+        {
             return valor.substring(0, 5);
         }
 
         const partes = valor.split(':');
-        if (partes.length >= 2) {
+        if (partes.length >= 2)
+        {
             const horas = partes[0].padStart(2, '0');
             const minutos = partes[1].padStart(2, '0');
             return `${horas}:${minutos}`;
         }
 
         return valor;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha('ViagemUpsert.js', 'formatarHora', error);
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "formatarHora", error);
         return valor;
     }
 }
 
-function aplicarFormatacaoHoras() {
-    try {
+function aplicarFormatacaoHoras()
+{
+    try
+    {
         const txtHoraInicial = document.getElementById('txtHoraInicial');
         const txtHoraFinal = document.getElementById('txtHoraFinal');
 
-        if (txtHoraInicial && txtHoraInicial.value) {
+        if (txtHoraInicial && txtHoraInicial.value)
+        {
             txtHoraInicial.value = formatarHora(txtHoraInicial.value);
         }
 
-        if (txtHoraFinal && txtHoraFinal.value) {
+        if (txtHoraFinal && txtHoraFinal.value)
+        {
             txtHoraFinal.value = formatarHora(txtHoraFinal.value);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'aplicarFormatacaoHoras',
-            error,
-        );
-    }
-}
-
-function configurarCampoNoFichaVistoria() {
-
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "aplicarFormatacaoHoras", error);
+    }
+}
+
+function configurarCampoNoFichaVistoria()
+{
+
+    try
+    {
         const txtNumerico = document.getElementById('txtNoFichaVistoria');
         const txtMobile = document.getElementById('txtNoFichaVistoriaMobile');
 
@@ -3793,47 +3805,48 @@
 
         const wrapperNumerico = txtNumerico.closest('.e-input-group');
 
-        if (wrapperNumerico) {
-
-            if (txtNumerico.style.display === 'none') {
+        if (wrapperNumerico)
+        {
+
+            if (txtNumerico.style.display === 'none')
+            {
                 wrapperNumerico.style.display = 'none';
-            } else {
+            }
+            else
+            {
                 wrapperNumerico.style.display = '';
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'configurarCampoNoFichaVistoria',
-            error,
-        );
-    }
-}
-
-async function carregarDadosMobile() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "configurarCampoNoFichaVistoria", error);
+    }
+}
+
+async function carregarDadosMobile()
+{
+    try
+    {
 
         const secaoMobile = document.getElementById('secaoMobile');
-        if (!secaoMobile || secaoMobile.style.display === 'none') {
-
-            return;
-        }
-
-        if (
-            !window.viagemId ||
-            window.viagemId === '' ||
-            window.viagemId === '00000000-0000-0000-0000-000000000000'
-        ) {
-
-            return;
-        }
-
-        const response = await fetch(
-            `/api/Viagem/ObterDadosMobile?viagemId=${window.viagemId}`,
-        );
+        if (!secaoMobile || secaoMobile.style.display === 'none')
+        {
+
+            return;
+        }
+
+        if (!window.viagemId || window.viagemId === '' || window.viagemId === '00000000-0000-0000-0000-000000000000')
+        {
+
+            return;
+        }
+
+        const response = await fetch(`/api/Viagem/ObterDadosMobile?viagemId=${window.viagemId}`);
         const data = await response.json();
 
-        if (data.success && data.isMobile) {
+        if (data.success && data.isMobile)
+        {
             _dadosMobile = data;
 
             carregarRubricas(data);
@@ -3842,34 +3855,41 @@
 
             carregarOcorrenciasViagem(data.ocorrencias);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarDadosMobile',
-            error,
-        );
-    }
-}
-
-function carregarRubricas(data) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarDadosMobile", error);
+    }
+}
+
+function carregarRubricas(data)
+{
+    try
+    {
 
         const imgRubricaInicial = document.getElementById('imgRubricaInicial');
         const semRubricaInicial = document.getElementById('semRubricaInicial');
 
-        if (data.temRubricaInicial && data.rubricaInicial) {
-            if (imgRubricaInicial) {
+        if (data.temRubricaInicial && data.rubricaInicial)
+        {
+            if (imgRubricaInicial)
+            {
                 imgRubricaInicial.src = data.rubricaInicial;
                 imgRubricaInicial.style.display = 'block';
             }
-            if (semRubricaInicial) {
+            if (semRubricaInicial)
+            {
                 semRubricaInicial.style.display = 'none';
             }
-        } else {
-            if (imgRubricaInicial) {
+        }
+        else
+        {
+            if (imgRubricaInicial)
+            {
                 imgRubricaInicial.style.display = 'none';
             }
-            if (semRubricaInicial) {
+            if (semRubricaInicial)
+            {
                 semRubricaInicial.style.display = 'block';
             }
         }
@@ -3877,99 +3897,71 @@
         const imgRubricaFinal = document.getElementById('imgRubricaFinal');
         const semRubricaFinal = document.getElementById('semRubricaFinal');
 
-        if (data.temRubricaFinal && data.rubricaFinal) {
-            if (imgRubricaFinal) {
+        if (data.temRubricaFinal && data.rubricaFinal)
+        {
+            if (imgRubricaFinal)
+            {
                 imgRubricaFinal.src = data.rubricaFinal;
                 imgRubricaFinal.style.display = 'block';
             }
-            if (semRubricaFinal) {
+            if (semRubricaFinal)
+            {
                 semRubricaFinal.style.display = 'none';
             }
-        } else {
-            if (imgRubricaFinal) {
+        }
+        else
+        {
+            if (imgRubricaFinal)
+            {
                 imgRubricaFinal.style.display = 'none';
             }
-            if (semRubricaFinal) {
+            if (semRubricaFinal)
+            {
                 semRubricaFinal.style.display = 'block';
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarRubricas',
-            error,
-        );
-    }
-}
-
-function carregarDocumentosItensMobile(data) {
-    try {
-
-        const chkStatusDocumento = document.getElementById(
-            'chkStatusDocumentoMobile',
-        );
-        const chkStatusCartao = document.getElementById(
-            'chkStatusCartaoMobile',
-        );
-        const chkCintaEntregue = document.getElementById(
-            'chkCintaEntregueMobile',
-        );
-        const chkTabletEntregue = document.getElementById(
-            'chkTabletEntregueMobile',
-        );
-
-        if (chkStatusDocumento)
-            chkStatusDocumento.checked = !!(
-                data.statusDocumento && data.statusDocumento.trim() !== ''
-            );
-        if (chkStatusCartao)
-            chkStatusCartao.checked = !!(
-                data.statusCartaoAbastecimento &&
-                data.statusCartaoAbastecimento.trim() !== ''
-            );
-        if (chkCintaEntregue)
-            chkCintaEntregue.checked = data.cintaEntregue === true;
-        if (chkTabletEntregue)
-            chkTabletEntregue.checked = data.tabletEntregue === true;
-
-        const chkStatusDocumentoFinal = document.getElementById(
-            'chkStatusDocumentoFinalMobile',
-        );
-        const chkStatusCartaoFinal = document.getElementById(
-            'chkStatusCartaoFinalMobile',
-        );
-        const chkCintaDevolvida = document.getElementById(
-            'chkCintaDevolvidaMobile',
-        );
-        const chkTabletDevolvido = document.getElementById(
-            'chkTabletDevolvidoMobile',
-        );
-
-        if (chkStatusDocumentoFinal)
-            chkStatusDocumentoFinal.checked = !!(
-                data.statusDocumentoFinal &&
-                data.statusDocumentoFinal.trim() !== ''
-            );
-        if (chkStatusCartaoFinal)
-            chkStatusCartaoFinal.checked = !!(
-                data.statusCartaoAbastecimentoFinal &&
-                data.statusCartaoAbastecimentoFinal.trim() !== ''
-            );
-        if (chkCintaDevolvida)
-            chkCintaDevolvida.checked = data.cintaDevolvida === true;
-        if (chkTabletDevolvido)
-            chkTabletDevolvido.checked = data.tabletDevolvido === true;
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarDocumentosItensMobile',
-            error,
-        );
-    }
-}
-
-function carregarOcorrenciasViagem(ocorrencias) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarRubricas", error);
+    }
+}
+
+function carregarDocumentosItensMobile(data)
+{
+    try
+    {
+
+        const chkStatusDocumento = document.getElementById('chkStatusDocumentoMobile');
+        const chkStatusCartao = document.getElementById('chkStatusCartaoMobile');
+        const chkCintaEntregue = document.getElementById('chkCintaEntregueMobile');
+        const chkTabletEntregue = document.getElementById('chkTabletEntregueMobile');
+
+        if (chkStatusDocumento) chkStatusDocumento.checked = !!(data.statusDocumento && data.statusDocumento.trim() !== '');
+        if (chkStatusCartao) chkStatusCartao.checked = !!(data.statusCartaoAbastecimento && data.statusCartaoAbastecimento.trim() !== '');
+        if (chkCintaEntregue) chkCintaEntregue.checked = data.cintaEntregue === true;
+        if (chkTabletEntregue) chkTabletEntregue.checked = data.tabletEntregue === true;
+
+        const chkStatusDocumentoFinal = document.getElementById('chkStatusDocumentoFinalMobile');
+        const chkStatusCartaoFinal = document.getElementById('chkStatusCartaoFinalMobile');
+        const chkCintaDevolvida = document.getElementById('chkCintaDevolvidaMobile');
+        const chkTabletDevolvido = document.getElementById('chkTabletDevolvidoMobile');
+
+        if (chkStatusDocumentoFinal) chkStatusDocumentoFinal.checked = !!(data.statusDocumentoFinal && data.statusDocumentoFinal.trim() !== '');
+        if (chkStatusCartaoFinal) chkStatusCartaoFinal.checked = !!(data.statusCartaoAbastecimentoFinal && data.statusCartaoAbastecimentoFinal.trim() !== '');
+        if (chkCintaDevolvida) chkCintaDevolvida.checked = data.cintaDevolvida === true;
+        if (chkTabletDevolvido) chkTabletDevolvido.checked = data.tabletDevolvido === true;
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarDocumentosItensMobile", error);
+    }
+}
+
+function carregarOcorrenciasViagem(ocorrencias)
+{
+    try
+    {
         _ocorrenciasViagem = ocorrencias || [];
 
         const tbody = document.querySelector('#tblOcorrenciasViagem tbody');
@@ -3979,7 +3971,8 @@
 
         tbody.innerHTML = '';
 
-        if (_ocorrenciasViagem.length === 0) {
+        if (_ocorrenciasViagem.length === 0)
+        {
 
             tbody.innerHTML = `
                 <tr id="rowSemOcorrenciasViagem">
@@ -3990,15 +3983,16 @@
                 </tr>`;
 
             if (badge) badge.style.display = 'none';
-        } else {
+        }
+        else
+        {
 
             let html = '';
             let idx = 1;
 
-            for (const oc of _ocorrenciasViagem) {
-                const statusClass = obterClasseStatusOcorrencia(
-                    oc.statusOcorrencia,
-                );
+            for (const oc of _ocorrenciasViagem)
+            {
+                const statusClass = obterClasseStatusOcorrencia(oc.statusOcorrencia);
                 const statusTexto = oc.statusOcorrencia || 'Aberta';
 
                 html += `
@@ -4024,94 +4018,99 @@
 
             tbody.innerHTML = html;
 
-            if (badge) {
+            if (badge)
+            {
                 badge.textContent = _ocorrenciasViagem.length;
                 badge.style.display = 'inline';
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarOcorrenciasViagem',
-            error,
-        );
-    }
-}
-
-function obterClasseStatusOcorrencia(status) {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasViagem", error);
+    }
+}
+
+function obterClasseStatusOcorrencia(status)
+{
+    try
+    {
         if (!status) return 'ftx-status-aberta';
 
         const statusLower = status.toLowerCase();
 
-        if (
-            statusLower === 'baixada' ||
-            statusLower === 'resolvida' ||
-            statusLower === 'fechada'
-        ) {
+        if (statusLower === 'baixada' || statusLower === 'resolvida' || statusLower === 'fechada')
+        {
             return 'ftx-status-baixada';
-        } else if (statusLower === 'pendente' || statusLower === 'em análise') {
+        }
+        else if (statusLower === 'pendente' || statusLower === 'em análise')
+        {
             return 'ftx-status-pendente';
-        } else {
+        }
+        else
+        {
             return 'ftx-status-aberta';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'obterClasseStatusOcorrencia',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "obterClasseStatusOcorrencia", error);
         return 'ftx-status-aberta';
     }
 }
 
-function truncarTextoMobile(texto, maxLength) {
-    try {
+function truncarTextoMobile(texto, maxLength)
+{
+    try
+    {
         if (!texto) return '';
         if (texto.length <= maxLength) return texto;
         return texto.substring(0, maxLength) + '...';
-    } catch (error) {
+    }
+    catch (error)
+    {
         return texto || '';
     }
 }
 
-function escapeHtmlMobile(text) {
-    try {
+function escapeHtmlMobile(text)
+{
+    try
+    {
         if (!text) return '';
         const div = document.createElement('div');
         div.textContent = text;
         return div.innerHTML;
-    } catch (error) {
+    }
+    catch (error)
+    {
         return text || '';
     }
 }
 
-async function verOcorrenciaViagem(ocorrenciaId) {
-    try {
-        console.log('verOcorrenciaViagem - ocorrenciaId:', ocorrenciaId);
-        console.log(
-            'verOcorrenciaViagem - _ocorrenciasViagem:',
-            _ocorrenciasViagem,
-        );
-        console.log(
-            'verOcorrenciaViagem - ocorrenciasUpsert:',
-            ocorrenciasUpsert,
-        );
+async function verOcorrenciaViagem(ocorrenciaId)
+{
+    try
+    {
+        console.log("verOcorrenciaViagem - ocorrenciaId:", ocorrenciaId);
+        console.log("verOcorrenciaViagem - _ocorrenciasViagem:", _ocorrenciasViagem);
+        console.log("verOcorrenciaViagem - ocorrenciasUpsert:", ocorrenciasUpsert);
 
         if (!ocorrenciaId) return;
 
         let oc = null;
 
-        if (_ocorrenciasViagem && _ocorrenciasViagem.length > 0) {
-            oc = _ocorrenciasViagem.find(
-                (o) => o.ocorrenciaViagemId === ocorrenciaId,
-            );
-        }
-
-        if (!oc && ocorrenciasUpsert && ocorrenciasUpsert.length > 0) {
-            oc = ocorrenciasUpsert.find((o) => o.id === ocorrenciaId);
-
-            if (oc) {
+        if (_ocorrenciasViagem && _ocorrenciasViagem.length > 0)
+        {
+            oc = _ocorrenciasViagem.find(o => o.ocorrenciaViagemId === ocorrenciaId);
+        }
+
+        if (!oc && ocorrenciasUpsert && ocorrenciasUpsert.length > 0)
+        {
+            oc = ocorrenciasUpsert.find(o => o.id === ocorrenciaId);
+
+            if (oc)
+            {
                 oc = {
                     ocorrenciaViagemId: oc.id,
                     resumo: oc.resumo,
@@ -4119,24 +4118,22 @@
                     dataOcorrencia: oc.dataCriacao,
                     statusOcorrencia: oc.status || 'Aberta',
                     imagemBase64: oc.imagemBase64,
-                    temImagem: oc.imagemBase64 && oc.imagemBase64.length > 0,
+                    temImagem: oc.imagemBase64 && oc.imagemBase64.length > 0
                 };
             }
         }
 
-        console.log('verOcorrenciaViagem - oc encontrada:', oc);
-
-        if (!oc) {
+        console.log("verOcorrenciaViagem - oc encontrada:", oc);
+
+        if (!oc)
+        {
             AppToast.show('Amarelo', 'Ocorrência não encontrada', 3000);
             return;
         }
 
-        document.getElementById('txtOcorrenciaResumo').textContent =
-            oc.resumo || '-';
-        document.getElementById('txtOcorrenciaDescricao').textContent =
-            oc.descricao || 'Sem descrição';
-        document.getElementById('txtOcorrenciaData').textContent =
-            oc.dataOcorrencia || '-';
+        document.getElementById('txtOcorrenciaResumo').textContent = oc.resumo || '-';
+        document.getElementById('txtOcorrenciaDescricao').textContent = oc.descricao || 'Sem descrição';
+        document.getElementById('txtOcorrenciaData').textContent = oc.dataOcorrencia || '-';
 
         const divStatus = document.getElementById('divOcorrenciaStatus');
         const statusClass = obterClasseStatusOcorrencia(oc.statusOcorrencia);
@@ -4145,64 +4142,72 @@
         const divSolucao = document.getElementById('divSolucaoOcorrencia');
         const txtSolucao = document.getElementById('txtOcorrenciaSolucao');
 
-        if (oc.solucao) {
+        if (oc.solucao)
+        {
             txtSolucao.textContent = oc.solucao;
             divSolucao.style.display = 'block';
-        } else {
+        }
+        else
+        {
             divSolucao.style.display = 'none';
         }
 
         const imgOcorrencia = document.getElementById('imgOcorrenciaViagem');
         const semImagem = document.getElementById('semImagemOcorrenciaViagem');
 
-        if (oc.temImagem && oc.imagemBase64) {
+        if (oc.temImagem && oc.imagemBase64)
+        {
             imgOcorrencia.src = oc.imagemBase64;
             imgOcorrencia.style.display = 'block';
             semImagem.style.display = 'none';
-        } else if (oc.temImagem) {
-
-            try {
-                const resp = await fetch(
-                    `/api/Viagem/ObterImagemOcorrencia?ocorrenciaId=${ocorrenciaId}`,
-                );
+        }
+        else if (oc.temImagem)
+        {
+
+            try
+            {
+                const resp = await fetch(`/api/Viagem/ObterImagemOcorrencia?ocorrenciaId=${ocorrenciaId}`);
                 const imgData = await resp.json();
 
-                if (imgData.success && imgData.temImagem) {
+                if (imgData.success && imgData.temImagem)
+                {
                     imgOcorrencia.src = imgData.imagemBase64;
                     imgOcorrencia.style.display = 'block';
                     semImagem.style.display = 'none';
-                } else {
+                }
+                else
+                {
                     imgOcorrencia.style.display = 'none';
                     semImagem.style.display = 'block';
                 }
-            } catch (e) {
+            }
+            catch (e)
+            {
                 imgOcorrencia.style.display = 'none';
                 semImagem.style.display = 'block';
             }
-        } else {
+        }
+        else
+        {
             imgOcorrencia.style.display = 'none';
             semImagem.style.display = 'block';
         }
 
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalVerOcorrenciaViagem'),
-        );
+        const modal = new bootstrap.Modal(document.getElementById('modalVerOcorrenciaViagem'));
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'verOcorrenciaViagem',
-            error,
-        );
-    }
-}
-
-function inicializarIntegracaoMobile() {
-    try {
-
-        const txtViagemId =
-            document.getElementById('txtViagemId') ||
-            document.getElementById('txtId');
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verOcorrenciaViagem", error);
+    }
+}
+
+function inicializarIntegracaoMobile()
+{
+    try
+    {
+
+        const txtViagemId = document.getElementById('txtViagemId') || document.getElementById('txtId');
         window.viagemId = txtViagemId ? txtViagemId.value : '';
 
         aplicarFormatacaoHoras();
@@ -4210,119 +4215,116 @@
         configurarCampoNoFichaVistoria();
 
         carregarDadosMobile();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'inicializarIntegracaoMobile',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
-
-        setTimeout(function () {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "inicializarIntegracaoMobile", error);
+    }
+}
+
+$(document).ready(function ()
+{
+    try
+    {
+
+        setTimeout(function ()
+        {
             inicializarIntegracaoMobile();
         }, 500);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'document.ready.mobile',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "document.ready.mobile", error);
     }
 });
 
 var ocorrenciasUpsert = [];
 
-function inicializarSistemaOcorrencias() {
-    try {
-
-        setTimeout(function () {
+function inicializarSistemaOcorrencias()
+{
+    try
+    {
+
+        setTimeout(function() {
             verificarVeiculoParaOcorrencias();
         }, 100);
 
-        $('#btnAdicionarOcorrenciaUpsert').on('click', function () {
+        $('#btnAdicionarOcorrenciaUpsert').on('click', function()
+        {
             abrirModalInserirOcorrencia();
         });
 
-        $('#fileImagemOcorrenciaUpsert').on('change', function () {
+        $('#fileImagemOcorrenciaUpsert').on('change', function()
+        {
             previewImagemOcorrencia(this);
         });
 
-        $('#btnLimparImagemOcorrenciaUpsert').on('click', function () {
+        $('#btnLimparImagemOcorrenciaUpsert').on('click', function()
+        {
             limparImagemOcorrencia();
         });
 
-        $('#btnConfirmarOcorrenciaUpsert').on('click', function () {
+        $('#btnConfirmarOcorrenciaUpsert').on('click', function()
+        {
             confirmarOcorrencia();
         });
 
-        $(document).on('click', '.btn-remover-ocorrencia-upsert', function () {
+        $(document).on('click', '.btn-remover-ocorrencia-upsert', function()
+        {
             const index = $(this).data('index');
             removerOcorrencia(index);
         });
 
-        $(document).on(
-            'click',
-            '.btn-ver-imagem-ocorrencia-upsert',
-            function () {
-                const index = $(this).data('index');
-                verImagemOcorrencia(index);
-            },
-        );
-
-        $(document).on(
-            'click',
-            '.btn-ver-detalhes-ocorrencia-upsert',
-            function () {
-                const id = $(this).data('id');
-                if (id) {
-                    verOcorrenciaViagem(id);
-                }
-            },
-        );
+        $(document).on('click', '.btn-ver-imagem-ocorrencia-upsert', function()
+        {
+            const index = $(this).data('index');
+            verImagemOcorrencia(index);
+        });
+
+        $(document).on('click', '.btn-ver-detalhes-ocorrencia-upsert', function()
+        {
+            const id = $(this).data('id');
+            if (id) {
+                verOcorrenciaViagem(id);
+            }
+        });
 
         carregarOcorrenciasExistentes();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'inicializarSistemaOcorrencias',
-            error,
-        );
-    }
-}
-
-function verificarVeiculoParaOcorrencias() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "inicializarSistemaOcorrencias", error);
+    }
+}
+
+function verificarVeiculoParaOcorrencias()
+{
+    try
+    {
         const cmbVeiculo = document.getElementById('cmbVeiculo');
-        if (
-            !cmbVeiculo ||
-            !cmbVeiculo.ej2_instances ||
-            !cmbVeiculo.ej2_instances[0]
-        )
-            return;
+        if (!cmbVeiculo || !cmbVeiculo.ej2_instances || !cmbVeiculo.ej2_instances[0]) return;
 
         let veiculoId = cmbVeiculo.ej2_instances[0].value;
-        if (Array.isArray(veiculoId)) {
+        if (Array.isArray(veiculoId))
+        {
             veiculoId = veiculoId[0];
         }
 
-        if (typeof controlarSecaoOcorrencias === 'function') {
+        if (typeof controlarSecaoOcorrencias === 'function')
+        {
             controlarSecaoOcorrencias(veiculoId);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'verificarVeiculoParaOcorrencias',
-            error,
-        );
-    }
-}
-
-function abrirModalInserirOcorrencia() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verificarVeiculoParaOcorrencias", error);
+    }
+}
+
+function abrirModalInserirOcorrencia()
+{
+    try
+    {
 
         $('#txtResumoOcorrenciaUpsert').val('');
         $('#txtDescricaoOcorrenciaUpsert').val('');
@@ -4330,59 +4332,60 @@
         $('#previewImagemOcorrenciaUpsert').hide();
         $('#imgPreviewOcorrenciaUpsert').attr('src', '');
 
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalInserirOcorrenciaUpsert'),
-        );
+        const modal = new bootstrap.Modal(document.getElementById('modalInserirOcorrenciaUpsert'));
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'abrirModalInserirOcorrencia',
-            error,
-        );
-    }
-}
-
-function previewImagemOcorrencia(input) {
-    try {
-        if (input.files && input.files[0]) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "abrirModalInserirOcorrencia", error);
+    }
+}
+
+function previewImagemOcorrencia(input)
+{
+    try
+    {
+        if (input.files && input.files[0])
+        {
             const reader = new FileReader();
-            reader.onload = function (e) {
+            reader.onload = function(e)
+            {
                 $('#imgPreviewOcorrenciaUpsert').attr('src', e.target.result);
                 $('#previewImagemOcorrenciaUpsert').show();
             };
             reader.readAsDataURL(input.files[0]);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'previewImagemOcorrencia',
-            error,
-        );
-    }
-}
-
-function limparImagemOcorrencia() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "previewImagemOcorrencia", error);
+    }
+}
+
+function limparImagemOcorrencia()
+{
+    try
+    {
         $('#fileImagemOcorrenciaUpsert').val('');
         $('#previewImagemOcorrenciaUpsert').hide();
         $('#imgPreviewOcorrenciaUpsert').attr('src', '');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'limparImagemOcorrencia',
-            error,
-        );
-    }
-}
-
-function confirmarOcorrencia() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "limparImagemOcorrencia", error);
+    }
+}
+
+function confirmarOcorrencia()
+{
+    try
+    {
         const resumo = $('#txtResumoOcorrenciaUpsert').val().trim();
         const descricao = $('#txtDescricaoOcorrenciaUpsert').val().trim();
         const imgPreview = $('#imgPreviewOcorrenciaUpsert').attr('src') || '';
 
-        if (!resumo) {
+        if (!resumo)
+        {
             AppToast.show('Amarelo', 'Informe o resumo da ocorrência', 3000);
             $('#txtResumoOcorrenciaUpsert').focus();
             return;
@@ -4393,7 +4396,7 @@
             resumo: resumo,
             descricao: descricao,
             imagemBase64: imgPreview,
-            dataCriacao: new Date().toLocaleString('pt-BR'),
+            dataCriacao: new Date().toLocaleString('pt-BR')
         };
 
         ocorrenciasUpsert.push(ocorrencia);
@@ -4402,68 +4405,65 @@
         atualizarBadgeOcorrencias();
         atualizarHiddenOcorrencias();
 
-        bootstrap.Modal.getInstance(
-            document.getElementById('modalInserirOcorrenciaUpsert'),
-        ).hide();
+        bootstrap.Modal.getInstance(document.getElementById('modalInserirOcorrenciaUpsert')).hide();
 
         AppToast.show('Verde', 'Ocorrência adicionada com sucesso', 2000);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'confirmarOcorrencia',
-            error,
-        );
-    }
-}
-
-function removerOcorrencia(index) {
-    try {
-        if (index >= 0 && index < ocorrenciasUpsert.length) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "confirmarOcorrencia", error);
+    }
+}
+
+function removerOcorrencia(index)
+{
+    try
+    {
+        if (index >= 0 && index < ocorrenciasUpsert.length)
+        {
             ocorrenciasUpsert.splice(index, 1);
             renderizarListaOcorrencias();
             atualizarBadgeOcorrencias();
             atualizarHiddenOcorrencias();
             AppToast.show('Vermelho', 'Ocorrência removida', 2000);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'removerOcorrencia',
-            error,
-        );
-    }
-}
-
-function verImagemOcorrencia(index) {
-    try {
-        if (index >= 0 && index < ocorrenciasUpsert.length) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "removerOcorrencia", error);
+    }
+}
+
+function verImagemOcorrencia(index)
+{
+    try
+    {
+        if (index >= 0 && index < ocorrenciasUpsert.length)
+        {
             const ocorrencia = ocorrenciasUpsert[index];
-            if (ocorrencia.imagemBase64) {
-                $('#imgViewerOcorrenciaUpsert').attr(
-                    'src',
-                    ocorrencia.imagemBase64,
-                );
-                const modal = new bootstrap.Modal(
-                    document.getElementById('modalVerImagemOcorrenciaUpsert'),
-                );
+            if (ocorrencia.imagemBase64)
+            {
+                $('#imgViewerOcorrenciaUpsert').attr('src', ocorrencia.imagemBase64);
+                const modal = new bootstrap.Modal(document.getElementById('modalVerImagemOcorrenciaUpsert'));
                 modal.show();
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'verImagemOcorrencia',
-            error,
-        );
-    }
-}
-
-function renderizarListaOcorrencias() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "verImagemOcorrencia", error);
+    }
+}
+
+function renderizarListaOcorrencias()
+{
+    try
+    {
         const container = $('#listaOcorrenciasUpsert');
         const semOcorrencias = $('#semOcorrenciasUpsert');
 
-        if (ocorrenciasUpsert.length === 0) {
+        if (ocorrenciasUpsert.length === 0)
+        {
             semOcorrencias.show();
             container.find('.ocorrencia-item').remove();
             return;
@@ -4473,12 +4473,10 @@
 
         container.find('.ocorrencia-item').remove();
 
-        ocorrenciasUpsert.forEach((oc, index) => {
+        ocorrenciasUpsert.forEach((oc, index) =>
+        {
             const temImagem = oc.imagemBase64 && oc.imagemBase64.length > 0;
-            const temId =
-                oc.id &&
-                oc.id !== '' &&
-                oc.id !== '00000000-0000-0000-0000-000000000000';
+            const temId = oc.id && oc.id !== "" && oc.id !== "00000000-0000-0000-0000-000000000000";
             const podeExcluir = !window.viagemFinalizada;
 
             const html = `
@@ -4491,103 +4489,90 @@
                         ${oc.descricao ? `<div class="ocorrencia-descricao">${escapeHtml(oc.descricao)}</div>` : ''}
                     </div>
                     <div class="ocorrencia-acoes">
-                        ${
-                            temId
-                                ? `
+                        ${temId ? `
                             <button type="button" class="btn btn-sm btn-outline-primary btn-ver-detalhes-ocorrencia-upsert"
                                     data-id="${oc.id}" data-index="${index}" title="Ver detalhes">
                                 <i class="fa fa-eye"></i>
                             </button>
-                        `
-                                : ''
-                        }
-                        ${
-                            temImagem
-                                ? `
+                        ` : ''}
+                        ${temImagem ? `
                             <button type="button" class="btn btn-sm btn-outline-info btn-ver-imagem-ocorrencia-upsert"
                                     data-index="${index}" title="Ver imagem">
                                 <i class="fa fa-image"></i>
                             </button>
-                        `
-                                : ''
-                        }
-                        ${
-                            podeExcluir
-                                ? `
+                        ` : ''}
+                        ${podeExcluir ? `
                             <button type="button" class="btn btn-sm btn-outline-danger btn-remover-ocorrencia-upsert"
                                     data-index="${index}" title="Remover">
                                 <i class="fa fa-trash"></i>
                             </button>
-                        `
-                                : ''
-                        }
+                        ` : ''}
                     </div>
                 </div>
             `;
 
             container.append(html);
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'renderizarListaOcorrencias',
-            error,
-        );
-    }
-}
-
-function atualizarBadgeOcorrencias() {
-    try {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "renderizarListaOcorrencias", error);
+    }
+}
+
+function atualizarBadgeOcorrencias()
+{
+    try
+    {
         const badge = $('#badgeOcorrenciasUpsert');
         const qtd = ocorrenciasUpsert.length;
 
-        if (qtd > 0) {
+        if (qtd > 0)
+        {
             badge.text(qtd);
             badge.show();
-        } else {
+        }
+        else
+        {
             badge.hide();
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'atualizarBadgeOcorrencias',
-            error,
-        );
-    }
-}
-
-function atualizarHiddenOcorrencias() {
-    try {
-        const json = JSON.stringify(
-            ocorrenciasUpsert.map((oc) => ({
-                Resumo: oc.resumo,
-                Descricao: oc.descricao,
-                ImagemOcorrencia: oc.imagemBase64 || '',
-            })),
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarBadgeOcorrencias", error);
+    }
+}
+
+function atualizarHiddenOcorrencias()
+{
+    try
+    {
+        const json = JSON.stringify(ocorrenciasUpsert.map(oc => ({
+            Resumo: oc.resumo,
+            Descricao: oc.descricao,
+            ImagemOcorrencia: oc.imagemBase64 || ''
+        })));
 
         $('#hiddenOcorrenciasJson').val(json);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'atualizarHiddenOcorrencias',
-            error,
-        );
-    }
-}
-
-function carregarOcorrenciasExistentes() {
-    try {
-
-        let viagemId =
-            window.viagemId ||
-            $('#txtViagemId').val() ||
-            $('input[name="ViagemObj.Viagem.ViagemId"]').val();
-
-        console.log('carregarOcorrenciasExistentes - viagemId:', viagemId);
-
-        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000') {
-            console.log('Nova viagem - não carregando ocorrências');
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "atualizarHiddenOcorrencias", error);
+    }
+}
+
+function carregarOcorrenciasExistentes()
+{
+    try
+    {
+
+        let viagemId = window.viagemId || $('#txtViagemId').val() || $('input[name="ViagemObj.Viagem.ViagemId"]').val();
+
+        console.log("carregarOcorrenciasExistentes - viagemId:", viagemId);
+
+        if (!viagemId || viagemId === '00000000-0000-0000-0000-000000000000')
+        {
+            console.log("Nova viagem - não carregando ocorrências");
             return;
         }
 
@@ -4595,86 +4580,68 @@
             url: '/api/Viagem/ObterDadosMobile',
             type: 'GET',
             data: { viagemId: viagemId },
-            success: function (response) {
-                try {
-                    console.log('ObterDadosMobile response:', response);
-                    if (
-                        response.success &&
-                        response.ocorrencias &&
-                        response.ocorrencias.length > 0
-                    ) {
-                        ocorrenciasUpsert = response.ocorrencias.map((oc) => ({
+            success: function(response)
+            {
+                try
+                {
+                    console.log("ObterDadosMobile response:", response);
+                    if (response.success && response.ocorrencias && response.ocorrencias.length > 0)
+                    {
+                        ocorrenciasUpsert = response.ocorrencias.map(oc => ({
                             id: oc.ocorrenciaViagemId,
                             resumo: oc.resumo || '',
                             descricao: oc.descricao || '',
-                            imagemBase64:
-                                oc.imagemBase64 ||
-                                oc.imagemOcorrencia ||
-                                oc.imagem ||
-                                '',
+                            imagemBase64: oc.imagemBase64 || oc.imagemOcorrencia || oc.imagem || '',
                             dataCriacao: oc.dataOcorrencia || '',
                             status: oc.statusOcorrencia || 'Aberta',
-                            temImagem:
-                                oc.temImagem ||
-                                (oc.imagemBase64 &&
-                                    oc.imagemBase64.length > 0) ||
-                                (oc.imagemOcorrencia &&
-                                    oc.imagemOcorrencia.length > 0),
+                            temImagem: oc.temImagem || (oc.imagemBase64 && oc.imagemBase64.length > 0) || (oc.imagemOcorrencia && oc.imagemOcorrencia.length > 0)
                         }));
 
-                        console.log(
-                            'ocorrenciasUpsert mapeadas:',
-                            ocorrenciasUpsert,
-                        );
+                        console.log("ocorrenciasUpsert mapeadas:", ocorrenciasUpsert);
 
                         renderizarListaOcorrencias();
                         atualizarBadgeOcorrencias();
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'ViagemUpsert.js',
-                        'carregarOcorrenciasExistentes.success',
-                        error,
-                    );
+                }
+                catch (error)
+                {
+                    Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasExistentes.success", error);
                 }
             },
-            error: function (xhr, status, error) {
-                console.log(
-                    'Erro ao carregar ocorrências:',
-                    error,
-                    xhr.responseText,
-                );
-            },
+            error: function(xhr, status, error)
+            {
+                console.log("Erro ao carregar ocorrências:", error, xhr.responseText);
+            }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'carregarOcorrenciasExistentes',
-            error,
-        );
-    }
-}
-
-function escapeHtml(text) {
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "carregarOcorrenciasExistentes", error);
+    }
+}
+
+function escapeHtml(text)
+{
     if (!text) return '';
     const div = document.createElement('div');
     div.textContent = text;
     return div.innerHTML;
 }
 
-$(document).ready(function () {
-    try {
-        setTimeout(function () {
+$(document).ready(function()
+{
+    try
+    {
+        setTimeout(function()
+        {
             inicializarSistemaOcorrencias();
 
             calcularDuracaoViagem();
             calcularKmPercorrido();
         }, 600);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'ViagemUpsert.js',
-            'document.ready.ocorrencias',
-            error,
-        );
+    }
+    catch (error)
+    {
+        Alerta.TratamentoErroComLinha("ViagemUpsert.js", "document.ready.ocorrencias", error);
     }
 });
```
