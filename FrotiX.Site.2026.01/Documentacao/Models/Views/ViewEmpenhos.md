# Documentação: ViewEmpenhos.cs

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

O Model `ViewEmpenhos` representa uma VIEW do banco de dados que consolida informações de empenhos com saldos calculados (inicial, final, movimentação, notas) e contagem de movimentações.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Saldos Calculados**: SaldoInicial, SaldoFinal, SaldoMovimentacao, SaldoNotas  
✅ **Contagem**: Campo Movimentacoes com quantidade

---

## Estrutura do Model

```csharp
public class ViewEmpenhos
{
    [Key]
    public Guid EmpenhoId { get; set; }

    public string? NotaEmpenho { get; set; }
    public DateTime? DataEmissao { get; set; }
    public int? AnoVigencia { get; set; }
    public DateTime? VigenciaInicial { get; set; }
    public DateTime? VigenciaFinal { get; set; }

    // Saldos calculados
    public double? SaldoInicial { get; set; }
    public double? SaldoFinal { get; set; }
    public double? SaldoMovimentacao { get; set; }
    public double? SaldoNotas { get; set; }

    public int? Movimentacoes { get; set; }  // Contagem

    // GUIDs vazios em vez de NULL
    public Guid ContratoId { get; set; }
    public Guid AtaId { get; set; }
}
```

**Propriedades Principais:**

- **Empenho**: EmpenhoId, NotaEmpenho, DataEmissao, AnoVigencia
- **Vigência**: VigenciaInicial, VigenciaFinal
- **Saldos**: SaldoInicial, SaldoFinal, SaldoMovimentacao, SaldoNotas
- **Contagem**: Movimentacoes (quantidade de movimentações)

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewEmpenhos`

**Tipo**: VIEW (não é tabela)

**Cálculos na View**:
- `SaldoFinal`: SaldoInicial - Movimentacoes - Notas
- `SaldoMovimentacao`: Soma das movimentações
- `SaldoNotas`: Soma das notas fiscais
- `Movimentacoes`: COUNT de movimentações

**Nota Especial**: View usa `ISNULL` para garantir GUID vazio (`00000000-0000-0000-0000-000000000000`) em vez de NULL nos campos ContratoId e AtaId.

---

## Notas Importantes

1. **Saldos Calculados**: Todos os saldos são calculados na view
2. **GUID Vazio**: ContratoId e AtaId usam GUID vazio em vez de NULL
3. **Performance**: View otimizada para consultas de saldos

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
