# Documentação: EmpenhoMulta.cs

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

O Model `EmpenhoMulta` representa empenhos específicos para pagamento de multas, vinculados a órgãos autuantes. Controla saldos disponíveis para pagamento de multas de trânsito.

**Principais características:**

✅ **Empenho Específico**: Dedicado a multas  
✅ **Órgão Autuante**: Vinculado a órgão específico  
✅ **Saldo Atual**: Campo SaldoAtual (diferente de SaldoFinal)

---

## Estrutura do Model

```csharp
public class EmpenhoMulta
{
    [Key]
    public Guid EmpenhoMultaId { get; set; }

    [Required(ErrorMessage = "(A nota de Empenho é obrigatória)")]
    [MinLength(12), MaxLength(12)]
    [Display(Name = "Nota de Empenho")]
    public string? NotaEmpenho { get; set; }

    [Required(ErrorMessage = "(O ano de vigência é obrigatório)")]
    [Display(Name = "Ano de Vigência")]
    public int? AnoVigencia { get; set; }

    [ValidaZero(ErrorMessage = "(O saldo inicial é obrigatório)")]
    [Required(ErrorMessage = "(O saldo inicial é obrigatório)")]
    [DataType(DataType.Currency)]
    [Display(Name = "Saldo Inicial (R$)")]
    public double? SaldoInicial { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Saldo Atual (R$)")]
    public double? SaldoAtual { get; set; }

    public bool Status { get; set; }

    [Display(Name = "Órgão Autuante")]
    public Guid OrgaoAutuanteId { get; set; }

    [ForeignKey("OrgaoAutuanteId")]
    public virtual OrgaoAutuante OrgaoAutuante { get; set; }
}
```

**Propriedades Principais:**

- `EmpenhoMultaId` (Guid): Chave primária
- `NotaEmpenho` (string): Número da nota (12 caracteres)
- `AnoVigencia` (int?): Ano de vigência
- `SaldoInicial` (double?): Saldo inicial
- `SaldoAtual` (double?): Saldo atual (atualizado conforme multas pagas)
- `Status` (bool): Ativo/Inativo
- `OrgaoAutuanteId` (Guid): FK para OrgaoAutuante (obrigatório)

---

## Notas Importantes

1. **Específico para Multas**: Diferente de `Empenho` geral
2. **Órgão Obrigatório**: Sempre vinculado a um órgão autuante
3. **Saldo Atual**: Atualizado conforme multas são pagas

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
