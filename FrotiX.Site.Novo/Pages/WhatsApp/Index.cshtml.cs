// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : Index.cshtml.cs                                                 ║
// ║ LOCALIZAÇÃO: Pages/WhatsApp/                                                 ║
// ║ FINALIDADE : Página de configuração e pareamento do WhatsApp via Evolution.  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ • DefaultSession: Obtém nome da sessão padrão das opções ("FrotiX")          ║
// ║ • Carrega configuração de EvolutionApiOptions via IOptions                   ║
// ║ • Tela exibe QR Code para pareamento quando necessário                       ║
// ║ • Requer autenticação [Authorize]                                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE: 23 — Pages/WhatsApp | DATA: 29/01/2026                                 ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Services.WhatsApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace FrotiX.Pages.WhatsApp
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public string DefaultSession { get; private set; }

        public IndexModel(IOptions<EvolutionApiOptions> opts)
        {
            DefaultSession = opts.Value.DefaultSession ?? "FrotiX";
        }

        public void OnGet()
        { }
    }
}
