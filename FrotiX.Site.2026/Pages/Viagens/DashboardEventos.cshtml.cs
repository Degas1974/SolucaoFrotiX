/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: DashboardEventos.cshtml.cs (Pages/Viagens)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de eventos. Página carregada via JavaScript,                                   ║
 * ║ OnGet vazio pois lógica de dados está no frontend/AJAX.                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via JavaScript                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ NOTA: Arquivo em Pages/Viagens mas namespace FrotiX.Pages.Eventos                                       ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Eventos
{
    public class DashboardEventosModel : PageModel
    {
        public void OnGet()
        {
            // Página carregada via JavaScript
        }
    }
}
