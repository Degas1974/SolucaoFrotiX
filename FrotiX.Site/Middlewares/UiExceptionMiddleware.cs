/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: UiExceptionMiddleware.cs                                                               ║
   ║ 📂 CAMINHO: /Middlewares                                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Middleware para tratamento de exceções com resposta diferenciada:                               ║
   ║    JSON para requisições AJAX/API, redirect /Erro para requisições HTML.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [Invoke] : Intercepta e trata exceções.......... (HttpContext) -> Task                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header da Função.    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Middlewares
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ CLASSE: UiExceptionMiddleware                                                      │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Middleware para tratamento de exceções com resposta diferenciada por tipo de       │
    // │    requisição. Verifica Accept header e X-Requested-With para decidir formato.        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Program.cs (pipeline de middlewares)                              │
    // │    ➡️ CHAMA       : /Erro (redirect HTML) ou JSON response (AJAX)                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public sealed class UiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UiExceptionMiddleware(RequestDelegate next)
        {
            ArgumentNullException.ThrowIfNull(next);
            _next = next;
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Invoke                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO: Intercepta exceções e decide formato de resposta (JSON ou HTML).       │
        // │    - Se Accept contém application/json OU X-Requested-With é XMLHttpRequest:         │
        // │      Retorna JSON com detalhes do erro.                                              │
        // │    - Caso contrário: Redireciona para /Erro (que lerá TempData["UiError"]).          │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS: • http [HttpContext]: Contexto da requisição                              │
        // │ 📤 OUTPUTS: • [Task] - JSON response ou redirect                                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        public async Task Invoke(HttpContext http)
        {
            try
            {
                // [LOGICA] Executar próximo middleware no pipeline
                await _next(http);
            }
            catch (Exception ex)
            {
                // [DADOS] Usa payload já registrado (se houver) ou monta um básico
                var payload =
                    http.Items.TryGetValue("UiError", out var v) && v is not null
                        ? v
                        : new
                        {
                            Origem = "Middleware",
                            Mensagem = ex.Message,
                            Stack = ex.StackTrace,
                        };

                // [LOGICA] Decide se a resposta deve ser JSON (AJAX/API) ou HTML (Razor/View)
                bool wantsJson = false;

                // [DADOS] Verificar Accept header
                var accept = http.Request.Headers["Accept"];
                if (accept.Count > 0)
                    wantsJson = accept.Any(h =>
                        h?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true
                    );

                // [DADOS] Verificar X-Requested-With para AJAX
                if (!wantsJson)
                {
                    var xrw = http.Request.Headers["X-Requested-With"];
                    if (xrw.Count > 0)
                        wantsJson = xrw.Any(h =>
                            string.Equals(h, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase)
                        );
                }

                // [UI] Definir status code de erro
                http.Response.StatusCode = StatusCodes.Status500InternalServerError;

                if (wantsJson)
                {
                    // [AJAX] Retornar resposta JSON para APIs
                    http.Response.ContentType = MediaTypeNames.Application.Json;

                    var problemJson = JsonSerializer.Serialize(
                        new
                        {
                            title = "Erro no servidor",
                            status = 500,
                            detail = payload,
                        }
                    );

                    await http.Response.WriteAsync(problemJson);
                    return;
                }
                else
                {
                    // [UI] HTML: redireciona para página de erro (que lerá TempData["UiError"])
                    http.Response.Redirect("/Erro");
                    return;
                }
            }
        }
    }
}
