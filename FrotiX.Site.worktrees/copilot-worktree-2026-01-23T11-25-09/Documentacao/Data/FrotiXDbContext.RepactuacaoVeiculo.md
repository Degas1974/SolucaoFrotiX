# Documentação: FrotiXDbContext.RepactuacaoVeiculo.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O arquivo `FrotiXDbContext.RepactuacaoVeiculo.cs` é uma extensão parcial do `FrotiXDbContext` que adiciona o DbSet para repactuações de veículos.

**Principais características:**

✅ **Partial Class**: Extensão do `FrotiXDbContext` principal  
✅ **Repactuações**: Entidade para repactuações de veículos em contratos

---

## DbSet Adicionado

### `DbSet<RepactuacaoVeiculo> RepactuacaoVeiculo`

**Descrição**: Entidade de repactuações de veículos

**Uso**: Armazena informações sobre repactuações de veículos em contratos ou atas

**Contexto de Negócio**:
- Quando um contrato ou ATA é repactuado, os valores dos veículos podem mudar
- Esta entidade registra essas mudanças ao longo do tempo
- Permite histórico de valores e condições

**Relacionamentos**:
- Relacionado com `Contrato` ou `AtaRegistroPrecos`
- Relacionado com `Veiculo`
- Pode ter múltiplos itens (`RepactuacaoContrato`, `RepactuacaoAta`)

---

## Interconexões

### Quem Usa Este DbSet

- **ContratoController**: Ao repactuar contratos
- **AtaRegistroPrecosController**: Ao repactuar atas
- **Relatórios**: Para histórico de repactuações

### O Que Este DbSet Usa

- **FrotiX.Models.RepactuacaoVeiculo**: Modelo de repactuação

---

## Casos de Uso

### 1. Listar Repactuações de um Veículo

```csharp
var repactuacoes = context.RepactuacaoVeiculo
    .Where(r => r.VeiculoId == veiculoId)
    .OrderByDescending(r => r.DataRepactuacao)
    .ToList();
```

### 2. Obter Última Repactuação

```csharp
var ultimaRepactuacao = context.RepactuacaoVeiculo
    .Where(r => r.VeiculoId == veiculoId)
    .OrderByDescending(r => r.DataRepactuacao)
    .FirstOrDefault();
```

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do FrotiXDbContext.RepactuacaoVeiculo

**Arquivos Afetados**:
- `Data/FrotiXDbContext.RepactuacaoVeiculo.cs`

**Impacto**: Documentação de referência para repactuações

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
