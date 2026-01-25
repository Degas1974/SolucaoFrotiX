/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HELPERS - MONTAGEM DE SCRIPTS SWEETALERT                                           #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using System;

namespace FrotiX.Helpers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ErroHelper                                                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Helper estático para geração de scripts JavaScript de SweetAlert.         ║
    /// ║    Monta strings de invocação de alertas para injeção em Blazor/Razor.       ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA:                                                              ║
    /// ║    Integração Backend→Frontend. Permite Controllers gerarem alertas JS.      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES:                                                                  ║
    /// ║    • MontarScriptErro() → Script de erro com stack trace                     ║
    /// ║    • MontarScriptAviso/Info() → Scripts de notificação                       ║
    /// ║    • MontarScriptConfirmacao() → Script de confirmação                       ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public static class ErroHelper
    {
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MontarScriptErro                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera script JS para SweetAlert de erro com classe, método e exception.    ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static string MontarScriptErro(string classe, string metodo, Exception ex)
            {
            if (ex == null)
                return string.Empty;

            string mensagem = (ex.Message ?? "Erro desconhecido").Replace("'", "\\'");
            string stack = (ex.StackTrace ?? "")
                .Replace("'", "\\'")
                .Replace("\r", "")
                .Replace("\n", " ");

            return $@"SweetAlertInterop.ShowTratamentoErroComLinha(
                '{classe}', '{metodo}', {{ message: '{mensagem}', stack: '{stack}' }});";
            }

        public static string MontarScriptAviso(string titulo, string mensagem)
            {
            return $@"SweetAlertInterop.ShowWarning(
                '{Sanitize(titulo)}',
                '{Sanitize(mensagem)}');";
            }

        public static string MontarScriptInfo(string titulo, string mensagem)
            {
            return $@"SweetAlertInterop.ShowInfo(
                '{Sanitize(titulo)}',
                '{Sanitize(mensagem)}');";
            }

        public static string MontarScriptConfirmacao(
            string titulo,
            string mensagem,
            string textoConfirmar,
            string textoCancelar
        )
            {
            return $@"SweetAlertInterop.ShowConfirm(
                '{Sanitize(titulo)}',
                '{Sanitize(mensagem)}',
                '{Sanitize(textoConfirmar)}',
                '{Sanitize(textoCancelar)}');";
            }

        private static string Sanitize(string input)
            {
            return (input ?? "").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            }
        }
    }


