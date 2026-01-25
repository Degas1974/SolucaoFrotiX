# Documentação: EstatisticaAbastecimentoCombustivel.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)

---

## Visão Geral

O Model `EstatisticaAbastecimentoCombustivel` representa estatísticas mensais de abastecimentos agrupadas por tipo de combustível. Inclui média de valor por litro calculada.

**Principais características:**

✅ **Agrupamento por Combustível**: Estatísticas por tipo (Gasolina, Diesel, etc.)  
✅ **Média de Preço**: Campo `MediaValorLitro` calculado  
✅ **Agregação Mensal**: Dados por mês/ano

---

## Estrutura do Model

```csharp
[Table("EstatisticaAbastecimentoCombustivel")]
public class EstatisticaAbastecimentoCombustivel
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    
    [StringLength(100)]
    public string TipoCombustivel { get; set; } = string.Empty;

    public int TotalAbastecimentos { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal LitrosTotal { get; set; }
    public decimal MediaValorLitro { get; set; }

    public DateTime DataAtualizacao { get; set; }
}
```

**Propriedades Especiais:**

- `MediaValorLitro`: Média do valor unitário do combustível no período

---

## Notas Importantes

1. **Média Calculada**: `MediaValorLitro = ValorTotal / LitrosTotal`
2. **Análise de Preços**: Usado para acompanhar variação de preços

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
