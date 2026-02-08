using FrotiX.Helpers;

/*
 *  _________________________________________________________________________________________________________
 * |                                                                                                         |
 * |                                   FROTIX - SOLUÇÃO GESTÃO DE FROTAS                                     |
 * |_________________________________________________________________________________________________________|
 * |                                                                                                         |
 * | (IA) CAMADA: CONTROLLERS (API)                                                                          |
 * | (IA) IDENTIDADE: EditorController.cs                                                                    |
 * | (IA) DESCRIÇÃO: API para operações de editor Syncfusion (conversão DOCX para PNG).                      |
 * | (IA) PADRÃO: FrotiX 2026 Core (ASCII Hero Banner + XML Documentation)                                   |
 * |_________________________________________________________________________________________________________|
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: EditorController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Gerenciar operações relacionadas ao editor de texto do FrotiX,
     *                   incluindo conversão de documentos DOCX para imagens PNG
     * 📥 ENTRADAS     : Arquivos DOCX via upload (IFormFile)
     * 📤 SAÍDAS       : Imagens PNG salvas em disco, respostas HTTP
     * 🔗 CHAMADA POR  : Frontend (páginas que utilizam editor Syncfusion)
     * 🔄 CHAMA        : SfdtHelper.SalvarImagemDeDocx()
     * 📦 DEPENDÊNCIAS : ASP.NET Core MVC, Syncfusion DocIO, SfdtHelper
     ****************************************************************************************/
    [Route("Editor")]
    public class EditorController :Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: DownloadImagemDocx
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Converte arquivo DOCX em imagem PNG para visualização/preview
         *                   Recebe um documento Word e gera uma imagem da primeira página
         * 📥 ENTRADAS     : [IFormFile] docx - Arquivo DOCX enviado via upload
         * 📤 SAÍDAS       : [IActionResult] Ok() se sucesso, StatusCode(500) se erro
         * 🔗 CHAMADA POR  : JavaScript do editor (frontend) via AJAX POST
         * 🔄 CHAMA        : SfdtHelper.SalvarImagemDeDocx() - Converte DOCX para PNG
         *                   File.WriteAllBytes() - Salva imagem no disco
         * 📦 DEPENDÊNCIAS : SfdtHelper, System.IO
         *
         * 📝 FLUXO:
         *    1. Recebe arquivo DOCX via POST
         *    2. Converte stream para array de bytes
         *    3. Usa SfdtHelper para converter DOCX → PDF → PNG
         *    4. Salva imagem em wwwroot/uploads/Editor.png
         *    5. Retorna Ok() confirmando operação
         *
         * ⚠️  OBSERVAÇÕES:
         *    - Sempre sobrescreve o arquivo Editor.png (não gera nome único)
         *    - Converte apenas a primeira página do documento
         *    - Usa Syncfusion DocIO para conversão
         ****************************************************************************************/
        [HttpPost("DownloadImagemDocx")]
        public IActionResult DownloadImagemDocx(IFormFile docx)
        {
            try
            {
                // [DOC] Abre stream do arquivo DOCX recebido do upload
                using var stream = docx.OpenReadStream();
                using var memory = new MemoryStream();
                stream.CopyTo(memory);
                var bytes = memory.ToArray(); // [DOC] Converte stream para array de bytes

                // [DOC] Converte DOCX para PNG usando helper Syncfusion (DOCX → PDF → PNG)
                var imagem = SfdtHelper.SalvarImagemDeDocx(bytes);

                // [DOC] Salva imagem no disco (sempre sobrescreve Editor.png)
                System.IO.File.WriteAllBytes("wwwroot/uploads/Editor.png" , imagem);

                return Ok();
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("EditorController.cs" , "DownloadImagemDocx" , error);
                return StatusCode(500);
            }
        }
    }
}
