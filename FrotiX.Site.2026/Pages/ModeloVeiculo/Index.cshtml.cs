/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: Index.cshtml.cs (Pages/ModeloVeiculo)                                                           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ PageModel mínimo para listagem de Modelos de Veículos. Dados via AJAX no ModeloVeiculoController.        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ HANDLERS                                                                                                  ║
 * ║ • OnGet() : Apenas tratamento de erro padrão (sem lógica de negócio)                                     ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ OBSERVAÇÃO                                                                                               ║
 * ║ O grid usa Syncfusion EJ2 Grid com dados carregados via ModeloVeiculoController.GetAll()                 ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FrotiX.Pages.ModeloVeiculo
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
