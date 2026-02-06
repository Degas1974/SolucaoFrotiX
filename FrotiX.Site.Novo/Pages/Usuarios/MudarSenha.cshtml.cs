/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ MudarSenha.cshtml.cs                                                                                ║
 * ║ PageModel para alteração de senha                                                                   ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    MudarSenha.cshtml.cs                                                                    ║
 * ║ @local      Pages/Usuarios/                                                                               ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Gestão de usuários                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrotiX.Pages.Usuarios
{
    [Authorize]
    public class MudarSenhaModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService _notyf;
        private readonly ILogger<MudarSenhaModel> _logger;

        public MudarSenhaModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork,
            INotyfService notyf,
            ILogger<MudarSenhaModel> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _notyf = notyf;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = string.Empty;

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string UsuarioExibicao { get; private set; } = string.Empty;
        public string? NomeCompleto { get; private set; }
        public string? Ponto { get; private set; }
        public string? FotoBase64 { get; private set; }
        public string StatusMessage { get; private set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            try
            {
                ReturnUrl = DetermineReturnUrl(returnUrl ?? ReturnUrl);
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("Usuario logado nao foi encontrado.");
                }

                await LoadUserDetailsAsync(user);
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MudarSenha.cshtml.cs", "OnGetAsync", error);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                ReturnUrl = DetermineReturnUrl(returnUrl ?? ReturnUrl);
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("Usuario logado nao foi encontrado.");
                }

                if (!ModelState.IsValid)
                {
                    await LoadUserDetailsAsync(user);
                    return Page();
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.SenhaAtual!, Input.NovaSenha!);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    await LoadUserDetailsAsync(user);
                    return Page();
                }

                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = "Senha atualizada com sucesso.";
                _notyf.Success(StatusMessage, 3);
                _logger.LogInformation("Usuario {UserId} atualizou a senha.", user.Id);
                await LoadUserDetailsAsync(user);
                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MudarSenha.cshtml.cs", "OnPostAsync", error);
                return Page();
            }
        }

        private async Task LoadUserDetailsAsync(IdentityUser baseUser)
        {
            if (baseUser == null) return;

            var user = await _unitOfWork.AspNetUsers.GetFirstOrDefaultAsync(u => u.Id == baseUser.Id);
            if (user != null)
            {
                Ponto = user.Ponto;

                if (!string.IsNullOrEmpty(user.NomeCompleto))
                {
                    var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
                    NomeCompleto = textInfo.ToTitleCase(user.NomeCompleto.ToLower());
                }

                if (user.Foto != null && user.Foto.Length > 0)
                {
                    FotoBase64 = $"data:image/png;base64,{Convert.ToBase64String(user.Foto)}";
                }

                UsuarioExibicao = $"{Ponto} - {NomeCompleto}";
            }
            else
            {
                UsuarioExibicao = baseUser.UserName ?? baseUser.Email ?? "Usuario";
            }
        }

        private string DetermineReturnUrl(string candidate)
        {
            if (!string.IsNullOrWhiteSpace(candidate))
            {
                return candidate;
            }

            return Url.Content("~/");
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Informe a senha atual.")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha atual")]
            public string? SenhaAtual { get; set; }

            [Required(ErrorMessage = "Informe a nova senha.")]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "A nova senha precisa ter ao menos 8 caracteres.")]
            [DataType(DataType.Password)]
            [Display(Name = "Nova senha")]
            public string? NovaSenha { get; set; }

            [Required(ErrorMessage = "Confirme a nova senha.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirmacao da nova senha")]
            [Compare("NovaSenha", ErrorMessage = "A nova senha e a confirmacao nao conferem.")]
            public string? ConfirmacaoNovaSenha { get; set; }
        }
    }
}

