# Pages/NotaFiscal/Index.cshtml

**Mudanca:** GRANDE | **+41** linhas | **-149** linhas

---

```diff
--- JANEIRO: Pages/NotaFiscal/Index.cshtml
+++ ATUAL: Pages/NotaFiscal/Index.cshtml
@@ -11,7 +11,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -125,7 +125,7 @@
             color: #ffffff !important;
             border: none !important;
             font-weight: 700 !important;
-            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3) !important;
+            text-shadow: 0 1px 2px rgba(0,0,0,0.3) !important;
         }
 
         .ftx-datatable-wrapper .dataTables_paginate .paginate_button:hover:not(.current):not(.disabled) {
@@ -276,39 +276,21 @@
         }
 
         @@keyframes ftxHeaderGradientShift {
-            0% {
-                background-position: 0% 50%;
-            }
-
-            50% {
-                background-position: 100% 50%;
-            }
-
-            100% {
-                background-position: 0% 50%;
-            }
+            0% { background-position: 0% 50%; }
+            50% { background-position: 100% 50%; }
+            100% { background-position: 0% 50%; }
         }
 
         @@keyframes ftxHeaderShine {
-
-            0%,
-            100% {
-                left: -100%;
-            }
-
-            50% {
-                left: 100%;
-            }
+            0%, 100% { left: -100%; }
+            50% { left: 100%; }
         }
 
         @@keyframes ftxHeaderIconPulse {
-
-            0%,
-            100% {
+            0%, 100% {
                 transform: scale(1);
                 filter: drop-shadow(0 2px 4px rgba(0, 0, 0, 0.3));
             }
-
             50% {
                 transform: scale(1.08);
                 filter: drop-shadow(0 3px 6px rgba(0, 0, 0, 0.4));
@@ -451,8 +433,7 @@
                                             <label class="ftx-label">
                                                 <i class="fa-duotone fa-file-signature"></i> Contratos
                                             </label>
-                                            <select id="ListaContratos" name="ListaContratos"
-                                                class="form-select form-select-sm">
+                                            <select id="ListaContratos" name="ListaContratos" class="form-select form-select-sm">
                                                 <option value="">-- Selecione um Contrato --</option>
                                             </select>
                                         </div>
@@ -460,8 +441,7 @@
                                             <label class="ftx-label">
                                                 <i class="fa-duotone fa-money-check-dollar"></i> Empenhos
                                             </label>
-                                            <select id="ListaEmpenhosContrato" name="ListaEmpenhosContrato"
-                                                class="form-select form-select-sm">
+                                            <select id="ListaEmpenhosContrato" name="ListaEmpenhosContrato" class="form-select form-select-sm">
                                                 <option value="">-- Selecione um Empenho --</option>
                                             </select>
                                         </div>
@@ -481,8 +461,7 @@
                                         </div>
                                         <div class="col-md-5">
                                             <label class="ftx-label">
-                                                <i class="fa-duotone fa-clipboard-list-check"></i> Atas de Registro de
-                                                Preço
+                                                <i class="fa-duotone fa-clipboard-list-check"></i> Atas de Registro de Preço
                                             </label>
                                             <select id="ListaAtas" name="ListaAtas" class="form-select form-select-sm">
                                                 <option value="">-- Selecione uma Ata --</option>
@@ -492,8 +471,7 @@
                                             <label class="ftx-label">
                                                 <i class="fa-duotone fa-money-check-dollar"></i> Empenhos
                                             </label>
-                                            <select id="ListaEmpenhosAta" name="ListaEmpenhosAta"
-                                                class="form-select form-select-sm">
+                                            <select id="ListaEmpenhosAta" name="ListaEmpenhosAta" class="form-select form-select-sm">
                                                 <option value="">-- Selecione um Empenho --</option>
                                             </select>
                                         </div>
@@ -536,8 +514,7 @@
                     <i class="fa-duotone fa-file-invoice-dollar"></i>
                     <span>Glosar Valores da Nota</span>
                 </h5>
-                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                    aria-label="Fechar"></button>
+                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
             </div>
 
             <div class="modal-body">
@@ -559,16 +536,14 @@
                         <p class="mb-2"><strong>O que deseja fazer com a nova glosa?</strong></p>
                         <div class="d-flex gap-3">
                             <div class="form-check">
-                                <input class="form-check-input" type="radio" name="rdModoGlosa" id="rdSomar"
-                                    value="somar">
+                                <input class="form-check-input" type="radio" name="rdModoGlosa" id="rdSomar" value="somar">
                                 <label class="form-check-label" for="rdSomar">
                                     <i class="fa-duotone fa-plus-circle text-success me-1"></i>
                                     <strong>Somar</strong>&nbsp;à glosa existente
                                 </label>
                             </div>
                             <div class="form-check">
-                                <input class="form-check-input" type="radio" name="rdModoGlosa" id="rdSubstituir"
-                                    value="substituir" checked>
+                                <input class="form-check-input" type="radio" name="rdModoGlosa" id="rdSubstituir" value="substituir" checked>
                                 <label class="form-check-label" for="rdSubstituir">
                                     <i class="fa-duotone fa-arrows-rotate text-primary me-1"></i>
                                     <strong>Substituir</strong>&nbsp;a glosa existente
@@ -582,16 +557,17 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-text"></i> Motivo da Glosa
                             </label>
-                            <input id="txtMotivoGlosaModal" class="form-control ftx-form-control"
-                                placeholder="Informe o motivo da glosa..." />
+                            <input id="txtMotivoGlosaModal" class="form-control ftx-form-control" placeholder="Informe o motivo da glosa..." />
                         </div>
                         <div class="col-md-4">
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-coins"></i> Valor da Glosa
                             </label>
-                            <input id="txtValorGlosaModal" class="form-control ftx-form-control"
-                                style="text-align: right; font-weight: 600; color: #722f37;"
-                                onkeypress="return moeda(this,'.',',',event)" placeholder="0,00" />
+                            <input id="txtValorGlosaModal"
+                                   class="form-control ftx-form-control"
+                                   style="text-align: right; font-weight: 600; color: #722f37;"
+                                   onkeypress="return moeda(this,'.',',',event)"
+                                   placeholder="0,00" />
                         </div>
                     </div>
 
@@ -621,24 +597,6 @@
     <script src="~/js/cadastros/notafiscal.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * NOTAS FISCAIS - LISTAGEM E GESTÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Sistema de gerenciamento de notas fiscais com DataTables.
-         * Suporta filtragem por contrato/ata, vinculação a empenhos e glosas.
-         * @@requires jQuery DataTables, Bootstrap 5, Alerta
-         * @@file NotaFiscal/Index.cshtml
-         */
-
-        /**
-         * Aplica máscara de moeda brasileira (R$) ao input
-         * @@param {HTMLInputElement} a - Campo de input
-         * @@param {string} e - Separador de milhar (.)
-         * @@param {string} r - Separador decimal (,)
-         * @@param {Event} t - Evento de teclado
-         * @@returns {boolean} True para permitir input, false para bloquear
-         */
         function moeda(a, e, r, t) {
             try {
                 let n = "",
@@ -686,40 +644,23 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * RENDERIZAÇÃO DE BOTÕES DE AÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Renderiza os botões de ação (editar, excluir, glosar) para cada linha da tabela
-         * @@param {number|string} data - ID da Nota Fiscal
-         * @@returns {string} HTML dos botões de ação formatados
-         */
         function renderActionButtons(data) {
             return `<div class="ftx-actions">
-                                <a href="/NotaFiscal/Upsert?id=${data}"
-                                   class="btn btn-icon-28 btn-azul text-white">
-                                    <i class="fa-duotone fa-pen-to-square"></i>
-                                </a>
-                                <a class="btn btn-icon-28 btn-vinho btn-delete text-white"
-                                   data-id="${data}">
-                                    <i class="fa-duotone fa-trash-can"></i>
-                                </a>
-                                <a class="btn btn-icon-28 btn-terracota btn-glosa text-white"
-                                   data-id="${data}">
-                                    <i class="fa-duotone fa-file-invoice-dollar"></i>
-                                </a>
-                            </div>`;
-        }
-
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * CONFIGURAÇÃO DO DATATABLE
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Retorna objeto de configuração padrão para DataTables de notas fiscais
-         * @@param {string} url - URL da API para buscar dados via AJAX
-         * @@param {Object} dataParam - Parâmetros adicionais para enviar na requisição
-         * @@returns {Object} Objeto de configuração do DataTable
-         */
+                        <a href="/NotaFiscal/Upsert?id=${data}"
+                           class="btn btn-icon-28 btn-azul text-white">
+                            <i class="fa-duotone fa-pen-to-square"></i>
+                        </a>
+                        <a class="btn btn-icon-28 btn-vinho btn-delete text-white"
+                           data-id="${data}">
+                            <i class="fa-duotone fa-trash-can"></i>
+                        </a>
+                        <a class="btn btn-icon-28 btn-terracota btn-glosa text-white"
+                           data-id="${data}">
+                            <i class="fa-duotone fa-file-invoice-dollar"></i>
+                        </a>
+                    </div>`;
+        }
+
         function getDataTableConfig(url, dataParam) {
             return {
                 columnDefs: [
@@ -826,14 +767,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE CONTRATOS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de contratos por status via AJAX e popula o select
-                 * @@param {number|string} status - Status do contrato (1=Ativos, 2=Inativos, etc)
-                 * @@returns {void}
-                 */
                 function loadListaContratos(status) {
                     try {
                         $.ajax({
@@ -862,14 +795,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE ATAS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de atas de registro de preços por status via AJAX
-                 * @@param {number|string} status - Status da ata (1=Ativas, 2=Inativas, etc)
-                 * @@returns {void}
-                 */
                 function loadListaAtas(status) {
                     try {
                         $.ajax({
@@ -898,11 +823,6 @@
                     }
                 }
 
-                /**
-                 * Destrói e limpa a tabela de Notas Fiscais
-                 * @@description Remove os dados e destrói a instância do DataTable se existir
-                 * @@returns {void}
-                 */
                 function resetNFTable() {
                     try {
                         if ($.fn.DataTable.isDataTable("#tblNotaFiscal")) {
@@ -925,7 +845,7 @@
                             return;
                         }
 
-                        GetEmpenhoList(contrato, "#ListaEmpenhosContrato", function () {
+                        GetEmpenhoList(contrato, "#ListaEmpenhosContrato", function() {
                             $("#divEmpenhosContrato").show();
                         });
 
@@ -950,7 +870,7 @@
                             return;
                         }
 
-                        GetEmpenhoListAta(ata, "#ListaEmpenhosAta", function () {
+                        GetEmpenhoListAta(ata, "#ListaEmpenhosAta", function() {
                             $("#divEmpenhosAta").show();
                         });
 
@@ -977,16 +897,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE EMPENHOS POR CONTRATO
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de empenhos associados a um contrato via AJAX
-                 * @@param {number|string} id - ID do contrato
-                 * @@param {string} targetSelect - Seletor jQuery do select de destino
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function GetEmpenhoList(id, targetSelect, callback) {
                     try {
                         $.ajax({
@@ -1019,16 +929,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE EMPENHOS POR ATA
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de empenhos associados a uma ata de registro de preços via AJAX
-                 * @@param {number|string} id - ID da ata de registro de preços
-                 * @@param {string} targetSelect - Seletor jQuery do select de destino
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function GetEmpenhoListAta(id, targetSelect, callback) {
                     try {
                         $.ajax({
@@ -1094,7 +994,7 @@
                     url: "/api/NotaFiscal/GetGlosa",
                     method: "GET",
                     data: { id: id },
-                    success: function (res) {
+                    success: function(res) {
                         try {
                             if (res.success) {
                                 valorNFAtual = res.valorNF || 0;
@@ -1116,7 +1016,7 @@
                             Alerta.TratamentoErroComLinha("Index.cshtml", ".btn-glosa.GetGlosa.success", error);
                         }
                     },
-                    error: function (error) {
+                    error: function(error) {
                         Alerta.TratamentoErroComLinha("Index.cshtml", ".btn-glosa.GetGlosa.ajax", error);
 
                         var modalElement = document.getElementById('modalglosa');
@@ -1129,15 +1029,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * ATUALIZAÇÃO DO PREVIEW DE GLOSA
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Atualiza o preview do valor final da glosa baseado no modo (somar/substituir)
-         * @@global glosaAtualValor {number} - Valor da glosa atual armazenado
-         * @@global valorNFAtual {number} - Valor da nota fiscal atual
-         * @@returns {void}
-         */
         function atualizarPreviewGlosa() {
             try {
                 var valorInformado = parseCurrencyBR($("#txtValorGlosaModal").val()) || 0;
@@ -1165,7 +1056,7 @@
             }
         }
 
-        $(document).on("keyup change", "#txtValorGlosaModal", function () {
+        $(document).on("keyup change", "#txtValorGlosaModal", function() {
             try {
                 if (glosaAtualValor > 0) {
                     $("#divPreviewGlosa").show();
@@ -1176,7 +1067,7 @@
             }
         });
 
-        $(document).on("change", "input[name='rdModoGlosa']", function () {
+        $(document).on("change", "input[name='rdModoGlosa']", function() {
             try {
                 atualizarPreviewGlosa();
             } catch (error) {
@@ -1184,12 +1075,6 @@
             }
         });
 
-        /**
-         * Converte valor monetário no formato brasileiro para número
-         * @@description Remove formatação R$, pontos de milhar e converte vírgula para ponto
-         * @@param {string} str - String com valor monetário no formato BR (ex: "R$ 1.234,56")
-         * @@returns {number} Valor numérico parseado, ou 0 se inválido
-         */
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
```
