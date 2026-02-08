# Documentação: ViewItensManutencao.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Índice
1. [Visão Geral](#visão-geral)
2. [Estrutura do Model](#estrutura-do-model)

---

## Visão Geral

O Model `ViewItensManutencao` representa uma VIEW do banco de dados que consolida informações de itens de manutenção (ocorrências convertidas em itens de manutenção) com dados relacionados de manutenções, motoristas e viagens.

---

## Estrutura do Model

```csharp
public class ViewItensManutencao
{
    public Guid ItemManutencaoId { get; set; }
    public Guid ManutencaoId { get; set; }
    public string? TipoItem { get; set; }
    public string? NumFicha { get; set; }
    public string? DataItem { get; set; }        // Formatada
    public string? Resumo { get; set; }
    public string? Descricao { get; set; }
    public string? Status { get; set; }
    public string? ImagemOcorrencia { get; set; }
    public string? NomeMotorista { get; set; }
    public Guid? MotoristaId { get; set; }
    public Guid? ViagemId { get; set; }
}
```

**Propriedades Principais:**

- **Item**: ItemManutencaoId, TipoItem, NumFicha, Status
- **Manutenção**: ManutencaoId
- **Ocorrência**: Resumo, Descricao, ImagemOcorrencia
- **Viagem**: ViagemId, NumFicha
- **Motorista**: MotoristaId, NomeMotorista

---

## Notas Importantes

1. **Conversão de Ocorrências**: Itens podem vir de ocorrências de viagem
2. **TipoItem**: Indica tipo do item de manutenção

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
