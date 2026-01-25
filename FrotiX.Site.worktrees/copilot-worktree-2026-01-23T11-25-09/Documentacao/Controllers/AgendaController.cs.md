# AgendaController.cs — Agenda e calendário

> **Arquivo:** `Controllers/AgendaController.cs`  
> **Papel:** endpoints de agenda, diagnósticos e testes da View de viagens.

---

## ✅ Visão Geral

Controller API que fornece dados de calendário, diagnósticos de período e integrações com `ViewViagensAgenda`.

---

## 🔧 Blocos Importantes

- **Diagnósticos**: `TesteView`, `DiagnosticoAgenda`, `TesteCarregaViagens`.
- **Serviços**: `ViagemEstatisticaService` para métricas.
- **Contexto**: trabalha com `ViewViagensAgenda` e ajustes de timezone (-3h).

---

## 🧩 Snippet Comentado

```csharp
[HttpGet("DiagnosticoAgenda")]
public IActionResult DiagnosticoAgenda(DateTime? start = null, DateTime? end = null)
{
    if (!start.HasValue)
        start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

    var startMenos3 = start.Value.AddHours(-3);
    var endMenos3 = end.Value.AddHours(-3);

    var totalGeral = _context.ViewViagensAgenda.Count();
    // ... retornos de diagnóstico
}
```

---

## ✅ Observações Técnicas

- O arquivo possui documentação extensa referenciada em `Documentacao/Pages/Agenda - Index.md`.
- Mantém `try-catch` com `Alerta.TratamentoErroComLinha` no construtor.


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
