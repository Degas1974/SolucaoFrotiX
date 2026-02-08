# Pages/Empenho/Index.cshtml

**Mudanca:** GRANDE | **+45** linhas | **-201** linhas

---

```diff
--- JANEIRO: Pages/Empenho/Index.cshtml
+++ ATUAL: Pages/Empenho/Index.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @model FrotiX.Pages.Empenho.IndexModel
 
 @{
@@ -12,7 +11,7 @@
 
 @section HeadBlock {
     <style>
-        /* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
+/* ======== OUTLINE BRANCO NO BOTÃO DO HEADER (Padrão FrotiX) ======== */
         .ftx-card-header .btn-fundo-laranja {
             outline: 2px solid rgba(255, 255, 255, 0.5) !important;
             outline-offset: 1px;
@@ -118,7 +117,7 @@
             color: #ffffff !important;
             border: none !important;
             font-weight: 700 !important;
-            text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3) !important;
+            text-shadow: 0 1px 2px rgba(0,0,0,0.3) !important;
         }
 
         .ftx-datatable-wrapper .dataTables_paginate .paginate_button:hover:not(.current):not(.disabled) {
@@ -301,8 +300,7 @@
                                             <label class="ftx-label">
                                                 <i class="fa-duotone fa-file-signature"></i> Contratos
                                             </label>
-                                            <select id="ListaContratos" name="ListaContratos"
-                                                class="form-select form-select-sm">
+                                            <select id="ListaContratos" name="ListaContratos" class="form-select form-select-sm">
                                                 <option value="">-- Selecione um Contrato --</option>
                                             </select>
                                         </div>
@@ -322,8 +320,7 @@
                                         </div>
                                         <div class="col-md-9">
                                             <label class="ftx-label">
-                                                <i class="fa-duotone fa-clipboard-list-check"></i> Atas de Registro de
-                                                Preço
+                                                <i class="fa-duotone fa-clipboard-list-check"></i> Atas de Registro de Preço
                                             </label>
                                             <select id="ListaAtas" name="ListaAtas" class="form-select form-select-sm">
                                                 <option value="">-- Selecione uma Ata --</option>
@@ -386,8 +383,7 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-text"></i> Descrição
                             </label>
-                            <input id="txtDescricao" class="form-control form-control-sm"
-                                placeholder="Descrição do aporte..." />
+                            <input id="txtDescricao" class="form-control form-control-sm" placeholder="Descrição do aporte..." />
                         </div>
                     </div>
                     <div class="row g-3 mt-2">
@@ -395,8 +391,11 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-coins"></i> Valor do Aporte
                             </label>
-                            <input id="txtValor" class="form-control form-control-sm" style="text-align: right;"
-                                onkeypress="return moeda(this,'.',',',event)" placeholder="0,00" />
+                            <input id="txtValor"
+                                   class="form-control form-control-sm"
+                                   style="text-align: right;"
+                                   onkeypress="return moeda(this,'.',',',event)"
+                                   placeholder="0,00" />
                         </div>
                     </div>
                 </form>
@@ -439,8 +438,7 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-text"></i> Descrição
                             </label>
-                            <input id="txtDescricaoanulacao" class="form-control form-control-sm"
-                                placeholder="Motivo da anulação..." />
+                            <input id="txtDescricaoanulacao" class="form-control form-control-sm" placeholder="Motivo da anulação..." />
                         </div>
                     </div>
                     <div class="row g-3 mt-2">
@@ -448,8 +446,11 @@
                             <label class="ftx-label">
                                 <i class="fa-duotone fa-coins"></i> Valor da Anulação
                             </label>
-                            <input id="txtValoranulacao" class="form-control form-control-sm" style="text-align: right;"
-                                onkeypress="return moeda(this,'.',',',event)" placeholder="0,00" />
+                            <input id="txtValoranulacao"
+                                   class="form-control form-control-sm"
+                                   style="text-align: right;"
+                                   onkeypress="return moeda(this,'.',',',event)"
+                                   placeholder="0,00" />
                         </div>
                     </div>
                 </form>
@@ -511,23 +512,7 @@
     <script src="~/js/cadastros/empenho.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES UTILITÁRIAS DE CONVERSÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Funções para conversão de valores monetários entre formatos
-        * brasileiro(1.234, 56) e numérico JavaScript(1234.56)
-            */
-
-        /**
-         * Converte string em formato monetário brasileiro para número
-         * @@description Remove símbolos de moeda, pontos de milhar e converte vírgula decimal
-        * @@param { string } str - Valor formatado como moeda BR(ex: "R$ 10.000,50")
-            * @@returns { number } Valor numérico(ex: 10000.50), retorna 0 se inválido
-                * @@example
-         * parseCurrencyBR("R$ 1.234,56")
-                * parseCurrencyBR("10.000,00")
-                */
+
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
@@ -543,25 +528,6 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * MÁSCARA DE MOEDA EM TEMPO REAL
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Aplica máscara de moeda brasileira durante digitação
-         * @@description Formata o valor em tempo real enquanto o usuário digita,
-         * considerando os últimos 2 dígitos como centavos.
-         * Para R$ 1.000,00 o usuário digita 100000.
-        * @@param { HTMLInputElement } input - Elemento input que recebe a digitação
-            * @@param { string } sep - Separador de milhar(geralmente ".")
-                * @@param { string } dec - Separador decimal(geralmente ",")
-                    * @@param { Event } event - Evento de teclado(keypress)
-                        * @@returns { boolean } false para prevenir inserção do caractere(tratado manualmente)
-                            * @@example
-         * <input onkeypress="return moeda(this, '.', ',', event)">
-                                */
         function moeda(input, sep, dec, event) {
             try {
                 let digitado = "",
@@ -577,8 +543,8 @@
                 if ("0123456789".indexOf(digitado) === -1) return false;
 
                 for (tamanho = input.value.length, i = 0;
-                    i < tamanho && (input.value.charAt(i) === "0" || input.value.charAt(i) === dec);
-                    i++);
+                     i < tamanho && (input.value.charAt(i) === "0" || input.value.charAt(i) === dec);
+                     i++);
 
                 for (limpo = ""; i < tamanho; i++) {
                     if ("0123456789".indexOf(input.value.charAt(i)) !== -1) {
@@ -622,36 +588,13 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INSTÂNCIAS DE MODAIS BOOTSTRAP 5
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Variáveis globais para controle programático dos modais
-                                * Aporte, Anulação e Notas Fiscais via Bootstrap Modal API
-                                */
-
-        /** @@type {bootstrap.Modal | null} Instância do modal de Aporte de valor */
         let modalAporteInstance = null;
-        /** @@type {bootstrap.Modal | null} Instância do modal de Anulação de valor */
         let modalAnulacaoInstance = null;
-        /** @@type {bootstrap.Modal | null} Instância do modal de Notas Fiscais */
         let modalNFInstance = null;
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DO DOCUMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Configura modais, eventos delegados para botões dinâmicos
-                                * do DataTable e handlers de filtro por instrumento
-                                */
         $(document).ready(function () {
             try {
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * INICIALIZAÇÃO DOS MODAIS
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Cria instâncias Bootstrap Modal para controle programático
-                                */
+
                 const modalAporteEl = document.getElementById('modalAporte');
                 const modalAnulacaoEl = document.getElementById('modalAnulacao');
                 const modalNFEl = document.getElementById('modalNF');
@@ -664,13 +607,6 @@
                     modalAnulacaoInstance = new bootstrap.Modal(modalAnulacaoEl);
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * EVENTO DELEGADO: BOTÃO APORTE
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Handler para botões .btn-aporte criados dinamicamente
-                                * pelo DataTable. Abre modal para adicionar valor ao empenho.
-                                */
                 $(document).on('click', '.btn-aporte', function (e) {
                     try {
                         e.preventDefault();
@@ -690,13 +626,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * EVENTO DELEGADO: BOTÃO ANULAÇÃO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Handler para botões .btn-anulacao criados dinamicamente.
-                                * Abre modal para registrar anulação/estorno de valor.
-                                */
                 $(document).on('click', '.btn-anulacao', function (e) {
                     try {
                         e.preventDefault();
@@ -720,13 +649,6 @@
                     modalNFInstance = new bootstrap.Modal(modalNFEl);
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * EVENTO DELEGADO: BOTÃO NOTAS FISCAIS
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Handler para botão .btn-nf-modal. Carrega DataTable
-                                * com notas fiscais vinculadas ao empenho selecionado.
-                                */
                 $(document).on('click', '.btn-nf-modal', function (e) {
                     try {
                         e.preventDefault();
@@ -764,14 +686,14 @@
                                     data: "notaFiscalId",
                                     render: function (data) {
                                         return `
-                                        <div class="ftx-actions">
-                                            <a href="/NotaFiscal/Upsert?id=${data}" class="btn btn-icon-28 btn-azul text-white">
-                                                <i class="fa-duotone fa-pen-to-square"></i>
-                                            </a>
-                                            <a class="btn btn-icon-28 btn-vinho btn-delete-nf text-white" data-id="${data}">
-                                                <i class="fa-duotone fa-trash-can"></i>
-                                            </a>
-                                        </div>`;
+                                            <div class="ftx-actions">
+                                                <a href="/NotaFiscal/Upsert?id=${data}" class="btn btn-icon-28 btn-azul text-white">
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="btn btn-icon-28 btn-vinho btn-delete-nf text-white" data-id="${data}">
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                            </div>`;
                                     }
                                 },
                                 { data: "contratoId" },
@@ -792,18 +714,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * EVENTOS DE FILTRO - SELEÇÃO DE INSTRUMENTO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Handlers para dropdowns de filtro que controlam
-                                * a exibição de contratos ou atas e carregam dados
-                                */
-
-                /**
-                 * Handler do dropdown de tipo de instrumento
-                 * @@description Alterna exibição entre filtro de Contrato (0) ou Ata (1)
-                                */
                 $("#lstInstrumento").on('change', function () {
                     try {
                         const val = $(this).val();
@@ -825,10 +735,6 @@
                     }
                 });
 
-                /**
-                 * Handler do dropdown de status de contrato
-                 * @@description Recarrega lista de contratos filtrada por status
-                                */
                 $("#lstStatusContrato").on('change', function () {
                     try {
                         $('#ListaContratos').empty().append('<option value="">-- Selecione um Contrato --</option>');
@@ -838,10 +744,6 @@
                     }
                 });
 
-                /**
-                 * Handler do dropdown de status de ata
-                 * @@description Recarrega lista de atas filtrada por status
-                                */
                 $("#lstStatusAta").on('change', function () {
                     try {
                         $('#ListaAtas').empty().append('<option value="">-- Selecione uma Ata --</option>');
@@ -851,10 +753,6 @@
                     }
                 });
 
-                /**
-                 * Handler do dropdown de contratos
-                 * @@description Ao selecionar contrato, carrega DataTable de empenhos
-                                */
                 $("#ListaContratos").on('change', function () {
                     try {
                         const id = $(this).val();
@@ -871,10 +769,6 @@
                     }
                 });
 
-                /**
-                 * Handler do dropdown de atas
-                 * @@description Ao selecionar ata, carrega DataTable de empenhos
-                                */
                 $("#ListaAtas").on('change', function () {
                     try {
                         const id = $(this).val();
@@ -891,18 +785,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * FUNÇÕES DE CARGA DE DADOS VIA AJAX
-                 * ═══════════════════════════════════════════════════════════════════
-                 */
-
-                /**
-                 * Carrega lista de contratos no dropdown
-                 * @@description Faz requisição AJAX para popular dropdown de contratos
-                                * filtrados por status (Ativo/Inativo)
-                                * @@param {number | string} status - Status do contrato (1=Ativo, 0=Inativo)
-                                */
                 function loadListaContratos(status) {
                     try {
                         $.ajax({
@@ -932,12 +814,6 @@
                     }
                 }
 
-                /**
-                 * Carrega lista de atas no dropdown
-                 * @@description Faz requisição AJAX para popular dropdown de Atas de
-                                * Registro de Preços filtradas por status
-                                * @@param {number | string} status - Status da ata (1=Ativa, 0=Inativa)
-                                */
                 function loadListaAtas(status) {
                     try {
                         $.ajax({
@@ -967,20 +843,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * FUNÇÃO DE PREENCHIMENTO DO DATATABLE DE EMPENHOS
-                 * ═══════════════════════════════════════════════════════════════════
-                 */
-
-                /**
-                 * Preenche DataTable com empenhos do instrumento selecionado
-                 * @@description Inicializa DataTable #tblEmpenho com dados de empenhos
-                                * vinculados ao contrato ou ata selecionado. Exibe colunas
-                                * de saldo, datas de vigência e botões de ação.
-                                * @@param {string | number} id - ID do contrato ou ata
-                                * @@param {string} instrumento - Tipo: "contrato" ou "ata"
-                                */
                 function PreencheTabelaEmpenho(id, instrumento) {
                     try {
                         $('#divEmpenho').show();
@@ -1017,31 +879,31 @@
                                     data: "empenhoId",
                                     render: function (data) {
                                         return `
-                                        <div class="ftx-actions">
-                                            <a href="/Empenho/Upsert?id=${data}" class="btn btn-icon-28 btn-azul text-white">
-                                                <i class="fa-duotone fa-pen-to-square"></i>
-                                            </a>
-                                            <a class="btn btn-icon-28 btn-vinho btn-delete text-white" data-id="${data}">
-                                                <i class="fa-duotone fa-trash-can"></i>
-                                            </a>
-                                            <a class="btn btn-icon-28 fundo-cinza btn-nf-modal text-white" data-ejtip="Notas Fiscais do Empenho" data-id="${data}">
-                                                <i class="fa-duotone fa-file-invoice"></i>
-                                            </a>
-                                        </div>`;
+                                            <div class="ftx-actions">
+                                                <a href="/Empenho/Upsert?id=${data}" class="btn btn-icon-28 btn-azul text-white">
+                                                    <i class="fa-duotone fa-pen-to-square"></i>
+                                                </a>
+                                                <a class="btn btn-icon-28 btn-vinho btn-delete text-white" data-id="${data}">
+                                                    <i class="fa-duotone fa-trash-can"></i>
+                                                </a>
+                                                <a class="btn btn-icon-28 fundo-cinza btn-nf-modal text-white" data-ejtip="Notas Fiscais do Empenho" data-id="${data}">
+                                                    <i class="fa-duotone fa-file-invoice"></i>
+                                                </a>
+                                            </div>`;
                                     }
                                 },
                                 {
                                     data: "empenhoId",
                                     render: function (data) {
                                         return `
-                                        <div class="ftx-actions">
-                                            <a class="btn btn-icon-28 btn-verde btn-aporte text-white" data-ejtip="Aportar Valor" data-id="${data}">
-                                                <i class="fa-duotone fa-circle-plus"></i>
-                                            </a>
-                                            <a class="btn btn-icon-28 btn-vinho btn-anulacao text-white" data-id="${data}">
-                                                <i class="fa-duotone fa-circle-minus"></i>
-                                            </a>
-                                        </div>`;
+                                            <div class="ftx-actions">
+                                                <a class="btn btn-icon-28 btn-verde btn-aporte text-white" data-ejtip="Aportar Valor" data-id="${data}">
+                                                    <i class="fa-duotone fa-circle-plus"></i>
+                                                </a>
+                                                <a class="btn btn-icon-28 btn-vinho btn-anulacao text-white" data-id="${data}">
+                                                    <i class="fa-duotone fa-circle-minus"></i>
+                                                </a>
+                                            </div>`;
                                     }
                                 },
                                 { data: "empenhoId" }
@@ -1061,18 +923,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * EVENTOS DE BOTÕES DE AÇÃO PRINCIPAIS
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handlers para botões de submissão dos modais de Aporte e Anulação
-         */
-
-        /**
-         * Handler do botão Aportar
-         * @@description Valida campos, envia requisição POST para /api/Empenho/Aporte
-         * com dados do aporte. Exibe spinner durante processamento.
-         */
         $("#btnAportar").on("click", function (e) {
             try {
                 e.preventDefault();
@@ -1135,11 +985,6 @@
             }
         });
 
-        /**
-         * Handler do botão Anular
-         * @@description Valida campos, envia requisição POST para /api/Empenho/anulacao
-         * para registrar anulação/estorno de valor do empenho
-         */
         $("#btnAnular").on("click", function (e) {
             try {
                 e.preventDefault();
@@ -1202,13 +1047,6 @@
             }
         });
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * EVENTO DELEGADO: EXCLUSÃO DE EMPENHO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Handler delegado para botões .btn-delete criados pelo DataTable.
-         * Exibe confirmação SweetAlert antes de excluir via POST.
-         */
         $(document).on("click", ".btn-delete", function () {
             try {
                 var id = $(this).data("id");
```
