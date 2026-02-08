# Pages/Manutencao/Upsert.cshtml

**Mudanca:** GRANDE | **+216** linhas | **-273** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/Upsert.cshtml
+++ ATUAL: Pages/Manutencao/Upsert.cshtml
@@ -16,7 +16,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -27,6 +27,7 @@
             outline: 2px solid rgba(255, 255, 255, 0.8) !important;
             outline-offset: 2px;
         }
+
     </style>
     <link href="~/css/ftx-card-styled.css" rel="stylesheet" asp-append-version="true" />
     <style>
@@ -43,7 +44,7 @@
         .ftx-card-header .ftx-card-title i {
             color: #fff !important;
             --fa-primary-color: #fff !important;
-            --fa-secondary-color: rgba(255, 255, 255, 0.7) !important;
+            --fa-secondary-color: rgba(255,255,255,0.7) !important;
         }
 
         /* Botão laranja usa padrão global do frotix.css */
@@ -51,14 +52,6 @@
 }
 
 <script>
-    /**
-     * ═══════════════════════════════════════════════════════════════════════════
-     * CALLBACK: SELEÇÃO DE VEÍCULO
-     * ═══════════════════════════════════════════════════════════════════════════
-     * @@description Acionado quando o ComboBox de veículo sofre alteração.
-     * Dispara o preenchimento de dados relacionados ao veículo
-        * selecionado(seção, motorista habitual, etc.).
-     */
     function VeiculoChange() {
         try {
             PreenchePagina();
@@ -73,9 +66,9 @@
         --bs-gutter-y: .4rem;
     }
 
-    .bs-form-spaced .row+.row {
-        margin-top: 0.75rem;
-    }
+        .bs-form-spaced .row + .row {
+            margin-top: 0.75rem;
+        }
 
     .bs-form-spaced .form-group {
         margin-bottom: .25rem;
@@ -210,20 +203,20 @@
         transition: background-color .3s ease;
     }
 
-    .btn-fundo-laranja-item i {
-        font-size: 1.3em;
-        --fa-primary-color: #ffffff;
-        --fa-secondary-color: #FFD700;
-        --fa-secondary-opacity: 1;
-        transform-origin: center center;
-        display: inline-block;
-        transition: transform .3s ease;
-    }
-
-    .btn-fundo-laranja-item:hover i {
-        transform: rotate(12deg) scale(1.15);
-        --fa-secondary-color: #FFB300;
-    }
+        .btn-fundo-laranja-item i {
+            font-size: 1.3em;
+            --fa-primary-color: #ffffff;
+            --fa-secondary-color: #FFD700;
+            --fa-secondary-opacity: 1;
+            transform-origin: center center;
+            display: inline-block;
+            transition: transform .3s ease;
+        }
+
+        .btn-fundo-laranja-item:hover i {
+            transform: rotate(12deg) scale(1.15);
+            --fa-secondary-color: #FFB300;
+        }
 
     .btn-vinho {
         background-color: #c82333 !important;
@@ -236,16 +229,13 @@
         text-decoration: none;
     }
 
-    .btn-vinho:hover,
-    .btn-vinho:focus,
-    .btn-vinho:active,
-    .btn-vinho:focus-visible {
-        background-color: #a71d2a !important;
-        border-color: #a71d2a !important;
-        color: #fff !important;
-        outline: none !important;
-        box-shadow: none !important;
-    }
+        .btn-vinho:hover, .btn-vinho:focus, .btn-vinho:active, .btn-vinho:focus-visible {
+            background-color: #a71d2a !important;
+            border-color: #a71d2a !important;
+            color: #fff !important;
+            outline: none !important;
+            box-shadow: none !important;
+        }
 
     .btn-azul {
         background-color: #0D98BA !important;
@@ -257,107 +247,102 @@
         align-items: center;
     }
 
-    .btn-azul:hover,
-    .btn-azul:focus,
-    .btn-azul:active,
-    .btn-azul:focus-visible {
-        background-color: #0c86a3 !important;
-        color: #fff !important;
-        border-color: #0c86a3 !important;
-        outline: none !important;
-        box-shadow: none !important;
-    }
-
-    .tblManutencao,
-    .tblManutencao th,
-    .tblManutencao td {
+        .btn-azul:hover, .btn-azul:focus, .btn-azul:active, .btn-azul:focus-visible {
+            background-color: #0c86a3 !important;
+            color: #fff !important;
+            border-color: #0c86a3 !important;
+            outline: none !important;
+            box-shadow: none !important;
+        }
+
+    .tblManutencao, .tblManutencao th, .tblManutencao td {
         font-size: .82rem;
         vertical-align: middle !important;
     }
 
-    .tblManutencao thead th {
-        font-size: .86rem;
-        font-weight: 700;
-    }
-
-    .tblManutencao .col-acao {
-        text-align: center;
-        white-space: nowrap;
-    }
-
-    .tblManutencao .col-acao .btn-acao {
-        width: 31px;
-        height: 30px;
-        padding: 0 !important;
-        display: inline-flex;
-        align-items: center;
-        justify-content: center;
-        border-radius: .30rem;
-        font-size: .95rem;
-        line-height: 1;
-        margin: 2px;
-        position: relative;
-        transition: transform .2s ease, box-shadow .2s ease;
-    }
-
-    .tblManutencao .col-acao .btn-acao:hover {
-        transform: scale(1.12);
-        box-shadow: 0 0 4px rgba(0, 0, 0, .2);
-    }
-
-    .tblManutencao .btn-acao[data-tooltip] {
-        position: relative;
-    }
-
-    .tblManutencao .btn-acao[data-tooltip]::after {
-        content: attr(data-tooltip);
-        position: absolute;
-        bottom: 115%;
-        left: 50%;
-        transform: translateX(-50%);
-        background-color: #444;
-        color: #fff;
-        padding: 4px 8px;
-        border-radius: 4px;
-        font-size: .7rem;
-        white-space: nowrap;
-        opacity: 0;
-        pointer-events: none;
-        transition: opacity .3s ease, transform .3s ease;
-        z-index: 10;
-    }
-
-    .tblManutencao .btn-acao[data-tooltip]:hover::after {
-        opacity: .95;
-        transform: translateX(-50%) translateY(-4px);
-    }
-
-    .tblManutencao .col-acao>div {
-        display: flex;
-        justify-content: center;
-        align-items: center;
-        gap: 4px;
-        flex-wrap: nowrap;
-        height: 100%;
-    }
+        .tblManutencao thead th {
+            font-size: .86rem;
+            font-weight: 700;
+        }
+
+        .tblManutencao .col-acao {
+            text-align: center;
+            white-space: nowrap;
+        }
+
+            .tblManutencao .col-acao .btn-acao {
+                width: 31px;
+                height: 30px;
+                padding: 0 !important;
+                display: inline-flex;
+                align-items: center;
+                justify-content: center;
+                border-radius: .30rem;
+                font-size: .95rem;
+                line-height: 1;
+                margin: 2px;
+                position: relative;
+                transition: transform .2s ease, box-shadow .2s ease;
+            }
+
+                .tblManutencao .col-acao .btn-acao:hover {
+                    transform: scale(1.12);
+                    box-shadow: 0 0 4px rgba(0,0,0,.2);
+                }
+
+        .tblManutencao .btn-acao[data-tooltip] {
+            position: relative;
+        }
+
+            .tblManutencao .btn-acao[data-tooltip]::after {
+                content: attr(data-tooltip);
+                position: absolute;
+                bottom: 115%;
+                left: 50%;
+                transform: translateX(-50%);
+                background-color: #444;
+                color: #fff;
+                padding: 4px 8px;
+                border-radius: 4px;
+                font-size: .7rem;
+                white-space: nowrap;
+                opacity: 0;
+                pointer-events: none;
+                transition: opacity .3s ease, transform .3s ease;
+                z-index: 10;
+            }
+
+            .tblManutencao .btn-acao[data-tooltip]:hover::after {
+                opacity: .95;
+                transform: translateX(-50%) translateY(-4px);
+            }
+
+        .tblManutencao .col-acao > div {
+            display: flex;
+            justify-content: center;
+            align-items: center;
+            gap: 4px;
+            flex-wrap: nowrap;
+            height: 100%;
+        }
 
     .btn {
         transition: all 0.2s ease-in-out;
     }
 
-    .btn:hover {
-        transform: scale(1.05);
-        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.25);
-    }
+        .btn:hover {
+            transform: scale(1.05);
+            box-shadow: 0 4px 12px rgba(0,0,0,0.25);
+        }
 
     .btn-foto {
         background: chocolate;
         border-color: chocolate;
     }
 
-    .btn-foto:hover {
-        filter: brightness(0.95);
-    }
+        .btn-foto:hover {
+            filter: brightness(0.95);
+        }
 
     .sf-tooltip-darkorange.e-tooltip-wrap {
         background: #bf5b00;
@@ -396,19 +381,15 @@
                             <i class="fa-duotone fa-screwdriver-wrench"></i>
                             @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada")
                             {
-                                <span>Registro de Manutenção - Fechado/Baixado <span
-                                        style="font-size: 0.85em; font-weight: 400;">(edição não permitida)</span></span>
+                                <span>Registro de Manutenção - Fechado/Baixado <span style="font-size: 0.85em; font-weight: 400;">(edição não permitida)</span></span>
                             }
                             else if (Model.ManutencaoObj.Manutencao.StatusOS == "Cancelada")
                             {
-                                <span>Registro de Manutenção - Cancelado <span
-                                        style="font-size: 0.85em; font-weight: 400;">(edição não permitida)</span></span>
+                                <span>Registro de Manutenção - Cancelado <span style="font-size: 0.85em; font-weight: 400;">(edição não permitida)</span></span>
                             }
                             else
                             {
-                                @(Model.ManutencaoObj.Manutencao.ManutencaoId != Guid.Empty ? "Atualizar " : "Criar ")
-
-                                <span>Registro de Manutenção</span>
+                                @(Model.ManutencaoObj.Manutencao.ManutencaoId != Guid.Empty ? "Atualizar " : "Criar ")<span>Registro de Manutenção</span>
                             }
                         </h2>
                         <div class="ftx-card-actions">
@@ -432,10 +413,8 @@
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
                                         <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.NumOS"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.NumOS"></span>
-                                        <input id="txtOS" class="form-control form-control-xs"
-                                            asp-for="ManutencaoObj.Manutencao.NumOS" disabled="disabled" />
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.NumOS"></span>
+                                        <input id="txtOS" class="form-control form-control-xs" asp-for="ManutencaoObj.Manutencao.NumOS" disabled="disabled" />
                                     </div>
                                 </div>
                             </div>
@@ -443,31 +422,23 @@
                             <div class="row g-4">
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
-                                        <label class="fw-bold"
-                                            asp-for="ManutencaoObj.Manutencao.DataSolicitacao"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.DataSolicitacao"></span>
-                                        <input id="txtDataSolicitacao" class="form-control form-control-xs" type="date"
-                                            asp-for="ManutencaoObj.Manutencao.DataSolicitacao" />
+                                        <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataSolicitacao"></label>
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataSolicitacao"></span>
+                                        <input id="txtDataSolicitacao" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataSolicitacao" />
                                     </div>
                                 </div>
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
-                                        <label class="fw-bold"
-                                            asp-for="ManutencaoObj.Manutencao.DataDisponibilidade"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.DataDisponibilidade"></span>
-                                        <input id="txtDataDisponibilidade" class="form-control form-control-xs"
-                                            type="date" asp-for="ManutencaoObj.Manutencao.DataDisponibilidade" />
+                                        <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataDisponibilidade"></label>
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataDisponibilidade"></span>
+                                        <input id="txtDataDisponibilidade" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataDisponibilidade" />
                                     </div>
                                 </div>
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
                                         <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.StatusOS"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.StatusOS"></span>
-                                        <select id="lstStatus" class="form-control form-control-xs"
-                                            asp-for="ManutencaoObj.Manutencao.StatusOS">
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.StatusOS"></span>
+                                        <select id="lstStatus" class="form-control form-control-xs" asp-for="ManutencaoObj.Manutencao.StatusOS">
                                             <option value="Aberta">Aberta</option>
                                             <option value="Fechada">Fechada</option>
                                         </select>
@@ -476,19 +447,15 @@
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
                                         <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataEntrega"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.DataEntrega"></span>
-                                        <input id="txtDataEntrega" class="form-control form-control-xs" type="date"
-                                            asp-for="ManutencaoObj.Manutencao.DataEntrega" />
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataEntrega"></span>
+                                        <input id="txtDataEntrega" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataEntrega" />
                                     </div>
                                 </div>
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
                                         <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataDevolucao"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.DataDevolucao"></span>
-                                        <input id="txtDataDevolucao" class="form-control form-control-xs" type="date"
-                                            asp-for="ManutencaoObj.Manutencao.DataDevolucao" />
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataDevolucao"></span>
+                                        <input id="txtDataDevolucao" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataDevolucao" />
                                     </div>
                                 </div>
                             </div>
@@ -496,13 +463,9 @@
                             <div class="row g-4">
                                 <div class="col-12 col-md-3 col-xl-2">
                                     <div class="form-group">
-                                        <label class="fw-bold"
-                                            asp-for="ManutencaoObj.Manutencao.ReservaEnviado"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.ReservaEnviado"></span>
-                                        <select id="lstReserva" class="form-control form-control-xs"
-                                            asp-for="ManutencaoObj.Manutencao.ReservaEnviado"
-                                            onchange="fnExibeReserva()">
+                                        <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.ReservaEnviado"></label>
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.ReservaEnviado"></span>
+                                        <select id="lstReserva" class="form-control form-control-xs" asp-for="ManutencaoObj.Manutencao.ReservaEnviado" onchange="fnExibeReserva()">
                                             <option value="1">Enviado</option>
                                             <option value="0" selected>Não Enviado</option>
                                         </select>
@@ -514,37 +477,32 @@
                                         <div class="row g-4">
                                             <div class="col-12 col-md-6 col-xl-4">
                                                 <label class="fw-bold">Veículo Reserva</label>
-                                                <ejs-combobox id="lstVeiculoReserva" placeholder="Selecione um Veículo"
-                                                    allowFiltering="true" filterType="Contains"
-                                                    dataSource="@ViewData["dataVeiculoReserva"]" popupHeight="200px"
-                                                    width="100%" showClearButton="true"
-                                                    ejs-for="@Model.ManutencaoObj.Manutencao.VeiculoReservaId">
-                                                    <e-combobox-fields text="Descricao"
-                                                        value="VeiculoId"></e-combobox-fields>
+                                                <ejs-combobox id="lstVeiculoReserva"
+                                                              placeholder="Selecione um Veículo"
+                                                              allowFiltering="true"
+                                                              filterType="Contains"
+                                                              dataSource="@ViewData["dataVeiculoReserva"]"
+                                                              popupHeight="200px"
+                                                              width="100%"
+                                                              showClearButton="true"
+                                                              ejs-for="@Model.ManutencaoObj.Manutencao.VeiculoReservaId">
+                                                    <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                                                 </ejs-combobox>
                                             </div>
 
                                             <div class="col-12 col-md-3 col-xl-2">
                                                 <div class="form-group">
-                                                    <label class="fw-bold"
-                                                        asp-for="ManutencaoObj.Manutencao.DataRecebimentoReserva"></label>
-                                                    <span class="text-danger"
-                                                        asp-validation-for="ManutencaoObj.Manutencao.DataRecebimentoReserva"></span>
-                                                    <input id="txtDataRecebimentoReserva"
-                                                        class="form-control form-control-xs" type="date"
-                                                        asp-for="ManutencaoObj.Manutencao.DataRecebimentoReserva" />
+                                                    <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataRecebimentoReserva"></label>
+                                                    <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataRecebimentoReserva"></span>
+                                                    <input id="txtDataRecebimentoReserva" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataRecebimentoReserva" />
                                                 </div>
                                             </div>
 
                                             <div class="col-12 col-md-3 col-xl-2">
                                                 <div class="form-group">
-                                                    <label class="fw-bold"
-                                                        asp-for="ManutencaoObj.Manutencao.DataDevolucaoReserva"></label>
-                                                    <span class="text-danger"
-                                                        asp-validation-for="ManutencaoObj.Manutencao.DataDevolucaoReserva"></span>
-                                                    <input id="txtDataDevolucaoReserva"
-                                                        class="form-control form-control-xs" type="date"
-                                                        asp-for="ManutencaoObj.Manutencao.DataDevolucaoReserva" />
+                                                    <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.DataDevolucaoReserva"></label>
+                                                    <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.DataDevolucaoReserva"></span>
+                                                    <input id="txtDataDevolucaoReserva" class="form-control form-control-xs" type="date" asp-for="ManutencaoObj.Manutencao.DataDevolucaoReserva" />
                                                 </div>
                                             </div>
                                         </div>
@@ -556,10 +514,8 @@
                                 <div class="col-12">
                                     <div class="form-group">
                                         <label class="fw-bold" asp-for="ManutencaoObj.Manutencao.ResumoOS"></label>
-                                        <span class="text-danger"
-                                            asp-validation-for="ManutencaoObj.Manutencao.ResumoOS"></span>
-                                        <input id="txtResumoOS" class="form-control form-control-xs"
-                                            asp-for="ManutencaoObj.Manutencao.ResumoOS" />
+                                        <span class="text-danger" asp-validation-for="ManutencaoObj.Manutencao.ResumoOS"></span>
+                                        <input id="txtResumoOS" class="form-control form-control-xs" asp-for="ManutencaoObj.Manutencao.ResumoOS" />
                                     </div>
                                 </div>
                             </div>
@@ -567,11 +523,16 @@
                             <div class="row g-4" id="LinhaVeiculo">
                                 <div class="col-12 col-md-6 col-xl-4">
                                     <label class="fw-bold">Veículo</label>
-                                    <ejs-combobox id="lstVeiculo" placeholder="Selecione um Veículo"
-                                        allowFiltering="true" filterType="Contains"
-                                        dataSource="@ViewData["dataVeiculo"]" popupHeight="200px" width="100%"
-                                        showClearButton="true" ejs-for="@Model.ManutencaoObj.Manutencao.VeiculoId"
-                                        change="VeiculoChange">
+                                    <ejs-combobox id="lstVeiculo"
+                                                  placeholder="Selecione um Veículo"
+                                                  allowFiltering="true"
+                                                  filterType="Contains"
+                                                  dataSource="@ViewData["dataVeiculo"]"
+                                                  popupHeight="200px"
+                                                  width="100%"
+                                                  showClearButton="true"
+                                                  ejs-for="@Model.ManutencaoObj.Manutencao.VeiculoId"
+                                                  change="VeiculoChange">
                                         <e-combobox-fields text="Descricao" value="VeiculoId"></e-combobox-fields>
                                     </ejs-combobox>
                                 </div>
@@ -583,8 +544,7 @@
                                 <div class="col-12">
                                     <div id="divOcorrencias" style="display:none">
                                         <h4>Ocorrências Disponíveis para o Veículo</h4>
-                                        <table id="tblOcorrencia"
-                                            class="table table-bordered table-striped tblManutencao" width="100%">
+                                        <table id="tblOcorrencia" class="table table-bordered table-striped tblManutencao" width="100%">
                                             <thead>
                                                 <tr>
                                                     <th>Ficha</th>
@@ -610,8 +570,7 @@
                                 <div class="col-12">
                                     <div id="divPendencias" style="display:none">
                                         <h4>Pendências em aberto para o Veículo</h4>
-                                        <table id="tblPendencia"
-                                            class="table table-bordered table-striped tblManutencao" width="100%">
+                                        <table id="tblPendencia" class="table table-bordered table-striped tblManutencao" width="100%">
                                             <thead>
                                                 <tr>
                                                     <th>Ficha</th>
@@ -637,29 +596,25 @@
                             <div class="row">
                                 <div class="col-12">
                                     <div id="divItens" style="display:none">
-                                        @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" ||
-                                                                                Model.ManutencaoObj.Manutencao.StatusOS == "Cancelada")
+                                        @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" || Model.ManutencaoObj.Manutencao.StatusOS == "Cancelada")
                                         {
-                                            <h4 id="lblItensSelecionados"><i
-                                                    class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no
-                                                veículo</h4>
+                                            <h4 id="lblItensSelecionados"><i class="fa-duotone fa-solid fa-wrench me-2"></i>Manutenções efetuadas no veículo</h4>
                                         }
                                         else
                                         {
-                                            <h4 id="lblItensSelecionados"><i
-                                                    class="fa-duotone fa-solid fa-list-check me-2"></i>Itens selecionados
-                                                para manutenção no veículo</h4>
-
-                                            <button id="btnAdicionaItem" type="button"
-                                                class="btn btn-sm fundo-chocolate btn-add-item" data-bs-toggle="modal"
-                                                data-bs-target="#modalManutencao"
-                                                data-ejtip="Adicionar Novo Item de Manutenção">
+                                            <h4 id="lblItensSelecionados"><i class="fa-duotone fa-solid fa-list-check me-2"></i>Itens selecionados para manutenção no veículo</h4>
+
+                                            <button id="btnAdicionaItem"
+                                                    type="button"
+                                                    class="btn btn-sm fundo-chocolate btn-add-item"
+                                                    data-bs-toggle="modal"
+                                                    data-bs-target="#modalManutencao"
+                                                    data-ejtip="Adicionar Novo Item de Manutenção">
                                                 <i class="fa-duotone fa-circle-plus"></i>&nbsp;Adicionar Item
                                             </button>
                                         }
 
-                                        <table id="tblItens" class="table table-bordered table-striped tblManutencao"
-                                            width="100%">
+                                        <table id="tblItens" class="table table-bordered table-striped tblManutencao" width="100%">
                                             <thead>
                                                 <tr>
                                                     <th>Tipo</th>
@@ -689,29 +644,22 @@
                                     <div class="form-group">
                                         <div id="divUsuarioCriacao" style="display: none;">
                                             <label id="lblUsuarioCriacao" class="font-weight-light d-block text-muted">
-                                                <i class="fa-sharp-duotone fa-solid fa-user-plus"></i> <span>Criado
-                                                    por:</span>
+                                                <i class="fa-sharp-duotone fa-solid fa-user-plus"></i> <span>Criado por:</span>
                                             </label>
                                         </div>
                                         <div id="divUsuarioAlteracao" style="display: none;">
-                                            <label id="lblUsuarioAlteracao"
-                                                class="font-weight-light d-block text-muted">
-                                                <i class="fa-sharp-duotone fa-solid fa-user-pen"></i> <span>Alterado
-                                                    por:</span>
+                                            <label id="lblUsuarioAlteracao" class="font-weight-light d-block text-muted">
+                                                <i class="fa-sharp-duotone fa-solid fa-user-pen"></i> <span>Alterado por:</span>
                                             </label>
                                         </div>
                                         <div id="divUsuarioCancelamento" style="display: none;">
-                                            <label id="lblUsuarioCancelamento"
-                                                class="font-weight-light d-block text-muted">
-                                                <i class="fa-sharp-duotone fa-solid fa-user-xmark"></i> <span>Cancelado
-                                                    por:</span>
+                                            <label id="lblUsuarioCancelamento" class="font-weight-light d-block text-muted">
+                                                <i class="fa-sharp-duotone fa-solid fa-user-xmark"></i> <span>Cancelado por:</span>
                                             </label>
                                         </div>
                                         <div id="divUsuarioFinalizacao" style="display: none;">
-                                            <label id="lblUsuarioFinalizacao"
-                                                class="font-weight-light d-block text-muted">
-                                                <i class="fa-duotone fa-solid fa-user-check"></i> <span>Finalizado
-                                                    por:</span>
+                                            <label id="lblUsuarioFinalizacao" class="font-weight-light d-block text-muted">
+                                                <i class="fa-duotone fa-solid fa-user-check"></i> <span>Finalizado por:</span>
                                             </label>
                                         </div>
                                     </div>
@@ -726,29 +674,21 @@
                                         <div class="col-12 col-md-4">
                                             @if (Model.ManutencaoObj.Manutencao.ManutencaoId != Guid.Empty)
                                             {
-                                                <button id="btnEdita" method="post" asp-page-handler="Edit"
-                                                    asp-route-id="@Model.ManutencaoObj.Manutencao.ManutencaoId"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i>
-                                                    Atualizar Manutenção
+                                                <button id="btnEdita" method="post" asp-page-handler="Edit" asp-route-id="@Model.ManutencaoObj.Manutencao.ManutencaoId" class="btn btn-azul btn-submit-spin form-control">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Atualizar Manutenção
                                                 </button>
                                             }
                                             else
                                             {
-                                                <button id="btnAdiciona" type="submit" value="Submit"
-                                                    asp-page-handler="Submit"
-                                                    class="btn btn-azul btn-submit-spin form-control">
-                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar
-                                                    Manutenção
+                                                <button id="btnAdiciona" type="submit" value="Submit" asp-page-handler="Submit" class="btn btn-azul btn-submit-spin form-control">
+                                                    <i class="fa-duotone fa-floppy-disk icon-space icon-pulse"></i> Criar Manutenção
                                                 </button>
                                             }
                                         </div>
 
                                         <div class="col-12 col-md-4 text-end">
-                                            <a id="btnVoltar" asp-page="./ListaManutencao"
-                                                class="btn btn-ftx-fechar form-control" data-ftx-loading>
-                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i>
-                                                Cancelar Operação
+                                            <a id="btnVoltar" asp-page="./ListaManutencao" class="btn btn-ftx-fechar form-control" data-ftx-loading>
+                                                <i class="fa-duotone fa-circle-xmark icon-space icon-pulse"></i> Cancelar Operação
                                             </a>
                                         </div>
                                     </div>
@@ -763,8 +703,7 @@
     </div>
 </form>
 
-<div class="modal fade" id="modalManutencao" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true"
-    enctype="multipart/form-data">
+<div class="modal fade" id="modalManutencao" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" enctype="multipart/form-data">
     <div class="modal-dialog modal-lg">
         <div class="modal-content">
             <div class="modal-header modal-header-azul">
@@ -790,16 +729,20 @@
 
                     <label class="fw-bold">Descrição do Item</label>
                     <ejs-richtexteditor id="rteManutencao" created="onCreate" locale="pt-BR" height="200px">
-                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage"
-                            path="./DadosEditaveis/ImagensOcorrencias/"></e-richtexteditor-insertimagesettings>
+                        <e-richtexteditor-insertimagesettings saveUrl="api/Viagem/SaveImage" path="./DadosEditaveis/ImagensOcorrencias/"></e-richtexteditor-insertimagesettings>
                     </ejs-richtexteditor>
 
                     <div class="row g-4">
                         <div class="col-12">
                             <label class="fw-bold">Motorista</label>
-                            <ejs-combobox id="lstMotorista" placeholder="Selecione um Motorista" allowFiltering="true"
-                                filterType="Contains" dataSource="@ViewData["dataMotorista"]" popupHeight="200px"
-                                width="100%" showClearButton="true">
+                            <ejs-combobox id="lstMotorista"
+                                          placeholder="Selecione um Motorista"
+                                          allowFiltering="true"
+                                          filterType="Contains"
+                                          dataSource="@ViewData["dataMotorista"]"
+                                          popupHeight="200px"
+                                          width="100%"
+                                          showClearButton="true">
                                 <e-combobox-fields text="Nome" value="MotoristaId"></e-combobox-fields>
                             </ejs-combobox>
                         </div>
@@ -811,19 +754,16 @@
                             <input class="form-control" type="file" id="txtFileItem" name="files">
                         </div>
                         <div class="col-12 col-md-6 d-flex align-items-end">
-                            <img class="img img-fluid" id="imgViewerItem"
-                                style="border: 1px solid #000000; max-height: 350px;" />
+                            <img class="img img-fluid" id="imgViewerItem" style="border: 1px solid #000000; max-height: 350px;" />
                         </div>
                     </div>
                 </div>
 
                 <div class="modal-footer">
-                    <button id="btnInsereItem" class="btn btn-azul form-control" type="submit" value="SUBMIT"
-                        style="max-width: 200px;">
+                    <button id="btnInsereItem" class="btn btn-azul form-control" type="submit" value="SUBMIT" style="max-width: 200px;">
                         <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> Inserir o Item
                     </button>
-                    <button id="btnFechar" type="button" class="btn btn-vinho form-control" data-bs-dismiss="modal"
-                        style="max-width: 150px;">
+                    <button id="btnFechar" type="button" class="btn btn-vinho form-control" data-bs-dismiss="modal" style="max-width: 150px;">
                         <i class="fa-duotone fa-rotate-left icon-space icon-rotate-left"></i> Fechar
                     </button>
                 </div>
@@ -842,8 +782,7 @@
                 </button>
             </div>
             <div class="modal-body table-bordered container-fluid">
-                <img class="img" id="imgViewerOcorrencia"
-                    style="border: 1px solid #000000; margin-top: 10px; height: 400px" />
+                <img class="img" id="imgViewerOcorrencia" style="border: 1px solid #000000; margin-top: 10px; height: 400px" />
                 <div class="modal-footer"></div>
             </div>
         </div>
@@ -870,8 +809,7 @@
     <div class="modal-dialog modal-xl">
         <div class="modal-content">
             <div class="modal-header modal-header-azul d-flex justify-content-between align-items-center">
-                @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" || Model.ManutencaoObj.Manutencao.StatusOS ==
-                                "Cancelada")
+                @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" || Model.ManutencaoObj.Manutencao.StatusOS == "Cancelada")
                 {
                     <h4 class="modal-title mb-0">Visualizar Foto do Item</h4>
                 }
@@ -880,8 +818,7 @@
                     <h4 class="modal-title mb-0">Foto do Item de Manutenção</h4>
                 }
                 <div class="d-flex gap-2">
-                    @if (Model.ManutencaoObj.Manutencao.StatusOS != "Fechada" && Model.ManutencaoObj.Manutencao.StatusOS
-                                        != "Cancelada")
+                    @if (Model.ManutencaoObj.Manutencao.StatusOS != "Fechada" && Model.ManutencaoObj.Manutencao.StatusOS != "Cancelada")
                     {
                         <button id="btnAdicionarFoto" type="button" class="btn btn-azul" style="width:200px">
                             <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i> Inserir Foto
@@ -893,8 +830,7 @@
                 </div>
             </div>
             <div class="modal-body">
-                @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" || Model.ManutencaoObj.Manutencao.StatusOS ==
-                                "Cancelada")
+                @if (Model.ManutencaoObj.Manutencao.StatusOS == "Fechada" || Model.ManutencaoObj.Manutencao.StatusOS == "Cancelada")
                 {
                     <div class="row g-3">
                         <div class="col-12 text-center">
@@ -930,39 +866,46 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
-
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
+
+    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet" type="text/css" />
     <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
     <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
     <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
+    <script src="https://cdn.kendostatic.com/2022.1.412/js/cultures/kendo.culture.pt-BR.min.js"></script>
+    <script src="https://cdn.kendostatic.com/2022.1.412/js/messages/kendo.messages.pt-BR.min.js"></script>
+    <script>
+        try {
+            if (window.kendo && kendo.culture) {
+                kendo.culture("pt-BR");
+            }
+        } catch (error) {
+            console.warn("Kendo pt-BR: falha ao aplicar cultura.", error);
+        }
+    </script>
 
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';</script>
 
-    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.3/moment-with-locales.min.js" crossorigin="anonymous"
-        referrerpolicy="no-referrer"></script>
+    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.3/moment-with-locales.min.js" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
 
     @using System.Text.Json
 
-<script>
-    const manutencaoId = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.ManutencaoId.ToString()));
-    const manutencaoDataCriacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataCriacao?.ToString("O")));
-    const manutencaoDataDisponibilidade = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataDisponibilidade?.ToString("O")));
-    const manutencaoIdUsuarioCriacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioCriacao?.ToString()));
-    const manutencaoIdUsuarioAlteracao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioAlteracao?.ToString()));
-    const manutencaoDataAlteracao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataAlteracao?.ToString("O")));
-    const manutencaoIdUsuarioCancelamento = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioCancelamento?.ToString()));
-    const manutencaoDataCancelamento = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataCancelamento?.ToString("O")));
-    const manutencaoIdUsuarioFinalizacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioFinalizacao?.ToString()));
-    const manutencaoDataFinalizacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataFinalizacao?.ToString("O")));
-    const manutencaoReservaEnviado = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.ReservaEnviado));
-    const manutencaoStatusOS = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.StatusOS ?? ""));
+    <script>
+        const manutencaoId = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.ManutencaoId.ToString()));
+        const manutencaoDataCriacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataCriacao?.ToString("O")));
+        const manutencaoDataDisponibilidade = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataDisponibilidade?.ToString("O")));
+        const manutencaoIdUsuarioCriacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioCriacao?.ToString()));
+        const manutencaoIdUsuarioAlteracao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioAlteracao?.ToString()));
+        const manutencaoDataAlteracao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataAlteracao?.ToString("O")));
+        const manutencaoIdUsuarioCancelamento = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioCancelamento?.ToString()));
+        const manutencaoDataCancelamento = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataCancelamento?.ToString("O")));
+        const manutencaoIdUsuarioFinalizacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.IdUsuarioFinalizacao?.ToString()));
+        const manutencaoDataFinalizacao = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.DataFinalizacao?.ToString("O")));
+        const manutencaoReservaEnviado = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.ReservaEnviado));
+        const manutencaoStatusOS = @Html.Raw(JsonSerializer.Serialize(Model.ManutencaoObj.Manutencao?.StatusOS ?? ""));
 
         window.manutencaoId = manutencaoId;
         window.manutencaoDataCriacao = manutencaoDataCriacao;
```
