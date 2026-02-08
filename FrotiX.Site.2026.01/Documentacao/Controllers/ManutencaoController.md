# Documentação: ManutencaoController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `ManutencaoController` gerencia operações CRUD de manutenções de veículos, incluindo filtros avançados por veículo, status, mês/ano e período.

**Principais características:**

✅ **CRUD Completo**: Listagem com filtros avançados  
✅ **Filtros Múltiplos**: Por veículo, status, mês/ano, período  
✅ **Cache**: Usa `IMemoryCache` para otimização  
✅ **Upload**: Upload de arquivos relacionados

---

## Endpoints API Principais

### GET `/api/Manutencao`

**Descrição**: Lista manutenções com filtros múltiplos

**Parâmetros**:
- `veiculoId` (string GUID opcional)
- `statusId` (string opcional)
- `mes` (string opcional)
- `ano` (string opcional)
- `dataInicial` (string opcional) - Formatos: dd/MM/yyyy, yyyy-MM-dd, etc.
- `dataFinal` (string opcional)

**Filtros**: Usa `ViewManutencao` com filtro por `DataSolicitacaoRaw`

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Manutencao/*.cshtml`
- **Pages**: `Pages/Veiculo/*.cshtml` - Para histórico de manutenções

### O Que Este Controller Chama

- **`_unitOfWork.ViewManutencao`**: View otimizada
- **`_unitOfWork.Manutencao`**: CRUD
- **`_cache`**: Cache de memória

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do ManutencaoController

**Arquivos Afetados**:
- `Controllers/ManutencaoController.cs`

**Impacto**: Documentação de referência para operações de manutenções

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
