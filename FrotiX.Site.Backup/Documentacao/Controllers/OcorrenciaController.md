# Documentação: OcorrenciaController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `OcorrenciaController` gerencia operações de ocorrências gerais (não específicas de viagem), incluindo listagem com filtros múltiplos, baixa e edição.

**Principais características:**

✅ **Listagem com Filtros**: Por veículo, motorista, status, data  
✅ **Baixa**: Sistema de baixa de ocorrências  
✅ **Edição**: Edição de ocorrências com upload de imagens  
✅ **Filtros Avançados**: Suporta múltiplos formatos de data

---

## Endpoints API Principais

### GET `/api/Ocorrencia`

**Descrição**: Lista ocorrências com filtros múltiplos

**Parâmetros**:
- `veiculoId` (string GUID opcional)
- `motoristaId` (string GUID opcional)
- `statusId` (string opcional)
- `data` (string opcional) - Data única
- `dataInicial` (string opcional) - Início do período
- `dataFinal` (string opcional) - Fim do período
- `debug` (string, default: "0")

**Filtros**: Filtra apenas viagens com `ResumoOcorrencia` preenchido

---

### POST `/api/Ocorrencia/BaixarOcorrencia`

**Descrição**: Dá baixa em ocorrência

**Request Body**: `ViagemID` com `ViagemId`

---

### POST `/api/Ocorrencia/EditaOcorrencia`

**Descrição**: Edita ocorrência com upload de imagem

**Request Body**: `FinalizacaoViagem` com dados da ocorrência

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Ocorrencia/*.cshtml`
- **Pages**: Páginas de gestão de ocorrências

### O Que Este Controller Chama

- **`_unitOfWork.ViewViagens`**: View otimizada com filtro de ocorrências
- **`_unitOfWork.Viagem`**: CRUD de viagens/ocorrências

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do OcorrenciaController

**Arquivos Afetados**:
- `Controllers/OcorrenciaController.cs`

**Impacto**: Documentação de referência para operações de ocorrências gerais

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
