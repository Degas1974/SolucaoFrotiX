# MotoristaController.cs — Motoristas

> **Arquivo:** `Controllers/MotoristaController.cs`  
> **Papel:** gestão de motoristas e fotos.

---

## ✅ Visão Geral

Controller API que lista motoristas, bloqueia exclusão quando há vínculos e permite alternar status. Possui documentação completa em `Documentacao/Pages/Motorista - Index.md`.

---

## 🔧 Endpoints Principais

- `Get`: lista motoristas com contrato ou tipo de condutor.
- `Delete`: bloqueia exclusão se houver vínculo.
- `UpdateStatusMotorista`: alterna status.
- `PegaFoto` / `PegaFotoModal`.

---

## 🧩 Snippet Comentado

```csharp
[Route("UpdateStatusMotorista")]
public JsonResult UpdateStatusMotorista(Guid Id)
{
    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == Id);
    objFromDb.Status = !objFromDb.Status;
    _unitOfWork.Motorista.Update(objFromDb);
    return Json(new { success = true, message = "Status atualizado", type = objFromDb.Status ? 0 : 1 });
}
```

---

## ✅ Observações Técnicas

- Retorna `ContratoMotorista` com fallback para tipo/sem contrato.
- Endpoints de foto retornam base64 com `GetImage`.


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
