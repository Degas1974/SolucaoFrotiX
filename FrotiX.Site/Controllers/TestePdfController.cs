/* ****************************************************************************************
 * ⚡ ARQUIVO: TestePdfController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Fornecer healthcheck simples para testar conectividade da API.
 *
 * 📥 ENTRADAS     : Nenhuma.
 *
 * 📤 SAÍDAS       : JSON com sucesso e mensagem.
 *
 * 🔗 CHAMADA POR  : Verificações rápidas de ambiente.
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: TestePdfController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Expor endpoint de ping/healthcheck.
     *
     * 📥 ENTRADAS     : Nenhuma.
     *
     * 📤 SAÍDAS       : JSON com status.
     ****************************************************************************************/
    [ApiController]
    [Route("api/[controller]")]
    public class TestePdfController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: Ping
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Responder confirmação de funcionamento da API.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Ok({ success: true, message }).
         *
         * 🔗 CHAMADA POR  : GET /api/TestePdf/Ping.
         ****************************************************************************************/
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { success = true , message = "TestePdf funcionando!" });
        }
    }
}
