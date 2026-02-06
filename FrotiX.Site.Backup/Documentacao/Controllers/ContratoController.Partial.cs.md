# ContratoController.Partial.cs — Listas auxiliares

> **Arquivo:** `Controllers/ContratoController.Partial.cs`  
> **Papel:** endpoint para listar contratos por status (dropdowns).

---

## ✅ Visão Geral

Retorna contratos filtrados por status, usados em telas como Nota Fiscal.

---

## 🧩 Snippet Comentado

```csharp
[Route("ListaContratosPorStatus")]
[HttpGet]
public IActionResult ListaContratosPorStatus(int status)
{
    bool statusBool = status == 1;
    var result = (
        from c in _unitOfWork.Contrato.GetAll()
        where c.Status == statusBool
        orderby c.AnoContrato descending, c.NumeroContrato descending
        select new { value = c.ContratoId, text = c.AnoContrato + "/" + c.NumeroContrato + " - " + c.Objeto }
    ).ToList();

    return Json(new { data = result });
}
```

---

## ✅ Observações Técnicas

- Retorna lista pronta para dropdowns.
- Falhas retornam lista vazia para o frontend.


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
