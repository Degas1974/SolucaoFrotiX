/*
 * ╔══════════════════════════════════════════════════════════════════════════════════════════════════════════╗
 * ║ FROTIX - SISTEMA DE GESTÃO DE FROTAS                                                                     ║
 * ║ Arquivo: TestePdfController.cs (Controllers)                                                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ DESCRIÇÃO                                                                                                 ║
 * ║ API Controller de TESTE para verificação de conectividade. Endpoint simples para healthcheck.           ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ROTA BASE: api/TestePdf                                                                                  ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ ENDPOINTS                                                                                                 ║
 * ║ • [GET] /Ping : Retorna { success: true, message: "TestePdf funcionando!" }                             ║
 * ╠══════════════════════════════════════════════════════════════════════════════════════════════════════════╣
 * ║ Documentação: 28/01/2026 | LOTE: 19                                                                      ║
 * ╚══════════════════════════════════════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc;

namespace FrotiX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestePdfController : Controller
    {
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { success = true , message = "TestePdf funcionando!" });
        }
    }
}
