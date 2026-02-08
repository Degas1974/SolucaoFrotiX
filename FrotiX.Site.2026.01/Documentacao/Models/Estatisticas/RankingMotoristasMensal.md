# Documentação: RankingMotoristasMensal.cs

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

O Model `RankingMotoristasMensal` representa rankings mensais de motoristas em diferentes categorias (viagens, KM, horas, abastecimentos, multas, performance). Permite identificar top performers e análises comparativas.

**Principais características:**

✅ **Múltiplos Rankings**: Diferentes tipos de ranking  
✅ **Posicionamento**: Campo `Posicao` para ordenação  
✅ **Valores Múltiplos**: Suporta valores principais e secundários  
✅ **Agregação Mensal**: Dados consolidados por mês

---

## Estrutura do Model

```csharp
[Table("RankingMotoristasMensal")]
public class RankingMotoristasMensal
{
    [Key]
    public Guid Id { get; set; }

    public int Ano { get; set; }
    public int Mes { get; set; }
    
    [StringLength(50)]
    public string TipoRanking { get; set; } // 'VIAGENS', 'KM', 'HORAS', 'ABASTECIMENTOS', 'MULTAS', 'PERFORMANCE'
    
    public int Posicao { get; set; }
    public Guid MotoristaId { get; set; }
    
    [StringLength(200)]
    public string NomeMotorista { get; set; }
    
    [StringLength(50)]
    public string TipoMotorista { get; set; } // Efetivo/Ferista/Cobertura

    // Valores conforme o tipo de ranking
    public decimal ValorPrincipal { get; set; } // Viagens/KM/Horas/etc
    public decimal ValorSecundario { get; set; } // KM (para performance), Valor (para multas)
    public decimal ValorTerciario { get; set; } // Horas (para performance)
    public int ValorQuaternario { get; set; } // Multas (para performance)

    public DateTime DataAtualizacao { get; set; }

    [ForeignKey("MotoristaId")]
    public virtual Motorista Motorista { get; set; }
}
```

**Tipos de Ranking:**

- `VIAGENS`: Ranking por quantidade de viagens
- `KM`: Ranking por quilometragem total
- `HORAS`: Ranking por horas trabalhadas
- `ABASTECIMENTOS`: Ranking por quantidade de abastecimentos
- `MULTAS`: Ranking por quantidade/valor de multas
- `PERFORMANCE`: Ranking composto (múltiplos valores)

---

## Lógica de Negócio

### Cálculo de Rankings

Rankings são calculados ordenando motoristas por métrica específica:

```csharp
// Exemplo: Ranking por KM
var motoristas = _unitOfWork.Viagem
    .GetAll(v => v.DataInicial.Year == ano && v.DataInicial.Month == mes)
    .GroupBy(v => v.MotoristaId)
    .Select(g => new {
        MotoristaId = g.Key,
        KmTotal = g.Sum(v => v.KmRodado ?? 0)
    })
    .OrderByDescending(x => x.KmTotal)
    .Take(10) // Top 10
    .ToList();

int posicao = 1;
foreach (var item in motoristas)
{
    var ranking = new RankingMotoristasMensal
    {
        TipoRanking = "KM",
        Posicao = posicao++,
        MotoristaId = item.MotoristaId,
        ValorPrincipal = item.KmTotal
    };
    // Salvar
}
```

---

## Notas Importantes

1. **Múltiplos Valores**: `ValorPrincipal`, `ValorSecundario`, etc. variam conforme tipo
2. **Posição**: Campo explícito para facilitar consultas ordenadas
3. **Top N**: Geralmente armazena apenas top 10 ou top 20

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
