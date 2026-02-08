# Pages/Multa/ListaEmpenhosMulta.cshtml

**Mudanca:** GRANDE | **+96** linhas | **-151** linhas

---

```diff
--- JANEIRO: Pages/Multa/ListaEmpenhosMulta.cshtml
+++ ATUAL: Pages/Multa/ListaEmpenhosMulta.cshtml
@@ -6,19 +6,19 @@
 @inject IUnitOfWork _unitOfWork
 
 @{
-@functions {
-    public void OnGet()
-    {
-        FrotiX.Pages.Multa.ListaEmpenhosMultaModel.Initialize(_unitOfWork);
-        ViewData["lstOrgaoAutuante"] = new ListaOrgaoAutuante(_unitOfWork).OrgaoAutuanteList();
+    @functions {
+        public void OnGet()
+        {
+            FrotiX.Pages.Multa.ListaEmpenhosMultaModel.Initialize(_unitOfWork);
+            ViewData["lstOrgaoAutuante"] = new ListaOrgaoAutuante(_unitOfWork).OrgaoAutuanteList();
+        }
     }
-}
-
-ViewData["Title"] = "Empenhos dos Órgãos Autuantes";
-ViewData["PageName"] = "multa_listaempenhosmulta";
-ViewData["Heading"] = "<i class='fa-duotone fa-money-check-alt'></i> Multas: <span class='fw-300'>Empenhos dos Órgãos Autuantes</span>";
-ViewData["Category1"] = "Multas";
-ViewData["PageIcon"] = "fa-duotone fa-money-check-alt";
+
+    ViewData["Title"] = "Empenhos dos Órgãos Autuantes";
+    ViewData["PageName"] = "multa_listaempenhosmulta";
+    ViewData["Heading"] = "<i class='fa-duotone fa-money-check-alt'></i> Multas: <span class='fw-300'>Empenhos dos Órgãos Autuantes</span>";
+    ViewData["Category1"] = "Multas";
+    ViewData["PageIcon"] = "fa-duotone fa-money-check-alt";
 }
 
 @section HeadBlock {
@@ -30,25 +30,11 @@
 
         /* ====== ANIMAÇÃO GLOW FROTIX ====== */
         @@keyframes buttonWiggle {
-            0% {
-                transform: translateY(0) rotate(0deg);
-            }
-
-            25% {
-                transform: translateY(-2px) rotate(-1deg);
-            }
-
-            50% {
-                transform: translateY(-3px) rotate(0deg);
-            }
-
-            75% {
-                transform: translateY(-2px) rotate(1deg);
-            }
-
-            100% {
-                transform: translateY(0) rotate(0deg);
-            }
+            0% { transform: translateY(0) rotate(0deg); }
+            25% { transform: translateY(-2px) rotate(-1deg); }
+            50% { transform: translateY(-3px) rotate(0deg); }
+            75% { transform: translateY(-2px) rotate(1deg); }
+            100% { transform: translateY(0) rotate(0deg); }
         }
 
         /* ====== VARIÁVEIS LOCAIS ====== */
@@ -89,18 +75,15 @@
         }
 
         /* ====== TABELAS ====== */
-        #tblEmpenho,
-        #tblMultas {
+        #tblEmpenho, #tblMultas {
             font-size: 0.85rem;
         }
 
-        #tblEmpenho thead,
-        #tblMultas thead {
+        #tblEmpenho thead, #tblMultas thead {
             background: linear-gradient(180deg, #3D5771 0%, #2d4559 100%);
         }
 
-        #tblEmpenho thead th,
-        #tblMultas thead th {
+        #tblEmpenho thead th, #tblMultas thead th {
             color: #fff !important;
             font-weight: 600;
             font-size: 0.75rem;
@@ -112,21 +95,18 @@
             vertical-align: middle !important;
         }
 
-        #tblEmpenho tbody tr,
-        #tblMultas tbody tr {
+        #tblEmpenho tbody tr, #tblMultas tbody tr {
             transition: background-color 0.15s ease;
         }
 
-        #tblEmpenho tbody tr:hover,
-        #tblMultas tbody tr:hover {
+        #tblEmpenho tbody tr:hover, #tblMultas tbody tr:hover {
             background-color: rgba(50, 93, 136, 0.06) !important;
         }
 
-        #tblEmpenho tbody td,
-        #tblMultas tbody td {
+        #tblEmpenho tbody td, #tblMultas tbody td {
             padding: 0.625rem 0.5rem;
             vertical-align: middle !important;
-            border-color: rgba(0, 0, 0, 0.05) !important;
+            border-color: rgba(0,0,0,0.05) !important;
         }
 
         /* ====== BOTÕES DE AÇÃO NA TABELA - COM GLOW FROTIX ====== */
@@ -169,14 +149,11 @@
             background-color: var(--ftx-empenho-azul) !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
         .ftx-btn-editar:hover:not([aria-disabled="true"]) {
             background-color: var(--ftx-empenho-azul-hover) !important;
             box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
         }
-
-        .ftx-btn-editar:active,
-        .ftx-btn-editar:focus {
+        .ftx-btn-editar:active, .ftx-btn-editar:focus {
             box-shadow: 0 0 0 0.2rem rgba(61, 87, 113, 0.35) !important;
         }
 
@@ -185,14 +162,11 @@
             background-color: var(--ftx-empenho-vinho) !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
         .ftx-btn-apagar:hover:not([aria-disabled="true"]) {
             background-color: var(--ftx-empenho-vinho-hover) !important;
             box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
         }
-
-        .ftx-btn-apagar:active,
-        .ftx-btn-apagar:focus {
+        .ftx-btn-apagar:active, .ftx-btn-apagar:focus {
             box-shadow: 0 0 0 0.2rem rgba(114, 47, 55, 0.25) !important;
         }
 
@@ -201,14 +175,11 @@
             background-color: var(--ftx-empenho-verde) !important;
             box-shadow: 0 0 8px rgba(46, 139, 87, 0.5), 0 2px 4px rgba(46, 139, 87, 0.3) !important;
         }
-
         .ftx-btn-aporte:hover:not([aria-disabled="true"]) {
             background-color: var(--ftx-empenho-verde-hover) !important;
             box-shadow: 0 0 20px rgba(46, 139, 87, 0.8), 0 6px 12px rgba(46, 139, 87, 0.5) !important;
         }
-
-        .ftx-btn-aporte:active,
-        .ftx-btn-aporte:focus {
+        .ftx-btn-aporte:active, .ftx-btn-aporte:focus {
             box-shadow: 0 0 0 0.2rem rgba(46, 139, 87, 0.25) !important;
         }
 
@@ -217,14 +188,11 @@
             background-color: var(--ftx-empenho-laranja) !important;
             box-shadow: 0 0 8px rgba(210, 105, 30, 0.5), 0 2px 4px rgba(210, 105, 30, 0.3) !important;
         }
-
         .ftx-btn-anulacao:hover:not([aria-disabled="true"]) {
             background-color: var(--ftx-empenho-laranja-hover) !important;
             box-shadow: 0 0 20px rgba(210, 105, 30, 0.8), 0 6px 12px rgba(210, 105, 30, 0.5) !important;
         }
-
-        .ftx-btn-anulacao:active,
-        .ftx-btn-anulacao:focus {
+        .ftx-btn-anulacao:active, .ftx-btn-anulacao:focus {
             box-shadow: 0 0 0 0.2rem rgba(210, 105, 30, 0.25) !important;
         }
 
@@ -233,14 +201,11 @@
             background-color: var(--ftx-empenho-roxo) !important;
             box-shadow: 0 0 8px rgba(124, 58, 237, 0.5), 0 2px 4px rgba(124, 58, 237, 0.3) !important;
         }
-
         .ftx-btn-multas:hover:not([aria-disabled="true"]) {
             background-color: var(--ftx-empenho-roxo-hover) !important;
             box-shadow: 0 0 20px rgba(124, 58, 237, 0.8), 0 6px 12px rgba(124, 58, 237, 0.5) !important;
         }
-
-        .ftx-btn-multas:active,
-        .ftx-btn-multas:focus {
+        .ftx-btn-multas:active, .ftx-btn-multas:focus {
             box-shadow: 0 0 0 0.2rem rgba(124, 58, 237, 0.25) !important;
         }
 
@@ -303,7 +268,6 @@
             background-color: var(--ftx-empenho-azul) !important;
             box-shadow: 0 0 8px rgba(61, 87, 113, 0.5), 0 2px 4px rgba(61, 87, 113, 0.3) !important;
         }
-
         .ftx-btn-confirmar:hover {
             background-color: var(--ftx-empenho-azul-hover) !important;
             box-shadow: 0 0 20px rgba(61, 87, 113, 0.8), 0 6px 12px rgba(61, 87, 113, 0.5) !important;
@@ -313,7 +277,6 @@
             background-color: var(--ftx-empenho-vinho) !important;
             box-shadow: 0 0 8px rgba(114, 47, 55, 0.5), 0 2px 4px rgba(114, 47, 55, 0.3) !important;
         }
-
         .ftx-btn-fechar:hover {
             background-color: var(--ftx-empenho-vinho-hover) !important;
             box-shadow: 0 0 20px rgba(114, 47, 55, 0.8), 0 6px 12px rgba(114, 47, 55, 0.5) !important;
@@ -408,10 +371,12 @@
                         </div>
                         <div class="row">
                             <div class="col-12 col-md-6">
-                                <ejs-combobox id="lstOrgaosAutuantes" placeholder="Selecione um Órgão"
-                                    allowFiltering="true" filterType="Contains"
-                                    dataSource="@ViewData["lstOrgaoAutuante"]" popupHeight="250px" width="100%"
-                                    showClearButton="true" change="OrgaoValueChange">
+                                <ejs-combobox id="lstOrgaosAutuantes"
+                                              placeholder="Selecione um Órgão"
+                                              allowFiltering="true" filterType="Contains"
+                                              dataSource="@ViewData["lstOrgaoAutuante"]"
+                                              popupHeight="250px" width="100%" showClearButton="true"
+                                              change="OrgaoValueChange">
                                     <e-combobox-fields text="Descricao" value="Id"></e-combobox-fields>
                                 </ejs-combobox>
                             </div>
@@ -427,7 +392,7 @@
                                     <th>Saldo Inicial</th>
                                     <th>Movimentações</th>
                                     <th>Saldo Atual</th>
-                                    <th>Ações</th>
+                                    <th>Ação</th>
                                     <th>Mov. Financeiras</th>
                                     <th></th>
                                 </tr>
@@ -466,8 +431,7 @@
                         <div class="col-12 col-md-7">
                             <div class="form-group">
                                 <label class="ftx-form-label">Descrição</label>
-                                <input id="txtDescricao" class="form-control ftx-form-control"
-                                    placeholder="Descrição do aporte" />
+                                <input id="txtDescricao" class="form-control ftx-form-control" placeholder="Descrição do aporte" />
                             </div>
                         </div>
                     </div>
@@ -477,7 +441,8 @@
                             <div class="form-group">
                                 <label class="ftx-form-label">Valor do Aporte</label>
                                 <input id="txtValor" class="form-control ftx-form-control" style="text-align:right;"
-                                    placeholder="0,00" onkeypress="return moeda(this,'.',',',event)" />
+                                       placeholder="0,00"
+                                       onkeypress="return moeda(this,'.',',',event)" />
                             </div>
                         </div>
                     </div>
@@ -485,13 +450,11 @@
             </div>
 
             <div class="ftx-modal-footer">
-                <button id="btnAportar" class="ftx-btn-modal ftx-btn-confirmar" type="button"
-                    data-ejtip="Confirmar aporte">
+                <button id="btnAportar" class="ftx-btn-modal ftx-btn-confirmar" type="button" data-ejtip="Confirmar aporte">
                     <i class="fa-duotone fa-floppy-disk icon-pulse"></i>
                     Aportar
                 </button>
-                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal"
-                    data-ejtip="Fechar modal">
+                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal" data-ejtip="Fechar modal">
                     <i class="fa-duotone fa-xmark"></i>
                     Fechar
                 </button>
@@ -525,8 +488,7 @@
                         <div class="col-12 col-md-7">
                             <div class="form-group">
                                 <label class="ftx-form-label">Descrição</label>
-                                <input id="txtDescricaoanulacao" class="form-control ftx-form-control"
-                                    placeholder="Descrição da anulação" />
+                                <input id="txtDescricaoanulacao" class="form-control ftx-form-control" placeholder="Descrição da anulação" />
                             </div>
                         </div>
                     </div>
@@ -535,9 +497,9 @@
                         <div class="col-12 col-md-4">
                             <div class="form-group">
                                 <label class="ftx-form-label">Valor da Anulação</label>
-                                <input id="txtValoranulacao" class="form-control ftx-form-control"
-                                    style="text-align:right;" placeholder="0,00"
-                                    onkeypress="return moeda(this,'.',',',event)" />
+                                <input id="txtValoranulacao" class="form-control ftx-form-control" style="text-align:right;"
+                                       placeholder="0,00"
+                                       onkeypress="return moeda(this,'.',',',event)" />
                             </div>
                         </div>
                     </div>
@@ -545,13 +507,11 @@
             </div>
 
             <div class="ftx-modal-footer">
-                <button id="btnAnular" class="ftx-btn-modal ftx-btn-confirmar" type="button"
-                    data-ejtip="Confirmar anulação">
+                <button id="btnAnular" class="ftx-btn-modal ftx-btn-confirmar" type="button" data-ejtip="Confirmar anulação">
                     <i class="fa-duotone fa-floppy-disk icon-pulse"></i>
                     Anular
                 </button>
-                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal"
-                    data-ejtip="Fechar modal">
+                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal" data-ejtip="Fechar modal">
                     <i class="fa-duotone fa-xmark"></i>
                     Fechar
                 </button>
@@ -581,7 +541,7 @@
                             <th>Local</th>
                             <th>Data Pagamento</th>
                             <th>Valor Pago</th>
-                            <th>Ações</th>
+                            <th>Ação</th>
                             <th></th>
                         </tr>
                     </thead>
@@ -590,8 +550,7 @@
             </div>
 
             <div class="ftx-modal-footer">
-                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal"
-                    data-ejtip="Fechar modal">
+                <button type="button" class="ftx-btn-modal ftx-btn-fechar" data-bs-dismiss="modal" data-ejtip="Fechar modal">
                     <i class="fa-duotone fa-xmark"></i>
                     Fechar
                 </button>
@@ -602,27 +561,11 @@
 </div>
 
 @section ScriptsBlock {
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * LISTA EMPENHOS DE MULTA - GERENCIAMENTO ORÇAMENTÁRIO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Gerencia empenhos de pagamento de multas por órgão autuante.
-             * Exibe saldos, movimentações e permite CRUD de empenhos.
-             * @@requires jQuery DataTables, Syncfusion ComboBox, Bootstrap 5
-            * @@file Multa / ListaEmpenhosMulta.cshtml
-            */
-
-            /**
-             * Handler de mudança do ComboBox de Órgão Autuante
-             * @@description Carrega DataTable filtrada por órgão selecionado.
-             * Exibe empenhos com saldos inicial, movimentação e atual.
-             */
+
         function OrgaoValueChange() {
             try {
                 const table = $('#tblEmpenho').DataTable();
@@ -664,23 +607,23 @@
                             render: function (data) {
                                 try {
                                     return `
-                                            <div class="text-center" style="white-space: nowrap;">
-                                                <a href="/Multa/UpsertEmpenhosMulta?id=${data}"
-                                                   class="ftx-btn-icon ftx-btn-editar"
-                                                   data-ejtip="Editar Empenho">
-                                                    <i class="fa-duotone fa-pen-to-square"></i>
-                                                </a>
-                                                <a class="ftx-btn-icon ftx-btn-apagar btn-delete"
-                                                   data-ejtip="Excluir Empenho"
-                                                   data-id="${data}">
-                                                    <i class="fa-duotone fa-trash-can"></i>
-                                                </a>
-                                                <a class="ftx-btn-icon ftx-btn-multas btn-ver-multas"
-                                                   data-ejtip="Multas do Empenho"
-                                                   data-id="${data}">
-                                                    <i class="fa-duotone fa-file-invoice-dollar"></i>
-                                                </a>
-                                            </div>`;
+                                        <div class="text-center" style="white-space: nowrap;">
+                                            <a href="/Multa/UpsertEmpenhosMulta?id=${data}"
+                                               class="ftx-btn-icon ftx-btn-editar"
+                                               data-ejtip="Editar Empenho">
+                                                <i class="fa-duotone fa-pen-to-square"></i>
+                                            </a>
+                                            <a class="ftx-btn-icon ftx-btn-apagar btn-delete"
+                                               data-ejtip="Excluir Empenho"
+                                               data-id="${data}">
+                                                <i class="fa-duotone fa-trash-can"></i>
+                                            </a>
+                                            <a class="ftx-btn-icon ftx-btn-multas btn-ver-multas"
+                                               data-ejtip="Multas do Empenho"
+                                               data-id="${data}">
+                                                <i class="fa-duotone fa-file-invoice-dollar"></i>
+                                            </a>
+                                        </div>`;
                                 }
                                 catch (error) {
                                     Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml", "render.acoes", error);
@@ -693,18 +636,18 @@
                             render: function (data) {
                                 try {
                                     return `
-                                            <div class="text-center" style="white-space: nowrap;">
-                                                <a class="ftx-btn-icon ftx-btn-aporte btn-abrir-aporte"
-                                                   data-ejtip="Aportar Valor"
-                                                   data-id="${data}">
-                                                    <i class="fa-duotone fa-circle-plus"></i>
-                                                </a>
-                                                <a class="ftx-btn-icon ftx-btn-anulacao btn-abrir-anulacao"
-                                                   data-ejtip="Anular Valor"
-                                                   data-id="${data}">
-                                                    <i class="fa-duotone fa-circle-minus"></i>
-                                                </a>
-                                            </div>`;
+                                        <div class="text-center" style="white-space: nowrap;">
+                                            <a class="ftx-btn-icon ftx-btn-aporte btn-abrir-aporte"
+                                               data-ejtip="Aportar Valor"
+                                               data-id="${data}">
+                                                <i class="fa-duotone fa-circle-plus"></i>
+                                            </a>
+                                            <a class="ftx-btn-icon ftx-btn-anulacao btn-abrir-anulacao"
+                                               data-ejtip="Anular Valor"
+                                               data-id="${data}">
+                                                <i class="fa-duotone fa-circle-minus"></i>
+                                            </a>
+                                        </div>`;
                                 }
                                 catch (error) {
                                     Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml", "render.movimentacoes", error);
@@ -943,23 +886,23 @@
                                     render: function (data) {
                                         try {
                                             return `
-                                                    <div class="text-center" style="white-space: nowrap;">
-                                                        <a href="/Multa/UpsertAutuacao?id=${data}"
-                                                           class="ftx-btn-icon ftx-btn-editar"
-                                                           data-ejtip="Editar Autuação">
-                                                            <i class="fa-duotone fa-file-lines"></i>
-                                                        </a>
-                                                        <a href="/Multa/UpsertPenalidade?id=${data}"
-                                                           class="ftx-btn-icon ftx-btn-multas"
-                                                           data-ejtip="Editar Penalidade">
-                                                            <i class="fa-duotone fa-file-invoice-dollar"></i>
-                                                        </a>
-                                                        <a class="ftx-btn-icon ftx-btn-apagar btn-delete-multa"
-                                                           data-ejtip="Excluir Multa"
-                                                           data-id="${data}">
-                                                            <i class="fa-duotone fa-trash-can"></i>
-                                                        </a>
-                                                    </div>`;
+                                                <div class="text-center" style="white-space: nowrap;">
+                                                    <a href="/Multa/UpsertAutuacao?id=${data}"
+                                                       class="ftx-btn-icon ftx-btn-editar"
+                                                       data-ejtip="Editar Autuação">
+                                                        <i class="fa-duotone fa-file-lines"></i>
+                                                    </a>
+                                                    <a href="/Multa/UpsertPenalidade?id=${data}"
+                                                       class="ftx-btn-icon ftx-btn-multas"
+                                                       data-ejtip="Editar Penalidade">
+                                                        <i class="fa-duotone fa-file-invoice-dollar"></i>
+                                                    </a>
+                                                    <a class="ftx-btn-icon ftx-btn-apagar btn-delete-multa"
+                                                       data-ejtip="Excluir Multa"
+                                                       data-id="${data}">
+                                                        <i class="fa-duotone fa-trash-can"></i>
+                                                    </a>
+                                                </div>`;
                                         }
                                         catch (error) {
                                             Alerta.TratamentoErroComLinha("ListaEmpenhosMulta.cshtml", "render.multas", error);
@@ -1099,21 +1042,7 @@
     </script>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * MÁSCARA DE MOEDA PT-BR
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Aplica máscara de moeda brasileira (R$) ao input
-             * @@param { HTMLInputElement } a - Campo de input
-            * @@param { string } e - Separador de milhar(.)
-                * @@param { string } r - Separador decimal(,)
-                    * @@param { Event } t - Evento de teclado
-                        * @@returns { boolean } True para permitir input, false para bloquear
-                            * @@description Formata valores como 1.234, 56 em tempo real
-                                */
+
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = 0, j = 0, u = 0, tamanho2 = 0, ajd2 = "", l = "";
```
