/* ****************************************************************************************
 * ⚡ ARQUIVO: MultaPdfViewerController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Servir PDFs de multas para o componente Syncfusion PDF Viewer,
 *                   com cache em memória para otimizar o carregamento.
 *
 * 📥 ENTRADAS     : Payload JSON do Syncfusion (document, isFileName, etc.).
 *
 * 📤 SAÍDAS       : JSON com páginas, textos, miniaturas e anotações; downloads e prints.
 *
 * 🔗 CHAMADA POR  : Frontend (Syncfusion PDF Viewer) nas telas de multas.
 *
 * 🔄 CHAMA        : PdfRenderer (Syncfusion), File System, IMemoryCache.
 *
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 PDF Viewer, IMemoryCache, IWebHostEnvironment.
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: MultaPdfViewerController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Controlar operações de visualização, renderização e exportação
 *                   de PDFs de multas.
 *
 * 📥 ENTRADAS     : Dicionários JSON enviados pelo PDF Viewer.
 *
 * 📤 SAÍDAS       : JSON/Content para páginas, textos, miniaturas e arquivos.
 *
 * 🔗 CHAMADA POR  : Componentes JS do Syncfusion PDF Viewer.
 *
 * 🔄 CHAMA        : PdfRenderer, ResolveDocumentStream, File IO.
 *
 * 📦 DEPENDÊNCIAS : Syncfusion EJ2 PDF Viewer, IMemoryCache, File System.
 ****************************************************************************************/
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
    [ApiController]
    [Route("api/[controller]")]
    public class MultaPdfViewerController :Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _cache;

        /****************************************************************************************
         * ⚡ FUNÇÃO: MultaPdfViewerController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências de ambiente e cache.
         *
         * 📥 ENTRADAS     : env, cache.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public MultaPdfViewerController(
            IWebHostEnvironment env ,
            IMemoryCache cache)
        {
            try
            {
                _env = env;
                _cache = cache;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "MultaPdfViewerController" , error);
            }
        }

        private string ResolveFolder()
        {
            try
            {
                var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath , "wwwroot");
                return Path.Combine(root , "DadosEditaveis" , "Multas");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveFolder" , error);
                return string.Empty;
            }
        }

        private Stream ResolveDocumentStream(Dictionary<string , string> json)
        {
            try
            {
                if (json == null)
                    return new MemoryStream();

                bool isFileName = true;
                if (json.TryGetValue("isFileName" , out var isFileNameStr))
                    bool.TryParse(isFileNameStr , out isFileName);

                if (!json.TryGetValue("document" , out var document) || string.IsNullOrWhiteSpace(document))
                    return new MemoryStream();

                if (isFileName)
                {
                    var folder = ResolveFolder();
                    var path = Path.Combine(folder , Path.GetFileName(document));

                    if (!System.IO.File.Exists(path))
                        throw new FileNotFoundException($"{document} não encontrado em {folder}");

                    var ms = new MemoryStream();
                    using (var fs = new FileStream(path , FileMode.Open , FileAccess.Read , FileShare.ReadWrite))
                    {
                        fs.CopyTo(ms);
                    }
                    ms.Position = 0;
                    return ms;
                }
                else
                {
                    byte[] bytes;
                    try
                    {
                        bytes = Convert.FromBase64String(document);
                    }
                    catch
                    {
                        bytes = Array.Empty<byte>();
                    }
                    return new MemoryStream(bytes);
                }
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveDocumentStream" , error);
                return new MemoryStream();
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Load
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Carregar documento PDF no viewer a partir do payload recebido.
         *
         * 📥 ENTRADAS     : json (document, isFileName, etc.).
         *
         * 📤 SAÍDAS       : JSON do Syncfusion com metadados do documento.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (Load).
         *
         * 🔄 CHAMA        : PdfRenderer.Load(), ResolveDocumentStream().
         ****************************************************************************************/
        [HttpPost("Load")]
        public IActionResult Load([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);

                var stream = ResolveDocumentStream(json);
                stream.Position = 0;

                var output = viewer.Load(stream , json);
                return Content(JsonConvert.SerializeObject(output) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Load" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfPages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar páginas do PDF sob demanda.
         *
         * 📥 ENTRADAS     : json com parâmetros de página.
         *
         * 📤 SAÍDAS       : JSON com imagens/streams das páginas.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (page rendering).
         *
         * 🔄 CHAMA        : PdfRenderer.GetPage().
         ****************************************************************************************/
        [HttpPost("RenderPdfPages")]
        public IActionResult RenderPdfPages([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetPage(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfPages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderPdfTexts
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Extrair textos do PDF para busca e seleção.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com textos do documento.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (text extraction).
         *
         * 🔄 CHAMA        : PdfRenderer.GetDocumentText().
         ****************************************************************************************/
        [HttpPost("RenderPdfTexts")]
        public IActionResult RenderPdfTexts([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetDocumentText(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfTexts" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderThumbnailImages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar miniaturas das páginas do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com miniaturas.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (thumbnails).
         *
         * 🔄 CHAMA        : PdfRenderer.GetThumbnailImages().
         ****************************************************************************************/
        [HttpPost("RenderThumbnailImages")]
        public IActionResult RenderThumbnailImages([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetThumbnailImages(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderThumbnailImages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Bookmarks
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Retornar bookmarks do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com bookmarks.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (bookmarks).
         *
         * 🔄 CHAMA        : PdfRenderer.GetBookmarks().
         ****************************************************************************************/
        [HttpPost("Bookmarks")]
        public IActionResult Bookmarks([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetBookmarks(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Bookmarks" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: RenderAnnotationComments
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Renderizar comentários/anotações do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com anotações.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (annotations).
         *
         * 🔄 CHAMA        : PdfRenderer.GetAnnotationComments().
         ****************************************************************************************/
        [HttpPost("RenderAnnotationComments")]
        public IActionResult RenderAnnotationComments([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetAnnotationComments(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderAnnotationComments" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Unload
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Liberar recursos do documento no viewer.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com status de unload.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (unload).
         *
         * 🔄 CHAMA        : PdfRenderer.ClearCache().
         ****************************************************************************************/
        [HttpPost("Unload")]
        public IActionResult Unload([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                viewer.ClearCache(json);
                return Content("Document cache is cleared" , "text/plain; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Unload" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportAnnotations
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exportar anotações do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com anotações exportadas.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (export annotations).
         *
         * 🔄 CHAMA        : PdfRenderer.ExportAnnotations().
         ****************************************************************************************/
        [HttpPost("ExportAnnotations")]
        public IActionResult ExportAnnotations([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.ExportAnnotation(json);
                return Content(result , "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportAnnotations" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportAnnotations
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar anotações para o PDF.
         *
         * 📥 ENTRADAS     : json com anotações.
         *
         * 📤 SAÍDAS       : JSON com status da importação.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (import annotations).
         *
         * 🔄 CHAMA        : PdfRenderer.ImportAnnotations().
         ****************************************************************************************/
        [HttpPost("ImportAnnotations")]
        public IActionResult ImportAnnotations([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);

                if (json != null && json.ContainsKey("fileName"))
                {
                    var path = Path.Combine(ResolveFolder() , Path.GetFileName(json["fileName"]));
                    if (System.IO.File.Exists(path))
                    {
                        var xfdf = System.IO.File.ReadAllText(path);
                        return Content(xfdf , "application/json; charset=utf-8");
                    }
                    return Content($"{json["fileName"]} not found" , "text/plain; charset=utf-8");
                }

                var result = viewer.ImportAnnotation(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportAnnotations" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ExportFormFields
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Exportar campos de formulário do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com campos exportados.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (export form fields).
         *
         * 🔄 CHAMA        : PdfRenderer.ExportFormFields().
         ****************************************************************************************/
        [HttpPost("ExportFormFields")]
        public IActionResult ExportFormFields([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.ExportFormFields(json);
                return Content(result , "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportFormFields" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: ImportFormFields
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Importar campos de formulário no PDF.
         *
         * 📥 ENTRADAS     : json com campos do formulário.
         *
         * 📤 SAÍDAS       : JSON com status da importação.
         *
         * 🔗 CHAMADA POR  : Syncfusion PDF Viewer (import form fields).
         *
         * 🔄 CHAMA        : PdfRenderer.ImportFormFields().
         ****************************************************************************************/
        [HttpPost("ImportFormFields")]
        public IActionResult ImportFormFields([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);

                if (json != null && json.ContainsKey("data"))
                    json["data"] = Path.Combine(ResolveFolder() , Path.GetFileName(json["data"]));

                var result = viewer.ImportFormFields(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportFormFields" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: Download
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar download do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : Arquivo PDF para download.
         *
         * 🔗 CHAMADA POR  : Botão de download do viewer.
         *
         * 🔄 CHAMA        : PdfRenderer.GetDocumentAsBase64().
         ****************************************************************************************/
        [HttpPost("Download")]
        public IActionResult Download([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var base64 = viewer.GetDocumentAsBase64(json);
                return Content(base64 , "text/plain; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Download" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /****************************************************************************************
         * ⚡ FUNÇÃO: PrintImages
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar imagens para impressão do PDF.
         *
         * 📥 ENTRADAS     : json com parâmetros do documento.
         *
         * 📤 SAÍDAS       : JSON com imagens para impressão.
         *
         * 🔗 CHAMADA POR  : Botão de impressão do viewer.
         *
         * 🔄 CHAMA        : PdfRenderer.GetPrintImage().
         ****************************************************************************************/
        [HttpPost("PrintImages")]
        public IActionResult PrintImages([FromBody] Dictionary<string , string> json)
        {
            try
            {
                var viewer = new PdfRenderer(_cache);
                var result = viewer.GetPrintImage(json);
                return Content(JsonConvert.SerializeObject(result) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "PrintImages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }
    }
}
