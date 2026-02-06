# EmpenhoController.cs — Empenhos

> **Arquivo:** `Controllers/EmpenhoController.cs`  
> **Papel:** listar, excluir e movimentar empenhos.

---

## ✅ Visão Geral

Controller API que busca empenhos por contrato ou ata, permite exclusão e gerencia movimentações (aporte/anulação).

---

## 🔧 Endpoints Principais

- `Get`: lista empenhos por `ContratoId` ou `AtaId`.
- `Delete`: remove empenho se não houver notas ou movimentações.
- `Aporte` / `EditarAporte` / `EditarAnulacao`: atualizam saldo.

---

## 🧩 Snippet Comentado

```csharp
[Route("Aporte")]
[HttpPost]
public IActionResult Aporte([FromBody] MovimentacaoEmpenho movimentacao)
{
    _unitOfWork.MovimentacaoEmpenho.Add(movimentacao);

    var empenho = _unitOfWork.Empenho.GetFirstOrDefault(u => u.EmpenhoId == movimentacao.EmpenhoId);
    empenho.SaldoFinal = empenho.SaldoFinal + movimentacao.Valor;
    _unitOfWork.Empenho.Update(empenho);

    _unitOfWork.Save();
    return Json(new { success = true, message = "Aporte realizado com sucesso" });
}
```

---

## ✅ Observações Técnicas

- Valores já vêm normalizados do frontend (sem divisão por 100).
- Impede exclusão quando há notas fiscais ou movimentações vinculadas.


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
