/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: LoginView.cs                                                                            ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: ViewModel para tela de login com validação de campos.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: LoginView                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations                                                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace FrotiX.Models
    {
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Dados de autenticação do usuário.
    // ==================================================================================================
    public class LoginView
        {
        // Nome de usuário.
        [Required]
        [UIHint("username")]
        public string UserName { get; set; }

        // Senha do usuário.
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
        }
    }

