# Pages/NotaFiscal/Upsert.cshtml

**Mudanca:** GRANDE | **+0** linhas | **-91** linhas

---

```diff
--- JANEIRO: Pages/NotaFiscal/Upsert.cshtml
+++ ATUAL: Pages/NotaFiscal/Upsert.cshtml
@@ -494,21 +494,7 @@
     <script src="~/js/cadastros/notafiscal.js" asp-append-version="true"></script>
 
     <script>
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * FUNÇÕES UTILITÁRIAS DE CONVERSÃO MONETÁRIA
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /**
-         * Aplica máscara de moeda brasileira durante digitação
-         * @@description Formata valor em tempo real(últimos 2 dígitos = centavos)
-        * @@param { HTMLInputElement } input - Elemento input que recebe a digitação
-            * @@param { string } sep - Separador de milhar(geralmente ".")
-                * @@param { string } dec - Separador decimal(geralmente ",")
-                    * @@param { Event } event - Evento de teclado(keypress)
-                        * @@returns { boolean } false para prevenir inserção padrão do caractere
-                            */
+
         function moeda(input, sep, dec, event) {
             try {
                 let digitado = "",
@@ -569,12 +555,6 @@
             }
         }
 
-        /**
-         * Converte string monetária brasileira para número
-         * @@description Remove símbolos R$, pontos de milhar e converte vírgula decimal
-        * @@param { string } str - Valor formatado como moeda BR(ex: "R$ 10.000,50")
-            * @@returns { number } Valor numérico(ex: 10000.50), retorna 0 se inválido
-                */
         function parseCurrencyBR(str) {
             try {
                 if (!str) return 0;
@@ -591,42 +571,19 @@
             }
         }
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * VARIÁVEIS GLOBAIS E CONFIGURAÇÃO DE EDIÇÃO
-         * ═══════════════════════════════════════════════════════════════════════════
-         */
-
-        /** @@type { number } Status do filtro(1 = Ativo, 0 = Inativo) */
         let statusAtivo = 1;
 
-        /**
-         * Variáveis de modo edição
-         * @@description IDs convertidos para lowercase para comparação case -insensitive
-        */
         const notaFiscalIdEdit = '@Model.NotaFiscalObj?.NotaFiscal?.NotaFiscalId'.toLowerCase();
         const contratoIdEdit = '@Model.NotaFiscalObj?.NotaFiscal?.ContratoId'.toLowerCase();
         const ataIdEdit = '@Model.NotaFiscalObj?.NotaFiscal?.AtaId'.toLowerCase();
         const empenhoIdEdit = '@Model.NotaFiscalObj?.NotaFiscal?.EmpenhoId'.toLowerCase();
-        /** @@type { boolean } Indica se está em modo edição */
         const isEditing = notaFiscalIdEdit && notaFiscalIdEdit !== '' && notaFiscalIdEdit !== '00000000-0000-0000-0000-000000000000';
 
-        /**
-         * ═══════════════════════════════════════════════════════════════════════════
-         * INICIALIZAÇÃO DO DOCUMENTO
-         * ═══════════════════════════════════════════════════════════════════════════
-         * @@description Configura estado inicial, carrega dados em modo edição,
-         * e configura handlers de eventos
-        */
         $(document).ready(function () {
             try {
-                /** Esconde seções condicionais inicialmente */
+
                 $("#divContrato, #divAta, #divEmpenho, #divStatus").hide();
 
-                /**
-                 * Modo edição: carrega dados existentes
-                 * Carrega dropdowns em cascata: Instrumento → Contrato/Ata → Empenho
-                 */
                 if (isEditing) {
                     if (contratoIdEdit && contratoIdEdit !== '' && contratoIdEdit !== '00000000-0000-0000-0000-000000000000') {
 
@@ -678,12 +635,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * TOGGLE STATUS ATIVO/INATIVO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Alterna filtro entre contratos / atas ativos e inativos
-                */
                 $("#toggleStatus").on('click', function () {
                     try {
                         const $toggle = $(this);
@@ -715,12 +666,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════
-                 * SELEÇÃO DE TIPO DE INSTRUMENTO
-                 * ═══════════════════════════════════════════════════════════════════
-                 * @@description Alterna entre Contrato(0) e Ata de Registro de Preços(1)
-                */
                 $("#lstInstrumento").on("change", function () {
                     try {
                         const valor = $(this).val();
@@ -744,15 +689,6 @@
                     }
                 });
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE CONTRATOS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de contratos por status via AJAX e popula o select
-                 * @@param {number|string} status - Status do contrato (1=Ativos, 2=Inativos, etc)
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function loadListaContratos(status, callback) {
                     try {
                         $.ajax({
@@ -785,15 +721,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE ATAS
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de atas de registro de preços por status via AJAX
-                 * @@param {number|string} status - Status da ata (1=Ativas, 2=Inativas, etc)
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function loadListaAtas(status, callback) {
                     try {
                         $.ajax({
@@ -826,15 +753,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE EMPENHOS POR CONTRATO
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de empenhos associados a um contrato via AJAX
-                 * @@param {number|string} contratoId - ID do contrato
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function loadEmpenhosContrato(contratoId, callback) {
                     try {
                         $.ajax({
@@ -870,15 +788,6 @@
                     }
                 }
 
-                /**
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * CARREGAMENTO DE EMPENHOS POR ATA
-                 * ═══════════════════════════════════════════════════════════════════════════
-                 * @@description Carrega lista de empenhos associados a uma ata de registro de preços via AJAX
-                 * @@param {number|string} ataId - ID da ata de registro de preços
-                 * @@param {Function} [callback] - Função callback opcional executada após carregar
-                 * @@returns {void}
-                 */
                 function loadEmpenhosAta(ataId, callback) {
                     try {
                         $.ajax({
```
