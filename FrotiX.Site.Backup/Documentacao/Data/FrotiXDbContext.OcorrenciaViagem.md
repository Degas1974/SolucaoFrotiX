# Documentação: FrotiXDbContext.OcorrenciaViagem.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O arquivo `FrotiXDbContext.OcorrenciaViagem.cs` é uma extensão parcial do `FrotiXDbContext` que adiciona DbSets relacionados a ocorrências de viagens e suas views otimizadas.

**Principais características:**

✅ **Partial Class**: Extensão do `FrotiXDbContext` principal  
✅ **Ocorrências de Viagem**: Entidade principal de ocorrências  
✅ **Views Otimizadas**: Views para consultas de ocorrências

---

## DbSets Adicionados

### `DbSet<OcorrenciaViagem> OcorrenciaViagem`

**Descrição**: Entidade principal de ocorrências de viagens

**Uso**: Armazena ocorrências registradas durante viagens (problemas, avarias, etc.)

**Relacionamentos**:
- Relacionado com `Viagem` (via `ViagemId`)
- Pode ter imagens anexadas
- Suporta status (Aberta, Fechada, etc.)

---

### `DbSet<ViewOcorrenciasViagem> ViewOcorrenciasViagem`

**Descrição**: View otimizada para listagem de ocorrências de viagens

**Uso**: Consultas rápidas com joins pré-calculados

**Vantagens**:
- Performance otimizada
- Dados agregados
- Filtros pré-aplicados

---

### `DbSet<ViewOcorrenciasAbertasVeiculo> ViewOcorrenciasAbertasVeiculo`

**Descrição**: View específica para ocorrências abertas por veículo

**Uso**: Dashboard de veículos com ocorrências pendentes

**Características**:
- Filtra apenas ocorrências com status "Aberta"
- Agrupa por veículo
- Inclui contadores e estatísticas

---

## Interconexões

### Quem Usa Estes DbSets

- **OcorrenciaViagemController**: CRUD de ocorrências
- **DashboardVeiculosController**: Estatísticas de ocorrências
- **ViagemController**: Exibição de ocorrências em viagens

### O Que Estes DbSets Usam

- **FrotiX.Models.OcorrenciaViagem**: Modelo principal
- **FrotiX.Models.Views**: Views otimizadas

---

## Casos de Uso

### 1. Listar Ocorrências de uma Viagem

```csharp
var ocorrencias = context.OcorrenciaViagem
    .Where(o => o.ViagemId == viagemId)
    .OrderByDescending(o => o.DataOcorrencia)
    .ToList();
```

### 2. Consultar Ocorrências Abertas por Veículo (View)

```csharp
var ocorrenciasAbertas = context.ViewOcorrenciasAbertasVeiculo
    .Where(v => v.VeiculoId == veiculoId)
    .ToList();
```

### 3. Contar Ocorrências Abertas

```csharp
var totalAbertas = context.ViewOcorrenciasAbertasVeiculo
    .Where(v => v.VeiculoId == veiculoId)
    .Count();
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do FrotiXDbContext.OcorrenciaViagem

**Arquivos Afetados**:
- `Data/FrotiXDbContext.OcorrenciaViagem.cs`

**Impacto**: Documentação de referência para ocorrências de viagens

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
