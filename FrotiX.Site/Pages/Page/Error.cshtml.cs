// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : Error.cshtml.cs                                                 ║
// ║ LOCALIZAÇÃO: Pages/Page/                                                     ║
// ║ FINALIDADE : Página de erro padrão do ASP.NET Core.                          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO: Exibe RequestId para depuração. Sem cache. Ignora antiforgery.     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE: 23 — Pages/Page | DATA: 29/01/2026                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public string RequestId { get; set; } = "";
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        [BindProperty(SupportsGet = true)]
        public int? StatusCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Message { get; set; }

        public string ErrorTitle { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public string ErrorIcon { get; set; } = "";
        public bool ShowLoginButton { get; set; } = false;

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // Se não foi passado status code, tentar pegar do contexto
            if (!StatusCode.HasValue)
            {
                StatusCode =
                    HttpContext.Response.StatusCode != 200 ? HttpContext.Response.StatusCode : 500;
            }

            SetupErrorDetails();

            _logger.LogWarning(
                "Página de erro acessada. Status: {StatusCode}, RequestId: {RequestId}",
                StatusCode,
                RequestId
            );
        }

        private void SetupErrorDetails()
        {
            switch (StatusCode)
            {
                case 400:
                    ErrorTitle = "Solicitação inválida";
                    ErrorMessage =
                        Message
                        ?? "A solicitação não pôde ser processada devido a dados inválidos.";
                    ErrorIcon = "fas fa-exclamation-circle";
                    break;

                case 401:
                    ErrorTitle = "Acesso não autorizado";
                    ErrorMessage = Message ?? "Você precisa fazer login para acessar esta página.";
                    ErrorIcon = "fas fa-lock";
                    ShowLoginButton = true;
                    break;

                case 403:
                    ErrorTitle = "Acesso negado";
                    ErrorMessage = Message ?? "Você não tem permissão para acessar esta página.";
                    ErrorIcon = "fas fa-ban";
                    break;

                case 404:
                    ErrorTitle = "Página não encontrada";
                    ErrorMessage =
                        Message ?? "A página que você está procurando não pôde ser encontrada.";
                    ErrorIcon = "fas fa-exclamation-triangle";
                    break;

                case 408:
                    ErrorTitle = "Tempo limite excedido";
                    ErrorMessage = Message ?? "A solicitação demorou muito para ser processada.";
                    ErrorIcon = "fas fa-clock";
                    break;

                case 429:
                    ErrorTitle = "Muitas solicitações";
                    ErrorMessage =
                        Message
                        ?? "Você fez muitas solicitações em pouco tempo. Tente novamente em instantes.";
                    ErrorIcon = "fas fa-tachometer-alt";
                    break;

                case 500:
                    ErrorTitle = "Erro interno do servidor";
                    ErrorMessage =
                        Message ?? "Ocorreu um erro interno. Tente novamente em alguns instantes.";
                    ErrorIcon = "fas fa-server";
                    break;

                case 502:
                    ErrorTitle = "Gateway inválido";
                    ErrorMessage = Message ?? "O servidor está temporariamente indisponível.";
                    ErrorIcon = "fas fa-plug";
                    break;

                case 503:
                    ErrorTitle = "Serviço indisponível";
                    ErrorMessage =
                        Message
                        ?? "O serviço está temporariamente indisponível. Tente novamente mais tarde.";
                    ErrorIcon = "fas fa-tools";
                    break;

                default:
                    ErrorTitle = "Erro inesperado";
                    ErrorMessage =
                        Message
                        ?? "Ocorreu um erro inesperado. Tente novamente ou entre em contato com o suporte.";
                    ErrorIcon = "fas fa-question-circle";
                    break;
            }
        }
    }
}
