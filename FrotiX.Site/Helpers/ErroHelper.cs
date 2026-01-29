/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ErroHelper.cs                                                                           ║
   ║ 📂 CAMINHO: /Helpers                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Geração de scripts JavaScript para SweetAlert Interop. Métodos para montar strings JS            ║
   ║    que chamam ShowTratamentoErroComLinha, ShowWarning, ShowInfo, ShowConfirm. Sanitiza strings.    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [MontarScriptErro]       : JS para SweetAlertInterop.ShowTratamentoErroComLinha (cls,mtd,ex)    ║
   ║ 2. [MontarScriptAviso]      : JS para SweetAlertInterop.ShowWarning.... (titulo,msg) -> string     ║
   ║ 3. [MontarScriptInfo]       : JS para SweetAlertInterop.ShowInfo....... (titulo,msg) -> string     ║
   ║ 4. [MontarScriptConfirmacao]: JS para SweetAlertInterop.ShowConfirm.... (tit,msg,btns) -> string   ║
   ║ 5. [Sanitize]               : Escapa aspas e quebras de linha.......... (input) -> string          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System (sem dependências externas - gera strings puras)                           ║
   ║ 📅 ATUALIZAÇÃO: 29/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;

namespace FrotiX.Helpers
    {
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


