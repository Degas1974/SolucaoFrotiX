# wwwroot/js/cadastros/multa.js

**Mudanca:** GRANDE | **+971** linhas | **-1060** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/multa.js
+++ ATUAL: wwwroot/js/cadastros/multa.js
@@ -1,5 +1,7 @@
-function tiraAcento(frase) {
-    try {
+function tiraAcento(frase)
+{
+    try
+    {
         if (!frase) return '';
 
         const semAcento = frase
@@ -9,173 +11,177 @@
             .toUpperCase();
 
         return semAcento;
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro em tiraAcento:', error);
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'tiraAcento', error);
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "tiraAcento", error);
         }
         return '';
     }
 }
 
-function getMainViewer() {
-    try {
-        return document.getElementById('pdfviewer')?.ej2_instances?.[0] || null;
-    } catch (err) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'getMainViewer', err);
+function getMainViewer()
+{
+    try
+    {
+        return document.getElementById("pdfviewer")?.ej2_instances?.[0] || null;
+    } catch (err)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "getMainViewer", err);
         }
         return null;
     }
 }
 
-function loadPdfInViewer(fileName) {
-    try {
+function loadPdfInViewer(fileName)
+{
+    try
+    {
         const viewer = getMainViewer();
-        if (!viewer) {
-            console.error('Viewer não encontrado');
+        if (!viewer)
+        {
+            console.error("Viewer não encontrado");
             return;
         }
 
         viewer.documentPath = fileName;
         viewer.dataBind();
         viewer.load(fileName, null);
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'loadPdfInViewer', error);
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "loadPdfInViewer", error);
         }
         console.error(error);
     }
 }
 
-function onSuccessAutuacao(e) {
-    try {
+function onSuccessAutuacao(e)
+{
+    try
+    {
         var files = e.files;
         if (!files || files.length === 0) return;
 
         var fileName = tiraAcento(files[0].name);
-        document
-            .getElementById('txtAutuacaoPDF')
-            ?.setAttribute('value', fileName);
+        document.getElementById("txtAutuacaoPDF")?.setAttribute('value', fileName);
 
         loadPdfInViewer(fileName);
 
-        if (window.AppToast?.show) {
-            AppToast.show(
-                'Verde',
-                'PDF de Autuação enviado com sucesso!',
-                3000,
-            );
-        }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'onSuccessAutuacao',
-                error,
-            );
-        }
-    }
-}
-
-function onSuccessPenalidade(e) {
-    try {
+        if (window.AppToast?.show)
+        {
+            AppToast.show('Verde', 'PDF de Autuação enviado com sucesso!', 3000);
+        }
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "onSuccessAutuacao", error);
+        }
+    }
+}
+
+function onSuccessPenalidade(e)
+{
+    try
+    {
         var files = e.files;
         if (!files || files.length === 0) return;
 
         var fileName = tiraAcento(files[0].name);
-        document
-            .getElementById('txtPenalidadePDF')
-            ?.setAttribute('value', fileName);
+        document.getElementById("txtPenalidadePDF")?.setAttribute('value', fileName);
 
         loadPdfInViewer(fileName);
 
-        if (window.AppToast?.show) {
-            AppToast.show(
-                'Verde',
-                'PDF de Penalidade enviado com sucesso!',
-                3000,
-            );
-        }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'onSuccessPenalidade',
-                error,
-            );
-        }
-    }
-}
-
-function onSuccessComprovante(e) {
-    try {
+        if (window.AppToast?.show)
+        {
+            AppToast.show('Verde', 'PDF de Penalidade enviado com sucesso!', 3000);
+        }
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "onSuccessPenalidade", error);
+        }
+    }
+}
+
+function onSuccessComprovante(e)
+{
+    try
+    {
         var files = e.files;
         if (!files || files.length === 0) return;
 
         var fileName = tiraAcento(files[0].name);
-        document
-            .getElementById('txtComprovantePDF')
-            ?.setAttribute('value', fileName);
+        document.getElementById("txtComprovantePDF")?.setAttribute('value', fileName);
 
         loadPdfInViewer(fileName);
 
-        if (window.AppToast?.show) {
+        if (window.AppToast?.show)
+        {
             AppToast.show('Verde', 'Comprovante enviado com sucesso!', 3000);
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'onSuccessComprovante',
-                error,
-            );
-        }
-    }
-}
-
-function onSuccessEDoc(e) {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "onSuccessComprovante", error);
+        }
+    }
+}
+
+function onSuccessEDoc(e)
+{
+    try
+    {
         var files = e.files;
         if (!files || files.length === 0) return;
 
         var fileName = tiraAcento(files[0].name);
-        document.getElementById('txtEDocPDF')?.setAttribute('value', fileName);
+        document.getElementById("txtEDocPDF")?.setAttribute('value', fileName);
 
         loadPdfInViewer(fileName);
 
-        if (window.AppToast?.show) {
+        if (window.AppToast?.show)
+        {
             AppToast.show('Verde', 'Processo EDoc enviado com sucesso!', 3000);
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'onSuccessEDoc', error);
-        }
-    }
-}
-
-function onSuccessDocumentos(e) {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "onSuccessEDoc", error);
+        }
+    }
+}
+
+function onSuccessDocumentos(e)
+{
+    try
+    {
         var files = e.files;
         if (!files || files.length === 0) return;
 
         var fileName = tiraAcento(files[0].name);
-        document
-            .getElementById('txtOutrosDocumentosPDF')
-            ?.setAttribute('value', fileName);
+        document.getElementById("txtOutrosDocumentosPDF")?.setAttribute('value', fileName);
 
         loadPdfInViewer(fileName);
 
-        if (window.AppToast?.show) {
+        if (window.AppToast?.show)
+        {
             AppToast.show('Verde', 'Documento enviado com sucesso!', 3000);
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'onSuccessDocumentos',
-                error,
-            );
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "onSuccessDocumentos", error);
         }
     }
 }
@@ -186,1359 +192,1264 @@
 var EscolhendoMotorista = false;
 var EscolhendoVeiculo = false;
 
-$(document).ready(function () {
-    try {
+$(document).ready(function ()
+{
+    try
+    {
 
         configurarControlesSyncfusion();
 
         verificarModoEdicao();
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'document.ready', error);
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "document.ready", error);
         }
     }
 });
 
-function configurarControlesSyncfusion() {
-    try {
-
-        if ($('#inputAutuacaoPDF').length) {
+function configurarControlesSyncfusion()
+{
+    try
+    {
+
+        if ($("#inputAutuacaoPDF").length)
+        {
             var uploadAutuacao = new ej.inputs.Uploader({
                 asyncSettings: {
-                    saveUrl: '/api/Upload/save',
-                    removeUrl: '/api/Upload/remove',
+                    saveUrl: "/api/Upload/save",
+                    removeUrl: "/api/Upload/remove"
                 },
                 allowedExtensions: '.pdf',
                 multiple: false,
                 autoUpload: true,
                 success: onSuccessAutuacao,
-                failure: function (args) {
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro no upload da Autuação',
-                            3000,
-                        );
-                    }
-                    console.error('Erro no upload da Autuação:', args);
-                },
+                failure: function (args)
+                {
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Vermelho', 'Erro no upload da Autuação', 3000);
+                    }
+                    console.error("Erro no upload da Autuação:", args);
+                }
             });
             uploadAutuacao.appendTo('#inputAutuacaoPDF');
         }
 
-        if ($('#pdf').length) {
+        if ($("#pdf").length)
+        {
             var uploadPenalidade = new ej.inputs.Uploader({
                 asyncSettings: {
-                    saveUrl: '/api/Upload/save',
-                    removeUrl: '/api/Upload/remove',
+                    saveUrl: "/api/Upload/save",
+                    removeUrl: "/api/Upload/remove"
                 },
                 allowedExtensions: '.pdf',
                 multiple: false,
                 autoUpload: true,
                 success: onSuccessPenalidade,
-                failure: function (args) {
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro no upload da Penalidade',
-                            3000,
-                        );
-                    }
-                    console.error('Erro no upload da Penalidade:', args);
-                },
+                failure: function (args)
+                {
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Vermelho', 'Erro no upload da Penalidade', 3000);
+                    }
+                    console.error("Erro no upload da Penalidade:", args);
+                }
             });
             uploadPenalidade.appendTo('#pdf');
         }
 
-        if ($('#flComprovante').length) {
+        if ($("#flComprovante").length)
+        {
             var uploadComprovante = new ej.inputs.Uploader({
                 asyncSettings: {
-                    saveUrl: '/api/Upload/save',
-                    removeUrl: '/api/Upload/remove',
+                    saveUrl: "/api/Upload/save",
+                    removeUrl: "/api/Upload/remove"
                 },
                 allowedExtensions: '.pdf',
                 multiple: false,
                 autoUpload: true,
                 success: onSuccessComprovante,
-                failure: function (args) {
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro no upload do Comprovante',
-                            3000,
-                        );
-                    }
-                    console.error('Erro no upload do Comprovante:', args);
-                },
+                failure: function (args)
+                {
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Vermelho', 'Erro no upload do Comprovante', 3000);
+                    }
+                    console.error("Erro no upload do Comprovante:", args);
+                }
             });
             uploadComprovante.appendTo('#flComprovante');
         }
 
-        if ($('#inputEDocPDF').length) {
+        if ($("#inputEDocPDF").length)
+        {
             var uploadEDoc = new ej.inputs.Uploader({
                 asyncSettings: {
-                    saveUrl: '/api/Upload/save',
-                    removeUrl: '/api/Upload/remove',
+                    saveUrl: "/api/Upload/save",
+                    removeUrl: "/api/Upload/remove"
                 },
                 allowedExtensions: '.pdf',
                 multiple: false,
                 autoUpload: true,
                 success: onSuccessEDoc,
-                failure: function (args) {
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro no upload do eDoc',
-                            3000,
-                        );
-                    }
-                    console.error('Erro no upload do eDoc:', args);
-                },
+                failure: function (args)
+                {
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Vermelho', 'Erro no upload do eDoc', 3000);
+                    }
+                    console.error("Erro no upload do eDoc:", args);
+                }
             });
             uploadEDoc.appendTo('#inputEDocPDF');
         }
 
-        if ($('#inputOutrosDocumentosPDF').length) {
+        if ($("#inputOutrosDocumentosPDF").length)
+        {
             var uploadDocumentos = new ej.inputs.Uploader({
                 asyncSettings: {
-                    saveUrl: '/api/Upload/save',
-                    removeUrl: '/api/Upload/remove',
+                    saveUrl: "/api/Upload/save",
+                    removeUrl: "/api/Upload/remove"
                 },
                 allowedExtensions: '.pdf',
                 multiple: false,
                 autoUpload: true,
                 success: onSuccessDocumentos,
-                failure: function (args) {
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Vermelho',
-                            'Erro no upload de Outros Documentos',
-                            3000,
-                        );
-                    }
-                    console.error('Erro no upload de Outros Documentos:', args);
-                },
+                failure: function (args)
+                {
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Vermelho', 'Erro no upload de Outros Documentos', 3000);
+                    }
+                    console.error("Erro no upload de Outros Documentos:", args);
+                }
             });
             uploadDocumentos.appendTo('#inputOutrosDocumentosPDF');
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'configurarControlesSyncfusion',
-                error,
-            );
-        }
-    }
-}
-
-function verificarModoEdicao() {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "configurarControlesSyncfusion", error);
+        }
+    }
+}
+
+function verificarModoEdicao()
+{
+    try
+    {
 
         var multaId = $('#MultaObj_Multa_MultaId').val() || '';
 
-        if (multaId && multaId != '00000000-0000-0000-0000-000000000000') {
+        if (multaId && multaId != '00000000-0000-0000-0000-000000000000')
+        {
             console.log('Modo Edição - MultaId:', multaId);
 
-            setTimeout(function () {
-                try {
-                    var lstInfracao = document.getElementById('lstInfracao');
-                    if (lstInfracao?.ej2_instances?.[0]) {
-                        var tipoMultaId = $(
-                            '#MultaObj_Multa_TipoMultaId',
-                        ).val();
-                        if (tipoMultaId) {
+            setTimeout(function ()
+            {
+                try
+                {
+                    var lstInfracao = document.getElementById("lstInfracao");
+                    if (lstInfracao?.ej2_instances?.[0])
+                    {
+                        var tipoMultaId = $('#MultaObj_Multa_TipoMultaId').val();
+                        if (tipoMultaId)
+                        {
                             lstInfracao.ej2_instances[0].value = tipoMultaId;
                         }
                     }
-                } catch (error) {
-                    if (window.Alerta?.TratamentoErroComLinha) {
-                        Alerta.TratamentoErroComLinha(
-                            'multa.js',
-                            'lstInfracao.setValue',
-                            error,
-                        );
+                } catch (error)
+                {
+                    if (window.Alerta?.TratamentoErroComLinha)
+                    {
+                        Alerta.TratamentoErroComLinha("multa.js", "lstInfracao.setValue", error);
                     }
                 }
             }, 500);
 
-            setTimeout(function () {
+            setTimeout(function ()
+            {
                 carregarPrimeiroPDF();
             }, 1000);
 
-            try {
+            try
+            {
                 inicializarValoresMonetarios();
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multa.js',
-                        'valoresMonetarios',
-                        error,
-                    );
-                }
-            }
-
-            try {
-                if (typeof lstEmpenhosChange === 'function') {
+            } catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multa.js", "valoresMonetarios", error);
+                }
+            }
+
+            try
+            {
+                if (typeof lstEmpenhosChange === 'function')
+                {
                     lstEmpenhosChange();
                 }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multa.js',
-                        'lstEmpenhosChange',
-                        error,
-                    );
-                }
-            }
-        } else {
+            } catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multa.js", "lstEmpenhosChange", error);
+                }
+            }
+        } else
+        {
             console.log('Modo Criação - Novo Registro');
             inicializarNovoRegistro();
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'verificarModoEdicao',
-                error,
-            );
-        }
-    }
-}
-
-function carregarPrimeiroPDF() {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "verificarModoEdicao", error);
+        }
+    }
+}
+
+function carregarPrimeiroPDF()
+{
+    try
+    {
 
         var pdfs = [
             { campo: 'txtPenalidadePDF', nome: 'Penalidade' },
             { campo: 'txtAutuacaoPDF', nome: 'Autuação' },
             { campo: 'txtComprovantePDF', nome: 'Comprovante' },
             { campo: 'txtEDocPDF', nome: 'EDoc' },
-            { campo: 'txtOutrosDocumentosPDF', nome: 'Outros Documentos' },
+            { campo: 'txtOutrosDocumentosPDF', nome: 'Outros Documentos' }
         ];
 
-        for (var i = 0; i < pdfs.length; i++) {
+        for (var i = 0; i < pdfs.length; i++)
+        {
             var pdfPath = $('#' + pdfs[i].campo).val();
-            if (pdfPath && pdfPath != '' && pdfPath != 'null') {
+            if (pdfPath && pdfPath != '' && pdfPath != 'null')
+            {
                 console.log('Carregando PDF de ' + pdfs[i].nome + ':', pdfPath);
                 loadPdfInViewer(pdfPath);
                 break;
             }
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'carregarPrimeiroPDF',
-                error,
-            );
-        }
-    }
-}
-
-function inicializarValoresMonetarios() {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "carregarPrimeiroPDF", error);
+        }
+    }
+}
+
+function inicializarValoresMonetarios()
+{
     var valorAteVencimento = $('#txtValorAteVencimento').val();
     var valorPosVencimento = $('#txtValorPosVencimento').val();
     var valorPago = $('#txtValorPago').val();
 
-    if (
-        !valorAteVencimento ||
-        valorAteVencimento == '0' ||
-        valorAteVencimento == 'null' ||
-        valorAteVencimento == ''
-    ) {
-        $('#txtValorAteVencimento').val('0,00');
-    }
-
-    if (
-        !valorPosVencimento ||
-        valorPosVencimento == '0' ||
-        valorPosVencimento == 'null' ||
-        valorPosVencimento == ''
-    ) {
-        $('#txtValorPosVencimento').val('0,00');
-    }
-
-    if (
-        !valorPago ||
-        valorPago == '0' ||
-        valorPago == 'null' ||
-        valorPago == ''
-    ) {
-        $('#txtValorPago').val('0,00');
-    }
-}
-
-function inicializarNovoRegistro() {
-    try {
-
-        var lstContratoVeiculo = document.getElementById('lstContratoVeiculo');
-        if (lstContratoVeiculo?.ej2_instances?.[0]) {
-            lstContratoVeiculo.ej2_instances[0].text = '';
-        }
-
-        var lstContratoMotorista = document.getElementById(
-            'lstContratoMotorista',
-        );
-        if (lstContratoMotorista?.ej2_instances?.[0]) {
-            lstContratoMotorista.ej2_instances[0].text = '';
-        }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'limparListas', error);
-        }
-    }
-
-    try {
+    if (!valorAteVencimento || valorAteVencimento == '0' || valorAteVencimento == 'null' || valorAteVencimento == '')
+    {
+        $('#txtValorAteVencimento').val("0,00");
+    }
+
+    if (!valorPosVencimento || valorPosVencimento == '0' || valorPosVencimento == 'null' || valorPosVencimento == '')
+    {
+        $('#txtValorPosVencimento').val("0,00");
+    }
+
+    if (!valorPago || valorPago == '0' || valorPago == 'null' || valorPago == '')
+    {
+        $('#txtValorPago').val("0,00");
+    }
+}
+
+function inicializarNovoRegistro()
+{
+    try
+    {
+
+        var lstContratoVeiculo = document.getElementById("lstContratoVeiculo");
+        if (lstContratoVeiculo?.ej2_instances?.[0])
+        {
+            lstContratoVeiculo.ej2_instances[0].text = "";
+        }
+
+        var lstContratoMotorista = document.getElementById("lstContratoMotorista");
+        if (lstContratoMotorista?.ej2_instances?.[0])
+        {
+            lstContratoMotorista.ej2_instances[0].text = "";
+        }
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "limparListas", error);
+        }
+    }
+
+    try
+    {
         inicializarValoresMonetarios();
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'valoresMonetariosNovo',
-                error,
-            );
-        }
-    }
-}
-
-function stopEnterSubmitting(e) {
-    try {
-        if (e.keyCode == 13) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "valoresMonetariosNovo", error);
+        }
+    }
+}
+
+function stopEnterSubmitting(e)
+{
+    try
+    {
+        if (e.keyCode == 13)
+        {
             var src = e.srcElement || e.target;
-            if (src.tagName.toLowerCase() != 'div') {
-                if (e.preventDefault) {
+            if (src.tagName.toLowerCase() != "div")
+            {
+                if (e.preventDefault)
+                {
                     e.preventDefault();
-                } else {
+                } else
+                {
                     e.returnValue = false;
                 }
             }
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'stopEnterSubmitting',
-                error,
-            );
-        }
-    }
-}
-
-function moeda(a, e, r, t) {
-    try {
-        let n = '',
-            h = (j = 0),
-            u = (tamanho2 = 0),
-            l = (ajd2 = ''),
-            o = window.Event ? t.which : t.keyCode;
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "stopEnterSubmitting", error);
+        }
+    }
+}
+
+function moeda(a, e, r, t)
+{
+    try
+    {
+        let n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
         if (13 == o || 8 == o) return true;
-        if (((n = String.fromCharCode(o)), -1 == '0123456789'.indexOf(n)))
-            return false;
-
-        for (
-            u = a.value.length, h = 0;
-            h < u && ('0' == a.value.charAt(h) || a.value.charAt(h) == r);
-            h++
-        );
-        for (l = ''; h < u; h++) {
-            if (-1 != '0123456789'.indexOf(a.value.charAt(h))) {
+        if (n = String.fromCharCode(o), -1 == "0123456789".indexOf(n)) return false;
+
+        for (u = a.value.length, h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++);
+        for (l = ""; h < u; h++)
+        {
+            if (-1 != "0123456789".indexOf(a.value.charAt(h)))
+            {
                 l += a.value.charAt(h);
             }
         }
 
-        if (
-            ((l += n),
-            0 == (u = l.length) && (a.value = ''),
-            1 == u && (a.value = '0' + r + '0' + l),
-            2 == u && (a.value = '0' + r + l),
-            u > 2)
-        ) {
-            for (ajd2 = '', j = 0, h = u - 3; h >= 0; h--) {
-                if (3 == j) {
+        if (l += n, 0 == (u = l.length) && (a.value = ""), 1 == u && (a.value = "0" + r + "0" + l), 2 == u && (a.value = "0" + r + l), u > 2)
+        {
+            for (ajd2 = "", j = 0, h = u - 3; h >= 0; h--)
+            {
+                if (3 == j)
+                {
                     ajd2 += e;
                     j = 0;
                 }
                 ajd2 += l.charAt(h);
                 j++;
             }
-            for (
-                a.value = '', tamanho2 = ajd2.length, h = tamanho2 - 1;
-                h >= 0;
-                h--
-            ) {
+            for (a.value = "", tamanho2 = ajd2.length, h = tamanho2 - 1; h >= 0; h--)
+            {
                 a.value += ajd2.charAt(h);
             }
             a.value += r + l.substr(u - 2, u);
         }
         return false;
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'moeda', error);
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "moeda", error);
         }
         return false;
     }
 }
 
-function lstOrgaoChange() {
-    try {
-        var lstEmpenhos = document.getElementById('lstEmpenhos');
-        if (lstEmpenhos?.ej2_instances?.[0]) {
+function lstOrgaoChange()
+{
+    try
+    {
+        var lstEmpenhos = document.getElementById("lstEmpenhos");
+        if (lstEmpenhos?.ej2_instances?.[0])
+        {
             lstEmpenhos.ej2_instances[0].dataSource = [];
             lstEmpenhos.ej2_instances[0].dataBind();
-            lstEmpenhos.ej2_instances[0].text = '';
-        }
-        $('#txtEmpenhoMultaId').attr('value', '');
-
-        var lstOrgao = document.getElementById('lstOrgao');
-        if (
-            !lstOrgao?.ej2_instances?.[0] ||
-            lstOrgao.ej2_instances[0].value === null
-        ) {
+            lstEmpenhos.ej2_instances[0].text = "";
+        }
+        $('#txtEmpenhoMultaId').attr('value', "");
+
+        var lstOrgao = document.getElementById("lstOrgao");
+        if (!lstOrgao?.ej2_instances?.[0] || lstOrgao.ej2_instances[0].value === null)
+        {
             return;
         }
 
         var orgaoid = String(lstOrgao.ej2_instances[0].value);
 
         $.ajax({
-            url: '/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Multa/UpsertPenalidade?handler=AJAXPreencheListaEmpenhos",
+            method: "GET",
+            datatype: "json",
             data: { id: orgaoid },
-            success: function (res) {
-                if (res.data.length != 0) {
+            success: function (res)
+            {
+                if (res.data.length != 0)
+                {
                     let EmpenhoList = [];
-                    for (var i = 0; i < res.data.length; ++i) {
+                    for (var i = 0; i < res.data.length; ++i)
+                    {
                         let empenho = {
                             EmpenhoMultaId: res.data[i].empenhoMultaId,
-                            NotaEmpenho: res.data[i].notaEmpenho,
+                            NotaEmpenho: res.data[i].notaEmpenho
                         };
                         EmpenhoList.push(empenho);
                     }
-                    if (lstEmpenhos?.ej2_instances?.[0]) {
+                    if (lstEmpenhos?.ej2_instances?.[0])
+                    {
                         lstEmpenhos.ej2_instances[0].dataSource = EmpenhoList;
                         lstEmpenhos.ej2_instances[0].dataBind();
                     }
                 }
-            },
+            }
         });
 
-        if (lstEmpenhos?.ej2_instances?.[0]) {
+        if (lstEmpenhos?.ej2_instances?.[0])
+        {
             lstEmpenhos.ej2_instances[0].refresh();
         }
 
         swal({
-            title: 'Empenho do Órgão',
-            text: 'Já existe o empenho correto cadastrado para o órgão?',
-            icon: 'info',
-            buttons: { ok: 'Ok' },
+            title: "Empenho do Órgão",
+            text: "Já existe o empenho correto cadastrado para o órgão?",
+            icon: "info",
+            buttons: { ok: "Ok" }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'lstOrgaoChange', error);
-        }
-    }
-}
-
-function lstEmpenhosChange() {
-    try {
-        var lstEmpenhos = document.getElementById('lstEmpenhos');
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstOrgaoChange", error);
+        }
+    }
+}
+
+function lstEmpenhosChange()
+{
+    try
+    {
+        var lstEmpenhos = document.getElementById("lstEmpenhos");
         if (!lstEmpenhos?.ej2_instances?.[0]) return;
 
-        $('#txtEmpenhoMultaId').attr(
-            'value',
-            lstEmpenhos.ej2_instances[0].value,
-        );
+        $('#txtEmpenhoMultaId').attr('value', lstEmpenhos.ej2_instances[0].value);
 
         var empenhoid = String(lstEmpenhos.ej2_instances[0].value);
 
         $.ajax({
-            url: '/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho',
-            method: 'GET',
-            datatype: 'json',
+            url: "/Multa/UpsertAutuacao?handler=PegaSaldoEmpenho",
+            method: "GET",
+            datatype: "json",
             data: { id: empenhoid },
-            success: function (res) {
+            success: function (res)
+            {
                 var saldoempenho = res.data;
-                $('#txtSaldoEmpenho').val(
-                    Intl.NumberFormat('pt-BR', {
-                        style: 'currency',
-                        currency: 'BRL',
-                    }).format(saldoempenho),
+                $("#txtSaldoEmpenho").val(
+                    Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(saldoempenho)
                 );
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstEmpenhosChange',
-                error,
-            );
-        }
-    }
-}
-
-function lstVeiculo_Select() {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstEmpenhosChange", error);
+        }
+    }
+}
+
+function lstVeiculo_Select()
+{
+    try
+    {
         EscolhendoVeiculo = true;
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstVeiculo_Select',
-                error,
-            );
-        }
-    }
-}
-
-function lstVeiculo_Change() {
-    try {
-        var lstVeiculo = document.getElementById('lstVeiculo');
-        if (
-            !lstVeiculo?.ej2_instances?.[0] ||
-            lstVeiculo.ej2_instances[0].value === ''
-        ) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstVeiculo_Select", error);
+        }
+    }
+}
+
+function lstVeiculo_Change()
+{
+    try
+    {
+        var lstVeiculo = document.getElementById("lstVeiculo");
+        if (!lstVeiculo?.ej2_instances?.[0] || lstVeiculo.ej2_instances[0].value === '')
+        {
             return;
         }
 
         var veiculoId = lstVeiculo.ej2_instances[0].value;
 
         $.ajax({
-            url: '/api/Multa/PegaInstrumentoVeiculo',
-            method: 'GET',
-            datatype: 'json',
+            url: "/api/Multa/PegaInstrumentoVeiculo",
+            method: "GET",
+            datatype: "json",
             data: { Id: veiculoId },
-            success: function (data) {
-                var lstContratoVeiculo =
-                    document.getElementById('lstContratoVeiculo');
-                var lstAtaVeiculo = document.getElementById('lstAtaVeiculo');
-
-                if (data.instrumentoid != null) {
-                    if (data.instrumento == 'contrato') {
-                        if (lstContratoVeiculo?.ej2_instances?.[0]) {
-                            lstContratoVeiculo.ej2_instances[0].value =
-                                data.instrumentoid;
+            success: function (data)
+            {
+                var lstContratoVeiculo = document.getElementById("lstContratoVeiculo");
+                var lstAtaVeiculo = document.getElementById("lstAtaVeiculo");
+
+                if (data.instrumentoid != null)
+                {
+                    if (data.instrumento == "contrato")
+                    {
+                        if (lstContratoVeiculo?.ej2_instances?.[0])
+                        {
+                            lstContratoVeiculo.ej2_instances[0].value = data.instrumentoid;
                         }
-                        if (lstAtaVeiculo?.ej2_instances?.[0]) {
+                        if (lstAtaVeiculo?.ej2_instances?.[0])
+                        {
                             lstAtaVeiculo.ej2_instances[0].value = '';
                         }
-                    } else {
-                        if (lstContratoVeiculo?.ej2_instances?.[0]) {
+                    } else
+                    {
+                        if (lstContratoVeiculo?.ej2_instances?.[0])
+                        {
                             lstContratoVeiculo.ej2_instances[0].value = '';
                         }
-                        if (lstAtaVeiculo?.ej2_instances?.[0]) {
-                            lstAtaVeiculo.ej2_instances[0].value =
-                                data.instrumentoid;
+                        if (lstAtaVeiculo?.ej2_instances?.[0])
+                        {
+                            lstAtaVeiculo.ej2_instances[0].value = data.instrumentoid;
                         }
                     }
-                } else {
-                    if (lstContratoVeiculo?.ej2_instances?.[0]) {
+                } else
+                {
+                    if (lstContratoVeiculo?.ej2_instances?.[0])
+                    {
                         lstContratoVeiculo.ej2_instances[0].value = '';
                     }
                     swal({
-                        title: 'Atenção ao Contrato do Veículo',
-                        text: 'O veículo escolhido não possui contrato/ata!',
-                        icon: 'info',
-                        buttons: { ok: 'Ok' },
+                        title: "Atenção ao Contrato do Veículo",
+                        text: "O veículo escolhido não possui contrato/ata!",
+                        icon: "info",
+                        buttons: { ok: "Ok" }
                     });
                 }
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstVeiculo_Change',
-                error,
-            );
-        }
-    }
-}
-
-function lstContratoVeiculo_Change() {
-    try {
-        if (EscolhendoVeiculo) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstVeiculo_Change", error);
+        }
+    }
+}
+
+function lstContratoVeiculo_Change()
+{
+    try
+    {
+        if (EscolhendoVeiculo)
+        {
             EscolhendoVeiculo = false;
             return;
         }
 
-        var lstAtaVeiculo = document.getElementById('lstAtaVeiculo');
-        if (lstAtaVeiculo?.ej2_instances?.[0]) {
+        var lstAtaVeiculo = document.getElementById("lstAtaVeiculo");
+        if (lstAtaVeiculo?.ej2_instances?.[0])
+        {
             lstAtaVeiculo.ej2_instances[0].value = '';
         }
 
-        var lstContratoVeiculo = document.getElementById('lstContratoVeiculo');
-        var lstVeiculo = document.getElementById('lstVeiculo');
-
-        if (
-            !lstContratoVeiculo?.ej2_instances?.[0] ||
-            !lstVeiculo?.ej2_instances?.[0]
-        )
-            return;
-        if (
-            lstContratoVeiculo.ej2_instances[0].value === '' ||
-            lstVeiculo.ej2_instances[0].value === ''
-        )
-            return;
+        var lstContratoVeiculo = document.getElementById("lstContratoVeiculo");
+        var lstVeiculo = document.getElementById("lstVeiculo");
+
+        if (!lstContratoVeiculo?.ej2_instances?.[0] || !lstVeiculo?.ej2_instances?.[0]) return;
+        if (lstContratoVeiculo.ej2_instances[0].value === '' || lstVeiculo.ej2_instances[0].value === '') return;
 
         var veiculoId = lstVeiculo.ej2_instances[0].value;
         var contratoId = lstContratoVeiculo.ej2_instances[0].value;
 
         $.ajax({
-            url: '/api/Multa/ValidaContratoVeiculo',
-            method: 'GET',
-            datatype: 'json',
+            url: "/api/Multa/ValidaContratoVeiculo",
+            method: "GET",
+            datatype: "json",
             data: { veiculoId: veiculoId, contratoId: contratoId },
-            success: function (data) {
-                if (data.success === false) {
+            success: function (data)
+            {
+                if (data.success === false)
+                {
                     swal({
-                        title: 'Alerta no Contrato do Veículo',
-                        text: 'O veículo escolhido não pertence a esse contrato!',
-                        icon: 'warning',
-                        buttons: { ok: 'Ok' },
+                        title: "Alerta no Contrato do Veículo",
+                        text: "O veículo escolhido não pertence a esse contrato!",
+                        icon: "warning",
+                        buttons: { ok: "Ok" }
                     });
-                    if (lstVeiculo?.ej2_instances?.[0]) {
+                    if (lstVeiculo?.ej2_instances?.[0])
+                    {
                         lstVeiculo.ej2_instances[0].value = '';
                     }
                 }
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstContratoVeiculo_Change',
-                error,
-            );
-        }
-    }
-}
-
-function lstAtaVeiculo_Change() {
-    try {
-        if (EscolhendoVeiculo) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstContratoVeiculo_Change", error);
+        }
+    }
+}
+
+function lstAtaVeiculo_Change()
+{
+    try
+    {
+        if (EscolhendoVeiculo)
+        {
             EscolhendoVeiculo = false;
             return;
         }
 
-        var lstContratoVeiculo = document.getElementById('lstContratoVeiculo');
-        if (lstContratoVeiculo?.ej2_instances?.[0]) {
+        var lstContratoVeiculo = document.getElementById("lstContratoVeiculo");
+        if (lstContratoVeiculo?.ej2_instances?.[0])
+        {
             lstContratoVeiculo.ej2_instances[0].value = '';
         }
 
-        var lstAtaVeiculo = document.getElementById('lstAtaVeiculo');
-        var lstVeiculo = document.getElementById('lstVeiculo');
-
-        if (
-            !lstAtaVeiculo?.ej2_instances?.[0] ||
-            !lstVeiculo?.ej2_instances?.[0]
-        )
-            return;
-        if (
-            lstAtaVeiculo.ej2_instances[0].value === '' ||
-            lstVeiculo.ej2_instances[0].value === ''
-        )
-            return;
+        var lstAtaVeiculo = document.getElementById("lstAtaVeiculo");
+        var lstVeiculo = document.getElementById("lstVeiculo");
+
+        if (!lstAtaVeiculo?.ej2_instances?.[0] || !lstVeiculo?.ej2_instances?.[0]) return;
+        if (lstAtaVeiculo.ej2_instances[0].value === '' || lstVeiculo.ej2_instances[0].value === '') return;
 
         var veiculoId = lstVeiculo.ej2_instances[0].value;
         var ataId = lstAtaVeiculo.ej2_instances[0].value;
 
         $.ajax({
-            url: '/api/Multa/ValidaAtaVeiculo',
-            method: 'GET',
-            datatype: 'json',
+            url: "/api/Multa/ValidaAtaVeiculo",
+            method: "GET",
+            datatype: "json",
             data: { veiculoId: veiculoId, ataId: ataId },
-            success: function (data) {
-                if (data.success === false) {
+            success: function (data)
+            {
+                if (data.success === false)
+                {
                     swal({
-                        title: 'Alerta na Ata do Veículo',
-                        text: 'O veículo escolhido não pertence a essa ata!',
-                        icon: 'warning',
-                        buttons: { ok: 'Ok' },
+                        title: "Alerta na Ata do Veículo",
+                        text: "O veículo escolhido não pertence a essa ata!",
+                        icon: "warning",
+                        buttons: { ok: "Ok" }
                     });
-                    if (lstVeiculo?.ej2_instances?.[0]) {
+                    if (lstVeiculo?.ej2_instances?.[0])
+                    {
                         lstVeiculo.ej2_instances[0].value = '';
                     }
                 }
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstAtaVeiculo_Change',
-                error,
-            );
-        }
-    }
-}
-
-function lstMotorista_Select() {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstAtaVeiculo_Change", error);
+        }
+    }
+}
+
+function lstMotorista_Select()
+{
+    try
+    {
         EscolhendoMotorista = true;
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstMotorista_Select',
-                error,
-            );
-        }
-    }
-}
-
-function lstMotorista_Change() {
-    try {
-        var lstMotorista = document.getElementById('lstMotorista');
-        if (
-            !lstMotorista?.ej2_instances?.[0] ||
-            lstMotorista.ej2_instances[0].value === ''
-        ) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstMotorista_Select", error);
+        }
+    }
+}
+
+function lstMotorista_Change()
+{
+    try
+    {
+        var lstMotorista = document.getElementById("lstMotorista");
+        if (!lstMotorista?.ej2_instances?.[0] || lstMotorista.ej2_instances[0].value === '')
+        {
             return;
         }
 
         var motoristaId = lstMotorista.ej2_instances[0].value;
 
         $.ajax({
-            url: '/api/Multa/PegaContratoMotorista',
-            method: 'GET',
-            datatype: 'json',
+            url: "/api/Multa/PegaContratoMotorista",
+            method: "GET",
+            datatype: "json",
             data: { Id: motoristaId },
-            success: function (data) {
-                var lstContratoMotorista = document.getElementById(
-                    'lstContratoMotorista',
-                );
-
-                if (data.contratoid != '') {
-                    if (lstContratoMotorista?.ej2_instances?.[0]) {
-                        lstContratoMotorista.ej2_instances[0].value =
-                            data.contratoid;
-                    }
-                } else {
-                    if (lstContratoMotorista?.ej2_instances?.[0]) {
+            success: function (data)
+            {
+                var lstContratoMotorista = document.getElementById("lstContratoMotorista");
+
+                if (data.contratoid != '')
+                {
+                    if (lstContratoMotorista?.ej2_instances?.[0])
+                    {
+                        lstContratoMotorista.ej2_instances[0].value = data.contratoid;
+                    }
+                } else
+                {
+                    if (lstContratoMotorista?.ej2_instances?.[0])
+                    {
                         lstContratoMotorista.ej2_instances[0].value = '';
                     }
                     swal({
-                        title: 'Atenção ao Contrato do Motorista',
-                        text: 'O motorista escolhido não possui contrato!',
-                        icon: 'info',
-                        buttons: { ok: 'Ok' },
+                        title: "Atenção ao Contrato do Motorista",
+                        text: "O motorista escolhido não possui contrato!",
+                        icon: "info",
+                        buttons: { ok: "Ok" }
                     });
                 }
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstMotorista_Change',
-                error,
-            );
-        }
-    }
-}
-
-function lstContratoMotorista_Change() {
-    try {
-        if (EscolhendoMotorista) {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstMotorista_Change", error);
+        }
+    }
+}
+
+function lstContratoMotorista_Change()
+{
+    try
+    {
+        if (EscolhendoMotorista)
+        {
             EscolhendoMotorista = false;
             return;
         }
 
-        var lstContratoMotorista = document.getElementById(
-            'lstContratoMotorista',
-        );
-        var lstMotorista = document.getElementById('lstMotorista');
-
-        if (
-            !lstContratoMotorista?.ej2_instances?.[0] ||
-            !lstMotorista?.ej2_instances?.[0]
-        )
-            return;
-        if (
-            lstContratoMotorista.ej2_instances[0].value === '' ||
-            lstMotorista.ej2_instances[0].value === ''
-        )
-            return;
+        var lstContratoMotorista = document.getElementById("lstContratoMotorista");
+        var lstMotorista = document.getElementById("lstMotorista");
+
+        if (!lstContratoMotorista?.ej2_instances?.[0] || !lstMotorista?.ej2_instances?.[0]) return;
+        if (lstContratoMotorista.ej2_instances[0].value === '' || lstMotorista.ej2_instances[0].value === '') return;
 
         var motoristaId = lstMotorista.ej2_instances[0].value;
         var contratoId = lstContratoMotorista.ej2_instances[0].value;
 
         $.ajax({
-            url: '/api/Multa/ValidaContratoMotorista',
-            method: 'GET',
-            datatype: 'json',
+            url: "/api/Multa/ValidaContratoMotorista",
+            method: "GET",
+            datatype: "json",
             data: { veiculoId: motoristaId, contratoId: contratoId },
-            success: function (data) {
-                if (data.success === false) {
+            success: function (data)
+            {
+                if (data.success === false)
+                {
                     swal({
-                        title: 'Alerta no Contrato do Motorista',
-                        text: 'O motorista escolhido não pertence a esse contrato!',
-                        icon: 'warning',
-                        buttons: { ok: 'Ok' },
+                        title: "Alerta no Contrato do Motorista",
+                        text: "O motorista escolhido não pertence a esse contrato!",
+                        icon: "warning",
+                        buttons: { ok: "Ok" }
                     });
-                    if (lstMotorista?.ej2_instances?.[0]) {
+                    if (lstMotorista?.ej2_instances?.[0])
+                    {
                         lstMotorista.ej2_instances[0].value = '';
                     }
                 }
-            },
+            }
         });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                'lstContratoMotorista_Change',
-                error,
-            );
-        }
-    }
-}
-
-$('#btnFecharModalComprovante').click(function () {
-    try {
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "lstContratoMotorista_Change", error);
+        }
+    }
+}
+
+$("#btnFecharModalComprovante").click(function ()
+{
+    try
+    {
         $('.modal-backdrop').hide();
         $('body').removeClass('modal-open');
-        $('#modalComprovante').hide();
-        $('body').css({ overflow: 'visible' });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                '#btnFecharModalComprovante.click',
-                error,
-            );
+        $("#modalComprovante").hide();
+        $('body').css({ 'overflow': 'visible' });
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "#btnFecharModalComprovante.click", error);
         }
     }
 });
 
-$('#btnFecharModalFichaVistoria').click(function () {
-    try {
+$("#btnFecharModalFichaVistoria").click(function ()
+{
+    try
+    {
         $('.modal-backdrop').hide();
         $('body').removeClass('modal-open');
-        $('#modalFicha').hide();
-        $('body').css({ overflow: 'visible' });
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                '#btnFecharModalFichaVistoria.click',
-                error,
-            );
+        $("#modalFicha").hide();
+        $('body').css({ 'overflow': 'visible' });
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "#btnFecharModalFichaVistoria.click", error);
         }
     }
 });
 
-$('#btnSubmit').click(function (event) {
-    try {
+$("#btnSubmit").click(function (event)
+{
+    try
+    {
         event.preventDefault();
 
-        if (document.getElementById('txtNumInfracao').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O número da Infração é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (document.getElementById('txtDataInfracao').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Data da Infração é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (document.getElementById('txtHoraInfracao').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Hora da Infração é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (document.getElementById('txtDataNotificacao').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Data da Notificação é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (document.getElementById('txtDataLimite').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Data Limite para Interposição de Defesa é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        var lstStatus = document.getElementById('lstStatus');
-        if (
-            lstStatus?.ej2_instances?.[0] &&
-            lstStatus.ej2_instances[0].value === null
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O Status é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (document.getElementById('txtLocalizacao').value === '') {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Localização da Infração é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        var lstInfracao = document.getElementById('lstInfracao');
-        if (
-            lstInfracao?.ej2_instances?.[0] &&
-            lstInfracao.ej2_instances[0].value === null
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'A Infração é obrigatória',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        var lstOrgao = document.getElementById('lstOrgao');
-        if (
-            lstOrgao?.ej2_instances?.[0] &&
-            lstOrgao.ej2_instances[0].value === null
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O Órgão Autuante é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        var lstVeiculo = document.getElementById('lstVeiculo');
-        if (
-            lstVeiculo?.ej2_instances?.[0] &&
-            lstVeiculo.ej2_instances[0].value === null
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O Veículo é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (
-            document.getElementById('txtValorAteVencimento').value === '' ||
-            document.getElementById('txtValorAteVencimento').value === '0'
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O Valor Até o Vencimento é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        if (
-            document.getElementById('txtValorPosVencimento').value === '' ||
-            document.getElementById('txtValorPosVencimento').value === '0'
-        ) {
-            swal({
-                title: 'Informação Ausente',
-                text: 'O Valor Após o Vencimento é obrigatório',
-                icon: 'error',
-                buttons: { ok: 'Ok' },
-            });
-            return;
-        }
-
-        $('#btnEscondido').click();
-        $('#btnSubmit').prop('disabled', true);
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multa.js',
-                '#btnSubmit.click',
-                error,
-            );
+        if (document.getElementById("txtNumInfracao").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "O número da Infração é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtDataInfracao").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "A Data da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtHoraInfracao").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "A Hora da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtDataNotificacao").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "A Data da Notificação é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtDataLimite").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "A Data Limite para Interposição de Defesa é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        var lstStatus = document.getElementById("lstStatus");
+        if (lstStatus?.ej2_instances?.[0] && lstStatus.ej2_instances[0].value === null)
+        {
+            swal({ title: "Informação Ausente", text: "O Status é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtLocalizacao").value === "")
+        {
+            swal({ title: "Informação Ausente", text: "A Localização da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        var lstInfracao = document.getElementById("lstInfracao");
+        if (lstInfracao?.ej2_instances?.[0] && lstInfracao.ej2_instances[0].value === null)
+        {
+            swal({ title: "Informação Ausente", text: "A Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        var lstOrgao = document.getElementById("lstOrgao");
+        if (lstOrgao?.ej2_instances?.[0] && lstOrgao.ej2_instances[0].value === null)
+        {
+            swal({ title: "Informação Ausente", text: "O Órgão Autuante é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        var lstVeiculo = document.getElementById("lstVeiculo");
+        if (lstVeiculo?.ej2_instances?.[0] && lstVeiculo.ej2_instances[0].value === null)
+        {
+            swal({ title: "Informação Ausente", text: "O Veículo é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtValorAteVencimento").value === "" || document.getElementById("txtValorAteVencimento").value === "0")
+        {
+            swal({ title: "Informação Ausente", text: "O Valor Até o Vencimento é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        if (document.getElementById("txtValorPosVencimento").value === "" || document.getElementById("txtValorPosVencimento").value === "0")
+        {
+            swal({ title: "Informação Ausente", text: "O Valor Após o Vencimento é obrigatório", icon: "error", buttons: { ok: "Ok" } });
+            return;
+        }
+
+        $("#btnEscondido").click();
+        $("#btnSubmit").prop("disabled", true);
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "#btnSubmit.click", error);
         }
     }
 });
 
-$(document).on('click', '.btnViagem', function () {
-    if (document.getElementById('txtDataInfracao').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Data da Infração deve ser informada!',
-            icon: 'error',
-            buttons: { ok: 'Ok' },
-        });
+$(document).on('click', '.btnViagem', function ()
+{
+    if (document.getElementById("txtDataInfracao").value === "")
+    {
+        swal({ title: "Informação Ausente", text: "A Data da Infração deve ser informada!", icon: "error", buttons: { ok: "Ok" } });
         return;
     }
 
-    if (document.getElementById('txtHoraInfracao').value === '') {
-        swal({
-            title: 'Informação Ausente',
-            text: 'A Hora da Infração é obrigatória',
-            icon: 'error',
-            buttons: { ok: 'Ok' },
-        });
+    if (document.getElementById("txtHoraInfracao").value === "")
+    {
+        swal({ title: "Informação Ausente", text: "A Hora da Infração é obrigatória", icon: "error", buttons: { ok: "Ok" } });
         return;
     }
 
-    var lstVeiculo = document.getElementById('lstVeiculo');
-    if (
-        !lstVeiculo?.ej2_instances?.[0] ||
-        lstVeiculo.ej2_instances[0].value === null
-    ) {
-        swal({
-            title: 'Informação Ausente',
-            text: 'O Veículo deve ser informado!',
-            icon: 'error',
-            buttons: { ok: 'Ok' },
-        });
+    var lstVeiculo = document.getElementById("lstVeiculo");
+    if (!lstVeiculo?.ej2_instances?.[0] || lstVeiculo.ej2_instances[0].value === null)
+    {
+        swal({ title: "Informação Ausente", text: "O Veículo deve ser informado!", icon: "error", buttons: { ok: "Ok" } });
         return;
     }
 
     var dataToPost = JSON.stringify({
-        VeiculoId: lstVeiculo.ej2_instances[0].value,
-        Data: document.getElementById('txtDataInfracao').value,
-        Hora: document.getElementById('txtHoraInfracao').value,
+        'VeiculoId': lstVeiculo.ej2_instances[0].value,
+        'Data': document.getElementById("txtDataInfracao").value,
+        'Hora': document.getElementById("txtHoraInfracao").value
     });
 
     $.ajax({
         url: '/api/Multa/ProcuraViagem',
-        type: 'POST',
+        type: "POST",
         data: dataToPost,
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        success: function (data) {
-            try {
-                if (data.success) {
-                    if (window.AppToast?.show) {
+        contentType: "application/json; charset=utf-8",
+        dataType: "json",
+        success: function (data)
+        {
+            try
+            {
+                if (data.success)
+                {
+                    if (window.AppToast?.show)
+                    {
                         AppToast.show('Verde', data.message);
                     }
-                    $('#txtNoFichaVistoria').attr(
-                        'value',
-                        data.nofichavistoria,
-                    );
-                    $('#txtNoFichaVistoriaEscondido').attr(
-                        'value',
-                        data.nofichavistoria,
-                    );
+                    $('#txtNoFichaVistoria').attr('value', data.nofichavistoria);
+                    $('#txtNoFichaVistoriaEscondido').attr('value', data.nofichavistoria);
 
                     EscolhendoMotorista = true;
-                    var lstMotorista = document.getElementById('lstMotorista');
-                    if (lstMotorista?.ej2_instances?.[0]) {
+                    var lstMotorista = document.getElementById("lstMotorista");
+                    if (lstMotorista?.ej2_instances?.[0])
+                    {
                         lstMotorista.ej2_instances[0].value = data.motoristaid;
                     }
-                } else {
+                } else
+                {
                     $('#txtNoFichaVistoria').attr('value', '');
                     $('#txtNoFichaVistoriaEscondido').attr('value', '');
-                    if (window.AppToast?.show) {
+                    if (window.AppToast?.show)
+                    {
                         AppToast.show('Vermelho', data.message);
                     }
                 }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multa.js',
-                        'ajax.success',
-                        error,
-                    );
+            } catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
                 }
             }
         },
-        error: function (err) {
+        error: function (err)
+        {
             console.log(err);
             alert('Algo deu errado');
-        },
+        }
     });
 });
 
-$(document).on('click', '.btnFicha', function () {
-    var dataToPost = JSON.stringify({
-        NoFichaVistoria: document.getElementById('txtNoFichaVistoria').value,
-    });
+$(document).on('click', '.btnFicha', function ()
+{
+    var dataToPost = JSON.stringify({ 'NoFichaVistoria': document.getElementById("txtNoFichaVistoria").value });
 
     $.ajax({
         url: '/api/Multa/ProcuraFicha',
-        type: 'POST',
+        type: "POST",
         data: dataToPost,
-        contentType: 'application/json; charset=utf-8',
-        dataType: 'json',
-        success: function (data) {
-            try {
-                if (data.success) {
+        contentType: "application/json; charset=utf-8",
+        dataType: "json",
+        success: function (data)
+        {
+            try
+            {
+                if (data.success)
+                {
                     ViagemId = data.viagemid;
-                    if (window.AppToast?.show) {
+                    if (window.AppToast?.show)
+                    {
                         AppToast.show('Verde', data.message);
                     }
                     $('#modalFicha').modal('show');
-                } else {
-                    if (window.AppToast?.show) {
+                } else
+                {
+                    if (window.AppToast?.show)
+                    {
                         AppToast.show('Vermelho', data.message);
                     }
                 }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multa.js',
-                        'ajax.success',
-                        error,
-                    );
+            } catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
                 }
             }
         },
-        error: function (err) {
+        error: function (err)
+        {
             console.log(err);
             alert('Algo deu errado');
-        },
+        }
     });
 });
 
-$('#modalFicha')
-    .modal({
-        keyboard: true,
-        backdrop: 'static',
-        show: false,
-    })
-    .on('show.bs.modal', function () {
-        try {
-            var id = ViagemId;
-            var label = document.getElementById('DynamicModalLabelFicha');
-            label.innerHTML = '';
-
-            $.ajax({
-                type: 'get',
-                url: '/api/Viagem/PegaFichaModal',
-                data: { id: id },
-                async: false,
-                success: function (res) {
-                    var fichavistoria =
-                        document.getElementById('txtNoFichaVistoria').value;
-                    $('#imgViewer').removeAttr('src');
-
-                    if (res === false) {
-                        label.innerHTML = 'Infração sem Autuação digitalizada';
-                        $('#imgViewer').attr(
-                            'src',
-                            '/Images/FichaAmarelaNova.jpg',
-                        );
-                    } else {
-                        label.innerHTML =
-                            'Ficha de Vistoria Nº: <b>' +
-                            fichavistoria +
-                            '</b>';
-                        $('#imgViewer').attr(
-                            'src',
-                            'data:image/jpg;base64,' + res,
-                        );
-                    }
-                },
-            });
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multa.js',
-                    'on show.bs.modal',
-                    error,
-                );
-            }
-        }
-    })
-    .on('hide.bs.modal', function () {
-        try {
-            $('#imgViewer').removeAttr('src');
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multa.js',
-                    'on hide.bs.modal',
-                    error,
-                );
-            }
-        }
-    });
-
-$('#txtNumInfracao').focusout(function () {
-    if (document.getElementById('txtNumInfracao').value === '') return;
-
-    var NumInfracao = document.getElementById('txtNumInfracao').value;
+$("#modalFicha").modal({
+    keyboard: true,
+    backdrop: "static",
+    show: false
+}).on("show.bs.modal", function ()
+{
+    try
+    {
+        var id = ViagemId;
+        var label = document.getElementById("DynamicModalLabelFicha");
+        label.innerHTML = "";
+
+        $.ajax({
+            type: "get",
+            url: "/api/Viagem/PegaFichaModal",
+            data: { id: id },
+            async: false,
+            success: function (res)
+            {
+                var fichavistoria = document.getElementById("txtNoFichaVistoria").value;
+                $('#imgViewer').removeAttr("src");
+
+                if (res === false)
+                {
+                    label.innerHTML = "Infração sem Autuação digitalizada";
+                    $('#imgViewer').attr('src', "/Images/FichaAmarelaNova.jpg");
+                } else
+                {
+                    label.innerHTML = "Ficha de Vistoria Nº: <b>" + fichavistoria + "</b>";
+                    $('#imgViewer').attr('src', "data:image/jpg;base64," + res);
+                }
+            }
+        });
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "on show.bs.modal", error);
+        }
+    }
+}).on("hide.bs.modal", function ()
+{
+    try
+    {
+        $('#imgViewer').removeAttr("src");
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "on hide.bs.modal", error);
+        }
+    }
+});
+
+$("#txtNumInfracao").focusout(function ()
+{
+    if (document.getElementById("txtNumInfracao").value === '') return;
+
+    var NumInfracao = document.getElementById("txtNumInfracao").value;
 
     $.ajax({
-        url: '/api/Multa/MultaExistente',
-        method: 'GET',
-        datatype: 'json',
+        url: "/api/Multa/MultaExistente",
+        method: "GET",
+        datatype: "json",
         data: { numinfracao: NumInfracao },
-        success: function (res) {
-            try {
+        success: function (res)
+        {
+            try
+            {
                 var ExisteFicha = [res.data];
-                if (ExisteFicha[0] === true) {
+                if (ExisteFicha[0] === true)
+                {
                     swal({
-                        title: 'Alerta no Número da Infração',
-                        text: 'Já existe uma Multa inserida com esta numeração!',
-                        icon: 'warning',
-                        buttons: { ok: 'Ok' },
+                        title: "Alerta no Número da Infração",
+                        text: "Já existe uma Multa inserida com esta numeração!",
+                        icon: "warning",
+                        buttons: { ok: "Ok" }
                     });
                 }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multa.js',
-                        'ajax.success',
-                        error,
-                    );
-                }
-            }
-        },
+            } catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multa.js", "ajax.success", error);
+                }
+            }
+        }
     });
 });
 
-$(document).on('click', '.btnComprovante', function () {
+$(document).on('click', '.btnComprovante', function ()
+{
     $('#modalComprovante').modal('show');
 });
 
-$('#modalComprovante')
-    .modal({
-        keyboard: true,
-        show: false,
-    })
-    .on('show.bs.modal', function () {
-        try {
-            var comprovantePath =
-                document.getElementById('txtComprovantePDF').value;
-            if (comprovantePath && comprovantePath != '') {
-                loadPdfInViewer(comprovantePath);
-            }
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multa.js',
-                    'on show.bs.modal',
-                    error,
-                );
-            }
-        }
-    })
-    .on('hide.bs.modal', function () {
-        try {
-            var uploaderElement = document.getElementById('flComprovante');
-            if (uploaderElement?.ej2_instances?.[0]) {
-                uploaderElement.ej2_instances[0].clearAll();
-            }
-            $('div').removeClass('modal-backdrop');
-            $('body').removeClass('modal-open');
-            $('body').css('overflow', 'auto');
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multa.js',
-                    'on hide.bs.modal',
-                    error,
-                );
-            }
-        }
-    });
-
-$(document).on('click', '.btnNotificacao', function () {
+$("#modalComprovante").modal({
+    keyboard: true,
+    show: false
+}).on("show.bs.modal", function ()
+{
+    try
+    {
+        var comprovantePath = document.getElementById("txtComprovantePDF").value;
+        if (comprovantePath && comprovantePath != '')
+        {
+            loadPdfInViewer(comprovantePath);
+        }
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "on show.bs.modal", error);
+        }
+    }
+}).on("hide.bs.modal", function ()
+{
+    try
+    {
+        var uploaderElement = document.getElementById("flComprovante");
+        if (uploaderElement?.ej2_instances?.[0])
+        {
+            uploaderElement.ej2_instances[0].clearAll();
+        }
+        $("div").removeClass("modal-backdrop");
+        $('body').removeClass('modal-open');
+        $("body").css("overflow", "auto");
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "on hide.bs.modal", error);
+        }
+    }
+});
+
+$(document).on('click', '.btnNotificacao', function ()
+{
     $('#modalAutuacao').modal('show');
 });
 
-function toolbarClick(e) {
-    try {
-        if (e.item.id == 'rte_toolbar_Image') {
+function toolbarClick(e)
+{
+    try
+    {
+        if (e.item.id == "rte_toolbar_Image")
+        {
             var element = document.getElementById('rte_upload');
-            if (element?.ej2_instances?.[0]) {
-                element.ej2_instances[0].uploading = function upload(args) {
-                    args.currentRequest.setRequestHeader(
-                        'XSRF-TOKEN',
-                        document.getElementsByName(
-                            '__RequestVerificationToken',
-                        )[0].value,
-                    );
+            if (element?.ej2_instances?.[0])
+            {
+                element.ej2_instances[0].uploading = function upload(args)
+                {
+                    args.currentRequest.setRequestHeader('XSRF-TOKEN',
+                        document.getElementsByName('__RequestVerificationToken')[0].value);
                 };
             }
         }
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha('multa.js', 'toolbarClick', error);
+    } catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multa.js", "toolbarClick", error);
         }
     }
 }
 
 ej.base.L10n.load({
-    'pt-BR': {
-        richtexteditor: {
-            alignments: 'Alinhamentos',
-            justifyLeft: 'Alinhar à Esquerda',
-            justifyCenter: 'Centralizar',
-            justifyRight: 'Alinhar à Direita',
-            justifyFull: 'Justificar',
-            fontName: 'Nome da Fonte',
-            fontSize: 'Tamanho da Fonte',
-            fontColor: 'Cor da Fonte',
-            backgroundColor: 'Cor de Fundo',
-            bold: 'Negrito',
-            italic: 'Itálico',
-            underline: 'Sublinhado',
-            strikethrough: 'Tachado',
-            clearFormat: 'Limpar Formatação',
-            clearAll: 'Limpar Tudo',
-            cut: 'Cortar',
-            copy: 'Copiar',
-            paste: 'Colar',
-            unorderedList: 'Lista com Marcadores',
-            orderedList: 'Lista Numerada',
-            indent: 'Aumentar Indentação',
-            outdent: 'Diminuir Indentação',
-            undo: 'Desfazer',
-            redo: 'Refazer',
-            superscript: 'Sobrescrito',
-            subscript: 'Subscrito',
-            createLink: 'Inserir Link',
-            openLink: 'Abrir Link',
-            editLink: 'Editar Link',
-            removeLink: 'Remover Link',
-            image: 'Inserir Imagem',
-            replace: 'Substituir',
-            align: 'Alinhar',
-            caption: 'Título da Imagem',
-            remove: 'Remover',
-            insertLink: 'Inserir Link',
-            display: 'Exibir',
-            altText: 'Texto Alternativo',
-            dimension: 'Mudar Tamanho',
-            fullscreen: 'Maximizar',
-            maximize: 'Maximizar',
-            minimize: 'Minimizar',
-            print: 'Imprimir',
-            formats: 'Formatos',
-            sourcecode: 'Visualizar Código',
-            preview: 'Exibir',
-        },
-    },
+    "pt-BR": {
+        "richtexteditor": {
+            "alignments": "Alinhamentos",
+            "justifyLeft": "Alinhar à Esquerda",
+            "justifyCenter": "Centralizar",
+            "justifyRight": "Alinhar à Direita",
+            "justifyFull": "Justificar",
+            "fontName": "Nome da Fonte",
+            "fontSize": "Tamanho da Fonte",
+            "fontColor": "Cor da Fonte",
+            "backgroundColor": "Cor de Fundo",
+            "bold": "Negrito",
+            "italic": "Itálico",
+            "underline": "Sublinhado",
+            "strikethrough": "Tachado",
+            "clearFormat": "Limpar Formatação",
+            "clearAll": "Limpar Tudo",
+            "cut": "Cortar",
+            "copy": "Copiar",
+            "paste": "Colar",
+            "unorderedList": "Lista com Marcadores",
+            "orderedList": "Lista Numerada",
+            "indent": "Aumentar Indentação",
+            "outdent": "Diminuir Indentação",
+            "undo": "Desfazer",
+            "redo": "Refazer",
+            "superscript": "Sobrescrito",
+            "subscript": "Subscrito",
+            "createLink": "Inserir Link",
+            "openLink": "Abrir Link",
+            "editLink": "Editar Link",
+            "removeLink": "Remover Link",
+            "image": "Inserir Imagem",
+            "replace": "Substituir",
+            "align": "Alinhar",
+            "caption": "Título da Imagem",
+            "remove": "Remover",
+            "insertLink": "Inserir Link",
+            "display": "Exibir",
+            "altText": "Texto Alternativo",
+            "dimension": "Mudar Tamanho",
+            "fullscreen": "Maximizar",
+            "maximize": "Maximizar",
+            "minimize": "Minimizar",
+            "print": "Imprimir",
+            "formats": "Formatos",
+            "sourcecode": "Visualizar Código",
+            "preview": "Exibir"
+        }
+    }
 });
```
