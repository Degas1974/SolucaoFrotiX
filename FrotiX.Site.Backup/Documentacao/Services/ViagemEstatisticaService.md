# Motor de Estatísticas e Análise Preditiva de Viagens

O ViagemEstatisticaService é o cérebro analítico do FrotiX. Ele não apenas lê dados; ele interpreta o comportamento da frota em datas específicas e consolida indicadores complexos (KPIs) em uma tabela de cache dedicada (ViagemEstatistica). Isso permite que os dashboards carreguem instantaneamente, eliminando a necessidade de varrer milhares de viagens a cada acesso.

## 📊 Arquitetura de Cache e Performance

O serviço opera sob um modelo híbrido: **Cálculo sob Demanda + Cache Persistente**.

### Estratégia de Atualização:
1.  **Sempre Atualizado:** Diferente de caches comuns, o método ObterEstatisticasAsync recalcula os dados sempre que solicitado para uma data específica. Isso garante que, se uma viagem for finalizada agora, o dashboard reflita essa mudança imediatamente após o refresh.
2.  **Leitura do Período (Fast Read):** Para visões históricas (gráficos mensais/semanais), o sistema utiliza o ObterEstatisticasPeriodoAsync. Este método possui uma correção crítica de performance: ele **apenas lê** o que já foi calculado na tabela de estatísticas, sem disparar novos cálculos em tempo real.
3.  **Consolidação de Custos:** O serviço soma atomatizadamente 5 dimensões de custo (Veículo, Motorista, Operador, Lavador e Combustível), provendo o custo total e médio por viagem.

## 🛠 Snippets de Lógica Principal

### Captura de Metadados e KPIs
Abaixo, a lógica que transforma uma coleção de viagens em dados gerenciais:

`csharp
// ESTATÍSTICAS DE STATUS
estatistica.TotalViagens = viagens.Count;
estatistica.ViagensFinalizadas = viagens.Count(v => v.Status == "Realizada");
estatistica.ViagensEmAndamento = viagens.Count(v => v.Status == "Aberta");

// CUSTOS GERAIS (Total de 5 Fontes de Custo)
estatistica.CustoTotal = (decimal)viagens.Sum(v =>
    (v.CustoVeiculo ?? 0) + (v.CustoMotorista ?? 0) + (v.CustoOperador ?? 0) +
    (v.CustoLavador ?? 0) + (v.CustoCombustivel ?? 0));
`

## 📝 Notas de Implementação

- **Integração com Dashboards:** Os dados gerados por este serviço alimentam o DashboardViagensController. O uso da tabela de estatísticas reduz a carga no SQL Server em frotas com mais de 500.000 viagens.
- **Quilometragem e Eficiência:** O serviço isola viagens marcadas como "Realizada" para calcular a metragem total e média, evitando poluir os dados com agendamentos cancelados ou pendentes.
- **Persistência Forçada:** Ao encontrar um registro pré-existente para a mesma data, o serviço utiliza AtualizarEstatistica para sobrepor os novos KPIs, mantendo a tabela de histórico sempre fiel à realidade operacional mais recente.

---
*Documentação de inteligência analítica - FrotiX 2026. Dados transformados em decisões estratégicas.*


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
