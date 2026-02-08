# Pages/Multa/ListaAutuacao.cshtml

**Mudanca:** GRANDE | **+117** linhas | **-186** linhas

---

```diff
--- JANEIRO: Pages/Multa/ListaAutuacao.cshtml
+++ ATUAL: Pages/Multa/ListaAutuacao.cshtml
@@ -9,24 +9,24 @@
 @inject IUnitOfWork _unitOfWork
 
 @{
-@functions {
-    public void OnGet()
-    {
-        FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
-        ViewData["dataOrgaoAutuante"] = new ListaOrgaoAutuanteMulta(_unitOfWork).OrgaoAutuanteList();
-        ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
-        ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
-        ViewData["lstTipoMulta"] = new ListaTipoMulta(_unitOfWork).TipoMultaList();
-        ViewData["lstStatus"] = new ListaStatusAutuacao(_unitOfWork).StatusList();
-        ViewData["lstStatusAlteracao"] = new ListaStatusAutuacaoAlteracao(_unitOfWork).StatusList();
+    @functions {
+        public void OnGet()
+        {
+            FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
+            ViewData["dataOrgaoAutuante"] = new ListaOrgaoAutuanteMulta(_unitOfWork).OrgaoAutuanteList();
+            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
+            ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
+            ViewData["lstTipoMulta"] = new ListaTipoMulta(_unitOfWork).TipoMultaList();
+            ViewData["lstStatus"] = new ListaStatusAutuacao(_unitOfWork).StatusList();
+            ViewData["lstStatusAlteracao"] = new ListaStatusAutuacaoAlteracao(_unitOfWork).StatusList();
+        }
     }
-}
-
-ViewData["Title"] = "Multas";
-ViewData["PageName"] = "multa_listaautuacao";
-ViewData["Heading"] = "<i class='fa-duotone fa-pen-to-square'></i> Multas: <span class='fw-300'>Autuações</span>";
-ViewData["Category1"] = "Autuações";
-ViewData["PageIcon"] = "fa-duotone fa-pen-to-square";
+
+    ViewData["Title"] = "Multas";
+    ViewData["PageName"] = "multa_listaautuacao";
+    ViewData["Heading"] = "<i class='fa-duotone fa-pen-to-square'></i> Multas: <span class='fw-300'>Autuações</span>";
+    ViewData["Category1"] = "Autuações";
+    ViewData["PageIcon"] = "fa-duotone fa-pen-to-square";
 }
 
 @section HeadBlock {
@@ -54,7 +54,7 @@
         .ftx-card-header .ftx-card-title i {
             color: #fff !important;
             --fa-primary-color: #fff !important;
-            --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
+            --fa-secondary-color: rgba(255,255,255,0.7) !important;
         }
 
         .ftx-card-header .btn-fundo-laranja {
@@ -136,8 +136,7 @@
             color: #fff !important;
         }
 
-        .btn-filtrar-autuacao:active,
-        .btn-filtrar-autuacao:focus {
+        .btn-filtrar-autuacao:active, .btn-filtrar-autuacao:focus {
             transform: translateY(0) scale(1) !important;
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
@@ -183,7 +182,7 @@
         #tblMulta tbody td {
             padding: 0.625rem;
             vertical-align: middle !important;
-            border-color: rgba(0, 0, 0, 0.05) !important;
+            border-color: rgba(0,0,0,0.05) !important;
         }
 
         /* ====== BADGES DE STATUS FrotiX ====== */
@@ -237,25 +236,11 @@
 
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
 
         /* ====== BOTÕES DE AÇÃO NA TABELA - COM GLOW FROTIX ====== */
@@ -305,14 +290,11 @@
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
 
@@ -321,14 +303,11 @@
             background-color: #d97706 !important;
             box-shadow: 0 0 8px rgba(217, 119, 6, 0.5), 0 2px 4px rgba(217, 119, 6, 0.3) !important;
         }
-
         .ftx-btn-status:hover:not([aria-disabled="true"]) {
             background-color: #b45309 !important;
             box-shadow: 0 0 20px rgba(217, 119, 6, 0.8), 0 6px 12px rgba(217, 119, 6, 0.5) !important;
         }
-
-        .ftx-btn-status:active,
-        .ftx-btn-status:focus {
+        .ftx-btn-status:active, .ftx-btn-status:focus {
             box-shadow: 0 0 0 0.2rem rgba(217, 119, 6, 0.35) !important;
         }
 
@@ -337,14 +316,11 @@
             background-color: #7c3aed !important;
             box-shadow: 0 0 8px rgba(124, 58, 237, 0.5), 0 2px 4px rgba(124, 58, 237, 0.3) !important;
         }
-
         .ftx-btn-penalidade:hover:not([aria-disabled="true"]) {
             background-color: #6d28d9 !important;
             box-shadow: 0 0 20px rgba(124, 58, 237, 0.8), 0 6px 12px rgba(124, 58, 237, 0.5) !important;
         }
-
-        .ftx-btn-penalidade:active,
-        .ftx-btn-penalidade:focus {
+        .ftx-btn-penalidade:active, .ftx-btn-penalidade:focus {
             box-shadow: 0 0 0 0.2rem rgba(124, 58, 237, 0.35) !important;
         }
 
@@ -353,14 +329,11 @@
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
 
@@ -369,14 +342,11 @@
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
 
@@ -469,7 +439,7 @@
         }
 
         #modalExibePDF .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -504,7 +474,7 @@
         }
 
         #modalAlteraStatus .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -545,7 +515,7 @@
             display: flex;
             justify-content: flex-end;
             align-items: center;
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 0.875rem 1.25rem;
             background: #f8f9fa;
             gap: 0.5rem;
@@ -615,53 +585,62 @@
 
                             <div class="ftx-autuacao-filter-group">
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
 
                             <div class="ftx-autuacao-filter-group">
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
 
                             <div class="ftx-autuacao-filter-group">
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
 
                             <div class="ftx-autuacao-filter-group">
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
 
                             <div class="ftx-autuacao-filter-group">
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
 
                             <div class="ftx-autuacao-filter-group">
                                 <label>&nbsp;</label>
-                                <a class="btn-filtrar-autuacao" id="btnFiltro" role="button"
-                                    data-ejtip="Filtrar Autuações" onclick="ListaTodasNotificacoes()">
+                                <a class="btn-filtrar-autuacao" id="btnFiltro" role="button" data-ejtip="Filtrar Autuações" onclick="ListaTodasNotificacoes()">
                                     <i class="fa-duotone fa-magnifying-glass-waveform"></i>
                                     Filtrar
                                 </a>
@@ -686,7 +665,7 @@
                                         <th>Até Vencimento</th>
                                         <th>Pós Vencimento</th>
                                         <th>Status</th>
-                                        <th>Ações</th>
+                                        <th>Ação</th>
                                     </tr>
                                 </thead>
                                 <tbody></tbody>
@@ -708,13 +687,14 @@
                         <i class="fa-duotone fa-file-lines"></i>
                         Ficha de Vistoria
                     </h4>
-                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                        aria-label="Fechar"></button>
+                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                 </div>
                 <div class="modal-body">
                     <div id="pdfViewerVistoria" style="height: 600px;">
-                        <ejs-pdfviewer id="pdfViewerVistoriaControl" style="height:100%"
-                            serviceUrl="/api/MultaPdfViewer" documentPath="">
+                        <ejs-pdfviewer id="pdfViewerVistoriaControl"
+                                       style="height:100%"
+                                       serviceUrl="/api/MultaPdfViewer"
+                                       documentPath="">
                         </ejs-pdfviewer>
                     </div>
                 </div>
@@ -729,8 +709,7 @@
     </div>
 </form>
 
-<div class="modal fade" id="modalExibePDF" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
+<div class="modal fade" id="modalExibePDF" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header">
@@ -738,14 +717,18 @@
                     <i class="fa-duotone fa-file-pdf"></i>
                     Notificação de Autuação
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <div id="pdfViewerAutuacao" style="height: 600px; width: 100%;">
-                    <ejs-pdfviewer id="pdfViewerAutuacaoControl" style="height:100%; width:100%;"
-                        serviceUrl="/api/MultaPdfViewer" documentPath="" enableToolbar="true"
-                        enableNavigationToolbar="true" enableThumbnail="true" zoomMode="FitToWidth">
+                    <ejs-pdfviewer id="pdfViewerAutuacaoControl"
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
@@ -759,8 +742,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalAlteraStatus" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true"
-    data-bs-backdrop="static" data-bs-keyboard="false">
+<div class="modal fade" id="modalAlteraStatus" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-md">
         <div class="modal-content">
             <div class="modal-header">
@@ -768,8 +750,7 @@
                     <i class="fa-duotone fa-bolt"></i>
                     Alterar Status da Autuação
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmStatusAutuacao">
@@ -813,9 +794,11 @@
                         <label class="label font-weight-bold">
                             <i class="fa-duotone fa-flag me-1"></i> Novo Status da Autuação
                         </label>
-                        <ejs-combobox id="lstStatusAlterado" placeholder="Selecione o novo status..."
-                            allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstStatusAlteracao"]"
-                            popupHeight="250px" width="100%" showClearButton="true">
+                        <ejs-combobox id="lstStatusAlterado"
+                                      placeholder="Selecione o novo status..."
+                                      allowFiltering="true" filterType="Contains"
+                                      dataSource="@ViewData["lstStatusAlteracao"]"
+                                      popupHeight="250px" width="100%" showClearButton="true">
                             <e-combobox-fields text="Status" value="StatusId"></e-combobox-fields>
                         </ejs-combobox>
                     </div>
@@ -836,8 +819,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalTransformaPenalidade" tabindex="-1" aria-labelledby="exampleModalLabel"
-    aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
+<div class="modal fade" id="modalTransformaPenalidade" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
             <div class="modal-header">
@@ -845,8 +827,7 @@
                     <i class="fa-duotone fa-money-bill-transfer"></i>
                     Transforma Autuação em Penalidade/Boleto
                 </h3>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmTransforma">
@@ -863,8 +844,7 @@
                         <div class="col-4">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Valor a Pagar</label>
-                                <input id="txtValorPenalidade" class="form-control form-control-xs"
-                                    style="text-align:right;" onkeypress="return moeda(this,'.',',',event)" />
+                                <input id="txtValorPenalidade" class="form-control form-control-xs" style="text-align:right;" onkeypress="return moeda(this,'.',',',event)" />
                             </div>
                         </div>
 
@@ -882,10 +862,8 @@
                         <div class="col-12">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Observações a respeito da Multa</label>
-                                <ejs-richtexteditor id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="150px"
-                                    created="onCreate">
-                                    <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
-                                        path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
+                                <ejs-richtexteditor id="rte" toolbarClick="toolbarClick" locale="pt-BR" height="150px" created="onCreate">
+                                    <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensViagens/"></e-richtexteditor-insertimagesettings>
                                 </ejs-richtexteditor>
                             </div>
                         </div>
@@ -897,16 +875,20 @@
                         <div class="col-12">
                             <div class="form-group">
                                 <label class="label font-weight-bold">Upload da Penalidade/Boleto (PDF)</label>
-                                <ejs-uploader id="uploaderPenalidade" name="UploadFiles" multiple="false"
-                                    autoUpload="true" allowedExtensions=".pdf">
+                                <ejs-uploader id="uploaderPenalidade"
+                                              name="UploadFiles"
+                                              multiple="false"
+                                              autoUpload="true"
+                                              allowedExtensions=".pdf">
                                     <e-uploader-asyncsettings saveUrl="/api/MultaUpload/save"
-                                        removeUrl="/api/MultaUpload/remove">
+                                                              removeUrl="/api/MultaUpload/remove">
                                     </e-uploader-asyncsettings>
                                 </ejs-uploader>
                                 <input id="txtPenalidadePDF" class="form-control form-control-xs" hidden />
                                 <div id="PDFContainer" style="margin-top: 15px;">
-                                    <ejs-pdfviewer id="pdfViewerPenalidadeControl" style="height:600px;width:100%;"
-                                        serviceUrl="/api/MultaPdfViewer">
+                                    <ejs-pdfviewer id="pdfViewerPenalidadeControl"
+                                                   style="height:600px;width:100%;"
+                                                   serviceUrl="/api/MultaPdfViewer">
                                     </ejs-pdfviewer>
                                 </div>
                             </div>
@@ -919,8 +901,7 @@
                     <i class="fa-duotone fa-xmark icon-space"></i>
                     Fechar
                 </button>
-                <button id="btnTransformaPenalidade" class="btn btn-verde" type="submit" value="SUBMIT"
-                    data-ejtip="Confirmar transformação">
+                <button id="btnTransformaPenalidade" class="btn btn-verde" type="submit" value="SUBMIT" data-ejtip="Confirmar transformação">
                     <span class="spinner-border spinner-border-sm d-none" role="status" aria-hidden="true"></span>
                     <i class="fa-duotone fa-money-bill-transfer icon-space"></i>
                     <span class="btn-text">Transformar em Penalidade</span>
@@ -932,8 +913,7 @@
 
 <div id="loadingOverlayAutuacao" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Autuações...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
@@ -941,31 +921,16 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="~/js/cadastros/tiraacento_001.js"></script>
     <script src="~/js/cadastros/listaautuacao.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LISTA AUTUAÇÃO - GERENCIAMENTO DE MULTAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de listagem e gestão de autuações / multas de trânsito.
-             * Inclui upload de PDFs, gestão de penalidades e pagamentos.
-             * @@requires jQuery DataTables, Syncfusion(RTE, Grid, Uploader), Bootstrap 5
-            * @@file Multa / ListaAutuacao.cshtml
-            */
 
         var defaultRTE;
 
-            /**
-             * Callback de criação do RichTextEditor
-             * @@description Armazena referência global do RTE para uso em modais
-            */
         function onCreate() {
             try {
                 defaultRTE = this;
@@ -975,11 +940,6 @@
             }
         }
 
-            /**
-             * Handler de clique na toolbar do RichTextEditor
-             * @@param { Object } e - Evento com item.id indicando botão clicado
-            * @@description Configura token XSRF para upload de imagens no RTE
-                */
         function toolbarClick(e) {
             try {
                 if (e.item.id === "rte_toolbar_Image") {
@@ -1172,7 +1132,7 @@
                             if (data.success) {
                                 AppToast.show('Verde', data.message, 2000);
                                 $('#modalAlteraStatus').modal('hide');
-                                setTimeout(function () {
+                                setTimeout(function() {
                                     $('#tblMulta').DataTable().ajax.reload(null, false);
                                 }, 300);
                             }
@@ -1210,20 +1170,7 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MÁSCARA DE MOEDA PT-BR
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Aplica máscara de moeda brasileira (R$) ao input
-             * @@param { HTMLInputElement } a - Campo de input
-            * @@param { string } e - Separador de milhar(.)
-                * @@param { string } r - Separador decimal(,)
-                    * @@param { Event } t - Evento de teclado
-                        * @@returns { boolean } True para permitir input, false para bloquear
-                            */
+
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = 0, j = 0, u = 0, tamanho2 = 0, ajd2 = "", l = "";
@@ -1424,17 +1371,7 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE UPLOAD - PDF PENALIDADE
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia upload, validação e exibição de PDFs de penalidade.
-             */
-
-            /**
-             * Obtém instância do PDF Viewer de Penalidade
-             * @@returns { Object | null } Instância Syncfusion PdfViewer ou null
-            */
+
         function getPenalidadeViewer() {
             try {
                 const viewerElement = document.getElementById('pdfViewerPenalidadeControl');
@@ -1445,11 +1382,6 @@
             }
         }
 
-            /**
-             * Carrega PDF no viewer de penalidade
-             * @@param { string } fileName - Nome do arquivo PDF(path relativo)
-            * @@description Configura documentPath e executa load no viewer
-                */
         function loadPdfInPenalidadeViewer(fileName) {
             try {
                 if (!fileName || fileName === '' || fileName === 'null') {
@@ -1472,37 +1404,27 @@
             }
         }
 
-            /**
-             * Valida arquivo selecionado para upload
-             * @@param { Object } args - Evento do Uploader com filesData
-            * @@description Cancela upload se não for PDF
-                */
-            function onPenalidadeUploadSelected(args) {
-                try {
-                    if (!args || !args.filesData || args.filesData.length === 0) return;
-
-                    const file = args.filesData[0];
-                    const fileName = (file?.name || "").toLowerCase();
-
-                    if (!fileName.endsWith(".pdf")) {
-                        args.cancel = true;
-
-                        if (window.AppToast?.show) {
-                            AppToast.show('Vermelho', 'Apenas arquivos PDF são permitidos', 3000);
-                        } else {
-                            Alerta.Warning('Arquivo Inválido', 'Apenas arquivos PDF são permitidos');
-                        }
+        function onPenalidadeUploadSelected(args) {
+            try {
+                if (!args || !args.filesData || args.filesData.length === 0) return;
+
+                const file = args.filesData[0];
+                const fileName = (file?.name || "").toLowerCase();
+
+                if (!fileName.endsWith(".pdf")) {
+                    args.cancel = true;
+
+                    if (window.AppToast?.show) {
+                        AppToast.show('Vermelho', 'Apenas arquivos PDF são permitidos', 3000);
+                    } else {
+                        Alerta.Warning('Arquivo Inválido', 'Apenas arquivos PDF são permitidos');
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("ListaAutuacao.cshtml", "onPenalidadeUploadSelected", error);
-                }
-            }
-
-            /**
-             * Handler de sucesso no upload de PDF
-             * @@param { Object } args - Resposta do servidor com dados do arquivo
-            * @@description Atualiza campo txtPenalidadePDF e carrega no viewer
-                */
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("ListaAutuacao.cshtml", "onPenalidadeUploadSelected", error);
+            }
+        }
+
         function onPenalidadeUploadSuccess(args) {
             try {
                 if (!args || !args.e) return;
@@ -1553,11 +1475,6 @@
             }
         }
 
-            /**
-             * Handler de falha no upload de PDF
-             * @@param { Object } args - Dados do erro de upload
-            * @@description Exibe toast / alerta de erro ao usuário
-                */
         function onPenalidadeUploadFailure(args) {
             try {
                 console.error("Erro no upload:", args);
@@ -1572,7 +1489,7 @@
             }
         }
 
-        $(document).ready(function () {
+        $(document).ready(function() {
             try {
                 const uploaderPenalidade = document.getElementById("uploaderPenalidade")?.ej2_instances?.[0];
                 if (uploaderPenalidade) {
```
