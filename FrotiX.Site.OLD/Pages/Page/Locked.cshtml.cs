// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : Locked.cshtml.cs                                                ║
// ║ LOCALIZAÇÃO: Pages/Page/                                                     ║
// ║ FINALIDADE : Template de página bloqueada (layout SmartAdmin).               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO: PageModel de template do tema. Não é funcionalidade FrotiX.       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE: 23 — Pages/Page (Templates) | DATA: 29/01/2026                         ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Pages.Page
{
    public class LockedModel : PageModel
    {
        private readonly ILogger<LockedModel> _logger;

        public LockedModel(ILogger<LockedModel> logger)
        {
            try
            {

                _logger = logger;
                
            }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("Locked.cshtml.cs", "LockedModel", error);
        }
}

public void OnGet()
{
    try
    {

        
    }
catch (Exception error)
{
    Alerta.TratamentoErroComLinha("Locked.cshtml.cs", "OnGet", error);
    return; // padronizado
}
}
}
}


