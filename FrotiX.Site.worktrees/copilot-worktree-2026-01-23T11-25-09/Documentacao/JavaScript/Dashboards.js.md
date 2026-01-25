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


---

# PARTE 2: LOG DE MODIFICAÃ‡Ã•ES/CORREÃ‡Ã•ES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [19/01/2026] - AtualizaÃ§Ã£o: ImplementaÃ§Ã£o de MÃ©todos com Tracking Seletivo

**DescriÃ§Ã£o**: MigraÃ§Ã£o de chamadas .AsTracking() para novos mÃ©todos GetWithTracking() e GetFirstOrDefaultWithTracking() como parte da otimizaÃ§Ã£o de performance do Entity Framework Core.

**Arquivos Afetados**:
- Este arquivo (uso dos novos mÃ©todos do repositÃ³rio)
- Repository/IRepository/IRepository.cs (definiÃ§Ã£o dos novos mÃ©todos)
- Repository/Repository.cs (implementaÃ§Ã£o)
- RegrasDesenvolvimentoFrotiX.md (seÃ§Ã£o 4.2 - nova regra permanente)

**MudanÃ§as**:
- âŒ **ANTES**: _unitOfWork.Entity.AsTracking().Get(id) ou _unitOfWork.Entity.AsTracking().GetFirstOrDefault(filter)
- âœ… **AGORA**: _unitOfWork.Entity.GetWithTracking(id) ou _unitOfWork.Entity.GetFirstOrDefaultWithTracking(filter)

**Motivo**: 
- OtimizaÃ§Ã£o de memÃ³ria e performance
- Tracking seletivo (apenas quando necessÃ¡rio para Update/Delete)
- PadrÃ£o mais limpo e explÃ­cito
- Conformidade com nova regra permanente (RegrasDesenvolvimentoFrotiX.md seÃ§Ã£o 4.2)

**Impacto**: 
- Melhoria de performance em operaÃ§Ãµes de leitura (usa AsNoTracking por padrÃ£o)
- Tracking correto em operaÃ§Ãµes de escrita (Update/Delete)
- Zero impacto funcional (comportamento mantido)

**Status**: âœ… **ConcluÃ­do**

**ResponsÃ¡vel**: Sistema (AtualizaÃ§Ã£o AutomÃ¡tica)

**VersÃ£o**: Incremento de patch
