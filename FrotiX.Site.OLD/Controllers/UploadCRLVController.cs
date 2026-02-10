/* ****************************************************************************************
 * ⚡ ARQUIVO: UploadCRLVController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fazer upload e remoção do CRLV digital do veículo (byte[] CRLV).
 *
 * 📥 ENTRADAS     : Arquivos enviados via multipart/form-data e veiculoId.
 *
 * 📤 SAÍDAS       : Content vazio com status HTTP correspondente.
 *
 * 🔗 CHAMADA POR  : Tela de upload de CRLV.
 *
 * 🔄 CHAMA        : IUnitOfWork.Veiculo, IWebHostEnvironment.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER PARTIAL: UploadCRLVController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para upload/remoção de CRLV digital.
     *
     * 📥 ENTRADAS     : Arquivos e IDs de veículo.
     *
     * 📤 SAÍDAS       : Content vazio com status.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCRLVController :Controller
    {
        private IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: UploadCRLVController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar hosting e unit of work.
         *
         * 📥 ENTRADAS     : env, unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public UploadCRLVController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
        {
            try
            {
                this.hostingEnv = env;
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadCRLVController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Save
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Salvar CRLV digital no cadastro do veículo.
         *
         * 📥 ENTRADAS     : UploadFiles (lista de arquivos), veiculoId (Guid).
         *
         * 📤 SAÍDAS       : Content vazio (200/500).
         *
         * 🔗 CHAMADA POR  : Upload de CRLV.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), Veiculo.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
        {
            try
            {
                if (UploadFiles != null && veiculoId != Guid.Empty)
                {
                    foreach (var file in UploadFiles)
                    {
                        var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                            u.VeiculoId == veiculoId
                        );

                        if (objFromDb != null)
                        {
                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                objFromDb.CRLV = target.ToArray();
                            }
                            _unitOfWork.Veiculo.Update(objFromDb);
                            _unitOfWork.Save();
                        }
                    }
                }
                return Content("");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Save" , error);
                Response.StatusCode = 500;
                return Content("");
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Remove
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover CRLV digital do cadastro do veículo.
         *
         * 📥 ENTRADAS     : UploadFiles (ignorado), veiculoId (Guid).
         *
         * 📤 SAÍDAS       : Content vazio (200/500).
         *
         * 🔗 CHAMADA POR  : Ação de remoção de CRLV.
         *
         * 🔄 CHAMA        : Veiculo.GetFirstOrDefault(), Veiculo.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid veiculoId)
        {
            try
            {
                if (veiculoId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Veiculo.GetFirstOrDefault(u =>
                        u.VeiculoId == veiculoId
                    );

                    if (objFromDb != null)
                    {
                        objFromDb.CRLV = null;
                        _unitOfWork.Veiculo.Update(objFromDb);
                        _unitOfWork.Save();
                    }
                }
                return Content("");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "Remove" , error);
                Response.Clear();
                Response.StatusCode = 500;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = error.Message;
                return Content("");
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: UploadFeatures
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar view de funcionalidades do upload (placeholder).
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : View padrão.
         *
         * 🔗 CHAMADA POR  : Navegação interna/placeholder.
         ****************************************************************************************/
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("UploadFeatures")]
        public ActionResult UploadFeatures()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadFeatures" , error);
                return View();
            }
        }
    }
}
