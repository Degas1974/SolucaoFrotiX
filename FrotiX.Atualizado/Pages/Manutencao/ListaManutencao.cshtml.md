# Pages/Manutencao/ListaManutencao.cshtml

**Mudanca:** GRANDE | **+42** linhas | **-71** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/ListaManutencao.cshtml
+++ ATUAL: Pages/Manutencao/ListaManutencao.cshtml
@@ -29,7 +29,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -40,6 +40,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
     <link href="~/css/ftx-card-styled.css" rel="stylesheet" asp-append-version="true" />
 }
@@ -67,7 +68,7 @@
     .ftx-card-header .ftx-card-title i {
         color: #fff !important;
         --fa-primary-color: #fff !important;
-        --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
+        --fa-secondary-color: rgba(255,255,255,0.7) !important;
     }
 
     /* Botão laranja usa padrão global do frotix.css */
@@ -200,7 +201,7 @@
     #tblManutencao tbody td {
         padding: 0.625rem;
         vertical-align: middle !important;
-        border-color: rgba(0, 0, 0, 0.05) !important;
+        border-color: rgba(0,0,0,0.05) !important;
     }
 
     /* ====== BADGES DE STATUS ====== */
@@ -241,7 +242,7 @@
 
     /* ====== BOTÕES DE AÇÃO ====== */
     #tblManutencao td:last-child .text-center,
-    #tblManutencao td:last-child>div {
+    #tblManutencao td:last-child > div {
         display: flex;
         justify-content: center;
         align-items: center;
@@ -269,7 +270,7 @@
 
     .ftx-manut-btn-icon:hover:not([aria-disabled="true"]) {
         transform: translateY(-2px);
-        box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2) !important;
+        box-shadow: 0 4px 10px rgba(0,0,0,0.2) !important;
     }
 
     .ftx-manut-btn-icon[aria-disabled="true"] {
@@ -346,13 +347,6 @@
 </style>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * CONTROLE DE ESTADO: FLAGS GLOBAIS DE FILTROS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Variáveis de estado que indicam qual filtro está ativo.
-     * Usadas para coordenar comportamento entre ComboBoxes e DataTables.
-     */
     window.escolhendoVeiculo = window.escolhendoVeiculo ?? false;
     window.escolhendoData = window.escolhendoData ?? false;
     window.escolhendoStatus = window.escolhendoStatus ?? false;
@@ -360,11 +354,6 @@
     window.URLapi = window.URLapi ?? "";
     window.IDapi = window.IDapi ?? "";
 
-    /**
-     * Ativa flag indicando que usuário está selecionando veículo.
-     * Reseta outras flags de escolha para evitar conflitos de filtro.
-     * @@description Chamada pelo evento de foco do ComboBox de veículos.
-     */
     function DefineEscolhaVeiculo() {
         try {
             escolhendoVeiculo = true;
@@ -380,25 +369,16 @@
         }
     }
 
-    /**
-     * Ativa flag indicando que usuário está selecionando data.
-     * @@description Chamada pelo evento de foco do DatePicker.
-     */
     function DefineEscolhaData() {
-            try {
-                escolhendoVeiculo = false;
-                escolhendoData = true;
-                escolhendoStatus = false;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("ListaManutencao.cshtml", "DefineEscolhaData", error);
-            }
-        }
-
-    /**
-     * Ativa flag indicando que usuário está selecionando status.
-     * Reseta outras flags de escolha.
-     * @@description Chamada pelo evento de foco do ComboBox de status.
-     */
+        try {
+            escolhendoVeiculo = false;
+            escolhendoData = true;
+            escolhendoStatus = false;
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("ListaManutencao.cshtml", "DefineEscolhaData", error);
+        }
+    }
+
     function DefineEscolhaStatus() {
         try {
             escolhendoVeiculo = false;
@@ -414,10 +394,6 @@
         }
     }
 
-    /**
-     * Callback disparado quando o valor do ComboBox de veículos muda.
-     * @@description Reservado para lógica adicional de filtro(stub atual).
-     */
     function VeiculosValueChange() {
         try {
         } catch (error) {
@@ -425,10 +401,6 @@
         }
     }
 
-    /**
-     * Callback disparado quando o valor do ComboBox de status muda.
-     * @@description Reservado para lógica adicional de filtro(stub atual).
-     */
     function StatusValueChange() {
         try {
         } catch (error) {
@@ -509,18 +481,22 @@
 
                         <div class="ftx-manut-filter-group" style="grid-column: span 2;">
                             <label>Veículo</label>
-                            <ejs-combobox id="lstVeiculos" placeholder="Selecione um Veículo..." allowFiltering="true"
-                                filterType="Contains" dataSource="@ViewData["lstVeiculos"]" popupHeight="250px"
-                                width="100%" showClearButton="true">
+                            <ejs-combobox id="lstVeiculos"
+                                          placeholder="Selecione um Veículo..."
+                                          allowFiltering="true" filterType="Contains"
+                                          dataSource="@ViewData["lstVeiculos"]"
+                                          popupHeight="250px" width="100%" showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
 
                         <div class="ftx-manut-filter-group">
                             <label>Status</label>
-                            <ejs-combobox id="lstStatus" placeholder="Status..." allowFiltering="true"
-                                filterType="Contains" dataSource="@ViewData["lstStatus"]" popupHeight="250px"
-                                width="100%" showClearButton="true">
+                            <ejs-combobox id="lstStatus"
+                                          placeholder="Status..."
+                                          allowFiltering="true" filterType="Contains"
+                                          dataSource="@ViewData["lstStatus"]"
+                                          popupHeight="250px" width="100%" showClearButton="true">
                                 <e-combobox-fields text="Status" value="StatusId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -550,7 +526,7 @@
                                     <th>Reserva</th>
                                     <th>Resumo Solicitação</th>
                                     <th>Status</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                 </tr>
                             </thead>
                             <tbody></tbody>
@@ -570,8 +546,7 @@
                     <i class="fa-duotone fa-file-check"></i>
                     Fechar Ordem de Serviço
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -584,10 +559,8 @@
                             <input id="txtDataDevolucao" class="form-control" type="date" />
                         </div>
                         <div class="col-md-9">
-                            <label class="form-label fw-semibold" for="txtResumoOS">Resumo/Observações da Ordem de
-                                Serviço</label>
-                            <input id="txtResumoOS" class="form-control"
-                                placeholder="Descreva o serviço realizado..." />
+                            <label class="form-label fw-semibold" for="txtResumoOS">Resumo/Observações da Ordem de Serviço</label>
+                            <input id="txtResumoOS" class="form-control" placeholder="Descreva o serviço realizado..." />
                         </div>
                     </div>
 
@@ -606,25 +579,26 @@
                         <div id="divReserva" class="col-md-9" style="display:none">
                             <div class="row g-3">
                                 <div class="col-md-6">
-                                    <label class="form-label fw-semibold" for="lstVeiculoReserva">Veículo
-                                        Reserva</label>
-                                    <ejs-combobox id="lstVeiculoReserva" placeholder="Selecione um Veículo"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["lstVeiculosReserva"]" popupHeight="200px" width="100%"
-                                        showClearButton="true">
+                                    <label class="form-label fw-semibold" for="lstVeiculoReserva">Veículo Reserva</label>
+                                    <ejs-combobox id="lstVeiculoReserva"
+                                                  placeholder="Selecione um Veículo"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["lstVeiculosReserva"]"
+                                                  popupHeight="200px"
+                                                  width="100%"
+                                                  showClearButton="true">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
 
                                 <div class="col-md-3">
-                                    <label class="form-label fw-semibold" for="txtDataRecebimentoReserva">Recebimento
-                                        Reserva</label>
+                                    <label class="form-label fw-semibold" for="txtDataRecebimentoReserva">Recebimento Reserva</label>
                                     <input id="txtDataRecebimentoReserva" class="form-control" type="date" />
                                 </div>
 
                                 <div class="col-md-3">
-                                    <label class="form-label fw-semibold" for="txtDataDevolucaoReserva">Devolução
-                                        Reserva</label>
+                                    <label class="form-label fw-semibold" for="txtDataDevolucaoReserva">Devolução Reserva</label>
                                     <input id="txtDataDevolucaoReserva" class="form-control" type="date" />
                                 </div>
                             </div>
@@ -644,7 +618,7 @@
                                     <th>Data</th>
                                     <th>Motorista</th>
                                     <th>Resumo Ocorrência</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                     <th>ItemManutençãoID</th>
                                     <th>Descrição</th>
                                     <th>Status</th>
@@ -681,13 +655,12 @@
                     <i class="fa-duotone fa-image"></i>
                     Foto da Ocorrência
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body text-center">
                 <img class="img-fluid img-ocorrencia-viewer" id="imgViewerOcorrencia" alt="Imagem da ocorrência"
-                    style="display:none; border-radius: 10px; max-height: 450px;" />
+                     style="display:none; border-radius: 10px; max-height: 450px;" />
                 <div class="no-image-placeholder" id="noImagePlaceholder">
                     <i class="fa-duotone fa-image-slash"></i>
                     <p>Nenhuma imagem disponível para esta ocorrência</p>
@@ -706,8 +679,7 @@
 
 <div id="loadingOverlayManutencao" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Manutencoes...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
```
