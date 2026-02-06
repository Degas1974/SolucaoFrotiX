# Documentação: EstatisticaMotoristasMensal.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
4. [Interconexões](#interconexões)

---

## Visão Geral

O Model `EstatisticaMotoristasMensal` representa estatísticas mensais consolidadas por motorista, incluindo viagens (total, KM, minutos), multas (total, valor) e abastecimentos (total, litros, valor).

**Principais características:**

✅ **Agrupamento por Motorista**: Estatísticas individuais por motorista  
✅ **Múltiplas Métricas**: Viagens, multas, abastecimentos  
✅ **Agregação Mensal**: Dados por mês/ano

---

## Estrutura do Model

```csharp
[Table("EstatisticaMotoristasMensal")]
public class EstatisticaMotoristasMensal
{
    [Key]
    public Guid Id { get; set; }

    public Guid MotoristaId { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }

    // Viagens
    public int TotalViagens { get; set; }
    public decimal KmTotal { get; set; }
    public int MinutosTotais { get; set; }

    // Multas
    public int TotalMultas { get; set; }
    public decimal ValorTotalMultas { get; set; }

    // Abastecimentos
    public int TotalAbastecimentos { get; set; }
    public decimal LitrosTotais { get; set; }
    public decimal ValorTotalAbastecimentos { get; set; }

    public DateTime DataAtualizacao { get; set; }

    [ForeignKey("MotoristaId")]
    public virtual Motorista Motorista { get; set; }
}
```

---

## Notas Importantes

1. **Múltiplas Entidades**: Agrega dados de Viagens, Multas e Abastecimentos
2. **Performance**: Pré-calculado para evitar múltiplos JOINs

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
