# DashboardViagensController_ExportacaoPDF.cs — Exportação PDF

> **Arquivo:** `Controllers/DashboardViagensController_ExportacaoPDF.cs`  
> **Papel:** gerar PDFs do dashboard de viagens, com gráficos e cards.

---

## ✅ Visão Geral

Partial class que gera PDF em GET (dados diretos) e POST (gráficos em Base64), usando Syncfusion.

---

## 🔧 Endpoints Principais

- `ExportarParaPDF` (GET): gera PDF por período.
- `ExportarParaPDF` (POST): recebe cards/gráficos Base64 e gera PDF visual.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost]
[Route("ExportarParaPDF")]
[RequestSizeLimit(104857600)]
public async Task<IActionResult> ExportarParaPDF([FromBody] ExportarDashboardParaPDFViewModel model)
{
    if (model?.Graficos == null)
        model.Graficos = new Dictionary<string, string>();

    using (PdfDocument document = new PdfDocument())
    {
        document.PageSettings.Size = PdfPageSize.A4;
        await CriarPagina1ComCardsVisuais(document, model.DataInicio, model.DataFim, model.Cards, model.Graficos);
        // ... páginas adicionais
        var stream = new MemoryStream();
        document.Save(stream);
        stream.Position = 0;
        return File(stream, "application/pdf");
    }
}
```

---

## ✅ Observações Técnicas

- Limite de request 100MB para imagens em Base64.
- Logs detalhados no console durante geração.


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
