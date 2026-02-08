# wwwroot/js/dashboards/dashboard-abastecimento.js

**Mudanca:** GRANDE | **+805** linhas | **-1909** linhas

---

```diff
--- JANEIRO: wwwroot/js/dashboards/dashboard-abastecimento.js
+++ ATUAL: wwwroot/js/dashboards/dashboard-abastecimento.js
@@ -4,60 +4,57 @@
         if (typeof ej !== 'undefined' && ej.base) {
 
             const numberingSystemsData = {
-                supplemental: {
-                    numberingSystems: {
-                        latn: { _digits: '0123456789', _type: 'numeric' },
-                    },
-                },
+                "supplemental": {
+                    "numberingSystems": {
+                        "latn": { "_digits": "0123456789", "_type": "numeric" }
+                    }
+                }
             };
 
             const numbersData = {
-                main: {
-                    'pt-BR': {
-                        numbers: {
-                            defaultNumberingSystem: 'latn',
-                            minimumGroupingDigits: '1',
-                            'symbols-numberSystem-latn': {
-                                decimal: ',',
-                                group: '.',
-                                list: ';',
-                                percentSign: '%',
-                                plusSign: '+',
-                                minusSign: '-',
-                                exponential: 'E',
-                                superscriptingExponent: '×',
-                                perMille: '‰',
-                                infinity: '∞',
-                                nan: 'NaN',
-                                timeSeparator: ':',
+                "main": {
+                    "pt-BR": {
+                        "numbers": {
+                            "defaultNumberingSystem": "latn",
+                            "minimumGroupingDigits": "1",
+                            "symbols-numberSystem-latn": {
+                                "decimal": ",",
+                                "group": ".",
+                                "list": ";",
+                                "percentSign": "%",
+                                "plusSign": "+",
+                                "minusSign": "-",
+                                "exponential": "E",
+                                "superscriptingExponent": "×",
+                                "perMille": "‰",
+                                "infinity": "∞",
+                                "nan": "NaN",
+                                "timeSeparator": ":"
                             },
-                            'decimalFormats-numberSystem-latn': {
-                                standard: '#,##0.###',
+                            "decimalFormats-numberSystem-latn": {
+                                "standard": "#,##0.###"
                             },
-                            'percentFormats-numberSystem-latn': {
-                                standard: '#,##0%',
+                            "percentFormats-numberSystem-latn": {
+                                "standard": "#,##0%"
                             },
-                            'currencyFormats-numberSystem-latn': {
-                                standard: '¤ #,##0.00',
-                            },
-                        },
-                    },
-                },
+                            "currencyFormats-numberSystem-latn": {
+                                "standard": "¤ #,##0.00"
+                            }
+                        }
+                    }
+                }
             };
 
             const currenciesData = {
-                main: {
-                    'pt-BR': {
-                        numbers: {
-                            currencies: {
-                                BRL: {
-                                    displayName: 'Real brasileiro',
-                                    symbol: 'R$',
-                                },
-                            },
-                        },
-                    },
-                },
+                "main": {
+                    "pt-BR": {
+                        "numbers": {
+                            "currencies": {
+                                "BRL": { "displayName": "Real brasileiro", "symbol": "R$" }
+                            }
+                        }
+                    }
+                }
             };
 
             ej.base.loadCldr(numberingSystemsData, numbersData, currenciesData);
@@ -91,63 +88,15 @@
 let chartValorMensalVeiculo = null;
 let chartRankingVeiculos = null;
 
-const MESES = [
-    '',
-    'jan',
-    'fev',
-    'mar',
-    'abr',
-    'mai',
-    'jun',
-    'jul',
-    'ago',
-    'set',
-    'out',
-    'nov',
-    'dez',
-];
-const MESES_COMPLETOS = [
-    '',
-    'janeiro',
-    'fevereiro',
-    'março',
-    'abril',
-    'maio',
-    'junho',
-    'julho',
-    'agosto',
-    'setembro',
-    'outubro',
-    'novembro',
-    'dezembro',
-];
+const MESES = ['', 'jan', 'fev', 'mar', 'abr', 'mai', 'jun', 'jul', 'ago', 'set', 'out', 'nov', 'dez'];
+const MESES_COMPLETOS = ['', 'janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro'];
 
 const CORES = {
     amber: ['#a8784c', '#8b5e3c', '#6d472c', '#5a3a24', '#4a2f1d'],
     gold: ['#d4a574', '#c4956a', '#a8784c', '#9a7045', '#8b6340'],
     warm: ['#c49a6c', '#b8916a', '#a88565', '#9a785a', '#8c6c50'],
-    multi: [
-        '#a8784c',
-        '#c4956a',
-        '#d4a574',
-        '#b8916a',
-        '#8b5e3c',
-        '#9a7045',
-        '#6d472c',
-        '#8b6340',
-    ],
-    categorias: [
-        '#a8784c',
-        '#c49a6c',
-        '#9a7045',
-        '#8b5e3c',
-        '#c4956a',
-        '#5a3a24',
-        '#6d472c',
-        '#8b6340',
-        '#d4a574',
-        '#4a2f1d',
-    ],
+    multi: ['#a8784c', '#c4956a', '#d4a574', '#b8916a', '#8b5e3c', '#9a7045', '#6d472c', '#8b6340'],
+    categorias: ['#a8784c', '#c49a6c', '#9a7045', '#8b5e3c', '#c4956a', '#5a3a24', '#6d472c', '#8b6340', '#d4a574', '#4a2f1d']
 };
 
 function mostrarLoading() {
@@ -166,9 +115,7 @@
 
 function exibirToastAmareloAposLoading(mensagens, duracaoMs = 4000) {
     try {
-        const lista = Array.isArray(mensagens)
-            ? mensagens.filter(Boolean)
-            : [mensagens].filter(Boolean);
+        const lista = Array.isArray(mensagens) ? mensagens.filter(Boolean) : [mensagens].filter(Boolean);
         if (lista.length === 0) return;
 
         setTimeout(() => {
@@ -179,11 +126,7 @@
             });
         }, 600);
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'exibirToastAmareloAposLoading',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "exibirToastAmareloAposLoading", error);
     }
 }
 
@@ -204,11 +147,7 @@
 
         inicializarFiltrosECarregar();
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'DOMContentLoaded',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "DOMContentLoaded", error);
     }
 });
 
@@ -230,53 +169,38 @@
                         return;
                     }
 
-                    const selectGeral =
-                        document.getElementById('filtroAnoGeral');
-                    const selectMensal =
-                        document.getElementById('filtroAnoMensal');
-                    const selectVeiculo =
-                        document.getElementById('filtroAnoVeiculo');
-
-                    [selectGeral, selectMensal, selectVeiculo].forEach(
-                        (select) => {
-                            if (!select) return;
-                            const isGeral = select.id === 'filtroAnoGeral';
-                            select.innerHTML = isGeral
-                                ? '<option value="">&lt;Todos os Anos&gt;</option>'
-                                : '<option value="">Selecione o Ano</option>';
-                            anos.forEach((ano) => {
-                                const option = document.createElement('option');
-                                option.value = ano;
-                                option.textContent = ano;
-                                select.appendChild(option);
+                    const selectGeral = document.getElementById('filtroAnoGeral');
+                    const selectMensal = document.getElementById('filtroAnoMensal');
+                    const selectVeiculo = document.getElementById('filtroAnoVeiculo');
+
+                    [selectGeral, selectMensal, selectVeiculo].forEach(select => {
+                        if (!select) return;
+                        const isGeral = select.id === 'filtroAnoGeral';
+                        select.innerHTML = isGeral ? '<option value="">&lt;Todos os Anos&gt;</option>' : '<option value="">Selecione o Ano</option>';
+                        anos.forEach(ano => {
+                            const option = document.createElement('option');
+                            option.value = ano;
+                            option.textContent = ano;
+                            select.appendChild(option);
+                        });
+                        select.dataset.initialized = 'true';
+
+                        if (!select.dataset.eventAdded) {
+                            select.addEventListener('change', function () {
+                                const anoSelecionado = this.value;
+                                const mesSelectId = this.id.replace('Ano', 'Mes');
+                                const mesSelect = document.getElementById(mesSelectId);
+
+                                if (anoSelecionado && mesSelect) {
+                                    popularMesesDoAno(anoSelecionado, mesSelect);
+                                } else if (mesSelect) {
+
+                                    mesSelect.innerHTML = '<option value="">&lt;Todos os Meses&gt;</option>';
+                                }
                             });
-                            select.dataset.initialized = 'true';
-
-                            if (!select.dataset.eventAdded) {
-                                select.addEventListener('change', function () {
-                                    const anoSelecionado = this.value;
-                                    const mesSelectId = this.id.replace(
-                                        'Ano',
-                                        'Mes',
-                                    );
-                                    const mesSelect =
-                                        document.getElementById(mesSelectId);
-
-                                    if (anoSelecionado && mesSelect) {
-                                        popularMesesDoAno(
-                                            anoSelecionado,
-                                            mesSelect,
-                                        );
-                                    } else if (mesSelect) {
-
-                                        mesSelect.innerHTML =
-                                            '<option value="">&lt;Todos os Meses&gt;</option>';
-                                    }
-                                });
-                                select.dataset.eventAdded = 'true';
-                            }
-                        },
-                    );
+                            select.dataset.eventAdded = 'true';
+                        }
+                    });
 
                     const filtroAplicado = data.filtroAplicado || {};
                     if (filtroAplicado.ano > 0) {
@@ -285,22 +209,16 @@
                             selectGeral.value = filtroAplicado.ano.toString();
                         }
 
-                        const mesSelectGeral =
-                            document.getElementById('filtroMesGeral');
+                        const mesSelectGeral = document.getElementById('filtroMesGeral');
                         if (mesSelectGeral) {
-                            popularMesesDoAno(
-                                filtroAplicado.ano,
-                                mesSelectGeral,
-                                function () {
-
-                                    if (filtroAplicado.mes > 0) {
-                                        mesSelectGeral.value =
-                                            filtroAplicado.mes.toString();
-                                    }
-
-                                    carregarDadosGeraisComFiltros();
-                                },
-                            );
+                            popularMesesDoAno(filtroAplicado.ano, mesSelectGeral, function () {
+
+                                if (filtroAplicado.mes > 0) {
+                                    mesSelectGeral.value = filtroAplicado.mes.toString();
+                                }
+
+                                carregarDadosGeraisComFiltros();
+                            });
                         } else {
 
                             carregarDadosGeraisComFiltros();
@@ -309,26 +227,19 @@
 
                         carregarDadosGeraisComFiltros();
                     }
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'inicializarFiltrosECarregar.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "inicializarFiltrosECarregar.success", error);
                     esconderLoading();
                 }
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao inicializar filtros:', error);
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'inicializarFiltrosECarregar',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "inicializarFiltrosECarregar", error);
         esconderLoading();
     }
 }
@@ -341,25 +252,11 @@
             data: { ano: ano },
             success: function (data) {
                 const meses = data.meses || [];
-                const nomesMeses = [
-                    '',
-                    'Janeiro',
-                    'Fevereiro',
-                    'Março',
-                    'Abril',
-                    'Maio',
-                    'Junho',
-                    'Julho',
-                    'Agosto',
-                    'Setembro',
-                    'Outubro',
-                    'Novembro',
-                    'Dezembro',
-                ];
-
-                mesSelect.innerHTML =
-                    '<option value="">&lt;Todos os Meses&gt;</option>';
-                meses.forEach((mes) => {
+                const nomesMeses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
+
+                mesSelect.innerHTML = '<option value="">&lt;Todos os Meses&gt;</option>';
+                meses.forEach(mes => {
                     const option = document.createElement('option');
                     option.value = mes;
                     option.textContent = nomesMeses[mes];
@@ -372,16 +269,11 @@
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao buscar meses disponíveis:', error);
-                mesSelect.innerHTML =
-                    '<option value="">&lt;Todos os Meses&gt;</option>';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'popularMesesDoAno',
-            error,
-        );
+                mesSelect.innerHTML = '<option value="">&lt;Todos os Meses&gt;</option>';
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "popularMesesDoAno", error);
     }
 }
 
@@ -401,31 +293,19 @@
                     dadosGerais = data;
                     renderizarAbaGeral(data);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'carregarDadosGeraisComFiltros.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGeraisComFiltros.success", error);
                 } finally {
                     esconderLoading();
                 }
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao carregar dados gerais:', error);
-                AppToast.show(
-                    'red',
-                    'Erro ao carregar dados do dashboard',
-                    5000,
-                );
+                AppToast.show('red', 'Erro ao carregar dados do dashboard', 5000);
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosGeraisComFiltros',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGeraisComFiltros", error);
         esconderLoading();
     }
 }
@@ -433,12 +313,10 @@
 function inicializarTabs() {
     try {
         const tabs = document.querySelectorAll('.dash-tab');
-        tabs.forEach((tab) => {
+        tabs.forEach(tab => {
             tab.addEventListener('click', function () {
                 const tabId = this.getAttribute('data-tab');
-                const tabAtual = document
-                    .querySelector('.dash-tab.active')
-                    ?.getAttribute('data-tab');
+                const tabAtual = document.querySelector('.dash-tab.active')?.getAttribute('data-tab');
 
                 if (tabId === tabAtual) return;
 
@@ -446,12 +324,10 @@
                 if (tabAtual === 'consumo-mensal') destruirGraficosMensal();
                 if (tabAtual === 'consumo-veiculo') destruirGraficosVeiculo();
 
-                tabs.forEach((t) => t.classList.remove('active'));
+                tabs.forEach(t => t.classList.remove('active'));
                 this.classList.add('active');
 
-                document
-                    .querySelectorAll('.dash-content')
-                    .forEach((c) => c.classList.remove('active'));
+                document.querySelectorAll('.dash-content').forEach(c => c.classList.remove('active'));
 
                 const tabContent = document.getElementById('tab-' + tabId);
                 if (tabContent) tabContent.classList.add('active');
@@ -468,260 +344,171 @@
             });
         });
 
-        document
-            .getElementById('btnFiltrarAnoMesGeral')
-            ?.addEventListener('click', function () {
-
-                document.getElementById('dataInicioGeral').value = '';
-                document.getElementById('dataFimGeral').value = '';
-                document
-                    .querySelectorAll('.btn-period-abast')
-                    .forEach((b) => b.classList.remove('active'));
-                carregarDadosGerais();
-            });
-
-        document
-            .getElementById('btnLimparAnoMesGeral')
-            ?.addEventListener('click', function () {
+        document.getElementById('btnFiltrarAnoMesGeral')?.addEventListener('click', function () {
+
+            document.getElementById('dataInicioGeral').value = '';
+            document.getElementById('dataFimGeral').value = '';
+            document.querySelectorAll('.btn-period-abast').forEach(b => b.classList.remove('active'));
+            carregarDadosGerais();
+        });
+
+        document.getElementById('btnLimparAnoMesGeral')?.addEventListener('click', function () {
+            document.getElementById('filtroAnoGeral').value = '';
+            document.getElementById('filtroMesGeral').value = '';
+            atualizarLabelPeriodoGeral();
+            carregarDadosGerais();
+        });
+
+        document.getElementById('btnFiltrarPeriodoGeral')?.addEventListener('click', function () {
+            const dataInicio = document.getElementById('dataInicioGeral').value;
+            const dataFim = document.getElementById('dataFimGeral').value;
+            if (dataInicio && dataFim) {
+
                 document.getElementById('filtroAnoGeral').value = '';
                 document.getElementById('filtroMesGeral').value = '';
-                atualizarLabelPeriodoGeral();
-                carregarDadosGerais();
-            });
-
-        document
-            .getElementById('btnFiltrarPeriodoGeral')
-            ?.addEventListener('click', function () {
-                const dataInicio =
-                    document.getElementById('dataInicioGeral').value;
-                const dataFim = document.getElementById('dataFimGeral').value;
-                if (dataInicio && dataFim) {
-
-                    document.getElementById('filtroAnoGeral').value = '';
-                    document.getElementById('filtroMesGeral').value = '';
-                    document
-                        .querySelectorAll('.btn-period-abast')
-                        .forEach((b) => b.classList.remove('active'));
-                    carregarDadosGeraisPeriodo(dataInicio, dataFim);
-                } else {
-                    Alerta.Warning('Preencha as datas de início e fim');
-                }
-            });
-
-        document
-            .getElementById('btnLimparPeriodoGeral')
-            ?.addEventListener('click', function () {
-                document.getElementById('dataInicioGeral').value = '';
-                document.getElementById('dataFimGeral').value = '';
-                document
-                    .querySelectorAll('.btn-period-abast')
-                    .forEach((b) => b.classList.remove('active'));
-                atualizarLabelPeriodoGeral();
-                carregarDadosGerais();
-            });
-
-        document.querySelectorAll('.btn-period-abast').forEach((btn) => {
+                document.querySelectorAll('.btn-period-abast').forEach(b => b.classList.remove('active'));
+                carregarDadosGeraisPeriodo(dataInicio, dataFim);
+            } else {
+                Alerta.Warning('Preencha as datas de início e fim');
+            }
+        });
+
+        document.getElementById('btnLimparPeriodoGeral')?.addEventListener('click', function () {
+            document.getElementById('dataInicioGeral').value = '';
+            document.getElementById('dataFimGeral').value = '';
+            document.querySelectorAll('.btn-period-abast').forEach(b => b.classList.remove('active'));
+            atualizarLabelPeriodoGeral();
+            carregarDadosGerais();
+        });
+
+        document.querySelectorAll('.btn-period-abast').forEach(btn => {
             btn.addEventListener('click', function () {
                 const dias = parseInt(this.dataset.dias);
                 const dataFim = new Date();
                 const dataInicio = new Date();
                 dataInicio.setDate(dataInicio.getDate() - dias);
 
-                document.getElementById('dataInicioGeral').value = dataInicio
-                    .toISOString()
-                    .split('T')[0];
-                document.getElementById('dataFimGeral').value = dataFim
-                    .toISOString()
-                    .split('T')[0];
+                document.getElementById('dataInicioGeral').value = dataInicio.toISOString().split('T')[0];
+                document.getElementById('dataFimGeral').value = dataFim.toISOString().split('T')[0];
                 document.getElementById('filtroAnoGeral').value = '';
                 document.getElementById('filtroMesGeral').value = '';
 
-                document
-                    .querySelectorAll('.btn-period-abast')
-                    .forEach((b) => b.classList.remove('active'));
+                document.querySelectorAll('.btn-period-abast').forEach(b => b.classList.remove('active'));
                 this.classList.add('active');
 
                 carregarDadosGeraisPeriodo(
                     document.getElementById('dataInicioGeral').value,
-                    document.getElementById('dataFimGeral').value,
+                    document.getElementById('dataFimGeral').value
                 );
             });
         });
 
-        document
-            .getElementById('btnFiltrarMensal')
-            ?.addEventListener('click', function () {
-                carregarDadosMensais();
-            });
-
-        document
-            .getElementById('btnLimparMensal')
-            ?.addEventListener('click', function () {
-                const selectAno = document.getElementById('filtroAnoMensal');
+        document.getElementById('btnFiltrarMensal')?.addEventListener('click', function () {
+            carregarDadosMensais();
+        });
+
+        document.getElementById('btnLimparMensal')?.addEventListener('click', function () {
+            const selectAno = document.getElementById('filtroAnoMensal');
+            if (selectAno && selectAno.options.length > 0) {
+                selectAno.selectedIndex = 0;
+            }
+            document.getElementById('filtroMesMensal').value = '';
+            carregarDadosMensais();
+        });
+
+        document.getElementById('btnLimparAnoMesVeiculo')?.addEventListener('click', function () {
+            try {
+                const selectAno = document.getElementById('filtroAnoVeiculo');
                 if (selectAno && selectAno.options.length > 0) {
                     selectAno.selectedIndex = 0;
                 }
-                document.getElementById('filtroMesMensal').value = '';
-                carregarDadosMensais();
-            });
-
-        document
-            .getElementById('btnLimparAnoMesVeiculo')
-            ?.addEventListener('click', function () {
-                try {
-                    const selectAno =
-                        document.getElementById('filtroAnoVeiculo');
-                    if (selectAno && selectAno.options.length > 0) {
-                        selectAno.selectedIndex = 0;
-                    }
-                    document.getElementById('filtroMesVeiculo').value = '';
-                    prepararAtualizacaoVeiculoSemPlaca();
-                    carregarDadosVeiculo(false);
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'btnLimparAnoMesVeiculo.click',
-                        error,
-                    );
-                }
-            });
-
-        document
-            .getElementById('filtroAnoVeiculo')
-            ?.addEventListener('change', function () {
-                try {
-                    document.getElementById('dataInicioVeiculo').value = '';
-                    document.getElementById('dataFimVeiculo').value = '';
-                    document.getElementById('filtroMesVeiculo').value = '';
-
-                    const $selectPlaca = $('#filtroPlacaVeiculo');
-                    let placaSelecionadaAntes = '';
-                    let textoPlacaAntes = '';
-
-                    if (
-                        $selectPlaca.length &&
-                        $selectPlaca.hasClass('select2-hidden-accessible')
-                    ) {
-                        placaSelecionadaAntes = $selectPlaca.val() || '';
-                        const select2Data = $selectPlaca.select2('data');
-                        textoPlacaAntes =
-                            select2Data && select2Data.length > 0
-                                ? select2Data[0].text
-                                : '';
-                    } else {
-                        const selectPlaca =
-                            document.getElementById('filtroPlacaVeiculo');
-                        placaSelecionadaAntes = selectPlaca?.value || '';
-                        textoPlacaAntes =
-                            selectPlaca?.options[selectPlaca.selectedIndex]
-                                ?.text || '';
-                    }
-
-                    prepararAtualizacaoVeiculoSemPlaca();
-
-                    if (this.value) {
-                        carregarDadosVeiculo(
-                            false,
-                            placaSelecionadaAntes,
-                            textoPlacaAntes,
-                        );
-                    } else {
-                        carregarDadosVeiculo(
-                            false,
-                            placaSelecionadaAntes,
-                            textoPlacaAntes,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'filtroAnoVeiculo.change',
-                        error,
-                    );
-                }
-            });
-
-        document
-            .getElementById('filtroMesVeiculo')
-            ?.addEventListener('change', function () {
-                try {
-                    document.getElementById('dataInicioVeiculo').value = '';
-                    document.getElementById('dataFimVeiculo').value = '';
-
-                    const $selectPlaca = $('#filtroPlacaVeiculo');
-                    let placaSelecionadaAntes = '';
-                    let textoPlacaAntes = '';
-
-                    if (
-                        $selectPlaca.length &&
-                        $selectPlaca.hasClass('select2-hidden-accessible')
-                    ) {
-
-                        placaSelecionadaAntes = $selectPlaca.val() || '';
-                        const select2Data = $selectPlaca.select2('data');
-                        textoPlacaAntes =
-                            select2Data && select2Data.length > 0
-                                ? select2Data[0].text
-                                : '';
-                    } else {
-
-                        const selectPlaca =
-                            document.getElementById('filtroPlacaVeiculo');
-                        placaSelecionadaAntes = selectPlaca?.value || '';
-                        textoPlacaAntes =
-                            selectPlaca?.options[selectPlaca.selectedIndex]
-                                ?.text || '';
-                    }
-
-                    prepararAtualizacaoVeiculoSemPlaca();
-                    if (this.value) {
-                        carregarDadosVeiculo(
-                            true,
-                            placaSelecionadaAntes,
-                            textoPlacaAntes,
-                        );
-                    } else {
-                        carregarDadosVeiculo(
-                            false,
-                            placaSelecionadaAntes,
-                            textoPlacaAntes,
-                        );
-                    }
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'filtroMesVeiculo.change',
-                        error,
-                    );
-                }
-            });
-
-        document
-            .getElementById('btnLimparPeriodoVeiculo')
-            ?.addEventListener('click', function () {
-                try {
-                    document.getElementById('dataInicioVeiculo').value = '';
-                    document.getElementById('dataFimVeiculo').value = '';
-                    carregarDadosVeiculo();
-                } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'btnLimparPeriodoVeiculo.click',
-                        error,
-                    );
-                }
-            });
-
-        [
-            document.getElementById('dataInicioVeiculo'),
-            document.getElementById('dataFimVeiculo'),
-        ].forEach((input) => {
+                document.getElementById('filtroMesVeiculo').value = '';
+                prepararAtualizacaoVeiculoSemPlaca();
+                carregarDadosVeiculo(false);
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "btnLimparAnoMesVeiculo.click", error);
+            }
+        });
+
+        document.getElementById('filtroAnoVeiculo')?.addEventListener('change', function () {
+            try {
+                document.getElementById('dataInicioVeiculo').value = '';
+                document.getElementById('dataFimVeiculo').value = '';
+                document.getElementById('filtroMesVeiculo').value = '';
+
+                const $selectPlaca = $('#filtroPlacaVeiculo');
+                let placaSelecionadaAntes = '';
+                let textoPlacaAntes = '';
+
+                if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
+                    placaSelecionadaAntes = $selectPlaca.val() || '';
+                    const select2Data = $selectPlaca.select2('data');
+                    textoPlacaAntes = (select2Data && select2Data.length > 0) ? select2Data[0].text : '';
+                } else {
+                    const selectPlaca = document.getElementById('filtroPlacaVeiculo');
+                    placaSelecionadaAntes = selectPlaca?.value || '';
+                    textoPlacaAntes = selectPlaca?.options[selectPlaca.selectedIndex]?.text || '';
+                }
+
+                prepararAtualizacaoVeiculoSemPlaca();
+
+                if (this.value) {
+                    carregarDadosVeiculo(false, placaSelecionadaAntes, textoPlacaAntes);
+                } else {
+                    carregarDadosVeiculo(false, placaSelecionadaAntes, textoPlacaAntes);
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "filtroAnoVeiculo.change", error);
+            }
+        });
+
+        document.getElementById('filtroMesVeiculo')?.addEventListener('change', function () {
+            try {
+                document.getElementById('dataInicioVeiculo').value = '';
+                document.getElementById('dataFimVeiculo').value = '';
+
+                const $selectPlaca = $('#filtroPlacaVeiculo');
+                let placaSelecionadaAntes = '';
+                let textoPlacaAntes = '';
+
+                if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
+
+                    placaSelecionadaAntes = $selectPlaca.val() || '';
+                    const select2Data = $selectPlaca.select2('data');
+                    textoPlacaAntes = (select2Data && select2Data.length > 0) ? select2Data[0].text : '';
+                } else {
+
+                    const selectPlaca = document.getElementById('filtroPlacaVeiculo');
+                    placaSelecionadaAntes = selectPlaca?.value || '';
+                    textoPlacaAntes = selectPlaca?.options[selectPlaca.selectedIndex]?.text || '';
+                }
+
+                prepararAtualizacaoVeiculoSemPlaca();
+                if (this.value) {
+                    carregarDadosVeiculo(true, placaSelecionadaAntes, textoPlacaAntes);
+                } else {
+                    carregarDadosVeiculo(false, placaSelecionadaAntes, textoPlacaAntes);
+                }
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "filtroMesVeiculo.change", error);
+            }
+        });
+
+        document.getElementById('btnLimparPeriodoVeiculo')?.addEventListener('click', function () {
+            try {
+                document.getElementById('dataInicioVeiculo').value = '';
+                document.getElementById('dataFimVeiculo').value = '';
+                carregarDadosVeiculo();
+            } catch (error) {
+                Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "btnLimparPeriodoVeiculo.click", error);
+            }
+        });
+
+        [document.getElementById('dataInicioVeiculo'), document.getElementById('dataFimVeiculo')].forEach(input => {
             input?.addEventListener('change', function () {
                 try {
-                    const dataInicio =
-                        document.getElementById('dataInicioVeiculo').value;
-                    const dataFim =
-                        document.getElementById('dataFimVeiculo').value;
+                    const dataInicio = document.getElementById('dataInicioVeiculo').value;
+                    const dataFim = document.getElementById('dataFimVeiculo').value;
                     if (dataInicio && dataFim && dataInicio > dataFim) {
                         document.getElementById('dataFimVeiculo').value = '';
                         prepararAtualizacaoVeiculoSemPlaca();
@@ -742,34 +529,21 @@
                         atualizarLabelPeriodoVeiculo();
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'dataInicioFimVeiculo.change',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "dataInicioFimVeiculo.change", error);
                 }
             });
         });
 
-        if (
-            $('#filtroPlacaVeiculo').length &&
-            typeof $.fn.select2 === 'function'
-        ) {
+        if ($('#filtroPlacaVeiculo').length && typeof $.fn.select2 === 'function') {
             $('#filtroPlacaVeiculo').select2({
                 placeholder: 'Digite para buscar...',
                 allowClear: true,
                 width: '260px',
                 language: {
-                    noResults: function () {
-                        return 'Nenhuma placa encontrada';
-                    },
-                    searching: function () {
-                        return 'Buscando...';
-                    },
-                    inputTooShort: function () {
-                        return 'Digite para buscar';
-                    },
-                },
+                    noResults: function () { return 'Nenhuma placa encontrada'; },
+                    searching: function () { return 'Buscando...'; },
+                    inputTooShort: function () { return 'Digite para buscar'; }
+                }
             });
         }
 
@@ -780,28 +554,18 @@
                 }
                 aplicarFiltroPlacaVeiculo();
             } catch (error) {
-                Alerta.TratamentoErroComLinha(
-                    'dashboard-abastecimento.js',
-                    'filtroPlacaVeiculo.change',
-                    error,
-                );
+                Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "filtroPlacaVeiculo.change", error);
             }
         });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'inicializarTabs',
-            error,
-        );
+
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "inicializarTabs", error);
     }
 }
 
 function limparSelectPlacaVeiculo() {
     const $selectPlaca = $('#filtroPlacaVeiculo');
-    if (
-        $selectPlaca.length &&
-        $selectPlaca.hasClass('select2-hidden-accessible')
-    ) {
+    if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
         $selectPlaca.val('').trigger('change');
     } else {
         document.getElementById('filtroPlacaVeiculo').value = '';
@@ -814,10 +578,7 @@
         const $selectPlaca = $('#filtroPlacaVeiculo');
 
         ignorarMudancaPlacaVeiculo = true;
-        if (
-            $selectPlaca.length &&
-            $selectPlaca.hasClass('select2-hidden-accessible')
-        ) {
+        if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
             $selectPlaca.select2('destroy');
         }
 
@@ -829,27 +590,17 @@
                 allowClear: true,
                 width: '260px',
                 language: {
-                    noResults: function () {
-                        return 'Nenhuma placa encontrada';
-                    },
-                    searching: function () {
-                        return 'Buscando...';
-                    },
-                    inputTooShort: function () {
-                        return 'Digite para buscar';
-                    },
-                },
+                    noResults: function () { return 'Nenhuma placa encontrada'; },
+                    searching: function () { return 'Buscando...'; },
+                    inputTooShort: function () { return 'Digite para buscar'; }
+                }
             });
             $selectPlaca.trigger('change.select2');
         }
         ignorarMudancaPlacaVeiculo = false;
     } catch (error) {
         ignorarMudancaPlacaVeiculo = false;
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'resetarOpcoesPlacaVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "resetarOpcoesPlacaVeiculo", error);
     }
 }
 
@@ -858,11 +609,7 @@
         renderizarAbaVeiculo({}, null, null);
         renderizarHeatmapVeiculo(null, null, null);
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'limparResumoVeiculoSemSelecao',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "limparResumoVeiculoSemSelecao", error);
     }
 }
 
@@ -876,11 +623,7 @@
         atualizarLabelPeriodoVeiculo();
     } catch (error) {
         ignorarMudancaPlacaVeiculo = false;
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'prepararAtualizacaoVeiculoSemPlaca',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "prepararAtualizacaoVeiculoSemPlaca", error);
     }
 }
 
@@ -903,11 +646,7 @@
             carregarDadosVeiculo();
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'aplicarFiltroPlacaVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "aplicarFiltroPlacaVeiculo", error);
     }
 }
 
@@ -946,35 +685,18 @@
                 try {
                     dadosGerais = data;
 
-                    if (
-                        !data ||
-                        !data.totais ||
-                        data.totais.valorTotal === 0 ||
-                        data.totais.valorTotal === null
-                    ) {
+                    if (!data || !data.totais || data.totais.valorTotal === 0 || data.totais.valorTotal === null) {
                         const di = new Date(dataInicio + 'T00:00:00');
                         const df = new Date(dataFim + 'T00:00:00');
-                        const periodo =
-                            di.toLocaleDateString('pt-BR') +
-                            ' a ' +
-                            df.toLocaleDateString('pt-BR');
-                        AppToast.show(
-                            'orange',
-                            'Não há registros de abastecimento para o período ' +
-                                periodo,
-                            5000,
-                        );
+                        const periodo = di.toLocaleDateString('pt-BR') + ' a ' + df.toLocaleDateString('pt-BR');
+                        AppToast.show('orange', 'Não há registros de abastecimento para o período ' + periodo, 5000);
                     }
 
                     renderizarAbaGeral(data);
                     esconderLoading();
                 } catch (error) {
                     esconderLoading();
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'carregarDadosGeraisPeriodo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGeraisPeriodo.success", error);
                 }
             },
             error: function (xhr, status, error) {
@@ -982,15 +704,11 @@
                 console.error('Erro ao carregar dados por período:', error);
 
                 carregarDadosGerais();
-            },
+            }
         });
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosGeraisPeriodo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGeraisPeriodo", error);
     }
 }
 
@@ -1007,8 +725,7 @@
     let placaTexto = '';
     if (placaSelect?.value) {
 
-        const textoCompleto =
-            placaSelect.options[placaSelect.selectedIndex]?.text || '';
+        const textoCompleto = placaSelect.options[placaSelect.selectedIndex]?.text || '';
         placaTexto = obterPlacaTextoCompleto(textoCompleto);
     }
 
@@ -1017,9 +734,7 @@
     if (dataInicio && dataFim) {
         const di = new Date(dataInicio + 'T00:00:00');
         const df = new Date(dataFim + 'T00:00:00');
-        partes.push(
-            `${di.toLocaleDateString('pt-BR')} a ${df.toLocaleDateString('pt-BR')}`,
-        );
+        partes.push(`${di.toLocaleDateString('pt-BR')} a ${df.toLocaleDateString('pt-BR')}`);
     } else if (ano && mes) {
         partes.push(`${MESES_COMPLETOS[parseInt(mes)]} de ${ano}`);
     } else if (ano) {
@@ -1052,21 +767,11 @@
                     dadosVeiculo = data;
                     mensagensToast = [];
 
-                    if (
-                        !data ||
-                        data.valorTotal === 0 ||
-                        data.valorTotal === null
-                    ) {
+                    if (!data || data.valorTotal === 0 || data.valorTotal === null) {
                         const di = new Date(dataInicio + 'T00:00:00');
                         const df = new Date(dataFim + 'T00:00:00');
-                        const periodo =
-                            di.toLocaleDateString('pt-BR') +
-                            ' a ' +
-                            df.toLocaleDateString('pt-BR');
-                        mensagensToast.push(
-                            'Não há registros de abastecimento para o período ' +
-                                periodo,
-                        );
+                        const periodo = di.toLocaleDateString('pt-BR') + ' a ' + df.toLocaleDateString('pt-BR');
+                        mensagensToast.push('Não há registros de abastecimento para o período ' + periodo);
                     }
 
                     const resultadoPlaca = preencherFiltrosVeiculo(data);
@@ -1075,30 +780,18 @@
 
                     if (resultadoPlaca?.placaRemovida) {
                         const placaMensagem = resultadoPlaca.placaTexto
-                            ? 'Nenhum Abastecimento no Período para o Veículo ' +
-                              resultadoPlaca.placaTexto
+                            ? 'Nenhum Abastecimento no Período para o Veículo ' + resultadoPlaca.placaTexto
                             : 'Nenhum Abastecimento no Período para o Veículo';
                         mensagensToast.push(placaMensagem);
                     }
 
-                    const placaSelect =
-                        document.getElementById('filtroPlacaVeiculo');
+                    const placaSelect = document.getElementById('filtroPlacaVeiculo');
                     const veiculoIdAtual = placaSelect?.value || '';
-                    const placaTextoCompleto =
-                        placaSelect?.options[placaSelect.selectedIndex]?.text;
-                    const placaSelecionada = placaTextoCompleto
-                        ? obterPlacaTextoCompleto(placaTextoCompleto)
-                        : null;
-                    const placaValida =
-                        veiculoIdAtual &&
-                        placaSelecionada &&
-                        placaSelecionada !== 'Todas';
-
-                    renderizarAbaVeiculo(
-                        data,
-                        placaValida ? veiculoIdAtual : null,
-                        placaValida ? placaSelecionada : null,
-                    );
+                    const placaTextoCompleto = placaSelect?.options[placaSelect.selectedIndex]?.text;
+                    const placaSelecionada = placaTextoCompleto ? obterPlacaTextoCompleto(placaTextoCompleto) : null;
+                    const placaValida = veiculoIdAtual && placaSelecionada && placaSelecionada !== 'Todas';
+
+                    renderizarAbaVeiculo(data, placaValida ? veiculoIdAtual : null, placaValida ? placaSelecionada : null);
 
                     if (placaValida) {
                         renderizarHeatmapVeiculo(null, placaSelecionada, null);
@@ -1109,30 +802,19 @@
                     exibirToastAmareloAposLoading(mensagensToast);
                 } catch (error) {
                     esconderLoading();
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'carregarDadosVeiculoPeriodo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosVeiculoPeriodo.success", error);
                 }
             },
             error: function (xhr, status, error) {
                 esconderLoading();
-                console.error(
-                    'Erro ao carregar dados de veículo por período:',
-                    error,
-                );
+                console.error('Erro ao carregar dados de veículo por período:', error);
 
                 carregarDadosVeiculo();
-            },
+            }
         });
     } catch (error) {
         esconderLoading();
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosVeiculoPeriodo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosVeiculoPeriodo", error);
     }
 }
 
@@ -1153,31 +835,19 @@
 
                     renderizarAbaGeral(data);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'carregarDadosGerais.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGerais.success", error);
                 } finally {
                     esconderLoading();
                 }
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao carregar dados gerais:', error);
-                AppToast.show(
-                    'red',
-                    'Erro ao carregar dados do dashboard',
-                    5000,
-                );
+                AppToast.show('red', 'Erro ao carregar dados do dashboard', 5000);
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosGerais',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosGerais", error);
         esconderLoading();
     }
 }
@@ -1202,22 +872,14 @@
         }
 
         if (!ano) {
-            AppToast.show(
-                'orange',
-                'Selecione um ano para visualizar os dados',
-                3000,
-            );
+            AppToast.show('orange', 'Selecione um ano para visualizar os dados', 3000);
             esconderLoading();
             return;
         }
 
         executarCarregamentoMensal(ano, mes);
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosMensais',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosMensais", error);
         esconderLoading();
     }
 }
@@ -1232,43 +894,16 @@
                 try {
                     dadosMensais = data;
 
-                    if (
-                        !data ||
-                        data.valorTotal === 0 ||
-                        data.valorTotal === null
-                    ) {
-                        const nomesMeses = [
-                            '',
-                            'Janeiro',
-                            'Fevereiro',
-                            'Março',
-                            'Abril',
-                            'Maio',
-                            'Junho',
-                            'Julho',
-                            'Agosto',
-                            'Setembro',
-                            'Outubro',
-                            'Novembro',
-                            'Dezembro',
-                        ];
-                        const periodo = mes
-                            ? nomesMeses[parseInt(mes)] + '/' + ano
-                            : ano;
-                        AppToast.show(
-                            'orange',
-                            'Não há registros de abastecimento para ' + periodo,
-                            5000,
-                        );
+                    if (!data || data.valorTotal === 0 || data.valorTotal === null) {
+                        const nomesMeses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                            'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
+                        const periodo = mes ? nomesMeses[parseInt(mes)] + '/' + ano : ano;
+                        AppToast.show('orange', 'Não há registros de abastecimento para ' + periodo, 5000);
                     }
 
                     renderizarAbaMensal(data);
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'executarCarregamentoMensal.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "executarCarregamentoMensal.success", error);
                 } finally {
                     esconderLoading();
                 }
@@ -1277,14 +912,10 @@
                 console.error('Erro ao carregar dados mensais:', error);
                 AppToast.show('red', 'Erro ao carregar dados mensais', 5000);
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'executarCarregamentoMensal',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "executarCarregamentoMensal", error);
         esconderLoading();
     }
 }
@@ -1297,25 +928,11 @@
             data: { ano: ano },
             success: function (data) {
                 const meses = data.meses || [];
-                const nomesMeses = [
-                    '',
-                    'Janeiro',
-                    'Fevereiro',
-                    'Março',
-                    'Abril',
-                    'Maio',
-                    'Junho',
-                    'Julho',
-                    'Agosto',
-                    'Setembro',
-                    'Outubro',
-                    'Novembro',
-                    'Dezembro',
-                ];
-
-                mesSelect.innerHTML =
-                    '<option value="">&lt;Todos os Meses&gt;</option>';
-                meses.forEach((mes) => {
+                const nomesMeses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                    'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
+
+                mesSelect.innerHTML = '<option value="">&lt;Todos os Meses&gt;</option>';
+                meses.forEach(mes => {
                     const option = document.createElement('option');
                     option.value = mes;
                     option.textContent = nomesMeses[mes];
@@ -1337,26 +954,17 @@
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao buscar meses disponíveis:', error);
-                mesSelect.innerHTML =
-                    '<option value="">&lt;Todos os Meses&gt;</option>';
+                mesSelect.innerHTML = '<option value="">&lt;Todos os Meses&gt;</option>';
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'popularMesesDoAnoECarregar',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "popularMesesDoAnoECarregar", error);
         esconderLoading();
     }
 }
 
-function carregarDadosVeiculo(
-    autoSelecionarAno = true,
-    placaAnterior = null,
-    textoPlacaAnterior = null,
-) {
+function carregarDadosVeiculo(autoSelecionarAno = true, placaAnterior = null, textoPlacaAnterior = null) {
     try {
         mostrarLoading();
         atualizarLabelPeriodoVeiculo();
@@ -1365,12 +973,7 @@
         let ano = selectAno?.value || '';
         let mes = selectMes?.value || '';
 
-        if (
-            !ano &&
-            autoSelecionarAno &&
-            selectAno &&
-            selectAno.options.length > 1
-        ) {
+        if (!ano && autoSelecionarAno && selectAno && selectAno.options.length > 1) {
 
             ano = selectAno.options[1].value;
             selectAno.value = ano;
@@ -1387,28 +990,14 @@
             return;
         }
 
-        executarCarregamentoVeiculo(
-            ano,
-            mes,
-            placaAnterior,
-            textoPlacaAnterior,
-        );
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'carregarDadosVeiculo',
-            error,
-        );
+        executarCarregamentoVeiculo(ano, mes, placaAnterior, textoPlacaAnterior);
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "carregarDadosVeiculo", error);
         esconderLoading();
     }
 }
 
-function executarCarregamentoVeiculo(
-    ano,
-    mes,
-    placaAnterior = null,
-    textoPlacaAnterior = null,
-) {
+function executarCarregamentoVeiculo(ano, mes, placaAnterior = null, textoPlacaAnterior = null) {
     try {
         const placaSelect = document.getElementById('filtroPlacaVeiculo');
         const veiculoId = placaSelect?.value || '';
@@ -1420,7 +1009,7 @@
                 ano: ano,
                 mes: mes || null,
                 veiculoId: veiculoId || null,
-                tipoVeiculo: null,
+                tipoVeiculo: null
             },
             success: function (data) {
 
@@ -1428,105 +1017,48 @@
                 try {
                     dadosVeiculo = data;
 
-                    if (
-                        !data ||
-                        data.valorTotal === 0 ||
-                        data.valorTotal === null
-                    ) {
-                        const nomesMeses = [
-                            '',
-                            'Janeiro',
-                            'Fevereiro',
-                            'Março',
-                            'Abril',
-                            'Maio',
-                            'Junho',
-                            'Julho',
-                            'Agosto',
-                            'Setembro',
-                            'Outubro',
-                            'Novembro',
-                            'Dezembro',
-                        ];
-                        const periodo = mes
-                            ? nomesMeses[parseInt(mes)] + '/' + ano
-                            : ano;
-                        mensagensToast.push(
-                            'Não há registros de abastecimento para ' + periodo,
-                        );
+                    if (!data || data.valorTotal === 0 || data.valorTotal === null) {
+                        const nomesMeses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
+                            'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
+                        const periodo = mes ? nomesMeses[parseInt(mes)] + '/' + ano : ano;
+                        mensagensToast.push('Não há registros de abastecimento para ' + periodo);
                     }
 
                     const resultadoPlaca = preencherFiltrosVeiculo(data);
 
                     atualizarLabelPeriodoVeiculo();
 
-                    if (
-                        placaAnterior &&
-                        textoPlacaAnterior &&
-                        textoPlacaAnterior !== 'Todas'
-                    ) {
+                    if (placaAnterior && textoPlacaAnterior && textoPlacaAnterior !== 'Todas') {
                         const placasDisponiveis = data?.placasDisponiveis || [];
-                        const placaAindaDisponivel = placasDisponiveis.some(
-                            (p) => p.veiculoId === placaAnterior,
-                        );
+                        const placaAindaDisponivel = placasDisponiveis.some(p => p.veiculoId === placaAnterior);
                         if (!placaAindaDisponivel) {
-                            const placaTexto =
-                                obterPlacaTextoCompleto(textoPlacaAnterior);
-                            mensagensToast.push(
-                                'Nenhum Abastecimento no Período para o Veículo' +
-                                    (placaTexto ? ' ' + placaTexto : ''),
-                            );
+                            const placaTexto = obterPlacaTextoCompleto(textoPlacaAnterior);
+                            mensagensToast.push('Nenhum Abastecimento no Período para o Veículo' + (placaTexto ? ' ' + placaTexto : ''));
                         }
                     }
 
                     else if (resultadoPlaca?.placaRemovida) {
                         const placaMensagem = resultadoPlaca.placaTexto
-                            ? 'Nenhum Abastecimento no Período para o Veículo ' +
-                              resultadoPlaca.placaTexto
+                            ? 'Nenhum Abastecimento no Período para o Veículo ' + resultadoPlaca.placaTexto
                             : 'Nenhum Abastecimento no Período para o Veículo';
                         mensagensToast.push(placaMensagem);
                     }
 
-                    const placaSelectAtual =
-                        document.getElementById('filtroPlacaVeiculo');
+                    const placaSelectAtual = document.getElementById('filtroPlacaVeiculo');
                     const veiculoIdAtual = placaSelectAtual?.value || '';
-                    const placaTextoCompleto =
-                        placaSelectAtual?.options[
-                            placaSelectAtual.selectedIndex
-                        ]?.text;
-                    const placaSelecionada = placaTextoCompleto
-                        ? obterPlacaTextoCompleto(placaTextoCompleto)
-                        : null;
-                    const placaValida =
-                        veiculoIdAtual &&
-                        placaSelecionada &&
-                        placaSelecionada !== 'Todas';
-
-                    renderizarAbaVeiculo(
-                        data,
-                        placaValida ? veiculoIdAtual : null,
-                        placaValida ? placaSelecionada : null,
-                    );
-
-                    if (
-                        veiculoIdAtual &&
-                        placaSelecionada &&
-                        placaSelecionada !== 'Todas'
-                    ) {
-                        renderizarHeatmapVeiculo(
-                            ano || null,
-                            placaSelecionada,
-                            null,
-                        );
+                    const placaTextoCompleto = placaSelectAtual?.options[placaSelectAtual.selectedIndex]?.text;
+                    const placaSelecionada = placaTextoCompleto ? obterPlacaTextoCompleto(placaTextoCompleto) : null;
+                    const placaValida = veiculoIdAtual && placaSelecionada && placaSelecionada !== 'Todas';
+
+                    renderizarAbaVeiculo(data, placaValida ? veiculoIdAtual : null, placaValida ? placaSelecionada : null);
+
+                    if (veiculoIdAtual && placaSelecionada && placaSelecionada !== 'Todas') {
+                        renderizarHeatmapVeiculo(ano || null, placaSelecionada, null);
                     } else {
                         renderizarHeatmapVeiculo(null, null, null);
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'executarCarregamentoVeiculo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "executarCarregamentoVeiculo.success", error);
                 } finally {
                     esconderLoading();
                     exibirToastAmareloAposLoading(mensagensToast);
@@ -1534,46 +1066,31 @@
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao carregar dados por veículo:', error);
-                AppToast.show(
-                    'red',
-                    'Erro ao carregar dados por veículo',
-                    5000,
-                );
+                AppToast.show('red', 'Erro ao carregar dados por veículo', 5000);
                 esconderLoading();
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'executarCarregamentoVeiculo',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "executarCarregamentoVeiculo", error);
         esconderLoading();
     }
 }
 
 function renderizarAbaGeral(data) {
     try {
-        document.getElementById('valorTotalGeral').textContent = formatarMoeda(
-            data.totais.valorTotal,
-        );
-        document.getElementById('litrosTotalGeral').textContent =
-            formatarLitros(data.totais.litrosTotal);
-        document.getElementById('qtdAbastecimentosGeral').textContent =
-            data.totais.qtdAbastecimentos.toLocaleString('pt-BR');
-
-        const mediaDiesel = data.mediaLitro.find((m) =>
-            m.combustivel.toLowerCase().includes('diesel'),
-        );
-        const mediaGasolina = data.mediaLitro.find((m) =>
-            m.combustivel.toLowerCase().includes('gasolina'),
-        );
+        document.getElementById('valorTotalGeral').textContent = formatarMoeda(data.totais.valorTotal);
+        document.getElementById('litrosTotalGeral').textContent = formatarLitros(data.totais.litrosTotal);
+        document.getElementById('qtdAbastecimentosGeral').textContent = data.totais.qtdAbastecimentos.toLocaleString('pt-BR');
+
+        const mediaDiesel = data.mediaLitro.find(m => m.combustivel.toLowerCase().includes('diesel'));
+        const mediaGasolina = data.mediaLitro.find(m => m.combustivel.toLowerCase().includes('gasolina'));
 
         document.getElementById('mediaDieselGeral').textContent = mediaDiesel
             ? formatarMoeda(mediaDiesel.media)
             : 'R$ 0';
-        document.getElementById('mediaGasolinaGeral').textContent =
-            mediaGasolina ? formatarMoeda(mediaGasolina.media) : 'R$ 0';
+        document.getElementById('mediaGasolinaGeral').textContent = mediaGasolina
+            ? formatarMoeda(mediaGasolina.media)
+            : 'R$ 0';
 
         renderizarTabelaResumoPorAno(data.resumoPorAno);
         renderizarChartValorCategoria(data.valorPorCategoria);
@@ -1585,11 +1102,7 @@
         const mesGeral = document.getElementById('filtroMesGeral')?.value || '';
         renderizarHeatmapDiaHora(anoGeral, mesGeral);
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarAbaGeral',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarAbaGeral", error);
     }
 }
 
@@ -1599,15 +1112,14 @@
         if (!container) return;
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
+            container.innerHTML = '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
             return;
         }
 
         let html = '';
         let totalValor = 0;
 
-        dados.forEach((item) => {
+        dados.forEach(item => {
             totalValor += item.valor;
             html += `
                 <div class="grid-row">
@@ -1626,11 +1138,7 @@
 
         container.innerHTML = html;
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarTabelaResumoPorAno',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarTabelaResumoPorAno", error);
     }
 }
 
@@ -1639,71 +1147,52 @@
         const container = document.getElementById('chartValorCategoria');
         if (!container) return;
 
-        if (chartValorCategoria) {
-            chartValorCategoria.destroy();
-            chartValorCategoria = null;
-        }
+        if (chartValorCategoria) { chartValorCategoria.destroy(); chartValorCategoria = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
         const dataSource = dados.map((item, idx) => ({
             x: item.categoria,
             y: item.valor,
-            color: CORES.multi[idx % CORES.multi.length],
+            color: CORES.multi[idx % CORES.multi.length]
         }));
 
         chartValorCategoria = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelStyle: { size: '9px' } },
             primaryYAxis: {
                 labelFormat: 'R$ {value}',
                 labelStyle: { size: '9px' },
                 labelIntersectAction: 'None',
-                edgeLabelPlacement: 'Shift',
-            },
-            series: [
-                {
-                    dataSource: dataSource,
-                    xName: 'x',
-                    yName: 'y',
-                    pointColorMapping: 'color',
-                    type: 'Bar',
-                    cornerRadius: { topRight: 4, bottomRight: 4 },
-                },
-            ],
+                edgeLabelPlacement: 'Shift'
+            },
+            series: [{
+                dataSource: dataSource,
+                xName: 'x',
+                yName: 'y',
+                pointColorMapping: 'color',
+                type: 'Bar',
+                cornerRadius: { topRight: 4, bottomRight: 4 }
+            }],
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
+                args.text = args.point.x + ': ' + formatarLabelMoeda(args.point.y);
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text
-                            .replace('R$ ', '')
-                            .replace(/\./g, '')
-                            .replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelMoeda(valor);
                 }
             },
             height: '300px',
-            chartArea: { border: { width: 0 } },
+            chartArea: { border: { width: 0 } }
         });
         chartValorCategoria.appendTo('#chartValorCategoria');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartValorCategoria',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartValorCategoria", error);
     }
 }
 
@@ -1712,25 +1201,19 @@
         const container = document.getElementById('chartValorLitro');
         if (!container) return;
 
-        if (chartValorLitro) {
-            chartValorLitro.destroy();
-            chartValorLitro = null;
-        }
+        if (chartValorLitro) { chartValorLitro.destroy(); chartValorLitro = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
-        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
+        const combustiveis = [...new Set(dados.map(d => d.combustivel))];
         const series = combustiveis.map((comb, idx) => {
             const dataPoints = [];
             for (let m = 1; m <= 12; m++) {
-                const item = dados.find(
-                    (d) => d.mes === m && d.combustivel === comb,
-                );
+                const item = dados.find(d => d.mes === m && d.combustivel === comb);
                 dataPoints.push({ x: MESES[m], y: item ? item.media : 0 });
             }
             return {
@@ -1740,33 +1223,23 @@
                 name: comb,
                 type: 'Line',
                 marker: { visible: true, width: 6, height: 6 },
-                width: 2,
+                width: 2
             };
         });
 
         chartValorLitro = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { size: '9px' },
-            },
-            primaryYAxis: {
-                labelFormat: 'R$ {value}',
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelStyle: { size: '9px' } },
+            primaryYAxis: { labelFormat: 'R$ {value}', labelStyle: { size: '9px' } },
             series: series,
             legendSettings: { visible: true, position: 'Bottom' },
             tooltip: { enable: true, format: '${series.name}: R$ ${point.y}' },
             height: '300px',
             chartArea: { border: { width: 0 } },
-            palettes: [CORES.amber[2], CORES.gold[1]],
+            palettes: [CORES.amber[2], CORES.gold[1]]
         });
         chartValorLitro.appendTo('#chartValorLitro');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartValorLitro',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartValorLitro", error);
     }
 }
 
@@ -1775,25 +1248,19 @@
         const container = document.getElementById('chartLitrosMes');
         if (!container) return;
 
-        if (chartLitrosMes) {
-            chartLitrosMes.destroy();
-            chartLitrosMes = null;
-        }
+        if (chartLitrosMes) { chartLitrosMes.destroy(); chartLitrosMes = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
-        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
+        const combustiveis = [...new Set(dados.map(d => d.combustivel))];
         const series = combustiveis.map((comb, idx) => {
             const dataPoints = [];
             for (let m = 1; m <= 12; m++) {
-                const item = dados.find(
-                    (d) => d.mes === m && d.combustivel === comb,
-                );
+                const item = dados.find(d => d.mes === m && d.combustivel === comb);
                 dataPoints.push({ x: MESES[m], y: item ? item.litros : 0 });
             }
             return {
@@ -1802,48 +1269,35 @@
                 yName: 'y',
                 name: comb,
                 type: 'StackingColumn',
-                cornerRadius: { topLeft: 2, topRight: 2 },
+                cornerRadius: { topLeft: 2, topRight: 2 }
             };
         });
 
         chartLitrosMes = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelStyle: { size: '9px' } },
             primaryYAxis: {
                 labelFormat: '{value}',
-                labelStyle: { size: '9px' },
+                labelStyle: { size: '9px' }
             },
             series: series,
             legendSettings: { visible: true, position: 'Bottom' },
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.series.name +
-                    ': ' +
-                    formatarLabelNumero(args.point.y) +
-                    ' L';
+                args.text = args.series.name + ': ' + formatarLabelNumero(args.point.y) + ' L';
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text.replace(/\./g, '').replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelNumero(valor);
                 }
             },
             height: '280px',
             chartArea: { border: { width: 0 } },
-            palettes: [CORES.amber[1], CORES.gold[1]],
+            palettes: [CORES.amber[1], CORES.gold[1]]
         });
         chartLitrosMes.appendTo('#chartLitrosMes');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartLitrosMes',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartLitrosMes", error);
     }
 }
 
@@ -1852,84 +1306,62 @@
         const container = document.getElementById('chartConsumoMes');
         if (!container) return;
 
-        if (chartConsumoMes) {
-            chartConsumoMes.destroy();
-            chartConsumoMes = null;
-        }
+        if (chartConsumoMes) { chartConsumoMes.destroy(); chartConsumoMes = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
         const dataSource = [];
         for (let m = 1; m <= 12; m++) {
-            const item = dados.find((d) => d.mes === m);
+            const item = dados.find(d => d.mes === m);
             dataSource.push({
                 x: MESES[m],
                 y: item ? item.valor : 0,
-                color: CORES.amber[m % CORES.amber.length],
+                color: CORES.amber[m % CORES.amber.length]
             });
         }
 
         chartConsumoMes = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelStyle: { size: '9px' } },
             primaryYAxis: {
                 labelFormat: 'R$ {value}',
-                labelStyle: { size: '9px' },
-            },
-            series: [
-                {
-                    dataSource: dataSource,
-                    xName: 'x',
-                    yName: 'y',
-                    pointColorMapping: 'color',
-                    type: 'Column',
-                    cornerRadius: { topLeft: 4, topRight: 4 },
-                },
-            ],
+                labelStyle: { size: '9px' }
+            },
+            series: [{
+                dataSource: dataSource,
+                xName: 'x',
+                yName: 'y',
+                pointColorMapping: 'color',
+                type: 'Column',
+                cornerRadius: { topLeft: 4, topRight: 4 }
+            }],
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
+                args.text = args.point.x + ': ' + formatarLabelMoeda(args.point.y);
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text
-                            .replace('R$ ', '')
-                            .replace(/\./g, '')
-                            .replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelMoeda(valor);
                 }
             },
             height: '280px',
-            chartArea: { border: { width: 0 } },
+            chartArea: { border: { width: 0 } }
         });
         chartConsumoMes.appendTo('#chartConsumoMes');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartConsumoMes',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartConsumoMes", error);
     }
 }
 
 function renderizarAbaMensal(data) {
     try {
 
-        document.getElementById('valorTotalMensal').textContent = formatarMoeda(
-            data.valorTotal,
-        );
-        document.getElementById('totalLitrosMensal').textContent =
-            formatarLitros(data.litrosTotal);
+        document.getElementById('valorTotalMensal').textContent = formatarMoeda(data.valorTotal);
+        document.getElementById('totalLitrosMensal').textContent = formatarLitros(data.litrosTotal);
 
         renderizarTabelaMediaLitroMensal(data.mediaLitro);
 
@@ -1942,17 +1374,12 @@
 
         renderizarChartConsumoCategoria(data.consumoPorCategoria);
 
-        const anoMensal =
-            document.getElementById('filtroAnoMensal')?.value || '';
+        const anoMensal = document.getElementById('filtroAnoMensal')?.value || '';
         if (anoMensal) {
             renderizarHeatmapCategoria(anoMensal);
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarAbaMensal',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarAbaMensal", error);
     }
 }
 
@@ -1962,13 +1389,12 @@
         if (!tbody) return;
 
         if (!dados || dados.length === 0) {
-            tbody.innerHTML =
-                '<tr><td colspan="2" class="text-center text-muted py-3">Sem dados</td></tr>';
+            tbody.innerHTML = '<tr><td colspan="2" class="text-center text-muted py-3">Sem dados</td></tr>';
             return;
         }
 
         let html = '';
-        dados.forEach((item) => {
+        dados.forEach(item => {
             html += `
                 <tr>
                     <td style="font-weight: 500;">${item.combustivel}</td>
@@ -1979,11 +1405,7 @@
 
         tbody.innerHTML = html;
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarTabelaMediaLitroMensal',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarTabelaMediaLitroMensal", error);
     }
 }
 
@@ -1992,15 +1414,11 @@
         const container = document.getElementById('chartPizzaCombustivel');
         if (!container) return;
 
-        if (chartPizzaCombustivel) {
-            chartPizzaCombustivel.destroy();
-            chartPizzaCombustivel = null;
-        }
+        if (chartPizzaCombustivel) { chartPizzaCombustivel.destroy(); chartPizzaCombustivel = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-4">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-4">Sem dados</div>';
             return;
         }
 
@@ -2008,47 +1426,37 @@
             x: item.combustivel,
             y: item.litros,
             text: item.combustivel,
-            color: CORES.multi[idx % CORES.multi.length],
+            color: CORES.multi[idx % CORES.multi.length]
         }));
 
         chartPizzaCombustivel = new ej.charts.AccumulationChart({
-            series: [
-                {
-                    dataSource: dataSource,
-                    xName: 'x',
-                    yName: 'y',
-                    pointColorMapping: 'color',
-                    type: 'Pie',
-                    dataLabel: {
-                        visible: false,
-                    },
-                    radius: '75%',
+            series: [{
+                dataSource: dataSource,
+                xName: 'x',
+                yName: 'y',
+                pointColorMapping: 'color',
+                type: 'Pie',
+                dataLabel: {
+                    visible: false
                 },
-            ],
+                radius: '75%'
+            }],
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.point.x +
-                    ': ' +
-                    formatarLabelNumero(args.point.y) +
-                    ' L';
+                args.text = args.point.x + ': ' + formatarLabelNumero(args.point.y) + ' L';
             },
 
             legendSettings: {
                 visible: true,
                 position: 'Bottom',
-                textStyle: { size: '11px', fontWeight: '500' },
+                textStyle: { size: '11px', fontWeight: '500' }
             },
             height: '180px',
-            enableSmartLabels: true,
+            enableSmartLabels: true
         });
         chartPizzaCombustivel.appendTo('#chartPizzaCombustivel');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartPizzaCombustivel',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartPizzaCombustivel", error);
     }
 }
 
@@ -2057,23 +1465,19 @@
         const container = document.getElementById('chartLitrosDia');
         if (!container) return;
 
-        if (chartLitrosDia) {
-            chartLitrosDia.destroy();
-            chartLitrosDia = null;
-        }
+        if (chartLitrosDia) { chartLitrosDia.destroy(); chartLitrosDia = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
-        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
+        const combustiveis = [...new Set(dados.map(d => d.combustivel))];
         const series = combustiveis.map((comb, idx) => {
             const dataPoints = dados
-                .filter((d) => d.combustivel === comb)
-                .map((d) => ({ x: d.dia, y: d.litros }));
+                .filter(d => d.combustivel === comb)
+                .map(d => ({ x: d.dia, y: d.litros }));
             return {
                 dataSource: dataPoints,
                 xName: 'x',
@@ -2081,7 +1485,7 @@
                 name: comb,
                 type: 'SplineArea',
                 opacity: 0.6,
-                border: { width: 2 },
+                border: { width: 2 }
             };
         });
 
@@ -2091,13 +1495,13 @@
                 labelStyle: { size: '10px' },
                 title: 'Dia do Mês',
                 titleStyle: { size: '11px', fontWeight: '600' },
-                interval: 1,
+                interval: 1
             },
             primaryYAxis: {
                 labelFormat: '{value}',
                 labelStyle: { size: '10px' },
                 title: 'Litros',
-                titleStyle: { size: '11px', fontWeight: '600' },
+                titleStyle: { size: '11px', fontWeight: '600' }
             },
             series: series,
 
@@ -2107,37 +1511,25 @@
                 textStyle: { size: '14px', fontWeight: '600' },
                 shapeHeight: 12,
                 shapeWidth: 12,
-                padding: 10,
+                padding: 10
             },
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.series.name +
-                    ' (Dia ' +
-                    args.point.x +
-                    '): ' +
-                    formatarLabelNumero(args.point.y) +
-                    ' L';
+                args.text = args.series.name + ' (Dia ' + args.point.x + '): ' + formatarLabelNumero(args.point.y) + ' L';
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text.replace(/\./g, '').replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelNumero(valor);
                 }
             },
             height: '220px',
             chartArea: { border: { width: 0 } },
-            palettes: [CORES.amber[2], CORES.gold[2]],
+            palettes: [CORES.amber[2], CORES.gold[2]]
         });
         chartLitrosDia.appendTo('#chartLitrosDia');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartLitrosDia',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartLitrosDia", error);
     }
 }
 
@@ -2147,8 +1539,7 @@
         if (!container) return;
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
+            container.innerHTML = '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
             return;
         }
 
@@ -2157,8 +1548,7 @@
 
         dados.forEach((item, idx) => {
             totalValor += item.valor;
-            const badgeClass =
-                idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
+            const badgeClass = idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
             html += `
                 <div class="grid-row grid-row-clicavel" data-unidade="${item.unidade}" title="Clique para ver abastecimentos desta unidade">
                     <div class="grid-cell"><span class="${badgeClass}">${idx + 1}</span> ${item.unidade}</div>
@@ -2176,7 +1566,7 @@
 
         container.innerHTML = html;
 
-        container.querySelectorAll('.grid-row-clicavel').forEach((row) => {
+        container.querySelectorAll('.grid-row-clicavel').forEach(row => {
             row.addEventListener('click', function () {
                 const unidade = this.dataset.unidade;
                 if (unidade) {
@@ -2185,11 +1575,7 @@
             });
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarTabelaValorPorUnidade',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarTabelaValorPorUnidade", error);
     }
 }
 
@@ -2199,8 +1585,7 @@
         if (!container) return;
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
+            container.innerHTML = '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
             return;
         }
 
@@ -2209,8 +1594,7 @@
 
         dados.forEach((item, idx) => {
             totalValor += item.valor;
-            const badgeClass =
-                idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
+            const badgeClass = idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
             html += `
                 <div class="grid-row grid-row-clicavel" data-veiculo-id="${item.veiculoId}" data-placa="${item.placa}" title="Clique para ver viagens deste veículo">
                     <div class="grid-cell"><span class="${badgeClass}">${idx + 1}</span> <strong>${item.placa}</strong></div>
@@ -2229,7 +1613,7 @@
 
         container.innerHTML = html;
 
-        container.querySelectorAll('.grid-row-clicavel').forEach((row) => {
+        container.querySelectorAll('.grid-row-clicavel').forEach(row => {
             row.addEventListener('click', function () {
                 const veiculoId = this.dataset.veiculoId;
                 const placa = this.dataset.placa;
@@ -2239,11 +1623,7 @@
             });
         });
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarTabelaValorPorPlaca',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarTabelaValorPorPlaca", error);
     }
 }
 
@@ -2252,70 +1632,55 @@
         const container = document.getElementById('chartConsumoCategoria');
         if (!container) return;
 
-        if (chartConsumoCategoria) {
-            chartConsumoCategoria.destroy();
-            chartConsumoCategoria = null;
-        }
+        if (chartConsumoCategoria) { chartConsumoCategoria.destroy(); chartConsumoCategoria = null; }
         container.innerHTML = '';
 
         console.log('consumoPorCategoria:', dados);
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados de categoria</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados de categoria</div>';
             return;
         }
 
         const dataSource = dados.map((item, idx) => ({
             x: item.categoria || 'Sem Categoria',
             y: item.valor || 0,
-            color: CORES.categorias[idx % CORES.categorias.length],
+            color: CORES.categorias[idx % CORES.categorias.length]
         }));
 
         chartConsumoCategoria = new ej.charts.Chart({
             primaryXAxis: {
                 valueType: 'Category',
                 labelRotation: -30,
-                labelStyle: { size: '11px', fontWeight: '500' },
+                labelStyle: { size: '11px', fontWeight: '500' }
             },
             primaryYAxis: {
                 labelFormat: 'R$ {value}',
-                labelStyle: { size: '10px' },
-            },
-            series: [
-                {
-                    dataSource: dataSource,
-                    xName: 'x',
-                    yName: 'y',
-                    pointColorMapping: 'color',
-                    type: 'Column',
-                    cornerRadius: { topLeft: 4, topRight: 4 },
-                    dataLabel: {
-                        visible: true,
-                        position: 'Top',
-                        font: { size: '10px', fontWeight: '600' },
-                    },
-                },
-            ],
+                labelStyle: { size: '10px' }
+            },
+            series: [{
+                dataSource: dataSource,
+                xName: 'x',
+                yName: 'y',
+                pointColorMapping: 'color',
+                type: 'Column',
+                cornerRadius: { topLeft: 4, topRight: 4 },
+                dataLabel: {
+                    visible: true,
+                    position: 'Top',
+                    font: { size: '10px', fontWeight: '600' }
+                }
+            }],
             tooltip: { enable: true },
             tooltipRender: function (args) {
-                args.text =
-                    args.point.x +
-                    ': ' +
-                    formatarLabelMoeda(args.point.y) +
-                    ' (clique para detalhes)';
+                args.text = args.point.x + ': ' + formatarLabelMoeda(args.point.y) + ' (clique para detalhes)';
             },
             textRender: function (args) {
                 args.text = formatarMoedaCompacta(args.point.y);
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text
-                            .replace('R$ ', '')
-                            .replace(/\./g, '')
-                            .replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelMoeda(valor);
                 }
             },
@@ -2327,15 +1692,11 @@
                 }
             },
             height: '280px',
-            chartArea: { border: { width: 0 } },
+            chartArea: { border: { width: 0 } }
         });
         chartConsumoCategoria.appendTo('#chartConsumoCategoria');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartConsumoCategoria',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartConsumoCategoria", error);
     }
 }
 
@@ -2346,56 +1707,27 @@
 
         if (temVeiculoSelecionado) {
 
-            document.getElementById('valorTotalVeiculo').textContent =
-                formatarMoeda(data.valorTotal);
-            document.getElementById('litrosTotalVeiculo').textContent =
-                formatarLitros(data.litrosTotal);
-            document.getElementById('descricaoVeiculoSelecionado').textContent =
-                data.descricaoVeiculo;
-            document.getElementById('categoriaVeiculoSelecionado').textContent =
-                data.categoriaVeiculo;
-
-            renderizarChartConsumoMensalVeiculo(
-                data.consumoMensalLitros,
-                data.mesSelecionado || 0,
-            );
-            renderizarChartValorMensalVeiculo(
-                data.valorMensal,
-                data.mesSelecionado || 0,
-            );
-            renderizarChartRankingVeiculos(
-                data.veiculosComValor,
-                veiculoSelecionadoId,
-                placaSelecionada,
-            );
+            document.getElementById('valorTotalVeiculo').textContent = formatarMoeda(data.valorTotal);
+            document.getElementById('litrosTotalVeiculo').textContent = formatarLitros(data.litrosTotal);
+            document.getElementById('descricaoVeiculoSelecionado').textContent = data.descricaoVeiculo;
+            document.getElementById('categoriaVeiculoSelecionado').textContent = data.categoriaVeiculo;
+
+            renderizarChartConsumoMensalVeiculo(data.consumoMensalLitros, data.mesSelecionado || 0);
+            renderizarChartValorMensalVeiculo(data.valorMensal, data.mesSelecionado || 0);
+            renderizarChartRankingVeiculos(data.veiculosComValor, veiculoSelecionadoId, placaSelecionada);
         } else {
 
             document.getElementById('valorTotalVeiculo').textContent = '-';
             document.getElementById('litrosTotalVeiculo').textContent = '-';
-            document.getElementById('descricaoVeiculoSelecionado').textContent =
-                'Nenhum veículo selecionado';
-            document.getElementById('categoriaVeiculoSelecionado').textContent =
-                '-';
-
-            renderizarGraficoVazioComMensagem(
-                'chartConsumoMensalVeiculo',
-                'Escolha um Veículo para visualizar os Dados',
-            );
-            renderizarGraficoVazioComMensagem(
-                'chartValorMensalVeiculo',
-                'Escolha um Veículo para visualizar os Dados',
-            );
-            renderizarGraficoVazioComMensagem(
-                'chartRankingVeiculos',
-                'Escolha um Veículo para visualizar os Dados',
-            );
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarAbaVeiculo',
-            error,
-        );
+            document.getElementById('descricaoVeiculoSelecionado').textContent = 'Nenhum veículo selecionado';
+            document.getElementById('categoriaVeiculoSelecionado').textContent = '-';
+
+            renderizarGraficoVazioComMensagem('chartConsumoMensalVeiculo', 'Escolha um Veículo para visualizar os Dados');
+            renderizarGraficoVazioComMensagem('chartValorMensalVeiculo', 'Escolha um Veículo para visualizar os Dados');
+            renderizarGraficoVazioComMensagem('chartRankingVeiculos', 'Escolha um Veículo para visualizar os Dados');
+        }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarAbaVeiculo", error);
     }
 }
 
@@ -2404,17 +1736,11 @@
         const container = document.getElementById(containerId);
         if (!container) return;
 
-        if (
-            containerId === 'chartConsumoMensalVeiculo' &&
-            chartConsumoMensalVeiculo
-        ) {
+        if (containerId === 'chartConsumoMensalVeiculo' && chartConsumoMensalVeiculo) {
             chartConsumoMensalVeiculo.destroy();
             chartConsumoMensalVeiculo = null;
         }
-        if (
-            containerId === 'chartValorMensalVeiculo' &&
-            chartValorMensalVeiculo
-        ) {
+        if (containerId === 'chartValorMensalVeiculo' && chartValorMensalVeiculo) {
             chartValorMensalVeiculo.destroy();
             chartValorMensalVeiculo = null;
         }
@@ -2430,11 +1756,7 @@
             </div>
         `;
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarGraficoVazioComMensagem',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarGraficoVazioComMensagem", error);
     }
 }
 
@@ -2443,30 +1765,23 @@
         const container = document.getElementById('chartConsumoMensalVeiculo');
         if (!container) return;
 
-        if (chartConsumoMensalVeiculo) {
-            chartConsumoMensalVeiculo.destroy();
-            chartConsumoMensalVeiculo = null;
-        }
+        if (chartConsumoMensalVeiculo) { chartConsumoMensalVeiculo.destroy(); chartConsumoMensalVeiculo = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
-        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
+        const combustiveis = [...new Set(dados.map(d => d.combustivel))];
         const series = combustiveis.map((comb) => {
             const dataPoints = [];
             for (let m = 1; m <= 12; m++) {
-                const item = dados.find(
-                    (d) => d.mes === m && d.combustivel === comb,
-                );
-                const ehMesSelecionado =
-                    mesSelecionado > 0 && m === mesSelecionado;
+                const item = dados.find(d => d.mes === m && d.combustivel === comb);
+                const ehMesSelecionado = mesSelecionado > 0 && m === mesSelecionado;
                 dataPoints.push({
                     x: ehMesSelecionado ? '★ ' + MESES[m] : MESES[m],
-                    y: item ? item.litros : 0,
+                    y: item ? item.litros : 0
                 });
             }
             return {
@@ -2476,51 +1791,36 @@
                 name: comb,
                 type: 'SplineArea',
                 opacity: 0.6,
-                border: { width: 2 },
+                border: { width: 2 }
             };
         });
 
         chartConsumoMensalVeiculo = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelStyle: { size: '9px' } },
             primaryYAxis: {
                 labelFormat: '{value}',
-                labelStyle: { size: '9px' },
+                labelStyle: { size: '9px' }
             },
             series: series,
             legendSettings: { visible: true, position: 'Bottom' },
             tooltip: { enable: true },
             tooltipRender: function (args) {
                 const label = args.point.x.replace('★ ', '');
-                args.text =
-                    args.series.name +
-                    ' (' +
-                    label +
-                    '): ' +
-                    formatarLabelNumero(args.point.y) +
-                    'L';
+                args.text = args.series.name + ' (' + label + '): ' + formatarLabelNumero(args.point.y) + 'L';
             },
             axisLabelRender: function (args) {
                 if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text.replace(/\./g, '').replace(',', '.'),
-                    );
+                    const valor = parseFloat(args.text.replace(/\./g, '').replace(',', '.'));
                     args.text = formatarLabelNumero(valor);
                 }
             },
             height: '160px',
             chartArea: { border: { width: 0 } },
-            palettes: ['#ff6b35', CORES.gold[1]],
+            palettes: ['#ff6b35', CORES.gold[1]]
         });
         chartConsumoMensalVeiculo.appendTo('#chartConsumoMensalVeiculo');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartConsumoMensalVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartConsumoMensalVeiculo", error);
     }
 }
 
@@ -2529,15 +1829,11 @@
         const container = document.getElementById('chartValorMensalVeiculo');
         if (!container) return;
 
-        if (chartValorMensalVeiculo) {
-            chartValorMensalVeiculo.destroy();
-            chartValorMensalVeiculo = null;
-        }
+        if (chartValorMensalVeiculo) { chartValorMensalVeiculo.destroy(); chartValorMensalVeiculo = null; }
         container.innerHTML = '';
 
         if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
             return;
         }
 
@@ -2546,30 +1842,129 @@
 
         const dataSource = [];
         for (let m = 1; m <= 12; m++) {
-            const item = dados.find((d) => d.mes === m);
+            const item = dados.find(d => d.mes === m);
             const ehMesSelecionado = mesSelecionado > 0 && m === mesSelecionado;
             dataSource.push({
                 x: MESES_COMPLETOS[m],
                 y: item ? item.valor : 0,
                 color: ehMesSelecionado ? corDestaque : corNormal,
                 mes: m,
-                selecionado: ehMesSelecionado,
+                selecionado: ehMesSelecionado
             });
         }
 
         chartValorMensalVeiculo = new ej.charts.Chart({
-            primaryXAxis: {
-                valueType: 'Category',
-                labelRotation: -45,
-                labelStyle: { size: '9px' },
-            },
+            primaryXAxis: { valueType: 'Category', labelRotation: -45, labelStyle: { size: '9px' } },
             primaryYAxis: {
                 labelFormat: 'R$ {value}',
-                labelStyle: { size: '9px' },
-            },
-            series: [
-                {
-                    dataSource: dataSource,
+                labelStyle: { size: '9px' }
+            },
+            series: [{
+                dataSource: dataSource,
+                xName: 'x',
+                yName: 'y',
+                pointColorMapping: 'color',
+                type: 'Bar',
+                cornerRadius: { topRight: 4, bottomRight: 4 },
+                marker: {
+                    dataLabel: {
+                        visible: true,
+                        position: 'Top',
+                        font: { size: '12px', color: '#ff6b35', fontWeight: 'bold' }
+                    }
+                }
+            }],
+            tooltip: { enable: true },
+            tooltipRender: function (args) {
+                args.text = args.point.x + ': ' + formatarLabelMoeda(args.point.y);
+            },
+            textRender: function (args) {
+
+                if (args.point && args.point.selecionado) {
+                    args.text = '★';
+                } else {
+                    args.text = '';
+                }
+            },
+            axisLabelRender: function (args) {
+                if (args.axis.name === 'primaryYAxis') {
+                    const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
+                    args.text = formatarLabelMoeda(valor);
+                }
+            },
+            height: '280px',
+            chartArea: { border: { width: 0 } }
+        });
+        chartValorMensalVeiculo.appendTo('#chartValorMensalVeiculo');
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartValorMensalVeiculo", error);
+    }
+}
+
+function renderizarChartRankingVeiculos(dados, veiculoSelecionadoId, placaSelecionada) {
+    try {
+        const container = document.getElementById('chartRankingVeiculos');
+        const tituloEl = document.getElementById('tituloRankingVeiculos');
+        const subtituloEl = document.getElementById('subtituloRankingVeiculos');
+        const iconEl = document.getElementById('iconRankingVeiculos');
+        if (!container) return;
+
+        if (chartRankingVeiculos) { chartRankingVeiculos.destroy(); chartRankingVeiculos = null; }
+        container.innerHTML = '';
+
+        if (!dados || dados.length === 0) {
+            container.innerHTML = '<div class="text-center text-muted py-5">Sem dados</div>';
+            return;
+        }
+
+        const modoComparativo = veiculoSelecionadoId && placaSelecionada;
+
+        if (modoComparativo) {
+
+            if (tituloEl) tituloEl.textContent = 'Comparativo de Consumo';
+            if (subtituloEl) subtituloEl.textContent = placaSelecionada + ' vs. Top 10';
+            if (iconEl) iconEl.className = 'fa-duotone fa-chart-mixed';
+
+            const top10 = dados.slice(0, 10);
+            const veiculoNoTop10 = top10.find(v => v.veiculoId == veiculoSelecionadoId);
+            const veiculoSelecionado = dados.find(v => v.veiculoId == veiculoSelecionadoId);
+
+            let dadosComparativo = [];
+
+            const corDestaque = '#ff6b35';
+
+            if (veiculoSelecionado && !veiculoNoTop10) {
+                dadosComparativo.push({
+                    x: veiculoSelecionado.placa,
+                    y: veiculoSelecionado.valor,
+                    color: corDestaque,
+                    veiculoId: veiculoSelecionado.veiculoId,
+                    selecionado: true
+                });
+            }
+
+            top10.forEach((item, idx) => {
+                const isSelecionado = item.veiculoId == veiculoSelecionadoId;
+                dadosComparativo.push({
+                    x: item.placa,
+                    y: item.valor,
+                    color: isSelecionado ? corDestaque : CORES.multi[idx % CORES.multi.length],
+                    veiculoId: item.veiculoId,
+                    selecionado: isSelecionado
+                });
+            });
+
+            chartRankingVeiculos = new ej.charts.Chart({
+                primaryXAxis: {
+                    valueType: 'Category',
+                    labelStyle: { size: '8px' }
+                },
+                primaryYAxis: {
+                    labelFormat: 'R$ {value}',
+                    labelStyle: { size: '9px' }
+                },
+                series: [{
+                    dataSource: dadosComparativo,
                     xName: 'x',
                     yName: 'y',
                     pointColorMapping: 'color',
@@ -2579,156 +1974,14 @@
                         dataLabel: {
                             visible: true,
                             position: 'Top',
-                            font: {
-                                size: '12px',
-                                color: '#ff6b35',
-                                fontWeight: 'bold',
-                            },
-                        },
-                    },
-                },
-            ],
-            tooltip: { enable: true },
-            tooltipRender: function (args) {
-                args.text =
-                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
-            },
-            textRender: function (args) {
-
-                if (args.point && args.point.selecionado) {
-                    args.text = '★';
-                } else {
-                    args.text = '';
-                }
-            },
-            axisLabelRender: function (args) {
-                if (args.axis.name === 'primaryYAxis') {
-                    const valor = parseFloat(
-                        args.text
-                            .replace('R$ ', '')
-                            .replace(/\./g, '')
-                            .replace(',', '.'),
-                    );
-                    args.text = formatarLabelMoeda(valor);
-                }
-            },
-            height: '280px',
-            chartArea: { border: { width: 0 } },
-        });
-        chartValorMensalVeiculo.appendTo('#chartValorMensalVeiculo');
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartValorMensalVeiculo',
-            error,
-        );
-    }
-}
-
-function renderizarChartRankingVeiculos(
-    dados,
-    veiculoSelecionadoId,
-    placaSelecionada,
-) {
-    try {
-        const container = document.getElementById('chartRankingVeiculos');
-        const tituloEl = document.getElementById('tituloRankingVeiculos');
-        const subtituloEl = document.getElementById('subtituloRankingVeiculos');
-        const iconEl = document.getElementById('iconRankingVeiculos');
-        if (!container) return;
-
-        if (chartRankingVeiculos) {
-            chartRankingVeiculos.destroy();
-            chartRankingVeiculos = null;
-        }
-        container.innerHTML = '';
-
-        if (!dados || dados.length === 0) {
-            container.innerHTML =
-                '<div class="text-center text-muted py-5">Sem dados</div>';
-            return;
-        }
-
-        const modoComparativo = veiculoSelecionadoId && placaSelecionada;
-
-        if (modoComparativo) {
-
-            if (tituloEl) tituloEl.textContent = 'Comparativo de Consumo';
-            if (subtituloEl)
-                subtituloEl.textContent = placaSelecionada + ' vs. Top 10';
-            if (iconEl) iconEl.className = 'fa-duotone fa-chart-mixed';
-
-            const top10 = dados.slice(0, 10);
-            const veiculoNoTop10 = top10.find(
-                (v) => v.veiculoId == veiculoSelecionadoId,
-            );
-            const veiculoSelecionado = dados.find(
-                (v) => v.veiculoId == veiculoSelecionadoId,
-            );
-
-            let dadosComparativo = [];
-
-            const corDestaque = '#ff6b35';
-
-            if (veiculoSelecionado && !veiculoNoTop10) {
-                dadosComparativo.push({
-                    x: veiculoSelecionado.placa,
-                    y: veiculoSelecionado.valor,
-                    color: corDestaque,
-                    veiculoId: veiculoSelecionado.veiculoId,
-                    selecionado: true,
-                });
-            }
-
-            top10.forEach((item, idx) => {
-                const isSelecionado = item.veiculoId == veiculoSelecionadoId;
-                dadosComparativo.push({
-                    x: item.placa,
-                    y: item.valor,
-                    color: isSelecionado
-                        ? corDestaque
-                        : CORES.multi[idx % CORES.multi.length],
-                    veiculoId: item.veiculoId,
-                    selecionado: isSelecionado,
-                });
-            });
-
-            chartRankingVeiculos = new ej.charts.Chart({
-                primaryXAxis: {
-                    valueType: 'Category',
-                    labelStyle: { size: '8px' },
-                },
-                primaryYAxis: {
-                    labelFormat: 'R$ {value}',
-                    labelStyle: { size: '9px' },
-                },
-                series: [
-                    {
-                        dataSource: dadosComparativo,
-                        xName: 'x',
-                        yName: 'y',
-                        pointColorMapping: 'color',
-                        type: 'Bar',
-                        cornerRadius: { topRight: 4, bottomRight: 4 },
-                        marker: {
-                            dataLabel: {
-                                visible: true,
-                                position: 'Top',
-                                font: {
-                                    size: '12px',
-                                    color: '#ff6b35',
-                                    fontWeight: 'bold',
-                                },
-                                template:
-                                    '<div style="font-size:14px;color:#ff6b35;">${point.selecionado ? "★" : ""}</div>',
-                            },
-                        },
-                    },
-                ],
+                            font: { size: '12px', color: '#ff6b35', fontWeight: 'bold' },
+                            template: '<div style="font-size:14px;color:#ff6b35;">${point.selecionado ? "★" : ""}</div>'
+                        }
+                    }
+                }],
                 tooltip: { enable: true },
                 tooltipRender: function (args) {
-                    args.text =
-                        args.point.x + ': ' + formatarLabelMoeda(args.point.y);
+                    args.text = args.point.x + ': ' + formatarLabelMoeda(args.point.y);
                 },
                 textRender: function (args) {
 
@@ -2740,17 +1993,12 @@
                 },
                 axisLabelRender: function (args) {
                     if (args.axis.name === 'primaryYAxis') {
-                        const valor = parseFloat(
-                            args.text
-                                .replace('R$ ', '')
-                                .replace(/\./g, '')
-                                .replace(',', '.'),
-                        );
+                        const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
                         args.text = formatarLabelMoeda(valor);
                     }
                 },
                 height: '280px',
-                chartArea: { border: { width: 0 } },
+                chartArea: { border: { width: 0 } }
             });
         } else {
 
@@ -2760,33 +2008,29 @@
 
             const top10 = dados.slice(0, 10);
             const dataSource = top10.map((item, idx) => ({
-                x:
-                    item.placa +
-                    (item.tipoVeiculo ? '\n' + item.tipoVeiculo : ''),
+                x: item.placa + (item.tipoVeiculo ? '\n' + item.tipoVeiculo : ''),
                 y: item.valor,
                 color: CORES.multi[idx % CORES.multi.length],
-                veiculoId: item.veiculoId,
+                veiculoId: item.veiculoId
             }));
 
             chartRankingVeiculos = new ej.charts.Chart({
                 primaryXAxis: {
                     valueType: 'Category',
-                    labelStyle: { size: '8px' },
+                    labelStyle: { size: '8px' }
                 },
                 primaryYAxis: {
                     labelFormat: 'R$ {value}',
-                    labelStyle: { size: '9px' },
+                    labelStyle: { size: '9px' }
                 },
-                series: [
-                    {
-                        dataSource: dataSource,
-                        xName: 'x',
-                        yName: 'y',
-                        pointColorMapping: 'color',
-                        type: 'Bar',
-                        cornerRadius: { topRight: 4, bottomRight: 4 },
-                    },
-                ],
+                series: [{
+                    dataSource: dataSource,
+                    xName: 'x',
+                    yName: 'y',
+                    pointColorMapping: 'color',
+                    type: 'Bar',
+                    cornerRadius: { topRight: 4, bottomRight: 4 }
+                }],
                 tooltip: { enable: true },
                 tooltipRender: function (args) {
                     const label = args.point.x.replace('\n', ' - ');
@@ -2794,12 +2038,7 @@
                 },
                 axisLabelRender: function (args) {
                     if (args.axis.name === 'primaryYAxis') {
-                        const valor = parseFloat(
-                            args.text
-                                .replace('R$ ', '')
-                                .replace(/\./g, '')
-                                .replace(',', '.'),
-                        );
+                        const valor = parseFloat(args.text.replace('R$ ', '').replace(/\./g, '').replace(',', '.'));
                         args.text = formatarLabelMoeda(valor);
                     }
                 },
@@ -2810,90 +2049,44 @@
                     }
                 },
                 height: '280px',
-                chartArea: { border: { width: 0 } },
+                chartArea: { border: { width: 0 } }
             });
         }
 
         chartRankingVeiculos.appendTo('#chartRankingVeiculos');
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarChartRankingVeiculos',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarChartRankingVeiculos", error);
     }
 }
 
 function destruirGraficosGeral() {
     try {
-        if (chartValorCategoria) {
-            chartValorCategoria.destroy();
-            chartValorCategoria = null;
-        }
-        if (chartValorLitro) {
-            chartValorLitro.destroy();
-            chartValorLitro = null;
-        }
-        if (chartLitrosMes) {
-            chartLitrosMes.destroy();
-            chartLitrosMes = null;
-        }
-        if (chartConsumoMes) {
-            chartConsumoMes.destroy();
-            chartConsumoMes = null;
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'destruirGraficosGeral',
-            error,
-        );
+        if (chartValorCategoria) { chartValorCategoria.destroy(); chartValorCategoria = null; }
+        if (chartValorLitro) { chartValorLitro.destroy(); chartValorLitro = null; }
+        if (chartLitrosMes) { chartLitrosMes.destroy(); chartLitrosMes = null; }
+        if (chartConsumoMes) { chartConsumoMes.destroy(); chartConsumoMes = null; }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "destruirGraficosGeral", error);
     }
 }
 
 function destruirGraficosMensal() {
     try {
-        if (chartPizzaCombustivel) {
-            chartPizzaCombustivel.destroy();
-            chartPizzaCombustivel = null;
-        }
-        if (chartLitrosDia) {
-            chartLitrosDia.destroy();
-            chartLitrosDia = null;
-        }
-        if (chartConsumoCategoria) {
-            chartConsumoCategoria.destroy();
-            chartConsumoCategoria = null;
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'destruirGraficosMensal',
-            error,
-        );
+        if (chartPizzaCombustivel) { chartPizzaCombustivel.destroy(); chartPizzaCombustivel = null; }
+        if (chartLitrosDia) { chartLitrosDia.destroy(); chartLitrosDia = null; }
+        if (chartConsumoCategoria) { chartConsumoCategoria.destroy(); chartConsumoCategoria = null; }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "destruirGraficosMensal", error);
     }
 }
 
 function destruirGraficosVeiculo() {
     try {
-        if (chartConsumoMensalVeiculo) {
-            chartConsumoMensalVeiculo.destroy();
-            chartConsumoMensalVeiculo = null;
-        }
-        if (chartValorMensalVeiculo) {
-            chartValorMensalVeiculo.destroy();
-            chartValorMensalVeiculo = null;
-        }
-        if (chartRankingVeiculos) {
-            chartRankingVeiculos.destroy();
-            chartRankingVeiculos = null;
-        }
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'destruirGraficosVeiculo',
-            error,
-        );
+        if (chartConsumoMensalVeiculo) { chartConsumoMensalVeiculo.destroy(); chartConsumoMensalVeiculo = null; }
+        if (chartValorMensalVeiculo) { chartValorMensalVeiculo.destroy(); chartValorMensalVeiculo = null; }
+        if (chartRankingVeiculos) { chartRankingVeiculos.destroy(); chartRankingVeiculos = null; }
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "destruirGraficosVeiculo", error);
     }
 }
 
@@ -2903,12 +2096,11 @@
         const $selectPlaca = $('#filtroPlacaVeiculo');
 
         const placaAtual = selectPlaca?.value || '';
-        const textoPlacaAtual =
-            selectPlaca?.options[selectPlaca.selectedIndex]?.text || '';
+        const textoPlacaAtual = selectPlaca?.options[selectPlaca.selectedIndex]?.text || '';
         let resultado = {
             placaMantida: false,
             placaRemovida: false,
-            placaTexto: obterPlacaTextoCompleto(textoPlacaAtual),
+            placaTexto: obterPlacaTextoCompleto(textoPlacaAtual)
         };
 
         if (!data || !Array.isArray(data.placasDisponiveis)) {
@@ -2917,34 +2109,24 @@
             return resultado;
         }
 
-        if (
-            $selectPlaca.length &&
-            $selectPlaca.hasClass('select2-hidden-accessible')
-        ) {
+        if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
             $selectPlaca.select2('destroy');
         }
 
         selectPlaca.innerHTML = '<option value="">Todas</option>';
 
-        data.placasDisponiveis.forEach((item) => {
+        data.placasDisponiveis.forEach(item => {
             const option = document.createElement('option');
             option.value = item.veiculoId;
-            const marcaModelo =
-                item.marcaModelo ||
-                item.marcaModeloDescricao ||
-                item.marcaModeloCompleto ||
-                '';
+            const marcaModelo = item.marcaModelo || item.marcaModeloDescricao || item.marcaModeloCompleto || '';
             const tipoVeiculo = item.tipoVeiculo || item.tipo || '';
             const descricao = marcaModelo || tipoVeiculo;
-            option.textContent =
-                item.placa + (descricao ? ' - ' + descricao : '');
+            option.textContent = item.placa + (descricao ? ' - ' + descricao : '');
             selectPlaca.appendChild(option);
         });
 
         if (placaAtual) {
-            const opcaoExiste = Array.from(selectPlaca.options).some(
-                (opt) => opt.value === placaAtual,
-            );
+            const opcaoExiste = Array.from(selectPlaca.options).some(opt => opt.value === placaAtual);
             if (opcaoExiste) {
                 selectPlaca.value = placaAtual;
                 resultado.placaMantida = true;
@@ -2952,10 +2134,7 @@
                 resultado.placaRemovida = true;
                 ignorarMudancaPlacaVeiculo = true;
                 selectPlaca.value = '';
-                if (
-                    $selectPlaca.length &&
-                    $selectPlaca.hasClass('select2-hidden-accessible')
-                ) {
+                if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
                     $selectPlaca.val('').trigger('change.select2');
                 }
                 ignorarMudancaPlacaVeiculo = false;
@@ -2968,16 +2147,10 @@
                 allowClear: true,
                 width: '260px',
                 language: {
-                    noResults: function () {
-                        return 'Nenhuma placa encontrada';
-                    },
-                    searching: function () {
-                        return 'Buscando...';
-                    },
-                    inputTooShort: function () {
-                        return 'Digite para buscar';
-                    },
-                },
+                    noResults: function () { return 'Nenhuma placa encontrada'; },
+                    searching: function () { return 'Buscando...'; },
+                    inputTooShort: function () { return 'Digite para buscar'; }
+                }
             });
 
             $selectPlaca.trigger('change.select2');
@@ -2987,11 +2160,7 @@
         }
         return resultado;
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'preencherFiltrosVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "preencherFiltrosVeiculo", error);
         return { placaMantida: false, placaRemovida: false, placaTexto: '' };
     }
 }
@@ -3000,21 +2169,14 @@
     try {
 
         const $selectPlaca = $('#filtroPlacaVeiculo');
-        if (
-            $selectPlaca.length &&
-            $selectPlaca.hasClass('select2-hidden-accessible')
-        ) {
+        if ($selectPlaca.length && $selectPlaca.hasClass('select2-hidden-accessible')) {
             $selectPlaca.val(veiculoId).trigger('change');
         } else {
             document.getElementById('filtroPlacaVeiculo').value = veiculoId;
             carregarDadosVeiculo();
         }
     } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'selecionarVeiculo',
-            error,
-        );
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "selecionarVeiculo", error);
     }
 }
 
@@ -3023,24 +2185,12 @@
     if (Math.abs(valor) >= 100) {
         return 'R$ ' + Math.round(valor).toLocaleString('pt-BR');
     }
-    return (
-        'R$ ' +
-        valor.toLocaleString('pt-BR', {
-            minimumFractionDigits: 2,
-            maximumFractionDigits: 2,
-        })
-    );
+    return 'R$ ' + valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
 }
 
 function formatarMoedaTabela(valor) {
     if (!valor) return 'R$ 0,00';
-    return (
-        'R$ ' +
-        valor.toLocaleString('pt-BR', {
-            minimumFractionDigits: 2,
-            maximumFractionDigits: 2,
-        })
-    );
+    return 'R$ ' + valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
 }
 
 function formatarMoedaCompacta(valor) {
@@ -3059,20 +2209,17 @@
     if (Math.abs(valor) >= 100) {
         return Math.round(valor).toLocaleString('pt-BR');
     }
-    return valor.toLocaleString('pt-BR', {
-        minimumFractionDigits: 2,
-        maximumFractionDigits: 2,
-    });
+    return valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
 }
 
 function formatarNumeroK(valor) {
     if (!valor) return '0';
     if (valor >= 1000000) {
-        const num = valor / 1000000;
+        const num = (valor / 1000000);
         return num >= 100 ? num.toFixed(0) + 'M' : num.toFixed(2) + 'M';
     }
     if (valor >= 1000) {
-        const num = valor / 1000;
+        const num = (valor / 1000);
         return num >= 100 ? num.toFixed(0) + 'K' : num.toFixed(2) + 'K';
     }
     return Math.round(valor).toLocaleString('pt-BR');
@@ -3089,13 +2236,7 @@
     if (Math.abs(valor) >= 100) {
         return 'R$ ' + Math.round(valor).toLocaleString('pt-BR');
     }
-    return (
-        'R$ ' +
-        valor.toLocaleString('pt-BR', {
-            minimumFractionDigits: 2,
-            maximumFractionDigits: 2,
-        })
-    );
+    return 'R$ ' + valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
 }
 
 function formatarLabelNumero(valor) {
@@ -3103,10 +2244,7 @@
     if (Math.abs(valor) >= 100) {
         return Math.round(valor).toLocaleString('pt-BR');
     }
-    return valor.toLocaleString('pt-BR', {
-        minimumFractionDigits: 2,
-        maximumFractionDigits: 2,
-    });
+    return valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
 }
 
 var heatmapDiaHora = null;
@@ -3114,23 +2252,15 @@
 var heatmapVeiculo = null;
 var modalDetalhes = null;
 
-const HEATMAP_CORES = [
-    '#f5ebe0',
-    '#d4a574',
-    '#c4956a',
-    '#a8784c',
-    '#8b5e3c',
-    '#6d472c',
-];
+const HEATMAP_CORES = ['#f5ebe0', '#d4a574', '#c4956a', '#a8784c', '#8b5e3c', '#6d472c'];
 
 function criarLegendaHeatmap(containerId, dados) {
     const container = document.getElementById(containerId);
     if (!container) return;
 
-    const valores = dados.flat().filter((v) => v > 0);
+    const valores = dados.flat().filter(v => v > 0);
     if (valores.length === 0) {
-        container.innerHTML =
-            '<span class="text-muted" style="font-size: 0.75rem;">Sem dados para exibir legenda</span>';
+        container.innerHTML = '<span class="text-muted" style="font-size: 0.75rem;">Sem dados para exibir legenda</span>';
         return;
     }
 
@@ -3142,8 +2272,8 @@
 
     let html = '';
     for (let i = 0; i < steps; i++) {
-        const valorInicio = i === 0 ? 0 : min + stepSize * (i - 0.5);
-        const valorFim = i === steps - 1 ? max : min + stepSize * (i + 0.5);
+        const valorInicio = i === 0 ? 0 : min + (stepSize * (i - 0.5));
+        const valorFim = i === steps - 1 ? max : min + (stepSize * (i + 0.5));
 
         let label;
         if (i === 0) {
@@ -3151,11 +2281,7 @@
         } else if (i === steps - 1) {
             label = '> R$ ' + formatarNumeroK(valorFim * 0.8);
         } else {
-            label =
-                'R$ ' +
-                formatarNumeroK(valorInicio) +
-                ' - ' +
-                formatarNumeroK(valorFim);
+            label = 'R$ ' + formatarNumeroK(valorInicio) + ' - ' + formatarNumeroK(valorFim);
         }
 
         html += `
@@ -3193,10 +2319,7 @@
                     for (var i = 0; i < diasSemana.length; i++) {
                         var row = [];
                         for (var j = 0; j < horas.length; j++) {
-                            var item = data.data.find(
-                                (d) =>
-                                    d.x === diasSemana[i] && d.y === horas[j],
-                            );
+                            var item = data.data.find(d => d.x === diasSemana[i] && d.y === horas[j]);
                             row.push(item ? item.value : 0);
                         }
                         heatmapData.push(row);
@@ -3206,16 +2329,16 @@
                         titleSettings: { text: '' },
                         xAxis: {
                             labels: diasSemana,
-                            textStyle: { size: '11px', fontFamily: 'Outfit' },
+                            textStyle: { size: '11px', fontFamily: 'Outfit' }
                         },
                         yAxis: {
                             labels: horas,
-                            textStyle: { size: '9px', fontFamily: 'Outfit' },
+                            textStyle: { size: '9px', fontFamily: 'Outfit' }
                         },
                         dataSource: heatmapData,
                         cellSettings: {
                             showLabel: false,
-                            border: { width: 1, color: 'white' },
+                            border: { width: 1, color: 'white' }
                         },
                         paletteSettings: {
                             palette: [
@@ -3224,67 +2347,46 @@
                                 { color: '#c4956a' },
                                 { color: '#a8784c' },
                                 { color: '#8b5e3c' },
-                                { color: '#6d472c' },
+                                { color: '#6d472c' }
                             ],
-                            type: 'Gradient',
+                            type: 'Gradient'
                         },
                         legendSettings: {
-                            visible: false,
+                            visible: false
                         },
                         tooltipRender: function (args) {
 
-                            var dia =
-                                args.xLabel ||
-                                (diasSemana[args.xValue] ?? 'N/A');
-                            var hora =
-                                args.yLabel || (horas[args.yValue] ?? 'N/A');
-                            args.content = [
-                                dia +
-                                    ' às ' +
-                                    hora +
-                                    ': ' +
-                                    formatarMoeda(args.value),
-                            ];
+                            var dia = args.xLabel || (diasSemana[args.xValue] ?? 'N/A');
+                            var hora = args.yLabel || (horas[args.yValue] ?? 'N/A');
+                            args.content = [dia + ' às ' + hora + ': ' + formatarMoeda(args.value)];
                         },
                         cellClick: function (args) {
                             var diaSemana = args.xValue;
                             var hora = parseInt(horas[args.yValue]);
                             abrirModalDetalhes({
-                                titulo:
-                                    'Abastecimentos - ' +
-                                    diasSemana[diaSemana] +
-                                    ' às ' +
-                                    horas[args.yValue],
+                                titulo: 'Abastecimentos - ' + diasSemana[diaSemana] + ' às ' + horas[args.yValue],
                                 ano: ano,
                                 mes: mes,
                                 diaSemana: diaSemana,
-                                hora: hora,
+                                hora: hora
                             });
-                        },
+                        }
                     });
                     heatmapDiaHora.appendTo('#heatmapDiaHora');
 
                     criarLegendaHeatmap('legendaHeatmapDiaHora', heatmapData);
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'renderizarHeatmapDiaHora.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapDiaHora.success", error);
                 }
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao carregar mapa de calor:', error);
-                container.innerHTML =
-                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarHeatmapDiaHora',
-            error,
-        );
+                container.innerHTML = '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapDiaHora", error);
     }
 }
 
@@ -3306,8 +2408,7 @@
                     container.innerHTML = '';
 
                     if (!data.xLabels || data.xLabels.length === 0) {
-                        container.innerHTML =
-                            '<div class="text-center text-muted py-4">Sem dados de categorias</div>';
+                        container.innerHTML = '<div class="text-center text-muted py-4">Sem dados de categorias</div>';
                         return;
                     }
 
@@ -3318,10 +2419,7 @@
                     for (var i = 0; i < categorias.length; i++) {
                         var row = [];
                         for (var j = 0; j < meses.length; j++) {
-                            var item = data.data.find(
-                                (d) =>
-                                    d.x === categorias[i] && d.y === meses[j],
-                            );
+                            var item = data.data.find(d => d.x === categorias[i] && d.y === meses[j]);
                             row.push(item ? item.value : 0);
                         }
                         heatmapData.push(row);
@@ -3331,16 +2429,16 @@
                         titleSettings: { text: '' },
                         xAxis: {
                             labels: categorias,
-                            textStyle: { size: '10px', fontFamily: 'Outfit' },
+                            textStyle: { size: '10px', fontFamily: 'Outfit' }
                         },
                         yAxis: {
                             labels: meses,
-                            textStyle: { size: '10px', fontFamily: 'Outfit' },
+                            textStyle: { size: '10px', fontFamily: 'Outfit' }
                         },
                         dataSource: heatmapData,
                         cellSettings: {
                             showLabel: false,
-                            border: { width: 1, color: 'white' },
+                            border: { width: 1, color: 'white' }
                         },
                         paletteSettings: {
                             palette: [
@@ -3349,69 +2447,47 @@
                                 { color: '#c4956a' },
                                 { color: '#a8784c' },
                                 { color: '#8b5e3c' },
-                                { color: '#6d472c' },
+                                { color: '#6d472c' }
                             ],
-                            type: 'Gradient',
+                            type: 'Gradient'
                         },
                         legendSettings: {
-                            visible: false,
+                            visible: false
                         },
                         tooltipRender: function (args) {
 
-                            var cat =
-                                args.xLabel ||
-                                (categorias[args.xValue] ?? 'N/A');
-                            var mes =
-                                args.yLabel || (meses[args.yValue] ?? 'N/A');
-                            args.content = [
-                                cat +
-                                    ' - ' +
-                                    mes +
-                                    ': ' +
-                                    formatarMoeda(args.value),
-                            ];
+                            var cat = args.xLabel || (categorias[args.xValue] ?? 'N/A');
+                            var mes = args.yLabel || (meses[args.yValue] ?? 'N/A');
+                            args.content = [cat + ' - ' + mes + ': ' + formatarMoeda(args.value)];
                         },
                         cellClick: function (args) {
-                            var categoria =
-                                args.xLabel || categorias[args.xValue];
+                            var categoria = args.xLabel || categorias[args.xValue];
                             var mesIdx = args.yValue;
                             var mesNum = mesIdx + 1;
                             var mesNome = args.yLabel || meses[mesIdx];
                             abrirModalDetalhes({
-                                titulo:
-                                    'Abastecimentos - ' +
-                                    categoria +
-                                    ' em ' +
-                                    mesNome,
+                                titulo: 'Abastecimentos - ' + categoria + ' em ' + mesNome,
                                 ano: ano,
                                 mes: mesNum,
-                                categoria: categoria,
+                                categoria: categoria
                             });
-                        },
+                        }
                     });
                     heatmapCategoria.appendTo('#heatmapCategoria');
 
                     criarLegendaHeatmap('legendaHeatmapCategoria', heatmapData);
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'renderizarHeatmapCategoria.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapCategoria.success", error);
                 }
             },
             error: function (xhr, status, error) {
                 console.error('Erro ao carregar mapa de calor:', error);
-                container.innerHTML =
-                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarHeatmapCategoria',
-            error,
-        );
+                container.innerHTML = '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapCategoria", error);
     }
 }
 
@@ -3419,9 +2495,7 @@
     try {
         const container = document.getElementById('heatmapVeiculo');
         const containerVazio = document.getElementById('heatmapVeiculoVazio');
-        const legendaContainer = document.getElementById(
-            'legendaHeatmapVeiculo',
-        );
+        const legendaContainer = document.getElementById('legendaHeatmapVeiculo');
         if (!container) return;
 
         if (!placa && !tipoVeiculo) {
@@ -3437,11 +2511,7 @@
         $.ajax({
             url: '/api/abastecimento/DashboardHeatmapVeiculo',
             type: 'GET',
-            data: {
-                ano: ano || null,
-                placa: placa || null,
-                tipoVeiculo: tipoVeiculo || null,
-            },
+            data: { ano: ano || null, placa: placa || null, tipoVeiculo: tipoVeiculo || null },
             success: function (data) {
                 try {
                     if (heatmapVeiculo) {
@@ -3450,14 +2520,8 @@
                     }
                     container.innerHTML = '';
 
-                    if (
-                        !data.xLabels ||
-                        data.xLabels.length === 0 ||
-                        !data.data ||
-                        data.data.length === 0
-                    ) {
-                        container.innerHTML =
-                            '<div class="text-center text-muted py-4">Sem dados de abastecimento para este veículo</div>';
+                    if (!data.xLabels || data.xLabels.length === 0 || !data.data || data.data.length === 0) {
+                        container.innerHTML = '<div class="text-center text-muted py-4">Sem dados de abastecimento para este veículo</div>';
                         return;
                     }
 
@@ -3468,10 +2532,7 @@
                     for (var i = 0; i < diasSemana.length; i++) {
                         var row = [];
                         for (var j = 0; j < horas.length; j++) {
-                            var item = data.data.find(
-                                (d) =>
-                                    d.x === diasSemana[i] && d.y === horas[j],
-                            );
+                            var item = data.data.find(d => d.x === diasSemana[i] && d.y === horas[j]);
                             row.push(item ? item.value : 0);
                         }
                         heatmapData.push(row);
@@ -3481,16 +2542,16 @@
                         titleSettings: { text: '' },
                         xAxis: {
                             labels: diasSemana,
-                            textStyle: { size: '11px', fontFamily: 'Outfit' },
+                            textStyle: { size: '11px', fontFamily: 'Outfit' }
                         },
                         yAxis: {
                             labels: horas,
-                            textStyle: { size: '9px', fontFamily: 'Outfit' },
+                            textStyle: { size: '9px', fontFamily: 'Outfit' }
                         },
                         dataSource: heatmapData,
                         cellSettings: {
                             showLabel: false,
-                            border: { width: 1, color: 'white' },
+                            border: { width: 1, color: 'white' }
                         },
                         paletteSettings: {
                             palette: [
@@ -3499,87 +2560,58 @@
                                 { color: '#c4956a' },
                                 { color: '#a8784c' },
                                 { color: '#8b5e3c' },
-                                { color: '#6d472c' },
+                                { color: '#6d472c' }
                             ],
-                            type: 'Gradient',
+                            type: 'Gradient'
                         },
                         legendSettings: {
-                            visible: false,
+                            visible: false
                         },
                         tooltipRender: function (args) {
 
-                            var dia =
-                                args.xLabel ||
-                                (diasSemana[args.xValue] ?? 'N/A');
-                            var hora =
-                                args.yLabel || (horas[args.yValue] ?? 'N/A');
-                            args.content = [
-                                dia +
-                                    ' às ' +
-                                    hora +
-                                    ': ' +
-                                    formatarMoeda(args.value),
-                            ];
+                            var dia = args.xLabel || (diasSemana[args.xValue] ?? 'N/A');
+                            var hora = args.yLabel || (horas[args.yValue] ?? 'N/A');
+                            args.content = [dia + ' às ' + hora + ': ' + formatarMoeda(args.value)];
                         },
                         cellClick: function (args) {
                             var diaSemana = args.xValue;
                             var hora = parseInt(horas[args.yValue]);
                             var filtroLabel = placa || tipoVeiculo || 'Veículo';
                             abrirModalDetalhes({
-                                titulo:
-                                    'Abastecimentos - ' +
-                                    filtroLabel +
-                                    ' - ' +
-                                    diasSemana[diaSemana] +
-                                    ' às ' +
-                                    horas[args.yValue],
+                                titulo: 'Abastecimentos - ' + filtroLabel + ' - ' + diasSemana[diaSemana] + ' às ' + horas[args.yValue],
                                 ano: ano,
                                 placa: placa || null,
                                 tipoVeiculo: tipoVeiculo || null,
                                 diaSemana: diaSemana,
-                                hora: hora,
+                                hora: hora
                             });
-                        },
+                        }
                     });
                     heatmapVeiculo.appendTo('#heatmapVeiculo');
 
                     criarLegendaHeatmap('legendaHeatmapVeiculo', heatmapData);
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'renderizarHeatmapVeiculo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapVeiculo.success", error);
                 }
             },
             error: function (xhr, status, error) {
-                console.error(
-                    'Erro ao carregar mapa de calor do veículo:',
-                    error,
-                );
-                container.innerHTML =
-                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'renderizarHeatmapVeiculo',
-            error,
-        );
+                console.error('Erro ao carregar mapa de calor do veículo:', error);
+                container.innerHTML = '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "renderizarHeatmapVeiculo", error);
     }
 }
 
 function abrirModalDetalhes(filtros) {
     try {
         if (!modalDetalhes) {
-            modalDetalhes = new bootstrap.Modal(
-                document.getElementById('modalDetalhesAbast'),
-            );
-        }
-
-        document.getElementById('modalDetalhesTitulo').textContent =
-            filtros.titulo || 'Detalhes dos Abastecimentos';
+            modalDetalhes = new bootstrap.Modal(document.getElementById('modalDetalhesAbast'));
+        }
+
+        document.getElementById('modalDetalhesTitulo').textContent = filtros.titulo || 'Detalhes dos Abastecimentos';
         document.getElementById('detalhesGrid').innerHTML = `
             <div class="detalhes-grid-header">Data</div>
             <div class="detalhes-grid-header">Veículo</div>
@@ -3606,18 +2638,14 @@
                 categoria: filtros.categoria || null,
                 tipoVeiculo: filtros.tipoVeiculo || null,
                 placa: filtros.placa || null,
-                diaSemana:
-                    filtros.diaSemana !== undefined ? filtros.diaSemana : null,
-                hora: filtros.hora !== undefined ? filtros.hora : null,
+                diaSemana: filtros.diaSemana !== undefined ? filtros.diaSemana : null,
+                hora: filtros.hora !== undefined ? filtros.hora : null
             },
             success: function (data) {
                 try {
-                    document.getElementById('detalhesQtd').textContent =
-                        data.totais.quantidade.toLocaleString('pt-BR');
-                    document.getElementById('detalhesLitros').textContent =
-                        formatarNumero(data.totais.litros) + ' L';
-                    document.getElementById('detalhesValor').textContent =
-                        formatarMoeda(data.totais.valor);
+                    document.getElementById('detalhesQtd').textContent = data.totais.quantidade.toLocaleString('pt-BR');
+                    document.getElementById('detalhesLitros').textContent = formatarNumero(data.totais.litros) + ' L';
+                    document.getElementById('detalhesValor').textContent = formatarMoeda(data.totais.valor);
 
                     var gridHtml = `
                         <div class="detalhes-grid-header">Data</div>
@@ -3637,21 +2665,15 @@
                                 <div class="detalhes-grid-cell">${formatarMoeda(reg.valorTotal)}</div>
                             `;
                         });
-                        document.getElementById('detalhesVazio').style.display =
-                            'none';
+                        document.getElementById('detalhesVazio').style.display = 'none';
                     } else {
-                        document.getElementById('detalhesVazio').style.display =
-                            'block';
-                    }
-
-                    document.getElementById('detalhesGrid').innerHTML =
-                        gridHtml;
+                        document.getElementById('detalhesVazio').style.display = 'block';
+                    }
+
+                    document.getElementById('detalhesGrid').innerHTML = gridHtml;
+
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'abrirModalDetalhes.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalDetalhes.success", error);
                 }
             },
             error: function (xhr, status, error) {
@@ -3663,29 +2685,20 @@
                     <div class="detalhes-grid-header">R$/Litro</div>
                     <div class="detalhes-grid-header">Total</div>
                 `;
-                document.getElementById('detalhesVazio').style.display =
-                    'block';
-                document.getElementById('detalhesVazio').innerHTML =
-                    '<i class="fa-duotone fa-circle-exclamation fa-2x mb-2"></i><div>Erro ao carregar detalhes</div>';
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'abrirModalDetalhes',
-            error,
-        );
+                document.getElementById('detalhesVazio').style.display = 'block';
+                document.getElementById('detalhesVazio').innerHTML = '<i class="fa-duotone fa-circle-exclamation fa-2x mb-2"></i><div>Erro ao carregar detalhes</div>';
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalDetalhes", error);
     }
 }
 
 function abrirModalViagensVeiculo(veiculoId, placa) {
     try {
 
-        const ano =
-            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
-            new Date().getFullYear();
-        const mes =
-            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
+        const ano = parseInt(document.getElementById('filtroAnoMensal')?.value) || new Date().getFullYear();
+        const mes = parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
 
         const corpoTabela = document.getElementById('corpoTabelaViagens');
         if (corpoTabela) {
@@ -3699,14 +2712,10 @@
             `;
         }
 
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalViagensVeiculo'),
-        );
+        const modal = new bootstrap.Modal(document.getElementById('modalViagensVeiculo'));
         modal.show();
 
-        const corpoTabelaAbastecimentos = document.getElementById(
-            'corpoTabelaAbastecimentos',
-        );
+        const corpoTabelaAbastecimentos = document.getElementById('corpoTabelaAbastecimentos');
 
         $.ajax({
             url: '/api/abastecimento/DashboardViagensVeiculo',
@@ -3715,57 +2724,28 @@
             success: function (data) {
                 try {
 
-                    document.getElementById('modalVeiculoPlaca').textContent =
-                        data.veiculo?.placa || placa || '---';
-                    document.getElementById('modalVeiculoModelo').textContent =
-                        data.veiculo?.marcaModelo || '---';
-                    document.getElementById(
-                        'modalVeiculoCategoria',
-                    ).textContent = data.veiculo?.categoria || '---';
-                    document.getElementById('modalPeriodo').textContent =
-                        data.periodo?.descricao || '---';
-
-                    document.getElementById('modalTotalViagens').textContent =
-                        data.totais?.totalViagens || 0;
-                    document.getElementById('modalTotalKm').textContent =
-                        formatarNumeroInteiro(data.totais?.totalKmRodado || 0) +
-                        ' km';
-                    document.getElementById(
-                        'modalTotalAbastecimentos',
-                    ).textContent = data.totais?.totalAbastecimentos || 0;
-                    document.getElementById('modalTotalLitros').textContent =
-                        formatarNumero(data.totais?.totalLitros || 0);
-                    document.getElementById('modalTotalValor').textContent =
-                        formatarMoeda(
-                            data.totais?.totalValorAbastecimentos || 0,
-                        );
-
-                    document.getElementById('badgeViagens').textContent =
-                        data.totais?.totalViagens || 0;
-                    document.getElementById('badgeAbastecimentos').textContent =
-                        data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('modalVeiculoPlaca').textContent = data.veiculo?.placa || placa || '---';
+                    document.getElementById('modalVeiculoModelo').textContent = data.veiculo?.marcaModelo || '---';
+                    document.getElementById('modalVeiculoCategoria').textContent = data.veiculo?.categoria || '---';
+                    document.getElementById('modalPeriodo').textContent = data.periodo?.descricao || '---';
+
+                    document.getElementById('modalTotalViagens').textContent = data.totais?.totalViagens || 0;
+                    document.getElementById('modalTotalKm').textContent = formatarNumeroInteiro(data.totais?.totalKmRodado || 0) + ' km';
+                    document.getElementById('modalTotalAbastecimentos').textContent = data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('modalTotalLitros').textContent = formatarNumero(data.totais?.totalLitros || 0);
+                    document.getElementById('modalTotalValor').textContent = formatarMoeda(data.totais?.totalValorAbastecimentos || 0);
+
+                    document.getElementById('badgeViagens').textContent = data.totais?.totalViagens || 0;
+                    document.getElementById('badgeAbastecimentos').textContent = data.totais?.totalAbastecimentos || 0;
 
                     if (data.viagens && data.viagens.length > 0) {
                         let html = '';
                         data.viagens.forEach(function (v) {
-                            const dataInicial = v.dataInicial
-                                ? formatarData(v.dataInicial)
-                                : '-';
-                            const horaInicio = v.horaInicio
-                                ? formatarHora(v.horaInicio)
-                                : '-';
-                            const dataFinal = v.dataFinal
-                                ? formatarData(v.dataFinal)
-                                : '-';
-                            const horaFim = v.horaFim
-                                ? formatarHora(v.horaFim)
-                                : '-';
-                            const kmRodadoClass =
-                                v.kmRodado > 500
-                                    ? 'text-danger fw-bold'
-                                    : v.kmRodado > 200
-                                      ? 'text-warning fw-bold'
-                                      : '';
+                            const dataInicial = v.dataInicial ? formatarData(v.dataInicial) : '-';
+                            const horaInicio = v.horaInicio ? formatarHora(v.horaInicio) : '-';
+                            const dataFinal = v.dataFinal ? formatarData(v.dataFinal) : '-';
+                            const horaFim = v.horaFim ? formatarHora(v.horaFim) : '-';
+                            const kmRodadoClass = v.kmRodado > 500 ? 'text-danger fw-bold' : (v.kmRodado > 200 ? 'text-warning fw-bold' : '');
 
                             html += `
                                 <tr>
@@ -3795,9 +2775,7 @@
                     if (data.abastecimentos && data.abastecimentos.length > 0) {
                         let htmlAbast = '';
                         data.abastecimentos.forEach(function (a) {
-                            const dataAbast = a.data
-                                ? formatarData(a.data)
-                                : '-';
+                            const dataAbast = a.data ? formatarData(a.data) : '-';
                             htmlAbast += `
                                 <tr>
                                     <td style="padding: 10px 15px;">${dataAbast}</td>
@@ -3821,11 +2799,7 @@
                         `;
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'abrirModalViagensVeiculo.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalViagensVeiculo.success", error);
                 }
             },
             error: function (xhr, status, error) {
@@ -3846,32 +2820,21 @@
                         </td>
                     </tr>
                 `;
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'abrirModalViagensVeiculo',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalViagensVeiculo", error);
     }
 }
 
 function abrirModalAbastecimentosUnidade(unidade) {
     try {
 
-        const ano =
-            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
-            new Date().getFullYear();
-        const mes =
-            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
-
-        const corpoTabelaAbast = document.getElementById(
-            'corpoTabelaAbastecimentosUnidade',
-        );
-        const corpoTabelaResumo = document.getElementById(
-            'corpoTabelaResumoVeiculos',
-        );
+        const ano = parseInt(document.getElementById('filtroAnoMensal')?.value) || new Date().getFullYear();
+        const mes = parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
+
+        const corpoTabelaAbast = document.getElementById('corpoTabelaAbastecimentosUnidade');
+        const corpoTabelaResumo = document.getElementById('corpoTabelaResumoVeiculos');
 
         if (corpoTabelaAbast) {
             corpoTabelaAbast.innerHTML = `
@@ -3894,9 +2857,7 @@
             `;
         }
 
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalAbastecimentosUnidade'),
-        );
+        const modal = new bootstrap.Modal(document.getElementById('modalAbastecimentosUnidade'));
         modal.show();
 
         $.ajax({
@@ -3906,35 +2867,20 @@
             success: function (data) {
                 try {
 
-                    document.getElementById('modalUnidadeNome').textContent =
-                        data.unidade || unidade || '---';
-                    document.getElementById('modalUnidadePeriodo').textContent =
-                        data.periodo?.descricao || '---';
-
-                    document.getElementById(
-                        'modalUnidadeTotalAbastecimentos',
-                    ).textContent = data.totais?.totalAbastecimentos || 0;
-                    document.getElementById(
-                        'modalUnidadeTotalLitros',
-                    ).textContent = formatarNumero(
-                        data.totais?.totalLitros || 0,
-                    );
-                    document.getElementById(
-                        'modalUnidadeTotalValor',
-                    ).textContent = formatarMoeda(data.totais?.totalValor || 0);
-
-                    document.getElementById(
-                        'badgeAbastecimentosUnidade',
-                    ).textContent = data.totais?.totalAbastecimentos || 0;
-                    document.getElementById('badgeResumoVeiculos').textContent =
-                        data.porVeiculo?.length || 0;
+                    document.getElementById('modalUnidadeNome').textContent = data.unidade || unidade || '---';
+                    document.getElementById('modalUnidadePeriodo').textContent = data.periodo?.descricao || '---';
+
+                    document.getElementById('modalUnidadeTotalAbastecimentos').textContent = data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('modalUnidadeTotalLitros').textContent = formatarNumero(data.totais?.totalLitros || 0);
+                    document.getElementById('modalUnidadeTotalValor').textContent = formatarMoeda(data.totais?.totalValor || 0);
+
+                    document.getElementById('badgeAbastecimentosUnidade').textContent = data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('badgeResumoVeiculos').textContent = data.porVeiculo?.length || 0;
 
                     if (data.abastecimentos && data.abastecimentos.length > 0) {
                         let html = '';
                         data.abastecimentos.forEach(function (a) {
-                            const dataAbast = a.data
-                                ? formatarData(a.data)
-                                : '-';
+                            const dataAbast = a.data ? formatarData(a.data) : '-';
                             html += `
                                 <tr>
                                     <td style="padding: 10px 15px;">${dataAbast}</td>
@@ -3984,11 +2930,7 @@
                         `;
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'abrirModalAbastecimentosUnidade.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalAbastecimentosUnidade.success", error);
                 }
             },
             error: function (xhr, status, error) {
@@ -4009,32 +2951,21 @@
                         </td>
                     </tr>
                 `;
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'abrirModalAbastecimentosUnidade',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalAbastecimentosUnidade", error);
     }
 }
 
 function abrirModalAbastecimentosCategoria(categoria) {
     try {
 
-        const ano =
-            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
-            new Date().getFullYear();
-        const mes =
-            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
-
-        const corpoTabelaVeiculos = document.getElementById(
-            'corpoTabelaVeiculosCategoria',
-        );
-        const corpoTabelaAbast = document.getElementById(
-            'corpoTabelaAbastecimentosCategoria',
-        );
+        const ano = parseInt(document.getElementById('filtroAnoMensal')?.value) || new Date().getFullYear();
+        const mes = parseInt(document.getElementById('filtroMesMensal')?.value) || 0;
+
+        const corpoTabelaVeiculos = document.getElementById('corpoTabelaVeiculosCategoria');
+        const corpoTabelaAbast = document.getElementById('corpoTabelaAbastecimentosCategoria');
 
         if (corpoTabelaVeiculos) {
             corpoTabelaVeiculos.innerHTML = `
@@ -4057,9 +2988,7 @@
             `;
         }
 
-        const modal = new bootstrap.Modal(
-            document.getElementById('modalAbastecimentosCategoria'),
-        );
+        const modal = new bootstrap.Modal(document.getElementById('modalAbastecimentosCategoria'));
         modal.show();
 
         $.ajax({
@@ -4069,33 +2998,16 @@
             success: function (data) {
                 try {
 
-                    document.getElementById('modalCategoriaNome').textContent =
-                        data.categoria || categoria || '---';
-                    document.getElementById(
-                        'modalCategoriaPeriodo',
-                    ).textContent = data.periodo?.descricao || '---';
-
-                    document.getElementById(
-                        'modalCategoriaTotalVeiculos',
-                    ).textContent = data.totais?.totalVeiculos || 0;
-                    document.getElementById(
-                        'modalCategoriaTotalAbastecimentos',
-                    ).textContent = data.totais?.totalAbastecimentos || 0;
-                    document.getElementById(
-                        'modalCategoriaTotalLitros',
-                    ).textContent = formatarNumero(
-                        data.totais?.totalLitros || 0,
-                    );
-                    document.getElementById(
-                        'modalCategoriaTotalValor',
-                    ).textContent = formatarMoeda(data.totais?.totalValor || 0);
-
-                    document.getElementById(
-                        'badgeVeiculosCategoria',
-                    ).textContent = data.porVeiculo?.length || 0;
-                    document.getElementById(
-                        'badgeAbastecimentosCategoria',
-                    ).textContent = data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('modalCategoriaNome').textContent = data.categoria || categoria || '---';
+                    document.getElementById('modalCategoriaPeriodo').textContent = data.periodo?.descricao || '---';
+
+                    document.getElementById('modalCategoriaTotalVeiculos').textContent = data.totais?.totalVeiculos || 0;
+                    document.getElementById('modalCategoriaTotalAbastecimentos').textContent = data.totais?.totalAbastecimentos || 0;
+                    document.getElementById('modalCategoriaTotalLitros').textContent = formatarNumero(data.totais?.totalLitros || 0);
+                    document.getElementById('modalCategoriaTotalValor').textContent = formatarMoeda(data.totais?.totalValor || 0);
+
+                    document.getElementById('badgeVeiculosCategoria').textContent = data.porVeiculo?.length || 0;
+                    document.getElementById('badgeAbastecimentosCategoria').textContent = data.totais?.totalAbastecimentos || 0;
 
                     if (data.porVeiculo && data.porVeiculo.length > 0) {
                         let htmlVeiculos = '';
@@ -4125,9 +3037,7 @@
                     if (data.abastecimentos && data.abastecimentos.length > 0) {
                         let htmlAbast = '';
                         data.abastecimentos.forEach(function (a) {
-                            const dataAbast = a.data
-                                ? formatarData(a.data)
-                                : '-';
+                            const dataAbast = a.data ? formatarData(a.data) : '-';
                             htmlAbast += `
                                 <tr>
                                     <td style="padding: 10px 15px;">${dataAbast}</td>
@@ -4152,11 +3062,7 @@
                         `;
                     }
                 } catch (error) {
-                    Alerta.TratamentoErroComLinha(
-                        'dashboard-abastecimento.js',
-                        'abrirModalAbastecimentosCategoria.success',
-                        error,
-                    );
+                    Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalAbastecimentosCategoria.success", error);
                 }
             },
             error: function (xhr, status, error) {
@@ -4177,14 +3083,10 @@
                         </td>
                     </tr>
                 `;
-            },
-        });
-    } catch (error) {
-        Alerta.TratamentoErroComLinha(
-            'dashboard-abastecimento.js',
-            'abrirModalAbastecimentosCategoria',
-            error,
-        );
+            }
+        });
+    } catch (error) {
+        Alerta.TratamentoErroComLinha("dashboard-abastecimento.js", "abrirModalAbastecimentosCategoria", error);
     }
 }
 
```
