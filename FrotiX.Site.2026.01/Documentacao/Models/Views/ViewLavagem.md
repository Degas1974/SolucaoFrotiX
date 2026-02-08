# Documentação: ViewLavagem.cs

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

O Model `ViewLavagem` representa uma VIEW do banco de dados que consolida informações de lavagens de veículos com dados relacionados de motoristas, veículos e lavadores.

**Principais características:**

✅ **View do Banco**: Representa uma VIEW SQL  
✅ **Dados Consolidados**: Inclui informações de múltiplas tabelas  
✅ **Lavadores Múltiplos**: Campo `LavadoresId` e `Lavadores` (string concatenada)  
✅ **Duração**: Campo `DuracaoMinutos` calculado

---

## Estrutura do Model

```csharp
public class ViewLavagem
{
    public Guid LavagemId { get; set; }
    public Guid MotoristaId { get; set; }
    public Guid VeiculoId { get; set; }
    public string? LavadoresId { get; set; }      // IDs concatenados
    public string? Data { get; set; }             // Formatada
    public string? HorarioInicio { get; set; }    // Formatado
    public string? HorarioFim { get; set; }       // Formatado
    public int? DuracaoMinutos { get; set; }      // Calculado
    public string? Lavadores { get; set; }        // Nomes concatenados
    public string? DescricaoVeiculo { get; set; }
    public string? Nome { get; set; }             // Nome do motorista
}
```

**Propriedades Principais:**

- **Lavagem**: LavagemId, Data, HorarioInicio, HorarioFim, DuracaoMinutos
- **Lavadores**: LavadoresId (IDs), Lavadores (nomes)
- **Veículo**: VeiculoId, DescricaoVeiculo
- **Motorista**: MotoristaId, Nome

---

## Mapeamento Model ↔ Banco de Dados

### View: `ViewLavagem`

**Tipo**: VIEW (não é tabela)

**Tabelas Envolvidas**:
- `Lavagem` (tabela principal)
- `Motorista` (JOIN)
- `Veiculo` (JOIN)
- `LavadoresLavagem` (JOIN para múltiplos lavadores)

**Cálculos na View**:
- `DuracaoMinutos`: DATEDIFF(MINUTE, HorarioInicio, HorarioFim)
- `Lavadores`: STRING_AGG ou CONCAT de nomes dos lavadores

---

## Notas Importantes

1. **Lavadores Múltiplos**: Uma lavagem pode ter múltiplos lavadores
2. **Duração Calculada**: Campo DuracaoMinutos é calculado na view
3. **Datas Formatadas**: Data e horários vêm formatados como string

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
