# NotaFiscalController.Partial.cs — Inserção/Edição

> **Arquivo:** `Controllers/NotaFiscalController.Partial.cs`  
> **Papel:** inserir e editar notas fiscais com validações.

---

## ✅ Visão Geral

Partial class com endpoints para inserir e editar nota fiscal, ajustando saldos do empenho conforme diferenças.

---

## 🔧 Endpoints Principais

- `Insere`: valida campos obrigatórios e cria nota fiscal.
- `Edita`: ajusta saldo do empenho com diferenças entre valores.

---

## 🧩 Snippet Comentado

```csharp
[Route("Insere")]
[HttpPost]
public IActionResult Insere([FromBody] NotaFiscal model)
{
    if (model.NumeroNF == null || model.EmpenhoId == Guid.Empty || model.ValorNF == 0)
        return Json(new { success = false, message = "Dados inválidos" });

    model.NotaFiscalId = Guid.NewGuid();
    model.ValorGlosa ??= 0;

    var empenho = _unitOfWork.Empenho.GetFirstOrDefault(e => e.EmpenhoId == model.EmpenhoId);
    if (empenho != null)
    {
        empenho.SaldoFinal = empenho.SaldoFinal - (model.ValorNF - model.ValorGlosa);
        _unitOfWork.Empenho.Update(empenho);
    }

    _unitOfWork.NotaFiscal.Add(model);
    _unitOfWork.Save();
    return Json(new { success = true, notaFiscalId = model.NotaFiscalId });
}
```

---

## ✅ Observações Técnicas

- `Edita` trata troca de empenho e diferença de valores.
- Inicializa `ValorGlosa` quando nulo.


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
