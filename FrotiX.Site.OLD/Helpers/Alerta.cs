/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Alerta.cs                                                                             ║
   ║ 📂 CAMINHO: Helpers/                                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Classe estática de alertas SweetAlert no servidor (origem SERVER). Exibe                        ║
   ║    Erro/Sucesso/Info/Warning/Confirmar via TempData e registra erros com linha.                    ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • Erro(string titulo, string texto, string confirmButtonText = "OK")                            ║
   ║    • Sucesso(string titulo, string texto, string confirmButtonText = "OK")                         ║
   ║    • Info(string titulo, string texto, string confirmButtonText = "OK")                            ║
   ║    • Warning(string titulo, string texto, string confirmButtonText = "OK")                         ║
   ║    • Confirmar(string titulo, string texto, string confirmButtonText = "Sim", string cancelButtonText = "Cancelar") ║
   ║    • TratamentoErroComLinha(string arquivo, string funcao, Exception error, ILogger logger = null) ║
   ║    • TratamentoErroComLinha(Exception error, string arquivo, string funcao, ILogger logger = null) ║
   ║    • GetIconePrioridade(PrioridadeAlerta prioridade)                                               ║
   ║    • GetCorPrioridade(PrioridadeAlerta prioridade)                                                 ║
   ║    • GetCorHexPrioridade(PrioridadeAlerta prioridade)                                              ║
   ║    • GetNomePrioridade(PrioridadeAlerta prioridade)                                                ║
   ║    • TempDataSet(string key, object value)                                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: ILogService, ITempDataDictionaryFactory, IHttpContextAccessor, ILoggerFactory      ║
   ║ 📅 ATUALIZAÇÃO: 30/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                     ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using FrotiX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace FrotiX.Helpers
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Alerta                                                                            │
    // │ 📦 TIPO: Estática                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Centralizar alertas SweetAlert no backend e registrar erros com arquivo/linha (SERVER).
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers, Pages, Services e Helpers internos
    // ➡️ CHAMA       : ILogService.Error(), ILogger.LogError(), TempData
    
    
    public static class Alerta
    {
        // --- Bridges para DI (preenchidos no Startup/Program) -----------------
        // Acesso ao HttpContext atual via IHttpContextAccessor.
        public static IHttpContextAccessor HttpCtx
        {
            get; set;
        }
        // Factory para acesso ao TempData de requisições.
        public static ITempDataDictionaryFactory TempFactory
        {
            get; set;
        }
        // Factory para criação de ILogger em fallback de log.
        public static ILoggerFactory LoggerFactory
        {
            get; set;
        }

        
        // Service Provider para obter ILogService via Service Locator pattern.
        // Preenchido no Startup/Program.
        
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }

        #region Métodos de Alerta Visual

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Erro                                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : SetAlert()                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Exibir alerta de erro via SweetAlert no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // texto - Mensagem do alerta
        // confirmButtonText - Texto do botão de confirmação
        
        
        // Param titulo: Título do alerta.
        // Param texto: Mensagem do alerta.
        // Param confirmButtonText: Texto do botão de confirmação.
        public static void Erro(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("error" , titulo , texto , confirmButtonText);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Sucesso                                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : SetAlert()                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Exibir alerta de sucesso via SweetAlert no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // texto - Mensagem do alerta
        // confirmButtonText - Texto do botão de confirmação
        
        
        // Param titulo: Título do alerta.
        // Param texto: Mensagem do alerta.
        // Param confirmButtonText: Texto do botão de confirmação.
        public static void Sucesso(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("success" , titulo , texto , confirmButtonText);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Info                                                                       │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : SetAlert()                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Exibir alerta informativo via SweetAlert no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // texto - Mensagem do alerta
        // confirmButtonText - Texto do botão de confirmação
        
        
        // Param titulo: Título do alerta.
        // Param texto: Mensagem do alerta.
        // Param confirmButtonText: Texto do botão de confirmação.
        public static void Info(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("info" , titulo , texto , confirmButtonText);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Warning                                                                    │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : SetAlert()                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Exibir alerta de aviso via SweetAlert no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // texto - Mensagem do alerta
        // confirmButtonText - Texto do botão de confirmação
        
        
        // Param titulo: Título do alerta.
        // Param texto: Mensagem do alerta.
        // Param confirmButtonText: Texto do botão de confirmação.
        public static void Warning(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("warning" , titulo , texto , confirmButtonText);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: Confirmar                                                                 │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : SetAlert()                                                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Exibir alerta de confirmação via SweetAlert no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // titulo - Título do alerta
        // texto - Mensagem do alerta
        // confirmButtonText - Texto do botão de confirmação
        // cancelButtonText - Texto do botão de cancelamento
        
        
        // Param titulo: Título do alerta.
        // Param texto: Mensagem do alerta.
        // Param confirmButtonText: Texto do botão de confirmação.
        // Param cancelButtonText: Texto do botão de cancelamento.
        public static void Confirmar(
            string titulo ,
            string texto ,
            string confirmButtonText = "Sim" ,
            string cancelButtonText = "Cancelar"
        )
        {
            SetAlert("confirm" , titulo , texto , confirmButtonText , cancelButtonText);
        }

        #endregion

        #region Tratamento de Erro com Linha

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TratamentoErroComLinha                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        // │    ➡️ CHAMA       : TentarObterLinha(), ILogService.Error(), ILogger.LogError(), SetErrorUnexpectedAlert() │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Registrar erro com arquivo/linha e exibir alerta técnico (origem SERVER).
        
        
        
        // 📥 PARÂMETROS:
        // arquivo - Caminho ou nome do arquivo de origem
        // funcao - Nome da função/método de origem
        // error - Exceção capturada
        // logger - Logger opcional para fallback
        
        
        // Param arquivo: Caminho ou nome do arquivo de origem.
        // Param funcao: Nome da função/método de origem.
        // Param error: Exceção capturada.
        // Param logger: Logger opcional para fallback.
        public static void TratamentoErroComLinha(
            string arquivo ,
            string funcao ,
            Exception error ,
            ILogger logger = null
        )
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            // Extrair informações de linha do stack trace
            var info = TentarObterLinha(error);
            string fileName = !string.IsNullOrWhiteSpace(arquivo)
                ? Path.GetFileName(arquivo)
                : (info.file != null ? Path.GetFileName(info.file) : "arquivo desconhecido");

            string member = !string.IsNullOrWhiteSpace(funcao)
                ? funcao
                : (info.member ?? "função desconhecida");

            int? lineNumber = info.line;
            string msg = $"{fileName}::{member}: {error.GetType().Name} - {error.Message}";

            // ===== TENTATIVA 1: Service Locator para ILogService (gravar no banco/arquivo unificado) =====
            bool loggedViaLogService = false;
            try
            {
                // Tentar obter ILogService via ServiceProvider estático
                var logService = ServiceProvider?.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                
                // Se não conseguiu via ServiceProvider, tentar via HttpContext.RequestServices
                if (logService == null && HttpCtx?.HttpContext?.RequestServices != null)
                {
                    logService = HttpCtx.HttpContext.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                }

                if (logService != null)
                {
                    // Gravar via ILogService (origem: SERVER)
                    logService.Error(
                        message: $"[SERVER] {error.Message}",
                        exception: error,
                        arquivo: fileName,
                        metodo: member,
                        linha: lineNumber
                    );
                    loggedViaLogService = true;
                }
            }
            catch
            {
                // Silencioso - continua para fallback
            }

            // ===== FALLBACK: ILogger ou Debug.WriteLine =====
            if (!loggedViaLogService)
            {
                string linhaText = lineNumber.HasValue ? $" (linha {lineNumber.Value})" : string.Empty;
                string fullMsg = $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";

                var useLogger = logger ?? LoggerFactory?.CreateLogger("Alerta");
                if (useLogger != null)
                {
                    useLogger.LogError(error, fullMsg);
                }
                else
                {
                    // Último recurso: Console/Debug
                    Debug.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
                    Debug.WriteLine($"[ALERTA-FALLBACK] Stack: {error.StackTrace}");
                    Console.Error.WriteLine($"[ALERTA-FALLBACK] {fullMsg}");
                }
            }

            // Alerta visual usando ShowErrorUnexpected
            SetErrorUnexpectedAlert(fileName , member , error);
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TratamentoErroComLinha (overload)                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Código legado                                                       │
        // │    ➡️ CHAMA       : TratamentoErroComLinha(arquivo, funcao, error, logger)              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Manter compatibilidade com a assinatura antiga (Exception primeiro).
        
        
        
        // 📥 PARÂMETROS:
        // error - Exceção capturada
        // arquivo - Caminho ou nome do arquivo de origem
        // funcao - Nome da função/método de origem
        // logger - Logger opcional para fallback
        
        
        // Param error: Exceção capturada.
        // Param arquivo: Caminho ou nome do arquivo de origem.
        // Param funcao: Nome da função/método de origem.
        // Param logger: Logger opcional para fallback.
        public static void TratamentoErroComLinha(
            Exception error ,
            string arquivo ,
            string funcao ,
            ILogger logger = null
        ) => TratamentoErroComLinha(arquivo , funcao , error , logger);

        #endregion

        #region Métodos de Prioridade de Alertas

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetIconePrioridade                                                      │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        // │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter o ícone FontAwesome Duotone baseado na prioridade do alerta.
        
        
        
        // 📥 PARÂMETROS:
        // prioridade - Prioridade do alerta.
        
        
        
        // 📤 RETORNO:
        // string - Classe CSS do ícone FontAwesome.
        
        
        // Param prioridade: Prioridade do alerta.
        // Returns: Classe CSS do ícone FontAwesome.
        public static string GetIconePrioridade(PrioridadeAlerta prioridade)
        {
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => "fa-duotone fa-circle-info",
                PrioridadeAlerta.Media => "fa-duotone fa-circle-exclamation",
                PrioridadeAlerta.Alta => "fa-duotone fa-triangle-exclamation",
                _ => "fa-duotone fa-circle"
            };
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCorPrioridade                                                         │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        // │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter a classe CSS de cor baseada na prioridade do alerta.
        
        
        
        // 📥 PARÂMETROS:
        // prioridade - Prioridade do alerta.
        
        
        
        // 📤 RETORNO:
        // string - Classe CSS para cor do alerta.
        
        
        // Param prioridade: Prioridade do alerta.
        // Returns: Classe CSS para cor do alerta.
        public static string GetCorPrioridade(PrioridadeAlerta prioridade)
        {
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => "text-info",
                PrioridadeAlerta.Media => "text-warning",
                PrioridadeAlerta.Alta => "text-danger",
                _ => "text-secondary"
            };
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCorHexPrioridade                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        // │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter a cor hexadecimal baseada na prioridade do alerta.
        
        
        
        // 📥 PARÂMETROS:
        // prioridade - Prioridade do alerta.
        
        
        
        // 📤 RETORNO:
        // string - Cor hexadecimal associada à prioridade.
        
        
        // Param prioridade: Prioridade do alerta.
        // Returns: Cor hexadecimal associada à prioridade.
        public static string GetCorHexPrioridade(PrioridadeAlerta prioridade)
        {
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => "#0ea5e9",    // azul
                PrioridadeAlerta.Media => "#f59e0b",    // laranja
                PrioridadeAlerta.Alta => "#dc2626",     // vermelho
                _ => "#6b7280"                          // cinza
            };
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetNomePrioridade                                                        │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        // │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Obter o nome descritivo da prioridade do alerta.
        
        
        
        // 📥 PARÂMETROS:
        // prioridade - Prioridade do alerta.
        
        
        
        // 📤 RETORNO:
        // string - Nome descritivo da prioridade.
        
        
        // Param prioridade: Prioridade do alerta.
        // Returns: Nome descritivo da prioridade.
        public static string GetNomePrioridade(PrioridadeAlerta prioridade)
        {
            return prioridade switch
            {
                PrioridadeAlerta.Baixa => "Prioridade Baixa",
                PrioridadeAlerta.Media => "Prioridade Média",
                PrioridadeAlerta.Alta => "Prioridade Alta",
                _ => "Prioridade Normal"
            };
        }

        #endregion

        #region Métodos Auxiliares

        
        // Define alerta para ser exibido no cliente
        
        private static void SetAlert(
            string type ,
            string title ,
            string message ,
            string confirmButton = "OK" ,
            string cancelButton = null
        )
        {
            var alertData = new
            {
                type = type ,
                title = title ,
                message = message ,
                confirmButton = confirmButton ,
                cancelButton = cancelButton ,
            };

            TempDataSet("ShowSweetAlert" , JsonSerializer.Serialize(alertData));
        }

        
        // Extrai detalhes do erro incluindo arquivo e linha do stack trace
        
        private static object ObterDetalhesErro(Exception ex)
        {
            try
            {
                var st = new StackTrace(ex , true);
                var frames = st.GetFrames();

                if (frames != null && frames.Length > 0)
                {
                    // Pegar o primeiro frame (onde o erro foi gerado)
                    var frame = frames[0];
                    var fileName = frame.GetFileName();
                    var lineNumber = frame.GetFileLineNumber();
                    var methodName = frame.GetMethod()?.Name;

                    return new
                    {
                        arquivo = fileName != null ? Path.GetFileName(fileName) : null ,
                        arquivoCompleto = fileName ,
                        linha = lineNumber > 0 ? lineNumber : (int?)null ,
                        metodo = methodName ,
                        tipo = ex.GetType().Name
                    };
                }
            }
            catch { }

            return new
            {
                arquivo = (string)null ,
                linha = (int?)null ,
                metodo = (string)null ,
                tipo = ex.GetType().Name
            };
        }

        
        // Define alerta de erro técnico com informações detalhadas
        
        private static void SetErrorUnexpectedAlert(string arquivo , string metodo , Exception error)
        {
            var alertData = new
            {
                type = "errorUnexpected" ,
                classe = arquivo ,
                metodo = metodo ,
                erro = error.Message ,
                stack = error.StackTrace ,
                innerErro = error.InnerException?.Message ,
                innerStack = error.InnerException?.StackTrace ,

                // Extrair informações de linha aqui no C#
                detalhes = ObterDetalhesErro(error)
            };

            TempDataSet("ShowSweetAlert" , JsonSerializer.Serialize(alertData));
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TempDataSet                                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : SetAlert(), SetErrorUnexpectedAlert()                               │
        // │    ➡️ CHAMA       : TempFactory.GetTempData()                                          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gravar uma entrada em TempData para exibição de alertas no cliente.
        
        
        
        // 📥 PARÂMETROS:
        // key - Chave do TempData
        // value - Valor a ser armazenado
        
        
        // Param key: Chave do TempData.
        // Param value: Valor a ser armazenado.
        public static void TempDataSet(string key , object value)
        {
            try
            {
                var http = HttpCtx?.HttpContext;
                if (http == null || TempFactory == null)
                    return;
                var temp = TempFactory.GetTempData(http);
                temp[key] = value;
            }
            catch
            {
                // silencioso por design (não atrapalhar fluxo de erro)
            }
        }

        
        // Percorre frames do stack para achar o primeiro com info de arquivo/linha.
        
        private static (int? line, string file, string member) TentarObterLinha(Exception ex)
        {
            try
            {
                var st = new StackTrace(ex , true);
                var frames = st.GetFrames();
                if (frames == null || frames.Length == 0)
                    return (null, null, null);

                for (int i = 0; i < frames.Length; i++)
                {
                    var f = frames[i];
                    var file = f.GetFileName();
                    if (!string.IsNullOrEmpty(file))
                    {
                        int line = f.GetFileLineNumber();
                        if (line <= 0)
                            line = f.GetILOffset();
                        var method = f.GetMethod();
                        var member = method != null ? method.Name : null;
                        return (line > 0 ? line : (int?)null, file, member);
                    }
                }

                return (null, null, null);
            }
            catch
            {
                return (null, null, null);
            }
        }

        #endregion
    }
}
