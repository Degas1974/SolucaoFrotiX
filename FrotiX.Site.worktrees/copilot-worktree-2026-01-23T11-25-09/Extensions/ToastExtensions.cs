/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Extensions para exibição de Toasts (Notificações) em Pages e Controllers.
 * =========================================================================================
 */

using FrotiX.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using FrotiX.Helpers;
using System;

namespace FrotiX.Extensions
{
    public static class ToastExtensions
    {
        // Extension para PageModel
        public static void ShowToast(this PageModel page , string texto , string cor = "Verde" , int duracao = 2000)
        {
            try
            {
                var toastService = page.HttpContext.RequestServices.GetService<IToastService>();
                toastService?.Show(texto , cor , duracao);
            }
            catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_PageModel", ex); }
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
            try
            {
                var toastService = controller.HttpContext.RequestServices.GetService<IToastService>();
                toastService?.Show(texto , cor , duracao);
            }
             catch (Exception ex) { Alerta.TratamentoErroComLinha("ToastExtensions.cs", "ShowToast_Controller", ex); }
         }
     }
 }
