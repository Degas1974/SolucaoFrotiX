# DashboardViagensController.cs — Dashboard de viagens

> **Arquivo:** `Controllers/DashboardViagensController.cs`  
> **Papel:** estatísticas, rankings e filtros de viagens.

---

## ✅ Visão Geral

Controller com KPIs por período, filtros de outliers de KM e estatísticas por status e dia da semana.

---

## 🔧 Endpoints Principais

- `ObterEstatisticasGerais`: KPIs com filtros de outliers.
- `ObterViagensPorDia` / `ObterViagensPorStatus`.
- Outros endpoints para rankings e agregações.

---

## 🧩 Snippet Comentado

```csharp
[Route("api/DashboardViagens/ObterEstatisticasGerais")]
public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim)
{
    if (!dataInicio.HasValue || !dataFim.HasValue)
    {
        dataFim = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        dataInicio = dataFim.Value.AddDays(-30);
    }

    var viagens = await _context.Viagem
        .Where(v => v.DataInicial >= dataInicio && v.DataInicial <= dataFim)
        .ToListAsync();

    // Filtro de outliers por KM
}
```

---

## ✅ Observações Técnicas

- Filtro de outliers usa constante `KM_MAXIMO_POR_VIAGEM`.
- Custos calculados apenas para viagens realizadas.


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
