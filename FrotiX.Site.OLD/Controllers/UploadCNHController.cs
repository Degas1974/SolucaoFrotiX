/* ****************************************************************************************
 * ⚡ ARQUIVO: UploadCNHController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fazer upload e remoção da CNH digital do motorista (byte[] CNHDigital).
 *
 * 📥 ENTRADAS     : Arquivos enviados via multipart/form-data e motoristaId.
 *
 * 📤 SAÍDAS       : Content vazio com status HTTP correspondente.
 *
 * 🔗 CHAMADA POR  : Tela de upload de CNH.
 *
 * 🔄 CHAMA        : IUnitOfWork.Motorista, IWebHostEnvironment.
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
     * ⚡ CONTROLLER PARTIAL: UploadCNHController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoints para upload/remoção de CNH digital.
     *
     * 📥 ENTRADAS     : Arquivos e IDs de motorista.
     *
     * 📤 SAÍDAS       : Content vazio com status.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCNHController :Controller
    {
        private IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: UploadCNHController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar hosting e unit of work.
         *
         * 📥 ENTRADAS     : env, unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public UploadCNHController(IWebHostEnvironment env , IUnitOfWork unitOfWork)
        {
            try
            {
                this.hostingEnv = env;
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadCNHController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Save
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Salvar CNH digital no cadastro do motorista.
         *
         * 📥 ENTRADAS     : UploadFiles (lista de arquivos), motoristaId (Guid).
         *
         * 📤 SAÍDAS       : Content vazio (200/500).
         *
         * 🔗 CHAMADA POR  : Upload de CNH.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), Motorista.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
        {
            try
            {
                if (UploadFiles != null && motoristaId != Guid.Empty)
                {
                    foreach (var file in UploadFiles)
                    {
                        var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                            u.MotoristaId == motoristaId
                        );

                        if (objFromDb != null)
                        {
                            using (var target = new MemoryStream())
                            {
                                file.CopyTo(target);
                                objFromDb.CNHDigital = target.ToArray();
                            }
                            _unitOfWork.Motorista.Update(objFromDb);
                            _unitOfWork.Save();
                        }
                    }
                }
                return Content("");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Save" , error);
                Response.StatusCode = 500;
                return Content("");
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Remove
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover CNH digital do cadastro do motorista.
         *
         * 📥 ENTRADAS     : UploadFiles (ignorado), motoristaId (Guid).
         *
         * 📤 SAÍDAS       : Content vazio (200/500).
         *
         * 🔗 CHAMADA POR  : Ação de remoção de CNH.
         *
         * 🔄 CHAMA        : Motorista.GetFirstOrDefault(), Motorista.Update(), UnitOfWork.Save().
         ****************************************************************************************/
        [AcceptVerbs("Post")]
        [HttpPost]
        [Route("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles , [FromQuery] Guid motoristaId)
        {
            try
            {
                if (motoristaId != Guid.Empty)
                {
                    var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u =>
                        u.MotoristaId == motoristaId
                    );

                    if (objFromDb != null)
                    {
                        objFromDb.CNHDigital = null;
                        _unitOfWork.Motorista.Update(objFromDb);
                        _unitOfWork.Save();
                    }
                }
                return Content("");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "Remove" , error);
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
                Alerta.TratamentoErroComLinha("UploadCNHController.cs" , "UploadFeatures" , error);
                return View();
            }
        }
    }
}
