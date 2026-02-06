using FrotiX.Repository.IRepository;
using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: UploadCNHController                                               ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Recepção e persistência de CNH digital de motoristas.                     ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/UploadCNH                                             ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCNHController : Controller
    {
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UploadCNHController (Construtor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa ambiente, UnitOfWork e serviço de log.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • env (IWebHostEnvironment): WebRoot/hosting.                            ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public UploadCNHController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                this.hostingEnv = env;
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCNHController.cs", "UploadCNHController", error);
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   SAVE - SALVA ARQUIVO DE CNH NO BANCO                |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Save (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Salva arquivo de CNH no banco para o motorista informado.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados.                     ║
        /// ║    • motoristaId (Guid): ID do motorista.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Conteúdo vazio.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
        {
            try
            {
                // [VALIDACAO] Arquivos e motorista.
                if (UploadFiles != null && motoristaId != Guid.Empty)
                {
                    foreach (var file in UploadFiles)
                    {
                        // [DADOS] Busca motorista.
                        var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                            u.MotoristaId == motoristaId
                        );

                        if (objFromDb != null)
                        {
                            // [ARQUIVO] Converte arquivo para bytes.
                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                objFromDb.CNHDigital = target.ToArray();
                            }

                            // [ACAO] Persiste CNH.
                            _unitOfWork.Motorista.Update(objFromDb);
                            _unitOfWork.Save();

                            _log.Info($"Upload de CNH realizado com sucesso para o Motorista ID: {motoristaId} (Arquivo: {file.FileName})", "UploadCNHController", "Save");
                        }
                    }
                }
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UploadCNHController", "Save");
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Save" , error);
                Response.StatusCode = 500;
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   REMOVE - REMOVE ARQUIVO DE CNH DO BANCO             |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Remove (POST)                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove arquivo de CNH do motorista informado.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados (não utilizados).    ║
        /// ║    • motoristaId (Guid): ID do motorista.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Conteúdo vazio.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid motoristaId)
        {
            try
            {
                // [VALIDACAO] ID do motorista.
                if (motoristaId != Guid.Empty)
                {
                    // [DADOS] Busca motorista.
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == motoristaId
                    );

                    if (objFromDb != null)
                    {
                        // [ACAO] Remove CNH.
                        objFromDb.CNHDigital = null;
                        _unitOfWork.Motorista.Update(objFromDb);
                        _unitOfWork.Save();

                        _log.Info($"Arquivo de CNH removido com sucesso para o Motorista ID: {motoristaId}", "UploadCNHController", "Remove");
                    }
                }
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UploadCNHController", "Remove");
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Remove" , error);
                Response.Clear();
                Response.StatusCode = 500;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = error.Message;
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
        }

        /*
        *  _______________________________________________________
        * |                                                       |
        * |   UPLOAD FEATURES - VIEW AUXILIAR                     |
        * |_______________________________________________________|
        */
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UploadFeatures (POST)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza view auxiliar de upload.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • ActionResult: View auxiliar.                                          ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("UploadFeatures")]
        public ActionResult UploadFeatures()
        {
            try
            {
                // [RETORNO] View auxiliar.
                return View();
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UploadCNHController", "UploadFeatures");
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadFeatures" , error);
                // [RETORNO] View fallback.
                return View();
            }
        }
    }
}
