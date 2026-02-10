/* ****************************************************************************************
 * ⚡ ARQUIVO: LoginController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer endpoints legados de login e utilidades relacionadas ao
 *                   usuário autenticado.
 *
 * 📥 ENTRADAS     : Requisições GET e acesso ao contexto de autenticação.
 *
 * 📤 SAÍDAS       : Views e JSON com dados do usuário corrente.
 *
 * 🔗 CHAMADA POR  : Navegação direta (legado) e componentes que consultam usuário atual.
 *
 * 🔄 CHAMA        : IUnitOfWork.AspNetUsers, ClaimsPrincipal, Alerta.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, ILogger, IWebHostEnvironment, IUnitOfWork.
 *
 * ⚠️ ATENÇÃO      : O sistema utiliza Identity para login; este controller é legado.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: LoginController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Manter compatibilidade com rotas antigas de login e fornecer dados
 *                   do usuário autenticado.
 *
 * 📥 ENTRADAS     : Claims do usuário corrente.
 *
 * 📤 SAÍDAS       : Views e JSON com nome/ponto do usuário.
 *
 * 🔗 CHAMADA POR  : Rotas antigas e utilidades internas.
 *
 * 🔄 CHAMA        : IUnitOfWork.AspNetUsers, ClaimsPrincipal.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, ILogger, IWebHostEnvironment.
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
         * 🎯 OBJETIVO     : Injetar dependências utilizadas por rotas legadas de login.
         *
         * 📥 ENTRADAS     : logger, hostingEnvironment, unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Index
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar a view principal de login (legado).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : [IActionResult] View.
         *
         * 🔗 CHAMADA POR  : Navegação direta.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar view padrão de login (legado).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : [IActionResult] View.
         *
         * 🔗 CHAMADA POR  : Chamadas GET simples para o controller.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: RecuperaUsuarioAtual
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter nome e ponto do usuário autenticado no sistema.
         *
         * 📥 ENTRADAS     : Claims do usuário atual.
         *
         * 📤 SAÍDAS       : JSON com { nome, ponto }.
         *
         * 🔗 CHAMADA POR  : Telas que precisam exibir dados do usuário corrente.
         *
         * 🔄 CHAMA        : _unitOfWork.AspNetUsers.GetFirstOrDefault().
         ****************************************************************************************/
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
