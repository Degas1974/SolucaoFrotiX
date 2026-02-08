# AdministracaoController.cs — Indicadores administrativos

> **Arquivo:** `Controllers/AdministracaoController.cs`  
> **Papel:** endpoints de resumo administrativo da frota (indicadores e heatmap).

---

## ✅ Visão Geral

Controller com endpoints de estatísticas gerais da frota, normalização de viagens, distribuição por tipo de uso e heatmap de viagens.

---

## 🔧 Endpoints Principais

- `ObterResumoGeralFrota`: veículos, motoristas, viagens e KM total.
- `ObterEstatisticasNormalizacao`: métricas de normalização de viagens.
- `ObterDistribuicaoTipoUso`: distribuição por tipo de uso (fallback de veículo).
- `ObterHeatmapViagens`: matriz 7x24 por dia/hora.

---

## 🧩 Snippet Comentado

```csharp
[HttpGet]
[Route("api/Administracao/ObterResumoGeralFrota")]
public async Task<IActionResult> ObterResumoGeralFrota(DateTime? dataInicio, DateTime? dataFim)
{
    if (!dataInicio.HasValue || !dataFim.HasValue)
    {
        dataFim = DateTime.Now.Date;
        dataInicio = dataFim.Value.AddDays(-30);
    }

    var veiculosAtivos = await _context.Veiculo.AsNoTracking().CountAsync(v => v.Status == true);
    var motoristasAtivos = await _context.Motorista.AsNoTracking().CountAsync(m => m.Status == true);

    return Ok(new { sucesso = true, dados = new { veiculosAtivos, motoristasAtivos } });
}
```

---

## ✅ Observações Técnicas

- Usa `FrotiXDbContext` direto para métricas rápidas.
- Fallbacks garantem resposta mesmo sem tabelas auxiliares.


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
