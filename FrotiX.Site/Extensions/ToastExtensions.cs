// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ToastExtensions.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Extension methods para exibição de notificações Toast.                       ║
// ║ Permite exibir mensagens de feedback ao usuário de forma simplificada.       ║
// ║                                                                              ║
// ║ MÉTODOS PARA PageModel:                                                      ║
// ║ - ShowToast()    → Exibe toast com cor e duração customizáveis               ║
// ║ - ShowSuccess()  → Toast verde (sucesso) - duração padrão 2000ms             ║
// ║ - ShowError()    → Toast vermelho (erro) - duração padrão 2000ms             ║
// ║ - ShowWarning()  → Toast amarelo (aviso) - duração padrão 2000ms             ║
// ║                                                                              ║
// ║ MÉTODOS PARA Controller:                                                     ║
// ║ - ShowToast()    → Exibe toast com cor e duração customizáveis               ║
// ║                                                                              ║
// ║ CORES DISPONÍVEIS:                                                           ║
// ║ - "Verde"    → Sucesso                                                       ║
// ║ - "Vermelho" → Erro                                                          ║
// ║ - "Amarelo"  → Aviso/Warning                                                 ║
// ║                                                                              ║
// ║ DEPENDÊNCIA: IToastService (injetado via HttpContext.RequestServices)        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace FrotiX.Extensions
{
    /// <summary>
    /// Extensions para exibição de notificações Toast.
    /// Suporta PageModel e Controller.
    /// </summary>
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
