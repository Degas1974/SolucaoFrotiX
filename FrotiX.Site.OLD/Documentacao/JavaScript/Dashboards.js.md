# Dashboards (JavaScript) - Visão Geral e Lógica de Gráficos

Os scripts de Dashboard no FrotiX são responsáveis por transformar dados brutos vindos das APIs em visualizações ricas, interativas e acionáveis para os gestores.

## O Que É?
Uma coleção de scripts especializados em visualização de dados localizada em wwwroot/js/dashboards/. Eles utilizam bibliotecas como Chart.js e componentes Syncfusion para montar KPIs e gráficos de desempenho.

## Por Que Existe?
Para fornecer uma experiência de monitoramento em tempo real sem a necessidade de recarregar a página. Eles garantem que a identidade visual (Cores FrotiX) seja consistente em todos os módulos de análise.

## Como Funciona?

### 1. Paleta de Cores e Formatação
Todos os dashboards compartilham o objeto CORES_FROTIX, garantindo que "Azul" seja sempre o mesmo tom de azul petróleo do sistema.
- **Função ormatarNumero:** Padroniza a exibição brasileira (Ponto para milhar, Vírgula para decimal).
- **Função ormatarValorMonetario:** Possui lógica inteligente (Valores < 100 exibem decimais, valores >= 100 omitem para facilitar a leitura visual).

### 2. Ciclo de Vida do Dashboard
1.  **inicializarDashboard():** Mostra o loading overlay específico do módulo, define o período inicial (geralmente últimos 30 dias) e limpa instâncias anteriores de gráficos.
2.  **carregarDadosDashboard():** Dispara chamadas etch para os Endpoints de API.
3.  **Renderização:** Ao receber o JSON, as funções de montagem (ex: montarGraficoViagensPorStatus) destroem o gráfico antigo (para evitar memory leaks) e criam o novo.

## Scripts de Destaque

### dashboard-viagens.js 
O mais complexo do sistema (3000+ linhas). Além dos gráficos, gerencia:
- **Ajuste de Viagem:** Modal que permite corrigir KMs e Datas diretamente do dashboard.
- **Visualização de PDF:** Integração para abrir relatórios ou tickets de pedágio sem sair da tela.

### dashboard-abastecimento.js
Focado em consumo e economia, calcula médias de KM/L e custo por KM em tempo real no lado do cliente.

## Detalhes Técnicos (Desenvolvedor)
- **Namespace:** Variáveis globais de controle para instâncias do Chart.js.
- **Modais:** Otimizados para shown.bs.modal para evitar erros de renderização de gráficos antes do elemento estar visível.
- **Loading:** Utiliza mostrarLoadingInicial() e sconderLoadingInicial() que manipulam opacidade e display para uma transição suave.
