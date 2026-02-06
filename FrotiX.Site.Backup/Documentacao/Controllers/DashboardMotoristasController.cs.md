# DashboardMotoristasController.cs — Dashboard de motoristas

> **Arquivo:** `Controllers/DashboardMotoristasController.cs`  
> **Papel:** estatísticas e filtros do dashboard de motoristas.

---

## ✅ Visão Geral

Controller com endpoints para listar anos/meses disponíveis, listar motoristas e gerar KPIs com base em tabelas estatísticas (fallback para viagens).

---

## 🔧 Endpoints Principais

- `ObterAnosMesesDisponiveis` / `ObterMesesPorAno`.
- `ObterListaMotoristas`.
- `ObterEstatisticasGerais` (otimizado por ano/mês ou período).

---

## 🧩 Snippet Comentado

```csharp
[Route("api/DashboardMotoristas/ObterEstatisticasGerais")]
public async Task<IActionResult> ObterEstatisticasGerais(DateTime? dataInicio, DateTime? dataFim, int? ano, int? mes)
{
    if (ano.HasValue)
    {
        dataInicio = new DateTime(ano.Value, mes ?? 1, 1);
        dataFim = dataInicio.Value.AddMonths(mes.HasValue ? 1 : 12).AddSeconds(-1);
    }

    // Busca estatísticas pré-calculadas ou fallback
}
```

---

## ✅ Observações Técnicas

- Usa `EstatisticaGeralMensal` quando disponível.
- Fallback para viagens quando não há dados estatísticos.


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
