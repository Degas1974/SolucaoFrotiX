# wwwroot/js/dashboards/dashboard-eventos.js

**Mudanca:** GRANDE | **+422** linhas | **-492** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-eventos.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-eventos.js
@@ -1,4 +1,5 @@
-if (typeof ej !== 'undefined' && ej.charts) {
+if (typeof ej !== 'undefined' && ej.charts)
+{
     console.log('🔧 Injetando módulos Syncfusion...');
 
     ej.charts.Chart.Inject(
@@ -7,18 +8,19 @@
         ej.charts.Category,
         ej.charts.Legend,
         ej.charts.Tooltip,
-        ej.charts.DataLabel,
+        ej.charts.DataLabel
     );
 
     ej.charts.AccumulationChart.Inject(
         ej.charts.PieSeries,
         ej.charts.AccumulationTooltip,
         ej.charts.AccumulationDataLabel,
-        ej.charts.AccumulationLegend,
+        ej.charts.AccumulationLegend
     );
 
     console.log('✅ Módulos Syncfusion injetados com sucesso!');
-} else {
+} else
+{
     console.error('❌ ERRO: Syncfusion (ej.charts) não está carregado!');
 }
 
@@ -30,31 +32,26 @@
     vermelho: '#dc2626',
     roxo: '#667eea',
     ciano: '#22d3ee',
-    rosa: '#ec4899',
+    rosa: '#ec4899'
 };
 
 let periodoAtual = {
     dataInicio: null,
-    dataFim: null,
+    dataFim: null
 };
 
 let chartEventosPorStatus = null;
 let chartEventosPorSetor = null;
 let chartEventosPorMes = null;
 
-async function inicializarDashboard() {
-    try {
+async function inicializarDashboard()
+{
+    try
+    {
         console.log('🎯 Iniciando Dashboard de Eventos...');
 
         const hoje = new Date();
-        periodoAtual.dataFim = new Date(
-            hoje.getFullYear(),
-            hoje.getMonth(),
-            hoje.getDate(),
-            23,
-            59,
-            59,
-        );
+        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
         periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
         periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);
 
@@ -62,70 +59,72 @@
 
         await carregarDadosDashboard();
 
-        AppToast.show(
-            'Verde',
-            'Dashboard de Eventos carregado com sucesso!',
-            3000,
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'inicializarDashboard',
-            error,
-        );
-    }
-}
-
-function inicializarCamposData() {
-    try {
+        AppToast.show('Verde', 'Dashboard de Eventos carregado com sucesso!', 3000);
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'inicializarDashboard', error);
+    }
+}
+
+function inicializarCamposData()
+{
+    try
+    {
         const dataInicio = document.getElementById('dataInicio');
         const dataFim = document.getElementById('dataFim');
 
-        if (dataInicio && dataFim) {
+        if (dataInicio && dataFim)
+        {
 
             dataInicio.value = formatarDataParaInput(periodoAtual.dataInicio);
             dataFim.value = formatarDataParaInput(periodoAtual.dataFim);
 
-            dataInicio.addEventListener('change', function () {
-                try {
-                    periodoAtual.dataInicio = new Date(
-                        this.value + 'T00:00:00',
-                    );
-                } catch (error) {
+            dataInicio.addEventListener('change', function ()
+            {
+                try
+                {
+                    periodoAtual.dataInicio = new Date(this.value + 'T00:00:00');
+                } catch (error)
+                {
                     console.error('Erro ao atualizar data inicial:', error);
                 }
             });
 
-            dataFim.addEventListener('change', function () {
-                try {
+            dataFim.addEventListener('change', function ()
+            {
+                try
+                {
                     periodoAtual.dataFim = new Date(this.value + 'T23:59:59');
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao atualizar data final:', error);
                 }
             });
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'inicializarCamposData',
-            error,
-        );
-    }
-}
-
-function formatarDataParaInput(data) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'inicializarCamposData', error);
+    }
+}
+
+function formatarDataParaInput(data)
+{
+    try
+    {
         const ano = data.getFullYear();
         const mes = String(data.getMonth() + 1).padStart(2, '0');
         const dia = String(data.getDate()).padStart(2, '0');
         return `${ano}-${mes}-${dia}`;
-    } catch (error) {
+    } catch (error)
+    {
         return '';
     }
 }
 
-async function carregarDadosDashboard() {
-    try {
+async function carregarDadosDashboard()
+{
+    try
+    {
         console.log('⏱️ Iniciando carregamento do dashboard...');
         const inicio = performance.now();
 
@@ -143,171 +142,143 @@
         console.log(`✅ Dashboard carregado em ${tempo}s`);
 
         const nomes = [
-            'EstatisticasGerais',
-            'EventosPorStatus',
-            'EventosPorSetor',
-            'EventosPorRequisitante',
-            'EventosPorMes',
-            'EventosPorTipo',
-            'EventosPorDia',
+            'EstatisticasGerais', 'EventosPorStatus', 'EventosPorSetor',
+            'EventosPorRequisitante', 'EventosPorMes', 'EventosPorTipo',
+            'EventosPorDia'
         ];
 
-        resultados.forEach((resultado, index) => {
-            if (resultado.status === 'rejected') {
+        resultados.forEach((resultado, index) =>
+        {
+            if (resultado.status === 'rejected')
+            {
                 console.error(`❌ ${nomes[index]} falhou:`, resultado.reason);
             }
         });
 
         esconderLoadingGeral();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarDadosDashboard',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarDadosDashboard', error);
         esconderLoadingGeral();
     }
 }
 
-function mostrarLoadingGeral() {
-    try {
+function mostrarLoadingGeral()
+{
+    try
+    {
         const loading = document.getElementById('loadingDashboard');
-        if (loading) {
+        if (loading)
+        {
             loading.classList.remove('d-none');
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao mostrar loading:', error);
     }
 }
 
-function esconderLoadingGeral() {
-    try {
+function esconderLoadingGeral()
+{
+    try
+    {
         const loading = document.getElementById('loadingDashboard');
-        if (loading) {
+        if (loading)
+        {
             loading.classList.add('d-none');
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao esconder loading:', error);
     }
 }
 
-function aplicarPeriodoRapido(dias) {
-    try {
+function aplicarPeriodoRapido(dias)
+{
+    try
+    {
         const hoje = new Date();
-        periodoAtual.dataFim = new Date(
-            hoje.getFullYear(),
-            hoje.getMonth(),
-            hoje.getDate(),
-            23,
-            59,
-            59,
-        );
+        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
         periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
-        periodoAtual.dataInicio.setDate(
-            periodoAtual.dataInicio.getDate() - dias,
-        );
-
-        document.getElementById('dataInicio').value = formatarDataParaInput(
-            periodoAtual.dataInicio,
-        );
-        document.getElementById('dataFim').value = formatarDataParaInput(
-            periodoAtual.dataFim,
-        );
+        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - dias);
+
+        document.getElementById('dataInicio').value = formatarDataParaInput(periodoAtual.dataInicio);
+        document.getElementById('dataFim').value = formatarDataParaInput(periodoAtual.dataFim);
 
         carregarDadosDashboard();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'aplicarPeriodoRapido',
-            error,
-        );
-    }
-}
-
-async function atualizarDashboard() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'aplicarPeriodoRapido', error);
+    }
+}
+
+async function atualizarDashboard()
+{
+    try
+    {
         await carregarDadosDashboard();
         AppToast.show('Verde', 'Dashboard atualizado!', 2000);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'atualizarDashboard',
-            error,
-        );
-    }
-}
-
-async function carregarEstatisticasGerais() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEstatisticasGerais?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
-
-        if (!response.ok)
-            throw new Error('Erro ao carregar estatísticas gerais');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'atualizarDashboard', error);
+    }
+}
+
+async function carregarEstatisticasGerais()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEstatisticasGerais?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
+
+        if (!response.ok) throw new Error('Erro ao carregar estatísticas gerais');
 
         const result = await response.json();
 
-        if (result.success) {
+        if (result.success)
+        {
             renderizarEstatisticasGerais(result);
-        } else {
+        } else
+        {
             console.error('Erro:', result.message);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEstatisticasGerais',
-            error,
-        );
-    }
-}
-
-function renderizarEstatisticasGerais(dados) {
-    try {
-
-        document.getElementById('statTotalEventos').textContent =
-            dados.totalEventos.toLocaleString();
-        document.getElementById('statEventosAtivos').textContent =
-            dados.eventosAtivos.toLocaleString();
-        document.getElementById('statEventosConcluidos').textContent =
-            dados.eventosConcluidos.toLocaleString();
-        document.getElementById('statEventosCancelados').textContent =
-            dados.eventosCancelados.toLocaleString();
-
-        document.getElementById('statTotalParticipantes').textContent =
-            dados.totalParticipantes.toLocaleString();
-        document.getElementById('statMediaParticipantes').textContent =
-            dados.mediaParticipantesPorEvento.toLocaleString() + ' part.';
-
-        calcularVariacao(
-            'totalEventos',
-            dados.totalEventos,
-            dados.periodoAnterior.totalEventos,
-            'variacaoTotalEventos',
-        );
-        calcularVariacao(
-            'totalParticipantes',
-            dados.totalParticipantes,
-            dados.periodoAnterior.totalParticipantes,
-            'variacaoTotalParticipantes',
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarEstatisticasGerais',
-            error,
-        );
-    }
-}
-
-function calcularVariacao(campo, valorAtual, valorAnterior, elementoId) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEstatisticasGerais', error);
+    }
+}
+
+function renderizarEstatisticasGerais(dados)
+{
+    try
+    {
+
+        document.getElementById('statTotalEventos').textContent = dados.totalEventos.toLocaleString();
+        document.getElementById('statEventosAtivos').textContent = dados.eventosAtivos.toLocaleString();
+        document.getElementById('statEventosConcluidos').textContent = dados.eventosConcluidos.toLocaleString();
+        document.getElementById('statEventosCancelados').textContent = dados.eventosCancelados.toLocaleString();
+
+        document.getElementById('statTotalParticipantes').textContent = dados.totalParticipantes.toLocaleString();
+        document.getElementById('statMediaParticipantes').textContent = dados.mediaParticipantesPorEvento.toLocaleString() + ' part.';
+
+        calcularVariacao('totalEventos', dados.totalEventos, dados.periodoAnterior.totalEventos, 'variacaoTotalEventos');
+        calcularVariacao('totalParticipantes', dados.totalParticipantes, dados.periodoAnterior.totalParticipantes, 'variacaoTotalParticipantes');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarEstatisticasGerais', error);
+    }
+}
+
+function calcularVariacao(campo, valorAtual, valorAnterior, elementoId)
+{
+    try
+    {
         const elemento = document.getElementById(elementoId);
         if (!elemento) return;
 
-        if (valorAnterior === 0) {
+        if (valorAnterior === 0)
+        {
             elemento.textContent = '—';
             elemento.className = 'variacao-metrica variacao-neutra';
             return;
@@ -318,187 +289,173 @@
         const sinal = variacao >= 0 ? '+' : '';
 
         elemento.textContent = `${sinal}${variacao.toFixed(1)}% vs anterior`;
-        elemento.className =
-            variacao >= 0
-                ? 'variacao-metrica variacao-positiva'
-                : 'variacao-metrica variacao-negativa';
-    } catch (error) {
+        elemento.className = variacao >= 0 ?
+            'variacao-metrica variacao-positiva' :
+            'variacao-metrica variacao-negativa';
+    } catch (error)
+    {
         console.error('Erro ao calcular variação:', error);
     }
 }
 
-async function carregarEventosPorStatus() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEventosPorStatus?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
-
-        if (!response.ok)
-            throw new Error('Erro ao carregar eventos por status');
+async function carregarEventosPorStatus()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEventosPorStatus?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
+
+        if (!response.ok) throw new Error('Erro ao carregar eventos por status');
 
         const result = await response.json();
 
-        if (result.success) {
+        if (result.success)
+        {
             renderizarGraficoEventosPorStatus(result.dados);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEventosPorStatus',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoEventosPorStatus(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorStatus', error);
+    }
+}
+
+function renderizarGraficoEventosPorStatus(dados)
+{
+    try
+    {
         const elemento = document.getElementById('chartEventosPorStatus');
 
-        if (!elemento) {
-            console.error(
-                '❌ Elemento #chartEventosPorStatus não encontrado no HTML!',
-            );
+        if (!elemento)
+        {
+            console.error('❌ Elemento #chartEventosPorStatus não encontrado no HTML!');
             return;
         }
 
-        if (!dados || dados.length === 0) {
+        if (!dados || dados.length === 0)
+        {
             console.warn('⚠️ Sem dados para renderizar gráfico de Status');
-            elemento.innerHTML =
-                '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
+            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
             return;
         }
 
-        if (chartEventosPorStatus) {
+        if (chartEventosPorStatus)
+        {
             chartEventosPorStatus.destroy();
             chartEventosPorStatus = null;
         }
 
         chartEventosPorStatus = new ej.charts.AccumulationChart({
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'status',
-                    yName: 'quantidade',
-                    innerRadius: '40%',
-                    dataLabel: {
-                        visible: true,
-                        position: 'Outside',
-                        name: 'status',
-                    },
-                    palettes: [
-                        '#0D6EFD',
-                        '#16a34a',
-                        '#dc2626',
-                        '#f59e0b',
-                        '#667eea',
-                    ],
+            series: [{
+                dataSource: dados,
+                xName: 'status',
+                yName: 'quantidade',
+                innerRadius: '40%',
+                dataLabel: {
+                    visible: true,
+                    position: 'Outside',
+                    name: 'status'
                 },
-            ],
+                palettes: ['#0D6EFD', '#16a34a', '#dc2626', '#f59e0b', '#667eea']
+            }],
             legendSettings: { visible: true },
-            tooltip: { enable: true },
+            tooltip: { enable: true }
         });
 
         chartEventosPorStatus.appendTo('#chartEventosPorStatus');
         console.log('✅ Gráfico de Status renderizado');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarGraficoEventosPorStatus',
-            error,
-        );
-    }
-}
-
-async function carregarEventosPorSetor() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEventosPorSetor?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorStatus', error);
+    }
+}
+
+async function carregarEventosPorSetor()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEventosPorSetor?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
 
         if (!response.ok) throw new Error('Erro ao carregar eventos por setor');
 
         const result = await response.json();
 
-        if (result.success) {
+        if (result.success)
+        {
             renderizarGraficoEventosPorSetor(result.dados);
             renderizarTabelaSetores(result.dados);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEventosPorSetor',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoEventosPorSetor(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorSetor', error);
+    }
+}
+
+function renderizarGraficoEventosPorSetor(dados)
+{
+    try
+    {
         const elemento = document.getElementById('chartEventosPorSetor');
 
-        if (!elemento) {
-            console.error(
-                '❌ Elemento #chartEventosPorSetor não encontrado no HTML!',
-            );
+        if (!elemento)
+        {
+            console.error('❌ Elemento #chartEventosPorSetor não encontrado no HTML!');
             return;
         }
 
-        if (!dados || dados.length === 0) {
+        if (!dados || dados.length === 0)
+        {
             console.warn('⚠️ Sem dados para renderizar gráfico de Setores');
-            elemento.innerHTML =
-                '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
+            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
             return;
         }
 
-        if (chartEventosPorSetor) {
+        if (chartEventosPorSetor)
+        {
             chartEventosPorSetor.destroy();
             chartEventosPorSetor = null;
         }
 
         chartEventosPorSetor = new ej.charts.Chart({
             primaryXAxis: {
-                valueType: 'Category',
+                valueType: 'Category'
             },
             primaryYAxis: {
-                title: 'Quantidade',
+                title: 'Quantidade'
             },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'setor',
-                    yName: 'quantidade',
-                    type: 'Column',
-                    name: 'Eventos',
-                    fill: '#667eea',
-                },
-            ],
+            series: [{
+                dataSource: dados,
+                xName: 'setor',
+                yName: 'quantidade',
+                type: 'Column',
+                name: 'Eventos',
+                fill: '#667eea'
+            }],
             legendSettings: { visible: false },
-            tooltip: { enable: true },
+            tooltip: { enable: true }
         });
 
         chartEventosPorSetor.appendTo('#chartEventosPorSetor');
         console.log('✅ Gráfico de Setores renderizado');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarGraficoEventosPorSetor',
-            error,
-        );
-    }
-}
-
-function renderizarTabelaSetores(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorSetor', error);
+    }
+}
+
+function renderizarTabelaSetores(dados)
+{
+    try
+    {
         const tbody = document.querySelector('#tabelaSetores tbody');
         if (!tbody) return;
 
         tbody.innerHTML = '';
 
-        dados.forEach((item, index) => {
+        dados.forEach((item, index) =>
+        {
             const tr = document.createElement('tr');
             tr.innerHTML = `
                 <td>${index + 1}</td>
@@ -514,48 +471,45 @@
             `;
             tbody.appendChild(tr);
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarTabelaSetores',
-            error,
-        );
-    }
-}
-
-async function carregarEventosPorRequisitante() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEventosPorRequisitante?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
-
-        if (!response.ok)
-            throw new Error('Erro ao carregar eventos por requisitante');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarTabelaSetores', error);
+    }
+}
+
+async function carregarEventosPorRequisitante()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEventosPorRequisitante?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
+
+        if (!response.ok) throw new Error('Erro ao carregar eventos por requisitante');
 
         const result = await response.json();
 
-        if (result.success) {
+        if (result.success)
+        {
             renderizarTabelaRequisitantes(result.dados);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEventosPorRequisitante',
-            error,
-        );
-    }
-}
-
-function renderizarTabelaRequisitantes(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorRequisitante', error);
+    }
+}
+
+function renderizarTabelaRequisitantes(dados)
+{
+    try
+    {
         const tbody = document.querySelector('#tabelaRequisitantes tbody');
         if (!tbody) return;
 
         tbody.innerHTML = '';
 
-        dados.forEach((item, index) => {
+        dados.forEach((item, index) =>
+        {
             const tr = document.createElement('tr');
             tr.innerHTML = `
                 <td>${index + 1}</td>
@@ -565,58 +519,55 @@
             `;
             tbody.appendChild(tr);
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarTabelaRequisitantes',
-            error,
-        );
-    }
-}
-
-async function carregarEventosPorMes() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEventosPorMes?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarTabelaRequisitantes', error);
+    }
+}
+
+async function carregarEventosPorMes()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEventosPorMes?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
 
         if (!response.ok) throw new Error('Erro ao carregar eventos por mês');
 
         const result = await response.json();
 
-        if (result.success) {
+        if (result.success)
+        {
             renderizarGraficoEventosPorMes(result.dados);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEventosPorMes',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoEventosPorMes(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorMes', error);
+    }
+}
+
+function renderizarGraficoEventosPorMes(dados)
+{
+    try
+    {
         const elemento = document.getElementById('chartEventosPorMes');
 
-        if (!elemento) {
-            console.error(
-                '❌ Elemento #chartEventosPorMes não encontrado no HTML!',
-            );
+        if (!elemento)
+        {
+            console.error('❌ Elemento #chartEventosPorMes não encontrado no HTML!');
             return;
         }
 
-        if (!dados || dados.length === 0) {
+        if (!dados || dados.length === 0)
+        {
             console.warn('⚠️ Sem dados para renderizar gráfico Mensal');
-            elemento.innerHTML =
-                '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
+            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
             return;
         }
 
-        if (chartEventosPorMes) {
+        if (chartEventosPorMes)
+        {
             chartEventosPorMes.destroy();
             chartEventosPorMes = null;
         }
@@ -624,38 +575,36 @@
         chartEventosPorMes = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
-                labelRotation: -45,
+                labelRotation: -45
             },
             primaryYAxis: {
                 title: 'Quantidade de Eventos',
-                labelFormat: '{value}',
+                labelFormat: '{value}'
             },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'mesNome',
-                    yName: 'quantidade',
-                    type: 'Line',
-                    name: 'Eventos',
-                    marker: {
-                        visible: true,
-                        width: 8,
-                        height: 8,
-                        dataLabel: { visible: true, position: 'Top' },
-                    },
-                    width: 3,
-                    fill: '#0D6EFD',
+            series: [{
+                dataSource: dados,
+                xName: 'mesNome',
+                yName: 'quantidade',
+                type: 'Line',
+                name: 'Eventos',
+                marker: {
+                    visible: true,
+                    width: 8,
+                    height: 8,
+                    dataLabel: { visible: true, position: 'Top' }
                 },
-            ],
+                width: 3,
+                fill: '#0D6EFD'
+            }],
             title: 'Evolução Mensal de Eventos',
             titleStyle: {
                 fontFamily: 'Helvetica',
                 fontWeight: '600',
-                size: '14px',
+                size: '14px'
             },
             tooltip: {
                 enable: true,
-                format: '${point.x}: ${point.y} eventos',
+                format: '${point.x}: ${point.y} eventos'
             },
 
             zoomSettings: {
@@ -663,53 +612,46 @@
                 enablePinchZooming: false,
                 enableMouseWheelZooming: false,
                 enableDeferredZooming: false,
-                enableScrollbar: false,
+                enableScrollbar: false
             },
-            enableAnimation: true,
+            enableAnimation: true
         });
 
         chartEventosPorMes.appendTo('#chartEventosPorMes');
         console.log('✅ Gráfico Mensal renderizado');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'renderizarGraficoEventosPorMes',
-            error,
-        );
-    }
-}
-
-async function carregarEventosPorDia() {
-    try {
-        const response = await fetch(
-            `/api/DashboardEventos/ObterEventosPorDia?` +
-                `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
-                `dataFim=${periodoAtual.dataFim.toISOString()}`,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorMes', error);
+    }
+}
+
+async function carregarEventosPorDia()
+{
+    try
+    {
+        const response = await fetch(`/api/DashboardEventos/ObterEventosPorDia?` +
+            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
+            `dataFim=${periodoAtual.dataFim.toISOString()}`);
 
         if (!response.ok) throw new Error('Erro ao carregar eventos por dia');
 
         const result = await response.json();
 
-        if (result.success) {
-
-            console.log(
-                '✅ Eventos por dia carregados:',
-                result.dados.length,
-                'dias',
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'carregarEventosPorDia',
-            error,
-        );
-    }
-}
-
-async function exportarParaPDF() {
-    try {
+        if (result.success)
+        {
+
+            console.log('✅ Eventos por dia carregados:', result.dados.length, 'dias');
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorDia', error);
+    }
+}
+
+async function exportarParaPDF()
+{
+    try
+    {
         console.log('📄 Iniciando exportação para PDF...');
 
         const dataInicio = periodoAtual.dataInicio.toISOString();
@@ -718,123 +660,111 @@
         window.location.href = `/ExportarParaPDF?dataInicio=${dataInicio}&dataFim=${dataFim}`;
 
         AppToast.show('Verde', 'PDF gerado com sucesso!', 3000);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'exportarParaPDF',
-            error,
-        );
-    }
-}
-
-$(document).ready(function () {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'exportarParaPDF', error);
+    }
+}
+
+$(document).ready(function ()
+{
+    try
+    {
         console.log('🚀 Dashboard de Eventos iniciando...');
 
         inicializarDashboard();
 
-        $('#btnAtualizarDashboard').on('click', function () {
-            try {
+        $('#btnAtualizarDashboard').on('click', function ()
+        {
+            try
+            {
                 atualizarDashboard();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btnAtualizarDashboard',
-                    error,
-                );
-            }
-        });
-
-        $('#btnExportarPDF').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btnAtualizarDashboard', error);
+            }
+        });
+
+        $('#btnExportarPDF').on('click', function ()
+        {
+            try
+            {
                 exportarParaPDF();
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btnExportarPDF',
-                    error,
-                );
-            }
-        });
-
-        $('#btn7Dias').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btnExportarPDF', error);
+            }
+        });
+
+        $('#btn7Dias').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(7);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn7Dias',
-                    error,
-                );
-            }
-        });
-
-        $('#btn15Dias').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn7Dias', error);
+            }
+        });
+
+        $('#btn15Dias').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(15);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn15Dias',
-                    error,
-                );
-            }
-        });
-
-        $('#btn30Dias').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn15Dias', error);
+            }
+        });
+
+        $('#btn30Dias').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(30);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn30Dias',
-                    error,
-                );
-            }
-        });
-
-        $('#btn90Dias').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn30Dias', error);
+            }
+        });
+
+        $('#btn90Dias').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(90);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn90Dias',
-                    error,
-                );
-            }
-        });
-
-        $('#btn180Dias').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn90Dias', error);
+            }
+        });
+
+        $('#btn180Dias').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(180);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn180Dias',
-                    error,
-                );
-            }
-        });
-
-        $('#btn1Ano').on('click', function () {
-            try {
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn180Dias', error);
+            }
+        });
+
+        $('#btn1Ano').on('click', function ()
+        {
+            try
+            {
                 aplicarPeriodoRapido(365);
-            } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-eventos.js',
-                    'click btn1Ano',
-                    error,
-                );
+            } catch (error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn1Ano', error);
             }
         });
 
         console.log('✅ Dashboard de Eventos pronto!');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-eventos.js',
-            'document.ready',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'document.ready', error);
     }
 });
```
