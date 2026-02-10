/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ToastService.cs                                                                         ║
   ║ 📂 CAMINHO: /Services                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: IToastService + ToastService. Toast notifications via TempData.                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Show, ShowSuccess, ShowError, ShowWarning, GetJavaScriptCall, ShowMultiple               ║
   ║ 🔗 DEPS: IHttpContextAccessor, ITempDataDictionaryFactory | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

// Arquivo: Services/ToastService.cs

using FrotiX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text.Json;

namespace FrotiX.Services
{
    public interface IToastService
    {
        void Show(string texto , string cor = "Verde" , int duracao = 2000);

        void ShowSuccess(string texto , int duracao = 2000);

        void ShowError(string texto , int duracao = 2000);

        void ShowWarning(string texto , int duracao = 2000);

        // Para uso direto com JavaScript quando necessário
        string GetJavaScriptCall(string texto , string cor = "Verde" , int duracao = 2000);

        // Para múltiplas mensagens
        void ShowMultiple(params ToastMessage[] messages);
    }

    public class ToastService : IToastService
    {
        private readonly ITempDataDictionary _tempData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string TOAST_KEY = "ToastMessages";

        // Construtor para injeção em PageModel
        public ToastService(ITempDataDictionaryFactory tempDataFactory , IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                _tempData = tempDataFactory.GetTempData(httpContext);
            }
        }

        public void Show(string texto , string cor = "Verde" , int duracao = 2000)
        {
            if (_tempData == null)
                return;

            var message = new ToastMessage(texto , cor , duracao);
            AddMessageToQueue(message);
        }

        public void ShowSuccess(string texto , int duracao = 2000)
        {
            Show(texto , "Verde" , duracao);
        }

        public void ShowError(string texto , int duracao = 2000)
        {
            Show(texto , "Vermelho" , duracao);
        }

        public void ShowWarning(string texto , int duracao = 2000)
        {
            Show(texto , "Amarelo" , duracao);
        }

        public void ShowMultiple(params ToastMessage[] messages)
        {
            foreach (var message in messages)
            {
                AddMessageToQueue(message);
            }
        }

        public string GetJavaScriptCall(string texto , string cor = "Verde" , int duracao = 2000)
        {
            // Escapa as aspas no texto para evitar problemas no JavaScript
            var textoEscapado = texto.Replace("'" , "\\'").Replace("\"" , "\\\"");
            return $"AppToast.show('{cor}', '{textoEscapado}', {duracao});";
        }

        private void AddMessageToQueue(ToastMessage message)
        {
            var messages = GetCurrentMessages();
            messages.Add(message);

            // Serializa a lista atualizada
            _tempData[TOAST_KEY] = JsonSerializer.Serialize(messages);
        }

        private List<ToastMessage> GetCurrentMessages()
        {
            if (_tempData[TOAST_KEY] is string json)
            {
                try
                {
                    return JsonSerializer.Deserialize<List<ToastMessage>>(json) ?? new List<ToastMessage>();
                }
                catch
                {
                    return new List<ToastMessage>();
                }
            }

            return new List<ToastMessage>();
        }
    }
}
