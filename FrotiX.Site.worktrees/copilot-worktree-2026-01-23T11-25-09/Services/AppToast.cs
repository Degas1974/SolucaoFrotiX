/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - TOAST NOTIFICATIONS                                                     #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FrotiX.Services
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: AppToast                                                            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço estático para exibição de notificações Toast no FrotiX.           ║
    /// ║    Utiliza TempData para persistir mensagens entre requisições (Redirect).   ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Sistema padronizado de feedback visual leve e não-intrusivo. Substitui   ║
    /// ║    alertas nativos do navegador por notificações estilizadas no canto da    ║
    /// ║    tela com cores por tipo (sucesso=verde, erro=vermelho, etc).             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • show() → Método principal para exibir toast                             ║
    /// ║    • ShowSuccess() → Atalho para toast verde (sucesso)                       ║
    /// ║    • ShowError() → Atalho para toast vermelho (erro)                         ║
    /// ║    • ShowWarning() → Atalho para toast amarelo (aviso)                       ║
    /// ║    • ShowInfo() → Atalho para toast azul (informação)                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura do sistema                    ║
    /// ║    • Arquivos relacionados: wwwroot/js/apptoast.js, _Layout.cshtml          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public static class AppToast
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        private static ITempDataDictionaryFactory? _tempDataFactory;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Configure                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Configura as dependências do serviço. Deve ser chamado no Startup.cs.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • httpContextAccessor: Acesso ao contexto HTTP                            ║
        /// ║    • tempDataFactory: Factory para criar TempData                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Configure(IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _tempDataFactory = tempDataFactory;
        }

        private static HttpContext? HttpContext => _httpContextAccessor?.HttpContext;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: show                                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Método principal que cria e armazena script de toast em TempData.         ║
        /// ║    Funciona com RedirectToAction preservando a mensagem entre requisições.   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • color (string): Cor do toast (Verde, Vermelho, Amarelo, Azul)           ║
        /// ║    • message (string): Mensagem a ser exibida                                ║
        /// ║    • duration (int): Duração em milissegundos (padrão: 2000)                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void show(string color, string message, int duration = 2000)
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

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ShowSuccess, ShowError, ShowWarning, ShowInfo                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Métodos de atalho para exibir toasts com cores pré-definidas.             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void ShowSuccess(string message, int duration = 2000)
            => show("Verde", message, duration);

        public static void ShowError(string message, int duration = 3000)
            => show("Vermelho", message, duration);

        public static void ShowWarning(string message, int duration = 2000)
            => show("Amarelo", message, duration);

        public static void ShowInfo(string message, int duration = 2000)
            => show("Azul", message, duration);

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: EscapeJs (Helper)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Escapa caracteres especiais para prevenir quebra do JavaScript gerado.    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
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
