/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : NormalizeController.cs                                          ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ Controller API para normalizacao de texto (acentos, caracteres especiais).   ║
║ Usa NormalizationService para padronizar strings.                            ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ ENDPOINTS                                                                    ║
║ - POST /api/Normalize/Text : Normaliza texto recebido no body                 ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.TextNormalization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FrotiX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NormalizeController :ControllerBase
    {
        private readonly NormalizationService _normalizer;

        /****************************************************************************************
         * ⚡ FUNÇÃO: NormalizeController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar dependência do serviço de normalização de texto
         * 📥 ENTRADAS     : [NormalizationService] normalizer - Serviço de normalização
         * 📤 SAÍDAS       : Instância inicializada do NormalizeController
         * 🔗 CHAMADA POR  : ASP.NET Core Dependency Injection
         * 🔄 CHAMA        : Alerta.TratamentoErroComLinha (em caso de erro)
         * 📦 DEPENDÊNCIAS : NormalizationService (serviço customizado)
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

        public record NormalizeRequest(string Text);

        /****************************************************************************************
         * ⚡ FUNÇÃO: Post
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Normalizar texto (remover acentos, caracteres especiais, padronizar)
         * 📥 ENTRADAS     : [NormalizeRequest] body - Objeto com propriedade Text
         * 📤 SAÍDAS       : [ActionResult<string>] Texto normalizado
         * 🔗 CHAMADA POR  : APIs/páginas que precisam normalizar texto
         * 🔄 CHAMA        : _normalizer.NormalizeAsync
         * 📦 DEPENDÊNCIAS : NormalizationService
         *
         * [DOC] Valida se texto não é nulo/vazio antes de normalizar
         * [DOC] Retorna BadRequest(400) se texto inválido, Ok(200) se sucesso
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
