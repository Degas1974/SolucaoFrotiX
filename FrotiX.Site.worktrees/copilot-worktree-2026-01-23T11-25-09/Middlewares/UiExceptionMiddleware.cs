using System;
using System.Linq; // ← Para .Any()
using System.Net.Mime; // ← Para MediaTypeNames
using System.Text.Json;
using System.Threading.Tasks; // ← Para Task
using Microsoft.AspNetCore.Http;

namespace FrotiX.Middlewares
{
    /// <summary>
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║                                                                              ║
    /// ║  🎨 ARQUIVO: UiExceptionMiddleware.cs (Middleware de Tratamento UI)         ║
    /// ║                                                                              ║
    /// ║  DESCRIÇÃO:                                                                  ║
    /// ║  Middleware para tratamento inteligente de exceções na UI.                  ║
    /// ║  Decide resposta com base no tipo de requisição: JSON (AJAX) ou HTML (View).║
    /// ║                                                                              ║
    /// ║  FUNCIONALIDADES:                                                            ║
    /// ║  1. Captura exceções não tratadas no pipeline HTTP.                         ║
    /// ║  2. Detecta tipo de requisição (AJAX vs. Razor).                            ║
    /// ║  3. Responde JSON para AJAX (Accept: application/json).                     ║
    /// ║  4. Redireciona para /Erro para requisições HTML (Razor Views).             ║
    /// ║  5. Usa payload pré-registrado em HttpContext.Items["UiError"].             ║
    /// ║                                                                              ║
    /// ║  DETECÇÃO DE AJAX:                                                           ║
    /// ║  - Header "Accept" contém "application/json" → Resposta JSON.               ║
    /// ║  - Header "X-Requested-With" == "XMLHttpRequest" → Resposta JSON.           ║
    /// ║  - Caso contrário → Redireciona para página de erro (/Erro).                ║
    /// ║                                                                              ║
    /// ║  PAYLOAD CUSTOMIZADO:                                                        ║
    /// ║  - Se HttpContext.Items["UiError"] existe → Usa payload customizado.        ║
    /// ║  - Senão → Cria payload básico (Origem, Mensagem, Stack).                   ║
    /// ║                                                                              ║
    /// ║  RESPOSTA JSON (AJAX):                                                       ║
    /// ║  {                                                                           ║
    /// ║    "title": "Erro no servidor",                                             ║
    /// ║    "status": 500,                                                            ║
    /// ║    "detail": { /* payload */ }                                              ║
    /// ║  }                                                                           ║
    /// ║                                                                              ║
    /// ║  RESPOSTA HTML (RAZOR):                                                      ║
    /// ║  - Redireciona para /Erro (ErrorController ou Razor Page).                  ║
    /// ║  - Página de erro lê TempData["UiError"] para exibir detalhes.              ║
    /// ║                                                                              ║
    /// ║  REGISTRO NO PIPELINE (Program.cs):                                          ║
    /// ║  app.UseMiddleware<UiExceptionMiddleware>(); // Após UseRouting()           ║
    /// ║                                                                              ║
    /// ║  ÚLTIMA ATUALIZAÇÃO: 19/01/2026                                              ║
    /// ║                                                                              ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    /// </summary>

    /// <summary>
    /// ╭──────────────────────────────────────────────────────────────────────────────
    /// │ CLASSE: UiExceptionMiddleware (Middleware Inteligente de UI)
    /// │──────────────────────────────────────────────────────────────────────────────
    /// │ DESCRIÇÃO:
    /// │    Middleware selado que captura exceções e decide resposta baseada no tipo
    /// │    de requisição (AJAX/JSON vs. Razor/HTML).
    /// │
    /// │ PROPRIEDADES:
    /// │    - _next: Próximo middleware no pipeline (RequestDelegate).
    /// │
    /// │ MÉTODO PRINCIPAL:
    /// │    - Invoke(): Chamado pelo ASP.NET Core para cada requisição HTTP.
    /// │
    /// │ FLUXO DE EXECUÇÃO:
    /// │    1. Try: Chama próximo middleware (_next).
    /// │    2. Catch: Captura exceções não tratadas.
    /// │       → Tenta obter payload customizado de HttpContext.Items["UiError"].
    /// │       → Detecta se requisição é AJAX (Accept: application/json ou X-Requested-With).
    /// │       → Se AJAX: Responde JSON (Status 500 + payload).
    /// │       → Se HTML: Redireciona para /Erro (ErrorController ou Razor Page).
    /// │
    /// │ DETECÇÃO DE AJAX (2 MÉTODOS):
    /// │    1. Header "Accept" contém "application/json" (case-insensitive).
    /// │    2. Header "X-Requested-With" == "XMLHttpRequest" (jQuery/AJAX padrão).
    /// │
    /// │ PAYLOAD CUSTOMIZADO:
    /// │    - Controllers podem definir HttpContext.Items["UiError"] antes de lançar exceção.
    /// │    - Permite mensagens de erro personalizadas para usuário final.
    /// │
    /// │ CLASSE SELADA (sealed):
    /// │    - Não pode ser herdada (performance + segurança).
    /// │    - JIT pode aplicar otimizações (devirtualização).
    /// │──────────────────────────────────────────────────────────────────────────────
    /// </summary>
    public sealed class UiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Construtor que recebe próximo middleware e valida null (C# 11 ThrowIfNull).
        /// </summary>
        public UiExceptionMiddleware(RequestDelegate next)
        {
            // [VALIDAÇÃO] - Lança ArgumentNullException se next == null (C# 11 helper)
            ArgumentNullException.ThrowIfNull(next);
            _next = next;
        }

        /// <summary>
        /// ╭──────────────────────────────────────────────────────────────────────
        /// │ MÉTODO: Invoke (Método Principal do Middleware)
        /// │──────────────────────────────────────────────────────────────────────
        /// │ DESCRIÇÃO:
        /// │    Método chamado pelo ASP.NET Core para CADA requisição HTTP.
        /// │    Intercepta exceções e decide resposta (JSON vs. HTML).
        /// │
        /// │ FLUXO:
        /// │    1. Try: Chama próximo middleware (_next).
        /// │    2. Catch: Captura exceções não tratadas.
        /// │       → [ETAPA 1] Tenta obter payload customizado (HttpContext.Items["UiError"]).
        /// │       → [ETAPA 2] Detecta se requisição é AJAX (wantsJson).
        /// │       → [ETAPA 3] Define StatusCode 500.
        /// │       → [ETAPA 4] Se AJAX: Responde JSON com payload.
        /// │       → [ETAPA 5] Se HTML: Redireciona para /Erro.
        /// │
        /// │ DETECÇÃO DE AJAX:
        /// │    - Verifica header "Accept" contém "application/json" (case-insensitive).
        /// │    - Se não, verifica header "X-Requested-With" == "XMLHttpRequest".
        /// │
        /// │ RESPOSTA JSON (AJAX):
        /// │    {
        /// │      "title": "Erro no servidor",
        /// │      "status": 500,
        /// │      "detail": { /* payload customizado ou básico */ }
        /// │    }
        /// │
        /// │ RESPOSTA HTML (RAZOR):
        /// │    - Response.Redirect("/Erro") → ErrorController.Index() ou Error.cshtml.
        /// │    - Página de erro deve ler TempData["UiError"] para exibir detalhes.
        /// │
        /// │ PAYLOAD BÁSICO (FALLBACK):
        /// │    - Origem: "Middleware" (indica que erro não foi customizado).
        /// │    - Mensagem: ex.Message (mensagem da exceção).
        /// │    - Stack: ex.StackTrace (stack trace completo para debugging).
        /// │──────────────────────────────────────────────────────────────────────
        /// </summary>
        public async Task Invoke(HttpContext http)
        {
            try
            {
                // [ETAPA 1] - Chama próximo middleware no pipeline (await assíncrono)
                await _next(http);
            }
            catch (Exception ex)
            {
                // [ETAPA 2] - Captura exceções não tratadas (try-catch global)

                // [ETAPA 2.1] - Tenta obter payload customizado de HttpContext.Items["UiError"]
                // Se não existir, cria payload básico com Origem/Mensagem/Stack
                var payload =
                    http.Items.TryGetValue("UiError", out var v) && v is not null
                        ? v // Payload customizado (definido por Controller)
                        : new // Payload básico (fallback)
                        {
                            Origem = "Middleware",
                            Mensagem = ex.Message,
                            Stack = ex.StackTrace,
                        };

                // [ETAPA 3] - Detecta se requisição é AJAX/JSON (wantsJson = true)
                bool wantsJson = false;

                // [ETAPA 3.1] - Verifica header "Accept" contém "application/json"
                var accept = http.Request.Headers["Accept"];
                if (accept.Count > 0)
                    wantsJson = accept.Any(h =>
                        h?.Contains("application/json", StringComparison.OrdinalIgnoreCase) == true
                    );

                // [ETAPA 3.2] - Se não detectou JSON no Accept, verifica "X-Requested-With"
                if (!wantsJson)
                {
                    var xrw = http.Request.Headers["X-Requested-With"];
                    if (xrw.Count > 0)
                        wantsJson = xrw.Any(h =>
                            string.Equals(h, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase)
                        );
                }

                // [ETAPA 4] - Define StatusCode 500 (Internal Server Error)
                http.Response.StatusCode = StatusCodes.Status500InternalServerError;

                // [ETAPA 5] - Responde JSON (AJAX) ou Redireciona (HTML)
                if (wantsJson)
                {
                    // [RESPOSTA JSON] - Para requisições AJAX/API
                    http.Response.ContentType = MediaTypeNames.Application.Json;

                    var problemJson = JsonSerializer.Serialize(
                        new
                        {
                            title = "Erro no servidor",
                            status = 500,
                            detail = payload, // Payload customizado ou básico
                        }
                    );

                    await http.Response.WriteAsync(problemJson);
                    return; // Garante que todos os caminhos retornam (evita CS0161)
                }
                else
                {
                    // [RESPOSTA HTML] - Para requisições Razor/View
                    // Redireciona para /Erro (ErrorController ou Error.cshtml)
                    // Página de erro deve ler TempData["UiError"] para exibir detalhes
                    http.Response.Redirect("/Erro");
                    return; // Garante que todos os caminhos retornam (evita CS0161)
                }
            }
        }
    }
}
