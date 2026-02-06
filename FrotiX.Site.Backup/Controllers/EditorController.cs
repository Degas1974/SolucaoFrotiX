using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers
{
    /* > ---------------------------------------------------------------------------------------
     > 📄 **CARD DE IDENTIDADE DO ARQUIVO**
     > ---------------------------------------------------------------------------------------
     > 🆔 **Nome:** EditorController.cs
     > 📍 **Local:** Controllers
     > ❓ **Por que existo?** Controlador auxiliar para operações do editor de texto (SFDT).
     >                      Gerencia upload de imagens em documentos DOCX.
     > 🔗 **Relevância:** Baixa (Utilitário de Edição)
     > --------------------------------------------------------------------------------------- */

    [Route("Editor")]
    public class EditorController : Controller
    {
        private readonly ILogService _logService;

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: EditorController (Constructor)                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador auxiliar do editor com serviço de log.          ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite rastreabilidade em operações de upload de imagens.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • logService (ILogService): serviço de log centralizado.                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • N/A                                                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Views/Editor/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        public EditorController(ILogService logService)
        {
            try
            {
                _logService = logService;
            }
            catch (Exception ex)
            {
                // Fallback para alerta de console, já que logService falhou na injeção
                Console.WriteLine($"Erro crítico no construtor do EditorController: {ex.Message}");
            }
        }

        /// <summary>
        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: DownloadImagemDocx                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Extrai e salva imagem de um arquivo DOCX para uso no editor.              ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita recursos visuais no editor SFDT.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • docx (IFormFile): arquivo DOCX enviado via formulário.                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: status da operação (Ok/500).                             ║
        /// ║    • Consumidor: UI do editor (SFDT).                                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • SfdtHelper.SalvarImagemDeDocx() → extração da imagem.                    ║
        /// ║    • _logService.Error() / Alerta.TratamentoErroComLinha() → erros.           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /Editor/DownloadImagemDocx                                         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Editor                                                  ║
        /// ║    • Arquivos relacionados: Views/Editor/*.cshtml                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        /// </summary>
        [HttpPost("DownloadImagemDocx")]
        public IActionResult DownloadImagemDocx(IFormFile docx)
        {
            try
            {
                // [DADOS] Leitura do arquivo DOCX
                using var stream = docx.OpenReadStream();
                using var memory = new MemoryStream();
                stream.CopyTo(memory);
                var bytes = memory.ToArray();

                // [LOGICA] Extração e persistência da imagem
                var imagem = SfdtHelper.SalvarImagemDeDocx(bytes);
                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png", imagem);
                return Ok();
            }
            catch (Exception error)
            {
                _logService?.Error(error.Message, error, "EditorController.cs", "DownloadImagemDocx");
                Alerta.TratamentoErroComLinha("EditorController.cs", "DownloadImagemDocx", error);
                return StatusCode(500);
            }
        }
    }
}
