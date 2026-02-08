# Pages/Viagens/Index.cshtml

**Mudanca:** GRANDE | **+342** linhas | **-495** linhas

---

```diff
--- JANEIRO: Pages/Viagens/Index.cshtml
+++ ATUAL: Pages/Viagens/Index.cshtml
@@ -10,33 +10,19 @@
 @functions {
     public void OnGet()
     {
-        try
-        {
-            FrotiX.Pages.Viagens.IndexModel.Initialize(_unitOfWork);
-            ViewData["dataCombustivel"] = new ListaNivelCombustivel(_unitOfWork).NivelCombustivelList();
-            ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
-            ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
-            ViewData["lstSetor"] = new ListaSetores(_unitOfWork).SetoresList();
-            ViewData["dataSetor"] = new ListaSetores(_unitOfWork).SetoresList();
-            ViewData["lstStatus"] = new ListaStatus(_unitOfWork).StatusList();
-            ViewData["lstEventos"] = new ListaEvento(_unitOfWork).EventosList();
-        }
-        catch (Exception ex)
-        {
-            Alerta.TratamentoErroComLinha("Pages/Viagens/Index.cshtml", "OnGet", ex);
-        }
+        FrotiX.Pages.Viagens.IndexModel.Initialize(_unitOfWork);
+        ViewData["dataCombustivel"] = new ListaNivelCombustivel(_unitOfWork).NivelCombustivelList();
+        ViewData["lstMotorista"] = new ListaMotorista(_unitOfWork).MotoristaList();
+        ViewData["lstVeiculos"] = new ListaVeiculos(_unitOfWork).VeiculosList();
+        ViewData["lstSetor"] = new ListaSetores(_unitOfWork).SetoresList();
+        ViewData["dataSetor"] = new ListaSetores(_unitOfWork).SetoresList();
+        ViewData["lstStatus"] = new ListaStatus(_unitOfWork).StatusList();
+        ViewData["lstEventos"] = new ListaEvento(_unitOfWork).EventosList();
     }
 
     public void OnPost()
     {
-        try
-        {
-
-        }
-        catch (Exception ex)
-        {
-            Alerta.TratamentoErroComLinha("Pages/Viagens/Index.cshtml", "OnPost", ex);
-        }
+
     }
 }
 
@@ -64,11 +50,9 @@
         0% {
             background-position: 0% 50%;
         }
-
         50% {
             background-position: 100% 50%;
         }
-
         100% {
             background-position: 0% 50%;
         }
@@ -112,9 +96,7 @@
     }
 
     /* Tipografia e alinhamento do grid */
-    #tblViagem,
-    #tblViagem th,
-    #tblViagem td {
+    #tblViagem, #tblViagem th, #tblViagem td {
         font-size: .82rem;
     }
 
@@ -123,8 +105,7 @@
         font-weight: 700;
     }
 
-    #tblViagem th,
-    #tblViagem td {
+    #tblViagem th, #tblViagem td {
         vertical-align: middle !important;
     }
 
@@ -148,7 +129,7 @@
     }
 
     #tblViagem td:last-child .text-center,
-    #tblViagem td:last-child>div {
+    #tblViagem td:last-child > div {
         display: flex;
         justify-content: center;
         align-items: center;
@@ -174,12 +155,12 @@
         text-transform: uppercase;
         letter-spacing: 0.3px;
         transition: all 0.2s ease;
-        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.15) !important;
+        box-shadow: 0 2px 4px rgba(0,0,0,0.15) !important;
     }
 
     .ftx-badge-status:hover {
         transform: translateY(-1px);
-        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2) !important;
+        box-shadow: 0 4px 8px rgba(0,0,0,0.2) !important;
     }
 
     .ftx-badge-status i {
@@ -288,97 +269,39 @@
         background-color: #4B0082 !important;
         border: none !important;
         color: #fff !important;
-        box-shadow: 0 0 8px rgba(75, 0, 130, .5), 0 2px 4px rgba(75, 0, 130, .3) !important;
+        box-shadow: 0 0 8px rgba(75,0,130,.5), 0 2px 4px rgba(75,0,130,.3) !important;
         transition: all 0.3s ease !important;
     }
-
     #tblViagem .btn-imprimir:hover {
         background-color: #3A006B !important;
         animation: buttonWiggle 0.5s ease-in-out !important;
-        box-shadow: 0 0 25px rgba(75, 0, 130, .8), 0 6px 15px rgba(75, 0, 130, .5) !important;
+        box-shadow: 0 0 25px rgba(75,0,130,.8), 0 6px 15px rgba(75,0,130,.5) !important;
     }
 
     #tblViagem .btn-finalizar-viagem {
         background-color: #2F4F4F !important;
         border: none !important;
         color: #fff !important;
-        box-shadow: 0 0 8px rgba(47, 79, 79, .5), 0 2px 4px rgba(47, 79, 79, .3) !important;
+        box-shadow: 0 0 8px rgba(47,79,79,.5), 0 2px 4px rgba(47,79,79,.3) !important;
         transition: all 0.3s ease !important;
     }
-
     #tblViagem .btn-finalizar-viagem:hover {
         background-color: #253A3A !important;
         animation: buttonWiggle 0.5s ease-in-out !important;
-        box-shadow: 0 0 25px rgba(47, 79, 79, .8), 0 6px 15px rgba(47, 79, 79, .5) !important;
+        box-shadow: 0 0 25px rgba(47,79,79,.8), 0 6px 15px rgba(47,79,79,.5) !important;
     }
 
     #tblViagem .btn-cancelar-viagem {
         background-color: #722F37 !important;
         border: none !important;
         color: #fff !important;
-        box-shadow: 0 0 8px rgba(114, 47, 55, .5), 0 2px 4px rgba(114, 47, 55, .3) !important;
+        box-shadow: 0 0 8px rgba(114,47,55,.5), 0 2px 4px rgba(114,47,55,.3) !important;
         transition: all 0.3s ease !important;
     }
-
     #tblViagem .btn-cancelar-viagem:hover {
         background-color: #5a252c !important;
         animation: buttonWiggle 0.5s ease-in-out !important;
-        box-shadow: 0 0 25px rgba(114, 47, 55, .8), 0 6px 15px rgba(114, 47, 55, .5) !important;
-    }
-
-    /* ===== BOTÃO FICHA DE VISTORIA (VERDE) ===== */
-    #tblViagem .btn-foto {
-        background-color: #228B22 !important;
-        border: none !important;
-        color: #fff !important;
-        box-shadow: 0 0 8px rgba(34, 139, 34, .5), 0 2px 4px rgba(34, 139, 34, .3) !important;
-        transition: all 0.3s ease !important;
-    }
-
-    #tblViagem .btn-foto:hover {
-        background-color: #1c7a1c !important;
-        animation: buttonWiggle 0.5s ease-in-out !important;
-        box-shadow: 0 0 25px rgba(34, 139, 34, .8), 0 6px 15px rgba(34, 139, 34, .5) !important;
-    }
-
-    #tblViagem .btn-foto.disabled {
-        background-color: #6c757d !important;
-        box-shadow: none !important;
-        opacity: 0.6 !important;
-    }
-
-    /* ===== ESTILOS DO MODAL FINALIZAR VIAGEM ===== */
-    #modalFinalizaViagem .card {
-        border: none;
-        border-radius: 8px;
-    }
-
-    #modalFinalizaViagem .card-header {
-        border-bottom: 1px solid rgba(0, 0, 0, 0.08);
-    }
-
-    #modalFinalizaViagem .ftx-item-card {
-        background: #fff;
-        border-radius: 6px;
-        padding: 0.75rem;
-        border: 1px solid #e9ecef;
-        text-align: center;
-        transition: all 0.2s ease;
-    }
-
-    #modalFinalizaViagem .ftx-item-card:hover {
-        border-color: #17a2b8;
-        box-shadow: 0 2px 8px rgba(23, 162, 184, 0.15);
-    }
-
-    #modalFinalizaViagem .ftx-switch-lg {
-        width: 3rem !important;
-        height: 1.5rem !important;
-    }
-
-    #modalFinalizaViagem .ftx-switch-label {
-        font-size: 0.75rem;
-        color: #6c757d;
+        box-shadow: 0 0 25px rgba(114,47,55,.8), 0 6px 15px rgba(114,47,55,.5) !important;
     }
 
     /* ===== BOTÃO ADICIONAR OCORRÊNCIA (LARANJA) ===== */
@@ -390,10 +313,9 @@
         background-color: #A0522D !important;
         border-color: #8B4513 !important;
         color: #fff !important;
-        box-shadow: 0 0 8px rgba(160, 82, 45, .5), 0 2px 4px rgba(160, 82, 45, .3) !important;
+        box-shadow: 0 0 8px rgba(160,82,45,.5), 0 2px 4px rgba(160,82,45,.3) !important;
         transition: all 0.3s ease !important;
     }
-
     #modalFinalizaViagem .btn-outline-secondary:hover,
     #modalFinalizaViagem [id*="btnAdd"]:hover,
     #modalFinalizaViagem [id*="Adicionar"]:hover,
@@ -402,7 +324,7 @@
         background-color: #8B4513 !important;
         border-color: #6B3410 !important;
         animation: buttonWiggle 0.5s ease-in-out !important;
-        box-shadow: 0 0 20px rgba(160, 82, 45, .8), 0 6px 12px rgba(160, 82, 45, .5) !important;
+        box-shadow: 0 0 20px rgba(160,82,45,.8), 0 6px 12px rgba(160,82,45,.5) !important;
         color: #fff !important;
     }
 
@@ -413,13 +335,11 @@
         min-height: 38px !important;
         border-bottom: 1px solid #ccc !important;
     }
-
     #modalFinalizaViagem .e-ddt .e-ddt-icon,
     #modalFinalizaViagem .e-dropdowntree .e-ddt-icon {
         height: 36px !important;
         line-height: 36px !important;
     }
-
     #modalFinalizaViagem .e-ddt input.e-dropdowntree,
     #modalFinalizaViagem .e-ddt .e-overflow {
         height: 36px !important;
@@ -433,8 +353,7 @@
         --bs-modal-width: 700px;
     }
 
-    #modalFicha .modal-header,
-    #modalFicha .modal-footer {
+    #modalFicha .modal-header, #modalFicha .modal-footer {
         padding: .75rem 1rem;
     }
 
@@ -449,57 +368,48 @@
 
     #reportViewer1 {
         width: 100%;
+        height: 640px;
+        min-height: 640px;
     }
 
     /* Estados de validação */
     :root {
         --ftx-invalid: #dc3545;
-        --ftx-glow: rgba(220, 53, 69, .18);
-        --ftx-glow-focus: rgba(220, 53, 69, .28);
+        --ftx-glow: rgba(220,53,69,.18);
+        --ftx-glow-focus: rgba(220,53,69,.28);
         --ftx-high: #fd7e14;
-        --ftx-high-glow: rgba(253, 126, 20, .18);
-    }
-
-    input.is-invalid,
-    textarea.is-invalid {
+        --ftx-high-glow: rgba(253,126,20,.18);
+    }
+
+    input.is-invalid, textarea.is-invalid {
         border-color: var(--ftx-invalid) !important;
         color: var(--ftx-invalid) !important;
         box-shadow: 0 0 0 .2rem var(--ftx-glow);
     }
 
-    input.is-invalid:focus,
-    textarea.is-invalid:focus {
+    input.is-invalid:focus, textarea.is-invalid:focus {
         box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
     }
 
-    .e-input-group.is-invalid,
-    .e-float-input.is-invalid,
-    .e-control-wrapper.is-invalid {
+    .e-input-group.is-invalid, .e-float-input.is-invalid, .e-control-wrapper.is-invalid {
         box-shadow: 0 0 0 .2rem var(--ftx-glow);
         border-radius: .25rem;
     }
 
-    .e-input-group.is-invalid:focus-within,
-    .e-float-input.is-invalid:focus-within,
-    .e-control-wrapper.is-invalid:focus-within {
+    .e-input-group.is-invalid:focus-within, .e-float-input.is-invalid:focus-within, .e-control-wrapper.is-invalid:focus-within {
         box-shadow: 0 0 0 .28rem var(--ftx-glow-focus);
     }
 
-    .e-input-group.is-invalid .e-input,
-    .e-float-input.is-invalid input,
-    .e-control-wrapper.is-invalid input {
+    .e-input-group.is-invalid .e-input, .e-float-input.is-invalid input, .e-control-wrapper.is-invalid input {
         border-color: var(--ftx-invalid) !important;
         color: var(--ftx-invalid) !important;
     }
 
-    input.is-high,
-    textarea.is-high {
+    input.is-high, textarea.is-high {
         box-shadow: 0 0 0 .2rem var(--ftx-high-glow);
     }
 
-    .e-input-group.is-high,
-    .e-float-input.is-high,
-    .e-control-wrapper.is-high {
+    .e-input-group.is-high, .e-float-input.is-high, .e-control-wrapper.is-high {
         box-shadow: 0 0 0 .2rem var(--ftx-high-glow);
         border-radius: .25rem;
     }
@@ -509,15 +419,14 @@
         position: absolute;
         top: 50%;
         left: 50%;
-        transform: translate(-50%, -50%);
+        transform: translate(-50%,-50%);
         z-index: 10;
-        background: rgba(255, 255, 255, 0.9);
+        background: rgba(255,255,255,0.9);
         padding: 30px;
         border-radius: 8px;
     }
 
-    #modalFicha #uploadContainer,
-    #modalFicha #imageContainer {
+    #modalFicha #uploadContainer, #modalFicha #imageContainer {
         transition: opacity .3s ease;
     }
 
@@ -531,7 +440,7 @@
         background-color: #333;
         border-color: #333;
         color: #fff;
-        box-shadow: 0 0 4px rgba(51, 51, 51, .6);
+        box-shadow: 0 0 4px rgba(51,51,51,.6);
     }
 
     /* Espaços do DataTables desta página */
@@ -574,7 +483,7 @@
         border-radius: 12px;
         padding: 1.25rem;
         margin-bottom: 1.5rem;
-        border: 1px solid rgba(0, 0, 0, 0.08);
+        border: 1px solid rgba(0,0,0,0.08);
     }
 
     /* Uniformiza altura de todos os controles de filtro */
@@ -640,58 +549,55 @@
     }
 
     /* ================================================================
-       TOOLTIPS PARA BOTÕES DESABILITADOS
+       CORREÇÃO PAGINAÇÃO - Especificidade máxima para esta página
+       Garante que os caracteres fiquem BRANCOS em todos os botões
        ================================================================ */
-    /* IMPORTANTE: Garante que tooltips funcionem em botões desabilitados */
-    .btn.disabled {
-        pointer-events: auto !important;
-        /* Permite hover em botões desabilitados */
-        cursor: not-allowed !important;
-        /* Cursor indica que ação não é permitida */
-    }
-
-    /* Previne cliques em botões desabilitados, mas mantém hover para tooltips */
-    .btn.disabled:active,
-    .btn.disabled:focus {
-        pointer-events: none !important;
-    }
-
-    /* Estilo personalizado para tooltips da página de Viagens */
-    .ftx-tooltip-viagens.e-tooltip-wrap {
-        z-index: 10000 !important;
-        /* Garante que tooltip fique acima de tudo */
+
+    /* Botão atual/ativo - FORÇA texto branco */
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current *,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current a,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current span,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current:hover,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current:hover *,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current:hover a,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.current:hover span {
+        color: #ffffff !important;
+    }
+
+    /* Todos os botões não desabilitados - FORÇA texto branco */
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled),
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled) *,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled) a,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled) span,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover *,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover a,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button:not(.disabled):hover span {
+        color: #ffffff !important;
+    }
+
+    /* Botões desabilitados também ficam brancos */
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.disabled,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.disabled *,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.disabled a,
+    #tblViagem_wrapper .dataTables_paginate .paginate_button.disabled span {
+        color: #ffffff !important;
     }
 </style>
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * VIAGENS INDEX - RICHTEXTEDITOR CALLBACKS
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Callbacks de inicialização para RichTextEditor nos modais de viagem.
-     * @@requires Syncfusion EJ2 RichTextEditor
-        * @@file Viagens / Index.cshtml
-        */
-
     var defaultRTE;
     var defaultRTEDescricao;
 
-    /**
-     * Callback de criação do RTE principal
-     * @@description Armazena referência da instância do RichTextEditor
-        */
     function onCreate() {
-            try {
-                defaultRTE = this;
-            } catch (error) {
-                Alerta.TratamentoErroComLinha("Index.cshtml", "onCreate", error);
-            }
+        try {
+            defaultRTE = this;
+        } catch (error) {
+            Alerta.TratamentoErroComLinha("Index.cshtml", "onCreate", error);
         }
-
-    /**
-     * Callback de criação do RTE de descrição
-     * @@description Armazena referência da instância do RichTextEditor de descrição
-        */
+    }
+
     function onCreateDescricao() {
         try {
             defaultRTEDescricao = this;
@@ -749,10 +655,14 @@
                                             <i class="fa-duotone fa-car"></i>
                                             Veículo
                                         </label>
-                                        <ejs-combobox id="lstVeiculos" placeholder="Todos os Veículos"
-                                            allowFiltering="true" filterType="Contains"
-                                            dataSource="@ViewData["lstVeiculos"]" popupHeight="250px" width="100%"
-                                            showClearButton="true">
+                                        <ejs-combobox id="lstVeiculos"
+                                                      placeholder="Todos os Veículos"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      dataSource="@ViewData["lstVeiculos"]"
+                                                      popupHeight="250px"
+                                                      width="100%"
+                                                      showClearButton="true">
                                             <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -762,10 +672,14 @@
                                             <i class="fa-duotone fa-id-card"></i>
                                             Motorista
                                         </label>
-                                        <ejs-combobox id="lstMotorista" placeholder="Todos os Motoristas"
-                                            allowFiltering="true" filterType="Contains"
-                                            dataSource="@ViewData["lstMotorista"]" popupHeight="250px" width="100%"
-                                            showClearButton="true">
+                                        <ejs-combobox id="lstMotorista"
+                                                      placeholder="Todos os Motoristas"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      dataSource="@ViewData["lstMotorista"]"
+                                                      popupHeight="250px"
+                                                      width="100%"
+                                                      showClearButton="true">
                                             <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -775,9 +689,14 @@
                                             <i class="fa-duotone fa-circle-check"></i>
                                             Status
                                         </label>
-                                        <ejs-combobox id="lstStatus" placeholder="Todos os Status" allowFiltering="true"
-                                            filterType="Contains" dataSource="@ViewData["lstStatus"]"
-                                            popupHeight="250px" width="100%" showClearButton="true">
+                                        <ejs-combobox id="lstStatus"
+                                                      placeholder="Todos os Status"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      dataSource="@ViewData["lstStatus"]"
+                                                      popupHeight="250px"
+                                                      width="100%"
+                                                      showClearButton="true">
                                             <e-combobox-fields text="Status" value="StatusId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -787,10 +706,14 @@
                                             <i class="fa-duotone fa-calendar-star"></i>
                                             Evento
                                         </label>
-                                        <ejs-combobox id="lstEventos" placeholder="Todos os Eventos"
-                                            allowFiltering="true" filterType="Contains"
-                                            dataSource="@ViewData["lstEventos"]" popupHeight="250px" width="100%"
-                                            showClearButton="true">
+                                        <ejs-combobox id="lstEventos"
+                                                      placeholder="Todos os Eventos"
+                                                      allowFiltering="true"
+                                                      filterType="Contains"
+                                                      dataSource="@ViewData["lstEventos"]"
+                                                      popupHeight="250px"
+                                                      width="100%"
+                                                      showClearButton="true">
                                             <e-combobox-fields text="Evento" value="EventoId"></e-combobox-fields>
                                         </ejs-combobox>
                                     </div>
@@ -798,9 +721,12 @@
 
                                 <div class="row mt-3">
                                     <div class="col-md-4">
-                                        <button type="button" class="btn btn-fundo-laranja text-white w-100"
-                                            id="btnFiltro" name="btnFiltro" style="height: 44px;"
-                                            onclick="FtxViagens.filtrar()">
+                                        <button type="button"
+                                                class="btn btn-fundo-laranja text-white w-100"
+                                                id="btnFiltro"
+                                                name="btnFiltro"
+                                                style="height: 44px;"
+                                                onclick="FtxViagens.filtrar()">
                                             <i class="fa-sharp-duotone fa-duotone fa-filter-list me-2"></i>
                                             <span>Filtrar Viagens</span>
                                         </button>
@@ -809,50 +735,29 @@
                             </div>
 
                             <div id="divViagens">
-                                <table id="tblViagem"
-                                    class="table table-bordered table-striped table-hover mt-2 ftx-table" width="100%">
+                                <table id="tblViagem" class="table table-bordered table-striped table-hover mt-2 ftx-table" width="100%">
                                     <thead style="background-color: #4a6fa5 !important;">
                                         <tr>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Vistoria</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Data</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Início</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Requisitante</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Setor</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Motorista</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Veículo</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Status</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Ações</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Km
-                                                Inicial</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Combustivel Inicial</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Data Final</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Hora Final</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Km
-                                                Final</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Combustivel Final</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Resumo Ocorrência</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Descrição Ocorrência</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Status Documento</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Status Cartão Abastecimento</th>
-                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">
-                                                Descricao</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Vistoria</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Data</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Início</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Requisitante</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Setor</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Motorista</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Veículo</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Status</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Ação</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Km Inicial</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Combustivel Inicial</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Data Final</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Hora Final</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Km Final</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Combustivel Final</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Resumo Ocorrência</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Descrição Ocorrência</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Status Documento</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Status Cartão Abastecimento</th>
+                                            <th style="background-color: #4a6fa5 !important; color: #fff !important;">Descricao</th>
                                         </tr>
                                     </thead>
                                     <tbody>
@@ -875,263 +780,159 @@
                     <i class="fa-duotone fa-flag-checkered" aria-hidden="true"></i>
                     <span>Finalizar a Viagem</span>
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
-            </div>
-
-            <div class="modal-body p-4" style="background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);">
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
+            </div>
+
+            <div class="modal-body">
                 <form id="frmRequisitante">
                     <input type="hidden" id="txtId" />
 
-                    <div class="card shadow-sm mb-4" style="border-left: 4px solid #6c757d;">
-                        <div class="card-header bg-light py-2">
-                            <h6 class="mb-0">
-                                <i class="fa-duotone fa-arrow-right-from-bracket text-secondary me-2"></i>
-                                <strong>Dados Iniciais da Viagem</strong>
-                            </h6>
-                        </div>
-                        <div class="card-body py-3">
-                            <div class="row g-3">
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-calendar-day"></i>
-                                        Data Inicial
-                                    </label>
-                                    <input readonly id="txtDataInicial" class="form-control form-control-sm" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-clock"></i>
-                                        Hora Inicial
-                                    </label>
-                                    <input readonly id="txtHoraInicial" class="form-control form-control-sm" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-gauge"></i>
-                                        Km Inicial
-                                    </label>
-                                    <input readonly id="txtKmInicial" class="form-control form-control-sm" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-gas-pump"></i>
-                                        Combustível Inicial
-                                    </label>
-                                    <ejs-dropdowntree id="ddtCombustivelInicial" popupHeight="200px"
-                                        showClearButton="true" class="form-control" enabled="false">
-                                        <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]" value="Nivel"
-                                            text="Descricao" imageURL="Imagem">
-                                        </e-dropdowntree-fields>
-                                    </ejs-dropdowntree>
-                                </div>
+                    <div class="row g-3 mb-4">
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-calendar-day"></i>
+                                Data Inicial
+                            </label>
+                            <input readonly id="txtDataInicial" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-clock"></i>
+                                Hora Inicial
+                            </label>
+                            <input readonly id="txtHoraInicial" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-gauge"></i>
+                                Km Inicial
+                            </label>
+                            <input readonly id="txtKmInicial" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-gas-pump"></i>
+                                Combustível Inicial
+                            </label>
+                            <ejs-dropdowntree id="ddtCombustivelInicial"
+                                              popupHeight="200px"
+                                              showClearButton="true"
+                                              class="form-control"
+                                              enabled="false">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]"
+                                                       value="Nivel"
+                                                       text="Descricao"
+                                                       imageURL="Imagem">
+                                </e-dropdowntree-fields>
+                            </ejs-dropdowntree>
+                        </div>
+                    </div>
+
+                    <hr class="my-4" style="border-color: rgba(0,0,0,0.1);" />
+
+                    <div class="row g-3 mb-4">
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-calendar-check"></i>
+                                Data Final <span class="text-danger">*</span>
+                            </label>
+                            <input id="txtDataFinal" type="date" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-clock"></i>
+                                Hora Final <span class="text-danger">*</span>
+                            </label>
+                            <input id="txtHoraFinal" type="time" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-gauge-max"></i>
+                                Km Final <span class="text-danger">*</span>
+                            </label>
+                            <input id="txtKmFinal" type="number" class="form-control" placeholder="0" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-gas-pump"></i>
+                                Combustível Final <span class="text-danger">*</span>
+                            </label>
+                            <ejs-dropdowntree id="ddtCombustivelFinal"
+                                              popupHeight="200px"
+                                              showClearButton="true"
+                                              class="form-control">
+                                <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]"
+                                                       value="Nivel"
+                                                       text="Descricao"
+                                                       imageURL="Imagem">
+                                </e-dropdowntree-fields>
+                            </ejs-dropdowntree>
+                        </div>
+                    </div>
+
+                    <div class="row g-3 mb-4">
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-road"></i>
+                                Km Percorrido
+                            </label>
+                            <input readonly id="txtKmPercorrido" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-stopwatch"></i>
+                                Duração
+                            </label>
+                            <input readonly id="txtDuracao" class="form-control" />
+                        </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-file-check"></i>
+                                Status Documento
+                            </label>
+                            <div class="form-check form-switch mt-2">
+                                <input class="form-check-input" type="checkbox" id="chkStatusDocumento" checked />
+                                <label class="form-check-label" for="chkStatusDocumento">Entregue</label>
                             </div>
                         </div>
-                    </div>
-
-                    <div class="card shadow-sm mb-4" style="border-left: 4px solid #28a745;">
-                        <div class="card-header bg-light py-2">
-                            <h6 class="mb-0">
-                                <i class="fa-duotone fa-flag-checkered text-success me-2"></i>
-                                <strong>Dados Finais da Viagem</strong>
-                            </h6>
-                        </div>
-                        <div class="card-body py-3">
-                            <div class="row g-3 mb-3">
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-calendar-check"></i>
-                                        Data Final <span class="text-danger">*</span>
-                                    </label>
-                                    <input id="txtDataFinal" type="date" class="form-control form-control-sm" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-clock"></i>
-                                        Hora Final <span class="text-danger">*</span>
-                                    </label>
-                                    <input id="txtHoraFinal" type="time" class="form-control form-control-sm" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-gauge-max"></i>
-                                        Km Final <span class="text-danger">*</span>
-                                    </label>
-                                    <input id="txtKmFinal" type="number" class="form-control form-control-sm"
-                                        placeholder="0" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-gas-pump"></i>
-                                        Combustível Final <span class="text-danger">*</span>
-                                    </label>
-                                    <ejs-dropdowntree id="ddtCombustivelFinal" popupHeight="200px"
-                                        showClearButton="true" class="form-control">
-                                        <e-dropdowntree-fields dataSource="@ViewData["dataCombustivel"]" value="Nivel"
-                                            text="Descricao" imageURL="Imagem">
-                                        </e-dropdowntree-fields>
-                                    </ejs-dropdowntree>
-                                </div>
+                        <div class="col-md-3">
+                            <label class="ftx-label">
+                                <i class="fa-duotone fa-credit-card"></i>
+                                Cartão Abastecimento
+                            </label>
+                            <div class="form-check form-switch mt-2">
+                                <input class="form-check-input" type="checkbox" id="chkStatusCartaoAbastecimento" checked />
+                                <label class="form-check-label" for="chkStatusCartaoAbastecimento">Entregue</label>
                             </div>
-                            <div class="row g-3">
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-road"></i>
-                                        Km Percorrido
-                                    </label>
-                                    <input readonly id="txtKmPercorrido"
-                                        class="form-control form-control-sm bg-light" />
-                                </div>
-                                <div class="col-md-3">
-                                    <label class="ftx-label">
-                                        <i class="fa-duotone fa-stopwatch"></i>
-                                        Duração
-                                    </label>
-                                    <input readonly id="txtDuracao" class="form-control form-control-sm bg-light" />
-                                </div>
-                            </div>
-                        </div>
-                    </div>
-
-                    <div class="card shadow-sm mb-4" style="border-left: 4px solid #17a2b8;">
-                        <div class="card-header bg-light py-2">
-                            <h6 class="mb-0">
-                                <i class="fa-duotone fa-boxes-stacked text-info me-2"></i>
-                                <strong>Controle de Itens Devolvidos</strong>
-                            </h6>
-                        </div>
-                        <div class="card-body py-3">
-                            <div class="row g-3">
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-file-check"></i>
-                                            Documento
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox"
-                                                id="chkStatusDocumento" checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkStatusDocumento">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-credit-card"></i>
-                                            Cartão
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox"
-                                                id="chkStatusCartaoAbastecimento" checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkStatusCartaoAbastecimento">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-plug"></i>
-                                            Cabo
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox" id="chkCabo"
-                                                checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkCabo">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-droplet"></i>
-                                            Arla
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox" id="chkArla"
-                                                checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkArla">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-link"></i>
-                                            Cinta
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox" id="chkCinta"
-                                                checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkCinta">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-
-                                <div class="col-6 col-md-2">
-                                    <div class="ftx-item-card">
-                                        <label class="ftx-label mb-1">
-                                            <i class="fa-duotone fa-tablet-screen-button"></i>
-                                            Tablet
-                                        </label>
-                                        <div class="form-check form-switch">
-                                            <input class="form-check-input ftx-switch-lg" type="checkbox" id="chkTablet"
-                                                checked />
-                                            <label class="form-check-label ftx-switch-label"
-                                                for="chkTablet">Devolvido</label>
-                                        </div>
-                                    </div>
-                                </div>
-                            </div>
-                        </div>
-                    </div>
-
-                    <div class="card shadow-sm mb-4" style="border-left: 4px solid #ffc107;">
-                        <div class="card-header bg-light py-2">
-                            <h6 class="mb-0">
-                                <i class="fa-duotone fa-pen-to-square text-warning me-2"></i>
-                                <strong>Observações</strong>
-                            </h6>
-                        </div>
-                        <div class="card-body py-3">
+                        </div>
+                    </div>
+
+                    <div class="row g-3 mb-3">
+                        <div class="col-12">
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-align-left"></i>
                                 Descrição / Observações
                             </label>
-                            <ejs-richtexteditor id="rteDescricao" placeholder="Digite observações sobre a viagem..."
-                                height="120px" created="onCreateDescricao">
+                            <ejs-richtexteditor id="rteDescricao"
+                                                placeholder="Digite observações sobre a viagem..."
+                                                height="150px"
+                                                created="onCreateDescricao">
                             </ejs-richtexteditor>
                         </div>
                     </div>
 
-                    <div class="card shadow-sm" style="border-left: 4px solid #dc3545;">
-                        <div class="card-header bg-light py-2 d-flex justify-content-between align-items-center">
-                            <h6 class="mb-0">
-                                <i class="fa-duotone fa-car-burst text-danger me-2"></i>
-                                <strong>Ocorrências da Viagem</strong>
-                            </h6>
-                            <button type="button" class="btn btn-adicionar-ocorrencia btn-sm"
-                                id="btnAdicionarOcorrencia">
-                                <i class="fa-duotone fa-circle-plus me-1"></i> Adicionar
+                    <div class="secao-ocorrencias mt-4">
+                        <div class="d-flex justify-content-between align-items-center mb-3">
+                            <h5 class="mb-0">
+                                <i class="fa-duotone fa-car-burst me-2 text-danger"></i>
+                                Ocorrências da Viagem
+                            </h5>
+                            <button type="button" class="btn btn-adicionar-ocorrencia btn-sm" id="btnAdicionarOcorrencia">
+                                <i class="fa-duotone fa-circle-plus me-1"></i> Adicionar Ocorrência
                             </button>
                         </div>
-                        <div class="card-body py-3">
-                            <div id="listaOcorrencias">
-                                <p class="text-muted text-center mb-0">
-                                    <i class="fa-duotone fa-info-circle me-1"></i>
-                                    Nenhuma ocorrência registrada
-                                </p>
-                            </div>
+                        <div id="listaOcorrencias">
+
                         </div>
                     </div>
                 </form>
@@ -1149,8 +950,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalCustosViagem" tabindex="-1" aria-labelledby="modalCustosViagemLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalCustosViagem" tabindex="-1" aria-labelledby="modalCustosViagemLabel" aria-hidden="true">
     <div class="modal-dialog modal-lg" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-dinheiro">
@@ -1158,8 +958,7 @@
                     <i class="fa-duotone fa-money-bill-trend-up" aria-hidden="true"></i>
                     <span>Custos da Viagem</span>
                 </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -1292,8 +1091,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalSolucaoOcorrencia" tabindex="-1" aria-labelledby="modalSolucaoOcorrenciaLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalSolucaoOcorrencia" tabindex="-1" aria-labelledby="modalSolucaoOcorrenciaLabel" aria-hidden="true">
     <div class="modal-dialog modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-verde">
@@ -1301,8 +1099,7 @@
                     <i class="fa-duotone fa-clipboard-check" aria-hidden="true"></i>
                     <span>Informar Solução</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -1313,8 +1110,10 @@
                         <i class="fa-duotone fa-message-lines me-1 text-success"></i>
                         Descreva a solução aplicada
                     </label>
-                    <textarea id="txtSolucaoBaixa" class="form-control" rows="4"
-                        placeholder="Informe como a ocorrência foi resolvida..."></textarea>
+                    <textarea id="txtSolucaoBaixa"
+                              class="form-control"
+                              rows="4"
+                              placeholder="Informe como a ocorrência foi resolvida..."></textarea>
                 </div>
             </div>
 
@@ -1330,53 +1129,78 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalFicha" tabindex="-1" aria-hidden="true" data-bs-backdrop="static"
-    data-bs-keyboard="false">
-    <div class="modal-dialog modal-lg" role="document">
+<div class="modal fade" id="modalFicha" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true" aria-hidden="true">
+    <div class="modal-dialog modal-dialog-centered" role="document">
         <div class="modal-content">
             <div class="modal-header modal-header-azul">
-                <h4 class="modal-title d-flex align-items-center gap-2">
-                    <i class="fa-duotone fa-clipboard-check" aria-hidden="true"></i>
+                <h5 class="modal-title d-flex align-items-center gap-2">
+                    <i class="fa-duotone fa-file-image" aria-hidden="true"></i>
                     <span>Ficha de Vistoria</span>
-                </h4>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
-            </div>
-
-            <div class="modal-body position-relative" style="min-height: 400px;">
-
+                </h5>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
+            </div>
+
+            <div class="modal-body">
                 <input type="hidden" id="hiddenViagemId" />
 
-                <div id="loadingSpinner" class="d-none text-center py-5">
-                    <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
+                <div id="loadingSpinner" style="display: none; text-align: center; padding: 2rem;">
+                    <div class="spinner-border text-primary" role="status">
                         <span class="visually-hidden">Carregando...</span>
                     </div>
-                    <p class="mt-3 text-muted">Processando...</p>
+                    <p class="mt-2 text-muted">Carregando ficha...</p>
                 </div>
 
+                <div id="uploadContainer" style="display: none;">
+                    <div class="mb-3">
+                        <label for="txtFile" class="form-label fw-semibold">
+                            <i class="fa-duotone fa-upload me-1"></i> Selecionar Ficha de Vistoria
+                        </label>
+                        <input type="file" class="form-control" id="txtFile" accept="image/*" />
+                        <small class="form-text text-muted">Formatos aceitos: JPG, PNG, GIF (máx. 5MB)</small>
+                    </div>
+                    <button type="button" class="btn btn-success w-100" id="btnSalvarFicha" onclick="salvarFichaVistoria()" style="display: none;">
+                        <i class="fa-duotone fa-save me-1"></i> Salvar Ficha
+                    </button>
+                </div>
+
                 <div id="imageContainer">
-
-                    <div id="uploadContainer" class="mb-3">
-                        <div class="ftx-label">
-                            <i class="fa-duotone fa-upload"></i>
-                            Selecione a Ficha de Vistoria
-                        </div>
-                        <input type="file" id="txtFile" class="form-control" accept="image/*,.pdf" />
-                        <small class="text-muted d-block mt-2">
-                            <i class="fa-duotone fa-info-circle me-1"></i>
-                            Formatos aceitos: JPG, PNG, PDF | Tamanho máximo: 5MB
-                        </small>
-                    </div>
-
-                    <div class="text-center">
-                        <img id="imgFichaViewer" src="" alt="Ficha de Vistoria" class="img-fluid"
-                            style="max-width: 100%; height: auto; display: none; border-radius: 8px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);" />
-                    </div>
-
-                    <div id="noImageContainer" class="text-center py-5" style="display: none;">
-                        <i class="fa-duotone fa-image-slash" style="font-size: 4rem; color: #ccc;"></i>
-                        <p class="text-muted mt-3">Nenhuma ficha de vistoria cadastrada</p>
-                    </div>
+                    <img id="imgFichaViewer" class="img-fluid" src="" alt="Ficha de Vistoria" style="display: none; max-width: 100%; height: auto; border: 1px solid #ddd; border-radius: 4px;" />
+
+                    <div id="noImageContainer" style="display: none; text-align: center; padding: 3rem 1rem;">
+                        <i class="fa-duotone fa-file-slash fa-4x text-muted mb-3"></i>
+                        <p class="text-muted mb-0">Nenhuma ficha de vistoria cadastrada</p>
+                        <small class="text-muted">Faça upload de uma imagem para registrar a ficha</small>
+                    </div>
+                </div>
+            </div>
+
+            <div class="modal-footer">
+                <button type="button" class="btn btn-secondary" id="btnAlterarFicha" style="display: none;" onclick="$('#uploadContainer').show(); $('#btnSalvarFicha').show();">
+                    <i class="fa-duotone fa-edit me-1"></i> Alterar Ficha
+                </button>
+                <button type="button" class="btn btn-vinho" data-bs-dismiss="modal">
+                    <i class="fa-duotone fa-times me-1"></i> Fechar
+                </button>
+            </div>
+        </div>
+    </div>
+</div>
+
+<div class="modal fade" id="modalPrint" tabindex="-1" data-bs-backdrop="static" data-bs-keyboard="true" aria-hidden="true">
+    <div class="modal-dialog modal-xl modal-dialog-centered" role="document">
+        <div class="modal-content">
+            <div class="modal-header modal-header-roxo">
+                <h5 class="modal-title d-flex align-items-center gap-2">
+                    <i class="fa-duotone fa-print" aria-hidden="true"></i>
+                    <span>Ficha da Viagem - Impressão</span>
+                </h5>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
+            </div>
+
+            <div class="modal-body">
+                <input type="hidden" id="txtViagemId" />
+                <div id="ReportContainer" style="min-height: 700px;">
+                    <div id="reportViewer1" style="width: 100%; height: 640px;">Loading...</div>
                 </div>
             </div>
 
@@ -1384,26 +1208,29 @@
                 <button type="button" class="btn btn-vinho" data-bs-dismiss="modal">
                     <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Fechar
                 </button>
-                <button type="button" id="btnAlterarFicha" class="btn btn-azul" style="display: none;">
-                    <i class="fa-duotone fa-pen-to-square me-1"></i> Alterar Ficha
-                </button>
-                <button type="button" id="btnSalvarFicha" class="btn btn-verde" style="display: none;">
-                    <i class="fa-duotone fa-floppy-disk me-1"></i> Salvar Ficha
-                </button>
             </div>
         </div>
     </div>
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.common.min.css" rel="stylesheet" />
     <link href="https://kendo.cdn.telerik.com/2022.1.412/styles/kendo.default.min.css" rel="stylesheet" />
     <script src="https://kendo.cdn.telerik.com/2022.1.412/js/kendo.all.min.js"></script>
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/cultures/kendo.culture.pt-BR.min.js"></script>
+    <script src="https://kendo.cdn.telerik.com/2022.1.412/js/messages/kendo.messages.pt-BR.min.js"></script>
+    <script>
+        try {
+            if (window.kendo && kendo.culture) {
+                kendo.culture("pt-BR");
+            }
+        } catch (error) {
+            console.warn("Kendo pt-BR: falha ao aplicar cultura.", error);
+        }
+    </script>
 
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.6.347/pdf.min.js"></script>
     <script>
```
