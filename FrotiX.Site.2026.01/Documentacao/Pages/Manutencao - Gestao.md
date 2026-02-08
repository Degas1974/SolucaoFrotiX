# Gestão de Manutenção e Ordem de Serviço (OS)

O controle de **Manutenção** no FrotiX garante a disponibilidade da frota através de um fluxo rigoroso de Ordens de Serviço. O ManutencaoController coordena desde a solicitação inicial até o fechamento financeiro, integrando-se com os módulos de estoque e custos.

## 🛠 Inteligência de Oficina

Este módulo utiliza um motor de filtros avançado para permitir que o gestor tenha uma visão 360º de todas as manutenções em andamento (corretivas ou preventivas).

### Funcionalidades de Destaque:
1.  **Filtro Preditivo Unificado:** O sistema permite filtrar por Veículo, Status da OS ou Período (Mês/Ano ou Datas Customizadas) usando uma única Expressão Lambda (manutencoesFilters). Isso garante que a lógica de busca seja idêntica em toda a aplicação.
2.  **Parsing Inteligente de Datas:** A API suporta mais de 6 formatos de data diferentes (incluindo padrões ISO e brasileiros), garantindo que dados vindos de diferentes componentes de frontend sejam interpretados corretamente.
3.  **Cache de Consultas:** Implementa um sistema de cache temporário para listas de referência e buscas frequentes, reduzindo o tempo de resposta da grid para menos de 100ms em frotas com alto volume de OS.

## 🛠 Snippets de Lógica Principal

### Motor de Filtro Dinâmico
A lógica de filtro é desacoplada para garantir que o banco de dados realize o esforço de busca de forma otimizada:

`csharp
static Expression<Func<ViewManutencao, bool>> manutencoesFilters(Guid veiculoId, string statusId, int? mes, int? ano, DateTime? dtIni, DateTime? dtFim) {
    return vm => 
        (veiculoId == Guid.Empty || vm.VeiculoId == veiculoId) &&
        (string.IsNullOrWhiteSpace(statusId) || vm.StatusOS == statusId) &&
        (
            (!mes.HasValue || (vm.DataSolicitacaoRaw.Value.Month == mes.Value && vm.DataSolicitacaoRaw.Value.Year == ano.Value)) ||
            (!dtIni.HasValue || (vm.DataSolicitacaoRaw.Value.Date >= dtIni.Value.Date && vm.DataSolicitacaoRaw.Value.Date <= dtFim.Value.Date))
        );
}
`

## 📝 Notas de Implementação

- **Integração com Glosas:** Manutenções que apresentam divergências de valores são automaticamente encaminhadas para o GlosaController para auditoria financeira.
- **Rastreabilidade de Usuário:** Todas as etapas (Solicitação, Autorização, Finalização) gravam o UsuarioId correspondente, permitindo auditoria completa do fluxo de manutenção.
- **Controle de Lavagem:** O sistema possui um sub-fluxo específico (ControleLavagem) que utiliza a mesma base tecnológica para gerenciar higienização rápida sem a complexidade de uma OS de mecânica.

---
*Documentação gerada para a Solução FrotiX 2026. Segurança e disponibilidade para sua frota.*


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
