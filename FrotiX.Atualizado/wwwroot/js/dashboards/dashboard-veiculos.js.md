# wwwroot/js/dashboards/dashboard-veiculos.js

**Mudanca:** GRANDE | **+401** linhas | **-457** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-veiculos.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-veiculos.js
@@ -8,17 +8,9 @@
     cream: '#e8f2ed',
 
     chart: [
-        '#5f8575',
-        '#7aa390',
-        '#8fb8a4',
-        '#4a6b5c',
-        '#3a5548',
-        '#14b8a6',
-        '#10b981',
-        '#06b6d4',
-        '#f59e0b',
-        '#8b5cf6',
-    ],
+        '#5f8575', '#7aa390', '#8fb8a4', '#4a6b5c', '#3a5548',
+        '#14b8a6', '#10b981', '#06b6d4', '#f59e0b', '#8b5cf6'
+    ]
 };
 
 let chartCategoria, chartStatus, chartOrigem, chartModelos, chartAnoFabricacao;
@@ -36,219 +28,216 @@
 });
 
 function initTabs() {
-    $('.dash-tab-veic').on('click', function () {
-        const tabId = $(this).data('tab');
-
-        $('.dash-tab-veic').removeClass('active');
-        $(this).addClass('active');
-
-        $('.dash-content-veic').removeClass('active');
-        $(`#tab-${tabId}`).addClass('active');
-
-        if (tabId === 'uso-veiculos' && !filtrosUsoInicializados) {
-            inicializarFiltrosUso();
-        } else if (tabId === 'custos' && !dadosCustos) {
-            carregarDadosCustos();
-        }
-    });
+    try {
+        $('.dash-tab-veic').on('click', function () {
+            const tabId = $(this).data('tab');
+
+            $('.dash-tab-veic').removeClass('active');
+            $(this).addClass('active');
+
+            $('.dash-content-veic').removeClass('active');
+            $(`#tab-${tabId}`).addClass('active');
+
+            if (tabId === 'uso-veiculos' && !filtrosUsoInicializados) {
+                inicializarFiltrosUso();
+            } else if (tabId === 'custos' && !dadosCustos) {
+                carregarDadosCustos();
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'initTabs', error);
+    }
 }
 
 function mostrarLoading(mensagem = 'Carregando...') {
-    $('#loadingOverlayVeic .ftx-loading-text').text(mensagem);
-    $('#loadingOverlayVeic').fadeIn(200);
+    try {
+        $('#loadingOverlayVeic .ftx-loading-text').text(mensagem);
+        $('#loadingOverlayVeic').fadeIn(200);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'mostrarLoading', error);
+    }
 }
 
 function esconderLoading() {
-    $('#loadingOverlayVeic').fadeOut(300);
+    try {
+        $('#loadingOverlayVeic').fadeOut(300);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'esconderLoading', error);
+    }
 }
 
 function carregarDadosGerais() {
-    mostrarLoading('Carregando dados da frota...');
-
-    $.ajax({
-        url: '/api/DashboardVeiculos/DashboardDados',
-        method: 'GET',
-        success: function (data) {
-            dadosGerais = data;
-            atualizarCardsGerais(data.totais);
-            renderizarGraficosGerais(data);
-            renderizarTabelasGerais(data);
-            esconderLoading();
-        },
-        error: function (xhr, status, error) {
-            console.error('Erro ao carregar dados gerais:', error);
-            esconderLoading();
-            mostrarErro('Erro ao carregar dados da frota');
-        },
-    });
+    try {
+        mostrarLoading('Carregando dados da frota...');
+
+        $.ajax({
+            url: '/api/DashboardVeiculos/DashboardDados',
+            method: 'GET',
+            success: function (data) {
+                dadosGerais = data;
+                atualizarCardsGerais(data.totais);
+                renderizarGraficosGerais(data);
+                renderizarTabelasGerais(data);
+                esconderLoading();
+            },
+            error: function (xhr, status, error) {
+                console.error('Erro ao carregar dados gerais:', error);
+                esconderLoading();
+                mostrarErro('Erro ao carregar dados da frota');
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosGerais', error);
+        esconderLoading();
+    }
 }
 
 function atualizarCardsGerais(totais) {
-    $('#totalVeiculos').text(totais.totalVeiculos.toLocaleString('pt-BR'));
-    $('#veiculosAtivos').text(totais.veiculosAtivos.toLocaleString('pt-BR'));
-    $('#veiculosInativos').text(
-        totais.veiculosInativos.toLocaleString('pt-BR'),
-    );
-    $('#veiculosReserva').text(totais.veiculosReserva.toLocaleString('pt-BR'));
-    $('#veiculosEfetivos').text(
-        totais.veiculosEfetivos.toLocaleString('pt-BR'),
-    );
-    $('#veiculosProprios').text(
-        totais.veiculosProprios.toLocaleString('pt-BR'),
-    );
-    $('#veiculosLocados').text(totais.veiculosLocados.toLocaleString('pt-BR'));
-    $('#idadeMedia').text(totais.idadeMedia.toFixed(1) + ' anos');
-    $('#valorMensalTotal').text(formatarMoeda(totais.valorMensalTotal));
+    try {
+        $('#totalVeiculos').text(totais.totalVeiculos.toLocaleString('pt-BR'));
+        $('#veiculosAtivos').text(totais.veiculosAtivos.toLocaleString('pt-BR'));
+        $('#veiculosInativos').text(totais.veiculosInativos.toLocaleString('pt-BR'));
+        $('#veiculosReserva').text(totais.veiculosReserva.toLocaleString('pt-BR'));
+        $('#veiculosEfetivos').text(totais.veiculosEfetivos.toLocaleString('pt-BR'));
+        $('#veiculosProprios').text(totais.veiculosProprios.toLocaleString('pt-BR'));
+        $('#veiculosLocados').text(totais.veiculosLocados.toLocaleString('pt-BR'));
+        $('#idadeMedia').text(totais.idadeMedia.toFixed(1) + ' anos');
+        $('#valorMensalTotal').text(formatarMoeda(totais.valorMensalTotal));
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsGerais', error);
+    }
 }
 
 function renderizarGraficosGerais(data) {
-
-    if (data.porCategoria && data.porCategoria.length > 0) {
-        renderizarChartPie(
-            'chartCategoria',
-            data.porCategoria.map((c) => ({
+    try {
+
+        if (data.porCategoria && data.porCategoria.length > 0) {
+            renderizarChartPie('chartCategoria', data.porCategoria.map(c => ({
                 x: c.categoria,
-                y: c.quantidade,
-            })),
-        );
-    }
-
-    if (data.porStatus && data.porStatus.length > 0) {
-        renderizarChartPie(
-            'chartStatus',
-            data.porStatus.map((s) => ({
+                y: c.quantidade
+            })));
+        }
+
+        if (data.porStatus && data.porStatus.length > 0) {
+            renderizarChartPie('chartStatus', data.porStatus.map(s => ({
                 x: s.status,
-                y: s.quantidade,
-            })),
-            ['#10b981', '#64748b'],
-        );
-    }
-
-    if (data.porOrigem && data.porOrigem.length > 0) {
-        renderizarChartPie(
-            'chartOrigem',
-            data.porOrigem.map((o) => ({
+                y: s.quantidade
+            })), ['#10b981', '#64748b']);
+        }
+
+        if (data.porOrigem && data.porOrigem.length > 0) {
+            renderizarChartPie('chartOrigem', data.porOrigem.map(o => ({
                 x: o.origem,
-                y: o.quantidade,
-            })),
-            ['#5f8575', '#f59e0b', '#06b6d4'],
-        );
-    }
-
-    if (data.porModelo && data.porModelo.length > 0) {
-        renderizarChartBarH(
-            'chartModelos',
-            data.porModelo.map((m) => ({
-                x:
-                    m.modelo.length > 25
-                        ? m.modelo.substring(0, 22) + '...'
-                        : m.modelo,
-                y: m.quantidade,
-            })),
-        );
-    }
-
-    if (data.porAnoFabricacao && data.porAnoFabricacao.length > 0) {
-        renderizarChartColumn(
-            'chartAnoFabricacao',
-            data.porAnoFabricacao.map((a) => ({
+                y: o.quantidade
+            })), ['#5f8575', '#f59e0b', '#06b6d4']);
+        }
+
+        if (data.porModelo && data.porModelo.length > 0) {
+            renderizarChartBarH('chartModelos', data.porModelo.map(m => ({
+                x: m.modelo.length > 25 ? m.modelo.substring(0, 22) + '...' : m.modelo,
+                y: m.quantidade
+            })));
+        }
+
+        if (data.porAnoFabricacao && data.porAnoFabricacao.length > 0) {
+            renderizarChartColumn('chartAnoFabricacao', data.porAnoFabricacao.map(a => ({
                 x: a.ano.toString(),
-                y: a.quantidade,
-            })),
-        );
+                y: a.quantidade
+            })));
+        }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosGerais', error);
     }
 }
 
 function renderizarTabelasGerais(data) {
-
-    let htmlCategoria = '';
-    if (data.porCategoria && data.porCategoria.length > 0) {
-        data.porCategoria.forEach((c) => {
+    try {
+
+        let htmlCategoria = '';
+        if (data.porCategoria && data.porCategoria.length > 0) {
+            data.porCategoria.forEach(c => {
+                htmlCategoria += `
+                    <div class="grid-row">
+                        <div class="grid-cell">${c.categoria}</div>
+                        <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
+                    </div>
+                `;
+            });
+
+            const totalCat = data.porCategoria.reduce((sum, c) => sum + c.quantidade, 0);
             htmlCategoria += `
-                <div class="grid-row">
-                    <div class="grid-cell">${c.categoria}</div>
-                    <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
+                <div class="grid-row grid-row-total">
+                    <div class="grid-cell"><strong>TOTAL</strong></div>
+                    <div class="grid-cell text-end"><strong>${totalCat}</strong></div>
                 </div>
             `;
-        });
-
-        const totalCat = data.porCategoria.reduce(
-            (sum, c) => sum + c.quantidade,
-            0,
-        );
-        htmlCategoria += `
-            <div class="grid-row grid-row-total">
-                <div class="grid-cell"><strong>TOTAL</strong></div>
-                <div class="grid-cell text-end"><strong>${totalCat}</strong></div>
-            </div>
-        `;
-    } else {
-        htmlCategoria =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
-    }
-    $('#tabelaCategoria').html(htmlCategoria);
-
-    let htmlCombustivel = '';
-    if (data.porCombustivel && data.porCombustivel.length > 0) {
-        data.porCombustivel.forEach((c) => {
-            htmlCombustivel += `
-                <div class="grid-row">
-                    <div class="grid-cell">${c.combustivel}</div>
-                    <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
-                </div>
-            `;
-        });
-    } else {
-        htmlCombustivel =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
-    }
-    $('#tabelaCombustivel').html(htmlCombustivel);
-
-    let htmlUnidade = '';
-    if (data.porUnidade && data.porUnidade.length > 0) {
-        data.porUnidade.forEach((u) => {
-            htmlUnidade += `
-                <div class="grid-row">
-                    <div class="grid-cell">${u.unidade}</div>
-                    <div class="grid-cell text-end"><strong>${u.quantidade}</strong></div>
-                </div>
-            `;
-        });
-    } else {
-        htmlUnidade =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
-    }
-    $('#tabelaUnidade').html(htmlUnidade);
-
-    let htmlTopKm = '';
-    if (data.topKm && data.topKm.length > 0) {
-        data.topKm.forEach((v, i) => {
-            const badgeClass = i < 3 ? 'top3' : '';
-            htmlTopKm += `
-                <div class="grid-row">
-                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
-                    <div class="grid-cell">
-                        <strong>${v.placa}</strong>
-                        <small class="d-block text-muted">${v.modelo}</small>
+        } else {
+            htmlCategoria = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
+        }
+        $('#tabelaCategoria').html(htmlCategoria);
+
+        let htmlCombustivel = '';
+        if (data.porCombustivel && data.porCombustivel.length > 0) {
+            data.porCombustivel.forEach(c => {
+                htmlCombustivel += `
+                    <div class="grid-row">
+                        <div class="grid-cell">${c.combustivel}</div>
+                        <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
                     </div>
-                    <div class="grid-cell text-end"><strong>${v.km.toLocaleString('pt-BR')} km</strong></div>
-                </div>
-            `;
-        });
-    } else {
-        htmlTopKm =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 3; text-align: center;">Nenhum dado encontrado</div></div>';
-    }
-    $('#tabelaTopKm').html(htmlTopKm);
+                `;
+            });
+        } else {
+            htmlCombustivel = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
+        }
+        $('#tabelaCombustivel').html(htmlCombustivel);
+
+        let htmlUnidade = '';
+        if (data.porUnidade && data.porUnidade.length > 0) {
+            data.porUnidade.forEach(u => {
+                htmlUnidade += `
+                    <div class="grid-row">
+                        <div class="grid-cell">${u.unidade}</div>
+                        <div class="grid-cell text-end"><strong>${u.quantidade}</strong></div>
+                    </div>
+                `;
+            });
+        } else {
+            htmlUnidade = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
+        }
+        $('#tabelaUnidade').html(htmlUnidade);
+
+        let htmlTopKm = '';
+        if (data.topKm && data.topKm.length > 0) {
+            data.topKm.forEach((v, i) => {
+                const badgeClass = i < 3 ? 'top3' : '';
+                htmlTopKm += `
+                    <div class="grid-row">
+                        <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
+                        <div class="grid-cell">
+                            <strong>${v.placa}</strong>
+                            <small class="d-block text-muted">${v.modelo}</small>
+                        </div>
+                        <div class="grid-cell text-end"><strong>${v.km.toLocaleString('pt-BR')} km</strong></div>
+                    </div>
+                `;
+            });
+        } else {
+            htmlTopKm = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 3; text-align: center;">Nenhum dado encontrado</div></div>';
+        }
+        $('#tabelaTopKm').html(htmlTopKm);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasGerais', error);
+    }
 }
 
 let filtroUsoAtual = { tipo: 'todos' };
 let filtrosUsoInicializados = false;
 
 function inicializarFiltrosUso() {
-    mostrarLoading('Carregando estatísticas de uso...');
-
-    $.ajax({
+    try {
+        mostrarLoading('Carregando estatísticas de uso...');
+
+        $.ajax({
         url: '/api/DashboardVeiculos/DashboardUso',
         method: 'GET',
         data: {},
@@ -282,8 +271,8 @@
 
                     if (viagensPorMes.length > 0) {
                         const mesesComDados = viagensPorMes
-                            .filter((item) => item.total > 0)
-                            .map((item) => item.mes)
+                            .filter(item => item.total > 0)
+                            .map(item => item.mes)
                             .sort((a, b) => b - a);
 
                         if (mesesComDados.length > 0) {
@@ -293,17 +282,9 @@
 
                     if (mesSelecionado) {
                         $('#filtroMesUso').val(mesSelecionado);
-                        filtroUsoAtual = {
-                            tipo: 'anoMes',
-                            ano: anoMaisRecente.toString(),
-                            mes: mesSelecionado,
-                        };
+                        filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: mesSelecionado };
                     } else {
-                        filtroUsoAtual = {
-                            tipo: 'anoMes',
-                            ano: anoMaisRecente.toString(),
-                            mes: '',
-                        };
+                        filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: '' };
                     }
 
                     atualizarPeriodoAtualLabel();
@@ -317,11 +298,7 @@
                 },
                 error: function () {
 
-                    filtroUsoAtual = {
-                        tipo: 'anoMes',
-                        ano: anoMaisRecente.toString(),
-                        mes: '',
-                    };
+                    filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: '' };
                     atualizarPeriodoAtualLabel();
                     dadosUso = data;
                     filtrosUsoInicializados = true;
@@ -329,18 +306,23 @@
                     renderizarGraficosUso(data);
                     renderizarTabelasUso(data);
                     esconderLoading();
-                },
+                }
             });
         },
         error: function (xhr, status, error) {
             console.error('Erro ao inicializar filtros de uso:', error);
             esconderLoading();
             mostrarErro('Erro ao carregar estatísticas de uso');
-        },
+        }
     });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'inicializarFiltrosUso', error);
+        esconderLoading();
+    }
 }
 
 function carregarDadosUso(params = {}) {
+    try {
     mostrarLoading('Carregando estatísticas de uso...');
 
     $.ajax({
@@ -351,11 +333,7 @@
             dadosUso = data;
 
             if ($('#filtroAnoUso option').length <= 1) {
-                preencherSelectAnos(
-                    '#filtroAnoUso',
-                    data.anosDisponiveis,
-                    null,
-                );
+                preencherSelectAnos('#filtroAnoUso', data.anosDisponiveis, null);
             }
 
             atualizarCardsUso(data.totais);
@@ -367,26 +345,17 @@
             console.error('Erro ao carregar dados de uso:', error);
             esconderLoading();
             mostrarErro('Erro ao carregar estatísticas de uso');
-        },
+        }
     });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosUso', error);
+        esconderLoading();
+    }
 }
 
 function atualizarPeriodoAtualLabel() {
-    const meses = [
-        '',
-        'Janeiro',
-        'Fevereiro',
-        'Março',
-        'Abril',
-        'Maio',
-        'Junho',
-        'Julho',
-        'Agosto',
-        'Setembro',
-        'Outubro',
-        'Novembro',
-        'Dezembro',
-    ];
+    try {
+    const meses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
     let label = 'Exibindo todos os dados';
 
     if (filtroUsoAtual.tipo === 'anoMes') {
@@ -409,13 +378,21 @@
         label = `Período: Últimos ${filtroUsoAtual.dias} dias`;
     }
 
-    $('#periodoAtualLabelUso').text(label);
+        $('#periodoAtualLabelUso').text(label);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarPeriodoAtualLabel', error);
+    }
 }
 
 function formatarDataBR(dataStr) {
-    if (!dataStr) return '';
-    const partes = dataStr.split('-');
-    return `${partes[2]}/${partes[1]}/${partes[0]}`;
+    try {
+        if (!dataStr) return '';
+        const partes = dataStr.split('-');
+        return `${partes[2]}/${partes[1]}/${partes[0]}`;
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'formatarDataBR', error);
+        return '';
+    }
 }
 
 $(document).on('click', '#btnFiltrarAnoMesUso', function () {
@@ -502,9 +479,7 @@
 
     const hoje = new Date();
     const dataFim = hoje.toISOString().split('T')[0];
-    const dataInicio = new Date(hoje.getTime() - dias * 24 * 60 * 60 * 1000)
-        .toISOString()
-        .split('T')[0];
+    const dataInicio = new Date(hoje.getTime() - (dias * 24 * 60 * 60 * 1000)).toISOString().split('T')[0];
 
     $('#dataInicioUso').val(dataInicio);
     $('#dataFimUso').val(dataFim);
@@ -521,62 +496,47 @@
 });
 
 function atualizarCardsUso(totais) {
-    $('#totalViagensUso').text(totais.totalViagens.toLocaleString('pt-BR'));
-    $('#kmTotalRodado').text(
-        totais.kmTotalRodado.toLocaleString('pt-BR') + ' km',
-    );
-    $('#totalAbastecimentosUso').text(
-        totais.totalAbastecimentos.toLocaleString('pt-BR'),
-    );
-    $('#totalLitrosUso').text(
-        totais.totalLitros.toLocaleString('pt-BR', {
-            minimumFractionDigits: 0,
-            maximumFractionDigits: 0,
-        }) + ' L',
-    );
-    $('#valorAbastecimentoUso').text(
-        formatarMoeda(totais.valorTotalAbastecimento),
-    );
+    try {
+        $('#totalViagensUso').text(totais.totalViagens.toLocaleString('pt-BR'));
+        $('#kmTotalRodado').text(totais.kmTotalRodado.toLocaleString('pt-BR') + ' km');
+        $('#totalAbastecimentosUso').text(totais.totalAbastecimentos.toLocaleString('pt-BR'));
+        $('#totalLitrosUso').text(totais.totalLitros.toLocaleString('pt-BR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }) + ' L');
+        $('#valorAbastecimentoUso').text(formatarMoeda(totais.valorTotalAbastecimento));
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsUso', error);
+    }
 }
 
 function renderizarGraficosUso(data) {
-
-    const meses = [
-        'Jan',
-        'Fev',
-        'Mar',
-        'Abr',
-        'Mai',
-        'Jun',
-        'Jul',
-        'Ago',
-        'Set',
-        'Out',
-        'Nov',
-        'Dez',
-    ];
+    try {
+
+    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
     const dadosViagens = [];
     for (let i = 1; i <= 12; i++) {
-        const item = data.viagensPorMes.find((v) => v.mes === i);
+        const item = data.viagensPorMes.find(v => v.mes === i);
         dadosViagens.push({
             x: meses[i - 1],
-            y: item ? item.quantidade : 0,
+            y: item ? item.quantidade : 0
         });
     }
     renderizarChartArea('chartViagensMes', dadosViagens, CORES_VEIC.primary);
 
     const dadosAbast = [];
     for (let i = 1; i <= 12; i++) {
-        const item = data.abastecimentoPorMes.find((a) => a.mes === i);
+        const item = data.abastecimentoPorMes.find(a => a.mes === i);
         dadosAbast.push({
             x: meses[i - 1],
-            y: item ? item.valor : 0,
+            y: item ? item.valor : 0
         });
     }
-    renderizarChartArea('chartAbastecimentoMes', dadosAbast, '#f59e0b');
+        renderizarChartArea('chartAbastecimentoMes', dadosAbast, '#f59e0b');
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosUso', error);
+    }
 }
 
 function renderizarTabelasUso(data) {
+    try {
 
     let htmlViagens = '';
     if (data.topViagens && data.topViagens.length > 0) {
@@ -595,8 +555,7 @@
             `;
         });
     } else {
-        htmlViagens =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
+        htmlViagens = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaTopViagens').html(htmlViagens);
 
@@ -617,8 +576,7 @@
             `;
         });
     } else {
-        htmlAbast =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
+        htmlAbast = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaTopAbastecimento').html(htmlAbast);
 
@@ -639,8 +597,7 @@
             `;
         });
     } else {
-        htmlLitros =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
+        htmlLitros = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaTopLitros').html(htmlLitros);
 
@@ -661,8 +618,7 @@
             `;
         });
     } else {
-        htmlConsumo =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
+        htmlConsumo = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaTopConsumo').html(htmlConsumo);
 
@@ -683,18 +639,21 @@
             `;
         });
     } else {
-        htmlEficiencia =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
+        htmlEficiencia = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaTopEficiencia').html(htmlEficiencia);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasUso', error);
+    }
 }
 
 function carregarDadosCustos(ano = null) {
-    mostrarLoading('Carregando dados de custos...');
-
-    const params = ano ? { ano: ano } : {};
-
-    $.ajax({
+    try {
+        mostrarLoading('Carregando dados de custos...');
+
+        const params = ano ? { ano: ano } : {};
+
+        $.ajax({
         url: '/api/DashboardVeiculos/DashboardCustos',
         method: 'GET',
         data: params,
@@ -704,17 +663,9 @@
             if ($('#filtroAnoCusto option').length <= 1) {
 
                 if (dadosUso && dadosUso.anosDisponiveis) {
-                    preencherSelectAnos(
-                        '#filtroAnoCusto',
-                        dadosUso.anosDisponiveis,
-                        data.anoSelecionado,
-                    );
+                    preencherSelectAnos('#filtroAnoCusto', dadosUso.anosDisponiveis, data.anoSelecionado);
                 } else {
-                    preencherSelectAnos(
-                        '#filtroAnoCusto',
-                        [new Date().getFullYear()],
-                        data.anoSelecionado,
-                    );
+                    preencherSelectAnos('#filtroAnoCusto', [new Date().getFullYear()], data.anoSelecionado);
                 }
             }
 
@@ -727,37 +678,29 @@
             console.error('Erro ao carregar dados de custos:', error);
             esconderLoading();
             mostrarErro('Erro ao carregar dados de custos');
-        },
+        }
     });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosCustos', error);
+        esconderLoading();
+    }
 }
 
 function atualizarCardsCustos(totais) {
-    $('#custoAbastecimento').text(formatarMoeda(totais.totalAbastecimento));
-    $('#custoManutencao').text(formatarMoeda(totais.totalManutencao));
-    $('#qtdAbastecimentosCusto').text(
-        totais.qtdAbastecimentos.toLocaleString('pt-BR'),
-    );
-    $('#qtdManutencoesCusto').text(
-        totais.qtdManutencoes.toLocaleString('pt-BR'),
-    );
+    try {
+        $('#custoAbastecimento').text(formatarMoeda(totais.totalAbastecimento));
+        $('#custoManutencao').text(formatarMoeda(totais.totalManutencao));
+        $('#qtdAbastecimentosCusto').text(totais.qtdAbastecimentos.toLocaleString('pt-BR'));
+        $('#qtdManutencoesCusto').text(totais.qtdManutencoes.toLocaleString('pt-BR'));
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsCustos', error);
+    }
 }
 
 function renderizarGraficosCustos(data) {
-
-    const meses = [
-        'Jan',
-        'Fev',
-        'Mar',
-        'Abr',
-        'Mai',
-        'Jun',
-        'Jul',
-        'Ago',
-        'Set',
-        'Out',
-        'Nov',
-        'Dez',
-    ];
+    try {
+
+    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
     const seriesAbast = [];
     const seriesManut = [];
 
@@ -766,32 +709,26 @@
         seriesManut.push({ x: meses[i], y: item.manutencao });
     });
 
-    renderizarChartColumnGrouped(
-        'chartComparativoMensal',
-        seriesAbast,
-        seriesManut,
-        'Abastecimento',
-        'Manutenção',
-    );
+    renderizarChartColumnGrouped('chartComparativoMensal', seriesAbast, seriesManut, 'Abastecimento', 'Manutenção');
 
     if (data.custoPorCategoria && data.custoPorCategoria.length > 0) {
-        renderizarChartBarH(
-            'chartCustoCategoria',
-            data.custoPorCategoria.map((c) => ({
-                x: c.categoria,
-                y: c.valorAbastecimento,
-            })),
-            '#f59e0b',
-        );
+        renderizarChartBarH('chartCustoCategoria', data.custoPorCategoria.map(c => ({
+            x: c.categoria,
+            y: c.valorAbastecimento
+        })), '#f59e0b');
+    }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosCustos', error);
     }
 }
 
 function renderizarTabelasCustos(data) {
+    try {
 
     let html = '';
     if (data.custoPorCategoria && data.custoPorCategoria.length > 0) {
         let total = 0;
-        data.custoPorCategoria.forEach((c) => {
+        data.custoPorCategoria.forEach(c => {
             total += c.valorAbastecimento;
             html += `
                 <div class="grid-row">
@@ -807,10 +744,12 @@
             </div>
         `;
     } else {
-        html =
-            '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
+        html = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
     }
     $('#tabelaCustoCategoria').html(html);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasCustos', error);
+    }
 }
 
 $(document).on('click', '#btnFiltrarCusto', function () {
@@ -819,46 +758,49 @@
 });
 
 function renderizarChartPie(containerId, dados, cores = CORES_VEIC.chart) {
-    const container = document.getElementById(containerId);
-    if (!container) return;
-    container.innerHTML = '';
-
-    const chart = new ej.charts.AccumulationChart({
-        series: [
-            {
-                dataSource: dados,
-                xName: 'x',
-                yName: 'y',
-                innerRadius: '50%',
-                palettes: cores,
-                dataLabel: {
-                    visible: true,
-                    position: 'Outside',
-                    name: 'x',
-                    font: { fontWeight: '600', size: '11px' },
-                    connectorStyle: { length: '10px', type: 'Curve' },
-                },
-                explode: true,
-                explodeOffset: '5%',
-                explodeIndex: 0,
+    try {
+        const container = document.getElementById(containerId);
+        if (!container) return;
+        container.innerHTML = '';
+
+        const chart = new ej.charts.AccumulationChart({
+        series: [{
+            dataSource: dados,
+            xName: 'x',
+            yName: 'y',
+            innerRadius: '50%',
+            palettes: cores,
+            dataLabel: {
+                visible: true,
+                position: 'Outside',
+                name: 'x',
+                font: { fontWeight: '600', size: '11px' },
+                connectorStyle: { length: '10px', type: 'Curve' }
             },
-        ],
+            explode: true,
+            explodeOffset: '5%',
+            explodeIndex: 0
+        }],
         legendSettings: {
             visible: true,
             position: 'Bottom',
-            textStyle: { size: '11px' },
+            textStyle: { size: '11px' }
         },
         tooltip: {
             enable: true,
-            format: '${point.x}: <b>${point.y}</b>',
+            format: '${point.x}: <b>${point.y}</b>'
         },
         background: 'transparent',
-        enableSmartLabels: true,
+        enableSmartLabels: true
     });
     chart.appendTo(container);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartPie', error);
+    }
 }
 
 function renderizarChartBarH(containerId, dados, cor = CORES_VEIC.primary) {
+    try {
     const container = document.getElementById(containerId);
     if (!container) return;
     container.innerHTML = '';
@@ -867,38 +809,34 @@
         primaryXAxis: {
             valueType: 'Category',
             labelStyle: { size: '10px' },
-            majorGridLines: { width: 0 },
+            majorGridLines: { width: 0 }
         },
         primaryYAxis: {
             labelFormat: '{value}',
             labelStyle: { size: '10px' },
-            majorGridLines: { dashArray: '3,3' },
-        },
-        series: [
-            {
-                dataSource: dados,
-                xName: 'x',
-                yName: 'y',
-                type: 'Bar',
-                fill: cor,
-                cornerRadius: { topLeft: 4, topRight: 4 },
-                marker: {
-                    dataLabel: {
-                        visible: true,
-                        position: 'Top',
-                        font: { size: '10px', fontWeight: '600' },
-                    },
-                },
-            },
-        ],
+            majorGridLines: { dashArray: '3,3' }
+        },
+        series: [{
+            dataSource: dados,
+            xName: 'x',
+            yName: 'y',
+            type: 'Bar',
+            fill: cor,
+            cornerRadius: { topLeft: 4, topRight: 4 },
+            marker: { dataLabel: { visible: true, position: 'Top', font: { size: '10px', fontWeight: '600' } } }
+        }],
         tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
         chartArea: { border: { width: 0 } },
-        background: 'transparent',
+        background: 'transparent'
     });
     chart.appendTo(container);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartBarH', error);
+    }
 }
 
 function renderizarChartColumn(containerId, dados, cor = CORES_VEIC.primary) {
+    try {
     const container = document.getElementById(containerId);
     if (!container) return;
     container.innerHTML = '';
@@ -907,38 +845,34 @@
         primaryXAxis: {
             valueType: 'Category',
             labelStyle: { size: '10px' },
-            majorGridLines: { width: 0 },
+            majorGridLines: { width: 0 }
         },
         primaryYAxis: {
             labelFormat: '{value}',
             labelStyle: { size: '10px' },
-            majorGridLines: { dashArray: '3,3' },
-        },
-        series: [
-            {
-                dataSource: dados,
-                xName: 'x',
-                yName: 'y',
-                type: 'Column',
-                fill: cor,
-                cornerRadius: { topLeft: 4, topRight: 4 },
-                marker: {
-                    dataLabel: {
-                        visible: true,
-                        position: 'Top',
-                        font: { size: '10px', fontWeight: '600' },
-                    },
-                },
-            },
-        ],
+            majorGridLines: { dashArray: '3,3' }
+        },
+        series: [{
+            dataSource: dados,
+            xName: 'x',
+            yName: 'y',
+            type: 'Column',
+            fill: cor,
+            cornerRadius: { topLeft: 4, topRight: 4 },
+            marker: { dataLabel: { visible: true, position: 'Top', font: { size: '10px', fontWeight: '600' } } }
+        }],
         tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
         chartArea: { border: { width: 0 } },
-        background: 'transparent',
+        background: 'transparent'
     });
     chart.appendTo(container);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartColumn', error);
+    }
 }
 
 function renderizarChartArea(containerId, dados, cor = CORES_VEIC.primary) {
+    try {
     const container = document.getElementById(containerId);
     if (!container) return;
     container.innerHTML = '';
@@ -947,45 +881,41 @@
         primaryXAxis: {
             valueType: 'Category',
             labelStyle: { size: '10px' },
-            majorGridLines: { width: 0 },
+            majorGridLines: { width: 0 }
         },
         primaryYAxis: {
             labelFormat: '{value}',
             labelStyle: { size: '10px' },
-            majorGridLines: { dashArray: '3,3' },
-        },
-        series: [
-            {
-                dataSource: dados,
-                xName: 'x',
-                yName: 'y',
-                type: 'SplineArea',
+            majorGridLines: { dashArray: '3,3' }
+        },
+        series: [{
+            dataSource: dados,
+            xName: 'x',
+            yName: 'y',
+            type: 'SplineArea',
+            fill: cor,
+            opacity: 0.5,
+            border: { width: 2, color: cor },
+            marker: {
+                visible: true,
+                width: 7,
+                height: 7,
                 fill: cor,
-                opacity: 0.5,
-                border: { width: 2, color: cor },
-                marker: {
-                    visible: true,
-                    width: 7,
-                    height: 7,
-                    fill: cor,
-                    border: { width: 2, color: '#fff' },
-                },
-            },
-        ],
+                border: { width: 2, color: '#fff' }
+            }
+        }],
         tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
         chartArea: { border: { width: 0 } },
-        background: 'transparent',
+        background: 'transparent'
     });
     chart.appendTo(container);
-}
-
-function renderizarChartColumnGrouped(
-    containerId,
-    series1,
-    series2,
-    nome1,
-    nome2,
-) {
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartArea', error);
+    }
+}
+
+function renderizarChartColumnGrouped(containerId, series1, series2, nome1, nome2) {
+    try {
     const container = document.getElementById(containerId);
     if (!container) return;
     container.innerHTML = '';
@@ -994,12 +924,12 @@
         primaryXAxis: {
             valueType: 'Category',
             labelStyle: { size: '10px' },
-            majorGridLines: { width: 0 },
+            majorGridLines: { width: 0 }
         },
         primaryYAxis: {
             labelFormat: 'R$ {value}',
             labelStyle: { size: '10px' },
-            majorGridLines: { dashArray: '3,3' },
+            majorGridLines: { dashArray: '3,3' }
         },
         series: [
             {
@@ -1009,7 +939,7 @@
                 name: nome1,
                 type: 'Column',
                 fill: '#f59e0b',
-                cornerRadius: { topLeft: 3, topRight: 3 },
+                cornerRadius: { topLeft: 3, topRight: 3 }
             },
             {
                 dataSource: series2,
@@ -1018,60 +948,74 @@
                 name: nome2,
                 type: 'Column',
                 fill: CORES_VEIC.primary,
-                cornerRadius: { topLeft: 3, topRight: 3 },
-            },
+                cornerRadius: { topLeft: 3, topRight: 3 }
+            }
         ],
         legendSettings: { visible: true, position: 'Top' },
         tooltip: {
             enable: true,
             shared: true,
-            format: '${series.name}: <b>${point.y}</b>',
+            format: '${series.name}: <b>${point.y}</b>'
         },
         chartArea: { border: { width: 0 } },
-        background: 'transparent',
+        background: 'transparent'
     });
     chart.appendTo(container);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartColumnGrouped', error);
+    }
 }
 
 function formatarMoeda(valor) {
-    if (valor === null || valor === undefined) return 'R$ 0,00';
-    return valor.toLocaleString('pt-BR', {
-        style: 'currency',
-        currency: 'BRL',
-        minimumFractionDigits: 2,
-    });
+    try {
+        if (valor === null || valor === undefined) return 'R$ 0,00';
+        return valor.toLocaleString('pt-BR', {
+            style: 'currency',
+            currency: 'BRL',
+            minimumFractionDigits: 2
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'formatarMoeda', error);
+        return 'R$ 0,00';
+    }
 }
 
 function preencherSelectAnos(seletor, anos, anoSelecionado) {
-    const $select = $(seletor);
-    $select.empty();
-
-    $select.append('<option value="">&lt;Todos os Anos&gt;</option>');
-
-    if (anos && anos.length > 0) {
-        anos.forEach((ano) => {
-            const selected = ano === anoSelecionado ? 'selected' : '';
-            $select.append(
-                `<option value="${ano}" ${selected}>${ano}</option>`,
-            );
-        });
-    } else {
-        const anoAtual = new Date().getFullYear();
-        $select.append(
-            `<option value="${anoAtual}" selected>${anoAtual}</option>`,
-        );
+    try {
+        const $select = $(seletor);
+        $select.empty();
+
+        $select.append('<option value="">&lt;Todos os Anos&gt;</option>');
+
+        if (anos && anos.length > 0) {
+            anos.forEach(ano => {
+                const selected = ano === anoSelecionado ? 'selected' : '';
+                $select.append(`<option value="${ano}" ${selected}>${ano}</option>`);
+            });
+        } else {
+            const anoAtual = new Date().getFullYear();
+            $select.append(`<option value="${anoAtual}" selected>${anoAtual}</option>`);
+        }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'preencherSelectAnos', error);
     }
 }
 
 function mostrarErro(mensagem) {
-    if (typeof Swal !== 'undefined') {
-        Swal.fire({
-            icon: 'error',
-            title: 'Erro',
-            text: mensagem,
-            confirmButtonColor: CORES_VEIC.primary,
-        });
-    } else {
-        alert(mensagem);
-    }
-}
+    try {
+        if (typeof Swal !== 'undefined') {
+            Swal.fire({
+                icon: 'error',
+                title: 'Erro',
+                text: mensagem,
+                confirmButtonColor: CORES_VEIC.primary
+            });
+        } else if (typeof AppToast !== 'undefined') {
+            AppToast.show('error', mensagem);
+        } else {
+            console.error('[dashboard-veiculos.js] Erro crítico (SweetAlert e AppToast indisponíveis):', mensagem);
+        }
+    } catch (error) {
+        console.error('[dashboard-veiculos.js] Erro ao exibir mensagem de erro:', error);
+    }
+}
```
