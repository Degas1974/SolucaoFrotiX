/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: PaginaPrincipal.cshtml.cs (Pages/Intel)                                                         ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel para página principal do módulo Intel. Serve como ponto de entrada e hub                      ║
 * ║ central para navegação entre os diferentes dashboards e análises disponíveis.                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Handler vazio - navegação definida na view                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DEPENDÊNCIAS                                                                                              ║
 * ║ • ILogger - Logging                                                                                      ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Pages.Intel
{
    public class PaginaPrincipalModel : PageModel
    {
        private readonly ILogger<PaginaPrincipalModel> _logger;

        public PaginaPrincipalModel(ILogger<PaginaPrincipalModel> logger)
        {
            try
            {

                _logger = logger;
                
            }
        catch (Exception error)
        {
            Alerta.TratamentoErroComLinha("PaginaPrincipal.cshtml.cs", "PaginaPrincipalModel", error);
        }
}

public void OnGet()
{
    try
    {

        
    }
catch (Exception error)
{
    Alerta.TratamentoErroComLinha("PaginaPrincipal.cshtml.cs", "OnGet", error);
    return; // padronizado
}
}
}
}


