/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API/MVC)                                                                      |
 * | (IA) IDENTIDADE: LoginController.cs                                                                     |
 * | (IA) DESCRIÇÃO: Controlador para gestão de sessão, acesso inicial e recuperação de dados do usuário.    |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: LoginController                                                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Processos de autenticação visual (View) e recuperação do usuário logado.  ║
    /// ║    Fornece dados para cabeçalho e informações básicas da conta.              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API/MVC                                                           ║
    /// ║    • Rota base: /api/Login                                                   ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: LoginController (Construtor)                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa uma nova instância do controlador de Login.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • logger: Infraestrutura de logging.                                      ║
        /// ║    • hostingEnvironment: Ambiente de hospedagem.                             ║
        /// ║    • unitOfWork: Injeção do repositório.                                     ║
        /// ║    • log: Injeção do serviço de logs integrado.                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public LoginController(ILogger<LoginController> logger, IWebHostEnvironment hostingEnvironment, IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LoginController.cs", "LoginController", error);
            }
        }

        [BindProperty]
        public Models.Abastecimento AbastecimentoObj { get; set; }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Index                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza a página de login customizada.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public IActionResult Index()
        {
            try
            {
                // [VIEW] Retorna view de login.
                return View();
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LoginController.cs", "Index");
                Alerta.TratamentoErroComLinha("LoginController.cs", "Index", error);
                return View();
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Get                                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza a view de login (fallback GET).                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • Nenhum                                                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: View Index.                                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // [VIEW] Retorna view de login (fallback GET).
                return View();
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LoginController.cs", "Get");
                Alerta.TratamentoErroComLinha("LoginController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RecuperaUsuarioAtual                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna nome completo e ponto do usuário logado.                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com nome e ponto.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [Route("RecuperaUsuarioAtual")]
        public IActionResult RecuperaUsuarioAtual()
        {
            try
            {
                string usuarioCorrenteNome;
                string usuarioCorrentePonto;

                // [DADOS] Recupera usuário corrente do ClaimsPrincipal.
                ClaimsPrincipal currentUser = User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                // [DADOS] Busca dados do usuário no repositório.
                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u => u.Id == currentUserID);

                usuarioCorrenteNome = objUsuario.NomeCompleto;
                usuarioCorrentePonto = objUsuario.Ponto;
                Settings.GlobalVariables.gPontoUsuario = objUsuario.Ponto;

                // [RETORNO] Dados básicos do usuário.
                return Ok(new { nome = usuarioCorrenteNome, ponto = usuarioCorrentePonto });
            }
            catch (Exception error)
            {
                _log.Error(error.Message, error, "LoginController.cs", "RecuperaUsuarioAtual");
                Alerta.TratamentoErroComLinha("LoginController.cs", "RecuperaUsuarioAtual", error);
                return BadRequest();
            }
        }
    }
}
