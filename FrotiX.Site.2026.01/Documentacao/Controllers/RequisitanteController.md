# Documentação: RequisitanteController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `RequisitanteController` gerencia operações CRUD de requisitantes (pessoas que solicitam viagens), incluindo relacionamento com setores solicitantes e hierarquia.

**Principais características:**

✅ **CRUD Completo**: Listagem, criação, atualização e exclusão  
✅ **Hierarquia de Setores**: Suporta estrutura hierárquica de setores  
✅ **Upsert**: Método unificado para criar/atualizar  
✅ **Validações**: Validações de dados obrigatórios

---

## Endpoints API

### GET `/api/Requisitante`

**Descrição**: Retorna lista de requisitantes com informações de setor

**Response**:
```json
{
  "data": [
    {
      "ponto": "PONTO_01",
      "nome": "João Silva",
      "ramal": 1234,
      "nomeSetor": "TI",
      "status": true,
      "requisitanteId": "guid"
    }
  ]
}
```

**Quando é chamado**: Pela página `Pages/Requisitante/Index.cshtml`

---

### GET `/api/Requisitante/GetAll`

**Descrição**: Retorna lista completa com campos formatados para formulários

**Response**: Lista com campos `requisitanteId`, `ponto`, `nome`, `ramal`, `setorSolicitanteId`, `setorNome`, `status`

---

### GET `/api/Requisitante/GetById`

**Descrição**: Obtém requisitante específico por ID

**Parâmetros**: `id` (string GUID)

**Response**:
```json
{
  "success": true,
  "data": {
    "requisitanteId": "guid",
    "ponto": "PONTO_01",
    "nome": "João Silva",
    "ramal": 1234,
    "setorSolicitanteId": "guid",
    "status": true
  }
}
```

---

### POST `/api/Requisitante/Upsert`

**Descrição**: **ENDPOINT PRINCIPAL** - Cria ou atualiza requisitante

**Request Body**: `RequisitanteUpsertModel`
```json
{
  "requisitanteId": "guid ou vazio",
  "ponto": "PONTO_01",
  "nome": "João Silva",
  "ramal": 1234,
  "setorSolicitanteId": "guid",
  "status": true
}
```

**Validações**:
- `Nome` é obrigatório
- Se `requisitanteId` vazio ou `Guid.Empty`, cria novo
- Senão, atualiza existente

**Lógica**:
- Obtém ID do usuário logado via Claims
- Define `DataAlteracao` e `UsuarioIdAlteracao`
- Salva alterações

**Quando é chamado**: Ao criar/editar requisitante na interface

---

### GET `/api/Requisitante/GetSetores`

**Descrição**: Retorna lista de setores solicitantes ativos

**Response**:
```json
[
  {
    "id": "guid",
    "nome": "TI"
  }
]
```

---

### GET `/api/Requisitante/GetSetoresHierarquia`

**Descrição**: Retorna setores em estrutura hierárquica (árvore)

**Response**:
```json
[
  {
    "id": "guid",
    "nome": "Setor Raiz",
    "hasChild": true,
    "children": [
      {
        "id": "guid-filho",
        "nome": "Subsetor",
        "hasChild": false,
        "children": null
      }
    ]
  }
]
```

**Lógica**: Monta hierarquia recursiva usando `SetorPaiId`

---

### POST `/api/Requisitante/AtualizarRequisitanteRamalSetor`

**Descrição**: Atualiza apenas ramal e/ou setor de um requisitante

**Request Body**: `AtualizarRequisitanteDto`
```json
{
  "requisitanteId": "guid",
  "ramal": 5678,
  "setorSolicitanteId": "guid"
}
```

**Lógica**: Atualiza apenas campos informados, mantém outros inalterados

---

### POST `/api/Requisitante/Delete`

**Descrição**: Exclui requisitante

**Request Body**: `RequisitanteViewModel` com `RequisitanteId`

**Response**:
```json
{
  "success": true,
  "message": "Requisitante removido com sucesso"
}
```

---

### GET `/api/Requisitante/UpdateStatusRequisitante`

**Descrição**: Alterna status ativo/inativo

**Parâmetros**: `Id` (Guid)

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/Requisitante/Index.cshtml`
- **Pages**: `Pages/Viagem/*.cshtml` - Para seleção de requisitantes

### O Que Este Controller Chama

- **`_unitOfWork.Requisitante`**: CRUD
- **`_unitOfWork.SetorSolicitante`**: Listagem e hierarquia
- **`User.FindFirst(ClaimTypes.NameIdentifier)`**: ID do usuário logado

---

## Notas Importantes

1. **Hierarquia**: Suporta setores com estrutura pai-filho
2. **Upsert**: Método unificado simplifica criação/edição
3. **Claims**: Usa Claims do Identity para rastrear alterações

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do RequisitanteController

**Arquivos Afetados**:
- `Controllers/RequisitanteController.cs`

**Impacto**: Documentação de referência para operações de requisitantes

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
