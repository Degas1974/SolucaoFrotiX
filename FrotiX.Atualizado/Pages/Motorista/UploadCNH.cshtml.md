# Pages/Motorista/UploadCNH.cshtml

**Mudanca:** GRANDE | **+54** linhas | **-68** linhas

---

```diff
--- JANEIRO: Pages/Motorista/UploadCNH.cshtml
+++ ATUAL: Pages/Motorista/UploadCNH.cshtml
@@ -13,7 +13,7 @@
 
     var asyncSettings = new Syncfusion.EJ2.Inputs.UploaderAsyncSettings
     {
-        SaveUrl = $"/api/UploadCNH/Save?motoristaId={Model.MotoristaObj.Motorista.MotoristaId}",
+        SaveUrl = $"/api/UploadCNH/Save?motoristaId={Model.MotoristaObj.Motorista.MotoristaId}" ,
         RemoveUrl = $"/api/UploadCNH/Remove?motoristaId={Model.MotoristaObj.Motorista.MotoristaId}"
     };
 }
@@ -67,9 +67,13 @@
                                 <div id="ControlRegion" class="row">
                                     <div class="col-lg-8">
                                         <div class="control_wrapper">
-                                            <ejs-uploader id="UploadFiles" removing="onFileRemove"
-                                                dropArea=".control-fluid" asyncSettings="asyncSettings" locale="pt-BR"
-                                                allowedExtensions=".pdf" actionComplete="onActionComplete">
+                                            <ejs-uploader id="UploadFiles"
+                                                          removing="onFileRemove"
+                                                          dropArea=".control-fluid"
+                                                          asyncSettings="asyncSettings"
+                                                          locale="pt-BR"
+                                                          allowedExtensions=".pdf"
+                                                          actionComplete="onActionComplete">
                                             </ejs-uploader>
                                         </div>
                                     </div>
@@ -77,8 +81,10 @@
                                     <div class="col-lg-4 property-section d-none">
                                         <div id="property" title="Properties">
                                             <div style="margin-left: 50px; padding-top:25px;">
-                                                <ejs-checkbox id="checkAutoUpload" label="Auto Upload" checked="true"
-                                                    change="onChange">
+                                                <ejs-checkbox id="checkAutoUpload"
+                                                              label="Auto Upload"
+                                                              checked="true"
+                                                              change="onChange">
                                                 </ejs-checkbox>
                                             </div>
                                         </div>
@@ -88,17 +94,17 @@
                                 <br />
 
                                 <div id="divpdf">
-                                    <ejs-pdfviewer id="pdfviewer" style="height:600px" serviceUrl="/api/PdfViewerCNH"
-                                        locale="pt-BR">
+                                    <ejs-pdfviewer id="pdfviewer"
+                                                   style="height:600px"
+                                                   serviceUrl="/api/PdfViewerCNH"
+                                                   locale="pt-BR">
                                     </ejs-pdfviewer>
                                 </div>
 
                                 <br />
                                 <a href="/Motorista/Upsert?id=@Model.MotoristaObj.Motorista.MotoristaId"
-                                    class="btn btn-voltar form-control float-right" style="width: 300px;"
-                                    data-ftx-loading>
-                                    <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i> Voltar para o
-                                    Motorista
+                                   class="btn btn-voltar form-control float-right" style="width: 300px;" data-ftx-loading>
+                                    <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i> Voltar para o Motorista
                                 </a>
                                 <div class="clearfix"></div>
                             </main>
@@ -112,10 +118,8 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <ejs-scripts></ejs-scripts>
 
@@ -212,12 +216,6 @@
             }
         });
 
-        /**
-         * Handler de conclusão de upload do componente Syncfusion
-         * @@description Exibe visualizador PDF e carrega documento após upload bem-sucedido
-         * @@param {Object} args - Argumentos do evento contendo fileData
-         * @@returns {void}
-         */
         function onActionComplete(args) {
             try {
                 if (!args || !args.fileData || !args.fileData.length) {
@@ -238,35 +236,29 @@
                     processData: false,
                     contentType: false
                 })
-                    .done(function (result) {
-                        try {
-                            console.log("Documento carregado.");
-                            var pdfViewer = document.getElementById('pdfviewer').ej2_instances[0];
-                            pdfViewer.load(result, null);
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "onActionComplete.ajax.done", error);
-                        }
-                    })
-                    .fail(function (msg) {
-                        try {
-                            console.error('Erro ao carregar PDF: ' + (msg && msg.responseText));
-                            AppToast.show("Vermelho", "Erro ao carregar o PDF", 3000);
-                        } catch (error) {
-                            Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "onActionComplete.ajax.fail", error);
-                        }
-                    });
+                .done(function (result) {
+                    try {
+                        console.log("Documento carregado.");
+                        var pdfViewer = document.getElementById('pdfviewer').ej2_instances[0];
+                        pdfViewer.load(result, null);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "onActionComplete.ajax.done", error);
+                    }
+                })
+                .fail(function (msg) {
+                    try {
+                        console.error('Erro ao carregar PDF: ' + (msg && msg.responseText));
+                        AppToast.show("Vermelho", "Erro ao carregar o PDF", 3000);
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "onActionComplete.ajax.fail", error);
+                    }
+                });
 
             } catch (error) {
                 Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "onActionComplete", error);
             }
         }
 
-        /**
-         * Handler de remoção de arquivo do componente Uploader
-         * @@description Desabilita envio do arquivo raw ao servidor
-         * @@param {Object} args - Argumentos do evento de remoção
-         * @@returns {void}
-         */
         function onFileRemove(args) {
             try {
                 args.postRawFile = false;
@@ -275,12 +267,6 @@
             }
         }
 
-        /**
-         * Handler de mudança do toggle de auto-upload
-         * @@description Altera configuração autoUpload do componente e limpa arquivos selecionados
-         * @@param {Object} args - Argumentos do evento contendo checked (boolean)
-         * @@returns {void}
-         */
         function onChange(args) {
             try {
                 var uploadObj = document.getElementById("UploadFiles");
@@ -311,23 +297,23 @@
                         processData: false,
                         contentType: false
                     })
-                        .done(function (result) {
-                            try {
-                                console.log("Documento carregado (auto).");
-                                var pdfViewer = document.getElementById('pdfviewer').ej2_instances[0];
-                                pdfViewer.load(result, null);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "document.ready.ajax.done", error);
-                            }
-                        })
-                        .fail(function (msg) {
-                            try {
-                                console.error('Erro ao carregar PDF: ' + (msg && msg.responseText));
-                                AppToast.show("Vermelho", "Erro ao carregar o PDF existente", 3000);
-                            } catch (error) {
-                                Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "document.ready.ajax.fail", error);
-                            }
-                        });
+                    .done(function (result) {
+                        try {
+                            console.log("Documento carregado (auto).");
+                            var pdfViewer = document.getElementById('pdfviewer').ej2_instances[0];
+                            pdfViewer.load(result, null);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "document.ready.ajax.done", error);
+                        }
+                    })
+                    .fail(function (msg) {
+                        try {
+                            console.error('Erro ao carregar PDF: ' + (msg && msg.responseText));
+                            AppToast.show("Vermelho", "Erro ao carregar o PDF existente", 3000);
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "document.ready.ajax.fail", error);
+                        }
+                    });
                 }
             } catch (error) {
                 Alerta.TratamentoErroComLinha("UploadCNH.cshtml", "document.ready", error);
```
