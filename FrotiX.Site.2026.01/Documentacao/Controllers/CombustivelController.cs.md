# CombustivelController.cs — Tipos de combustível

> **Arquivo:** `Controllers/CombustivelController.cs`  
> **Papel:** listar e gerenciar tipos de combustível.

---

## ✅ Visão Geral

Controller API que retorna tipos de combustível, permite exclusão e alterna status ativo/inativo.

---

## 🔧 Endpoints Principais

- `Get`: lista todos os combustíveis.
- `Delete`: remove combustível se não houver veículos vinculados.
- `UpdateStatusCombustivel`: alterna status do combustível.

---

## 🧩 Snippet Comentado

```csharp
[Route("Delete")]
[HttpPost]
public IActionResult Delete(CombustivelViewModel model)
{
    if (model != null && model.CombustivelId != Guid.Empty)
    {
        var objFromDb = _unitOfWork.Combustivel.GetFirstOrDefault(u => u.CombustivelId == model.CombustivelId);
        var veiculo = _unitOfWork.Veiculo.GetFirstOrDefault(u => u.CombustivelId == model.CombustivelId);
        if (veiculo != null)
        {
            return Json(new { success = false, message = "Existem veículos associados a essa combustível" });
        }
        _unitOfWork.Combustivel.Remove(objFromDb);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Tipo de Combustível removido com sucesso" });
    }

    return Json(new { success = false, message = "Erro ao apagar Tipo de Combustível" });
}
```

---

## ✅ Observações Técnicas

- Possui cabeçalho indicando documentação completa em `Documentacao/Pages/Combustivel - Index.md`.
- Usa `Alerta.TratamentoErroComLinha` para erros.


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
