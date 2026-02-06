/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                      SOLUÇÃO FROTIX - GESTÃO DE FROTAS                   ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ 📄 ARQUIVO: dashboard-abastecimento.js                                  ║
 * ║ 📍 LOCAL: wwwroot/js/dashboards/                                        ║
 * ║ 📋 VERSÃO: 2.0                                                           ║
 * ║ 📅 ATUALIZAÇÃO: 23/01/2026                                               ║
 * ╠══════════════════════════════════════════════════════════════════════════╣
 * ║ ❓ POR QUE EXISTO?                                                       ║
 * ║    Dashboard analítico de Abastecimentos com Syncfusion Charts.          ║
 * ║    • KPIs de consumo, custo e eficiência                                 ║
 * ║    • Gráficos de evolução mensal                                         ║
 * ║    • Top veículos e motoristas por consumo                               ║
 * ║    • Análise por tipo de combustível                                     ║
 * ║    Paleta: Âmbar/Dourado                                                 ║
 * ║                                                                          ║
 * ║ 🔗 RELEVÂNCIA: Alta (Dashboard Gerencial)                                ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

// ====== CONFIGURAÇÃO CLDR PARA SYNCFUSION CHARTS ======
// Carrega dados CLDR de forma síncrona para evitar erro "percentSign undefined"
(function initCldrForCharts() {
    try {
        // Verifica se ej.base (global) existe - usado pelos scripts do CDN
        if (typeof ej !== 'undefined' && ej.base) {
            // Dados CLDR mínimos necessários para formatação de números pt-BR
            const numberingSystemsData = {
                supplemental: {
                    numberingSystems: {
                        latn: { _digits: '0123456789', _type: 'numeric' },
                    },
                },
            };

            const numbersData = {
                main: {
                    'pt-BR': {
                        numbers: {
                            defaultNumberingSystem: 'latn',
                            minimumGroupingDigits: '1',
                            'symbols-numberSystem-latn': {
                                decimal: ',',
                                group: '.',
                                list: ';',
                                percentSign: '%',
                                plusSign: '+',
                                minusSign: '-',
                                exponential: 'E',
                                superscriptingExponent: '×',
                                perMille: '‰',
                                infinity: '∞',
                                nan: 'NaN',
                                timeSeparator: ':',
                            },
                            'decimalFormats-numberSystem-latn': {
                                standard: '#,##0.###',
                            },
                            'percentFormats-numberSystem-latn': {
                                standard: '#,##0%',
                            },
                            'currencyFormats-numberSystem-latn': {
                                standard: '¤ #,##0.00',
                            },
                        },
                    },
                },
            };

            const currenciesData = {
                main: {
                    'pt-BR': {
                        numbers: {
                            currencies: {
                                BRL: {
                                    displayName: 'Real brasileiro',
                                    symbol: 'R$',
                                },
                            },
                        },
                    },
                },
            };

            // Carrega os dados CLDR
            ej.base.loadCldr(numberingSystemsData, numbersData, currenciesData);
            ej.base.setCulture('pt-BR');
            ej.base.setCurrencyCode('BRL');
            console.log('✅ CLDR pt-BR carregado de forma síncrona');
        }
    } catch (e) {
        console.warn('⚠️ Erro ao carregar CLDR para charts:', e);
    }
})();

// ====== CLASSE NO BODY PARA ESTILIZAÇÃO DE TOOLTIPS ======
// Adiciona classe ao body para permitir CSS específico do dashboard
(function addDashboardBodyClass() {
    document.body.classList.add('dashboard-abastecimento');
})();

// ====== VARIÁVEIS GLOBAIS ======
let dadosGerais = null;
let dadosMensais = null;
let dadosVeiculo = null;
let modalLoading = null;
let ignorarMudancaPlacaVeiculo = false;

// Instâncias dos gráficos
let chartValorCategoria = null;
let chartValorLitro = null;
let chartLitrosMes = null;
let chartConsumoMes = null;
let chartPizzaCombustivel = null;
let chartLitrosDia = null;
let chartConsumoCategoria = null;
let chartConsumoMensalVeiculo = null;
let chartValorMensalVeiculo = null;
let chartRankingVeiculos = null;

// Nomes dos meses
const MESES = [
    '',
    'jan',
    'fev',
    'mar',
    'abr',
    'mai',
    'jun',
    'jul',
    'ago',
    'set',
    'out',
    'nov',
    'dez',
];
const MESES_COMPLETOS = [
    '',
    'janeiro',
    'fevereiro',
    'março',
    'abril',
    'maio',
    'junho',
    'julho',
    'agosto',
    'setembro',
    'outubro',
    'novembro',
    'dezembro',
];

// Paleta de cores CARAMELO SUAVE
const CORES = {
    amber: ['#a8784c', '#8b5e3c', '#6d472c', '#5a3a24', '#4a2f1d'],
    gold: ['#d4a574', '#c4956a', '#a8784c', '#9a7045', '#8b6340'],
    warm: ['#c49a6c', '#b8916a', '#a88565', '#9a785a', '#8c6c50'],
    multi: [
        '#a8784c',
        '#c4956a',
        '#d4a574',
        '#b8916a',
        '#8b5e3c',
        '#9a7045',
        '#6d472c',
        '#8b6340',
    ],
    categorias: [
        '#a8784c',
        '#c49a6c',
        '#9a7045',
        '#8b5e3c',
        '#c4956a',
        '#5a3a24',
        '#6d472c',
        '#8b6340',
        '#d4a574',
        '#4a2f1d',
    ],
};

// ====== OVERLAY DE LOADING - Padrão FrotiX ======
function mostrarLoading() {
    const el = document.getElementById('loadingOverlayAbast');
    if (el) {
        el.style.display = 'flex';
    }
}

function esconderLoading() {
    const el = document.getElementById('loadingOverlayAbast');
    if (el) {
        el.style.display = 'none';
    }
}

function exibirToastAmareloAposLoading(mensagens, duracaoMs = 4000) {
    try {
        const lista = Array.isArray(mensagens)
            ? mensagens.filter(Boolean)
            : [mensagens].filter(Boolean);
        if (lista.length === 0) return;

        setTimeout(() => {
            lista.forEach((mensagem, idx) => {
                setTimeout(() => {
                    AppToast.show('orange', mensagem, duracaoMs);
                }, idx * 400);
            });
        }, 600);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'exibirToastAmareloAposLoading',
            error,
        );
    }
}

function obterPlacaTextoCompleto(textoCompleto) {
    if (!textoCompleto) return '';
    let placa = textoCompleto;
    if (placa.includes(' - ')) {
        placa = placa.split(' - ')[0].trim();
    } else if (placa.includes(' (')) {
        placa = placa.split(' (')[0].trim();
    }
    return placa === 'Todas' ? '' : placa;
}

// ====== INICIALIZAÇÃO ======
document.addEventListener('DOMContentLoaded', function () {
    try {
        inicializarTabs();
        // Primeiro busca anos disponíveis, depois carrega com filtros
        inicializarFiltrosECarregar();
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'DOMContentLoaded',
            error,
        );
    }
});

/**
 * Inicializa os filtros buscando anos disponíveis e deixa mês vazio até usuário selecionar ano
 * Lógica: Popula anos disponíveis → Usuário seleciona ano → Popula meses daquele ano
 */
function inicializarFiltrosECarregar() {
    try {
        mostrarLoading();

        // Busca para obter anos disponíveis
        $.ajax({
            url: '/api/abastecimento/DashboardDados',
            type: 'GET',
            data: { ano: null, mes: null },
            success: function (data) {
                try {
                    const anos = data.anosDisponiveis || [];

                    if (anos.length === 0) {
                        // Sem dados disponíveis
                        esconderLoading();
                        return;
                    }

                    // Preencher selects de ano
                    const selectGeral =
                        document.getElementById('filtroAnoGeral');
                    const selectMensal =
                        document.getElementById('filtroAnoMensal');
                    const selectVeiculo =
                        document.getElementById('filtroAnoVeiculo');

                    [selectGeral, selectMensal, selectVeiculo].forEach(
                        (select) => {
                            if (!select) return;
                            const isGeral = select.id === 'filtroAnoGeral';
                            select.innerHTML = isGeral
                                ? '<option value="">&lt;Todos os Anos&gt;</option>'
                                : '<option value="">Selecione o Ano</option>';
                            anos.forEach((ano) => {
                                const option = document.createElement('option');
                                option.value = ano;
                                option.textContent = ano;
                                select.appendChild(option);
                            });
                            select.dataset.initialized = 'true';

                            // Adicionar evento para popular meses quando ano for selecionado
                            if (!select.dataset.eventAdded) {
                                select.addEventListener('change', function () {
                                    const anoSelecionado = this.value;
                                    const mesSelectId = this.id.replace(
                                        'Ano',
                                        'Mes',
                                    );
                                    const mesSelect =
                                        document.getElementById(mesSelectId);

                                    if (anoSelecionado && mesSelect) {
                                        popularMesesDoAno(
                                            anoSelecionado,
                                            mesSelect,
                                        );
                                    } else if (mesSelect) {
                                        // Limpar dropdown de mês se ano for desmarcado
                                        mesSelect.innerHTML =
                                            '<option value="">&lt;Todos os Meses&gt;</option>';
                                    }
                                });
                                select.dataset.eventAdded = 'true';
                            }
                        },
                    );

                    // Posicionar dropdowns baseado no filtro aplicado pela API
                    const filtroAplicado = data.filtroAplicado || {};
                    if (filtroAplicado.ano > 0) {
                        // Selecionar o ano no dropdown Geral
                        if (selectGeral) {
                            selectGeral.value = filtroAplicado.ano.toString();
                        }

                        // Popular meses do ano selecionado
                        const mesSelectGeral =
                            document.getElementById('filtroMesGeral');
                        if (mesSelectGeral) {
                            popularMesesDoAno(
                                filtroAplicado.ano,
                                mesSelectGeral,
                                function () {
                                    // Callback: Após popular meses, selecionar o mês
                                    if (filtroAplicado.mes > 0) {
                                        mesSelectGeral.value =
                                            filtroAplicado.mes.toString();
                                    }
                                    // Carregar dados APÓS os filtros estarem prontos
                                    carregarDadosGeraisComFiltros();
                                },
                            );
                        } else {
                            // Se não há select de mês, carregar direto
                            carregarDadosGeraisComFiltros();
                        }
                    } else {
                        // Se não há filtro de ano aplicado, carregar direto
                        carregarDadosGeraisComFiltros();
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'inicializarFiltrosECarregar.success',
                        error,
                    );
                    esconderLoading();
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao inicializar filtros:', error);
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'inicializarFiltrosECarregar',
            error,
        );
        esconderLoading();
    }
}

/**
 * Popula o dropdown de mês com os meses disponíveis de um ano específico
 * @param {number} ano - Ano para buscar meses disponíveis
 * @param {HTMLElement} mesSelect - Elemento select de mês
 * @param {Function} callback - Função a ser executada após popular os meses (opcional)
 */
function popularMesesDoAno(ano, mesSelect, callback) {
    try {
        $.ajax({
            url: '/api/abastecimento/DashboardMesesDisponiveis',
            type: 'GET',
            data: { ano: ano },
            success: function (data) {
                const meses = data.meses || [];
                const nomesMeses = [
                    '',
                    'Janeiro',
                    'Fevereiro',
                    'Março',
                    'Abril',
                    'Maio',
                    'Junho',
                    'Julho',
                    'Agosto',
                    'Setembro',
                    'Outubro',
                    'Novembro',
                    'Dezembro',
                ];

                mesSelect.innerHTML =
                    '<option value="">&lt;Todos os Meses&gt;</option>';
                meses.forEach((mes) => {
                    const option = document.createElement('option');
                    option.value = mes;
                    option.textContent = nomesMeses[mes];
                    mesSelect.appendChild(option);
                });

                // Executar callback se fornecido
                if (typeof callback === 'function') {
                    callback();
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao buscar meses disponíveis:', error);
                mesSelect.innerHTML =
                    '<option value="">&lt;Todos os Meses&gt;</option>';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'popularMesesDoAno',
            error,
        );
    }
}

/**
 * Carrega dados gerais usando os filtros já selecionados (sem repreencher filtros)
 */
function carregarDadosGeraisComFiltros() {
    try {
        const ano = document.getElementById('filtroAnoGeral')?.value || '';
        const mes = document.getElementById('filtroMesGeral')?.value || '';

        // Atualiza a label do período com os filtros selecionados
        atualizarLabelPeriodoGeral();

        $.ajax({
            url: '/api/abastecimento/DashboardDados',
            type: 'GET',
            data: { ano: ano || null, mes: mes || null },
            success: function (data) {
                try {
                    dadosGerais = data;
                    renderizarAbaGeral(data);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'carregarDadosGeraisComFiltros.success',
                        error,
                    );
                } finally {
                    esconderLoading();
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados gerais:', error);
                AppToast.show(
                    'red',
                    'Erro ao carregar dados do dashboard',
                    5000,
                );
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosGeraisComFiltros',
            error,
        );
        esconderLoading();
    }
}

// ====== NAVEGAÇÃO DE ABAS ======
function inicializarTabs() {
    try {
        const tabs = document.querySelectorAll('.dash-tab');
        tabs.forEach((tab) => {
            tab.addEventListener('click', function () {
                const tabId = this.getAttribute('data-tab');
                const tabAtual = document
                    .querySelector('.dash-tab.active')
                    ?.getAttribute('data-tab');

                if (tabId === tabAtual) return;

                if (tabAtual === 'consumo-geral') destruirGraficosGeral();
                if (tabAtual === 'consumo-mensal') destruirGraficosMensal();
                if (tabAtual === 'consumo-veiculo') destruirGraficosVeiculo();

                tabs.forEach((t) => t.classList.remove('active'));
                this.classList.add('active');

                document
                    .querySelectorAll('.dash-content')
                    .forEach((c) => c.classList.remove('active'));

                const tabContent = document.getElementById('tab-' + tabId);
                if (tabContent) tabContent.classList.add('active');

                setTimeout(() => {
                    if (tabId === 'consumo-geral') {
                        carregarDadosGerais();
                    } else if (tabId === 'consumo-mensal') {
                        carregarDadosMensais();
                    } else if (tabId === 'consumo-veiculo') {
                        carregarDadosVeiculo();
                    }
                }, 100);
            });
        });

        // ====== ABA GERAL - Filtros ======
        document
            .getElementById('btnFiltrarAnoMesGeral')
            ?.addEventListener('click', function () {
                // Limpa período personalizado ao usar ano/mês
                document.getElementById('dataInicioGeral').value = '';
                document.getElementById('dataFimGeral').value = '';
                document
                    .querySelectorAll('.btn-period-abast')
                    .forEach((b) => b.classList.remove('active'));
                carregarDadosGerais();
            });

        document
            .getElementById('btnLimparAnoMesGeral')
            ?.addEventListener('click', function () {
                document.getElementById('filtroAnoGeral').value = '';
                document.getElementById('filtroMesGeral').value = '';
                atualizarLabelPeriodoGeral();
                carregarDadosGerais();
            });

        document
            .getElementById('btnFiltrarPeriodoGeral')
            ?.addEventListener('click', function () {
                const dataInicio =
                    document.getElementById('dataInicioGeral').value;
                const dataFim = document.getElementById('dataFimGeral').value;
                if (dataInicio && dataFim) {
                    // Limpa ano/mês ao usar período personalizado
                    document.getElementById('filtroAnoGeral').value = '';
                    document.getElementById('filtroMesGeral').value = '';
                    document
                        .querySelectorAll('.btn-period-abast')
                        .forEach((b) => b.classList.remove('active'));
                    carregarDadosGeraisPeriodo(dataInicio, dataFim);
                } else {
                    Alerta.Warning('Preencha as datas de início e fim');
                }
            });

        document
            .getElementById('btnLimparPeriodoGeral')
            ?.addEventListener('click', function () {
                document.getElementById('dataInicioGeral').value = '';
                document.getElementById('dataFimGeral').value = '';
                document
                    .querySelectorAll('.btn-period-abast')
                    .forEach((b) => b.classList.remove('active'));
                atualizarLabelPeriodoGeral();
                carregarDadosGerais();
            });

        // Períodos Rápidos
        document.querySelectorAll('.btn-period-abast').forEach((btn) => {
            btn.addEventListener('click', function () {
                const dias = parseInt(this.dataset.dias);
                const dataFim = new Date();
                const dataInicio = new Date();
                dataInicio.setDate(dataInicio.getDate() - dias);

                document.getElementById('dataInicioGeral').value = dataInicio
                    .toISOString()
                    .split('T')[0];
                document.getElementById('dataFimGeral').value = dataFim
                    .toISOString()
                    .split('T')[0];
                document.getElementById('filtroAnoGeral').value = '';
                document.getElementById('filtroMesGeral').value = '';

                document
                    .querySelectorAll('.btn-period-abast')
                    .forEach((b) => b.classList.remove('active'));
                this.classList.add('active');

                carregarDadosGeraisPeriodo(
                    document.getElementById('dataInicioGeral').value,
                    document.getElementById('dataFimGeral').value,
                );
            });
        });

        // ====== ABA MENSAL - Filtros ======
        document
            .getElementById('btnFiltrarMensal')
            ?.addEventListener('click', function () {
                carregarDadosMensais();
            });

        document
            .getElementById('btnLimparMensal')
            ?.addEventListener('click', function () {
                const selectAno = document.getElementById('filtroAnoMensal');
                if (selectAno && selectAno.options.length > 0) {
                    selectAno.selectedIndex = 0;
                }
                document.getElementById('filtroMesMensal').value = '';
                carregarDadosMensais();
            });

        // ====== ABA VEÍCULO - Filtros ======
        document
            .getElementById('btnLimparAnoMesVeiculo')
            ?.addEventListener('click', function () {
                try {
                    const selectAno =
                        document.getElementById('filtroAnoVeiculo');
                    if (selectAno && selectAno.options.length > 0) {
                        selectAno.selectedIndex = 0;
                    }
                    document.getElementById('filtroMesVeiculo').value = '';
                    prepararAtualizacaoVeiculoSemPlaca();
                    carregarDadosVeiculo(false);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'btnLimparAnoMesVeiculo.click',
                        error,
                    );
                }
            });

        document
            .getElementById('filtroAnoVeiculo')
            ?.addEventListener('change', function () {
                try {
                    document.getElementById('dataInicioVeiculo').value = '';
                    document.getElementById('dataFimVeiculo').value = '';
                    document.getElementById('filtroMesVeiculo').value = '';

                    // Guardar placa selecionada antes de limpar para verificar posteriormente
                    const $selectPlaca = $('#filtroPlacaVeiculo');
                    let placaSelecionadaAntes = '';
                    let textoPlacaAntes = '';

                    if (
                        $selectPlaca.length &&
                        $selectPlaca.hasClass('select2-hidden-accessible')
                    ) {
                        placaSelecionadaAntes = $selectPlaca.val() || '';
                        const select2Data = $selectPlaca.select2('data');
                        textoPlacaAntes =
                            select2Data && select2Data.length > 0
                                ? select2Data[0].text
                                : '';
                    } else {
                        const selectPlaca =
                            document.getElementById('filtroPlacaVeiculo');
                        placaSelecionadaAntes = selectPlaca?.value || '';
                        textoPlacaAntes =
                            selectPlaca?.options[selectPlaca.selectedIndex]
                                ?.text || '';
                    }

                    prepararAtualizacaoVeiculoSemPlaca();

                    // Se mudou para um ano válido, carregar dados para verificar placas disponíveis
                    if (this.value) {
                        carregarDadosVeiculo(
                            false,
                            placaSelecionadaAntes,
                            textoPlacaAntes,
                        );
                    } else {
                        carregarDadosVeiculo(
                            false,
                            placaSelecionadaAntes,
                            textoPlacaAntes,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'filtroAnoVeiculo.change',
                        error,
                    );
                }
            });

        document
            .getElementById('filtroMesVeiculo')
            ?.addEventListener('change', function () {
                try {
                    document.getElementById('dataInicioVeiculo').value = '';
                    document.getElementById('dataFimVeiculo').value = '';

                    // Guardar placa selecionada antes de limpar para verificar posteriormente
                    // Usar Select2 API se disponível, senão usar DOM nativo
                    const $selectPlaca = $('#filtroPlacaVeiculo');
                    let placaSelecionadaAntes = '';
                    let textoPlacaAntes = '';

                    if (
                        $selectPlaca.length &&
                        $selectPlaca.hasClass('select2-hidden-accessible')
                    ) {
                        // Select2 está ativo - usar sua API
                        placaSelecionadaAntes = $selectPlaca.val() || '';
                        const select2Data = $selectPlaca.select2('data');
                        textoPlacaAntes =
                            select2Data && select2Data.length > 0
                                ? select2Data[0].text
                                : '';
                    } else {
                        // Fallback para DOM nativo
                        const selectPlaca =
                            document.getElementById('filtroPlacaVeiculo');
                        placaSelecionadaAntes = selectPlaca?.value || '';
                        textoPlacaAntes =
                            selectPlaca?.options[selectPlaca.selectedIndex]
                                ?.text || '';
                    }

                    prepararAtualizacaoVeiculoSemPlaca();
                    if (this.value) {
                        carregarDadosVeiculo(
                            true,
                            placaSelecionadaAntes,
                            textoPlacaAntes,
                        );
                    } else {
                        carregarDadosVeiculo(
                            false,
                            placaSelecionadaAntes,
                            textoPlacaAntes,
                        );
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'filtroMesVeiculo.change',
                        error,
                    );
                }
            });

        document
            .getElementById('btnLimparPeriodoVeiculo')
            ?.addEventListener('click', function () {
                try {
                    document.getElementById('dataInicioVeiculo').value = '';
                    document.getElementById('dataFimVeiculo').value = '';
                    carregarDadosVeiculo();
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'btnLimparPeriodoVeiculo.click',
                        error,
                    );
                }
            });

        [
            document.getElementById('dataInicioVeiculo'),
            document.getElementById('dataFimVeiculo'),
        ].forEach((input) => {
            input?.addEventListener('change', function () {
                try {
                    const dataInicio =
                        document.getElementById('dataInicioVeiculo').value;
                    const dataFim =
                        document.getElementById('dataFimVeiculo').value;
                    if (dataInicio && dataFim && dataInicio > dataFim) {
                        document.getElementById('dataFimVeiculo').value = '';
                        prepararAtualizacaoVeiculoSemPlaca();
                        return;
                    }

                    if ((dataInicio && !dataFim) || (!dataInicio && dataFim)) {
                        prepararAtualizacaoVeiculoSemPlaca();
                        return;
                    }

                    document.getElementById('filtroAnoVeiculo').value = '';
                    document.getElementById('filtroMesVeiculo').value = '';

                    if (dataInicio && dataFim) {
                        carregarDadosVeiculoPeriodo(dataInicio, dataFim);
                    } else {
                        atualizarLabelPeriodoVeiculo();
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'dataInicioFimVeiculo.change',
                        error,
                    );
                }
            });
        });

        // Inicializar Select2 no filtro de placa para torná-lo pesquisável
        // A seleção de placa dispara o filtro imediatamente
        if (
            $('#filtroPlacaVeiculo').length &&
            typeof $.fn.select2 === 'function'
        ) {
            $('#filtroPlacaVeiculo').select2({
                placeholder: 'Digite para buscar...',
                allowClear: true,
                width: '260px',
                language: {
                    noResults: function () {
                        return 'Nenhuma placa encontrada';
                    },
                    searching: function () {
                        return 'Buscando...';
                    },
                    inputTooShort: function () {
                        return 'Digite para buscar';
                    },
                },
            });
        }

        $('#filtroPlacaVeiculo').on('change', function () {
            try {
                if (ignorarMudancaPlacaVeiculo) {
                    return;
                }
                aplicarFiltroPlacaVeiculo();
            } catch (error) {
                Alerta.TratamentoErroComLinha(
                    'dashboard-abastecimento.js',
                    'filtroPlacaVeiculo.change',
                    error,
                );
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'inicializarTabs',
            error,
        );
    }
}

// ====== FUNÇÕES AUXILIARES DE FILTRO ======

/**
 * Limpa o filtro de placa (compatível com Select2)
 */
function limparSelectPlacaVeiculo() {
    const $selectPlaca = $('#filtroPlacaVeiculo');
    if (
        $selectPlaca.length &&
        $selectPlaca.hasClass('select2-hidden-accessible')
    ) {
        $selectPlaca.val('').trigger('change');
    } else {
        document.getElementById('filtroPlacaVeiculo').value = '';
    }
}

function resetarOpcoesPlacaVeiculo() {
    try {
        const selectPlaca = document.getElementById('filtroPlacaVeiculo');
        const $selectPlaca = $('#filtroPlacaVeiculo');

        ignorarMudancaPlacaVeiculo = true;
        if (
            $selectPlaca.length &&
            $selectPlaca.hasClass('select2-hidden-accessible')
        ) {
            $selectPlaca.select2('destroy');
        }

        selectPlaca.innerHTML = '<option value="">Todas</option>';

        if ($selectPlaca.length && typeof $.fn.select2 === 'function') {
            $selectPlaca.select2({
                placeholder: 'Digite para buscar...',
                allowClear: true,
                width: '260px',
                language: {
                    noResults: function () {
                        return 'Nenhuma placa encontrada';
                    },
                    searching: function () {
                        return 'Buscando...';
                    },
                    inputTooShort: function () {
                        return 'Digite para buscar';
                    },
                },
            });
            $selectPlaca.trigger('change.select2');
        }
        ignorarMudancaPlacaVeiculo = false;
    } catch (error) {
        ignorarMudancaPlacaVeiculo = false;
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'resetarOpcoesPlacaVeiculo',
            error,
        );
    }
}

function limparResumoVeiculoSemSelecao() {
    try {
        renderizarAbaVeiculo({}, null, null);
        renderizarHeatmapVeiculo(null, null, null);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'limparResumoVeiculoSemSelecao',
            error,
        );
    }
}

function prepararAtualizacaoVeiculoSemPlaca() {
    try {
        ignorarMudancaPlacaVeiculo = true;
        resetarOpcoesPlacaVeiculo();
        limparSelectPlacaVeiculo();
        ignorarMudancaPlacaVeiculo = false;
        limparResumoVeiculoSemSelecao();
        atualizarLabelPeriodoVeiculo();
    } catch (error) {
        ignorarMudancaPlacaVeiculo = false;
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'prepararAtualizacaoVeiculoSemPlaca',
            error,
        );
    }
}

function aplicarFiltroPlacaVeiculo() {
    try {
        atualizarLabelPeriodoVeiculo();
        const placaSelect = document.getElementById('filtroPlacaVeiculo');
        const veiculoId = placaSelect?.value || '';
        const dataInicio = document.getElementById('dataInicioVeiculo')?.value;
        const dataFim = document.getElementById('dataFimVeiculo')?.value;

        if (!veiculoId) {
            limparResumoVeiculoSemSelecao();
            return;
        }

        if (dataInicio && dataFim) {
            carregarDadosVeiculoPeriodo(dataInicio, dataFim);
        } else {
            carregarDadosVeiculo();
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'aplicarFiltroPlacaVeiculo',
            error,
        );
    }
}

function atualizarLabelPeriodoGeral() {
    const label = document.getElementById('periodoAtualLabelGeral');
    if (!label) return;

    const ano = document.getElementById('filtroAnoGeral')?.value;
    const mes = document.getElementById('filtroMesGeral')?.value;
    const dataInicio = document.getElementById('dataInicioGeral')?.value;
    const dataFim = document.getElementById('dataFimGeral')?.value;

    if (dataInicio && dataFim) {
        const di = new Date(dataInicio + 'T00:00:00');
        const df = new Date(dataFim + 'T00:00:00');
        label.textContent = `Período: ${di.toLocaleDateString('pt-BR')} a ${df.toLocaleDateString('pt-BR')}`;
    } else if (ano && mes) {
        label.textContent = `Período: ${MESES_COMPLETOS[parseInt(mes)]} de ${ano}`;
    } else if (ano) {
        label.textContent = `Período: Ano de ${ano}`;
    } else {
        label.textContent = 'Exibindo todos os dados';
    }
}

function carregarDadosGeraisPeriodo(dataInicio, dataFim) {
    try {
        mostrarLoading();
        atualizarLabelPeriodoGeral();

        $.ajax({
            url: '/api/abastecimento/DashboardDadosPeriodo',
            type: 'GET',
            data: { dataInicio: dataInicio, dataFim: dataFim },
            success: function (data) {
                try {
                    dadosGerais = data;

                    // Verificar se a consulta retornou dados
                    if (
                        !data ||
                        !data.totais ||
                        data.totais.valorTotal === 0 ||
                        data.totais.valorTotal === null
                    ) {
                        const di = new Date(dataInicio + 'T00:00:00');
                        const df = new Date(dataFim + 'T00:00:00');
                        const periodo =
                            di.toLocaleDateString('pt-BR') +
                            ' a ' +
                            df.toLocaleDateString('pt-BR');
                        AppToast.show(
                            'orange',
                            'Não há registros de abastecimento para o período ' +
                                periodo,
                            5000,
                        );
                    }

                    renderizarAbaGeral(data);
                    esconderLoading();
                } catch (error) {
                    esconderLoading();
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'carregarDadosGeraisPeriodo.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                esconderLoading();
                console.error('Erro ao carregar dados por período:', error);
                // Fallback: usar filtro por ano/mês se endpoint não existir
                carregarDadosGerais();
            },
        });
    } catch (error) {
        esconderLoading();
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosGeraisPeriodo',
            error,
        );
    }
}

function atualizarLabelPeriodoVeiculo() {
    const label = document.getElementById('periodoAtualLabelVeiculo');
    if (!label) return;

    const ano = document.getElementById('filtroAnoVeiculo')?.value;
    const mes = document.getElementById('filtroMesVeiculo')?.value;
    const dataInicio = document.getElementById('dataInicioVeiculo')?.value;
    const dataFim = document.getElementById('dataFimVeiculo')?.value;

    // Obter texto da placa selecionada (não o value que é GUID)
    const placaSelect = document.getElementById('filtroPlacaVeiculo');
    let placaTexto = '';
    if (placaSelect?.value) {
        // Pegar o texto da opção selecionada e extrair apenas a placa
        const textoCompleto =
            placaSelect.options[placaSelect.selectedIndex]?.text || '';
        placaTexto = obterPlacaTextoCompleto(textoCompleto);
    }

    let partes = [];

    if (dataInicio && dataFim) {
        const di = new Date(dataInicio + 'T00:00:00');
        const df = new Date(dataFim + 'T00:00:00');
        partes.push(
            `${di.toLocaleDateString('pt-BR')} a ${df.toLocaleDateString('pt-BR')}`,
        );
    } else if (ano && mes) {
        partes.push(`${MESES_COMPLETOS[parseInt(mes)]} de ${ano}`);
    } else if (ano) {
        partes.push(`Ano de ${ano}`);
    }

    if (placaTexto) {
        partes.push(`Placa: ${placaTexto}`);
    }

    if (partes.length > 0) {
        label.textContent = `Período: ${partes.join(' | ')}`;
    } else {
        label.textContent = 'Exibindo todos os dados';
    }
}

function carregarDadosVeiculoPeriodo(dataInicio, dataFim) {
    try {
        mostrarLoading();
        atualizarLabelPeriodoVeiculo();

        $.ajax({
            url: '/api/abastecimento/DashboardDadosVeiculoPeriodo',
            type: 'GET',
            data: { dataInicio: dataInicio, dataFim: dataFim },
            success: function (data) {
                let mensagensToast = [];
                try {
                    dadosVeiculo = data;
                    mensagensToast = [];

                    // Verificar se a consulta retornou dados
                    if (
                        !data ||
                        data.valorTotal === 0 ||
                        data.valorTotal === null
                    ) {
                        const di = new Date(dataInicio + 'T00:00:00');
                        const df = new Date(dataFim + 'T00:00:00');
                        const periodo =
                            di.toLocaleDateString('pt-BR') +
                            ' a ' +
                            df.toLocaleDateString('pt-BR');
                        mensagensToast.push(
                            'Não há registros de abastecimento para o período ' +
                                periodo,
                        );
                    }

                    const resultadoPlaca = preencherFiltrosVeiculo(data);

                    atualizarLabelPeriodoVeiculo();

                    if (resultadoPlaca?.placaRemovida) {
                        const placaMensagem = resultadoPlaca.placaTexto
                            ? 'Nenhum Abastecimento no Período para o Veículo ' +
                              resultadoPlaca.placaTexto
                            : 'Nenhum Abastecimento no Período para o Veículo';
                        mensagensToast.push(placaMensagem);
                    }

                    const placaSelect =
                        document.getElementById('filtroPlacaVeiculo');
                    const veiculoIdAtual = placaSelect?.value || '';
                    const placaTextoCompleto =
                        placaSelect?.options[placaSelect.selectedIndex]?.text;
                    const placaSelecionada = placaTextoCompleto
                        ? obterPlacaTextoCompleto(placaTextoCompleto)
                        : null;
                    const placaValida =
                        veiculoIdAtual &&
                        placaSelecionada &&
                        placaSelecionada !== 'Todas';

                    renderizarAbaVeiculo(
                        data,
                        placaValida ? veiculoIdAtual : null,
                        placaValida ? placaSelecionada : null,
                    );

                    if (placaValida) {
                        renderizarHeatmapVeiculo(null, placaSelecionada, null);
                    } else {
                        renderizarHeatmapVeiculo(null, null, null);
                    }
                    esconderLoading();
                    exibirToastAmareloAposLoading(mensagensToast);
                } catch (error) {
                    esconderLoading();
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'carregarDadosVeiculoPeriodo.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                esconderLoading();
                console.error(
                    'Erro ao carregar dados de veículo por período:',
                    error,
                );
                // Fallback: usar filtro por ano/mês se endpoint não existir
                carregarDadosVeiculo();
            },
        });
    } catch (error) {
        esconderLoading();
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosVeiculoPeriodo',
            error,
        );
    }
}

// ====== CARREGAMENTO DE DADOS ======

function carregarDadosGerais() {
    try {
        mostrarLoading();
        atualizarLabelPeriodoGeral();
        const ano = document.getElementById('filtroAnoGeral')?.value || '';
        const mes = document.getElementById('filtroMesGeral')?.value || '';

        $.ajax({
            url: '/api/abastecimento/DashboardDados',
            type: 'GET',
            data: { ano: ano || null, mes: mes || null },
            success: function (data) {
                try {
                    dadosGerais = data;
                    // Não repreenche os filtros, apenas renderiza
                    renderizarAbaGeral(data);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'carregarDadosGerais.success',
                        error,
                    );
                } finally {
                    esconderLoading();
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados gerais:', error);
                AppToast.show(
                    'red',
                    'Erro ao carregar dados do dashboard',
                    5000,
                );
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosGerais',
            error,
        );
        esconderLoading();
    }
}

function carregarDadosMensais() {
    try {
        mostrarLoading();
        const selectAno = document.getElementById('filtroAnoMensal');
        const selectMes = document.getElementById('filtroMesMensal');
        let ano = selectAno?.value || '';
        let mes = selectMes?.value || '';

        // Se ano não estiver selecionado, selecionar automaticamente o mais recente e o mês mais recente
        if (!ano && selectAno && selectAno.options.length > 1) {
            // Pegar o primeiro ano disponível (após "Selecione o Ano") - é o mais recente
            ano = selectAno.options[1].value;
            selectAno.value = ano;

            // Popular meses do ano e selecionar o mais recente, depois carregar dados
            if (selectMes) {
                popularMesesDoAnoECarregar(ano, selectMes, 'mensal');
                return; // A função popularMesesDoAnoECarregar vai chamar a API
            }
        }

        // Validar que ano foi selecionado
        if (!ano) {
            AppToast.show(
                'orange',
                'Selecione um ano para visualizar os dados',
                3000,
            );
            esconderLoading();
            return;
        }

        executarCarregamentoMensal(ano, mes);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosMensais',
            error,
        );
        esconderLoading();
    }
}

/**
 * Executa o carregamento efetivo dos dados mensais
 */
function executarCarregamentoMensal(ano, mes) {
    try {
        $.ajax({
            url: '/api/abastecimento/DashboardMensal',
            type: 'GET',
            data: { ano: ano, mes: mes || null },
            success: function (data) {
                try {
                    dadosMensais = data;

                    // Verificar se a consulta retornou dados
                    if (
                        !data ||
                        data.valorTotal === 0 ||
                        data.valorTotal === null
                    ) {
                        const nomesMeses = [
                            '',
                            'Janeiro',
                            'Fevereiro',
                            'Março',
                            'Abril',
                            'Maio',
                            'Junho',
                            'Julho',
                            'Agosto',
                            'Setembro',
                            'Outubro',
                            'Novembro',
                            'Dezembro',
                        ];
                        const periodo = mes
                            ? nomesMeses[parseInt(mes)] + '/' + ano
                            : ano;
                        AppToast.show(
                            'orange',
                            'Não há registros de abastecimento para ' + periodo,
                            5000,
                        );
                    }

                    renderizarAbaMensal(data);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'executarCarregamentoMensal.success',
                        error,
                    );
                } finally {
                    esconderLoading();
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados mensais:', error);
                AppToast.show('red', 'Erro ao carregar dados mensais', 5000);
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'executarCarregamentoMensal',
            error,
        );
        esconderLoading();
    }
}

/**
 * Popula meses do ano, seleciona o mais recente e carrega os dados
 * @param {string} ano - Ano selecionado
 * @param {HTMLElement} mesSelect - Elemento select de mês
 * @param {string} tipo - 'mensal' ou 'veiculo'
 */
function popularMesesDoAnoECarregar(ano, mesSelect, tipo) {
    try {
        $.ajax({
            url: '/api/abastecimento/DashboardMesesDisponiveis',
            type: 'GET',
            data: { ano: ano },
            success: function (data) {
                const meses = data.meses || [];
                const nomesMeses = [
                    '',
                    'Janeiro',
                    'Fevereiro',
                    'Março',
                    'Abril',
                    'Maio',
                    'Junho',
                    'Julho',
                    'Agosto',
                    'Setembro',
                    'Outubro',
                    'Novembro',
                    'Dezembro',
                ];

                mesSelect.innerHTML =
                    '<option value="">&lt;Todos os Meses&gt;</option>';
                meses.forEach((mes) => {
                    const option = document.createElement('option');
                    option.value = mes;
                    option.textContent = nomesMeses[mes];
                    mesSelect.appendChild(option);
                });

                // Selecionar o mês mais recente (último da lista)
                let mesSelecionado = '';
                if (meses.length > 0) {
                    mesSelecionado = meses[meses.length - 1].toString();
                    mesSelect.value = mesSelecionado;
                }

                // Carregar dados conforme o tipo
                if (tipo === 'mensal') {
                    executarCarregamentoMensal(ano, mesSelecionado);
                } else if (tipo === 'veiculo') {
                    atualizarLabelPeriodoVeiculo();
                    executarCarregamentoVeiculo(ano, mesSelecionado);
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao buscar meses disponíveis:', error);
                mesSelect.innerHTML =
                    '<option value="">&lt;Todos os Meses&gt;</option>';
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'popularMesesDoAnoECarregar',
            error,
        );
        esconderLoading();
    }
}

function carregarDadosVeiculo(
    autoSelecionarAno = true,
    placaAnterior = null,
    textoPlacaAnterior = null,
) {
    try {
        mostrarLoading();
        atualizarLabelPeriodoVeiculo();
        const selectAno = document.getElementById('filtroAnoVeiculo');
        const selectMes = document.getElementById('filtroMesVeiculo');
        let ano = selectAno?.value || '';
        let mes = selectMes?.value || '';

        // Se ano não estiver selecionado, selecionar automaticamente o mais recente e o mês mais recente
        if (
            !ano &&
            autoSelecionarAno &&
            selectAno &&
            selectAno.options.length > 1
        ) {
            // Pegar o primeiro ano disponível (após "Selecione o Ano") - é o mais recente
            ano = selectAno.options[1].value;
            selectAno.value = ano;

            // Popular meses do ano e selecionar o mais recente, depois carregar dados
            if (selectMes) {
                popularMesesDoAnoECarregar(ano, selectMes, 'veiculo');
                return; // A função popularMesesDoAnoECarregar vai chamar a API
            }
        }

        // Se não há ano selecionado, apenas limpa os cards e encerra
        if (!ano) {
            limparResumoVeiculoSemSelecao();
            esconderLoading();
            return;
        }

        executarCarregamentoVeiculo(
            ano,
            mes,
            placaAnterior,
            textoPlacaAnterior,
        );
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'carregarDadosVeiculo',
            error,
        );
        esconderLoading();
    }
}

/**
 * Executa o carregamento efetivo dos dados por veículo
 * @param {string} ano - Ano selecionado
 * @param {string} mes - Mês selecionado
 * @param {string} placaAnterior - ID da placa que estava selecionada antes da mudança de filtro (para verificar se ainda está disponível)
 * @param {string} textoPlacaAnterior - Texto da placa que estava selecionada antes da mudança de filtro
 */
function executarCarregamentoVeiculo(
    ano,
    mes,
    placaAnterior = null,
    textoPlacaAnterior = null,
) {
    try {
        const placaSelect = document.getElementById('filtroPlacaVeiculo');
        const veiculoId = placaSelect?.value || '';

        $.ajax({
            url: '/api/abastecimento/DashboardVeiculo',
            type: 'GET',
            data: {
                ano: ano,
                mes: mes || null,
                veiculoId: veiculoId || null,
                tipoVeiculo: null,
            },
            success: function (data) {
                // Declarar mensagensToast fora do try para estar acessível no finally
                const mensagensToast = [];
                try {
                    dadosVeiculo = data;

                    // Verificar se a consulta retornou dados
                    if (
                        !data ||
                        data.valorTotal === 0 ||
                        data.valorTotal === null
                    ) {
                        const nomesMeses = [
                            '',
                            'Janeiro',
                            'Fevereiro',
                            'Março',
                            'Abril',
                            'Maio',
                            'Junho',
                            'Julho',
                            'Agosto',
                            'Setembro',
                            'Outubro',
                            'Novembro',
                            'Dezembro',
                        ];
                        const periodo = mes
                            ? nomesMeses[parseInt(mes)] + '/' + ano
                            : ano;
                        mensagensToast.push(
                            'Não há registros de abastecimento para ' + periodo,
                        );
                    }

                    const resultadoPlaca = preencherFiltrosVeiculo(data);

                    atualizarLabelPeriodoVeiculo();

                    // Verificar se havia uma placa selecionada antes da mudança de filtro
                    // e se ela não está mais disponível nas novas opções
                    if (
                        placaAnterior &&
                        textoPlacaAnterior &&
                        textoPlacaAnterior !== 'Todas'
                    ) {
                        const placasDisponiveis = data?.placasDisponiveis || [];
                        const placaAindaDisponivel = placasDisponiveis.some(
                            (p) => p.veiculoId === placaAnterior,
                        );
                        if (!placaAindaDisponivel) {
                            const placaTexto =
                                obterPlacaTextoCompleto(textoPlacaAnterior);
                            mensagensToast.push(
                                'Nenhum Abastecimento no Período para o Veículo' +
                                    (placaTexto ? ' ' + placaTexto : ''),
                            );
                        }
                    }
                    // Fallback: verificar pelo resultado da função preencherFiltrosVeiculo (para outros casos)
                    else if (resultadoPlaca?.placaRemovida) {
                        const placaMensagem = resultadoPlaca.placaTexto
                            ? 'Nenhum Abastecimento no Período para o Veículo ' +
                              resultadoPlaca.placaTexto
                            : 'Nenhum Abastecimento no Período para o Veículo';
                        mensagensToast.push(placaMensagem);
                    }

                    const placaSelectAtual =
                        document.getElementById('filtroPlacaVeiculo');
                    const veiculoIdAtual = placaSelectAtual?.value || '';
                    const placaTextoCompleto =
                        placaSelectAtual?.options[
                            placaSelectAtual.selectedIndex
                        ]?.text;
                    const placaSelecionada = placaTextoCompleto
                        ? obterPlacaTextoCompleto(placaTextoCompleto)
                        : null;
                    const placaValida =
                        veiculoIdAtual &&
                        placaSelecionada &&
                        placaSelecionada !== 'Todas';

                    renderizarAbaVeiculo(
                        data,
                        placaValida ? veiculoIdAtual : null,
                        placaValida ? placaSelecionada : null,
                    );

                    if (
                        veiculoIdAtual &&
                        placaSelecionada &&
                        placaSelecionada !== 'Todas'
                    ) {
                        renderizarHeatmapVeiculo(
                            ano || null,
                            placaSelecionada,
                            null,
                        );
                    } else {
                        renderizarHeatmapVeiculo(null, null, null);
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'executarCarregamentoVeiculo.success',
                        error,
                    );
                } finally {
                    esconderLoading();
                    exibirToastAmareloAposLoading(mensagensToast);
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados por veículo:', error);
                AppToast.show(
                    'red',
                    'Erro ao carregar dados por veículo',
                    5000,
                );
                esconderLoading();
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'executarCarregamentoVeiculo',
            error,
        );
        esconderLoading();
    }
}

// ====== RENDERIZAÇÃO - ABA GERAL ======
function renderizarAbaGeral(data) {
    try {
        document.getElementById('valorTotalGeral').textContent = formatarMoeda(
            data.totais.valorTotal,
        );
        document.getElementById('litrosTotalGeral').textContent =
            formatarLitros(data.totais.litrosTotal);
        document.getElementById('qtdAbastecimentosGeral').textContent =
            data.totais.qtdAbastecimentos.toLocaleString('pt-BR');

        const mediaDiesel = data.mediaLitro.find((m) =>
            m.combustivel.toLowerCase().includes('diesel'),
        );
        const mediaGasolina = data.mediaLitro.find((m) =>
            m.combustivel.toLowerCase().includes('gasolina'),
        );

        document.getElementById('mediaDieselGeral').textContent = mediaDiesel
            ? formatarMoeda(mediaDiesel.media)
            : 'R$ 0';
        document.getElementById('mediaGasolinaGeral').textContent =
            mediaGasolina ? formatarMoeda(mediaGasolina.media) : 'R$ 0';

        renderizarTabelaResumoPorAno(data.resumoPorAno);
        renderizarChartValorCategoria(data.valorPorCategoria);
        renderizarChartValorLitro(data.valorLitroPorMes);
        renderizarChartLitrosMes(data.litrosPorMes);
        renderizarChartConsumoMes(data.consumoPorMes);

        // Mapa de Calor Dia/Hora
        const anoGeral = document.getElementById('filtroAnoGeral')?.value || '';
        const mesGeral = document.getElementById('filtroMesGeral')?.value || '';
        renderizarHeatmapDiaHora(anoGeral, mesGeral);
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarAbaGeral',
            error,
        );
    }
}

function renderizarTabelaResumoPorAno(dados) {
    try {
        const container = document.getElementById('tabelaResumoPorAno');
        if (!container) return;

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
            return;
        }

        let html = '';
        let totalValor = 0;

        dados.forEach((item) => {
            totalValor += item.valor;
            html += `
                <div class="grid-row">
                    <div class="grid-cell" style="font-weight: 600;">${item.ano}</div>
                    <div class="grid-cell text-end">${formatarMoeda(item.valor)}</div>
                </div>
            `;
        });

        html += `
            <div class="grid-row grid-row-total">
                <div class="grid-cell">TOTAL</div>
                <div class="grid-cell text-end">${formatarMoeda(totalValor)}</div>
            </div>
        `;

        container.innerHTML = html;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarTabelaResumoPorAno',
            error,
        );
    }
}

function renderizarChartValorCategoria(dados) {
    try {
        const container = document.getElementById('chartValorCategoria');
        if (!container) return;

        if (chartValorCategoria) {
            chartValorCategoria.destroy();
            chartValorCategoria = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const dataSource = dados.map((item, idx) => ({
            x: item.categoria,
            y: item.valor,
            color: CORES.multi[idx % CORES.multi.length],
        }));

        chartValorCategoria = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: 'R$ {value}',
                labelStyle: { size: '9px' },
                labelIntersectAction: 'None',
                edgeLabelPlacement: 'Shift',
            },
            series: [
                {
                    dataSource: dataSource,
                    xName: 'x',
                    yName: 'y',
                    pointColorMapping: 'color',
                    type: 'Bar',
                    cornerRadius: { topRight: 4, bottomRight: 4 },
                },
            ],
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text
                            .replace('R$ ', '')
                            .replace(/\./g, '')
                            .replace(',', '.'),
                    );
                    args.text = formatarLabelMoeda(valor);
                }
            },
            height: '300px',
            chartArea: { border: { width: 0 } },
        });
        chartValorCategoria.appendTo('#chartValorCategoria');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartValorCategoria',
            error,
        );
    }
}

function renderizarChartValorLitro(dados) {
    try {
        const container = document.getElementById('chartValorLitro');
        if (!container) return;

        if (chartValorLitro) {
            chartValorLitro.destroy();
            chartValorLitro = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
        const series = combustiveis.map((comb, idx) => {
            const dataPoints = [];
            for (let m = 1; m <= 12; m++) {
                const item = dados.find(
                    (d) => d.mes === m && d.combustivel === comb,
                );
                dataPoints.push({ x: MESES[m], y: item ? item.media : 0 });
            }
            return {
                dataSource: dataPoints,
                xName: 'x',
                yName: 'y',
                name: comb,
                type: 'Line',
                marker: { visible: true, width: 6, height: 6 },
                width: 2,
            };
        });

        chartValorLitro = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: 'R$ {value}',
                labelStyle: { size: '9px' },
            },
            series: series,
            legendSettings: { visible: true, position: 'Bottom' },
            tooltip: { enable: true, format: '${series.name}: R$ ${point.y}' },
            height: '300px',
            chartArea: { border: { width: 0 } },
            palettes: [CORES.amber[2], CORES.gold[1]],
        });
        chartValorLitro.appendTo('#chartValorLitro');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartValorLitro',
            error,
        );
    }
}

function renderizarChartLitrosMes(dados) {
    try {
        const container = document.getElementById('chartLitrosMes');
        if (!container) return;

        if (chartLitrosMes) {
            chartLitrosMes.destroy();
            chartLitrosMes = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
        const series = combustiveis.map((comb, idx) => {
            const dataPoints = [];
            for (let m = 1; m <= 12; m++) {
                const item = dados.find(
                    (d) => d.mes === m && d.combustivel === comb,
                );
                dataPoints.push({ x: MESES[m], y: item ? item.litros : 0 });
            }
            return {
                dataSource: dataPoints,
                xName: 'x',
                yName: 'y',
                name: comb,
                type: 'StackingColumn',
                cornerRadius: { topLeft: 2, topRight: 2 },
            };
        });

        chartLitrosMes = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: '{value}',
                labelStyle: { size: '9px' },
            },
            series: series,
            legendSettings: { visible: true, position: 'Bottom' },
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.series.name +
                    ': ' +
                    formatarLabelNumero(args.point.y) +
                    ' L';
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text.replace(/\./g, '').replace(',', '.'),
                    );
                    args.text = formatarLabelNumero(valor);
                }
            },
            height: '280px',
            chartArea: { border: { width: 0 } },
            palettes: [CORES.amber[1], CORES.gold[1]],
        });
        chartLitrosMes.appendTo('#chartLitrosMes');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartLitrosMes',
            error,
        );
    }
}

function renderizarChartConsumoMes(dados) {
    try {
        const container = document.getElementById('chartConsumoMes');
        if (!container) return;

        if (chartConsumoMes) {
            chartConsumoMes.destroy();
            chartConsumoMes = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const dataSource = [];
        for (let m = 1; m <= 12; m++) {
            const item = dados.find((d) => d.mes === m);
            dataSource.push({
                x: MESES[m],
                y: item ? item.valor : 0,
                color: CORES.amber[m % CORES.amber.length],
            });
        }

        chartConsumoMes = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: 'R$ {value}',
                labelStyle: { size: '9px' },
            },
            series: [
                {
                    dataSource: dataSource,
                    xName: 'x',
                    yName: 'y',
                    pointColorMapping: 'color',
                    type: 'Column',
                    cornerRadius: { topLeft: 4, topRight: 4 },
                },
            ],
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text
                            .replace('R$ ', '')
                            .replace(/\./g, '')
                            .replace(',', '.'),
                    );
                    args.text = formatarLabelMoeda(valor);
                }
            },
            height: '280px',
            chartArea: { border: { width: 0 } },
        });
        chartConsumoMes.appendTo('#chartConsumoMes');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartConsumoMes',
            error,
        );
    }
}

// ====== RENDERIZAÇÃO - ABA MENSAL ======
function renderizarAbaMensal(data) {
    try {
        // Cards de totais
        document.getElementById('valorTotalMensal').textContent = formatarMoeda(
            data.valorTotal,
        );
        document.getElementById('totalLitrosMensal').textContent =
            formatarLitros(data.litrosTotal);

        // Tabela média do litro
        renderizarTabelaMediaLitroMensal(data.mediaLitro);

        // Gráfico pizza combustíveis (COM LEGENDA)
        renderizarChartPizzaCombustivel(data.porCombustivel);

        // Gráfico Litros por Dia (LEGENDA MAIOR)
        renderizarChartLitrosDia(data.litrosPorDia);

        // TABELAS TOP 15
        renderizarTabelaValorPorUnidade(data.valorPorUnidade);
        renderizarTabelaValorPorPlaca(data.valorPorPlaca);

        // Gráfico de consumo por CATEGORIA REAL
        renderizarChartConsumoCategoria(data.consumoPorCategoria);

        // Mapa de Calor Categoria/Mês
        const anoMensal =
            document.getElementById('filtroAnoMensal')?.value || '';
        if (anoMensal) {
            renderizarHeatmapCategoria(anoMensal);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarAbaMensal',
            error,
        );
    }
}

function renderizarTabelaMediaLitroMensal(dados) {
    try {
        const tbody = document.getElementById('tabelaMediaLitroMensal');
        if (!tbody) return;

        if (!dados || dados.length === 0) {
            tbody.innerHTML =
                '<tr><td colspan="2" class="text-center text-muted py-3">Sem dados</td></tr>';
            return;
        }

        let html = '';
        dados.forEach((item) => {
            html += `
                <tr>
                    <td style="font-weight: 500;">${item.combustivel}</td>
                    <td class="text-end" style="font-weight: 600; color: var(--dash-marrom-dark);">${formatarMoeda(item.media)}</td>
                </tr>
            `;
        });

        tbody.innerHTML = html;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarTabelaMediaLitroMensal',
            error,
        );
    }
}

/**
 * Pizza de Combustíveis - COM LEGENDA VISÍVEL
 */
function renderizarChartPizzaCombustivel(dados) {
    try {
        const container = document.getElementById('chartPizzaCombustivel');
        if (!container) return;

        if (chartPizzaCombustivel) {
            chartPizzaCombustivel.destroy();
            chartPizzaCombustivel = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-4">Sem dados</div>';
            return;
        }

        const dataSource = dados.map((item, idx) => ({
            x: item.combustivel,
            y: item.litros,
            text: item.combustivel,
            color: CORES.multi[idx % CORES.multi.length],
        }));

        chartPizzaCombustivel = new ej.charts.AccumulationChart({
            series: [
                {
                    dataSource: dataSource,
                    xName: 'x',
                    yName: 'y',
                    pointColorMapping: 'color',
                    type: 'Pie',
                    dataLabel: {
                        visible: false, // Desabilita labels externos para não poluir
                    },
                    radius: '75%',
                },
            ],
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.point.x +
                    ': ' +
                    formatarLabelNumero(args.point.y) +
                    ' L';
            },
            // LEGENDA VISÍVEL
            legendSettings: {
                visible: true,
                position: 'Bottom',
                textStyle: { size: '11px', fontWeight: '500' },
            },
            height: '180px',
            enableSmartLabels: true,
        });
        chartPizzaCombustivel.appendTo('#chartPizzaCombustivel');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartPizzaCombustivel',
            error,
        );
    }
}

/**
 * Litros por Dia - LEGENDA 40% MAIOR
 */
function renderizarChartLitrosDia(dados) {
    try {
        const container = document.getElementById('chartLitrosDia');
        if (!container) return;

        if (chartLitrosDia) {
            chartLitrosDia.destroy();
            chartLitrosDia = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
        const series = combustiveis.map((comb, idx) => {
            const dataPoints = dados
                .filter((d) => d.combustivel === comb)
                .map((d) => ({ x: d.dia, y: d.litros }));
            return {
                dataSource: dataPoints,
                xName: 'x',
                yName: 'y',
                name: comb,
                type: 'SplineArea',
                opacity: 0.6,
                border: { width: 2 },
            };
        });

        chartLitrosDia = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Double',
                labelStyle: { size: '10px' },
                title: 'Dia do Mês',
                titleStyle: { size: '11px', fontWeight: '600' },
                interval: 1,
            },
            primaryYAxis: {
                labelFormat: '{value}',
                labelStyle: { size: '10px' },
                title: 'Litros',
                titleStyle: { size: '11px', fontWeight: '600' },
            },
            series: series,
            // LEGENDA 40% MAIOR
            legendSettings: {
                visible: true,
                position: 'Top',
                textStyle: { size: '14px', fontWeight: '600' },
                shapeHeight: 12,
                shapeWidth: 12,
                padding: 10,
            },
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.series.name +
                    ' (Dia ' +
                    args.point.x +
                    '): ' +
                    formatarLabelNumero(args.point.y) +
                    ' L';
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text.replace(/\./g, '').replace(',', '.'),
                    );
                    args.text = formatarLabelNumero(valor);
                }
            },
            height: '220px',
            chartArea: { border: { width: 0 } },
            palettes: [CORES.amber[2], CORES.gold[2]],
        });
        chartLitrosDia.appendTo('#chartLitrosDia');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartLitrosDia',
            error,
        );
    }
}

/**
 * Tabela TOP 15 por UNIDADE - Usando DIVs
 * Clique na linha abre modal com abastecimentos da unidade
 */
function renderizarTabelaValorPorUnidade(dados) {
    try {
        const container = document.getElementById('tabelaValorPorUnidade');
        if (!container) return;

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
            return;
        }

        let html = '';
        let totalValor = 0;

        dados.forEach((item, idx) => {
            totalValor += item.valor;
            const badgeClass =
                idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
            html += `
                <div class="grid-row grid-row-clicavel" data-unidade="${item.unidade}" title="Clique para ver abastecimentos desta unidade">
                    <div class="grid-cell"><span class="${badgeClass}">${idx + 1}</span> ${item.unidade}</div>
                    <div class="grid-cell text-end" style="color: #4a7c59; font-weight: 600;">${formatarMoedaTabela(item.valor)}</div>
                </div>
            `;
        });

        // Linha de total (não clicável)
        html += `
            <div class="grid-row grid-row-total">
                <div class="grid-cell"><strong>Total (Top 15)</strong></div>
                <div class="grid-cell text-end" style="color: #2d5a3d; font-weight: 700;">${formatarMoedaTabela(totalValor)}</div>
            </div>
        `;

        container.innerHTML = html;

        // Adicionar eventos de clique nas linhas
        container.querySelectorAll('.grid-row-clicavel').forEach((row) => {
            row.addEventListener('click', function () {
                const unidade = this.dataset.unidade;
                if (unidade) {
                    abrirModalAbastecimentosUnidade(unidade);
                }
            });
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarTabelaValorPorUnidade',
            error,
        );
    }
}

/**
 * Tabela TOP 15 por PLACA individual - Usando DIVs
 * Clique na linha abre modal com viagens do veículo
 */
function renderizarTabelaValorPorPlaca(dados) {
    try {
        const container = document.getElementById('tabelaValorPorPlaca');
        if (!container) return;

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="grid-row" style="justify-content: center; padding: 20px; color: #6c757d;">Sem dados</div>';
            return;
        }

        let html = '';
        let totalValor = 0;

        dados.forEach((item, idx) => {
            totalValor += item.valor;
            const badgeClass =
                idx < 3 ? 'badge-rank-abast top3' : 'badge-rank-abast';
            html += `
                <div class="grid-row grid-row-clicavel" data-veiculo-id="${item.veiculoId}" data-placa="${item.placa}" title="Clique para ver viagens deste veículo">
                    <div class="grid-cell"><span class="${badgeClass}">${idx + 1}</span> <strong>${item.placa}</strong></div>
                    <div class="grid-cell" style="font-size: 0.7rem; color: #666;">${item.tipoVeiculo || '-'}</div>
                    <div class="grid-cell text-end" style="color: #4a7c59; font-weight: 600;">${formatarMoedaTabela(item.valor)}</div>
                </div>
            `;
        });

        // Linha de total (não clicável)
        html += `
            <div class="grid-row grid-row-total">
                <div class="grid-cell" style="grid-column: span 2;"><strong>Total (Top 15)</strong></div>
                <div class="grid-cell text-end" style="color: #2d5a3d; font-weight: 700;">${formatarMoedaTabela(totalValor)}</div>
            </div>
        `;

        container.innerHTML = html;

        // Adicionar eventos de clique nas linhas
        container.querySelectorAll('.grid-row-clicavel').forEach((row) => {
            row.addEventListener('click', function () {
                const veiculoId = this.dataset.veiculoId;
                const placa = this.dataset.placa;
                if (veiculoId) {
                    abrirModalViagensVeiculo(veiculoId, placa);
                }
            });
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarTabelaValorPorPlaca',
            error,
        );
    }
}

/**
 * Gráfico de Consumo por CATEGORIA REAL do veículo
 * (Ambulância, Carga Leve, Carga Pesada, Coletivos Pequenos, Depol, Mesa, Ônibus/Microônibus, Passeio)
 * Clique na barra abre modal com veículos da categoria
 */
function renderizarChartConsumoCategoria(dados) {
    try {
        const container = document.getElementById('chartConsumoCategoria');
        if (!container) return;

        if (chartConsumoCategoria) {
            chartConsumoCategoria.destroy();
            chartConsumoCategoria = null;
        }
        container.innerHTML = '';

        console.log('consumoPorCategoria:', dados); // Debug

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados de categoria</div>';
            return;
        }

        const dataSource = dados.map((item, idx) => ({
            x: item.categoria || 'Sem Categoria',
            y: item.valor || 0,
            color: CORES.categorias[idx % CORES.categorias.length],
        }));

        chartConsumoCategoria = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -30,
                labelStyle: { size: '11px', fontWeight: '500' },
            },
            primaryYAxis: {
                labelFormat: 'R$ {value}',
                labelStyle: { size: '10px' },
            },
            series: [
                {
                    dataSource: dataSource,
                    xName: 'x',
                    yName: 'y',
                    pointColorMapping: 'color',
                    type: 'Column',
                    cornerRadius: { topLeft: 4, topRight: 4 },
                    dataLabel: {
                        visible: true,
                        position: 'Top',
                        font: { size: '10px', fontWeight: '600' },
                    },
                },
            ],
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.point.x +
                    ': ' +
                    formatarLabelMoeda(args.point.y) +
                    ' (clique para detalhes)';
            },
            textRender: function (args) {
                args.text = formatarMoedaCompacta(args.point.y);
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text
                            .replace('R$ ', '')
                            .replace(/\./g, '')
                            .replace(',', '.'),
                    );
                    args.text = formatarLabelMoeda(valor);
                }
            },
            pointClick: function (args) {
                // Ao clicar em uma barra, abre o modal da categoria
                const categoria = args.point.x;
                if (categoria && categoria !== 'Sem Categoria') {
                    abrirModalAbastecimentosCategoria(categoria);
                }
            },
            height: '280px',
            chartArea: { border: { width: 0 } },
        });
        chartConsumoCategoria.appendTo('#chartConsumoCategoria');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartConsumoCategoria',
            error,
        );
    }
}

// ====== RENDERIZAÇÃO - ABA VEÍCULO ======
function renderizarAbaVeiculo(data, veiculoSelecionadoId, placaSelecionada) {
    try {
        // Verificar se há veículo selecionado
        const temVeiculoSelecionado = veiculoSelecionadoId && placaSelecionada;

        if (temVeiculoSelecionado) {
            // Veículo selecionado - mostrar dados normalmente
            document.getElementById('valorTotalVeiculo').textContent =
                formatarMoeda(data.valorTotal);
            document.getElementById('litrosTotalVeiculo').textContent =
                formatarLitros(data.litrosTotal);
            document.getElementById('descricaoVeiculoSelecionado').textContent =
                data.descricaoVeiculo;
            document.getElementById('categoriaVeiculoSelecionado').textContent =
                data.categoriaVeiculo;

            // Passar mesSelecionado para destacar no gráfico
            renderizarChartConsumoMensalVeiculo(
                data.consumoMensalLitros,
                data.mesSelecionado || 0,
            );
            renderizarChartValorMensalVeiculo(
                data.valorMensal,
                data.mesSelecionado || 0,
            );
            renderizarChartRankingVeiculos(
                data.veiculosComValor,
                veiculoSelecionadoId,
                placaSelecionada,
            );
        } else {
            // Nenhum veículo selecionado - mostrar mensagem orientativa em todos os gráficos
            document.getElementById('valorTotalVeiculo').textContent = '-';
            document.getElementById('litrosTotalVeiculo').textContent = '-';
            document.getElementById('descricaoVeiculoSelecionado').textContent =
                'Nenhum veículo selecionado';
            document.getElementById('categoriaVeiculoSelecionado').textContent =
                '-';

            // Mostrar gráficos vazios com mensagem
            renderizarGraficoVazioComMensagem(
                'chartConsumoMensalVeiculo',
                'Escolha um Veículo para visualizar os Dados',
            );
            renderizarGraficoVazioComMensagem(
                'chartValorMensalVeiculo',
                'Escolha um Veículo para visualizar os Dados',
            );
            renderizarGraficoVazioComMensagem(
                'chartRankingVeiculos',
                'Escolha um Veículo para visualizar os Dados',
            );
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarAbaVeiculo',
            error,
        );
    }
}

/**
 * Renderiza um container de gráfico vazio com mensagem orientativa
 * @param {string} containerId - ID do elemento container
 * @param {string} mensagem - Mensagem a ser exibida
 */
function renderizarGraficoVazioComMensagem(containerId, mensagem) {
    try {
        const container = document.getElementById(containerId);
        if (!container) return;

        // Destruir gráfico existente se houver
        if (
            containerId === 'chartConsumoMensalVeiculo' &&
            chartConsumoMensalVeiculo
        ) {
            chartConsumoMensalVeiculo.destroy();
            chartConsumoMensalVeiculo = null;
        }
        if (
            containerId === 'chartValorMensalVeiculo' &&
            chartValorMensalVeiculo
        ) {
            chartValorMensalVeiculo.destroy();
            chartValorMensalVeiculo = null;
        }
        if (containerId === 'chartRankingVeiculos' && chartRankingVeiculos) {
            chartRankingVeiculos.destroy();
            chartRankingVeiculos = null;
        }

        container.innerHTML = `
            <div class="d-flex flex-column align-items-center justify-content-center h-100 py-4" style="min-height: 160px;">
                <i class="fa-duotone fa-car-side fa-3x mb-3" style="color: #c4956a; opacity: 0.6;"></i>
                <span class="text-muted" style="font-size: 14px; color: #8b7355 !important;">${mensagem}</span>
            </div>
        `;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarGraficoVazioComMensagem',
            error,
        );
    }
}

function renderizarChartConsumoMensalVeiculo(dados, mesSelecionado = 0) {
    try {
        const container = document.getElementById('chartConsumoMensalVeiculo');
        if (!container) return;

        if (chartConsumoMensalVeiculo) {
            chartConsumoMensalVeiculo.destroy();
            chartConsumoMensalVeiculo = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        const combustiveis = [...new Set(dados.map((d) => d.combustivel))];
        const series = combustiveis.map((comb) => {
            const dataPoints = [];
            for (let m = 1; m <= 12; m++) {
                const item = dados.find(
                    (d) => d.mes === m && d.combustivel === comb,
                );
                const ehMesSelecionado =
                    mesSelecionado > 0 && m === mesSelecionado;
                dataPoints.push({
                    x: ehMesSelecionado ? '★ ' + MESES[m] : MESES[m],
                    y: item ? item.litros : 0,
                });
            }
            return {
                dataSource: dataPoints,
                xName: 'x',
                yName: 'y',
                name: comb,
                type: 'SplineArea',
                opacity: 0.6,
                border: { width: 2 },
            };
        });

        chartConsumoMensalVeiculo = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: '{value}',
                labelStyle: { size: '9px' },
            },
            series: series,
            legendSettings: { visible: true, position: 'Bottom' },
            tooltip: { enable: true },
            tooltipRender: function (args) {
                const label = args.point.x.replace('★ ', '');
                args.text =
                    args.series.name +
                    ' (' +
                    label +
                    '): ' +
                    formatarLabelNumero(args.point.y) +
                    'L';
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text.replace(/\./g, '').replace(',', '.'),
                    );
                    args.text = formatarLabelNumero(valor);
                }
            },
            height: '160px',
            chartArea: { border: { width: 0 } },
            palettes: ['#ff6b35', CORES.gold[1]], // Laranja forte como primeira cor
        });
        chartConsumoMensalVeiculo.appendTo('#chartConsumoMensalVeiculo');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartConsumoMensalVeiculo',
            error,
        );
    }
}

function renderizarChartValorMensalVeiculo(dados, mesSelecionado = 0) {
    try {
        const container = document.getElementById('chartValorMensalVeiculo');
        if (!container) return;

        if (chartValorMensalVeiculo) {
            chartValorMensalVeiculo.destroy();
            chartValorMensalVeiculo = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        // Cor laranja forte para o mês selecionado
        const corDestaque = '#ff6b35'; // Laranja forte FrotiX
        const corNormal = '#c4956a'; // Caramelo suave

        const dataSource = [];
        for (let m = 1; m <= 12; m++) {
            const item = dados.find((d) => d.mes === m);
            const ehMesSelecionado = mesSelecionado > 0 && m === mesSelecionado;
            dataSource.push({
                x: MESES_COMPLETOS[m],
                y: item ? item.valor : 0,
                color: ehMesSelecionado ? corDestaque : corNormal,
                mes: m,
                selecionado: ehMesSelecionado,
            });
        }

        chartValorMensalVeiculo = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelStyle: { size: '9px' },
            },
            primaryYAxis: {
                labelFormat: 'R$ {value}',
                labelStyle: { size: '9px' },
            },
            series: [
                {
                    dataSource: dataSource,
                    xName: 'x',
                    yName: 'y',
                    pointColorMapping: 'color',
                    type: 'Bar',
                    cornerRadius: { topRight: 4, bottomRight: 4 },
                    marker: {
                        dataLabel: {
                            visible: true,
                            position: 'Top',
                            font: {
                                size: '12px',
                                color: '#ff6b35',
                                fontWeight: 'bold',
                            },
                        },
                    },
                },
            ],
            tooltip: { enable: true },
            tooltipRender: function (args) {
                args.text =
                    args.point.x + ': ' + formatarLabelMoeda(args.point.y);
            },
            textRender: function (args) {
                // Mostrar estrela apenas para o mês selecionado
                if (args.point && args.point.selecionado) {
                    args.text = '★';
                } else {
                    args.text = '';
                }
            },
            axisLabelRender: function (args) {
                if (args.axis.name === 'primaryYAxis') {
                    const valor = parseFloat(
                        args.text
                            .replace('R$ ', '')
                            .replace(/\./g, '')
                            .replace(',', '.'),
                    );
                    args.text = formatarLabelMoeda(valor);
                }
            },
            height: '280px',
            chartArea: { border: { width: 0 } },
        });
        chartValorMensalVeiculo.appendTo('#chartValorMensalVeiculo');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartValorMensalVeiculo',
            error,
        );
    }
}

function renderizarChartRankingVeiculos(
    dados,
    veiculoSelecionadoId,
    placaSelecionada,
) {
    try {
        const container = document.getElementById('chartRankingVeiculos');
        const tituloEl = document.getElementById('tituloRankingVeiculos');
        const subtituloEl = document.getElementById('subtituloRankingVeiculos');
        const iconEl = document.getElementById('iconRankingVeiculos');
        if (!container) return;

        if (chartRankingVeiculos) {
            chartRankingVeiculos.destroy();
            chartRankingVeiculos = null;
        }
        container.innerHTML = '';

        if (!dados || dados.length === 0) {
            container.innerHTML =
                '<div class="text-center text-muted py-5">Sem dados</div>';
            return;
        }

        // Verificar se há veículo selecionado
        const modoComparativo = veiculoSelecionadoId && placaSelecionada;

        if (modoComparativo) {
            // Modo comparativo: veículo selecionado vs TOP 10
            if (tituloEl) tituloEl.textContent = 'Comparativo de Consumo';
            if (subtituloEl)
                subtituloEl.textContent = placaSelecionada + ' vs. Top 10';
            if (iconEl) iconEl.className = 'fa-duotone fa-chart-mixed';

            const top10 = dados.slice(0, 10);
            const veiculoNoTop10 = top10.find(
                (v) => v.veiculoId == veiculoSelecionadoId,
            );
            const veiculoSelecionado = dados.find(
                (v) => v.veiculoId == veiculoSelecionadoId,
            );

            let dadosComparativo = [];

            // Cor laranja forte para o veículo selecionado (padrão FrotiX)
            const corDestaque = '#ff6b35';

            // Se o veículo selecionado não está no TOP 10, adiciona ele primeiro
            if (veiculoSelecionado && !veiculoNoTop10) {
                dadosComparativo.push({
                    x: veiculoSelecionado.placa,
                    y: veiculoSelecionado.valor,
                    color: corDestaque,
                    veiculoId: veiculoSelecionado.veiculoId,
                    selecionado: true,
                });
            }

            // Adiciona TOP 10, destacando o selecionado se estiver nele
            top10.forEach((item, idx) => {
                const isSelecionado = item.veiculoId == veiculoSelecionadoId;
                dadosComparativo.push({
                    x: item.placa,
                    y: item.valor,
                    color: isSelecionado
                        ? corDestaque
                        : CORES.multi[idx % CORES.multi.length],
                    veiculoId: item.veiculoId,
                    selecionado: isSelecionado,
                });
            });

            chartRankingVeiculos = new ej.charts.Chart({
                primaryXAxis: {
                    valueType: 'Category',
                    labelStyle: { size: '8px' },
                },
                primaryYAxis: {
                    labelFormat: 'R$ {value}',
                    labelStyle: { size: '9px' },
                },
                series: [
                    {
                        dataSource: dadosComparativo,
                        xName: 'x',
                        yName: 'y',
                        pointColorMapping: 'color',
                        type: 'Bar',
                        cornerRadius: { topRight: 4, bottomRight: 4 },
                        marker: {
                            dataLabel: {
                                visible: true,
                                position: 'Top',
                                font: {
                                    size: '12px',
                                    color: '#ff6b35',
                                    fontWeight: 'bold',
                                },
                                template:
                                    '<div style="font-size:14px;color:#ff6b35;">${point.selecionado ? "★" : ""}</div>',
                            },
                        },
                    },
                ],
                tooltip: { enable: true },
                tooltipRender: function (args) {
                    args.text =
                        args.point.x + ': ' + formatarLabelMoeda(args.point.y);
                },
                textRender: function (args) {
                    // Mostrar estrela apenas para o veículo selecionado
                    if (args.point && args.point.selecionado) {
                        args.text = '★';
                    } else {
                        args.text = '';
                    }
                },
                axisLabelRender: function (args) {
                    if (args.axis.name === 'primaryYAxis') {
                        const valor = parseFloat(
                            args.text
                                .replace('R$ ', '')
                                .replace(/\./g, '')
                                .replace(',', '.'),
                        );
                        args.text = formatarLabelMoeda(valor);
                    }
                },
                height: '280px',
                chartArea: { border: { width: 0 } },
            });
        } else {
            // Modo ranking normal
            if (tituloEl) tituloEl.textContent = 'Ranking de Veículos (Top 10)';
            if (subtituloEl) subtituloEl.textContent = 'Por placa individual';
            if (iconEl) iconEl.className = 'fa-duotone fa-ranking-star';

            const top10 = dados.slice(0, 10);
            const dataSource = top10.map((item, idx) => ({
                x:
                    item.placa +
                    (item.tipoVeiculo ? '\n' + item.tipoVeiculo : ''),
                y: item.valor,
                color: CORES.multi[idx % CORES.multi.length],
                veiculoId: item.veiculoId,
            }));

            chartRankingVeiculos = new ej.charts.Chart({
                primaryXAxis: {
                    valueType: 'Category',
                    labelStyle: { size: '8px' },
                },
                primaryYAxis: {
                    labelFormat: 'R$ {value}',
                    labelStyle: { size: '9px' },
                },
                series: [
                    {
                        dataSource: dataSource,
                        xName: 'x',
                        yName: 'y',
                        pointColorMapping: 'color',
                        type: 'Bar',
                        cornerRadius: { topRight: 4, bottomRight: 4 },
                    },
                ],
                tooltip: { enable: true },
                tooltipRender: function (args) {
                    const label = args.point.x.replace('\n', ' - ');
                    args.text = label + ': ' + formatarLabelMoeda(args.point.y);
                },
                axisLabelRender: function (args) {
                    if (args.axis.name === 'primaryYAxis') {
                        const valor = parseFloat(
                            args.text
                                .replace('R$ ', '')
                                .replace(/\./g, '')
                                .replace(',', '.'),
                        );
                        args.text = formatarLabelMoeda(valor);
                    }
                },
                pointClick: function (args) {
                    const veiculoId = args.point.veiculoId;
                    if (veiculoId) {
                        selecionarVeiculo(veiculoId);
                    }
                },
                height: '280px',
                chartArea: { border: { width: 0 } },
            });
        }

        chartRankingVeiculos.appendTo('#chartRankingVeiculos');
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarChartRankingVeiculos',
            error,
        );
    }
}

// ====== FUNÇÕES DE DESTRUIÇÃO DE GRÁFICOS ======

function destruirGraficosGeral() {
    try {
        if (chartValorCategoria) {
            chartValorCategoria.destroy();
            chartValorCategoria = null;
        }
        if (chartValorLitro) {
            chartValorLitro.destroy();
            chartValorLitro = null;
        }
        if (chartLitrosMes) {
            chartLitrosMes.destroy();
            chartLitrosMes = null;
        }
        if (chartConsumoMes) {
            chartConsumoMes.destroy();
            chartConsumoMes = null;
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'destruirGraficosGeral',
            error,
        );
    }
}

function destruirGraficosMensal() {
    try {
        if (chartPizzaCombustivel) {
            chartPizzaCombustivel.destroy();
            chartPizzaCombustivel = null;
        }
        if (chartLitrosDia) {
            chartLitrosDia.destroy();
            chartLitrosDia = null;
        }
        if (chartConsumoCategoria) {
            chartConsumoCategoria.destroy();
            chartConsumoCategoria = null;
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'destruirGraficosMensal',
            error,
        );
    }
}

function destruirGraficosVeiculo() {
    try {
        if (chartConsumoMensalVeiculo) {
            chartConsumoMensalVeiculo.destroy();
            chartConsumoMensalVeiculo = null;
        }
        if (chartValorMensalVeiculo) {
            chartValorMensalVeiculo.destroy();
            chartValorMensalVeiculo = null;
        }
        if (chartRankingVeiculos) {
            chartRankingVeiculos.destroy();
            chartRankingVeiculos = null;
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'destruirGraficosVeiculo',
            error,
        );
    }
}

// ====== FUNÇÕES AUXILIARES ======

function preencherFiltrosVeiculo(data) {
    try {
        const selectPlaca = document.getElementById('filtroPlacaVeiculo');
        const $selectPlaca = $('#filtroPlacaVeiculo');

        const placaAtual = selectPlaca?.value || '';
        const textoPlacaAtual =
            selectPlaca?.options[selectPlaca.selectedIndex]?.text || '';
        let resultado = {
            placaMantida: false,
            placaRemovida: false,
            placaTexto: obterPlacaTextoCompleto(textoPlacaAtual),
        };

        if (!data || !Array.isArray(data.placasDisponiveis)) {
            resetarOpcoesPlacaVeiculo();
            resultado.placaRemovida = !!placaAtual;
            return resultado;
        }

        // Preencher placas - filtrado pelo backend por período
        // Destruir Select2 antes de modificar opções
        if (
            $selectPlaca.length &&
            $selectPlaca.hasClass('select2-hidden-accessible')
        ) {
            $selectPlaca.select2('destroy');
        }

        selectPlaca.innerHTML = '<option value="">Todas</option>';

        // Usar placasDisponiveis direto do backend (já filtrado por período)
        data.placasDisponiveis.forEach((item) => {
            const option = document.createElement('option');
            option.value = item.veiculoId;
            const marcaModelo =
                item.marcaModelo ||
                item.marcaModeloDescricao ||
                item.marcaModeloCompleto ||
                '';
            const tipoVeiculo = item.tipoVeiculo || item.tipo || '';
            const descricao = marcaModelo || tipoVeiculo;
            option.textContent =
                item.placa + (descricao ? ' - ' + descricao : '');
            selectPlaca.appendChild(option);
        });

        // Restaurar valor se ainda existir nas opções
        if (placaAtual) {
            const opcaoExiste = Array.from(selectPlaca.options).some(
                (opt) => opt.value === placaAtual,
            );
            if (opcaoExiste) {
                selectPlaca.value = placaAtual;
                resultado.placaMantida = true;
            } else {
                resultado.placaRemovida = true;
                ignorarMudancaPlacaVeiculo = true;
                selectPlaca.value = '';
                if (
                    $selectPlaca.length &&
                    $selectPlaca.hasClass('select2-hidden-accessible')
                ) {
                    $selectPlaca.val('').trigger('change.select2');
                }
                ignorarMudancaPlacaVeiculo = false;
            }
        }

        // Reinicializar Select2 com as novas opções
        if ($selectPlaca.length && typeof $.fn.select2 === 'function') {
            $selectPlaca.select2({
                placeholder: 'Digite para buscar...',
                allowClear: true,
                width: '260px',
                language: {
                    noResults: function () {
                        return 'Nenhuma placa encontrada';
                    },
                    searching: function () {
                        return 'Buscando...';
                    },
                    inputTooShort: function () {
                        return 'Digite para buscar';
                    },
                },
            });
            // Trigger change para atualizar visualmente
            $selectPlaca.trigger('change.select2');
        }
        if (!resultado.placaMantida && !resultado.placaRemovida) {
            resultado.placaTexto = '';
        }
        return resultado;
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'preencherFiltrosVeiculo',
            error,
        );
        return { placaMantida: false, placaRemovida: false, placaTexto: '' };
    }
}

function selecionarVeiculo(veiculoId) {
    try {
        // Não limpar mais o modelo - filtros são independentes
        // document.getElementById('filtroModeloVeiculo').value = '';

        // Selecionar a placa no Select2
        const $selectPlaca = $('#filtroPlacaVeiculo');
        if (
            $selectPlaca.length &&
            $selectPlaca.hasClass('select2-hidden-accessible')
        ) {
            $selectPlaca.val(veiculoId).trigger('change');
        } else {
            document.getElementById('filtroPlacaVeiculo').value = veiculoId;
            carregarDadosVeiculo();
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'selecionarVeiculo',
            error,
        );
    }
}

// ====== FUNÇÕES DE FORMATAÇÃO ======

function formatarMoeda(valor) {
    if (!valor) return 'R$ 0';
    if (Math.abs(valor) >= 100) {
        return 'R$ ' + Math.round(valor).toLocaleString('pt-BR');
    }
    return (
        'R$ ' +
        valor.toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        })
    );
}

function formatarMoedaTabela(valor) {
    if (!valor) return 'R$ 0,00';
    return (
        'R$ ' +
        valor.toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        })
    );
}

function formatarMoedaCompacta(valor) {
    if (!valor) return 'R$ 0';
    if (valor >= 1000000) {
        return 'R$ ' + (valor / 1000000).toFixed(1) + 'M';
    }
    if (valor >= 1000) {
        return 'R$ ' + (valor / 1000).toFixed(0) + 'K';
    }
    return 'R$ ' + Math.round(valor).toLocaleString('pt-BR');
}

function formatarNumero(valor) {
    if (!valor) return '0';
    if (Math.abs(valor) >= 100) {
        return Math.round(valor).toLocaleString('pt-BR');
    }
    return valor.toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });
}

function formatarNumeroK(valor) {
    if (!valor) return '0';
    if (valor >= 1000000) {
        const num = valor / 1000000;
        return num >= 100 ? num.toFixed(0) + 'M' : num.toFixed(2) + 'M';
    }
    if (valor >= 1000) {
        const num = valor / 1000;
        return num >= 100 ? num.toFixed(0) + 'K' : num.toFixed(2) + 'K';
    }
    return Math.round(valor).toLocaleString('pt-BR');
}

function formatarLitros(valor) {
    if (!valor) return '0 lt';
    // Formata número completo sem abreviação, com separador de milhar
    return Math.round(valor).toLocaleString('pt-BR') + ' lt';
}

function formatarLabelMoeda(valor) {
    if (!valor) return 'R$ 0';
    if (Math.abs(valor) >= 100) {
        return 'R$ ' + Math.round(valor).toLocaleString('pt-BR');
    }
    return (
        'R$ ' +
        valor.toLocaleString('pt-BR', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        })
    );
}

function formatarLabelNumero(valor) {
    if (!valor) return '0';
    if (Math.abs(valor) >= 100) {
        return Math.round(valor).toLocaleString('pt-BR');
    }
    return valor.toLocaleString('pt-BR', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    });
}

// ====== MAPAS DE CALOR ======

var heatmapDiaHora = null;
var heatmapCategoria = null;
var heatmapVeiculo = null;
var modalDetalhes = null;

// Paleta de cores do heatmap (mesma ordem usada no paletteSettings)
const HEATMAP_CORES = [
    '#f5ebe0',
    '#d4a574',
    '#c4956a',
    '#a8784c',
    '#8b5e3c',
    '#6d472c',
];

/**
 * Cria a legenda customizada com faixas de valores para o heatmap
 * @param {string} containerId - ID do container da legenda
 * @param {Array} dados - Array de valores do heatmap
 */
function criarLegendaHeatmap(containerId, dados) {
    const container = document.getElementById(containerId);
    if (!container) return;

    // Extrair todos os valores
    const valores = dados.flat().filter((v) => v > 0);
    if (valores.length === 0) {
        container.innerHTML =
            '<span class="text-muted" style="font-size: 0.75rem;">Sem dados para exibir legenda</span>';
        return;
    }

    const min = Math.min(...valores);
    const max = Math.max(...valores);
    const range = max - min;
    const steps = HEATMAP_CORES.length;
    const stepSize = range / (steps - 1);

    // Criar os itens da legenda
    let html = '';
    for (let i = 0; i < steps; i++) {
        const valorInicio = i === 0 ? 0 : min + stepSize * (i - 0.5);
        const valorFim = i === steps - 1 ? max : min + stepSize * (i + 0.5);

        let label;
        if (i === 0) {
            label = 'R$ 0';
        } else if (i === steps - 1) {
            label = '> R$ ' + formatarNumeroK(valorFim * 0.8);
        } else {
            label =
                'R$ ' +
                formatarNumeroK(valorInicio) +
                ' - ' +
                formatarNumeroK(valorFim);
        }

        html += `
            <div class="heatmap-legenda-item">
                <div class="heatmap-legenda-cor" style="background-color: ${HEATMAP_CORES[i]};"></div>
                <span class="heatmap-legenda-range">${label}</span>
            </div>
        `;
    }

    container.innerHTML = html;
}

/**
 * Renderiza o Mapa de Calor: Dia da Semana x Hora (Aba 1 - Consumo Geral)
 */
function renderizarHeatmapDiaHora(ano, mes) {
    try {
        const container = document.getElementById('heatmapDiaHora');
        if (!container) return;

        $.ajax({
            url: '/api/abastecimento/DashboardHeatmapHora',
            type: 'GET',
            data: { ano: ano || null, mes: mes || null },
            success: function (data) {
                try {
                    if (heatmapDiaHora) {
                        heatmapDiaHora.destroy();
                        heatmapDiaHora = null;
                    }
                    container.innerHTML = '';

                    // Preparar dados para o HeatMap
                    var heatmapData = [];
                    var diasSemana = data.xLabels;
                    var horas = data.yLabels;

                    // Criar matriz de dados
                    for (var i = 0; i < diasSemana.length; i++) {
                        var row = [];
                        for (var j = 0; j < horas.length; j++) {
                            var item = data.data.find(
                                (d) =>
                                    d.x === diasSemana[i] && d.y === horas[j],
                            );
                            row.push(item ? item.value : 0);
                        }
                        heatmapData.push(row);
                    }

                    heatmapDiaHora = new ej.heatmap.HeatMap({
                        titleSettings: { text: '' },
                        xAxis: {
                            labels: diasSemana,
                            textStyle: { size: '11px', fontFamily: 'Outfit' },
                        },
                        yAxis: {
                            labels: horas,
                            textStyle: { size: '9px', fontFamily: 'Outfit' },
                        },
                        dataSource: heatmapData,
                        cellSettings: {
                            showLabel: false,
                            border: { width: 1, color: 'white' },
                        },
                        paletteSettings: {
                            palette: [
                                { color: '#f5ebe0' },
                                { color: '#d4a574' },
                                { color: '#c4956a' },
                                { color: '#a8784c' },
                                { color: '#8b5e3c' },
                                { color: '#6d472c' },
                            ],
                            type: 'Gradient',
                        },
                        legendSettings: {
                            visible: false,
                        },
                        tooltipRender: function (args) {
                            // Usar xLabel e yLabel que contém os labels reais
                            var dia =
                                args.xLabel ||
                                (diasSemana[args.xValue] ?? 'N/A');
                            var hora =
                                args.yLabel || (horas[args.yValue] ?? 'N/A');
                            args.content = [
                                dia +
                                    ' às ' +
                                    hora +
                                    ': ' +
                                    formatarMoeda(args.value),
                            ];
                        },
                        cellClick: function (args) {
                            var diaSemana = args.xValue;
                            var hora = parseInt(horas[args.yValue]);
                            abrirModalDetalhes({
                                titulo:
                                    'Abastecimentos - ' +
                                    diasSemana[diaSemana] +
                                    ' às ' +
                                    horas[args.yValue],
                                ano: ano,
                                mes: mes,
                                diaSemana: diaSemana,
                                hora: hora,
                            });
                        },
                    });
                    heatmapDiaHora.appendTo('#heatmapDiaHora');

                    // Criar legenda customizada com faixas de valores
                    criarLegendaHeatmap('legendaHeatmapDiaHora', heatmapData);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'renderizarHeatmapDiaHora.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar mapa de calor:', error);
                container.innerHTML =
                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarHeatmapDiaHora',
            error,
        );
    }
}

/**
 * Renderiza o Mapa de Calor: Categoria x Mês (Aba 2 - Consumo Mensal)
 */
function renderizarHeatmapCategoria(ano) {
    try {
        const container = document.getElementById('heatmapCategoria');
        if (!container) return;

        $.ajax({
            url: '/api/abastecimento/DashboardHeatmapCategoria',
            type: 'GET',
            data: { ano: ano },
            success: function (data) {
                try {
                    if (heatmapCategoria) {
                        heatmapCategoria.destroy();
                        heatmapCategoria = null;
                    }
                    container.innerHTML = '';

                    if (!data.xLabels || data.xLabels.length === 0) {
                        container.innerHTML =
                            '<div class="text-center text-muted py-4">Sem dados de categorias</div>';
                        return;
                    }

                    // Preparar dados para o HeatMap
                    var categorias = data.xLabels;
                    var meses = data.yLabels;
                    var heatmapData = [];

                    for (var i = 0; i < categorias.length; i++) {
                        var row = [];
                        for (var j = 0; j < meses.length; j++) {
                            var item = data.data.find(
                                (d) =>
                                    d.x === categorias[i] && d.y === meses[j],
                            );
                            row.push(item ? item.value : 0);
                        }
                        heatmapData.push(row);
                    }

                    heatmapCategoria = new ej.heatmap.HeatMap({
                        titleSettings: { text: '' },
                        xAxis: {
                            labels: categorias,
                            textStyle: { size: '10px', fontFamily: 'Outfit' },
                        },
                        yAxis: {
                            labels: meses,
                            textStyle: { size: '10px', fontFamily: 'Outfit' },
                        },
                        dataSource: heatmapData,
                        cellSettings: {
                            showLabel: false,
                            border: { width: 1, color: 'white' },
                        },
                        paletteSettings: {
                            palette: [
                                { color: '#f5ebe0' },
                                { color: '#d4a574' },
                                { color: '#c4956a' },
                                { color: '#a8784c' },
                                { color: '#8b5e3c' },
                                { color: '#6d472c' },
                            ],
                            type: 'Gradient',
                        },
                        legendSettings: {
                            visible: false,
                        },
                        tooltipRender: function (args) {
                            // Usar xLabel e yLabel que contém os labels reais
                            var cat =
                                args.xLabel ||
                                (categorias[args.xValue] ?? 'N/A');
                            var mes =
                                args.yLabel || (meses[args.yValue] ?? 'N/A');
                            args.content = [
                                cat +
                                    ' - ' +
                                    mes +
                                    ': ' +
                                    formatarMoeda(args.value),
                            ];
                        },
                        cellClick: function (args) {
                            var categoria =
                                args.xLabel || categorias[args.xValue];
                            var mesIdx = args.yValue;
                            var mesNum = mesIdx + 1;
                            var mesNome = args.yLabel || meses[mesIdx];
                            abrirModalDetalhes({
                                titulo:
                                    'Abastecimentos - ' +
                                    categoria +
                                    ' em ' +
                                    mesNome,
                                ano: ano,
                                mes: mesNum,
                                categoria: categoria,
                            });
                        },
                    });
                    heatmapCategoria.appendTo('#heatmapCategoria');

                    // Criar legenda customizada com faixas de valores
                    criarLegendaHeatmap('legendaHeatmapCategoria', heatmapData);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'renderizarHeatmapCategoria.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar mapa de calor:', error);
                container.innerHTML =
                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarHeatmapCategoria',
            error,
        );
    }
}

/**
 * Renderiza o Mapa de Calor: Dia da Semana x Hora de um Veículo ou Modelo específico (Aba 3)
 */
function renderizarHeatmapVeiculo(ano, placa, tipoVeiculo) {
    try {
        const container = document.getElementById('heatmapVeiculo');
        const containerVazio = document.getElementById('heatmapVeiculoVazio');
        const legendaContainer = document.getElementById(
            'legendaHeatmapVeiculo',
        );
        if (!container) return;

        // Se não tem placa nem modelo selecionado, mostrar mensagem
        if (!placa && !tipoVeiculo) {
            container.style.display = 'none';
            if (legendaContainer) legendaContainer.innerHTML = '';
            if (containerVazio) containerVazio.style.display = 'block';
            return;
        }

        container.style.display = 'block';
        if (containerVazio) containerVazio.style.display = 'none';

        $.ajax({
            url: '/api/abastecimento/DashboardHeatmapVeiculo',
            type: 'GET',
            data: {
                ano: ano || null,
                placa: placa || null,
                tipoVeiculo: tipoVeiculo || null,
            },
            success: function (data) {
                try {
                    if (heatmapVeiculo) {
                        heatmapVeiculo.destroy();
                        heatmapVeiculo = null;
                    }
                    container.innerHTML = '';

                    if (
                        !data.xLabels ||
                        data.xLabels.length === 0 ||
                        !data.data ||
                        data.data.length === 0
                    ) {
                        container.innerHTML =
                            '<div class="text-center text-muted py-4">Sem dados de abastecimento para este veículo</div>';
                        return;
                    }

                    // Preparar dados para o HeatMap
                    var heatmapData = [];
                    var diasSemana = data.xLabels;
                    var horas = data.yLabels;

                    // Criar matriz de dados
                    for (var i = 0; i < diasSemana.length; i++) {
                        var row = [];
                        for (var j = 0; j < horas.length; j++) {
                            var item = data.data.find(
                                (d) =>
                                    d.x === diasSemana[i] && d.y === horas[j],
                            );
                            row.push(item ? item.value : 0);
                        }
                        heatmapData.push(row);
                    }

                    heatmapVeiculo = new ej.heatmap.HeatMap({
                        titleSettings: { text: '' },
                        xAxis: {
                            labels: diasSemana,
                            textStyle: { size: '11px', fontFamily: 'Outfit' },
                        },
                        yAxis: {
                            labels: horas,
                            textStyle: { size: '9px', fontFamily: 'Outfit' },
                        },
                        dataSource: heatmapData,
                        cellSettings: {
                            showLabel: false,
                            border: { width: 1, color: 'white' },
                        },
                        paletteSettings: {
                            palette: [
                                { color: '#f5ebe0' },
                                { color: '#d4a574' },
                                { color: '#c4956a' },
                                { color: '#a8784c' },
                                { color: '#8b5e3c' },
                                { color: '#6d472c' },
                            ],
                            type: 'Gradient',
                        },
                        legendSettings: {
                            visible: false,
                        },
                        tooltipRender: function (args) {
                            // Usar xLabel e yLabel que contém os labels reais
                            var dia =
                                args.xLabel ||
                                (diasSemana[args.xValue] ?? 'N/A');
                            var hora =
                                args.yLabel || (horas[args.yValue] ?? 'N/A');
                            args.content = [
                                dia +
                                    ' às ' +
                                    hora +
                                    ': ' +
                                    formatarMoeda(args.value),
                            ];
                        },
                        cellClick: function (args) {
                            var diaSemana = args.xValue;
                            var hora = parseInt(horas[args.yValue]);
                            var filtroLabel = placa || tipoVeiculo || 'Veículo';
                            abrirModalDetalhes({
                                titulo:
                                    'Abastecimentos - ' +
                                    filtroLabel +
                                    ' - ' +
                                    diasSemana[diaSemana] +
                                    ' às ' +
                                    horas[args.yValue],
                                ano: ano,
                                placa: placa || null,
                                tipoVeiculo: tipoVeiculo || null,
                                diaSemana: diaSemana,
                                hora: hora,
                            });
                        },
                    });
                    heatmapVeiculo.appendTo('#heatmapVeiculo');

                    // Criar legenda customizada com faixas de valores
                    criarLegendaHeatmap('legendaHeatmapVeiculo', heatmapData);
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'renderizarHeatmapVeiculo.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error(
                    'Erro ao carregar mapa de calor do veículo:',
                    error,
                );
                container.innerHTML =
                    '<div class="text-center text-muted py-4">Erro ao carregar mapa de calor</div>';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'renderizarHeatmapVeiculo',
            error,
        );
    }
}

// ====== MODAL DE DETALHES ======

/**
 * Abre o modal de detalhes com os abastecimentos filtrados
 */
function abrirModalDetalhes(filtros) {
    try {
        if (!modalDetalhes) {
            modalDetalhes = new bootstrap.Modal(
                document.getElementById('modalDetalhesAbast'),
            );
        }

        document.getElementById('modalDetalhesTitulo').textContent =
            filtros.titulo || 'Detalhes dos Abastecimentos';
        document.getElementById('detalhesGrid').innerHTML = `
            <div class="detalhes-grid-header">Data</div>
            <div class="detalhes-grid-header">Veículo</div>
            <div class="detalhes-grid-header">Litros</div>
            <div class="detalhes-grid-header">R$/Litro</div>
            <div class="detalhes-grid-header">Total</div>
            <div style="grid-column: span 5; text-align: center; padding: 30px;">
                <i class="fa-duotone fa-spinner-third fa-spin fa-2x"></i>
            </div>
        `;
        document.getElementById('detalhesVazio').style.display = 'none';
        document.getElementById('detalhesQtd').textContent = '...';
        document.getElementById('detalhesLitros').textContent = '...';
        document.getElementById('detalhesValor').textContent = '...';

        modalDetalhes.show();

        $.ajax({
            url: '/api/abastecimento/DashboardDetalhes',
            type: 'GET',
            data: {
                ano: filtros.ano || null,
                mes: filtros.mes || null,
                categoria: filtros.categoria || null,
                tipoVeiculo: filtros.tipoVeiculo || null,
                placa: filtros.placa || null,
                diaSemana:
                    filtros.diaSemana !== undefined ? filtros.diaSemana : null,
                hora: filtros.hora !== undefined ? filtros.hora : null,
            },
            success: function (data) {
                try {
                    document.getElementById('detalhesQtd').textContent =
                        data.totais.quantidade.toLocaleString('pt-BR');
                    document.getElementById('detalhesLitros').textContent =
                        formatarNumero(data.totais.litros) + ' L';
                    document.getElementById('detalhesValor').textContent =
                        formatarMoeda(data.totais.valor);

                    var gridHtml = `
                        <div class="detalhes-grid-header">Data</div>
                        <div class="detalhes-grid-header">Veículo</div>
                        <div class="detalhes-grid-header">Litros</div>
                        <div class="detalhes-grid-header">R$/Litro</div>
                        <div class="detalhes-grid-header">Total</div>
                    `;

                    if (data.registros && data.registros.length > 0) {
                        data.registros.forEach(function (reg) {
                            gridHtml += `
                                <div class="detalhes-grid-cell">${reg.data}</div>
                                <div class="detalhes-grid-cell">${reg.placa}</div>
                                <div class="detalhes-grid-cell">${formatarNumero(reg.litros)}</div>
                                <div class="detalhes-grid-cell">${formatarMoeda(reg.valorUnitario)}</div>
                                <div class="detalhes-grid-cell">${formatarMoeda(reg.valorTotal)}</div>
                            `;
                        });
                        document.getElementById('detalhesVazio').style.display =
                            'none';
                    } else {
                        document.getElementById('detalhesVazio').style.display =
                            'block';
                    }

                    document.getElementById('detalhesGrid').innerHTML =
                        gridHtml;
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'abrirModalDetalhes.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar detalhes:', error);
                document.getElementById('detalhesGrid').innerHTML = `
                    <div class="detalhes-grid-header">Data</div>
                    <div class="detalhes-grid-header">Veículo</div>
                    <div class="detalhes-grid-header">Litros</div>
                    <div class="detalhes-grid-header">R$/Litro</div>
                    <div class="detalhes-grid-header">Total</div>
                `;
                document.getElementById('detalhesVazio').style.display =
                    'block';
                document.getElementById('detalhesVazio').innerHTML =
                    '<i class="fa-duotone fa-circle-exclamation fa-2x mb-2"></i><div>Erro ao carregar detalhes</div>';
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'abrirModalDetalhes',
            error,
        );
    }
}

/**
 * Abre o modal com as viagens de um veículo específico
 * Usa o período (ano/mês) atualmente selecionado no filtro
 */
function abrirModalViagensVeiculo(veiculoId, placa) {
    try {
        // Obter ano e mês do filtro atual (aba Consumo Mensal)
        const ano =
            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
            new Date().getFullYear();
        const mes =
            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;

        // Mostrar loading na tabela
        const corpoTabela = document.getElementById('corpoTabelaViagens');
        if (corpoTabela) {
            corpoTabela.innerHTML = `
                <tr>
                    <td colspan="8" class="text-center py-4">
                        <i class="fa-duotone fa-spinner-third fa-spin fa-2x text-muted"></i>
                        <div class="mt-2 text-muted">Carregando viagens...</div>
                    </td>
                </tr>
            `;
        }

        // Abrir o modal
        const modal = new bootstrap.Modal(
            document.getElementById('modalViagensVeiculo'),
        );
        modal.show();

        // Buscar dados da API
        const corpoTabelaAbastecimentos = document.getElementById(
            'corpoTabelaAbastecimentos',
        );

        $.ajax({
            url: '/api/abastecimento/DashboardViagensVeiculo',
            type: 'GET',
            data: { veiculoId: veiculoId, ano: ano, mes: mes || null },
            success: function (data) {
                try {
                    // Preencher informações do veículo
                    document.getElementById('modalVeiculoPlaca').textContent =
                        data.veiculo?.placa || placa || '---';
                    document.getElementById('modalVeiculoModelo').textContent =
                        data.veiculo?.marcaModelo || '---';
                    document.getElementById(
                        'modalVeiculoCategoria',
                    ).textContent = data.veiculo?.categoria || '---';
                    document.getElementById('modalPeriodo').textContent =
                        data.periodo?.descricao || '---';

                    // Totais
                    document.getElementById('modalTotalViagens').textContent =
                        data.totais?.totalViagens || 0;
                    document.getElementById('modalTotalKm').textContent =
                        formatarNumeroInteiro(data.totais?.totalKmRodado || 0) +
                        ' km';
                    document.getElementById(
                        'modalTotalAbastecimentos',
                    ).textContent = data.totais?.totalAbastecimentos || 0;
                    document.getElementById('modalTotalLitros').textContent =
                        formatarNumero(data.totais?.totalLitros || 0);
                    document.getElementById('modalTotalValor').textContent =
                        formatarMoeda(
                            data.totais?.totalValorAbastecimentos || 0,
                        );

                    // Badges das abas
                    document.getElementById('badgeViagens').textContent =
                        data.totais?.totalViagens || 0;
                    document.getElementById('badgeAbastecimentos').textContent =
                        data.totais?.totalAbastecimentos || 0;

                    // Preencher tabela de VIAGENS
                    if (data.viagens && data.viagens.length > 0) {
                        let html = '';
                        data.viagens.forEach(function (v) {
                            const dataInicial = v.dataInicial
                                ? formatarData(v.dataInicial)
                                : '-';
                            const horaInicio = v.horaInicio
                                ? formatarHora(v.horaInicio)
                                : '-';
                            const dataFinal = v.dataFinal
                                ? formatarData(v.dataFinal)
                                : '-';
                            const horaFim = v.horaFim
                                ? formatarHora(v.horaFim)
                                : '-';
                            const kmRodadoClass =
                                v.kmRodado > 500
                                    ? 'text-danger fw-bold'
                                    : v.kmRodado > 200
                                      ? 'text-warning fw-bold'
                                      : '';

                            html += `
                                <tr>
                                    <td style="padding: 10px 15px; font-weight: 600; color: #325d88;">${v.noFichaVistoria || '-'}</td>
                                    <td style="padding: 10px 15px;">${dataInicial}</td>
                                    <td style="padding: 10px 15px;">${horaInicio}</td>
                                    <td style="padding: 10px 15px;">${dataFinal}</td>
                                    <td style="padding: 10px 15px;">${horaFim}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${formatarNumeroInteiro(v.kmInicial)}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${formatarNumeroInteiro(v.kmFinal)}</td>
                                    <td style="padding: 10px 15px; text-align: right; ${kmRodadoClass}">${formatarNumeroInteiro(v.kmRodado)}</td>
                                </tr>
                            `;
                        });
                        corpoTabela.innerHTML = html;
                    } else {
                        corpoTabela.innerHTML = `
                            <tr>
                                <td colspan="8" class="text-center py-4">
                                    <i class="fa-duotone fa-car-burst fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhuma viagem encontrada para este período</div>
                                </td>
                            </tr>
                        `;
                    }

                    // Preencher tabela de ABASTECIMENTOS
                    if (data.abastecimentos && data.abastecimentos.length > 0) {
                        let htmlAbast = '';
                        data.abastecimentos.forEach(function (a) {
                            const dataAbast = a.data
                                ? formatarData(a.data)
                                : '-';
                            htmlAbast += `
                                <tr>
                                    <td style="padding: 10px 15px;">${dataAbast}</td>
                                    <td style="padding: 10px 15px;">${a.combustivel || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #0ea5e9;">${a.litros || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${a.valorUnitario || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #dc2626;">${a.valorTotal || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${a.kmRodado || '-'}</td>
                                </tr>
                            `;
                        });
                        corpoTabelaAbastecimentos.innerHTML = htmlAbast;
                    } else {
                        corpoTabelaAbastecimentos.innerHTML = `
                            <tr>
                                <td colspan="6" class="text-center py-4">
                                    <i class="fa-duotone fa-gas-pump fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhum abastecimento encontrado para este período</div>
                                </td>
                            </tr>
                        `;
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'abrirModalViagensVeiculo.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados:', error);
                corpoTabela.innerHTML = `
                    <tr>
                        <td colspan="8" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados do veículo</div>
                        </td>
                    </tr>
                `;
                corpoTabelaAbastecimentos.innerHTML = `
                    <tr>
                        <td colspan="6" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados do veículo</div>
                        </td>
                    </tr>
                `;
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'abrirModalViagensVeiculo',
            error,
        );
    }
}

/**
 * Abre o modal com os abastecimentos de uma unidade específica
 * Usa o período (ano/mês) atualmente selecionado no filtro
 */
function abrirModalAbastecimentosUnidade(unidade) {
    try {
        // Obter ano e mês do filtro atual (aba Consumo Mensal)
        const ano =
            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
            new Date().getFullYear();
        const mes =
            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;

        // Mostrar loading nas tabelas
        const corpoTabelaAbast = document.getElementById(
            'corpoTabelaAbastecimentosUnidade',
        );
        const corpoTabelaResumo = document.getElementById(
            'corpoTabelaResumoVeiculos',
        );

        if (corpoTabelaAbast) {
            corpoTabelaAbast.innerHTML = `
                <tr>
                    <td colspan="7" class="text-center py-4">
                        <i class="fa-duotone fa-spinner-third fa-spin fa-2x text-muted"></i>
                        <div class="mt-2 text-muted">Carregando abastecimentos...</div>
                    </td>
                </tr>
            `;
        }
        if (corpoTabelaResumo) {
            corpoTabelaResumo.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center py-4">
                        <i class="fa-duotone fa-spinner-third fa-spin fa-2x text-muted"></i>
                        <div class="mt-2 text-muted">Carregando...</div>
                    </td>
                </tr>
            `;
        }

        // Abrir o modal
        const modal = new bootstrap.Modal(
            document.getElementById('modalAbastecimentosUnidade'),
        );
        modal.show();

        // Buscar dados da API
        $.ajax({
            url: '/api/abastecimento/DashboardAbastecimentosUnidade',
            type: 'GET',
            data: { unidade: unidade, ano: ano, mes: mes || null },
            success: function (data) {
                try {
                    // Preencher informações da unidade
                    document.getElementById('modalUnidadeNome').textContent =
                        data.unidade || unidade || '---';
                    document.getElementById('modalUnidadePeriodo').textContent =
                        data.periodo?.descricao || '---';

                    // Totais
                    document.getElementById(
                        'modalUnidadeTotalAbastecimentos',
                    ).textContent = data.totais?.totalAbastecimentos || 0;
                    document.getElementById(
                        'modalUnidadeTotalLitros',
                    ).textContent = formatarNumero(
                        data.totais?.totalLitros || 0,
                    );
                    document.getElementById(
                        'modalUnidadeTotalValor',
                    ).textContent = formatarMoeda(data.totais?.totalValor || 0);

                    // Badges das abas
                    document.getElementById(
                        'badgeAbastecimentosUnidade',
                    ).textContent = data.totais?.totalAbastecimentos || 0;
                    document.getElementById('badgeResumoVeiculos').textContent =
                        data.porVeiculo?.length || 0;

                    // Preencher tabela de ABASTECIMENTOS
                    if (data.abastecimentos && data.abastecimentos.length > 0) {
                        let html = '';
                        data.abastecimentos.forEach(function (a) {
                            const dataAbast = a.data
                                ? formatarData(a.data)
                                : '-';
                            html += `
                                <tr>
                                    <td style="padding: 10px 15px;">${dataAbast}</td>
                                    <td style="padding: 10px 15px; font-weight: 600; color: #325d88;">${a.placa || '-'}</td>
                                    <td style="padding: 10px 15px; font-size: 0.85rem; color: #666;">${a.tipoVeiculo || '-'}</td>
                                    <td style="padding: 10px 15px;">${a.combustivel || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #0ea5e9;">${a.litros || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${a.valorUnitario || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #dc2626;">${a.valorTotal || '-'}</td>
                                </tr>
                            `;
                        });
                        corpoTabelaAbast.innerHTML = html;
                    } else {
                        corpoTabelaAbast.innerHTML = `
                            <tr>
                                <td colspan="7" class="text-center py-4">
                                    <i class="fa-duotone fa-gas-pump fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhum abastecimento encontrado para este período</div>
                                </td>
                            </tr>
                        `;
                    }

                    // Preencher tabela de RESUMO POR VEÍCULO
                    if (data.porVeiculo && data.porVeiculo.length > 0) {
                        let htmlResumo = '';
                        data.porVeiculo.forEach(function (v) {
                            htmlResumo += `
                                <tr>
                                    <td style="padding: 10px 15px; font-weight: 600; color: #325d88;">${v.placa || '-'}</td>
                                    <td style="padding: 10px 15px; font-size: 0.85rem; color: #666;">${v.tipoVeiculo || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${v.qtdAbastecimentos || 0}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #0ea5e9;">${formatarNumero(v.litros || 0)}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #dc2626;">${formatarMoeda(v.valor || 0)}</td>
                                </tr>
                            `;
                        });
                        corpoTabelaResumo.innerHTML = htmlResumo;
                    } else {
                        corpoTabelaResumo.innerHTML = `
                            <tr>
                                <td colspan="5" class="text-center py-4">
                                    <i class="fa-duotone fa-cars fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhum veículo encontrado para este período</div>
                                </td>
                            </tr>
                        `;
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'abrirModalAbastecimentosUnidade.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados:', error);
                corpoTabelaAbast.innerHTML = `
                    <tr>
                        <td colspan="7" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados da unidade</div>
                        </td>
                    </tr>
                `;
                corpoTabelaResumo.innerHTML = `
                    <tr>
                        <td colspan="5" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados da unidade</div>
                        </td>
                    </tr>
                `;
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'abrirModalAbastecimentosUnidade',
            error,
        );
    }
}

/**
 * Abre o modal com os abastecimentos de uma categoria específica
 * Usa o período (ano/mês) atualmente selecionado no filtro
 */
function abrirModalAbastecimentosCategoria(categoria) {
    try {
        // Obter ano e mês do filtro atual (aba Consumo Mensal)
        const ano =
            parseInt(document.getElementById('filtroAnoMensal')?.value) ||
            new Date().getFullYear();
        const mes =
            parseInt(document.getElementById('filtroMesMensal')?.value) || 0;

        // Mostrar loading nas tabelas
        const corpoTabelaVeiculos = document.getElementById(
            'corpoTabelaVeiculosCategoria',
        );
        const corpoTabelaAbast = document.getElementById(
            'corpoTabelaAbastecimentosCategoria',
        );

        if (corpoTabelaVeiculos) {
            corpoTabelaVeiculos.innerHTML = `
                <tr>
                    <td colspan="5" class="text-center py-4">
                        <i class="fa-duotone fa-spinner-third fa-spin fa-2x text-muted"></i>
                        <div class="mt-2 text-muted">Carregando veículos...</div>
                    </td>
                </tr>
            `;
        }
        if (corpoTabelaAbast) {
            corpoTabelaAbast.innerHTML = `
                <tr>
                    <td colspan="7" class="text-center py-4">
                        <i class="fa-duotone fa-spinner-third fa-spin fa-2x text-muted"></i>
                        <div class="mt-2 text-muted">Carregando abastecimentos...</div>
                    </td>
                </tr>
            `;
        }

        // Abrir o modal
        const modal = new bootstrap.Modal(
            document.getElementById('modalAbastecimentosCategoria'),
        );
        modal.show();

        // Buscar dados da API
        $.ajax({
            url: '/api/abastecimento/DashboardAbastecimentosCategoria',
            type: 'GET',
            data: { categoria: categoria, ano: ano, mes: mes || null },
            success: function (data) {
                try {
                    // Preencher informações da categoria
                    document.getElementById('modalCategoriaNome').textContent =
                        data.categoria || categoria || '---';
                    document.getElementById(
                        'modalCategoriaPeriodo',
                    ).textContent = data.periodo?.descricao || '---';

                    // Totais
                    document.getElementById(
                        'modalCategoriaTotalVeiculos',
                    ).textContent = data.totais?.totalVeiculos || 0;
                    document.getElementById(
                        'modalCategoriaTotalAbastecimentos',
                    ).textContent = data.totais?.totalAbastecimentos || 0;
                    document.getElementById(
                        'modalCategoriaTotalLitros',
                    ).textContent = formatarNumero(
                        data.totais?.totalLitros || 0,
                    );
                    document.getElementById(
                        'modalCategoriaTotalValor',
                    ).textContent = formatarMoeda(data.totais?.totalValor || 0);

                    // Badges das abas
                    document.getElementById(
                        'badgeVeiculosCategoria',
                    ).textContent = data.porVeiculo?.length || 0;
                    document.getElementById(
                        'badgeAbastecimentosCategoria',
                    ).textContent = data.totais?.totalAbastecimentos || 0;

                    // Preencher tabela de VEÍCULOS DA CATEGORIA
                    if (data.porVeiculo && data.porVeiculo.length > 0) {
                        let htmlVeiculos = '';
                        data.porVeiculo.forEach(function (v) {
                            htmlVeiculos += `
                                <tr>
                                    <td style="padding: 10px 15px; font-weight: 600; color: #325d88;">${v.placa || '-'}</td>
                                    <td style="padding: 10px 15px; font-size: 0.85rem; color: #666;">${v.tipoVeiculo || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${v.qtdAbastecimentos || 0}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #0ea5e9;">${formatarNumero(v.litros || 0)}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #dc2626;">${formatarMoeda(v.valor || 0)}</td>
                                </tr>
                            `;
                        });
                        corpoTabelaVeiculos.innerHTML = htmlVeiculos;
                    } else {
                        corpoTabelaVeiculos.innerHTML = `
                            <tr>
                                <td colspan="5" class="text-center py-4">
                                    <i class="fa-duotone fa-cars fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhum veículo encontrado para este período</div>
                                </td>
                            </tr>
                        `;
                    }

                    // Preencher tabela de ABASTECIMENTOS
                    if (data.abastecimentos && data.abastecimentos.length > 0) {
                        let htmlAbast = '';
                        data.abastecimentos.forEach(function (a) {
                            const dataAbast = a.data
                                ? formatarData(a.data)
                                : '-';
                            htmlAbast += `
                                <tr>
                                    <td style="padding: 10px 15px;">${dataAbast}</td>
                                    <td style="padding: 10px 15px; font-weight: 600; color: #325d88;">${a.placa || '-'}</td>
                                    <td style="padding: 10px 15px; font-size: 0.85rem; color: #666;">${a.tipoVeiculo || '-'}</td>
                                    <td style="padding: 10px 15px;">${a.combustivel || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #0ea5e9;">${a.litros || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right;">${a.valorUnitario || '-'}</td>
                                    <td style="padding: 10px 15px; text-align: right; font-weight: 600; color: #dc2626;">${a.valorTotal || '-'}</td>
                                </tr>
                            `;
                        });
                        corpoTabelaAbast.innerHTML = htmlAbast;
                    } else {
                        corpoTabelaAbast.innerHTML = `
                            <tr>
                                <td colspan="7" class="text-center py-4">
                                    <i class="fa-duotone fa-gas-pump fa-2x text-muted mb-2"></i>
                                    <div class="text-muted">Nenhum abastecimento encontrado para este período</div>
                                </td>
                            </tr>
                        `;
                    }
                } catch (error) {
                    Alerta.TratamentoErroComLinha(
                        'dashboard-abastecimento.js',
                        'abrirModalAbastecimentosCategoria.success',
                        error,
                    );
                }
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados:', error);
                corpoTabelaVeiculos.innerHTML = `
                    <tr>
                        <td colspan="5" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados da categoria</div>
                        </td>
                    </tr>
                `;
                corpoTabelaAbast.innerHTML = `
                    <tr>
                        <td colspan="7" class="text-center py-4">
                            <i class="fa-duotone fa-circle-exclamation fa-2x text-danger mb-2"></i>
                            <div class="text-muted">Erro ao carregar dados da categoria</div>
                        </td>
                    </tr>
                `;
            },
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha(
            'dashboard-abastecimento.js',
            'abrirModalAbastecimentosCategoria',
            error,
        );
    }
}

/**
 * Formata data ISO para dd/mm/aaaa
 */
function formatarData(dataISO) {
    if (!dataISO) return '-';
    const data = new Date(dataISO);
    const dia = data.getDate().toString().padStart(2, '0');
    const mes = (data.getMonth() + 1).toString().padStart(2, '0');
    const ano = data.getFullYear();
    return `${dia}/${mes}/${ano}`;
}

/**
 * Formata hora ISO para HH:mm
 */
function formatarHora(dataISO) {
    if (!dataISO) return '-';
    const data = new Date(dataISO);
    const hora = data.getHours().toString().padStart(2, '0');
    const minuto = data.getMinutes().toString().padStart(2, '0');
    return `${hora}:${minuto}`;
}

/**
 * Formata número inteiro com separador de milhares
 */
function formatarNumeroInteiro(valor) {
    if (valor === null || valor === undefined) return '0';
    return valor.toLocaleString('pt-BR');
}
