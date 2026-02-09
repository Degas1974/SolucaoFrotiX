/* ****************************************************************************************
 * ⚡ ARQUIVO: ErrorViewModel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Disponibilizar ViewModel de erro e stub de envio de email para Identity UI.
 *
 * 📥 ENTRADAS     : RequestId e dados de envio de email.
 *
 * 📤 SAÍDAS       : ViewModel para tela de erro e serviço de email stub.
 *
 * 🔗 CHAMADA POR  : Páginas de erro e infraestrutura do Identity.
 *
 * 🔄 CHAMA        : IEmailSender.
 *
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Identity.UI.Services.
 **************************************************************************************** */

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: ErrorViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor RequestId em telas de erro.
     *
     * 📥 ENTRADAS     : RequestId.
     *
     * 📤 SAÍDAS       : ViewModel para a UI de erro.
     *
     * 🔗 CHAMADA POR  : Views de erro.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ErrorViewModel
        {
        // Identificador da requisição.
        public string RequestId { get; set; }

        // Indica se o RequestId deve ser exibido.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        }

    /****************************************************************************************
     * ⚡ SERVIÇO: EmailSender
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Implementação mínima de envio de email para Identity UI.
     *
     * 📥 ENTRADAS     : email, subject e htmlMessage.
     *
     * 📤 SAÍDAS       : Task de envio (stub).
     *
     * 🔗 CHAMADA POR  : Identity UI.
     *
     * 🔄 CHAMA        : Não se aplica.
     *
     * ⚠️ ATENÇÃO      : Stub sem provedor de email configurado.
     ****************************************************************************************/
    public class EmailSender : IEmailSender
        {
        /****************************************************************************************
         * ⚡ MÉTODO: SendEmailAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Enviar email via Identity (stub).
         *
         * 📥 ENTRADAS     : email, subject, htmlMessage.
         *
         * 📤 SAÍDAS       : Task; lança exceção por ausência de provedor.
         *
         * 🔗 CHAMADA POR  : Identity UI.
         *
         * 🔄 CHAMA        : Não se aplica.
         *
         * ⚠️ ATENÇÃO      : Implementação lança NotImplementedException.
         ****************************************************************************************/
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
            throw new NotImplementedException("No email provider is implemented by default, please Google on how to add one, like SendGrid.");
            }
        }
    }
