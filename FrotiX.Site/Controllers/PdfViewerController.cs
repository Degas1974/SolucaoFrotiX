/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: PdfViewerController.cs                                           ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

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
     * ⚡ CONTROLLER: PdfViewer API (Syncfusion)
     * 🎯 OBJETIVO: Fornecer endpoints para visualização e manipulação de PDFs com Syncfusion
     * 📋 ROTAS: /api/PdfViewer/* (Load, Bookmarks, RenderPdfPages, Download, Print, etc)
     * 🔗 ENTIDADES: Nenhuma (manipulação de arquivos PDF)
     * 📦 DEPENDÊNCIAS: Syncfusion.EJ2.PdfViewer, IWebHostEnvironment
     * 🌐 CORS: AllowAllOrigins habilitado em todos os endpoints
     ****************************************************************************************/
    [Route("api/[controller]")]
    [ApiController]
    public class PdfViewerController :ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Load
         * 🎯 OBJETIVO: Carregar PDF de arquivo físico ou string base64 para o viewer
         * 📥 ENTRADAS: jsonObject { document, isFileName? }
         * 📤 SAÍDAS: JSON serializado com dados do PDF
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (JavaScript frontend)
         * 🔄 CHAMA: PdfRenderer.Load()
         ****************************************************************************************/
        [HttpPost("Load")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
        public IActionResult Load([FromBody] Dictionary<string , string> jsonObject)
        {
            try
            {
                PdfRenderer pdfviewer = new PdfRenderer();
                MemoryStream stream = new MemoryStream();
                object jsonResult = new object();

                if (jsonObject != null && jsonObject.ContainsKey("document"))
                {
                    // [DOC] Suporta dois modos: arquivo físico (isFileName=true) ou base64
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
                        // [DOC] Decodifica PDF de string base64
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Bookmarks
         * 🎯 OBJETIVO: Obter marcadores (bookmarks) do PDF
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: JSON com estrutura de bookmarks
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer
         * 🔄 CHAMA: PdfRenderer.GetBookmarks()
         ****************************************************************************************/
        [HttpPost("Bookmarks")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfPages
         * 🎯 OBJETIVO: Renderizar páginas específicas do PDF
         * 📥 ENTRADAS: jsonObject (documento e índices de páginas)
         * 📤 SAÍDAS: JSON com imagens das páginas renderizadas
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (navegação de páginas)
         * 🔄 CHAMA: PdfRenderer.GetPage()
         ****************************************************************************************/
        [HttpPost("RenderPdfPages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfTexts
         * 🎯 OBJETIVO: Extrair texto do PDF (para busca e seleção)
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: JSON com texto extraído do PDF
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (funcionalidade de busca)
         * 🔄 CHAMA: PdfRenderer.GetDocumentText()
         ****************************************************************************************/
        [HttpPost("RenderPdfTexts")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderThumbnailImages
         * 🎯 OBJETIVO: Gerar miniaturas (thumbnails) das páginas do PDF
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: JSON com imagens thumbnail das páginas
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (painel de miniaturas)
         * 🔄 CHAMA: PdfRenderer.GetThumbnailImages()
         ****************************************************************************************/
        [HttpPost("RenderThumbnailImages")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderAnnotationComments
         * 🎯 OBJETIVO: Obter comentários de anotações do PDF
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: JSON com comentários das anotações
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (painel de comentários)
         * 🔄 CHAMA: PdfRenderer.GetAnnotationComments()
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportAnnotations
         * 🎯 OBJETIVO: Exportar anotações do PDF (marcações, destaques, comentários)
         * 📥 ENTRADAS: jsonObject (documento e anotações)
         * 📤 SAÍDAS: String JSON com anotações exportadas
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (botão Exportar Anotações)
         * 🔄 CHAMA: PdfRenderer.ExportAnnotation()
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportAnnotations
         * 🎯 OBJETIVO: Importar anotações para o PDF
         * 📥 ENTRADAS: jsonObject (documento e anotações a importar)
         * 📤 SAÍDAS: JSON com resultado da importação
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (botão Importar Anotações)
         * 🔄 CHAMA: PdfRenderer.ImportAnnotation()
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportAnnotations" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportFormFields
         * 🎯 OBJETIVO: Exportar campos de formulário PDF (valores preenchidos)
         * 📥 ENTRADAS: jsonObject (documento e form fields)
         * 📤 SAÍDAS: String JSON com campos exportados
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (exportação de formulários)
         * 🔄 CHAMA: PdfRenderer.ExportFormFields()
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ExportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportFormFields
         * 🎯 OBJETIVO: Importar/preencher campos de formulário PDF
         * 📥 ENTRADAS: jsonObject (documento e valores dos campos)
         * 📤 SAÍDAS: JSON com resultado da importação
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (importação de formulários)
         * 🔄 CHAMA: PdfRenderer.ImportFormFields()
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "ImportFormFields" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Unload
         * 🎯 OBJETIVO: Limpar cache do PDF quando viewer é fechado (liberar memória)
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: Mensagem de sucesso
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (evento onUnload)
         * 🔄 CHAMA: PdfRenderer.ClearCache()
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "Unload" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Download
         * 🎯 OBJETIVO: Baixar PDF completo como string base64
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: String base64 do PDF
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (botão Download)
         * 🔄 CHAMA: PdfRenderer.GetDocumentAsBase64()
         ****************************************************************************************/
        [HttpPost("Download")]
        [Microsoft.AspNetCore.Cors.EnableCors("AllowAllOrigins")]
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: PrintImages
         * 🎯 OBJETIVO: Gerar imagens das páginas do PDF para impressão
         * 📥 ENTRADAS: jsonObject (documento)
         * 📤 SAÍDAS: JSON com imagens das páginas para impressão
         * 🔗 CHAMADA POR: Syncfusion PDF Viewer (botão Imprimir)
         * 🔄 CHAMA: PdfRenderer.GetPrintImage()
         ****************************************************************************************/
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
                Alerta.TratamentoErroComLinha("PdfViewerController.cs" , "PrintImages" , error);
                return Content(JsonConvert.SerializeObject(new
                {
                    error = error.Message
                }));
            }
        }
    }
}
