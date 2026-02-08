# Pages/Unidade/LotacaoMotoristas.cshtml

**Mudanca:** GRANDE | **+146** linhas | **-151** linhas

---

```diff
--- JANEIRO: Pages/Unidade/LotacaoMotoristas.cshtml
+++ ATUAL: Pages/Unidade/LotacaoMotoristas.cshtml
@@ -8,15 +8,15 @@
 @inject IUnitOfWork _unitOfWork
 
 @{
-@functions {
-    public void OnGet()
-    {
-        FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
-        ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
-        ViewData["lstUnidade"] = new ListaUnidades(_unitOfWork).UnidadesList();
-        ViewData["lstMudança"] = new ListaMudancas(_unitOfWork).MudançasList();
+    @functions{
+        public void OnGet()
+        {
+            FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
+            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
+            ViewData["lstUnidade"] = new ListaUnidades(_unitOfWork).UnidadesList();
+            ViewData["lstMudança"] = new ListaMudancas(_unitOfWork).MudançasList();
+        }
     }
-}
 }
 
 @{
@@ -29,7 +29,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -40,12 +40,8 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>
         window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
@@ -77,18 +73,13 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
         }
 
         @@keyframes shimmer {
-            0% {
-                left: -100%;
-            }
-
-            100% {
-                left: 100%;
-            }
+            0% { left: -100%; }
+            100% { left: 100%; }
         }
 
         .ftx-card-header .titulo-paginas {
@@ -139,7 +130,7 @@
             background: #fff;
             border-radius: 8px;
             margin-bottom: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
             overflow: hidden;
         }
 
@@ -203,9 +194,7 @@
         }
 
         @@keyframes ftx-spin {
-            to {
-                transform: rotate(360deg);
-            }
+            to { transform: rotate(360deg); }
         }
 
         .ftx-loading-box p {
@@ -220,7 +209,7 @@
             background: #fff;
             border-radius: 8px;
             padding: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
         }
 
         #tblLotacao thead {
@@ -279,7 +268,7 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
         }
 
@@ -324,7 +313,7 @@
         }
 
         .ftx-modal .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 1rem 1.25rem;
             background: #f8f9fa;
             gap: 0.75rem;
@@ -434,22 +423,8 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * LOTAÇÃO DE MOTORISTAS - GESTÃO DE ALOCAÇÃO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Funções para gestão de lotação de motoristas em unidades,
-     * incluindo criação, edição e finalização de lotações.
-     * @@requires jQuery, DataTables, Syncfusion EJ2 ComboBox, Bootstrap Modal
-        * @@file LotacaoMotoristas.cshtml
-        */
-
     var defaultRTE;
 
-    /**
-     * Callback de inicialização do RichTextEditor
-     * @@description Armazena a instância do RTE na variável global
-        */
     function onCreate() {
         try {
             defaultRTE = this;
@@ -461,10 +436,6 @@
     var motoristaId = "";
     var dataTableLotacao = null;
 
-    /**
-     * Callback de mudança no ComboBox de motoristas
-     * @@description Carrega a tabela de lotações do motorista selecionado
-        */
     function onMotoristaChange() {
         try {
             var motoristas = document.getElementById('lstMotorista').ej2_instances[0];
@@ -501,8 +472,11 @@
                         Lotação de Motoristas
                     </h2>
                     <div class="ftx-card-actions">
-                        <button type="button" class="btn btn-fundo-laranja" id="btn-inicialotacao"
-                            data-bs-toggle="modal" data-bs-target="#modalIniciaLotacao">
+                        <button type="button"
+                                class="btn btn-fundo-laranja"
+                                id="btn-inicialotacao"
+                                data-bs-toggle="modal"
+                                data-bs-target="#modalIniciaLotacao">
                             <i class="fa-duotone fa-file-plus icon-pulse me-1"></i>
                             Iniciar Lotação
                         </button>
@@ -519,10 +493,15 @@
                         <div class="ftx-filter-body">
                             <div class="row">
                                 <div class="col-md-6">
-                                    <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["lstMotorista"]" popupHeight="250px" width="100%"
-                                        showClearButton="true" change="onMotoristaChange">
+                                    <ejs-combobox id="lstMotorista"
+                                                  placeholder="Selecione um Motorista"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["lstMotorista"]"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  showClearButton="true"
+                                                  change="onMotoristaChange">
                                         <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -539,7 +518,7 @@
                                     <th>Data Fim</th>
                                     <th>Lotado</th>
                                     <th>Motivo</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
@@ -557,15 +536,13 @@
 
 <div id="loadingOverlayMot" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text" id="txtLoadingMessage">Carregando...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
     </div>
 </div>
-<div class="modal fade ftx-modal" id="modalIniciaLotacao" tabindex="-1" aria-labelledby="modalIniciaLotacaoLabel"
-    aria-hidden="true">
+<div class="modal fade ftx-modal" id="modalIniciaLotacao" tabindex="-1" aria-labelledby="modalIniciaLotacaoLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header header-terracota">
@@ -573,8 +550,7 @@
                     <i class="fa-duotone fa-user-plus"></i>
                     Lotar Motorista em uma Unidade
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmLotacao">
@@ -585,9 +561,14 @@
                                     <i class="fa-duotone fa-user-tie"></i>
                                     Motorista
                                 </label>
-                                <ejs-combobox id="lstMotoristaLotacao" placeholder="Selecione um Motorista"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstMotoristaLotacao"
+                                              placeholder="Selecione um Motorista"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstMotorista"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -601,9 +582,14 @@
                                     <i class="fa-duotone fa-building"></i>
                                     Unidade
                                 </label>
-                                <ejs-combobox id="lstUnidadeLotacao" placeholder="Selecione uma Unidade"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstUnidadeLotacao"
+                                              placeholder="Selecione uma Unidade"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstUnidade"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="UnidadeId"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -617,9 +603,14 @@
                                     <i class="fa-duotone fa-clipboard-list"></i>
                                     Motivo da Lotação
                                 </label>
-                                <ejs-combobox id="lstMotivoLotacao" placeholder="Selecione um Motivo"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMudança"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstMotivoLotacao"
+                                              placeholder="Selecione um Motivo"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstMudança"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="MudancaId"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -671,8 +662,7 @@
     </div>
 </div>
 
-<div class="modal fade ftx-modal" id="modalEditaLotacao" tabindex="-1" aria-labelledby="modalEditaLotacaoLabel"
-    aria-hidden="true">
+<div class="modal fade ftx-modal" id="modalEditaLotacao" tabindex="-1" aria-labelledby="modalEditaLotacaoLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
@@ -680,8 +670,7 @@
                     <i class="fa-duotone fa-pen-to-square"></i>
                     Editar Lotação do Motorista
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmEditaLotacao">
@@ -694,9 +683,14 @@
                                     <i class="fa-duotone fa-user-tie"></i>
                                     Motorista
                                 </label>
-                                <ejs-combobox id="lstMotoristaEditaLotacao" placeholder="Selecione um Motorista"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstMotoristaEditaLotacao"
+                                              placeholder="Selecione um Motorista"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstMotorista"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -710,9 +704,14 @@
                                     <i class="fa-duotone fa-building"></i>
                                     Unidade
                                 </label>
-                                <ejs-combobox id="lstUnidadeEditaLotacao" placeholder="Selecione uma Unidade"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstUnidadeEditaLotacao"
+                                              placeholder="Selecione uma Unidade"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstUnidade"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="UnidadeId"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -726,9 +725,14 @@
                                     <i class="fa-duotone fa-clipboard-list"></i>
                                     Motivo
                                 </label>
-                                <ejs-combobox id="lstEditaMotivo" placeholder="Selecione um Motivo"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMudança"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstEditaMotivo"
+                                              placeholder="Selecione um Motivo"
+                                              allowFiltering="true"
+                                              filterType="Contains"
+                                              dataSource="@ViewData["lstMudança"]"
+                                              popupHeight="250px"
+                                              width="100%"
+                                              showClearButton="true">
                                     <e-combobox-fields text="Descricao" value="MudancaId"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -760,8 +764,7 @@
                                     <i class="fa-duotone fa-eraser"></i>
                                     Limpar Data Fim
                                 </label>
-                                <button type="button" class="btn fundo-vermelho btn-icon-28 d-block"
-                                    onclick="ApagaDataFim()">
+                                <button type="button" class="btn fundo-vermelho btn-icon-28 d-block" onclick="ApagaDataFim()">
                                     <i class="fa-duotone fa-trash-can"></i>
                                 </button>
                             </div>
@@ -792,8 +795,7 @@
     </div>
 </div>
 
-<div class="modal fade ftx-modal" id="modalFinalizaLotacao" tabindex="-1" aria-labelledby="modalFinalizaLotacaoLabel"
-    aria-hidden="true">
+<div class="modal fade ftx-modal" id="modalFinalizaLotacao" tabindex="-1" aria-labelledby="modalFinalizaLotacaoLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
@@ -801,8 +803,7 @@
                     <i class="fa-duotone fa-user-minus"></i>
                     Finalizar Lotação do Motorista
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <input type="hidden" id="txtIdFinaliza" />
@@ -814,9 +815,14 @@
                                 <i class="fa-duotone fa-user-tie"></i>
                                 Motorista
                             </label>
-                            <ejs-combobox id="lstMotoristaFinalizaLotacao" placeholder="Selecione um Motorista"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstMotoristaFinalizaLotacao"
+                                          placeholder="Selecione um Motorista"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@ViewData["lstMotorista"]"
+                                          popupHeight="250px"
+                                          width="100%"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -828,9 +834,14 @@
                                 <i class="fa-duotone fa-building"></i>
                                 Unidade Atual
                             </label>
-                            <ejs-combobox id="lstUnidadeLotacaoAtual" placeholder="Selecione uma Unidade"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstUnidadeLotacaoAtual"
+                                          placeholder="Selecione uma Unidade"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@ViewData["lstUnidade"]"
+                                          popupHeight="250px"
+                                          width="100%"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="UnidadeId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -883,9 +894,14 @@
                                 <i class="fa-duotone fa-building-circle-arrow-right"></i>
                                 Nova Lotação (se necessário)
                             </label>
-                            <ejs-combobox id="lstUnidadeNovaLotacao" placeholder="Selecione uma Unidade"
-                                allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                popupHeight="250px" width="100%" showClearButton="true">
+                            <ejs-combobox id="lstUnidadeNovaLotacao"
+                                          placeholder="Selecione uma Unidade"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@ViewData["lstUnidade"]"
+                                          popupHeight="250px"
+                                          width="100%"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="UnidadeId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -909,9 +925,13 @@
                                 Motorista de Cobertura das Férias
                             </label>
                             <ejs-combobox id="lstUnidadeMotoristaCobertura"
-                                placeholder="Selecione o Motorista da Cobertura" allowFiltering="true"
-                                filterType="Contains" dataSource="@ViewData["lstMotorista"]" popupHeight="450px"
-                                width="100%" showClearButton="true">
+                                          placeholder="Selecione o Motorista da Cobertura"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@ViewData["lstMotorista"]"
+                                          popupHeight="450px"
+                                          width="100%"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -952,18 +972,7 @@
 
 @section ScriptsBlock {
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE LOADING E LISTAGEM DE LOTAÇÕES
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Controle de overlay de loading e carregamento de DataTable
-            * com histórico de lotações do motorista.
-             */
-
-            /**
-             * Exibe o overlay de loading padrão FrotiX
-             * @@param { string }[mensagem = 'Carregando...'] - Texto exibido durante o loading
-            */
+
         function mostrarLoading(mensagem) {
             try {
                 $('#txtLoadingMessage').text(mensagem || 'Carregando...');
@@ -976,9 +985,6 @@
             }
         }
 
-        /**
-         * Oculta o overlay de loading padrão FrotiX
-         */
         function esconderLoading() {
             try {
                 var overlay = document.getElementById('loadingOverlayMot');
@@ -990,11 +996,6 @@
             }
         }
 
-            /**
-             * Carrega a tabela de lotações do motorista selecionado
-             * @@param { string } motoristaId - ID do motorista para buscar lotações
-            * @@description Inicializa DataTable com dados via AJAX server - side
-                */
         function ListaTblLotacoes(motoristaId) {
             try {
 
@@ -1042,7 +1043,7 @@
                         type: "GET",
                         data: { motoristaId: motoristaId },
                         datatype: "json",
-                        dataSrc: function (json) {
+                        dataSrc: function(json) {
                             esconderLoading();
 
                             if (Array.isArray(json)) {
@@ -1053,7 +1054,7 @@
                             }
                             return [];
                         },
-                        error: function (xhr, error, thrown) {
+                        error: function(xhr, error, thrown) {
                             esconderLoading();
                             console.error("Erro ao carregar lotações:", error, thrown);
                         }
@@ -1068,12 +1069,12 @@
                                 try {
                                     if (data) {
                                         return `<a href="javascript:void(0)" class="btn btn-verde btn-xs" data-ejtip="Motorista lotado">
-                                                        <i class="fa-duotone fa-circle-check"></i> Lotado
-                                                    </a>`;
+                                                    <i class="fa-duotone fa-circle-check"></i> Lotado
+                                                </a>`;
                                     }
                                     return `<a href="javascript:void(0)" class="btn fundo-cinza btn-xs" data-ejtip="Motorista removido">
-                                                    <i class="fa-duotone fa-circle-xmark"></i> Removido
-                                                </a>`;
+                                                <i class="fa-duotone fa-circle-xmark"></i> Removido
+                                            </a>`;
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml", "lotado.render", error);
                                     return "";
@@ -1096,25 +1097,25 @@
                             render: function (data, type, full) {
                                 try {
                                     return `<div class="ftx-btn-acoes">
-                                                    <a href="javascript:void(0)"
-                                                       class="btn btn-editar btn-icon-28 btn-editalotacao"
-                                                       data-ejtip="Editar Lotação"
-                                                       data-id="${data}">
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </a>
-                                                    <a href="javascript:void(0)"
-                                                       class="btn btn-fundo-laranja btn-icon-28 btn-finalizalotacao"
-                                                       data-ejtip="Finalizar Lotação"
-                                                       data-id="${data}">
-                                                        <i class="fa-duotone fa-right-from-bracket"></i>
-                                                    </a>
-                                                    <a href="javascript:void(0)"
-                                                       class="btn fundo-vermelho btn-icon-28 btn-apagar"
-                                                       data-ejtip="Excluir Lotação"
-                                                       data-id="${data}">
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </a>
-                                                </div>`;
+                                                <a href="javascript:void(0)"
+                                                   class="btn btn-editar btn-icon-28 btn-editalotacao"
+                                                   data-ejtip="Editar Lotação"
+                                                   data-id="${data}">
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a href="javascript:void(0)"
+                                                   class="btn btn-fundo-laranja btn-icon-28 btn-finalizalotacao"
+                                                   data-ejtip="Finalizar Lotação"
+                                                   data-id="${data}">
+                                                    <i class="fa-duotone fa-right-from-bracket"></i>
+                                                </a>
+                                                <a href="javascript:void(0)"
+                                                   class="btn fundo-vermelho btn-icon-28 btn-apagar"
+                                                   data-ejtip="Excluir Lotação"
+                                                   data-id="${data}">
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>`;
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("LotacaoMotoristas.cshtml", "acoes.render", error);
                                     return "";
@@ -1139,9 +1140,6 @@
     </script>
 
     <script>
-        /**
-         * Limpa o campo de data fim na edição de lotação
-         */
         function ApagaDataFim() {
             try {
                 $('#txtDataFimEdicao').val('');
@@ -1305,10 +1303,6 @@
             }
         });
 
-            /**
-             * Finaliza a lotação atual e opcionalmente cria nova lotação
-             * @@description Encerra lotação com data fim e pode criar nova lotação ou cobertura
-            */
         function FinalizaLotacao() {
             try {
                 var lotacaoMotoristaId = $('#txtIdFinaliza').val();
@@ -1383,7 +1377,7 @@
             }
         });
 
-        $(document).on('click', '.btn-editalotacao', function (e) {
+        $(document).on('click', '.btn-editalotacao', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
@@ -1396,7 +1390,7 @@
             }
         });
 
-        $(document).on('click', '.btn-finalizalotacao', function (e) {
+        $(document).on('click', '.btn-finalizalotacao', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
```
