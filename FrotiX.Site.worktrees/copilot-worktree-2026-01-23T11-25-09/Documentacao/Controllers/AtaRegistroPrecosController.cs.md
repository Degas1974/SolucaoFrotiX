# AtaRegistroPrecosController.cs — Atas de registro de preços

> **Arquivo:** `Controllers/AtaRegistroPrecosController.cs`  
> **Papel:** CRUD e status de atas de registro de preços.

---

## ✅ Visão Geral

Controller API que lista atas, valida dependências e executa inserção/edição/remoção de registros com revalidação de integridade.

---

## 🔧 Endpoints Principais

- `Get`: lista atas com fornecedor e contagem de dependências.
- `Delete`: remove ata e seus itens de repactuação.
- `UpdateStatusAta`: alterna status ativo/inativo.
- `InsereAta` / `EditaAta`: grava ata e repactuação inicial.

---

## 🧩 Snippet Comentado

```csharp
[HttpGet]
public IActionResult Get()
{
    var result = (
        from a in _unitOfWork.AtaRegistroPrecos.GetAll()
        join f in _unitOfWork.Fornecedor.GetAll() on a.FornecedorId equals f.FornecedorId
        orderby a.AnoAta descending
        select new
        {
            AtaCompleta = a.AnoAta + "/" + a.NumeroAta,
            ProcessoCompleto = a.NumeroProcesso + "/" + a.AnoProcesso.ToString().Substring(2, 2),
            a.Objeto,
            f.DescricaoFornecedor,
            depItens = _unitOfWork.ItemVeiculoAta.GetAll(i => i.RepactuacaoAta.AtaId == a.AtaId).Count(),
            depVeiculos = _unitOfWork.VeiculoAta.GetAll(v => v.AtaId == a.AtaId).Count()
        }).ToList();

    return Ok(new { data = result });
}
```

---

## ✅ Observações Técnicas

- `Delete` remove repactuações e itens vinculados antes de excluir a ata.
- Mantém `try-catch` com `Alerta.TratamentoErroComLinha`.


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
