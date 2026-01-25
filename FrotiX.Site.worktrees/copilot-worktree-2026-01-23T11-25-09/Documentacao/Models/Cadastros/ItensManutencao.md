# Documentação: ItensManutencao.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O Model `ItensManutencao` representa itens individuais de uma ordem de serviço de manutenção. Podem ser criados a partir de ocorrências de viagem ou diretamente na manutenção.

**Principais características:**

✅ **Itens de OS**: Itens individuais de uma ordem de serviço  
✅ **Origem de Ocorrências**: Podem vir de ocorrências de viagem  
✅ **Status**: Controle de status do item  
✅ **Imagem**: Suporte a imagem da ocorrência

## Estrutura do Model

```csharp
public class ItensManutencao
{
    [Key]
    public Guid ItemManutencaoId { get; set; }

    public string? TipoItem { get; set; }
    public string? NumFicha { get; set; }
    public DateTime? DataItem { get; set; }
    public string? Resumo { get; set; }
    public string? Descricao { get; set; }
    public string? Status { get; set; }
    public string? ImagemOcorrencia { get; set; }

    public Guid? ManutencaoId { get; set; }
    [ForeignKey("ManutencaoId")]
    public virtual Manutencao Manutencao { get; set; }

    public Guid? MotoristaId { get; set; }
    [ForeignKey("MotoristaId")]
    public virtual Motorista Motorista { get; set; }

    public Guid? ViagemId { get; set; }
    [ForeignKey("ViagemId")]
    public virtual Viagem Viagem { get; set; }

    [NotMapped]
    public string NumOS { get; set; }

    [NotMapped]
    public string DataOS { get; set; }
}
```

## Interconexões

Controllers de manutenção usam para gerenciar itens de OS.

## Notas Importantes

1. **Origem**: Item pode vir de ocorrência de viagem (ViagemId) ou ser criado diretamente
2. **TipoItem**: Indica tipo do item (ex: "Ocorrência", "Manutenção Preventiva")

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
