# Pages/Unidade/VisualizaLotacoes.cshtml

**Mudanca:** GRANDE | **+94** linhas | **-115** linhas

---

```diff
--- JANEIRO: Pages/Unidade/VisualizaLotacoes.cshtml
+++ ATUAL: Pages/Unidade/VisualizaLotacoes.cshtml
@@ -8,15 +8,15 @@
 @inject IUnitOfWork _unitOfWork
 
 @{
-@functions {
-    public void OnGet()
-    {
-        FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
-        ViewData["lstCategoria"] = new ListaCategoria(_unitOfWork).CategoriasList();
-        ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
-        ViewData["lstUnidade"] = new ListaUnidades(_unitOfWork).UnidadesList();
+    @functions {
+        public void OnGet()
+        {
+            FrotiX.Pages.Multa.ListaMultaModel.Initialize(_unitOfWork);
+            ViewData["lstCategoria"] = new ListaCategoria(_unitOfWork).CategoriasList();
+            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
+            ViewData["lstUnidade"] = new ListaUnidades(_unitOfWork).UnidadesList();
+        }
     }
-}
 }
 
 @{
@@ -53,18 +53,13 @@
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
@@ -98,7 +93,7 @@
             background: #fff;
             border-radius: 8px;
             margin-bottom: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
             overflow: hidden;
         }
 
@@ -162,9 +157,7 @@
         }
 
         @@keyframes ftx-spin {
-            to {
-                transform: rotate(360deg);
-            }
+            to { transform: rotate(360deg); }
         }
 
         .ftx-loading-box p {
@@ -179,7 +172,7 @@
             background: #fff;
             border-radius: 8px;
             padding: 1rem;
-            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.06);
+            box-shadow: 0 1px 3px rgba(0,0,0,0.06);
         }
 
         #tblLotacao thead {
@@ -238,7 +231,7 @@
             left: -100%;
             width: 100%;
             height: 100%;
-            background: linear-gradient(90deg, transparent, rgba(255, 255, 255, 0.1), transparent);
+            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.1), transparent);
             animation: shimmer 3s infinite;
         }
 
@@ -279,7 +272,7 @@
         }
 
         .ftx-modal .modal-footer {
-            border-top: 1px solid rgba(0, 0, 0, 0.08);
+            border-top: 1px solid rgba(0,0,0,0.08);
             padding: 1rem 1.25rem;
             background: #f8f9fa;
             gap: 0.75rem;
@@ -385,23 +378,9 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * VISUALIZAÇÃO DE LOTAÇÕES POR CATEGORIA
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Funções para visualização de lotações agrupadas por categoria,
-     * com filtro e exibição em DataTable.
-     * @@requires jQuery, DataTables, Syncfusion EJ2 ComboBox, Bootstrap Modal
-        * @@file VisualizaLotacoes.cshtml
-        */
-
     let categoriaId = null;
     var dataTableLotacao = null;
 
-    /**
-     * Callback de inicialização do RichTextEditor
-     * @@description Placeholder para configurações iniciais do RTE
-        */
     function onCreate() {
         try {
 
@@ -410,10 +389,6 @@
         }
     }
 
-    /**
-     * Callback de mudança no ComboBox de categorias
-     * @@description Carrega a tabela de lotações da categoria selecionada
-        */
     function onCategoriaChange() {
         try {
             const categorias = document.getElementById('lstCategorias').ej2_instances[0];
@@ -461,10 +436,15 @@
                         <div class="ftx-filter-body">
                             <div class="row">
                                 <div class="col-md-6">
-                                    <ejs-combobox id="lstCategorias" placeholder="Selecione uma Categoria"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["lstCategoria"]" popupHeight="250px" width="100%"
-                                        showClearButton="true" change="onCategoriaChange">
+                                    <ejs-combobox id="lstCategorias"
+                                                  placeholder="Selecione uma Categoria"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["lstCategoria"]"
+                                                  popupHeight="250px"
+                                                  width="100%"
+                                                  showClearButton="true"
+                                                  change="onCategoriaChange">
                                         <e-combobox-fields text="Categoria" value="CategoriaId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -480,7 +460,7 @@
                                     <th>Nome da Unidade</th>
                                     <th>Motorista</th>
                                     <th>Data Início</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                     <th></th>
                                     <th></th>
                                     <th></th>
@@ -499,16 +479,14 @@
 
 <div id="loadingOverlayLotacoes" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text" id="txtLoadingMessage">Carregando...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
     </div>
 </div>
 
-<div class="modal fade ftx-modal" id="modalEditaLotacao" tabindex="-1" aria-labelledby="modalEditaLotacaoLabel"
-    aria-hidden="true">
+<div class="modal fade ftx-modal" id="modalEditaLotacao" tabindex="-1" aria-labelledby="modalEditaLotacaoLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
@@ -516,8 +494,7 @@
                     <i class="fa-duotone fa-pen-to-square"></i>
                     Editar Lotação do Motorista
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmEditaLotacao">
@@ -530,9 +507,14 @@
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
@@ -546,9 +528,14 @@
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
@@ -600,8 +587,7 @@
     </div>
 </div>
 
-<div class="modal fade ftx-modal" id="modalFinalizaLotacao" tabindex="-1" aria-labelledby="modalFinalizaLotacaoLabel"
-    aria-hidden="true">
+<div class="modal fade ftx-modal" id="modalFinalizaLotacao" tabindex="-1" aria-labelledby="modalFinalizaLotacaoLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg modal-dialog-centered">
         <div class="modal-content">
             <div class="modal-header">
@@ -609,8 +595,7 @@
                     <i class="fa-duotone fa-user-minus"></i>
                     Finalizar Lotação do Motorista
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
             <div class="modal-body">
                 <form id="frmFinalizaLotacao">
@@ -623,9 +608,14 @@
                                     <i class="fa-duotone fa-user-tie"></i>
                                     Motorista
                                 </label>
-                                <ejs-combobox id="lstMotoristaFinalizaLotacao" placeholder="Selecione um Motorista"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstMotorista"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstMotoristaFinalizaLotacao"
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
@@ -636,9 +626,14 @@
                                     <i class="fa-duotone fa-building"></i>
                                     Unidade Atual
                                 </label>
-                                <ejs-combobox id="lstUnidadeFinalizaLotacao" placeholder="Selecione uma Unidade"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstUnidadeFinalizaLotacao"
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
@@ -675,9 +670,14 @@
                                     <i class="fa-duotone fa-building-circle-arrow-right"></i>
                                     Nova Lotação (se necessário)
                                 </label>
-                                <ejs-combobox id="lstUnidadeNovaLotacao" placeholder="Selecione uma Unidade"
-                                    allowFiltering="true" filterType="Contains" dataSource="@ViewData["lstUnidade"]"
-                                    popupHeight="250px" width="100%" showClearButton="true">
+                                <ejs-combobox id="lstUnidadeNovaLotacao"
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
@@ -710,18 +710,7 @@
 
 @section ScriptsBlock {
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES DE LOADING E LISTAGEM DE LOTAÇÕES POR CATEGORIA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Controle de overlay de loading e carregamento de DataTable
-        * com lotações agrupadas por categoria.
-         */
-
-        /**
-         * Exibe o overlay de loading padrão FrotiX
-         * @@param { string } [mensagem = 'Carregando...'] - Texto exibido durante o loading
-        */
+
         function mostrarLoading(mensagem) {
             try {
                 $('#txtLoadingMessage').text(mensagem || 'Carregando...');
@@ -734,9 +723,6 @@
             }
         }
 
-        /**
-         * Oculta o overlay de loading padrão FrotiX
-         */
         function esconderLoading() {
             try {
                 var overlay = document.getElementById('loadingOverlayLotacoes');
@@ -748,11 +734,6 @@
             }
         }
 
-        /**
-         * Carrega a tabela de lotações da categoria selecionada
-         * @@param { string } categoriaIdParam - ID da categoria para buscar lotações
-        * @@description Inicializa DataTable com dados via AJAX
-            */
         function ListaTblLotacoes(categoriaIdParam) {
             try {
 
@@ -796,7 +777,7 @@
                         type: "GET",
                         data: { categoriaId: categoriaIdParam },
                         datatype: "json",
-                        dataSrc: function (json) {
+                        dataSrc: function(json) {
                             esconderLoading();
                             if (Array.isArray(json)) {
                                 return json;
@@ -806,7 +787,7 @@
                             }
                             return [];
                         },
-                        error: function (xhr, error, thrown) {
+                        error: function(xhr, error, thrown) {
                             esconderLoading();
                             console.error("Erro ao carregar lotações:", error, thrown);
                         }
@@ -821,25 +802,25 @@
                             render: function (data, type, full) {
                                 try {
                                     return `<div class="ftx-btn-acoes">
-                                                        <a href="javascript:void(0)"
-                                                           class="btn btn-editar btn-icon-28 btn-editalotacao"
-                                                           data-ejtip="Editar Lotação"
-                                                           data-id="${data}">
-                                                            <i class="fa-duotone fa-pen-to-square"></i>
-                                                        </a>
-                                                        <a href="javascript:void(0)"
-                                                           class="btn btn-fundo-laranja btn-icon-28 btn-finalizalotacao"
-                                                           data-ejtip="Finalizar Lotação"
-                                                           data-id="${data}">
-                                                            <i class="fa-duotone fa-right-from-bracket"></i>
-                                                        </a>
-                                                        <a href="javascript:void(0)"
-                                                           class="btn fundo-vermelho btn-icon-28 btn-apagar"
-                                                           data-ejtip="Excluir Lotação"
-                                                           data-id="${data}">
-                                                            <i class="fa-duotone fa-trash-can"></i>
-                                                        </a>
-                                                    </div>`;
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
                                     Alerta.TratamentoErroComLinha("VisualizaLotacoes.cshtml", "acoes.render", error);
                                     return "";
@@ -885,7 +866,7 @@
             }
         });
 
-        $(document).on('click', '.btn-editalotacao', function (e) {
+        $(document).on('click', '.btn-editalotacao', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
@@ -898,7 +879,7 @@
             }
         });
 
-        $(document).on('click', '.btn-finalizalotacao', function (e) {
+        $(document).on('click', '.btn-finalizalotacao', function(e) {
             try {
                 e.preventDefault();
                 e.stopPropagation();
@@ -1123,10 +1104,6 @@
                 const unidadeId = data['unidadeId'];
                 const lotacaoMotoristaId = data['lotacaoMotoristaId'];
 
-                /**
-                 * Alterna o estado disabled de um elemento e seus filhos recursivamente
-                 * @@param {HTMLElement} el - Elemento a ser processado
-                 */
                 function toggleDisabled(el) {
                     try { el.disabled = !el.disabled; } catch { }
                     if (el.childNodes && el.childNodes.length > 0) {
```
