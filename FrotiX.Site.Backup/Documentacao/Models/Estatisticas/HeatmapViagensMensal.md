# Documentação: HeatmapViagensMensal.cs

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

O Model `HeatmapViagensMensal` representa dados para criação de heatmaps (mapas de calor) de viagens, mostrando distribuição de viagens por dia da semana e hora do dia. Permite visualizar padrões de uso da frota.

**Principais características:**

✅ **Heatmap**: Dados para visualização de calor  
✅ **Distribuição Temporal**: Por dia da semana e hora  
✅ **Filtro por Motorista**: Pode ser geral ou por motorista específico  
✅ **Agregação Mensal**: Dados consolidados por mês

---

## Estrutura do Model

```csharp
[Table("HeatmapViagensMensal")]
public class HeatmapViagensMensal
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    public Guid? MotoristaId { get; set; } // NULL = todos os motoristas
    public int DiaSemana { get; set; } // 0=Domingo, 1=Segunda, ... 6=Sábado
    public int Hora { get; set; } // 0-23
    public int TotalViagens { get; set; }

    public DateTime DataAtualizacao { get; set; }

    [ForeignKey("MotoristaId")]
    public virtual Motorista Motorista { get; set; }
}
```

**Propriedades:**

- `DiaSemana`: 0=Domingo, 1=Segunda, 2=Terça, ..., 6=Sábado
- `Hora`: 0-23 (hora do dia)
- `MotoristaId`: NULL = todos os motoristas, Guid = motorista específico

---

## Lógica de Negócio

### Estrutura de Heatmap

Dados são organizados em matriz:
- **Linhas**: Dias da semana (0-6)
- **Colunas**: Horas do dia (0-23)
- **Valores**: Total de viagens naquela célula

---

## Notas Importantes

1. **Visualização**: Usado para criar heatmaps visuais
2. **Padrões**: Identifica horários e dias de maior movimento
3. **Motorista NULL**: Representa dados gerais de todos os motoristas

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
