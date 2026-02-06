/* ****************************************************************************************
 * ⚡ ARQUIVO: MailRequest.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar requisição de envio de email no sistema.
 *
 * 📥 ENTRADAS     : ToEmail, Subject e Body.
 *
 * 📤 SAÍDAS       : DTO para serviços de email.
 *
 * 🔗 CHAMADA POR  : Serviços de envio de email.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : Nenhuma.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ DTO: MailRequest
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados necessários para envio de email.
     *
     * 📥 ENTRADAS     : ToEmail, Subject e Body.
     *
     * 📤 SAÍDAS       : DTO para disparo de mensagens.
     *
     * 🔗 CHAMADA POR  : Serviços de email e notificações.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class MailRequest
        {
        // Email de destino.
        public string ToEmail { get; set; }

        // Assunto do email.
        public string Subject { get; set; }

        // Corpo do email.
        public string Body { get; set; }
        }
    }
