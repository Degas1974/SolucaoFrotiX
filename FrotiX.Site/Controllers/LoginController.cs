/****************************************************************************************
 * ⚡ CONTROLLER: LoginController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Controller de login (provavelmente legado, Identity é usado para autenticação)
 * 📥 ENTRADAS     : Nenhuma (retorna views)
 * 📤 SAÍDAS       : Views
 * 🔗 CHAMADA POR  : Navegação direta (provavelmente não utilizado)
 * 🔄 CHAMA        : IUnitOfWork, ILogger
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, ILogger, IWebHostEnvironment
 *
 * ⚠️  NOTA: Este controller parece ser legado. O sistema utiliza Identity para login
 *           (ver Areas/Identity/Pages/Account/Login.cshtml.cs)
 ****************************************************************************************/
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace FrotiX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController :Controller
    {
        private readonly ILogger<AbastecimentoController> _logger;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: LoginController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências
         ****************************************************************************************/
        public LoginController(
            ILogger<AbastecimentoController> logger ,
            IWebHostEnvironment hostingEnvironment ,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                _logger = logger;
                _hostingEnvironment = hostingEnvironment;
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LoginController.cs" , "LoginController" , error);
            }
        }

        [BindProperty]
        public Models.Abastecimento AbastecimentoObj
        {
            get; set;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LoginController.cs" , "Index" , error);
                return View(); // padronizado
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LoginController.cs" , "Get" , error);
                return View(); // padronizado
            }
        }

        //Recupera o nome do Usuário de Criação/Finalização
        //=================================================
        [Route("RecuperaUsuarioAtual")]
        public IActionResult RecuperaUsuarioAtual()
        {
            try
            {
                string usuarioCorrenteNome;
                string usuarioCorrentePonto;

                //Pega o usuário corrente
                //=======================
                ClaimsPrincipal currentUser = User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                var objUsuario = _unitOfWork.AspNetUsers.GetFirstOrDefault(u =>
                    u.Id == currentUserID
                );

                usuarioCorrenteNome = objUsuario.NomeCompleto;
                usuarioCorrentePonto = objUsuario.Ponto;
                Settings.GlobalVariables.gPontoUsuario = objUsuario.Ponto;

                return Json(new
                {
                    nome = usuarioCorrenteNome ,
                    ponto = usuarioCorrentePonto
                });
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("LoginController.cs" , "RecuperaUsuarioAtual" , error);
                return View(); // padronizado
            }
        }
    }
}
