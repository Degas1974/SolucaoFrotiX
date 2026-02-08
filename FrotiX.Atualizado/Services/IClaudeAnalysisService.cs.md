# Services/IClaudeAnalysisService.cs

**ARQUIVO NOVO** | 26 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System.Threading.Tasks;
using FrotiX.Models;

namespace FrotiX.Services;

public interface IClaudeAnalysisService
{

    Task<ClaudeAnalysisResult> AnalyzeErrorAsync(LogErro logErro);

    bool IsConfigured { get; }
}

public class ClaudeAnalysisResult
{

    public bool Success { get; set; }

    public string? Error { get; set; }

    public string? Analysis { get; set; }

    public string? Model { get; set; }

    public int InputTokens { get; set; }

    public int OutputTokens { get; set; }

    public System.DateTime AnalyzedAt { get; set; } = System.DateTime.Now;
}

public class ClaudeAISettings
{

    public string ApiKey { get; set; } = "";

    public string Model { get; set; } = "claude-sonnet-4-20250514";

    public int MaxTokens { get; set; } = 4096;

    public double Temperature { get; set; } = 0.3;

    public string SystemPrompt { get; set; } = "Você é um especialista em análise de erros de software.";
}
```
