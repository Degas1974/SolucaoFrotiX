# wwwroot/js/dashboards/dashboard-viagens.js

**Mudanca:** GRANDE | **+1671** linhas | **-2033** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-viagens.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-viagens.js
@@ -6,12 +6,15 @@
     vermelho: '#dc2626',
     roxo: '#9d4edd',
     ciano: '#22d3ee',
-    rosa: '#ec4899',
+    rosa: '#ec4899'
 };
 
-function formatarNumero(valor, casasDecimais = 0) {
-    try {
-        if (valor === null || valor === undefined || isNaN(valor)) {
+function formatarNumero(valor, casasDecimais = 0)
+{
+    try
+    {
+        if (valor === null || valor === undefined || isNaN(valor))
+        {
             return '0';
         }
 
@@ -21,36 +24,40 @@
         const parteInteira = partes[0];
         const parteDecimal = partes[1];
 
-        const parteInteiraFormatada = parteInteira.replace(
-            /\B(?=(\d{3})+(?!\d))/g,
-            '.',
-        );
-
-        if (casasDecimais > 0 && parteDecimal) {
+        const parteInteiraFormatada = parteInteira.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
+
+        if (casasDecimais > 0 && parteDecimal)
+        {
             return `${parteInteiraFormatada},${parteDecimal}`;
         }
 
         return parteInteiraFormatada;
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao formatar número:', error);
         return '0';
     }
 }
 
-function formatarValorMonetario(valor) {
-    try {
-        if (valor === null || valor === undefined || isNaN(valor)) {
+function formatarValorMonetario(valor)
+{
+    try
+    {
+        if (valor === null || valor === undefined || isNaN(valor))
+        {
             return '0';
         }
 
         const valorNumerico = Number(valor);
 
-        if (valorNumerico < 100) {
+        if (valorNumerico < 100)
+        {
             return formatarNumero(valorNumerico, 2);
         }
 
         return formatarNumero(valorNumerico, 0);
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao formatar valor monetário:', error);
         return '0';
     }
@@ -58,7 +65,7 @@
 
 let periodoAtual = {
     dataInicio: null,
-    dataFim: null,
+    dataFim: null
 };
 
 let chartViagensPorStatus = null;
@@ -70,45 +77,48 @@
 let viagemAtualId = null;
 let modalAjustaViagemDashboard = null;
 
-function mostrarLoadingInicial() {
-    try {
+function mostrarLoadingInicial()
+{
+    try
+    {
         const loadingEl = document.getElementById('loadingInicialDashboard');
-        if (loadingEl) {
+        if (loadingEl)
+        {
             loadingEl.style.display = 'flex';
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao mostrar loading inicial:', error);
     }
 }
 
-function esconderLoadingInicial() {
-    try {
+function esconderLoadingInicial()
+{
+    try
+    {
         const loadingEl = document.getElementById('loadingInicialDashboard');
-        if (loadingEl) {
+        if (loadingEl)
+        {
             loadingEl.style.opacity = '0';
-            setTimeout(function () {
+            setTimeout(function() {
                 loadingEl.style.display = 'none';
             }, 300);
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao esconder loading inicial:', error);
     }
 }
 
-async function inicializarDashboard() {
-    try {
+async function inicializarDashboard()
+{
+    try
+    {
 
         mostrarLoadingInicial();
 
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
 
@@ -121,66 +131,72 @@
         esconderLoadingInicial();
 
         AppToast.show('Verde', 'Dashboard carregado com sucesso!', 3000);
-    } catch (error) {
+    } catch (error)
+    {
         esconderLoadingInicial();
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'inicializarDashboard',
-            error,
-        );
-    }
-}
-
-function inicializarCamposData() {
-    try {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarDashboard', error);
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
-            'dashboard-viagens.js',
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
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarCamposData', error);
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
 
@@ -202,938 +218,806 @@
             carregarTop10ViagensMaisCaras(),
             carregarHeatmapViagens(),
             carregarTop10VeiculosKm(),
-            carregarCustoMedioPorFinalidade(),
+            carregarCustoMedioPorFinalidade()
         ]);
 
         const tempo = ((performance.now() - inicio) / 1000).toFixed(2);
         console.log(`✅ Dashboard carregado em ${tempo}s`);
 
         const nomes = [
-            'EstatisticasGerais',
-            'ViagensPorDia',
-            'ViagensPorStatus',
-            'ViagensPorMotorista',
-            'ViagensPorVeiculo',
-            'CustosPorDia',
-            'CustosPorTipo',
-            'ViagensPorFinalidade',
-            'ViagensPorRequisitante',
-            'ViagensPorSetor',
-            'CustosPorMotorista',
-            'CustosPorVeiculo',
-            'Top10ViagensMaisCaras',
-            'HeatmapViagens',
-            'Top10VeiculosKm',
-            'CustoMedioPorFinalidade',
+            'EstatisticasGerais', 'ViagensPorDia', 'ViagensPorStatus', 'ViagensPorMotorista',
+            'ViagensPorVeiculo', 'CustosPorDia', 'CustosPorTipo', 'ViagensPorFinalidade',
+            'ViagensPorRequisitante', 'ViagensPorSetor', 'CustosPorMotorista',
+            'CustosPorVeiculo', 'Top10ViagensMaisCaras', 'HeatmapViagens', 'Top10VeiculosKm',
+            'CustoMedioPorFinalidade'
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
-            'dashboard-viagens.js',
-            'carregarDadosDashboard',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosDashboard', error);
         esconderLoadingGeral();
     }
 }
 
-async function carregarEstatisticasGerais() {
-    try {
+async function carregarEstatisticasGerais()
+{
+    try
+    {
+        const params = new URLSearchParams({
+            dataInicio: periodoAtual.dataInicio.toISOString(),
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterEstatisticasGerais?${params}`);
+        const result = await response.json();
+
+        if (result.success)
+        {
+            const data = result;
+
+            $('#statTotalViagens').text(formatarNumero(data.totalViagens, 0));
+            $('#statViagensFinalizadas').text(formatarNumero(data.viagensFinalizadas, 0));
+            $('#statCustoTotal').text('R$ ' + formatarValorMonetario(data.custoTotal));
+            $('#statCustoMedio').text('R$ ' + formatarValorMonetario(data.custoMedioPorViagem));
+            $('#statKmTotal').text(formatarNumero(data.kmTotal, 0) + ' km');
+            $('#statKmMedio').text(formatarNumero(data.kmMedioPorViagem, 2) + ' km');
+            $('#statViagensEmAndamento').text(formatarNumero(data.viagensEmAndamento, 0));
+            $('#statViagensAgendadas').text(formatarNumero(data.viagensAgendadas || 0, 0));
+            $('#statViagensCanceladas').text(formatarNumero(data.viagensCanceladas, 0));
+
+            if (data.periodoAnterior)
+            {
+
+                atualizarVariacao('variacaoCusto', data.custoTotal, data.periodoAnterior.custoTotal);
+                atualizarVariacao('variacaoViagens', data.totalViagens, data.periodoAnterior.totalViagens);
+                atualizarVariacao('variacaoCustoMedio', data.custoMedioPorViagem, data.periodoAnterior.custoMedioPorViagem);
+                atualizarVariacao('variacaoKm', data.kmTotal, data.periodoAnterior.kmTotal);
+                atualizarVariacao('variacaoKmMedio', data.kmMedioPorViagem, data.periodoAnterior.kmMedioPorViagem);
+
+                atualizarVariacao('variacaoRealizadas', data.viagensFinalizadas, data.periodoAnterior.viagensFinalizadas);
+                atualizarVariacao('variacaoAbertas', data.viagensEmAndamento, data.periodoAnterior.viagensEmAndamento);
+                atualizarVariacao('variacaoAgendadas', data.viagensAgendadas, data.periodoAnterior.viagensAgendadas);
+                atualizarVariacao('variacaoCanceladas', data.viagensCanceladas, data.periodoAnterior.viagensCanceladas);
+
+                atualizarVariacao('variacaoCustoCombustivel', data.custoCombustivel, data.periodoAnterior.custoCombustivel);
+                atualizarVariacao('variacaoCustoVeiculo', data.custoVeiculo, data.periodoAnterior.custoVeiculo);
+                atualizarVariacao('variacaoCustoMotorista', data.custoMotorista, data.periodoAnterior.custoMotorista);
+                atualizarVariacao('variacaoCustoOperador', data.custoOperador, data.periodoAnterior.custoOperador);
+                atualizarVariacao('variacaoCustoLavador', data.custoLavador, data.periodoAnterior.custoLavador);
+            }
+            else
+            {
+
+                $('#variacaoCusto, #variacaoViagens, #variacaoCustoMedio, #variacaoKm, #variacaoKmMedio, #variacaoRealizadas, #variacaoAbertas, #variacaoAgendadas, #variacaoCanceladas, #variacaoCustoCombustivel, #variacaoCustoVeiculo, #variacaoCustoMotorista, #variacaoCustoOperador, #variacaoCustoLavador')
+                    .text('-')
+                    .removeClass('variacao-positiva variacao-negativa')
+                    .addClass('variacao-neutra');
+            }
+
+            $('#statCustoCombustivel').text('R$ ' + formatarValorMonetario(data.custoCombustivel || 0));
+            $('#statCustoVeiculo').text('R$ ' + formatarValorMonetario(data.custoVeiculo || 0));
+            $('#statCustoMotorista').text('R$ ' + formatarValorMonetario(data.custoMotorista || 0));
+            $('#statCustoOperador').text('R$ ' + formatarValorMonetario(data.custoOperador || 0));
+            $('#statCustoLavador').text('R$ ' + formatarValorMonetario(data.custoLavador || 0));
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarEstatisticasGerais', error);
+    }
+}
+
+async function carregarViagensPorDia()
+{
+    try
+    {
+        const params = new URLSearchParams({
+            dataInicio: periodoAtual.dataInicio.toISOString(),
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorDia?${params}`);
+        const result = await response.json();
+
+        if (result.success && result.data.length > 0)
+        {
+            renderizarGraficoViagensPorDia(result.data);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorDia', error);
+    }
+}
+
+function renderizarGraficoViagensPorDia(dados)
+{
+    try
+    {
+        const chart = new ej.charts.Chart({
+            primaryXAxis: {
+                valueType: 'Category',
+                title: 'Dia da Semana'
+            },
+            primaryYAxis: {
+                labelFormat: '{value}',
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'diaSemana',
+                yName: 'total',
+                name: 'Total',
+                type: 'Column',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.azul
+            }],
+            tooltip: {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
+                        args.text = formatarNumero(args.value, 0);
+                    }
+                } catch (error)
+                {
+                    console.error('Erro ao formatar label:', error);
+                }
+            },
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
+                    console.error('Erro ao formatar tooltip:', error);
+                }
+            },
+            legendSettings: { visible: false },
+            height: '350px'
+        });
+
+        chart.appendTo('#chartViagensPorDia');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorDia', error);
+    }
+}
+
+async function carregarViagensPorStatus()
+{
+    try
+    {
+        const params = new URLSearchParams({
+            dataInicio: periodoAtual.dataInicio.toISOString(),
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorStatus?${params}`);
+        const result = await response.json();
+
+        if (result.success && result.data.length > 0)
+        {
+            renderizarGraficoViagensPorStatus(result.data);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorStatus', error);
+    }
+}
+
+function renderizarGraficoViagensPorStatus(dados)
+{
+    try
+    {
+
+        if (chartViagensPorStatus)
+        {
+            chartViagensPorStatus.destroy();
+            chartViagensPorStatus = null;
+        }
+
+        chartViagensPorStatus = new ej.charts.AccumulationChart({
+            series: [{
+                dataSource: dados,
+                xName: 'status',
+                yName: 'total',
+                innerRadius: '40%',
+                dataLabel: {
+                    visible: true,
+                    position: 'Outside',
+                    name: 'status',
+                    font: { fontWeight: '600' }
+                }
+            }],
+            enableSmartLabels: true,
+            legendSettings: {
+                visible: true,
+                position: 'Bottom'
+            },
+            tooltip: {
+                enable: true,
+                format: '${point.x}: ${point.y} viagens',
+                template: null
+            },
+            tooltipRender: function(args) {
+                try {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error) {
+                    console.error('Erro ao formatar tooltip:', error);
+                }
+            },
+            height: '350px'
+        });
+
+        chartViagensPorStatus.appendTo('#chartViagensPorStatus');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorStatus', error);
+    }
+}
+
+async function carregarViagensPorMotorista()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterEstatisticasGerais?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorMotorista?${params}`);
         const result = await response.json();
 
-        if (result.success) {
-            const data = result;
-
-            $('#statTotalViagens').text(formatarNumero(data.totalViagens, 0));
-            $('#statViagensFinalizadas').text(
-                formatarNumero(data.viagensFinalizadas, 0),
-            );
-            $('#statCustoTotal').text(
-                'R$ ' + formatarValorMonetario(data.custoTotal),
-            );
-            $('#statCustoMedio').text(
-                'R$ ' + formatarValorMonetario(data.custoMedioPorViagem),
-            );
-            $('#statKmTotal').text(formatarNumero(data.kmTotal, 0) + ' km');
-            $('#statKmMedio').text(
-                formatarNumero(data.kmMedioPorViagem, 2) + ' km',
-            );
-            $('#statViagensEmAndamento').text(
-                formatarNumero(data.viagensEmAndamento, 0),
-            );
-            $('#statViagensAgendadas').text(
-                formatarNumero(data.viagensAgendadas || 0, 0),
-            );
-            $('#statViagensCanceladas').text(
-                formatarNumero(data.viagensCanceladas, 0),
-            );
-
-            if (data.periodoAnterior) {
-
-                atualizarVariacao(
-                    'variacaoCusto',
-                    data.custoTotal,
-                    data.periodoAnterior.custoTotal,
-                );
-                atualizarVariacao(
-                    'variacaoViagens',
-                    data.totalViagens,
-                    data.periodoAnterior.totalViagens,
-                );
-                atualizarVariacao(
-                    'variacaoCustoMedio',
-                    data.custoMedioPorViagem,
-                    data.periodoAnterior.custoMedioPorViagem,
-                );
-                atualizarVariacao(
-                    'variacaoKm',
-                    data.kmTotal,
-                    data.periodoAnterior.kmTotal,
-                );
-                atualizarVariacao(
-                    'variacaoKmMedio',
-                    data.kmMedioPorViagem,
-                    data.periodoAnterior.kmMedioPorViagem,
-                );
-
-                atualizarVariacao(
-                    'variacaoRealizadas',
-                    data.viagensFinalizadas,
-                    data.periodoAnterior.viagensFinalizadas,
-                );
-                atualizarVariacao(
-                    'variacaoAbertas',
-                    data.viagensEmAndamento,
-                    data.periodoAnterior.viagensEmAndamento,
-                );
-                atualizarVariacao(
-                    'variacaoAgendadas',
-                    data.viagensAgendadas,
-                    data.periodoAnterior.viagensAgendadas,
-                );
-                atualizarVariacao(
-                    'variacaoCanceladas',
-                    data.viagensCanceladas,
-                    data.periodoAnterior.viagensCanceladas,
-                );
-
-                atualizarVariacao(
-                    'variacaoCustoCombustivel',
-                    data.custoCombustivel,
-                    data.periodoAnterior.custoCombustivel,
-                );
-                atualizarVariacao(
-                    'variacaoCustoVeiculo',
-                    data.custoVeiculo,
-                    data.periodoAnterior.custoVeiculo,
-                );
-                atualizarVariacao(
-                    'variacaoCustoMotorista',
-                    data.custoMotorista,
-                    data.periodoAnterior.custoMotorista,
-                );
-                atualizarVariacao(
-                    'variacaoCustoOperador',
-                    data.custoOperador,
-                    data.periodoAnterior.custoOperador,
-                );
-                atualizarVariacao(
-                    'variacaoCustoLavador',
-                    data.custoLavador,
-                    data.periodoAnterior.custoLavador,
-                );
-            } else {
-
-                $(
-                    '#variacaoCusto, #variacaoViagens, #variacaoCustoMedio, #variacaoKm, #variacaoKmMedio, #variacaoRealizadas, #variacaoAbertas, #variacaoAgendadas, #variacaoCanceladas, #variacaoCustoCombustivel, #variacaoCustoVeiculo, #variacaoCustoMotorista, #variacaoCustoOperador, #variacaoCustoLavador',
-                )
-                    .text('-')
-                    .removeClass('variacao-positiva variacao-negativa')
-                    .addClass('variacao-neutra');
-            }
-
-            $('#statCustoCombustivel').text(
-                'R$ ' + formatarValorMonetario(data.custoCombustivel || 0),
-            );
-            $('#statCustoVeiculo').text(
-                'R$ ' + formatarValorMonetario(data.custoVeiculo || 0),
-            );
-            $('#statCustoMotorista').text(
-                'R$ ' + formatarValorMonetario(data.custoMotorista || 0),
-            );
-            $('#statCustoOperador').text(
-                'R$ ' + formatarValorMonetario(data.custoOperador || 0),
-            );
-            $('#statCustoLavador').text(
-                'R$ ' + formatarValorMonetario(data.custoLavador || 0),
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarEstatisticasGerais',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorDia() {
-    try {
-        const params = new URLSearchParams({
-            dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorDia?${params}`,
-        );
-        const result = await response.json();
-
-        if (result.success && result.data.length > 0) {
-            renderizarGraficoViagensPorDia(result.data);
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorDia',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorDia(dados) {
-    try {
-        const chart = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                title: 'Dia da Semana',
-            },
-            primaryYAxis: {
-                labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'diaSemana',
-                    yName: 'total',
-                    name: 'Total',
-                    type: 'Column',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.azul,
-                },
-            ],
-            tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
-                        args.text = formatarNumero(args.value, 0);
-                    }
-                } catch (error) {
-                    console.error('Erro ao formatar label:', error);
-                }
-            },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
-                    console.error('Erro ao formatar tooltip:', error);
-                }
-            },
-            legendSettings: { visible: false },
-            height: '350px',
-        });
-
-        chart.appendTo('#chartViagensPorDia');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorDia',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorStatus() {
-    try {
-        const params = new URLSearchParams({
-            dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorStatus?${params}`,
-        );
-        const result = await response.json();
-
-        if (result.success && result.data.length > 0) {
-            renderizarGraficoViagensPorStatus(result.data);
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorStatus',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorStatus(dados) {
-    try {
-
-        if (chartViagensPorStatus) {
-            chartViagensPorStatus.destroy();
-            chartViagensPorStatus = null;
-        }
-
-        chartViagensPorStatus = new ej.charts.AccumulationChart({
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'status',
-                    yName: 'total',
-                    innerRadius: '40%',
-                    dataLabel: {
-                        visible: true,
-                        position: 'Outside',
-                        name: 'status',
-                        font: { fontWeight: '600' },
-                    },
-                },
-            ],
-            enableSmartLabels: true,
-            legendSettings: {
-                visible: true,
-                position: 'Bottom',
-            },
-            tooltip: {
-                enable: true,
-                format: '${point.x}: ${point.y} viagens',
-                template: null,
-            },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
-                    console.error('Erro ao formatar tooltip:', error);
-                }
-            },
-            height: '350px',
-        });
-
-        chartViagensPorStatus.appendTo('#chartViagensPorStatus');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorStatus',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorMotorista() {
-    try {
-        const params = new URLSearchParams({
-            dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorMotorista?${params}`,
-        );
-        const result = await response.json();
-
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoViagensPorMotorista(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorMotorista',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorMotorista(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorMotorista', error);
+    }
+}
+
+function renderizarGraficoViagensPorMotorista(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -45,
-                labelIntersectAction: 'Rotate45',
+                labelIntersectAction: 'Rotate45'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'motorista',
-                    yName: 'totalViagens',
-                    type: 'Column',
-                    name: 'Viagens',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.ciano,
-                },
-            ],
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'motorista',
+                yName: 'totalViagens',
+                type: 'Column',
+                name: 'Viagens',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.ciano
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '350px',
+            height: '350px'
         });
 
         chart.appendTo('#chartViagensPorMotorista');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorMotorista',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorVeiculo() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorMotorista', error);
+    }
+}
+
+async function carregarViagensPorVeiculo()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorVeiculo?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorVeiculo?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoViagensPorVeiculo(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorVeiculo',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorVeiculo(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorVeiculo', error);
+    }
+}
+
+function renderizarGraficoViagensPorVeiculo(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -45,
-                labelIntersectAction: 'Rotate45',
+                labelIntersectAction: 'Rotate45'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'veiculo',
-                    yName: 'totalViagens',
-                    type: 'Column',
-                    name: 'Viagens',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.laranja,
-                },
-            ],
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'veiculo',
+                yName: 'totalViagens',
+                type: 'Column',
+                name: 'Viagens',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.laranja
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '350px',
+            height: '350px'
         });
 
         chart.appendTo('#chartViagensPorVeiculo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorVeiculo',
-            error,
-        );
-    }
-}
-
-async function carregarCustosPorDia() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorVeiculo', error);
+    }
+}
+
+async function carregarCustosPorDia()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterCustosPorDia?${params}`,
-        );
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterCustosPorDia?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoCustosPorDia(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarCustosPorDia',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoCustosPorDia(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorDia', error);
+    }
+}
+
+function renderizarGraficoCustosPorDia(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'DateTime',
                 labelFormat: 'dd/MM',
                 intervalType: 'Days',
-                edgeLabelPlacement: 'Shift',
+                edgeLabelPlacement: 'Shift'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
                 title: 'Custos (R$)',
-                minimum: 0,
-            },
-            series: [
-                {
-                    dataSource: dados.map((d) => ({
-                        x: new Date(d.data),
-                        y:
-                            (d.combustivel || 0) +
-                            (d.veiculo || 0) +
-                            (d.motorista || 0) +
-                            (d.operador || 0) +
-                            (d.lavador || 0),
-                    })),
-                    xName: 'x',
-                    yName: 'y',
-                    name: 'Custo Total',
-                    type: 'Area',
-                    opacity: 0.5,
-                    fill: CORES_FROTIX.azul,
-                    border: { width: 2, color: CORES_FROTIX.azul },
-                },
-            ],
+                minimum: 0
+            },
+            series: [{
+                dataSource: dados.map(d => ({
+                    x: new Date(d.data),
+                    y: (d.combustivel || 0) + (d.veiculo || 0) + (d.motorista || 0) + (d.operador || 0) + (d.lavador || 0)
+                })),
+                xName: 'x',
+                yName: 'y',
+                name: 'Custo Total',
+                type: 'Area',
+                opacity: 0.5,
+                fill: CORES_FROTIX.azul,
+                border: { width: 2, color: CORES_FROTIX.azul }
+            }],
+            tooltip: {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
+                        args.text = 'R$ ' + formatarValorMonetario(args.value);
+                    }
+                } catch (error)
+                {
+                    console.error('Erro ao formatar label:', error);
+                }
+            },
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = 'Custo Total<br/>R$ ' + formatarValorMonetario(args.point.y);
+                } catch (error)
+                {
+                    console.error('Erro ao formatar tooltip:', error);
+                }
+            },
+            legendSettings: { visible: false },
+            height: '350px'
+        });
+
+        chart.appendTo('#chartCustosPorDia');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorDia', error);
+    }
+}
+
+async function carregarCustosPorTipo()
+{
+    try
+    {
+        const params = new URLSearchParams({
+            dataInicio: periodoAtual.dataInicio.toISOString(),
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterCustosPorTipo?${params}`);
+        const result = await response.json();
+
+        if (result.success && result.data.length > 0)
+        {
+            renderizarGraficoCustosPorTipo(result.data);
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorTipo', error);
+    }
+}
+
+function renderizarGraficoCustosPorTipo(dados)
+{
+    try
+    {
+
+        if (chartCustosPorTipo)
+        {
+            chartCustosPorTipo.destroy();
+            chartCustosPorTipo = null;
+        }
+
+        chartCustosPorTipo = new ej.charts.AccumulationChart({
+            series: [{
+                dataSource: dados,
+                xName: 'tipo',
+                yName: 'custo',
+                dataLabel: {
+                    visible: true,
+                    position: 'Outside',
+                    name: 'tipo',
+                    font: { fontWeight: '600' }
+                }
+            }],
+            enableSmartLabels: true,
+            legendSettings: {
+                visible: true,
+                position: 'Bottom'
+            },
             tooltip: {
                 enable: true,
-            },
-            axisLabelRender: function (args) {
+                format: '${point.x}: R$ ${point.y}',
+                template: null
+            },
+            tooltipRender: function(args) {
                 try {
-                    if (args.axis.name === 'primaryYAxis') {
-                        args.text = 'R$ ' + formatarValorMonetario(args.value);
-                    }
-                } catch (error) {
-                    console.error('Erro ao formatar label:', error);
-                }
-            },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        'Custo Total<br/>R$ ' +
-                        formatarValorMonetario(args.point.y);
+                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
                 } catch (error) {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
-            legendSettings: { visible: false },
-            height: '350px',
-        });
-
-        chart.appendTo('#chartCustosPorDia');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoCustosPorDia',
-            error,
-        );
-    }
-}
-
-async function carregarCustosPorTipo() {
-    try {
+            height: '350px'
+        });
+
+        chartCustosPorTipo.appendTo('#chartCustosPorTipo');
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorTipo', error);
+    }
+}
+
+async function carregarViagensPorFinalidade()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterCustosPorTipo?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorFinalidade?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
-            renderizarGraficoCustosPorTipo(result.data);
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarCustosPorTipo',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoCustosPorTipo(dados) {
-    try {
-
-        if (chartCustosPorTipo) {
-            chartCustosPorTipo.destroy();
-            chartCustosPorTipo = null;
-        }
-
-        chartCustosPorTipo = new ej.charts.AccumulationChart({
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'tipo',
-                    yName: 'custo',
-                    dataLabel: {
-                        visible: true,
-                        position: 'Outside',
-                        name: 'tipo',
-                        font: { fontWeight: '600' },
-                    },
-                },
-            ],
-            enableSmartLabels: true,
-            legendSettings: {
-                visible: true,
-                position: 'Bottom',
-            },
-            tooltip: {
-                enable: true,
-                format: '${point.x}: R$ ${point.y}',
-                template: null,
-            },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': R$ ' +
-                        formatarValorMonetario(args.point.y);
-                } catch (error) {
-                    console.error('Erro ao formatar tooltip:', error);
-                }
-            },
-            height: '350px',
-        });
-
-        chartCustosPorTipo.appendTo('#chartCustosPorTipo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoCustosPorTipo',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorFinalidade() {
-    try {
-        const params = new URLSearchParams({
-            dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorFinalidade?${params}`,
-        );
-        const result = await response.json();
-
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoViagensPorFinalidade(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorFinalidade',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorFinalidade(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorFinalidade', error);
+    }
+}
+
+function renderizarGraficoViagensPorFinalidade(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -45,
-                labelIntersectAction: 'Rotate45',
+                labelIntersectAction: 'Rotate45'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'finalidade',
-                    yName: 'total',
-                    type: 'Column',
-                    name: 'Viagens',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.verde,
-                },
-            ],
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'finalidade',
+                yName: 'total',
+                type: 'Column',
+                name: 'Viagens',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.verde
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '420px',
+            height: '420px'
         });
 
         chart.appendTo('#chartViagensPorFinalidade');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorFinalidade',
-            error,
-        );
-    }
-}
-
-async function carregarKmPorVeiculo() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorFinalidade', error);
+    }
+}
+
+async function carregarKmPorVeiculo()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterKmPorVeiculo?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterKmPorVeiculo?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoKmPorVeiculo(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarKmPorVeiculo',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoKmPorVeiculo(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarKmPorVeiculo', error);
+    }
+}
+
+function renderizarGraficoKmPorVeiculo(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: 0,
                 labelIntersectAction: 'Trim',
-                maximumLabelWidth: 120,
+                maximumLabelWidth: 120
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quilometragem',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'veiculo',
-                    yName: 'kmTotal',
-                    type: 'Bar',
-                    name: 'KM',
-                    cornerRadius: { topRight: 10, bottomRight: 10 },
-                    fill: CORES_FROTIX.roxo,
-                },
-            ],
+                title: 'Quilometragem'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'veiculo',
+                yName: 'kmTotal',
+                type: 'Bar',
+                name: 'KM',
+                cornerRadius: { topRight: 10, bottomRight: 10 },
+                fill: CORES_FROTIX.roxo
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0) + ' km';
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' km';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' km';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '420px',
+            height: '420px'
         });
 
         chart.appendTo('#chartKmPorVeiculo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoKmPorVeiculo',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorRequisitante() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoKmPorVeiculo', error);
+    }
+}
+
+async function carregarViagensPorRequisitante()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 6,
+            top: 6
         });
 
         console.log('🔍 Carregando Top 6 Requisitantes...', {
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorRequisitante?${params}`,
-        );
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorRequisitante?${params}`);
         const result = await response.json();
 
         console.log('📊 Resposta API - Top 6 Requisitantes:', result);
 
-        if (result.success && result.data && result.data.length > 0) {
-            console.log(
-                '✅ Renderizando gráfico com',
-                result.data.length,
-                'requisitantes',
-            );
+        if (result.success && result.data && result.data.length > 0)
+        {
+            console.log('✅ Renderizando gráfico com', result.data.length, 'requisitantes');
             renderizarGraficoViagensPorRequisitante(result.data);
 
-            if (result.viagensCtran !== undefined) {
-                $('#infoViagensCtranRequisitante').text(
-                    `Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`,
-                );
+            if (result.viagensCtran !== undefined)
+            {
+                $('#infoViagensCtranRequisitante').text(`Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`);
                 $('#footerRequisitante').removeClass('d-none');
-            } else {
+            }
+            else
+            {
                 $('#footerRequisitante').addClass('d-none');
             }
-        } else {
+        }
+        else
+        {
             console.warn('⚠️ Nenhum dado de requisitantes para exibir');
             document.getElementById('chartViagensPorRequisitante').innerHTML =
                 '<div class="text-center py-5 text-muted">Nenhum dado disponível para o período selecionado</div>';
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorRequisitante',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorRequisitante(dados) {
-    try {
-        console.log(
-            '🎨 Renderizando gráfico de requisitantes com dados:',
-            dados,
-        );
-
-        const containerElement = document.getElementById(
-            'chartViagensPorRequisitante',
-        );
-        if (
-            containerElement &&
-            containerElement.ej2_instances &&
-            containerElement.ej2_instances.length > 0
-        ) {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorRequisitante', error);
+    }
+}
+
+function renderizarGraficoViagensPorRequisitante(dados)
+{
+    try
+    {
+        console.log('🎨 Renderizando gráfico de requisitantes com dados:', dados);
+
+        const containerElement = document.getElementById('chartViagensPorRequisitante');
+        if (containerElement && containerElement.ej2_instances && containerElement.ej2_instances.length > 0)
+        {
             containerElement.ej2_instances[0].destroy();
         }
 
@@ -1142,375 +1026,370 @@
                 valueType: 'Category',
                 labelRotation: 0,
                 labelIntersectAction: 'Trim',
-                maximumLabelWidth: 100,
+                maximumLabelWidth: 100
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'requisitante',
-                    yName: 'totalViagens',
-                    type: 'Bar',
-                    name: 'Viagens',
-                    cornerRadius: { topRight: 10, bottomRight: 10 },
-                    fill: CORES_FROTIX.rosa,
-                },
-            ],
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'requisitante',
+                yName: 'totalViagens',
+                type: 'Bar',
+                name: 'Viagens',
+                cornerRadius: { topRight: 10, bottomRight: 10 },
+                fill: CORES_FROTIX.rosa
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
             height: '280px',
-            loaded: function () {
-                try {
-                    console.log(
-                        '✅ Gráfico de Requisitantes carregado com sucesso!',
-                    );
-                } catch (error) {
+            loaded: function ()
+            {
+                try
+                {
+                    console.log('✅ Gráfico de Requisitantes carregado com sucesso!');
+                } catch (error)
+                {
                     console.error('Erro no evento loaded:', error);
                 }
-            },
+            }
         });
 
         chart.appendTo('#chartViagensPorRequisitante');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorRequisitante',
-            error,
-        );
-    }
-}
-
-async function carregarViagensPorSetor() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorRequisitante', error);
+    }
+}
+
+async function carregarViagensPorSetor()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 6,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterViagensPorSetor?${params}`,
-        );
+            top: 6
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterViagensPorSetor?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoViagensPorSetor(result.data);
 
-            if (result.viagensCtran !== undefined) {
-                $('#infoViagensCtranSetor').text(
-                    `Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`,
-                );
+            if (result.viagensCtran !== undefined)
+            {
+                $('#infoViagensCtranSetor').text(`Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`);
                 $('#footerSetor').removeClass('d-none');
-            } else {
+            }
+            else
+            {
                 $('#footerSetor').addClass('d-none');
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarViagensPorSetor',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoViagensPorSetor(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorSetor', error);
+    }
+}
+
+function renderizarGraficoViagensPorSetor(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: 0,
                 labelIntersectAction: 'Trim',
-                maximumLabelWidth: 100,
+                maximumLabelWidth: 100
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quantidade de Viagens',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'setor',
-                    yName: 'totalViagens',
-                    type: 'Bar',
-                    name: 'Viagens',
-                    cornerRadius: { topRight: 10, bottomRight: 10 },
-                    fill: CORES_FROTIX.amarelo,
-                },
-            ],
+                title: 'Quantidade de Viagens'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'setor',
+                yName: 'totalViagens',
+                type: 'Bar',
+                name: 'Viagens',
+                cornerRadius: { topRight: 10, bottomRight: 10 },
+                fill: CORES_FROTIX.amarelo
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' viagens';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '280px',
+            height: '280px'
         });
 
         chart.appendTo('#chartViagensPorSetor');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoViagensPorSetor',
-            error,
-        );
-    }
-}
-
-async function carregarCustosPorMotorista() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorSetor', error);
+    }
+}
+
+async function carregarCustosPorMotorista()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterCustosPorMotorista?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterCustosPorMotorista?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoCustosPorMotorista(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarCustosPorMotorista',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoCustosPorMotorista(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorMotorista', error);
+    }
+}
+
+function renderizarGraficoCustosPorMotorista(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -45,
-                labelIntersectAction: 'Rotate45',
+                labelIntersectAction: 'Rotate45'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Custo Total (R$)',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'motorista',
-                    yName: 'custoTotal',
-                    type: 'Column',
-                    name: 'Custo',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.vermelho,
-                },
-            ],
+                title: 'Custo Total (R$)'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'motorista',
+                yName: 'custoTotal',
+                type: 'Column',
+                name: 'Custo',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.vermelho
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = 'R$ ' + formatarValorMonetario(args.value);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': R$ ' +
-                        formatarValorMonetario(args.point.y);
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '350px',
+            height: '350px'
         });
 
         chart.appendTo('#chartCustosPorMotorista');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoCustosPorMotorista',
-            error,
-        );
-    }
-}
-
-async function carregarCustosPorVeiculo() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorMotorista', error);
+    }
+}
+
+async function carregarCustosPorVeiculo()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterCustosPorVeiculo?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterCustosPorVeiculo?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarGraficoCustosPorVeiculo(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarCustosPorVeiculo',
-            error,
-        );
-    }
-}
-
-function renderizarGraficoCustosPorVeiculo(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorVeiculo', error);
+    }
+}
+
+function renderizarGraficoCustosPorVeiculo(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -45,
-                labelIntersectAction: 'Rotate45',
+                labelIntersectAction: 'Rotate45'
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Custo Total (R$)',
-            },
-            series: [
-                {
-                    dataSource: dados,
-                    xName: 'veiculo',
-                    yName: 'custoTotal',
-                    type: 'Column',
-                    name: 'Custo',
-                    cornerRadius: { topLeft: 10, topRight: 10 },
-                    fill: CORES_FROTIX.azul,
-                },
-            ],
+                title: 'Custo Total (R$)'
+            },
+            series: [{
+                dataSource: dados,
+                xName: 'veiculo',
+                yName: 'custoTotal',
+                type: 'Column',
+                name: 'Custo',
+                cornerRadius: { topLeft: 10, topRight: 10 },
+                fill: CORES_FROTIX.azul
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = 'R$ ' + formatarValorMonetario(args.value);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': R$ ' +
-                        formatarValorMonetario(args.point.y);
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '350px',
+            height: '350px'
         });
 
         chart.appendTo('#chartCustosPorVeiculo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarGraficoCustosPorVeiculo',
-            error,
-        );
-    }
-}
-
-async function carregarTop10ViagensMaisCaras() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorVeiculo', error);
+    }
+}
+
+async function carregarTop10ViagensMaisCaras()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterTop10ViagensMaisCaras?${params}`,
-        );
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterTop10ViagensMaisCaras?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarTabelaTop10(result.data);
-        } else {
-            $('#tabelaTop10Body').html(
-                '<tr><td colspan="7" class="text-center">Nenhuma viagem encontrada</td></tr>',
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarTop10ViagensMaisCaras',
-            error,
-        );
+        } else
+        {
+            $('#tabelaTop10Body').html('<tr><td colspan="7" class="text-center">Nenhuma viagem encontrada</td></tr>');
+        }
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarTop10ViagensMaisCaras', error);
     }
 }
 
 let dadosTop10Viagens = [];
 
-function renderizarTabelaTop10(dados) {
-    try {
+function renderizarTabelaTop10(dados)
+{
+    try
+    {
 
         dadosTop10Viagens = dados;
 
         let html = '';
 
-        dados.forEach((viagem, index) => {
-
-            const noFichaFormatado =
-                viagem.noFichaVistoria && viagem.noFichaVistoria !== 'N/A'
-                    ? formatarNumero(parseInt(viagem.noFichaVistoria) || 0, 0)
-                    : 'N/A';
+        dados.forEach((viagem, index) =>
+        {
+
+            const noFichaFormatado = viagem.noFichaVistoria && viagem.noFichaVistoria !== 'N/A'
+                ? formatarNumero(parseInt(viagem.noFichaVistoria) || 0, 0)
+                : 'N/A';
 
             html += `
                 <tr data-viagem-index="${index}" onclick="abrirModalDetalhesViagem(${index})" title="Clique para ver detalhes">
@@ -1526,19 +1405,19 @@
         });
 
         $('#tabelaTop10Body').html(html);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarTabelaTop10',
-            error,
-        );
-    }
-}
-
-function abrirModalDetalhesViagem(index) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarTabelaTop10', error);
+    }
+}
+
+function abrirModalDetalhesViagem(index)
+{
+    try
+    {
         const viagem = dadosTop10Viagens[index];
-        if (!viagem) {
+        if (!viagem)
+        {
             console.error('Viagem não encontrada no índice:', index);
             return;
         }
@@ -1546,68 +1425,53 @@
         viagemAtualId = viagem.viagemId;
 
         $('#modalNoFicha').text(viagem.noFichaVistoria || 'N/A');
-        $('#modalStatus').html(
-            viagem.status
-                ? `<span class="badge bg-success">${viagem.status}</span>`
-                : '-',
-        );
+        $('#modalStatus').html(viagem.status
+            ? `<span class="badge bg-success">${viagem.status}</span>`
+            : '-');
         $('#modalDataInicial').text(viagem.dataInicial || '-');
         $('#modalDataFinal').text(viagem.dataFinal || '-');
         $('#modalMotorista').text(viagem.motorista || '-');
         $('#modalVeiculo').text(viagem.veiculo || '-');
-        $('#modalKmRodado').text(
-            viagem.kmRodado ? formatarNumero(viagem.kmRodado, 0) + ' km' : '-',
-        );
-        $('#modalDuracao').text(
-            viagem.duracao || viagem.minutos
-                ? formatarDuracao(viagem.minutos || 0)
-                : '-',
-        );
+        $('#modalKmRodado').text(viagem.kmRodado
+            ? formatarNumero(viagem.kmRodado, 0) + ' km'
+            : '-');
+        $('#modalDuracao').text(viagem.duracao || viagem.minutos
+            ? formatarDuracao(viagem.minutos || 0)
+            : '-');
         $('#modalFinalidade').text(viagem.finalidade || '-');
 
         const alertaKmZero = document.getElementById('alertaKmZero');
-        if (alertaKmZero) {
-            if (!viagem.kmRodado || viagem.kmRodado <= 0) {
+        if (alertaKmZero)
+        {
+            if (!viagem.kmRodado || viagem.kmRodado <= 0)
+            {
                 alertaKmZero.classList.remove('d-none');
-            } else {
+            }
+            else
+            {
                 alertaKmZero.classList.add('d-none');
             }
         }
 
-        $('#modalCustoCombustivel').text(
-            'R$ ' + formatarValorMonetario(viagem.custoCombustivel || 0),
-        );
-        $('#modalCustoVeiculo').text(
-            'R$ ' + formatarValorMonetario(viagem.custoVeiculo || 0),
-        );
-        $('#modalCustoMotorista').text(
-            'R$ ' + formatarValorMonetario(viagem.custoMotorista || 0),
-        );
-        $('#modalCustoOperador').text(
-            'R$ ' + formatarValorMonetario(viagem.custoOperador || 0),
-        );
-        $('#modalCustoLavador').text(
-            'R$ ' + formatarValorMonetario(viagem.custoLavador || 0),
-        );
-        $('#modalCustoTotal').text(
-            'R$ ' + formatarValorMonetario(viagem.custoTotal || 0),
-        );
-
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalDetalhesViagem'),
-        );
+        $('#modalCustoCombustivel').text('R$ ' + formatarValorMonetario(viagem.custoCombustivel || 0));
+        $('#modalCustoVeiculo').text('R$ ' + formatarValorMonetario(viagem.custoVeiculo || 0));
+        $('#modalCustoMotorista').text('R$ ' + formatarValorMonetario(viagem.custoMotorista || 0));
+        $('#modalCustoOperador').text('R$ ' + formatarValorMonetario(viagem.custoOperador || 0));
+        $('#modalCustoLavador').text('R$ ' + formatarValorMonetario(viagem.custoLavador || 0));
+        $('#modalCustoTotal').text('R$ ' + formatarValorMonetario(viagem.custoTotal || 0));
+
+        const modal = new bootstrap.Modal(document.getElementById('modalDetalhesViagem'));
         modal.show();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'abrirModalDetalhesViagem',
-            error,
-        );
-    }
-}
-
-function formatarDuracao(minutos) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'abrirModalDetalhesViagem', error);
+    }
+}
+
+function formatarDuracao(minutos)
+{
+    try
+    {
         if (!minutos || minutos <= 0) return '-';
 
         const horas = Math.floor(minutos / 60);
@@ -1616,43 +1480,45 @@
         if (horas === 0) return mins + 'min';
         if (mins === 0) return horas + 'h';
         return horas + 'h ' + String(mins).padStart(2, '0') + 'min';
-    } catch (error) {
+    } catch (error)
+    {
         return '-';
     }
 }
 
-async function carregarHeatmapViagens() {
-    try {
+async function carregarHeatmapViagens()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterHeatmapViagens?${params}`,
-        );
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterHeatmapViagens?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarHeatmapViagens(result.data, result.maxValor);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarHeatmapViagens',
-            error,
-        );
-    }
-}
-
-function renderizarHeatmapViagens(dados, maxValor) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarHeatmapViagens', error);
+    }
+}
+
+function renderizarHeatmapViagens(dados, maxValor)
+{
+    try
+    {
         const tbody = document.getElementById('heatmapBody');
         if (!tbody) return;
 
         tbody.innerHTML = '';
 
-        function obterCorHeatmap(valor, max) {
+        function obterCorHeatmap(valor, max)
+        {
             if (max === 0 || valor === 0) return '#f5f5f5';
 
             const intensidade = valor / max;
@@ -1664,7 +1530,8 @@
             return '#2e7d32';
         }
 
-        dados.forEach((dia) => {
+        dados.forEach(dia =>
+        {
             const tr = document.createElement('tr');
 
             const tdDia = document.createElement('td');
@@ -1672,25 +1539,25 @@
             tdDia.textContent = dia.diaSemana;
             tr.appendChild(tdDia);
 
-            dia.horas.forEach((quantidade, hora) => {
+            dia.horas.forEach((quantidade, hora) =>
+            {
                 const td = document.createElement('td');
                 td.className = 'text-center';
-                td.style.backgroundColor = obterCorHeatmap(
-                    quantidade,
-                    maxValor,
-                );
-                td.style.color = quantidade > maxValor * 0.6 ? 'white' : '#333';
+                td.style.backgroundColor = obterCorHeatmap(quantidade, maxValor);
+                td.style.color = quantidade > (maxValor * 0.6) ? 'white' : '#333';
                 td.style.fontWeight = quantidade > 0 ? '600' : 'normal';
                 td.style.cursor = 'pointer';
                 td.style.transition = 'transform 0.2s';
                 td.textContent = quantidade > 0 ? quantidade : '';
                 td.title = `${dia.diaSemana} ${hora.toString().padStart(2, '0')}:00 - ${quantidade} viagem(s)`;
 
-                td.addEventListener('mouseenter', function () {
+                td.addEventListener('mouseenter', function ()
+                {
                     this.style.transform = 'scale(1.1)';
                     this.style.zIndex = '10';
                 });
-                td.addEventListener('mouseleave', function () {
+                td.addEventListener('mouseleave', function ()
+                {
                     this.style.transform = 'scale(1)';
                     this.style.zIndex = '1';
                 });
@@ -1700,46 +1567,44 @@
 
             tbody.appendChild(tr);
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarHeatmapViagens',
-            error,
-        );
-    }
-}
-
-async function carregarTop10VeiculosKm() {
-    try {
+
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarHeatmapViagens', error);
+    }
+}
+
+async function carregarTop10VeiculosKm()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterTop10VeiculosPorKm?${params}`,
-        );
+            dataFim: periodoAtual.dataFim.toISOString()
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterTop10VeiculosPorKm?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarTop10VeiculosKm(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarTop10VeiculosKm',
-            error,
-        );
-    }
-}
-
-function renderizarTop10VeiculosKm(dados) {
-    try {
-
-        const dadosFormatados = dados.map((d) => ({
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarTop10VeiculosKm', error);
+    }
+}
+
+function renderizarTop10VeiculosKm(dados)
+{
+    try
+    {
+
+        const dadosFormatados = dados.map(d => ({
             veiculo: d.placa,
             totalKm: d.totalKm,
-            tooltip: `${d.placa} - ${d.marcaModelo}\n${d.totalViagens} viagens | Média: ${d.mediaKmPorViagem} km/viagem`,
+            tooltip: `${d.placa} - ${d.marcaModelo}\n${d.totalViagens} viagens | Média: ${d.mediaKmPorViagem} km/viagem`
         }));
 
         const chart = new ej.charts.Chart({
@@ -1747,106 +1612,102 @@
                 valueType: 'Category',
                 labelRotation: 0,
                 labelIntersectAction: 'Trim',
-                maximumLabelWidth: 80,
+                maximumLabelWidth: 80
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Quilometragem Total',
-            },
-            series: [
-                {
-                    dataSource: dadosFormatados,
-                    xName: 'veiculo',
-                    yName: 'totalKm',
-                    type: 'Bar',
-                    name: 'KM Rodado',
-                    cornerRadius: { topRight: 8, bottomRight: 8 },
-                    fill: CORES_FROTIX.verde,
-                },
-            ],
+                title: 'Quilometragem Total'
+            },
+            series: [{
+                dataSource: dadosFormatados,
+                xName: 'veiculo',
+                yName: 'totalKm',
+                type: 'Bar',
+                name: 'KM Rodado',
+                cornerRadius: { topRight: 8, bottomRight: 8 },
+                fill: CORES_FROTIX.verde
+            }],
             tooltip: {
-                enable: true,
-            },
-            axisLabelRender: function (args) {
-                try {
-                    if (args.axis.name === 'primaryYAxis') {
+                enable: true
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+                    if (args.axis.name === 'primaryYAxis')
+                    {
                         args.text = formatarNumero(args.value, 0) + ' km';
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
-                    args.text =
-                        args.point.x +
-                        ': ' +
-                        formatarNumero(args.point.y, 0) +
-                        ' km';
-                } catch (error) {
+            tooltipRender: function (args)
+            {
+                try
+                {
+                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' km';
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: { visible: false },
-            height: '420px',
+            height: '420px'
         });
 
         chart.appendTo('#chartTop10VeiculosKm');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarTop10VeiculosKm',
-            error,
-        );
-    }
-}
-
-async function carregarCustoMedioPorFinalidade() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarTop10VeiculosKm', error);
+    }
+}
+
+async function carregarCustoMedioPorFinalidade()
+{
+    try
+    {
         const params = new URLSearchParams({
             dataInicio: periodoAtual.dataInicio.toISOString(),
             dataFim: periodoAtual.dataFim.toISOString(),
-            top: 10,
-        });
-
-        const response = await fetch(
-            `/api/DashboardViagens/ObterCustoMedioPorFinalidade?${params}`,
-        );
+            top: 10
+        });
+
+        const response = await fetch(`/api/DashboardViagens/ObterCustoMedioPorFinalidade?${params}`);
         const result = await response.json();
 
-        if (result.success && result.data.length > 0) {
+        if (result.success && result.data.length > 0)
+        {
             renderizarCustoMedioPorFinalidade(result.data);
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarCustoMedioPorFinalidade',
-            error,
-        );
-    }
-}
-
-function renderizarCustoMedioPorFinalidade(dados) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustoMedioPorFinalidade', error);
+    }
+}
+
+function renderizarCustoMedioPorFinalidade(dados)
+{
+    try
+    {
         const chart = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: 0,
                 labelIntersectAction: 'Trim',
-                maximumLabelWidth: 120,
+                maximumLabelWidth: 120
             },
             primaryYAxis: {
                 labelFormat: '{value}',
-                title: 'Custo Total (R$)',
-            },
-            axes: [
-                {
-                    name: 'yAxisMedio',
-                    opposedPosition: true,
-                    labelFormat: '{value}',
-                    title: 'Custo Médio (R$)',
-                },
-            ],
+                title: 'Custo Total (R$)'
+            },
+            axes: [{
+                name: 'yAxisMedio',
+                opposedPosition: true,
+                labelFormat: '{value}',
+                title: 'Custo Médio (R$)'
+            }],
             series: [
                 {
                     dataSource: dados,
@@ -1857,7 +1718,7 @@
                     cornerRadius: { topRight: 8, bottomRight: 8 },
                     fill: CORES_FROTIX.vermelho,
                     opacity: 0.8,
-                    tooltipMappingName: 'finalidade',
+                    tooltipMappingName: 'finalidade'
                 },
                 {
                     dataSource: dados,
@@ -1870,119 +1731,101 @@
                         visible: true,
                         width: 10,
                         height: 10,
-                        fill: CORES_FROTIX.azul,
+                        fill: CORES_FROTIX.azul
                     },
                     fill: CORES_FROTIX.azul,
                     width: 3,
-                    tooltipMappingName: 'finalidade',
-                },
+                    tooltipMappingName: 'finalidade'
+                }
             ],
             tooltip: {
                 enable: true,
-                shared: false,
-            },
-            axisLabelRender: function (args) {
-                try {
-
-                    if (
-                        args.axis.name === 'primaryYAxis' ||
-                        args.axis.name === 'yAxisMedio'
-                    ) {
+                shared: false
+            },
+            axisLabelRender: function (args)
+            {
+                try
+                {
+
+                    if (args.axis.name === 'primaryYAxis' || args.axis.name === 'yAxisMedio')
+                    {
                         args.text = 'R$ ' + formatarNumero(args.value, 0);
                     }
-                } catch (error) {
+                } catch (error)
+                {
                     console.error('Erro ao formatar label:', error);
                 }
             },
-            tooltipRender: function (args) {
-                try {
+            tooltipRender: function (args)
+            {
+                try
+                {
                     const nomeSerie = args.series.name || '';
                     const valor = Number(args.point.y) || 0;
                     const categoria = args.point.x || '';
-                    args.text =
-                        '<b>' +
-                        categoria +
-                        '</b><br/>' +
-                        nomeSerie +
-                        ': R$ ' +
-                        formatarNumero(valor, 2);
-                } catch (error) {
+                    args.text = '<b>' + categoria + '</b><br/>' + nomeSerie + ': R$ ' + formatarNumero(valor, 2);
+                } catch (error)
+                {
                     console.error('Erro ao formatar tooltip:', error);
                 }
             },
             legendSettings: {
                 visible: true,
-                position: 'Top',
-            },
-            height: '380px',
+                position: 'Top'
+            },
+            height: '380px'
         });
 
         chart.appendTo('#chartCustoMedioPorFinalidade');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'renderizarCustoMedioPorFinalidade',
-            error,
-        );
-    }
-}
-
-function aplicarFiltroPeriodo(dias) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarCustoMedioPorFinalidade', error);
+    }
+}
+
+function aplicarFiltroPeriodo(dias)
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
+        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - dias);
 
         const dataInicio = document.getElementById('dataInicio');
         const dataFim = document.getElementById('dataFim');
-        if (dataInicio && dataFim) {
+        if (dataInicio && dataFim)
+        {
             dataInicio.value = formatarDataParaInput(periodoAtual.dataInicio);
             dataFim.value = formatarDataParaInput(periodoAtual.dataFim);
         }
 
         carregarDadosDashboard();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'aplicarFiltroPeriodo',
-            error,
-        );
-    }
-}
-
-function aplicarFiltroPersonalizado() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'aplicarFiltroPeriodo', error);
+    }
+}
+
+function aplicarFiltroPersonalizado()
+{
+    try
+    {
         const dataInicioInput = document.getElementById('dataInicio');
         const dataFimInput = document.getElementById('dataFim');
 
-        if (!dataInicioInput?.value || !dataFimInput?.value) {
-            AppToast.show(
-                'Amarelo',
-                'Preencha as datas De e Até para filtrar.',
-                3000,
-            );
+        if (!dataInicioInput?.value || !dataFimInput?.value)
+        {
+            AppToast.show('Amarelo', 'Preencha as datas De e Até para filtrar.', 3000);
             return;
         }
 
         const dataInicio = new Date(dataInicioInput.value + 'T00:00:00');
         const dataFim = new Date(dataFimInput.value + 'T23:59:59');
 
-        if (dataInicio > dataFim) {
-            AppToast.show(
-                'Vermelho',
-                'A data inicial não pode ser maior que a data final.',
-                3000,
-            );
+        if (dataInicio > dataFim)
+        {
+            AppToast.show('Vermelho', 'A data inicial não pode ser maior que a data final.', 3000);
             return;
         }
 
@@ -1992,99 +1835,97 @@
         $('.btn-period').removeClass('active');
 
         carregarDadosDashboard();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'aplicarFiltroPersonalizado',
-            error,
-        );
-    }
-}
-
-function atualizarDashboard() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'aplicarFiltroPersonalizado', error);
+    }
+}
+
+function atualizarDashboard()
+{
+    try
+    {
 
         const dataInicio = document.getElementById('dataInicio');
         const dataFim = document.getElementById('dataFim');
 
-        if (dataInicio && dataFim && dataInicio.value && dataFim.value) {
+        if (dataInicio && dataFim && dataInicio.value && dataFim.value)
+        {
             periodoAtual.dataInicio = new Date(dataInicio.value + 'T00:00:00');
             periodoAtual.dataFim = new Date(dataFim.value + 'T23:59:59');
         }
 
         carregarDadosDashboard();
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'atualizarDashboard',
-            error,
-        );
-    }
-}
-
-function mostrarLoadingGeral(mensagem) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'atualizarDashboard', error);
+    }
+}
+
+function mostrarLoadingGeral(mensagem)
+{
+    try
+    {
         const elemento = document.getElementById('loadingInicialDashboard');
-        if (!elemento) {
+        if (!elemento)
+        {
             console.error('❌ Elemento #loadingInicialDashboard não existe!');
             return;
         }
 
         const textoLoading = elemento.querySelector('.ftx-loading-text');
-        if (textoLoading && mensagem) {
+        if (textoLoading && mensagem)
+        {
             textoLoading.textContent = mensagem;
         }
 
         elemento.classList.remove('d-none');
         elemento.style.display = 'flex';
         elemento.style.opacity = '1';
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'mostrarLoadingGeral',
-            error,
-        );
-    }
-}
-
-function esconderLoadingGeral() {
-    try {
-
-        setTimeout(() => {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'mostrarLoadingGeral', error);
+    }
+}
+
+function esconderLoadingGeral()
+{
+    try
+    {
+
+        setTimeout(() =>
+        {
             const elemento = document.getElementById('loadingInicialDashboard');
-            if (elemento) {
+            if (elemento)
+            {
                 elemento.style.opacity = '0';
                 setTimeout(() => {
                     elemento.classList.add('d-none');
                     elemento.style.display = 'none';
 
-                    const textoLoading =
-                        elemento.querySelector('.ftx-loading-text');
-                    if (textoLoading) {
-                        textoLoading.textContent =
-                            'Carregando Dashboard de Viagens';
+                    const textoLoading = elemento.querySelector('.ftx-loading-text');
+                    if (textoLoading)
+                    {
+                        textoLoading.textContent = 'Carregando Dashboard de Viagens';
                     }
                 }, 300);
             }
         }, 500);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'esconderLoadingGeral',
-            error,
-        );
-    }
-}
-
-function atualizarVariacao(elementoId, valorAtual, valorAnterior) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'esconderLoadingGeral', error);
+    }
+}
+
+function atualizarVariacao(elementoId, valorAtual, valorAnterior)
+{
+    try
+    {
         const elemento = $(`#${elementoId}`);
 
-        if (!valorAnterior || valorAnterior === 0) {
-            elemento
-                .text('-')
-                .removeClass('variacao-positiva variacao-negativa')
-                .addClass('variacao-neutra');
+        if (!valorAnterior || valorAnterior === 0)
+        {
+            elemento.text('-').removeClass('variacao-positiva variacao-negativa').addClass('variacao-neutra');
             return;
         }
 
@@ -2094,44 +1935,39 @@
 
         elemento.text(texto);
 
-        if (variacao > 0) {
-            elemento
-                .removeClass('variacao-negativa variacao-neutra')
-                .addClass('variacao-positiva');
-        } else if (variacao < 0) {
-            elemento
-                .removeClass('variacao-positiva variacao-neutra')
-                .addClass('variacao-negativa');
-        } else {
-            elemento
-                .removeClass('variacao-positiva variacao-negativa')
-                .addClass('variacao-neutra');
-        }
-    } catch (error) {
+        if (variacao > 0)
+        {
+            elemento.removeClass('variacao-negativa variacao-neutra').addClass('variacao-positiva');
+        }
+        else if (variacao < 0)
+        {
+            elemento.removeClass('variacao-positiva variacao-neutra').addClass('variacao-negativa');
+        }
+        else
+        {
+            elemento.removeClass('variacao-positiva variacao-negativa').addClass('variacao-neutra');
+        }
+    } catch (error)
+    {
         console.error('Erro ao atualizar variação:', error);
     }
 }
 
-async function exportarParaPDF() {
-    try {
+async function exportarParaPDF()
+{
+    try
+    {
         console.log('🚀 ===== INICIANDO EXPORTAÇÃO PARA PDF =====');
 
-        if (!periodoAtual.dataInicio || !periodoAtual.dataFim) {
+        if (!periodoAtual.dataInicio || !periodoAtual.dataFim)
+        {
             console.error('❌ Período inválido!');
-            AppToast.show(
-                'Amarelo',
-                'Por favor, selecione um período válido.',
-                3000,
-            );
+            AppToast.show('Amarelo', 'Por favor, selecione um período válido.', 3000);
             return;
         }
         console.log('✅ Período válido:', periodoAtual);
 
-        AppToast.show(
-            'Amarelo',
-            'Capturando gráficos, cards e gerando PDF, aguarde...',
-            8000,
-        );
+        AppToast.show('Amarelo', 'Capturando gráficos, cards e gerando PDF, aguarde...', 8000);
 
         console.log('📊 Iniciando captura de gráficos...');
         const graficos = await capturarGraficos();
@@ -2139,10 +1975,7 @@
 
         console.log('🎨 Iniciando captura de cards...');
         const cards = await capturarCards();
-        console.log(
-            '🎨 Cards capturados:',
-            Object.keys(cards).filter((k) => cards[k]).length,
-        );
+        console.log('🎨 Cards capturados:', Object.keys(cards).filter(k => cards[k]).length);
 
         const dataInicio = periodoAtual.dataInicio.toISOString();
         const dataFim = periodoAtual.dataFim.toISOString();
@@ -2152,34 +1985,32 @@
             dataInicio: dataInicio,
             dataFim: dataFim,
             graficos: graficos,
-            cards: cards,
+            cards: cards
         };
         const payloadJSON = JSON.stringify(payload);
         const tamanhoMB = (payloadJSON.length / 1024 / 1024).toFixed(2);
         console.log('📦 Tamanho total do payload:', tamanhoMB, 'MB');
         console.log('📦 Tamanho por componente:');
         console.log(' 📊 Gráficos:');
-        for (const [key, base64] of Object.entries(graficos)) {
+        for (const [key, base64] of Object.entries(graficos))
+        {
             const tamanhoKB = (base64.length / 1024).toFixed(1);
             console.log(` - ${key}: ${tamanhoKB} KB`);
         }
         console.log(' 🎨 Cards:');
-        for (const [key, base64] of Object.entries(cards)) {
-            if (base64) {
+        for (const [key, base64] of Object.entries(cards))
+        {
+            if (base64)
+            {
                 const tamanhoKB = (base64.length / 1024).toFixed(1);
                 console.log(` - ${key}: ${tamanhoKB} KB`);
             }
         }
 
-        if (parseFloat(tamanhoMB) > 30) {
-            console.error(
-                '❌ PAYLOAD MUITO GRANDE! ASP.NET Core tem limite de 30MB por padrão.',
-            );
-            AppToast.show(
-                'Vermelho',
-                'Payload muito grande. Contate o administrador.',
-                5000,
-            );
+        if (parseFloat(tamanhoMB) > 30)
+        {
+            console.error('❌ PAYLOAD MUITO GRANDE! ASP.NET Core tem limite de 30MB por padrão.');
+            AppToast.show('Vermelho', 'Payload muito grande. Contate o administrador.', 5000);
             return;
         }
 
@@ -2193,14 +2024,15 @@
                 dataInicio: dataInicio,
                 dataFim: dataFim,
                 graficos: graficos,
-                cards: cards,
-            }),
+                cards: cards
+            })
         });
 
         console.log('📥 Resposta recebida:', response);
         console.log(' Status:', response.status, response.statusText);
 
-        if (!response.ok) {
+        if (!response.ok)
+        {
             const errorText = await response.text();
             console.error('❌ Erro na resposta:', errorText);
             throw new Error(`Erro ao gerar PDF: ${errorText}`);
@@ -2212,20 +2044,17 @@
 
         console.log('🔄 Convertendo Blob para Base64...');
         const reader = new FileReader();
-        reader.onloadend = function () {
-            console.log(
-                '✅ Base64 criado:',
-                reader.result.substring(0, 100) + '...',
-            );
+        reader.onloadend = function ()
+        {
+            console.log('✅ Base64 criado:', reader.result.substring(0, 100) + '...');
             const base64PDF = reader.result;
 
             console.log('🖥️ Abrindo modal...');
-            const modal = new bootstrap.Modal(
-                document.getElementById('modalPDFViewer'),
-            );
+            const modal = new bootstrap.Modal(document.getElementById('modalPDFViewer'));
             modal.show();
 
-            $('#modalPDFViewer').one('shown.bs.modal', function () {
+            $('#modalPDFViewer').one('shown.bs.modal', function ()
+            {
                 console.log('✅ Modal aberto, carregando PDF no viewer...');
                 carregarPDFNoViewer(base64PDF);
             });
@@ -2234,68 +2063,67 @@
             console.log('🎉 ===== EXPORTAÇÃO CONCLUÍDA COM SUCESSO =====');
         };
 
-        reader.onerror = function (error) {
+        reader.onerror = function (error)
+        {
             console.error('❌ Erro ao ler Blob:', error);
         };
 
         reader.readAsDataURL(pdfAtualBlob);
-    } catch (error) {
+    } catch (error)
+    {
         console.error('❌ ===== ERRO NA EXPORTAÇÃO =====');
         console.error('Erro:', error);
         console.error('Stack:', error.stack);
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'exportarParaPDF',
-            error,
-        );
-    }
-}
-
-function carregarPDFNoViewer(base64PDF) {
-    try {
-
-        if (pdfViewerInstance) {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'exportarParaPDF', error);
+    }
+}
+
+function carregarPDFNoViewer(base64PDF)
+{
+    try
+    {
+
+        if (pdfViewerInstance)
+        {
             pdfViewerInstance.destroy();
         }
 
         pdfViewerInstance = new ej.pdfviewer.PdfViewer({
             documentPath: base64PDF,
-            serviceUrl:
-                'https://ej2services.syncfusion.com/production/web-services/api/pdfviewer',
+            serviceUrl: 'https://ej2services.syncfusion.com/production/web-services/api/pdfviewer',
             enableToolbar: true,
             enableNavigationToolbar: true,
             enableThumbnail: true,
             zoomMode: 'FitToWidth',
             locale: 'pt-BR',
-            documentLoad: function () {
+            documentLoad: function ()
+            {
                 console.log('✅ PDF carregado no viewer');
 
-                setTimeout(() => {
-                    if (pdfViewerInstance) {
+                setTimeout(() =>
+                {
+                    if (pdfViewerInstance)
+                    {
                         pdfViewerInstance.magnification.fitToWidth();
                     }
                 }, 500);
-            },
+            }
         });
 
         pdfViewerInstance.appendTo('#pdfViewerContainer');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarPDFNoViewer',
-            error,
-        );
-    }
-}
-
-function baixarPDF() {
-    try {
-        if (!pdfAtualBlob) {
-            AppToast.show(
-                'Amarelo',
-                'Nenhum PDF disponível para download.',
-                3000,
-            );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarPDFNoViewer', error);
+    }
+}
+
+function baixarPDF()
+{
+    try
+    {
+        if (!pdfAtualBlob)
+        {
+            AppToast.show('Amarelo', 'Nenhum PDF disponível para download.', 3000);
             return;
         }
 
@@ -2309,191 +2137,129 @@
         document.body.removeChild(a);
 
         AppToast.show('Verde', 'PDF baixado com sucesso!', 3000);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'baixarPDF',
-            error,
-        );
-    }
-}
-
-async function capturarGraficos() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'baixarPDF', error);
+    }
+}
+
+async function capturarGraficos()
+{
+    try
+    {
         console.log('🎯 INICIANDO CAPTURA DE GRÁFICOS...');
 
         const graficos = {};
 
         console.log('🔍 Verificando gráfico de Status...');
         console.log('chartViagensPorStatus:', chartViagensPorStatus);
-        if (chartViagensPorStatus) {
+        if (chartViagensPorStatus)
+        {
             console.log('✅ chartViagensPorStatus existe, capturando...');
-            graficos.status = await exportarGraficoSyncfusion(
-                chartViagensPorStatus,
-                'status',
-            );
-            console.log(
-                '📊 Status capturado:',
-                graficos.status ? 'SIM' : 'NÃO',
-            );
-        } else {
+            graficos.status = await exportarGraficoSyncfusion(chartViagensPorStatus, 'status');
+            console.log('📊 Status capturado:', graficos.status ? 'SIM' : 'NÃO');
+        }
+        else
+        {
             console.warn('⚠️ chartViagensPorStatus não existe!');
         }
 
         console.log('🔍 Verificando gráfico de Motoristas...');
-        const chartMotoristas = document.querySelector(
-            '#chartViagensPorMotorista',
-        );
+        const chartMotoristas = document.querySelector('#chartViagensPorMotorista');
         console.log('Elemento #chartViagensPorMotorista:', chartMotoristas);
-        if (
-            chartMotoristas &&
-            chartMotoristas.ej2_instances &&
-            chartMotoristas.ej2_instances[0]
-        ) {
+        if (chartMotoristas && chartMotoristas.ej2_instances && chartMotoristas.ej2_instances[0])
+        {
             console.log('✅ Motoristas existe, capturando...');
-            graficos.motoristas = await exportarGraficoSyncfusion(
-                chartMotoristas.ej2_instances[0],
-                'motoristas',
-            );
-            console.log(
-                '📊 Motoristas capturado:',
-                graficos.motoristas ? 'SIM' : 'NÃO',
-            );
-        } else {
-            console.warn(
-                '⚠️ chartViagensPorMotorista não encontrado ou sem instância!',
-            );
+            graficos.motoristas = await exportarGraficoSyncfusion(chartMotoristas.ej2_instances[0], 'motoristas');
+            console.log('📊 Motoristas capturado:', graficos.motoristas ? 'SIM' : 'NÃO');
+        }
+        else
+        {
+            console.warn('⚠️ chartViagensPorMotorista não encontrado ou sem instância!');
         }
 
         console.log('🔍 Verificando gráfico de Veículos...');
         const chartVeiculos = document.querySelector('#chartViagensPorVeiculo');
         console.log('Elemento #chartViagensPorVeiculo:', chartVeiculos);
-        if (
-            chartVeiculos &&
-            chartVeiculos.ej2_instances &&
-            chartVeiculos.ej2_instances[0]
-        ) {
+        if (chartVeiculos && chartVeiculos.ej2_instances && chartVeiculos.ej2_instances[0])
+        {
             console.log('✅ Veículos existe, capturando...');
-            graficos.veiculos = await exportarGraficoSyncfusion(
-                chartVeiculos.ej2_instances[0],
-                'veiculos',
-            );
-            console.log(
-                '📊 Veículos capturado:',
-                graficos.veiculos ? 'SIM' : 'NÃO',
-            );
-        } else {
-            console.warn(
-                '⚠️ chartViagensPorVeiculo não encontrado ou sem instância!',
-            );
+            graficos.veiculos = await exportarGraficoSyncfusion(chartVeiculos.ej2_instances[0], 'veiculos');
+            console.log('📊 Veículos capturado:', graficos.veiculos ? 'SIM' : 'NÃO');
+        }
+        else
+        {
+            console.warn('⚠️ chartViagensPorVeiculo não encontrado ou sem instância!');
         }
 
         console.log('🔍 Verificando gráfico de Finalidades...');
-        const chartFinalidades = document.querySelector(
-            '#chartViagensPorFinalidade',
-        );
+        const chartFinalidades = document.querySelector('#chartViagensPorFinalidade');
         console.log('Elemento #chartViagensPorFinalidade:', chartFinalidades);
-        if (
-            chartFinalidades &&
-            chartFinalidades.ej2_instances &&
-            chartFinalidades.ej2_instances[0]
-        ) {
+        if (chartFinalidades && chartFinalidades.ej2_instances && chartFinalidades.ej2_instances[0])
+        {
             console.log('✅ Finalidades existe, capturando...');
-            graficos.finalidades = await exportarGraficoSyncfusion(
-                chartFinalidades.ej2_instances[0],
-                'finalidades',
-            );
-            console.log(
-                '📊 Finalidades capturado:',
-                graficos.finalidades ? 'SIM' : 'NÃO',
-            );
-        } else {
-            console.warn(
-                '⚠️ chartViagensPorFinalidade não encontrado ou sem instância!',
-            );
+            graficos.finalidades = await exportarGraficoSyncfusion(chartFinalidades.ej2_instances[0], 'finalidades');
+            console.log('📊 Finalidades capturado:', graficos.finalidades ? 'SIM' : 'NÃO');
+        }
+        else
+        {
+            console.warn('⚠️ chartViagensPorFinalidade não encontrado ou sem instância!');
         }
 
         console.log('🔍 Verificando gráfico de Requisitantes...');
-        const chartRequisitantes = document.querySelector(
-            '#chartViagensPorRequisitante',
-        );
-        console.log(
-            'Elemento #chartViagensPorRequisitante:',
-            chartRequisitantes,
-        );
-        if (
-            chartRequisitantes &&
-            chartRequisitantes.ej2_instances &&
-            chartRequisitantes.ej2_instances[0]
-        ) {
+        const chartRequisitantes = document.querySelector('#chartViagensPorRequisitante');
+        console.log('Elemento #chartViagensPorRequisitante:', chartRequisitantes);
+        if (chartRequisitantes && chartRequisitantes.ej2_instances && chartRequisitantes.ej2_instances[0])
+        {
             console.log('✅ Requisitantes existe, capturando...');
-            graficos.requisitantes = await exportarGraficoSyncfusion(
-                chartRequisitantes.ej2_instances[0],
-                'requisitantes',
-            );
-            console.log(
-                '📊 Requisitantes capturado:',
-                graficos.requisitantes ? 'SIM' : 'NÃO',
-            );
-        } else {
-            console.warn(
-                '⚠️ chartViagensPorRequisitante não encontrado ou sem instância!',
-            );
+            graficos.requisitantes = await exportarGraficoSyncfusion(chartRequisitantes.ej2_instances[0], 'requisitantes');
+            console.log('📊 Requisitantes capturado:', graficos.requisitantes ? 'SIM' : 'NÃO');
+        }
+        else
+        {
+            console.warn('⚠️ chartViagensPorRequisitante não encontrado ou sem instância!');
         }
 
         console.log('🔍 Verificando gráfico de Setores...');
         const chartSetores = document.querySelector('#chartViagensPorSetor');
         console.log('Elemento #chartViagensPorSetor:', chartSetores);
-        if (
-            chartSetores &&
-            chartSetores.ej2_instances &&
-            chartSetores.ej2_instances[0]
-        ) {
+        if (chartSetores && chartSetores.ej2_instances && chartSetores.ej2_instances[0])
+        {
             console.log('✅ Setores existe, capturando...');
-            graficos.setores = await exportarGraficoSyncfusion(
-                chartSetores.ej2_instances[0],
-                'setores',
-            );
-            console.log(
-                '📊 Setores capturado:',
-                graficos.setores ? 'SIM' : 'NÃO',
-            );
-        } else {
-            console.warn(
-                '⚠️ chartViagensPorSetor não encontrado ou sem instância!',
-            );
+            graficos.setores = await exportarGraficoSyncfusion(chartSetores.ej2_instances[0], 'setores');
+            console.log('📊 Setores capturado:', graficos.setores ? 'SIM' : 'NÃO');
+        }
+        else
+        {
+            console.warn('⚠️ chartViagensPorSetor não encontrado ou sem instância!');
         }
 
         console.log('🎯 CAPTURA FINALIZADA!');
-        console.log(
-            '📊 Total de gráficos capturados:',
-            Object.keys(graficos).filter((k) => graficos[k]).length,
-        );
+        console.log('📊 Total de gráficos capturados:', Object.keys(graficos).filter(k => graficos[k]).length);
         console.log('📊 Gráficos capturados:', graficos);
 
         console.log('🔄 Convertendo SVG para PNG...');
         const graficosPNG = {};
 
-        for (const [key, svgBase64] of Object.entries(graficos)) {
+        for (const [key, svgBase64] of Object.entries(graficos))
+        {
             console.log(`🔄 [${key}] Processando conversão...`);
 
-            if (!svgBase64) {
+            if (!svgBase64)
+            {
                 console.warn(`⚠️ [${key}] SVG vazio, pulando conversão`);
                 graficosPNG[key] = '';
                 continue;
             }
 
-            try {
-                console.log(
-                    ` 🔍 [${key}] Iniciando conversão de ${(svgBase64.length / 1024).toFixed(1)}KB...`,
-                );
+            try
+            {
+                console.log(` 🔍 [${key}] Iniciando conversão de ${(svgBase64.length / 1024).toFixed(1)}KB...`);
                 graficosPNG[key] = await converterSvgParaPng(svgBase64);
                 console.log(`✅ [${key}] SVG convertido para PNG com sucesso!`);
-            } catch (erro) {
-                console.error(
-                    `❌ [${key}] ERRO ao converter SVG para PNG:`,
-                    erro,
-                );
+            } catch (erro)
+            {
+                console.error(`❌ [${key}] ERRO ao converter SVG para PNG:`, erro);
                 console.error(`❌ [${key}] Mensagem:`, erro.message);
                 console.error(`❌ [${key}] Stack:`, erro.stack);
                 graficosPNG[key] = '';
@@ -2501,58 +2267,51 @@
         }
 
         console.log('✅ Todos os gráficos convertidos para PNG!');
-        console.log(
-            '📊 Total de gráficos PNG:',
-            Object.keys(graficosPNG).filter((k) => graficosPNG[k]).length,
-        );
+        console.log('📊 Total de gráficos PNG:', Object.keys(graficosPNG).filter(k => graficosPNG[k]).length);
         return graficosPNG;
-    } catch (error) {
+    } catch (error)
+    {
         console.error('❌ ERRO FATAL em capturarGraficos:', error);
         console.error('Stack trace:', error.stack);
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'capturarGraficos',
-            error,
-        );
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'capturarGraficos', error);
         return {};
     }
 }
 
-async function converterSvgParaPng(svgBase64) {
-    try {
-        return new Promise((resolve, reject) => {
-            try {
+async function converterSvgParaPng(svgBase64)
+{
+    try
+    {
+        return new Promise((resolve, reject) =>
+        {
+            try
+            {
 
                 const base64Data = svgBase64.split(',')[1];
-                if (!base64Data) {
-                    reject(
-                        new Error(
-                            'SVG Base64 inválido - sem dados após vírgula',
-                        ),
-                    );
+                if (!base64Data)
+                {
+                    reject(new Error('SVG Base64 inválido - sem dados após vírgula'));
                     return;
                 }
 
                 const svgString = atob(base64Data);
 
-                const blob = new Blob([svgString], {
-                    type: 'image/svg+xml;charset=utf-8',
-                });
+                const blob = new Blob([svgString], { type: 'image/svg+xml;charset=utf-8' });
                 const url = URL.createObjectURL(blob);
 
                 const img = new Image();
 
-                img.onload = () => {
-                    try {
+                img.onload = () =>
+                {
+                    try
+                    {
 
                         const canvas = document.createElement('canvas');
 
                         canvas.width = img.width > 0 ? img.width : 800;
                         canvas.height = img.height > 0 ? img.height : 600;
 
-                        console.log(
-                            ` 📐 Dimensões: ${canvas.width}x${canvas.height}`,
-                        );
+                        console.log(` 📐 Dimensões: ${canvas.width}x${canvas.height}`);
 
                         const ctx = canvas.getContext('2d');
 
@@ -2565,71 +2324,59 @@
 
                         URL.revokeObjectURL(url);
 
-                        const tamanhoAntes = (svgBase64.length / 1024).toFixed(
-                            1,
-                        );
-                        const tamanhoDepois = (pngBase64.length / 1024).toFixed(
-                            1,
-                        );
-                        console.log(
-                            ` 🔄 ${tamanhoAntes}KB (SVG) → ${tamanhoDepois}KB (PNG)`,
-                        );
+                        const tamanhoAntes = (svgBase64.length / 1024).toFixed(1);
+                        const tamanhoDepois = (pngBase64.length / 1024).toFixed(1);
+                        console.log(` 🔄 ${tamanhoAntes}KB (SVG) → ${tamanhoDepois}KB (PNG)`);
 
                         resolve(pngBase64);
-                    } catch (erro) {
+                    } catch (erro)
+                    {
                         URL.revokeObjectURL(url);
-                        reject(
-                            new Error(
-                                'Erro ao desenhar no canvas: ' + erro.message,
-                            ),
-                        );
+                        reject(new Error('Erro ao desenhar no canvas: ' + erro.message));
                     }
                 };
 
-                img.onerror = (erro) => {
+                img.onerror = (erro) =>
+                {
                     URL.revokeObjectURL(url);
-                    reject(
-                        new Error('Falha ao carregar SVG como imagem: ' + erro),
-                    );
+                    reject(new Error('Falha ao carregar SVG como imagem: ' + erro));
                 };
 
                 img.crossOrigin = 'anonymous';
                 img.src = url;
-            } catch (erro) {
-                reject(
-                    new Error('Erro ao processar SVG Base64: ' + erro.message),
-                );
-            }
-        });
-    } catch (erro) {
+            } catch (erro)
+            {
+                reject(new Error('Erro ao processar SVG Base64: ' + erro.message));
+            }
+        });
+    } catch (erro)
+    {
         console.error('❌ Erro em converterSvgParaPng:', erro);
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'converterSvgParaPng',
-            erro,
-        );
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'converterSvgParaPng', erro);
         throw erro;
     }
 }
 
-function exportarGraficoSyncfusion(chart, nome) {
-    return new Promise((resolve, reject) => {
-        try {
+function exportarGraficoSyncfusion(chart, nome)
+{
+    return new Promise((resolve, reject) =>
+    {
+        try
+        {
             console.log(`🔍 [${nome}] Iniciando captura do gráfico...`);
 
-            if (!chart) {
+            if (!chart)
+            {
                 console.error(`❌ [${nome}] Chart é null ou undefined`);
                 resolve(null);
                 return;
             }
             console.log(`✅ [${nome}] Chart existe:`, chart);
 
-            if (!chart.element) {
+            if (!chart.element)
+            {
                 console.error(`❌ [${nome}] chart.element não existe`);
-                console.log(
-                    `[${nome}] Propriedades do chart:`,
-                    Object.keys(chart),
-                );
+                console.log(`[${nome}] Propriedades do chart:`, Object.keys(chart));
                 resolve(null);
                 return;
             }
@@ -2638,66 +2385,61 @@
             const chartElement = chart.element;
 
             const canvas = chartElement.querySelector('canvas');
-            if (canvas) {
+            if (canvas)
+            {
                 console.log(`✅ [${nome}] Canvas encontrado!`);
-                console.log(
-                    `[${nome}] Canvas dimensões: ${canvas.width}x${canvas.height}`,
-                );
-
-                try {
+                console.log(`[${nome}] Canvas dimensões: ${canvas.width}x${canvas.height}`);
+
+                try
+                {
                     const base64 = canvas.toDataURL('image/png');
-                    console.log(
-                        `✅ [${nome}] Canvas convertido para Base64 (${Math.round(base64.length / 1024)}KB)`,
-                    );
+                    console.log(`✅ [${nome}] Canvas convertido para Base64 (${Math.round(base64.length / 1024)}KB)`);
                     resolve(base64);
                     return;
-                } catch (canvasError) {
-                    console.error(
-                        `❌ [${nome}] Erro ao converter canvas:`,
-                        canvasError,
-                    );
-                }
-            } else {
-                console.warn(
-                    `⚠️ [${nome}] Canvas NÃO encontrado, tentando SVG...`,
-                );
+                }
+                catch (canvasError)
+                {
+                    console.error(`❌ [${nome}] Erro ao converter canvas:`, canvasError);
+                }
+            }
+            else
+            {
+                console.warn(`⚠️ [${nome}] Canvas NÃO encontrado, tentando SVG...`);
             }
 
             const svg = chartElement.querySelector('svg');
-            if (svg) {
+            if (svg)
+            {
                 console.log(`✅ [${nome}] SVG encontrado!`);
 
-                try {
+                try
+                {
 
                     const svgData = new XMLSerializer().serializeToString(svg);
-                    const svgBase64 =
-                        'data:image/svg+xml;base64,' +
-                        btoa(unescape(encodeURIComponent(svgData)));
-
-                    console.log(
-                        `✅ [${nome}] SVG convertido para Base64 (${Math.round(svgBase64.length / 1024)}KB)`,
-                    );
+                    const svgBase64 = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svgData)));
+
+                    console.log(`✅ [${nome}] SVG convertido para Base64 (${Math.round(svgBase64.length / 1024)}KB)`);
                     resolve(svgBase64);
                     return;
-                } catch (svgError) {
-                    console.error(
-                        `❌ [${nome}] Erro ao converter SVG:`,
-                        svgError,
-                    );
-                }
-            } else {
+                }
+                catch (svgError)
+                {
+                    console.error(`❌ [${nome}] Erro ao converter SVG:`, svgError);
+                }
+            }
+            else
+            {
                 console.warn(`⚠️ [${nome}] SVG NÃO encontrado`);
             }
 
             console.error(`❌ [${nome}] Nem canvas nem SVG encontrados!`);
-            console.log(
-                `[${nome}] HTML do elemento:`,
-                chartElement.innerHTML.substring(0, 500),
-            );
+            console.log(`[${nome}] HTML do elemento:`, chartElement.innerHTML.substring(0, 500));
             console.log(`[${nome}] Filhos do elemento:`, chartElement.children);
 
             resolve(null);
-        } catch (error) {
+        }
+        catch (error)
+        {
             console.error(`❌ [${nome}] ERRO GERAL:`, error);
             console.error(`[${nome}] Stack trace:`, error.stack);
             resolve(null);
@@ -2705,330 +2447,252 @@
     });
 }
 
-function limparPDFViewer() {
-    try {
-        if (pdfViewerInstance) {
+function limparPDFViewer()
+{
+    try
+    {
+        if (pdfViewerInstance)
+        {
             pdfViewerInstance.destroy();
             pdfViewerInstance = null;
         }
 
         $('#pdfViewerContainer').empty();
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao limpar PDFViewer:', error);
     }
 }
 
-function inicializarModalAjuste() {
-    try {
+function inicializarModalAjuste()
+{
+    try
+    {
         const modalEl = document.getElementById('modalAjustaViagemDashboard');
-        if (modalEl) {
+        if (modalEl)
+        {
             modalAjustaViagemDashboard = new bootstrap.Modal(modalEl, {
                 keyboard: true,
-                backdrop: 'static',
+                backdrop: 'static'
             });
 
-            const btnAjustar = document.getElementById(
-                'btnAjustarViagemDashboard',
-            );
-            if (btnAjustar) {
+            const btnAjustar = document.getElementById('btnAjustarViagemDashboard');
+            if (btnAjustar)
+            {
                 btnAjustar.addEventListener('click', gravarViagemDashboard);
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'inicializarModalAjuste',
-            error,
-        );
-    }
-}
-
-function abrirModalAjusteViagem() {
-    try {
-        if (!viagemAtualId) {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarModalAjuste', error);
+    }
+}
+
+function abrirModalAjusteViagem()
+{
+    try
+    {
+        if (!viagemAtualId)
+        {
             AppToast.show('Amarelo', 'Nenhuma viagem selecionada', 3000);
             return;
         }
 
-        const modalDetalhes = bootstrap.Modal.getInstance(
-            document.getElementById('modalDetalhesViagem'),
-        );
-        if (modalDetalhes) {
+        const modalDetalhes = bootstrap.Modal.getInstance(document.getElementById('modalDetalhesViagem'));
+        if (modalDetalhes)
+        {
             modalDetalhes.hide();
         }
 
         carregarDadosViagemParaAjuste(viagemAtualId);
 
-        if (modalAjustaViagemDashboard) {
+        if (modalAjustaViagemDashboard)
+        {
             modalAjustaViagemDashboard.show();
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'abrirModalAjusteViagem',
-            error,
-        );
-    }
-}
-
-function carregarDadosViagemParaAjuste(viagemId) {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'abrirModalAjusteViagem', error);
+    }
+}
+
+function carregarDadosViagemParaAjuste(viagemId)
+{
+    try
+    {
         $.ajax({
             type: 'GET',
             url: '/api/Viagem/GetViagem/' + viagemId,
-            success: function (res) {
-                try {
-                    if (res && res.success && res.data) {
+            success: function (res)
+            {
+                try
+                {
+                    if (res && res.success && res.data)
+                    {
                         const viagem = res.data;
 
-                        document.getElementById('txtIdDashboard').value =
-                            viagem.viagemId;
-                        document.getElementById(
-                            'txtNoFichaVistoriaDashboard',
-                        ).value = viagem.noFichaVistoria || '';
-
-                        const lstFinalidade = document.getElementById(
-                            'lstFinalidadeAlteradaDashboard',
-                        );
-                        if (lstFinalidade && lstFinalidade.ej2_instances) {
-                            lstFinalidade.ej2_instances[0].value =
-                                viagem.finalidade || null;
+                        document.getElementById('txtIdDashboard').value = viagem.viagemId;
+                        document.getElementById('txtNoFichaVistoriaDashboard').value = viagem.noFichaVistoria || '';
+
+                        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
+                        if (lstFinalidade && lstFinalidade.ej2_instances)
+                        {
+                            lstFinalidade.ej2_instances[0].value = viagem.finalidade || null;
                         }
 
-                        const lstEvento =
-                            document.getElementById('lstEventoDashboard');
-                        if (lstEvento && lstEvento.ej2_instances) {
-                            if (
-                                viagem.finalidade === 'Evento' &&
-                                viagem.eventoId
-                            ) {
+                        const lstEvento = document.getElementById('lstEventoDashboard');
+                        if (lstEvento && lstEvento.ej2_instances)
+                        {
+                            if (viagem.finalidade === 'Evento' && viagem.eventoId)
+                            {
                                 lstEvento.ej2_instances[0].enabled = true;
-                                lstEvento.ej2_instances[0].value = [
-                                    viagem.eventoId.toString(),
-                                ];
+                                lstEvento.ej2_instances[0].value = [viagem.eventoId.toString()];
                                 $('.esconde-diveventos-dashboard').show();
-                            } else {
+                            } else
+                            {
                                 lstEvento.ej2_instances[0].enabled = false;
                                 lstEvento.ej2_instances[0].value = null;
                                 $('.esconde-diveventos-dashboard').hide();
                             }
                         }
 
-                        document.getElementById(
-                            'txtDataInicialDashboard',
-                        ).value = viagem.dataInicial || '';
-                        document.getElementById(
-                            'txtHoraInicialDashboard',
-                        ).value = viagem.horaInicio || '';
-                        document.getElementById('txtDataFinalDashboard').value =
-                            viagem.dataFinal || '';
-                        document.getElementById('txtHoraFinalDashboard').value =
-                            viagem.horaFim || '';
-
-                        document.getElementById('txtKmInicialDashboard').value =
-                            viagem.kmInicial || '';
-                        document.getElementById('txtKmFinalDashboard').value =
-                            viagem.kmFinal || '';
-
-                        document.getElementById(
-                            'txtRamalRequisitanteDashboard',
-                        ).value = viagem.ramalRequisitante || '';
-
-                        setTimeout(function () {
+                        document.getElementById('txtDataInicialDashboard').value = viagem.dataInicial || '';
+                        document.getElementById('txtHoraInicialDashboard').value = viagem.horaInicio || '';
+                        document.getElementById('txtDataFinalDashboard').value = viagem.dataFinal || '';
+                        document.getElementById('txtHoraFinalDashboard').value = viagem.horaFim || '';
+
+                        document.getElementById('txtKmInicialDashboard').value = viagem.kmInicial || '';
+                        document.getElementById('txtKmFinalDashboard').value = viagem.kmFinal || '';
+
+                        document.getElementById('txtRamalRequisitanteDashboard').value = viagem.ramalRequisitante || '';
+
+                        setTimeout(function() {
                             try {
 
-                                const lstMotorista = document.getElementById(
-                                    'lstMotoristaAlteradoDashboard',
-                                );
-                                if (
-                                    lstMotorista &&
-                                    lstMotorista.ej2_instances &&
-                                    viagem.motoristaId
-                                ) {
-                                    lstMotorista.ej2_instances[0].value =
-                                        viagem.motoristaId;
+                                const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
+                                if (lstMotorista && lstMotorista.ej2_instances && viagem.motoristaId)
+                                {
+                                    lstMotorista.ej2_instances[0].value = viagem.motoristaId;
                                 }
 
-                                const lstVeiculo = document.getElementById(
-                                    'lstVeiculoAlteradoDashboard',
-                                );
-                                if (
-                                    lstVeiculo &&
-                                    lstVeiculo.ej2_instances &&
-                                    viagem.veiculoId
-                                ) {
-                                    lstVeiculo.ej2_instances[0].value =
-                                        viagem.veiculoId;
+                                const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
+                                if (lstVeiculo && lstVeiculo.ej2_instances && viagem.veiculoId)
+                                {
+                                    lstVeiculo.ej2_instances[0].value = viagem.veiculoId;
                                 }
 
-                                const lstRequisitante = document.getElementById(
-                                    'lstRequisitanteAlteradoDashboard',
-                                );
-                                if (
-                                    lstRequisitante &&
-                                    lstRequisitante.ej2_instances &&
-                                    viagem.requisitanteId
-                                ) {
-                                    lstRequisitante.ej2_instances[0].value =
-                                        viagem.requisitanteId;
+                                const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
+                                if (lstRequisitante && lstRequisitante.ej2_instances && viagem.requisitanteId)
+                                {
+                                    lstRequisitante.ej2_instances[0].value = viagem.requisitanteId;
                                 }
 
-                                const lstSetor = document.getElementById(
-                                    'lstSetorSolicitanteAlteradoDashboard',
-                                );
-                                if (
-                                    lstSetor &&
-                                    lstSetor.ej2_instances &&
-                                    viagem.setorSolicitanteId
-                                ) {
-                                    lstSetor.ej2_instances[0].value = [
-                                        viagem.setorSolicitanteId,
-                                    ];
+                                const lstSetor = document.getElementById('lstSetorSolicitanteAlteradoDashboard');
+                                if (lstSetor && lstSetor.ej2_instances && viagem.setorSolicitanteId)
+                                {
+                                    lstSetor.ej2_instances[0].value = [viagem.setorSolicitanteId];
                                 }
                             } catch (error) {
-                                console.error(
-                                    'Erro ao setar valores dos combos:',
-                                    error,
-                                );
+                                console.error('Erro ao setar valores dos combos:', error);
                             }
                         }, 300);
-                    } else {
-                        AppToast.show(
-                            'Amarelo',
-                            res.message || 'Viagem não encontrada',
-                            3000,
-                        );
+
+                    } else
+                    {
+                        AppToast.show('Amarelo', res.message || 'Viagem não encontrada', 3000);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-viagens.js',
-                        'carregarDadosViagemParaAjuste.success',
-                        error,
-                    );
-                }
-            },
-            error: function (xhr, status, error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-viagens.js',
-                    'carregarDadosViagemParaAjuste.error',
-                    error,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'carregarDadosViagemParaAjuste',
-            error,
-        );
-    }
-}
-
-function FinalidadeChangeDashboard() {
-    try {
-        var finalidadeCb = document.getElementById(
-            'lstFinalidadeAlteradaDashboard',
-        ).ej2_instances[0];
-        var eventoDdt =
-            document.getElementById('lstEventoDashboard').ej2_instances[0];
-
-        if (finalidadeCb && eventoDdt) {
-            if (finalidadeCb.value === 'Evento') {
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste.success', error);
+                }
+            },
+            error: function (xhr, status, error)
+            {
+                Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste.error', error);
+            }
+        });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste', error);
+    }
+}
+
+function FinalidadeChangeDashboard()
+{
+    try
+    {
+        var finalidadeCb = document.getElementById('lstFinalidadeAlteradaDashboard').ej2_instances[0];
+        var eventoDdt = document.getElementById('lstEventoDashboard').ej2_instances[0];
+
+        if (finalidadeCb && eventoDdt)
+        {
+            if (finalidadeCb.value === 'Evento')
+            {
                 eventoDdt.enabled = true;
                 $('.esconde-diveventos-dashboard').show();
-            } else {
+            } else
+            {
                 eventoDdt.enabled = false;
                 eventoDdt.value = null;
                 $('.esconde-diveventos-dashboard').hide();
             }
         }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'FinalidadeChangeDashboard',
-            error,
-        );
-    }
-}
-
-function gravarViagemDashboard() {
-    try {
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'FinalidadeChangeDashboard', error);
+    }
+}
+
+function gravarViagemDashboard()
+{
+    try
+    {
         const viagemId = document.getElementById('txtIdDashboard').value;
-        const noFichaVistoria = document.getElementById(
-            'txtNoFichaVistoriaDashboard',
-        ).value;
-
-        const lstFinalidade = document.getElementById(
-            'lstFinalidadeAlteradaDashboard',
-        );
-        const finalidade =
-            lstFinalidade && lstFinalidade.ej2_instances
-                ? lstFinalidade.ej2_instances[0].value
-                : null;
+        const noFichaVistoria = document.getElementById('txtNoFichaVistoriaDashboard').value;
+
+        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
+        const finalidade = lstFinalidade && lstFinalidade.ej2_instances ? lstFinalidade.ej2_instances[0].value : null;
 
         const lstEvento = document.getElementById('lstEventoDashboard');
         let eventoId = null;
-        if (lstEvento && lstEvento.ej2_instances) {
+        if (lstEvento && lstEvento.ej2_instances)
+        {
             const eventoValue = lstEvento.ej2_instances[0].value;
-            if (eventoValue && eventoValue.length > 0) {
+            if (eventoValue && eventoValue.length > 0)
+            {
                 eventoId = eventoValue[0];
             }
         }
 
-        const dataInicial =
-            document.getElementById('txtDataInicialDashboard').value || null;
-        const horaInicial =
-            document.getElementById('txtHoraInicialDashboard').value || null;
-        const dataFinal =
-            document.getElementById('txtDataFinalDashboard').value || null;
-        const horaFinal =
-            document.getElementById('txtHoraFinalDashboard').value || null;
-
-        const kmInicial =
-            parseInt(document.getElementById('txtKmInicialDashboard').value) ||
-            null;
-        const kmFinal =
-            parseInt(document.getElementById('txtKmFinalDashboard').value) ||
-            null;
-
-        const lstMotorista = document.getElementById(
-            'lstMotoristaAlteradoDashboard',
-        );
-        const motoristaId =
-            lstMotorista && lstMotorista.ej2_instances
-                ? lstMotorista.ej2_instances[0].value
-                : null;
-
-        const lstVeiculo = document.getElementById(
-            'lstVeiculoAlteradoDashboard',
-        );
-        const veiculoId =
-            lstVeiculo && lstVeiculo.ej2_instances
-                ? lstVeiculo.ej2_instances[0].value
-                : null;
-
-        const lstSetor = document.getElementById(
-            'lstSetorSolicitanteAlteradoDashboard',
-        );
+        const dataInicial = document.getElementById('txtDataInicialDashboard').value || null;
+        const horaInicial = document.getElementById('txtHoraInicialDashboard').value || null;
+        const dataFinal = document.getElementById('txtDataFinalDashboard').value || null;
+        const horaFinal = document.getElementById('txtHoraFinalDashboard').value || null;
+
+        const kmInicial = parseInt(document.getElementById('txtKmInicialDashboard').value) || null;
+        const kmFinal = parseInt(document.getElementById('txtKmFinalDashboard').value) || null;
+
+        const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
+        const motoristaId = lstMotorista && lstMotorista.ej2_instances ? lstMotorista.ej2_instances[0].value : null;
+
+        const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
+        const veiculoId = lstVeiculo && lstVeiculo.ej2_instances ? lstVeiculo.ej2_instances[0].value : null;
+
+        const lstSetor = document.getElementById('lstSetorSolicitanteAlteradoDashboard');
         let setorSolicitanteId = null;
-        if (lstSetor && lstSetor.ej2_instances) {
+        if (lstSetor && lstSetor.ej2_instances)
+        {
             const setorValue = lstSetor.ej2_instances[0].value;
-            if (setorValue && setorValue.length > 0) {
+            if (setorValue && setorValue.length > 0)
+            {
                 setorSolicitanteId = setorValue[0];
             }
         }
 
-        const lstRequisitante = document.getElementById(
-            'lstRequisitanteAlteradoDashboard',
-        );
-        const requisitanteId =
-            lstRequisitante && lstRequisitante.ej2_instances
-                ? lstRequisitante.ej2_instances[0].value
-                : null;
-
-        const ramalRequisitante =
-            document.getElementById('txtRamalRequisitanteDashboard').value ||
-            null;
+        const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
+        const requisitanteId = lstRequisitante && lstRequisitante.ej2_instances ? lstRequisitante.ej2_instances[0].value : null;
+
+        const ramalRequisitante = document.getElementById('txtRamalRequisitanteDashboard').value || null;
 
         const dados = {
             ViagemId: viagemId,
@@ -3045,7 +2709,7 @@
             VeiculoId: veiculoId,
             SetorSolicitanteId: setorSolicitanteId,
             RequisitanteId: requisitanteId,
-            RamalRequisitante: ramalRequisitante,
+            RamalRequisitante: ramalRequisitante
         };
 
         const btnAjustar = document.getElementById('btnAjustarViagemDashboard');
@@ -3060,92 +2724,84 @@
             url: '/api/Viagem/AtualizarDadosViagemDashboard',
             contentType: 'application/json',
             data: JSON.stringify(dados),
-            success: function (res) {
-                try {
+            success: function (res)
+            {
+                try
+                {
 
                     if (spinner) spinner.classList.add('d-none');
                     if (btnText) btnText.textContent = 'Ajustar Viagem';
                     btnAjustar.disabled = false;
 
-                    if (res.success) {
-
-                        if (modalAjustaViagemDashboard) {
+                    if (res.success)
+                    {
+
+                        if (modalAjustaViagemDashboard)
+                        {
                             modalAjustaViagemDashboard.hide();
                         }
 
-                        AppToast.show(
-                            'Verde',
-                            'Viagem atualizada com sucesso!',
-                            3000,
-                        );
-
-                        mostrarLoadingGeral(
-                            'Recalculando Custos e Atualizando Dashboard...',
-                        );
-
-                        setTimeout(function () {
+                        AppToast.show('Verde', 'Viagem atualizada com sucesso!', 3000);
+
+                        mostrarLoadingGeral('Recalculando Custos e Atualizando Dashboard...');
+
+                        setTimeout(function() {
 
                             carregarDadosDashboard();
                         }, 500);
-                    } else {
-                        AppToast.show(
-                            'Vermelho',
-                            res.message || 'Erro ao atualizar viagem',
-                            4000,
-                        );
+                    } else
+                    {
+                        AppToast.show('Vermelho', res.message || 'Erro ao atualizar viagem', 4000);
                     }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-viagens.js',
-                        'gravarViagemDashboard.success',
-                        error,
-                    );
-                }
-            },
-            error: function (xhr, status, error) {
+                } catch (error)
+                {
+                    Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard.success', error);
+                }
+            },
+            error: function (xhr, status, error)
+            {
 
                 if (spinner) spinner.classList.add('d-none');
                 if (btnText) btnText.textContent = 'Ajustar Viagem';
                 btnAjustar.disabled = false;
 
                 AppToast.show('Vermelho', 'Erro ao gravar: ' + error, 4000);
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-viagens.js',
-                    'gravarViagemDashboard.error',
-                    error,
-                );
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'gravarViagemDashboard',
-            error,
-        );
-    }
-}
-
-function popularAnosDisponiveis() {
-    try {
+                Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard.error', error);
+            }
+        });
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard', error);
+    }
+}
+
+function popularAnosDisponiveis()
+{
+    try
+    {
         const selectAno = document.getElementById('filtroAno');
         if (!selectAno) return;
 
         const anoAtual = new Date().getFullYear();
         selectAno.innerHTML = '<option value="">&lt;Todos os Anos&gt;</option>';
 
-        for (let ano = anoAtual; ano >= anoAtual - 5; ano--) {
+        for (let ano = anoAtual; ano >= anoAtual - 5; ano--)
+        {
             const option = document.createElement('option');
             option.value = ano;
             option.textContent = ano;
             selectAno.appendChild(option);
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao popular anos:', error);
     }
 }
 
-function atualizarLabelPeriodo() {
-    try {
+function atualizarLabelPeriodo()
+{
+    try
+    {
         const label = document.getElementById('periodoAtualLabel');
         if (!label) return;
 
@@ -3154,42 +2810,41 @@
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
-
-        if (dataInicio && dataFim) {
+        const meses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                       'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
+
+        if (dataInicio && dataFim)
+        {
             const dtIni = new Date(dataInicio + 'T00:00:00');
             const dtFim = new Date(dataFim + 'T23:59:59');
             label.textContent = `Período: ${dtIni.toLocaleDateString('pt-BR')} a ${dtFim.toLocaleDateString('pt-BR')}`;
-        } else if (ano && mes) {
+        }
+        else if (ano && mes)
+        {
             label.textContent = `Período: ${meses[parseInt(mes)]}/${ano}`;
-        } else if (ano && !mes) {
+        }
+        else if (ano && !mes)
+        {
             label.textContent = `Período: Ano ${ano} (todos os meses)`;
-        } else if (!ano && mes) {
+        }
+        else if (!ano && mes)
+        {
             label.textContent = `Período: ${meses[parseInt(mes)]} (todos os anos)`;
-        } else {
+        }
+        else
+        {
             label.textContent = 'Exibindo todos os dados';
         }
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao atualizar label de período:', error);
     }
 }
 
-function filtrarPorAnoMes() {
-    try {
+function filtrarPorAnoMes()
+{
+    try
+    {
         const ano = document.getElementById('filtroAno')?.value;
         const mes = document.getElementById('filtroMes')?.value;
 
@@ -3197,7 +2852,8 @@
         document.getElementById('dataFim').value = '';
         $('.btn-period').removeClass('active');
 
-        if (!ano && !mes) {
+        if (!ano && !mes)
+        {
             const anoAtual = new Date().getFullYear();
             periodoAtual.dataInicio = new Date(anoAtual - 5, 0, 1, 0, 0, 0);
             periodoAtual.dataFim = new Date(anoAtual, 11, 31, 23, 59, 59);
@@ -3210,93 +2866,87 @@
         const anoNum = ano ? parseInt(ano) : null;
         const mesNum = mes ? parseInt(mes) : null;
 
-        if (anoNum && mesNum) {
+        if (anoNum && mesNum)
+        {
 
             periodoAtual.dataInicio = new Date(anoNum, mesNum - 1, 1, 0, 0, 0);
             periodoAtual.dataFim = new Date(anoNum, mesNum, 0, 23, 59, 59);
-        } else if (anoNum && !mesNum) {
+        }
+        else if (anoNum && !mesNum)
+        {
 
             periodoAtual.dataInicio = new Date(anoNum, 0, 1, 0, 0, 0);
             periodoAtual.dataFim = new Date(anoNum, 11, 31, 23, 59, 59);
-        } else if (!anoNum && mesNum) {
+        }
+        else if (!anoNum && mesNum)
+        {
 
             const anoAtual = new Date().getFullYear();
             const anosParaBuscar = [];
 
-            for (let a = anoAtual; a >= anoAtual - 5; a--) {
+            for (let a = anoAtual; a >= anoAtual - 5; a--)
+            {
                 anosParaBuscar.push(a);
             }
 
-            periodoAtual.dataInicio = new Date(
-                anoAtual - 5,
-                mesNum - 1,
-                1,
-                0,
-                0,
-                0,
-            );
+            periodoAtual.dataInicio = new Date(anoAtual - 5, mesNum - 1, 1, 0, 0, 0);
             periodoAtual.dataFim = new Date(anoAtual, mesNum, 0, 23, 59, 59);
         }
 
         atualizarLabelPeriodo();
         carregarDadosDashboard();
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao filtrar por ano/mês:', error);
         AppToast.show('Vermelho', 'Erro ao filtrar por ano/mês.', 3000);
     }
 }
 
-function limparFiltroAnoMes() {
-    try {
+function limparFiltroAnoMes()
+{
+    try
+    {
         document.getElementById('filtroAno').value = '';
         document.getElementById('filtroMes').value = '';
 
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
 
         atualizarLabelPeriodo();
         carregarDadosDashboard();
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao limpar filtro ano/mês:', error);
     }
 }
 
-function limparFiltroPeriodo() {
-    try {
+function limparFiltroPeriodo()
+{
+    try
+    {
         document.getElementById('dataInicio').value = '';
         document.getElementById('dataFim').value = '';
         $('.btn-period').removeClass('active');
 
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
 
         atualizarLabelPeriodo();
         carregarDadosDashboard();
-    } catch (error) {
+    } catch (error)
+    {
         console.error('Erro ao limpar filtro de período:', error);
     }
 }
 
-$(document).ready(function () {
-    try {
+$(document).ready(function ()
+{
+    try
+    {
         popularAnosDisponiveis();
         inicializarDashboard();
 
@@ -3306,7 +2956,7 @@
         $('#btnFiltrarPeriodo').on('click', aplicarFiltroPersonalizado);
         $('#btnLimparPeriodo').on('click', limparFiltroPeriodo);
 
-        $('.btn-period').on('click', function () {
+        $('.btn-period').on('click', function() {
             const dias = parseInt($(this).data('dias'));
             if (dias) {
 
@@ -3328,43 +2978,39 @@
         $('#modalPDFViewer').on('hidden.bs.modal', limparPDFViewer);
 
         $('#btnEditarViagemDashboard').on('click', abrirModalAjusteViagem);
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'document.ready',
-            error,
-        );
+    } catch (error)
+    {
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'document.ready', error);
     }
 });
 
-async function capturarCards() {
-    try {
+async function capturarCards()
+{
+    try
+    {
         console.log('🎨 ===== INICIANDO CAPTURA DE CARDS =====');
 
         const cards = {};
 
         const cardIds = [
-            'cardCustoTotal',
-            'cardTotalViagens',
-            'cardCustoMedio',
-            'cardKmTotal',
-            'cardKmMedio',
-            'cardViagensFinalizadas',
-            'cardViagensEmAndamento',
-            'cardViagensAgendadas',
-            'cardViagensCanceladas',
+            'cardCustoTotal', 'cardTotalViagens', 'cardCustoMedio',
+            'cardKmTotal', 'cardKmMedio', 'cardViagensFinalizadas',
+            'cardViagensEmAndamento', 'cardViagensAgendadas', 'cardViagensCanceladas'
         ];
 
-        for (const cardId of cardIds) {
+        for (const cardId of cardIds)
+        {
             const elemento = document.getElementById(cardId);
 
-            if (!elemento) {
+            if (!elemento)
+            {
                 console.warn(`⚠️ [${cardId}] Elemento não encontrado no DOM`);
                 cards[cardId] = '';
                 continue;
             }
 
-            try {
+            try
+            {
                 console.log(`🎨 [${cardId}] Capturando card...`);
 
                 const canvas = await html2canvas(elemento, {
@@ -3372,7 +3018,7 @@
                     scale: 2,
                     logging: false,
                     useCORS: true,
-                    allowTaint: true,
+                    allowTaint: true
                 });
 
                 const base64PNG = canvas.toDataURL('image/png');
@@ -3381,30 +3027,24 @@
 
                 const tamanhoKB = (base64PNG.length / 1024).toFixed(1);
                 console.log(`✅ [${cardId}] Card capturado (${tamanhoKB} KB)`);
-            } catch (erro) {
+            } catch (erro)
+            {
                 console.error(`❌ [${cardId}] Erro ao capturar card:`, erro);
                 console.error(`❌ [${cardId}] Mensagem:`, erro.message);
                 cards[cardId] = '';
             }
         }
 
-        const totalCapturados = Object.keys(cards).filter(
-            (k) => cards[k],
-        ).length;
-        console.log(
-            `✅ Total de cards capturados: ${totalCapturados}/${cardIds.length}`,
-        );
+        const totalCapturados = Object.keys(cards).filter(k => cards[k]).length;
+        console.log(`✅ Total de cards capturados: ${totalCapturados}/${cardIds.length}`);
         console.log('🎨 ===== CAPTURA DE CARDS FINALIZADA =====');
 
         return cards;
-    } catch (error) {
+    } catch (error)
+    {
         console.error('❌ ERRO FATAL em capturarCards:', error);
         console.error('Stack trace:', error.stack);
-        Alerta.TratamentoErroComLinha(
-            'dashboard-viagens.js',
-            'capturarCards',
-            error,
-        );
+        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'capturarCards', error);
         return {};
     }
 }
```
