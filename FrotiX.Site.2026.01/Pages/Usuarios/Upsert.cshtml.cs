/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║  ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ███████╗██╗████████╗███████╗                        ║
 * ║  ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ██╔════╝██║╚══██╔══╝██╔════╝                        ║
 * ║  █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝     ███████╗██║   ██║   █████╗                          ║
 * ║  ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ╚════██║██║   ██║   ██╔══╝                          ║
 * ║  ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████║██║   ██║   ███████╗                        ║
 * ║  ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝╚═╝   ╚═╝   ╚══════╝                        ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Upsert.cshtml.cs                                                                                    ║
 * ║ PageModel para criação/edição de registros                                                          ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ METADATA                                                                                             ║
 * ║ @arquivo    Upsert.cshtml.cs                                                                        ║
 * ║ @local      Pages/Usuarios/                                                                               ║
 * ║ @versao     1.0.0                                                                                    ║
 * ║ @data       23/01/2026                                                                               ║
 * ║ @relevancia Gestão de usuários                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */
using AspNetCoreHero.ToastNotification.Abstractions;
using FrotiX.Models;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FrotiX.Pages.Usuarios
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly INotyfService _notyf;

        public static string UsuarioId;
        public static byte[] FotoUsuario;

        public UpsertModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork unitOfWork,
            ILogger<IndexModel> logger,
            IWebHostEnvironment hostingEnvironment,
            INotyfService notyf
        )
        {
            try
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _notyf = notyf;
                _userManager = userManager;
                _signInManager = signInManager;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "UpsertModel", error);
            }
        }

        [BindProperty]
        public UsuarioViewModel UsuarioObj { get; set; }

        [BindProperty]
        public IFormFile FotoUpload { get; set; }

        public byte[] FotoPadraoBytes { get; set; }

        private void SetViewModel()
        {
            try
            {
                UsuarioObj = new UsuarioViewModel { AspNetUsers = new Models.AspNetUsers() };
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "SetViewModel", error);
            }
        }

        private byte[] CarregarFotoPadrao()
        {
            try
            {
                string caminhoFotoPadrao = Path.Combine(_hostingEnvironment.WebRootPath, "Images", "sucesso_transparente.png");
                
                if (System.IO.File.Exists(caminhoFotoPadrao))
                {
                    return System.IO.File.ReadAllBytes(caminhoFotoPadrao);
                }
                
                return null;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "CarregarFotoPadrao", error);
                return null;
            }
        }

        public IActionResult OnGet(string? id)
        {
            try
            {
                SetViewModel();

                if (id != null)
                {
                    UsuarioId = id;
                }

                if (id != null && id != "")
                {
                    UsuarioObj.AspNetUsers = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == id);

                    if (UsuarioObj == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    // Se for criação (novo usuário), inicializa com Status ativo e foto padrão
                    UsuarioObj.AspNetUsers.Status = true;
                    
                    FotoPadraoBytes = CarregarFotoPadrao();
                    if (FotoPadraoBytes != null)
                    {
                        UsuarioObj.AspNetUsers.Foto = FotoPadraoBytes;
                    }
                }

                return Page();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "OnGet", error);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            try
            {
                if (UsuarioObj?.AspNetUsers == null)
                {
                    SetViewModel();
                    return Page();
                }

                if (ChecaDuplicado(null))
                {
                    return Page();
                }

                string valor = UsuarioObj.AspNetUsers.Ponto?.Trim() ?? "";

                valor = System.Text.RegularExpressions.Regex.Replace(valor, "^[pP]_", "");
                valor = System.Text.RegularExpressions.Regex.Replace(valor, @"\D", "");

                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor.Length > 10)
                    {
                        ModelState.AddModelError("UsuarioObj.AspNetUsers.Ponto", "(O ponto deve ter no maximo 10 numeros apos p_)");
                    }
                    else
                    {
                        valor = "p_" + valor;
                        UsuarioObj.AspNetUsers.Ponto = valor;
                        UsuarioObj.AspNetUsers.UserName = valor;
                    }
                }
                else
                {
                    ModelState.AddModelError("UsuarioObj.AspNetUsers.Ponto", "(O ponto e obrigatorio e deve conter numeros)");
                }

                if (!string.IsNullOrEmpty(UsuarioObj.AspNetUsers.Email))
                {
                    string email = UsuarioObj.AspNetUsers.Email.Trim().ToLower();
                    if (!email.EndsWith("@camara.leg.br"))
                    {
                        email = System.Text.RegularExpressions.Regex.Replace(email, @"@camara\.leg\.br$", "");
                        email = System.Text.RegularExpressions.Regex.Replace(email, "@+$", "");
                        if (!string.IsNullOrEmpty(email))
                        {
                            UsuarioObj.AspNetUsers.Email = email + "@camara.leg.br";
                        }
                        else
                        {
                            ModelState.AddModelError("UsuarioObj.AspNetUsers.Email", "(O email deve terminar em @camara.leg.br)");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UsuarioObj.AspNetUsers.PhoneNumber))
                {
                    string celular = UsuarioObj.AspNetUsers.PhoneNumber.Trim();
                    // Aceita formato celular (xx) xxxxx-xxxx (11 digitos) ou fixo (xx) xxxx-xxxx (10 digitos)
                    if (!System.Text.RegularExpressions.Regex.IsMatch(celular, @"^\(\d{2}\) \d{4,5}-\d{4}$"))
                    {
                        ModelState.AddModelError("UsuarioObj.AspNetUsers.PhoneNumber", "(O celular deve estar no formato (xx) xxxxx-xxxx)");
                    }
                }

                if (UsuarioObj.AspNetUsers.Ramal.HasValue && (UsuarioObj.AspNetUsers.Ramal.Value < 10000000 || UsuarioObj.AspNetUsers.Ramal.Value > 99999999))
                {
                    ModelState.AddModelError("UsuarioObj.AspNetUsers.Ramal", "(O ramal deve ter 8 digitos e nao pode comecar com zero)");
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                byte[] fotoBytes = null;
                if (FotoUpload != null && FotoUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await FotoUpload.CopyToAsync(memoryStream);
                        fotoBytes = memoryStream.ToArray();
                    }
                }
                else
                {
                    fotoBytes = CarregarFotoPadrao();
                }

                // OnPostSubmit é para CRIAÇÃO - Status SEMPRE true (Ativo)
                // Não usar ?? porque false ?? true = false, não true!
                bool statusUsuario = true;

                // IMPORTANTE: O UserManager<IdentityUser> nao salva propriedades customizadas de AspNetUsers
                // Por isso usamos diretamente o _unitOfWork para garantir que TODAS as propriedades sejam salvas

                var novoUsuario = new Models.AspNetUsers
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = UsuarioObj.AspNetUsers.UserName,
                    NormalizedUserName = UsuarioObj.AspNetUsers.UserName?.ToUpper(),
                    Email = UsuarioObj.AspNetUsers.Email,
                    NormalizedEmail = UsuarioObj.AspNetUsers.Email?.ToUpper(),
                    EmailConfirmed = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    PhoneNumber = UsuarioObj.AspNetUsers.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Discriminator = "Usuario",
                    // Propriedades customizadas
                    NomeCompleto = UsuarioObj.AspNetUsers.NomeCompleto,
                    Ponto = UsuarioObj.AspNetUsers.Ponto,
                    Ramal = UsuarioObj.AspNetUsers.Ramal,
                    DetentorCargaPatrimonial = UsuarioObj.AspNetUsers.DetentorCargaPatrimonial ?? false,
                    Status = statusUsuario,
                    Foto = fotoBytes,
                    Criacao = DateTime.Now,
                    UsuarioIdAlteracao = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                };

                // Gera o hash da senha usando o PasswordHasher do Identity
                novoUsuario.PasswordHash = _userManager.PasswordHasher.HashPassword(null, "visual");

                _unitOfWork.AspNetUsers.Add(novoUsuario);
                _unitOfWork.Save();

                CriarControleAcessoParaUsuario(novoUsuario.Id);

                _notyf.Success("Usuario inserido com sucesso!", 3);

                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "OnPostSubmit", error);
                return RedirectToPage("./Index");
            }
        }

        public async Task<IActionResult> OnPostEdit(string id)
        {
            try
            {
                if (UsuarioObj?.AspNetUsers == null)
                {
                    SetViewModel();
                    return Page();
                }

                var usuarioExistente = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == id);

                UsuarioObj.AspNetUsers.Id = id;
                UsuarioObj.AspNetUsers.Discriminator = "Usuario";

                string valor = UsuarioObj.AspNetUsers.Ponto?.Trim() ?? "";

                valor = System.Text.RegularExpressions.Regex.Replace(valor, "^[pP]_", "");
                valor = System.Text.RegularExpressions.Regex.Replace(valor, @"\D", "");

                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor.Length > 10)
                    {
                        ModelState.AddModelError("UsuarioObj.AspNetUsers.Ponto", "(O ponto deve ter no maximo 10 numeros apos p_)");
                    }
                    else
                    {
                        valor = "p_" + valor;
                        UsuarioObj.AspNetUsers.Ponto = valor;
                        UsuarioObj.AspNetUsers.UserName = valor;
                    }
                }
                else
                {
                    ModelState.AddModelError("UsuarioObj.AspNetUsers.Ponto", "(O ponto e obrigatorio e deve conter numeros)");
                }

                if (!string.IsNullOrEmpty(UsuarioObj.AspNetUsers.Email))
                {
                    string email = UsuarioObj.AspNetUsers.Email.Trim().ToLower();
                    if (!email.EndsWith("@camara.leg.br"))
                    {
                        email = System.Text.RegularExpressions.Regex.Replace(email, @"@camara\.leg\.br$", "");
                        email = System.Text.RegularExpressions.Regex.Replace(email, "@+$", "");
                        if (!string.IsNullOrEmpty(email))
                        {
                            UsuarioObj.AspNetUsers.Email = email + "@camara.leg.br";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(UsuarioObj.AspNetUsers.PhoneNumber))
                {
                    string celular = UsuarioObj.AspNetUsers.PhoneNumber.Trim();
                    // Aceita formato celular (xx) xxxxx-xxxx (11 digitos) ou fixo (xx) xxxx-xxxx (10 digitos)
                    if (!System.Text.RegularExpressions.Regex.IsMatch(celular, @"^\(\d{2}\) \d{4,5}-\d{4}$"))
                    {
                        ModelState.AddModelError("UsuarioObj.AspNetUsers.PhoneNumber", "(O celular deve estar no formato (xx) xxxxx-xxxx)");
                    }
                }

                if (UsuarioObj.AspNetUsers.Ramal.HasValue && (UsuarioObj.AspNetUsers.Ramal.Value < 10000000 || UsuarioObj.AspNetUsers.Ramal.Value > 99999999))
                {
                    ModelState.AddModelError("UsuarioObj.AspNetUsers.Ramal", "(O ramal deve ter 8 digitos e nao pode comecar com zero)");
                }

                if (!ModelState.IsValid)
                {
                    if (usuarioExistente != null && usuarioExistente.Foto != null && usuarioExistente.Foto.Length > 0)
                    {
                        UsuarioObj.AspNetUsers.Foto = usuarioExistente.Foto;
                    }
                    return Page();
                }

                if (FotoUpload != null && FotoUpload.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await FotoUpload.CopyToAsync(memoryStream);
                        UsuarioObj.AspNetUsers.Foto = memoryStream.ToArray();
                    }
                }
                else if (usuarioExistente != null && usuarioExistente.Foto != null && usuarioExistente.Foto.Length > 0)
                {
                    UsuarioObj.AspNetUsers.Foto = usuarioExistente.Foto;
                }
                else
                {
                    UsuarioObj.AspNetUsers.Foto = CarregarFotoPadrao();
                }

                // Garantir que os campos obrigatórios do Identity estejam preenchidos
                UsuarioObj.AspNetUsers.NormalizedUserName = UsuarioObj.AspNetUsers.UserName?.ToUpper();
                UsuarioObj.AspNetUsers.NormalizedEmail = UsuarioObj.AspNetUsers.Email?.ToUpper();
                UsuarioObj.AspNetUsers.UsuarioIdAlteracao = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                // Preservar campos do usuário existente que não devem ser alterados
                if (usuarioExistente != null)
                {
                    UsuarioObj.AspNetUsers.PasswordHash = usuarioExistente.PasswordHash;
                    UsuarioObj.AspNetUsers.SecurityStamp = usuarioExistente.SecurityStamp;
                    UsuarioObj.AspNetUsers.ConcurrencyStamp = usuarioExistente.ConcurrencyStamp;
                    UsuarioObj.AspNetUsers.Criacao = usuarioExistente.Criacao;
                    UsuarioObj.AspNetUsers.EmailConfirmed = usuarioExistente.EmailConfirmed;
                    UsuarioObj.AspNetUsers.PhoneNumberConfirmed = usuarioExistente.PhoneNumberConfirmed;
                    UsuarioObj.AspNetUsers.TwoFactorEnabled = usuarioExistente.TwoFactorEnabled;
                    UsuarioObj.AspNetUsers.LockoutEnabled = usuarioExistente.LockoutEnabled;
                    UsuarioObj.AspNetUsers.AccessFailedCount = usuarioExistente.AccessFailedCount;
                    UsuarioObj.AspNetUsers.UltimoLogin = usuarioExistente.UltimoLogin;
                }

                _unitOfWork.AspNetUsers.Update(UsuarioObj.AspNetUsers);
                _unitOfWork.Save();

                _notyf.Success("Usuario atualizado com sucesso!", 3);

                return RedirectToPage("./Index");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "OnPostEdit", error);
                return RedirectToPage("./Index");
            }
        }

        private void CriarControleAcessoParaUsuario(string usuarioId)
        {
            var objRecursos = _unitOfWork.Recurso.GetAll();

            foreach (var recurso in objRecursos)
            {
                var objAcesso = new ControleAcesso
                {
                    UsuarioId = usuarioId,
                    RecursoId = recurso.RecursoId,
                    Acesso = true,
                };

                _unitOfWork.ControleAcesso.Add(objAcesso);
            }

            _unitOfWork.Save();
        }

        private bool ChecaDuplicado(string? id)
        {
            try
            {
                return false;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("Upsert.cshtml.cs", "ChecaDuplicado", error);
                return false;
            }
        }
    }
}

