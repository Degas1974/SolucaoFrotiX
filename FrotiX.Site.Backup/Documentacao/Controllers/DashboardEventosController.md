# Documentação: Inteligência e Analytics de Eventos (DashboardEventosController)

O \DashboardEventosController\ transforma dados brutos de agendamentos em inteligência estratégica. Ele permite que os gestores compreendam não apenas *que* eventos ocorreram, mas qual o impacto operacional deles em termos de mobilização de pessoas e demanda por setor. É o painel de controle para medir a eficiência da frota no suporte às atividades institucionais.

## 1. Análise Comparativa Temporal (Time-Travel)

O diferencial deste dashboard é o endpoint \ObterEstatisticasGerais\, que implementa uma lógica de **Período Anterior Automático**. Ao selecionar um intervalo de datas, o sistema calcula instantaneamente o mesmo intervalo no passado (ex: se você olha os últimos 30 dias, ele busca também os 30 dias imediatamente anteriores).

\\\csharp
// Lógica de "espelhamento" temporal para comparação de performance
var diasPeriodo = (dataFim.Value - DataInicial.Value).Days;
var DataInicialAnterior = DataInicial.Value.AddDays(-(diasPeriodo + 1));
var dataFimAnterior = DataInicial.Value.AddSeconds(-1);

var eventosAnteriores = await _context.Evento
    .Where(e => e.DataInicial >= DataInicialAnterior && e.DataInicial <= dataFimAnterior)
    .ToListAsync();
\\\

Isso permite que o gestor visualize se houve um crescimento ou queda na demanda de transporte para eventos em relação ao mês anterior, facilitando a previsão de custos e escalonamento de motoristas.

## 2. Métricas de Engajamento e Participação

Além da contagem de eventos, o controller foca no **Volume de Participantes**. Através dos endpoints \ObterEventosPorStatus\ e \ObterTop10EventosMaiores\, o FrotiX identifica quais tipos de atividade geram maior necessidade de transporte coletivo. A média de participantes por evento é um KPI crucial para decidir entre o envio de vários veículos leves ou a locação de uma van/micro-ônibus.

## 3. Desempenho por Setor (Taxa de Conclusão)

O endpoint \ObterEventosPorSetor\ vai além da simples contagem. Ele calcula a **Taxa de Conclusão**, permitindo identificar setores que agendam muitos eventos mas acabam cancelando a maioria (\ventosCancelados\). Essa visão ajuda a combater o desperdício de reserva de veículos, otimizando a agenda global para quem realmente executa as atividades.

## 4. Exportação e Prestação de Contas

Por ser uma \partial class\, a lógica de geração de documentos está separada em um arquivo de exportação PDF específico (\DashboardEventosController_ExportacaoPDF.cs\). Isso permite gerar relatórios prontos para impressão que consolidam todos os gráficos e tabelas do dashboard em um documento A4 profissional para reuniões de diretoria.

---

### Notas de Implementação (Padrão FrotiX)

*   **Asynchronous Queries:** Todas as consultas ao banco utilizam \ToListAsync\, garantindo que o servidor não trave enquanto processa grandes volumes de dados de eventos.
*   **Ajuste de Fim de Dia:** O sistema ajusta a \dataFim\ para \23:59:59\ automágicamente, garantindo que eventos ocorridos no último segundo do dia sejam contabilizados.
*   **Serialização JSON:** O retorno utiliza o padrão caminhos de propriedade (camelCase) para integração direta com as bibliotecas de gráficos (ApexCharts/Chart.js) no frontend.

---
*Documentação atualizada em 2026.01.14 conforme novo padrão de Prosa Leve.*


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
