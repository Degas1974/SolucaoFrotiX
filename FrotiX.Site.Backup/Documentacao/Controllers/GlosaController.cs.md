# GlosaController.cs — Glosas e exportações

> **Arquivo:** `Controllers/GlosaController.cs`  
> **Papel:** fornecer dados de glosas e exportar relatórios em Excel.

---

## ✅ Visão Geral

Controller API que atende grids Syncfusion (resumo/detalhes) e exporta planilhas Excel via ClosedXML.

---

## 🔧 Endpoints Principais

- `Resumo` / `Detalhes`: retornos para DataManager.
- `ExportResumo` / `ExportDetalhes` / `ExportAmbos`: planilhas com dados.

---

## 🧩 Snippet Comentado

```csharp
[HttpGet("resumo")]
public IActionResult Resumo(DataManagerRequest dm, Guid contratoId, int ano, int mes)
{
    var data = _service.ListarResumo(contratoId, mes, ano).AsQueryable();
    var result = new DataOperations().PerformSorting(data, dm.Sorted);
    return new JsonResult(new DataResult { Result = result, Count = result.Cast<object>().Count() });
}
```

---

## ✅ Observações Técnicas

- Usa `DataOperations` do Syncfusion para filtros/paginação.
- Helpers formatam colunas de moeda e datas ao exportar.


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
