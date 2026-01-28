/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: UploadCRLVController.cs                                          ║
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
     * ⚡ CONTROLLER: UploadCRLV API (Syncfusion Uploader)
     * 🎯 OBJETIVO: Gerenciar upload de CRLV (Certificado de Registro e Licenciamento de Veículo)
     * 📋 ROTAS: /api/UploadCRLV/* (Save, Remove, UploadFeatures)
     * 🔗 ENTIDADES: Veiculo (campo CRLV byte[])
     * 📦 DEPENDÊNCIAS: IWebHostEnvironment, IUnitOfWork, Syncfusion Uploader
     * 💾 ARMAZENAMENTO: PDF convertido para byte[] e salvo no banco de dados
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public partial class UploadCRLVController :Controller
    {
        private IWebHostEnvironment hostingEnv;
        private readonly IUnitOfWork _unitOfWork;

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
         * 🎯 OBJETIVO: Salvar arquivo PDF do CRLV no banco de dados (conversão para byte[])
         * 📥 ENTRADAS: UploadFiles (IFormFile[]), veiculoId (Guid query param)
         * 📤 SAÍDAS: Content("") com StatusCode 200 ou 500
         * 🔗 CHAMADA POR: Syncfusion Uploader (JavaScript component)
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), Veiculo.Update()
         * 💾 CONVERSÃO: IFormFile → MemoryStream → byte[] → Veiculo.CRLV
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
                            // [DOC] Converte arquivo enviado para byte array e salva no banco
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
         * 🎯 OBJETIVO: Remover CRLV do veículo (limpa campo CRLV)
         * 📥 ENTRADAS: UploadFiles (IFormFile[] - não usado), veiculoId (Guid query param)
         * 📤 SAÍDAS: Content("") com StatusCode 200 ou 500
         * 🔗 CHAMADA POR: Syncfusion Uploader (botão de remoção)
         * 🔄 CHAMA: Veiculo.GetFirstOrDefault(), Veiculo.Update()
         * 🗑️ OPERAÇÃO: Define CRLV = null
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
                Alerta.TratamentoErroComLinha("UploadCRLVController.cs" , "UploadFeatures" , error);
                return View();
            }
        }
    }
}
