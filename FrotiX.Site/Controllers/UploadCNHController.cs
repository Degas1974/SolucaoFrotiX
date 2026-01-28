/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: UploadCNHController.cs                                           ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
     * ⚡ CONTROLLER: UploadCNH API (Syncfusion Uploader)
     * 🎯 OBJETIVO: Gerenciar upload de CNH digital (PDF) de motoristas
     * 📋 ROTAS: /api/UploadCNH/* (Save, Remove, UploadFeatures)
     * 🔗 ENTIDADES: Motorista (campo CNHDigital byte[])
     * 📦 DEPENDÊNCIAS: IWebHostEnvironment, IUnitOfWork, Syncfusion Uploader
     * 💾 ARMAZENAMENTO: PDF convertido para byte[] e salvo no banco de dados
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCNHController :Controller
    {
        private IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;

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
         * 🎯 OBJETIVO: Salvar arquivo PDF da CNH no banco de dados (conversão para byte[])
         * 📥 ENTRADAS: UploadFiles (IFormFile[]), motoristaId (Guid query param)
         * 📤 SAÍDAS: Content("") com StatusCode 200 ou 500
         * 🔗 CHAMADA POR: Syncfusion Uploader (JavaScript component)
         * 🔄 CHAMA: Motorista.GetFirstOrDefault(), Motorista.Update()
         * 💾 CONVERSÃO: IFormFile → MemoryStream → byte[] → Motorista.CNHDigital
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
                            // [DOC] Converte arquivo enviado para byte array e salva no banco
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
         * 🎯 OBJETIVO: Remover CNH digital do motorista (limpa campo CNHDigital)
         * 📥 ENTRADAS: UploadFiles (IFormFile[] - não usado), motoristaId (Guid query param)
         * 📤 SAÍDAS: Content("") com StatusCode 200 ou 500
         * 🔗 CHAMADA POR: Syncfusion Uploader (botão de remoção)
         * 🔄 CHAMA: Motorista.GetFirstOrDefault(), Motorista.Update()
         * 🗑️ OPERAÇÃO: Define CNHDigital = null
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
         * 🎯 OBJETIVO: Renderizar página de demonstração do uploader (uso interno/teste)
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: View (Razor Page)
         * 🔗 CHAMADA POR: Acesso direto para visualizar funcionalidades do uploader
         * 🔄 CHAMA: View()
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
