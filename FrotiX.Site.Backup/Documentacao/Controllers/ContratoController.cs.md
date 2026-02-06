# ContratoController.cs — Contratos

> **Arquivo:** `Controllers/ContratoController.cs`  
> **Papel:** CRUD e validações para contratos e dependências associadas.

---

## ✅ Visão Geral

Controller API que lista contratos com dependências (veículos, itens, repactuações, etc.), permite exclusão e gravação. Possui documentação completa em `Documentacao/Pages/Contrato - Index.md`.

---

## 🔧 Endpoints Principais

- `Get`: lista contratos com fornecedores e contagens de dependências.
- `Delete`: remove contrato se não houver vínculos bloqueantes.
- `EditaContrato` / `InsereContrato`: gravação de contrato e repactuação.

---

## 🧩 Snippet Comentado

```csharp
[HttpGet]
public IActionResult Get()
{
    var contratos = (
        from c in _unitOfWork.Contrato.GetAll()
        join f in _unitOfWork.Fornecedor.GetAll() on c.FornecedorId equals f.FornecedorId
        orderby c.AnoContrato descending
        select new
        {
            ContratoCompleto = c.AnoContrato + "/" + c.NumeroContrato,
            ProcessoCompleto = c.NumeroProcesso + "/" + c.AnoProcesso.ToString().Substring(2, 2),
            c.Status,
            c.ContratoId
        }).ToList();

    return Json(new { data = contratos });
}
```

---

## ✅ Observações Técnicas

- Possui cabeçalho indicando documentação detalhada externa.
- Usa dicionários para contar dependências em lote.


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
