/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LogErrosDashboard.cshtml.cs                                                            ║
   ║ 📂 CAMINHO: /Pages/Administracao                                                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Dashboard de métricas e análises de logs de erros do sistema FrotiX.                 ║
   ║              Exibe gráficos, rankings, tendências e alertas em tempo real.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 FUNCIONALIDADES:                                                                                ║
   ║ • Estatísticas gerais (total, erros, warnings, etc.)                                              ║
   ║ • Gráfico de timeline (erros por hora/dia)                                                        ║
   ║ • Gráfico de distribuição por tipo (pizza)                                                        ║
   ║ • Top 10 páginas com mais erros                                                                   ║
   ║ • Top 10 erros mais frequentes                                                                    ║
   ║ • Análise de horários de pico                                                                     ║
   ║ • Detecção de anomalias                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: ILogRepository                                                                            ║
   ║ 📅 31/01/2026 | 👤 Claude Code | 📝 v1.0                                                           ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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

    // ====== PROPRIEDADES PARA A VIEW ======

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
