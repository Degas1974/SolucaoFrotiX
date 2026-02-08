# Pages/Viagens/GestaoFluxo.cshtml

**Mudanca:** GRANDE | **+55** linhas | **-73** linhas

---

```diff
--- JANEIRO: Pages/Viagens/GestaoFluxo.cshtml
+++ ATUAL: Pages/Viagens/GestaoFluxo.cshtml
@@ -21,7 +21,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -32,6 +32,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
 
 }
@@ -108,8 +109,7 @@
     }
 
     /* Igualar altura dos campos - 34px */
-    #txtDataDe,
-    #txtDataAte {
+    #txtData {
         height: 34px !important;
         padding: 0.375rem 0.75rem !important;
         font-size: 0.875rem !important;
@@ -183,31 +183,13 @@
 </style>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * GESTÃO DE FLUXO DE PASSAGEIROS - DATATABLE SERVER-SIDE
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Módulo de gestão de fichas de passageiros com DataTable server - side,
-     * filtros por data / veículo / motorista, edição via modal e exclusão.
-     * @@requires jQuery, DataTables, Syncfusion EJ2 ComboBox, Bootstrap Modal
-        * @@file GestaoFluxo.cshtml
-        */
-
     let dataTable;
 
-    /**
-     * Exibe o overlay de loading padrão FrotiX
-     * @@description Mostra sobreposição visual durante operações assíncronas
-        */
     function mostrarLoading() {
         const overlay = document.getElementById('loadingOverlayGestaoFluxo');
         if (overlay) overlay.style.display = 'flex';
     }
 
-    /**
-     * Oculta o overlay de loading padrão FrotiX
-     * @@description Remove a sobreposição visual após conclusão da operação
-        */
     function esconderLoading() {
         const overlay = document.getElementById('loadingOverlayGestaoFluxo');
         if (overlay) overlay.style.display = 'none';
@@ -222,8 +204,7 @@
             const ano = hoje.getFullYear();
             const dataFormatada = `${ano}-${mes}-${dia}`;
 
-            $('#txtDataDe').val(dataFormatada);
-            $('#txtDataAte').val(dataFormatada);
+            $('#txtData').val(dataFormatada);
 
             dataTable = $('#tblFluxo').DataTable({
                 processing: false,
@@ -254,15 +235,14 @@
                             const veiculos = document.getElementById('lstVeiculos')?.ej2_instances?.[0];
                             const motoristas = document.getElementById('lstMotorista')?.ej2_instances?.[0];
 
-                            d.dataFluxoDe = $('#txtDataDe').val() ?? '';
-                            d.dataFluxoAte = $('#txtDataAte').val() ?? '';
+                            d.dataFluxo = $('#txtData').val() ?? '';
                             d.veiculoId = veiculos ? veiculos.value : null;
                             d.motoristaId = motoristas ? motoristas.value : null;
                         } catch (error) {
                             Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml", "dataTable.ajax.data", error);
                         }
                     },
-                    beforeSend: function () {
+                    beforeSend: function() {
                         try {
 
                             $('body').addClass('datatable-loading');
@@ -271,7 +251,7 @@
                             Alerta.TratamentoErroComLinha("GestaoFluxo.cshtml", "dataTable.ajax.beforeSend", error);
                         }
                     },
-                    complete: function () {
+                    complete: function() {
                         try {
 
                             $('body').removeClass('datatable-loading');
@@ -532,7 +512,7 @@
                 }
             });
 
-            $('#txtDataDe, #txtDataAte').on('keypress', function (e) {
+            $('#txtData').on('keypress', function (e) {
                 try {
                     if (e.which === 13) AplicarFiltros();
                 } catch (error) {
@@ -545,10 +525,6 @@
         }
     });
 
-    /**
-     * Aplica os filtros selecionados e recarrega o DataTable
-     * @@description Adiciona spinner no botão, mostra overlay e dispara reload server - side
-        */
     function AplicarFiltros() {
         try {
             const btn = document.getElementById('btnAplicarFiltro');
@@ -560,7 +536,7 @@
             mostrarLoading();
 
             if (dataTable) {
-                dataTable.ajax.reload(function () {
+                dataTable.ajax.reload(function() {
                     try {
 
                         iconOriginal.className = iconClasses;
@@ -575,14 +551,9 @@
         }
     }
 
-    /**
-     * Limpa todos os filtros e recarrega a tabela
-     * @@description Zera campos de data e ComboBoxes, depois recarrega DataTable
-        */
     function LimparFiltros() {
         try {
-            $('#txtDataDe').val("");
-            $('#txtDataAte').val("");
+            $('#txtData').val("");
             const veiculos = document.getElementById('lstVeiculos')?.ej2_instances?.[0];
             const motoristas = document.getElementById('lstMotorista')?.ej2_instances?.[0];
             if (veiculos) veiculos.value = null;
@@ -625,43 +596,48 @@
                         <div class="col-12">
                             <div class="form-group row align-items-end">
                                 <div class="col-2">
-                                    <label class="label">De:</label>
-                                    <input id="txtDataDe" class="form-control form-control-xs" type="date" />
+                                    <label class="label">Escolha uma Data</label>
+                                    <input id="txtData" class="form-control form-control-xs" type="date" />
                                 </div>
 
-                                <div class="col-2">
-                                    <label class="label">Até:</label>
-                                    <input id="txtDataAte" class="form-control form-control-xs" type="date" />
-                                </div>
-
-                                <div class="col-2">
+                                <div class="col-3">
                                     <label class="label">Veículo (Economildo)</label>
-                                    <ejs-combobox id="lstVeiculos" placeholder="Selecione um Veículo"
-                                        allowFiltering="true" filterType="Contains" dataSource="@listaVeiculos"
-                                        popupHeight="250px" width="100%" cssClass="e-small" showClearButton="true">
+                                    <ejs-combobox id="lstVeiculos"
+                                                  placeholder="Selecione um Veículo"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@listaVeiculos"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  cssClass="e-small"
+                                                  showClearButton="true">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
 
-                                <div class="col-2">
+                                <div class="col-3">
                                     <label class="label">Motorista</label>
-                                    <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista"
-                                        allowFiltering="true" filterType="Contains" dataSource="@listaMotorista"
-                                        popupHeight="250px" width="100%" cssClass="e-small" showClearButton="true">
+                                    <ejs-combobox id="lstMotorista"
+                                                  placeholder="Selecione um Motorista"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@listaMotorista"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  cssClass="e-small"
+                                                  showClearButton="true">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
 
                                 <div class="col-2">
-                                    <button id="btnAplicarFiltro" type="button"
-                                        class="btn btn-azul form-control form-control-xs" onclick="AplicarFiltros()">
+                                    <button id="btnAplicarFiltro" type="button" class="btn btn-azul form-control form-control-xs" onclick="AplicarFiltros()">
                                         <i class="fa-duotone fa-magnifying-glass icon-space"></i> Aplicar
                                     </button>
                                 </div>
 
                                 <div class="col-2 text-end">
-                                    <button type="button" class="btn btn-vinho form-control form-control-xs"
-                                        onclick="LimparFiltros()">
+                                    <button type="button" class="btn btn-vinho form-control form-control-xs" onclick="LimparFiltros()">
                                         <i class="fa-duotone fa-eraser icon-space"></i> Limpar
                                     </button>
                                 </div>
@@ -680,7 +656,7 @@
                                             <th>Hora Início</th>
                                             <th>Hora Fim</th>
                                             <th>Quantidade</th>
-                                            <th>Ações</th>
+                                            <th>Ação</th>
                                         </tr>
                                     </thead>
                                     <tbody></tbody>
@@ -694,16 +670,14 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalEditaRegistro" tabindex="-1" aria-labelledby="modalEditaRegistroLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalEditaRegistro" tabindex="-1" aria-labelledby="modalEditaRegistroLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
             <div class="modal-header ftx-modal-header-terracota">
                 <h3 class="modal-title" id="modalEditaRegistroLabel">
                     <i class="fa-duotone fa-pen-to-square"></i> Editar Registro
                 </h3>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Close"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
             </div>
             <div class="modal-body">
                 <form id="frmOcorrencia">
@@ -744,17 +718,27 @@
                     <div class="form-group row mt-2">
                         <div class="col-6">
                             <label class="label">Economildo (veículo)</label>
-                            <ejs-combobox id="lstVeiculosEconomildos" allowFiltering="true" filterType="Contains"
-                                dataSource="@listaVeiculos" popupHeight="250px" width="100%" cssClass="e-small"
-                                showClearButton="true">
+                            <ejs-combobox id="lstVeiculosEconomildos"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@listaVeiculos"
+                                          popupHeight="250px"
+                                          width="100%"
+                                          cssClass="e-small"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
                         <div class="col-6">
                             <label class="label">Motorista</label>
-                            <ejs-combobox id="lstMotoristasEconomildo" allowFiltering="true" filterType="Contains"
-                                dataSource="@listaMotorista" popupHeight="250px" width="100%" cssClass="e-small"
-                                showClearButton="true">
+                            <ejs-combobox id="lstMotoristasEconomildo"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@listaMotorista"
+                                          popupHeight="250px"
+                                          width="100%"
+                                          cssClass="e-small"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -790,8 +774,7 @@
 
 <div id="loadingOverlayGestaoFluxo" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Fluxo de Viagens...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
@@ -799,8 +782,6 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 }
```
