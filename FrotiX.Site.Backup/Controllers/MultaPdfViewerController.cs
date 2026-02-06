using FrotiX.Services;
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
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MultaPdfViewerController                                           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Backend para o componente Syncfusion PdfViewer.                           ║
    /// ║    Responsável por carregar, renderizar e manipular PDFs de Multas.           ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/MultaPdfViewer                                         ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [ApiController]
    [Route("api/[controller]")]
    public class MultaPdfViewerController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _cache;
        private readonly ILogService _logService;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MultaPdfViewerController (Construtor)                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador com serviços ambientais, cache e log.            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • env (IWebHostEnvironment): Ambiente da aplicação.                       ║
        /// ║    • cache (IMemoryCache): Cache em memória.                                 ║
        /// ║    • logService (ILogService): Serviço de log centralizado.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public MultaPdfViewerController(
            IWebHostEnvironment env ,
            IMemoryCache cache ,
            ILogService logService)
        {
            try
            {
                _env = env;
                _cache = cache;
                _logService = logService;
            }
            catch (Exception error)
            {
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "MultaPdfViewerController (Construtor)");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "MultaPdfViewerController" , error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ResolveFolder                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Retorna o caminho físico da pasta onde os PDFs estão armazenados.         ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private string ResolveFolder()
        {
            try
            {
                // [DADOS] Resolve raiz do conteúdo e pasta de multas.
                var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath , "wwwroot");
                return Path.Combine(root , "DadosEditaveis" , "Multas");
            }
            catch (Exception error)
            {
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveFolder");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveFolder" , error);
                return string.Empty;
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: ResolveDocumentStream                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém o Stream do arquivo via nome do arquivo ou Base64.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private Stream ResolveDocumentStream(Dictionary<string , string> json)
        {
            try
            {
                // [VALIDACAO] Garante payload válido.
                if (json == null)
                    return new MemoryStream();

                // [DADOS] Identifica se é nome de arquivo.
                bool isFileName = true;
                if (json.TryGetValue("isFileName" , out var isFileNameStr))
                    bool.TryParse(isFileNameStr , out isFileName);

                if (!json.TryGetValue("document" , out var document) || string.IsNullOrWhiteSpace(document))
                    return new MemoryStream();

                if (isFileName)
                {
                    // [ARQUIVO] Carrega arquivo físico por nome.
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
                    // [ARQUIVO] Converte Base64 para stream.
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ResolveDocumentStream");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ResolveDocumentStream" , error);
                return new MemoryStream();
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Load (POST)                                                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Carrega o arquivo PDF inicial solicitado pelo componente PDF Viewer.     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com dados do PDF.                                   ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Load")]
        public IActionResult Load([FromBody] Dictionary<string , string> json)
        {
            try
            {
            // [SERVICO] Inicializa renderer do PdfViewer.
                var viewer = new PdfRenderer(_cache);

            // [ARQUIVO] Resolve stream do documento.
                var stream = ResolveDocumentStream(json);
                stream.Position = 0;

            // [RETORNO] Retorna resultado serializado.
                var output = viewer.Load(stream , json);
                return Content(JsonConvert.SerializeObject(output) ,
                               "application/json; charset=utf-8");
            }
            catch (Exception error)
            {
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Load");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Load" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: RenderPdfPages                                                                             |
        /// | Descrição: Renderiza individualmente as páginas do PDF sob demanda.                               |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfPages");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfPages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: RenderPdfTexts                                                                             |
        /// | Descrição: Recupera as camadas de texto do PDF para fins de busca e seleção.                     |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderPdfTexts");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderPdfTexts" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: RenderThumbnailImages                                                                      |
        /// | Descrição: Renderiza as miniaturas (thumbnails) das páginas do PDF.                               |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderThumbnailImages");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderThumbnailImages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Bookmarks                                                                                  |
        /// | Descrição: Recupera os marcadores (bookmarks/sumário) internos do PDF.                           |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Bookmarks");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Bookmarks" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: RenderAnnotationComments                                                                   |
        /// | Descrição: Renderiza os comentários e anotações presentes no PDF.                                |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "RenderAnnotationComments");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "RenderAnnotationComments" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Unload                                                                                     |
        /// | Descrição: Limpa o cache do documento no servidor quando o visualizador é fechado.               |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Unload");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Unload" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ExportAnnotations                                                                          |
        /// | Descrição: Exporta as anotações do PDF para o formato XFDF ou JSON.                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportAnnotations");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportAnnotations" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ImportAnnotations                                                                          |
        /// | Descrição: Importa anotações de um arquivo externo para o PDF atual.                             |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportAnnotations");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportAnnotations" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ExportFormFields                                                                           |
        /// | Descrição: Exporta os dados preenchidos nos campos de formulário do PDF.                          |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ExportFormFields");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ExportFormFields" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: ImportFormFields                                                                           |
        /// | Descrição: Importa dados para os campos de formulário do PDF atual.                              |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "ImportFormFields");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "ImportFormFields" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: Download                                                                                   |
        /// | Descrição: Disponibiliza o documento PDF em formato Base64 para download pelo cliente.           |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "Download");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "Download" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }

        /// <summary>
        /// ___________________________________________________________________________________________________
        /// |                                     FROTIX - SOLUÇÃO EM GESTÃO                                   |
        /// |__________________________________________________________________________________________________|
        /// | Nome: PrintImages                                                                                |
        /// | Descrição: Gera imagens otimizadas das páginas para o processo de impressão.                     |
        /// |__________________________________________________________________________________________________|
        /// </summary>
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
                _logService.Error(error.Message , error , "MultaPdfViewerController.cs" , "PrintImages");
                Alerta.TratamentoErroComLinha("MultaPdfViewerController.cs" , "PrintImages" , error);
                return StatusCode(500 , new
                {
                    error = error.Message
                });
            }
        }
    }
}
