# MultaUploadController.cs — Upload de PDFs

> **Arquivo:** `Controllers/MultaUploadController.cs`  
> **Papel:** upload e remoção de PDFs de multas.

---

## ✅ Visão Geral

Controller API compatível com Syncfusion EJ2 Uploader. Valida PDFs, normaliza nomes e salva em `DadosEditaveis/Multas`.

---

## 🔧 Endpoints Principais

- `Save`: upload múltiplo com validação de PDF.
- `Remove`: remove arquivos enviados.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost("Save")]
public IActionResult Save(IList<IFormFile> UploadFiles)
{
    foreach (var file in UploadFiles)
    {
        var extensao = Path.GetExtension(file.FileName).ToLower();
        if (extensao != ".pdf") { /* erro */ }

        var nomeNormalizado = Servicos.TiraAcento(Path.GetFileNameWithoutExtension(file.FileName));
        var nomeArquivo = $"{nomeNormalizado}_{DateTime.Now:yyyyMMddHHmmss}{extensao}";
        file.CopyTo(new FileStream(Path.Combine(pastaMultas, nomeArquivo), FileMode.Create));
    }

    return Ok(new { files = uploadedFiles });
}
```

---

## ✅ Observações Técnicas

- Suporta remoção por nome quando não há arquivos na requisição.
- Respostas seguem padrão EJ2 (`files` com status e códigos).


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
