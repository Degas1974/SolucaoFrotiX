/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ErroHelper.cs                                                                         ║
   ║ 📂 CAMINHO: Helpers/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Gerar scripts JavaScript para integração com SweetAlert Interop. Inclui montagem de            ║
   ║    mensagens e sanitização para uso seguro em strings JS.                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • MontarScriptErro(string classe, string metodo, Exception ex)                                 ║
   ║    • MontarScriptAviso(string titulo, string mensagem)                                             ║
   ║    • MontarScriptInfo(string titulo, string mensagem)                                              ║
   ║    • MontarScriptConfirmacao(string titulo, string mensagem, string textoConfirmar, string textoCancelar) ║
   ║    • Sanitize(string input)                                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System                                                                            ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;

namespace FrotiX.Helpers
    {
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ErroHelper                                                                        │
    // │ 📦 TIPO: Estática                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Montar scripts JavaScript para exibição de alertas via SweetAlert Interop.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers, Pages e Helpers de UI
    // ➡️ CHAMA       : SweetAlertInterop.* (no cliente), Sanitize()
    
    
    public static class ErroHelper
        {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MontarScriptErro                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Handlers de erro no servidor                                        │
        // │    ➡️ CHAMA       : SweetAlertInterop.ShowTratamentoErroComLinha                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar o script de erro detalhado para o SweetAlert Interop.
        
        
        
        // 📥 PARÂMETROS:
        // classe - Nome da classe origem
        // metodo - Nome do método origem
        // ex - Exceção a ser exibida
        
        
        
        // 📤 RETORNO:
        // string - Script JS pronto para execução no cliente.
        
        
        // Param classe: Nome da classe origem.
        // Param metodo: Nome do método origem.
        // Param ex: Exceção a ser exibida.
        // Returns: Script JS pronto para execução no cliente.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MontarScriptAviso                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Fluxos de alerta no servidor                                        │
        // │    ➡️ CHAMA       : SweetAlertInterop.ShowWarning, Sanitize()                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar o script de aviso para o SweetAlert Interop.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // mensagem - Mensagem do alerta
        
        
        
        // 📤 RETORNO:
        // string - Script JS pronto para execução no cliente.
        
        
        // Param titulo: Título do alerta.
        // Param mensagem: Mensagem do alerta.
        // Returns: Script JS pronto para execução no cliente.
        public static string MontarScriptAviso(string titulo, string mensagem)
            {
            return $@"SweetAlertInterop.ShowWarning(
                '{Sanitize(titulo)}',
                '{Sanitize(mensagem)}');";
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MontarScriptInfo                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Fluxos de alerta no servidor                                        │
        // │    ➡️ CHAMA       : SweetAlertInterop.ShowInfo, Sanitize()                                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar o script informativo para o SweetAlert Interop.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // mensagem - Mensagem do alerta
        
        
        
        // 📤 RETORNO:
        // string - Script JS pronto para execução no cliente.
        
        
        // Param titulo: Título do alerta.
        // Param mensagem: Mensagem do alerta.
        // Returns: Script JS pronto para execução no cliente.
        public static string MontarScriptInfo(string titulo, string mensagem)
            {
            return $@"SweetAlertInterop.ShowInfo(
                '{Sanitize(titulo)}',
                '{Sanitize(mensagem)}');";
            }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: MontarScriptConfirmacao                                                  │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Fluxos de confirmação no servidor                                    │
        // │    ➡️ CHAMA       : SweetAlertInterop.ShowConfirm, Sanitize()                             │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar o script de confirmação para o SweetAlert Interop.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // mensagem - Mensagem do alerta
        // textoConfirmar - Texto do botão de confirmação
        // textoCancelar - Texto do botão de cancelamento
        
        
        
        // 📤 RETORNO:
        // string - Script JS pronto para execução no cliente.
        
        
        // Param titulo: Título do alerta.
        // Param mensagem: Mensagem do alerta.
        // Param textoConfirmar: Texto do botão de confirmação.
        // Param textoCancelar: Texto do botão de cancelamento.
        // Returns: Script JS pronto para execução no cliente.
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

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Sanitize                                                                   │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : MontarScriptAviso/Info/Confirmacao                                  │
        // │    ➡️ CHAMA       : Replace()                                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Escapar aspas e remover quebras de linha para uso seguro em strings JS.
        
        
        
        // 📥 PARÂMETROS:
        // input - Texto original.
        
        
        
        // 📤 RETORNO:
        // string - Texto sanitizado.
        
        
        // Param input: Texto original.
        // Returns: Texto sanitizado.
        private static string Sanitize(string input)
            {
            return (input ?? "").Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            }
        }
    }

