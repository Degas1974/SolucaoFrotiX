/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: DashboardLavagem.cshtml.cs (Pages/Manutencao)                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de lavagem de veículos. Exibe métricas e estatísticas do serviço               ║
 * ║ de lavagem da frota. Todos os dados são carregados via API no JavaScript (SPA-like).                    ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via JavaScript/API                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ARQUITETURA                                                                                               ║
 * ║ • Este PageModel é mínimo por design                                                                     ║
 * ║ • Dashboard consome APIs REST para gráficos e indicadores                                                ║
 * ║ • Permite carregamento assíncrono e atualização dinâmica                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Manutencao
{
    public class DashboardLavagemModel : PageModel
    {
        public void OnGet()
        {
            // Dashboard de Lavagem nao precisa de dados pre-carregados
            // Todos os dados sao carregados via API no JavaScript
        }
    }
}
