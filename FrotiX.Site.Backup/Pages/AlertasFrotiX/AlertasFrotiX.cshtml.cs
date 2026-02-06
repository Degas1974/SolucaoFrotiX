// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: AlertasFrotiX.cshtml.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para listagem de alertas do sistema FrotiX.                       ║
// ║ Central de notificações e lembretes com SignalR real-time.                  ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • [Authorize] - Requer autenticação                                         ║
// ║ • Injeção de IUnitOfWork e IAlertasFrotiXRepository                         ║
// ║ • OnGetAsync - Carrega página de alertas                                    ║
// ║ • Dados adicionais carregados via AJAX                                      ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace FrotiX.Pages.AlertasFrotiX
{
    [Authorize]
    public class IndexModel :PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAlertasFrotiXRepository _alertasRepo;

        public IndexModel(IUnitOfWork unitOfWork , IAlertasFrotiXRepository alertasRepo)
        {
            try
            {
                _unitOfWork = unitOfWork;
                _alertasRepo = alertasRepo;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml.cs" , "IndexModel" , error);
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AlertasFrotiX.cshtml.cs" , "OnGetAsync" , error);
                TempData["erro"] = "Erro ao carregar a página de alertas";
                return Page();
            }
        }
    }
}
