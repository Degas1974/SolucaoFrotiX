/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Extensions/ToastExtensions.cs                                  ║
 * ║  Descrição: Métodos de extensão para exibir notificações Toast em        ║
 * ║             PageModel e Controller. Integrado com IToastService.         ║
 * ║             Suporta cores: Verde (sucesso), Vermelho (erro), Amarelo.    ║
 * ║  Data: 28/01/2026 | LOTE: 21                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace FrotiX.Extensions
{
    public static class ToastExtensions
    {
        // Extension para PageModel
        public static void ShowToast(this PageModel page , string texto , string cor = "Verde" , int duracao = 2000)
        {
            var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
            toastService?.Show(texto , cor , duracao);
        }

        public static void ShowSuccess(this PageModel page , string texto , int duracao = 2000)
        {
            page.ShowToast(texto , "Verde" , duracao);
        }

        public static void ShowError(this PageModel page , string texto , int duracao = 2000)
        {
            page.ShowToast(texto , "Vermelho" , duracao);
        }

        public static void ShowWarning(this PageModel page , string texto , int duracao = 2000)
        {
            page.ShowToast(texto , "Amarelo" , duracao);
        }

        // Extension para Controller (caso use também)
        public static void ShowToast(this Controller controller , string texto , string cor = "Verde" , int duracao = 2000)
        {
            var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
            toastService?.Show(texto , cor , duracao);
        }
    }
}
