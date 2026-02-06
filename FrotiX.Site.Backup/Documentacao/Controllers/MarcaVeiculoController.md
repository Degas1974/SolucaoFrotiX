# Documentação: MarcaVeiculoController.cs

> **Última Atualização**: 08/01/2026  
> **Versão Atual**: 2.0

---

# PARTE 1: DOCUMENTAÇÃO DA FUNCIONALIDADE

## Visão Geral

O `MarcaVeiculoController` gerencia operações CRUD de marcas de veículos (Ford, Chevrolet, etc.) no sistema FrotiX.

**Principais características:**

✅ **CRUD Básico**: Listagem, exclusão e atualização de status  
✅ **Validação de Dependências**: Verifica modelos antes de excluir  
✅ **Status**: Alterna entre ativo/inativo

---

## Endpoints API

### GET `/api/MarcaVeiculo`

**Descrição**: Retorna lista de todas as marcas de veículos

**Response**:
```json
{
  "data": [
    {
      "marcaId": "guid",
      "descricaoMarca": "Ford",
      "status": true
    }
  ]
}
```

**Quando é chamado**: 
- Pela página `Pages/MarcaVeiculo/Index.cshtml`
- Para popular dropdowns em cadastro de veículos

---

### POST `/api/MarcaVeiculo/Delete`

**Descrição**: Exclui marca com validação

**Validações**: Verifica se há modelos associados (`ModeloVeiculo.MarcaId`)

**Response**:
```json
{
  "success": false,
  "message": "Existem modelos associados a essa marca"
}
```

---

### GET `/api/MarcaVeiculo/UpdateStatusMarcaVeiculo`

**Descrição**: Alterna status ativo/inativo

**Parâmetros**: `Id` (Guid)

**Response**:
```json
{
  "success": true,
  "message": "Atualizado Status da Marca [Nome: Ford] (Ativo)",
  "type": 0
}
```

---

## Interconexões

### Quem Chama Este Controller

- **Pages**: `Pages/MarcaVeiculo/Index.cshtml`
- **Pages**: `Pages/Veiculo/*.cshtml` - Para dropdowns

### O Que Este Controller Chama

- **`_unitOfWork.MarcaVeiculo`**: CRUD
- **`_unitOfWork.ModeloVeiculo`**: Validação de dependências

---

# PARTE 2: LOG DE MODIFICAÇÕES/CORREÇÕES

## [08/01/2026] - Documentação Inicial Completa

**Descrição**: Criação da documentação completa do MarcaVeiculoController

**Arquivos Afetados**:
- `Controllers/MarcaVeiculoController.cs`

**Impacto**: Documentação de referência para operações de marcas de veículos

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
