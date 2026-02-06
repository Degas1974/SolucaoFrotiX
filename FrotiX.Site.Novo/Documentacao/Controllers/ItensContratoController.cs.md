# ItensContratoController.cs — Itens de contrato/ata

> **Arquivo:** `Controllers/ItensContratoController.cs`  
> **Papel:** endpoints para listar contratos/atas e itens associados.

---

## ✅ Visão Geral

Controller API que oferece dados para dropdowns, detalhes de contrato/ata e vínculos de veículos/itens.

---

## 🔧 Endpoints Principais

- `ListaContratos` / `ListaAtas`.
- `GetContratoDetalhes` / `GetAtaDetalhes`.
- `GetVeiculosContrato` (usa repactuações e itens).

---

## 🧩 Snippet Comentado

```csharp
[Route("ListaContratos")]
[HttpGet]
public IActionResult ListaContratos(bool status = true)
{
    var contratos = _unitOfWork.Contrato.GetAll(filter: c => c.Status == status, includeProperties: "Fornecedor")
        .OrderBy(c => c.NumeroContrato)
        .ThenBy(c => c.AnoContrato)
        .Select(c => new { value = c.ContratoId, text = $"{c.NumeroContrato}/{c.AnoContrato} - {c.TipoContrato} - {c.Fornecedor.DescricaoFornecedor}" })
        .ToList();

    return Ok(new { success = true, data = contratos });
}
```

---

## ✅ Observações Técnicas

- Inclui flags de terceirização e custos no detalhe do contrato.
- `GetVeiculosContrato` cruza repactuações e itens para descrição do item.


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
