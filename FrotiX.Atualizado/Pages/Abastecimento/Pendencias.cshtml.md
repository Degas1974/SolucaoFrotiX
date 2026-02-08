# Pages/Abastecimento/Pendencias.cshtml

**Mudanca:** GRANDE | **+0** linhas | **-105** linhas

---

```diff
--- JANEIRO: Pages/Abastecimento/Pendencias.cshtml
+++ ATUAL: Pages/Abastecimento/Pendencias.cshtml
@@ -740,20 +740,10 @@
         var pendenciaAtual = null;
         var filtroAtivo = 'total';
 
-        /**
-         * Exibe overlay de loading com animação
-         * @@description Mostra overlay de carregamento com flex display
-         * @@returns {void}
-         */
         function mostrarLoading() {
             $('#loadingOverlayPendencias').css('display', 'flex');
         }
 
-        /**
-         * Esconde overlay de loading
-         * @@description Oculta overlay de carregamento
-         * @@returns {void}
-         */
         function esconderLoading() {
             $('#loadingOverlayPendencias').css('display', 'none');
         }
@@ -769,12 +759,6 @@
             }
         });
 
-        /**
-         * Filtra pendências por tipo de card clicado
-         * @@description Aplica filtro visual e atualiza DataTable conforme tipo selecionado
-         * @@param {string} tipo - Tipo de filtro: 'total', 'veiculo', 'motorista', 'km', 'corrigivel'
-         * @@returns {void}
-         */
         function filtrarPorCard(tipo) {
             try {
                 mostrarLoading();
@@ -808,11 +792,6 @@
             }
         }
 
-        /**
-         * Aplica filtro customizado no DataTable de pendências
-         * @@description Usa $.fn.dataTable.ext.search para filtrar linhas conforme tipo ativo
-         * @@returns {void}
-         */
         function aplicarFiltroDataTable() {
             try {
                 if (!tblPendencias) return;
@@ -844,11 +823,6 @@
             }
         }
 
-        /**
-         * Carrega contadores de pendências por tipo via API
-         * @@description Busca estatísticas e atualiza cards de resumo
-         * @@returns {void}
-         */
         function carregarEstatisticas() {
             try {
                 $.ajax({
@@ -875,11 +849,6 @@
             }
         }
 
-        /**
-         * Inicializa ou reinicializa DataTable de pendências
-         * @@description Configura DataTable com AJAX, colunas e renderizadores customizados
-         * @@returns {void}
-         */
         function carregarPendencias() {
             try {
                 if (tblPendencias) {
@@ -943,12 +912,6 @@
             }
         }
 
-        /**
-         * Renderiza badge visual indicando tipo de pendência
-         * @@description Gera HTML de badge com ícone duotone, classe de cor e tooltip
-         * @@param {Object} row - Dados da linha do DataTable
-         * @@returns {string} HTML do badge
-         */
         function renderBadgePendencia(row) {
             try {
                 var tipo = row.tipoPendencia || 'erro';
@@ -966,12 +929,6 @@
             }
         }
 
-        /**
-         * Renderiza botões de ação para cada pendência
-         * @@description Gera HTML com botões de sugestão, editar e excluir
-         * @@param {Object} row - Dados da linha do DataTable
-         * @@returns {string} HTML dos botões de ação
-         */
         function renderAcoes(row) {
             try {
                 var html = '<div class="acoes-pendencia">';
@@ -995,12 +952,6 @@
             }
         }
 
-        /**
-         * Exibe modal de confirmação para aplicar sugestão de correção
-         * @@description Busca detalhes da pendência via API e exibe SweetAlert com valores atual/sugerido
-         * @@param {string} id - ID da pendência
-         * @@returns {void}
-         */
         function aplicarSugestao(id) {
             try {
 
@@ -1048,11 +999,6 @@
             }
         }
 
-        /**
-         * Confirma aplicação de sugestão no modal
-         * @@description Valida ID, ativa loading e dispara execução
-         * @@returns {void}
-         */
         function confirmarAplicarSugestao() {
             try {
                 var id = $("#txtSugestaoId").val();
@@ -1072,12 +1018,6 @@
             }
         }
 
-        /**
-         * Executa aplicação de sugestão via API
-         * @@description Envia POST para aplicar correção automática e recarrega tabela
-         * @@param {string} id - ID da pendência
-         * @@returns {void}
-         */
         function executarAplicarSugestao(id) {
             try {
                 $.ajax({
@@ -1120,12 +1060,6 @@
             }
         }
 
-        /**
-         * Abre modal de edição com dados da pendência
-         * @@description Busca dados via API e preenche formulário do modal
-         * @@param {string} id - ID da pendência
-         * @@returns {void}
-         */
         function editarPendencia(id) {
             try {
                 $.ajax({
@@ -1188,11 +1122,6 @@
             }
         }
 
-        /**
-         * Aplica sugestão de correção nos campos do modal de edição
-         * @@description Preenche campo KM com valor sugerido e recalcula KM rodado
-         * @@returns {void}
-         */
         function aplicarSugestaoNoModal() {
             try {
                 if (!pendenciaAtual || !pendenciaAtual.temSugestao) return;
@@ -1213,11 +1142,6 @@
             }
         }
 
-        /**
-         * Salva alterações da pendência via API
-         * @@description Coleta dados do formulário e envia POST para atualizar pendência
-         * @@returns {void}
-         */
         function salvarPendencia() {
             try {
                 setButtonLoading("#btnSalvarPendencia", true);
@@ -1277,11 +1201,6 @@
             }
         }
 
-        /**
-         * Salva pendência e tenta importar como abastecimento válido
-         * @@description Salva alterações e executa resolução automática da pendência
-         * @@returns {void}
-         */
         function salvarEImportar() {
             try {
                 setButtonLoading("#btnSalvarImportar", true);
@@ -1341,12 +1260,6 @@
             }
         }
 
-        /**
-         * Resolve pendência convertendo para abastecimento válido
-         * @@description Busca dados da pendência e envia para endpoint de resolução
-         * @@param {string} id - ID da pendência
-         * @@returns {void}
-         */
         function resolverPendenciaAjax(id) {
             try {
                 $.ajax({
@@ -1402,12 +1315,6 @@
             }
         }
 
-        /**
-         * Exclui pendência após confirmação
-         * @@description Exibe SweetAlert de confirmação e remove pendência via API
-         * @@param {string} id - ID da pendência
-         * @@returns {void}
-         */
         function excluirPendencia(id) {
             try {
                 Alerta.Confirmar("Excluir Pendência?", "Esta ação não poderá ser desfeita.", "Excluir", "Cancelar")
@@ -1445,11 +1352,6 @@
             }
         }
 
-        /**
-         * Exclui todas as pendências após confirmação
-         * @@description Exibe SweetAlert de confirmação e remove todas via API
-         * @@returns {void}
-         */
         function excluirTodasPendencias() {
             try {
                 Alerta.Confirmar("Excluir TODAS as Pendências?", "Esta ação excluirá permanentemente todas as pendências. Não poderá ser desfeita!", "Excluir Todas", "Cancelar")
@@ -1485,13 +1387,6 @@
             }
         }
 
-        /**
-         * Alterna estado de loading em botão
-         * @@description Mostra/oculta spinner e desabilita/habilita botão
-         * @@param {string} selector - Seletor jQuery do botão
-         * @@param {boolean} loading - True para ativar loading, false para desativar
-         * @@returns {void}
-         */
         function setButtonLoading(selector, loading) {
             try {
                 if (loading) {
```
