// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LogErros.cshtml.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para visualização do log de erros do sistema.                     ║
// ║ Exibe erros capturados por Alerta.TratamentoErroComLinha.                   ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • [AllowAnonymous] - Acesso sem autenticação (debug)                        ║
// ║ • OnGet vazio - dados carregados via JavaScript                             ║
// ║ • Lê arquivos de log do sistema                                             ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Administracao
{
    [AllowAnonymous]
    public class LogErrosModel : PageModel
    {
        public void OnGet()
        {
            try
            {

            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LogErros.cshtml.cs", "OnGet", error);
                return;
            }
        }
    }
}
