/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO DISPONÍVEL                                              ║
 * ║  📄 DocumentacaoIntraCodigo/DocumentacaoIntracodigo.md                  ║
 * ║  Seção: TestePdfController.cs                                            ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Mvc;

namespace FrotiX.Controllers
{
    /****************************************************************************************
     * ⚡ CONTROLLER: TestePdf API
     * 🎯 OBJETIVO: Endpoint de teste/health check para verificar funcionamento da API
     * 📋 ROTAS: /api/TestePdf/Ping
     * 🔗 ENTIDADES: Nenhuma
     * 📦 DEPENDÊNCIAS: Nenhuma (controller minimalista)
     ****************************************************************************************/
    [ApiController]
    [Route("api/[controller]")]
    public class TestePdfController : Controller
    {
        /****************************************************************************************
         * ⚡ FUNÇÃO: Ping
         * 🎯 OBJETIVO: Retornar resposta simples para verificar se a API está ativa
         * 📥 ENTRADAS: Nenhuma
         * 📤 SAÍDAS: JSON { success: true, message: "TestePdf funcionando!" }
         * 🔗 CHAMADA POR: Health checks, testes de conectividade, monitoramento
         * 🔄 CHAMA: Nenhum
         ****************************************************************************************/
        [HttpGet("Ping")]
        public IActionResult Ping()
        {
            return Ok(new { success = true , message = "TestePdf funcionando!" });
        }
    }
}
