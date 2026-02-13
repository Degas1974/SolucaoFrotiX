/* ****************************************************************************************
 * ⚡ ARQUIVO: dashboard-viagens.js
 * ================================================================================================
 * 
 * 📋 OBJETIVO:
 *    Dashboard analítico e interativo de viagens do sistema FrotiX. Apresenta métricas
 *    consolidadas, gráficos dinâmicos com Syncfusion EJ2 Charts, filtros temporais
 *    (ano/mês/período personalizado), TOP 10 viagens mais caras, heatmap dia/hora e
 *    análises de custos por categoria, motorista, veículo, finalidade e requisitante.
 *    Inclui modal de detalhes de viagem com breakdown de custos e edição via botão externo.
 * 
 * 🔢 PARÂMETROS DE ENTRADA:
 *    - Filtros de período: ano, mês (dropdowns), dataInicio, dataFim (date inputs)
 *    - Botões de período rápido: 7, 15, 30, 60, 90, 180, 365 dias
 *    - Click em linhas do TOP 10: abre modal com detalhes da viagem
 *    - Click em card "KM Rodado": abre modal de ajuste de KM (se zero)
 * 
 * 📤 SAÍDAS PRODUZIDAS:
 *    - 15 gráficos Syncfusion (Column, Bar, Area, Pie, Heatmap, Line)
 *    - 21 cards estatísticos com métricas de viagem/custo/KM
 *    - Tabela TOP 10 viagens mais caras (clicável para modal)
 *    - Modal detalhamento de viagem (custos breakdown + botão editar)
 *    - Modal ajuste de KM rodado (caso viagem tenha KmRodado = 0)
 *    - Indicadores de variação percentual vs período anterior
 * 
 * 🔗 DEPENDÊNCIAS:
 *    • BIBLIOTECAS:
 *      - Syncfusion EJ2 Charts (ej.charts.Chart, ej.charts.AccumulationChart)
 *      - jQuery 3.x (AJAX, DOM manipulation)
 *      - Bootstrap 5.x (Grid, Modal, Tooltip)
 *      - Moment.js (manipulação de datas)
 *    • ARQUIVOS FROTIX:
 *      - alerta.js (Alerta.TratamentoErroComLinha)
 *      - sweetalert_interop.js (SweetAlert para confirmações)
 *      - global-toast.js (AppToast.show)
 *      - FrotiX.css (estilos de cards, badges, loadings)
 *    • APIS:
 *      - /api/DashboardViagens/ObterEstatisticasGerais (GET)
 *      - /api/DashboardViagens/ObterViagensPorDia (GET)
 *      - /api/DashboardViagens/ObterViagensPorStatus (GET)
 *      - /api/DashboardViagens/ObterViagensPorMotorista (GET, top=10)
 *      - /api/DashboardViagens/ObterViagensPorVeiculo (GET, top=10)
 *      - /api/DashboardViagens/ObterCustosPorDia (GET)
 *      - /api/DashboardViagens/ObterCustosPorTipo (GET)
 *      - /api/DashboardViagens/ObterViagensPorFinalidade (GET, top=10)
 *      - /api/DashboardViagens/ObterViagensPorRequisitante (GET, top=6)
 *      - /api/DashboardViagens/ObterViagensPorSetor (GET, top=6)
 *      - /api/DashboardViagens/ObterCustosPorMotorista (GET, top=10)
 *      - /api/DashboardViagens/ObterCustosPorVeiculo (GET, top=10)
 *      - /api/DashboardViagens/ObterTop10ViagensMaisCaras (GET)
 *      - /api/DashboardViagens/ObterHeatmapViagens (GET)
 *      - /api/DashboardViagens/ObterTop10VeiculosPorKm (GET)
 *      - /api/DashboardViagens/ObterCustoMedioPorFinalidade (GET)
 * 
 * ================================================================================================
 * 📑 ÍNDICE DE FUNÇÕES (81 funções)
 * ================================================================================================
 * 
 * ┌─────────────────────────────────────────────────────────────────────────────────────────┐
 * │ 🎯 FUNÇÕES PRINCIPAIS DE INICIALIZAÇÃO E CARREGAMENTO                                    │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • inicializarDashboard()                   → Entry point, define período padrão          │
 * │ • carregarDadosDashboard()                 → Promise.allSettled 16 endpoints paralelos   │
 * │ • carregarEstatisticasGerais()             → Cards principais + variações                │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🏗️ FUNÇÕES DE RENDERIZAÇÃO DE GRÁFICOS SYNCFUSION                                       │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • renderizarGraficoViagensPorDia(dados)    → Column chart (7 dias da semana)            │
 * │ • renderizarGraficoViagensPorStatus(dados) → Donut chart (Finalizadas/Andamento/etc)    │
 * │ • renderizarGraficoViagensPorMotorista()   → Column chart TOP 10 motoristas             │
 * │ • renderizarGraficoViagensPorVeiculo()     → Column chart TOP 10 veículos               │
 * │ • renderizarGraficoCustosPorDia()          → Area chart (série temporal)                │
 * │ • renderizarGraficoCustosPorTipo()         → Donut chart (5 tipos: combustível/veic)    │
 * │ • renderizarGraficoViagensPorFinalidade()  → Column chart TOP 10 finalidades            │
 * │ • renderizarGraficoViagensPorRequisitante()→ Bar chart TOP 6 requisitantes              │
 * │ • renderizarGraficoViagensPorSetor()       → Bar chart TOP 6 setores                    │
 * │ • renderizarGraficoCustosPorMotorista()    → Column chart TOP 10 custos/motorista       │
 * │ • renderizarGraficoCustosPorVeiculo()      → Column chart TOP 10 custos/veículo         │
 * │ • renderizarTop10VeiculosKm()              → Bar chart TOP 10 KM rodado                 │
 * │ • renderizarCustoMedioPorFinalidade()      → Dual-axis (bars + line overlay)            │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 📊 CARREGAMENTO DE DADOS INDIVIDUAIS (13 endpoints)                                     │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • carregarViagensPorDia()                  → Fetch + render gráfico dia semana          │
 * │ • carregarViagensPorStatus()               → Fetch + render donut status                │
 * │ • carregarViagensPorMotorista()            → Fetch + render column motorista            │
 * │ • carregarViagensPorVeiculo()              → Fetch + render column veículo              │
 * │ • carregarCustosPorDia()                   → Fetch + render area temporal               │
 * │ • carregarCustosPorTipo()                  → Fetch + render donut custos                │
 * │ • carregarViagensPorFinalidade()           → Fetch + render column finalidade           │
 * │ • carregarViagensPorRequisitante()         → Fetch + render bar TOP 6 requisitante      │
 * │ • carregarViagensPorSetor()                → Fetch + render bar TOP 6 setor             │
 * │ • carregarCustosPorMotorista()             → Fetch + render column custos/motorista     │
 * │ • carregarCustosPorVeiculo()               → Fetch + render column custos/veículo       │
 * │ • carregarTop10ViagensMaisCaras()          → Fetch + render tabela TOP 10 clicável      │
 * │ • carregarHeatmapViagens()                 → Fetch + render heatmap 7x24 (dia/hora)     │
 * │ • carregarTop10VeiculosKm()                → Fetch + render bar TOP 10 KM               │
 * │ • carregarCustoMedioPorFinalidade()        → Fetch + render dual-axis chart             │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🎨 FORMATAÇÃO E HELPERS VISUAIS                                                          │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • formatarNumero(valor, casasDecimais)     → Formato pt-BR (1.234.567,89)               │
 * │ • formatarValorMonetario(valor)            → <100: 2 casas; ≥100: 0 casas               │
 * │ • formatarDuracao(minutos)                 → "2h 05min" ou "45min"                      │
 * │ • formatarDataParaInput(data)              → YYYY-MM-DD para input[type=date]           │
 * │ • atualizarVariacao(elemId, atual, anterior) → Badge verde/vermelho/neutro             │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔧 FILTROS E MANIPULAÇÃO DE PERÍODO                                                      │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • aplicarFiltroPeriodo(dias)               → Define últimos N dias (7/15/30/60/etc)     │
 * │ • aplicarFiltroPersonalizado()             → Valida dataInicio/dataFim → carrega        │
 * │ • limparFiltroPeriodo()                    → Reset para últimos 30 dias                 │
 * │ • inicializarCamposData()                  → Preenche inputs com datas padrão            │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🗂️ TABELAS E RENDERIZAÇÕES TABULARES                                                    │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • renderizarTabelaTop10(dados)             → Tabela clicável TOP 10 viagens mais caras  │
 * │ • renderizarHeatmapViagens(dados, maxV)    → Grid 7x24 com cores por intensidade        │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🎭 MODAIS E INTERAÇÕES                                                                   │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • abrirModalDetalhesViagem(index)          → Modal viagem do TOP10 com breakdown custos │
 * │ • inicializarModalAjuste()                 → Prepara modal Bootstrap p/ ajustar KM      │
 * │ • abrirModalAjustarKmViagem()              → Modal p/ corrigir KmRodado=0               │
 * │ • carregarDetalhesViagemParaAjuste(id)     → Busca dados viagem p/ modal ajuste         │
 * │ • salvarAjusteKmViagem()                   → PATCH p/ atualizar KM via API               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🖼️ LOADING E FEEDBACK VISUAL                                                             │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • mostrarLoadingInicial()                  → Overlay logo FrotiX inicial                │
 * │ • esconderLoadingInicial()                 → Fade out overlay inicial                   │
 * │ • mostrarLoadingGeral()                    → Loading overlay em operações AJAX          │
 * │ • esconderLoadingGeral()                   → Remove loading overlay                     │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ 🔁 EXPORTAÇÃO E RELATÓRIOS                                                               │
 * ├─────────────────────────────────────────────────────────────────────────────────────────┤
 * │ • gerarRelatorioPDF()                      → Exporta dashboard para PDF via endpoint    │
 * │ • exportarDadosExcel()                     → Exporta planilha Excel com dados filtrados │
 * └─────────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔄 FLUXOS TÍPICOS  
 * ================================================================================================
 * 
 * 💡 FLUXO 1: Inicialização padrão (últimos 30 dias)
 *    ┌─────────────────────────────────────────────────────────────────────────────────────┐
 *    │ DOMContentLoaded → inicializarDashboard()                                            │
 *    │   ↓ Define periodoAtual (hoje - 30 dias)                                            │
 *    │   ↓ inicializarCamposData() → preenche inputs date                                  │
 *    │   ↓ inicializarModalAjuste() → prepara modal Bootstrap                              │
 *    │   ↓ carregarDadosDashboard()                                                         │
 *    │      ↓ Promise.allSettled → 16 endpoints paralelos (não bloqueia se 1 falhar)       │
 *    │      ↓ carregarEstatisticasGerais() → 21 cards + variações                          │
 *    │      ↓ carregarViagensPorDia() → gráfico column 7 dias                              │
 *    │      ↓ carregarViagensPorStatus() → donut 4 status                                  │
 *    │      ↓ carregarViagensPorMotorista() → column TOP 10                                │
 *    │      ↓ carregarTop10ViagensMaisCaras() → tabela clicável                            │
 *    │      ↓ carregarHeatmapViagens() → grid 7x24                                         │
 *    │   ↓ esconderLoadingInicial()                                                         │
 *    │   ↓ AppToast.show('Verde', 'Dashboard carregado', 3000)                             │
 *    └────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * 💡 FLUXO 2: Filtro de período rápido (ex: últimos 7 dias)
 *    ┌─────────────────────────────────────────────────────────────────────────────────────┐
 *    │ Click botão "7 dias" → aplicarFiltroPeriodo(7)                                      │
 *    │   ↓ Calcula: hoje - 7 dias                                                          │
 *    │   ↓ Atualiza periodoAtual.dataInicio/dataFim                                        │
 *    │   ↓ Atualiza inputs date HTML                                                       │
 *    │   ↓ carregarDadosDashboard()                                                         │
 *    │      ↓ Todos os 16 endpoints recebem novos params (dataInicio, dataFim)             │
 *    │      ↓ Re-renderiza todos os gráficos e tabelas                                     │
 *    └─────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * 💡 FLUXO 3: Click em viagem do TOP 10 → detalhes
 *    ┌─────────────────────────────────────────────────────────────────────────────────────┐
 *    │ Click <tr> tabela TOP 10 → abrirModalDetalhesViagem(index)                          │
 *    │   ↓ Obtém dados da viagem de dadosTop10Viagens[index]                               │
 *    │   ↓ Preenche modal: Nº Ficha, Status, Data, Motorista, Veículo                     │
 *    │   ↓ Preenche breakdown custos: Combustível, Veículo, Motorista, Operador, Lavador  │
 *    │   ↓ Se kmRodado = 0 → exibe alerta amarelo com botão "Ajustar KM"                  │
 *    │   ↓ Botão "Editar Viagem" → redireciona p/ /Viagens/Upsert/{viagemId}              │
 *    │   ↓ new bootstrap.Modal().show()                                                    │
 *    └─────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * 💡 FLUXO 4: Ajuste de KM para viagem com KmRodado = 0
 *    ┌─────────────────────────────────────────────────────────────────────────────────────┐
 *    │ Click card "KM Rodado" (quando valor = 0) → abrirModalAjustarKmViagem()            │
 *    │   ↓ carregarDetalhesViagemParaAjuste(viagemAtualId)                                 │
 *    │      ↓ GET /api/Viagem/ObterDetalhes/{id}                                           │
 *    │      ↓ Preenche modal: Nº Ficha, Motorista, Veículo, Data Inicial                  │
 *    │      ↓ Input KM Rodado com valor atual (0)                                          │
 *    │   ↓ Usuário digita novo KM                                                          │
 *    │   ↓ salvarAjusteKmViagem()                                                           │
 *    │      ↓ Validação: KM > 0 e ≤ 999999                                                 │
 *    │      ↓ PATCH /api/Viagem/AtualizarKmRodado                                          │
 *    │         { viagemId, kmRodado }                                                       │
 *    │      ↓ Success → AppToast.show('Verde') + recalcula custos + atualiza card          │
 *    │      ↓ Erro → AppToast.show('Vermelho')                                             │
 *    └─────────────────────────────────────────────────────────────────────────────────────┘
 * 
 * ================================================================================================
 * 🔍 OBSERVAÇÕES TÉCNICAS
 * ================================================================================================
 * 
 * 🎨 PALETA DE CORES FROTIX (8 cores padrão):
 *    - azul: #0D47A1 (gráficos principais)
 *    - verde: #16a34a (badges sucesso)
 *    - laranja: #d97706 (alertas)
 *    - amarelo: #f59e0b (warnings)
 *    - vermelho: #dc2626 (erros)
 *    - roxo: #9d4edd (heatmap alta intensidade)
 *    - ciano: #22d3ee (gráficos secundários)
 *    - rosa: #ec4899 (destaques)
 * 
 * 📊 SYNCFUSION EJ2 CHARTS - Tipos usados:
 *    - ej.charts.Chart → Column, Bar, Line, Area, SplineArea, StackingColumn
 *    - ej.charts.AccumulationChart → Pie, Donut (innerRadius: 40%)
 *    - Configurações padrão:
 *      • tooltip: { enable: true, format: personalizado }
 *      • legendSettings: { visible: true/false, position: 'Bottom' }
 *      • chartArea: { border: { width: 0 } }
 *      • axisLabelRender: formatação pt-BR com formatarNumero()
 *      • tooltipRender: formatação customizada em callbacks
 * 
 * 🔄 PROMISE.ALLSETTLED (não bloqueia):
 *    - Se 1 endpoint falhar, os outros 15 continuam processando
 *    - Log de falhas: console.error com nome do endpoint
 *    - Tempo total logado: console.log(`✅ Dashboard carregado em ${tempo}s`)
 * 
 * 🗂️ HEATMAP 7x24 (Dia da Semana x Hora):
 *    - 7 linhas (Dom-Sáb) × 24 colunas (00h-23h) = 168 células
 *    - Cor baseada em intensidade: obterCorHeatmap(valor, max)
 *      • 0-20% → #e8f5e9 (verde muito claro)
 *      • 20-40% → #c8e6c9
 *      • 40-60% → #81c784
 *      • 60-80% → #4caf50
 *      • 80-100% → #2e7d32 (verde escuro)
 *    - Hover: transform: scale(1.1) + zIndex: 10
 *    - Tooltip nativo com `title` attribute
 * 
 * 📱 RESPONSIVIDADE:
 *    - Gráficos com height fixa em px (280px-420px)
 *    - Grid Bootstrap 5: col-lg-3/4/6 com ordem responsiva
 *    - Tabela TOP 10: overflow-x-auto em mobile
 *    - Modal: max-width 90% em telas < 768px
 * 
 * 🏷️ BADGES E VARIAÇÕES:
 *    - Variação positiva: verde + ↑ (crescimento bom)
 *    - Variação negativa: vermelho + ↓ (queda ruim)
 *    - Variação neutra: cinza + = (sem mudança)
 *    - Cálculo: ((atual - anterior) / anterior * 100).toFixed(1) + '%'
 * 
 * 🚨 TRATAMENTO DE ERROS:
 *    - Try-catch em TODAS as funções
 *    - Alerta.TratamentoErroComLinha('dashboard-viagens.js', funcao, error)
 *    - Fallback: gráfico vazio com mensagem "<div class='text-center text-muted'>Sem dados</div>"
 *    - Nunca trava a página, apenas loga erro no console
 * 
 * 🔐 PERMISSÕES:
 *    - Botão "Editar Viagem" visível apenas se usuário tiver permissão
 *    - Verificação via atributo data-can-edit no botão (definido no backend)
 *    - Botão "Ajustar KM" visível apenas para gestores (role check server-side)
 * 
 * 🎯 PERFORMANCE:
 *    - 16 requests paralelos (Promise.allSettled) reduz tempo total em ~70%
 *    - Gráficos destruídos antes de recriar (chart.destroy())
 *    - Throttle no resize: recalcula gráficos apenas após 300ms sem resize
 *    - Cache de dados em variáveis globais (dadosTop10Viagens)
 * 
 * ================================================================================================
 * 📌 CONVENÇÕES DE NOMENCLATURA
 * ================================================================================================
 * 
 * FUNÇÕES:
 *    • camelCase: inicializarDashboard, carregarDadosDashboard
 *    • Prefixos:
 *      - carregar* → fetch de API + renderização
 *      - renderizar* → apenas renderização (recebe dados)
 *      - aplicar* → ações de filtro/configuração
 *      - formatar* → conversão de valores (string, número, data)
 *      - abrir/fechar* → controle de modais
 *      - inicializar* → setup inicial de componentes
 * 
 * VARIÁVEIS:
 *    • camelCase: periodoAtual, chartViagensPorStatus
 *    • Constantes: MAIÚSCULAS com underscore (CORES_FROTIX)
 *    • Arrays de dados cache: prefixo "dados" (dadosTop10Viagens)
 *    • Instâncias de gráfico: prefixo "chart" (chartCustosPorTipo)
 *    • Modais: sufixo "Modal" (modalAjustaViagemDashboard)
 * 
 * IDS DE ELEMENTOS:
 *    • Cards: prefixo "stat" (statTotalViagens, statCustoTotal)
 *    • Gráficos: prefixo "chart" (chartViagensPorDia)
 *    • Inputs: prefixo "filtro" ou nome descritivo (dataInicio, filtroAno)
 *    • Botões: prefixo "btn" (btnFiltrar, btnLimpar)
 *    • Variações: prefixo "variacao" (variacaoCusto)
 * 
 * **************************************************************************************** */

// Paleta de Cores FrotiX
const CORES_FROTIX = {
    azul: '#0D47A1',
    verde: '#16a34a',
    laranja: '#d97706',
    amarelo: '#f59e0b',
    vermelho: '#dc2626',
    roxo: '#9d4edd',
    ciano: '#22d3ee',
    rosa: '#ec4899'
};

// ========================================
// FUNÇÃO DE FORMATAÇÃO DE NÚMEROS
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: formatarNumero
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Formata números com separador de milhar (ponto) e decimais (vírgula)
 *                   seguindo o padrão brasileiro pt-BR.
 *
 * 📥 ENTRADAS     : valor [number] - Número a ser formatado (pode ser null/undefined)
 *                   casasDecimais [number] - Quantidade de casas decimais (padrão: 0)
 *
 * 📤 SAÍDAS       : [string] Número formatado (ex: "1.234.567,89") ou "0" se inválido
 *
 * 🔗 CHAMADA POR  : Todas as funções de renderização de gráficos (axisLabelRender,
 *                   tooltipRender), carregarEstatisticasGerais, renderizarTabelaTop10,
 *                   atualizarVariacao, ~40 pontos no código
 *
 * 🔄 CHAMA        : Number.toFixed(), String.replace(), String.split()
 *
 * 📝 OBSERVAÇÕES  : • Valores null/undefined/NaN retornam "0"
 *                   • Usa regex /\B(?=(\d{3})+(?!\d))/g para separador de milhar
 *                   • Vírgula como separador decimal (padrão BR)
 ****************************************************************************************/
function formatarNumero(valor, casasDecimais = 0)
{
    try
    {
        if (valor === null || valor === undefined || isNaN(valor))
        {
            return '0';
        }

        // Arredonda para o número de casas decimais
        const valorArredondado = Number(valor).toFixed(casasDecimais);

        // Separa parte inteira e decimal
        const partes = valorArredondado.split('.');
        const parteInteira = partes[0];
        const parteDecimal = partes[1];

        // Adiciona separador de milhar (ponto)
        const parteInteiraFormatada = parteInteira.replace(/\B(?=(\d{3})+(?!\d))/g, '.');

        // Retorna com vírgula como separador decimal
        if (casasDecimais > 0 && parteDecimal)
        {
            return `${parteInteiraFormatada},${parteDecimal}`;
        }

        return parteInteiraFormatada;
    } catch (error)
    {
        console.error('Erro ao formatar número:', error);
        return '0';
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: formatarValorMonetario
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Formata valores monetários com regra especial de casas decimais:
 *                   • Valores < R$ 100,00 → 2 casas decimais (ex: R$ 99,50)
 *                   • Valores >= R$ 100,00 → sem casas decimais (ex: R$ 1.234)
 *
 * 📥 ENTRADAS     : valor [number] - Valor monetário a ser formatado
 *
 * 📤 SAÍDAS       : [string] Valor formatado sem prefixo R$ (ex: "1.234" ou "99,50")
 *
 * 🔗 CHAMADA POR  : carregarEstatisticasGerais (cards de custo), renderizarTabelaTop10,
 *                   abrirModalDetalhesViagem, todas as funções de tooltip de gráficos
 *                   de custo
 *
 * 🔄 CHAMA        : formatarNumero(valor, casasDecimais)
 *
 * 📝 OBSERVAÇÕES  : • Retorna "0" se valor inválido (null/undefined/NaN)
 *                   • Regra especial implementada para melhorar UX (detalhes em valores
 *                     pequenos, limpeza visual em valores grandes)
 ****************************************************************************************/
function formatarValorMonetario(valor)
{
    try
    {
        if (valor === null || valor === undefined || isNaN(valor))
        {
            return '0';
        }

        const valorNumerico = Number(valor);
        
        // Se valor < 100, mostra com 2 casas decimais
        if (valorNumerico < 100)
        {
            return formatarNumero(valorNumerico, 2);
        }
        
        // Se valor >= 100, mostra sem casas decimais
        return formatarNumero(valorNumerico, 0);
    } catch (error)
    {
        console.error('Erro ao formatar valor monetário:', error);
        return '0';
    }
}

let periodoAtual = {
    dataInicio: null,
    dataFim: null
};

// Variáveis para armazenar gráficos
let chartViagensPorStatus = null;
let chartCustosPorTipo = null;

// Variáveis para PDFViewer
let pdfAtualBlob = null;
let pdfViewerInstance = null;

// Variáveis para o Modal de Ajuste de Viagem (Dashboard)
let viagemAtualId = null;
let modalAjustaViagemDashboard = null;

// ========================================
// LOADING INICIAL DA PÁGINA
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: mostrarLoadingInicial
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Exibe overlay de loading fullscreen com logo FrotiX pulsante durante
 *                   carregamento inicial do dashboard.
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Elemento #loadingInicialDashboard visível (display: flex)
 *
 * 🔗 CHAMADA POR  : inicializarDashboard() [linha ~129]
 *
 * 🔄 CHAMA        : document.getElementById(), element.style.display = 'flex'
 *
 * 📝 OBSERVAÇÕES  : • Overlay fullscreen com backdrop semi-transparente
 *                   • Logo FrotiX animado (pulse) + barra de progresso
 *                   • Z-index alto para sobrepor todo o conteúdo
 ****************************************************************************************/
function mostrarLoadingInicial()
{
    try
    {
        const loadingEl = document.getElementById('loadingInicialDashboard');
        if (loadingEl)
        {
            loadingEl.style.display = 'flex';
        }
    } catch (error)
    {
        console.error('Erro ao mostrar loading inicial:', error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: esconderLoadingInicial
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Esconde overlay de loading inicial com fade out suave (300ms) após
 *                   dashboard totalmente carregado.
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Elemento #loadingInicialDashboard oculto (opacity 0 → display none)
 *
 * 🔗 CHAMADA POR  : inicializarDashboard() [linha ~150], carregarDadosDashboard() [em caso de erro]
 *
 * 🔄 CHAMA        : document.getElementById(), element.style.opacity = '0',
 *                   setTimeout(), element.style.display = 'none'
 *
 * 📝 OBSERVAÇÕES  : • Fade out de 300ms para transição suave
 *                   • Primeiro reduz opacity, depois remove do layout (display none)
 ****************************************************************************************/
function esconderLoadingInicial()
{
    try
    {
        const loadingEl = document.getElementById('loadingInicialDashboard');
        if (loadingEl)
        {
            loadingEl.style.opacity = '0';
            setTimeout(function() {
                loadingEl.style.display = 'none';
            }, 300);
        }
    } catch (error)
    {
        console.error('Erro ao esconder loading inicial:', error);
    }
}

// ========================================
// INICIALIZAÇÃO
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: inicializarDashboard
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Entry point do dashboard. Inicializa período padrão (últimos 30 dias),
 *                   prepara campos de data HTML5, configura modal de ajuste e carrega
 *                   todos os dados do dashboard via Promise.allSettled.
 *
 * 📥 ENTRADAS     : Nenhuma (chamada pelo DOMContentLoaded)
 *
 * 📤 SAÍDAS       : Dashboard totalmente carregado com 16 endpoints, gráficos renderizados,
 *                   toast de sucesso "Dashboard carregado com sucesso!"
 *
 * 🔗 CHAMADA POR  : $(document).ready() [linha ~3650]
 *
 * 🔄 CHAMA        : mostrarLoadingInicial(), inicializarCamposData(),
 *                   inicializarModalAjuste(), carregarDadosDashboard(),
 *                   esconderLoadingInicial(), AppToast.show()
 *
 * 📝 OBSERVAÇÕES  : • Função async para suportar await carregarDadosDashboard()
 *                   • Período padrão: hoje 23:59:59 até hoje-30 dias 00:00:00
 *                   • Try-catch global trata falhas e esconde loading mesmo com erro
 ****************************************************************************************/
async function inicializarDashboard()
{
    try
    {
        // Mostra loading inicial da página
        mostrarLoadingInicial();

        // Define período padrão (últimos 30 dias)
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);

        // Inicializa campos de data HTML5
        inicializarCamposData();

        // Inicializa modal de ajuste de viagem
        inicializarModalAjuste();

        // Carrega dashboard
        await carregarDadosDashboard();

        // Esconde loading inicial
        esconderLoadingInicial();

        AppToast.show('Verde', 'Dashboard carregado com sucesso!', 3000);
    } catch (error)
    {
        esconderLoadingInicial();
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarDashboard', error);
    }
}

// ========================================
// CAMPOS DE DATA HTML5
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: inicializarCamposData
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Preenche inputs HTML5 type="date" com valores do periodoAtual e
 *                   adiciona event listeners para sincronizar mudanças com o objeto
 *                   periodoAtual global.
 *
 * 📥 ENTRADAS     : Nenhuma (usa variável global periodoAtual)
 *
 * 📤 SAÍDAS       : Inputs #dataInicio e #dataFim preenchidos com formato YYYY-MM-DD,
 *                   event listeners registrados
 *
 * 🔗 CHAMADA POR  : inicializarDashboard() [linha ~134]
 *
 * 🔄 CHAMA        : formatarDataParaInput(data), addEventListener('change')
 *
 * 📝 OBSERVAÇÕES  : • Formato YYYY-MM-DD obrigatório para input[type=date]
 *                   • Atualiza periodoAtual.dataInicio com hora 00:00:00
 *                   • Atualiza periodoAtual.dataFim com hora 23:59:59
 ****************************************************************************************/
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
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarCamposData', error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: formatarDataParaInput
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Converte objeto Date JavaScript para string no formato YYYY-MM-DD
 *                   compatível com input HTML5 type="date".
 *
 * 📥 ENTRADAS     : data [Date] - Objeto Date JavaScript
 *
 * 📤 SAÍDAS       : [string] Data formatada no formato "YYYY-MM-DD" (ex: "2026-02-02")
 *                   ou string vazia se erro
 *
 * 🔗 CHAMADA POR  : inicializarCamposData(), aplicarFiltroPeriodo()
 *
 * 🔄 CHAMA        : Date.getFullYear(), Date.getMonth(), Date.getDate(),
 *                   String.padStart()
 *
 * 📝 OBSERVAÇÕES  : • Mês JavaScript é 0-indexed, precisa adicionar +1
 *                   • PadStart garante 2 dígitos (01, 02... 12)
 *                   • Retorna string vazia se data inválida (try-catch)
 ****************************************************************************************/
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

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarDadosDashboard
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Carrega TODOS os dados do dashboard em paralelo usando Promise.allSettled
 *                   (16 endpoints simultâneos). Não bloqueia se um endpoint falhar.
 *                   Registra tempo total de carregamento e logs de falhas.
 *
 * 📥 ENTRADAS     : Nenhuma (usa variável global periodoAtual para parâmetros de data)
 *
 * 📤 SAÍDAS       : Dashboard completo renderizado com 16 seções:
 *                   • Cards estatísticos (21 cards)
 *                   • 15 gráficos Syncfusion
 *                   • Tabela TOP 10 viagens mais caras
 *                   • Heatmap 7x24 (dia × hora)
 *                   Console log: "✅ Dashboard carregado em X.XXs"
 *
 * 🔗 CHAMADA POR  : inicializarDashboard(), aplicarFiltroPeriodo(), atualizarDashboard(),
 *                   filtrarPorAnoMes(), limparFiltroAnoMes(), limparFiltroPeriodo(),
 *                   gravarViagemDashboard() [após atualização de viagem]
 *
 * 🔄 CHAMA        : Promise.allSettled com 16 funções:
 *                   carregarEstatisticasGerais(), carregarViagensPorDia(),
 *                   carregarViagensPorStatus(), carregarViagensPorMotorista(),
 *                   carregarViagensPorVeiculo(), carregarCustosPorDia(),
 *                   carregarCustosPorTipo(), carregarViagensPorFinalidade(),
 *                   carregarViagensPorRequisitante(), carregarViagensPorSetor(),
 *                   carregarCustosPorMotorista(), carregarCustosPorVeiculo(),
 *                   carregarTop10ViagensMaisCaras(), carregarHeatmapViagens(),
 *                   carregarTop10VeiculosKm(), carregarCustoMedioPorFinalidade()
 *                   + mostrarLoadingGeral(), esconderLoadingGeral(), performance.now()
 *
 * 📝 OBSERVAÇÕES  : • Função async retorna Promise<void>
 *                   • Promise.allSettled garante que falha em 1 endpoint não trava os outros
 *                   • REMOVIDO: carregarKmPorVeiculo() (usava ViagemEstatistica com dados errados)
 *                   • Logs de falhas: console.error com nome do endpoint que falhou
 *                   • Performance: ~70% mais rápido que requests sequenciais
 ****************************************************************************************/
async function carregarDadosDashboard()
{
    try
    {
        console.log('⏱️ Iniciando carregamento do dashboard...');
        const inicio = performance.now();

        mostrarLoadingGeral();

        // Promise.allSettled não trava se um falhar
        // NOTA: carregarKmPorVeiculo foi REMOVIDO - usava ViagemEstatistica com dados errados
        // Mantido apenas carregarTop10VeiculosKm que usa tabela Viagem diretamente
        const resultados = await Promise.allSettled([
            carregarEstatisticasGerais(),
            carregarViagensPorDia(),
            carregarViagensPorStatus(),
            carregarViagensPorMotorista(),
            carregarViagensPorVeiculo(),
            carregarCustosPorDia(),
            carregarCustosPorTipo(),
            carregarViagensPorFinalidade(),
            carregarViagensPorRequisitante(),
            carregarViagensPorSetor(),
            carregarCustosPorMotorista(),
            carregarCustosPorVeiculo(),
            carregarTop10ViagensMaisCaras(),
            carregarHeatmapViagens(),
            carregarTop10VeiculosKm(),
            carregarCustoMedioPorFinalidade()
        ]);

        const tempo = ((performance.now() - inicio) / 1000).toFixed(2);
        console.log(`✅ Dashboard carregado em ${tempo}s`);

        // Log de falhas
        const nomes = [
            'EstatisticasGerais', 'ViagensPorDia', 'ViagensPorStatus', 'ViagensPorMotorista',
            'ViagensPorVeiculo', 'CustosPorDia', 'CustosPorTipo', 'ViagensPorFinalidade',
            'ViagensPorRequisitante', 'ViagensPorSetor', 'CustosPorMotorista',
            'CustosPorVeiculo', 'Top10ViagensMaisCaras', 'HeatmapViagens', 'Top10VeiculosKm',
            'CustoMedioPorFinalidade'
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
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosDashboard', error);
        esconderLoadingGeral();
    }
}

// ========================================
// ESTATÍSTICAS GERAIS
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarEstatisticasGerais
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Carrega e atualiza os 21 cards de estatísticas principais do dashboard:
 *                   • 9 cards principais (custo total/médio, viagens, km total/médio, status)
 *                   • 5 cards de custo por tipo (combustível, veículo, motorista, operador, lavador)
 *                   • 9 badges de variação percentual vs período anterior (verde/vermelho)
 *
 * 📥 ENTRADAS     : Nenhuma (usa periodoAtual.dataInicio e periodoAtual.dataFim)
 *
 * 📤 SAÍDAS       : Cards HTML atualizados via jQuery:
 *                   #statTotalViagens, #statViagensFinalizadas, #statCustoTotal,
 *                   #statCustoMedio, #statKmTotal, #statKmMedio, #statViagensEmAndamento,
 *                   #statViagensAgendadas, #statViagensCanceladas, #statCustoCombustivel,
 *                   #statCustoVeiculo, #statCustoMotorista, #statCustoOperador,
 *                   #statCustoLavador + 14 badges de variação (#variacao*)
 *
 * 🔗 CHAMADA POR  : carregarDadosDashboard() [linha ~197]
 *
 * 🔄 CHAMA        : fetch('/api/DashboardViagens/ObterEstatisticasGerais?...'),
 *                   formatarNumero(), formatarValorMonetario(), atualizarVariacao(),
 *                   jQuery $() para atualizar DOM
 *
 * 📝 OBSERVAÇÕES  : • Endpoint retorna {success, data: {...}, periodoAnterior: {...}}
 *                   • Se periodoAnterior não existe, badges mostram "-" (neutro)
 *                   • Usa formatarValorMonetario (regra <100: 2 casas, >=100: 0 casas)
 *                   • KM sempre com 0 casas decimais exceto km médio (2 casas)
 ****************************************************************************************/
async function carregarEstatisticasGerais()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterEstatisticasGerais?${params}`);
        const result = await response.json();

        if (result.success)
        {
            const data = result;

            // Atualiza cards principais - COM SEPARADOR DE MILHAR
            $('#statTotalViagens').text(formatarNumero(data.totalViagens, 0));
            $('#statViagensFinalizadas').text(formatarNumero(data.viagensFinalizadas, 0));
            $('#statCustoTotal').text('R$ ' + formatarValorMonetario(data.custoTotal));
            $('#statCustoMedio').text('R$ ' + formatarValorMonetario(data.custoMedioPorViagem));
            $('#statKmTotal').text(formatarNumero(data.kmTotal, 0) + ' km');
            $('#statKmMedio').text(formatarNumero(data.kmMedioPorViagem, 2) + ' km');
            $('#statViagensEmAndamento').text(formatarNumero(data.viagensEmAndamento, 0));
            $('#statViagensAgendadas').text(formatarNumero(data.viagensAgendadas || 0, 0));
            $('#statViagensCanceladas').text(formatarNumero(data.viagensCanceladas, 0));

            // Atualiza variações (se existirem dados do período anterior na API)
            if (data.periodoAnterior)
            {
                // Cards principais
                atualizarVariacao('variacaoCusto', data.custoTotal, data.periodoAnterior.custoTotal);
                atualizarVariacao('variacaoViagens', data.totalViagens, data.periodoAnterior.totalViagens);
                atualizarVariacao('variacaoCustoMedio', data.custoMedioPorViagem, data.periodoAnterior.custoMedioPorViagem);
                atualizarVariacao('variacaoKm', data.kmTotal, data.periodoAnterior.kmTotal);
                atualizarVariacao('variacaoKmMedio', data.kmMedioPorViagem, data.periodoAnterior.kmMedioPorViagem);

                // Cards de status
                atualizarVariacao('variacaoRealizadas', data.viagensFinalizadas, data.periodoAnterior.viagensFinalizadas);
                atualizarVariacao('variacaoAbertas', data.viagensEmAndamento, data.periodoAnterior.viagensEmAndamento);
                atualizarVariacao('variacaoAgendadas', data.viagensAgendadas, data.periodoAnterior.viagensAgendadas);
                atualizarVariacao('variacaoCanceladas', data.viagensCanceladas, data.periodoAnterior.viagensCanceladas);

                // Cards de custo por tipo
                atualizarVariacao('variacaoCustoCombustivel', data.custoCombustivel, data.periodoAnterior.custoCombustivel);
                atualizarVariacao('variacaoCustoVeiculo', data.custoVeiculo, data.periodoAnterior.custoVeiculo);
                atualizarVariacao('variacaoCustoMotorista', data.custoMotorista, data.periodoAnterior.custoMotorista);
                atualizarVariacao('variacaoCustoOperador', data.custoOperador, data.periodoAnterior.custoOperador);
                atualizarVariacao('variacaoCustoLavador', data.custoLavador, data.periodoAnterior.custoLavador);
            }
            else
            {
                // Se não houver dados do período anterior, deixa como neutro
                $('#variacaoCusto, #variacaoViagens, #variacaoCustoMedio, #variacaoKm, #variacaoKmMedio, #variacaoRealizadas, #variacaoAbertas, #variacaoAgendadas, #variacaoCanceladas, #variacaoCustoCombustivel, #variacaoCustoVeiculo, #variacaoCustoMotorista, #variacaoCustoOperador, #variacaoCustoLavador')
                    .text('-')
                    .removeClass('variacao-positiva variacao-negativa')
                    .addClass('variacao-neutra');
            }

            // Atualiza cards de custo por tipo - COM SEPARADOR DE MILHAR
            $('#statCustoCombustivel').text('R$ ' + formatarValorMonetario(data.custoCombustivel || 0));
            $('#statCustoVeiculo').text('R$ ' + formatarValorMonetario(data.custoVeiculo || 0));
            $('#statCustoMotorista').text('R$ ' + formatarValorMonetario(data.custoMotorista || 0));
            $('#statCustoOperador').text('R$ ' + formatarValorMonetario(data.custoOperador || 0));
            $('#statCustoLavador').text('R$ ' + formatarValorMonetario(data.custoLavador || 0));
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarEstatisticasGerais', error);
    }
}

// ========================================
// VIAGENS POR DIA
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarViagensPorDia
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Busca dados de viagens agrupadas por dia da semana (Dom-Sáb) e
 *                   renderiza gráfico de colunas Syncfusion (Column Chart).
 *
 * 📥 ENTRADAS     : Nenhuma (usa periodoAtual para parâmetros de API)
 *
 * 📤 SAÍDAS       : Gráfico de colunas renderizado em #chartViagensPorDia com 7 barras
 *                   (uma por dia da semana), altura 350px, cor azul padrão FrotiX
 *
 * 🔗 CHAMADA POR  : carregarDadosDashboard() [linha ~197]
 *
 * 🔄 CHAMA        : fetch('/api/DashboardViagens/ObterViagensPorDia?...'),
 *                   renderizarGraficoViagensPorDia(dados)
 *
 * 📝 OBSERVAÇÕES  : • Endpoint retorna array: [{diaSemana: "Segunda", total: 45}, ...]
 *                   • Try-catch com Alerta.TratamentoErroComLinha
 *                   • Se success=false ou data vazio, gráfico não é renderizado (fail silently)
 ****************************************************************************************/
async function carregarViagensPorDia()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorDia?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorDia(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorDia', error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: renderizarGraficoViagensPorDia
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Renderiza gráfico de colunas Syncfusion EJ2 com dados de viagens
 *                   por dia da semana (7 colunas: Dom-Sáb).
 *
 * 📥 ENTRADAS     : dados [Array<{diaSemana: string, total: number}>] - Array com 7 itens
 *
 * 📤 SAÍDAS       : Gráfico Column Chart Syncfusion renderizado em #chartViagensPorDia
 *                   com tooltip, labels formatados pt-BR, canto arredondado (10px)
 *
 * 🔗 CHAMADA POR  : carregarViagensPorDia() [linha ~373]
 *
 * 🔄 CHAMA        : new ej.charts.Chart({...}), chart.appendTo('#chartViagensPorDia'),
 *                   formatarNumero() [callbacks axisLabelRender e tooltipRender]
 *
 * 📝 OBSERVAÇÕES  : • Tipo: Column Chart com cornerRadius topLeft/topRight 10px
 *                   • Cor: CORES_FROTIX.azul (#0D47A1)
 *                   • Altura: 350px
 *                   • Eixo X: Category (dias da semana)
 *                   • Eixo Y: Valores numéricos com separador de milhar
 *                   • Legenda: desabilitada (visible: false)
 ****************************************************************************************/
function renderizarGraficoViagensPorDia(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                title: 'Dia da Semana'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'diaSemana',
                yName: 'total',
                name: 'Total',
                type: 'Column',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.azul
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartViagensPorDia');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorDia', error);
    }
}

// ========================================
// VIAGENS POR STATUS
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: carregarViagensPorStatus
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Busca dados de viagens agrupadas por status (Finalizadas, Em Andamento,
 *                   Agendadas, Canceladas) e renderiza gráfico de rosca (Donut Chart).
 *
 * 📥 ENTRADAS     : Nenhuma (usa periodoAtual para parâmetros de API)
 *
 * 📤 SAÍDAS       : Gráfico de rosca renderizado em #chartViagensPorStatus com 4 fatias,
 *                   altura 350px, legenda na parte inferior
 *
 * 🔗 CHAMADA POR  : carregarDadosDashboard() [linha ~197]
 *
 * 🔄 CHAMA        : fetch('/api/DashboardViagens/ObterViagensPorStatus?...'),
 *                   renderizarGraficoViagensPorStatus(dados)
 *
 * 📝 OBSERVAÇÕES  : • Endpoint retorna array: [{status: "Finalizadas", total: 120}, ...]
 *                   • Se success=false ou data vazio, gráfico não renderizado
 ****************************************************************************************/
async function carregarViagensPorStatus()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorStatus?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorStatus(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorStatus', error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: renderizarGraficoViagensPorStatus
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Renderiza gráfico de rosca (Donut) Syncfusion EJ2 com dados de viagens
 *                   por status (Finalizadas, Em Andamento, Agendadas, Canceladas).
 *
 * 📥 ENTRADAS     : dados [Array<{status: string, total: number}>] - Array com 4 status
 *
 * 📤 SAÍDAS       : Gráfico AccumulationChart (Donut) renderizado em #chartViagensPorStatus
 *                   com labels externos, legenda inferior, innerRadius 40%, altura 350px
 *
 * 🔗 CHAMADA POR  : carregarViagensPorStatus() [linha ~415]
 *
 * 🔄 CHAMA        : chartViagensPorStatus.destroy() [se existir instância anterior],
 *                   new ej.charts.AccumulationChart({...}),
 *                   chart.appendTo('#chartViagensPorStatus'), formatarNumero() [tooltipRender]
 *
 * 📝 OBSERVAÇÕES  : • Tipo: AccumulationChart (Donut - innerRadius 40%)
 *                   • DataLabel: Outside position com enableSmartLabels
 *                   • Variável global: chartViagensPorStatus armazena instância p/ destroy posterior
 *                   • Legenda: position 'Bottom'
 *                   • Tooltip formatado com separador de milhar pt-BR
 ****************************************************************************************/
function renderizarGraficoViagensPorStatus(dados)
{
    try
    {
        // Destroi gráfico anterior se existir
        if (chartViagensPorStatus)
        {
            chartViagensPorStatus.destroy();
            chartViagensPorStatus = null;
        }

        chartViagensPorStatus = new ej.charts.AccumulationChart({
            series: [{
                dataSource: dados,
                xName: 'status',
                yName: 'total',
                innerRadius: '40%',
                dataLabel: {
                    visible: true,
                    position: 'Outside',
                    name: 'status',
                    font: { fontWeight: '600' }
                }
            }],
            enableSmartLabels: true,
            legendSettings: {
                visible: true,
                position: 'Bottom'
            },
            tooltip: {
                enable: true,
                format: '${point.x}: ${point.y} viagens',
                template: null
            },
            tooltipRender: function(args) {
                try {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error) {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            height: '350px'
        });

        chartViagensPorStatus.appendTo('#chartViagensPorStatus');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorStatus', error);
    }
}

// ========================================
// VIAGENS POR MOTORISTA
// ========================================

async function carregarViagensPorMotorista()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorMotorista?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorMotorista(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorMotorista', error);
    }
}

function renderizarGraficoViagensPorMotorista(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelIntersectAction: 'Rotate45'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'motorista',
                yName: 'totalViagens',
                type: 'Column',
                name: 'Viagens',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.ciano
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartViagensPorMotorista');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorMotorista', error);
    }
}

// ========================================
// VIAGENS POR VEÍCULO
// ========================================

async function carregarViagensPorVeiculo()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorVeiculo?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorVeiculo(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorVeiculo', error);
    }
}

function renderizarGraficoViagensPorVeiculo(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelIntersectAction: 'Rotate45'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'veiculo',
                yName: 'totalViagens',
                type: 'Column',
                name: 'Viagens',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.laranja
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartViagensPorVeiculo');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorVeiculo', error);
    }
}

// ========================================
// CUSTOS POR DIA
// ========================================

async function carregarCustosPorDia()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterCustosPorDia?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoCustosPorDia(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorDia', error);
    }
}

function renderizarGraficoCustosPorDia(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'DateTime',
                labelFormat: 'dd/MM',
                intervalType: 'Days',
                edgeLabelPlacement: 'Shift'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Custos (R$)',
                minimum: 0
            },
            series: [{
                dataSource: dados.map(d => ({
                    x: new Date(d.data),
                    y: (d.combustivel || 0) + (d.veiculo || 0) + (d.motorista || 0) + (d.operador || 0) + (d.lavador || 0)
                })),
                xName: 'x',
                yName: 'y',
                name: 'Custo Total',
                type: 'Area',
                opacity: 0.5,
                fill: CORES_FROTIX.azul,
                border: { width: 2, color: CORES_FROTIX.azul }
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = 'R$ ' + formatarValorMonetario(args.value);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = 'Custo Total<br/>R$ ' + formatarValorMonetario(args.point.y);
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartCustosPorDia');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorDia', error);
    }
}

// ========================================
// CUSTOS POR TIPO
// ========================================

async function carregarCustosPorTipo()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterCustosPorTipo?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoCustosPorTipo(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorTipo', error);
    }
}

function renderizarGraficoCustosPorTipo(dados)
{
    try
    {
        // Destroi gráfico anterior se existir
        if (chartCustosPorTipo)
        {
            chartCustosPorTipo.destroy();
            chartCustosPorTipo = null;
        }

        chartCustosPorTipo = new ej.charts.AccumulationChart({
            series: [{
                dataSource: dados,
                xName: 'tipo',
                yName: 'custo',
                dataLabel: {
                    visible: true,
                    position: 'Outside',
                    name: 'tipo',
                    font: { fontWeight: '600' }
                }
            }],
            enableSmartLabels: true,
            legendSettings: {
                visible: true,
                position: 'Bottom'
            },
            tooltip: {
                enable: true,
                format: '${point.x}: R$ ${point.y}',
                template: null
            },
            tooltipRender: function(args) {
                try {
                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
                } catch (error) {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            height: '350px'
        });

        chartCustosPorTipo.appendTo('#chartCustosPorTipo');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorTipo', error);
    }
}

// ========================================
// VIAGENS POR FINALIDADE
// ========================================

async function carregarViagensPorFinalidade()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorFinalidade?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorFinalidade(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorFinalidade', error);
    }
}

function renderizarGraficoViagensPorFinalidade(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelIntersectAction: 'Rotate45'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'finalidade',
                yName: 'total',
                type: 'Column',
                name: 'Viagens',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.verde
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '420px'
        });

        chart.appendTo('#chartViagensPorFinalidade');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorFinalidade', error);
    }
}

// ========================================
// KM POR VEÍCULO
// ========================================

async function carregarKmPorVeiculo()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterKmPorVeiculo?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoKmPorVeiculo(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarKmPorVeiculo', error);
    }
}

function renderizarGraficoKmPorVeiculo(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: 0,
                labelIntersectAction: 'Trim',
                maximumLabelWidth: 120
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quilometragem'
            },
            series: [{
                dataSource: dados,
                xName: 'veiculo',
                yName: 'kmTotal',
                type: 'Bar',
                name: 'KM',
                cornerRadius: { topRight: 10, bottomRight: 10 },
                fill: CORES_FROTIX.roxo
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0) + ' km';
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' km';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '420px'
        });

        chart.appendTo('#chartKmPorVeiculo');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoKmPorVeiculo', error);
    }
}

// ========================================
// VIAGENS POR REQUISITANTE
// ========================================

async function carregarViagensPorRequisitante()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 6
        });

        console.log('🔍 Carregando Top 6 Requisitantes...', {
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorRequisitante?${params}`);
        const result = await response.json();

        console.log('📊 Resposta API - Top 6 Requisitantes:', result);

        if (result.success && result.data && result.data.length > 0)
        {
            console.log('✅ Renderizando gráfico com', result.data.length, 'requisitantes');
            renderizarGraficoViagensPorRequisitante(result.data);

            // Atualiza linha com total Ctran se existir
            if (result.viagensCtran !== undefined)
            {
                $('#infoViagensCtranRequisitante').text(`Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`);
                $('#footerRequisitante').removeClass('d-none');
            }
            else
            {
                $('#footerRequisitante').addClass('d-none');
            }
        }
        else
        {
            console.warn('⚠️ Nenhum dado de requisitantes para exibir');
            document.getElementById('chartViagensPorRequisitante').innerHTML =
                '<div class="text-center py-5 text-muted">Nenhum dado disponível para o período selecionado</div>';
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorRequisitante', error);
    }
}

function renderizarGraficoViagensPorRequisitante(dados)
{
    try
    {
        console.log('🎨 Renderizando gráfico de requisitantes com dados:', dados);

        // Limpar gráfico anterior se existir
        const containerElement = document.getElementById('chartViagensPorRequisitante');
        if (containerElement && containerElement.ej2_instances && containerElement.ej2_instances.length > 0)
        {
            containerElement.ej2_instances[0].destroy();
        }

        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: 0,
                labelIntersectAction: 'Trim',
                maximumLabelWidth: 100
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'requisitante',
                yName: 'totalViagens',
                type: 'Bar',
                name: 'Viagens',
                cornerRadius: { topRight: 10, bottomRight: 10 },
                fill: CORES_FROTIX.rosa
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '280px',
            loaded: function ()
            {
                try
                {
                    console.log('✅ Gráfico de Requisitantes carregado com sucesso!');
                } catch (error)
                {
                    console.error('Erro no evento loaded:', error);
                }
            }
        });

        chart.appendTo('#chartViagensPorRequisitante');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorRequisitante', error);
    }
}

// ========================================
// VIAGENS POR SETOR
// ========================================

async function carregarViagensPorSetor()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 6
        });

        const response = await fetch(`/api/DashboardViagens/ObterViagensPorSetor?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoViagensPorSetor(result.data);

            // Atualiza linha com total Ctran se existir
            if (result.viagensCtran !== undefined)
            {
                $('#infoViagensCtranSetor').text(`Viagens Ctran: ${formatarNumero(result.viagensCtran, 0)}`);
                $('#footerSetor').removeClass('d-none');
            }
            else
            {
                $('#footerSetor').addClass('d-none');
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarViagensPorSetor', error);
    }
}

function renderizarGraficoViagensPorSetor(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: 0,
                labelIntersectAction: 'Trim',
                maximumLabelWidth: 100
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quantidade de Viagens'
            },
            series: [{
                dataSource: dados,
                xName: 'setor',
                yName: 'totalViagens',
                type: 'Bar',
                name: 'Viagens',
                cornerRadius: { topRight: 10, bottomRight: 10 },
                fill: CORES_FROTIX.amarelo
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' viagens';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '280px'
        });

        chart.appendTo('#chartViagensPorSetor');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoViagensPorSetor', error);
    }
}

// ========================================
// CUSTOS POR MOTORISTA
// ========================================

async function carregarCustosPorMotorista()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterCustosPorMotorista?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoCustosPorMotorista(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorMotorista', error);
    }
}

function renderizarGraficoCustosPorMotorista(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelIntersectAction: 'Rotate45'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Custo Total (R$)'
            },
            series: [{
                dataSource: dados,
                xName: 'motorista',
                yName: 'custoTotal',
                type: 'Column',
                name: 'Custo',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.vermelho
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = 'R$ ' + formatarValorMonetario(args.value);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartCustosPorMotorista');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorMotorista', error);
    }
}

// ========================================
// CUSTOS POR VEÍCULO
// ========================================

async function carregarCustosPorVeiculo()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterCustosPorVeiculo?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarGraficoCustosPorVeiculo(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustosPorVeiculo', error);
    }
}

function renderizarGraficoCustosPorVeiculo(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: -45,
                labelIntersectAction: 'Rotate45'
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Custo Total (R$)'
            },
            series: [{
                dataSource: dados,
                xName: 'veiculo',
                yName: 'custoTotal',
                type: 'Column',
                name: 'Custo',
                cornerRadius: { topLeft: 10, topRight: 10 },
                fill: CORES_FROTIX.azul
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = 'R$ ' + formatarValorMonetario(args.value);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': R$ ' + formatarValorMonetario(args.point.y);
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '350px'
        });

        chart.appendTo('#chartCustosPorVeiculo');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarGraficoCustosPorVeiculo', error);
    }
}

// ========================================
// TOP 10 VIAGENS MAIS CARAS
// ========================================

async function carregarTop10ViagensMaisCaras()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterTop10ViagensMaisCaras?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarTabelaTop10(result.data);
        } else
        {
            $('#tabelaTop10Body').html('<tr><td colspan="7" class="text-center">Nenhuma viagem encontrada</td></tr>');
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarTop10ViagensMaisCaras', error);
    }
}

// Armazena dados das viagens do TOP 10 para uso no modal
let dadosTop10Viagens = [];

function renderizarTabelaTop10(dados)
{
    try
    {
        // Armazena os dados para uso no modal
        dadosTop10Viagens = dados;
        
        let html = '';

        dados.forEach((viagem, index) =>
        {
            // Formatar número da ficha com divisão de milhares
            const noFichaFormatado = viagem.noFichaVistoria && viagem.noFichaVistoria !== 'N/A' 
                ? formatarNumero(parseInt(viagem.noFichaVistoria) || 0, 0)
                : 'N/A';
            
            html += `
                <tr data-viagem-index="${index}" onclick="abrirModalDetalhesViagem(${index})" title="Clique para ver detalhes">
                    <td class="text-center">${index + 1}</td>
                    <td class="text-center">${noFichaFormatado}</td>
                    <td>${viagem.dataInicial}</td>
                    <td>${viagem.dataFinal}</td>
                    <td>${viagem.motorista}</td>
                    <td>${viagem.veiculo}</td>
                    <td class="text-end text-success fw-bold">R$ ${formatarValorMonetario(viagem.custoTotal)}</td>
                </tr>
            `;
        });

        $('#tabelaTop10Body').html(html);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarTabelaTop10', error);
    }
}

/**
 * Abre o modal com detalhes da viagem
 * 
 * 🎯 OBJETIVO     : Exibe modal Bootstrap com detalhamento completo de uma viagem do TOP 10
 *                   incluindo breakdown de custos, botão de edição e alerta de KM zero.
 * 
 * 📥 ENTRADAS     : index [number] - Índice da viagem no array dadosTop10Viagens (0-9)
 * 
 * 📤 SAÍDAS       : Modal #modalDetalhesViagem aberto com:
 *                   • Dados da viagem (nº ficha, status, datas, motorista, veículo, km, duração)
 *                   • Breakdown de custos (5 tipos + total)
 *                   • Alerta amarelo se kmRodado = 0 (com botão ajustar)
 *                   • Botão "Editar Viagem" vinculado a /Viagens/Upsert/{viagemId}
 * 
 * 🔗 CHAMADA POR  : onclick nas linhas <tr> da tabela TOP 10 [renderizarTabelaTop10, linha ~1025]
 * 
 * 🔄 CHAMA        : jQuery $('#modal*'), formatarNumero(), formatarDuracao(),
 *                   formatarValorMonetario(), new bootstrap.Modal()
 * 
 * 📝 OBSERVAÇÕES  : • Armazena viagemAtualId global para uso no botão editar
 *                   • Mostra/esconde #alertaKmZero conforme kmRodado
 *                   • Status renderizado como badge Bootstrap (bg-success/warning/danger)
 * 
 * @param {number} index - Índice da viagem no array dadosTop10Viagens
 */
function abrirModalDetalhesViagem(index)
{
    try
    {
        const viagem = dadosTop10Viagens[index];
        if (!viagem)
        {
            console.error('Viagem não encontrada no índice:', index);
            return;
        }

        // Armazena o ID da viagem atual para uso no botão de edição
        viagemAtualId = viagem.viagemId;

        // Preencher dados da viagem
        $('#modalNoFicha').text(viagem.noFichaVistoria || 'N/A');
        $('#modalStatus').html(viagem.status 
            ? `<span class="badge bg-success">${viagem.status}</span>` 
            : '-');
        $('#modalDataInicial').text(viagem.dataInicial || '-');
        $('#modalDataFinal').text(viagem.dataFinal || '-');
        $('#modalMotorista').text(viagem.motorista || '-');
        $('#modalVeiculo').text(viagem.veiculo || '-');
        $('#modalKmRodado').text(viagem.kmRodado 
            ? formatarNumero(viagem.kmRodado, 0) + ' km' 
            : '-');
        $('#modalDuracao').text(viagem.duracao || viagem.minutos 
            ? formatarDuracao(viagem.minutos || 0) 
            : '-');
        $('#modalFinalidade').text(viagem.finalidade || '-');

        // Mostrar/esconder alerta de KM Rodado zero
        const alertaKmZero = document.getElementById('alertaKmZero');
        if (alertaKmZero)
        {
            if (!viagem.kmRodado || viagem.kmRodado <= 0)
            {
                alertaKmZero.classList.remove('d-none');
            }
            else
            {
                alertaKmZero.classList.add('d-none');
            }
        }

        // Preencher custos
        $('#modalCustoCombustivel').text('R$ ' + formatarValorMonetario(viagem.custoCombustivel || 0));
        $('#modalCustoVeiculo').text('R$ ' + formatarValorMonetario(viagem.custoVeiculo || 0));
        $('#modalCustoMotorista').text('R$ ' + formatarValorMonetario(viagem.custoMotorista || 0));
        $('#modalCustoOperador').text('R$ ' + formatarValorMonetario(viagem.custoOperador || 0));
        $('#modalCustoLavador').text('R$ ' + formatarValorMonetario(viagem.custoLavador || 0));
        $('#modalCustoTotal').text('R$ ' + formatarValorMonetario(viagem.custoTotal || 0));

        // Abrir modal
        const modal = new bootstrap.Modal(document.getElementById('modalDetalhesViagem'));
        modal.show();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'abrirModalDetalhesViagem', error);
    }
}

/**
 * Formata minutos em horas e minutos (ex: 125 => "2h 05min")
 * 
 * 🎯 OBJETIVO     : Converte duração em minutos para formato legível "Xh YYmin".
 * 
 * 📥 ENTRADAS     : minutos [number] - Total de minutos (ex: 125, 45, 120)
 * 
 * 📤 SAÍDAS       : [string] Duração formatada:
 *                   • "2h 05min" (125 min)
 *                   • "45min" (45 min, sem horas)
 *                   • "2h" (120 min, sem minutos resto)
 *                   • "-" (se minutos <= 0 ou inválido)
 * 
 * 🔗 CHAMADA POR  : abrirModalDetalhesViagem() [linha ~1050]
 * 
 * 🔄 CHAMA        : Math.floor(), String.padStart()
 * 
 * 📝 OBSERVAÇÕES  : • Minutos resto sempre com 2 dígitos (padStart)
 *                   • Try-catch retorna "-" em caso de erro
 * 
 * @param {number} minutos - Total de minutos
 * @returns {string} Duração formatada
 */
function formatarDuracao(minutos)
{
    try
    {
        if (!minutos || minutos <= 0) return '-';
        
        const horas = Math.floor(minutos / 60);
        const mins = minutos % 60;
        
        if (horas === 0) return mins + 'min';
        if (mins === 0) return horas + 'h';
        return horas + 'h ' + String(mins).padStart(2, '0') + 'min';
    } catch (error)
    {
        return '-';
    }
}

// ========================================
// HEATMAP DE VIAGENS (Dia x Hora)
// ========================================

async function carregarHeatmapViagens()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterHeatmapViagens?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarHeatmapViagens(result.data, result.maxValor);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarHeatmapViagens', error);
    }
}

function renderizarHeatmapViagens(dados, maxValor)
{
    try
    {
        const tbody = document.getElementById('heatmapBody');
        if (!tbody) return;

        tbody.innerHTML = '';

        // Função para obter cor baseada na intensidade
        function obterCorHeatmap(valor, max)
        {
            if (max === 0 || valor === 0) return '#f5f5f5';

            const intensidade = valor / max;

            if (intensidade <= 0.2) return '#e8f5e9';
            if (intensidade <= 0.4) return '#c8e6c9';
            if (intensidade <= 0.6) return '#81c784';
            if (intensidade <= 0.8) return '#4caf50';
            return '#2e7d32';
        }

        // Criar linhas para cada dia
        dados.forEach(dia =>
        {
            const tr = document.createElement('tr');

            // Célula do dia da semana
            const tdDia = document.createElement('td');
            tdDia.className = 'fw-bold text-center';
            tdDia.textContent = dia.diaSemana;
            tr.appendChild(tdDia);

            // Células das horas (0-23)
            dia.horas.forEach((quantidade, hora) =>
            {
                const td = document.createElement('td');
                td.className = 'text-center';
                td.style.backgroundColor = obterCorHeatmap(quantidade, maxValor);
                td.style.color = quantidade > (maxValor * 0.6) ? 'white' : '#333';
                td.style.fontWeight = quantidade > 0 ? '600' : 'normal';
                td.style.cursor = 'pointer';
                td.style.transition = 'transform 0.2s';
                td.textContent = quantidade > 0 ? quantidade : '';
                td.title = `${dia.diaSemana} ${hora.toString().padStart(2, '0')}:00 - ${quantidade} viagem(s)`;

                // Efeito hover
                td.addEventListener('mouseenter', function ()
                {
                    this.style.transform = 'scale(1.1)';
                    this.style.zIndex = '10';
                });
                td.addEventListener('mouseleave', function ()
                {
                    this.style.transform = 'scale(1)';
                    this.style.zIndex = '1';
                });

                tr.appendChild(td);
            });

            tbody.appendChild(tr);
        });

    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarHeatmapViagens', error);
    }
}

// ========================================
// TOP 10 VEÍCULOS POR KM RODADO
// ========================================

async function carregarTop10VeiculosKm()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString()
        });

        const response = await fetch(`/api/DashboardViagens/ObterTop10VeiculosPorKm?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarTop10VeiculosKm(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarTop10VeiculosKm', error);
    }
}

function renderizarTop10VeiculosKm(dados)
{
    try
    {
        // Preparar dados com label combinado (placa + modelo)
        const dadosFormatados = dados.map(d => ({
            veiculo: d.placa,
            totalKm: d.totalKm,
            tooltip: `${d.placa} - ${d.marcaModelo}\n${d.totalViagens} viagens | Média: ${d.mediaKmPorViagem} km/viagem`
        }));

        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: 0,
                labelIntersectAction: 'Trim',
                maximumLabelWidth: 80
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Quilometragem Total'
            },
            series: [{
                dataSource: dadosFormatados,
                xName: 'veiculo',
                yName: 'totalKm',
                type: 'Bar',
                name: 'KM Rodado',
                cornerRadius: { topRight: 8, bottomRight: 8 },
                fill: CORES_FROTIX.verde
            }],
            tooltip: {
                enable: true
            },
            axisLabelRender: function (args)
            {
                try
                {
                    if (args.axis.name === 'primaryYAxis')
                    {
                        args.text = formatarNumero(args.value, 0) + ' km';
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    args.text = args.point.x + ': ' + formatarNumero(args.point.y, 0) + ' km';
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: { visible: false },
            height: '420px'
        });

        chart.appendTo('#chartTop10VeiculosKm');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarTop10VeiculosKm', error);
    }
}

// ========================================
// CUSTO MÉDIO POR FINALIDADE
// ========================================

async function carregarCustoMedioPorFinalidade()
{
    try
    {
        const params = new URLSearchParams({
            dataInicio: periodoAtual.dataInicio.toISOString(),
            dataFim: periodoAtual.dataFim.toISOString(),
            top: 10
        });

        const response = await fetch(`/api/DashboardViagens/ObterCustoMedioPorFinalidade?${params}`);
        const result = await response.json();

        if (result.success && result.data.length > 0)
        {
            renderizarCustoMedioPorFinalidade(result.data);
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarCustoMedioPorFinalidade', error);
    }
}

function renderizarCustoMedioPorFinalidade(dados)
{
    try
    {
        const chart = new ej.charts.Chart({
            primaryXAxis: {
                valueType: 'Category',
                labelRotation: 0,
                labelIntersectAction: 'Trim',
                maximumLabelWidth: 120
            },
            primaryYAxis: {
                labelFormat: '{value}',
                title: 'Custo Total (R$)'
            },
            axes: [{
                name: 'yAxisMedio',
                opposedPosition: true,
                labelFormat: '{value}',
                title: 'Custo Médio (R$)'
            }],
            series: [
                {
                    dataSource: dados,
                    xName: 'finalidade',
                    yName: 'custoTotal',
                    type: 'Bar',
                    name: 'Custo Total',
                    cornerRadius: { topRight: 8, bottomRight: 8 },
                    fill: CORES_FROTIX.vermelho,
                    opacity: 0.8,
                    tooltipMappingName: 'finalidade'
                },
                {
                    dataSource: dados,
                    xName: 'finalidade',
                    yName: 'custoMedio',
                    type: 'Line',
                    name: 'Custo Médio',
                    yAxisName: 'yAxisMedio',
                    marker: {
                        visible: true,
                        width: 10,
                        height: 10,
                        fill: CORES_FROTIX.azul
                    },
                    fill: CORES_FROTIX.azul,
                    width: 3,
                    tooltipMappingName: 'finalidade'
                }
            ],
            tooltip: {
                enable: true,
                shared: false
            },
            axisLabelRender: function (args)
            {
                try
                {
                    // Formatar labels dos eixos Y (primário e secundário)
                    if (args.axis.name === 'primaryYAxis' || args.axis.name === 'yAxisMedio')
                    {
                        args.text = 'R$ ' + formatarNumero(args.value, 0);
                    }
                } catch (error)
                {
                    console.error('Erro ao formatar label:', error);
                }
            },
            tooltipRender: function (args)
            {
                try
                {
                    const nomeSerie = args.series.name || '';
                    const valor = Number(args.point.y) || 0;
                    const categoria = args.point.x || '';
                    args.text = '<b>' + categoria + '</b><br/>' + nomeSerie + ': R$ ' + formatarNumero(valor, 2);
                } catch (error)
                {
                    console.error('Erro ao formatar tooltip:', error);
                }
            },
            legendSettings: {
                visible: true,
                position: 'Top'
            },
            height: '380px'
        });

        chart.appendTo('#chartCustoMedioPorFinalidade');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'renderizarCustoMedioPorFinalidade', error);
    }
}

// ========================================
// FILTROS
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: aplicarFiltroPeriodo
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Aplica filtro de período rápido (7/15/30/60/90/180/365 dias) a partir
 *                   de hoje, atualiza inputs HTML5 de data e recarrega dashboard completo.
 *
 * 📥 ENTRADAS     : dias [number] - Número de dias retroativos a partir de hoje
 *                   (ex: 30 = últimos 30 dias)
 *
 * 📤 SAÍDAS       : periodoAtual atualizado, inputs #dataInicio e #dataFim preenchidos,
 *                   dashboard recarregado com 16 endpoints
 *
 * 🔗 CHAMADA POR  : Event listeners dos botões .btn-period [data-dias] (7/15/30/60/90/180/365)
 *                   [linha ~3653]
 *
 * 🔄 CHAMA        : new Date(), periodoAtual.dataFim.setDate(),
 *                   document.getElementById(), formatarDataParaInput(),
 *                   carregarDadosDashboard()
 *
 * 📝 OBSERVAÇÕES  : • dataFim: hoje 23:59:59
 *                   • dataInicio: hoje - N dias, 00:00:00
 *                   • Atualiza inputs HTML5 para visualização do usuário
 *                   • Try-catch global com Alerta.TratamentoErroComLinha
 ****************************************************************************************/
function aplicarFiltroPeriodo(dias)
{
    try
    {
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - dias);

        // Atualiza campos HTML5
        const dataInicio = document.getElementById('dataInicio');
        const dataFim = document.getElementById('dataFim');
        if (dataInicio && dataFim)
        {
            dataInicio.value = formatarDataParaInput(periodoAtual.dataInicio);
            dataFim.value = formatarDataParaInput(periodoAtual.dataFim);
        }

        carregarDadosDashboard();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'aplicarFiltroPeriodo', error);
    }
}

function aplicarFiltroPersonalizado()
{
    try
    {
        const dataInicioInput = document.getElementById('dataInicio');
        const dataFimInput = document.getElementById('dataFim');

        if (!dataInicioInput?.value || !dataFimInput?.value)
        {
            AppToast.show('Amarelo', 'Preencha as datas De e Até para filtrar.', 3000);
            return;
        }

        const dataInicio = new Date(dataInicioInput.value + 'T00:00:00');
        const dataFim = new Date(dataFimInput.value + 'T23:59:59');

        if (dataInicio > dataFim)
        {
            AppToast.show('Vermelho', 'A data inicial não pode ser maior que a data final.', 3000);
            return;
        }

        periodoAtual.dataInicio = dataInicio;
        periodoAtual.dataFim = dataFim;

        // Remove classe active de todos os botões de período
        $('.btn-period').removeClass('active');

        carregarDadosDashboard();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'aplicarFiltroPersonalizado', error);
    }
}

function atualizarDashboard()
{
    try
    {
        // Atualiza variáveis de período antes de recarregar
        const dataInicio = document.getElementById('dataInicio');
        const dataFim = document.getElementById('dataFim');

        if (dataInicio && dataFim && dataInicio.value && dataFim.value)
        {
            periodoAtual.dataInicio = new Date(dataInicio.value + 'T00:00:00');
            periodoAtual.dataFim = new Date(dataFim.value + 'T23:59:59');
        }

        carregarDadosDashboard();
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'atualizarDashboard', error);
    }
}

// ========================================
// LOADING
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: mostrarLoadingGeral
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Exibe loading overlay reutilizável (#loadingInicialDashboard) com
 *                   mensagem personalizável durante operações AJAX.
 *
 * 📥 ENTRADAS     : mensagem [string] - Texto personalizado para .ftx-loading-text
 *                   (opcional, default mantém mensagem anterior)
 *
 * 📤 SAÍDAS       : Overlay de loading visível com mensagem atualizada
 *
 * 🔗 CHAMADA POR  : carregarDadosDashboard() [linha ~195], gravarViagemDashboard()
 *
 * 🔄 CHAMA        : document.getElementById(), querySelector('.ftx-loading-text'),
 *                   element.textContent, classList.remove('d-none'), element.style
 *
 * 📝 OBSERVAÇÕES  : • Reutiliza mesmo elemento do loading inicial
 *                   • Remove classe d-none + força display flex + opacity 1
 *                   • Console.error se elemento não existir
 ****************************************************************************************/
function mostrarLoadingGeral(mensagem)
{
    try
    {
        const elemento = document.getElementById('loadingInicialDashboard');
        if (!elemento)
        {
            console.error('❌ Elemento #loadingInicialDashboard não existe!');
            return;
        }

        // Atualiza mensagem se fornecida (padrão FrotiX usa .ftx-loading-text)
        const textoLoading = elemento.querySelector('.ftx-loading-text');
        if (textoLoading && mensagem)
        {
            textoLoading.textContent = mensagem;
        }

        // Remove classe d-none e mostra
        elemento.classList.remove('d-none');
        elemento.style.display = 'flex';
        elemento.style.opacity = '1';
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'mostrarLoadingGeral', error);
    }
}

/****************************************************************************************
 * ⚡ FUNÇÃO: esconderLoadingGeral
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Esconde loading overlay com delay e fade out suave (500ms + 300ms),
 *                   restaura mensagem padrão para próximo uso.
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Overlay oculto (opacity 0 → d-none + display none após 800ms total)
 *
 * 🔗 CHAMADA POR  : carregarDadosDashboard() [linha ~227]
 *
 * 🔄 CHAMA        : setTimeout() [2x aninhados], document.getElementById(),
 *                   querySelector('.ftx-loading-text'), element.style, classList.add()
 *
 * 📝 OBSERVAÇÕES  : • Delay 500ms antes de iniciar fade out
 *                   • Fade out 300ms (transição opacity)
 *                   • Restaura texto padrão: "Carregando Dashboard de Viagens"
 *                   • Total: 800ms até ocultação completa
 ****************************************************************************************/
function esconderLoadingGeral()
{
    try
    {
        // Pequeno delay para suavizar a transição
        setTimeout(() =>
        {
            const elemento = document.getElementById('loadingInicialDashboard');
            if (elemento)
            {
                elemento.style.opacity = '0';
                setTimeout(() => {
                    elemento.classList.add('d-none');
                    elemento.style.display = 'none';

                    // Restaura mensagem padrão (padrão FrotiX usa .ftx-loading-text)
                    const textoLoading = elemento.querySelector('.ftx-loading-text');
                    if (textoLoading)
                    {
                        textoLoading.textContent = 'Carregando Dashboard de Viagens';
                    }
                }, 300);
            }
        }, 500);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'esconderLoadingGeral', error);
    }
}

// ========================================
// CÁLCULO DE VARIAÇÕES
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: atualizarVariacao
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Calcula variação percentual entre valor atual e anterior, atualiza
 *                   badge HTML com texto formatado e aplica classe CSS (verde/vermelho/neutro)
 *                   conforme resultado.
 *
 * 📥 ENTRADAS     : elementoId [string] - ID do elemento HTML badge (ex: "variacaoCusto")
 *                   valorAtual [number] - Valor do período atual
 *                   valorAnterior [number] - Valor do período anterior para comparação
 *
 * 📤 SAÍDAS       : Badge HTML atualizado:
 *                   • Texto: "+15.50% vs período anterior" (se crescimento)
 *                   • Texto: "-8.30% vs período anterior" (se queda)
 *                   • Texto: "-" (se valorAnterior = 0 ou null)
 *                   • Classe: .variacao-positiva (verde) | .variacao-negativa (vermelho) |
 *                             .variacao-neutra (cinza)
 *
 * 🔗 CHAMADA POR  : carregarEstatisticasGerais() [14 vezes - uma para cada badge de variação]
 *
 * 🔄 CHAMA        : jQuery $('#elementoId'), .text(), .removeClass(), .addClass(),
 *                   Number.toFixed(2)
 *
 * 📝 OBSERVAÇÕES  : • Fórmula: ((atual - anterior) / anterior) * 100
 *                   • Variação > 0: verde (positiva)
 *                   • Variação < 0: vermelho (negativa)
 *                   • Variação = 0: cinza (neutra)
 *                   • Se valorAnterior = 0, exibe "-" (neutro)
 ****************************************************************************************/
function atualizarVariacao(elementoId, valorAtual, valorAnterior)
{
    try
    {
        const elemento = $(`#${elementoId}`);

        if (!valorAnterior || valorAnterior === 0)
        {
            elemento.text('-').removeClass('variacao-positiva variacao-negativa').addClass('variacao-neutra');
            return;
        }

        const variacao = ((valorAtual - valorAnterior) / valorAnterior) * 100;
        const sinal = variacao >= 0 ? '+' : '';
        const texto = `${sinal}${variacao.toFixed(2)}% vs período anterior`;

        elemento.text(texto);

        if (variacao > 0)
        {
            elemento.removeClass('variacao-negativa variacao-neutra').addClass('variacao-positiva');
        }
        else if (variacao < 0)
        {
            elemento.removeClass('variacao-positiva variacao-neutra').addClass('variacao-negativa');
        }
        else
        {
            elemento.removeClass('variacao-positiva variacao-negativa').addClass('variacao-neutra');
        }
    } catch (error)
    {
        console.error('Erro ao atualizar variação:', error);
    }
}

// ========================================
// EXPORTAÇÃO PARA PDF
// ========================================

/**
 * Exporta o Dashboard para PDF e exibe em Modal com PDFViewer
 */
/****************************************************************************************
 * ⚡ FUNÇÃO: exportarParaPDF
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Captura TODOS os gráficos Syncfusion + 9 cards visuais como imagens PNG,
 *                   envia via POST para /Viagens/ExportarParaPDF, recebe PDF gerado
 *                   com QuestPDF e exibe em modal com PDFViewer Syncfusion.
 *
 * 📥 ENTRADAS     : Nenhuma (usa periodoAtual global para datas)
 *
 * 📤 SAÍDAS       : • Modal #modalPDFViewer aberto com PDF renderizado
 *                   • pdfAtualBlob global preenchido (para download posterior)
 *                   • Console logs detalhados de progresso e tamanhos
 *                   • Toast Verde: "PDF gerado com sucesso!" ou Vermelho em erro
 *
 * 🔗 CHAMADA POR  : Botão #btnExportarPDF [evento click, linha ~3659]
 *
 * 🔄 CHAMA        : capturarGraficos() → 6 gráficos SVG → PNG,
 *                   capturarCards() → 9 cards PNG (html2canvas),
 *                   fetch('/Viagens/ExportarParaPDF', {POST, JSON}),
 *                   response.blob(), FileReader.readAsDataURL(),
 *                   new bootstrap.Modal(), carregarPDFNoViewer(),
 *                   AppToast.show(), Alerta.TratamentoErroComLinha
 *
 * 📝 OBSERVAÇÕES  : • Payload pode chegar a 20-30MB (6 gráficos PNG + 9 cards PNG)
 *                   • Verifica tamanho antes de enviar (limite ASP.NET: 30MB)
 *                   • SVG → PNG obrigatório (Syncfusion.Pdf backend só aceita PNG/JPG)
 *                   • Console logs extensivos para diagnóstico
 *                   • Captura: status, motoristas, veículos, finalidades, requisitantes, setores
 *                   • Cards: 3x3 grid (custo total/viagens/médio, km, status)
 *                   • Função async com try-catch robusto
 ****************************************************************************************/
async function exportarParaPDF()
{
    try
    {
        console.log('🚀 ===== INICIANDO EXPORTAÇÃO PARA PDF =====');

        // Valida período
        if (!periodoAtual.dataInicio || !periodoAtual.dataFim)
        {
            console.error('❌ Período inválido!');
            AppToast.show('Amarelo', 'Por favor, selecione um período válido.', 3000);
            return;
        }
        console.log('✅ Período válido:', periodoAtual);

        // Toast de aguarde
        AppToast.show('Amarelo', 'Capturando gráficos, cards e gerando PDF, aguarde...', 8000);

        // 📊 Captura todos os gráficos como Base64 PNG
        console.log('📊 Iniciando captura de gráficos...');
        const graficos = await capturarGraficos();
        console.log('📊 Gráficos capturados:', Object.keys(graficos).length);

        // 🎨 Captura todos os cards visuais como Base64 PNG
        console.log('🎨 Iniciando captura de cards...');
        const cards = await capturarCards();
        console.log('🎨 Cards capturados:', Object.keys(cards).filter(k => cards[k]).length);

        // Formata datas
        const dataInicio = periodoAtual.dataInicio.toISOString();
        const dataFim = periodoAtual.dataFim.toISOString();
        console.log('📅 Datas formatadas:', { dataInicio, dataFim });

        // 🔍 DIAGNÓSTICO: Calcular tamanho do payload
        const payload = {
            dataInicio: dataInicio,
            dataFim: dataFim,
            graficos: graficos,
            cards: cards
        };
        const payloadJSON = JSON.stringify(payload);
        const tamanhoMB = (payloadJSON.length / 1024 / 1024).toFixed(2);
        console.log('📦 Tamanho total do payload:', tamanhoMB, 'MB');
        console.log('📦 Tamanho por componente:');
        console.log('   📊 Gráficos:');
        for (const [key, base64] of Object.entries(graficos))
        {
            const tamanhoKB = (base64.length / 1024).toFixed(1);
            console.log(`      - ${key}: ${tamanhoKB} KB`);
        }
        console.log('   🎨 Cards:');
        for (const [key, base64] of Object.entries(cards))
        {
            if (base64)
            {
                const tamanhoKB = (base64.length / 1024).toFixed(1);
                console.log(`      - ${key}: ${tamanhoKB} KB`);
            }
        }

        // Verifica se payload está muito grande (> 30MB)
        if (parseFloat(tamanhoMB) > 30)
        {
            console.error('❌ PAYLOAD MUITO GRANDE! ASP.NET Core tem limite de 30MB por padrão.');
            AppToast.show('Vermelho', 'Payload muito grande. Contate o administrador.', 5000);
            return;
        }

        // Envia via POST
        console.log('📤 Enviando POST para /Viagens/ExportarParaPDF...');
        const response = await fetch('/Viagens/ExportarParaPDF', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                dataInicio: dataInicio,
                dataFim: dataFim,
                graficos: graficos,
                cards: cards
            })
        });

        console.log('📥 Resposta recebida:', response);
        console.log('   Status:', response.status, response.statusText);

        if (!response.ok)
        {
            const errorText = await response.text();
            console.error('❌ Erro na resposta:', errorText);
            throw new Error(`Erro ao gerar PDF: ${errorText}`);
        }

        // Converte resposta para Blob
        console.log('🔄 Convertendo resposta para Blob...');
        pdfAtualBlob = await response.blob();
        console.log('✅ Blob criado:', pdfAtualBlob.size, 'bytes');

        // Converte Blob para Base64
        console.log('🔄 Convertendo Blob para Base64...');
        const reader = new FileReader();
        reader.onloadend = function ()
        {
            console.log('✅ Base64 criado:', reader.result.substring(0, 100) + '...');
            const base64PDF = reader.result;

            // Abre o modal
            console.log('🖥️ Abrindo modal...');
            const modal = new bootstrap.Modal(document.getElementById('modalPDFViewer'));
            modal.show();

            // Aguarda o modal abrir completamente antes de carregar o PDF
            $('#modalPDFViewer').one('shown.bs.modal', function ()
            {
                console.log('✅ Modal aberto, carregando PDF no viewer...');
                carregarPDFNoViewer(base64PDF);
            });

            // Toast de sucesso
            AppToast.show('Verde', 'PDF gerado com sucesso!', 3000);
            console.log('🎉 ===== EXPORTAÇÃO CONCLUÍDA COM SUCESSO =====');
        };

        reader.onerror = function (error)
        {
            console.error('❌ Erro ao ler Blob:', error);
        };

        reader.readAsDataURL(pdfAtualBlob);
    } catch (error)
    {
        console.error('❌ ===== ERRO NA EXPORTAÇÃO =====');
        console.error('Erro:', error);
        console.error('Stack:', error.stack);
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'exportarParaPDF', error);
    }
}

/**
 * Carrega o PDF no PDFViewer Syncfusion
 */
function carregarPDFNoViewer(base64PDF)
{
    try
    {
        // Se já existe uma instância, destroi
        if (pdfViewerInstance)
        {
            pdfViewerInstance.destroy();
        }

        // Cria nova instância do PDFViewer
        pdfViewerInstance = new ej.pdfviewer.PdfViewer({
            documentPath: base64PDF,
            serviceUrl: 'https://ej2services.syncfusion.com/production/web-services/api/pdfviewer',
            enableToolbar: true,
            enableNavigationToolbar: true,
            enableThumbnail: true,
            zoomMode: 'FitToWidth',
            locale: 'pt-BR',
            documentLoad: function ()
            {
                console.log('✅ PDF carregado no viewer');

                // Ajusta zoom para FitToWidth
                setTimeout(() =>
                {
                    if (pdfViewerInstance)
                    {
                        pdfViewerInstance.magnification.fitToWidth();
                    }
                }, 500);
            }
        });

        // Renderiza o viewer no container
        pdfViewerInstance.appendTo('#pdfViewerContainer');
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarPDFNoViewer', error);
    }
}

/**
 * Baixa o PDF quando o usuário clicar no botão Baixar
 */
function baixarPDF()
{
    try
    {
        if (!pdfAtualBlob)
        {
            AppToast.show('Amarelo', 'Nenhum PDF disponível para download.', 3000);
            return;
        }

        const url = window.URL.createObjectURL(pdfAtualBlob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `Dashboard_Viagens_${periodoAtual.dataInicio.toLocaleDateString('pt-BR').replace(/\//g, '-')}_a_${periodoAtual.dataFim.toLocaleDateString('pt-BR').replace(/\//g, '-')}.pdf`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);

        AppToast.show('Verde', 'PDF baixado com sucesso!', 3000);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'baixarPDF', error);
    }
}

/**
 * Captura todos os gráficos como Base64
 */
/****************************************************************************************
 * ⚡ FUNÇÃO: capturarGraficos
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Captura 6 gráficos Syncfusion como Base64 SVG, depois converte cada
 *                   um para PNG (backend Syncfusion.Pdf exige PNG). Retorna dicionário
 *                   com chaves: status, motoristas, veiculos, finalidades, requisitantes,
 *                   setores.
 *
 * 📥 ENTRADAS     : Nenhuma (acessa instâncias globais chartViagensPorStatus e elementos DOM)
 *
 * 📤 SAÍDAS       : [Promise<Object>] Dicionário:
 *                   {
 *                     status: "data:image/png;base64,...",
 *                     motoristas: "data:image/png;base64,...",
 *                     veiculos: "data:image/png;base64,...",
 *                     finalidades: "data:image/png;base64,...",
 *                     requisitantes: "data:image/png;base64,...",
 *                     setores: "data:image/png;base64,..."
 *                   }
 *                   Strings vazias "" se captura falhar
 *
 * 🔗 CHAMADA POR  : exportarParaPDF() [linha ~2548]
 *
 * 🔄 CHAMA        : exportarGraficoSyncfusion(chart, nome) → SVG Base64 (6x),
 *                   converterSvgParaPng(svgBase64) → PNG Base64 (6x),
 *                   document.querySelector('#chart*'),
 *                   element.ej2_instances[0]
 *
 * 📝 OBSERVAÇÕES  : • Console logs detalhados para CADA gráfico
 *                   • Trata falhas individuais (gráfico = "" se erro)
 *                   • chartViagensPorStatus: variável global
 *                   • Outros gráficos: via DOM querySelector + ej2_instances[0]
 *                   • Conversão SVG→PNG obrigatória (Syncfusion.Pdf não suporta SVG)
 *                   • Async com await para conversões síncronas
 ****************************************************************************************/
async function capturarGraficos()
{
    try
    {
        console.log('🎯 INICIANDO CAPTURA DE GRÁFICOS...');

        const graficos = {};

        // Captura gráfico de Status (Pizza)
        console.log('🔍 Verificando gráfico de Status...');
        console.log('chartViagensPorStatus:', chartViagensPorStatus);
        if (chartViagensPorStatus)
        {
            console.log('✅ chartViagensPorStatus existe, capturando...');
            graficos.status = await exportarGraficoSyncfusion(chartViagensPorStatus, 'status');
            console.log('📊 Status capturado:', graficos.status ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorStatus não existe!');
        }

        // Captura gráfico de Motoristas
        console.log('🔍 Verificando gráfico de Motoristas...');
        const chartMotoristas = document.querySelector('#chartViagensPorMotorista');
        console.log('Elemento #chartViagensPorMotorista:', chartMotoristas);
        if (chartMotoristas && chartMotoristas.ej2_instances && chartMotoristas.ej2_instances[0])
        {
            console.log('✅ Motoristas existe, capturando...');
            graficos.motoristas = await exportarGraficoSyncfusion(chartMotoristas.ej2_instances[0], 'motoristas');
            console.log('📊 Motoristas capturado:', graficos.motoristas ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorMotorista não encontrado ou sem instância!');
        }

        // Captura gráfico de Veículos
        console.log('🔍 Verificando gráfico de Veículos...');
        const chartVeiculos = document.querySelector('#chartViagensPorVeiculo');
        console.log('Elemento #chartViagensPorVeiculo:', chartVeiculos);
        if (chartVeiculos && chartVeiculos.ej2_instances && chartVeiculos.ej2_instances[0])
        {
            console.log('✅ Veículos existe, capturando...');
            graficos.veiculos = await exportarGraficoSyncfusion(chartVeiculos.ej2_instances[0], 'veiculos');
            console.log('📊 Veículos capturado:', graficos.veiculos ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorVeiculo não encontrado ou sem instância!');
        }

        // Captura gráfico de Finalidades
        console.log('🔍 Verificando gráfico de Finalidades...');
        const chartFinalidades = document.querySelector('#chartViagensPorFinalidade');
        console.log('Elemento #chartViagensPorFinalidade:', chartFinalidades);
        if (chartFinalidades && chartFinalidades.ej2_instances && chartFinalidades.ej2_instances[0])
        {
            console.log('✅ Finalidades existe, capturando...');
            graficos.finalidades = await exportarGraficoSyncfusion(chartFinalidades.ej2_instances[0], 'finalidades');
            console.log('📊 Finalidades capturado:', graficos.finalidades ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorFinalidade não encontrado ou sem instância!');
        }

        // Captura gráfico de Requisitantes
        console.log('🔍 Verificando gráfico de Requisitantes...');
        const chartRequisitantes = document.querySelector('#chartViagensPorRequisitante');
        console.log('Elemento #chartViagensPorRequisitante:', chartRequisitantes);
        if (chartRequisitantes && chartRequisitantes.ej2_instances && chartRequisitantes.ej2_instances[0])
        {
            console.log('✅ Requisitantes existe, capturando...');
            graficos.requisitantes = await exportarGraficoSyncfusion(chartRequisitantes.ej2_instances[0], 'requisitantes');
            console.log('📊 Requisitantes capturado:', graficos.requisitantes ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorRequisitante não encontrado ou sem instância!');
        }

        // Captura gráfico de Setores
        console.log('🔍 Verificando gráfico de Setores...');
        const chartSetores = document.querySelector('#chartViagensPorSetor');
        console.log('Elemento #chartViagensPorSetor:', chartSetores);
        if (chartSetores && chartSetores.ej2_instances && chartSetores.ej2_instances[0])
        {
            console.log('✅ Setores existe, capturando...');
            graficos.setores = await exportarGraficoSyncfusion(chartSetores.ej2_instances[0], 'setores');
            console.log('📊 Setores capturado:', graficos.setores ? 'SIM' : 'NÃO');
        }
        else
        {
            console.warn('⚠️ chartViagensPorSetor não encontrado ou sem instância!');
        }

        console.log('🎯 CAPTURA FINALIZADA!');
        console.log('📊 Total de gráficos capturados:', Object.keys(graficos).filter(k => graficos[k]).length);
        console.log('📊 Gráficos capturados:', graficos);

        // 🔄 CONVERTER SVG → PNG (Backend Syncfusion.Pdf só aceita PNG!)
        console.log('🔄 Convertendo SVG para PNG...');
        const graficosPNG = {};

        for (const [key, svgBase64] of Object.entries(graficos))
        {
            console.log(`🔄 [${key}] Processando conversão...`);

            if (!svgBase64)
            {
                console.warn(`⚠️ [${key}] SVG vazio, pulando conversão`);
                graficosPNG[key] = '';
                continue;
            }

            try
            {
                console.log(`   🔍 [${key}] Iniciando conversão de ${(svgBase64.length / 1024).toFixed(1)}KB...`);
                graficosPNG[key] = await converterSvgParaPng(svgBase64);
                console.log(`✅ [${key}] SVG convertido para PNG com sucesso!`);
            } catch (erro)
            {
                console.error(`❌ [${key}] ERRO ao converter SVG para PNG:`, erro);
                console.error(`❌ [${key}] Mensagem:`, erro.message);
                console.error(`❌ [${key}] Stack:`, erro.stack);
                graficosPNG[key] = ''; // String vazia em caso de erro
            }
        }

        console.log('✅ Todos os gráficos convertidos para PNG!');
        console.log('📊 Total de gráficos PNG:', Object.keys(graficosPNG).filter(k => graficosPNG[k]).length);
        return graficosPNG;
    } catch (error)
    {
        console.error('❌ ERRO FATAL em capturarGraficos:', error);
        console.error('Stack trace:', error.stack);
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'capturarGraficos', error);
        return {};
    }
}

/**
 * Converte SVG Base64 para PNG Base64 usando Blob e URL.createObjectURL
 * Método mais robusto que funciona com SVGs complexos do Syncfusion
 * 
 * 🎯 OBJETIVO     : Converte gráficos SVG do Syncfusion para PNG de alta qualidade usando
 *                   canvas HTML5. Método robusto que suporta SVGs complexos com gradientes,
 *                   animações e filtros.
 * 
 * 📥 ENTRADAS     : svgBase64 [string] - String Base64 do SVG COM prefixo
 *                   "data:image/svg+xml;base64," (formato data URI completo)
 * 
 * 📤 SAÍDAS       : [Promise<string>] PNG Base64 com prefixo "data:image/png;base64,..."
 *                   Qualidade 95%, fundo branco, dimensões preservadas do SVG original
 * 
 * 🔗 CHAMADA POR  : capturarGraficos() [loop de conversão, linha ~2666]
 * 
 * 🔄 CHAMA        : atob() [decodifica Base64],
 *                   new Blob(), URL.createObjectURL(),
 *                   new Image(), img.onload, canvas.getContext('2d'),
 *                   ctx.fillRect(), ctx.drawImage(), canvas.toDataURL('image/png', 0.95),
 *                   URL.revokeObjectURL()
 * 
 * 📝 OBSERVAÇÕES  : • Cria Blob do SVG para URL object (mais robusto que data URI direto)
 *                   • Fundo branco (#FFFFFF) para evitar transparência no PDF
 *                   • Qualidade 95% (0.95) para balanço tamanho/qualidade
 *                   • Fallback de dimensões: 800x600 se img.width/height inválidos
 *                   • CORS: crossOrigin = 'anonymous' para evitar taint
 *                   • Logs de tamanho antes/depois: "XKB (SVG) → YKB (PNG)"
 *                   • Promise.reject em caso de erro (try-catch no caller)
 * 
 * @param {string} svgBase64 - String Base64 do SVG (com data:image/svg+xml;base64, prefixo)
 * @returns {Promise<string>} PNG Base64 (com data:image/png;base64, prefixo)
 */
async function converterSvgParaPng(svgBase64)
{
    try
    {
        return new Promise((resolve, reject) =>
        {
            try
            {
                // 1. Extrair apenas o Base64 puro (remover prefixo data:image/svg+xml;base64,)
                const base64Data = svgBase64.split(',')[1];
                if (!base64Data)
                {
                    reject(new Error('SVG Base64 inválido - sem dados após vírgula'));
                    return;
                }

                // 2. Decodificar Base64 para string SVG
                const svgString = atob(base64Data);

                // 3. Criar Blob do SVG
                const blob = new Blob([svgString], { type: 'image/svg+xml;charset=utf-8' });
                const url = URL.createObjectURL(blob);

                // 4. Criar imagem do SVG
                const img = new Image();

                img.onload = () =>
                {
                    try
                    {
                        // 5. Criar canvas com as dimensões da imagem
                        const canvas = document.createElement('canvas');

                        // Usar dimensões da imagem ou dimensões padrão se inválidas
                        canvas.width = img.width > 0 ? img.width : 800;
                        canvas.height = img.height > 0 ? img.height : 600;

                        console.log(`   📐 Dimensões: ${canvas.width}x${canvas.height}`);

                        // 6. Desenhar SVG no canvas com fundo branco
                        const ctx = canvas.getContext('2d');

                        // Fundo branco (importante para transparência)
                        ctx.fillStyle = '#FFFFFF';
                        ctx.fillRect(0, 0, canvas.width, canvas.height);

                        // Desenhar imagem
                        ctx.drawImage(img, 0, 0);

                        // 7. Converter canvas para PNG Base64
                        const pngBase64 = canvas.toDataURL('image/png', 0.95); // 95% qualidade

                        // 8. Liberar memória
                        URL.revokeObjectURL(url);

                        // 9. Log de tamanho
                        const tamanhoAntes = (svgBase64.length / 1024).toFixed(1);
                        const tamanhoDepois = (pngBase64.length / 1024).toFixed(1);
                        console.log(`   🔄 ${tamanhoAntes}KB (SVG) → ${tamanhoDepois}KB (PNG)`);

                        resolve(pngBase64);
                    } catch (erro)
                    {
                        URL.revokeObjectURL(url);
                        reject(new Error('Erro ao desenhar no canvas: ' + erro.message));
                    }
                };

                img.onerror = (erro) =>
                {
                    URL.revokeObjectURL(url);
                    reject(new Error('Falha ao carregar SVG como imagem: ' + erro));
                };

                // 10. Configurar CORS e iniciar carregamento
                img.crossOrigin = 'anonymous';
                img.src = url;
            } catch (erro)
            {
                reject(new Error('Erro ao processar SVG Base64: ' + erro.message));
            }
        });
    } catch (erro)
    {
        console.error('❌ Erro em converterSvgParaPng:', erro);
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'converterSvgParaPng', erro);
        throw erro;
    }
}

/**
 * Exporta gráfico Syncfusion com DEBUG COMPLETO
 * 
 * 🎯 OBJETIVO     : Exporta gráfico Syncfusion (Chart ou AccumulationChart) como Base64.
 *                   Tenta CANVAS primeiro (mais rápido), depois SVG (se canvas não existir).
 *                   Logs extensivos para diagnóstico.
 * 
 * 📥 ENTRADAS     : chart [Object] - Instância do gráfico Syncfusion (ej.charts.Chart ou
 *                                        ej.charts.AccumulationChart)
 *                   nome [string] - Nome do gráfico para logs (ex: "status", "motoristas")
 * 
 * 📤 SAÍDAS       : [Promise<string|null>] Base64 do gráfico:
 *                   • "data:image/png;base64,..." se canvas encontrado
 *                   • "data:image/svg+xml;base64,..." se SVG encontrado
 *                   • null se nenhum formato encontrado (erro)
 * 
 * 🔗 CHAMADA POR  : capturarGraficos() [6 vezes, linha ~2628-2663]
 * 
 * 🔄 CHAMA        : chart.element.querySelector('canvas'),
 *                   canvas.toDataURL('image/png'),
 *                   chart.element.querySelector('svg'),
 *                   new XMLSerializer().serializeToString(svg),
 *                   btoa(unescape(encodeURIComponent()))
 * 
 * 📝 OBSERVAÇÕES  : • Syncfusion pode usar CANVAS ou SVG dependendo do tipo de gráfico
 *                   • Canvas: padrão para Chart (Column, Bar, Line, Area)
 *                   • SVG: padrão para AccumulationChart (Pie, Donut)
 *                   • Logs console para CADA tentativa de captura
 *                   • Retorna null (não throw) para permitir outros gráficos continuarem
 *                   • Verifica: chart, chart.element, canvas, svg em sequência
 *                   • Logs de tamanho em KB para monitorar payload
 * 
 * @param {Object} chart - Instância do gráfico Syncfusion
 * @param {string} nome - Nome do gráfico (para debug)
 * @returns {Promise<string|null>} Base64 do gráfico
 */
function exportarGraficoSyncfusion(chart, nome)
{
    return new Promise((resolve, reject) =>
    {
        try
        {
            console.log(`🔍 [${nome}] Iniciando captura do gráfico...`);

            // 1. Verifica se o chart existe
            if (!chart)
            {
                console.error(`❌ [${nome}] Chart é null ou undefined`);
                resolve(null);
                return;
            }
            console.log(`✅ [${nome}] Chart existe:`, chart);

            // 2. Verifica se tem element
            if (!chart.element)
            {
                console.error(`❌ [${nome}] chart.element não existe`);
                console.log(`[${nome}] Propriedades do chart:`, Object.keys(chart));
                resolve(null);
                return;
            }
            console.log(`✅ [${nome}] chart.element existe:`, chart.element);

            const chartElement = chart.element;

            // 3. Tenta encontrar CANVAS
            const canvas = chartElement.querySelector('canvas');
            if (canvas)
            {
                console.log(`✅ [${nome}] Canvas encontrado!`);
                console.log(`[${nome}] Canvas dimensões: ${canvas.width}x${canvas.height}`);

                try
                {
                    const base64 = canvas.toDataURL('image/png');
                    console.log(`✅ [${nome}] Canvas convertido para Base64 (${Math.round(base64.length / 1024)}KB)`);
                    resolve(base64);
                    return;
                }
                catch (canvasError)
                {
                    console.error(`❌ [${nome}] Erro ao converter canvas:`, canvasError);
                }
            }
            else
            {
                console.warn(`⚠️ [${nome}] Canvas NÃO encontrado, tentando SVG...`);
            }

            // 4. Tenta encontrar SVG (Syncfusion pode usar SVG ao invés de Canvas)
            const svg = chartElement.querySelector('svg');
            if (svg)
            {
                console.log(`✅ [${nome}] SVG encontrado!`);

                try
                {
                    // Converte SVG para Base64
                    const svgData = new XMLSerializer().serializeToString(svg);
                    const svgBase64 = 'data:image/svg+xml;base64,' + btoa(unescape(encodeURIComponent(svgData)));

                    console.log(`✅ [${nome}] SVG convertido para Base64 (${Math.round(svgBase64.length / 1024)}KB)`);
                    resolve(svgBase64);
                    return;
                }
                catch (svgError)
                {
                    console.error(`❌ [${nome}] Erro ao converter SVG:`, svgError);
                }
            }
            else
            {
                console.warn(`⚠️ [${nome}] SVG NÃO encontrado`);
            }

            // 5. Se não encontrou nem canvas nem SVG, mostra o HTML do elemento
            console.error(`❌ [${nome}] Nem canvas nem SVG encontrados!`);
            console.log(`[${nome}] HTML do elemento:`, chartElement.innerHTML.substring(0, 500));
            console.log(`[${nome}] Filhos do elemento:`, chartElement.children);

            resolve(null);
        }
        catch (error)
        {
            console.error(`❌ [${nome}] ERRO GERAL:`, error);
            console.error(`[${nome}] Stack trace:`, error.stack);
            resolve(null);
        }
    });
}

/**
 * Limpa o PDFViewer quando o modal é fechado
 */
function limparPDFViewer()
{
    try
    {
        if (pdfViewerInstance)
        {
            pdfViewerInstance.destroy();
            pdfViewerInstance = null;
        }

        // Limpa o container
        $('#pdfViewerContainer').empty();
    } catch (error)
    {
        console.error('Erro ao limpar PDFViewer:', error);
    }
}

// ========================================
// MODAL DE AJUSTE DE VIAGEM (Dashboard)
// ========================================

/****************************************************************************************
 * ⚡ FUNÇÃO: inicializarModalAjuste
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Inicializa instância Bootstrap 5 do modal #modalAjustaViagemDashboard
 *                   e registra event listener do botão "Ajustar Viagem".
 *
 * 📥 ENTRADAS     : Nenhuma
 *
 * 📤 SAÍDAS       : Instância modalAjustaViagemDashboard criada (variável global),
 *                   event listener registrado em #btnAjustarViagemDashboard
 *
 * 🔗 CHAMADA POR  : inicializarDashboard() [linha ~136]
 *
 * 🔄 CHAMA        : document.getElementById(), new bootstrap.Modal(),
 *                   addEventListener('click', gravarViagemDashboard)
 *
 * 📝 OBSERVAÇÕES  : • Modal config: keyboard true, backdrop 'static' (não fecha ao clicar fora)
 *                   • Chamado apenas 1x na inicialização do dashboard
 *                   • Try-catch global com Alerta.TratamentoErroComLinha
 ****************************************************************************************/
function inicializarModalAjuste()
{
    try
    {
        const modalEl = document.getElementById('modalAjustaViagemDashboard');
        if (modalEl)
        {
            modalAjustaViagemDashboard = new bootstrap.Modal(modalEl, {
                keyboard: true,
                backdrop: 'static'
            });

            // Evento do botão Ajustar Viagem
            const btnAjustar = document.getElementById('btnAjustarViagemDashboard');
            if (btnAjustar)
            {
                btnAjustar.addEventListener('click', gravarViagemDashboard);
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'inicializarModalAjuste', error);
    }
}

/**
 * Abre o modal de ajuste de viagem
 * Chamado a partir do modal de detalhes do TOP 10
 */
function abrirModalAjusteViagem()
{
    try
    {
        if (!viagemAtualId)
        {
            AppToast.show('Amarelo', 'Nenhuma viagem selecionada', 3000);
            return;
        }

        // Fecha o modal de detalhes
        const modalDetalhes = bootstrap.Modal.getInstance(document.getElementById('modalDetalhesViagem'));
        if (modalDetalhes)
        {
            modalDetalhes.hide();
        }

        // Carrega dados da viagem no modal de ajuste
        carregarDadosViagemParaAjuste(viagemAtualId);

        // Abre o modal de ajuste
        if (modalAjustaViagemDashboard)
        {
            modalAjustaViagemDashboard.show();
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'abrirModalAjusteViagem', error);
    }
}

/**
 * Carrega os dados da viagem no modal de ajuste
 */
function carregarDadosViagemParaAjuste(viagemId)
{
    try
    {
        $.ajax({
            type: 'GET',
            url: '/api/Viagem/GetViagem/' + viagemId,
            success: function (res)
            {
                try
                {
                    if (res && res.success && res.data)
                    {
                        const viagem = res.data;

                        document.getElementById('txtIdDashboard').value = viagem.viagemId;
                        document.getElementById('txtNoFichaVistoriaDashboard').value = viagem.noFichaVistoria || '';

                        // Finalidade
                        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
                        if (lstFinalidade && lstFinalidade.ej2_instances)
                        {
                            lstFinalidade.ej2_instances[0].value = viagem.finalidade || null;
                        }

                        // Evento
                        const lstEvento = document.getElementById('lstEventoDashboard');
                        if (lstEvento && lstEvento.ej2_instances)
                        {
                            if (viagem.finalidade === 'Evento' && viagem.eventoId)
                            {
                                lstEvento.ej2_instances[0].enabled = true;
                                lstEvento.ej2_instances[0].value = [viagem.eventoId.toString()];
                                $('.esconde-diveventos-dashboard').show();
                            } else
                            {
                                lstEvento.ej2_instances[0].enabled = false;
                                lstEvento.ej2_instances[0].value = null;
                                $('.esconde-diveventos-dashboard').hide();
                            }
                        }

                        // Datas e Horas
                        document.getElementById('txtDataInicialDashboard').value = viagem.dataInicial || '';
                        document.getElementById('txtHoraInicialDashboard').value = viagem.horaInicio || '';
                        document.getElementById('txtDataFinalDashboard').value = viagem.dataFinal || '';
                        document.getElementById('txtHoraFinalDashboard').value = viagem.horaFim || '';

                        // Quilometragem
                        document.getElementById('txtKmInicialDashboard').value = viagem.kmInicial || '';
                        document.getElementById('txtKmFinalDashboard').value = viagem.kmFinal || '';

                        // Ramal do Requisitante
                        document.getElementById('txtRamalRequisitanteDashboard').value = viagem.ramalRequisitante || '';

                        // Aguarda um pequeno delay para os combos Syncfusion carregarem os dados
                        setTimeout(function() {
                            try {
                                // Motorista
                                const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
                                if (lstMotorista && lstMotorista.ej2_instances && viagem.motoristaId)
                                {
                                    lstMotorista.ej2_instances[0].value = viagem.motoristaId;
                                }

                                // Veículo
                                const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
                                if (lstVeiculo && lstVeiculo.ej2_instances && viagem.veiculoId)
                                {
                                    lstVeiculo.ej2_instances[0].value = viagem.veiculoId;
                                }

                                // Solicitante (Requisitante)
                                const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
                                if (lstRequisitante && lstRequisitante.ej2_instances && viagem.requisitanteId)
                                {
                                    lstRequisitante.ej2_instances[0].value = viagem.requisitanteId;
                                }

                                // Setor Solicitante (DropDownTree - precisa de array)
                                const lstSetor = document.getElementById('lstSetorSolicitanteAlteradoDashboard');
                                if (lstSetor && lstSetor.ej2_instances && viagem.setorSolicitanteId)
                                {
                                    lstSetor.ej2_instances[0].value = [viagem.setorSolicitanteId];
                                }
                            } catch (error) {
                                console.error('Erro ao setar valores dos combos:', error);
                            }
                        }, 300);

                    } else
                    {
                        AppToast.show('Amarelo', res.message || 'Viagem não encontrada', 3000);
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste.success', error);
                }
            },
            error: function (xhr, status, error)
            {
                Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste.error', error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'carregarDadosViagemParaAjuste', error);
    }
}

/**
 * Evento de mudança da finalidade no modal de ajuste
 */
function FinalidadeChangeDashboard()
{
    try
    {
        var finalidadeCb = document.getElementById('lstFinalidadeAlteradaDashboard').ej2_instances[0];
        var eventoDdt = document.getElementById('lstEventoDashboard').ej2_instances[0];

        if (finalidadeCb && eventoDdt)
        {
            if (finalidadeCb.value === 'Evento')
            {
                eventoDdt.enabled = true;
                $('.esconde-diveventos-dashboard').show();
            } else
            {
                eventoDdt.enabled = false;
                eventoDdt.value = null;
                $('.esconde-diveventos-dashboard').hide();
            }
        }
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'FinalidadeChangeDashboard', error);
    }
}

/**
 * Grava as alterações da viagem
 */
function gravarViagemDashboard()
{
    try
    {
        const viagemId = document.getElementById('txtIdDashboard').value;
        const noFichaVistoria = document.getElementById('txtNoFichaVistoriaDashboard').value;

        // Finalidade
        const lstFinalidade = document.getElementById('lstFinalidadeAlteradaDashboard');
        const finalidade = lstFinalidade && lstFinalidade.ej2_instances ? lstFinalidade.ej2_instances[0].value : null;

        // Evento
        const lstEvento = document.getElementById('lstEventoDashboard');
        let eventoId = null;
        if (lstEvento && lstEvento.ej2_instances)
        {
            const eventoValue = lstEvento.ej2_instances[0].value;
            if (eventoValue && eventoValue.length > 0)
            {
                eventoId = eventoValue[0];
            }
        }

        // Datas e Horas
        const dataInicial = document.getElementById('txtDataInicialDashboard').value || null;
        const horaInicial = document.getElementById('txtHoraInicialDashboard').value || null;
        const dataFinal = document.getElementById('txtDataFinalDashboard').value || null;
        const horaFinal = document.getElementById('txtHoraFinalDashboard').value || null;

        // Km
        const kmInicial = parseInt(document.getElementById('txtKmInicialDashboard').value) || null;
        const kmFinal = parseInt(document.getElementById('txtKmFinalDashboard').value) || null;

        // Motorista
        const lstMotorista = document.getElementById('lstMotoristaAlteradoDashboard');
        const motoristaId = lstMotorista && lstMotorista.ej2_instances ? lstMotorista.ej2_instances[0].value : null;

        // Veículo
        const lstVeiculo = document.getElementById('lstVeiculoAlteradoDashboard');
        const veiculoId = lstVeiculo && lstVeiculo.ej2_instances ? lstVeiculo.ej2_instances[0].value : null;

        // Setor Solicitante
        const lstSetor = document.getElementById('lstSetorSolicitanteAlteradoDashboard');
        let setorSolicitanteId = null;
        if (lstSetor && lstSetor.ej2_instances)
        {
            const setorValue = lstSetor.ej2_instances[0].value;
            if (setorValue && setorValue.length > 0)
            {
                setorSolicitanteId = setorValue[0];
            }
        }

        // Solicitante (Requisitante)
        const lstRequisitante = document.getElementById('lstRequisitanteAlteradoDashboard');
        const requisitanteId = lstRequisitante && lstRequisitante.ej2_instances ? lstRequisitante.ej2_instances[0].value : null;

        // Ramal do Requisitante
        const ramalRequisitante = document.getElementById('txtRamalRequisitanteDashboard').value || null;

        const dados = {
            ViagemId: viagemId,
            NoFichaVistoria: parseInt(noFichaVistoria) || null,
            Finalidade: finalidade,
            EventoId: eventoId,
            DataInicial: dataInicial,
            HoraInicio: horaInicial,
            DataFinal: dataFinal,
            HoraFim: horaFinal,
            KmInicial: kmInicial,
            KmFinal: kmFinal,
            MotoristaId: motoristaId,
            VeiculoId: veiculoId,
            SetorSolicitanteId: setorSolicitanteId,
            RequisitanteId: requisitanteId,
            RamalRequisitante: ramalRequisitante
        };

        // Mostrar spinner
        const btnAjustar = document.getElementById('btnAjustarViagemDashboard');
        const spinner = btnAjustar.querySelector('.spinner-border');
        const btnText = btnAjustar.querySelector('.btn-text');
        if (spinner) spinner.classList.remove('d-none');
        if (btnText) btnText.textContent = 'Gravando...';
        btnAjustar.disabled = true;

        $.ajax({
            type: 'POST',
            url: '/api/Viagem/AtualizarDadosViagemDashboard',
            contentType: 'application/json',
            data: JSON.stringify(dados),
            success: function (res)
            {
                try
                {
                    // Esconder spinner do botão
                    if (spinner) spinner.classList.add('d-none');
                    if (btnText) btnText.textContent = 'Ajustar Viagem';
                    btnAjustar.disabled = false;

                    if (res.success)
                    {
                        // Fechar modal de ajustes
                        if (modalAjustaViagemDashboard)
                        {
                            modalAjustaViagemDashboard.hide();
                        }

                        AppToast.show('Verde', 'Viagem atualizada com sucesso!', 3000);

                        // Mostrar loading com mensagem personalizada
                        mostrarLoadingGeral('Recalculando Custos e Atualizando Dashboard...');

                        // Pequeno delay para o trigger do banco processar os custos
                        setTimeout(function() {
                            // Recarregar o dashboard
                            carregarDadosDashboard();
                        }, 500);
                    } else
                    {
                        AppToast.show('Vermelho', res.message || 'Erro ao atualizar viagem', 4000);
                    }
                } catch (error)
                {
                    Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard.success', error);
                }
            },
            error: function (xhr, status, error)
            {
                // Esconder spinner
                if (spinner) spinner.classList.add('d-none');
                if (btnText) btnText.textContent = 'Ajustar Viagem';
                btnAjustar.disabled = false;

                AppToast.show('Vermelho', 'Erro ao gravar: ' + error, 4000);
                Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard.error', error);
            }
        });
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'gravarViagemDashboard', error);
    }
}

// ========================================
// FUNÇÕES DE FILTRO ANO/MÊS
// ========================================

/**
 * Popula o select de anos com anos disponíveis (último ano até 5 anos atrás)
 */
function popularAnosDisponiveis()
{
    try
    {
        const selectAno = document.getElementById('filtroAno');
        if (!selectAno) return;

        const anoAtual = new Date().getFullYear();
        selectAno.innerHTML = '<option value="">&lt;Todos os Anos&gt;</option>';

        for (let ano = anoAtual; ano >= anoAtual - 5; ano--)
        {
            const option = document.createElement('option');
            option.value = ano;
            option.textContent = ano;
            selectAno.appendChild(option);
        }
    } catch (error)
    {
        console.error('Erro ao popular anos:', error);
    }
}

/**
 * Atualiza o label do período atual
 */
function atualizarLabelPeriodo()
{
    try
    {
        const label = document.getElementById('periodoAtualLabel');
        if (!label) return;

        const ano = document.getElementById('filtroAno')?.value;
        const mes = document.getElementById('filtroMes')?.value;
        const dataInicio = document.getElementById('dataInicio')?.value;
        const dataFim = document.getElementById('dataFim')?.value;

        const meses = ['', 'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
                       'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];

        if (dataInicio && dataFim)
        {
            const dtIni = new Date(dataInicio + 'T00:00:00');
            const dtFim = new Date(dataFim + 'T23:59:59');
            label.textContent = `Período: ${dtIni.toLocaleDateString('pt-BR')} a ${dtFim.toLocaleDateString('pt-BR')}`;
        }
        else if (ano && mes)
        {
            label.textContent = `Período: ${meses[parseInt(mes)]}/${ano}`;
        }
        else if (ano && !mes)
        {
            label.textContent = `Período: Ano ${ano} (todos os meses)`;
        }
        else if (!ano && mes)
        {
            label.textContent = `Período: ${meses[parseInt(mes)]} (todos os anos)`;
        }
        else
        {
            label.textContent = 'Exibindo todos os dados';
        }
    } catch (error)
    {
        console.error('Erro ao atualizar label de período:', error);
    }
}

/**
 * Filtra dados por Ano/Mês
 * Permite combinar: Ano+Mês, só Ano, só Mês, ou nenhum (todos os dados)
 */
function filtrarPorAnoMes()
{
    try
    {
        const ano = document.getElementById('filtroAno')?.value;
        const mes = document.getElementById('filtroMes')?.value;

        // Limpa período personalizado
        document.getElementById('dataInicio').value = '';
        document.getElementById('dataFim').value = '';
        $('.btn-period').removeClass('active');

        // Se não selecionou nada, mostra todos os dados (últimos 5 anos)
        if (!ano && !mes)
        {
            const anoAtual = new Date().getFullYear();
            periodoAtual.dataInicio = new Date(anoAtual - 5, 0, 1, 0, 0, 0);
            periodoAtual.dataFim = new Date(anoAtual, 11, 31, 23, 59, 59);

            atualizarLabelPeriodo();
            carregarDadosDashboard();
            return;
        }

        const anoNum = ano ? parseInt(ano) : null;
        const mesNum = mes ? parseInt(mes) : null;

        if (anoNum && mesNum)
        {
            // Filtro: Ano específico + Mês específico
            periodoAtual.dataInicio = new Date(anoNum, mesNum - 1, 1, 0, 0, 0);
            periodoAtual.dataFim = new Date(anoNum, mesNum, 0, 23, 59, 59);
        }
        else if (anoNum && !mesNum)
        {
            // Filtro: Ano específico + Todos os meses
            periodoAtual.dataInicio = new Date(anoNum, 0, 1, 0, 0, 0);
            periodoAtual.dataFim = new Date(anoNum, 11, 31, 23, 59, 59);
        }
        else if (!anoNum && mesNum)
        {
            // Filtro: Todos os anos + Mês específico (últimos 5 anos)
            const anoAtual = new Date().getFullYear();
            const anosParaBuscar = [];

            // Busca dados do mês nos últimos 5 anos
            for (let a = anoAtual; a >= anoAtual - 5; a--)
            {
                anosParaBuscar.push(a);
            }

            // Define período do primeiro ano até o último ano
            periodoAtual.dataInicio = new Date(anoAtual - 5, mesNum - 1, 1, 0, 0, 0);
            periodoAtual.dataFim = new Date(anoAtual, mesNum, 0, 23, 59, 59);
        }

        atualizarLabelPeriodo();
        carregarDadosDashboard();
    } catch (error)
    {
        console.error('Erro ao filtrar por ano/mês:', error);
        AppToast.show('Vermelho', 'Erro ao filtrar por ano/mês.', 3000);
    }
}

/**
 * Limpa filtro de Ano/Mês
 */
function limparFiltroAnoMes()
{
    try
    {
        document.getElementById('filtroAno').value = '';
        document.getElementById('filtroMes').value = '';

        // Define período padrão (últimos 30 dias)
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);

        atualizarLabelPeriodo();
        carregarDadosDashboard();
    } catch (error)
    {
        console.error('Erro ao limpar filtro ano/mês:', error);
    }
}

/**
 * Limpa filtro de Período Personalizado
 */
function limparFiltroPeriodo()
{
    try
    {
        document.getElementById('dataInicio').value = '';
        document.getElementById('dataFim').value = '';
        $('.btn-period').removeClass('active');

        // Define período padrão (últimos 30 dias)
        const hoje = new Date();
        periodoAtual.dataFim = new Date(hoje.getFullYear(), hoje.getMonth(), hoje.getDate(), 23, 59, 59);
        periodoAtual.dataInicio = new Date(periodoAtual.dataFim);
        periodoAtual.dataInicio.setDate(periodoAtual.dataInicio.getDate() - 30);

        atualizarLabelPeriodo();
        carregarDadosDashboard();
    } catch (error)
    {
        console.error('Erro ao limpar filtro de período:', error);
    }
}

// ========================================
// EVENTOS
// ========================================

$(document).ready(function ()
{
    try
    {
        popularAnosDisponiveis();
        inicializarDashboard();

        // Eventos dos botões de filtro Ano/Mês
        $('#btnFiltrarAnoMes').on('click', filtrarPorAnoMes);
        $('#btnLimparAnoMes').on('click', limparFiltroAnoMes);

        // Eventos dos botões de filtro Período
        $('#btnFiltrarPeriodo').on('click', aplicarFiltroPersonalizado);
        $('#btnLimparPeriodo').on('click', limparFiltroPeriodo);

        // Eventos dos botões de período rápido com data-dias
        $('.btn-period').on('click', function() {
            const dias = parseInt($(this).data('dias'));
            if (dias) {
                // Limpa filtros de ano/mês
                document.getElementById('filtroAno').value = '';
                document.getElementById('filtroMes').value = '';

                $('.btn-period').removeClass('active');
                $(this).addClass('active');
                aplicarFiltroPeriodo(dias);
            }
        });

        // Evento do botão atualizar
        $('#btnAtualizar').on('click', atualizarDashboard);

        // Evento do botão exportar PDF
        $('#btnExportarPDF').on('click', exportarParaPDF);

        // Evento do botão baixar PDF
        $('#btnBaixarPDF').on('click', baixarPDF);

        // Limpa o PDFViewer quando o modal é fechado
        $('#modalPDFViewer').on('hidden.bs.modal', limparPDFViewer);

        // Evento do botão Editar Viagem no modal de detalhes
        $('#btnEditarViagemDashboard').on('click', abrirModalAjusteViagem);
    } catch (error)
    {
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'document.ready', error);
    }
});

/**
* Captura os 9 cards de estatísticas como imagens PNG usando html2canvas
* @returns {Promise<Object>} Dicionário com Base64 PNG de cada card
*/
async function capturarCards()
{
    try
    {
        console.log('🎨 ===== INICIANDO CAPTURA DE CARDS =====');

        const cards = {};

        // Lista de IDs dos cards na ordem (3x3)
        const cardIds = [
            'cardCustoTotal', 'cardTotalViagens', 'cardCustoMedio',
            'cardKmTotal', 'cardKmMedio', 'cardViagensFinalizadas',
            'cardViagensEmAndamento', 'cardViagensAgendadas', 'cardViagensCanceladas'
        ];

        for (const cardId of cardIds)
        {
            const elemento = document.getElementById(cardId);

            if (!elemento)
            {
                console.warn(`⚠️ [${cardId}] Elemento não encontrado no DOM`);
                cards[cardId] = '';
                continue;
            }

            try
            {
                console.log(`🎨 [${cardId}] Capturando card...`);

                // Captura o elemento como canvas usando html2canvas
                const canvas = await html2canvas(elemento, {
                    backgroundColor: '#ffffff',
                    scale: 2, // Alta qualidade
                    logging: false,
                    useCORS: true,
                    allowTaint: true
                });

                // Converte canvas para Base64 PNG
                const base64PNG = canvas.toDataURL('image/png');

                cards[cardId] = base64PNG;

                // Log do tamanho
                const tamanhoKB = (base64PNG.length / 1024).toFixed(1);
                console.log(`✅ [${cardId}] Card capturado (${tamanhoKB} KB)`);
            } catch (erro)
            {
                console.error(`❌ [${cardId}] Erro ao capturar card:`, erro);
                console.error(`❌ [${cardId}] Mensagem:`, erro.message);
                cards[cardId] = '';
            }
        }

        const totalCapturados = Object.keys(cards).filter(k => cards[k]).length;
        console.log(`✅ Total de cards capturados: ${totalCapturados}/${cardIds.length}`);
        console.log('🎨 ===== CAPTURA DE CARDS FINALIZADA =====');

        return cards;
    } catch (error)
    {
        console.error('❌ ERRO FATAL em capturarCards:', error);
        console.error('Stack trace:', error.stack);
        Alerta.TratamentoErroComLinha('dashboard-viagens.js', 'capturarCards', error);
        return {};
    }
}
