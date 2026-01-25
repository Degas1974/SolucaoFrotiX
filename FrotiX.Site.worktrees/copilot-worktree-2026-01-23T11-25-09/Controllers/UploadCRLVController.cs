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
    /// ║ 📌 NOME: UploadCRLVController                                              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Recepção e persistência de CRLV digital de veículos.                      ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/UploadCRLV                                            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCRLVController : Controller
    {
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UploadCRLVController (Construtor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa ambiente, UnitOfWork e serviço de log.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • env (IWebHostEnvironment): WebRoot/hosting.                            ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public UploadCRLVController(IWebHostEnvironment env, IUnitOfWork unitOfWork, ILogService log)
        {
            try
            {
                this.hostingEnv = env;
                _unitOfWork = unitOfWork;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs", "UploadCRLVController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Save (POST)                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Salva arquivo de CRLV no cadastro do veículo.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados.                     ║
        /// ║    • veiculoId (Guid): ID do veículo.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Conteúdo vazio.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
        {
            try
            {
                // [REGRA] Evita operação com identificador inválido
                if (UploadFiles != null && veiculoId != Guid.Empty)
                {
                    foreach (var file in UploadFiles)
                    {
                        // [DADOS] Carrega veículo para persistir o CRLV
                        var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.VeiculoId == veiculoId
                        );

                        if (objFromDb != null)
                        {
                            // [DADOS] Converte arquivo em byte[]
                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                objFromDb.CRLV = target.ToArray();
                            }

                            // [DADOS] Atualiza entidade e persiste
                            _unitOfWork.Veiculo.Update(objFromDb);
                            _unitOfWork.Save();

                            _log.Info($"Upload de CRLV realizado com sucesso para o Veículo ID: {veiculoId} (Arquivo: {file.FileName})", "UploadCRLVController", "Save");
                        }
                    }
                }
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UploadCRLVController", "Save");
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Save" , error);
                Response.StatusCode = 500;
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Remove (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove o CRLV do veículo informado.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados (não usados).         ║
        /// ║    • veiculoId (Guid): ID do veículo.                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Conteúdo vazio.                                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles, [FromQuery] Guid veiculoId)
        {
            try
            {
                // [REGRA] Evita operação com identificador inválido
                if (veiculoId != Guid.Empty)
                {
                    // [DADOS] Carrega veículo para remoção do CRLV
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == veiculoId
                    );

                    if (objFromDb != null)
                    {
                        // [DADOS] Limpa binário do documento
                        objFromDb.CRLV = null;
                        _unitOfWork.Veiculo.Update(objFromDb);
                        _unitOfWork.Save();

                        _log.Info($"Arquivo de CRLV removido com sucesso para o Veículo ID: {veiculoId}", "UploadCRLVController", "Remove");
                    }
                }
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
            catch (Exception error)
            {
                _log.Error("Erro", error, "UploadCRLVController", "Remove");
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Remove" , error);
                Response.Clear();
                Response.StatusCode = 500;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = error.Message;
                // [RETORNO] Conteúdo vazio.
                return Content("");
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: UploadFeatures (POST)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna view auxiliar usada pelo uploader.                               ║
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
                _log.Error("Erro", error, "UploadCRLVController", "UploadFeatures");
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadFeatures" , error);
                // [RETORNO] View fallback.
                return View();
            }
        }
    }
}
