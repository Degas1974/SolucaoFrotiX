// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Index.cshtml.cs (AtaRegistroPrecos)                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para listagem de Atas de Registro de Preços.                      ║
// ║ Lista atas utilizadas para aquisição de veículos via licitação.             ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • OnGet vazio - dados carregados via AJAX                                   ║
// ║ • Grid com AtaRegistroPrecosController endpoints                            ║
// ║ • Tratamento de erros com Alerta.TratamentoErroComLinha                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.AtaRegistroPrecos
{
    public class IndexModel :PageModel
    {
        public void OnGet()
        {
            try
            {

            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Index.cshtml.cs" , "OnGet" , error);
                return;
            }
        }
    }
}
