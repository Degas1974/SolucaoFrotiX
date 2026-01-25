# Inteligência de Glosas e Auditoria de Contratos

O GlosaService é o componente encarregado de calcular a liquidação financeira de contratos de prestação de serviços por disponibilidade. Sua missão principal é transformar os dados operacionais de indisponibilidade de veículos (saída para manutenção, atrasos na entrega) em valores financeiros reais de desconto, garantindo que o sistema pague apenas pelo que foi efetivamente entregue.

## 🧠 Lógica de Consolidação (Ateste Mensal)

Diferente de uma simples consulta, este serviço realiza uma agregação multidimensional. Ele cruza o que foi **contratado** (Itens de Contrato) com o que foi **executado** (Ordens de Serviço na oficina).

### Como o cálculo funciona:
1.  **Visão por Item:** O serviço agrupa todas as Ordens de Serviço por item de contrato. Isso é crucial porque um mesmo item (ex: "Sedan Premium") pode ter múltiplos veículos sofrendo glosas no mesmo mês.
2.  **Cálculo do Ateste:** O serviço define o valor de "Ateste" (o valor liberado para pagamento) subtraindo o somatório das glosas do valor total mensal contratado.
3.  **Independência de O.S.:** Uma característica vital da implementação é que o preço total mensal do item é derivado da Quantidade * ValorUnitario do contrato, e não da soma das O.S., garantindo que o teto contratual seja respeitado.

## 🛠 Snippets de Lógica Principal

### Agregação de Valores via LINQ
Abaixo, o trecho que realiza a mágica da consolidação financeira por item de contrato:

`csharp
var query = baseQuery
    .GroupBy(g => new { g.NumItem, g.Descricao })
    .Select(s => new GlosaResumoItemDto {
        NumItem = s.Key.NumItem,
        Descricao = s.Key.Descricao,
        // O preço total é a potência contratada máxima do item
        PrecoTotalMensal = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)),
        // A glosa é a soma real de todas as indisponibilidades do período
        Glosa = s.Sum(i => i.ValorGlosa),
        // Valor Final = Contrato - Penalidades
        ValorParaAteste = (s.Max(i => i.Quantidade) * s.Max(i => i.ValorUnitario)) - s.Sum(i => i.ValorGlosa),
    });
`

## 📝 Notas de Implementação

- **DTOs Desacoplados:** Utilizamos o GlosaResumoItemDto e GlosaDetalheItemDto para garantir que a interface receba apenas os campos formatados (como datas amigáveis e valores em decimal), sem expor entidades ricas do EF Core.
- **Performance:** O uso de GetAllReducedIQueryable com sNoTracking: true garante que o processamento seja feito predominantemente em memória ou otimizado pelo SQL Server, ideal para relatórios complexos.
- **Data de Retorno:** No detalhamento de glosas, a DataDevolucao é apresentada como o "Retorno" do veículo à frota ativa, fechando o ciclo de indisponibilidade.

---
*Documentação de inteligência de negócios - FrotiX 2026. Precisão absoluta no controle de custos.*


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
