// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: AppToast.cs                                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Classe estática para exibição de notificações toast via TempData.            ║
// ║ Funciona com redirects (PRG pattern) graças ao uso de TempData.              ║
// ║                                                                              ║
// ║ MÉTODOS DISPONÍVEIS:                                                         ║
// ║ - show()         → Método base com cor, mensagem e duração                   ║
// ║ - ShowSuccess()  → Toast verde (sucesso)                                     ║
// ║ - ShowError()    → Toast vermelho (erro) - duração maior                     ║
// ║ - ShowWarning()  → Toast amarelo (aviso)                                     ║
// ║ - ShowInfo()     → Toast azul (informação)                                   ║
// ║                                                                              ║
// ║ CORES DISPONÍVEIS:                                                           ║
// ║ - "Verde", "Vermelho", "Amarelo", "Azul"                                     ║
// ║                                                                              ║
// ║ CONFIGURAÇÃO (Program.cs):                                                   ║
// ║ AppToast.Configure(httpContextAccessor, tempDataFactory);                    ║
// ║                                                                              ║
// ║ INTEGRAÇÃO FRONTEND:                                                         ║
// ║ - Lê TempData["ToastScripts"] e executa AppToast.show() no JavaScript        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 14                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrotiX.Services
{
    /// <summary>
    /// Classe estática para toasts via TempData. Funciona com redirects.
    /// </summary>
    public static class AppToast
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static ITempDataDictionaryFactory? _tempDataFactory;

        // Configure no Startup.cs
        public static void Configure(IHttpContextAccessor httpContextAccessor , ITempDataDictionaryFactory tempDataFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataFactory = tempDataFactory;
        }

        private static HttpContext? HttpContext => _httpContextAccessor?.HttpContext;

        // 🎯 MÉTODO PRINCIPAL - show MINÚSCULO - FUNCIONA COM REDIRECT
        public static void show(string color , string message , int duration = 2000)
        {
            if (HttpContext == null || _tempDataFactory == null)
                return;

            var script = $"AppToast.show('{color}', '{EscapeJs(message)}', {duration});";

            // Cria TempData para a requisição atual
            var tempData = _tempDataFactory.GetTempData(HttpContext);

            if (tempData.ContainsKey("ToastScripts"))
            {
                tempData["ToastScripts"] += script;
            }
            else
            {
                tempData["ToastScripts"] = script;
            }
        }

        // 🎯 MÉTODOS DE ATALHO
        public static void ShowSuccess(string message , int duration = 2000)
            => show("Verde" , message , duration);

        public static void ShowError(string message , int duration = 3000)
            => show("Vermelho" , message , duration);

        public static void ShowWarning(string message , int duration = 2000)
            => show("Amarelo" , message , duration);

        public static void ShowInfo(string message , int duration = 2000)
            => show("Azul" , message , duration);

        private static string EscapeJs(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return input
                .Replace("\\" , "\\\\")
                .Replace("'" , "\\'")
                .Replace("\"" , "\\\"")
                .Replace("\n" , "\\n")
                .Replace("\r" , "\\r");
        }
    }
}
