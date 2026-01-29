/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Helpers/Alerta.cs                                              ║
 * ║  Descrição: Classe estática principal de alertas SweetAlert. Exibe       ║
 * ║             Erro/Sucesso/Info/Warning/Confirmar via TempData. Inclui     ║
 * ║             TratamentoErroComLinha com extração automática de stack      ║
 * ║             trace e integração com ILogService via Service Locator.      ║
 * ║             ORIGEM: SERVER (para diferenciar de CLIENT_JS)               ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
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
    /// <summary>
    /// Utilitário de tratamento/log de erros com indicação de arquivo e linha.
    /// Integrado com sistema de alertas SweetAlert personalizado.
    /// Usa Service Locator pattern para obter ILogService e gravar erros no banco/arquivo
    /// de forma unificada (origem: SERVER).
    /// </summary>
    public static class Alerta
    {
        // --- Bridges para DI (preenchidos no Startup/Program) -----------------
        public static IHttpContextAccessor HttpCtx
        {
            get; set;
        }
        public static ITempDataDictionaryFactory TempFactory
        {
            get; set;
        }
        public static ILoggerFactory LoggerFactory
        {
            get; set;
        }

        /// <summary>
        /// Service Provider para obter ILogService via Service Locator pattern.
        /// Preenchido no Startup/Program.
        /// </summary>
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }

        #region Métodos de Alerta Visual

        /// <summary>
        /// Exibe alerta de erro
        /// </summary>
        public static void Erro(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("error" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// Exibe alerta de sucesso
        /// </summary>
        public static void Sucesso(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("success" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// Exibe alerta de informação
        /// </summary>
        public static void Info(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("info" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// Exibe alerta de aviso
        /// </summary>
        public static void Warning(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("warning" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// Exibe alerta de confirmação
        /// </summary>
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

        /// <summary>
        /// Tratamento de erro com linha - Log unificado (ILogService) + Alerta visual.
        /// Usa Service Locator para obter ILogService do container DI.
        /// Se ILogService não estiver disponível (ex: thread background), faz fallback para ILogger/Console.
        /// ORIGEM: SERVER (para diferenciar de erros CLIENT_JS)
        /// </summary>
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

        /// <summary>
        /// Overload legado (Exception primeiro). Redireciona para a ordem nova.
        /// </summary>
        public static void TratamentoErroComLinha(
            Exception error ,
            string arquivo ,
            string funcao ,
            ILogger logger = null
        ) => TratamentoErroComLinha(arquivo , funcao , error , logger);

        #endregion

        #region Métodos de Prioridade de Alertas

        /// <summary>
        /// Obtém o ícone FontAwesome Duotone baseado na prioridade do alerta
        /// </summary>
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

        /// <summary>
        /// Obtém a classe CSS de cor baseada na prioridade do alerta
        /// </summary>
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

        /// <summary>
        /// Obtém a cor hexadecimal baseada na prioridade do alerta
        /// </summary>
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

        /// <summary>
        /// Obtém o nome descritivo da prioridade
        /// </summary>
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

        /// <summary>
        /// Define alerta para ser exibido no cliente
        /// </summary>
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

        /// <summary>
        /// Extrai detalhes do erro incluindo arquivo e linha do stack trace
        /// </summary>
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

        /// <summary>
        /// Define alerta de erro técnico com informações detalhadas
        /// </summary>
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

        /// <summary>
        /// Grava uma entrada em TempData (se disponível).
        /// </summary>
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

        /// <summary>
        /// Percorre frames do stack para achar o primeiro com info de arquivo/linha.
        /// </summary>
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
