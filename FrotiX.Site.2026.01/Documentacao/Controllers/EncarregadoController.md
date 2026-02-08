# Documentação: EncarregadoController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `EncarregadoController` gerencia operações CRUD de encarregados, incluindo gestão de fotos e relacionamentos com contratos.

**Principais características:**

✅ **CRUD Completo**: Listagem, exclusão e atualização de status  
✅ **Gestão de Fotos**: Upload e recuperação de fotos em Base64  
✅ **Validação de Dependências**: Verifica contratos antes de excluir  
✅ **Relacionamentos**: Gerencia encarregados por contrato

---

## Endpoints API

### GET `/api/Encarregado`

**Descrição**: Retorna lista de encarregados com informações de contratos e usuário de alteração

**Response**:
```json
{
  "data": [
    {
      "encarregadoId": "guid",
      "nome": "João Silva",
      "ponto": "PONTO_01",
      "celular01": "11999999999",
      "contratoEncarregado": "2024/001 - Empresa XYZ",
      "status": true,
      "foto": null,
      "datadeAlteracao": "08/01/26",
      "nomeCompleto": "João da Silva"
    }
  ]
}
```

**Quando é chamado**: Pela página `Pages/Encarregado/Index.cshtml`

---

### POST `/api/Encarregado/Delete`

**Descrição**: Exclui encarregado com validação de dependências

**Validações**: Verifica se encarregado está associado a contratos (`EncarregadoContrato`)

---

### GET `/api/Encarregado/UpdateStatusEncarregado`

**Descrição**: Alterna status ativo/inativo

**Parâmetros**: `Id` (Guid)

---

### GET `/api/Encarregado/PegaFoto`

**Descrição**: Obtém foto do encarregado convertida para Base64

**Parâmetros**: `id` (Guid)

---

### GET `/api/Encarregado/PegaFotoModal`

**Descrição**: Obtém apenas foto do encarregado para exibição em modal

**Parâmetros**: `id` (Guid)

---

### GET `/api/Encarregado/EncarregadoContratos`

**Descrição**: Lista encarregados associados a um contrato

**Parâmetros**: `Id` (Guid) - ID do contrato

---

### POST `/api/Encarregado/DeleteContrato`

**Descrição**: Remove associação de encarregado com contrato

**Request Body**: `EncarregadoViewModel` com `EncarregadoId` e `ContratoId`

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Encarregado/Index.cshtml`
- **Pages**: `Pages/Contrato/*.cshtml`

### O Que Este Controller Chama

- **`_unitOfWork.Encarregado`**: CRUD
- **`_unitOfWork.EncarregadoContrato`**: Relacionamentos
- **`_unitOfWork.Contrato`**: Join para informações de contrato
- **`_unitOfWork.Fornecedor`**: Join para informações de fornecedor
- **`_unitOfWork.AspNetUsers`**: Join para usuário de alteração

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do EncarregadoController

**Arquivos Afetados**:
- `Controllers/EncarregadoController.cs`

**Impacto**: Documentação de referência para operações de encarregados

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
