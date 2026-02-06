# FornecedorController.cs — Fornecedores

> **Arquivo:** `Controllers/FornecedorController.cs`  
> **Papel:** listar, excluir e alternar status de fornecedores.

---

## ✅ Visão Geral

Controller API que lista fornecedores, impede exclusão quando há contratos associados e atualiza status ativo/inativo.

---

## 🔧 Endpoints Principais

- `Get`: lista fornecedores.
- `Delete`: remove fornecedor se não houver contratos vinculados.
- `UpdateStatusFornecedor`: alterna status.

---

## 🧩 Snippet Comentado

```csharp
[Route("Delete")]
[HttpPost]
public IActionResult Delete(FornecedorViewModel model)
{
    var objFromDb = _unitOfWork.Fornecedor.GetFirstOrDefault(u => u.FornecedorId == model.FornecedorId);
    var contrato = _unitOfWork.Contrato.GetFirstOrDefault(u => u.FornecedorId == model.FornecedorId);

    if (contrato != null)
    {
        return Json(new { success = false, message = "Existem contratos associados a esse fornecedor" });
    }

    _unitOfWork.Fornecedor.Remove(objFromDb);
    _unitOfWork.Save();
    return Json(new { success = true, message = "Fornecedor removido com sucesso" });
}
```

---

## ✅ Observações Técnicas

- `UpdateStatusFornecedor` alterna status e retorna descrição formatada.


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
