# wwwroot/js/dashboards/dashboard-motoristas.js

**Mudanca:** GRANDE | **+346** linhas | **-681** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-motoristas.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-motoristas.js
@@ -7,7 +7,7 @@
     vermelho: '#dc2626',
     roxo: '#9d4edd',
     ciano: '#22d3ee',
-    rosa: '#ec4899',
+    rosa: '#ec4899'
 };
 
 function formatarNumero(valor, casasDecimais = 0) {
@@ -19,10 +19,7 @@
         const partes = valorArredondado.split('.');
         const parteInteira = partes[0];
         const parteDecimal = partes[1];
-        const parteInteiraFormatada = parteInteira.replace(
-            /\B(?=(\d{3})+(?!\d))/g,
-            '.',
-        );
+        const parteInteiraFormatada = parteInteira.replace(/\B(?=(\d{3})+(?!\d))/g, '.');
         if (casasDecimais > 0 && parteDecimal) {
             return `${parteInteiraFormatada},${parteDecimal}`;
         }
@@ -51,33 +48,20 @@
 
 let periodoAtual = {
     dataInicio: null,
-    dataFim: null,
+    dataFim: null
 };
 
 let filtroAnoMes = {
     ano: null,
-    mes: null,
+    mes: null
 };
 
 let usarFiltroAnoMes = true;
 
 let motoristaFiltro = null;
 
-const NOMES_MESES = [
-    '',
-    'Janeiro',
-    'Fevereiro',
-    'Março',
-    'Abril',
-    'Maio',
-    'Junho',
-    'Julho',
-    'Agosto',
-    'Setembro',
-    'Outubro',
-    'Novembro',
-    'Dezembro',
-];
+const NOMES_MESES = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
 
 let chartViagensPorMotorista = null;
 let chartKmPorMotorista = null;
@@ -106,7 +90,7 @@
         const loadingEl = document.getElementById('loadingInicialDashboardMot');
         if (loadingEl) {
             loadingEl.style.opacity = '0';
-            setTimeout(function () {
+            setTimeout(function() {
                 loadingEl.style.display = 'none';
             }, 300);
         }
@@ -133,7 +117,7 @@
 }
 
 function esconderLoadingGeral() {
-    $('#loadingGeralDashboardMot').fadeOut(300, function () {
+    $('#loadingGeralDashboardMot').fadeOut(300, function() {
         $(this).remove();
     });
 }
@@ -159,11 +143,7 @@
         esconderLoadingInicial();
         console.error('Erro ao inicializar dashboard:', error);
         if (typeof Alerta !== 'undefined') {
-            Alerta.TratamentoErroComLinha(
-                'dashboard-motoristas.js',
-                'inicializarDashboard',
-                error,
-            );
+            Alerta.TratamentoErroComLinha('dashboard-motoristas.js', 'inicializarDashboard', error);
         }
     }
 }
@@ -172,7 +152,7 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterAnosMesesDisponiveis',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success && response.anos && response.anos.length > 0) {
@@ -180,9 +160,8 @@
             const selectMes = document.getElementById('filtroMes');
 
             if (selectAno) {
-                selectAno.innerHTML =
-                    '<option value="">&lt;Todos os Anos&gt;</option>';
-                response.anos.forEach((ano) => {
+                selectAno.innerHTML = '<option value="">&lt;Todos os Anos&gt;</option>';
+                response.anos.forEach(ano => {
                     const option = document.createElement('option');
                     option.value = ano;
                     option.textContent = ano;
@@ -201,32 +180,16 @@
             filtroAnoMes.mes = hoje.getMonth() + 1;
             usarFiltroAnoMes = false;
 
-            periodoAtual.dataFim = new Date(
-                hoje.getFullYear(),
-                hoje.getMonth(),
-                hoje.getDate(),
-                23,
-                59,
-                59,
-            );
+            periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
             periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
-            periodoAtual.dataInicio.setDate(
-                periodoAtual.dataInicio.getDate() - 30,
-            );
+            periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);
         }
     } catch (error) {
         console.error('Erro ao carregar anos/meses disponíveis:', error);
 
         usarFiltroAnoMes = false;
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
     }
@@ -237,14 +200,14 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterMesesPorAno',
             type: 'GET',
-            data: { ano: ano },
+            data: { ano: ano }
         });
 
         if (response.success && response.meses && response.meses.length > 0) {
             const selectMes = document.getElementById('filtroMes');
             if (selectMes) {
                 selectMes.innerHTML = '';
-                response.meses.forEach((mes) => {
+                response.meses.forEach(mes => {
                     const option = document.createElement('option');
                     option.value = mes;
                     option.textContent = NOMES_MESES[mes];
@@ -269,11 +232,7 @@
 
     if (usarFiltroAnoMes && filtroAnoMes.ano && filtroAnoMes.mes) {
         textoIndicador = `${NOMES_MESES[filtroAnoMes.mes]}/${filtroAnoMes.ano}`;
-    } else if (
-        !usarFiltroAnoMes &&
-        periodoAtual.dataInicio &&
-        periodoAtual.dataFim
-    ) {
+    } else if (!usarFiltroAnoMes && periodoAtual.dataInicio && periodoAtual.dataFim) {
         const inicio = periodoAtual.dataInicio.toLocaleDateString('pt-BR');
         const fim = periodoAtual.dataFim.toLocaleDateString('pt-BR');
         textoIndicador = `${inicio} a ${fim}`;
@@ -299,11 +258,7 @@
 
     if (!selectAno?.value || !selectMes?.value) {
         if (typeof AppToast !== 'undefined') {
-            AppToast.show(
-                'Amarelo',
-                'Selecione o ano e o mês para filtrar.',
-                3000,
-            );
+            AppToast.show('Amarelo', 'Selecione o ano e o mês para filtrar.', 3000);
         }
         return;
     }
@@ -312,9 +267,7 @@
     filtroAnoMes.mes = parseInt(selectMes.value);
     usarFiltroAnoMes = true;
 
-    document
-        .querySelectorAll('.btn-period-mot')
-        .forEach((btn) => btn.classList.remove('active'));
+    document.querySelectorAll('.btn-period-mot').forEach(btn => btn.classList.remove('active'));
 
     atualizarIndicadorPeriodo();
 
@@ -326,11 +279,7 @@
     }
 
     if (typeof AppToast !== 'undefined') {
-        AppToast.show(
-            'Verde',
-            `Exibindo dados de ${NOMES_MESES[filtroAnoMes.mes]}/${filtroAnoMes.ano}`,
-            3000,
-        );
+        AppToast.show('Verde', `Exibindo dados de ${NOMES_MESES[filtroAnoMes.mes]}/${filtroAnoMes.ano}`, 3000);
     }
 }
 
@@ -338,12 +287,12 @@
     if (usarFiltroAnoMes && filtroAnoMes.ano && filtroAnoMes.mes) {
         return {
             ano: filtroAnoMes.ano,
-            mes: filtroAnoMes.mes,
+            mes: filtroAnoMes.mes
         };
     } else {
         return {
             dataInicio: periodoAtual.dataInicio.toISOString(),
-            dataFim: periodoAtual.dataFim.toISOString(),
+            dataFim: periodoAtual.dataFim.toISOString()
         };
     }
 }
@@ -357,11 +306,11 @@
             dataInicio.value = formatarDataParaInput(periodoAtual.dataInicio);
             dataFim.value = formatarDataParaInput(periodoAtual.dataFim);
 
-            dataInicio.addEventListener('change', function () {
+            dataInicio.addEventListener('change', function() {
                 periodoAtual.dataInicio = new Date(this.value + 'T00:00:00');
             });
 
-            dataFim.addEventListener('change', function () {
+            dataFim.addEventListener('change', function() {
                 periodoAtual.dataFim = new Date(this.value + 'T23:59:59');
             });
         }
@@ -385,22 +334,21 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterListaMotoristas',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success) {
             const select = document.getElementById('filtroMotorista');
             if (select) {
-                select.innerHTML =
-                    '<option value="">Todos os Motoristas</option>';
-                response.data.forEach((m) => {
+                select.innerHTML = '<option value="">Todos os Motoristas</option>';
+                response.data.forEach(m => {
                     const option = document.createElement('option');
                     option.value = m.motoristaId;
                     option.textContent = m.nome;
                     select.appendChild(option);
                 });
 
-                select.addEventListener('change', function () {
+                select.addEventListener('change', function() {
                     motoristaFiltro = this.value || null;
                     if (motoristaFiltro) {
                         carregarDadosMotoristaIndividual(motoristaFiltro);
@@ -439,13 +387,8 @@
 }
 
 function esconderSecoesColetivas() {
-    const secoes = [
-        'secaoCardsColetivos',
-        'secaoGraficosColetivos',
-        'secaoGraficosColetivos2',
-        'secaoTabelasColetivas',
-    ];
-    secoes.forEach((id) => {
+    const secoes = ['secaoCardsColetivos', 'secaoGraficosColetivos', 'secaoGraficosColetivos2', 'secaoTabelasColetivas'];
+    secoes.forEach(id => {
         const el = document.getElementById(id);
         if (el) el.style.display = 'none';
     });
@@ -459,13 +402,8 @@
 }
 
 function mostrarSecoesColetivas() {
-    const secoes = [
-        'secaoCardsColetivos',
-        'secaoGraficosColetivos',
-        'secaoGraficosColetivos2',
-        'secaoTabelasColetivas',
-    ];
-    secoes.forEach((id) => {
+    const secoes = ['secaoCardsColetivos', 'secaoGraficosColetivos', 'secaoGraficosColetivos2', 'secaoTabelasColetivas'];
+    secoes.forEach(id => {
         const el = document.getElementById(id);
         if (el) el.style.display = '';
     });
@@ -496,7 +434,7 @@
             carregarDistribuicaoPorTempoEmpresa(),
             carregarMotoristasComCnhProblema(),
             carregarTop10Performance(),
-            carregarHeatmapViagens(),
+            carregarHeatmapViagens()
         ]);
 
         const tempo = ((performance.now() - inicio) / 1000).toFixed(2);
@@ -516,96 +454,37 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterEstatisticasGerais',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success) {
 
-            atualizarElemento(
-                'statTotalMotoristas',
-                formatarNumero(response.totalMotoristas),
-            );
-            atualizarElemento(
-                'statMotoristasAtivos',
-                formatarNumero(response.motoristasAtivos),
-            );
-            atualizarElemento(
-                'statTotalViagens',
-                formatarNumero(response.totalViagens),
-            );
-            atualizarElemento(
-                'statKmTotal',
-                formatarNumero(response.kmTotal) + ' km',
-            );
-            atualizarElemento(
-                'statHorasTotais',
-                formatarNumero(response.horasTotais, 1) + 'h',
-            );
-
-            atualizarElemento(
-                'statEfetivos',
-                formatarNumero(response.efetivos),
-            );
-            atualizarElemento(
-                'statFeristas',
-                formatarNumero(response.feristas),
-            );
-            atualizarElemento(
-                'statCobertura',
-                formatarNumero(response.cobertura),
-            );
-            atualizarElemento(
-                'statInativos',
-                formatarNumero(response.motoristasInativos),
-            );
+            atualizarElemento('statTotalMotoristas', formatarNumero(response.totalMotoristas));
+            atualizarElemento('statMotoristasAtivos', formatarNumero(response.motoristasAtivos));
+            atualizarElemento('statTotalViagens', formatarNumero(response.totalViagens));
+            atualizarElemento('statKmTotal', formatarNumero(response.kmTotal) + ' km');
+            atualizarElemento('statHorasTotais', formatarNumero(response.horasTotais, 1) + 'h');
+
+            atualizarElemento('statEfetivos', formatarNumero(response.efetivos));
+            atualizarElemento('statFeristas', formatarNumero(response.feristas));
+            atualizarElemento('statCobertura', formatarNumero(response.cobertura));
+            atualizarElemento('statInativos', formatarNumero(response.motoristasInativos));
 
             const total = response.motoristasAtivos || 1;
-            atualizarElemento(
-                'percentualAtivos',
-                `${Math.round((response.motoristasAtivos / response.totalMotoristas) * 100)}% do total`,
-            );
-            atualizarElemento(
-                'percentualEfetivos',
-                `${Math.round((response.efetivos / total) * 100)}% dos ativos`,
-            );
-            atualizarElemento(
-                'percentualFeristas',
-                `${Math.round((response.feristas / total) * 100)}% dos ativos`,
-            );
-            atualizarElemento(
-                'percentualCobertura',
-                `${Math.round((response.cobertura / total) * 100)}% dos ativos`,
-            );
-            atualizarElemento(
-                'percentualInativos',
-                `${Math.round((response.motoristasInativos / response.totalMotoristas) * 100)}% do total`,
-            );
-
-            atualizarElemento(
-                'statCnhVencidas',
-                formatarNumero(response.cnhVencidas),
-            );
-            atualizarElemento(
-                'statCnhVencendo',
-                formatarNumero(response.cnhVencendo30Dias),
-            );
-            atualizarElemento(
-                'statTotalMultas',
-                formatarNumero(response.totalMultas),
-            );
-            atualizarElemento(
-                'valorTotalMultas',
-                'R$ ' + formatarValorMonetario(response.valorTotalMultas),
-            );
+            atualizarElemento('percentualAtivos', `${Math.round((response.motoristasAtivos / response.totalMotoristas) * 100)}% do total`);
+            atualizarElemento('percentualEfetivos', `${Math.round((response.efetivos / total) * 100)}% dos ativos`);
+            atualizarElemento('percentualFeristas', `${Math.round((response.feristas / total) * 100)}% dos ativos`);
+            atualizarElemento('percentualCobertura', `${Math.round((response.cobertura / total) * 100)}% dos ativos`);
+            atualizarElemento('percentualInativos', `${Math.round((response.motoristasInativos / response.totalMotoristas) * 100)}% do total`);
+
+            atualizarElemento('statCnhVencidas', formatarNumero(response.cnhVencidas));
+            atualizarElemento('statCnhVencendo', formatarNumero(response.cnhVencendo30Dias));
+            atualizarElemento('statTotalMultas', formatarNumero(response.totalMultas));
+            atualizarElemento('valorTotalMultas', 'R$ ' + formatarValorMonetario(response.valorTotalMultas));
 
             if (response.cnhVencidas > 0) {
-                atualizarElemento(
-                    'alertaCnhVencidas',
-                    'requer atenção imediata!',
-                );
-                document
-                    .getElementById('alertaCnhVencidas')
-                    ?.classList.add('variacao-negativa');
+                atualizarElemento('alertaCnhVencidas', 'requer atenção imediata!');
+                document.getElementById('alertaCnhVencidas')?.classList.add('variacao-negativa');
             }
             if (response.cnhVencendo30Dias > 0) {
                 atualizarElemento('alertaCnhVencendo', 'renovar em breve');
@@ -631,23 +510,17 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterDadosMotorista',
             type: 'GET',
-            data: params,
+            data: params
         });
 
         if (response.success) {
             const m = response.motorista;
             const e = response.estatisticas;
 
-            atualizarElemento(
-                'tituloMotoristaIndividual',
-                `Dados de ${m.nome}`,
-            );
+            atualizarElemento('tituloMotoristaIndividual', `Dados de ${m.nome}`);
             atualizarElemento('nomeMotoristaIndividual', m.nome || '-');
             atualizarElemento('cpfMotoristaIndividual', `CPF: ${m.cpf || '-'}`);
-            atualizarElemento(
-                'pontoMotoristaIndividual',
-                `Ponto: ${m.ponto || '-'}`,
-            );
+            atualizarElemento('pontoMotoristaIndividual', `Ponto: ${m.ponto || '-'}`);
 
             atualizarElemento('dataIngressoIndividual', m.dataIngresso || '-');
             atualizarElemento('tempoEmpresaIndividual', m.tempoEmpresa || '-');
@@ -683,20 +556,13 @@
                     fotoContainer.innerHTML = `<img src="/api/DashboardMotoristas/ObterFotoMotorista/${motoristaId}" class="foto-motorista" alt="Foto do motorista" />`;
                     fotoContainer.className = '';
                 } else {
-                    fotoContainer.innerHTML =
-                        '<i class="fa-duotone fa-user"></i>';
+                    fotoContainer.innerHTML = '<i class="fa-duotone fa-user"></i>';
                     fotoContainer.className = 'foto-motorista-placeholder';
                 }
             }
 
-            atualizarElemento(
-                'statVencimentoCnhIndividual',
-                m.dataVencimentoCnh || '-',
-            );
-            atualizarElemento(
-                'statCategoriaCnhIndividual',
-                m.categoriaCNH || '-',
-            );
+            atualizarElemento('statVencimentoCnhIndividual', m.dataVencimentoCnh || '-');
+            atualizarElemento('statCategoriaCnhIndividual', m.categoriaCNH || '-');
 
             const badgeCnh = document.getElementById('badgeCnhStatus');
             if (badgeCnh) {
@@ -704,57 +570,25 @@
                 if (m.statusCnh === 'Vencida') {
                     badgeCnh.className = 'badge-cnh-vencida';
                     badgeCnh.textContent = 'Vencida';
-                    atualizarElemento(
-                        'diasParaVencerCnh',
-                        `Venceu há ${Math.abs(m.diasParaVencerCnh)} dias`,
-                    );
+                    atualizarElemento('diasParaVencerCnh', `Venceu há ${Math.abs(m.diasParaVencerCnh)} dias`);
                 } else if (m.statusCnh === 'Vencendo') {
                     badgeCnh.className = 'badge-cnh-vencendo';
                     badgeCnh.textContent = 'Vencendo';
-                    atualizarElemento(
-                        'diasParaVencerCnh',
-                        `Vence em ${m.diasParaVencerCnh} dias`,
-                    );
+                    atualizarElemento('diasParaVencerCnh', `Vence em ${m.diasParaVencerCnh} dias`);
                 } else {
                     badgeCnh.className = 'badge-cnh-ok';
                     badgeCnh.textContent = 'OK';
-                    atualizarElemento(
-                        'diasParaVencerCnh',
-                        m.diasParaVencerCnh
-                            ? `Vence em ${m.diasParaVencerCnh} dias`
-                            : '-',
-                    );
+                    atualizarElemento('diasParaVencerCnh', m.diasParaVencerCnh ? `Vence em ${m.diasParaVencerCnh} dias` : '-');
                 }
             }
 
-            atualizarElemento(
-                'statViagensIndividual',
-                formatarNumero(e.totalViagens),
-            );
-            atualizarElemento(
-                'statKmIndividual',
-                formatarNumero(e.kmTotal) + ' km',
-            );
-            atualizarElemento(
-                'statHorasDirigidasIndividual',
-                formatarNumero(e.horasDirigidas, 1) + 'h',
-            );
-            atualizarElemento(
-                'statAbastecimentosIndividual',
-                formatarNumero(e.abastecimentos),
-            );
-            atualizarElemento(
-                'statMultasIndividual',
-                formatarNumero(e.totalMultas),
-            );
-            atualizarElemento(
-                'valorMultasIndividual',
-                'R$ ' + formatarValorMonetario(e.valorMultas),
-            );
-            atualizarElemento(
-                'statMediaKmViagemIndividual',
-                formatarNumero(e.mediaKmPorViagem, 1) + ' km',
-            );
+            atualizarElemento('statViagensIndividual', formatarNumero(e.totalViagens));
+            atualizarElemento('statKmIndividual', formatarNumero(e.kmTotal) + ' km');
+            atualizarElemento('statHorasDirigidasIndividual', formatarNumero(e.horasDirigidas, 1) + 'h');
+            atualizarElemento('statAbastecimentosIndividual', formatarNumero(e.abastecimentos));
+            atualizarElemento('statMultasIndividual', formatarNumero(e.totalMultas));
+            atualizarElemento('valorMultasIndividual', 'R$ ' + formatarValorMonetario(e.valorMultas));
+            atualizarElemento('statMediaKmViagemIndividual', formatarNumero(e.mediaKmPorViagem, 1) + ' km');
         }
     } catch (error) {
         console.error('Erro ao carregar dados do motorista:', error);
@@ -766,17 +600,14 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterTop10PorViagens',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
-                x:
-                    item.motorista.length > 15
-                        ? item.motorista.substring(0, 12) + '...'
-                        : item.motorista,
+            const dados = response.data.map(item => ({
+                x: item.motorista.length > 15 ? item.motorista.substring(0, 12) + '...' : item.motorista,
                 y: item.totalViagens,
-                nomeCompleto: item.motorista,
+                nomeCompleto: item.motorista
             }));
 
             if (chartViagensPorMotorista) {
@@ -787,27 +618,25 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { size: '11px' },
-                    labelIntersectAction: 'Rotate45',
+                    labelIntersectAction: 'Rotate45'
                 },
                 primaryYAxis: {
                     title: 'Viagens',
-                    labelFormat: '{value}',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Column',
-                        fill: CORES_MOTORISTAS.azul,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                    },
-                ],
+                    labelFormat: '{value}'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Column',
+                    fill: CORES_MOTORISTAS.azul,
+                    cornerRadius: { topLeft: 4, topRight: 4 }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} viagens',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y} viagens'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartViagensPorMotorista.appendTo('#chartViagensPorMotorista');
@@ -822,17 +651,14 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterTop10PorKm',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
-                x:
-                    item.motorista.length > 15
-                        ? item.motorista.substring(0, 12) + '...'
-                        : item.motorista,
+            const dados = response.data.map(item => ({
+                x: item.motorista.length > 15 ? item.motorista.substring(0, 12) + '...' : item.motorista,
                 y: Number(item.kmTotal),
-                nomeCompleto: item.motorista,
+                nomeCompleto: item.motorista
             }));
 
             if (chartKmPorMotorista) {
@@ -843,27 +669,25 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { size: '11px' },
-                    labelIntersectAction: 'Rotate45',
+                    labelIntersectAction: 'Rotate45'
                 },
                 primaryYAxis: {
                     title: 'Quilômetros',
-                    labelFormat: '{value}',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Column',
-                        fill: CORES_MOTORISTAS.laranja,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                    },
-                ],
+                    labelFormat: '{value}'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Column',
+                    fill: CORES_MOTORISTAS.laranja,
+                    cornerRadius: { topLeft: 4, topRight: 4 }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} km',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y} km'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartKmPorMotorista.appendTo('#chartKmPorMotorista');
@@ -877,20 +701,20 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterDistribuicaoPorTipo',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success && response.data.length > 0) {
             const coresTipo = {
-                Efetivo: CORES_MOTORISTAS.esmeralda,
-                Ferista: CORES_MOTORISTAS.ciano,
-                Cobertura: CORES_MOTORISTAS.roxo,
+                'Efetivo': CORES_MOTORISTAS.esmeralda,
+                'Ferista': CORES_MOTORISTAS.ciano,
+                'Cobertura': CORES_MOTORISTAS.roxo
             };
 
-            const dados = response.data.map((item) => ({
+            const dados = response.data.map(item => ({
                 x: item.tipo,
                 y: item.quantidade,
-                fill: coresTipo[item.tipo] || CORES_MOTORISTAS.verde,
+                fill: coresTipo[item.tipo] || CORES_MOTORISTAS.verde
             }));
 
             if (chartDistribuicaoTipo) {
@@ -898,31 +722,29 @@
             }
 
             chartDistribuicaoTipo = new ej.charts.AccumulationChart({
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        pointColorMapping: 'fill',
-                        type: 'Pie',
-                        dataLabel: {
-                            visible: true,
-                            position: 'Outside',
-                            name: 'x',
-                            font: { size: '12px', fontWeight: '600' },
-                            connectorStyle: { length: '20px' },
-                        },
-                        innerRadius: '40%',
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    pointColorMapping: 'fill',
+                    type: 'Pie',
+                    dataLabel: {
+                        visible: true,
+                        position: 'Outside',
+                        name: 'x',
+                        font: { size: '12px', fontWeight: '600' },
+                        connectorStyle: { length: '20px' }
                     },
-                ],
+                    innerRadius: '40%'
+                }],
                 legendSettings: {
                     visible: true,
-                    position: 'Bottom',
+                    position: 'Bottom'
                 },
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} motoristas',
-                },
+                    format: '${point.x}: ${point.y} motoristas'
+                }
             });
 
             chartDistribuicaoTipo.appendTo('#chartDistribuicaoTipo');
@@ -936,19 +758,19 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterDistribuicaoPorStatus',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success && response.data.length > 0) {
             const coresStatus = {
-                Ativos: CORES_MOTORISTAS.verde,
-                Inativos: '#6b7280',
+                'Ativos': CORES_MOTORISTAS.verde,
+                'Inativos': '#6b7280'
             };
 
-            const dados = response.data.map((item) => ({
+            const dados = response.data.map(item => ({
                 x: item.status,
                 y: item.quantidade,
-                fill: coresStatus[item.status] || CORES_MOTORISTAS.verde,
+                fill: coresStatus[item.status] || CORES_MOTORISTAS.verde
             }));
 
             if (chartDistribuicaoStatus) {
@@ -956,31 +778,29 @@
             }
 
             chartDistribuicaoStatus = new ej.charts.AccumulationChart({
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        pointColorMapping: 'fill',
-                        type: 'Pie',
-                        dataLabel: {
-                            visible: true,
-                            position: 'Outside',
-                            name: 'x',
-                            font: { size: '12px', fontWeight: '600' },
-                            connectorStyle: { length: '20px' },
-                        },
-                        innerRadius: '40%',
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    pointColorMapping: 'fill',
+                    type: 'Pie',
+                    dataLabel: {
+                        visible: true,
+                        position: 'Outside',
+                        name: 'x',
+                        font: { size: '12px', fontWeight: '600' },
+                        connectorStyle: { length: '20px' }
                     },
-                ],
+                    innerRadius: '40%'
+                }],
                 legendSettings: {
                     visible: true,
-                    position: 'Bottom',
+                    position: 'Bottom'
                 },
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} motoristas',
-                },
+                    format: '${point.x}: ${point.y} motoristas'
+                }
             });
 
             chartDistribuicaoStatus.appendTo('#chartDistribuicaoStatus');
@@ -1001,13 +821,13 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterEvolucaoViagens',
             type: 'GET',
-            data: params,
+            data: params
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
+            const dados = response.data.map(item => ({
                 x: new Date(item.data),
-                y: item.totalViagens,
+                y: item.totalViagens
             }));
 
             if (chartEvolucaoViagens) {
@@ -1019,34 +839,32 @@
                     valueType: 'DateTime',
                     labelFormat: 'dd/MM',
                     intervalType: 'Days',
-                    edgeLabelPlacement: 'Shift',
+                    edgeLabelPlacement: 'Shift'
                 },
                 primaryYAxis: {
                     title: 'Viagens',
-                    labelFormat: '{value}',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'SplineArea',
-                        fill: CORES_MOTORISTAS.esmeralda,
-                        opacity: 0.3,
-                        border: { width: 2, color: CORES_MOTORISTAS.esmeralda },
-                        marker: {
-                            visible: true,
-                            width: 6,
-                            height: 6,
-                            fill: CORES_MOTORISTAS.esmeralda,
-                        },
-                    },
-                ],
+                    labelFormat: '{value}'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'SplineArea',
+                    fill: CORES_MOTORISTAS.esmeralda,
+                    opacity: 0.3,
+                    border: { width: 2, color: CORES_MOTORISTAS.esmeralda },
+                    marker: {
+                        visible: true,
+                        width: 6,
+                        height: 6,
+                        fill: CORES_MOTORISTAS.esmeralda
+                    }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} viagens',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y} viagens'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartEvolucaoViagens.appendTo('#chartEvolucaoViagens');
@@ -1061,16 +879,13 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterTop10PorHoras',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
-                x:
-                    item.motorista.length > 15
-                        ? item.motorista.substring(0, 12) + '...'
-                        : item.motorista,
-                y: item.horasTotais,
+            const dados = response.data.map(item => ({
+                x: item.motorista.length > 15 ? item.motorista.substring(0, 12) + '...' : item.motorista,
+                y: item.horasTotais
             }));
 
             if (chartHorasPorMotorista) {
@@ -1081,27 +896,25 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { size: '11px' },
-                    labelIntersectAction: 'Rotate45',
+                    labelIntersectAction: 'Rotate45'
                 },
                 primaryYAxis: {
                     title: 'Horas',
-                    labelFormat: '{value}h',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Column',
-                        fill: CORES_MOTORISTAS.roxo,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                    },
-                ],
+                    labelFormat: '{value}h'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Column',
+                    fill: CORES_MOTORISTAS.roxo,
+                    cornerRadius: { topLeft: 4, topRight: 4 }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y}h',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y}h'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartHorasPorMotorista.appendTo('#chartHorasPorMotorista');
@@ -1116,16 +929,13 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterTop10PorAbastecimentos',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
-                x:
-                    item.motorista.length > 15
-                        ? item.motorista.substring(0, 12) + '...'
-                        : item.motorista,
-                y: item.totalAbastecimentos,
+            const dados = response.data.map(item => ({
+                x: item.motorista.length > 15 ? item.motorista.substring(0, 12) + '...' : item.motorista,
+                y: item.totalAbastecimentos
             }));
 
             if (chartAbastecimentosPorMotorista) {
@@ -1136,42 +946,35 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { size: '11px' },
-                    labelIntersectAction: 'Rotate45',
+                    labelIntersectAction: 'Rotate45'
                 },
                 primaryYAxis: {
                     title: 'Abastecimentos',
-                    labelFormat: '{value}',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Column',
-                        fill: CORES_MOTORISTAS.amarelo,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                    },
-                ],
+                    labelFormat: '{value}'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Column',
+                    fill: CORES_MOTORISTAS.amarelo,
+                    cornerRadius: { topLeft: 4, topRight: 4 }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} abastecimentos',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y} abastecimentos'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
-            chartAbastecimentosPorMotorista.appendTo(
-                '#chartAbastecimentosPorMotorista',
-            );
+            chartAbastecimentosPorMotorista.appendTo('#chartAbastecimentosPorMotorista');
         } else {
 
             if (chartAbastecimentosPorMotorista) {
                 chartAbastecimentosPorMotorista.destroy();
                 chartAbastecimentosPorMotorista = null;
             }
-            document.getElementById(
-                'chartAbastecimentosPorMotorista',
-            ).innerHTML =
-                '<div class="text-center text-muted py-5"><i class="fa-duotone fa-gas-pump fa-3x mb-3" style="color: #d97706;"></i><p>Não há Abastecimentos no Período</p></div>';
+            document.getElementById('chartAbastecimentosPorMotorista').innerHTML = '<div class="text-center text-muted py-5"><i class="fa-duotone fa-gas-pump fa-3x mb-3" style="color: #d97706;"></i><p>Não há Abastecimentos no Período</p></div>';
         }
     } catch (error) {
         console.error('Erro ao carregar top 10 por abastecimentos:', error);
@@ -1183,16 +986,13 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterMotoristasComMaisMultas',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success && response.data.length > 0) {
-            const dados = response.data.map((item) => ({
-                x:
-                    item.motorista.length > 15
-                        ? item.motorista.substring(0, 12) + '...'
-                        : item.motorista,
-                y: item.totalMultas,
+            const dados = response.data.map(item => ({
+                x: item.motorista.length > 15 ? item.motorista.substring(0, 12) + '...' : item.motorista,
+                y: item.totalMultas
             }));
 
             if (chartMultasPorMotorista) {
@@ -1203,33 +1003,30 @@
                 primaryXAxis: {
                     valueType: 'Category',
                     labelStyle: { size: '11px' },
-                    labelIntersectAction: 'Rotate45',
+                    labelIntersectAction: 'Rotate45'
                 },
                 primaryYAxis: {
                     title: 'Multas',
-                    labelFormat: '{value}',
-                },
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Column',
-                        fill: CORES_MOTORISTAS.vermelho,
-                        cornerRadius: { topLeft: 4, topRight: 4 },
-                    },
-                ],
+                    labelFormat: '{value}'
+                },
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Column',
+                    fill: CORES_MOTORISTAS.vermelho,
+                    cornerRadius: { topLeft: 4, topRight: 4 }
+                }],
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} multas',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x}: ${point.y} multas'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartMultasPorMotorista.appendTo('#chartMultasPorMotorista');
         } else {
-            document.getElementById('chartMultasPorMotorista').innerHTML =
-                '<div class="text-center text-muted py-5"><i class="fa-duotone fa-check-circle fa-3x mb-3 text-success"></i><p>Nenhuma multa registrada no período</p></div>';
+            document.getElementById('chartMultasPorMotorista').innerHTML = '<div class="text-center text-muted py-5"><i class="fa-duotone fa-check-circle fa-3x mb-3 text-success"></i><p>Nenhuma multa registrada no período</p></div>';
         }
     } catch (error) {
         console.error('Erro ao carregar motoristas com mais multas:', error);
@@ -1240,7 +1037,7 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterDistribuicaoPorTempoEmpresa',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success && response.data.length > 0) {
@@ -1249,13 +1046,13 @@
                 CORES_MOTORISTAS.azul,
                 CORES_MOTORISTAS.esmeralda,
                 CORES_MOTORISTAS.laranja,
-                CORES_MOTORISTAS.roxo,
+                CORES_MOTORISTAS.roxo
             ];
 
             const dados = response.data.map((item, index) => ({
                 x: item.faixa,
                 y: item.quantidade,
-                fill: coresFaixas[index % coresFaixas.length],
+                fill: coresFaixas[index % coresFaixas.length]
             }));
 
             if (chartTempoEmpresa) {
@@ -1263,40 +1060,35 @@
             }
 
             chartTempoEmpresa = new ej.charts.AccumulationChart({
-                series: [
-                    {
-                        dataSource: dados,
-                        xName: 'x',
-                        yName: 'y',
-                        pointColorMapping: 'fill',
-                        type: 'Pie',
-                        dataLabel: {
-                            visible: true,
-                            position: 'Outside',
-                            name: 'x',
-                            font: { size: '11px', fontWeight: '600' },
-                            connectorStyle: { length: '15px' },
-                        },
-                        innerRadius: '40%',
+                series: [{
+                    dataSource: dados,
+                    xName: 'x',
+                    yName: 'y',
+                    pointColorMapping: 'fill',
+                    type: 'Pie',
+                    dataLabel: {
+                        visible: true,
+                        position: 'Outside',
+                        name: 'x',
+                        font: { size: '11px', fontWeight: '600' },
+                        connectorStyle: { length: '15px' }
                     },
-                ],
+                    innerRadius: '40%'
+                }],
                 legendSettings: {
                     visible: true,
-                    position: 'Bottom',
+                    position: 'Bottom'
                 },
                 tooltip: {
                     enable: true,
-                    format: '${point.x}: ${point.y} motoristas',
-                },
+                    format: '${point.x}: ${point.y} motoristas'
+                }
             });
 
             chartTempoEmpresa.appendTo('#chartTempoEmpresa');
         }
     } catch (error) {
-        console.error(
-            'Erro ao carregar distribuição por tempo de empresa:',
-            error,
-        );
+        console.error('Erro ao carregar distribuição por tempo de empresa:', error);
     }
 }
 
@@ -1304,26 +1096,21 @@
     try {
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterMotoristasComCnhProblema',
-            type: 'GET',
+            type: 'GET'
         });
 
         if (response.success) {
             const tbody = document.getElementById('tabelaCnhBody');
             if (tbody) {
                 if (response.data.length === 0) {
-                    tbody.innerHTML =
-                        '<tr><td colspan="6" class="text-center py-4"><i class="fa-duotone fa-check-circle text-success me-2"></i>Nenhum motorista com CNH vencida ou vencendo</td></tr>';
+                    tbody.innerHTML = '<tr><td colspan="6" class="text-center py-4"><i class="fa-duotone fa-check-circle text-success me-2"></i>Nenhum motorista com CNH vencida ou vencendo</td></tr>';
                     return;
                 }
 
                 let html = '';
-                response.data.forEach((item) => {
-                    const badgeClass =
-                        item.status === 'Vencida' ? 'bg-danger' : 'bg-warning';
-                    const diasTexto =
-                        item.dias < 0
-                            ? `${Math.abs(item.dias)} dias atrás`
-                            : `${item.dias} dias`;
+                response.data.forEach(item => {
+                    const badgeClass = item.status === 'Vencida' ? 'bg-danger' : 'bg-warning';
+                    const diasTexto = item.dias < 0 ? `${Math.abs(item.dias)} dias atrás` : `${item.dias} dias`;
 
                     html += `
                         <tr>
@@ -1350,28 +1137,22 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterTop10Performance',
             type: 'GET',
-            data: obterParametrosFiltro(),
+            data: obterParametrosFiltro()
         });
 
         if (response.success) {
             const tbody = document.getElementById('tabelaTop10Body');
             if (tbody) {
                 if (response.data.length === 0) {
-                    tbody.innerHTML =
-                        '<tr><td colspan="7" class="text-center py-4">Nenhum dado disponível para o período</td></tr>';
+                    tbody.innerHTML = '<tr><td colspan="7" class="text-center py-4">Nenhum dado disponível para o período</td></tr>';
                     return;
                 }
 
                 let html = '';
                 response.data.forEach((item, index) => {
-                    const medalha =
-                        index < 3 ? ['🥇', '🥈', '🥉'][index] : index + 1;
-                    const badgeTipo =
-                        item.tipo === 'Efetivo'
-                            ? 'badge-efetivo'
-                            : item.tipo === 'Ferista'
-                              ? 'badge-ferista'
-                              : 'badge-cobertura';
+                    const medalha = index < 3 ? ['🥇', '🥈', '🥉'][index] : (index + 1);
+                    const badgeTipo = item.tipo === 'Efetivo' ? 'badge-efetivo' :
+                                      item.tipo === 'Ferista' ? 'badge-ferista' : 'badge-cobertura';
 
                     html += `
                         <tr>
@@ -1405,7 +1186,7 @@
         const response = await $.ajax({
             url: '/api/DashboardMotoristas/ObterHeatmapViagens',
             type: 'GET',
-            data: params,
+            data: params
         });
 
         if (response.success) {
@@ -1414,9 +1195,7 @@
             if (subtitle) {
                 if (motoristaFiltro) {
                     const select = document.getElementById('filtroMotorista');
-                    const nomeMotorista =
-                        select?.options[select.selectedIndex]?.text ||
-                        'Motorista Selecionado';
+                    const nomeMotorista = select?.options[select.selectedIndex]?.text || 'Motorista Selecionado';
                     subtitle.textContent = `(${nomeMotorista})`;
                 } else {
                     subtitle.textContent = '(Todos os Motoristas)';
@@ -1428,15 +1207,7 @@
                 totalEl.textContent = `Total: ${formatarNumero(response.totalViagens)} viagens no período`;
             }
 
-            const diasSemana = [
-                'Domingo',
-                'Segunda',
-                'Terça',
-                'Quarta',
-                'Quinta',
-                'Sexta',
-                'Sábado',
-            ];
+            const diasSemana = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];
             const horas = [];
             for (let h = 0; h < 24; h++) {
                 horas.push(h.toString().padStart(2, '0') + ':00');
@@ -1446,13 +1217,11 @@
             diasSemana.forEach((dia, diaIndex) => {
                 horas.forEach((hora, horaIndex) => {
 
-                    const dado = response.data.find(
-                        (d) => d.x === dia && d.y === hora,
-                    );
+                    const dado = response.data.find(d => d.x === dia && d.y === hora);
                     dadosCompletos.push({
                         x: dia,
                         y: hora,
-                        value: dado ? dado.value : 0,
+                        value: dado ? dado.value : 0
                     });
                 });
             });
@@ -1466,31 +1235,28 @@
                     valueType: 'Category',
                     labels: diasSemana,
                     title: 'Dia da Semana',
-                    labelStyle: { size: '11px' },
+                    labelStyle: { size: '11px' }
                 },
                 primaryYAxis: {
                     valueType: 'Category',
                     labels: horas,
                     title: 'Hora do Dia',
                     labelStyle: { size: '10px' },
-                    isInversed: false,
-                },
-                series: [
-                    {
-                        dataSource: dadosCompletos,
-                        xName: 'x',
-                        yName: 'y',
-                        type: 'Heatmap',
-                        colorName: 'value',
-                        marker: {
-                            dataLabel: {
-                                visible: true,
-                                template:
-                                    '<div style="font-size:10px;font-weight:bold;color:#333;">${value > 0 ? value : ""}</div>',
-                            },
-                        },
-                    },
-                ],
+                    isInversed: false
+                },
+                series: [{
+                    dataSource: dadosCompletos,
+                    xName: 'x',
+                    yName: 'y',
+                    type: 'Heatmap',
+                    colorName: 'value',
+                    marker: {
+                        dataLabel: {
+                            visible: true,
+                            template: '<div style="font-size:10px;font-weight:bold;color:#333;">${value > 0 ? value : ""}</div>'
+                        }
+                    }
+                }],
                 paletteSeries: [
                     { color: '#ecfdf5', value: 0 },
                     { color: '#a7f3d0', value: 1 },
@@ -1498,13 +1264,13 @@
                     { color: '#34d399', value: 5 },
                     { color: '#10b981', value: 10 },
                     { color: '#059669', value: 20 },
-                    { color: '#047857', value: response.valorMaximo || 50 },
+                    { color: '#047857', value: response.valorMaximo || 50 }
                 ],
                 tooltip: {
                     enable: true,
-                    format: '${point.x} às ${point.y}: ${point.value} viagens',
-                },
-                chartArea: { border: { width: 0 } },
+                    format: '${point.x} às ${point.y}: ${point.value} viagens'
+                },
+                chartArea: { border: { width: 0 } }
             });
 
             chartHeatmapViagens.appendTo('#heatmapViagens');
@@ -1520,22 +1286,14 @@
     const container = document.getElementById('heatmapViagens');
     if (!container) return;
 
-    const diasSemana = [
-        'Domingo',
-        'Segunda',
-        'Terça',
-        'Quarta',
-        'Quinta',
-        'Sexta',
-        'Sábado',
-    ];
+    const diasSemana = ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'];
     const horas = [];
     for (let h = 0; h < 24; h++) {
         horas.push(h.toString().padStart(2, '0') + ':00');
     }
 
     const dadosMap = {};
-    dados.forEach((d) => {
+    dados.forEach(d => {
         dadosMap[`${d.x}-${d.y}`] = d.value;
     });
 
@@ -1552,28 +1310,21 @@
     }
 
     let html = '<div style="overflow-x: auto;">';
-    html +=
-        '<table style="border-collapse: collapse; width: 100%; min-width: 600px;">';
-
-    html +=
-        '<thead><tr><th style="padding: 8px; background: #f3f4f6; border: 1px solid #e5e7eb; font-size: 11px;"></th>';
-    diasSemana.forEach((dia) => {
+    html += '<table style="border-collapse: collapse; width: 100%; min-width: 600px;">';
+
+    html += '<thead><tr><th style="padding: 8px; background: #f3f4f6; border: 1px solid #e5e7eb; font-size: 11px;"></th>';
+    diasSemana.forEach(dia => {
         html += `<th style="padding: 8px; background: var(--mot-gradient); color: white; border: 1px solid #e5e7eb; font-size: 11px; text-align: center;">${dia}</th>`;
     });
     html += '</tr></thead>';
 
     html += '<tbody>';
-    horas.forEach((hora) => {
+    horas.forEach(hora => {
         html += `<tr><td style="padding: 6px 8px; background: #f9fafb; border: 1px solid #e5e7eb; font-size: 10px; font-weight: 600; text-align: right; white-space: nowrap;">${hora}</td>`;
-        diasSemana.forEach((dia) => {
+        diasSemana.forEach(dia => {
             const value = dadosMap[`${dia}-${hora}`] || 0;
             const color = getColor(value, valorMaximo);
-            const textColor =
-                value > 0
-                    ? value > valorMaximo * 0.5
-                        ? 'white'
-                        : '#111827'
-                    : 'transparent';
+            const textColor = value > 0 ? (value > valorMaximo * 0.5 ? 'white' : '#111827') : 'transparent';
             html += `<td style="padding: 4px; background: ${color}; border: 1px solid #e5e7eb; text-align: center; font-size: 10px; font-weight: bold; color: ${textColor}; cursor: ${value > 0 ? 'pointer' : 'default'};" title="${dia} às ${hora}: ${value} viagem(ns)">${value > 0 ? value : ''}</td>`;
         });
         html += '</tr>';
@@ -1590,94 +1341,63 @@
         const params = obterParametrosFiltro();
         params.motoristaId = motoristaId;
 
-        const [
-            resultMotorista,
-            resultTop10Viagens,
-            resultTop10Km,
-            resultPosicao,
-        ] = await Promise.allSettled([
+        const [resultMotorista, resultTop10Viagens, resultTop10Km, resultPosicao] = await Promise.allSettled([
             $.ajax({
                 url: '/api/DashboardMotoristas/ObterDadosMotorista',
                 type: 'GET',
-                data: params,
+                data: params
             }),
             $.ajax({
                 url: '/api/DashboardMotoristas/ObterTop10PorViagens',
                 type: 'GET',
-                data: obterParametrosFiltro(),
+                data: obterParametrosFiltro()
             }),
             $.ajax({
                 url: '/api/DashboardMotoristas/ObterTop10PorKm',
                 type: 'GET',
-                data: obterParametrosFiltro(),
+                data: obterParametrosFiltro()
             }),
             $.ajax({
                 url: '/api/DashboardMotoristas/ObterPosicaoMotorista',
                 type: 'GET',
-                data: params,
-            }),
+                data: params
+            })
         ]);
 
-        const responseMotorista =
-            resultMotorista.status === 'fulfilled'
-                ? resultMotorista.value
-                : null;
-        const responseTop10Viagens =
-            resultTop10Viagens.status === 'fulfilled'
-                ? resultTop10Viagens.value
-                : null;
-        const responseTop10Km =
-            resultTop10Km.status === 'fulfilled' ? resultTop10Km.value : null;
-        const responsePosicao =
-            resultPosicao.status === 'fulfilled' ? resultPosicao.value : null;
+        const responseMotorista = resultMotorista.status === 'fulfilled' ? resultMotorista.value : null;
+        const responseTop10Viagens = resultTop10Viagens.status === 'fulfilled' ? resultTop10Viagens.value : null;
+        const responseTop10Km = resultTop10Km.status === 'fulfilled' ? resultTop10Km.value : null;
+        const responsePosicao = resultPosicao.status === 'fulfilled' ? resultPosicao.value : null;
 
         if (responseMotorista?.success) {
             dadosMotoristaAtual = {
                 nome: responseMotorista.motorista.nome,
                 viagens: responseMotorista.estatisticas.totalViagens,
                 km: responseMotorista.estatisticas.kmTotal,
-                posicaoViagens: responsePosicao?.success
-                    ? responsePosicao.posicaoViagens
-                    : null,
-                posicaoKm: responsePosicao?.success
-                    ? responsePosicao.posicaoKm
-                    : null,
+                posicaoViagens: responsePosicao?.success ? responsePosicao.posicaoViagens : null,
+                posicaoKm: responsePosicao?.success ? responsePosicao.posicaoKm : null
             };
 
             if (responseTop10Viagens?.success && responseTop10Viagens.data) {
-                montarTabelaComparativaViagens(
-                    responseTop10Viagens.data,
-                    dadosMotoristaAtual,
-                );
+                montarTabelaComparativaViagens(responseTop10Viagens.data, dadosMotoristaAtual);
             } else {
-                document.getElementById(
-                    'tabelaComparativaViagensBody',
-                ).innerHTML =
-                    '<tr><td colspan="3" class="text-center py-3 text-muted">Sem dados disponíveis</td></tr>';
+                document.getElementById('tabelaComparativaViagensBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-muted">Sem dados disponíveis</td></tr>';
             }
 
             if (responseTop10Km?.success && responseTop10Km.data) {
-                montarTabelaComparativaKm(
-                    responseTop10Km.data,
-                    dadosMotoristaAtual,
-                );
+                montarTabelaComparativaKm(responseTop10Km.data, dadosMotoristaAtual);
             } else {
-                document.getElementById('tabelaComparativaKmBody').innerHTML =
-                    '<tr><td colspan="3" class="text-center py-3 text-muted">Sem dados disponíveis</td></tr>';
+                document.getElementById('tabelaComparativaKmBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-muted">Sem dados disponíveis</td></tr>';
             }
         } else {
 
-            document.getElementById('tabelaComparativaViagensBody').innerHTML =
-                '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar dados</td></tr>';
-            document.getElementById('tabelaComparativaKmBody').innerHTML =
-                '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar dados</td></tr>';
+            document.getElementById('tabelaComparativaViagensBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar dados</td></tr>';
+            document.getElementById('tabelaComparativaKmBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar dados</td></tr>';
         }
     } catch (error) {
         console.error('Erro ao carregar tabelas comparativas:', error);
-        document.getElementById('tabelaComparativaViagensBody').innerHTML =
-            '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar</td></tr>';
-        document.getElementById('tabelaComparativaKmBody').innerHTML =
-            '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar</td></tr>';
+        document.getElementById('tabelaComparativaViagensBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar</td></tr>';
+        document.getElementById('tabelaComparativaKmBody').innerHTML = '<tr><td colspan="3" class="text-center py-3 text-danger">Erro ao carregar</td></tr>';
     }
 }
 
@@ -1685,24 +1405,17 @@
     const tbody = document.getElementById('tabelaComparativaViagensBody');
     if (!tbody) return;
 
-    const posicaoNoTop10 = top10Data.findIndex(
-        (item) =>
-            item.motorista.toLowerCase().trim() ===
-            motorista.nome.toLowerCase().trim(),
+    const posicaoNoTop10 = top10Data.findIndex(item =>
+        item.motorista.toLowerCase().trim() === motorista.nome.toLowerCase().trim()
     );
 
     let html = '';
     const medalhas = ['🥇', '🥈', '🥉'];
 
     top10Data.forEach((item, index) => {
-        const ehMotorista =
-            item.motorista.toLowerCase().trim() ===
-            motorista.nome.toLowerCase().trim();
+        const ehMotorista = item.motorista.toLowerCase().trim() === motorista.nome.toLowerCase().trim();
         const classe = ehMotorista ? 'linha-motorista-selecionado' : '';
-        const medalha =
-            index < 3
-                ? `<span class="medalha-posicao">${medalhas[index]}</span>`
-                : index + 1;
+        const medalha = index < 3 ? `<span class="medalha-posicao">${medalhas[index]}</span>` : (index + 1);
 
         html += `
             <tr class="${classe}">
@@ -1715,7 +1428,7 @@
 
     if (posicaoNoTop10 === -1) {
 
-        const posicaoReal = motorista.posicaoViagens || top10Data.length + 1;
+        const posicaoReal = motorista.posicaoViagens || (top10Data.length + 1);
 
         html += `
             <tr class="linha-separador-comparativa">
@@ -1736,24 +1449,17 @@
     const tbody = document.getElementById('tabelaComparativaKmBody');
     if (!tbody) return;
 
-    const posicaoNoTop10 = top10Data.findIndex(
-        (item) =>
-            item.motorista.toLowerCase().trim() ===
-            motorista.nome.toLowerCase().trim(),
+    const posicaoNoTop10 = top10Data.findIndex(item =>
+        item.motorista.toLowerCase().trim() === motorista.nome.toLowerCase().trim()
     );
 
     let html = '';
     const medalhas = ['🥇', '🥈', '🥉'];
 
     top10Data.forEach((item, index) => {
-        const ehMotorista =
-            item.motorista.toLowerCase().trim() ===
-            motorista.nome.toLowerCase().trim();
+        const ehMotorista = item.motorista.toLowerCase().trim() === motorista.nome.toLowerCase().trim();
         const classe = ehMotorista ? 'linha-motorista-selecionado' : '';
-        const medalha =
-            index < 3
-                ? `<span class="medalha-posicao">${medalhas[index]}</span>`
-                : index + 1;
+        const medalha = index < 3 ? `<span class="medalha-posicao">${medalhas[index]}</span>` : (index + 1);
 
         html += `
             <tr class="${classe}">
@@ -1766,7 +1472,7 @@
 
     if (posicaoNoTop10 === -1) {
 
-        const posicaoReal = motorista.posicaoKm || top10Data.length + 1;
+        const posicaoReal = motorista.posicaoKm || (top10Data.length + 1);
 
         html += `
             <tr class="linha-separador-comparativa">
@@ -1785,14 +1491,7 @@
 
 function aplicarFiltroPeriodo(dias, btnElement) {
     const hoje = new Date();
-    periodoAtual.dataFim = new Date(
-        hoje.getFullYear(),
-        hoje.getMonth(),
-        hoje.getDate(),
-        23,
-        59,
-        59,
-    );
+    periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
     periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
     periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - dias);
 
@@ -1805,9 +1504,7 @@
         dataFim.value = formatarDataParaInput(periodoAtual.dataFim);
     }
 
-    document
-        .querySelectorAll('.btn-period-mot')
-        .forEach((btn) => btn.classList.remove('active'));
+    document.querySelectorAll('.btn-period-mot').forEach(btn => btn.classList.remove('active'));
 
     if (btnElement) {
         btnElement.classList.add('active');
@@ -1829,11 +1526,7 @@
 
     if (!dataInicioInput?.value || !dataFimInput?.value) {
         if (typeof AppToast !== 'undefined') {
-            AppToast.show(
-                'Amarelo',
-                'Preencha as datas De e Até para filtrar.',
-                3000,
-            );
+            AppToast.show('Amarelo', 'Preencha as datas De e Até para filtrar.', 3000);
         }
         return;
     }
@@ -1843,20 +1536,14 @@
 
     if (periodoAtual.dataInicio > periodoAtual.dataFim) {
         if (typeof AppToast !== 'undefined') {
-            AppToast.show(
-                'Amarelo',
-                'A data inicial não pode ser maior que a final.',
-                3000,
-            );
+            AppToast.show('Amarelo', 'A data inicial não pode ser maior que a final.', 3000);
         }
         return;
     }
 
     usarFiltroAnoMes = false;
 
-    document
-        .querySelectorAll('.btn-period-mot')
-        .forEach((btn) => btn.classList.remove('active'));
+    document.querySelectorAll('.btn-period-mot').forEach(btn => btn.classList.remove('active'));
 
     atualizarIndicadorPeriodo();
 
@@ -1878,17 +1565,13 @@
 
     esconderSecaoIndividual();
 
-    const tbodyViagens = document.getElementById(
-        'tabelaComparativaViagensBody',
-    );
+    const tbodyViagens = document.getElementById('tabelaComparativaViagensBody');
     const tbodyKm = document.getElementById('tabelaComparativaKmBody');
     if (tbodyViagens) {
-        tbodyViagens.innerHTML =
-            '<tr><td colspan="3" class="text-center py-3 text-muted">Selecione um motorista</td></tr>';
+        tbodyViagens.innerHTML = '<tr><td colspan="3" class="text-center py-3 text-muted">Selecione um motorista</td></tr>';
     }
     if (tbodyKm) {
-        tbodyKm.innerHTML =
-            '<tr><td colspan="3" class="text-center py-3 text-muted">Selecione um motorista</td></tr>';
+        tbodyKm.innerHTML = '<tr><td colspan="3" class="text-center py-3 text-muted">Selecione um motorista</td></tr>';
     }
 
     carregarEvolucaoViagens();
@@ -1911,9 +1594,7 @@
         }
     });
 
-    document
-        .querySelectorAll('.btn-period-mot')
-        .forEach((btn) => btn.classList.remove('active'));
+    document.querySelectorAll('.btn-period-mot').forEach(btn => btn.classList.remove('active'));
 
     if (typeof AppToast !== 'undefined') {
         AppToast.show('Verde', 'Filtro de período restaurado', 2000);
@@ -1932,9 +1613,7 @@
 
     usarFiltroAnoMes = true;
 
-    document
-        .querySelectorAll('.btn-period-mot')
-        .forEach((btn) => btn.classList.remove('active'));
+    document.querySelectorAll('.btn-period-mot').forEach(btn => btn.classList.remove('active'));
 
     atualizarIndicadorPeriodo();
     carregarDadosDashboard();
@@ -1965,7 +1644,7 @@
     carregarHeatmapViagens();
 }
 
-$(document).ready(function () {
+$(document).ready(function() {
 
     inicializarDashboard();
 
@@ -1979,34 +1658,20 @@
 
     $('#btnLimparPeriodo').on('click', limparFiltroPeriodo);
 
-    $('#filtroAno').on('change', function () {
+    $('#filtroAno').on('change', function() {
         const ano = parseInt(this.value);
         if (ano) {
             carregarMesesPorAno(ano);
         }
     });
 
-    $('#btn7Dias').on('click', function () {
-        aplicarFiltroPeriodo(7, this);
-    });
-    $('#btn15Dias').on('click', function () {
-        aplicarFiltroPeriodo(15, this);
-    });
-    $('#btn30Dias').on('click', function () {
-        aplicarFiltroPeriodo(30, this);
-    });
-    $('#btn60Dias').on('click', function () {
-        aplicarFiltroPeriodo(60, this);
-    });
-    $('#btn90Dias').on('click', function () {
-        aplicarFiltroPeriodo(90, this);
-    });
-    $('#btn180Dias').on('click', function () {
-        aplicarFiltroPeriodo(180, this);
-    });
-    $('#btn365Dias').on('click', function () {
-        aplicarFiltroPeriodo(365, this);
-    });
+    $('#btn7Dias').on('click', function() { aplicarFiltroPeriodo(7, this); });
+    $('#btn15Dias').on('click', function() { aplicarFiltroPeriodo(15, this); });
+    $('#btn30Dias').on('click', function() { aplicarFiltroPeriodo(30, this); });
+    $('#btn60Dias').on('click', function() { aplicarFiltroPeriodo(60, this); });
+    $('#btn90Dias').on('click', function() { aplicarFiltroPeriodo(90, this); });
+    $('#btn180Dias').on('click', function() { aplicarFiltroPeriodo(180, this); });
+    $('#btn365Dias').on('click', function() { aplicarFiltroPeriodo(365, this); });
 
     $('#btnFiltrar').on('click', aplicarFiltroPersonalizado);
 });
```
