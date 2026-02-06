/* ****************************************************************************************
 * ⚡ ARQUIVO: LoginView.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Representar dados de login com validação de campos.
 *
 * 📥 ENTRADAS     : UserName e Password.
 *
 * 📤 SAÍDAS       : ViewModel para autenticação.
 *
 * 🔗 CHAMADA POR  : Tela de login.
 *
 * 🔄 CHAMA        : DataAnnotations.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: LoginView
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar credenciais de autenticação.
     *
     * 📥 ENTRADAS     : UserName e Password.
     *
     * 📤 SAÍDAS       : ViewModel para validação e login.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de autenticação.
     *
     * 🔄 CHAMA        : Required, UIHint.
     ****************************************************************************************/
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
