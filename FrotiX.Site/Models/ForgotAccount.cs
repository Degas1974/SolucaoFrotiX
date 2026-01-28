// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ForgotAccount.cs                                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para solicitação de recuperação de conta.                            ║
// ║ Usado na página de "Esqueci minha senha".                                   ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - UserName: Nome de usuário para recuperação                                ║
// ║ - Email: Email associado à conta                                            ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


