# Pages/Administracao/AjustaCustosViagem.cshtml

**Mudanca:** GRANDE | **+15** linhas | **-47** linhas

---

```diff
--- JANEIRO: Pages/Administracao/AjustaCustosViagem.cshtml
+++ ATUAL: Pages/Administracao/AjustaCustosViagem.cshtml
@@ -115,7 +115,6 @@
 
         /* ===== AJUSTES RESPONSIVOS ===== */
         @@media (max-width: 768px) {
-
             #modalAjustaCustos .modal-dialog,
             #modalFicha .modal-dialog {
                 max-width: 95%;
@@ -126,28 +125,16 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * HANDLER DE MUDANÇA DE FINALIDADE
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Controla visibilidade do dropdown de Eventos
-        * Quando a finalidade é "Evento", habilita e exibe o campo de seleção de evento
-            * @@returns { void}
-            */
     function FinalidadeChange() {
         try {
-            /** @@type { Object } Instância Syncfusion do ComboBox de Finalidade */
             var finalidadeCb = document.getElementById('lstFinalidadeAlterada').ej2_instances[0];
-            /** @@type { Object } Instância Syncfusion do ComboBox de Evento */
             var eventoDdt = document.getElementById('lstEvento').ej2_instances[0];
 
             if (finalidadeCb && eventoDdt) {
                 if (finalidadeCb.value === 'Evento') {
-
                     eventoDdt.enabled = true;
                     $(".esconde-diveventos").show();
                 } else {
-
                     eventoDdt.enabled = false;
                     eventoDdt.value = null;
                     $(".esconde-diveventos").hide();
@@ -189,8 +176,7 @@
                             </div>
 
                             <div id="divViagens">
-                                <table id="tblViagem" class="table table-bordered table-striped table-hover ftx-table"
-                                    width="100%">
+                                <table id="tblViagem" class="table table-bordered table-striped table-hover ftx-table" width="100%">
                                     <thead>
                                         <tr>
                                             <th>Vistoria</th>
@@ -203,7 +189,7 @@
                                             <th>Veiculo</th>
                                             <th>Km Inicial</th>
                                             <th>Km Final</th>
-                                            <th>Ações</th>
+                                            <th>Ação</th>
                                             <th></th>
                                         </tr>
                                     </thead>
@@ -227,8 +213,7 @@
                     <i class="fa-duotone fa-pen-to-square"></i>
                     Editar a Viagem
                 </h3>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmViagem">
@@ -246,24 +231,16 @@
                                 <div class="col-6">
                                     <div class="form-group">
                                         <label class="label font-weight-bold">Finalidade da Viagem</label>
-                                        <ejs-combobox id="lstFinalidadeAlterada" placeholder="Selecione uma Finalidade"
-                                            allowFiltering="true" filterType="Contains"
-                                            dataSource="@ViewData["lstFinalidade"]" popupHeight="250px" width="100%"
-                                            showClearButton="true" change="FinalidadeChange">
-                                            <e-combobox-fields text="Descricao"
-                                                value="FinalidadeId"></e-combobox-fields>
+                                        <ejs-combobox id="lstFinalidadeAlterada" placeholder="Selecione uma Finalidade" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstFinalidade"]" popupHeight="250px" width="100%" showClearButton="true" change="FinalidadeChange">
+                                            <e-combobox-fields text="Descricao" value="FinalidadeId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
                                 </div>
                                 <div class="col-6">
                                     <div class="form-group">
                                         <label class="label font-weight-bold">Nome do Evento</label>
-                                        <ejs-dropdowntree id="lstEvento" placeholder="Selecione um Evento..."
-                                            showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
-                                            filterType="Contains" filterBarPlaceholder="Procurar..."
-                                            popupHeight="200px">
-                                            <e-dropdowntree-fields dataSource="@ViewData["lstEvento"]" value="EventoId"
-                                                text="Evento"></e-dropdowntree-fields>
+                                        <ejs-dropdowntree id="lstEvento" placeholder="Selecione um Evento..." showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar..." popupHeight="200px">
+                                            <e-dropdowntree-fields dataSource="@ViewData["lstEvento"]" value="EventoId" text="Evento"></e-dropdowntree-fields>
                                         </ejs-dropdowntree>
                                     </div>
                                 </div>
@@ -316,17 +293,13 @@
                     <div class="row">
                         <div class="col-6">
                             <label class="label font-weight-bold">Motorista</label>
-                            <ejs-combobox id="lstMotoristaAlterado" placeholder="Selecione um Motorista"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstMotoristaAlterado" placeholder="Selecione um Motorista" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]" popupHeight="250px" width="100%" showClearButton="true">
                                 <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
                         <div class="col-6">
                             <label class="label font-weight-bold">Veículo</label>
-                            <ejs-combobox id="lstVeiculoAlterado" placeholder="Selecione um Veículo"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstVeiculos"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstVeiculoAlterado" placeholder="Selecione um Veículo" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstVeiculos"]" popupHeight="250px" width="100%" showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -335,9 +308,7 @@
                     <div class="row mt-3">
                         <div class="col-5">
                             <label class="label font-weight-bold">Solicitante</label>
-                            <ejs-combobox id="lstRequisitanteAlterado" placeholder="Selecione um Solicitante"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstRequisitante"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstRequisitanteAlterado" placeholder="Selecione um Solicitante" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstRequisitante"]" popupHeight="250px" width="100%" showClearButton="true">
                                 <e-combobox-fields text="Requisitante" value="RequisitanteId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -347,11 +318,8 @@
                         </div>
                         <div class="col-5">
                             <label class="label font-weight-bold">Setor Solicitante</label>
-                            <ejs-dropdowntree id="lstSetorSolicitanteAlterado" placeholder="Selecione um Setor"
-                                sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
-                                allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
-                                <e-dropdowntree-fields dataSource="@ViewData["lstSetor"]" value="SetorSolicitanteId"
-                                    text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
+                            <ejs-dropdowntree id="lstSetorSolicitanteAlterado" placeholder="Selecione um Setor" sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
+                                <e-dropdowntree-fields dataSource="@ViewData["lstSetor"]" value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
                             </ejs-dropdowntree>
                         </div>
                     </div>
@@ -374,8 +342,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalFicha" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true"
-    aria-labelledby="DynamicModalLabel" aria-hidden="true">
+<div class="modal fade" id="modalFicha" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true" aria-labelledby="DynamicModalLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
@@ -383,8 +350,7 @@
                     <i class="fa-duotone fa-file-image"></i>
                     Ficha de Vistoria
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body text-center">
                 <input type="hidden" id="txtViagemId" />
@@ -401,8 +367,7 @@
 
 <div id="loadingOverlayCustos" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text" id="txtLoadingMessage">Carregando Dados de Viagens...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
```

### REMOVER do Janeiro

```html
    /**
     * ═══════════════════════════════════════════════════════════════════════════
     * HANDLER DE MUDANÇA DE FINALIDADE
     * ═══════════════════════════════════════════════════════════════════════════
     * @@description Controla visibilidade do dropdown de Eventos
        * Quando a finalidade é "Evento", habilita e exibe o campo de seleção de evento
            * @@returns { void}
            */
            /** @@type { Object } Instância Syncfusion do ComboBox de Finalidade */
            /** @@type { Object } Instância Syncfusion do ComboBox de Evento */
                                <table id="tblViagem" class="table table-bordered table-striped table-hover ftx-table"
                                    width="100%">
                                            <th>Ações</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
                                        <ejs-combobox id="lstFinalidadeAlterada" placeholder="Selecione uma Finalidade"
                                            allowFiltering="true" filterType="Contains"
                                            dataSource="@ViewData["lstFinalidade"]" popupHeight="250px" width="100%"
                                            showClearButton="true" change="FinalidadeChange">
                                            <e-combobox-fields text="Descricao"
                                                value="FinalidadeId"></e-combobox-fields>
                                        <ejs-dropdowntree id="lstEvento" placeholder="Selecione um Evento..."
                                            showCheckBox="false" allowMultiSelection="false" allowFiltering="true"
                                            filterType="Contains" filterBarPlaceholder="Procurar..."
                                            popupHeight="200px">
                                            <e-dropdowntree-fields dataSource="@ViewData["lstEvento"]" value="EventoId"
                                                text="Evento"></e-dropdowntree-fields>
                            <ejs-combobox id="lstMotoristaAlterado" placeholder="Selecione um Motorista"
                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
                                popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-combobox id="lstVeiculoAlterado" placeholder="Selecione um Veículo"
                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstVeiculos"]"
                                popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-combobox id="lstRequisitanteAlterado" placeholder="Selecione um Solicitante"
                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstRequisitante"]"
                                popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-dropdowntree id="lstSetorSolicitanteAlterado" placeholder="Selecione um Setor"
                                sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false"
                                allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
                                <e-dropdowntree-fields dataSource="@ViewData["lstSetor"]" value="SetorSolicitanteId"
                                    text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
<div class="modal fade" id="modalFicha" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true"
    aria-labelledby="DynamicModalLabel" aria-hidden="true">
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
                    aria-label="Fechar"></button>
        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
            style="display: block;" />
```


### ADICIONAR ao Janeiro

```html
                                <table id="tblViagem" class="table table-bordered table-striped table-hover ftx-table" width="100%">
                                            <th>Ação</th>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                                        <ejs-combobox id="lstFinalidadeAlterada" placeholder="Selecione uma Finalidade" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstFinalidade"]" popupHeight="250px" width="100%" showClearButton="true" change="FinalidadeChange">
                                            <e-combobox-fields text="Descricao" value="FinalidadeId"></e-combobox-fields>
                                        <ejs-dropdowntree id="lstEvento" placeholder="Selecione um Evento..." showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar..." popupHeight="200px">
                                            <e-dropdowntree-fields dataSource="@ViewData["lstEvento"]" value="EventoId" text="Evento"></e-dropdowntree-fields>
                            <ejs-combobox id="lstMotoristaAlterado" placeholder="Selecione um Motorista" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]" popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-combobox id="lstVeiculoAlterado" placeholder="Selecione um Veículo" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstVeiculos"]" popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-combobox id="lstRequisitanteAlterado" placeholder="Selecione um Solicitante" allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstRequisitante"]" popupHeight="250px" width="100%" showClearButton="true">
                            <ejs-dropdowntree id="lstSetorSolicitanteAlterado" placeholder="Selecione um Setor" sortOrder="Ascending" showCheckBox="false" allowMultiSelection="false" allowFiltering="true" filterType="Contains" filterBarPlaceholder="Procurar...">
                                <e-dropdowntree-fields dataSource="@ViewData["lstSetor"]" value="SetorSolicitanteId" text="Nome" parentValue="SetorPaiId" hasChildren="HasChild"></e-dropdowntree-fields>
<div class="modal fade" id="modalFicha" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true" aria-labelledby="DynamicModalLabel" aria-hidden="true">
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
```
