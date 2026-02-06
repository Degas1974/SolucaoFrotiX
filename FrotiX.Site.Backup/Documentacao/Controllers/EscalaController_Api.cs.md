# EscalaController_Api.cs — APIs de escala

> **Arquivo:** `Controllers/EscalaController_Api.cs`  
> **Papel:** endpoints API para DataTables e filtros de escala.

---

## ✅ Visão Geral

Partial class do `EscalaController` com endpoints server-side para DataTables e filtros customizados.

---

## 🔧 Endpoints Principais

- `ListaEscalasServerSide`: paginação e filtros para DataTables.
- `GetEscalasFiltradas`: endpoint alternativo com filtros manuais.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost]
[Route("api/Escala/ListaEscalasServerSide")]
public IActionResult ListaEscalasServerSide()
{
    var draw = Request.Form["draw"].FirstOrDefault();
    var startStr = Request.Form["start"].FirstOrDefault();
    var lengthStr = Request.Form["length"].FirstOrDefault();
    var query = _unitOfWork.ViewEscalasCompletas.GetAll().AsQueryable();

    // aplica filtros e paginação
    var data = query.Skip(skip).Take(pageSize).ToList();
    return Json(new { draw, recordsTotal, recordsFiltered, data });
}
```

---

## ✅ Observações Técnicas

- Comentário de correção indica remoção de `[ApiController]` nesta partial.
- Retorna erros com `Alerta.TratamentoErroComLinha` e log via `_logger`.


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
