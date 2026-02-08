# Pages/Viagens/ListaEventos.cshtml

**Mudanca:** GRANDE | **+84** linhas | **-128** linhas

---

```diff
--- JANEIRO: Pages/Viagens/ListaEventos.cshtml
+++ ATUAL: Pages/Viagens/ListaEventos.cshtml
@@ -22,11 +22,9 @@
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
@@ -67,15 +65,13 @@
     #tblEventos .text-center .btn.disabled {
         opacity: 0.5 !important;
         cursor: not-allowed !important;
-        pointer-events: auto !important;
-        /* Permite tooltip mesmo desabilitado */
+        pointer-events: auto !important; /* Permite tooltip mesmo desabilitado */
     }
 
     #tblEventos .btn.disabled:hover,
     #tblEventos a.btn.disabled:hover {
         background-color: #6c757d !important;
-        transform: none !important;
-        /* Remove efeito de hover */
+        transform: none !important; /* Remove efeito de hover */
     }
 
     /* ======== MODAIS DE CUSTO - 20% MENORES ======== */
@@ -142,13 +138,13 @@
     #tblViagensModal .btn-detalhes-viagem,
     .btn-detalhes-viagem.fundo-roxo {
         background-color: #6B2FA2 !important;
-        box-shadow: 0 0 6px rgba(107, 47, 162, .4), 0 2px 4px rgba(107, 47, 162, .25) !important;
+        box-shadow: 0 0 6px rgba(107,47,162,.4), 0 2px 4px rgba(107,47,162,.25) !important;
     }
 
     #tblViagensModal .btn-detalhes-viagem:hover,
     .btn-detalhes-viagem.fundo-roxo:hover {
         background-color: #5A199B !important;
-        box-shadow: 0 0 18px rgba(107, 47, 162, .6), 0 4px 10px rgba(107, 47, 162, .4) !important;
+        box-shadow: 0 0 18px rgba(107,47,162,.6), 0 4px 10px rgba(107,47,162,.4) !important;
     }
 
     /* ======== ESTILOS FrotiX - TABELA ======== */
@@ -233,8 +229,7 @@
                     <div class="panel-content">
                         <div class="box-body">
                             <div id="divEventos" class="mt-3">
-                                <table id="tblEventos" class="table table-bordered table-striped mt-2 ftx-table"
-                                    width="100%">
+                                <table id="tblEventos" class="table table-bordered table-striped mt-2 ftx-table" width="100%">
                                     <thead>
                                         <tr>
                                             <th>Evento</th>
@@ -246,7 +241,7 @@
                                             <th>Setor Requisitante</th>
                                             <th>Custo</th>
                                             <th>Status</th>
-                                            <th>Ações</th>
+                                            <th>Ação</th>
                                         </tr>
                                     </thead>
                                     <tbody></tbody>
@@ -266,8 +261,7 @@
                     <h5 class="modal-title" id="modalCustoLabel">
                         <i class="fa-duotone fa-money-check-dollar"></i> Custo do Evento
                     </h5>
-                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                        aria-label="Fechar"></button>
+                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                 </div>
                 <div class="modal-body">
 
@@ -319,8 +313,9 @@
                                     <i class="fa-duotone fa-car"></i> Total de Viagens
                                 </label>
                                 <input type="text" id="totalViagensModal"
-                                    class="form-control text-center font-weight-bold" readonly value="0"
-                                    style="background-color: #e3f2fd;" />
+                                       class="form-control text-center font-weight-bold"
+                                       readonly value="0"
+                                       style="background-color: #e3f2fd;" />
                             </div>
                         </div>
 
@@ -330,8 +325,9 @@
                                     <i class="fa-duotone fa-dollar-sign"></i> Valor Total do Evento
                                 </label>
                                 <input type="text" id="custoTotalViagensModal"
-                                    class="form-control text-center font-weight-bold" readonly value="R$ 0,00"
-                                    style="background-color: #e8f5e9;" />
+                                       class="form-control text-center font-weight-bold"
+                                       readonly value="R$ 0,00"
+                                       style="background-color: #e8f5e9;" />
                             </div>
                         </div>
 
@@ -341,8 +337,9 @@
                                     <i class="fa-duotone fa-chart-line"></i> Custo Médio por Viagem
                                 </label>
                                 <input type="text" id="custoMedioViagemModal"
-                                    class="form-control text-center font-weight-bold" readonly value="R$ 0,00"
-                                    style="background-color: #fff3e0;" />
+                                       class="form-control text-center font-weight-bold"
+                                       readonly value="R$ 0,00"
+                                       style="background-color: #fff3e0;" />
                             </div>
                         </div>
                     </div>
@@ -364,8 +361,7 @@
                     <h5 class="modal-title text-white" id="modalDetalhesLabel">
                         <i class="fa-duotone fa-money-check-dollar"></i> Detalhamento de Custos da Viagem
                     </h5>
-                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                        aria-label="Fechar"></button>
+                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                 </div>
                 <div class="modal-body">
 
@@ -443,8 +439,7 @@
 
                     <div class="row">
                         <div class="col-12">
-                            <div class="card text-white"
-                                style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
+                            <div class="card text-white" style="background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);">
                                 <div class="card-body text-center py-4">
                                     <i class="fa-duotone fa-sack-dollar fa-3x mb-2"></i>
                                     <h5 class="mb-2">Custo Total da Viagem</h5>
@@ -465,12 +460,11 @@
         </div>
     </div>
 
-    <div id="loadingOverlayEventos" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
+    <div id="loadingOverlayEventos" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: flex;">
         <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-            <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-                style="display: block;" />
+            <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
             <div class="ftx-loading-bar"></div>
-            <div class="ftx-loading-text" id="loadingMessage">Carregando Eventos...</div>
+            <div class="ftx-loading-text" id="loadingMessage">Carregando Página...</div>
             <div class="ftx-loading-subtext">Aguarde, por favor</div>
         </div>
     </div>
@@ -479,16 +473,7 @@
 
 @section ScriptsBlock
 {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
-
-    <link href="https://cdn.kendostatic.com/2022.1.412/styles/kendo.default-main.min.css" rel="stylesheet"
-        type="text/css" />
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/jszip.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.all.min.js"></script>
-    <script src="https://cdn.kendostatic.com/2022.1.412/js/kendo.aspnetmvc.min.js"></script>
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
     <script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.js"></script>
     <script>
         window.pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.2.2/pdf.worker.js';
@@ -499,6 +484,7 @@
     <script>
         $(document).ready(function () {
             try {
+                mostrarLoading('Carregando Página...');
                 ListaTodosEventos();
 
                 $(document).on('click', '.btn-apagar', function () {
@@ -627,36 +613,16 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LISTA DE EVENTOS - FUNÇÕES DE LOADING E LISTAGEM
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções de controle de overlay e listagem de eventos com
-            * paginação server - side via DataTables.
-             * @@requires jQuery, DataTables, Bootstrap
-                * @@file ListaEventos.cshtml
-                */
-
-            /**
-             * Exibe o overlay de loading padrão FrotiX
-             * @@param { string } [mensagem = 'Carregando Eventos...'] - Texto exibido durante o loading
-            */
+
         function mostrarLoading(mensagem = 'Carregando Eventos...') {
             $('#loadingMessage').text(mensagem);
             $('#loadingOverlayEventos').css('display', 'flex');
         }
 
-        /**
-         * Oculta o overlay de loading padrão FrotiX
-         */
         function esconderLoading() {
             $('#loadingOverlayEventos').css('display', 'none');
         }
 
-            /**
-             * Inicializa e recarrega a lista de eventos com DataTable server-side
-             * @@description Destrói DataTable existente e recria com paginação server - side
-            */
         function ListaTodosEventos() {
             try {
 
@@ -703,13 +669,13 @@
                             targets: 4, className: "text-left", width: "18%",
                             render: function (data, type, full) {
                                 return `
-                                        <div class="text-center">
-                                            <a data-ejtip="&#129489; (${full.nomeRequisitanteHTML})"
-                                               style="cursor:pointer;"
-                                               data-id='${data}'>
-                                               ${full.nomeSetor}
-                                            </a>
-                                        </div>`;
+                                    <div class="text-center">
+                                        <a data-ejtip="&#129489; (${full.nomeRequisitanteHTML})"
+                                           style="cursor:pointer;"
+                                           data-id='${data}'>
+                                           ${full.nomeSetor}
+                                        </a>
+                                    </div>`;
                             }
                         },
                         { targets: 5, className: "text-right", width: "7%" },
@@ -721,6 +687,26 @@
                         url: "/api/viagem/listaeventos",
                         type: "GET",
                         datatype: "json",
+                        data: function(d) {
+
+                            if (d.order && d.order.length > 0) {
+                                return {
+                                    draw: d.draw,
+                                    start: d.start,
+                                    length: d.length,
+                                    orderColumn: d.order[0].column,
+                                    orderDir: d.order[0].dir
+                                };
+                            }
+
+                            return {
+                                draw: d.draw,
+                                start: d.start,
+                                length: d.length,
+                                orderColumn: 1,
+                                orderDir: 'desc'
+                            };
+                        },
                         error: function (xhr, error, thrown) {
                             try {
 
@@ -732,7 +718,7 @@
                             }
                         }
                     },
-                    initComplete: function () {
+                    initComplete: function() {
 
                         esconderLoading();
                     },
@@ -788,24 +774,23 @@
                         },
                         {
                             data: "status",
-                            orderable: false,
                             searchable: false,
                             render: function (data, type, row) {
                                 try {
                                     if (data === 1) {
                                         return `<a href="javascript:void(0)"
-                                                       class="updateStatusEvento btn btn-verde btn-xs text-white"
-                                                       data-url="/api/ViagemEvento/UpdateStatusEvento?Id=${row.eventoId}"
-                                                       data-ejtip="Clique para Inativar">
-                                                        <i class="fa-duotone fa-circle-check me-1"></i>Ativo
-                                                    </a>`;
+                                                   class="updateStatusEvento btn btn-verde btn-xs text-white"
+                                                   data-url="/api/ViagemEvento/UpdateStatusEvento?Id=${row.eventoId}"
+                                                   data-ejtip="Clique para Inativar">
+                                                    <i class="fa-duotone fa-circle-check me-1"></i>Ativo
+                                                </a>`;
                                     } else {
                                         return `<a href="javascript:void(0)"
-                                                       class="updateStatusEvento btn fundo-cinza btn-xs text-white"
-                                                       data-url="/api/ViagemEvento/UpdateStatusEvento?Id=${row.eventoId}"
-                                                       data-ejtip="Clique para Ativar">
-                                                        <i class="fa-duotone fa-circle-xmark me-1"></i>Inativo
-                                                    </a>`;
+                                                   class="updateStatusEvento btn fundo-cinza btn-xs text-white"
+                                                   data-url="/api/ViagemEvento/UpdateStatusEvento?Id=${row.eventoId}"
+                                                   data-ejtip="Clique para Ativar">
+                                                    <i class="fa-duotone fa-circle-xmark me-1"></i>Inativo
+                                                </a>`;
                                     }
                                 } catch (error) {
                                     return '<span class="badge bg-secondary">-</span>';
@@ -829,23 +814,23 @@
                                     var btnApagarDataId = temViagens ? "" : `data-id="${data}"`;
 
                                     return `
-                                        <div class="text-center">
-                                            <a href="/Viagens/UpsertEvento?id=${data}"
-                                               class="btn btn-azul text-white btn-icon-28"
-                                               data-ejtip="Editar Evento">
-                                                <i class="fa-duotone fa-pen-to-square"></i>
-                                            </a>
-                                            <a class="btn btn-foto text-white btn-icon-28 btn-custo-evento"
-                                               data-ejtip="Ver Custo do Evento"
-                                               data-id="${data}">
-                                                <i class="fa-duotone fa-money-check-dollar"></i>
-                                            </a>
-                                            <a class="${btnApagarClasse}"
-                                               data-ejtip="${btnApagarTooltip}"
-                                               ${btnApagarDataId}>
-                                                <i class="fa-duotone fa-trash"></i>
-                                            </a>
-                                        </div>`;
+                                    <div class="text-center">
+                                        <a href="/Viagens/UpsertEvento?id=${data}"
+                                           class="btn btn-azul text-white btn-icon-28"
+                                           data-ejtip="Editar Evento">
+                                            <i class="fa-duotone fa-pen-to-square"></i>
+                                        </a>
+                                        <a class="btn btn-foto text-white btn-icon-28 btn-custo-evento"
+                                           data-ejtip="Ver Custo do Evento"
+                                           data-id="${data}">
+                                            <i class="fa-duotone fa-money-check-dollar"></i>
+                                        </a>
+                                        <a class="${btnApagarClasse}"
+                                           data-ejtip="${btnApagarTooltip}"
+                                           ${btnApagarDataId}>
+                                            <i class="fa-duotone fa-trash"></i>
+                                        </a>
+                                    </div>`;
                                 } catch (error) {
                                     Alerta.TratamentoErroComLinha("ListaEventos.cshtml", "render.acao", error);
                                     return '';
@@ -974,14 +959,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MODAL DE CUSTO DO EVENTO - CONTROLE E ESTATÍSTICAS
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Funções para exibição de detalhes de custo do evento,
-             * tabela de viagens e estatísticas de valores.
-             */
-
         let tabelaViagensModal = null;
         let eventoIdAtual = null;
 
@@ -999,11 +976,6 @@
             }
         });
 
-            /**
-             * Carrega os dados básicos do evento via AJAX
-             * @@param { number | string } eventoId - ID do evento a ser carregado
-            * @@description Atualiza título e período do evento no modal
-                */
         function carregarDadosEvento(eventoId) {
             try {
                 $.ajax({
@@ -1042,10 +1014,6 @@
             }
         }
 
-            /**
-             * Inicializa a tabela de viagens dentro do modal de custo
-             * @@description Cria DataTable para exibição das viagens associadas ao evento
-            */
         function inicializarTabelaViagensModal() {
             try {
                 tabelaViagensModal = $('#tblViagensModal').DataTable({
@@ -1069,7 +1037,7 @@
 
                                 esconderLoading();
 
-                                setTimeout(function () {
+                                setTimeout(function() {
                                     if ($('#modalCusto').hasClass('show') && !$('.modal-backdrop').length) {
                                         $('body').append('<div class="modal-backdrop fade show"></div>');
                                     }
@@ -1084,7 +1052,7 @@
 
                                 esconderLoading();
 
-                                setTimeout(function () {
+                                setTimeout(function() {
                                     if ($('#modalCusto').hasClass('show') && !$('.modal-backdrop').length) {
                                         $('body').append('<div class="modal-backdrop fade show"></div>');
                                     }
@@ -1186,12 +1154,12 @@
                             orderable: false,
                             render: function (data, type, row) {
                                 return `
-                                        <a class="btn fundo-roxo text-white btn-icon-28 btn-detalhes-viagem"
-                                           data-id="${data}"
-                                           data-ejtip="Detalhamento de Custos">
-                                            <i class="fa-duotone fa-file-invoice-dollar"></i>
-                                        </a>
-                                    `;
+                                    <a class="btn fundo-roxo text-white btn-icon-28 btn-detalhes-viagem"
+                                       data-id="${data}"
+                                       data-ejtip="Detalhamento de Custos">
+                                        <i class="fa-duotone fa-file-invoice-dollar"></i>
+                                    </a>
+                                `;
                             }
                         }
                     ],
@@ -1219,10 +1187,6 @@
             }
         }
 
-            /**
-             * Carrega as estatísticas de custo das viagens do evento
-             * @@description Obtém total de viagens, custo total e custo médio via API
-            */
         function carregarEstatisticasViagensModal() {
             try {
                 $.ajax({
@@ -1268,11 +1232,6 @@
             }
         }
 
-            /**
-             * Formata um valor numérico para moeda brasileira (R$)
-             * @@param { number } valor - Valor numérico a ser formatado
-            * @@returns { string } Valor formatado como moeda(ex: "R$ 1.234,56")
-                */
         function formatarMoeda(valor) {
             try {
                 if (!valor && valor !== 0) return "R$ 0,00";
@@ -1285,11 +1244,6 @@
             }
         }
 
-            /**
-             * Formata uma data ISO para o padrão brasileiro (DD/MM/YYYY)
-             * @@param { string } data - Data em formato ISO ou string válida
-            * @@returns { string } Data formatada ou '-' se inválida
-                */
         function formatarData(data) {
             try {
                 if (!data) return '-';
```
