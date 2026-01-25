# DashboardEventosController.cs — Dashboard de eventos

> **Arquivo:** `Controllers/DashboardEventosController.cs`  
> **Papel:** endpoints de estatísticas e gráficos do dashboard de eventos.

---

## ✅ Visão Geral

Controller protegido por `[Authorize]` que fornece dados agregados de eventos por status, setor, requisitante e evolução mensal.

---

## 🔧 Endpoints Principais

- `Index`: página `/Pages/Eventos/DashboardEventos.cshtml`.
- `ObterEstatisticasGerais`: KPIs (totais, status, participantes).
- `ObterEventosPorStatus` / `ObterEventosPorSetor` / `ObterEventosPorRequisitante`.
- `ObterEventosPorMes`: evolução mensal.

---

## 🧩 Snippet Comentado

```csharp
[Route("api/DashboardEventos/ObterEstatisticasGerais")]
public async Task<IActionResult> ObterEstatisticasGerais(DateTime? DataInicial, DateTime? dataFim)
{
    if (!DataInicial.HasValue || !dataFim.HasValue)
    {
        dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        DataInicial = dataFim.Value.AddDays(-30);
    }

    var eventos = await _context.Evento
        .Include(e => e.SetorSolicitante)
        .Include(e => e.Requisitante)
        .Where(e => e.DataInicial >= DataInicial && e.DataInicial <= dataFim)
        .ToListAsync();

    return Json(new { success = true, totalEventos = eventos.Count });
}
```

---

## ✅ Observações Técnicas

- Define período padrão quando datas não são informadas.
- Usa `FrotiXDbContext` para consultas agregadas.


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
