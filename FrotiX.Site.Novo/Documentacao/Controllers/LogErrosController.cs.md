# LogErrosController.cs — Logs de erro

> **Arquivo:** `Controllers/LogErrosController.cs`  
> **Papel:** registrar e consultar logs de erro do sistema.

---

## ✅ Visão Geral

Controller API que registra erros JavaScript e fornece endpoints para listar, filtrar e limpar logs.

---

## 🔧 Endpoints Principais

- `LogJavaScript`: registra log client-side.
- `ObterLogs` / `ObterLogsPorData` / `ListarArquivos`.
- `ObterEstatisticas`.
- `LimparLogs` / `LimparLogsAntigos` / `DownloadLog`.

---

## 🧩 Snippet Comentado

```csharp
[HttpPost]
[Route("LogJavaScript")]
public IActionResult LogJavaScript([FromBody] LogJavaScriptRequest request)
{
    if (request == null || string.IsNullOrEmpty(request.Mensagem))
    {
        return BadRequest(new { success = false, error = "Dados de log inválidos" });
    }

    _logService.ErrorJS(request.Mensagem, request.Arquivo, request.Metodo, request.Linha, request.Coluna, request.Stack, request.UserAgent, request.Url);
    return Ok(new { success = true });
}
```

---

## ✅ Observações Técnicas

- Serializa JSON manualmente para evitar problemas com interceptadores.
- Permite download direto do arquivo de log.


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
