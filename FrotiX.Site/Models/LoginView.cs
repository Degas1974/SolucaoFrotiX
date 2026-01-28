// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LoginView.cs                                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo para tela de login do sistema.                                       ║
// ║ Usado em páginas de autenticação alternativas.                              ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - UserName: Nome de usuário (obrigatório)                                   ║
// ║ - Password: Senha (obrigatório)                                             ║
// ║                                                                              ║
// ║ UI HINTS: username, password para renderização específica                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace FrotiX.Models
    {
    public class LoginView
        {
        [Required]
        [UIHint("username")]
        public string UserName { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        }
    }


