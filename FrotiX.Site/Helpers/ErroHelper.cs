// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ErroHelper.cs                                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Helper para geração de scripts JavaScript de alertas SweetAlert.             ║
// ║ Usado para injetar alertas no frontend via ViewBag/TempData.                 ║
// ║                                                                              ║
// ║ MÉTODOS DISPONÍVEIS:                                                         ║
// ║ - MontarScriptErro()        → Script de erro com stack trace                 ║
// ║ - MontarScriptAviso()       → Script de warning amarelo                      ║
// ║ - MontarScriptInfo()        → Script de informação azul                      ║
// ║ - MontarScriptConfirmacao() → Script com Sim/Cancelar                        ║
// ║                                                                              ║
// ║ SANITIZAÇÃO:                                                                 ║
// ║ - Escapa aspas simples (\')                                                  ║
// ║ - Remove quebras de linha (\r\n)                                             ║
// ║                                                                              ║
// ║ INTEGRAÇÃO:                                                                  ║
// ║ - SweetAlertInterop.ShowTratamentoErroComLinha()                             ║
// ║ - SweetAlertInterop.ShowWarning()                                            ║
// ║ - SweetAlertInterop.ShowInfo()                                               ║
// ║ - SweetAlertInterop.ShowConfirm()                                            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 12                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;

namespace FrotiX.Helpers
    {
    /// <summary>
    /// Helper para geração de scripts JavaScript de alertas SweetAlert.
    /// </summary>
    public static class ErroHelper
        {
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


