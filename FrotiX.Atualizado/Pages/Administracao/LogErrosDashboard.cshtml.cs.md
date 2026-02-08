# Pages/Administracao/LogErrosDashboard.cshtml.cs

**ARQUIVO NOVO** | 33 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using FrotiX.Repository.IRepository;

namespace FrotiX.Pages.Administracao;

[Authorize]
public class LogErrosDashboardModel : PageModel
{
    private readonly ILogRepository _logRepository;

    public LogErrosDashboardModel(ILogRepository logRepository)
    {
        _logRepository = logRepository;
    }

    public LogDashboardStats Stats { get; set; } = new();
    public int DiasAnalise { get; set; } = 7;

    public async Task<IActionResult> OnGetAsync(int? dias = 7)
    {
        try
        {
            DiasAnalise = dias ?? 7;
            var startDate = DateTime.Now.AddDays(-DiasAnalise);

            Stats = await _logRepository.GetDashboardStatsAsync(startDate, DateTime.Now);

            return Page();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[LogErrosDashboard] Erro: {ex.Message}");
            return Page();
        }
    }
}
```
