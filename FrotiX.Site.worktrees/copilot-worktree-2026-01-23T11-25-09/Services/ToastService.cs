/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  SERVIÇOS - TOAST NOTIFICATIONS (TempData)                                          #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using FrotiX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Text.Json;

namespace FrotiX.Services
{
    public interface IToastService
    {
        void Show(string texto, string cor = "Verde", int duracao = 2000);
        void ShowSuccess(string texto, int duracao = 2000);
        void ShowError(string texto, int duracao = 2000);
        void ShowWarning(string texto, int duracao = 2000);
        string GetJavaScriptCall(string texto, string cor = "Verde", int duracao = 2000);
        void ShowMultiple(params ToastMessage[] messages);
    }

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: ToastService                                                        ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Serviço injetável de notificações Toast usando TempData. Similar ao      ║
    /// ║    AppToast mas com injeção de dependência via interface.                    ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
    /// ║    Padrão de notificações para Razor Pages e Controllers com suporte a DI.  ║
    /// ║    Permite múltiplas mensagens e persistência entre requisições.            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • Show() → Exibe toast customizado                                        ║
    /// ║    • ShowSuccess/Error/Warning() → Atalhos por tipo                          ║
    /// ║    • ShowMultiple() → Múltiplas notificações simultâneas                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: INTERNA - Serviço de infraestrutura                               ║
    /// ║    • Arquivos relacionados: AppToast.cs, _Layout.cshtml                     ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public class ToastService : IToastService
    {
        private readonly ITempDataDictionary _tempData;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string TOAST_KEY = "ToastMessages";

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ToastService (Construtor)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o serviço com acesso a TempData via factory e HttpContext.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • tempDataFactory: Factory para criar TempData                            ║
        /// ║    • httpContextAccessor: Acesso ao contexto HTTP                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public ToastService(ITempDataDictionaryFactory tempDataFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                _tempData = tempDataFactory.GetTempData(httpContext);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Show                                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe notificação toast com cor e duração personalizadas.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • texto: Mensagem a exibir                                                ║
        /// ║    • cor: Verde, Vermelho, Amarelo, Azul (padrão: Verde)                     ║
        /// ║    • duracao: Tempo em ms (padrão: 2000)                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void Show(string texto, string cor = "Verde", int duracao = 2000)
        {
            // [REGRA] Validação de TempData disponível
            if (_tempData == null)
                return;

            // [DADOS] Cria mensagem estruturada
            var message = new ToastMessage(texto, cor, duracao);
            AddMessageToQueue(message);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ShowSuccess, ShowError, ShowWarning                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Métodos de atalho com cores pré-definidas.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public void ShowSuccess(string texto, int duracao = 2000)
        {
            Show(texto, "Verde", duracao);
        }

        public void ShowError(string texto, int duracao = 2000)
        {
            Show(texto, "Vermelho", duracao);
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
