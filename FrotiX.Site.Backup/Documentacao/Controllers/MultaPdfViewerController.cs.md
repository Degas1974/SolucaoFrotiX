# MultaPdfViewerController.cs — Visualização de PDF

> **Arquivo:** `Controllers/MultaPdfViewerController.cs`  
> **Papel:** endpoints do Syncfusion PDF Viewer para multas.

---

## ✅ Visão Geral

Controller API que carrega PDFs por arquivo ou Base64 e fornece operações do viewer (thumbnails, textos, bookmarks, etc.).

---

## 🔧 Endpoints Principais

- `Load`: carrega PDF para o viewer.
- `RenderPdfPages`, `RenderPdfTexts`, `RenderThumbnailImages`.
- `Bookmarks`, `RenderAnnotationComments`, `Unload`.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost("Load")]
public IActionResult Load([FromBody] Dictionary<string, string> json)
{
    var viewer = new PdfRenderer(_cache);
    var stream = ResolveDocumentStream(json);
    var output = viewer.Load(stream, json);
    return Content(JsonConvert.SerializeObject(output), "application/json; charset=utf-8");
}
```

---

## ✅ Observações Técnicas

- `ResolveDocumentStream` suporta arquivo físico ou Base64.
- Pasta base: `wwwroot/DadosEditaveis/Multas`.


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
