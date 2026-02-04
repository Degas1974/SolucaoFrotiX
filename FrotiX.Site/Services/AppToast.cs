/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AppToast.cs                                                                             ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Helper estático para Toast notifications via TempData. Funciona com redirect.          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Configure(), show(color,message,duration), Success(), Error(), Info(), Warning()         ║
   ║ 🔗 DEPS: IHttpContextAccessor, ITempDataDictionaryFactory | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

// Arquivo: Services/AppToast.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrotiX.Services
{
    public static class AppToast
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static ITempDataDictionaryFactory? _tempDataFactory;

        // Configure no Startup.cs
        /***********************************************************************************
         * ⚡ FUNÇÃO: Configure
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar os acessadores estáticos necessários para o AppToast
         *                   funcionar em qualquer lugar do Controller ou Razor Page
         *
         * 📥 ENTRADAS     : httpContextAccessor [IHttpContextAccessor] - Acesso ao HttpContext
         *                   tempDataFactory [ITempDataDictionaryFactory] - Factory de TempData
         *
         * 📤 SAÍDAS       : void - Configuração static
         *
         * ⬅️ CHAMADO POR  : Startup.cs → ConfigureServices() [durante DI]
         *
         * ➡️ CHAMA        : Nenhuma dependência (setup apenas)
         *
         * 📝 OBSERVAÇÕES  : DEVE ser chamado no ConfigureServices durante DI. Sem isso,
         *                   _httpContextAccessor será null e show() não funcionará.
         ***********************************************************************************/
        public static void Configure(IHttpContextAccessor httpContextAccessor , ITempDataDictionaryFactory tempDataFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataFactory = tempDataFactory;
        }

        private static HttpContext? HttpContext => _httpContextAccessor?.HttpContext;

        // 🎯 MÉTODO PRINCIPAL - show MINÚSCULO - FUNCIONA COM REDIRECT
        /***********************************************************************************
         * ⚡ FUNÇÃO: show
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Adicionar comando de toast à TempData para executar após redirect.
         *                   Habilita mostrar notificações em Razor Pages após POST/redirect
         *
         * 📥 ENTRADAS     : color [string] - Cor do toast (Verde|Vermelho|Amarelo|Azul)
         *                   message [string] - Mensagem a exibir
         *                   duration [int] - Duração em ms (padrão 2000)
         *
         * 📤 SAÍDAS       : void - Enqueue na TempData
         *
         * ⬅️ CHAMADO POR  : Controllers, Handlers de Razor Pages (qualquer método público)
         *
         * ➡️ CHAMA        : EscapeJs() [linha 91]
         *                   _tempDataFactory.GetTempData() [DI]
         *
         * 📝 OBSERVAÇÕES  : Nome MINÚSCULO intencional (show vs ShowSuccess). Acumula
         *                   scripts em TempData["ToastScripts"] para exec no frontend.
         ***********************************************************************************/
        public static void show(string color , string message , int duration = 2000)
        {
            if (HttpContext == null || _tempDataFactory == null)
                return;

            // [DADOS] Montar comando JavaScript para executar no cliente
            // Escapar string para evitar injection de quotes
            var script = $"AppToast.show('{color}', '{EscapeJs(message)}', {duration});";

            // [UI] Recuperar TempData da requisição atual ou criar nova
            var tempData = _tempDataFactory.GetTempData(HttpContext);

            // [LOGICA] Acumular scripts na chave para permitir múltiplas chamadas
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
