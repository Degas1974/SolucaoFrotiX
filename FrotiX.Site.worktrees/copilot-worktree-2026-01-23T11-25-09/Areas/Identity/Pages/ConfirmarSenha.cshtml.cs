/* > ---------------------------------------------------------------------------------------
 > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
 > ---------------------------------------------------------------------------------------
 > 🆔 **Nome:** ConfirmarSenha.cshtml.cs
 > 📍 **Local:** Areas/Identity/Pages
 > ❓ **Por que existo?** Página de confirmação de senha para usuários não autenticados.
 > 🔗 **Relevância:** Alta
 > ---------------------------------------------------------------------------------------
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FrotiX.Models;
using FrotiX.Helpers;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using FrotiX.Services;

namespace FrotiX.Areas.Identity.Pages
{
    [AllowAnonymous]
    public class ConfirmarSenha : PageModel
    {
        /*
        ### 📡 SAÍDA (Quem este arquivo chama?)
        * **Injeção de Dependência:** `_signInManager`, `_logger`, `_log`
        * **Autenticação:** `HttpContext.SignOutAsync`
        * **Helper:** `Url.Content`

        ### 🧲 ENTRADA (Quem pode chamar este arquivo?)
        * **Métodos Públicos:** `OnGetAsync`, `OnPostAsync`
        */
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ConfirmarSenhaModel> _logger;
        private readonly ILogService _log;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ConfirmarSenha                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa dependências da página de confirmação de senha.                ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Prepara o fluxo de validação de senha para usuários não autenticados.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • signInManager (SignInManager<IdentityUser>): gerenciador de login.      ║
        /// ║    • logger (ILogger<ConfirmarSenhaModel>): logger da página.                ║
        /// ║    • log (ILogService): serviço de log FrotiX.                               ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • void: constrói o PageModel.                                             ║
        /// ║    • Significado: prepara o fluxo de confirmação.                            ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Nenhuma (apenas inicialização de dependências).                          ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • DI container do ASP.NET Core.                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmarSenha.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public ConfirmarSenha(SignInManager<IdentityUser> signInManager, ILogger<ConfirmarSenhaModel> logger, ILogService log)
        {
            _signInManager = signInManager;
            _logger = logger;
            _log = log;
        }

        [BindProperty]
        public ConfirmarSenhaModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class ConfirmarSenhaModel
        {
            [Required(ErrorMessage = "A senha é obrigatória!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Compare(nameof(Password), ErrorMessage = "A confirmação da senha não combina com a senha!")]
            [DataType(DataType.Password)]
            public string ConfirmacaoPassword { get; set; }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnGetAsync                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa a página de confirmação, limpando logins externos.             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Garante um estado limpo de autenticação antes de confirmar a senha.       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno após a ação (opcional).              ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Task: operação assíncrona sem retorno explícito.                        ║
        /// ║    • Significado: prepara o GET da página.                                   ║
        /// ║    • Consumidor: pipeline Razor Pages.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • HttpContext.SignOutAsync() → limpa cookies externos.                    ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Roteamento Razor Pages (GET /Identity/ConfirmarSenha).                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmarSenha.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task OnGetAsync(string returnUrl = null)
        {
            try
            {
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Inicialização e Limpeza
                // Exibe mensagens de erro anteriores e limpa sessão externa
                // ═══════════════════════════════════════════════════════════════
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }

                returnUrl = returnUrl ?? Url.Content("~/");

                // [REGRA] Limpar cookie externo para garantir login limpo
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

                ReturnUrl = returnUrl;
            }
            catch (Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                _log.Error("Erro ao carregar tela de ConfirmarSenha", error);
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnGetAsync", error);
            }
        }


        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: OnPostAsync                                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa a confirmação de senha submetida pelo formulário.                ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Centraliza a validação e redirecionamento de confirmação de senha.        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • returnUrl (string): URL de retorno (opcional).                          ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: redirecionamento para Login ou reexibição com erro.      ║
        /// ║    • Significado: confirma a validação e encerra o fluxo.                    ║
        /// ║    • Consumidor: fluxo de UI do Identity.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • RedirectToPage() → redireciona usuário.                                 ║
        /// ║                                                                              ║
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Submit do formulário na view ConfirmarSenha.cshtml.                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: ConfirmarSenha.cshtml                            ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                // ═══════════════════════════════════════════════════════════════
                // 🔹 BLOCO: Validação
                // Verifica se o modelo recebido é válido
                // ═══════════════════════════════════════════════════════════════
                
                // [REGRA] Validar estado do modelo
                if (ModelState.IsValid)
                {
                    _log.Info("Senha confirmada com sucesso.");
                }

                // Se chegou até aqui, redireciona para Login
                return RedirectToPage("Account/LoginFrotiX");
            }
            catch (Exception error)
            {
                // 🛡️ Blindagem Padronizada FrotiX
                _log.Error("Erro ao processar confirmação de senha", error);
                Alerta.TratamentoErroComLinha("ConfirmarSenha.cshtml.cs", "OnPostAsync", error);
                return Page();
            }
        }
    }
}


