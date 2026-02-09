/* ****************************************************************************************
 * ⚡ ARQUIVO: NormalizeController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor API para normalização de textos (acentos e caracteres especiais).
 *
 * 📥 ENTRADAS     : Texto recebido via body.
 *
 * 📤 SAÍDAS       : Texto normalizado.
 *
 * 🔗 CHAMADA POR  : APIs e telas que precisam padronizar textos.
 *
 * 🔄 CHAMA        : NormalizationService.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.TextNormalization.
 **************************************************************************************** */

using FrotiX.TextNormalization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: NormalizeController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Normalizar textos recebidos via API.
     *
     * 📥 ENTRADAS     : Payload com texto.
     *
     * 📤 SAÍDAS       : Texto normalizado em JSON.
     *
     * 🔗 CHAMADA POR  : Integrações e formulários do sistema.
     *
     * 🔄 CHAMA        : NormalizationService.NormalizeAsync().
     *
     * 📦 DEPENDÊNCIAS : FrotiX.TextNormalization.
     ****************************************************************************************/
    [ApiController]
    [Route("api/[controller]")]
    public class NormalizeController :ControllerBase
    {
        private readonly NormalizationService _normalizer;

        /****************************************************************************************
         * ⚡ FUNÇÃO: NormalizeController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar serviço de normalização de texto.
         *
         * 📥 ENTRADAS     : [NormalizationService] normalizer.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
         ****************************************************************************************/
        public NormalizeController(NormalizationService normalizer)
        {
            try
            {
                _normalizer = normalizer;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NormalizeController.cs" , "NormalizeController" , error);
            }
        }

        /****************************************************************************************
         * ⚡ CLASSE: NormalizeRequest
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Representar o payload de normalização.
         *
         * 📥 ENTRADAS     : Text.
         *
         * 📤 SAÍDAS       : Objeto utilizado no endpoint Post.
         ****************************************************************************************/
        public record NormalizeRequest(string Text);

        /****************************************************************************************
         * ⚡ FUNÇÃO: Post
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Normalizar texto removendo acentos e caracteres especiais.
         *
         * 📥 ENTRADAS     : [NormalizeRequest] body.
         *
         * 📤 SAÍDAS       : [ActionResult<string>] texto normalizado.
         *
         * 🔗 CHAMADA POR  : APIs/páginas que precisam normalizar texto.
         *
         * 🔄 CHAMA        : _normalizer.NormalizeAsync().
         *
         * 📝 OBSERVAÇÕES  : Retorna BadRequest se o texto for nulo/vazio.
         ****************************************************************************************/
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] NormalizeRequest body)
        {
            try
            {
                // [DOC] Validação: texto é obrigatório
                if (body is null || string.IsNullOrWhiteSpace(body.Text))
                    return BadRequest("Text is required.");

                // [DOC] Chama serviço de normalização assíncrono
                var result = await _normalizer.NormalizeAsync(body.Text);
                return Ok(result);
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("NormalizeController.cs" , "Post" , error);
                return StatusCode(500 , "Erro ao normalizar texto");
            }
        }
    }
}
