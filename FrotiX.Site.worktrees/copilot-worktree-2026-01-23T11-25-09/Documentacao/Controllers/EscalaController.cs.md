# EscalaController.cs — Escalas diárias (MVC)

> **Arquivo:** `Controllers/EscalaController.cs`  
> **Papel:** CRUD MVC de escalas diárias de motoristas.

---

## ✅ Visão Geral

Controller MVC com telas de criação e edição de escala, validação de conflitos e notificações via SignalR.

---

## 🔧 Ações Principais

- `Create` (GET/POST): cria escala e associa motorista/veículo.
- `Edit` (GET/POST): atualiza escala e valida conflitos.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(EscalaDiariaViewModel model)
{
    var horaInicio = TimeSpan.Parse(model.HoraInicio);
    var horaFim = TimeSpan.Parse(model.HoraFim);

    var conflito = await _unitOfWork.EscalaDiaria.ExisteEscalaConflitanteAsync(
        model.MotoristaId.Value, model.DataEscala, horaInicio, horaFim, null);

    if (conflito)
    {
        TempData["error"] = "Já existe uma escala para este motorista neste horário";
        return RedirectToAction(nameof(Create));
    }

    // ... cria escala
}
```

---

## ✅ Observações Técnicas

- Usa `EscalaHub` para notificar atualizações.
- Recarrega listas de dropdown quando há erro.


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
