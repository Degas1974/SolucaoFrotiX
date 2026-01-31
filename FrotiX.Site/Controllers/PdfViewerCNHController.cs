/* ****************************************************************************************
 * ⚡ ARQUIVO: PdfViewerCNHController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Disponibilizar endpoints do Syncfusion PdfViewer para CNH digital,
 *                   incluindo carregamento do arquivo do motorista e operações de viewer.
 *
 * 📥 ENTRADAS     : Payloads JSON do viewer, id do motorista.
 *
 * 📤 SAÍDAS       : JSON/Content com páginas, anotações, base64 e mensagens de erro.
 *
 * 🔗 CHAMADA POR  : Tela de visualização de CNH e componentes PdfViewer.
 *
 * 🔄 CHAMA        : PdfRenderer (Syncfusion), IMemoryCache, IUnitOfWork.Motorista.
 **************************************************************************************** */

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: PdfViewerCNHController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir operações do PdfViewer para CNH digital por motorista.
     *
     * 📥 ENTRADAS     : JSONs do viewer e IDs de motorista.
     *
     * 📤 SAÍDAS       : JSON/Content com renderização, anotações e downloads.
     *
     * 🔗 CHAMADA POR  : Módulos de CNH digital.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class PdfViewerCNHController :Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        /****************************************************************************************
         * ⚡ FUNÇÃO: PdfViewerCNHController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do hosting, cache e unit of work.
         *
         * 📥 ENTRADAS     : hostingEnvironment, cache, unitOfWork.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public PdfViewerCNHController(
            IWebHostEnvironment hostingEnvironment ,
            IMemoryCache cache ,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
                _cache = cache;
                _unitOfWork = unitOfWork;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PdfViewerCNHController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PdfViewerFeatures
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar a view de recursos do PdfViewer para CNH.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : View padrão da página.
         *
         * 🔗 CHAMADA POR  : Navegação interna do módulo CNH.
         ****************************************************************************************/
        public IActionResult PdfViewerFeatures()
        {
            try
            {
                return View();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PdfViewerFeatures" , error);
                return View();
            }
        }

        [HttpPost]
        [Route("Load")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: Load
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Carregar documento PDF (arquivo ou base64) para o viewer.
         *
         * 📥 ENTRADAS     : jsonObject (document, isFileName).
         *
         * 📤 SAÍDAS       : JSON serializado com resultado do PdfRenderer.Load().
         *
         * 🔗 CHAMADA POR  : PdfViewer (evento de load).
         *
         * 🔄 CHAMA        : GetDocumentPath(), PdfRenderer.Load().
         ****************************************************************************************/
        public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                MemoryStream stream = new MemoryStream();
                object jsonResult = new object();

                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    if (bool.Parse(jsonObject["isFileName"]))
                    {
                        string documentPath = GetDocumentPath(jsonObject["document"]);
                        if (!string.IsNullOrEmpty(documentPath))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(documentPath);
                            stream = new MemoryStream(bytes);
                        }
                        else
                        {
                            return Content(jsonObject["document"] + " não encontrado");
                        }
                    }
                    else
                    {
                        byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                        stream = new MemoryStream(bytes);
                    }
                }

                jsonResult = pdfviewer.Load(stream , jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Load" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("RenderPdfPages")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfPages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar páginas do PDF para o viewer.
         *
         * 📥 ENTRADAS     : jsonObject com parâmetros de página.
         *
         * 📤 SAÍDAS       : JSON serializado com páginas renderizadas.
         *
         * 🔗 CHAMADA POR  : PdfViewer (render pages).
         ****************************************************************************************/
        public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object jsonResult = pdfviewer.GetPage(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderPdfPages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("RenderAnnotationComments")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderAnnotationComments
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Buscar comentários/anotações do PDF para o viewer.
         *
         * 📥 ENTRADAS     : jsonObject com parâmetros de anotação.
         *
         * 📤 SAÍDAS       : JSON serializado com anotações.
         *
         * 🔗 CHAMADA POR  : PdfViewer (annotations).
         ****************************************************************************************/
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderAnnotationComments" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("Unload")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: Unload
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Limpar o cache do documento no PdfViewer.
         *
         * 📥 ENTRADAS     : jsonObject com identificador do documento.
         *
         * 📤 SAÍDAS       : Content com mensagem de status.
         *
         * 🔗 CHAMADA POR  : PdfViewer (unload).
         ****************************************************************************************/
        public IActionResult Unload([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                pdfviewer.ClearCache(jsonObject);
                return Content("Document cache is cleared");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Unload" , error);
                return Content("Erro ao limpar cache");
            }
        }

        [HttpPost]
        [Route("RenderThumbnailImages")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderThumbnailImages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar miniaturas de páginas do PDF.
         *
         * 📥 ENTRADAS     : jsonObject com parâmetros do viewer.
         *
         * 📤 SAÍDAS       : JSON serializado com miniaturas.
         *
         * 🔗 CHAMADA POR  : PdfViewer (thumbnails).
         ****************************************************************************************/
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object result = pdfviewer.GetThumbnailImages(jsonObject);
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderThumbnailImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("Bookmarks")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: Bookmarks
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar bookmarks do documento PDF.
         *
         * 📥 ENTRADAS     : jsonObject com referência do documento.
         *
         * 📤 SAÍDAS       : JSON serializado com bookmarks.
         *
         * 🔗 CHAMADA POR  : PdfViewer (bookmarks).
         ****************************************************************************************/
        public IActionResult Bookmarks([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object jsonResult = pdfviewer.GetBookmarks(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Bookmarks" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("Download")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: Download
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exportar o documento em base64 para download.
         *
         * 📥 ENTRADAS     : jsonObject com referência do documento.
         *
         * 📤 SAÍDAS       : Content com base64 do PDF.
         *
         * 🔗 CHAMADA POR  : PdfViewer (download).
         ****************************************************************************************/
        public IActionResult Download([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
                return Content(documentBase);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Download" , error);
                return Content(string.Empty);
            }
        }

        [HttpPost]
        [Route("PrintImages")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: PrintImages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar imagens para impressão do PDF.
         *
         * 📥 ENTRADAS     : jsonObject com parâmetros de impressão.
         *
         * 📤 SAÍDAS       : JSON serializado com imagens.
         *
         * 🔗 CHAMADA POR  : PdfViewer (print).
         ****************************************************************************************/
        public IActionResult PrintImages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object pageImage = pdfviewer.GetPrintImage(jsonObject);
                return Content(JsonConvert.SerializeObject(pageImage));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PrintImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost]
        [Route("ExportAnnotations")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportAnnotations
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exportar anotações do documento.
         *
         * 📥 ENTRADAS     : jsonObject com referência do documento.
         *
         * 📤 SAÍDAS       : Content com JSON de anotações.
         *
         * 🔗 CHAMADA POR  : PdfViewer (export annotations).
         ****************************************************************************************/
        public IActionResult ExportAnnotations([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
                return Content(jsonResult);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportAnnotations" , error);
                return Content(string.Empty);
            }
        }

        [HttpPost]
        [Route("ImportAnnotations")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportAnnotations
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar anotações a partir de arquivo no servidor.
         *
         * 📥 ENTRADAS     : jsonObject com fileName/document.
         *
         * 📤 SAÍDAS       : Content com JSON de anotações.
         *
         * 🔗 CHAMADA POR  : PdfViewer (import annotations).
         *
         * 🔄 CHAMA        : GetDocumentPath().
         ****************************************************************************************/
        public IActionResult ImportAnnotations([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                string jsonResult = string.Empty;

                if (jsonObject != null && jsonObject.ContainsKey("fileName"))
                {
                    string documentPath = GetDocumentPath(jsonObject["fileName"]);
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        jsonResult = System.IO.File.ReadAllText(documentPath);
                    }
                    else
                    {
                        return Content(jsonObject["document"] + " não encontrado");
                    }
                }

                return Content(jsonResult);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportAnnotations" , error);
                return Content(string.Empty);
            }
        }

        [HttpPost]
        [Route("ExportFormFields")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportFormFields
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exportar campos de formulário do PDF.
         *
         * 📥 ENTRADAS     : jsonObject com referência do documento.
         *
         * 📤 SAÍDAS       : Content com JSON de campos.
         *
         * 🔗 CHAMADA POR  : PdfViewer (export form fields).
         ****************************************************************************************/
        public IActionResult ExportFormFields([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                string result = pdfviewer.ExportFormFields(jsonObject);
                return Content(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportFormFields" , error);
                return Content(string.Empty);
            }
        }

        [HttpPost]
        [Route("ImportFormFields")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportFormFields
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar campos de formulário no documento.
         *
         * 📥 ENTRADAS     : jsonObject com dados de formulário.
         *
         * 📤 SAÍDAS       : JSON serializado com resultado da importação.
         *
         * 🔗 CHAMADA POR  : PdfViewer (import form fields).
         ****************************************************************************************/
        public IActionResult ImportFormFields([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object result = pdfviewer.ImportFormFields(jsonObject);
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetDocumentPath (Helper)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Resolver caminho absoluto do documento do viewer.
         *
         * 📥 ENTRADAS     : document (nome/arquivo).
         *
         * 📤 SAÍDAS       : Caminho completo do arquivo ou string vazia.
         *
         * 🔄 CHAMA        : IWebHostEnvironment.WebRootPath.
         ****************************************************************************************/
        private string GetDocumentPath(string document)
        {
            try
            {
                string documentPath = string.Empty;
                if (!System.IO.File.Exists(document))
                {
                    string basePath = _hostingEnvironment.WebRootPath;
                    string dataPath = string.Empty;
                    dataPath = basePath + @"/scripts/pdfviewer/";
                    if (System.IO.File.Exists(dataPath + document))
                        documentPath = dataPath + document;
                }
                else
                {
                    documentPath = document;
                }
                return documentPath;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocumentPath" , error);
                return string.Empty;
            }
        }

        [HttpPost]
        [Route("GetDocument")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: GetDocument
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar CNH digital (base64) do motorista.
         *
         * 📥 ENTRADAS     : id (Guid do motorista).
         *
         * 📤 SAÍDAS       : String base64 no formato data:application/pdf;base64,.
         *
         * 🔗 CHAMADA POR  : Viewer da CNH.
         *
         * 🔄 CHAMA        : IUnitOfWork.Motorista.GetFirstOrDefault().
         ****************************************************************************************/
        public string GetDocument(Guid id)
        {
            try
            {
                var objFromDb = _unitOfWork.Motorista.GetFirstOrDefault(u => u.MotoristaId == id);
                if (objFromDb != null)
                {
                    if (objFromDb.CNHDigital != null)
                    {
                        byte[] byteArray = objFromDb.CNHDigital;
                        return "data:application/pdf;base64," + Convert.ToBase64String(byteArray);
                    }
                }

                return "data:application/pdf;base64,";
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocument" , error);
                return string.Empty;
            }
        }
    }
}
