// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Index.cshtml.cs (Fornecedor)                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ PageModel para listagem de fornecedores (locadoras, oficinas).              ║
// ║ Cadastro base para contratos, atas e manutenções.                           ║
// ║                                                                              ║
// ║ CARACTERÍSTICAS:                                                              ║
// ║ • OnGet vazio - dados carregados via AJAX                                   ║
// ║ • Grid com FornecedorController endpoints                                   ║
// ║ • Tratamento de erros com Alerta.TratamentoErroComLinha                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 19                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.Fornecedor
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
