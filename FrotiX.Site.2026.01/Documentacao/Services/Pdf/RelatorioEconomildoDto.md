# Documentação: RelatorioEconomildoDto.cs

> **Última Atualização**: 14/01/2026
> **Versão Atual**: 1.1

---

## PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

Arquivo contendo todas as **classes DTO** (Data Transfer Objects) usadas na geração de PDFs do Dashboard Economildo. Define a estrutura de dados passada para o `RelatorioEconomildoPdfService`.

## Localização

`Services/Pdf/RelatorioEconomildoDto.cs`

---

## Classes e Enumerações

### `TipoRelatorioEconomildo` (enum)

Tipos de relatórios disponíveis:

| Valor | Descrição |
|-------|-----------|
| `UsuariosMes` | Gráfico de usuários por mês |
| `UsuariosTurno` | Gráfico de pizza por turno |
| `ComparativoMob` | Comparativo entre MOBs |
| `UsuariosDiaSemana` | Usuários por dia da semana |
| `DistribuicaoHorario` | Distribuição por horário |
| `TopVeiculos` | Ranking top 10 veículos |
| `HeatmapViagens` | Mapa de calor de viagens |
| `HeatmapPassageiros` | Mapa de calor de passageiros |

---

### `FiltroEconomildoDto`

Classe principal de filtros compartilhada por todos os relatórios.

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Mob` | `string?` | MOB selecionado (PGR, Rodoviária, Cefor) ou null para todos |
| `Mes` | `int?` | Mês (1-12) ou null para todos |
| `Ano` | `int?` | Ano (ex: 2026) |

**Propriedades Calculadas**:

| Propriedade | Descrição |
|-------------|-----------|
| `DataInicio` | Data inicial do período baseada nos filtros |
| `DataFim` | Data final do período baseada nos filtros |
| `PeriodoFormatado` | String formatada "DD/MM/YYYY a DD/MM/YYYY" |
| `NomeVeiculo` | "Economildo - Todos" ou "Economildo {MOB}" |

**Lógica de Cálculo do Período**:

```csharp
// DataInicio
Ano + Mes definidos → Primeiro dia do mês
Apenas Ano definido → 01/01 do ano
Nenhum definido → Primeiro dia do mês atual

// DataFim
Ano + Mes definidos → Último dia do mês
Apenas Ano definido → Se ano atual: data atual; Senão: 31/12
Nenhum definido → Data atual
```

---

### `HeatmapDto`

DTO para mapas de calor (viagens e passageiros).

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Titulo` | `string` | Título do relatório |
| `Subtitulo` | `string` | Subtítulo descritivo |
| `UnidadeLegenda` | `string` | "viagens" ou "passageiros" |
| `Valores` | `int[7,24]` | Matriz 7 dias × 24 horas |
| `ValorMaximo` | `int` | Maior valor na matriz |
| `DiaPico` | `string` | Dia com mais atividade |
| `HoraPico` | `int` | Hora com mais atividade |
| `HorarioPicoManha` | `string` | Faixa de pico pela manhã |
| `DiaMaisMovimentado` | `string` | Nome do dia mais movimentado |
| `PeriodoOperacao` | `string` | Período de operação "XXh - YYh" |
| `Filtro` | `FiltroEconomildoDto` | Filtros aplicados |

---

### `GraficoBarrasDto`

DTO para gráficos de barras (vertical/horizontal).

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Titulo` | `string` | Título do gráfico |
| `Subtitulo` | `string` | Subtítulo |
| `EixoX` | `string` | Label do eixo X |
| `EixoY` | `string` | Label do eixo Y |
| `Dados` | `List<ItemGraficoDto>` | Lista de itens |
| `Filtro` | `FiltroEconomildoDto` | Filtros aplicados |
| `Total` | `int` | Soma de todos os valores (calculado) |

---

### `ItemGraficoDto`

Item individual de um gráfico.

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Label` | `string` | Rótulo (ex: "Jan", "Seg") |
| `Valor` | `int` | Valor numérico |
| `Cor` | `string?` | Cor opcional em hex |
| `Percentual` | `double` | Percentual do total |

---

### `GraficoPizzaDto`

DTO para gráficos de pizza/rosca.

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Titulo` | `string` | Título do gráfico |
| `Subtitulo` | `string` | Subtítulo |
| `Dados` | `List<ItemGraficoDto>` | Fatias da pizza |
| `Filtro` | `FiltroEconomildoDto` | Filtros aplicados |
| `Total` | `int` | Soma de todos os valores (calculado) |

---

### `GraficoComparativoDto`

DTO para gráfico comparativo por MOB.

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Titulo` | `string` | Título do gráfico |
| `Subtitulo` | `string` | Subtítulo |
| `Labels` | `List<string>` | Labels do eixo X (meses) |
| `Series` | `List<SerieGraficoDto>` | Séries (PGR, Rodoviária, Cefor) |
| `Filtro` | `FiltroEconomildoDto` | Filtros aplicados |

---

### `SerieGraficoDto`

Série de dados para gráfico comparativo.

**Propriedades**:

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `Nome` | `string` | Nome da série (ex: "PGR") |
| `Cor` | `string` | Cor em hex |
| `Valores` | `List<int>` | Valores por mês |
| `Total` | `int` | Soma dos valores (calculado) |

---

## Arquivos Relacionados

- `Services/Pdf/RelatorioEconomildoPdfService.cs`: Serviço que consome os DTOs
- `Controllers/RelatoriosController.cs`: Controller que monta os DTOs
- `Pages/Frota/DashboardEconomildo.cshtml`: Frontend que chama exportação

---

## PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

> **FORMATO**: Entradas em ordem **decrescente** (mais recente primeiro)

---

## [14/01/2026] - Correção do cálculo de DataFim

**Descrição**:

- Corrigida lógica de `DataFim` em `FiltroEconomildoDto`
- **Problema**: Quando não havia mês selecionado, sempre mostrava até 31/12, mesmo para o ano atual
- **Solução**: Agora verifica se é o ano corrente e mostra até a data atual

**Arquivos Afetados**:

- `Services/Pdf/RelatorioEconomildoDto.cs` (linhas 37-42)

**Código Alterado**:

```csharp
// ANTES
public DateTime DataFim => Ano.HasValue && Mes.HasValue
    ? new DateTime(Ano.Value, Mes.Value, 1).AddMonths(1).AddDays(-1)
    : Ano.HasValue
        ? new DateTime(Ano.Value, 12, 31)
        : DateTime.Now;

// DEPOIS
public DateTime DataFim => Ano.HasValue && Mes.HasValue
    ? new DateTime(Ano.Value, Mes.Value, 1).AddMonths(1).AddDays(-1)
    : Ano.HasValue
        // Se é o ano atual e não tem mês, mostra até hoje; senão até 31/12
        ? (Ano.Value == DateTime.Now.Year ? DateTime.Now : new DateTime(Ano.Value, 12, 31))
        : DateTime.Now;
```

**Impacto**: PDFs agora mostram período correto (ex: "01/01/2026 a 14/01/2026" em vez de "01/01/2026 a 31/12/2026")

**Status**: Concluído

**Versão**: 1.1

---

**Última atualização**: 14/01/2026
**Autor**: Sistema FrotiX
**Versão**: 1.1


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
