# Documentação: EstatisticaAbastecimentoCategoria.cs

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

O Model `EstatisticaAbastecimentoCategoria` representa estatísticas mensais de abastecimentos agrupadas por categoria de veículo. Permite análise de consumo e custos por tipo de veículo (ex: Sedan, SUV, Caminhão).

**Principais características:**

✅ **Agrupamento por Categoria**: Estatísticas por categoria de veículo  
✅ **Agregação Mensal**: Dados por mês/ano  
✅ **Métricas Consolidadas**: Total, valor, litros

---

## Estrutura do Model

```csharp
[Table("EstatisticaAbastecimentoCategoria")]
public class EstatisticaAbastecimentoCategoria
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    
    [StringLength(100)]
    public string Categoria { get; set; } = string.Empty;

    public int TotalAbastecimentos { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal LitrosTotal { get; set; }

    public DateTime DataAtualizacao { get; set; }
}
```

**Propriedades:**

- `Categoria` (string): Categoria do veículo (ex: "Sedan", "SUV", "Caminhão")
- `TotalAbastecimentos`: Quantidade de abastecimentos na categoria
- `ValorTotal`: Valor total gasto
- `LitrosTotal`: Total de litros abastecidos

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `EstatisticaAbastecimentoCategoria`

**Tipo**: Tabela de agregação

**Chaves e Índices**:
- **PK**: `Id` (CLUSTERED)
- **IX**: `IX_EstatisticaAbastecimentoCategoria_Ano_Mes_Categoria` (Ano, Mes, Categoria)

---

## Interconexões

Controllers de dashboard e relatórios consultam esta tabela para análises por categoria de veículo.

---

## Notas Importantes

1. **Agrupamento**: Dados agrupados por categoria do veículo
2. **Performance**: Pré-calculado para consultas rápidas

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
