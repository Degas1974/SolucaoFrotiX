# wwwroot/js/dashboards/dashboard-lavagem.js

**Mudanca:** GRANDE | **+167** linhas | **-530** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-lavagem.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-lavagem.js
@@ -4,7 +4,6 @@
 let chartTopLavadores = null;
 let chartTopVeiculos = null;
 let chartCategoria = null;
-let chartDuracao = null;
 
 const CORES_LAV = {
     primary: '#0891b2',
@@ -13,15 +12,7 @@
     dark: '#0e7490',
     light: '#ecfeff',
     gradient: ['#0891b2', '#06b6d4', '#22d3ee', '#67e8f9', '#a5f3fc'],
-    heatmap: [
-        '#ecfeff',
-        '#a5f3fc',
-        '#67e8f9',
-        '#22d3ee',
-        '#06b6d4',
-        '#0891b2',
-        '#0e7490',
-    ],
+    heatmap: ['#ecfeff', '#a5f3fc', '#67e8f9', '#22d3ee', '#06b6d4', '#0891b2', '#0e7490']
 };
 
 function popularAnosDisponiveis() {
@@ -53,21 +44,8 @@
         const dataInicio = document.getElementById('dataInicio')?.value;
         const dataFim = document.getElementById('dataFim')?.value;
 
-        const meses = [
-            '',
-            'Janeiro',
-            'Fevereiro',
-            'Março',
-            'Abril',
-            'Maio',
-            'Junho',
-            'Julho',
-            'Agosto',
-            'Setembro',
-            'Outubro',
-            'Novembro',
-            'Dezembro',
-        ];
+        const meses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                       'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
 
         if (dataInicio && dataFim) {
             const dtIni = new Date(dataInicio + 'T00:00:00');
@@ -94,19 +72,15 @@
 
         document.getElementById('dataInicio').value = '';
         document.getElementById('dataFim').value = '';
-        document
-            .querySelectorAll('.btn-period-lav')
-            .forEach((b) => b.classList.remove('active'));
+        document.querySelectorAll('.btn-period-lav').forEach(b => b.classList.remove('active'));
 
         if (!ano && !mes) {
             const anoAtual = new Date().getFullYear();
             const dataInicio = new Date(anoAtual - 5, 0, 1);
             const dataFim = new Date(anoAtual, 11, 31);
 
-            document.getElementById('dataInicio').value =
-                formatarDataInput(dataInicio);
-            document.getElementById('dataFim').value =
-                formatarDataInput(dataFim);
+            document.getElementById('dataInicio').value = formatarDataInput(dataInicio);
+            document.getElementById('dataFim').value = formatarDataInput(dataFim);
 
             atualizarLabelPeriodo();
             carregarDados();
@@ -134,8 +108,7 @@
             dataFim = new Date(anoAtual, mesNum, 0);
         }
 
-        document.getElementById('dataInicio').value =
-            formatarDataInput(dataInicio);
+        document.getElementById('dataInicio').value = formatarDataInput(dataInicio);
         document.getElementById('dataFim').value = formatarDataInput(dataFim);
 
         atualizarLabelPeriodo();
@@ -167,9 +140,7 @@
 
 function limparFiltroPeriodo() {
     try {
-        document
-            .querySelectorAll('.btn-period-lav')
-            .forEach((b) => b.classList.remove('active'));
+        document.querySelectorAll('.btn-period-lav').forEach(b => b.classList.remove('active'));
 
         const hoje = new Date();
         const inicio = new Date();
@@ -197,60 +168,40 @@
         document.getElementById('dataInicio').value = formatarDataInput(inicio);
         document.getElementById('dataFim').value = formatarDataInput(hoje);
 
-        document
-            .getElementById('btnFiltrarAnoMes')
-            ?.addEventListener('click', filtrarPorAnoMes);
-        document
-            .getElementById('btnLimparAnoMes')
-            ?.addEventListener('click', limparFiltroAnoMes);
-
-        document
-            .getElementById('btnFiltrarPeriodo')
-            ?.addEventListener('click', carregarDados);
-        document
-            .getElementById('btnLimparPeriodo')
-            ?.addEventListener('click', limparFiltroPeriodo);
-
-        document
-            .getElementById('dataInicio')
-            .addEventListener('change', function () {
-                document
-                    .querySelectorAll('.btn-period-lav')
-                    .forEach((b) => b.classList.remove('active'));
+        document.getElementById('btnFiltrarAnoMes')?.addEventListener('click', filtrarPorAnoMes);
+        document.getElementById('btnLimparAnoMes')?.addEventListener('click', limparFiltroAnoMes);
+
+        document.getElementById('btnFiltrarPeriodo')?.addEventListener('click', carregarDados);
+        document.getElementById('btnLimparPeriodo')?.addEventListener('click', limparFiltroPeriodo);
+
+        document.getElementById('dataInicio').addEventListener('change', function() {
+            document.querySelectorAll('.btn-period-lav').forEach(b => b.classList.remove('active'));
+
+            document.getElementById('filtroAno').value = '';
+            document.getElementById('filtroMes').value = '';
+        });
+        document.getElementById('dataFim').addEventListener('change', function() {
+            document.querySelectorAll('.btn-period-lav').forEach(b => b.classList.remove('active'));
+
+            document.getElementById('filtroAno').value = '';
+            document.getElementById('filtroMes').value = '';
+        });
+
+        document.querySelectorAll('.btn-period-lav').forEach(btn => {
+            btn.addEventListener('click', function () {
 
                 document.getElementById('filtroAno').value = '';
                 document.getElementById('filtroMes').value = '';
-            });
-        document
-            .getElementById('dataFim')
-            .addEventListener('change', function () {
-                document
-                    .querySelectorAll('.btn-period-lav')
-                    .forEach((b) => b.classList.remove('active'));
-
-                document.getElementById('filtroAno').value = '';
-                document.getElementById('filtroMes').value = '';
-            });
-
-        document.querySelectorAll('.btn-period-lav').forEach((btn) => {
-            btn.addEventListener('click', function () {
-
-                document.getElementById('filtroAno').value = '';
-                document.getElementById('filtroMes').value = '';
-
-                document
-                    .querySelectorAll('.btn-period-lav')
-                    .forEach((b) => b.classList.remove('active'));
+
+                document.querySelectorAll('.btn-period-lav').forEach(b => b.classList.remove('active'));
                 this.classList.add('active');
 
                 const dias = parseInt(this.dataset.dias);
                 const novoInicio = new Date();
                 novoInicio.setDate(hoje.getDate() - dias);
 
-                document.getElementById('dataInicio').value =
-                    formatarDataInput(novoInicio);
-                document.getElementById('dataFim').value =
-                    formatarDataInput(hoje);
+                document.getElementById('dataInicio').value = formatarDataInput(novoInicio);
+                document.getElementById('dataFim').value = formatarDataInput(hoje);
 
                 atualizarLabelPeriodo();
                 carregarDados();
@@ -280,9 +231,8 @@
             carregarTopVeiculos(dataInicio, dataFim),
             carregarHeatmap(dataInicio, dataFim),
             carregarCategoria(dataInicio, dataFim),
-            carregarDuracao(dataInicio, dataFim),
             carregarTabelaLavadores(dataInicio, dataFim),
-            carregarTabelaVeiculos(dataInicio, dataFim),
+            carregarTabelaVeiculos(dataInicio, dataFim)
         ]);
 
         ocultarLoading();
@@ -295,45 +245,29 @@
 
 async function carregarEstatisticasGerais(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/EstatisticasGerais?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/EstatisticasGerais?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success) {
-            document.getElementById('totalLavagens').textContent =
-                formatarNumero(result.totalLavagens);
-            document.getElementById('veiculosLavados').textContent =
-                formatarNumero(result.veiculosLavados);
-            document.getElementById('lavadoresAtivos').textContent =
-                formatarNumero(result.lavadoresAtivos);
-            document.getElementById('mediaDiaria').textContent =
-                result.mediaDiaria.toFixed(1);
-            document.getElementById('mediaPorVeiculo').textContent =
-                result.mediaPorVeiculo.toFixed(1);
-
-            document.getElementById('lavadorDestaqueNome').textContent =
-                result.lavadorDestaque?.nome || '-';
-            document.getElementById('lavadorDestaqueQtd').textContent =
-                `${result.lavadorDestaque?.quantidade || 0} lavagens`;
-
-            document.getElementById('veiculoMaisLavadoPlaca').textContent =
-                result.veiculoMaisLavado?.placa || '-';
-            document.getElementById('veiculoMaisLavadoQtd').textContent =
-                `${result.veiculoMaisLavado?.quantidade || 0} lavagens`;
-
-            document.getElementById('diaMaisMovimentado').textContent =
-                result.diaMaisMovimentado || '-';
-            document.getElementById('horarioPico').textContent =
-                result.horarioPico || '-';
+            document.getElementById('totalLavagens').textContent = formatarNumero(result.totalLavagens);
+            document.getElementById('veiculosLavados').textContent = formatarNumero(result.veiculosLavados);
+            document.getElementById('lavadoresAtivos').textContent = formatarNumero(result.lavadoresAtivos);
+            document.getElementById('mediaDiaria').textContent = result.mediaDiaria.toFixed(1);
+            document.getElementById('mediaPorVeiculo').textContent = result.mediaPorVeiculo.toFixed(1);
+
+            document.getElementById('lavadorDestaqueNome').textContent = result.lavadorDestaque?.nome || '-';
+            document.getElementById('lavadorDestaqueQtd').textContent = `${result.lavadorDestaque?.quantidade || 0} lavagens`;
+
+            document.getElementById('veiculoMaisLavadoPlaca').textContent = result.veiculoMaisLavado?.placa || '-';
+            document.getElementById('veiculoMaisLavadoQtd').textContent = `${result.veiculoMaisLavado?.quantidade || 0} lavagens`;
+
+            document.getElementById('diaMaisMovimentado').textContent = result.diaMaisMovimentado || '-';
+            document.getElementById('horarioPico').textContent = result.horarioPico || '-';
 
             const variacaoEl = document.getElementById('variacaoLavagens');
             const anterior = result.periodoAnterior?.totalLavagens || 0;
             if (anterior > 0) {
-                const variacao = (
-                    ((result.totalLavagens - anterior) / anterior) *
-                    100
-                ).toFixed(1);
+                const variacao = ((result.totalLavagens - anterior) / anterior * 100).toFixed(1);
                 if (variacao > 0) {
                     variacaoEl.className = 'variacao-metrica variacao-positiva';
                     variacaoEl.innerHTML = `<i class="fa-solid fa-arrow-up me-1"></i>+${variacao}%`;
@@ -355,9 +289,7 @@
 
 async function carregarGraficosDiaSemana(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/LavagensPorDiaSemana?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/LavagensPorDiaSemana?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -366,32 +298,24 @@
             chartDiaSemana = new ej.charts.Chart({
                 primaryXAxis: {
                     valueType: 'Category',
-                    labelStyle: { color: '#6B7280' },
+                    labelStyle: { color: '#6B7280' }
                 },
                 primaryYAxis: {
                     labelStyle: { color: '#6B7280' },
-                    minimum: 0,
-                },
-                series: [
-                    {
-                        dataSource: result.data,
-                        xName: 'dia',
-                        yName: 'quantidade',
-                        type: 'Column',
-                        fill: CORES_LAV.primary,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                        marker: {
-                            dataLabel: {
-                                visible: true,
-                                position: 'Top',
-                                font: { fontWeight: '600' },
-                            },
-                        },
-                    },
-                ],
+                    minimum: 0
+                },
+                series: [{
+                    dataSource: result.data,
+                    xName: 'dia',
+                    yName: 'quantidade',
+                    type: 'Column',
+                    fill: CORES_LAV.primary,
+                    cornerRadius: { topLeft: 4, topRight: 4 },
+                    marker: { dataLabel: { visible: true, position: 'Top', font: { fontWeight: '600' } } }
+                }],
                 tooltip: { enable: true },
                 chartArea: { border: { width: 0 } },
-                background: 'transparent',
+                background: 'transparent'
             });
 
             chartDiaSemana.appendTo('#chartDiaSemana');
@@ -403,9 +327,7 @@
 
 async function carregarGraficosHorario(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/LavagensPorHorario?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/LavagensPorHorario?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -415,26 +337,24 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { color: '#6B7280' },
-                    labelRotation: -45,
+                    labelRotation: -45
                 },
                 primaryYAxis: {
                     labelStyle: { color: '#6B7280' },
-                    minimum: 0,
-                },
-                series: [
-                    {
-                        dataSource: result.data,
-                        xName: 'hora',
-                        yName: 'quantidade',
-                        type: 'Area',
-                        fill: CORES_LAV.primary,
-                        opacity: 0.6,
-                        border: { width: 2, color: CORES_LAV.dark },
-                    },
-                ],
+                    minimum: 0
+                },
+                series: [{
+                    dataSource: result.data,
+                    xName: 'hora',
+                    yName: 'quantidade',
+                    type: 'Area',
+                    fill: CORES_LAV.primary,
+                    opacity: 0.6,
+                    border: { width: 2, color: CORES_LAV.dark }
+                }],
                 tooltip: { enable: true },
                 chartArea: { border: { width: 0 } },
-                background: 'transparent',
+                background: 'transparent'
             });
 
             chartHorario.appendTo('#chartHorario');
@@ -446,9 +366,7 @@
 
 async function carregarGraficosEvolucao() {
     try {
-        const response = await fetch(
-            '/api/DashboardLavagem/EvolucaoMensal?meses=12',
-        );
+        const response = await fetch('/api/DashboardLavagem/EvolucaoMensal?meses=12');
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -457,36 +375,30 @@
             chartEvolucao = new ej.charts.Chart({
                 primaryXAxis: {
                     valueType: 'Category',
-                    labelStyle: { color: '#6B7280' },
+                    labelStyle: { color: '#6B7280' }
                 },
                 primaryYAxis: {
                     labelStyle: { color: '#6B7280' },
-                    minimum: 0,
-                },
-                series: [
-                    {
-                        dataSource: result.data,
-                        xName: 'mes',
-                        yName: 'quantidade',
-                        type: 'Line',
+                    minimum: 0
+                },
+                series: [{
+                    dataSource: result.data,
+                    xName: 'mes',
+                    yName: 'quantidade',
+                    type: 'Line',
+                    fill: CORES_LAV.primary,
+                    width: 3,
+                    marker: {
+                        visible: true,
+                        width: 8,
+                        height: 8,
                         fill: CORES_LAV.primary,
-                        width: 3,
-                        marker: {
-                            visible: true,
-                            width: 8,
-                            height: 8,
-                            fill: CORES_LAV.primary,
-                            dataLabel: {
-                                visible: true,
-                                position: 'Top',
-                                font: { fontWeight: '600' },
-                            },
-                        },
-                    },
-                ],
+                        dataLabel: { visible: true, position: 'Top', font: { fontWeight: '600' } }
+                    }
+                }],
                 tooltip: { enable: true },
                 chartArea: { border: { width: 0 } },
-                background: 'transparent',
+                background: 'transparent'
             });
 
             chartEvolucao.appendTo('#chartEvolucao');
@@ -498,9 +410,7 @@
 
 async function carregarTopLavadores(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/TopLavadores?dataInicio=${dataInicio}&dataFim=${dataFim}&top=10`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/TopLavadores?dataInicio=${dataInicio}&dataFim=${dataFim}&top=10`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -509,37 +419,24 @@
             chartTopLavadores = new ej.charts.Chart({
                 primaryXAxis: {
                     valueType: 'Category',
-                    labelStyle: { color: '#6B7280' },
+                    labelStyle: { color: '#6B7280' }
                 },
                 primaryYAxis: {
                     labelStyle: { color: '#6B7280' },
-                    minimum: 0,
-                },
-                series: [
-                    {
-                        dataSource: result.data.reverse(),
-                        xName: 'nome',
-                        yName: 'quantidade',
-                        type: 'Bar',
-                        fill: CORES_LAV.secondary,
-                        cornerRadius: {
-                            topLeft: 4,
-                            topRight: 4,
-                            bottomLeft: 4,
-                            bottomRight: 4,
-                        },
-                        marker: {
-                            dataLabel: {
-                                visible: true,
-                                position: 'Top',
-                                font: { fontWeight: '600' },
-                            },
-                        },
-                    },
-                ],
+                    minimum: 0
+                },
+                series: [{
+                    dataSource: result.data.reverse(),
+                    xName: 'nome',
+                    yName: 'quantidade',
+                    type: 'Bar',
+                    fill: CORES_LAV.secondary,
+                    cornerRadius: { topLeft: 4, topRight: 4, bottomLeft: 4, bottomRight: 4 },
+                    marker: { dataLabel: { visible: true, position: 'Top', font: { fontWeight: '600' } } }
+                }],
                 tooltip: { enable: true },
                 chartArea: { border: { width: 0 } },
-                background: 'transparent',
+                background: 'transparent'
             });
 
             chartTopLavadores.appendTo('#chartTopLavadores');
@@ -551,9 +448,7 @@
 
 async function carregarTopVeiculos(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/TopVeiculos?dataInicio=${dataInicio}&dataFim=${dataFim}&top=10`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/TopVeiculos?dataInicio=${dataInicio}&dataFim=${dataFim}&top=10`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -562,37 +457,24 @@
             chartTopVeiculos = new ej.charts.Chart({
                 primaryXAxis: {
                     valueType: 'Category',
-                    labelStyle: { color: '#6B7280' },
+                    labelStyle: { color: '#6B7280' }
                 },
                 primaryYAxis: {
                     labelStyle: { color: '#6B7280' },
-                    minimum: 0,
-                },
-                series: [
-                    {
-                        dataSource: result.data.reverse(),
-                        xName: 'placa',
-                        yName: 'quantidade',
-                        type: 'Bar',
-                        fill: CORES_LAV.accent,
-                        cornerRadius: {
-                            topLeft: 4,
-                            topRight: 4,
-                            bottomLeft: 4,
-                            bottomRight: 4,
-                        },
-                        marker: {
-                            dataLabel: {
-                                visible: true,
-                                position: 'Top',
-                                font: { fontWeight: '600' },
-                            },
-                        },
-                    },
-                ],
+                    minimum: 0
+                },
+                series: [{
+                    dataSource: result.data.reverse(),
+                    xName: 'placa',
+                    yName: 'quantidade',
+                    type: 'Bar',
+                    fill: CORES_LAV.accent,
+                    cornerRadius: { topLeft: 4, topRight: 4, bottomLeft: 4, bottomRight: 4 },
+                    marker: { dataLabel: { visible: true, position: 'Top', font: { fontWeight: '600' } } }
+                }],
                 tooltip: { enable: true },
                 chartArea: { border: { width: 0 } },
-                background: 'transparent',
+                background: 'transparent'
             });
 
             chartTopVeiculos.appendTo('#chartTopVeiculos');
@@ -604,9 +486,7 @@
 
 async function carregarHeatmap(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/HeatmapDiaHora?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/HeatmapDiaHora?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -619,9 +499,7 @@
 
 async function carregarCategoria(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/LavagensPorCategoria?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/LavagensPorCategoria?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
@@ -634,82 +512,57 @@
             if (!container) return;
 
             if (result.data.length === 0) {
-                container.innerHTML =
-                    '<div class="text-center text-muted py-5"><i class="fa-duotone fa-chart-pie fa-3x mb-3"></i><p>Sem dados de categoria</p></div>';
+                container.innerHTML = '<div class="text-center text-muted py-5"><i class="fa-duotone fa-chart-pie fa-3x mb-3"></i><p>Sem dados de categoria</p></div>';
                 return;
             }
 
             if (ej.charts.AccumulationChart) {
                 chartCategoria = new ej.charts.AccumulationChart({
-                    series: [
-                        {
-                            dataSource: result.data,
-                            xName: 'categoria',
-                            yName: 'quantidade',
-                            innerRadius: '40%',
-                            palettes: CORES_LAV.gradient,
-                            dataLabel: {
-                                visible: true,
-                                name: 'categoria',
-                                position: 'Outside',
-                                font: { fontWeight: '600', size: '11px' },
-                                connectorStyle: {
-                                    length: '10px',
-                                    type: 'Curve',
-                                },
-                            },
-                        },
-                    ],
+                    series: [{
+                        dataSource: result.data,
+                        xName: 'categoria',
+                        yName: 'quantidade',
+                        innerRadius: '40%',
+                        palettes: CORES_LAV.gradient,
+                        dataLabel: {
+                            visible: true,
+                            name: 'categoria',
+                            position: 'Outside',
+                            font: { fontWeight: '600', size: '11px' },
+                            connectorStyle: { length: '10px', type: 'Curve' }
+                        }
+                    }],
                     legendSettings: {
                         visible: true,
                         position: 'Bottom',
-                        textStyle: { size: '11px' },
+                        textStyle: { size: '11px' }
                     },
                     tooltip: {
                         enable: true,
-                        format: '${point.x}: <b>${point.y} lavagens</b>',
+                        format: '${point.x}: <b>${point.y} lavagens</b>'
                     },
                     enableSmartLabels: true,
                     background: 'transparent',
                     width: '100%',
-                    height: '100%',
+                    height: '100%'
                 });
             } else {
 
                 chartCategoria = new ej.charts.Chart({
-                    primaryXAxis: {
-                        valueType: 'Category',
-                        labelStyle: { color: '#6B7280' },
-                    },
-                    primaryYAxis: {
-                        labelStyle: { color: '#6B7280' },
-                        minimum: 0,
-                    },
-                    series: [
-                        {
-                            dataSource: result.data,
-                            xName: 'categoria',
-                            yName: 'quantidade',
-                            type: 'Bar',
-                            fill: CORES_LAV.primary,
-                            cornerRadius: {
-                                topLeft: 4,
-                                topRight: 4,
-                                bottomLeft: 4,
-                                bottomRight: 4,
-                            },
-                            marker: {
-                                dataLabel: {
-                                    visible: true,
-                                    position: 'Top',
-                                    font: { fontWeight: '600' },
-                                },
-                            },
-                        },
-                    ],
+                    primaryXAxis: { valueType: 'Category', labelStyle: { color: '#6B7280' } },
+                    primaryYAxis: { labelStyle: { color: '#6B7280' }, minimum: 0 },
+                    series: [{
+                        dataSource: result.data,
+                        xName: 'categoria',
+                        yName: 'quantidade',
+                        type: 'Bar',
+                        fill: CORES_LAV.primary,
+                        cornerRadius: { topLeft: 4, topRight: 4, bottomLeft: 4, bottomRight: 4 },
+                        marker: { dataLabel: { visible: true, position: 'Top', font: { fontWeight: '600' } } }
+                    }],
                     tooltip: { enable: true },
                     chartArea: { border: { width: 0 } },
-                    background: 'transparent',
+                    background: 'transparent'
                 });
             }
 
@@ -718,239 +571,13 @@
     } catch (error) {
         console.error('Erro ao carregar grafico categoria:', error);
     }
-}
-
-async function carregarDuracao(dataInicio, dataFim) {
-    try {
-        const response = await fetch(
-            `/api/DashboardLavagem/DuracaoLavagens?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
-
-        if (!response.ok) {
-            console.warn('API DuracaoLavagens retornou erro:', response.status);
-            exibirDuracaoMockup();
-            return;
-        }
-
-        const result = await response.json();
-
-        const statsDuracao = document.getElementById('statsDuracao');
-        if (statsDuracao) {
-            if (
-                result.success &&
-                result.estatisticas &&
-                result.estatisticas.totalComDuracao > 0
-            ) {
-                document.getElementById('duracaoMedia').textContent =
-                    `${result.estatisticas.duracaoMedia} min`;
-                document.getElementById('duracaoMinima').textContent =
-                    `${result.estatisticas.duracaoMinima} min`;
-                document.getElementById('duracaoMaxima').textContent =
-                    `${result.estatisticas.duracaoMaxima} min`;
-                document.getElementById('totalComDuracao').textContent =
-                    result.estatisticas.totalComDuracao;
-            } else {
-                exibirDuracaoMockup();
-            }
-        }
-
-        if (chartDuracao) chartDuracao.destroy();
-
-        let dadosDistribuicao;
-        if (
-            result.success &&
-            result.distribuicao &&
-            result.distribuicao.some((d) => d.quantidade > 0)
-        ) {
-            dadosDistribuicao = result.distribuicao;
-        } else {
-
-            dadosDistribuicao = [
-                { faixa: '0-15 min', quantidade: 12 },
-                { faixa: '15-30 min', quantidade: 45 },
-                { faixa: '30-45 min', quantidade: 28 },
-                { faixa: '45-60 min', quantidade: 15 },
-                { faixa: '60+ min', quantidade: 8 },
-            ];
-        }
-
-        chartDuracao = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { color: '#6B7280', fontWeight: '500' },
-                majorGridLines: { width: 0 },
-            },
-            primaryYAxis: {
-                labelStyle: { color: '#6B7280' },
-                minimum: 0,
-                majorGridLines: { width: 1, color: '#f0f0f0' },
-            },
-            series: [
-                {
-                    dataSource: dadosDistribuicao,
-                    xName: 'faixa',
-                    yName: 'quantidade',
-                    type: 'Column',
-                    fill: CORES_LAV.primary,
-                    cornerRadius: { topLeft: 6, topRight: 6 },
-                    marker: {
-                        dataLabel: {
-                            visible: true,
-                            position: 'Top',
-                            font: { fontWeight: '600', color: '#333' },
-                        },
-                    },
-                },
-            ],
-            tooltip: {
-                enable: true,
-                format: '${point.x}: <b>${point.y} lavagens</b>',
-            },
-            chartArea: { border: { width: 0 } },
-            background: 'transparent',
-            width: '100%',
-            height: '280px',
-        });
-
-        chartDuracao.appendTo('#chartDuracao');
-
-        renderizarDuracaoPorCategoria(
-            result.success ? result.duracaoPorCategoria : null,
-        );
-    } catch (error) {
-        console.error('Erro ao carregar grafico duracao:', error);
-        exibirDuracaoMockup();
-        renderizarGraficoDuracaoMockup();
-        renderizarDuracaoPorCategoria(null);
-    }
-}
-
-function renderizarGraficoDuracaoMockup() {
-    try {
-        if (chartDuracao) chartDuracao.destroy();
-
-        const dadosDistribuicao = [
-            { faixa: '0-15 min', quantidade: 12 },
-            { faixa: '15-30 min', quantidade: 45 },
-            { faixa: '30-45 min', quantidade: 28 },
-            { faixa: '45-60 min', quantidade: 15 },
-            { faixa: '60+ min', quantidade: 8 },
-        ];
-
-        chartDuracao = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { color: '#6B7280', fontWeight: '500' },
-                majorGridLines: { width: 0 },
-            },
-            primaryYAxis: {
-                labelStyle: { color: '#6B7280' },
-                minimum: 0,
-                majorGridLines: { width: 1, color: '#f0f0f0' },
-            },
-            series: [
-                {
-                    dataSource: dadosDistribuicao,
-                    xName: 'faixa',
-                    yName: 'quantidade',
-                    type: 'Column',
-                    fill: CORES_LAV.primary,
-                    cornerRadius: { topLeft: 6, topRight: 6 },
-                    marker: {
-                        dataLabel: {
-                            visible: true,
-                            position: 'Top',
-                            font: { fontWeight: '600', color: '#333' },
-                        },
-                    },
-                },
-            ],
-            tooltip: {
-                enable: true,
-                format: '${point.x}: <b>${point.y} lavagens</b>',
-            },
-            chartArea: { border: { width: 0 } },
-            background: 'transparent',
-            width: '100%',
-            height: '280px',
-        });
-
-        chartDuracao.appendTo('#chartDuracao');
-    } catch (err) {
-        console.error('Erro ao renderizar grafico duracao mockup:', err);
-    }
-}
-
-function exibirDuracaoMockup() {
-
-    const duracaoMedia = document.getElementById('duracaoMedia');
-    if (duracaoMedia) duracaoMedia.textContent = '32 min';
-
-    const duracaoMinima = document.getElementById('duracaoMinima');
-    if (duracaoMinima) duracaoMinima.textContent = '8 min';
-
-    const duracaoMaxima = document.getElementById('duracaoMaxima');
-    if (duracaoMaxima) duracaoMaxima.textContent = '75 min';
-
-    const totalComDuracao = document.getElementById('totalComDuracao');
-    if (totalComDuracao) totalComDuracao.textContent = '0';
-
-    const msgMockup = document.getElementById('msgMockup');
-    if (msgMockup) msgMockup.style.display = 'block';
-}
-
-function renderizarDuracaoPorCategoria(dados) {
-    const container = document.getElementById('duracaoPorCategoria');
-    if (!container) return;
-
-    const dadosExibir =
-        dados && dados.length > 0
-            ? dados
-            : [
-                  { categoria: 'PM', mediaMinutos: 45.2, quantidade: 35 },
-                  { categoria: 'Passeio', mediaMinutos: 28.5, quantidade: 42 },
-                  {
-                      categoria: 'Utilitario',
-                      mediaMinutos: 38.0,
-                      quantidade: 18,
-                  },
-                  { categoria: 'SUV', mediaMinutos: 42.8, quantidade: 12 },
-                  { categoria: 'Outros', mediaMinutos: 32.1, quantidade: 5 },
-              ];
-
-    const maxMedia = Math.max(...dadosExibir.map((d) => d.mediaMinutos));
-
-    let html = dadosExibir
-        .map((item) => {
-            const percentual = ((item.mediaMinutos / maxMedia) * 100).toFixed(
-                0,
-            );
-            return `
-            <div class="duracao-categoria-item mb-2">
-                <div class="d-flex justify-content-between align-items-center mb-1">
-                    <span class="fw-600">${item.categoria}</span>
-                    <span class="text-muted small">${item.quantidade} lavagens</span>
-                </div>
-                <div class="progress" style="height: 20px;">
-                    <div class="progress-bar" role="progressbar"
-                         style="width: ${percentual}%; background: linear-gradient(90deg, ${CORES_LAV.primary}, ${CORES_LAV.secondary});"
-                         aria-valuenow="${percentual}" aria-valuemin="0" aria-valuemax="100">
-                        <span class="fw-bold">${item.mediaMinutos.toFixed(1)} min</span>
-                    </div>
-                </div>
-            </div>
-        `;
-        })
-        .join('');
-
-    container.innerHTML = html;
 }
 
 function renderizarHeatmap(data) {
     const container = document.getElementById('heatmapContainer');
     const diasSemana = ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'];
 
-    const maxValor = Math.max(...data.map((d) => d.quantidade), 1);
+    const maxValor = Math.max(...data.map(d => d.quantidade), 1);
 
     let html = '<table class="table table-sm mb-0" style="width: auto;">';
     html += '<thead><tr><th></th>';
@@ -964,13 +591,10 @@
         html += `<tr><td style="font-size: 0.75rem; font-weight: 600;">${diasSemana[d]}</td>`;
 
         for (let h = 0; h < 24; h++) {
-            const item = data.find((x) => x.dia === d && x.hora === h);
+            const item = data.find(x => x.dia === d && x.hora === h);
             const valor = item ? item.quantidade : 0;
             const intensidade = valor / maxValor;
-            const corIndex = Math.min(
-                Math.floor(intensidade * (CORES_LAV.heatmap.length - 1)),
-                CORES_LAV.heatmap.length - 1,
-            );
+            const corIndex = Math.min(Math.floor(intensidade * (CORES_LAV.heatmap.length - 1)), CORES_LAV.heatmap.length - 1);
             const cor = CORES_LAV.heatmap[corIndex];
             const corTexto = intensidade > 0.5 ? '#fff' : '#333';
 
@@ -989,26 +613,19 @@
 
 async function carregarTabelaLavadores(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/EstatisticasPorLavador?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/EstatisticasPorLavador?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
             const tbody = document.querySelector('#tabelaLavadores tbody');
-            tbody.innerHTML = result.data
-                .slice(0, 15)
-                .map(
-                    (item) => `
+            tbody.innerHTML = result.data.slice(0, 15).map(item => `
                 <tr>
                     <td>${item.nome}</td>
                     <td class="text-center"><strong>${item.lavagens}</strong></td>
                     <td class="text-center">${item.percentual}%</td>
                     <td class="text-center">${item.mediaDia}</td>
                 </tr>
-            `,
-                )
-                .join('');
+            `).join('');
         }
     } catch (error) {
         console.error('Erro ao carregar tabela lavadores:', error);
@@ -1017,28 +634,24 @@
 
 async function carregarTabelaVeiculos(dataInicio, dataFim) {
     try {
-        const response = await fetch(
-            `/api/DashboardLavagem/EstatisticasPorVeiculo?dataInicio=${dataInicio}&dataFim=${dataFim}`,
-        );
+        const response = await fetch(`/api/DashboardLavagem/EstatisticasPorVeiculo?dataInicio=${dataInicio}&dataFim=${dataFim}`);
         const result = await response.json();
 
         if (result.success && result.data) {
             const tbody = document.querySelector('#tabelaVeiculos tbody');
-            tbody.innerHTML = result.data
-                .slice(0, 15)
-                .map((item) => {
-                    let badgeDias = '';
-                    if (item.diasSemLavar >= 0) {
-                        if (item.diasSemLavar <= 3) {
-                            badgeDias = `<span class="badge bg-success">${item.diasSemLavar}</span>`;
-                        } else if (item.diasSemLavar <= 7) {
-                            badgeDias = `<span class="badge bg-warning text-dark">${item.diasSemLavar}</span>`;
-                        } else {
-                            badgeDias = `<span class="badge bg-danger">${item.diasSemLavar}</span>`;
-                        }
+            tbody.innerHTML = result.data.slice(0, 15).map(item => {
+                let badgeDias = '';
+                if (item.diasSemLavar >= 0) {
+                    if (item.diasSemLavar <= 3) {
+                        badgeDias = `<span class="badge bg-success">${item.diasSemLavar}</span>`;
+                    } else if (item.diasSemLavar <= 7) {
+                        badgeDias = `<span class="badge bg-warning text-dark">${item.diasSemLavar}</span>`;
+                    } else {
+                        badgeDias = `<span class="badge bg-danger">${item.diasSemLavar}</span>`;
                     }
-
-                    return `
+                }
+
+                return `
                     <tr>
                         <td><strong>${item.placa}</strong> <small class="text-muted">${item.modelo}</small></td>
                         <td class="text-center"><strong>${item.lavagens}</strong></td>
@@ -1046,8 +659,7 @@
                         <td class="text-center">${badgeDias}</td>
                     </tr>
                 `;
-                })
-                .join('');
+            }).join('');
         }
     } catch (error) {
         console.error('Erro ao carregar tabela veiculos:', error);
```
