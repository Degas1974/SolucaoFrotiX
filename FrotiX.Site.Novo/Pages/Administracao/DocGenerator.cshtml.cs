/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║                                                                          ║
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║                                                                          ║
 * ║  Este arquivo está documentado em:                                       ║
 * ║  📄 Documentacao/Pages/Administracao/DocGenerator.md                     ║
 * ║                                                                          ║
 * ║  Última atualização: 13/01/2026                                          ║
 * ║                                                                          ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrotiX.Pages.Administracao
{
    [Authorize]
    public class DocGeneratorModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
