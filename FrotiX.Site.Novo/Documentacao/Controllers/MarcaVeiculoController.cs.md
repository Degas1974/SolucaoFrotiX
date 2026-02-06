# MarcaVeiculoController.cs — Marcas de veículos

> **Arquivo:** `Controllers/MarcaVeiculoController.cs`  
> **Papel:** listar, excluir e alternar status das marcas.

---

## ✅ Visão Geral

Controller API para gestão de marcas de veículos. Possui documentação completa em `Documentacao/Pages/MarcaVeiculo - Index.md`.

---

## 🔧 Endpoints Principais

- `Get`: lista marcas.
- `Delete`: impede exclusão quando há modelos vinculados.
- `UpdateStatusMarcaVeiculo`: alterna status.

---

## 🧩 Snippet Comentado

```csharp
[Route("UpdateStatusMarcaVeiculo")]
public JsonResult UpdateStatusMarcaVeiculo(Guid Id)
{
    var objFromDb = _unitOfWork.MarcaVeiculo.GetFirstOrDefault(u => u.MarcaId == Id);
    objFromDb.Status = !objFromDb.Status;
    _unitOfWork.MarcaVeiculo.Update(objFromDb);
    return Json(new { success = true, message = "Status atualizado", type = objFromDb.Status ? 0 : 1 });
}
```

---

## ✅ Observações Técnicas

- Retorna mensagem formatada com nome da marca.
- `Delete` bloqueia se existir modelo associado.


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
