# wwwroot/js/administracao.js

**Mudanca:** GRANDE | **+234** linhas | **-392** linhas

---

```diff
--- JANEIRO: wwwroot/js/administracao.js
+++ ATUAL: wwwroot/js/administracao.js
@@ -18,10 +18,10 @@
     'rgba(199, 199, 199, 0.8)',
     'rgba(83, 102, 255, 0.8)',
     'rgba(255, 99, 255, 0.8)',
-    'rgba(99, 255, 132, 0.8)',
+    'rgba(99, 255, 132, 0.8)'
 ];
 
-const coresBorda = cores.map((c) => c.replace('0.8', '1'));
+const coresBorda = cores.map(c => c.replace('0.8', '1'));
 
 document.addEventListener('DOMContentLoaded', function () {
     try {
@@ -39,9 +39,7 @@
     try {
         const hoje = new Date();
         const dataFim = hoje.toISOString().split('T')[0];
-        const dataInicio = new Date(hoje.getTime() - 30 * 24 * 60 * 60 * 1000)
-            .toISOString()
-            .split('T')[0];
+        const dataInicio = new Date(hoje.getTime() - 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
 
         document.getElementById('dataInicio').value = dataInicio;
         document.getElementById('dataFim').value = dataFim;
@@ -54,9 +52,7 @@
     try {
         const hoje = new Date();
         const dataFim = hoje.toISOString().split('T')[0];
-        const dataInicio = new Date(hoje.getTime() - dias * 24 * 60 * 60 * 1000)
-            .toISOString()
-            .split('T')[0];
+        const dataInicio = new Date(hoje.getTime() - dias * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
 
         document.getElementById('dataInicio').value = dataInicio;
         document.getElementById('dataFim').value = dataFim;
@@ -78,10 +74,7 @@
     }
 }
 
-function mostrarSemDados(
-    containerId,
-    mensagem = 'Sem dados para o período selecionado',
-) {
+function mostrarSemDados(containerId, mensagem = 'Sem dados para o período selecionado') {
     try {
         const container = document.getElementById(containerId);
         if (container) {
@@ -118,7 +111,7 @@
             carregarCustoPorFinalidade(),
             carregarComparativoPropiosTerceirizados(),
             carregarEficienciaFrota(),
-            carregarEvolucaoMensalCustos(),
+            carregarEvolucaoMensalCustos()
         ]);
 
         resultados.forEach((r, i) => {
@@ -126,6 +119,7 @@
                 console.error(`Erro no gráfico ${i}:`, r.reason);
             }
         });
+
     } catch (e) {
         console.error('Erro carregarTodosGraficos:', e);
     }
@@ -133,13 +127,8 @@
 
 function mostrarLoadingCards() {
     try {
-        const cards = [
-            'cardVeiculosAtivos',
-            'cardMotoristasAtivos',
-            'cardViagensRealizadas',
-            'cardTotalKm',
-        ];
-        cards.forEach((id) => {
+        const cards = ['cardVeiculosAtivos', 'cardMotoristasAtivos', 'cardViagensRealizadas', 'cardTotalKm'];
+        cards.forEach(id => {
             const el = document.getElementById(id);
             if (el) el.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
         });
@@ -154,9 +143,9 @@
             { id: 'cardVeiculosAtivos', valor: '0' },
             { id: 'cardMotoristasAtivos', valor: '0' },
             { id: 'cardViagensRealizadas', valor: '0' },
-            { id: 'cardTotalKm', valor: '0 km' },
+            { id: 'cardTotalKm', valor: '0 km' }
         ];
-        cards.forEach((c) => {
+        cards.forEach(c => {
             const el = document.getElementById(c.id);
             if (el && el.innerHTML.includes('fa-spinner')) {
                 el.textContent = c.valor;
@@ -169,22 +158,15 @@
 
 async function carregarResumoGeral() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterResumoGeralFrota?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterResumoGeralFrota?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados) {
             const dados = result.dados;
-            document.getElementById('cardVeiculosAtivos').textContent =
-                formatarNumero(dados.veiculosAtivos);
-            document.getElementById('cardMotoristasAtivos').textContent =
-                formatarNumero(dados.motoristasAtivos);
-            document.getElementById('cardViagensRealizadas').textContent =
-                formatarNumero(dados.viagensRealizadas);
-            document.getElementById('cardTotalKm').textContent = formatarKm(
-                dados.totalKm,
-            );
+            document.getElementById('cardVeiculosAtivos').textContent = formatarNumero(dados.veiculosAtivos);
+            document.getElementById('cardMotoristasAtivos').textContent = formatarNumero(dados.motoristasAtivos);
+            document.getElementById('cardViagensRealizadas').textContent = formatarNumero(dados.viagensRealizadas);
+            document.getElementById('cardTotalKm').textContent = formatarKm(dados.totalKm);
         } else {
             pararLoadingCards();
         }
@@ -196,54 +178,29 @@
 
 async function carregarEstatisticasNormalizacao() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterEstatisticasNormalizacao?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterEstatisticasNormalizacao?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados) {
             const dados = result.dados;
 
-            if (
-                dados.resumo &&
-                (dados.resumo.viagensOriginais > 0 ||
-                    dados.resumo.viagensNormalizadas > 0)
-            ) {
+            if (dados.resumo && (dados.resumo.viagensOriginais > 0 || dados.resumo.viagensNormalizadas > 0)) {
                 renderizarPizzaNormalizacao(dados.resumo);
-                document.getElementById(
-                    'lblPercentualNormalizadas',
-                ).textContent =
+                document.getElementById('lblPercentualNormalizadas').textContent =
                     `${dados.resumo.percentualNormalizadas}% das viagens foram normalizadas`;
             } else {
-                mostrarSemDados(
-                    'containerPizzaNormalizacao',
-                    'Sem dados de normalização',
-                );
-                document.getElementById(
-                    'lblPercentualNormalizadas',
-                ).textContent = '';
-            }
-
-            if (
-                dados.porTipoNormalizacao &&
-                dados.porTipoNormalizacao.length > 0
-            ) {
+                mostrarSemDados('containerPizzaNormalizacao', 'Sem dados de normalização');
+                document.getElementById('lblPercentualNormalizadas').textContent = '';
+            }
+
+            if (dados.porTipoNormalizacao && dados.porTipoNormalizacao.length > 0) {
                 renderizarBarrasTipoNormalizacao(dados.porTipoNormalizacao);
             } else {
-                mostrarSemDados(
-                    'containerNormalizacaoTipo',
-                    'Sem tipos de normalização',
-                );
+                mostrarSemDados('containerNormalizacaoTipo', 'Sem tipos de normalização');
             }
         } else {
-            mostrarSemDados(
-                'containerPizzaNormalizacao',
-                'Erro ao carregar dados',
-            );
-            mostrarSemDados(
-                'containerNormalizacaoTipo',
-                'Erro ao carregar dados',
-            );
+            mostrarSemDados('containerPizzaNormalizacao', 'Erro ao carregar dados');
+            mostrarSemDados('containerNormalizacaoTipo', 'Erro ao carregar dados');
         }
     } catch (e) {
         console.error('Erro carregarEstatisticasNormalizacao:', e);
@@ -255,9 +212,7 @@
 function renderizarPizzaNormalizacao(resumo) {
     try {
         restaurarCanvas('containerPizzaNormalizacao', 'chartNormalizacaoPizza');
-        const ctx = document
-            .getElementById('chartNormalizacaoPizza')
-            .getContext('2d');
+        const ctx = document.getElementById('chartNormalizacaoPizza').getContext('2d');
 
         if (chartNormalizacaoPizza) {
             chartNormalizacaoPizza.destroy();
@@ -267,17 +222,12 @@
             type: 'doughnut',
             data: {
                 labels: ['Originais', 'Normalizadas'],
-                datasets: [
-                    {
-                        data: [
-                            resumo.viagensOriginais,
-                            resumo.viagensNormalizadas,
-                        ],
-                        backgroundColor: [cores[0], cores[3]],
-                        borderColor: [coresBorda[0], coresBorda[3]],
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    data: [resumo.viagensOriginais, resumo.viagensNormalizadas],
+                    backgroundColor: [cores[0], cores[3]],
+                    borderColor: [coresBorda[0], coresBorda[3]],
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -287,21 +237,14 @@
                     tooltip: {
                         callbacks: {
                             label: function (context) {
-                                const total =
-                                    resumo.viagensOriginais +
-                                    resumo.viagensNormalizadas;
-                                const percentual =
-                                    total > 0
-                                        ? ((context.raw / total) * 100).toFixed(
-                                              1,
-                                          )
-                                        : 0;
+                                const total = resumo.viagensOriginais + resumo.viagensNormalizadas;
+                                const percentual = total > 0 ? ((context.raw / total) * 100).toFixed(1) : 0;
                                 return `${context.label}: ${formatarNumero(context.raw)} (${percentual}%)`;
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarPizzaNormalizacao:', e);
@@ -311,22 +254,20 @@
 function renderizarBarrasTipoNormalizacao(dados) {
     try {
         restaurarCanvas('containerNormalizacaoTipo', 'chartNormalizacaoTipo');
-        const ctx = document
-            .getElementById('chartNormalizacaoTipo')
-            .getContext('2d');
+        const ctx = document.getElementById('chartNormalizacaoTipo').getContext('2d');
 
         if (chartNormalizacaoTipo) {
             chartNormalizacaoTipo.destroy();
         }
 
-        const labels = dados.map((d) => {
+        const labels = dados.map(d => {
             const texto = formatarTipoNormalizacao(d.tipo);
 
             if (texto.length > 18) {
                 const palavras = texto.split(' ');
                 const linhas = [];
                 let linhaAtual = '';
-                palavras.forEach((palavra) => {
+                palavras.forEach(palavra => {
                     if ((linhaAtual + ' ' + palavra).trim().length <= 18) {
                         linhaAtual = (linhaAtual + ' ' + palavra).trim();
                     } else {
@@ -339,21 +280,19 @@
             }
             return texto;
         });
-        const valores = dados.map((d) => d.quantidade);
+        const valores = dados.map(d => d.quantidade);
 
         chartNormalizacaoTipo = new Chart(ctx, {
             type: 'bar',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        label: 'Quantidade',
-                        data: valores,
-                        backgroundColor: cores[1],
-                        borderColor: coresBorda[1],
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    label: 'Quantidade',
+                    data: valores,
+                    backgroundColor: cores[1],
+                    borderColor: coresBorda[1],
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -361,8 +300,8 @@
                 indexAxis: 'y',
                 layout: {
                     padding: {
-                        left: 10,
-                    },
+                        left: 10
+                    }
                 },
                 plugins: {
                     legend: { display: false },
@@ -370,27 +309,25 @@
                         callbacks: {
                             title: function (context) {
 
-                                return formatarTipoNormalizacao(
-                                    dados[context[0].dataIndex].tipo,
-                                );
-                            },
-                        },
-                    },
+                                return formatarTipoNormalizacao(dados[context[0].dataIndex].tipo);
+                            }
+                        }
+                    }
                 },
                 scales: {
                     x: {
                         beginAtZero: true,
-                        ticks: { stepSize: 1 },
+                        ticks: { stepSize: 1 }
                     },
                     y: {
                         ticks: {
                             font: { size: 10 },
                             autoSkip: false,
-                            maxRotation: 0,
-                        },
-                    },
-                },
-            },
+                            maxRotation: 0
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarBarrasTipoNormalizacao:', e);
@@ -398,21 +335,25 @@
 }
 
 function formatarTipoNormalizacao(tipo) {
-    if (!tipo) return 'Não especificado';
-
-    return tipo
-        .replace(/_/g, ' ')
-        .replace(/([A-Z])/g, ' $1')
-        .trim()
-        .toLowerCase()
-        .replace(/\b\w/g, (l) => l.toUpperCase());
+    try {
+        if (!tipo) return 'Não especificado';
+
+        return tipo
+            .replace(/_/g, ' ')
+            .replace(/([A-Z])/g, ' $1')
+            .trim()
+            .toLowerCase()
+            .replace(/\b\w/g, l => l.toUpperCase());
+    } catch (e) {
+        console.error('Erro formatarTipoNormalizacao:', e);
+        Alerta.TratamentoErroComLinha('administracao.js', 'formatarTipoNormalizacao', e);
+        return 'Não especificado';
+    }
 }
 
 async function carregarDistribuicaoTipoUso() {
     try {
-        const response = await fetch(
-            '/api/Administracao/ObterDistribuicaoTipoUso',
-        );
+        const response = await fetch('/api/Administracao/ObterDistribuicaoTipoUso');
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
@@ -436,29 +377,27 @@
             chartTipoUso.destroy();
         }
 
-        const labels = dados.map((d) => d.tipoUso || 'Não especificado');
-        const valores = dados.map((d) => d.quantidade);
+        const labels = dados.map(d => d.tipoUso || 'Não especificado');
+        const valores = dados.map(d => d.quantidade);
 
         chartTipoUso = new Chart(ctx, {
             type: 'pie',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        data: valores,
-                        backgroundColor: cores.slice(0, dados.length),
-                        borderColor: coresBorda.slice(0, dados.length),
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    data: valores,
+                    backgroundColor: cores.slice(0, dados.length),
+                    borderColor: coresBorda.slice(0, dados.length),
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
                 maintainAspectRatio: false,
                 plugins: {
-                    legend: { position: 'bottom' },
-                },
-            },
+                    legend: { position: 'bottom' }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarTipoUso:', e);
@@ -467,9 +406,7 @@
 
 async function carregarHeatmap() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterHeatmapViagens?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterHeatmapViagens?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados) {
@@ -486,20 +423,12 @@
 function renderizarHeatmap(dados) {
     try {
         const tbody = document.getElementById('heatmapBody');
-        const diasSemana = [
-            'Segunda',
-            'Terça',
-            'Quarta',
-            'Quinta',
-            'Sexta',
-            'Sábado',
-            'Domingo',
-        ];
+        const diasSemana = ['Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'];
 
         let maxValor = 0;
         if (dados.matriz) {
-            dados.matriz.forEach((row) => {
-                row.forEach((val) => {
+            dados.matriz.forEach(row => {
+                row.forEach(val => {
                     if (val > maxValor) maxValor = val;
                 });
             });
@@ -509,10 +438,7 @@
         for (let dia = 0; dia < 7; dia++) {
             html += `<tr><td class="text-center small fw-bold">${diasSemana[dia]}</td>`;
             for (let hora = 0; hora < 24; hora++) {
-                const valor =
-                    dados.matriz && dados.matriz[dia]
-                        ? dados.matriz[dia][hora] || 0
-                        : 0;
+                const valor = dados.matriz && dados.matriz[dia] ? (dados.matriz[dia][hora] || 0) : 0;
                 const cor = obterCorHeatmap(valor, maxValor);
                 html += `<td class="heatmap-cell" style="background-color: ${cor};" title="${diasSemana[dia]} ${hora}h: ${valor} viagens">${valor > 0 ? valor : ''}</td>`;
             }
@@ -529,15 +455,7 @@
 function renderizarHeatmapVazio() {
     try {
         const tbody = document.getElementById('heatmapBody');
-        const diasSemana = [
-            'Segunda',
-            'Terça',
-            'Quarta',
-            'Quinta',
-            'Sexta',
-            'Sábado',
-            'Domingo',
-        ];
+        const diasSemana = ['Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'];
 
         let html = '';
         for (let dia = 0; dia < 7; dia++) {
@@ -555,29 +473,30 @@
 }
 
 function obterCorHeatmap(valor, maxValor) {
-    if (valor === 0 || maxValor === 0) return '#f5f5f5';
-    const percentual = valor / maxValor;
-    if (percentual <= 0.2) return '#e8f5e9';
-    if (percentual <= 0.4) return '#c8e6c9';
-    if (percentual <= 0.6) return '#81c784';
-    if (percentual <= 0.8) return '#4caf50';
-    return '#2e7d32';
+    try {
+        if (valor === 0 || maxValor === 0) return '#f5f5f5';
+        const percentual = valor / maxValor;
+        if (percentual <= 0.2) return '#e8f5e9';
+        if (percentual <= 0.4) return '#c8e6c9';
+        if (percentual <= 0.6) return '#81c784';
+        if (percentual <= 0.8) return '#4caf50';
+        return '#2e7d32';
+    } catch (e) {
+        console.error('Erro obterCorHeatmap:', e);
+        Alerta.TratamentoErroComLinha('administracao.js', 'obterCorHeatmap', e);
+        return '#f5f5f5';
+    }
 }
 
 async function carregarTop10Veiculos() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterTop10VeiculosPorKm?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterTop10VeiculosPorKm?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
             renderizarTop10Veiculos(result.dados);
         } else {
-            mostrarSemDados(
-                'containerTop10Veiculos',
-                'Sem dados de veículos no período',
-            );
+            mostrarSemDados('containerTop10Veiculos', 'Sem dados de veículos no período');
         }
     } catch (e) {
         console.error('Erro carregarTop10Veiculos:', e);
@@ -588,32 +507,26 @@
 function renderizarTop10Veiculos(dados) {
     try {
         restaurarCanvas('containerTop10Veiculos', 'chartTop10Veiculos');
-        const ctx = document
-            .getElementById('chartTop10Veiculos')
-            .getContext('2d');
+        const ctx = document.getElementById('chartTop10Veiculos').getContext('2d');
 
         if (chartTop10Veiculos) {
             chartTop10Veiculos.destroy();
         }
 
-        const labels = dados.map((d) =>
-            truncarTexto(d.placa || d.veiculoDescricao, 15),
-        );
-        const valores = dados.map((d) => d.totalKm);
+        const labels = dados.map(d => truncarTexto(d.placa || d.veiculoDescricao, 15));
+        const valores = dados.map(d => d.totalKm);
 
         chartTop10Veiculos = new Chart(ctx, {
             type: 'bar',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        label: 'KM Rodados',
-                        data: valores,
-                        backgroundColor: cores[0],
-                        borderColor: coresBorda[0],
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    label: 'KM Rodados',
+                    data: valores,
+                    backgroundColor: cores[0],
+                    borderColor: coresBorda[0],
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -631,11 +544,11 @@
                                 const item = dados[context.dataIndex];
                                 return [
                                     `Total KM: ${formatarKm(item.totalKm)}`,
-                                    `Viagens: ${item.totalViagens}`,
+                                    `Viagens: ${item.totalViagens}`
                                 ];
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
                     x: {
@@ -643,11 +556,11 @@
                         ticks: {
                             callback: function (value) {
                                 return formatarKm(value);
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarTop10Veiculos:', e);
@@ -656,18 +569,13 @@
 
 async function carregarTop10Motoristas() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterTop10MotoristasPorKm?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterTop10MotoristasPorKm?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
             renderizarTop10Motoristas(result.dados);
         } else {
-            mostrarSemDados(
-                'containerTop10Motoristas',
-                'Sem dados de motoristas no período',
-            );
+            mostrarSemDados('containerTop10Motoristas', 'Sem dados de motoristas no período');
         }
     } catch (e) {
         console.error('Erro carregarTop10Motoristas:', e);
@@ -678,30 +586,26 @@
 function renderizarTop10Motoristas(dados) {
     try {
         restaurarCanvas('containerTop10Motoristas', 'chartTop10Motoristas');
-        const ctx = document
-            .getElementById('chartTop10Motoristas')
-            .getContext('2d');
+        const ctx = document.getElementById('chartTop10Motoristas').getContext('2d');
 
         if (chartTop10Motoristas) {
             chartTop10Motoristas.destroy();
         }
 
-        const labels = dados.map((d) => truncarTexto(d.nome, 15));
-        const valores = dados.map((d) => d.totalKm);
+        const labels = dados.map(d => truncarTexto(d.nome, 15));
+        const valores = dados.map(d => d.totalKm);
 
         chartTop10Motoristas = new Chart(ctx, {
             type: 'bar',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        label: 'KM Rodados',
-                        data: valores,
-                        backgroundColor: cores[1],
-                        borderColor: coresBorda[1],
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    label: 'KM Rodados',
+                    data: valores,
+                    backgroundColor: cores[1],
+                    borderColor: coresBorda[1],
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -718,11 +622,11 @@
                                 const item = dados[context.dataIndex];
                                 return [
                                     `Total KM: ${formatarKm(item.totalKm)}`,
-                                    `Viagens: ${item.totalViagens}`,
+                                    `Viagens: ${item.totalViagens}`
                                 ];
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
                     x: {
@@ -730,11 +634,11 @@
                         ticks: {
                             callback: function (value) {
                                 return formatarKm(value);
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarTop10Motoristas:', e);
@@ -743,18 +647,13 @@
 
 async function carregarCustoPorFinalidade() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterCustoPorFinalidade?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterCustoPorFinalidade?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
             renderizarCustoPorFinalidade(result.dados);
         } else {
-            mostrarSemDados(
-                'containerCustoPorFinalidade',
-                'Sem dados de custos por finalidade',
-            );
+            mostrarSemDados('containerCustoPorFinalidade', 'Sem dados de custos por finalidade');
         }
     } catch (e) {
         console.error('Erro carregarCustoPorFinalidade:', e);
@@ -764,34 +663,27 @@
 
 function renderizarCustoPorFinalidade(dados) {
     try {
-        restaurarCanvas(
-            'containerCustoPorFinalidade',
-            'chartCustoPorFinalidade',
-        );
-        const ctx = document
-            .getElementById('chartCustoPorFinalidade')
-            .getContext('2d');
+        restaurarCanvas('containerCustoPorFinalidade', 'chartCustoPorFinalidade');
+        const ctx = document.getElementById('chartCustoPorFinalidade').getContext('2d');
 
         if (chartCustoPorFinalidade) {
             chartCustoPorFinalidade.destroy();
         }
 
-        const labels = dados.map((d) => truncarTexto(d.finalidade, 15));
-        const valores = dados.map((d) => d.custoMedio);
+        const labels = dados.map(d => truncarTexto(d.finalidade, 15));
+        const valores = dados.map(d => d.custoMedio);
 
         chartCustoPorFinalidade = new Chart(ctx, {
             type: 'bar',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        label: 'Custo Médio (R$)',
-                        data: valores,
-                        backgroundColor: cores[2],
-                        borderColor: coresBorda[2],
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    label: 'Custo Médio (R$)',
+                    data: valores,
+                    backgroundColor: cores[2],
+                    borderColor: coresBorda[2],
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -808,11 +700,11 @@
                                 return [
                                     `Custo Médio: ${formatarMoeda(item.custoMedio)}`,
                                     `Custo Total: ${formatarMoeda(item.custoTotal)}`,
-                                    `Viagens: ${item.totalViagens}`,
+                                    `Viagens: ${item.totalViagens}`
                                 ];
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
                     y: {
@@ -820,11 +712,11 @@
                         ticks: {
                             callback: function (value) {
                                 return formatarMoeda(value);
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarCustoPorFinalidade:', e);
@@ -833,29 +725,18 @@
 
 async function carregarComparativoPropiosTerceirizados() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterComparativoPropiosTerceirizados?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterComparativoPropiosTerceirizados?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados) {
             const dados = result.dados;
-            if (
-                dados.proprios.totalViagens > 0 ||
-                dados.terceirizados.totalViagens > 0
-            ) {
+            if (dados.proprios.totalViagens > 0 || dados.terceirizados.totalViagens > 0) {
                 renderizarPropriosTerceirizados(dados);
             } else {
-                mostrarSemDados(
-                    'containerPropriosTerceirizados',
-                    'Sem dados de veículos próprios/terceirizados',
-                );
+                mostrarSemDados('containerPropriosTerceirizados', 'Sem dados de veículos próprios/terceirizados');
             }
         } else {
-            mostrarSemDados(
-                'containerPropriosTerceirizados',
-                'Sem dados disponíveis',
-            );
+            mostrarSemDados('containerPropriosTerceirizados', 'Sem dados disponíveis');
         }
     } catch (e) {
         console.error('Erro carregarComparativoPropiosTerceirizados:', e);
@@ -865,13 +746,8 @@
 
 function renderizarPropriosTerceirizados(dados) {
     try {
-        restaurarCanvas(
-            'containerPropriosTerceirizados',
-            'chartPropriosTerceirizados',
-        );
-        const ctx = document
-            .getElementById('chartPropriosTerceirizados')
-            .getContext('2d');
+        restaurarCanvas('containerPropriosTerceirizados', 'chartPropriosTerceirizados');
+        const ctx = document.getElementById('chartPropriosTerceirizados').getContext('2d');
 
         if (chartPropriosTerceirizados) {
             chartPropriosTerceirizados.destroy();
@@ -884,27 +760,19 @@
                 datasets: [
                     {
                         label: 'Próprios',
-                        data: [
-                            dados.proprios.totalViagens,
-                            dados.proprios.totalKm / 1000,
-                            dados.proprios.custoTotal / 1000,
-                        ],
+                        data: [dados.proprios.totalViagens, dados.proprios.totalKm / 1000, dados.proprios.custoTotal / 1000],
                         backgroundColor: cores[0],
                         borderColor: coresBorda[0],
-                        borderWidth: 1,
+                        borderWidth: 1
                     },
                     {
                         label: 'Terceirizados',
-                        data: [
-                            dados.terceirizados.totalViagens,
-                            dados.terceirizados.totalKm / 1000,
-                            dados.terceirizados.custoTotal / 1000,
-                        ],
+                        data: [dados.terceirizados.totalViagens, dados.terceirizados.totalKm / 1000, dados.terceirizados.custoTotal / 1000],
                         backgroundColor: cores[3],
                         borderColor: coresBorda[3],
-                        borderWidth: 1,
-                    },
-                ],
+                        borderWidth: 1
+                    }
+                ]
             },
             options: {
                 responsive: true,
@@ -915,24 +783,19 @@
                         callbacks: {
                             label: function (context) {
                                 const label = context.dataset.label;
-                                const tipo =
-                                    label === 'Próprios'
-                                        ? dados.proprios
-                                        : dados.terceirizados;
+                                const tipo = label === 'Próprios' ? dados.proprios : dados.terceirizados;
                                 const idx = context.dataIndex;
-                                if (idx === 0)
-                                    return `${label}: ${formatarNumero(tipo.totalViagens)} viagens`;
-                                if (idx === 1)
-                                    return `${label}: ${formatarKm(tipo.totalKm)}`;
+                                if (idx === 0) return `${label}: ${formatarNumero(tipo.totalViagens)} viagens`;
+                                if (idx === 1) return `${label}: ${formatarKm(tipo.totalKm)}`;
                                 return `${label}: ${formatarMoeda(tipo.custoTotal)}`;
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
-                    y: { beginAtZero: true },
-                },
-            },
+                    y: { beginAtZero: true }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarPropriosTerceirizados:', e);
@@ -941,18 +804,13 @@
 
 async function carregarEficienciaFrota() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterEficienciaFrota?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterEficienciaFrota?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
             renderizarEficiencia(result.dados);
         } else {
-            mostrarSemDados(
-                'containerEficiencia',
-                'Sem dados de eficiência no período',
-            );
+            mostrarSemDados('containerEficiencia', 'Sem dados de eficiência no período');
         }
     } catch (e) {
         console.error('Erro carregarEficienciaFrota:', e);
@@ -969,8 +827,8 @@
             chartEficiencia.destroy();
         }
 
-        const labels = dados.map((d) => truncarTexto(d.placa, 12));
-        const valores = dados.map((d) => d.custoPorKm);
+        const labels = dados.map(d => truncarTexto(d.placa, 12));
+        const valores = dados.map(d => d.custoPorKm);
 
         const coresEficiencia = dados.map((_, i) => {
             if (i < 3) return 'rgba(76, 175, 80, 0.8)';
@@ -982,17 +840,13 @@
             type: 'bar',
             data: {
                 labels: labels,
-                datasets: [
-                    {
-                        label: 'Custo por KM (R$)',
-                        data: valores,
-                        backgroundColor: coresEficiencia,
-                        borderColor: coresEficiencia.map((c) =>
-                            c.replace('0.8', '1'),
-                        ),
-                        borderWidth: 1,
-                    },
-                ],
+                datasets: [{
+                    label: 'Custo por KM (R$)',
+                    data: valores,
+                    backgroundColor: coresEficiencia,
+                    borderColor: coresEficiencia.map(c => c.replace('0.8', '1')),
+                    borderWidth: 1
+                }]
             },
             options: {
                 responsive: true,
@@ -1003,8 +857,7 @@
                     tooltip: {
                         callbacks: {
                             title: function (context) {
-                                return dados[context[0].dataIndex]
-                                    .veiculoDescricao;
+                                return dados[context[0].dataIndex].veiculoDescricao;
                             },
                             label: function (context) {
                                 const item = dados[context.dataIndex];
@@ -1012,11 +865,11 @@
                                     `Custo/KM: ${formatarMoeda(item.custoPorKm)}`,
                                     `Total KM: ${formatarNumero(item.totalKm)}`,
                                     `Custo Total: ${formatarMoeda(item.custoTotal)}`,
-                                    `Viagens: ${item.totalViagens}`,
+                                    `Viagens: ${item.totalViagens}`
                                 ];
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
                     x: {
@@ -1024,11 +877,11 @@
                         ticks: {
                             callback: function (value) {
                                 return 'R$ ' + value.toFixed(2);
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarEficiencia:', e);
@@ -1037,18 +890,13 @@
 
 async function carregarEvolucaoMensalCustos() {
     try {
-        const response = await fetch(
-            `/api/Administracao/ObterEvolucaoMensalCustos?${obterParametrosFiltro()}`,
-        );
+        const response = await fetch(`/api/Administracao/ObterEvolucaoMensalCustos?${obterParametrosFiltro()}`);
         const result = await response.json();
 
         if (result.sucesso && result.dados && result.dados.length > 0) {
             renderizarEvolucaoMensal(result.dados);
         } else {
-            mostrarSemDados(
-                'containerEvolucaoMensal',
-                'Sem dados de evolução no período',
-            );
+            mostrarSemDados('containerEvolucaoMensal', 'Sem dados de evolução no período');
         }
     } catch (e) {
         console.error('Erro carregarEvolucaoMensalCustos:', e);
@@ -1059,15 +907,13 @@
 function renderizarEvolucaoMensal(dados) {
     try {
         restaurarCanvas('containerEvolucaoMensal', 'chartEvolucaoMensal');
-        const ctx = document
-            .getElementById('chartEvolucaoMensal')
-            .getContext('2d');
+        const ctx = document.getElementById('chartEvolucaoMensal').getContext('2d');
 
         if (chartEvolucaoMensal) {
             chartEvolucaoMensal.destroy();
         }
 
-        const labels = dados.map((d) => d.mesAno);
+        const labels = dados.map(d => d.mesAno);
 
         chartEvolucaoMensal = new Chart(ctx, {
             type: 'line',
@@ -1076,29 +922,29 @@
                 datasets: [
                     {
                         label: 'Combustível',
-                        data: dados.map((d) => d.custoCombustivel),
+                        data: dados.map(d => d.custoCombustivel),
                         borderColor: cores[0],
                         backgroundColor: cores[0].replace('0.8', '0.1'),
                         fill: true,
-                        tension: 0.3,
+                        tension: 0.3
                     },
                     {
                         label: 'Motorista',
-                        data: dados.map((d) => d.custoMotorista),
+                        data: dados.map(d => d.custoMotorista),
                         borderColor: cores[1],
                         backgroundColor: cores[1].replace('0.8', '0.1'),
                         fill: true,
-                        tension: 0.3,
+                        tension: 0.3
                     },
                     {
                         label: 'Lavador',
-                        data: dados.map((d) => d.custoLavador),
+                        data: dados.map(d => d.custoLavador),
                         borderColor: cores[2],
                         backgroundColor: cores[2].replace('0.8', '0.1'),
                         fill: true,
-                        tension: 0.3,
-                    },
-                ],
+                        tension: 0.3
+                    }
+                ]
             },
             options: {
                 responsive: true,
@@ -1113,11 +959,11 @@
                                     '',
                                     `Total: ${formatarMoeda(item.custoTotal)}`,
                                     `Viagens: ${item.totalViagens}`,
-                                    `KM: ${formatarKm(item.totalKm)}`,
+                                    `KM: ${formatarKm(item.totalKm)}`
                                 ];
-                            },
-                        },
-                    },
+                            }
+                        }
+                    }
                 },
                 scales: {
                     y: {
@@ -1125,11 +971,11 @@
                         ticks: {
                             callback: function (value) {
                                 return formatarMoeda(value);
-                            },
-                        },
-                    },
-                },
-            },
+                            }
+                        }
+                    }
+                }
+            }
         });
     } catch (e) {
         console.error('Erro renderizarEvolucaoMensal:', e);
@@ -1148,10 +994,7 @@
 function formatarMoeda(valor) {
     try {
         if (valor === null || valor === undefined) return 'R$ 0,00';
-        return valor.toLocaleString('pt-BR', {
-            style: 'currency',
-            currency: 'BRL',
-        });
+        return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
     } catch (e) {
         return 'R$ 0,00';
     }
```
