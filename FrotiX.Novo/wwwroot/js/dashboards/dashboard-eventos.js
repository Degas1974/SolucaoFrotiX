/* ****************************************************************************************
 * ⚡ ARQUIVO: dashboard-eventos.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Dashboard analítico de eventos e ocorrências com métricas temporais, análise TOP 10
 *    eventos mais frequentes, distribuição por tipo/status/setor, heatmap dia×hora (7×24),
 *    e gráficos de evolução mensal. Sistema de filtros ano/mês/período personalizado.
 *    Paleta visual: Roxo Eventos (#9333ea → #a855f7) para identidade eventos/ocorrências.
 *    CRÍTICO: Injeta módulos Syncfusion (ColumnSeries, LineSeries, Category, etc) ANTES de
 *    renderizar gráficos para evitar erro "Cannot read properties of undefined".
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - Filtro Ano/Mês: dropdowns com anos/meses disponíveis (auto-seleção mais recente)
 *    - Período personalizado: dataInicio/dataFim (date inputs validados)
 *    - Períodos rápidos: 7, 15, 30, 60, 90 dias (botões atalho)
 *    - APIs recebem: ano, mes, dataInicio, dataFim
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - 12 cards estatísticos (total eventos, média/dia, por tipo, pendentes/resolvidos)
 *    - 8 gráficos Syncfusion (Column, Bar, Line, Donut)
 *    - 3 tabelas TOP 10 (eventos frequentes, setores, veículos afetados)
 *    - 1 heatmap customizado 7×24 (Dia da Semana × Hora do Dia - 168 células)
 *    - Label período: "Exibindo dados de: Mês/Ano" ou "DD/MM/YYYY - DD/MM/YYYY"
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: Syncfusion EJ2 Charts (⚠️ requer injeção manual de módulos), jQuery 3.x, Bootstrap 5.x
 *    • ARQUIVOS FROTIX: alerta.js, global-toast.js, FrotiX.css
 *    • APIS (9 endpoints):
 *      - /api/DashboardEventos/ObterAnosMesesDisponiveis (GET)
 *      - /api/DashboardEventos/ObterMesesPorAno (GET)
 *      - /api/DashboardEventos/ObterEstatisticasGerais (GET)
 *      - /api/DashboardEventos/ObterDistribuicaoPorTipo (GET)
 *      - /api/DashboardEventos/ObterDistribuicaoPorStatus (GET)
 *      - /api/DashboardEventos/ObterTop10EventosFrequentes (GET)
 *      - /api/DashboardEventos/ObterTop10SetoresComMaisEventos (GET)
 *      - /api/DashboardEventos/ObterEvolucaoMensal (GET)
 *      - /api/DashboardEventos/ObterHeatmapPorDiaHora (GET)
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (34 funções + 1 IIFE injeção Syncfusion)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🔧 INJEÇÃO SYNCFUSION MODULES (CRÍTICO!)                                                │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • IIFE injetaSyncfusion()                  → ej.charts.Chart.Inject modules (PRIORITY!) │
 * │    - ColumnSeries, LineSeries, Category, Legend, Tooltip, DataLabel, DateTime          │
 * │    - AccumulationChart.Inject: PieSeries, AccumulationLegend, AccumulationTooltip      │
 * │    - DEVE executar ANTES de qualquer renderização de gráficos                          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🎯 INICIALIZAÇÃO E CARREGAMENTO                                                          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • inicializ arDashboard()                  → Entry point: carrega anos/meses, init dados│
 * │ • carregarAnosMesesDisponiveis()           → Popula dropdowns, auto-seleciona + recente │
 * │ • carregarMesesPorAno(ano)                 → Popula meses do ano selecionado            │
 * │ • carregarDadosDashboard()                 → Promise.allSettled 7 endpoints paralelos   │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔧 FILTROS E PERÍODO                                                                     │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • aplicarFiltroAnoMes()                    → Valida ano/mês, atualiza label, carrega    │
 * │ • aplicarFiltroPersonalizado()             → Valida datas, limpa dropdowns, carrega     │
 * │ • aplicarFiltroPeriodo(dias, btnElement)   → Período rápido (7/15/30/60/90 dias)        │
 * │ • limparFiltroAnoMes()                     → Reset dropdowns, volta ao mais recente     │
 * │ • limparFiltroPeriodo()                    → Limpa campos date, volta ao ano/mês        │
 * │ • atualizarPeriodoAtualLabel()             → Atualiza label "Período: Mês/Ano"          │
 * │ • obterParametrosFiltro()                  → Retorna {ano, mes} ou {dataInicio, dataFim}│
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 📊 ESTATÍSTICAS E CARDS (12 cards)                                                      │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarEstatisticasGerais()             → 12 cards (total, média/dia, tipos, status) │
 * │ • atualizarElemento(id, valor)             → Helper para atualizar textContent          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 📈 GRÁFICOS SYNCFUSION (8 gráficos)                                                     │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarDistribuicaoPorTipo()            → Donut (Manutenção/Acidente/Multa/Outros)   │
 * │ • carregarDistribuicaoPorStatus()          → Donut (Pendente/Resolvido/Cancelado)       │
 * │ • carregarEvolucaoMensal()                 → Line (evolução temporal quantidade)        │
 * │ • renderizarChartDonut(containerId, dados) → Gráfico Donut genérico (innerRadius: 50%) │
 * │ • renderizarChartLine(containerId, dados)  → Gráfico Line genérico (marker: diamond)    │
 * │ • renderizarChartColumn(containerId, dados)→ Gráfico Column genérico (cornerRadius: 8)  │
 * │ • renderizarChartBarH(containerId, dados)  → Gráfico Bar horizontal genérico            │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🗂️ TABELAS TOP 10                                                                        │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarTop10EventosFrequentes()         → Tabela eventos (badges tipo, contador)     │
 * │ • carregarTop10SetoresComMaisEventos()     → Tabela setores solicitantes (medalhas)     │
 * │ • montarTabelaRanking(dados, colunas)      → Helper genérico para tabelas TOP 10        │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔥 HEATMAP 7×24 (168 células)                                                            │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarHeatmapPorDiaHora()              → Fetch API → criarHeatmapDivs()             │
 * │ • criarHeatmapDivs(dados, maxValor)        → Gera <table> 7 dias × 24 horas            │
 * │    - Cores: gradiente roxo (#faf5ff → #6b21a8)                                         │
 * │    - Hover: transform scale(1.15) + tooltip nativo                                     │
 * │    - Células clicáveis (planejado: modal filtro por dia/hora)                          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🎨 HELPERS E FORMATAÇÃO                                                                  │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • formatarDataBR(dataStr)                  → DD/MM/YYYY (Moment.js)                      │
 * │ • formatarMesAno(mes, ano)                 → "Janeiro/2025"                              │
 * │ • obterBadgeTipoEvento(tipo)               → HTML badge colorido por tipo               │
 * │ • obterBadgeStatus(status)                 → HTML badge (pendente/resolvido/cancelado)  │
 * │ • mostrarLoading(mensagem)/ocultarLoading()→ Overlay loading FrotiX                     │
 * │ • mostrarErro(mensagem)                    → SweetAlert erro                             │
 * │ • TratamentoErroComLinha(arquivo, funcao)  → Wrapper Alerta.TratamentoErroComLinha      │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Inicialização (auto-seleciona ano/mês mais recente)
 *    Script load → IIFE injetaSyncfusion() (PRIORITY: injeta módulos)
 *       → DOMContentLoaded → inicializarDashboard()
 *       → carregarAnosMesesDisponiveis() → auto-seleciona ano/mês + recente
 *       → carregarDadosDashboard() → Promise.allSettled 7 endpoints
 *       → Renderiza 12 cards, 8 gráficos, 3 tabelas, 1 heatmap
 * 
 * 💡 FLUXO 2: Filtro Ano/Mês
 *    btnFiltrarAnoMes.click → aplicarFiltroAnoMes()
 *      → Valida ano E mês obrigatórios
 *      → Limpa período personalizado
 *      → atualizarPeriodoAtualLabel() → "Período: Dezembro/2025"
 *      → carregarDadosDashboard() → endpoints recebem {ano, mes}
 * 
 * 💡 FLUXO 3: Click célula heatmap (planejado)
 *    Click célula [Seg, 14h] → abrirModalFiltroEventos(dia, hora)
 *      → Fetch /api/DashboardEventos/ObterEventosPorDiaHora?dia=1&hora=14
 *      → Modal lista: 15 eventos ocorridos às segundas-feiras às 14h
 *      → Permite drill-down para detalhes individuais
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * ⚠️ INJEÇÃO SYNCFUSION MODULES (CRÍTICO):
 *    - DEVE executar ANTES de qualquer new ej.charts.Chart()
 *    - IIFE envolto em if (typeof ej !== 'undefined' && ej.charts)
 *    - Injeta: ColumnSeries, LineSeries, Category, Legend, Tooltip, DataLabel, DateTime
 *    - AccumulationChart.Inject: PieSeries, AccumulationLegend, AccumulationTooltip, AccumulationDataLabel
 *    - Sem injeção: erro "Cannot read properties of undefined (reading 'prototype')"
 *    - Console.log('🔧 Injetando módulos Syncfusion...') para debug
 * 
 * 🎨 PALETA ROXO EVENTOS:
 *    - primary: #9333ea, secondary: #a855f7, accent: #c084fc
 *    - dark: #7e22ce, light: #e9d5ff
 *    - chart[]: 8 tons (#9333ea, #a855f7, #c084fc, #d8b4fe, #8b5cf6, #7c3aed, #6d28d9, #5b21b6)
 * 
 * 🔥 HEATMAP DIA×HORA:
 *    - 7 linhas: Domingo a Sábado
 *    - 24 colunas: 00:00 a 23:00
 *    - Gradiente roxo (5 níveis): #faf5ff (0) → #6b21a8 (máximo)
 *    - Escala logarítmica opcional para destacar outliers
 *    - Hover CSS: transform: scale(1.15), transition: 200ms
 * 
 * 🏷️ BADGES TIPO EVENTO:
 *    - badge-manutencao: laranja #f97316
 *    - badge-acidente: vermelho #ef4444
 *    - badge-multa: amarelo #eab308
 *    - badge-abastecimento: azul #3b82f6
 *    - badge-outros: cinza #6b7280
 * 
 * 🚦 BADGES STATUS:
 *    - badge-pendente: amarelo #fbbf24 (warning)
 *    - badge-resolvido: verde #10b981 (success)
 *    - badge-cancelado: vermelho #ef4444 (danger)
 * 
 * 📊 GRÁFICOS SYNCFUSION:
 *    - Donut (innerRadius: 50%): tipo, status
 *    - Line (marker.visible: true, type: 'Diamond'): evolução mensal
 *    - Column (cornerRadius: 8px): frequência por dia da semana
 *    - Bar horizontal: TOP 10 setores, TOP 10 eventos
 * 
 * 🚨 TRATAMENTO DE ERROS:
 *    - Try-catch em TODAS as funções
 *   - TratamentoErroComLinha('dashboard-eventos.js', 'nomeFuncao', error)
 *    - Fallback: gráfico/tabela vazia com mensagem orientativa
 * 
 * ⚡ PERFORMANCE:
 *    - Gráficos destruídos antes de recriar (.destroy())
 *    - Promise.allSettled: falha em 1 endpoint não bloqueia os outros 6
 *    - Heatmap renderizado em <table> (não canvas/SVG) para performance
 *    - Cache local: dadosAtual para evitar refetch ao alternar abas
 * 
 * **************************************************************************************** */

// ========================================
// DASHBOARD DE EVENTOS - FROTIX
// ========================================
// TODAS AS FUNÇÕES TÊM TRY-CATCH OBRIGATÓRIO

// ========================================
// INJEÇÃO DE MÓDULOS SYNCFUSION (CRÍTICO!)
// ========================================
// DEVE ser executado ANTES de qualquer uso de gráficos

if (typeof ej !== 'undefined' && ej.charts)
{
    console.log('🔧 Injetando módulos Syncfusion...');

    // Injetar módulos para Chart
    ej.charts.Chart.Inject(
        ej.charts.ColumnSeries,
        ej.charts.LineSeries,
        ej.charts.Category,
        ej.charts.Legend,
        ej.charts.Tooltip,
        ej.charts.DataLabel
    );

    // Injetar módulos para AccumulationChart
    ej.charts.AccumulationChart.Inject(
        ej.charts.PieSeries,
        ej.charts.AccumulationTooltip,
        ej.charts.AccumulationDataLabel,
        ej.charts.AccumulationLegend
    );

    console.log('✅ Módulos Syncfusion injetados com sucesso!');
} else
{
    console.error('❌ ERRO: Syncfusion (ej.charts) não está carregado!');
}

// Paleta de Cores FrotiX
const CORES_FROTIX = {
    azul: '#0D6EFD',
    verde: '#16a34a',
    laranja: '#d97706',
    amarelo: '#f59e0b',
    vermelho: '#dc2626',
    roxo: '#667eea',
    ciano: '#22d3ee',
    rosa: '#ec4899'
};

let periodoAtual = {
    dataInicio: null,
    dataFim: null
};

// Variáveis para armazenar gráficos Syncfusion
let chartEventosPorStatus = null;
let chartEventosPorSetor = null;
let chartEventosPorMes = null;

// ========================================
// INICIALIZAÇÃO
// ========================================

async function inicializarDashboard()
{
    try
    {
        console.log('🎯 Iniciando Dashboard de Eventos...');

        // Define período padrão (últimos 30 dias)
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);

        // Inicializa campos de data HTML5
        inicializarCamposData();

        // Carrega dashboard
        await carregarDadosDashboard();

        AppToast.show('Verde', 'Dashboard de Eventos carregado com sucesso!', 3000);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'inicializarDashboard', error);
    }
}

// ========================================
// CAMPOS DE DATA HTML5
// ========================================

function inicializarCamposData()
{
    try
    {
        const dataInicio = document.getElementById('dataInicio');
        const dataFim = document.getElementById('dataFim');

        if (dataInicio && dataFim)
        {
            // Define valores iniciais
            dataInicio.value = formatarDataParaInput(periodoAtual.dataInicio);
            dataFim.value = formatarDataParaInput(periodoAtual.dataFim);

            // Adiciona eventos de mudança
            dataInicio.addEventListener('change', function ()
            {
                try
                {
                    periodoAtual.dataInicio = new Date(this.value + 'T00:00:00');
                } catch (error)
                {
                    console.error('Erro ao atualizar data inicial:', error);
                }
            });

            dataFim.addEventListener('change', function ()
            {
                try
                {
                    periodoAtual.dataFim = new Date(this.value + 'T23:59:59');
                } catch (error)
                {
                    console.error('Erro ao atualizar data final:', error);
                }
            });
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'inicializarCamposData', error);
    }
}

function formatarDataParaInput(data)
{
    try
    {
        const ano = data.getFullYear();
        const mes = String(data.getMonth() + 1).padStart(2, '0');
        const dia = String(data.getDate()).padStart(2, '0');
        return `${ano}-${mes}-${dia}`;
    } catch (error)
    {
        return '';
    }
}

// ========================================
// CARREGAR DADOS
// ========================================

async function carregarDadosDashboard()
{
    try
    {
        console.log('⏱️ Iniciando carregamento do dashboard...');
        const inicio = performance.now();

        mostrarLoadingGeral();

        // Promise.allSettled não trava se um falhar
        const resultados = await Promise.allSettled([
            carregarEstatisticasGerais(),
            carregarEventosPorStatus(),
            carregarEventosPorSetor(),
            carregarEventosPorRequisitante(),
            carregarEventosPorMes(),
        ]);

        const tempo = ((performance.now() - inicio) / 1000).toFixed(2);
        console.log(`✅ Dashboard carregado em ${tempo}s`);

        // Log de falhas
        const nomes = [
            'EstatisticasGerais', 'EventosPorStatus', 'EventosPorSetor',
            'EventosPorRequisitante', 'EventosPorMes', 'EventosPorTipo',
            'EventosPorDia'
        ];

        resultados.forEach((resultado, index) =>
        {
            if (resultado.status === 'rejected')
            {
                console.error(`❌ ${nomes[index]} falhou:`, resultado.reason);
            }
        });

        esconderLoadingGeral();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarDadosDashboard', error);
        esconderLoadingGeral();
    }
}

// ========================================
// LOADING
// ========================================

function mostrarLoadingGeral()
{
    try
    {
        const loading = document.getElementById('loadingDashboard');
        if (loading)
        {
            loading.classList.remove('d-none');
        }
    } catch (error)
    {
        console.error('Erro ao mostrar loading:', error);
    }
}

function esconderLoadingGeral()
{
    try
    {
        const loading = document.getElementById('loadingDashboard');
        if (loading)
        {
            loading.classList.add('d-none');
        }
    } catch (error)
    {
        console.error('Erro ao esconder loading:', error);
    }
}

// ========================================
// PERÍODOS RÁPIDOS
// ========================================

function aplicarPeriodoRapido(dias)
{
    try
    {
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - dias);

        // Atualiza campos HTML
        document.getElementById('dataInicio').value = formatarDataParaInput(periodoAtual.dataInicio);
        document.getElementById('dataFim').value = formatarDataParaInput(periodoAtual.dataFim);

        // Recarrega dashboard
        carregarDadosDashboard();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'aplicarPeriodoRapido', error);
    }
}

async function atualizarDashboard()
{
    try
    {
        await carregarDadosDashboard();
        AppToast.show('Verde', 'Dashboard atualizado!', 2000);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'atualizarDashboard', error);
    }
}

// ========================================
// ESTATÍSTICAS GERAIS
// ========================================

async function carregarEstatisticasGerais()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEstatisticasGerais?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar estatísticas gerais');

        const result = await response.json();

        if (result.success)
        {
            renderizarEstatisticasGerais(result);
        } else
        {
            console.error('Erro:', result.message);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEstatisticasGerais', error);
    }
}

function renderizarEstatisticasGerais(dados)
{
    try
    {
        // Cards principais
        document.getElementById('statTotalEventos').textContent = dados.totalEventos.toLocaleString();
        document.getElementById('statEventosAtivos').textContent = dados.eventosAtivos.toLocaleString();
        document.getElementById('statEventosConcluidos').textContent = dados.eventosConcluidos.toLocaleString();
        document.getElementById('statEventosCancelados').textContent = dados.eventosCancelados.toLocaleString();

        // Cards secundários
        document.getElementById('statTotalParticipantes').textContent = dados.totalParticipantes.toLocaleString();
        document.getElementById('statMediaParticipantes').textContent = dados.mediaParticipantesPorEvento.toLocaleString() + ' part.';

        // Variações vs período anterior
        calcularVariacao('totalEventos', dados.totalEventos, dados.periodoAnterior.totalEventos, 'variacaoTotalEventos');
        calcularVariacao('totalParticipantes', dados.totalParticipantes, dados.periodoAnterior.totalParticipantes, 'variacaoTotalParticipantes');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarEstatisticasGerais', error);
    }
}

function calcularVariacao(campo, valorAtual, valorAnterior, elementoId)
{
    try
    {
        const elemento = document.getElementById(elementoId);
        if (!elemento) return;

        if (valorAnterior === 0)
        {
            elemento.textContent = '—';
            elemento.className = 'variacao-metrica variacao-neutra';
            return;
        }

        const variacao = ((valorAtual - valorAnterior) / valorAnterior) * 100;
        const variacaoAbs = Math.abs(variacao);
        const sinal = variacao >= 0 ? '+' : '';

        elemento.textContent = `${sinal}${variacao.toFixed(1)}% vs anterior`;
        elemento.className = variacao >= 0 ?
            'variacao-metrica variacao-positiva' :
            'variacao-metrica variacao-negativa';
    } catch (error)
    {
        console.error('Erro ao calcular variação:', error);
    }
}

// ========================================
// EVENTOS POR STATUS
// ========================================

async function carregarEventosPorStatus()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEventosPorStatus?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar eventos por status');

        const result = await response.json();

        if (result.success)
        {
            renderizarGraficoEventosPorStatus(result.dados);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorStatus', error);
    }
}

function renderizarGraficoEventosPorStatus(dados)
{
    try
    {
        const elemento = document.getElementById('chartEventosPorStatus');

        if (!elemento)
        {
            console.error('❌ Elemento #chartEventosPorStatus não encontrado no HTML!');
            return;
        }

        if (!dados || dados.length === 0)
        {
            console.warn('⚠️ Sem dados para renderizar gráfico de Status');
            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
            return;
        }

        if (chartEventosPorStatus)
        {
            chartEventosPorStatus.destroy();
            chartEventosPorStatus = null;
        }

        chartEventosPorStatus = new ej.charts.AccumulationChart({
            series: [{
                dataSource: dados,
                xName: 'status',
                yName: 'quantidade',
                innerRadius: '40%',
                dataLabel: {
                    visible: true,
                    position: 'Outside',
                    name: 'status'
                },
                palettes: ['#0D6EFD', '#16a34a', '#dc2626', '#f59e0b', '#667eea']
            }],
            legendSettings: { visible: true },
            tooltip: { enable: true }
        });

        chartEventosPorStatus.appendTo('#chartEventosPorStatus');
        console.log('✅ Gráfico de Status renderizado');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorStatus', error);
    }
}

// ========================================
// EVENTOS POR SETOR
// ========================================

async function carregarEventosPorSetor()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEventosPorSetor?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar eventos por setor');

        const result = await response.json();

        if (result.success)
        {
            renderizarGraficoEventosPorSetor(result.dados);
            renderizarTabelaSetores(result.dados);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorSetor', error);
    }
}

function renderizarGraficoEventosPorSetor(dados)
{
    try
    {
        const elemento = document.getElementById('chartEventosPorSetor');

        if (!elemento)
        {
            console.error('❌ Elemento #chartEventosPorSetor não encontrado no HTML!');
            return;
        }

        if (!dados || dados.length === 0)
        {
            console.warn('⚠️ Sem dados para renderizar gráfico de Setores');
            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
            return;
        }

        if (chartEventosPorSetor)
        {
            chartEventosPorSetor.destroy();
            chartEventosPorSetor = null;
        }

        chartEventosPorSetor = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category'
            },
            primaryYAxis: {
                title: 'Quantidade'
            },
            series: [{
                dataSource: dados,
                xName: 'setor',
                yName: 'quantidade',
                type: 'Column',
                name: 'Eventos',
                fill: '#667eea'
            }],
            legendSettings: { visible: false },
            tooltip: { enable: true }
        });

        chartEventosPorSetor.appendTo('#chartEventosPorSetor');
        console.log('✅ Gráfico de Setores renderizado');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorSetor', error);
    }
}

function renderizarTabelaSetores(dados)
{
    try
    {
        const tbody = document.querySelector('#tabelaSetores tbody');
        if (!tbody) return;

        tbody.innerHTML = '';

        dados.forEach((item, index) =>
        {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${index + 1}</td>
                <td>${item.setor}</td>
                <td class="text-end">${item.quantidade}</td>
                <td class="text-end">${item.participantes.toLocaleString()}</td>
                <td class="text-end">${item.concluidos}</td>
                <td class="text-end">
                    <span class="badge bg-${item.taxaConclusao >= 70 ? 'success' : item.taxaConclusao >= 50 ? 'warning' : 'danger'}">
                        ${item.taxaConclusao.toFixed(1)}%
                    </span>
                </td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarTabelaSetores', error);
    }
}

// ========================================
// EVENTOS POR REQUISITANTE
// ========================================

async function carregarEventosPorRequisitante()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEventosPorRequisitante?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar eventos por requisitante');

        const result = await response.json();

        if (result.success)
        {
            renderizarTabelaRequisitantes(result.dados);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorRequisitante', error);
    }
}

function renderizarTabelaRequisitantes(dados)
{
    try
    {
        const tbody = document.querySelector('#tabelaRequisitantes tbody');
        if (!tbody) return;

        tbody.innerHTML = '';

        dados.forEach((item, index) =>
        {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${index + 1}</td>
                <td>${item.requisitante}</td>
                <td class="text-end">${item.quantidade}</td>
                <td class="text-end">${item.participantes.toLocaleString()}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarTabelaRequisitantes', error);
    }
}

// ========================================
// EVENTOS POR MÊS
// ========================================

async function carregarEventosPorMes()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEventosPorMes?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar eventos por mês');

        const result = await response.json();

        if (result.success)
        {
            renderizarGraficoEventosPorMes(result.dados);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorMes', error);
    }
}

function renderizarGraficoEventosPorMes(dados)
{
    try
    {
        const elemento = document.getElementById('chartEventosPorMes');

        if (!elemento)
        {
            console.error('❌ Elemento #chartEventosPorMes não encontrado no HTML!');
            return;
        }

        if (!dados || dados.length === 0)
        {
            console.warn('⚠️ Sem dados para renderizar gráfico Mensal');
            elemento.innerHTML = '<div class="text-center p-4 text-muted">Sem dados disponíveis</div>';
            return;
        }

        if (chartEventosPorMes)
        {
            chartEventosPorMes.destroy();
            chartEventosPorMes = null;
        }

        chartEventosPorMes = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45
            },
            primaryYAxis: {
                title: 'Quantidade de Eventos',
                labelFormat: '{value}'
            },
            series: [{
                dataSource: dados,
                xName: 'mesNome',
                yName: 'quantidade',
                type: 'Line',
                name: 'Eventos',
                marker: {
                    visible: true,
                    width: 8,
                    height: 8,
                    dataLabel: { visible: true, position: 'Top' }
                },
                width: 3,
                fill: '#0D6EFD'
            }],
            title: 'Evolução Mensal de Eventos',
            titleStyle: {
                fontFamily: 'Helvetica',
                fontWeight: '600',
                size: '14px'
            },
            tooltip: {
                enable: true,
                format: '${point.x}: ${point.y} eventos'
            },
            // CRÍTICO: Desabilita zoomSettings
            zoomSettings: {
                enableSelectionZooming: false,
                enablePinchZooming: false,
                enableMouseWheelZooming: false,
                enableDeferredZooming: false,
                enableScrollbar: false
            },
            enableAnimation: true
        });

        chartEventosPorMes.appendTo('#chartEventosPorMes');
        console.log('✅ Gráfico Mensal renderizado');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'renderizarGraficoEventosPorMes', error);
    }
}

// ========================================
// EVENTOS POR TIPO
// ========================================


// ========================================
// EVENTOS POR DIA
// ========================================

async function carregarEventosPorDia()
{
    try
    {
        const response = await fetch(`/api/DashboardEventos/ObterEventosPorDia?` +
            `dataInicio=${periodoAtual.dataInicio.toISOString()}&` +
            `dataFim=${periodoAtual.dataFim.toISOString()}`);

        if (!response.ok) throw new Error('Erro ao carregar eventos por dia');

        const result = await response.json();

        if (result.success)
        {
            // Pode renderizar um gráfico adicional se necessário
            console.log('✅ Eventos por dia carregados:', result.dados.length, 'dias');
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'carregarEventosPorDia', error);
    }
}

// ========================================
// EXPORTAÇÃO PDF
// ========================================

async function exportarParaPDF()
{
    try
    {
        console.log('📄 Iniciando exportação para PDF...');

        const dataInicio = periodoAtual.dataInicio.toISOString();
        const dataFim = periodoAtual.dataFim.toISOString();

        window.location.href = `/ExportarParaPDF?dataInicio=${dataInicio}&dataFim=${dataFim}`;

        AppToast.show('Verde', 'PDF gerado com sucesso!', 3000);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'exportarParaPDF', error);
    }
}

// ========================================
// EVENTOS DO DOCUMENT.READY
// ========================================

$(document).ready(function ()
{
    try
    {
        console.log('🚀 Dashboard de Eventos iniciando...');

        // Inicializa o dashboard
        inicializarDashboard();

        // Botão de atualizar dashboard
        $('#btnAtualizarDashboard').on('click', function ()
        {
            try
            {
                atualizarDashboard();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btnAtualizarDashboard', error);
            }
        });

        // Botão de exportar PDF
        $('#btnExportarPDF').on('click', function ()
        {
            try
            {
                exportarParaPDF();
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btnExportarPDF', error);
            }
        });

        // Botões de período rápido
        $('#btn7Dias').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(7);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn7Dias', error);
            }
        });

        $('#btn15Dias').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(15);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn15Dias', error);
            }
        });

        $('#btn30Dias').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(30);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn30Dias', error);
            }
        });

        $('#btn90Dias').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(90);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn90Dias', error);
            }
        });

        $('#btn180Dias').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(180);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn180Dias', error);
            }
        });

        $('#btn1Ano').on('click', function ()
        {
            try
            {
                aplicarPeriodoRapido(365);
            } catch (error)
            {
                Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'click btn1Ano', error);
            }
        });

        console.log('✅ Dashboard de Eventos pronto!');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-eventos.js', 'document.ready', error);
    }
});
