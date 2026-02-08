# wwwroot/js/cadastros/multas-upload-handler.js

**Mudanca:** GRANDE | **+157** linhas | **-164** linhas

---

```diff
--- JANEIRO: wwwroot/js/cadastros/multas-upload-handler.js
+++ ATUAL: wwwroot/js/cadastros/multas-upload-handler.js
@@ -1,157 +1,158 @@
-var MultasUpload = (function () {
+var MultasUpload = (function ()
+{
     'use strict';
 
-    function getViewer(viewerId) {
-        try {
-
-            return (
-                document.getElementById(viewerId)?.ej2_instances?.[0] || null
-            );
-        } catch (err) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multas-upload-handler.js',
-                    'getViewer',
-                    err,
-                );
+    function getViewer(viewerId)
+    {
+        try
+        {
+            return document.getElementById(viewerId)?.ej2_instances?.[0] || null;
+        }
+        catch (err)
+        {
+            if (window.Alerta?.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("multas-upload-handler.js", "getViewer", err);
             }
             return null;
         }
     }
 
-    function loadPdfInViewer(fileName, viewerId) {
-        try {
-
-            if (!fileName || fileName === '' || fileName === 'null') {
-                console.warn(
-                    'Nome de arquivo inválido para carregar no viewer',
-                );
+    function loadPdfInViewer(fileName, viewerId)
+    {
+        try
+        {
+            if (!fileName || fileName === '' || fileName === 'null')
+            {
+                console.warn("Nome de arquivo inválido para carregar no viewer");
                 return;
             }
 
             const viewer = getViewer(viewerId);
-            if (!viewer) {
-                console.error('Viewer não encontrado:', viewerId);
+            if (!viewer)
+            {
+                console.error("Viewer não encontrado:", viewerId);
                 return;
             }
 
             viewer.documentPath = fileName;
             viewer.dataBind();
             viewer.load(fileName, null);
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multas-upload-handler.js',
-                    'loadPdfInViewer',
-                    error,
-                );
+        }
+        catch (error)
+        {
+            if (window.Alerta?.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("multas-upload-handler.js", "loadPdfInViewer", error);
             }
             console.error(error);
         }
     }
 
-    function extractPayload(args) {
-        try {
-
-            if (args?.response?.response) {
+    function extractPayload(args)
+    {
+        try
+        {
+            if (args?.response?.response)
+            {
                 return JSON.parse(args.response.response);
             }
-
-            else if (args?.e?.target?.response) {
+            else if (args?.e?.target?.response)
+            {
                 return JSON.parse(args.e.target.response);
             }
-
-            else if (typeof args?.response === 'string') {
+            else if (typeof args?.response === "string")
+            {
                 return JSON.parse(args.response);
             }
             return null;
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multas-upload-handler.js',
-                    'extractPayload',
-                    error,
-                );
+        }
+        catch (error)
+        {
+            if (window.Alerta?.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("multas-upload-handler.js", "extractPayload", error);
             }
             return null;
         }
     }
 
-    function onUploadSelected(args) {
-        try {
-
+    function onUploadSelected(args)
+    {
+        try
+        {
             if (!args || !args.filesData || args.filesData.length === 0) return;
 
             const file = args.filesData[0];
-            const fileName = (file?.name || '').toLowerCase();
-
-            if (!fileName.endsWith('.pdf')) {
-
+            const fileName = (file?.name || "").toLowerCase();
+
+            if (!fileName.endsWith(".pdf"))
+            {
                 args.cancel = true;
 
-                if (window.AppToast?.show) {
-                    AppToast.show(
-                        'Vermelho',
-                        'Apenas arquivos PDF são permitidos',
-                        3000,
-                    );
-                } else {
-                    alert('Apenas arquivos PDF são permitidos.');
-                }
-            }
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multas-upload-handler.js',
-                    'onUploadSelected',
-                    error,
-                );
-            }
-        }
-    }
-
-    function onUploadFailure(args) {
-        try {
-
-            let msg = 'Falha no upload do PDF';
-
-            if (args?.response?.responseText) {
-                msg += ': ' + args.response.responseText;
-            }
-
-            if (window.AppToast?.show) {
+                if (window.AppToast?.show)
+                {
+                    AppToast.show('Vermelho', 'Apenas arquivos PDF são permitidos', 3000);
+                }
+                else
+                {
+                    console.error('[multas-upload-handler.js] Apenas arquivos PDF são permitidos');
+                }
+            }
+        }
+        catch (error)
+        {
+            if (window.Alerta?.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("multas-upload-handler.js", "onUploadSelected", error);
+            }
+        }
+    }
+
+    function onUploadFailure(args)
+    {
+        try
+        {
+            let msg = "Falha no upload do PDF";
+
+            if (args?.response?.responseText)
+            {
+                msg += ": " + args.response.responseText;
+            }
+
+            if (window.AppToast?.show)
+            {
                 AppToast.show('Vermelho', msg, 4000);
-            } else {
-                alert(msg);
-            }
-        } catch (error) {
-            if (window.Alerta?.TratamentoErroComLinha) {
-                Alerta.TratamentoErroComLinha(
-                    'multas-upload-handler.js',
-                    'onUploadFailure',
-                    error,
-                );
-            }
-        }
-    }
-
-    function createSuccessHandler(inputId, viewerId, successMessage) {
-        return function (args) {
-            try {
-
+            }
+            else
+            {
+                console.error('[multas-upload-handler.js] Falha no upload:', msg);
+            }
+        }
+        catch (error)
+        {
+            if (window.Alerta?.TratamentoErroComLinha)
+            {
+                Alerta.TratamentoErroComLinha("multas-upload-handler.js", "onUploadFailure", error);
+            }
+        }
+    }
+
+    function createSuccessHandler(inputId, viewerId, successMessage)
+    {
+        return function (args)
+        {
+            try
+            {
                 const payload = extractPayload(args);
 
-                if (!payload || !payload.fileName) {
-                    console.warn(
-                        'Upload OK, mas não veio fileName na resposta',
-                    );
-
-                    if (window.AppToast?.show) {
-                        AppToast.show(
-                            'Amarelo',
-                            'Arquivo enviado, mas houve problema ao processar',
-                            3000,
-                        );
+                if (!payload || !payload.fileName)
+                {
+                    console.warn("Upload OK, mas não veio fileName na resposta");
+
+                    if (window.AppToast?.show)
+                    {
+                        AppToast.show('Amarelo', 'Arquivo enviado, mas houve problema ao processar', 3000);
                     }
                     return;
                 }
@@ -160,16 +161,16 @@
 
                 loadPdfInViewer(payload.fileName, viewerId);
 
-                if (window.AppToast?.show) {
+                if (window.AppToast?.show)
+                {
                     AppToast.show('Verde', successMessage, 3000);
                 }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multas-upload-handler.js',
-                        'successHandler',
-                        error,
-                    );
+            }
+            catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multas-upload-handler.js", "successHandler", error);
                 }
                 console.error(error);
             }
@@ -179,31 +180,31 @@
     var onUploadSuccess_Autuacao = createSuccessHandler(
         '#txtAutuacaoPDF',
         'pdfviewerAutuacao',
-        'PDF de Autuação enviado com sucesso!',
+        'PDF de Autuação enviado com sucesso!'
     );
 
     var onUploadSuccess_Penalidade = createSuccessHandler(
         '#txtPenalidadePDF',
         'pdfviewerPenalidade',
-        'PDF de Penalidade enviado com sucesso!',
+        'PDF de Penalidade enviado com sucesso!'
     );
 
     var onUploadSuccess_Comprovante = createSuccessHandler(
         '#txtComprovantePDF',
         'pdfviewerComprovante',
-        'Comprovante enviado com sucesso!',
+        'Comprovante enviado com sucesso!'
     );
 
     var onUploadSuccess_EDoc = createSuccessHandler(
         '#txtEDocPDF',
         'pdfviewerEDoc',
-        'Processo e-Doc enviado com sucesso!',
+        'Processo e-Doc enviado com sucesso!'
     );
 
     var onUploadSuccess_OutrosDocumentos = createSuccessHandler(
         '#txtOutrosDocumentosPDF',
         'pdfviewerOutrosDocumentos',
-        'Documento enviado com sucesso!',
+        'Documento enviado com sucesso!'
     );
 
     return {
@@ -218,82 +219,65 @@
         onUploadSuccess_OutrosDocumentos: onUploadSuccess_OutrosDocumentos,
 
         loadPdfInViewer: loadPdfInViewer,
-        getViewer: getViewer,
+        getViewer: getViewer
     };
 })();
 
-$(document).ready(function () {
-    try {
-        console.log('MultasUpload Handler inicializado com sucesso!');
-
-        setTimeout(function () {
-            try {
+$(document).ready(function ()
+{
+    try
+    {
+        console.log("MultasUpload Handler inicializado com sucesso!");
+
+        setTimeout(function ()
+        {
+            try
+            {
 
                 const autuacaoPDF = $('#txtAutuacaoPDF').val();
-                if (
-                    autuacaoPDF &&
-                    autuacaoPDF !== '' &&
-                    autuacaoPDF !== 'null'
-                ) {
-                    MultasUpload.loadPdfInViewer(
-                        autuacaoPDF,
-                        'pdfviewerAutuacao',
-                    );
+                if (autuacaoPDF && autuacaoPDF !== '' && autuacaoPDF !== 'null')
+                {
+                    MultasUpload.loadPdfInViewer(autuacaoPDF, 'pdfviewerAutuacao');
                 }
 
                 const penalidadePDF = $('#txtPenalidadePDF').val();
-                if (
-                    penalidadePDF &&
-                    penalidadePDF !== '' &&
-                    penalidadePDF !== 'null'
-                ) {
-                    MultasUpload.loadPdfInViewer(
-                        penalidadePDF,
-                        'pdfviewerPenalidade',
-                    );
+                if (penalidadePDF && penalidadePDF !== '' && penalidadePDF !== 'null')
+                {
+                    MultasUpload.loadPdfInViewer(penalidadePDF, 'pdfviewerPenalidade');
                 }
 
                 const comprovantePDF = $('#txtComprovantePDF').val();
-                if (
-                    comprovantePDF &&
-                    comprovantePDF !== '' &&
-                    comprovantePDF !== 'null'
-                ) {
-                    MultasUpload.loadPdfInViewer(
-                        comprovantePDF,
-                        'pdfviewerComprovante',
-                    );
+                if (comprovantePDF && comprovantePDF !== '' && comprovantePDF !== 'null')
+                {
+                    MultasUpload.loadPdfInViewer(comprovantePDF, 'pdfviewerComprovante');
                 }
 
                 const edocPDF = $('#txtEDocPDF').val();
-                if (edocPDF && edocPDF !== '' && edocPDF !== 'null') {
+                if (edocPDF && edocPDF !== '' && edocPDF !== 'null')
+                {
                     MultasUpload.loadPdfInViewer(edocPDF, 'pdfviewerEDoc');
                 }
 
                 const outrosPDF = $('#txtOutrosDocumentosPDF').val();
-                if (outrosPDF && outrosPDF !== '' && outrosPDF !== 'null') {
-                    MultasUpload.loadPdfInViewer(
-                        outrosPDF,
-                        'pdfviewerOutrosDocumentos',
-                    );
-                }
-            } catch (error) {
-                if (window.Alerta?.TratamentoErroComLinha) {
-                    Alerta.TratamentoErroComLinha(
-                        'multas-upload-handler.js',
-                        'carregarPDFsExistentes',
-                        error,
-                    );
+                if (outrosPDF && outrosPDF !== '' && outrosPDF !== 'null')
+                {
+                    MultasUpload.loadPdfInViewer(outrosPDF, 'pdfviewerOutrosDocumentos');
+                }
+            }
+            catch (error)
+            {
+                if (window.Alerta?.TratamentoErroComLinha)
+                {
+                    Alerta.TratamentoErroComLinha("multas-upload-handler.js", "carregarPDFsExistentes", error);
                 }
             }
         }, 1500);
-    } catch (error) {
-        if (window.Alerta?.TratamentoErroComLinha) {
-            Alerta.TratamentoErroComLinha(
-                'multas-upload-handler.js',
-                'document.ready',
-                error,
-            );
+    }
+    catch (error)
+    {
+        if (window.Alerta?.TratamentoErroComLinha)
+        {
+            Alerta.TratamentoErroComLinha("multas-upload-handler.js", "document.ready", error);
         }
     }
 });
```
