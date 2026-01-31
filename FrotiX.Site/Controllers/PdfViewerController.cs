/* ****************************************************************************************
 * ⚡ ARQUIVO: PdfViewerController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Disponibilizar endpoints do Syncfusion PdfViewer para PDFs gerais,
 *                   com suporte a arquivo físico ou conteúdo base64.
 *
 * 📥 ENTRADAS     : Payloads JSON do viewer.
 *
 * 📤 SAÍDAS       : JSON/Content com páginas, anotações, base64 e mensagens de erro.
 *
 * 🔗 CHAMADA POR  : Componentes PdfViewer via API.
 *
 * 🔄 CHAMA        : PdfRenderer (Syncfusion), IWebHostEnvironment.
 **************************************************************************************** */

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Syncfusion.EJ2.PdfViewer;
using System;
using System.Collections.Generic;
using System.IO;

namespace FrotiX.Controllers.API
{
    /****************************************************************************************
     * ⚡ CONTROLLER: PdfViewerController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Servir operações do PdfViewer para PDFs gerais.
     *
     * 📥 ENTRADAS     : JSONs do viewer (document, isFileName etc).
     *
     * 📤 SAÍDAS       : JSON/Content com renderização, anotações e downloads.
     *
     * 🔗 CHAMADA POR  : PdfViewer em páginas diversas.
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class PdfViewerController :ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        /****************************************************************************************
         * ⚡ FUNÇÃO: PdfViewerController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar o hosting environment para resolver caminhos.
         *
         * 📥 ENTRADAS     : hostingEnvironment.
         *
         * 📤 SAÍDAS       : Instância configurada do controller.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public PdfViewerController(IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PdfViewerController" , error);
            }
        }

        [HttpPost("Load")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
         ****************************************************************************************/
        public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                MemoryStream stream = new MemoryStream();
                object jsonResult = new object();

                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    if (bool.TryParse(jsonObject["isFileName"] , out bool isFileName) && isFileName)
                    {
                        string documentPath = jsonObject["document"].TrimStart('/');
                        string fullPath = Path.Combine(_hostingEnvironment.WebRootPath , documentPath);

                        if (System.IO.File.Exists(fullPath))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                            stream = new MemoryStream(bytes);
                        }
                        else
                        {
                            return Content(JsonConvert.SerializeObject(new
                            {
                                error = "Arquivo não encontrado: " + fullPath
                            }));
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Load" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("Bookmarks")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetBookmarks(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Bookmarks" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("RenderPdfPages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetPage(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfPages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("RenderPdfTexts")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfTexts
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Extrair texto do documento PDF.
         *
         * 📥 ENTRADAS     : jsonObject com referência do documento.
         *
         * 📤 SAÍDAS       : JSON serializado com texto extraído.
         *
         * 🔗 CHAMADA POR  : PdfViewer (render texts).
         ****************************************************************************************/
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.GetDocumentText(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderPdfTexts" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("RenderThumbnailImages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                object result = pdfviewer.GetThumbnailImages(jsonObject);
                return Content(JsonConvert.SerializeObject(result));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "RenderThumbnailImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("RenderAnnotationComments")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        [HttpPost("ImportAnnotations")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportAnnotations
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar anotações no documento.
         *
         * 📥 ENTRADAS     : jsonObject com dados de anotações.
         *
         * 📤 SAÍDAS       : JSON serializado com resultado.
         *
         * 🔗 CHAMADA POR  : PdfViewer (import annotations).
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportAnnotations" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("ExportFormFields")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                string jsonResult = pdfviewer.ExportFormFields(jsonObject);
                return Content(jsonResult);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ExportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("ImportFormFields")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                object jsonResult = pdfviewer.ImportFormFields(jsonObject);
                return Content(JsonConvert.SerializeObject(jsonResult));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("Unload")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                pdfviewer.ClearCache(jsonObject);
                return Ok("Document cache cleared");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Unload" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("Download")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                string documentBase = pdfviewer.GetDocumentAsBase64(jsonObject);
                return Content(documentBase);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Download" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        [HttpPost("PrintImages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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
                PdfRenderer pdfviewer = new PdfRenderer();
                object pageImage = pdfviewer.GetPrintImage(jsonObject);
                return Content(JsonConvert.SerializeObject(pageImage));
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PrintImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }
    }
}
