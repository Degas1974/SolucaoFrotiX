# Documentação: NotaFiscalController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `NotaFiscalController` gerencia operações CRUD de notas fiscais, incluindo gestão de glosas e integração com empenhos.

**Principais características:**

✅ **CRUD Completo**: Exclusão e gestão de glosas  
✅ **Integração com Empenhos**: Atualiza saldo de empenho ao excluir NF  
✅ **Glosas**: Sistema de gestão de glosas em notas fiscais

**Nota**: Controller implementado como partial class dividido em múltiplos arquivos.

---

## Endpoints API Principais

### POST `/api/NotaFiscal/Delete`

**Descrição**: Exclui nota fiscal e atualiza saldo do empenho

**Lógica**:
- Ao excluir NF, devolve valor líquido ao empenho: `SaldoFinal = SaldoFinal + (ValorNF - ValorGlosa)`

---

### GET `/api/NotaFiscal/GetGlosa`

**Descrição**: Obtém dados de glosa de uma nota fiscal

**Parâmetros**: `id` (Guid) - ID da nota fiscal

**Response**: Dados da glosa incluindo valor e motivo

---

### POST `/api/NotaFiscal/Glosa`

**Descrição**: Aplica glosa em nota fiscal

**Request Body**: `GlosaNota` com `NotaFiscalId`, `ValorGlosa`, `MotivoGlosa`

**Lógica**: Atualiza `ValorGlosa` e `MotivoGlosa` da NF e ajusta saldo do empenho

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/NotaFiscal/*.cshtml`
- **Pages**: `Pages/Empenho/*.cshtml` - Para gestão de NFs de empenhos

### O Que Este Controller Chama

- **`_unitOfWork.NotaFiscal`**: CRUD
- **`_unitOfWork.Empenho`**: Atualização de saldo

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do NotaFiscalController

**Arquivos Afetados**:
- `Controllers/NotaFiscalController.cs`
- `Controllers/NotaFiscalController.Partial.cs`

**Impacto**: Documentação de referência para operações de notas fiscais

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
