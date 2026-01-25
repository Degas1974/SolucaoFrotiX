# Documentação: EmpenhoRepository.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `EmpenhoRepository` é um repository específico para a entidade `Empenho`, com método de dropdown que faz JOIN com Contrato para exibir informações completas.

**Principais características:**

✅ **Herança**: Herda de `Repository<Empenho>`  
✅ **Interface Específica**: Implementa `IEmpenhoRepository`  
✅ **JOIN com Contrato**: Dropdown inclui informações do contrato relacionado

---

## Métodos Específicos

### `GetEmpenhoListForDropDown()`

**Descrição**: Retorna lista de empenhos formatada para DropDownList com JOIN em Contrato

**JOIN**: Faz `Join` entre `Empenho` e `Contrato` via `ContratoId`

**Ordenação**: Por `NotaEmpenho`

**Formato**: `"{NotaEmpenho}({AnoContrato}/{NumeroContrato})"` como texto, `ContratoId` como valor

**Nota**: ⚠️ Retorna `ContratoId` como valor, não `EmpenhoId`

**Uso**:
```csharp
var empenhos = unitOfWork.Empenho.GetEmpenhoListForDropDown();
// Retorna: "12345(2026/001)" como texto, ContratoId como valor
```

---

### `Update(Empenho empenho)`

**Descrição**: Atualiza empenho com lógica específica

**Nota**: ⚠️ Chama `SaveChanges()` diretamente (inconsistente com padrão)

---

## Interconexões

### Quem Usa Este Repository

- **EmpenhoController**: CRUD de empenhos
- **NotaFiscalController**: Para seleção de empenhos em notas fiscais

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do EmpenhoRepository

**Arquivos Afetados**:
- `Repository/EmpenhoRepository.cs`

**Impacto**: Documentação de referência para repository de empenhos

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
