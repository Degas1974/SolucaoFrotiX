# Documentação: RepactuacaoVeiculo.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `RepactuacaoVeiculo` registra valores individuais de cada veículo durante repactuação de contrato de locação. Permite ajustar valores por veículo específico.

## Estrutura do Model

```csharp
public class RepactuacaoVeiculo
{
    [Key]
    public Guid RepactuacaoVeiculoId { get; set; }

    [Display(Name = "Repactuação")]
    public Guid RepactuacaoContratoId { get; set; }
    [ForeignKey("RepactuacaoContratoId")]
    public virtual RepactuacaoContrato RepactuacaoContrato { get; set; }

    [Display(Name = "Veículo")]
    public Guid VeiculoId { get; set; }
    [ForeignKey("VeiculoId")]
    public virtual Veiculo Veiculo { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Valor (R$)")]
    public double? Valor { get; set; }

    [Display(Name = "Observação")]
    public string? Observacao { get; set; }
}
```

## Interconexões

Controllers de repactuação de contratos usam para ajustar valores por veículo.

## Notas Importantes

1. **Repactuação**: Vinculado a RepactuacaoContrato
2. **Valor Individual**: Permite ajustar valor por veículo específico

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
