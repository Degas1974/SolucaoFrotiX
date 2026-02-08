# DashboardVeiculosController.cs — Dashboard de veículos

> **Arquivo:** `Controllers/DashboardVeiculosController.cs`  
> **Papel:** agregações e rankings do dashboard de veículos.

---

## ✅ Visão Geral

Controller API que agrega totais, distribuição por categoria/status/origem e rankings de quilometragem.

---

## 🔧 Endpoints Principais

- `DashboardDados`: totais e distribuições.
- `DashboardUso`: estatísticas de viagens e abastecimentos por período.

---

## 🧩 Snippet Comentado

```csharp
[Route("DashboardDados")]
[HttpGet]
public IActionResult DashboardDados()
{
    var veiculos = _unitOfWork.ViewVeiculos.GetAll().ToList();
    var totalVeiculos = veiculos.Count;
    var veiculosAtivos = veiculos.Count(v => v.Status == true);

    return Ok(new { totais = new { totalVeiculos, veiculosAtivos } });
}
```

---

## ✅ Observações Técnicas

- Usa `ViewVeiculos` e `Veiculo` para métricas complementares.
- Inclui cálculo de idade média e valor mensal total.


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
