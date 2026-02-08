# Pages/Manutencao/Glosas.cshtml

**Mudanca:** GRANDE | **+121** linhas | **-189** linhas

---

```diff
--- JANEIRO: Pages/Manutencao/Glosas.cshtml
+++ ATUAL: Pages/Manutencao/Glosas.cshtml
@@ -9,7 +9,7 @@
 
 <style>
     .row-glosa-positiva {
-        background: rgba(255, 99, 132, .08) !important;
+        background: rgba(255,99,132,.08) !important;
     }
 </style>
 
@@ -43,32 +43,40 @@
             <div class="row g-3 align-items-end">
                 <div class="col-lg-4 col-md-6">
                     <label class="form-label">Contrato</label>
-                    <ejs-dropdownlist id="ContratoId" locale="pt-BR" dataSource="@Model.ContratoList"
-                        placeholder="Selecione um contrato" allowFiltering="true" popupHeight="300px">
+                    <ejs-dropdownlist id="ContratoId"
+                                      locale="pt-BR"
+                                      dataSource="@Model.ContratoList"
+                                      placeholder="Selecione um contrato"
+                                      allowFiltering="true"
+                                      popupHeight="300px">
                         <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                     </ejs-dropdownlist>
                 </div>
 
                 <div class="col-lg-2 col-md-3">
                     <label class="form-label">Ano</label>
-                    <ejs-dropdownlist id="Ano" locale="pt-BR" dataSource="@Model.Anos" value="@Model.AnoSelecionado"
-                        placeholder="Ano">
+                    <ejs-dropdownlist id="Ano"
+                                      locale="pt-BR"
+                                      dataSource="@Model.Anos"
+                                      value="@Model.AnoSelecionado"
+                                      placeholder="Ano">
                     </ejs-dropdownlist>
                 </div>
 
                 <div class="col-lg-2 col-md-3">
                     <label class="form-label">Mês</label>
-                    <ejs-dropdownlist id="Mes" locale="pt-BR" dataSource="@Model.Meses" value="@Model.MesSelecionado"
-                        placeholder="Mês">
+                    <ejs-dropdownlist id="Mes"
+                                      locale="pt-BR"
+                                      dataSource="@Model.Meses"
+                                      value="@Model.MesSelecionado"
+                                      placeholder="Mês">
                         <e-dropdownlist-fields text="Text" value="Value"></e-dropdownlist-fields>
                     </ejs-dropdownlist>
                 </div>
 
                 <div class="col-lg-4 col-md-12 d-flex justify-content-end gap-2">
-                    <ejs-button id="btnFiltrar" cssClass="btn-filtrar-royal" iconCss="fa-duotone fa-filter"
-                        content="Filtrar" data-ejtip="Aplicar Filtros"></ejs-button>
-                    <ejs-button id="btnExportExcel" cssClass="btn-export-royal" iconCss="fa-duotone fa-file-excel"
-                        content="Exportar Excel" data-ejtip="Exportar para Excel"></ejs-button>
+                    <ejs-button id="btnFiltrar" cssClass="btn-filtrar-royal" iconCss="fa-duotone fa-filter" content="Filtrar" data-ejtip="Aplicar Filtros"></ejs-button>
+                    <ejs-button id="btnExportExcel" cssClass="btn-export-royal" iconCss="fa-duotone fa-file-excel" content="Exportar Excel" data-ejtip="Exportar para Excel"></ejs-button>
                 </div>
             </div>
         </div>
@@ -105,30 +113,29 @@
         </div>
 
         <div class="card-body p-2">
-            <ejs-grid id="gridDetalhes" height="200" locale="pt-BR" allowPaging="true" allowSorting="true"
-                allowResizing="true" gridLines="Both" enableStickyHeader="true">
+            <ejs-grid id="gridDetalhes"
+                      height="200"
+                      locale="pt-BR"
+                      allowPaging="true"
+                      allowSorting="true"
+                      allowResizing="true"
+                      gridLines="Both"
+                      enableStickyHeader="true">
                 <e-grid-pagesettings pageSize="20"></e-grid-pagesettings>
                 <e-data-manager url="/glosa/detalhes" adaptor="WebApiAdaptor" crossDomain="true"></e-data-manager>
                 <e-grid-columns>
                     <e-grid-column field="numItem" headerText="# Item" width="110" textAlign="Right"></e-grid-column>
-                    <e-grid-column field="descricao" headerText="Descrição" width="260"
-                        clipMode="EllipsisWithTooltip"></e-grid-column>
+                    <e-grid-column field="descricao" headerText="Descrição" width="260" clipMode="EllipsisWithTooltip"></e-grid-column>
                     <e-grid-column field="placa" headerText="Placa" width="120" textAlign="Center"></e-grid-column>
-                    <e-grid-column field="dataSolicitacao" headerText="Solicitação" width="120"
-                        textAlign="Center"></e-grid-column>
-                    <e-grid-column field="dataDisponibilidade" headerText="Disponível" width="130"
-                        textAlign="Center"></e-grid-column>
-                    <e-grid-column field="dataRecolhimento" headerText="Recolhimento" width="130"
-                        textAlign="Center"></e-grid-column>
-                    <e-grid-column field="dataDevolucao" headerText="Retorno" width="120"
-                        textAlign="Center"></e-grid-column>
-                    <e-grid-column field="diasGlosa" headerText="Dias de Glosa" width="130"
-                        textAlign="Right"></e-grid-column>
+                    <e-grid-column field="dataSolicitacao" headerText="Solicitação" width="120" textAlign="Center"></e-grid-column>
+                    <e-grid-column field="dataDisponibilidade" headerText="Disponível" width="130" textAlign="Center"></e-grid-column>
+                    <e-grid-column field="dataRecolhimento" headerText="Recolhimento" width="130" textAlign="Center"></e-grid-column>
+                    <e-grid-column field="dataDevolucao" headerText="Retorno" width="120" textAlign="Center"></e-grid-column>
+                    <e-grid-column field="diasGlosa" headerText="Dias de Glosa" width="130" textAlign="Right"></e-grid-column>
                 </e-grid-columns>
                 <e-grid-aggregates>
                     <e-grid-aggregate-rows>
-                        <e-grid-aggregate-columns field="diasGlosa" type="Sum"
-                            footerTemplate="Total de dias: ${Sum}"></e-grid-aggregate-columns>
+                        <e-grid-aggregate-columns field="diasGlosa" type="Sum" footerTemplate="Total de dias: ${Sum}"></e-grid-aggregate-columns>
                     </e-grid-aggregate-rows>
                 </e-grid-aggregates>
             </ejs-grid>
@@ -145,8 +152,7 @@
 
 <div id="loadingOverlayGlosas" class="ftx-spin-overlay" style="z-index: 999999; cursor: wait; display: none;">
     <div class="ftx-spin-box" style="text-align: center; min-width: 300px;">
-        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo"
-            style="display: block;" />
+        <img src="/images/logo_gota_frotix_transparente.png" alt="FrotiX" class="ftx-loading-logo" style="display: block;" />
         <div class="ftx-loading-bar"></div>
         <div class="ftx-loading-text">Carregando Glosas...</div>
         <div class="ftx-loading-subtext">Aguarde, por favor</div>
@@ -163,54 +169,30 @@
 
 @section ScriptsBlock {
 
-    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css"
-        crossorigin="anonymous" />
-    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js"
-        crossorigin="anonymous"></script>
+    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" crossorigin="anonymous" />
+    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
 
     <ejs-scripts></ejs-scripts>
 
     <script>
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * FUNÇÕES DE LOADING - PADRÃO FROTIX
-             * ═══════════════════════════════════════════════════════════════════════════
-             * @@description Controla overlay de carregamento com contador para múltiplos
-            * grids carregando simultaneamente.
-             */
-
-        /**
-         * Exibe o overlay de loading na tela.
-         */
+
         function mostrarLoading() {
             const overlay = document.getElementById('loadingOverlayGlosas');
             if (overlay) overlay.style.display = 'flex';
         }
 
-        /**
-         * Oculta o overlay de loading.
-         */
         function esconderLoading() {
             const overlay = document.getElementById('loadingOverlayGlosas');
             if (overlay) overlay.style.display = 'none';
         }
 
-            /** @@type { number } Contador para controlar múltiplos grids carregando */
         let gridsCarregando = 0;
 
-            /**
-             * Incrementa contador e exibe loading.
-             * @@description Chamada no início do carregamento de cada grid.
-             */
         function iniciarCarregamentoGrid() {
             gridsCarregando++;
             mostrarLoading();
         }
 
-            /**
-             * Decrementa contador e oculta loading quando todos finalizam.
-             * @@description Chamada no dataBound de cada grid Syncfusion.
-             */
         function finalizarCarregamentoGrid() {
             gridsCarregando--;
             if (gridsCarregando <= 0) {
@@ -219,19 +201,8 @@
             }
         }
 
-            /**
-             * ═══════════════════════════════════════════════════════════════════════════
-             * HANDLERS GLOBAIS DOS GRIDS SYNCFUSION
-             * ═══════════════════════════════════════════════════════════════════════════
-             */
-
-            /**
-             * Handler global do dataBound - aplica estilo em linhas com glosa positiva.
-             * @@description Percorre as linhas do grid e adiciona classe CSS para destacar
-            * registros onde o valor de glosa é positivo(> 0).
-             * @@param { Object } args - Argumentos do evento dataBound do Syncfusion Grid.
-             */
-            window.onResumoDataBound = function (args) {
+        /** Handler GLOBAL do dataBound – robusto */
+        window.onResumoDataBound = function (args) {
             try {
                 const grid = this || document.getElementById('gridResumo')?.ej2_instances?.[0];
                 if (!grid || !grid.getRows) return;
@@ -251,11 +222,7 @@
             }
         };
 
-            /**
-             * Handler global do created - conecta eventos de loading ao grid.
-             * @@description Sobrescreve dataBound para incluir finalizarCarregamentoGrid
-            * e configura actionFailure para tratamento de erros.
-             */
+        /** Handler GLOBAL do created – apenas conecta eventos, sem chamar dataBound manualmente */
         window.onGridCreated = function () {
             try {
                 const gridInstance = this;
@@ -333,145 +300,106 @@
                     }
                 }, 100);
 
-                    /**
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     * HELPERS E FUNÇÕES AUXILIARES
-                     * ═══════════════════════════════════════════════════════════════════════════
-                     */
-
-                    /**
-                     * Atalho para obter instância EJ2 de um elemento pelo ID.
-                     * @@param { string } id - ID do elemento DOM.
-                     * @@returns { Object| undefined} Instância Syncfusion ou undefined.
-                     */
-            const $ej = (id) => document.getElementById(id)?.ej2_instances?.[0];
-
-                    /**
-                     * Formata número como moeda brasileira (R$).
-                     * @@param { number | string } v - Valor a formatar.
-                     * @@returns { string } Valor formatado como moeda.
-                     */
-            const toCurrency = (v) => new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(Number(v || 0));
-
-                    /**
-                     * Extrai array de dados de diferentes formatos de resposta da API.
-                     * @@param {*} payload - Resposta da API(pode ser array ou objeto com result).
-                     * @@returns { Array } Array de dados extraído.
-                     */
-            const getArrayFromData = (payload) => {
-                if (Array.isArray(payload)) return payload;
-                if (payload && Array.isArray(payload.result)) return payload.result;
-                if (payload && payload.result && Array.isArray(payload.result.result)) return payload.result.result;
-                return [];
-            };
-
-                    /**
-                     * Constrói Query Syncfusion com parâmetros de filtro atuais.
-                     * @@description Lê valores dos ComboBoxes de Contrato, Ano e Mês.
-                     * @@returns { ej.data.Query } Query configurada com parâmetros.
-                     */
-            function buildQuery() {
-                try {
-                    const contratoId = $ej('ContratoId')?.value;
-                    const ano = $ej('Ano')?.value;
-                    const mes = $ej('Mes')?.value;
-
-                    return new ej.data.Query()
-                        .addParams('contratoId', contratoId || '')
-                        .addParams('ano', ano || '')
-                        .addParams('mes', mes || '');
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Glosas.cshtml", "buildQuery", error);
-                    return new ej.data.Query();
+                const $ej = (id) => document.getElementById(id)?.ej2_instances?.[0];
+                const toCurrency = (v) => new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(Number(v || 0));
+                const getArrayFromData = (payload) => {
+                    if (Array.isArray(payload)) return payload;
+                    if (payload && Array.isArray(payload.result)) return payload.result;
+                    if (payload && payload.result && Array.isArray(payload.result.result)) return payload.result.result;
+                    return [];
+                };
+
+                function buildQuery() {
+                    try {
+                        const contratoId = $ej('ContratoId')?.value;
+                        const ano = $ej('Ano')?.value;
+                        const mes = $ej('Mes')?.value;
+
+                        return new ej.data.Query()
+                            .addParams('contratoId', contratoId || '')
+                            .addParams('ano', ano || '')
+                            .addParams('mes', mes || '');
+                    } catch (error) {
+                        Alerta.TratamentoErroComLinha("Glosas.cshtml", "buildQuery", error);
+                        return new ej.data.Query();
+                    }
                 }
-            }
-
-                    /**
-                     * Aplica query de filtro a ambos os grids e atualiza cards de totais.
-                     * @@description Mostra loading, atualiza grids Resumo e Detalhes,
-                     * e dispara updateCardTotals.
-                     */
-            function applyQuery() {
-                try {
-                    const query = buildQuery();
-                    const gridResumo = $ej('gridResumo');
-                    const gridDetalhe = $ej('gridDetalhes');
-
-                    if (gridResumo || gridDetalhe) {
-                        if (gridResumo) iniciarCarregamentoGrid();
-                        if (gridDetalhe) iniciarCarregamentoGrid();
-                    }
-
-                    if (gridResumo) { gridResumo.query = query; gridResumo.refresh(); }
-                    if (gridDetalhe) { gridDetalhe.query = query; gridDetalhe.refresh(); }
-                    updateCardTotals();
-                } catch (error) {
-                    esconderLoading();
-                    Alerta.TratamentoErroComLinha("Glosas.cshtml", "applyQuery", error);
+
+                function applyQuery() {
+                    try {
+                        const query = buildQuery();
+                        const gridResumo = $ej('gridResumo');
+                        const gridDetalhe = $ej('gridDetalhes');
+
+                        if (gridResumo || gridDetalhe) {
+                            if (gridResumo) iniciarCarregamentoGrid();
+                            if (gridDetalhe) iniciarCarregamentoGrid();
+                        }
+
+                        if (gridResumo) { gridResumo.query = query; gridResumo.refresh(); }
+                        if (gridDetalhe) { gridDetalhe.query = query; gridDetalhe.refresh(); }
+                        updateCardTotals();
+                    } catch (error) {
+                        esconderLoading();
+                        Alerta.TratamentoErroComLinha("Glosas.cshtml", "applyQuery", error);
+                    }
                 }
-            }
-
-            const btnFiltrar = document.getElementById('btnFiltrar');
-            if (btnFiltrar) {
-                btnFiltrar.addEventListener('click', function () {
-                    try {
-                        applyQuery();
-                    } catch (error) {
-                        Alerta.TratamentoErroComLinha("Glosas.cshtml", "btnFiltrar.click", error);
-                    }
-                });
-            }
-
-            const btnExport = document.getElementById('btnExportExcel');
-            if (btnExport) {
-                btnExport.addEventListener('click', function () {
+
+                const btnFiltrar = document.getElementById('btnFiltrar');
+                if (btnFiltrar) {
+                    btnFiltrar.addEventListener('click', function () {
+                        try {
+                            applyQuery();
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Glosas.cshtml", "btnFiltrar.click", error);
+                        }
+                    });
+                }
+
+                const btnExport = document.getElementById('btnExportExcel');
+                if (btnExport) {
+                    btnExport.addEventListener('click', function () {
+                        try {
+                            const contratoId = $ej('ContratoId')?.value || '';
+                            const ano = $ej('Ano')?.value || '';
+                            const mes = $ej('Mes')?.value || '';
+                            const url = `/glosa/export?contratoId=${encodeURIComponent(contratoId)}&ano=${encodeURIComponent(ano)}&mes=${encodeURIComponent(mes)}`;
+                            window.location.href = url;
+                        } catch (error) {
+                            Alerta.TratamentoErroComLinha("Glosas.cshtml", "btnExportExcel.click", error);
+                        }
+                    });
+                }
+
+                async function updateCardTotals() {
                     try {
                         const contratoId = $ej('ContratoId')?.value || '';
                         const ano = $ej('Ano')?.value || '';
                         const mes = $ej('Mes')?.value || '';
-                        const url = `/glosa/export?contratoId=${encodeURIComponent(contratoId)}&ano=${encodeURIComponent(ano)}&mes=${encodeURIComponent(mes)}`;
-                        window.location.href = url;
+                        if (!contratoId || !ano || !mes) return;
+
+                        const qs = new URLSearchParams({ contratoId, ano, mes });
+                        const [rResumo, rDetalhes] = await Promise.all([
+                            fetch(`/glosa/resumo?${qs}`),
+                            fetch(`/glosa/detalhes?${qs}`)
+                        ]);
+
+                        const resumo = getArrayFromData(await rResumo.json());
+                        const detalhes = getArrayFromData(await rDetalhes.json());
+
+                        const sum = (arr, sel) => arr.reduce((a, b) => a + (Number(sel(b)) || 0), 0);
+
+                        document.getElementById('tot-resumo-total').textContent = toCurrency(sum(resumo, x => x.precoTotalMensal));
+                        document.getElementById('tot-resumo-glosa').textContent = toCurrency(sum(resumo, x => x.glosa));
+                        document.getElementById('tot-resumo-ateste').textContent = toCurrency(sum(resumo, x => x.valorParaAteste));
+                        document.getElementById('tot-detalhes-dias').textContent = String(sum(detalhes, x => x.diasGlosa));
                     } catch (error) {
-                        Alerta.TratamentoErroComLinha("Glosas.cshtml", "btnExportExcel.click", error);
-                    }
-                });
+                        Alerta.TratamentoErroComLinha("Glosas.cshtml", "updateCardTotals", error);
+                    }
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("Glosas.cshtml", "DOMContentLoaded", error);
             }
-
-                    /**
-                     * Atualiza cards de totais via fetch assíncrono.
-                     * @@description Busca dados dos endpoints / glosa / resumo e / glosa / detalhes,
-                     * calcula somas e atualiza elementos DOM dos cards.
-                     * @@async
-                     */
-            async function updateCardTotals() {
-                try {
-                    const contratoId = $ej('ContratoId')?.value || '';
-                    const ano = $ej('Ano')?.value || '';
-                    const mes = $ej('Mes')?.value || '';
-                    if (!contratoId || !ano || !mes) return;
-
-                    const qs = new URLSearchParams({ contratoId, ano, mes });
-                    const [rResumo, rDetalhes] = await Promise.all([
-                        fetch(`/glosa/resumo?${qs}`),
-                        fetch(`/glosa/detalhes?${qs}`)
-                    ]);
-
-                    const resumo = getArrayFromData(await rResumo.json());
-                    const detalhes = getArrayFromData(await rDetalhes.json());
-
-                    const sum = (arr, sel) => arr.reduce((a, b) => a + (Number(sel(b)) || 0), 0);
-
-                    document.getElementById('tot-resumo-total').textContent = toCurrency(sum(resumo, x => x.precoTotalMensal));
-                    document.getElementById('tot-resumo-glosa').textContent = toCurrency(sum(resumo, x => x.glosa));
-                    document.getElementById('tot-resumo-ateste').textContent = toCurrency(sum(resumo, x => x.valorParaAteste));
-                    document.getElementById('tot-detalhes-dias').textContent = String(sum(detalhes, x => x.diasGlosa));
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha("Glosas.cshtml", "updateCardTotals", error);
-                }
-            }
-        } catch (error) {
-            Alerta.TratamentoErroComLinha("Glosas.cshtml", "DOMContentLoaded", error);
-        }
-            });
+        });
     </script>
 }
```
