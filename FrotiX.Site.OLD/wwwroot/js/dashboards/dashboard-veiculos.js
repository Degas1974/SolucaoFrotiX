/* ****************************************************************************************
 * ⚡ ARQUIVO: dashboard-veiculos.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Dashboard analítico de gestão da frota com foco em veículos. Apresenta visão geral da
 *    frota (ativo/inativo/reserva/efetivo), métricas de uso (viagens/km/abastecimentos),
 *    análise de custos mensais (abastecimento/manutenção) e comparativos por categoria.
 *    Sistema de abas permite alternar entre: Visão Geral, Uso dos Veículos e Custos.
 *    Paleta visual: Verde Sage (#5f8575) para harmonia com identidade FrotiX Frota.
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - Aba "Uso": ano (dropdown), mês (dropdown), período personalizado (date inputs)
 *    - Aba "Custos": ano (dropdown para filtrar custos anuais)
 *    - Botões período rápido: 7, 15, 30, 60, 90 dias (apenas Aba Uso)
 *    - Filtros aceitos por APIs: ano, mes, dataInicio, dataFim
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - 14 gráficos Syncfusion (Donut, Column, Bar, Area, Line, Grouped)
 *    - 24 cards estatísticos (composição frota, totais uso, custos)
 *    - 7 tabelas grid customizadas (TOP KM, TOP Viagens, TOP Consumo)
 *    - Filtros dinâmicos com auto-seleção do ano/mês mais recente
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS: Syncfusion EJ2 Charts, jQuery 3.x, Bootstrap 5.x
 *    • ARQUIVOS FROTIX: alerta.js, global-toast.js, FrotiX.css
 *    • APIS:
 *      - /api/DashboardVeiculos/DashboardDados (GET) → Visão Geral
 *      - /api/DashboardVeiculos/DashboardUso (GET) → Uso Veículos + Anos/Meses disponíveis
 *      - /api/DashboardVeiculos/DashboardCustos (GET) → Custos anuais
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (38 funções)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🎯 INICIALIZAÇÃO E NAVEGAÇÃO                                                             │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • $(document).ready()                      → Inicializa tabs e carrega dados gerais     │
 * │ • initTabs()                               → Configura eventos de troca de abas         │
 * │ • carregarDadosGerais()                    → Fetch visão geral da frota                 │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🛠️ FILTROS E PERÍODO (Aba Uso)                                                           │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • inicializarFiltrosUso()                  → Detecta ano/mês mais recente, auto-seleciona│
 * │ • popularMesesDoAnoECarregar()             → Popula meses do ano, seleciona mais recente │
 * │ • aplicarFiltroAnoMes()                    → Filtra por ano/mês selecionado             │
 * │ • aplicarFiltroPeriodo(dias, btnElement)   → Aplica período rápido (ex: últimos 30 dias)│
 * │ • aplicarFiltroPersonalizado()             → Valida dataInicio/dataFim → carrega        │
 * │ • limparFiltroAnoMes()                     → Reset filtros ano/mês                       │
 * │ • limparFiltroPeriodo()                    → Limpa período personalizado                │
 * │ • atualizarPeriodoAtualLabel()             → Atualiza label "Exibindo dados de: ..."    │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 📊 RENDERIZAÇÃO - ABA GERAL (11 funções)                                                │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • atualizarCardsGerais(totais)             → 9 cards (ativo/reserva/próprio/locado)     │
 * │ • renderizarGraficosGerais(data)           → 5 gráficos (categoria/status/origem/modelo)│
 * │ • renderizarTabelasGerais(data)            → 4 tabelas (categoria/combustível/unidade)  │
 * │ • renderizarChartPie(containerId, dados)   → Gráfico Donut genérico                     │
 * │ • renderizarChartBarH(containerId, dados)  → Gráfico Bar horizontal genérico            │
 * │ • renderizarChartColumn(containerId, dados)→ Gráfico Column genérico                    │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 📈 RENDERIZAÇÃO - ABA USO (8 funções)                                                   │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarDadosUso(params)                 → Fetch dados de uso com filtros             │
 * │ • atualizarCardsUso(totais)                → 5 cards (viagens/km/abastecimentos/litros) │
 * │ • renderizarGraficosUso(data)              → 2 gráficos (viagens mês, abastecimento mês)│
 * │ • renderizarTabelasUso(data)               → 5 tabelas TOP (viagens/abastecimento/km)   │
 * │ • renderizarChartArea(containerId, dados)  → Gráfico SplineArea genérico                │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 💰 RENDERIZAÇÃO - ABA CUSTOS (5 funções)                                                │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarDadosCustos(ano)                 → Fetch custos anuais                        │
 * │ • atualizarCardsCustos(totais)             → 4 cards (abastecimento/manutenção/qtds)    │
 * │ • renderizarGraficosCustos(data)           → 2 gráficos (comparativo mensal/categoria)  │
 * │ • renderizarTabelasCustos(data)            → Tabela custos por categoria                │
 * │ • renderizarChartColumnGrouped()           → Gráfico barras agrupadas (abast+manut)     │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🎨 HELPERS E FORMATAÇÃO                                                                  │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • formatarMoeda(valor)                     → R$ 1.234,56 (pt-BR)                        │
 * │ • formatarDataBR(dataStr)                  → DD/MM/YYYY                                  │
 * │ • preencherSelectAnos(seletor, anos)       → Popula dropdown com anos disponíveis       │
 * │ • mostrarLoading(mensagem)/ocultarLoading()→ Overlay loading FrotiX                     │
 * │ • mostrarErro(mensagem)                    → SweetAlert erro                             │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Inicialização (carrega Visão Geral automaticamente)
 *    DOMContentLoaded → initTabs() + carregarDadosGerais()
 *      → Fetch /api/DashboardVeiculos/DashboardDados
 *      → Renderiza 9 cards, 5 gráficos, 4 tabelas
 * 
 * 💡 FLUXO 2: Troca para Aba "Uso dos Veículos" (auto-seleciona ano/mês mais recente)
 *    Click aba "Uso" → inicializarFiltrosUso()
 *      → Fetch anos disponíveis → Seleciona ano mais recente
 *      → Fetch meses do ano → Seleciona mês mais recente
 *      → Fetch /api/DashboardVeiculos/DashboardUso?ano=X&mes=Y
 *      → Renderiza 5 cards, 2 gráficos, 5 tabelas TOP
 * 
 * 💡 FLUXO 3: Filtro período rápido "Últimos 30 dias"
 *    Click btn "30 dias" → aplicarFiltroPeriodo(30, btnElement)
 *      → Calcula dataInicio/dataFim
 *      → Limpa filtros ano/mês
 *      → Fetch /api/DashboardVeiculos/DashboardUso?dataInicio=X&dataFim=Y
 *      → Re-renderiza gráficos e tabelas
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * 🎨 PALETA VERDE SAGE (harmonia com tema Frota FrotiX):
 *    - primary: #5f8575, secondary: #7aa390, accent: #8fb8a4
 *    - dark: #4a6b5c, cream: #e8f2ed
 *    - chart[]: 10 tons variados para gráficos
 * 
 * 🔄 AUTO-SELEÇÃO INTELIGENTE (Aba Uso):
 *    - Ao abrir aba, detecta ano/mês mais recente COM DADOS (não apenas ano atual)
 *    - Função popularMesesDoAnoECarregar() com callback para selecionar mês após popular
 *    - Label atualizada automaticamente: "Exibindo dados de: Dezembro/2025"
 * 
 * FLUXO USO INTELIGENTE: Para a aba Uso dos Veículos, obtem os anos disponíveis, seleciona o mais recente,
 * carrega os meses daquele ano, seleciona o mês mais recente, e então carrega os dados com esses filtros pré-selecionados.
 * 
 * 📊 GRÁFICOS SYNCFUSION:
 *    - Donut (innerRadius: 50%): categoria, status, origem
 *    - Bar horizontal: modelos, requisitantes, setores
 *    - Column: ano fabricação, categoria custos
 *    - SplineArea (opacity: 0.5): viagens mês, abastecimento mês
 *    - Column Grouped: comparativo abastecimento × manutenção
 * 
 * 🏷️ BADGES CUSTOMIZADOS:
 *    - badge-rank-veic: ranking TOP (1º-10º)
 *    - badge-rank-veic.top3: ouro/prata/bronze (medalhas)
 *    - badge-tipo-categoria: Passeio/Carga/PM/etc
 * 
 * 🚨 TRATAMENTO DE ERROS:
 *    - Try-catch em todas as funções assíncronas
 *    - Fallback: gráfico vazio com "<div class='text-center text-muted'>Nenhum dado encontrado</div>"
 *    - Alerta backend via Alerta.TratamentoErroComLinha() (não implementado neste arquivo,
 *      mas padrão FrotiX)
 * 
 * ⚡ PERFORMANCE:
 *    - Gráficos destruídos antes de recriar (.destroy() callback)
 *    - Cache local: dadosGerais, dadosUso, dadosCustos
 *    - Lazy loading: abas só carregam dados ao serem ativadas
 * 
 * **************************************************************************************** */

// Paleta de cores do tema Verde Sage
const CORES_VEIC = {
    primary: '#5f8575',
    secondary: '#7aa390',
    accent: '#8fb8a4',
    dark: '#4a6b5c',
    darker: '#3a5548',
    light: '#f0f7f4',
    cream: '#e8f2ed',
    // Cores complementares para gráficos
    chart: [
        '#5f8575', '#7aa390', '#8fb8a4', '#4a6b5c', '#3a5548',
        '#14b8a6', '#10b981', '#06b6d4', '#f59e0b', '#8b5cf6'
    ]
};

// Instâncias dos gráficos Syncfusion
let chartCategoria, chartStatus, chartOrigem, chartModelos, chartAnoFabricacao;
let chartViagensMes, chartAbastecimentoMes;
let chartComparativoMensal, chartCustoCategoria;

// Dados globais
let dadosGerais = null;
let dadosUso = null;
let dadosCustos = null;

$(document).ready(function () {
    // Inicialização
    initTabs();
    carregarDadosGerais();
});

// ==============================================
// NAVEGAÇÃO DE ABAS
// ==============================================

/****************************************************************************************
 * 🔧 FUNÇÃO: initTabs
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Inicializa sistema de navegação entre as 3 abas do dashboard de veículos
 *    (Visão Geral, Uso dos Veículos, Custos) com lazy loading inteligente.
 * 
 * 📥 ENTRADAS:
 *    - Nenhuma (usa seletores DOM para elementos .dash-tab-veic)
 * 
 * 📤 SAÍDAS:
 *    • Event listeners jQuery nos botões de aba
 *    • Troca de classes .active em tabs e conteúdos
 *    • Trigger de funções de carregamento (inicializarFiltrosUso, carregarDadosCustos)
 * 
 * 🔗 CHAMADA POR:
 *    • $(document).ready() → Inicialização única ao carregar página
 * 
 * 🔄 CHAMA:
 *    - inicializarFiltrosUso() → Primeira vez que abre aba "Uso"
 *    - carregarDadosCustos() → Primeira vez que abre aba "Custos"
 * 
 * 📝 OBSERVAÇÕES:
 *    - Lazy loading: dados só são carregados na primeira abertura da aba
 *    - Flags de controle: filtrosUsoInicializados, dadosCustos (null check)
 *    - Visão Geral carrega automaticamente no ready (não precisa lazy)
 * 
 ****************************************************************************************/
function initTabs() {
    try {
        $('.dash-tab-veic').on('click', function () {
            const tabId = $(this).data('tab');

            // Atualiza classes das abas
            $('.dash-tab-veic').removeClass('active');
            $(this).addClass('active');

            // Mostra conteúdo correto
            $('.dash-content-veic').removeClass('active');
            $(`#tab-${tabId}`).addClass('active');

            // Carrega dados se necessário
            if (tabId === 'uso-veiculos' && !filtrosUsoInicializados) {
                inicializarFiltrosUso();
            } else if (tabId === 'custos' && !dadosCustos) {
                carregarDadosCustos();
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'initTabs', error);
    }
}

// ==============================================
// LOADING OVERLAY
// ==============================================

/****************************************************************************************
 * 🔧 FUNÇÃO: mostrarLoading
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Exibe overlay de loading fullscreen com mensagem personalizável durante
 *    operações assíncronas (fetch API, Ajax).
 * 
 * 📥 ENTRADAS:
 *    • mensagem {String} [opcional='Carregando...'] - Texto exibido no loading
 * 
 * 📤 SAÍDAS:
 *    • Atualiza textContent de #loadingOverlayVeic .ftx-loading-text
 *    • FadeIn 200ms do overlay #loadingOverlayVeic
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais(), inicializarFiltrosUso(), carregarDadosUso(), carregarDadosCustos()
 * 
 * 🔄 CHAMA:
 *    - jQuery.fadeIn() (animação)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Overlay possui z-index 9999 (sobre todo conteúdo)
 *    - Animação rápida (200ms) para UX responsivo
 * 
 ****************************************************************************************/
function mostrarLoading(mensagem = 'Carregando...') {
    try {
        $('#loadingOverlayVeic .ftx-loading-text').text(mensagem);
        $('#loadingOverlayVeic').fadeIn(200);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'mostrarLoading', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: esconderLoading
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Oculta overlay de loading com animação fade out após conclusão de
 *    operações assíncronas.
 * 
 * 📥 ENTRADAS:
 *    - Nenhuma
 * 
 * 📤 SAÍDAS:
 *    • FadeOut 300ms do overlay #loadingOverlayVeic
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais() (success/error), inicializarFiltrosUso(), carregarDadosUso(),
 *      carregarDadosCustos() (todos callbacks Ajax)
 * 
 * 🔄 CHAMA:
 *    - jQuery.fadeOut() (animação)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Animação fade out (300ms) ligeiramente mais longa que fade in para UX suave
 *    - Sempre chamada após mostrarLoading(), mesmo em caso de erro
 * 
 ****************************************************************************************/
function esconderLoading() {
    try {
        $('#loadingOverlayVeic').fadeOut(300);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'esconderLoading', error);
    }
}

// ==============================================
// ABA 1: VISÃO GERAL
// ==============================================

/****************************************************************************************
 * 🔧 FUNÇÃO: carregarDadosGerais
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Carrega dados da Visão Geral da Frota via API e renderiza 9 cards, 5 gráficos
 *    e 4 tabelas (categoria, combustível, unidade, top KM).
 * 
 * 📥 ENTRADAS:
 *    - Nenhuma (busca todos os dados sem filtros)
 * 
 * 📤 SAÍDAS:
 *    • dadosGerais {Object} - Cache global dos dados retornados
 *    • Renderiza em tela:
 *      - 9 cards: total/ativos/inativos/reserva/efetivos/próprios/locados/idade/valor
 *      - 5 gráficos Syncfusion: categoria, status, origem, modelos, ano fabricação
 *      - 4 tabelas: categoria, combustível, unidade, top KM
 * 
 * 🔗 CHAMADA POR:
 *    • $(document).ready() → Carregamento automático na inicialização
 * 
 * 🔄 CHAMA:
 *    - mostrarLoading() / esconderLoading()
 *    - atualizarCardsGerais(data.totais)
 *    - renderizarGraficosGerais(data)
 *    - renderizarTabelasGerais(data)
 *    - mostrarErro() (em caso de falha)
 * 
 * 📝 OBSERVAÇÕES:
 *    - API: GET /api/DashboardVeiculos/DashboardDados
 *    - Sempre executada no pageload (não tem lazy loading como outras abas)
 *    - Error handler robusto com mensagem amigável ao usuário
 * 
 ****************************************************************************************/
function carregarDadosGerais() {
    try {
        mostrarLoading('Carregando dados da frota...');

        $.ajax({
            url: '/api/DashboardVeiculos/DashboardDados',
            method: 'GET',
            success: function (data) {
                dadosGerais = data;
                atualizarCardsGerais(data.totais);
                renderizarGraficosGerais(data);
                renderizarTabelasGerais(data);
                esconderLoading();
            },
            error: function (xhr, status, error) {
                console.error('Erro ao carregar dados gerais:', error);
                esconderLoading();
                mostrarErro('Erro ao carregar dados da frota');
            }
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosGerais', error);
        esconderLoading();
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: atualizarCardsGerais
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Atualiza os 9 cards estatísticos da Visão Geral com totais da frota (ativo/inativo/
 *    reserva/efetivo/próprio/locado/idade média/valor mensal).
 * 
 * 📥 ENTRADAS:
 *    • totais {Object} - Objeto com propriedades:
 *      - totalVeiculos {Number}    → Total de veículos cadastrados
 *      - veiculosAtivos {Number}   → Veículos em operação
 *      - veiculosInativos {Number} → Veículos fora de operação
 *      - veiculosReserva {Number}  → Veículos em reserva técnica
 *      - veiculosEfetivos {Number} → Veículos do quadro efetivo
 *      - veiculosProprios {Number} → Veículos próprios da frota
 *      - veiculosLocados {Number}  → Veículos locados de terceiros
 *      - idadeMedia {Number}       → Idade média em anos (com decimais)
 *      - valorMensalTotal {Number} → Valor total mensal (R$)
 * 
 * 📤 SAÍDAS:
 *    • Atualiza textContent de 9 elementos #totalVeiculos, #veiculosAtivos, etc.
 *    • Formata valores numéricos com separador de milhares (pt-BR)
 *    • Formata idade com 1 casa decimal + " anos"
 *    • Formata valor mensal com formatarMoeda() (R$ 1.234,56)
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais() → Após fetch /api/DashboardVeiculos/DashboardDados
 * 
 * 🔄 CHAMA:
 *    - formatarMoeda(valor) → Formatação monetária pt-BR
 * 
 * 📝 OBSERVAÇÕES:
 *    - toLocaleString('pt-BR') para separador de milhares
 *    - idadeMedia.toFixed(1) para 1 casa decimal (ex: 5,8 anos)
 *    - Cards usam classes .card-stat-value do FrotiX.css
 * 
 ****************************************************************************************/
function atualizarCardsGerais(totais) {
    try {
        $('#totalVeiculos').text(totais.totalVeiculos.toLocaleString('pt-BR'));
        $('#veiculosAtivos').text(totais.veiculosAtivos.toLocaleString('pt-BR'));
        $('#veiculosInativos').text(totais.veiculosInativos.toLocaleString('pt-BR'));
        $('#veiculosReserva').text(totais.veiculosReserva.toLocaleString('pt-BR'));
        $('#veiculosEfetivos').text(totais.veiculosEfetivos.toLocaleString('pt-BR'));
        $('#veiculosProprios').text(totais.veiculosProprios.toLocaleString('pt-BR'));
        $('#veiculosLocados').text(totais.veiculosLocados.toLocaleString('pt-BR'));
        $('#idadeMedia').text(totais.idadeMedia.toFixed(1) + ' anos');
        $('#valorMensalTotal').text(formatarMoeda(totais.valorMensalTotal));
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsGerais', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarGraficosGerais
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza os 5 gráficos Syncfusion da Visão Geral (categoria, status, origem,
 *    modelos, ano fabricação) com dados da API.
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 5 arrays:
 *      - porCategoria {Array}     → [{categoria, quantidade}] - Passeio/Carga/PM/etc
 *      - porStatus {Array}        → [{status, quantidade}] - Ativo/Inativo
 *      - porOrigem {Array}        → [{origem, quantidade}] - Próprio/Locado/Terceiro
 *      - porModelo {Array}        → [{modelo, quantidade}] - Modelos de veículos
 *      - porAnoFabricacao {Array} → [{ano, quantidade}] - Anos de fabricação
 * 
 * 📤 SAÍDAS:
 *    • 5 gráficos Syncfusion renderizados:
 *      1. chartCategoria (Donut): distribuição por categoria
 *      2. chartStatus (Donut): ativo vs inativo (verde/cinza)
 *      3. chartOrigem (Donut): próprio/locado/terceiro (3 cores)
 *      4. chartModelos (Bar horizontal): top modelos
 *      5. chartAnoFabricacao (Column): distribuição temporal
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais() → Após fetch API
 * 
 * 🔄 CHAMA:
 *    - renderizarChartPie() (3x: categoria, status, origem)
 *    - renderizarChartBarH() (1x: modelos)
 *    - renderizarChartColumn() (1x: ano fabricação)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Trunca nome de modelo para 25 chars (... ao final)
 *    - Paletas customizadas: status (#10b981/#64748b), origem (#5f8575/#f59e0b/#06b6d4)
 *    - Validação data.length > 0 antes de renderizar
 *    - Converte ano para string no gráfico (eixo X categórico)
 * 
 ****************************************************************************************/
function renderizarGraficosGerais(data) {
    try {
        // Gráfico de Categoria (Donut)
        if (data.porCategoria && data.porCategoria.length > 0) {
            renderizarChartPie('chartCategoria', data.porCategoria.map(c => ({
                x: c.categoria,
                y: c.quantidade
            })));
        }

        // Gráfico de Status (Donut)
        if (data.porStatus && data.porStatus.length > 0) {
            renderizarChartPie('chartStatus', data.porStatus.map(s => ({
                x: s.status,
                y: s.quantidade
            })), ['#10b981', '#64748b']);
        }

        // Gráfico de Origem (Donut)
        if (data.porOrigem && data.porOrigem.length > 0) {
            renderizarChartPie('chartOrigem', data.porOrigem.map(o => ({
                x: o.origem,
                y: o.quantidade
            })), ['#5f8575', '#f59e0b', '#06b6d4']);
        }

        // Gráfico de Modelos (Barras Horizontais)
        if (data.porModelo && data.porModelo.length > 0) {
            renderizarChartBarH('chartModelos', data.porModelo.map(m => ({
                x: m.modelo.length > 25 ? m.modelo.substring(0, 22) + '...' : m.modelo,
                y: m.quantidade
            })));
        }

        // Gráfico de Ano de Fabricação (Colunas)
        if (data.porAnoFabricacao && data.porAnoFabricacao.length > 0) {
            renderizarChartColumn('chartAnoFabricacao', data.porAnoFabricacao.map(a => ({
                x: a.ano.toString(),
                y: a.quantidade
            })));
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosGerais', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarTabelasGerais
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza 4 tabelas HTML da Visão Geral com dados da frota (categoria, combustível,
 *    unidade, TOP 10 KM) usando grid customizado FrotiX.
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 4 arrays:
 *      - porCategoria {Array}   → [{categoria, quantidade}] - Ex: Passeio, Carga, PM
 *      - porCombustivel {Array} → [{combustivel, quantidade}] - Ex: Gasolina, Diesel
 *      - porUnidade {Array}     → [{unidade, quantidade}] - Ex: Sede, Filial 1
 *      - topKm {Array}          → [{placa, modelo, km}] - TOP 10 veículos por KM
 * 
 * 📤 SAÍDAS:
 *    • Atualiza innerHTML de 4 elementos:
 *      - #tabelaCategoria: grid 2 colunas (categoria | qtd) + linha total
 *      - #tabelaCombustivel: grid 2 colunas (combustível | qtd)
 *      - #tabelaUnidade: grid 2 colunas (unidade | qtd)
 *      - #tabelaTopKm: grid 3 colunas (rank | veículo+modelo | km)
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais() → Após fetch API
 * 
 * 🔄 CHAMA:
 *    - Nenhuma função (manipulação DOM pura)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Badges de ranking: .badge-rank-veic (padrão) + .top3 (ouro/prata/bronze)
 *    - Fallback "Nenhum dado encontrado" se array vazio
 *    - Linha de TOTAL apenas na tabela de Categorias (reduce sum)
 *    - toLocaleString('pt-BR') para separador de milhares em KM
 *    - Grid customizado: classes .grid-row, .grid-cell do FrotiX.css
 * 
 ****************************************************************************************/
function renderizarTabelasGerais(data) {
    try {
        // Tabela de Categorias
        let htmlCategoria = '';
        if (data.porCategoria && data.porCategoria.length > 0) {
            data.porCategoria.forEach(c => {
                htmlCategoria += `
                    <div class="grid-row">
                        <div class="grid-cell">${c.categoria}</div>
                        <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
                    </div>
                `;
            });
            // Total
            const totalCat = data.porCategoria.reduce((sum, c) => sum + c.quantidade, 0);
            htmlCategoria += `
                <div class="grid-row grid-row-total">
                    <div class="grid-cell"><strong>TOTAL</strong></div>
                    <div class="grid-cell text-end"><strong>${totalCat}</strong></div>
                </div>
            `;
        } else {
            htmlCategoria = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
        }
        $('#tabelaCategoria').html(htmlCategoria);

        // Tabela de Combustível
        let htmlCombustivel = '';
        if (data.porCombustivel && data.porCombustivel.length > 0) {
            data.porCombustivel.forEach(c => {
                htmlCombustivel += `
                    <div class="grid-row">
                        <div class="grid-cell">${c.combustivel}</div>
                        <div class="grid-cell text-end"><strong>${c.quantidade}</strong></div>
                    </div>
                `;
            });
        } else {
            htmlCombustivel = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
        }
        $('#tabelaCombustivel').html(htmlCombustivel);

        // Tabela de Unidades
        let htmlUnidade = '';
        if (data.porUnidade && data.porUnidade.length > 0) {
            data.porUnidade.forEach(u => {
                htmlUnidade += `
                    <div class="grid-row">
                        <div class="grid-cell">${u.unidade}</div>
                        <div class="grid-cell text-end"><strong>${u.quantidade}</strong></div>
                    </div>
                `;
            });
        } else {
            htmlUnidade = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
        }
        $('#tabelaUnidade').html(htmlUnidade);

        // Tabela Top KM
        let htmlTopKm = '';
        if (data.topKm && data.topKm.length > 0) {
            data.topKm.forEach((v, i) => {
                const badgeClass = i < 3 ? 'top3' : '';
                htmlTopKm += `
                    <div class="grid-row">
                        <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                        <div class="grid-cell">
                            <strong>${v.placa}</strong>
                            <small class="d-block text-muted">${v.modelo}</small>
                        </div>
                        <div class="grid-cell text-end"><strong>${v.km.toLocaleString('pt-BR')} km</strong></div>
                    </div>
                `;
            });
        } else {
            htmlTopKm = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 3; text-align: center;">Nenhum dado encontrado</div></div>';
        }
        $('#tabelaTopKm').html(htmlTopKm);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasGerais', error);
    }
}

// ==============================================
// ABA 2: USO DOS VEÍCULOS
// ==============================================

// Variáveis de estado dos filtros
let filtroUsoAtual = { tipo: 'todos' };
let filtrosUsoInicializados = false;

/****************************************************************************************
 * 🔧 FUNÇÃO: inicializarFiltrosUso
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Inicializa filtros da Aba "Uso dos Veículos" com auto-seleção inteligente do
 *    ano/mês mais recente COM DADOS (não apenas ano atual). Usa 2 chamadas Ajax
 *    encadeadas para determinar período ótimo.
 * 
 * 📥 ENTRADAS:
 *    - Nenhuma (busca anos disponíveis da API)
 * 
 * 📤 SAÍDAS:
 *    • filtrosUsoInicializados {Boolean} - Flag global setada como true
 *    • Popula #filtroAnoUso com anos disponíveis (pré-seleciona mais recente)
 *    • Popula #filtroMesUso com mês mais recente com dados
 *    • Atualiza label: "Período: Dezembro/2025"
 *    • Renderiza cards, gráficos e tabelas da aba Uso
 * 
 * 🔗 CHAMADA POR:
 *    • initTabs() → Primeira vez que usuário clica na aba "Uso"
 * 
 * 🔄 CHAMA:
 *    - mostrarLoading() / esconderLoading()
 *    - preencherSelectAnos()
 *    - atualizarPeriodoAtualLabel()
 *    - atualizarCardsUso(), renderizarGraficosUso(), renderizarTabelasUso()
 *    - mostrarErro() (em caso de falha)
 * 
 * 📝 OBSERVAÇÕES:
 *    - **FLUXO COMPLEXO COM 2 AJAX ANINHADOS:**
 *      1. Fetch /DashboardUso (sem params) → obtém anosDisponiveis
 *      2. Seleciona ano mais recente (anos[0])
 *      3. Fetch /DashboardUso?ano=X → obtém viagensPorMes do ano
 *      4. Filtra meses com total > 0, ordena desc, seleciona mês mais recente
 *      5. Popula dropdowns + renderiza dashboard
 *    - Se anos.length === 0: renderiza vazio sem erro
 *    - Ajax aninhado com error fallback: usa dados da 1ª chamada se 2ª falhar
 *    - Flag filtrosUsoInicializados previne reinicialização desnecessária
 * 
 ****************************************************************************************/
function inicializarFiltrosUso() {
    try {
        mostrarLoading('Carregando estatísticas de uso...');

        // Primeira chamada: obter anos disponíveis
        $.ajax({
        url: '/api/DashboardVeiculos/DashboardUso',
        method: 'GET',
        data: {},
        success: function (data) {
            const anos = data.anosDisponiveis || [];

            if (anos.length === 0) {
                // Sem dados disponíveis
                dadosUso = data;
                filtrosUsoInicializados = true;
                preencherSelectAnos('#filtroAnoUso', [], null);
                atualizarCardsUso(data.totais);
                renderizarGraficosUso(data);
                renderizarTabelasUso(data);
                esconderLoading();
                return;
            }

            // Ano com último registro (primeiro da lista, ordenado desc)
            const anoMaisRecente = anos[0];

            // Preencher select de anos e pré-selecionar o mais recente
            preencherSelectAnos('#filtroAnoUso', anos, anoMaisRecente);
            $('#filtroAnoUso').val(anoMaisRecente.toString());

            // Buscar dados DO ANO MAIS RECENTE para determinar o mês mais recente
            $.ajax({
                url: '/api/DashboardVeiculos/DashboardUso',
                method: 'GET',
                data: { ano: anoMaisRecente },
                success: function (dataAno) {
                    let mesSelecionado = '';
                    const viagensPorMes = dataAno.viagensPorMes || [];

                    // Encontrar o último mês com dados (maior número de mês com valor > 0)
                    if (viagensPorMes.length > 0) {
                        const mesesComDados = viagensPorMes
                            .filter(item => item.total > 0)
                            .map(item => item.mes)
                            .sort((a, b) => b - a); // Ordenar decrescente

                        if (mesesComDados.length > 0) {
                            mesSelecionado = mesesComDados[0].toString();
                        }
                    }

                    // Pré-selecionar mês se encontrado
                    if (mesSelecionado) {
                        $('#filtroMesUso').val(mesSelecionado);
                        filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: mesSelecionado };
                    } else {
                        filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: '' };
                    }

                    // Atualizar label de período
                    atualizarPeriodoAtualLabel();

                    // Carregar dados com filtros aplicados
                    dadosUso = dataAno;
                    filtrosUsoInicializados = true;
                    atualizarCardsUso(dataAno.totais);
                    renderizarGraficosUso(dataAno);
                    renderizarTabelasUso(dataAno);
                    esconderLoading();
                },
                error: function () {
                    // Em caso de erro, usa os dados da primeira chamada
                    filtroUsoAtual = { tipo: 'anoMes', ano: anoMaisRecente.toString(), mes: '' };
                    atualizarPeriodoAtualLabel();
                    dadosUso = data;
                    filtrosUsoInicializados = true;
                    atualizarCardsUso(data.totais);
                    renderizarGraficosUso(data);
                    renderizarTabelasUso(data);
                    esconderLoading();
                }
            });
        },
        error: function (xhr, status, error) {
            console.error('Erro ao inicializar filtros de uso:', error);
            esconderLoading();
            mostrarErro('Erro ao carregar estatísticas de uso');
        }
    });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'inicializarFiltrosUso', error);
        esconderLoading();
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: carregarDadosUso
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Carrega dados da Aba "Uso dos Veículos" com filtros aplicados (ano/mês ou
 *    período personalizado) e renderiza 5 cards, 2 gráficos e 5 tabelas TOP.
 * 
 * 📥 ENTRADAS:
 *    • params {Object} [opcional={}] - Parâmetros de filtro:
 *      - ano {String}       → Filtro por ano (ex: "2025")
 *      - mes {String}       → Filtro por mês (ex: "12")
 *      - dataInicio {String}→ Data início (YYYY-MM-DD)
 *      - dataFim {String}   → Data fim (YYYY-MM-DD)
 * 
 * 📤 SAÍDAS:
 *    • dadosUso {Object} - Cache global atualizado
 *    • Renderiza em tela:
 *      - 5 cards: viagens/km/abastecimentos/litros/valor
 *      - 2 gráficos Syncfusion SplineArea: viagens por mês, abastecimento por mês
 *      - 5 tabelas TOP: viagens, abastecimento, litros, consumo (pior), eficiência (melhor)
 * 
 * 🔗 CHAMADA POR:
 *    • Event handlers: #btnFiltrarAnoMesUso, #btnFiltrarPeriodoUso, .btn-period-veic
 * 
 * 🔄 CHAMA:
 *    - mostrarLoading() / esconderLoading()
 *    - preencherSelectAnos() (se select vazio)
 *    - atualizarCardsUso(), renderizarGraficosUso(), renderizarTabelasUso()
 *    - mostrarErro() (em caso de falha)
 * 
 * 📝 OBSERVAÇÕES:
 *    - API: GET /api/DashboardVeiculos/DashboardUso?ano=X&mes=Y
 *    - Se #filtroAnoUso vazio, popula com anosDisponiveis do response
 *    - Parâmetros aceitos: {ano, mes} OU {dataInicio, dataFim} (mutuamente exclusivos)
 * 
 ****************************************************************************************/
function carregarDadosUso(params = {}) {
    try {
    mostrarLoading('Carregando estatísticas de uso...');

    $.ajax({
        url: '/api/DashboardVeiculos/DashboardUso',
        method: 'GET',
        data: params,
        success: function (data) {
            dadosUso = data;

            // Preencher select de anos se não preenchido
            if ($('#filtroAnoUso option').length <= 1) {
                preencherSelectAnos('#filtroAnoUso', data.anosDisponiveis, null);
            }

            atualizarCardsUso(data.totais);
            renderizarGraficosUso(data);
            renderizarTabelasUso(data);
            esconderLoading();
        },
        error: function (xhr, status, error) {
            console.error('Erro ao carregar dados de uso:', error);
            esconderLoading();
            mostrarErro('Erro ao carregar estatísticas de uso');
        }
    });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosUso', error);
        esconderLoading();
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: atualizarPeriodoAtualLabel
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Atualiza label de período exibido acima dos gráficos da Aba Uso com texto
 *    amigável baseado no filtro ativo (ano/mês, período ou rápido).
 * 
 * 📥 ENTRADAS:
 *    - Nenhuma (lê variável global filtroUsoAtual)
 * 
 * 📤 SAÍDAS:
 *    • Atualiza textContent de #periodoAtualLabelUso com:
 *      - "Período: Dezembro/2025" (tipo anoMes)
 *      - "Período: 01/01/2026 a 31/01/2026" (tipo periodo)
 *      - "Período: Últimos 30 dias" (tipo rapido)
 *      - "Exibindo todos os dados" (tipo todos)
 * 
 * 🔗 CHAMADA POR:
 *    • inicializarFiltrosUso(), event handlers de filtros
 * 
 * 🔄 CHAMA:
 *    - formatarDataBR() (converte YYYY-MM-DD → DD/MM/YYYY)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Array meses[1-12] começa com '' no índice 0 para facilitar acesso direto
 *    - Trata 4 tipos: 'anoMes', 'periodo', 'rapido', 'todos'
 *    - Lógica condicional complexa para combinações (ano sem mês, mês sem ano)
 * 
 ****************************************************************************************/
function atualizarPeriodoAtualLabel() {
    try {
    const meses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
    let label = 'Exibindo todos os dados';

    if (filtroUsoAtual.tipo === 'anoMes') {
        const ano = filtroUsoAtual.ano;
        const mes = filtroUsoAtual.mes;
        if (ano && mes) {
            label = `Período: ${meses[parseInt(mes)]}/${ano}`;
        } else if (ano) {
            label = `Período: Ano ${ano}`;
        } else if (mes) {
            label = `Período: ${meses[parseInt(mes)]} (todos os anos)`;
        }
    } else if (filtroUsoAtual.tipo === 'periodo') {
        const di = filtroUsoAtual.dataInicio;
        const df = filtroUsoAtual.dataFim;
        if (di && df) {
            label = `Período: ${formatarDataBR(di)} a ${formatarDataBR(df)}`;
        }
    } else if (filtroUsoAtual.tipo === 'rapido') {
        label = `Período: Últimos ${filtroUsoAtual.dias} dias`;
    }

        $('#periodoAtualLabelUso').text(label);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarPeriodoAtualLabel', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: formatarDataBR
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Converte data formato ISO (YYYY-MM-DD) para formato brasileiro (DD/MM/YYYY).
 * 
 * 📥 ENTRADAS:
 *    • dataStr {String} - Data em formato "YYYY-MM-DD" (ex: "2026-02-02")
 * 
 * 📤 SAÍDAS:
 *    • {String} "DD/MM/YYYY" (ex: "02/02/2026") ou "" se dataStr vazio/nulo
 * 
 * 🔗 CHAMADA POR:
 *    • atualizarPeriodoAtualLabel() → Label de período personalizado
 * 
 * 🔄 CHAMA:
 *    - String.split() (manipulação básica)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Simples split por '-' e reordenação [ano, mes, dia] → [dia, mes, ano]
 *    - Não valida formato - assume entrada sempre YYYY-MM-DD
 *    - Sem tratamento de timezone (trabalha com strings puras)
 * 
 ****************************************************************************************/
function formatarDataBR(dataStr) {
    try {
        if (!dataStr) return '';
        const partes = dataStr.split('-');
        return `${partes[2]}/${partes[1]}/${partes[0]}`;
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'formatarDataBR', error);
        return '';
    }
}

// Eventos dos filtros - Ano/Mês
$(document).on('click', '#btnFiltrarAnoMesUso', function () {
    const ano = $('#filtroAnoUso').val();
    const mes = $('#filtroMesUso').val();

    const params = {};
    if (ano) params.ano = ano;
    if (mes) params.mes = mes;

    filtroUsoAtual = { tipo: 'anoMes', ano, mes };
    atualizarPeriodoAtualLabel();

    // Limpar campos de período
    $('#dataInicioUso').val('');
    $('#dataFimUso').val('');
    $('.btn-period-veic').removeClass('active');

    carregarDadosUso(params);
});

$(document).on('click', '#btnLimparAnoMesUso', function () {
    $('#filtroAnoUso').val('');
    $('#filtroMesUso').val('');
    $('#dataInicioUso').val('');
    $('#dataFimUso').val('');
    $('.btn-period-veic').removeClass('active');

    filtroUsoAtual = { tipo: 'todos' };
    atualizarPeriodoAtualLabel();

    carregarDadosUso({});
});

// Eventos dos filtros - Período Personalizado
$(document).on('click', '#btnFiltrarPeriodoUso', function () {
    const dataInicio = $('#dataInicioUso').val();
    const dataFim = $('#dataFimUso').val();

    if (!dataInicio || !dataFim) {
        mostrarErro('Preencha as datas de início e fim');
        return;
    }

    if (new Date(dataInicio) > new Date(dataFim)) {
        mostrarErro('Data de início deve ser anterior à data de fim');
        return;
    }

    const params = { dataInicio, dataFim };

    filtroUsoAtual = { tipo: 'periodo', dataInicio, dataFim };
    atualizarPeriodoAtualLabel();

    // Limpar campos de ano/mês
    $('#filtroAnoUso').val('');
    $('#filtroMesUso').val('');
    $('.btn-period-veic').removeClass('active');

    carregarDadosUso(params);
});

$(document).on('click', '#btnLimparPeriodoUso', function () {
    $('#dataInicioUso').val('');
    $('#dataFimUso').val('');
    $('.btn-period-veic').removeClass('active');

    // Manter ano/mês se estiverem preenchidos
    const ano = $('#filtroAnoUso').val();
    const mes = $('#filtroMesUso').val();

    if (ano || mes) {
        filtroUsoAtual = { tipo: 'anoMes', ano, mes };
        const params = {};
        if (ano) params.ano = ano;
        if (mes) params.mes = mes;
        carregarDadosUso(params);
    } else {
        filtroUsoAtual = { tipo: 'todos' };
        carregarDadosUso({});
    }

    atualizarPeriodoAtualLabel();
});

// Eventos dos Períodos Rápidos
$(document).on('click', '.btn-period-veic', function () {
    const dias = parseInt($(this).data('dias'));

    // Calcular datas
    const hoje = new Date();
    const dataFim = hoje.toISOString().split('T')[0];
    const dataInicio = new Date(hoje.getTime() - (dias * 24 * 60 * 60 * 1000)).toISOString().split('T')[0];

    // Atualizar campos visuais
    $('#dataInicioUso').val(dataInicio);
    $('#dataFimUso').val(dataFim);
    $('#filtroAnoUso').val('');
    $('#filtroMesUso').val('');

    // Marcar botão ativo
    $('.btn-period-veic').removeClass('active');
    $(this).addClass('active');

    filtroUsoAtual = { tipo: 'rapido', dias, dataInicio, dataFim };
    atualizarPeriodoAtualLabel();

    carregarDadosUso({ dataInicio, dataFim });
});

/****************************************************************************************
 * 🔧 FUNÇÃO: atualizarCardsUso
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Atualiza os 5 cards estatísticos da Aba "Uso dos Veículos" com totais de
 *    viagens, quilometragem, abastecimentos e custos.
 * 
 * 📥 ENTRADAS:
 *    • totais {Object} - Objeto com propriedades:
 *      - totalViagens {Number}          → Quantidade de viagens realizadas
 *      - kmTotalRodado {Number}         → Quilometragem total percorrida
 *      - totalAbastecimentos {Number}   → Quantidade de abastecimentos
 *      - totalLitros {Number}           → Litros abastecidos (total)
 *      - valorTotalAbastecimento {Number} → Valor em reais gasto
 * 
 * 📤 SAÍDAS:
 *    • Atualiza textContent de 5 elementos:
 *      - #totalViagensUso, #kmTotalRodado, #totalAbastecimentosUso,
 *        #totalLitrosUso, #valorAbastecimentoUso
 *    • Formata valores com separador milhares (pt-BR)
 *    • Litros: 0 casas decimais, Valor: formatarMoeda()
 * 
 * 🔗 CHAMADA POR:
 *    • inicializarFiltrosUso(), carregarDadosUso()
 * 
 * 🔄 CHAMA:
 *    - formatarMoeda() → Formatação R$ para valor abastecimento
 * 
 * 📝 OBSERVAÇÕES:
 *    - toLocaleString('pt-BR') com opções min/max para litros (0 decimais)
 *    - Sufixos ' km' e ' L' adicionados aos valores
 * 
 ****************************************************************************************/
function atualizarCardsUso(totais) {
    try {
        $('#totalViagensUso').text(totais.totalViagens.toLocaleString('pt-BR'));
        $('#kmTotalRodado').text(totais.kmTotalRodado.toLocaleString('pt-BR') + ' km');
        $('#totalAbastecimentosUso').text(totais.totalAbastecimentos.toLocaleString('pt-BR'));
        $('#totalLitrosUso').text(totais.totalLitros.toLocaleString('pt-BR', { minimumFractionDigits: 0, maximumFractionDigits: 0 }) + ' L');
        $('#valorAbastecimentoUso').text(formatarMoeda(totais.valorTotalAbastecimento));
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsUso', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarGraficosUso
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza 2 gráficos Syncfusion SplineArea da Aba "Uso dos Veículos":
 *    viagens por mês (12 meses) e abastecimento por mês (12 meses).
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 2 arrays:
 *      - viagensPorMes {Array}       → [{mes: 1-12, quantidade}]
 *      - abastecimentoPorMes {Array} → [{mes: 1-12, valor}]
 * 
 * 📤 SAÍDAS:
 *    • 2 gráficos Syncfusion renderizados:
 *      1. chartViagensMes (SplineArea verde sage): Jan-Dez com quantidade de viagens
 *      2. chartAbastecimentoMes (SplineArea laranja #f59e0b): Jan-Dez com valor R$
 * 
 * 🔗 CHAMADA POR:
 *    • inicializarFiltrosUso(), carregarDadosUso()
 * 
 * 🔄 CHAMA:
 *    - renderizarChartArea() (2x com cores diferentes)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Array meses[]: nomes abreviados Jan-Dez
 *    - Loop 1-12: busca dados do mês na API, fallback 0 se não encontrado
 *    - Cores: CORES_VEIC.primary (verde sage) vs #f59e0b (laranja padrão)
 *    - Preenche TODOS os 12 meses mesmo sem dados (gráfico completo)
 * 
 ****************************************************************************************/
function renderizarGraficosUso(data) {
    try {
    // Gráfico Viagens por Mês
    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    const dadosViagens = [];
    for (let i = 1; i <= 12; i++) {
        const item = data.viagensPorMes.find(v => v.mes === i);
        dadosViagens.push({
            x: meses[i - 1],
            y: item ? item.quantidade : 0
        });
    }
    renderizarChartArea('chartViagensMes', dadosViagens, CORES_VEIC.primary);

    // Gráfico Abastecimento por Mês
    const dadosAbast = [];
    for (let i = 1; i <= 12; i++) {
        const item = data.abastecimentoPorMes.find(a => a.mes === i);
        dadosAbast.push({
            x: meses[i - 1],
            y: item ? item.valor : 0
        });
    }
        renderizarChartArea('chartAbastecimentoMes', dadosAbast, '#f59e0b');
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosUso', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarTabelasUso
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza 5 tabelas HTML grid da Aba "Uso" com rankings TOP 10 de veículos
 *    por viagens, abastecimento (valor), litros abastecidos, menor consumo (km/L)
 *    e maior eficiência (km/L).
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 5 arrays:
 *      - topViagens {Array}         → [{placa, modelo, quantidade, kmTotal}]
 *      - topAbastecimento {Array}   → [{placa, modelo, litros, valor}]
 *      - topLitrosAbastecidos {Array} → [{placa, modelo, litros, qtdAbastecimentos}]
 *      - topConsumo {Array}         → [{placa, modelo, consumo km/L, kmRodado}] (PIOR)
 *      - topEficiencia {Array}      → [{placa, modelo, consumo km/L, kmRodado}] (MELHOR)
 * 
 * 📤 SAÍDAS:
 *    • Atualiza innerHTML de 5 elementos:
 *      - #tabelaTopViagens: grid 4 colunas (rank | veículo+modelo | qtd | km)
 *      - #tabelaTopAbastecimento: grid 4 colunas (rank | veículo+modelo | litros | valor)
 *      - #tabelaTopLitros: grid 4 colunas (rank | veículo+modelo | litros | qtd)
 *      - #tabelaTopConsumo: grid 4 colunas (rank | veículo+modelo | km/L VERMELHO | km)
 *      - #tabelaTopEficiencia: grid 4 colunas (rank | veículo+modelo | km/L VERDE | km)
 * 
 * 🔗 CHAMADA POR:
 *    • inicializarFiltrosUso(), carregarDadosUso()
 * 
 * 🔄 CHAMA:
 *    - formatarMoeda() → Formatação valores abastecimento
 * 
 * 📝 OBSERVAÇÕES:
 *    - Badges ranking: .badge-rank-veic (padrão) + .top3 (ouro/prata/bronze)
 *    - Consumo vermelho (#ef4444): PIOR km/L (menor é pior)
 *    - Eficiência verde (#10b981): MELHOR km/L (maior é melhor)
 *    - Litros: 1 casa decimal, Km/L: 2 casas decimais
 *    - Fallback "Nenhum dado encontrado" se array vazio
 *    - Grid customizado FrotiX: .grid-row, .grid-cell
 * 
 ****************************************************************************************/
function renderizarTabelasUso(data) {
    try {
    // Tabela Top Viagens
    let htmlViagens = '';
    if (data.topViagens && data.topViagens.length > 0) {
        data.topViagens.forEach((v, i) => {
            const badgeClass = i < 3 ? 'top3' : '';
            htmlViagens += `
                <div class="grid-row">
                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                    <div class="grid-cell">
                        <strong>${v.placa}</strong>
                        <small class="d-block text-muted">${v.modelo}</small>
                    </div>
                    <div class="grid-cell text-center"><strong>${v.quantidade}</strong></div>
                    <div class="grid-cell text-end">${v.kmTotal.toLocaleString('pt-BR')} km</div>
                </div>
            `;
        });
    } else {
        htmlViagens = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaTopViagens').html(htmlViagens);

    // Tabela Top Abastecimento
    let htmlAbast = '';
    if (data.topAbastecimento && data.topAbastecimento.length > 0) {
        data.topAbastecimento.forEach((v, i) => {
            const badgeClass = i < 3 ? 'top3' : '';
            htmlAbast += `
                <div class="grid-row">
                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                    <div class="grid-cell">
                        <strong>${v.placa}</strong>
                        <small class="d-block text-muted">${v.modelo}</small>
                    </div>
                    <div class="grid-cell text-end">${v.litros.toLocaleString('pt-BR', { minimumFractionDigits: 1 })} L</div>
                    <div class="grid-cell text-end"><strong>${formatarMoeda(v.valor)}</strong></div>
                </div>
            `;
        });
    } else {
        htmlAbast = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaTopAbastecimento').html(htmlAbast);

    // Tabela Top Litros Abastecidos
    let htmlLitros = '';
    if (data.topLitrosAbastecidos && data.topLitrosAbastecidos.length > 0) {
        data.topLitrosAbastecidos.forEach((v, i) => {
            const badgeClass = i < 3 ? 'top3' : '';
            htmlLitros += `
                <div class="grid-row">
                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                    <div class="grid-cell">
                        <strong>${v.placa}</strong>
                        <small class="d-block text-muted">${v.modelo}</small>
                    </div>
                    <div class="grid-cell text-end"><strong>${v.litros.toLocaleString('pt-BR', { minimumFractionDigits: 1 })} L</strong></div>
                    <div class="grid-cell text-center">${v.qtdAbastecimentos}</div>
                </div>
            `;
        });
    } else {
        htmlLitros = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaTopLitros').html(htmlLitros);

    // Tabela Top Menos Eficientes (menor km/l)
    let htmlConsumo = '';
    if (data.topConsumo && data.topConsumo.length > 0) {
        data.topConsumo.forEach((v, i) => {
            const badgeClass = i < 3 ? 'top3' : '';
            htmlConsumo += `
                <div class="grid-row">
                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                    <div class="grid-cell">
                        <strong>${v.placa}</strong>
                        <small class="d-block text-muted">${v.modelo}</small>
                    </div>
                    <div class="grid-cell text-end"><strong style="color: #ef4444;">${v.consumo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</strong></div>
                    <div class="grid-cell text-end">${v.kmRodado.toLocaleString('pt-BR')}</div>
                </div>
            `;
        });
    } else {
        htmlConsumo = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaTopConsumo').html(htmlConsumo);

    // Tabela Top Mais Eficientes (maior km/l)
    let htmlEficiencia = '';
    if (data.topEficiencia && data.topEficiencia.length > 0) {
        data.topEficiencia.forEach((v, i) => {
            const badgeClass = i < 3 ? 'top3' : '';
            htmlEficiencia += `
                <div class="grid-row">
                    <div class="grid-cell"><span class="badge-rank-veic ${badgeClass}">${i + 1}</span></div>
                    <div class="grid-cell">
                        <strong>${v.placa}</strong>
                        <small class="d-block text-muted">${v.modelo}</small>
                    </div>
                    <div class="grid-cell text-end"><strong style="color: #10b981;">${v.consumo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</strong></div>
                    <div class="grid-cell text-end">${v.kmRodado.toLocaleString('pt-BR')}</div>
                </div>
            `;
        });
    } else {
        htmlEficiencia = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 4; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaTopEficiencia').html(htmlEficiencia);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasUso', error);
    }
}

// ==============================================
// ABA 3: CUSTOS
// ==============================================

/****************************************************************************************
 * 🔧 FUNÇÃO: carregarDadosCustos
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Carrega dados da Aba "Custos" com filtro de ano e renderiza 4 cards,
 *    2 gráficos e 1 tabela com distribuição de custos por categoria.
 * 
 * 📥 ENTRADAS:
 *    • ano {Number|null} [opcional=null] - Ano para filtrar (ex: 2025)
 *      Se null: exibe todos os anos
 * 
 * 📤 SAÍDAS:
 *    • dadosCustos {Object} - Cache global atualizado
 *    • Renderiza em tela:
 *      - 4 cards: custo abastecimento/manutenção, qtd abastecimentos/manutenções
 *      - 2 gráficos: comparativo mensal (barras agrupadas), custo por categoria
 *      - 1 tabela: custo por categoria (com linha TOTAL)
 * 
 * 🔗 CHAMADA POR:
 *    • initTabs() → Primeira vez que abre aba "Custos"
 *    • Event handler: #btnFiltrarCusto
 * 
 * 🔄 CHAMA:
 *    - mostrarLoading() / esconderLoading()
 *    - preencherSelectAnos() (se select vazio)
 *    - atualizarCardsCustos(), renderizarGraficosCustos(), renderizarTabelasCustos()
 *    - mostrarErro() (em caso de falha)
 * 
 * 📝 OBSERVAÇÕES:
 *    - API: GET /api/DashboardVeiculos/DashboardCustos?ano=X
 *    - Fallback inteligente: usa dadosUso.anosDisponiveis se disponível,
 *      senão usa ano atual para popular select
 *    - Lazy loading: aba só carrega dados na primeira abertura
 * 
 ****************************************************************************************/
function carregarDadosCustos(ano = null) {
    try {
        mostrarLoading('Carregando dados de custos...');

        const params = ano ? { ano: ano } : {};

        $.ajax({
        url: '/api/DashboardVeiculos/DashboardCustos',
        method: 'GET',
        data: params,
        success: function (data) {
            dadosCustos = data;

            // Preencher select de anos se não preenchido
            if ($('#filtroAnoCusto option').length <= 1) {
                // Usar anos do dadosUso se disponível
                if (dadosUso && dadosUso.anosDisponiveis) {
                    preencherSelectAnos('#filtroAnoCusto', dadosUso.anosDisponiveis, data.anoSelecionado);
                } else {
                    preencherSelectAnos('#filtroAnoCusto', [new Date().getFullYear()], data.anoSelecionado);
                }
            }

            atualizarCardsCustos(data.totais);
            renderizarGraficosCustos(data);
            renderizarTabelasCustos(data);
            esconderLoading();
        },
        error: function (xhr, status, error) {
            console.error('Erro ao carregar dados de custos:', error);
            esconderLoading();
            mostrarErro('Erro ao carregar dados de custos');
        }
    });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'carregarDadosCustos', error);
        esconderLoading();
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: atualizarCardsCustos
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Atualiza os 4 cards estatísticos da Aba "Custos" com totais de abastecimento,
 *    manutenção e quantidades.
 * 
 * 📥 ENTRADAS:
 *    • totais {Object} - Objeto com propriedades:
 *      - totalAbastecimento {Number} → Valor total gasto em abastecimento (R$)
 *      - totalManutencao {Number}    → Valor total gasto em manutenção (R$)
 *      - qtdAbastecimentos {Number}  → Quantidade de abastecimentos realizados
 *      - qtdManutencoes {Number}     → Quantidade de manutenções realizadas
 * 
 * 📤 SAÍDAS:
 *    • Atualiza textContent de 4 elementos:
 *      - #custoAbastecimento, #custoManutencao (formatado R$)
 *      - #qtdAbastecimentosCusto, #qtdManutencoesCusto (separador milhares)
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosCustos() → Após fetch API
 * 
 * 🔄 CHAMA:
 *    - formatarMoeda() (2x para valores monetários)
 * 
 * 📝 OBSERVAÇÕES:
 *    - toLocaleString('pt-BR') para quantidades (separador milhares)
 *    - formatarMoeda() para valores (R$ 1.234,56)
 * 
 ****************************************************************************************/
function atualizarCardsCustos(totais) {
    try {
        $('#custoAbastecimento').text(formatarMoeda(totais.totalAbastecimento));
        $('#custoManutencao').text(formatarMoeda(totais.totalManutencao));
        $('#qtdAbastecimentosCusto').text(totais.qtdAbastecimentos.toLocaleString('pt-BR'));
        $('#qtdManutencoesCusto').text(totais.qtdManutencoes.toLocaleString('pt-BR'));
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'atualizarCardsCustos', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarGraficosCustos
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza 2 gráficos Syncfusion da Aba "Custos": comparativo mensal
 *    (abastecimento vs manutenção) e custo por categoria de veículo.
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 2 arrays:
 *      - comparativoMensal {Array}  → [{abastecimento, manutencao}] 12 meses
 *      - custoPorCategoria {Array} → [{categoria, valorAbastecimento}]
 * 
 * 📤 SAÍDAS:
 *    • 2 gráficos Syncfusion renderizados:
 *      1. chartComparativoMensal (Column Grouped): 12 meses, 2 séries (laranja/verde)
 *      2. chartCustoCategoria (Bar horizontal laranja): categorias de veículos
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosCustos() → Após fetch API
 * 
 * 🔄 CHAMA:
 *    - renderizarChartColumnGrouped() → Gráfico barras agrupadas
 *    - renderizarChartBarH() → Gráfico barras horizontais
 * 
 * 📝 OBSERVAÇÕES:
 *    - Array meses[]: nomes abreviados Jan-Dez
 *    - comparativoMensal: laranja (#f59e0b) abastecimento, verde sage manutenção
 *    - Validação data.length > 0 antes de renderizar categoria
 * 
 ****************************************************************************************/
function renderizarGraficosCustos(data) {
    try {
    // Gráfico Comparativo Mensal (Barras Agrupadas)
    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    const seriesAbast = [];
    const seriesManut = [];

    data.comparativoMensal.forEach((item, i) => {
        seriesAbast.push({ x: meses[i], y: item.abastecimento });
        seriesManut.push({ x: meses[i], y: item.manutencao });
    });

    renderizarChartColumnGrouped('chartComparativoMensal', seriesAbast, seriesManut, 'Abastecimento', 'Manutenção');

    // Gráfico Custo por Categoria
    if (data.custoPorCategoria && data.custoPorCategoria.length > 0) {
        renderizarChartBarH('chartCustoCategoria', data.custoPorCategoria.map(c => ({
            x: c.categoria,
            y: c.valorAbastecimento
        })), '#f59e0b');
    }
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarGraficosCustos', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: renderizarTabelasCustos
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Renderiza tabela HTML grid da Aba "Custos" com custo de abastecimento por
 *    categoria de veículo (Passeio/Carga/PM/etc) e linha de TOTAL.
 * 
 * 📥 ENTRADAS:
 *    • data {Object} - Objeto com 1 array:
 *      - custoPorCategoria {Array} → [{categoria, valorAbastecimento}]
 * 
 * 📤 SAÍDAS:
 *    • Atualiza innerHTML de #tabelaCustoCategoria:
 *      - Grid 2 colunas: categoria | valor R$
 *      - Linha TOTAL com soma de todos valores (reduce)
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosCustos() → Após fetch API
 * 
 * 🔄 CHAMA:
 *    - formatarMoeda() → Formatação valores (R$ 1.234,56)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Usa Array.reduce() para calcular total acumulado
 *    - Linha TOTAL com classe .grid-row-total (destaque visual)
 *    - Fallback "Nenhum dado encontrado" se array vazio
 *    - Grid customizado FrotiX: .grid-row, .grid-cell
 * 
 ****************************************************************************************/
function renderizarTabelasCustos(data) {
    try {
    // Tabela Custo por Categoria
    let html = '';
    if (data.custoPorCategoria && data.custoPorCategoria.length > 0) {
        let total = 0;
        data.custoPorCategoria.forEach(c => {
            total += c.valorAbastecimento;
            html += `
                <div class="grid-row">
                    <div class="grid-cell">${c.categoria}</div>
                    <div class="grid-cell text-end"><strong>${formatarMoeda(c.valorAbastecimento)}</strong></div>
                </div>
            `;
        });
        html += `
            <div class="grid-row grid-row-total">
                <div class="grid-cell"><strong>TOTAL</strong></div>
                <div class="grid-cell text-end"><strong>${formatarMoeda(total)}</strong></div>
            </div>
        `;
    } else {
        html = '<div class="grid-row"><div class="grid-cell" style="grid-column: span 2; text-align: center;">Nenhum dado encontrado</div></div>';
    }
    $('#tabelaCustoCategoria').html(html);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarTabelasCustos', error);
    }
}

// Evento do botão filtrar Custos
$(document).on('click', '#btnFiltrarCusto', function () {
    const ano = $('#filtroAnoCusto').val();
    carregarDadosCustos(ano);
});

// ==============================================
// GRÁFICOS SYNCFUSION
// ==============================================

function renderizarChartPie(containerId, dados, cores = CORES_VEIC.chart) {
    try {
        const container = document.getElementById(containerId);
        if (!container) return;
        container.innerHTML = '';

        const chart = new ej.charts.AccumulationChart({
        series: [{
            dataSource: dados,
            xName: 'x',
            yName: 'y',
            innerRadius: '50%',
            palettes: cores,
            dataLabel: {
                visible: true,
                position: 'Outside',
                name: 'x',
                font: { fontWeight: '600', size: '11px' },
                connectorStyle: { length: '10px', type: 'Curve' }
            },
            explode: true,
            explodeOffset: '5%',
            explodeIndex: 0
        }],
        legendSettings: {
            visible: true,
            position: 'Bottom',
            textStyle: { size: '11px' }
        },
        tooltip: {
            enable: true,
            format: '${point.x}: <b>${point.y}</b>'
        },
        background: 'transparent',
        enableSmartLabels: true
    });
    chart.appendTo(container);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartPie', error);
    }
}

function renderizarChartBarH(containerId, dados, cor = CORES_VEIC.primary) {
    try {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';

    const chart = new ej.charts.Chart({
        primaryXAxis: {
            valueType: 'Category',
            labelStyle: { size: '10px' },
            majorGridLines: { width: 0 }
        },
        primaryYAxis: {
            labelFormat: '{value}',
            labelStyle: { size: '10px' },
            majorGridLines: { dashArray: '3,3' }
        },
        series: [{
            dataSource: dados,
            xName: 'x',
            yName: 'y',
            type: 'Bar',
            fill: cor,
            cornerRadius: { topLeft: 4, topRight: 4 },
            marker: { dataLabel: { visible: true, position: 'Top', font: { size: '10px', fontWeight: '600' } } }
        }],
        tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
        chartArea: { border: { width: 0 } },
        background: 'transparent'
    });
    chart.appendTo(container);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartBarH', error);
    }
}

function renderizarChartColumn(containerId, dados, cor = CORES_VEIC.primary) {
    try {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';

    const chart = new ej.charts.Chart({
        primaryXAxis: {
            valueType: 'Category',
            labelStyle: { size: '10px' },
            majorGridLines: { width: 0 }
        },
        primaryYAxis: {
            labelFormat: '{value}',
            labelStyle: { size: '10px' },
            majorGridLines: { dashArray: '3,3' }
        },
        series: [{
            dataSource: dados,
            xName: 'x',
            yName: 'y',
            type: 'Column',
            fill: cor,
            cornerRadius: { topLeft: 4, topRight: 4 },
            marker: { dataLabel: { visible: true, position: 'Top', font: { size: '10px', fontWeight: '600' } } }
        }],
        tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
        chartArea: { border: { width: 0 } },
        background: 'transparent'
    });
    chart.appendTo(container);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartColumn', error);
    }
}

function renderizarChartArea(containerId, dados, cor = CORES_VEIC.primary) {
    try {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';

    const chart = new ej.charts.Chart({
        primaryXAxis: {
            valueType: 'Category',
            labelStyle: { size: '10px' },
            majorGridLines: { width: 0 }
        },
        primaryYAxis: {
            labelFormat: '{value}',
            labelStyle: { size: '10px' },
            majorGridLines: { dashArray: '3,3' }
        },
        series: [{
            dataSource: dados,
            xName: 'x',
            yName: 'y',
            type: 'SplineArea',
            fill: cor,
            opacity: 0.5,
            border: { width: 2, color: cor },
            marker: {
                visible: true,
                width: 7,
                height: 7,
                fill: cor,
                border: { width: 2, color: '#fff' }
            }
        }],
        tooltip: { enable: true, format: '${point.x}: <b>${point.y}</b>' },
        chartArea: { border: { width: 0 } },
        background: 'transparent'
    });
    chart.appendTo(container);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartArea', error);
    }
}

function renderizarChartColumnGrouped(containerId, series1, series2, nome1, nome2) {
    try {
    const container = document.getElementById(containerId);
    if (!container) return;
    container.innerHTML = '';

    const chart = new ej.charts.Chart({
        primaryXAxis: {
            valueType: 'Category',
            labelStyle: { size: '10px' },
            majorGridLines: { width: 0 }
        },
        primaryYAxis: {
            labelFormat: 'R$ {value}',
            labelStyle: { size: '10px' },
            majorGridLines: { dashArray: '3,3' }
        },
        series: [
            {
                dataSource: series1,
                xName: 'x',
                yName: 'y',
                name: nome1,
                type: 'Column',
                fill: '#f59e0b',
                cornerRadius: { topLeft: 3, topRight: 3 }
            },
            {
                dataSource: series2,
                xName: 'x',
                yName: 'y',
                name: nome2,
                type: 'Column',
                fill: CORES_VEIC.primary,
                cornerRadius: { topLeft: 3, topRight: 3 }
            }
        ],
        legendSettings: { visible: true, position: 'Top' },
        tooltip: {
            enable: true,
            shared: true,
            format: '${series.name}: <b>${point.y}</b>'
        },
        chartArea: { border: { width: 0 } },
        background: 'transparent'
    });
    chart.appendTo(container);
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'renderizarChartColumnGrouped', error);
    }
}

// ==============================================
// FUNÇÕES AUXILIARES
// ==============================================

/****************************************************************************************
 * 🔧 FUNÇÃO: formatarMoeda
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Formata valores numéricos para moeda brasileira (R$ 1.234,56) usando
 *    Intl.NumberFormat padrão pt-BR.
 * 
 * 📥 ENTRADAS:
 *    • valor {Number|null|undefined} - Valor numérico a ser formatado
 * 
 * 📤 SAÍDAS:
 *    • {String} "R$ 1.234,56" (2 casas decimais) ou "R$ 0,00" se valor nulo
 * 
 * 🔗 CHAMADA POR:
 *    • atualizarCardsGerais(), atualizarCardsUso(), atualizarCardsCustos(),
 *      renderizarTabelasUso(), renderizarTabelasCustos()
 * 
 * 🔄 CHAMA:
 *    - Number.toLocaleString() (ECMAScript Intl API)
 * 
 * 📝 OBSERVAÇÕES:
 *    - Fallback seguro: null/undefined retorna "R$ 0,00"
 *    - Sempre usa 2 casas decimais (minimumFractionDigits: 2)
 *    - Padrão BRL (currency: 'BRL')
 * 
 ****************************************************************************************/
function formatarMoeda(valor) {
    try {
        if (valor === null || valor === undefined) return 'R$ 0,00';
        return valor.toLocaleString('pt-BR', {
            style: 'currency',
            currency: 'BRL',
            minimumFractionDigits: 2
        });
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'formatarMoeda', error);
        return 'R$ 0,00';
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: preencherSelectAnos
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Popula dropdown <select> com lista de anos disponíveis, incluindo opção
 *    "Todos os Anos" e pré-seleção opcional.
 * 
 * 📥 ENTRADAS:
 *    • seletor {String}       - Seletor jQuery do <select> (ex: '#filtroAnoUso')
 *    • anos {Array<Number>}   - Array de anos [2025, 2024, 2023...]
 *    • anoSelecionado {Number} - Ano a pré-selecionar (opcional)
 * 
 * 📤 SAÍDAS:
 *    • Atualiza innerHTML do <select> com <option> elements
 *    • Primeira option: "<Todos os Anos>" (value="")
 *    • Options subsequentes: anos da lista ou ano atual se vazio
 * 
 * 🔗 CHAMADA POR:
 *    • inicializarFiltrosUso(), carregarDadosUso(), carregarDadosCustos()
 * 
 * 🔄 CHAMA:
 *    - jQuery.empty(), jQuery.append()
 * 
 * 📝 OBSERVAÇÕES:
 *    - Sempre inclui opção "Todos" primeiro
 *    - Fallback: se anos vazio, adiciona apenas ano atual
 *    - Atributo selected adicionado se ano === anoSelecionado
 * 
 ****************************************************************************************/
function preencherSelectAnos(seletor, anos, anoSelecionado) {
    try {
        const $select = $(seletor);
        $select.empty();

        // Adiciona opção "Todos os Anos" primeiro
        $select.append('<option value="">&lt;Todos os Anos&gt;</option>');

        if (anos && anos.length > 0) {
            anos.forEach(ano => {
                const selected = ano === anoSelecionado ? 'selected' : '';
                $select.append(`<option value="${ano}" ${selected}>${ano}</option>`);
            });
        } else {
            const anoAtual = new Date().getFullYear();
            $select.append(`<option value="${anoAtual}" selected>${anoAtual}</option>`);
        }
    } catch (error) {
        Alerta.TratamentoErroComLinha('dashboard-veiculos.js', 'preencherSelectAnos', error);
    }
}

/****************************************************************************************
 * 🔧 FUNÇÃO: mostrarErro
 * ================================================================================================
 * 
 * 🎯 OBJETIVO:
 *    Exibe mensagem de erro ao usuário com SweetAlert2 (prioritário) ou AppToast
 *    (fallback), eliminando uso de alert() nativo.
 * 
 * 📥 ENTRADAS:
 *    • mensagem {String} - Texto da mensagem de erro a ser exibida
 * 
 * 📤 SAÍDAS:
 *    - Swal.fire() modal (se SweetAlert2 disponível)
 *    - AppToast.show() toast (se AppToast disponível)
 *    - console.error() (se ambos indisponíveis)
 * 
 * 🔗 CHAMADA POR:
 *    • carregarDadosGerais(), carregarDadosUso(), carregarDadosCustos()
 *    • Todos os handlers de erro Ajax
 * 
 * 🔄 CHAMA:
 *    - Swal.fire() (SweetAlert2)
 *    - AppToast.show() (global-toast.js)
 * 
 * 📝 OBSERVAÇÕES:
 *    - NUNCA usa alert() nativo (violação padrão FrotiX)
 *    - Cor do botão: CORES_VEIC.primary (#5f8575)
 *    - Fallback hierárquico: Swal → AppToast → console.error
 * 
 ****************************************************************************************/
function mostrarErro(mensagem) {
    try {
        if (typeof Swal !== 'undefined') {
            Swal.fire({
                icon: 'error',
                title: 'Erro',
                text: mensagem,
                confirmButtonColor: CORES_VEIC.primary
            });
        } else if (typeof AppToast !== 'undefined') {
            AppToast.show('error', mensagem);
        } else {
            console.error('[dashboard-veiculos.js] Erro crítico (SweetAlert e AppToast indisponíveis):', mensagem);
        }
    } catch (error) {
        console.error('[dashboard-veiculos.js] Erro ao exibir mensagem de erro:', error);
    }
}
