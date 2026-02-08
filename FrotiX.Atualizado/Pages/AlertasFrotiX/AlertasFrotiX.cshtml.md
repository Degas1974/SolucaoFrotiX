# Pages/AlertasFrotiX/AlertasFrotiX.cshtml

**Mudanca:** GRANDE | **+200** linhas | **-271** linhas

---

```diff
--- JANEIRO: Pages/AlertasFrotiX/AlertasFrotiX.cshtml
+++ ATUAL: Pages/AlertasFrotiX/AlertasFrotiX.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.AlertasFrotiX.IndexModel
 @{
     ViewData["Title"] = "Gestão de Alertas";
@@ -11,7 +10,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -31,11 +30,11 @@
             transition: all 0.3s ease;
         }
 
-        .btn-azul:hover {
-            background-color: #0284c7;
-            transform: translateY(-2px);
-            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
-        }
+            .btn-azul:hover {
+                background-color: #0284c7;
+                transform: translateY(-2px);
+                box-shadow: 0 4px 8px rgba(0,0,0,0.2);
+            }
 
         /* Título da página */
         .titulo-pagina {
@@ -76,64 +75,22 @@
             color: white;
         }
 
-        .badge-aniversario {
-            background-color: #ec4899;
+        .badge-diversos {
+            background-color: #6c757d;
             color: white;
         }
 
-        /* Prioridades - PADRÃO BADGE */
-        .badge-prioridade-ftx {
-            font-size: 0.7rem;
-            padding: 0.25rem 0.6rem;
-            border-radius: 4px;
-            font-weight: 700;
-            text-transform: uppercase;
-            display: inline-block;
-            letter-spacing: 0.5px;
-        }
-
+        /* Prioridades */
         .prioridade-baixa {
-            background-color: #dcfce7 !important;
-            /* Verde mais claro */
-            color: #166534 !important;
+            color: #10b981;
         }
 
         .prioridade-media {
-            background-color: #fef3c7 !important;
-            /* Creme mais escuro */
-            color: #92400e !important;
+            color: #f59e0b;
         }
 
         .prioridade-alta {
-            background-color: #722f37 !important;
-            /* Vermelho vinho */
-            color: white !important;
-        }
-
-        /* Custom Tab Badges */
-        .badge-verde-militar {
-            background-color: #5d6d31 !important;
-            color: white !important;
-        }
-
-        .badge-laranja-sobrio {
-            background-color: #d97706 !important;
-            color: white !important;
-        }
-
-        /* Botão Olho (Azul Petróleo) */
-        .btn-eye-frotix {
-            background-color: #325d88 !important;
-            color: white !important;
-            border: none !important;
-            transition: all 0.2s ease;
-        }
-
-        .btn-eye-frotix:hover {
-            background-color: #264666 !important;
-            color: white !important;
-            transform: scale(1.05);
-            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
+            color: #ef4444;
         }
 
         /* TABS CUSTOMIZADO - SEM DEPENDÊNCIA DE BOOTSTRAP JS */
@@ -190,12 +147,9 @@
             padding: 0.375rem 0.75rem;
         }
 
-        #tblAlertasLidos td,
-        #tblAlertasLidos th,
-        #tblAlertasInativos td,
-        #tblAlertasInativos th,
-        #tblMeusAlertas td,
-        #tblMeusAlertas th {
+        #tblAlertasLidos td, #tblAlertasLidos th,
+        #tblAlertasInativos td, #tblAlertasInativos th,
+        #tblMeusAlertas td, #tblMeusAlertas th {
             padding: 0.5rem;
             vertical-align: middle;
         }
@@ -211,74 +165,74 @@
             padding-left: 40px;
         }
 
-        .timeline-vertical::before {
-            content: '';
-            position: absolute;
-            left: 15px;
-            top: 0;
-            bottom: 0;
-            width: 2px;
-            background: #dee2e6;
-        }
+            .timeline-vertical::before {
+                content: '';
+                position: absolute;
+                left: 15px;
+                top: 0;
+                bottom: 0;
+                width: 2px;
+                background: #dee2e6;
+            }
 
         .timeline-item {
             position: relative;
             padding-bottom: 20px;
         }
 
-        .timeline-item::before {
-            content: '';
-            position: absolute;
-            left: -29px;
-            top: 5px;
-            width: 12px;
-            height: 12px;
-            border-radius: 50%;
-            background: #fff;
-            border: 2px solid #007bff;
-        }
-
-        .timeline-item.lido::before {
-            background: #28a745;
-            border-color: #28a745;
-        }
-
-        .timeline-item.nao-lido::before {
-            background: #ffc107;
-            border-color: #ffc107;
-        }
-
-        .timeline-item.apagado::before {
-            background: #dc3545;
-            border-color: #dc3545;
-        }
+            .timeline-item::before {
+                content: '';
+                position: absolute;
+                left: -29px;
+                top: 5px;
+                width: 12px;
+                height: 12px;
+                border-radius: 50%;
+                background: #fff;
+                border: 2px solid #007bff;
+            }
+
+            .timeline-item.lido::before {
+                background: #28a745;
+                border-color: #28a745;
+            }
+
+            .timeline-item.nao-lido::before {
+                background: #ffc107;
+                border-color: #ffc107;
+            }
+
+            .timeline-item.apagado::before {
+                background: #dc3545;
+                border-color: #dc3545;
+            }
 
         /* Cards */
         .card {
             transition: transform 0.3s ease, box-shadow 0.3s ease;
         }
 
-        .card:hover {
-            transform: translateY(-2px);
-            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.2) !important;
-        }
+            .card:hover {
+                transform: translateY(-2px);
+                box-shadow: 0 8px 16px rgba(0,0,0,0.2) !important;
+            }
 
         /* Progress bar */
         .progress-bar {
             transition: width 1s ease, background 0.3s ease;
         }
 
-        .progress-bar.bg-danger {
-            background: linear-gradient(90deg, #dc3545 0%, #c82333 100%);
-        }
-
-        .progress-bar.bg-warning {
-            background: linear-gradient(90deg, #ffc107 0%, #e0a800 100%);
-        }
-
-        .progress-bar.bg-success {
-            background: linear-gradient(90deg, #28a745 0%, #218838 100%);
-        }
+            .progress-bar.bg-danger {
+                background: linear-gradient(90deg, #dc3545 0%, #c82333 100%);
+            }
+
+            .progress-bar.bg-warning {
+                background: linear-gradient(90deg, #ffc107 0%, #e0a800 100%);
+            }
+
+            .progress-bar.bg-success {
+                background: linear-gradient(90deg, #28a745 0%, #218838 100%);
+            }
 
         /* Progress bar no modal - FINO */
         #progressoLeitura {
@@ -310,19 +264,19 @@
             color: white !important;
         }
 
-        .btn-vinho-wine:hover {
-            background-color: #5a252c !important;
-            border-color: #4a1f24 !important;
-            transform: translateY(-2px);
-            box-shadow: 0 4px 8px rgba(114, 47, 55, 0.3);
-        }
-
-        .btn-vinho-wine:active,
-        .btn-vinho-wine:focus {
-            background-color: #4a1f24 !important;
-            border-color: #3a1820 !important;
-            box-shadow: 0 0 0 0.2rem rgba(114, 47, 55, 0.25);
-        }
+            .btn-vinho-wine:hover {
+                background-color: #5a252c !important;
+                border-color: #4a1f24 !important;
+                transform: translateY(-2px);
+                box-shadow: 0 4px 8px rgba(114, 47, 55, 0.3);
+            }
+
+            .btn-vinho-wine:active,
+            .btn-vinho-wine:focus {
+                background-color: #4a1f24 !important;
+                border-color: #3a1820 !important;
+                box-shadow: 0 0 0 0.2rem rgba(114, 47, 55, 0.25);
+            }
 
         /* Melhorar visual do botão fechar */
         #modalDetalhesAlerta .btn-vinho {
@@ -330,12 +284,12 @@
             border-color: #722f37;
         }
 
-        #modalDetalhesAlerta .btn-vinho:hover {
-            background-color: #5a252c;
-            border-color: #4a1f24;
-            transform: translateY(-2px);
-            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
-        }
+            #modalDetalhesAlerta .btn-vinho:hover {
+                background-color: #5a252c;
+                border-color: #4a1f24;
+                transform: translateY(-2px);
+                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
+            }
     </style>
 }
 
@@ -357,34 +311,39 @@
 
             <div class="panel-container show pt-4">
 
-                <input type="hidden" id="usuarioAtualId"
-                    value="@User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value" />
+                <input type="hidden" id="usuarioAtualId" value="@User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value" />
 
                 <div class="px-3 mt-3">
 
                     <ul class="nav nav-tabs nav-tabs-custom" id="alertasTabs" role="tablist">
                         <li class="nav-item" role="presentation">
-                            <a class="nav-link active" href="#tabInativos" data-tab="tabInativos"
-                                data-ejtip="Alertas que já foram finalizados">
+                            <a class="nav-link active"
+                               href="#tabInativos"
+                               data-tab="tabInativos"
+                               data-ejtip="Alertas que já foram finalizados">
                                 <i class="fa-duotone fa-history me-2"></i>
                                 Alertas Inativos
                                 <span class="badge bg-secondary" id="badgeInativos">0</span>
                             </a>
                         </li>
                         <li class="nav-item" role="presentation">
-                            <a class="nav-link" href="#tabAtivos" data-tab="tabAtivos"
-                                data-ejtip="Alertas atualmente ativos no sistema">
+                            <a class="nav-link"
+                               href="#tabAtivos"
+                               data-tab="tabAtivos"
+                               data-ejtip="Alertas atualmente ativos no sistema">
                                 <i class="fa-duotone fa-bell-on me-2"></i>
                                 Alertas Ativos
-                                <span class="badge badge-verde-militar" id="badgeAtivos">0</span>
+                                <span class="badge bg-danger" id="badgeAtivos">0</span>
                             </a>
                         </li>
                         <li class="nav-item" role="presentation">
-                            <a class="nav-link" href="#tabMeusAlertas" data-tab="tabMeusAlertas"
-                                data-ejtip="Alertas que você recebeu">
+                            <a class="nav-link"
+                               href="#tabMeusAlertas"
+                               data-tab="tabMeusAlertas"
+                               data-ejtip="Alertas que você recebeu">
                                 <i class="fa-duotone fa-user-circle me-2"></i>
                                 Meus Alertas
-                                <span class="badge badge-laranja-sobrio" id="badgeMeusAlertas">0</span>
+                                <span class="badge bg-primary" id="badgeMeusAlertas">0</span>
                             </a>
                         </li>
                     </ul>
@@ -394,17 +353,16 @@
                         <div class="tab-pane active" id="tabInativos" role="tabpanel">
                             <div class="panel-content pt-3">
                                 <div class="box-body">
-                                    <table id="tblAlertasInativos" class="table table-bordered table-striped"
-                                        width="100%">
+                                    <table id="tblAlertasInativos" class="table table-bordered table-striped" width="100%">
                                         <thead>
                                             <tr>
-                                                <th width="10%">Data Inserção</th>
                                                 <th width="3%">Ícone</th>
-                                                <th width="15%">Título</th>
-                                                <th width="35%">Descrição</th>
+                                                <th width="17%">Título</th>
+                                                <th width="37%">Descrição</th>
                                                 <th width="7%">Tipo</th>
                                                 <th width="7%">Prioridade</th>
-                                                <th width="10%">Data Baixa</th>
+                                                <th width="8%">Data Inserção</th>
+                                                <th width="8%">Data Baixa</th>
                                                 <th width="8%">% Leitura</th>
                                                 <th width="5%">Ações</th>
                                             </tr>
@@ -432,16 +390,15 @@
                                     <table id="tblMeusAlertas" class="table table-bordered table-striped" width="100%">
                                         <thead>
                                             <tr>
-                                                <th width="10%">Inserido Em</th>
                                                 <th width="3%">Ícone</th>
                                                 <th width="15%">Título</th>
-                                                <th width="25%">Descrição</th>
-                                                <th width="7%">Tipo</th>
-                                                <th width="7%">Notificado</th>
+                                                <th width="30%">Descrição</th>
+                                                <th width="8%">Tipo</th>
+                                                <th width="8%">Notificado</th>
                                                 <th width="10%">Notificado Em</th>
-                                                <th width="7%">Lido</th>
+                                                <th width="8%">Lido</th>
                                                 <th width="10%">Lido Em</th>
-                                                <th width="6%">Ações</th>
+                                                <th width="8%">Ações</th>
                                             </tr>
                                         </thead>
                                         <tbody>
@@ -457,8 +414,7 @@
     </div>
 </div>
 
-<div class="modal fade" id="modalDetalhesAlerta" tabindex="-1" role="dialog" aria-labelledby="modalDetalhesAlertaLabel"
-    aria-hidden="true">
+<div class="modal fade" id="modalDetalhesAlerta" tabindex="-1" role="dialog" aria-labelledby="modalDetalhesAlertaLabel" aria-hidden="true">
     <div class="modal-dialog modal-xl" role="document">
         <div class="modal-content">
 
@@ -474,8 +430,7 @@
                         </div>
                     </div>
                 </div>
-                <button type="button" class="close" data-bs-dismiss="modal" aria-label="Fechar"
-                    data-ejtip="Fechar Modal">
+                <button type="button" class="close" data-bs-dismiss="modal" aria-label="Fechar" data-ejtip="Fechar Modal">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
@@ -523,8 +478,7 @@
                                         <div class="col-6">
                                             <p class="mb-2">
                                                 <i class="fa-duotone fa-users text-primary"></i>
-                                                <strong>Destinatários:</strong> <span
-                                                    id="totalDestinatariosResumo">0</span>
+                                                <strong>Destinatários:</strong> <span id="totalDestinatariosResumo">0</span>
                                             </p>
                                             <p class="mb-2">
                                                 <i class="fa-duotone fa-bell text-info"></i>
@@ -532,8 +486,7 @@
                                             </p>
                                             <p class="mb-0">
                                                 <i class="fa-duotone fa-clock text-warning"></i>
-                                                <strong>Aguardando:</strong> <span
-                                                    id="aguardandoNotificacaoResumo">0</span>
+                                                <strong>Aguardando:</strong> <span id="aguardandoNotificacaoResumo">0</span>
                                             </p>
                                         </div>
                                         <div class="col-6">
@@ -634,8 +587,12 @@
                             </div>
                             <div class="progress" style="height: 30px;">
                                 <div class="progress-bar progress-bar-striped progress-bar-animated"
-                                    id="progressoLeitura" role="progressbar" aria-valuenow="0" aria-valuemin="0"
-                                    aria-valuemax="100" style="width: 0%;">
+                                     id="progressoLeitura"
+                                     role="progressbar"
+                                     aria-valuenow="0"
+                                     aria-valuemin="0"
+                                     aria-valuemax="100"
+                                     style="width: 0%;">
                                     <span id="textoProgressoBarra">0%</span>
                                 </div>
                             </div>
@@ -657,8 +614,7 @@
                                 <i class="fa-duotone fa-users"></i>
                                 Usuários que Receberam o Alerta
                             </span>
-                            <button class="btn btn-sm btn-verde" id="btnExportarDetalhes" style="display: none;"
-                                data-ejtip="Exportar para Excel">
+                            <button class="btn btn-sm btn-verde" id="btnExportarDetalhes" style="display: none;" data-ejtip="Exportar para Excel">
                                 <i class="fa-duotone fa-file-excel"></i> Exportar Excel
                             </button>
                         </div>
@@ -678,8 +634,7 @@
                                     <tbody id="tabelaUsuarios">
                                         <tr>
                                             <td colspan="6" class="text-center py-4">
-                                                <div class="spinner-border spinner-border-sm text-primary"
-                                                    role="status">
+                                                <div class="spinner-border spinner-border-sm text-primary" role="status">
                                                     <span class="visually-hidden">Carregando...</span>
                                                 </div>
                                                 <span class="ms-2 text-muted">Carregando usuários...</span>
@@ -695,15 +650,21 @@
 
             <div class="modal-footer d-flex justify-content-between">
 
-                <button type="button" class="btn btn-azul form-control" id="btnBaixaAlerta" style="max-width: 250px;"
-                    data-ejtip="Finalizar este alerta">
+                <button type="button"
+                        class="btn btn-azul form-control"
+                        id="btnBaixaAlerta"
+                        style="max-width: 250px;"
+                        data-ejtip="Finalizar este alerta">
                     <i class="fa-duotone fa-file-plus icon-space icon-pulse"></i>
                     Dar Baixa no Alerta
                 </button>
 
                 <div>
-                    <button type="button" class="btn btn-vinho form-control" data-bs-dismiss="modal"
-                        data-ejtip="Fechar Modal" style="max-width: 150px;">
+                    <button type="button"
+                            class="btn btn-vinho form-control"
+                            data-bs-dismiss="modal"
+                            data-ejtip="Fechar Modal"
+                            style="max-width: 150px;">
                         <i class="fa-duotone fa-circle-xmark icon-space"></i>
                         Fechar
                     </button>
@@ -718,142 +679,104 @@
     <script src="~/js/alertas/alertas_gestao.js" asp-append-version="true"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * SISTEMA DE TABS MANUAL - GESTÃO DE ALERTAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Sistema de navegação por tabs sem dependência de Bootstrap JS.
-             * Controla a exibição de Alertas Inativos, Ativos e Meus Alertas.
-             * Carrega DataTables e alertas sob demanda conforme a tab visitada.
-             * @@requires alertas_gestao.js - Funções de carregamento de alertas
-            * @@requires signalr.min.js - Para notificações em tempo real
-                */
-                    (function () {
-                        'use strict';
-
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CONTROLE DE ESTADO DAS TABS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Rastreia quais tabs já foram visitadas para evitar
-                            * reinicialização desnecessária de DataTables
-                                * @@type { Object.< string, boolean>}
-                        */
-                var tabsVisitadas = {
-            tabInativos: false,
-            tabAtivos: false,
-            tabMeusAlertas: false
-        };
-
-                /**
-                 * Alterna entre as tabs de alertas
-                 * @@description Remove estado ativo das outras tabs, esconde seus painéis,
-                 * e ativa a tab selecionada.Carrega alertas ativos sob demanda.
-                 * @@param { string } tabId - ID da tab a ser ativada(tabInativos, tabAtivos, tabMeusAlertas)
-            */
-        function switchTab(tabId) {
-            try {
-                console.log('Trocando para tab:', tabId);
-
-                document.querySelectorAll('.nav-tabs-custom .nav-link').forEach(function (link) {
-                    link.classList.remove('active');
-                });
-
-                document.querySelectorAll('.tab-pane').forEach(function (pane) {
-                    pane.classList.remove('active');
-                });
-
-                var clickedLink = document.querySelector('.nav-link[data-tab="' + tabId + '"]');
-                if (clickedLink) {
-                    clickedLink.classList.add('active');
-                }
-
-                var targetPane = document.getElementById(tabId);
-                if (targetPane) {
-                    targetPane.classList.add('active');
-                }
-
-                console.log('Tab ativada:', tabId);
-
-                if (tabId === 'tabAtivos') {
-
-                    if (typeof carregarAlertasGestao === 'function') {
-                        console.log('Carregando alertas ativos...');
-                        setTimeout(function () {
-                            carregarAlertasGestao();
-                        }, 150);
+
+        (function() {
+            'use strict';
+
+            var tabsVisitadas = {
+                tabInativos: false,
+                tabAtivos: false,
+                tabMeusAlertas: false
+            };
+
+            function switchTab(tabId) {
+                try {
+                    console.log('Trocando para tab:', tabId);
+
+                    document.querySelectorAll('.nav-tabs-custom .nav-link').forEach(function(link) {
+                        link.classList.remove('active');
+                    });
+
+                    document.querySelectorAll('.tab-pane').forEach(function(pane) {
+                        pane.classList.remove('active');
+                    });
+
+                    var clickedLink = document.querySelector('.nav-link[data-tab="' + tabId + '"]');
+                    if (clickedLink) {
+                        clickedLink.classList.add('active');
+                    }
+
+                    var targetPane = document.getElementById(tabId);
+                    if (targetPane) {
+                        targetPane.classList.add('active');
+                    }
+
+                    console.log('Tab ativada:', tabId);
+
+                    if (tabId === 'tabAtivos') {
+
+                        if (typeof carregarAlertasGestao === 'function') {
+                            console.log('Carregando alertas ativos...');
+                            setTimeout(function() {
+                                carregarAlertasGestao();
+                            }, 150);
+                        }
+                    }
+
+                    if (tabsVisitadas.hasOwnProperty(tabId)) {
+                        if (!tabsVisitadas[tabId]) {
+                            console.log('Primeira visita à tab:', tabId);
+                            tabsVisitadas[tabId] = true;
+                        }
+                    }
+
+                } catch (error) {
+                    console.error('Erro ao trocar tab:', error);
+                    if (typeof Alerta !== 'undefined' && typeof Alerta.TratamentoErroComLinha === 'function') {
+                        Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml", "switchTab", error);
                     }
                 }
-
-                if (tabsVisitadas.hasOwnProperty(tabId)) {
-                    if (!tabsVisitadas[tabId]) {
-                        console.log('Primeira visita à tab:', tabId);
-                        tabsVisitadas[tabId] = true;
+            }
+
+            function initTabs() {
+                try {
+                    console.log('Inicializando sistema de tabs manual...');
+
+                    tabsVisitadas.tabInativos = true;
+
+                    document.querySelectorAll('.nav-tabs-custom .nav-link').forEach(function(link) {
+                        link.addEventListener('click', function(e) {
+                            e.preventDefault();
+                            var tabId = this.getAttribute('data-tab');
+                            if (tabId) {
+                                switchTab(tabId);
+                            }
+                        });
+                    });
+
+                    console.log('✅ Sistema de tabs manual inicializado!');
+                    console.log('✅ Event listeners adicionados a', document.querySelectorAll('.nav-tabs-custom .nav-link').length, 'tabs');
+                } catch (error) {
+                    console.error('Erro ao inicializar tabs:', error);
+                    if (typeof Alerta !== 'undefined' && typeof Alerta.TratamentoErroComLinha === 'function') {
+                        Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml", "initTabs", error);
                     }
                 }
-
-            } catch (error) {
-                console.error('Erro ao trocar tab:', error);
-                if (typeof Alerta !== 'undefined' && typeof Alerta.TratamentoErroComLinha === 'function') {
-                    Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml", "switchTab", error);
+            }
+
+            if (document.readyState === 'loading') {
+                document.addEventListener('DOMContentLoaded', initTabs);
+            } else {
+                initTabs();
+            }
+
+            window.addEventListener('load', function() {
+
+                var initialized = document.querySelector('.nav-tabs-custom .nav-link[data-initialized]');
+                if (!initialized) {
+                    initTabs();
                 }
-            }
-        }
-
-                /**
-                 * Inicializa o sistema de tabs manual
-                 * @@description Marca a primeira tab como visitada e adiciona event listeners
-            * a todos os links de navegação para tratar cliques.
-                 * Previne comportamento padrão do link e chama switchTab().
-                 */
-        function initTabs() {
-            try {
-                console.log('Inicializando sistema de tabs manual...');
-
-                tabsVisitadas.tabInativos = true;
-
-                document.querySelectorAll('.nav-tabs-custom .nav-link').forEach(function (link) {
-                    link.addEventListener('click', function (e) {
-                        e.preventDefault();
-                        var tabId = this.getAttribute('data-tab');
-                        if (tabId) {
-                            switchTab(tabId);
-                        }
-                    });
-                });
-
-                console.log('✅ Sistema de tabs manual inicializado!');
-                console.log('✅ Event listeners adicionados a', document.querySelectorAll('.nav-tabs-custom .nav-link').length, 'tabs');
-            } catch (error) {
-                console.error('Erro ao inicializar tabs:', error);
-                if (typeof Alerta !== 'undefined' && typeof Alerta.TratamentoErroComLinha === 'function') {
-                    Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml", "initTabs", error);
-                }
-            }
-        }
-
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * INICIALIZAÇÃO DO SISTEMA DE TABS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Executa initTabs() quando o DOM estiver pronto ou
-            * imediatamente se já estiver carregado.Também adiciona
-                * fallback no evento window.load para garantir inicialização.
-                 */
-
-        if (document.readyState === 'loading') {
-            document.addEventListener('DOMContentLoaded', initTabs);
-        } else {
-            initTabs();
-        }
-
-        window.addEventListener('load', function () {
-
-            var initialized = document.querySelector('.nav-tabs-custom .nav-link[data-initialized]');
-            if (!initialized) {
-                initTabs();
-            }
-        });
-            }) ();
+            });
+        })();
     </script>
 }
```
