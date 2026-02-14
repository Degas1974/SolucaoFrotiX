/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: DashboardAbastecimento.cshtml.cs (Pages/Abastecimento)                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de abastecimento. Exibe métricas de consumo de combustível,                    ║
 * ║ análises por veículo/motorista e indicadores de eficiência energética da frota.                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - dados carregados via AJAX no JavaScript                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ARQUITETURA                                                                                               ║
 * ║ • PageModel mínimo - dashboard carrega dados via APIs REST                                               ║
 * ║ • Permite atualizações em tempo real e filtros dinâmicos                                                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Abastecimento
{
    public class DashboardAbastecimentoModel : PageModel
    {
        public void OnGet()
        {
            // Dados são carregados via AJAX
        }
    }
}
