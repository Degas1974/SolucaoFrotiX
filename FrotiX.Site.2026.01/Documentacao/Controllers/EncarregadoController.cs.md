# EncarregadoController.cs — Encarregados

> **Arquivo:** `Controllers/EncarregadoController.cs`  
> **Papel:** gestão de encarregados e imagens.

---

## ✅ Visão Geral

Controller API que lista encarregados com contrato/fornecedor, permite exclusão e alterna status. Possui documentação completa em `Documentacao/Pages/Encarregado - Index.md`.

---

## 🔧 Endpoints Principais

- `Get`: lista encarregados com contrato e fornecedor.
- `Delete`: impede exclusão se houver vínculo em `EncarregadoContrato`.
- `UpdateStatusEncarregado`: alterna ativo/inativo.
- `PegaFoto` / `PegaFotoModal`: retorna foto base64.

---

## 🧩 Snippet Comentado

```csharp
[Route("UpdateStatusEncarregado")]
public JsonResult UpdateStatusEncarregado(Guid Id)
{
    var objFromDb = _unitOfWork.Encarregado.GetFirstOrDefault(u => u.EncarregadoId == Id);
    objFromDb.Status = !objFromDb.Status;
    _unitOfWork.Encarregado.Update(objFromDb);
    return Json(new { success = true, message = "Status atualizado", type = objFromDb.Status ? 0 : 1 });
}
```

---

## ✅ Observações Técnicas

- Retorna HTML em `ContratoEncarregado` quando não há vínculo.
- Usa helper `GetImage` para converter base64.


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
