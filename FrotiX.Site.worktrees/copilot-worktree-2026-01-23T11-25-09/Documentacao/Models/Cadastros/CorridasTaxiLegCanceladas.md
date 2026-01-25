# Documentação: CorridasTaxiLegCanceladas.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `CorridasTaxiLegCanceladas` representa corridas de táxi legislativo que foram canceladas. Registra motivo e tipo de cancelamento.

## Estrutura do Model

```csharp
public class CorridasCanceladasTaxiLeg
{
    [Key]
    public Guid CorridaCanceladaId { get; set; }

    public string? Origem { get; set; }
    public string? Setor { get; set; }
    public string? SetorExtra { get; set; }
    public string? Unidade { get; set; }
    public string? UnidadeExtra { get; set; }
    public int? QtdPassageiros { get; set; }
    public string? MotivoUso { get; set; }

    public DateTime? DataAgenda { get; set; }
    public string? HoraAgenda { get; set; }
    public DateTime? DataHoraCancelamento { get; set; }
    public string? HoraCancelamento { get; set; }
    public string? TipoCancelamento { get; set; }
    public string? MotivoCancelamento { get; set; }
    public int? TempoEspera { get; set; }
}
```

## Notas Importantes

1. **Cancelamentos**: Registra corridas canceladas
2. **TipoCancelamento**: Indica tipo de cancelamento
3. **TempoEspera**: Tempo de espera antes do cancelamento

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
