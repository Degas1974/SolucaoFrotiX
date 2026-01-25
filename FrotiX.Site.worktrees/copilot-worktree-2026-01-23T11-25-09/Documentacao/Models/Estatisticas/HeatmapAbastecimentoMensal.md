# Documentação: HeatmapAbastecimentoMensal.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)

---

## Visão Geral

O Model `HeatmapAbastecimentoMensal` representa dados para criação de heatmaps de abastecimentos, mostrando distribuição por dia da semana e hora do dia. Similar a `HeatmapViagensMensal`, mas para abastecimentos.

---

## Estrutura do Model

```csharp
[Table("HeatmapAbastecimentoMensal")]
public class HeatmapAbastecimentoMensal
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    public Guid? VeiculoId { get; set; }        // NULL = todos os veículos
    public string? TipoVeiculo { get; set; }    // NULL = todos os tipos
    public int DiaSemana { get; set; }          // 0=Domingo, 1=Segunda, ... 6=Sábado
    public int Hora { get; set; }                // 0-23

    public int TotalAbastecimentos { get; set; }
    public decimal ValorTotal { get; set; }

    public DateTime DataAtualizacao { get; set; }
}
```

---

## Notas Importantes

1. **Filtros Opcionais**: VeiculoId e TipoVeiculo podem ser NULL para dados gerais
2. **Heatmap**: Usado para visualização de padrões de abastecimento

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Status**: ✅ **Concluído**

**Versão**: 2.0

---

**Última atualização**: 08/01/2026  
**Autor**: Sistema FrotiX  
**Versão**: 2.0


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
