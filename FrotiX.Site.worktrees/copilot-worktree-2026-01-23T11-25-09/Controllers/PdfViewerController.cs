using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;
using FrotiX.Services;

namespace FrotiX.Controllers.API
{
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
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  SERVIÇO DE VISUALIZAÇÃO DE DOCUMENTOS (PDF)                                         #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: PdfViewerController                                                ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Controlador utilitário para Syncfusion PdfViewer (Docs Gerais).           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/PdfViewer                                             ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class PdfViewerController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: PdfViewerController (Construtor)                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o visualizador genérico de documentos FrotiX.                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • hostingEnvironment (IWebHostEnvironment): Ambiente de hospedagem.      ║
        /// ║    • log (ILogService): Serviço de log centralizado.                         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public PdfViewerController(IWebHostEnvironment hostingEnvironment, ILogService log)
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
                _log = log;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs", "PdfViewerController", error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Load (POST)                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega documento por caminho relativo (wwwroot) ou Base64.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do PDF.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Load")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer();
                MemoryStream stream = new MemoryStream();
                object jsonResult = new object();

                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    if (bool.TryParse(jsonObject["isFileName"] , out bool isFileName) && isFileName)
                    {
                        // [ARQUIVO] Resolve caminho físico do documento.
                        string documentPath = jsonObject["document"].TrimStart('/');
                        string fullPath = Path.Combine(_hostingEnvironment.WebRootPath , documentPath);

                        if (System.IO.File.Exists(fullPath))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                            stream = new MemoryStream(bytes);
                        }
                        else
                        {
                            // [RETORNO] Arquivo não encontrado.
                            _log.Warning($"PdfViewerController.Load: Arquivo não encontrado em {fullPath}");
                            return Content(JsonConvert.SerializeObject(new
                            {
                                error = "Arquivo não encontrado: " + fullPath
                            }));
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
                _log.Info("PdfViewerController.Load: PDF carregado com sucesso (Genérico).");
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.Load", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Load" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Bookmarks (POST)                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna os marcadores do PDF para navegação.                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com bookmarks.                                      ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Bookmarks")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult Bookmarks([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetBookmarks(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.Bookmarks", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Bookmarks" , error);
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
        /// ║    Renderiza páginas individuais sob demanda.                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com páginas renderizadas.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("RenderPdfPages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetPage(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.RenderPdfPages", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfPages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RenderPdfTexts (POST)                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Extrai texto do PDF para busca e seleção.                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com texto extraído.                                 ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("RenderPdfTexts")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetDocumentText(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.RenderPdfTexts", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfTexts" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RenderThumbnailImages (POST)                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Gera miniaturas das páginas para a barra lateral.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • jsonObject (Dictionary<string,string>): Payload do viewer.              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com miniaturas.                                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("RenderThumbnailImages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
            // [SERVICO] Inicializa renderer.
                PdfRenderer pdfviewer = new PdfRenderer();
                object result = pdfviewer.GetThumbnailImages(jsonObject);
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.RenderThumbnailImages", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderThumbnailImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("RenderAnnotationComments")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetAnnotationComments(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderAnnotationComments" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("ExportAnnotations")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult ExportAnnotations([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                string jsonResult = pdfviewer.ExportAnnotation(jsonObject);
                return Content(jsonResult);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ExportAnnotations" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Importar Anotações
        /// │ DESCRIÇÃO: Carrega marcações de terceiros no visualizador.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("ImportAnnotations")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult ImportAnnotations([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.ImportAnnotation(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.ImportAnnotations", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportAnnotations" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Exportar Campos de Formulário
        /// │ DESCRIÇÃO: Exporta dados de campos preenchíveis do PDF.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("ExportFormFields")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult ExportFormFields([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                string jsonResult = pdfviewer.ExportFormFields(jsonObject);
                return Content(jsonResult);
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.ExportFormFields", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ExportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Importar Campos de Formulário
        /// │ DESCRIÇÃO: Preenche os campos do PDF com dados fornecidos.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("ImportFormFields")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult ImportFormFields([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.ImportFormFields(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.ImportFormFields", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Descarregar (Unload)
        /// │ DESCRIÇÃO: Limpa o cache do documento visualizado.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("Unload")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult Unload([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                pdfviewer.ClearCache(jsonObject);
                return Ok("Document cache cleared");
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.Unload", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Unload" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Download (Base64)
        /// │ DESCRIÇÃO: Retorna o documento PDF em formato Base64 para download.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("Download")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult Download([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
                _log.Info("PdfViewerController.Download: Documento exportado em Base64 para download.");
                return Content(documentBase);
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.Download", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Download" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────────────
        /// │ FUNCIONALIDADE: Imagens de Impressão
        /// │ DESCRIÇÃO: Gera imagens otimizadas para o processo de impressão do PDF.
        /// │──────────────────────────────────────────────────────────────────────────────
        /// </summary>
        [HttpPost("PrintImages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult PrintImages([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                object pageImage = pdfviewer.GetPrintImage(jsonObject);
                return Content(JsonConvert.SerializeObject(pageImage));
            }
            catch (Exception error)
            {
                _log.Error("PdfViewerController.PrintImages", error);
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PrintImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }
    }
}
