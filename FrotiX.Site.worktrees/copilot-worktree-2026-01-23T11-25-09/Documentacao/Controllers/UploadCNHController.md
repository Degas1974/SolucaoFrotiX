# Documentação: UploadCNHController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `UploadCNHController` gerencia upload e remoção de CNH digital de motoristas usando Syncfusion Uploader.

**Principais características:**

✅ **Upload**: Upload de CNH digital para motorista  
✅ **Remoção**: Remove CNH digital do motorista  
✅ **Syncfusion Uploader**: Integração com componente Syncfusion

---

## Endpoints API

### POST `/api/UploadCNH/Save`

**Descrição**: Salva CNH digital de um motorista

**Request**: `multipart/form-data` com arquivo e `motoristaId` (query)

**Lógica**: Converte arquivo para byte array e salva em `Motorista.CNHDigital`

---

### POST `/api/UploadCNH/Remove`

**Descrição**: Remove CNH digital de um motorista

**Parâmetros**: `motoristaId` (Guid, query)

**Lógica**: Define `Motorista.CNHDigital = null`

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Motorista/*.cshtml` - Upload de CNH
- **Syncfusion Uploader**: Componente de upload

### O Que Este Controller Chama

- **`_unitOfWork.Motorista`**: Atualização de CNH digital

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do UploadCNHController

**Arquivos Afetados**:
- `Controllers/UploadCNHController.cs`

**Impacto**: Documentação de referência para upload de CNH

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
