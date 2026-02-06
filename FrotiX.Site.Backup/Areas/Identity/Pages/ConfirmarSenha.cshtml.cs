/* ****************************************************************************************
 * ⚡ ARQUIVO: ConfirmarSenha.cshtml.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmação de senha via token de reset.
 *
 * 📥 ENTRADAS     : Token (query), UserName (query), Input.Password, Input.ConfirmacaoPassword.
 *
 * 📤 SAÍDAS       : IActionResult (Redirect/Page) e mensagens de erro.
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST /ConfirmarSenha).
 *
 * 🔄 CHAMA        : UserManager.FindByNameAsync(), UserManager.ResetPasswordAsync().
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, UserManager.
 *
 * 📝 OBSERVAÇÕES  : Reset de senha com validação de token.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CLASSE: ConfirmarSenhaModel (PageModel)
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : PageModel para confirmação de senha após solicitação de reset.
 *                   Valida token e reseta senha do usuário.
 *
 * 📥 ENTRADAS     : Token (string) - Token de validação
 *                   UserName (string) - Nome de usuário
 *                   Input.Password (string) - Nova senha
 *                   Input.ConfirmacaoPassword (string) - Confirmação da senha
 *
 * 📤 SAÍDAS       : Redirect para ResetPasswordConfirmation ou Page com erro
 *
 * 🔗 CHAMADA POR  : Motor Razor (GET/POST de /ConfirmarSenha)
 *
 * 🔄 CHAMA        : UserManager.FindByNameAsync(), UserManager.ResetPasswordAsync()
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core Identity, UserManager
 *
 * 📝 OBSERVAÇÕES  : Implementa reset de senha com token de segurança.
 ****************************************************************************************/
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using FrotiX.Helpers;

namespace FrotiX.Areas.Identity.Pages
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  Identity / Segurança                                                              #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    [AllowAnonymous]
    public class ConfirmarSenhaModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        /****************************************************************************************
         * ⚡ CONSTRUTOR: ConfirmarSenhaModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializa dependências via injeção de dependência (UserManager).
         *
         * 📥 ENTRADAS     : [UserManager<IdentityUser>] userManager - Gerenciador de usuários
         *
         * 📤 SAÍDAS       : Instância configurada de ConfirmarSenhaModel
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI Container
         *
         * 🔄 CHAMA        : Nenhum
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity
         ****************************************************************************************/
        public ConfirmarSenhaModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Token { get; set; }
        public string UserName { get; set; }

        /****************************************************************************************
         * ⚡ CLASSE INTERNA: InputModel
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Modelo de dados para formulário de reset de senha com validações.
         *
         * 📥 ENTRADAS     : Nenhuma (propriedades setadas via binding)
         *
         * 📤 SAÍDAS       : Validação via Data Annotations
         *
         * 🔗 CHAMADA POR  : Motor Razor (binding de formulário)
         *
         * 🔄 CHAMA        : Data Annotations Validators
         *
         * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations
         *
         * 📝 OBSERVAÇÕES  : Validação de senha vs confirmação via [Compare].
         ****************************************************************************************/
        public class InputModel
        {
            [Required(ErrorMessage = "A senha é obrigatória")]
            [StringLength(100, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar Senha")]
            [Compare("Password", ErrorMessage = "A senha e a confirmação não são iguais.")]
            public string ConfirmacaoPassword { get; set; }
        }

        /****************************************************************************************
         * ⚡ MÉTODO: OnGetAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Preparar página de reset de senha (GET). Valida token e username.
         *
         * 📥 ENTRADAS     : [string] token - Token de validação
         *                   [string] userName - Nome de usuário
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - Page() ou BadRequest
         *
         * 🔗 CHAMADA POR  : Motor Razor (GET /ConfirmarSenha)
         *
         * 🔄 CHAMA        : Alerta.TratamentoErroComLinha()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core MVC
         *
         * 📝 OBSERVAÇÕES  : Valida presença de token e username.
         ****************************************************************************************/
        public async Task<IActionResult> OnGetAsync(string token, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userName))
                {
                    return BadRequest("Token e nome de usuário são obrigatórios.");
                }

                Token = token;
                UserName = userName;

                return Page();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnGetAsync", ex);
                return BadRequest("Erro ao carregar página.");
            }
        }

        /****************************************************************************************
         * ⚡ MÉTODO: OnPostAsync
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Processar reset de senha (POST). Valida token e reseta senha.
         *
         * 📥 ENTRADAS     : [string] token - Token de validação
         *                   [string] userName - Nome de usuário
         *
         * 📤 SAÍDAS       : [Task<IActionResult>] - RedirectToPage ou Page com erro
         *
         * 🔗 CHAMADA POR  : Motor Razor (POST /ConfirmarSenha)
         *
         * 🔄 CHAMA        : _userManager.FindByNameAsync(), _userManager.ResetPasswordAsync(),
         *                   Alerta.TratamentoErroComLinha()
         *
         * 📦 DEPENDÊNCIAS : ASP.NET Core Identity
         *
         * 📝 OBSERVAÇÕES  : Implementa reset de senha com token. Não revela se usuário existe.
         ****************************************************************************************/
        public async Task<IActionResult> OnPostAsync(string token, string userName)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Token = token;
                    UserName = userName;
                    return Page();
                }

                // [DADOS] Busca o usuário
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    // [SEGURANCA] Não revela que o usuário não existe
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                // [LOGICA] Reset de senha com token
                var result = await _userManager.ResetPasswordAsync(user, token, Input.Password);

                if (result.Succeeded)
                {
                    return RedirectToPage("./ResetPasswordConfirmation");
                }

                // [VALIDACAO] Adiciona erros ao ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                Token = token;
                UserName = userName;
                return Page();
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnPostAsync", ex);
                ModelState.AddModelError(string.Empty, "Erro ao processar sua solicitação.");
                Token = token;
                UserName = userName;
                return Page();
            }
        }
    }
}

