# NotaFiscalController.cs — Nota fiscal

> **Arquivo:** `Controllers/NotaFiscalController.cs`  
> **Papel:** gerenciar notas fiscais, glosas e saldo de empenhos.

---

## ✅ Visão Geral

Controller API que permite inserir, editar e remover notas fiscais, além de aplicar glosas com ajuste de saldo do empenho.

---

## 🔧 Endpoints Principais

- `Delete`: remove NF e devolve valor líquido ao empenho.
- `GetGlosa` / `Glosa`: consulta e aplica glosa.
- `EmpenhoList` / `EmpenhoListAta`: listas por vínculo.
- `Insere` / `Edita`: CRUD de nota fiscal.

---

## 🧩 Snippet Comentado

```csharp
[Route("Glosa")]
[HttpPost]
public IActionResult Glosa([FromBody] GlosaNota glosanota)
{
    var notaFiscal = _unitOfWork.NotaFiscal.GetFirstOrDefault(u => u.NotaFiscalId == glosanota.NotaFiscalId);
    var glosaAntiga = notaFiscal.ValorGlosa ?? 0;
    var valorGlosaInformada = glosanota.ValorGlosa ?? 0;

    var novaGlosa = glosanota.ModoGlosa == "somar"
        ? glosaAntiga + valorGlosaInformada
        : valorGlosaInformada;

    notaFiscal.ValorGlosa = novaGlosa;
    _unitOfWork.NotaFiscal.Update(notaFiscal);
    _unitOfWork.Save();

    return Json(new { success = true, novaGlosa });
}
```

---

## ✅ Observações Técnicas

- Ajusta saldo do empenho ao inserir, editar e excluir NFs.
- Valida glosa para não exceder o valor da nota fiscal.


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
