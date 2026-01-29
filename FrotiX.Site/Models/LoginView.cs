/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: LoginView.cs                                                                            ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: ViewModel para tela de login do sistema com validação de campos.                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: UserName [Required], Password [Required]                                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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


