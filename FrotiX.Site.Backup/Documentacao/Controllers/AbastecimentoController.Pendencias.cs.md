# AbastecimentoController.Pendencias.cs — Pendências de importação

> **Arquivo:** `Controllers/AbastecimentoController.Pendencias.cs`  
> **Papel:** listar e resolver pendências geradas na importação.

---

## ✅ Visão Geral

Partial class com DTOs para pendências e endpoints de listagem, contagem e resolução.

---

## 🔧 Endpoints Principais

- `ListarPendencias`: lista pendências abertas.
- `ContarPendencias`: totaliza pendências por tipo.
- `ObterPendencia`: detalhes por ID.
- `ResolverPendencia`: tenta corrigir e importar pendência.

---

## 🧩 Snippet Comentado

```csharp
[Route("ListarPendencias")]
[HttpGet]
public IActionResult ListarPendencias()
{
    var pendencias = _unitOfWork.AbastecimentoPendente.GetAll()
        .Where(p => p.Status == 0)
        .OrderByDescending(p => p.DataImportacao)
        .ToList();

    var resultado = pendencias.Select(p => new PendenciaDTO
    {
        AbastecimentoPendenteId = p.AbastecimentoPendenteId.ToString(),
        Placa = p.Placa,
        DataHora = p.DataHora?.ToString("dd/MM/yyyy HH:mm"),
        TipoPendencia = p.TipoPendencia,
        TemSugestao = p.TemSugestao
    }).ToList();

    return Ok(new { data = resultado });
}
```

---

## ✅ Observações Técnicas

- Mantém o padrão `try-catch` com `Alerta.TratamentoErroComLinha`.
- Em `ResolverPendencia`, recalcula KM rodado e atualiza média de consumo.


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
