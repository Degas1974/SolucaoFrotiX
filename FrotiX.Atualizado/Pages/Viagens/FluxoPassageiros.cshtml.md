# Pages/Viagens/FluxoPassageiros.cshtml

**Mudanca:** GRANDE | **+52** linhas | **-76** linhas

---

```diff
--- JANEIRO: Pages/Viagens/FluxoPassageiros.cshtml
+++ ATUAL: Pages/Viagens/FluxoPassageiros.cshtml
@@ -47,7 +47,7 @@
         left: -100%;
         width: 100%;
         height: 100%;
-        background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.12), transparent);
+        background: linear-gradient(90deg, transparent, rgba(255,255,255,0.12), transparent);
         animation: ftxHeaderShine 4s ease-in-out infinite;
         pointer-events: none;
     }
@@ -138,11 +138,9 @@
         0% {
             transform: scale(1);
         }
-
         50% {
             transform: scale(1.15);
         }
-
         100% {
             transform: scale(1);
         }
@@ -196,16 +194,14 @@
        BOTÃO INSERIR LINHA - 50% MAIOR
        ================================================================ */
     .btn-insere-linha {
-        width: 51px !important;
-        /* 34px * 1.5 = 51px */
+        width: 51px !important; /* 34px * 1.5 = 51px */
         height: 51px !important;
         padding: 0 !important;
         display: flex !important;
         align-items: center !important;
         justify-content: center !important;
         border-radius: 6px !important;
-        margin-bottom: -8px;
-        /* Ajuste para alinhar com input de 34px */
+        margin-bottom: -8px; /* Ajuste para alinhar com input de 34px */
     }
 
     .btn-insere-linha i {
@@ -222,20 +218,7 @@
 </style>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * FLUXO DE PASSAGEIROS - SELEÇÃO DE VEÍCULOS E DATAS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Funções de controle para seleção de veículo e data no registro
-        * de viagens do Economildo(controle de passageiros).
-     * @@requires Syncfusion EJ2 ComboBox
-        * @@file FluxoPassageiros.cshtml
-        */
-
-    /**
-     * Obtém a instância do ComboBox Syncfusion de veículos
-     * @@returns { Object | undefined } Instância ej2 do ComboBox de veículos
-        */
+
     function DefineEscolhaVeiculo() {
         try {
             var veiculos = document.getElementById('lstVeiculos').ej2_instances?.[0];
@@ -245,10 +228,6 @@
         }
     }
 
-    /**
-     * Handler reservado para regras de escolha de data
-     * @@description Placeholder para futuras validações / regras de negócio
-        */
     function DefineEscolhaData() {
         try {
 
@@ -257,10 +236,6 @@
         }
     }
 
-    /**
-     * Callback de ValueChange do ComboBox de veículos
-     * @@description Executado quando o usuário seleciona um veículo
-        */
     function VeiculosValueChange() {
         try {
 
@@ -350,20 +325,30 @@
 
                                 <div class="col-4">
                                     <label class="label font-weight-bold color-black">Placa do Economildo</label>
-                                    <ejs-combobox id="lstVeiculos" placeholder="Selecione um Veículo"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["lstVeiculos"]" popupHeight="250px" width="100%"
-                                        cssClass="e-small" showClearButton="true">
+                                    <ejs-combobox id="lstVeiculos"
+                                                  placeholder="Selecione um Veículo"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["lstVeiculos"]"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  cssClass="e-small"
+                                                  showClearButton="true">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
 
                                 <div class="col-4">
                                     <label class="label font-weight-bold color-black">Motorista do Economildo</label>
-                                    <ejs-combobox id="lstMotoristas" placeholder="Selecione um Motorista"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["lstMotoristas"]" popupHeight="250px" width="100%"
-                                        cssClass="e-small" showClearButton="true">
+                                    <ejs-combobox id="lstMotoristas"
+                                                  placeholder="Selecione um Motorista"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["lstMotoristas"]"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  cssClass="e-small"
+                                                  showClearButton="true">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -391,8 +376,7 @@
                                 </div>
 
                                 <div class="col-4">
-                                    <button id="btnInsereFicha" type="button"
-                                        class="btn btn-azul form-control form-control-xs">
+                                    <button id="btnInsereFicha" type="button" class="btn btn-azul form-control form-control-xs">
                                         <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> Insere Ficha
                                     </button>
                                 </div>
@@ -425,14 +409,12 @@
 
                                             <div class="col-2 col-sm-2">
                                                 <label class="label font-weight-bold color-black">Hora Início</label>
-                                                <input id="txtHoraInicio" class="form-control form-control-xs"
-                                                    type="time" />
+                                                <input id="txtHoraInicio" class="form-control form-control-xs" type="time" />
                                             </div>
 
                                             <div class="col-2 col-sm-2">
                                                 <label class="label font-weight-bold color-black">Hora Fim</label>
-                                                <input id="txtHoraFim" class="form-control form-control-xs"
-                                                    type="time" />
+                                                <input id="txtHoraFim" class="form-control form-control-xs" type="time" />
                                             </div>
 
                                             <div class="col-2 col-sm-2">
@@ -442,8 +424,10 @@
 
                                             <div class="col-1 d-flex align-items-end">
                                                 <a class="btn btn-azul text-white btnFicha btn-insere-linha"
-                                                    id="btnInsere" href="#" style="cursor: pointer;"
-                                                    data-ejtip="Insere a Linha na Tabela">
+                                                   id="btnInsere"
+                                                   href="#"
+                                                   style="cursor: pointer;"
+                                                   data-ejtip="Insere a Linha na Tabela">
                                                     <i class="fa-duotone fa-circle-plus"></i>
                                                 </a>
                                             </div>
@@ -459,22 +443,19 @@
                                     <div class="box-body">
                                         <h3 id="TituloIdas">Viagens de Ida</h3>
                                         @{
-                                            List<object> colsida = new List<object> { new { field = "horainicioida" ,
-                                                                                direction = "Ascending" } };
+                                            List<object> colsida = new List<object> { new { field = "horainicioida" , direction = "Ascending" } };
                                         }
-                                        <ejs-grid id="grdIda" toolbar="@(new List<string>() { "Delete" })"
-                                            GridLines="Both" allowSorting="true">
-                                            <e-grid-editSettings allowAdding="true" allowDeleting="true"
-                                                allowEditing="true" newRowPosition="Bottom"
-                                                showDeleteConfirmDialog="true"></e-grid-editSettings>
+                                        <ejs-grid id="grdIda" toolbar="@(new List<string>() { "Delete" })" GridLines="Both" allowSorting="true">
+                                            <e-grid-editSettings allowAdding="true"
+                                                                 allowDeleting="true"
+                                                                 allowEditing="true"
+                                                                 newRowPosition="Bottom"
+                                                                 showDeleteConfirmDialog="true"></e-grid-editSettings>
                                             <e-grid-sortsettings columns="colsida"></e-grid-sortsettings>
                                             <e-grid-columns>
-                                                <e-grid-column field="horainicioida" headerText="Hora"
-                                                    textAlign="Center" width="30" allowEditing="true"></e-grid-column>
-                                                <e-grid-column field="horafimida" headerText="Hora" textAlign="Center"
-                                                    width="30" allowEditing="true"></e-grid-column>
-                                                <e-grid-column field="qtdpassageirosida" headerText="Quantidade"
-                                                    textAlign="Center" width="20" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="horainicioida" headerText="Hora" textAlign="Center" width="30" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="horafimida" headerText="Hora" textAlign="Center" width="30" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="qtdpassageirosida" headerText="Quantidade" textAlign="Center" width="20" allowEditing="true"></e-grid-column>
                                             </e-grid-columns>
                                         </ejs-grid>
                                     </div>
@@ -486,22 +467,19 @@
                                     <div class="box-body">
                                         <h3 id="TituloVoltas">Viagens de Volta</h3>
                                         @{
-                                            List<object> colsvolta = new List<object> { new { field = "horainiciovolta" ,
-                                                                                direction = "Ascending" } };
+                                            List<object> colsvolta = new List<object> { new { field = "horainiciovolta" , direction = "Ascending" } };
                                         }
-                                        <ejs-grid id="grdVolta" toolbar="@(new List<string>() { "Delete" })"
-                                            GridLines="Both" allowSorting="true">
-                                            <e-grid-editSettings allowAdding="true" allowDeleting="true"
-                                                allowEditing="true" newRowPosition="Bottom"
-                                                showDeleteConfirmDialog="true"></e-grid-editSettings>
+                                        <ejs-grid id="grdVolta" toolbar="@(new List<string>() { "Delete" })" GridLines="Both" allowSorting="true">
+                                            <e-grid-editSettings allowAdding="true"
+                                                                 allowDeleting="true"
+                                                                 allowEditing="true"
+                                                                 newRowPosition="Bottom"
+                                                                 showDeleteConfirmDialog="true"></e-grid-editSettings>
                                             <e-grid-sortsettings columns="colsvolta"></e-grid-sortsettings>
                                             <e-grid-columns>
-                                                <e-grid-column field="horainiciovolta" headerText="Hora"
-                                                    textAlign="Center" width="30" allowEditing="true"></e-grid-column>
-                                                <e-grid-column field="horafimvolta" headerText="Hora" textAlign="Center"
-                                                    width="30" allowEditing="true"></e-grid-column>
-                                                <e-grid-column field="qtdpassageirosvolta" headerText="Quantidade"
-                                                    textAlign="Center" width="20" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="horainiciovolta" headerText="Hora" textAlign="Center" width="30" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="horafimvolta" headerText="Hora" textAlign="Center" width="30" allowEditing="true"></e-grid-column>
+                                                <e-grid-column field="qtdpassageirosvolta" headerText="Quantidade" textAlign="Center" width="20" allowEditing="true"></e-grid-column>
                                             </e-grid-columns>
                                         </ejs-grid>
                                     </div>
@@ -515,14 +493,12 @@
                                 <div class="row">
                                     <div class="col-6">
                                         <button id="btnSubmite" type="button" class="btn btn-azul form-control">
-                                            <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> Registrar
-                                            Viagens
+                                            <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> Registrar Viagens
                                         </button>
                                     </div>
                                     <div class="col-6">
                                         <button id="btnCancela" type="button" class="btn btn-vinho form-control">
-                                            <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i>
-                                            Cancela Inserção
+                                            <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i> Cancela Inserção
                                         </button>
                                     </div>
                                 </div>
@@ -545,10 +521,8 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script src="/js/cadastros/fluxopassageiros.js" asp-append-version="true"></script>
 }
```
