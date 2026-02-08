# Pages/MovimentacaoPatrimonio/Upsert.cshtml

**Mudanca:** GRANDE | **+72** linhas | **-77** linhas

---

```diff
--- JANEIRO: Pages/MovimentacaoPatrimonio/Upsert.cshtml
+++ ATUAL: Pages/MovimentacaoPatrimonio/Upsert.cshtml
@@ -38,14 +38,14 @@
         border: 1px solid #ced4da !important;
     }
 
-    .e-control-wrapper.e-input-group input.e-input {
-        height: calc(var(--ctrl-h) - 2px) !important;
-        line-height: calc(var(--ctrl-h) - 2px) !important;
-        padding: var(--ctrl-pad-y) var(--ctrl-pad-x) !important;
-        font-size: 1rem !important;
-        background: transparent !important;
-        border: none !important;
-    }
+        .e-control-wrapper.e-input-group input.e-input {
+            height: calc(var(--ctrl-h) - 2px) !important;
+            line-height: calc(var(--ctrl-h) - 2px) !important;
+            padding: var(--ctrl-pad-y) var(--ctrl-pad-x) !important;
+            font-size: 1rem !important;
+            background: transparent !important;
+            border: none !important;
+        }
 
     .e-input-group-icon,
     .e-clear-icon,
@@ -58,7 +58,7 @@
     .e-control-wrapper.e-input-focus,
     .e-input-group.e-control-wrapper.e-input-focus {
         border-color: #80bdff !important;
-        box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, .25) !important;
+        box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25) !important;
     }
 
     .e-input-group::before,
@@ -75,11 +75,11 @@
         font-size: 1.05rem;
     }
 
-    .section-legend .legend-note {
-        font-weight: 500;
-        font-size: .9rem;
-        color: #6c757d;
-    }
+        .section-legend .legend-note {
+            font-weight: 500;
+            font-size: .9rem;
+            color: #6c757d;
+        }
 
     .label {
         margin-bottom: .25rem;
@@ -113,10 +113,10 @@
         color: var(--accent-orange-dark) !important;
     }
 
-    .section-legend.text-orange-dark .fa-duotone {
-        --fa-primary-color: var(--accent-orange-dark);
-        --fa-secondary-color: var(--accent-orange-dark);
-    }
+        .section-legend.text-orange-dark .fa-duotone {
+            --fa-primary-color: var(--accent-orange-dark);
+            --fa-secondary-color: var(--accent-orange-dark);
+        }
 </style>
 
 <form id="formsMovimentacaoPatrimonio">
@@ -128,7 +128,7 @@
                         <div asp-validation-summary="ModelOnly" class="text-danger"></div>
 
                         <input type="hidden" id="MovimentacaoPatrimonioId"
-                            value="@(Model.MovimentacaoPatrimonioObj?.MovimentacaoPatrimonio?.MovimentacaoPatrimonioId ?? Guid.Empty)" />
+                               value="@(Model.MovimentacaoPatrimonioObj?.MovimentacaoPatrimonio?.MovimentacaoPatrimonioId ?? Guid.Empty)" />
                         <input type="hidden" id="SetorOrigemId" value="" />
                         <input type="hidden" id="SecaoOrigemId" value="" />
                         <input type="hidden" id="PatrimonioId" value="" />
@@ -146,9 +146,13 @@
                                 <div class="col-12 col-md-6">
                                     <div class="form-group">
                                         <label class="label" for="cmbPatrimonio">Patrimônio *</label>
-                                        <ejs-combobox id="cmbPatrimonio" placeholder="Selecione o Patrimônio"
-                                            allowFiltering="true" filterType="Contains" popupHeight="200px" width="100%"
-                                            showClearButton="true">
+                                        <ejs-combobox id="cmbPatrimonio"
+                                                      placeholder="Selecione o Patrimônio"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      popupHeight="200px"
+                                                      width="100%"
+                                                      showClearButton="true">
                                             <e-combobox-fields text="text" value="value"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -157,8 +161,11 @@
                                 <div class="col-12 col-md-4">
                                     <div class="form-group">
                                         <label class="label" for="dataMov">Data da Movimentação *</label>
-                                        <ejs-datepicker id="dataMov" format="dd/MM/yyyy" floatLabelType="Never"
-                                            placeholder="dd/mm/aaaa" width="100%">
+                                        <ejs-datepicker id="dataMov"
+                                                        format="dd/MM/yyyy"
+                                                        floatLabelType="Never"
+                                                        placeholder="dd/mm/aaaa"
+                                                        width="100%">
                                         </ejs-datepicker>
                                     </div>
                                 </div>
@@ -166,11 +173,13 @@
                                 <div class="col-12 col-md-2">
                                     <div class="status-switch">
                                         <label for="StatusCheckbox" class="status-title me-2">Status:</label>
-                                        <ejs-checkbox id="StatusCheckbox" name="StatusCheckbox"
-                                            change="onEJ2CheckboxChange">
+                                        <ejs-checkbox id="StatusCheckbox"
+                                                      name="StatusCheckbox"
+                                                      change="onEJ2CheckboxChange">
                                         </ejs-checkbox>
-                                        <label id="StatusCheckboxLabel" class="status-label ms-2"
-                                            for="StatusCheckbox"></label>
+                                        <label id="StatusCheckboxLabel"
+                                               class="status-label ms-2"
+                                               for="StatusCheckbox"></label>
                                     </div>
                                 </div>
                             </div>
@@ -184,15 +193,21 @@
                                     <div class="col-12 col-md-6">
                                         <div class="form-group">
                                             <label class="label" for="SetorOrigem">Setor (Origem)</label>
-                                            <input id="SetorOrigem" type="text" class="form-control control-height"
-                                                disabled readonly />
+                                            <input id="SetorOrigem"
+                                                   type="text"
+                                                   class="form-control control-height"
+                                                   disabled
+                                                   readonly />
                                         </div>
                                     </div>
                                     <div class="col-12 col-md-6">
                                         <div class="form-group">
                                             <label class="label" for="SecaoOrigem">Seção (Origem)</label>
-                                            <input id="SecaoOrigem" type="text" class="form-control control-height"
-                                                disabled readonly />
+                                            <input id="SecaoOrigem"
+                                                   type="text"
+                                                   class="form-control control-height"
+                                                   disabled
+                                                   readonly />
                                         </div>
                                     </div>
                                 </div>
@@ -208,9 +223,12 @@
                                         <div class="form-group">
                                             <label class="label" for="cmbSetorDestino">Setor (Destino) *</label>
                                             <ejs-combobox id="cmbSetorDestino"
-                                                placeholder="Selecione o Setor de Destino" allowFiltering="true"
-                                                filterType="Contains" popupHeight="200px" width="100%"
-                                                showClearButton="true">
+                                                          placeholder="Selecione o Setor de Destino"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          popupHeight="200px"
+                                                          width="100%"
+                                                          showClearButton="true">
                                                 <e-combobox-fields text="text" value="value"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
@@ -220,9 +238,12 @@
                                         <div class="form-group">
                                             <label class="label" for="cmbSecoesDestino">Seção (Destino) *</label>
                                             <ejs-combobox id="cmbSecoesDestino"
-                                                placeholder="Primeiro selecione o Setor de Destino"
-                                                allowFiltering="true" filterType="Contains" popupHeight="200px"
-                                                width="100%" showClearButton="true">
+                                                          placeholder="Primeiro selecione o Setor de Destino"
+                                                          allowFiltering="true"
+                                                          filterType="Contains"
+                                                          popupHeight="200px"
+                                                          width="100%"
+                                                          showClearButton="true">
                                                 <e-combobox-fields text="text" value="value"></e-combobox-fields>
                                             </ejs-combobox>
                                         </div>
@@ -234,19 +255,20 @@
                                 <div class="col-12">
                                     <div class="row">
                                         <div class="col-8">
-                                            <button type="button" id="btnSalvar"
-                                                class="btn btn-azul btn-submit-spin form-control me-4"
-                                                style="max-width:300px; display:inline-block"
-                                                data-ejtip="Salvar a movimentação de patrimônio">
-                                                <i
-                                                    class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>&nbsp;&nbsp;
+                                            <button type="button"
+                                                    id="btnSalvar"
+                                                    class="btn btn-azul btn-submit-spin form-control me-4"
+                                                    style="max-width:300px; display:inline-block"
+                                                    data-ejtip="Salvar a movimentação de patrimônio">
+                                                <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>&nbsp;&nbsp;
                                                 <span id="textoBotaoSalvar">Criar Movimentação</span>
                                             </button>
 
-                                            <a asp-page="./Index" class="btn btn-ftx-fechar form-control"
-                                                style="max-width:300px; display:inline-block" data-ftx-loading>
-                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                                Cancelar Operação
+                                            <a asp-page="./Index"
+                                               class="btn btn-ftx-fechar form-control"
+                                               style="max-width:300px; display:inline-block"
+                                               data-ftx-loading>
+                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                                             </a>
                                         </div>
                                     </div>
@@ -262,24 +284,12 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="~/js/cadastros/movimentacaopatrimonio.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * MOVIMENTAÇÃO DE PATRIMÔNIO - SCRIPTS DE FORMULÁRIO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Gerencia criação e edição de movimentações de patrimônio,
-         * incluindo carregamento de dados em modo edição e salvamento via AJAX.
-         * @@requires jQuery, Bootstrap 5, Syncfusion EJ2 (ComboBox, DatePicker, CheckBox)
-         * @@file Upsert.cshtml
-         */
-
         $(document).ready(function () {
             try {
 
@@ -322,14 +332,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * CARREGAMENTO DE DADOS DA MOVIMENTAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Carrega os dados de uma movimentação existente para edição
-         * @@param {string} id - ID (GUID) da movimentação a ser carregada
-         * @@returns {void}
-         */
         function carregarDadosMovimentacao(id) {
             try {
                 $.ajax({
@@ -390,14 +392,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * SALVAR MOVIMENTAÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Coleta dados dos componentes EJ2, valida e envia via AJAX
-         * para criar ou atualizar a movimentação de patrimônio
-         * @@returns {void}
-         */
         function salvarMovimentacao() {
             try {
 
```
