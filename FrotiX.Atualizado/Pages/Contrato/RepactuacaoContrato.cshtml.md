# Pages/Contrato/RepactuacaoContrato.cshtml

**Mudanca:** GRANDE | **+70** linhas | **-154** linhas

---

```diff
--- JANEIRO: Pages/Contrato/RepactuacaoContrato.cshtml
+++ ATUAL: Pages/Contrato/RepactuacaoContrato.cshtml
@@ -1,5 +1,4 @@
 @page
-
 @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
 
 @model FrotiX.Pages.Contrato.RepactuacaoContratoModel
@@ -336,6 +335,7 @@
         vertical-align: middle;
         padding: 0.4rem 0.5rem;
     }
+
 </style>
 
 @section HeadBlock {
@@ -375,8 +375,7 @@
                         </div>
                         <div class="col-md-3">
                             <label class="label">Valor Atual</label>
-                            <input id="txtValorAtual" class="form-control form-control-xs" type="text" disabled
-                                style="text-align: right;" />
+                            <input id="txtValorAtual" class="form-control form-control-xs" type="text" disabled style="text-align: right;" />
                         </div>
                     </div>
                 </div>
@@ -406,8 +405,7 @@
                                     <div class="col-md-2">
                                         <div class="ftx-resumo-item">
                                             <span class="ftx-resumo-label">Valor Anual</span>
-                                            <span id="resumoValorAnual" class="ftx-resumo-valor ftx-valor-destaque">R$
-                                                0,00</span>
+                                            <span id="resumoValorAnual" class="ftx-resumo-valor ftx-valor-destaque">R$ 0,00</span>
                                         </div>
                                     </div>
                                     <div class="col-md-2">
@@ -494,8 +492,7 @@
                     </div>
 
                     <div class="table-responsive">
-                        <table id="tblRepactuacoes" class="table table-striped table-bordered dt-responsive nowrap"
-                            width="100%">
+                        <table id="tblRepactuacoes" class="table table-striped table-bordered dt-responsive nowrap" width="100%">
                             <thead>
                                 <tr>
                                     <th style="text-align:center;">Data</th>
@@ -513,8 +510,7 @@
 
                     <div class="row mt-3">
                         <div class="col-md-4">
-                            <button id="btnNovaRepactuacao" type="button"
-                                class="ftx-btn-action btn-verde-militar w-100">
+                            <button id="btnNovaRepactuacao" type="button" class="ftx-btn-action btn-verde-militar w-100">
                                 <i class="fa-duotone fa-file-plus"></i> Nova Repactuação
                             </button>
                         </div>
@@ -540,7 +536,7 @@
                         <div class="col-md-2">
                             <label class="label">Novo Valor (R$)</label>
                             <input id="txtValor" class="form-control form-control-xs" style="text-align: right;"
-                                onKeyPress="return(moeda(this,'.',',',event))" />
+                                   onKeyPress="return(moeda(this,'.',',',event))" />
                         </div>
                         <div class="col-md-2">
                             <label class="label">Vigência</label>
@@ -572,12 +568,12 @@
                         </div>
                         <div class="col-md-2">
                             <label class="label">Reajuste (%)</label>
-                            <input id="txtPercentual" class="form-control form-control-xs" style="text-align: right;"
-                                placeholder="Ex: 5,25" onKeyPress="return(moeda(this,'.',',',event))" />
+                            <input id="txtPercentual" class="form-control form-control-xs"
+                                   style="text-align: right;" placeholder="Ex: 5,25"
+                                   onKeyPress="return(moeda(this,'.',',',event))" />
                         </div>
                         <div class="col-md-2 d-flex align-items-end">
-                            <button id="btnAplicarPercentual" type="button" class="ftx-btn-action btn-laranja w-100"
-                                style="height:32px;">
+                            <button id="btnAplicarPercentual" type="button" class="ftx-btn-action btn-laranja w-100" style="height:32px;">
                                 <i class="fa-duotone fa-percent"></i> Aplicar
                             </button>
                         </div>
@@ -586,8 +582,7 @@
                     <div class="row mt-2">
                         <div class="col-md-12">
                             <label class="label">Descrição</label>
-                            <input id="txtDescricao" class="form-control form-control-xs" type="text"
-                                placeholder="Ex: Reajuste anual IPCA 2024" />
+                            <input id="txtDescricao" class="form-control form-control-xs" type="text" placeholder="Ex: Reajuste anual IPCA 2024" />
                         </div>
                     </div>
                 </div>
@@ -600,8 +595,7 @@
 
                     <div class="ftx-info-box">
                         <i class="fa-duotone fa-info-circle"></i>
-                        Informe os valores mensais unitários para cada categoria. Marque apenas as categorias que fazem
-                        parte deste contrato.
+                        Informe os valores mensais unitários para cada categoria. Marque apenas as categorias que fazem parte deste contrato.
                     </div>
 
                     <div class="row">
@@ -614,14 +608,12 @@
                                 <div class="row">
                                     <div class="col-7">
                                         <label class="label" style="font-size:0.75rem;">Valor (R$)</label>
-                                        <input id="txtCustoMensalEncarregados" disabled
-                                            class="form-control form-control-xs" style="text-align:right;"
-                                            onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <input id="txtCustoMensalEncarregados" disabled class="form-control form-control-xs"
+                                               style="text-align:right;" onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                     <div class="col-5">
                                         <label class="label" style="font-size:0.75rem;">Qtd.</label>
-                                        <input id="txtQtdEncarregados" disabled class="form-control form-control-xs"
-                                            type="number" style="text-align:center;" />
+                                        <input id="txtQtdEncarregados" disabled class="form-control form-control-xs" type="number" style="text-align:center;" />
                                     </div>
                                 </div>
                             </div>
@@ -636,14 +628,12 @@
                                 <div class="row">
                                     <div class="col-7">
                                         <label class="label" style="font-size:0.75rem;">Valor (R$)</label>
-                                        <input id="txtCustoMensalOperadores" disabled
-                                            class="form-control form-control-xs" style="text-align:right;"
-                                            onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <input id="txtCustoMensalOperadores" disabled class="form-control form-control-xs"
+                                               style="text-align:right;" onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                     <div class="col-5">
                                         <label class="label" style="font-size:0.75rem;">Qtd.</label>
-                                        <input id="txtQtdOperadores" disabled class="form-control form-control-xs"
-                                            type="number" style="text-align:center;" />
+                                        <input id="txtQtdOperadores" disabled class="form-control form-control-xs" type="number" style="text-align:center;" />
                                     </div>
                                 </div>
                             </div>
@@ -658,14 +648,12 @@
                                 <div class="row">
                                     <div class="col-7">
                                         <label class="label" style="font-size:0.75rem;">Valor (R$)</label>
-                                        <input id="txtCustoMensalMotoristas" disabled
-                                            class="form-control form-control-xs" style="text-align:right;"
-                                            onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <input id="txtCustoMensalMotoristas" disabled class="form-control form-control-xs"
+                                               style="text-align:right;" onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                     <div class="col-5">
                                         <label class="label" style="font-size:0.75rem;">Qtd.</label>
-                                        <input id="txtQtdMotoristas" disabled class="form-control form-control-xs"
-                                            type="number" style="text-align:center;" />
+                                        <input id="txtQtdMotoristas" disabled class="form-control form-control-xs" type="number" style="text-align:center;" />
                                     </div>
                                 </div>
                             </div>
@@ -680,14 +668,12 @@
                                 <div class="row">
                                     <div class="col-7">
                                         <label class="label" style="font-size:0.75rem;">Valor (R$)</label>
-                                        <input id="txtCustoMensalLavadores" disabled
-                                            class="form-control form-control-xs" style="text-align:right;"
-                                            onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <input id="txtCustoMensalLavadores" disabled class="form-control form-control-xs"
+                                               style="text-align:right;" onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                     <div class="col-5">
                                         <label class="label" style="font-size:0.75rem;">Qtd.</label>
-                                        <input id="txtQtdLavadores" disabled class="form-control form-control-xs"
-                                            type="number" style="text-align:center;" />
+                                        <input id="txtQtdLavadores" disabled class="form-control form-control-xs" type="number" style="text-align:center;" />
                                     </div>
                                 </div>
                             </div>
@@ -703,8 +689,7 @@
 
                     <div class="ftx-info-box">
                         <i class="fa-duotone fa-info-circle"></i>
-                        Edite os valores e quantidades de cada item. Use o botão de percentual acima para aplicar
-                        reajuste em todos os itens.
+                        Edite os valores e quantidades de cada item. Use o botão de percentual acima para aplicar reajuste em todos os itens.
                     </div>
 
                     <div class="row mb-3">
@@ -738,8 +723,7 @@
                     <div class="row mt-3">
                         <div class="col-md-3">
                             <label class="label">Total Geral</label>
-                            <input id="txtTotal" class="form-control form-control-xs"
-                                style="text-align:right; font-weight:bold;" disabled />
+                            <input id="txtTotal" class="form-control form-control-xs" style="text-align:right; font-weight:bold;" disabled />
                         </div>
                     </div>
                 </div>
@@ -752,8 +736,7 @@
                                     <i class="fa-duotone fa-pen-to-square me-2"></i>
                                     <span id="modalItemTitulo">Editar Item</span>
                                 </h5>
-                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"
-                                    aria-label="Fechar"></button>
+                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Fechar"></button>
                             </div>
                             <div class="modal-body">
                                 <input type="hidden" id="txtItemVeiculoId" />
@@ -772,18 +755,16 @@
                                 <div class="row mb-3">
                                     <div class="col-md-4">
                                         <label class="label">Quantidade</label>
-                                        <input id="txtItemQuantidade" class="form-control form-control-xs" type="number"
-                                            style="text-align:center;" />
+                                        <input id="txtItemQuantidade" class="form-control form-control-xs" type="number" style="text-align:center;" />
                                     </div>
                                     <div class="col-md-4">
                                         <label class="label">Valor Unitário</label>
-                                        <input id="txtItemValorUnitario" class="form-control form-control-xs"
-                                            style="text-align:right;" onKeyPress="return(moeda(this,'.',',',event))" />
+                                        <input id="txtItemValorUnitario" class="form-control form-control-xs" style="text-align:right;"
+                                               onKeyPress="return(moeda(this,'.',',',event))" />
                                     </div>
                                     <div class="col-md-4">
                                         <label class="label">Valor Total</label>
-                                        <input id="txtItemValorTotal" class="form-control form-control-xs"
-                                            style="text-align:right;" disabled />
+                                        <input id="txtItemValorTotal" class="form-control form-control-xs" style="text-align:right;" disabled />
                                     </div>
                                 </div>
                             </div>
@@ -809,7 +790,7 @@
                         <div class="col-md-4">
                             <label class="label">Valor Anual do Serviço (R$)</label>
                             <input id="txtValorServico" class="form-control form-control-xs" style="text-align: right;"
-                                onKeyPress="return(moeda(this,'.',',',event))" />
+                                   onKeyPress="return(moeda(this,'.',',',event))" />
                         </div>
                     </div>
                 </div>
@@ -842,50 +823,23 @@
     <script asp-append-version="true">
 
         if (typeof $.debounce !== 'function') {
-            $.debounce = function (delay, callback) {
+            $.debounce = function(delay, callback) {
                 var timer = null;
-                return function () {
+                return function() {
                     var context = this, args = arguments;
                     clearTimeout(timer);
-                    timer = setTimeout(function () {
+                    timer = setTimeout(function() {
                         callback.apply(context, args);
                     }, delay);
                 };
             };
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * VARIÁVEIS GLOBAIS - REPACTUAÇÃO DE CONTRATO
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Estado global da página de repactuação de contratos.
-             * Controla modo de edição, contrato selecionado e tipo.
-             */
-
-            /** @@type { boolean } Indica se está em modo de edição de repactuação existente */
         var modoEdicao = false;
-            /** @@type { Object | null } Objeto do contrato atualmente selecionado */
         var contratoSelecionado = null;
-            /** @@type { string } Tipo do contrato atual(Locação, Terceirização, Serviços) */
-            var tipoContratoAtual = '';
-            /** @@type { string | null } ID da repactuação anterior(para mover veículos) */
+        var tipoContratoAtual = '';
         var repactuacaoAnteriorId = null;
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES UTILITÁRIAS - FORMATAÇÃO E CONVERSÃO
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Formata valor monetário durante digitação
-             * @@description Aplica máscara monetária brasileira em tempo real
-            * @@param { HTMLInputElement } a - Elemento input sendo editado
-                * @@param { string } e - Separador de milhar(geralmente '.')
-                    * @@param { string } r - Separador decimal(geralmente ',')
-                        * @@param { KeyboardEvent } t - Evento de teclado
-                            * @@returns { boolean } false para prevenir digitação inválida
-                                */
         function moeda(a, e, r, t) {
             try {
                 let n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "",
@@ -912,12 +866,6 @@
             }
         }
 
-            /**
-             * Converte valor float para formato brasileiro
-             * @@description Usa Intl.NumberFormat para formatar com 2 casas decimais
-            * @@param { number } valor - Valor numérico(ex: 1234.56)
-                * @@returns { string } Valor formatado(ex: "1.234,56")
-                    */
         function floatToBR(valor) {
             try {
                 if (valor === null || valor === undefined) return "0,00";
@@ -928,12 +876,6 @@
             }
         }
 
-            /**
-             * Converte valor em formato brasileiro para float
-             * @@description Remove pontos de milhar, troca vírgula por ponto e símbolos de moeda
-            * @@param { string| number} valor - Valor a converter(ex: "1.234,56" ou "R$ 1.234,56")
-                * @@returns { number } Valor numérico(ex: 1234.56)
-                    */
         function brToFloat(valor) {
             try {
                 if (!valor) return 0;
@@ -949,11 +891,6 @@
             }
         }
 
-            /**
-             * Converte data formato BR (dd/mm/yyyy) para ISO (yyyy-mm-dd)
-             * @@param { string } dateStr - Data no formato brasileiro
-            * @@returns { string } Data no formato ISO
-                */
         function formatDateToISO(dateStr) {
             try {
                 if (!dateStr) return '';
@@ -968,11 +905,6 @@
             }
         }
 
-            /**
-             * Converte data formato ISO (yyyy-mm-dd) para BR (dd/mm/yyyy)
-             * @@param { string } dateStr - Data no formato ISO
-            * @@returns { string } Data no formato brasileiro
-                */
         function formatDateToBR(dateStr) {
             try {
                 if (!dateStr) return '';
@@ -987,13 +919,6 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * INICIALIZAÇÃO DA PÁGINA
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Inicializa a página verificando parâmetros da URL e
-            * configurando eventos dos elementos.
-             */
         document.addEventListener('DOMContentLoaded', function () {
             try {
 
@@ -1009,12 +934,6 @@
             }
         });
 
-            /**
-             * Carrega lista de contratos no dropdown
-             * @@description Busca contratos via API e popula o select.
-             * Se houver contratoId na URL, seleciona automaticamente.
-             * @@param { string| null} contratoIdParaSelecionar - GUID do contrato a selecionar
-            */
         function carregarContratos(contratoIdParaSelecionar) {
             try {
                 $.ajax({
@@ -1050,7 +969,7 @@
                                     carregarDadosContrato(contratoIdUpper);
                                 } else {
                                     console.warn('❌ Contrato não encontrado na lista:', contratoIdUpper);
-                                    console.log('Valores disponíveis:', res.data.map(function (i) { return i.value; }));
+                                    console.log('Valores disponíveis:', res.data.map(function(i) { return i.value; }));
                                 }
                             }
                         } catch (error) {
@@ -1066,11 +985,6 @@
             }
         }
 
-            /**
-             * Configura todos os event listeners da página
-             * @@description Adiciona handlers para dropdown de contratos, botões de ação,
-             * checkboxes de terceirização e outras interações.
-             */
         function configurarEventos() {
             try {
 
@@ -1250,28 +1164,28 @@
                                 res.data.forEach(function (item) {
                                     var tr = document.createElement('tr');
                                     tr.innerHTML = `
-                                                <td style="text-align:center;">${item.dataFormatada || ''}</td>
-                                                <td>${item.descricao || ''}</td>
-                                                <td style="text-align:right;">${item.valor || 'R$ 0,00'}</td>
-                                                <td style="text-align:right;">${item.valorMensal || 'R$ 0,00'}</td>
-                                                <td style="text-align:center;">${item.vigencia || '-'}</td>
-                                                <td style="text-align:center;">${item.prorrogacao || '-'}</td>
-                                                <td style="text-align:center;">
-                                                    <button type="button" class="btn btn-icon-28 btn-azul btn-editar" data-id="${item.repactuacaoContratoId}">
-                                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                                    </button>
-                                                    <button type="button" class="btn btn-icon-28 btn-vinho btn-excluir" data-id="${item.repactuacaoContratoId}">
-                                                        <i class="fa-duotone fa-trash-can"></i>
-                                                    </button>
-                                                </td>
-                                            `;
+                                        <td style="text-align:center;">${item.dataFormatada || ''}</td>
+                                        <td>${item.descricao || ''}</td>
+                                        <td style="text-align:right;">${item.valor || 'R$ 0,00'}</td>
+                                        <td style="text-align:right;">${item.valorMensal || 'R$ 0,00'}</td>
+                                        <td style="text-align:center;">${item.vigencia || '-'}</td>
+                                        <td style="text-align:center;">${item.prorrogacao || '-'}</td>
+                                        <td style="text-align:center;">
+                                            <button type="button" class="btn btn-icon-28 btn-azul btn-editar" data-id="${item.repactuacaoContratoId}">
+                                                <i class="fa-duotone fa-pen-to-square"></i>
+                                            </button>
+                                            <button type="button" class="btn btn-icon-28 btn-vinho btn-excluir" data-id="${item.repactuacaoContratoId}">
+                                                <i class="fa-duotone fa-trash-can"></i>
+                                            </button>
+                                        </td>
+                                    `;
                                     tbody.appendChild(tr);
                                 });
 
                                 bindBotoesTabela();
 
                                 if (tipoContratoAtual === 'Locação') {
-                                    setTimeout(function () {
+                                    setTimeout(function() {
                                         verificarVeiculosRepactuacoes();
                                     }, 300);
                                 }
@@ -1354,8 +1268,8 @@
 
                             var descricaoLower = (rep.descricao || '').toLowerCase();
                             var isValorInicial = descricaoLower.includes('valor inicial') ||
-                                descricaoLower.includes('inicial') ||
-                                descricaoLower.includes('contrato original');
+                                                 descricaoLower.includes('inicial') ||
+                                                 descricaoLower.includes('contrato original');
 
                             var badgeTipo = document.getElementById('badgeTipoRepactuacao');
                             var txtTipo = document.getElementById('txtTipoRepactuacao');
@@ -1893,20 +1807,20 @@
                     var tr = document.createElement('tr');
                     tr.setAttribute('data-index', index);
                     tr.innerHTML = `
-                                <td style="text-align:center;">${item.numItem || item.NumItem || ''}</td>
-                                <td>${item.descricao || item.Descricao || ''}</td>
-                                <td style="text-align:center;">${qtd}</td>
-                                <td style="text-align:right;">R$ ${floatToBR(valUnit)}</td>
-                                <td style="text-align:right;">R$ ${floatToBR(valTotal)}</td>
-                                <td style="text-align:center;">
-                                    <button type="button" class="btn btn-icon-28 btn-azul" onclick="abrirModalItem(${index})">
-                                        <i class="fa-duotone fa-pen-to-square"></i>
-                                    </button>
-                                    <button type="button" class="btn btn-icon-28 btn-vinho" onclick="excluirItemLocacao(${index})" ${temVeiculo ? 'disabled' : ''}>
-                                        <i class="fa-duotone fa-trash-can"></i>
-                                    </button>
-                                </td>
-                            `;
+                        <td style="text-align:center;">${item.numItem || item.NumItem || ''}</td>
+                        <td>${item.descricao || item.Descricao || ''}</td>
+                        <td style="text-align:center;">${qtd}</td>
+                        <td style="text-align:right;">R$ ${floatToBR(valUnit)}</td>
+                        <td style="text-align:right;">R$ ${floatToBR(valTotal)}</td>
+                        <td style="text-align:center;">
+                            <button type="button" class="btn btn-icon-28 btn-azul" onclick="abrirModalItem(${index})">
+                                <i class="fa-duotone fa-pen-to-square"></i>
+                            </button>
+                            <button type="button" class="btn btn-icon-28 btn-vinho" onclick="excluirItemLocacao(${index})" ${temVeiculo ? 'disabled' : ''}>
+                                <i class="fa-duotone fa-trash-can"></i>
+                            </button>
+                        </td>
+                    `;
                     tbody.appendChild(tr);
                 });
             } catch (error) {
@@ -2277,7 +2191,7 @@
                                     salvarItensLocacao(novaRepactuacaoId);
 
                                     if (isNovaRepactuacao && repactuacaoAnteriorId) {
-                                        setTimeout(function () {
+                                        setTimeout(function() {
                                             perguntarMoverVeiculos(repactuacaoAnteriorId, novaRepactuacaoId, contratoId);
                                         }, 500);
                                     } else {
@@ -2346,7 +2260,7 @@
                                 allowOutsideClick: false,
                                 allowEscapeKey: false,
                                 showConfirmButton: false,
-                                didOpen: function () {
+                                didOpen: function() {
                                     Swal.showLoading();
                                 }
                             });
```
