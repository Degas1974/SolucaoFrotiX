# AbastecimentoController.cs — API base de abastecimentos

> **Arquivo:** `Controllers/AbastecimentoController.cs`  
> **Papel:** endpoints CRUD/listagem e importação básica de abastecimentos.

---

## ✅ Visão Geral

Controller API principal para abastecimentos. Expõe listagens por filtros (veículo, combustível, unidade, motorista, data) e um fluxo de importação via Excel (NPOI).

---

## 🔧 Estrutura e Dependências

- `IUnitOfWork` para consultas (`ViewAbastecimentos`, `Veiculo`).
- `IHubContext<ImportacaoHub>` para progresso de importação.
- `FrotiXDbContext` para operações diretas.
- Importação usando `NPOI` (XLS/XLSX).

---

## 🧩 Snippets Comentados

```csharp
[Route("AbastecimentoVeiculos")]
[HttpGet]
public IActionResult AbastecimentoVeiculos(Guid Id)
{
    var dados = _unitOfWork.ViewAbastecimentos.GetAll()
        .Where(va => va.VeiculoId == Id)
        .OrderByDescending(va => va.DataHora)
        .ToList();

    return Ok(new { data = dados });
}
```

```csharp
[Route("Import")]
[HttpPost]
public ActionResult Import()
{
    IFormFile file = Request.Form.Files[0];
    // ... leitura XLS/XLSX e validações de placa/data
}
```

---

## ✅ Observações Técnicas

- Possui `try-catch` em todas as ações, com `Alerta.TratamentoErroComLinha`.
- `Import` valida existência de veículo por placa antes de inserir dados.


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
