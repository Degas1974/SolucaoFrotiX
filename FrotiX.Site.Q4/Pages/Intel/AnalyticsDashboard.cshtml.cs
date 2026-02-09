/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: AnalyticsDashboard.cshtml.cs (Pages/Intel)                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para dashboard de analytics/inteligência. Exibe métricas e indicadores de desempenho          ║
 * ║ da frota. Inicializa variável global com ponto do usuário corrente para personalização.                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Obtém usuário corrente via Claims e define Settings.GlobalVariables.gPontoUsuario           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ VARIÁVEIS GLOBAIS                                                                                         ║
 * ║ • Settings.GlobalVariables.gPontoUsuario : Ponto funcional do usuário logado                            ║
 * ║   - Usado para filtrar dados por unidade/setor do usuário                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ AUTENTICAÇÃO                                                                                              ║
 * ║ • ClaimsPrincipal.User - Usuário autenticado via Identity                                                ║
 * ║ • ClaimTypes.NameIdentifier - ID único do usuário                                                        ║
 * ║ • AspNetUsers.Ponto - Ponto funcional para personalização                                                ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • IUnitOfWork - Repository pattern                                                                       ║
 * ║ • ILogger - Logging                                                                                      ║
 * ║ • AspNetUsers - Dados do usuário                                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace FrotiX.Pages.Intel
{
    public class AnalyticsDashboardModel :PageModel
    {
        private readonly ILogger<AnalyticsDashboardModel> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsDashboardModel(ILogger<AnalyticsDashboardModel> logger , IUnitOfWork unitOfWork)
        {
            try
            {
                _logger = logger;
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AnalyticsDashboard.cshtml.cs" , "AnalyticsDashboardModel" , error);
            }
        }

        public void OnGet()
        {
            try
            {
                string usuarioCorrentePonto;
                // Pega o usuário corrente
                // =======================
                ClaimsPrincipal currentUser = User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == currentUserID);
                usuarioCorrentePonto = objUsuario.Ponto;
                Settings.GlobalVariables.gPontoUsuario = objUsuario.Ponto;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("AnalyticsDashboard.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
