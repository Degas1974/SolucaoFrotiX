/* ****************************************************************************************
 * ⚡ ARQUIVO: ForgotAccount.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar dados para recuperação de conta/senha.
 *
 * 📥 ENTRADAS     : UserName e Email.
 *
 * 📤 SAÍDAS       : DTO para fluxo de recuperação.
 *
 * 🔗 CHAMADA POR  : Telas de recuperação de acesso.
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
     * ⚡ DTO: ForgotAccount
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados básicos para recuperação de acesso.
     *
     * 📥 ENTRADAS     : UserName e Email.
     *
     * 📤 SAÍDAS       : Dados para validação e envio de instruções.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de recuperação.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ForgotAccount
        {
        // Nome de usuário.
        public string UserName { get; set; }

        // Email de contato.
        public string Email { get; set; }
        }
    }
