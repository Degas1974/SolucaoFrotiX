/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/ForgotAccount.cs                                        ║
 * ║  Descrição: Modelo para recuperação de conta/senha de usuário            ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    public class ForgotAccount
        {
        public string UserName { get; set; }
        public string Email { get; set; }
        }
    }


