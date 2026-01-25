/*
*  #################################################################################################
*  #                                                                                               #
*  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
*  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
*  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
*  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
*  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
*  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
*  #                                                                                               #
*  #   PROJETO: FROTIX - GESTÃO DE FROTAS                                                          #
*  #   MODULO:  DOCUMENTOS (VISUALIZADOR CNH)                                                      #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #                                                                                               #
*  #################################################################################################
*/

using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: PdfViewerCNHController                                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Subsistema de visualização de documentos de habilitação (CNH).            ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API/MVC                                                          ║
    /// ║    • Rota base: /api/PdfViewerCNH                                          ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class PdfViewerCNHController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PdfViewerCNHController (Construtor)                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador do visualizador de CNH.                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • hostingEnvironment (IWebHostEnvironment): Ambiente de hospedagem.      ║
        /// ║    • cache (IMemoryCache): Cache em memória.                                 ║
        /// ║    • unitOfWork (IUnitOfWork): Acesso a dados.                               ║
        /// ║    • logService (ILogService): Serviço de log centralizado.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public PdfViewerCNHController(
            IWebHostEnvironment hostingEnvironment ,
            IMemoryCache cache ,
            IUnitOfWork unitOfWork ,
            ILogService logService
        )
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
                _cache = cache;
                _unitOfWork = unitOfWork;
                _log = logService;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PdfViewerCNHController" , error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PdfViewerFeatures                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna a view principal do visualizador.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public IActionResult PdfViewerFeatures()
        {
            try
            {
                // [VIEW] Retorna view do visualizador.
                return View();
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.PdfViewerFeatures", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PdfViewerFeatures" , error);
                return View();
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Load (POST)                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega arquivo PDF (Path ou Base64) para o Syncfusion Viewer.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do PDF.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("Load")]
        public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                MemoryStream stream = new MemoryStream();
                object jsonResult = new object();

                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    if (bool.Parse(jsonObject["isFileName"]))
                    {
                        // [ARQUIVO] Carrega arquivo físico.
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
                        // [ARQUIVO] Converte Base64 para stream.
                        byte[] bytes = Convert.FromBase64String(jsonObject["document"]);
                        stream = new MemoryStream(bytes);
                    }
                }

                // [RETORNO] Retorna resultado do viewer.
                jsonResult = pdfviewer.Load(stream , jsonObject);
                _log.Info("PdfViewerCNHController.Load: Documento carregado no visualizador.");
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.Load", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Load" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RenderPdfPages (POST)                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza páginas sob demanda para o viewer.                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com páginas renderizadas.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("RenderPdfPages")]
        public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object jsonResult = pdfviewer.GetPage(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.RenderPdfPages", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderPdfPages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RenderAnnotationComments (POST)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Processa comentários e anotações do PDF.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com anotações.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("RenderAnnotationComments")]
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.RenderAnnotationComments", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderAnnotationComments" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Unload (POST)                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Limpa o cache do documento quando o viewer é fechado.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: Status da limpeza.                                       ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("Unload")]
        public IActionResult Unload([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer e limpa cache.
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                pdfviewer.ClearCache(jsonObject);
                return Content("Document cache is cleared");
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.Unload", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Unload" , error);
                return Content("Erro ao limpar cache");
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RenderThumbnailImages (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Renderiza miniaturas das páginas do PDF.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com miniaturas.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost]
        [Route("RenderThumbnailImages")]
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
            // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                object result = pdfviewer.GetThumbnailImages(jsonObject);
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.RenderThumbnailImages", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "RenderThumbnailImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Bookmarks
        /// │ DESCRIÇÃO: Obtém marcadores do documento PDF.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("Bookmarks")]
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
                _log.Error("PdfViewerCNHController.Bookmarks", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Bookmarks" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Download
        /// │ DESCRIÇÃO: Retorna o documento PDF em formato Base64 para download.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("Download")]
        public IActionResult Download([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer(_cache);
                string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
                _log.Info("PdfViewerCNHController.Download: Download de PDF (CNH) realizado.");
                return Content(documentBase);
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerCNHController.Download", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "Download" , error);
                return Content(string.Empty);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Imprimir Imagens
        /// │ DESCRIÇÃO: Renderiza imagens otimizadas para impressão.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("PrintImages")]
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
                _log.Error("PdfViewerCNHController.PrintImages", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "PrintImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Exportar Anotações
        /// │ DESCRIÇÃO: Exporta as anotações do PDF para formato JSON.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("ExportAnnotations")]
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
                _log.Error("PdfViewerCNHController.ExportAnnotations", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportAnnotations" , error);
                return Content(string.Empty);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Importar Anotações
        /// │ DESCRIÇÃO: Importa anotações de um arquivo JSON.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("ImportAnnotations")]
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
                _log.Error("PdfViewerCNHController.ImportAnnotations", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportAnnotations" , error);
                return Content(string.Empty);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Exportar Campos de Formulário
        /// │ DESCRIÇÃO: Exporta os valores preenchidos no PDF.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("ExportFormFields")]
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
                _log.Error("PdfViewerCNHController.ExportFormFields", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ExportFormFields" , error);
                return Content(string.Empty);
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Importar Campos de Formulário
        /// │ DESCRIÇÃO: Importa dados para preencher campos no PDF.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("ImportFormFields")]
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
                _log.Error("PdfViewerCNHController.ImportFormFields", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "ImportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Obter Caminho do Documento
        /// │ DESCRIÇÃO: Resolve o caminho físico do arquivo PDF no servidor.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
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
                _log.Error("PdfViewerCNHController.GetDocumentPath", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocumentPath" , error);
                return string.Empty;
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Obter Documento (Base64)
        /// │ DESCRIÇÃO: Recupera a CNH do motorista do banco e retorna em Base64 para exibição.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost]
        [Route("GetDocument")]
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
                _log.Error("PdfViewerCNHController.GetDocument", error);
                Alerta.TratamentoErroComLinha("PdfViewerCNHController.cs" , "GetDocument" , error);
                return string.Empty;
            }
        }
    }
}
