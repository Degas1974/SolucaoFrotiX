# Documentação: EstatisticaAbastecimentoMensal.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)
3. [Mapeamento Model ↔ Banco de Dados](#mapeamento-model--banco-de-dados)
4. [Interconexões](#interconexões)
5. [Lógica de Negócio](#lógica-de-negócio)

---

## Visão Geral

O Model `EstatisticaAbastecimentoMensal` representa estatísticas consolidadas mensais de abastecimentos da frota, incluindo total de abastecimentos, valor total e litros totais. Tabela de agregação pré-calculada para dashboards.

**Principais características:**

✅ **Agregação Mensal**: Dados por mês/ano  
✅ **Métricas Consolidadas**: Total, valor, litros  
✅ **Performance**: Pré-calculado para consultas rápidas

---

## Estrutura do Model

```csharp
[Table("EstatisticaAbastecimentoMensal")]
public class EstatisticaAbastecimentoMensal
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    public int TotalAbastecimentos { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal LitrosTotal { get; set; }
    public DateTime DataAtualizacao { get; set; }
}
```

---

## Mapeamento Model ↔ Banco de Dados

### Tabela: `EstatisticaAbastecimentoMensal`

**Tipo**: Tabela de agregação

**Chaves e Índices**:
- **PK**: `Id` (CLUSTERED)
- **IX**: `IX_EstatisticaAbastecimentoMensal_Ano_Mes` (Ano, Mes)

---

## Interconexões

### Quem Chama Este Arquivo

Controllers de dashboard e relatórios de abastecimento consultam esta tabela para exibir estatísticas mensais.

---

## Lógica de Negócio

### Cálculo de Estatísticas

```csharp
var abastecimentos = _unitOfWork.Abastecimento
    .GetAll(a => a.DataHora.Value.Year == ano && a.DataHora.Value.Month == mes);

var estatistica = new EstatisticaAbastecimentoMensal
{
    Ano = ano,
    Mes = mes,
    TotalAbastecimentos = abastecimentos.Count(),
    ValorTotal = abastecimentos.Sum(a => (a.Litros * a.ValorUnitario) ?? 0),
    LitrosTotal = abastecimentos.Sum(a => a.Litros ?? 0),
    DataAtualizacao = DateTime.Now
};
```

---

## Notas Importantes

1. **Pré-calculado**: Dados calculados periodicamente
2. **Performance**: Consultas muito mais rápidas que agregar em tempo real

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
