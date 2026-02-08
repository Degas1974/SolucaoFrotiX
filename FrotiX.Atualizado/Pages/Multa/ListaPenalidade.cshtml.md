# Pages/Multa/ListaPenalidade.cshtml

**Mudanca:** GRANDE | **+273** linhas | **-275** linhas

---

```diff
--- JANEIRO: Pages/Multa/ListaPenalidade.cshtml
+++ ATUAL: Pages/Multa/ListaPenalidade.cshtml
@@ -7,18 +7,18 @@
 @inject IUnitOfWork _unitOfWork
 
 @{
-@functions {
-    public void OnGet()
-    {
-        FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
-
-        ViewData["dataOrgaoAutuante"] = new ListaOrgaoAutuanteMulta(_unitOfWork).OrgaoAutuanteList();
-        ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
-        ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
-        ViewData["lstTipoMulta"] = new ListaTipoMulta(_unitOfWork).TipoMultaList();
-        ViewData["lstStatus"] = new ListaStatusPenalidade(_unitOfWork).StatusList();
+    @functions {
+        public void OnGet()
+        {
+            FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
+
+            ViewData["dataOrgaoAutuante"] = new ListaOrgaoAutuanteMulta(_unitOfWork).OrgaoAutuanteList();
+            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
+            ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
+            ViewData["lstTipoMulta"] = new ListaTipoMulta(_unitOfWork).TipoMultaList();
+            ViewData["lstStatus"] = new ListaStatusPenalidade(_unitOfWork).StatusList();
+        }
     }
-}
 }
 
 @model FrotiX.Models.Multa
@@ -43,25 +43,11 @@
 
         /* ====== ANIMAÇÃO GLOW FROTIX ====== */
         @@keyframes buttonWiggle {
-            0% {
-                transform: translateY(0) rotate(0deg);
-            }
-
-            25% {
-                transform: translateY(-2px) rotate(-1deg);
-            }
-
-            50% {
-                transform: translateY(-3px) rotate(0deg);
-            }
-
-            75% {
-                transform: translateY(-2px) rotate(1deg);
-            }
-
-            100% {
-                transform: translateY(0) rotate(0deg);
-            }
+            0% { transform: translateY(0) rotate(0deg); }
+            25% { transform: translateY(-2px) rotate(-1deg); }
+            50% { transform: translateY(-3px) rotate(0deg); }
+            75% { transform: translateY(-2px) rotate(1deg); }
+            100% { transform: translateY(0) rotate(0deg); }
         }
 
         /* ====== VARIÁVEIS LOCAIS ====== */
@@ -84,7 +70,7 @@
         .ftx-card-header .ftx-card-title i {
             color: #fff !important;
             --fa-primary-color: #fff !important;
-            --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
+            --fa-secondary-color: rgba(255,255,255,0.7) !important;
         }
 
         /* ====== ÁREA DE FILTROS ====== */
@@ -154,8 +140,7 @@
             color: #fff !important;
         }
 
-        .btn-filtrar-penalidade:active,
-        .btn-filtrar-penalidade:focus {
+        .btn-filtrar-penalidade:active, .btn-filtrar-penalidade:focus {
             transform: translateY(0) scale(1) !important;
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
@@ -201,7 +186,7 @@
         #tblMulta tbody td {
             padding: 0.625rem;
             vertical-align: middle !important;
-            border-color: rgba(0, 0, 0, 0.05) !important;
+            border-color: rgba(0,0,0,0.05) !important;
         }
 
         /* ====== BADGES DE STATUS FrotiX ====== */
@@ -312,14 +297,11 @@
             background-color: #3D5771 !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
         .ftx-btn-editar:hover:not([aria-disabled="true"]) {
             background-color: #2d4559 !important;
             box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
         }
-
-        .ftx-btn-editar:active,
-        .ftx-btn-editar:focus {
+        .ftx-btn-editar:active, .ftx-btn-editar:focus {
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
 
@@ -328,14 +310,11 @@
             background-color: #D2691E !important;
             box-shadow: 0 0 8px rgba(210, 105, 30, 0.5), 0 2px 4px rgba(210, 105, 30, 0.3) !important;
         }
-
         .ftx-btn-pagamento:hover:not([aria-disabled="true"]) {
             background-color: #b35a18 !important;
             box-shadow: 0 0 20px rgba(210, 105, 30, 0.8), 0 6px 12px rgba(210, 105, 30, 0.5) !important;
         }
-
-        .ftx-btn-pagamento:active,
-        .ftx-btn-pagamento:focus {
+        .ftx-btn-pagamento:active, .ftx-btn-pagamento:focus {
             box-shadow: 0 0 0 0.2rem rgba(210, 105, 30, 0.35) !important;
         }
 
@@ -353,14 +332,11 @@
             background-color: #722f37 !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
         .ftx-btn-apagar:hover:not([aria-disabled="true"]) {
             background-color: #5a252c !important;
             box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
         }
-
-        .ftx-btn-apagar:active,
-        .ftx-btn-apagar:focus {
+        .ftx-btn-apagar:active, .ftx-btn-apagar:focus {
             box-shadow: 0 0 0 0.2rem rgba(114, 47, 55, 0.25) !important;
         }
 
@@ -369,14 +345,11 @@
             background-color: #1e1e1e !important;
             box-shadow: 0 0 8px rgba(30, 30, 30, 0.5), 0 2px 4px rgba(30, 30, 30, 0.3) !important;
         }
-
         .ftx-btn-pdf:hover:not([aria-disabled="true"]) {
             background-color: #333333 !important;
             box-shadow: 0 0 20px rgba(30, 30, 30, 0.8), 0 6px 12px rgba(30, 30, 30, 0.5) !important;
         }
-
-        .ftx-btn-pdf:active,
-        .ftx-btn-pdf:focus {
+        .ftx-btn-pdf:active, .ftx-btn-pdf:focus {
             box-shadow: 0 0 0 0.2rem rgba(30, 30, 30, 0.25) !important;
         }
 
@@ -394,14 +367,11 @@
             background-color: #2E8B57 !important;
             box-shadow: 0 0 8px rgba(46, 139, 87, 0.5), 0 2px 4px rgba(46, 139, 87, 0.3) !important;
         }
-
         .ftx-btn-comprovante:hover:not([aria-disabled="true"]) {
             background-color: #236B43 !important;
             box-shadow: 0 0 20px rgba(46, 139, 87, 0.8), 0 6px 12px rgba(46, 139, 87, 0.5) !important;
         }
-
-        .ftx-btn-comprovante:active,
-        .ftx-btn-comprovante:focus {
+        .ftx-btn-comprovante:active, .ftx-btn-comprovante:focus {
             box-shadow: 0 0 0 0.2rem rgba(46, 139, 87, 0.25) !important;
         }
 
@@ -472,7 +442,7 @@
         }
 
         #modalPDF .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -510,7 +480,7 @@
             display: flex;
             justify-content: flex-end;
             align-items: center;
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -557,7 +527,7 @@
         }
 
         #modalExibePDFPenalidade .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -591,7 +561,7 @@
         }
 
         #modalExibeComprovante .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -623,8 +593,7 @@
             box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
         }
 
-        .btn-modal-confirmar:active,
-        .btn-modal-confirmar:focus {
+        .btn-modal-confirmar:active, .btn-modal-confirmar:focus {
             transform: translateY(0) scale(1) !important;
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
@@ -651,8 +620,7 @@
             box-shadow: 0 0 20px rgba(46, 139, 87, 0.8), 0 6px 12px rgba(46, 139, 87, 0.5) !important;
         }
 
-        .btn-modal-pagar:active,
-        .btn-modal-pagar:focus {
+        .btn-modal-pagar:active, .btn-modal-pagar:focus {
             transform: translateY(0) scale(1) !important;
             box-shadow: 0 0 0 0.2rem rgba(46, 139, 87, 0.25) !important;
         }
@@ -745,53 +713,65 @@
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>Órgão Autuante</label>
-                                <ejs-combobox id="lstOrgao" placeholder="Selecione..." allowFiltering="true"
-                                    filterType="Contains" dataSource="@ViewData["dataOrgaoAutuante"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstOrgao"
+                                              placeholder="Selecione..."
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["dataOrgaoAutuante"]"
+                                              popupHeight="250px" width="100%" showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>Infração</label>
-                                <ejs-combobox id="lstTiposMulta" placeholder="Selecione..." allowFiltering="true"
-                                    filterType="Contains" dataSource="@ViewData["lstTipoMulta"]" popupHeight="250px"
-                                    width="100%" showClearButton="true">
+                                <ejs-combobox id="lstTiposMulta"
+                                              placeholder="Selecione..."
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["lstTipoMulta"]"
+                                              popupHeight="250px" width="100%" showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>Veículo</label>
-                                <ejs-combobox id="lstVeiculos" placeholder="Selecione..." allowFiltering="true"
-                                    filterType="Contains" dataSource="@ViewData["lstVeiculos"]" popupHeight="250px"
-                                    width="100%" showClearButton="true">
+                                <ejs-combobox id="lstVeiculos"
+                                              placeholder="Selecione..."
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["lstVeiculos"]"
+                                              popupHeight="250px" width="100%" showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>Motorista</label>
-                                <ejs-combobox id="lstMotorista" placeholder="Selecione..." allowFiltering="true"
-                                    filterType="Contains" dataSource="@ViewData["lstMotorista"]" popupHeight="250px"
-                                    width="100%" showClearButton="true">
+                                <ejs-combobox id="lstMotorista"
+                                              placeholder="Selecione..."
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["lstMotorista"]"
+                                              popupHeight="250px" width="100%" showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>Status</label>
-                                <ejs-combobox id="lstStatus" placeholder="Selecione..." allowFiltering="true"
-                                    filterType="Contains" dataSource="@ViewData["lstStatus"]" popupHeight="250px"
-                                    width="100%" showClearButton="true">
+                                <ejs-combobox id="lstStatus"
+                                              placeholder="Selecione..."
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["lstStatus"]"
+                                              popupHeight="250px" width="100%" showClearButton="true">
                                     <e-combobox-fields text="Status" value="StatusId"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
 
                             <div class="ftx-penalidade-filter-group">
                                 <label>&nbsp;</label>
-                                <button type="button" id="btnFiltro" name="btnFiltro" class="btn-filtrar-penalidade"
-                                    data-ejtip="Filtrar as Penalidades" onclick="ListaTblPenalidades()">
+                                <button type="button" id="btnFiltro" name="btnFiltro"
+                                        class="btn-filtrar-penalidade"
+                                        data-ejtip="Filtrar as Penalidades"
+                                        onclick="ListaTblPenalidades()">
                                     <i class="fa-duotone fa-magnifying-glass"></i>
                                     Filtrar
                                 </button>
@@ -817,7 +797,7 @@
                                         <th>Valor Pago</th>
                                         <th>(R$) Pós Vencimento</th>
                                         <th>Processo eDoc</th>
-                                        <th>Ações</th>
+                                        <th>Ação</th>
                                     </tr>
                                 </thead>
                                 <tbody></tbody>
@@ -831,8 +811,7 @@
     </div>
 </form>
 
-<div class="modal fade" id="modalPDF" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
+<div class="modal fade" id="modalPDF" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
@@ -840,14 +819,18 @@
                     <i class="fa-duotone fa-file-pdf"></i>
                     Ficha de Vistoria
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div id="pdfViewer" style="height: 600px;">
-                    <ejs-pdfviewer id="pdfViewerControl" style="height:100%" serviceUrl="/api/PdfViewer" documentPath=""
-                        enableToolbar="true" enableNavigationToolbar="true" enableThumbnail="true"
-                        zoomMode="FitToWidth">
+                    <ejs-pdfviewer id="pdfViewerControl"
+                                   style="height:100%"
+                                   serviceUrl="/api/PdfViewer"
+                                   documentPath=""
+                                   enableToolbar="true"
+                                   enableNavigationToolbar="true"
+                                   enableThumbnail="true"
+                                   zoomMode="FitToWidth">
                     </ejs-pdfviewer>
                 </div>
             </div>
@@ -861,8 +844,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalRegistraPagamento" tabindex="-1" aria-labelledby="h3Titulo" aria-hidden="true"
-    data-bs-backdrop="static" data-bs-keyboard="false">
+<div class="modal fade" id="modalRegistraPagamento" tabindex="-1" aria-labelledby="h3Titulo" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-md">
         <div class="modal-content">
 
@@ -871,8 +853,7 @@
                     <i class="fa-duotone fa-money-bill-wave"></i>
                     Registrar Pagamento da Infração
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -892,7 +873,7 @@
                             <div class="form-group">
                                 <label class="label font-weight-bold">Valor Pago</label>
                                 <input id="txtValorPago" class="form-control form-control-xs" style="text-align:right;"
-                                    onkeypress="return moeda(this,'.',',',event)" placeholder="R$ 0,00" />
+                                       onkeypress="return moeda(this,'.',',',event)" placeholder="R$ 0,00" />
                             </div>
                         </div>
 
@@ -913,18 +894,22 @@
                     <div class="control-wrapper">
                         <div id="divPDF">
                             <label class="label font-weight-bold">Upload do Comprovante de Pagamento (PDF)</label>
-                            <ejs-uploader id="pdfComprovante" name="UploadFiles" multiple="false" autoUpload="true"
-                                allowedExtensions=".pdf">
+                            <ejs-uploader id="pdfComprovante"
+                                          name="UploadFiles"
+                                          multiple="false"
+                                          autoUpload="true"
+                                          allowedExtensions=".pdf">
                                 <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                    removeUrl="/api/MultaUpload/remove">
+                                                          removeUrl="/api/MultaUpload/remove">
                                 </e-uploader-asyncsettings>
                             </ejs-uploader>
 
                             <input id="txtComprovantePDF" class="form-control form-control-xs" style="display:none" />
 
                             <div id="ComprovantePDFViewerContainer" style="margin-top: 10px; display:none;">
-                                <ejs-pdfviewer id="ComprovantePDFViewer" style="height:400px;width:100%;"
-                                    serviceUrl="/api/MultaPdfViewer">
+                                <ejs-pdfviewer id="ComprovantePDFViewer"
+                                               style="height:400px;width:100%;"
+                                               serviceUrl="/api/MultaPdfViewer">
                                 </ejs-pdfviewer>
                             </div>
                         </div>
@@ -948,8 +933,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalExibePDFPenalidade" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
+<div class="modal fade" id="modalExibePDFPenalidade" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
@@ -957,14 +941,18 @@
                     <i class="fa-duotone fa-file-lines"></i>
                     Notificação de Penalidade
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div id="pdfViewerPenalidade" style="height: 600px; width: 100%;">
-                    <ejs-pdfviewer id="pdfViewerPenalidadeControl" style="height:100%; width:100%;"
-                        serviceUrl="/api/MultaPdfViewer" documentPath="" enableToolbar="true"
-                        enableNavigationToolbar="true" enableThumbnail="true" zoomMode="FitToWidth">
+                    <ejs-pdfviewer id="pdfViewerPenalidadeControl"
+                                   style="height:100%; width:100%;"
+                                   serviceUrl="/api/MultaPdfViewer"
+                                   documentPath=""
+                                   enableToolbar="true"
+                                   enableNavigationToolbar="true"
+                                   enableThumbnail="true"
+                                   zoomMode="FitToWidth">
                     </ejs-pdfviewer>
                 </div>
             </div>
@@ -978,8 +966,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalExibeComprovante" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
+<div class="modal fade" id="modalExibeComprovante" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
@@ -987,14 +974,18 @@
                     <i class="fa-duotone fa-receipt"></i>
                     Comprovante de Pagamento
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div id="pdfViewerComprovante" style="height: 600px; width: 100%;">
-                    <ejs-pdfviewer id="pdfViewerComprovanteControl" style="height:100%; width:100%;"
-                        serviceUrl="/api/MultaPdfViewer" documentPath="" enableToolbar="true"
-                        enableNavigationToolbar="true" enableThumbnail="true" zoomMode="FitToWidth">
+                    <ejs-pdfviewer id="pdfViewerComprovanteControl"
+                                   style="height:100%; width:100%;"
+                                   serviceUrl="/api/MultaPdfViewer"
+                                   documentPath=""
+                                   enableToolbar="true"
+                                   enableNavigationToolbar="true"
+                                   enableThumbnail="true"
+                                   zoomMode="FitToWidth">
                     </ejs-pdfviewer>
                 </div>
             </div>
@@ -1010,8 +1001,7 @@
 
 <div id="loadingOverlayPenalidade" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Penalidades...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
@@ -1019,10 +1009,8 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="https://cdn.datatables.net/1.13.6/js/jquery.dataTables.min.js"></script>
     <script src="https://cdn.datatables.net/buttons/2.4.1/js/dataTables.buttons.min.js"></script>
@@ -1034,25 +1022,12 @@
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * LISTA DE PENALIDADES - CONFIGURAÇÃO E LOADING
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Gerenciamento de multas/penalidades com DataTable,
-         * filtros por status, upload de comprovantes e visualização PDF.
-         */
         var URLapi = "/api/multa/listamultas";
 
-        /**
-         * Exibe overlay de loading.
-         */
         function mostrarLoading() {
             $('#loadingOverlayPenalidade').css('display', 'flex');
         }
 
-        /**
-         * Oculta overlay de loading.
-         */
         function esconderLoading() {
             $('#loadingOverlayPenalidade').css('display', 'none');
         }
@@ -1075,10 +1050,6 @@
             }
         });
 
-        /**
-         * Lista todas as penalidades aplicando filtro de status padrão.
-         * @@description Verifica ComboBox de status e define URL da API.
-         */
         function ListaTodasPenalidades() {
             try {
                 const statusCmb = document.getElementById('lstStatus')?.ej2_instances?.[0];
@@ -1093,17 +1064,7 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * RENDERIZAÇÃO DE BADGES DE STATUS
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Renderiza badge HTML colorido baseado no status da penalidade.
-         * @@param {string} status - Status da penalidade (Pago, Pendente, Enviada, etc.).
-         * @@returns {string} HTML do badge com ícone e cor apropriados.
-         */
+
         function renderStatusBadge(status) {
             try {
                 if (!status) return '<span class="ftx-badge-status ftx-badge-default"><i class="fa-duotone fa-circle-question"></i> N/D</span>';
@@ -1138,17 +1099,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * DATATABLE DE PENALIDADES
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Carrega/recarrega o DataTable de penalidades/multas.
-         * @@description Lê filtros dos ComboBoxes, destrói tabela existente,
-         * recria com dados da API e aplica formatações.
-         */
         function ListaTblPenalidades() {
             try {
 
@@ -1214,10 +1164,10 @@
                             width: "3%",
                             render: function (data, type, full) {
                                 return `<div class="text-center">
-                                        <a data-ejtip="Observação: ${full.observacao}" style="cursor:pointer;" data-id="${data}">
-                                            ${full.numInfracao}
-                                        </a>
-                                    </div>`;
+                                    <a data-ejtip="Observação: ${full.observacao}" style="cursor:pointer;" data-id="${data}">
+                                        ${full.numInfracao}
+                                    </a>
+                                </div>`;
                             }
                         },
                         { targets: 2, className: "text-center", width: "3%" },
@@ -1228,10 +1178,10 @@
                             width: "8%",
                             render: function (data, type, full) {
                                 return `<div class="text-center">
-                                        <a data-ejtip="Telefone: ${full.telefone}" style="cursor:pointer;" data-id="${data}">
-                                            ${full.nome}
-                                        </a>
-                                    </div>`;
+                                    <a data-ejtip="Telefone: ${full.telefone}" style="cursor:pointer;" data-id="${data}">
+                                        ${full.nome}
+                                    </a>
+                                </div>`;
                             }
                         },
                         { targets: 5, className: "text-center", width: "3%" },
@@ -1242,10 +1192,10 @@
                             width: "8%",
                             render: function (data, type, full) {
                                 return `<div class="text-center">
-                                        <a data-ejtip="Descrição: ${full.descricao}" style="cursor:pointer;" data-id="${data}">
-                                            ${full.artigo}
-                                        </a>
-                                    </div>`;
+                                    <a data-ejtip="Descrição: ${full.descricao}" style="cursor:pointer;" data-id="${data}">
+                                        ${full.artigo}
+                                    </a>
+                                </div>`;
                             }
                         },
                         { targets: 8, className: "text-left", width: "20%" },
@@ -1255,10 +1205,10 @@
                             width: "3%",
                             render: function (data, type, full) {
                                 return `<div class="text-center">
-                                        <a data-ejtip="Data Original de Vencimento: ${full.vencimento}" style="cursor:pointer;" data-id="${data}">
-                                            ${full.dataPagamento}
-                                        </a>
-                                    </div>`;
+                                    <a data-ejtip="Data Original de Vencimento: ${full.vencimento}" style="cursor:pointer;" data-id="${data}">
+                                        ${full.dataPagamento}
+                                    </a>
+                                </div>`;
                             }
                         },
                         {
@@ -1267,10 +1217,10 @@
                             width: "6%",
                             render: function (data, type, full) {
                                 return `<div class="text-center">
-                                        <a data-ejtip="Valor Original de Pagamento: ${full.valorAteVencimento}" style="cursor:pointer;" data-id="${data}">
-                                            ${full.valorPago}
-                                        </a>
-                                    </div>`;
+                                    <a data-ejtip="Valor Original de Pagamento: ${full.valorAteVencimento}" style="cursor:pointer;" data-id="${data}">
+                                        ${full.valorPago}
+                                    </a>
+                                </div>`;
                             }
                         },
                         { targets: 11, className: "text-right", width: "5%" },
@@ -1289,37 +1239,37 @@
                                 const pagamentoTooltip = pagamentoDisabled ? 'Pagamento já registrado' : (full.tooltip || 'Registrar Pagamento');
 
                                 return `<div class="text-center" style="white-space: nowrap;">
-                                        <a href="/Multa/UpsertPenalidade?id=${data}"
-                                           class="ftx-btn-icon ftx-btn-editar"
-                                           data-ejtip="Editar a Penalidade">
-                                            <i class="fa-duotone fa-pen-to-square"></i>
-                                        </a>
-                                        <a href="#" class="ftx-btn-icon ftx-btn-pagamento btn-pagamento ${pagamentoClass}"
-                                           data-ejtip="${pagamentoTooltip}"
-                                           data-id="${data}"
-                                           ${pagamentoDisabled ? 'aria-disabled="true"' : ''}>
-                                            <i class="fa-duotone fa-dollar-sign"></i>
-                                        </a>
-                                        <a class="ftx-btn-icon ftx-btn-apagar btn-apagar"
-                                           data-ejtip="Apagar a Penalidade"
-                                           data-id="${data}">
-                                            <i class="fa-duotone fa-trash-can"></i>
-                                        </a>
-                                        <a class="ftx-btn-icon ftx-btn-pdf btn-exibe-penalidade ${!hasPenalidadePDF ? 'ftx-btn-pdf-disabled' : ''}"
-                                           data-id="${data}"
-                                           data-penalidadepdf="${full.penalidadePDF || ''}"
-                                           data-ejtip="Exibe a Notificação de Penalidade"
-                                           ${!hasPenalidadePDF ? 'aria-disabled="true"' : ''}>
-                                            <i class="fa-duotone fa-file-lines"></i>
-                                        </a>
-                                        <a class="ftx-btn-icon ftx-btn-comprovante btn-exibe-comprovante ${!hasComprovantePDF ? 'ftx-btn-comprovante-disabled' : ''}"
-                                           data-id="${data}"
-                                           data-comprovantepdf="${full.comprovantePDF || ''}"
-                                           data-ejtip="${hasComprovantePDF ? 'Exibe o Comprovante de Pagamento' : 'Sem comprovante anexado'}"
-                                           ${!hasComprovantePDF ? 'aria-disabled="true"' : ''}>
-                                            <i class="fa-duotone fa-receipt"></i>
-                                        </a>
-                                    </div>`;
+                                    <a href="/Multa/UpsertPenalidade?id=${data}"
+                                       class="ftx-btn-icon ftx-btn-editar"
+                                       data-ejtip="Editar a Penalidade">
+                                        <i class="fa-duotone fa-pen-to-square"></i>
+                                    </a>
+                                    <a href="#" class="ftx-btn-icon ftx-btn-pagamento btn-pagamento ${pagamentoClass}"
+                                       data-ejtip="${pagamentoTooltip}"
+                                       data-id="${data}"
+                                       ${pagamentoDisabled ? 'aria-disabled="true"' : ''}>
+                                        <i class="fa-duotone fa-dollar-sign"></i>
+                                    </a>
+                                    <a class="ftx-btn-icon ftx-btn-apagar btn-apagar"
+                                       data-ejtip="Apagar a Penalidade"
+                                       data-id="${data}">
+                                        <i class="fa-duotone fa-trash-can"></i>
+                                    </a>
+                                    <a class="ftx-btn-icon ftx-btn-pdf btn-exibe-penalidade ${!hasPenalidadePDF ? 'ftx-btn-pdf-disabled' : ''}"
+                                       data-id="${data}"
+                                       data-penalidadepdf="${full.penalidadePDF || ''}"
+                                       data-ejtip="Exibe a Notificação de Penalidade"
+                                       ${!hasPenalidadePDF ? 'aria-disabled="true"' : ''}>
+                                        <i class="fa-duotone fa-file-lines"></i>
+                                    </a>
+                                    <a class="ftx-btn-icon ftx-btn-comprovante btn-exibe-comprovante ${!hasComprovantePDF ? 'ftx-btn-comprovante-disabled' : ''}"
+                                       data-id="${data}"
+                                       data-comprovantepdf="${full.comprovantePDF || ''}"
+                                       data-ejtip="${hasComprovantePDF ? 'Exibe o Comprovante de Pagamento' : 'Sem comprovante anexado'}"
+                                       ${!hasComprovantePDF ? 'aria-disabled="true"' : ''}>
+                                        <i class="fa-duotone fa-receipt"></i>
+                                    </a>
+                                </div>`;
                             }
                         }
                     ],
@@ -1440,44 +1390,35 @@
     </script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES AUXILIARES PARA PDF - UPLOAD COMPROVANTE
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Gerencia upload de comprovantes PDF e visualização
-         * usando Syncfusion PdfViewer.
-         */
+
         var uploaderEventosVinculados = false;
 
-        /**
-         * Obtém instância do PdfViewer Syncfusion pelo ID.
-         * @@param {string} viewerId - ID do elemento viewer.
-         * @@returns {Object|null} Instância do PdfViewer ou null.
-         */
-        function getViewer(viewerId) {
-            try {
+        function getViewer(viewerId)
+        {
+            try
+            {
                 const viewerElement = document.getElementById(viewerId);
                 return viewerElement?.ej2_instances?.[0] || null;
-            } catch (err) {
+            } catch (err)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "getViewer", err);
                 return null;
             }
         }
 
-        /**
-         * Carrega PDF no viewer especificado.
-         * @@param {string} fileName - Nome do arquivo PDF.
-         * @@param {string} viewerId - ID do viewer onde carregar.
-         */
-        function loadPdfInViewer(fileName, viewerId) {
-            try {
-                if (!fileName || fileName === '' || fileName === 'null') {
+        function loadPdfInViewer(fileName, viewerId)
+        {
+            try
+            {
+                if (!fileName || fileName === '' || fileName === 'null')
+                {
                     console.warn(`⚠️ Nome de arquivo inválido para carregar no viewer ${viewerId}`);
                     return;
                 }
 
                 const viewer = getViewer(viewerId);
-                if (!viewer) {
+                if (!viewer)
+                {
                     console.error(`❌ Viewer ${viewerId} não encontrado`);
                     return;
                 }
@@ -1488,35 +1429,37 @@
                 viewer.dataBind();
                 viewer.load(fileName, null);
                 console.log(`✅ PDF carregado no viewer ${viewerId}: ${fileName}`);
-            } catch (error) {
+            } catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "loadPdfInViewer", error);
                 console.error("❌ Erro ao carregar PDF:", error);
             }
         }
 
-        /**
-         * Handler de seleção de arquivo no uploader de comprovante.
-         * @@param {Object} args - Argumentos do evento de seleção.
-         * @@description Valida se arquivo é PDF antes de permitir upload.
-         */
-        function onComprovanteUploadSelected(args) {
-            try {
+        function onComprovanteUploadSelected(args)
+        {
+            try
+            {
                 if (!args || !args.filesData || args.filesData.length === 0) return;
 
                 const file = args.filesData[0];
                 const fileName = (file?.name || "").toLowerCase();
 
-                if (!fileName.endsWith(".pdf")) {
+                if (!fileName.endsWith(".pdf"))
+                {
                     args.cancel = true;
                     AppToast.show('Vermelho', 'Apenas arquivos PDF são permitidos', 3000);
                 }
-            } catch (error) {
+            } catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "onComprovanteUploadSelected", error);
             }
         }
 
-        function onComprovanteUploadSuccess(args) {
-            try {
+        function onComprovanteUploadSuccess(args)
+        {
+            try
+            {
                 if (!args || !args.e) return;
 
                 console.log('✅ Upload Comprovante success args:', args);
@@ -1527,14 +1470,16 @@
 
                 console.log('📦 Server response:', serverResponse);
 
-                if (serverResponse.error) {
+                if (serverResponse.error)
+                {
                     console.error('❌ Erro do servidor:', serverResponse.error);
                     AppToast.show('Vermelho', serverResponse.error.message || 'Erro ao enviar arquivo', 3000);
                     return;
                 }
 
                 const uploadedFiles = serverResponse.files || [];
-                if (uploadedFiles.length === 0) {
+                if (uploadedFiles.length === 0)
+                {
                     console.error('❌ Nenhum arquivo retornado pelo servidor');
                     return;
                 }
@@ -1545,33 +1490,40 @@
 
                 loadPdfInViewer(fileName, 'ComprovantePDFViewer');
                 AppToast.show('Verde', 'Comprovante enviado com sucesso!', 3000);
-            } catch (error) {
+            } catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "onComprovanteUploadSuccess", error);
             }
         }
 
-        function onComprovanteUploadFailure(args) {
-            try {
+        function onComprovanteUploadFailure(args)
+        {
+            try
+            {
                 console.error("❌ Erro no upload de Comprovante:", args);
                 AppToast.show('Vermelho', 'Erro ao enviar PDF de Comprovante', 3000);
-            } catch (error) {
+            } catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "onComprovanteUploadFailure", error);
             }
         }
 
-        function vincularEventosUploader() {
+        function vincularEventosUploader()
+        {
             try {
                 if (uploaderEventosVinculados) return;
 
                 const uploaderComprovante = document.getElementById("pdfComprovante")?.ej2_instances?.[0];
-                if (uploaderComprovante) {
+                if (uploaderComprovante)
+                {
                     uploaderComprovante.selected = onComprovanteUploadSelected;
                     uploaderComprovante.success = onComprovanteUploadSuccess;
                     uploaderComprovante.failure = onComprovanteUploadFailure;
                     uploaderEventosVinculados = true;
                     console.log('✅ Eventos do Uploader Comprovante vinculados');
                 }
-                else {
+                else
+                {
                     console.warn('⚠️ Uploader pdfComprovante não encontrado');
                 }
             } catch (error) {
@@ -1579,7 +1531,7 @@
             }
         }
 
-        $(document).ready(function () {
+        $(document).ready(function() {
             vincularEventosUploader();
         });
     </script>
@@ -1618,7 +1570,7 @@
 
                 const pdfViewer = document.getElementById('ComprovantePDFViewer')?.ej2_instances?.[0];
                 if (pdfViewer) {
-                    try { pdfViewer.unload(); } catch (e) { }
+                    try { pdfViewer.unload(); } catch(e) {}
                 }
                 document.getElementById('ComprovantePDFViewerContainer').style.display = 'none';
 
@@ -1845,8 +1797,10 @@
 
     <script>
 
-        $(document).on('click', '.btn-exibe-penalidade', function (e) {
-            try {
+        $(document).on('click', '.btn-exibe-penalidade', function (e)
+        {
+            try
+            {
                 e.preventDefault();
 
                 if ($(this).attr('aria-disabled') === 'true') return;
@@ -1855,7 +1809,8 @@
 
                 console.log('🔍 Clique em btn-exibe-penalidade, penalidadePDF:', penalidadePDF);
 
-                if (!penalidadePDF || penalidadePDF === '') {
+                if (!penalidadePDF || penalidadePDF === '')
+                {
                     Alerta.Warning(
                         'PDF não disponível',
                         'Esta penalidade não possui PDF anexado.',
@@ -1866,13 +1821,16 @@
 
                 exibirPDFPenalidade(penalidadePDF);
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "btn-exibe-penalidade.click", error);
             }
         });
 
-        $(document).on('click', '.btn-exibe-comprovante', function (e) {
-            try {
+        $(document).on('click', '.btn-exibe-comprovante', function (e)
+        {
+            try
+            {
                 e.preventDefault();
 
                 if ($(this).attr('aria-disabled') === 'true') return;
@@ -1881,7 +1839,8 @@
 
                 console.log('🔍 Clique em btn-exibe-comprovante, comprovantePDF:', comprovantePDF);
 
-                if (!comprovantePDF || comprovantePDF === '') {
+                if (!comprovantePDF || comprovantePDF === '')
+                {
                     Alerta.Warning(
                         'PDF não disponível',
                         'Esta penalidade não possui comprovante de pagamento anexado.',
@@ -1892,39 +1851,45 @@
 
                 exibirComprovantePDF(comprovantePDF);
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "btn-exibe-comprovante.click", error);
             }
         });
 
-        function exibirPDFPenalidade(nomeArquivo) {
-            try {
-                if (!nomeArquivo || nomeArquivo === '') {
+        function exibirPDFPenalidade(nomeArquivo)
+        {
+            try
+            {
+                if (!nomeArquivo || nomeArquivo === '')
+                {
                     console.error('Nome do arquivo PDF inválido');
                     return;
                 }
 
                 $('#modalExibePDFPenalidade').modal('show');
 
-                setTimeout(function () {
+                setTimeout(function() {
                     try {
 
                         const viewerElement = document.getElementById('pdfViewerPenalidadeControl');
-                        if (!viewerElement) {
+                        if (!viewerElement)
+                        {
                             console.error('PDF Viewer de Penalidade não encontrado');
                             return;
                         }
 
                         const pdfViewer = viewerElement.ej2_instances?.[0];
-                        if (!pdfViewer) {
+                        if (!pdfViewer)
+                        {
                             console.error('Instância do PDF Viewer não encontrada');
                             return;
                         }
 
-                        pdfViewer.documentLoad = function () {
+                        pdfViewer.documentLoad = function() {
                             try {
 
-                                setTimeout(function () {
+                                setTimeout(function() {
                                     pdfViewer.magnificationModule.fitToWidth();
                                     console.log('✅ Zoom ajustado para FitToWidth');
                                 }, 100);
@@ -1944,39 +1909,45 @@
                     }
                 }, 500);
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "exibirPDFPenalidade", error);
             }
         }
 
-        function exibirComprovantePDF(nomeArquivo) {
-            try {
-                if (!nomeArquivo || nomeArquivo === '') {
+        function exibirComprovantePDF(nomeArquivo)
+        {
+            try
+            {
+                if (!nomeArquivo || nomeArquivo === '')
+                {
                     console.error('Nome do arquivo PDF inválido');
                     return;
                 }
 
                 $('#modalExibeComprovante').modal('show');
 
-                setTimeout(function () {
+                setTimeout(function() {
                     try {
 
                         const viewerElement = document.getElementById('pdfViewerComprovanteControl');
-                        if (!viewerElement) {
+                        if (!viewerElement)
+                        {
                             console.error('PDF Viewer de Comprovante não encontrado');
                             return;
                         }
 
                         const pdfViewer = viewerElement.ej2_instances?.[0];
-                        if (!pdfViewer) {
+                        if (!pdfViewer)
+                        {
                             console.error('Instância do PDF Viewer não encontrada');
                             return;
                         }
 
-                        pdfViewer.documentLoad = function () {
+                        pdfViewer.documentLoad = function() {
                             try {
 
-                                setTimeout(function () {
+                                setTimeout(function() {
                                     pdfViewer.magnificationModule.fitToWidth();
                                     console.log('✅ Zoom ajustado para FitToWidth');
                                 }, 100);
@@ -1996,46 +1967,59 @@
                     }
                 }, 500);
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "exibirComprovantePDF", error);
             }
         }
 
-        $('#modalExibePDFPenalidade').on('hidden.bs.modal', function () {
-            try {
+        $('#modalExibePDFPenalidade').on('hidden.bs.modal', function ()
+        {
+            try
+            {
                 const pdfViewer = document.getElementById('pdfViewerPenalidadeControl')?.ej2_instances?.[0];
-                if (pdfViewer) {
+                if (pdfViewer)
+                {
                     pdfViewer.unload();
                 }
                 console.log('✅ Modal Penalidade fechado e viewer limpo');
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "modalExibePDFPenalidade.hidden", error);
             }
         });
 
-        $('#modalExibeComprovante').on('hidden.bs.modal', function () {
-            try {
+        $('#modalExibeComprovante').on('hidden.bs.modal', function ()
+        {
+            try
+            {
                 const pdfViewer = document.getElementById('pdfViewerComprovanteControl')?.ej2_instances?.[0];
-                if (pdfViewer) {
+                if (pdfViewer)
+                {
                     pdfViewer.unload();
                 }
                 console.log('✅ Modal Comprovante fechado e viewer limpo');
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "modalExibeComprovante.hidden", error);
             }
         });
 
-        $('#modalPDF').on('hidden.bs.modal', function () {
-            try {
+        $('#modalPDF').on('hidden.bs.modal', function ()
+        {
+            try
+            {
                 const pdfViewer = document.getElementById('pdfViewerControl')?.ej2_instances?.[0];
-                if (pdfViewer) {
+                if (pdfViewer)
+                {
                     pdfViewer.unload();
                 }
                 console.log('✅ Modal Ficha de Vistoria fechado e viewer limpo');
             }
-            catch (error) {
+            catch (error)
+            {
                 Alerta.TratamentoErroComLinha("ListaPenalidade.cshtml", "modalPDF.hidden", error);
             }
         });
```
