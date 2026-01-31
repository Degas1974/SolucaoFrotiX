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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: Alerta                                                                            │
    /// │ 📦 TIPO: Estática                                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// <para>
    /// 🎯 <b>OBJETIVO:</b><br/>
    ///    Centralizar alertas SweetAlert no backend e registrar erros com arquivo/linha (SERVER).
    /// </para>
    ///
    /// <para>
    /// 🔗 <b>RASTREABILIDADE:</b><br/>
    ///    ⬅️ CHAMADO POR : Controllers, Pages, Services e Helpers internos<br/>
    ///    ➡️ CHAMA       : ILogService.Error(), ILogger.LogError(), TempData
    /// </para>
    /// </summary>
    public static class Alerta
    {
        // --- Bridges para DI (preenchidos no Startup/Program) -----------------
        /// <summary>Acesso ao HttpContext atual via IHttpContextAccessor.</summary>
        public static IHttpContextAccessor HttpCtx
        {
            get; set;
        }
        /// <summary>Factory para acesso ao TempData de requisições.</summary>
        public static ITempDataDictionaryFactory TempFactory
        {
            get; set;
        }
        /// <summary>Factory para criação de ILogger em fallback de log.</summary>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Erro                                                                       │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : SetAlert()                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Exibir alerta de erro via SweetAlert no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    titulo - Título do alerta<br/>
        ///    texto - Mensagem do alerta<br/>
        ///    confirmButtonText - Texto do botão de confirmação
        /// </para>
        /// </summary>
        /// <param name="titulo">Título do alerta.</param>
        /// <param name="texto">Mensagem do alerta.</param>
        /// <param name="confirmButtonText">Texto do botão de confirmação.</param>
        public static void Erro(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("error" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Sucesso                                                                    │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : SetAlert()                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Exibir alerta de sucesso via SweetAlert no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    titulo - Título do alerta<br/>
        ///    texto - Mensagem do alerta<br/>
        ///    confirmButtonText - Texto do botão de confirmação
        /// </para>
        /// </summary>
        /// <param name="titulo">Título do alerta.</param>
        /// <param name="texto">Mensagem do alerta.</param>
        /// <param name="confirmButtonText">Texto do botão de confirmação.</param>
        public static void Sucesso(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("success" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Info                                                                       │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : SetAlert()                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Exibir alerta informativo via SweetAlert no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    titulo - Título do alerta<br/>
        ///    texto - Mensagem do alerta<br/>
        ///    confirmButtonText - Texto do botão de confirmação
        /// </para>
        /// </summary>
        /// <param name="titulo">Título do alerta.</param>
        /// <param name="texto">Mensagem do alerta.</param>
        /// <param name="confirmButtonText">Texto do botão de confirmação.</param>
        public static void Info(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("info" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Warning                                                                    │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : SetAlert()                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Exibir alerta de aviso via SweetAlert no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    titulo - Título do alerta<br/>
        ///    texto - Mensagem do alerta<br/>
        ///    confirmButtonText - Texto do botão de confirmação
        /// </para>
        /// </summary>
        /// <param name="titulo">Título do alerta.</param>
        /// <param name="texto">Mensagem do alerta.</param>
        /// <param name="confirmButtonText">Texto do botão de confirmação.</param>
        public static void Warning(string titulo , string texto , string confirmButtonText = "OK")
        {
            SetAlert("warning" , titulo , texto , confirmButtonText);
        }

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Confirmar                                                                 │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : SetAlert()                                                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Exibir alerta de confirmação via SweetAlert no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    titulo - Título do alerta<br/>
        ///    texto - Mensagem do alerta<br/>
        ///    confirmButtonText - Texto do botão de confirmação<br/>
        ///    cancelButtonText - Texto do botão de cancelamento
        /// </para>
        /// </summary>
        /// <param name="titulo">Título do alerta.</param>
        /// <param name="texto">Mensagem do alerta.</param>
        /// <param name="confirmButtonText">Texto do botão de confirmação.</param>
        /// <param name="cancelButtonText">Texto do botão de cancelamento.</param>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TratamentoErroComLinha                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Controllers, Pages, Services                                         │
        /// │    ➡️ CHAMA       : TentarObterLinha(), ILogService.Error(), ILogger.LogError(), SetErrorUnexpectedAlert() │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Registrar erro com arquivo/linha e exibir alerta técnico (origem SERVER).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    arquivo - Caminho ou nome do arquivo de origem<br/>
        ///    funcao - Nome da função/método de origem<br/>
        ///    error - Exceção capturada<br/>
        ///    logger - Logger opcional para fallback
        /// </para>
        /// </summary>
        /// <param name="arquivo">Caminho ou nome do arquivo de origem.</param>
        /// <param name="funcao">Nome da função/método de origem.</param>
        /// <param name="error">Exceção capturada.</param>
        /// <param name="logger">Logger opcional para fallback.</param>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TratamentoErroComLinha (overload)                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Código legado                                                       │
        /// │    ➡️ CHAMA       : TratamentoErroComLinha(arquivo, funcao, error, logger)              │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Manter compatibilidade com a assinatura antiga (Exception primeiro).
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    error - Exceção capturada<br/>
        ///    arquivo - Caminho ou nome do arquivo de origem<br/>
        ///    funcao - Nome da função/método de origem<br/>
        ///    logger - Logger opcional para fallback
        /// </para>
        /// </summary>
        /// <param name="error">Exceção capturada.</param>
        /// <param name="arquivo">Caminho ou nome do arquivo de origem.</param>
        /// <param name="funcao">Nome da função/método de origem.</param>
        /// <param name="logger">Logger opcional para fallback.</param>
        public static void TratamentoErroComLinha(
            Exception error ,
            string arquivo ,
            string funcao ,
            ILogger logger = null
        ) => TratamentoErroComLinha(arquivo , funcao , error , logger);

        #endregion

        #region Métodos de Prioridade de Alertas

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetIconePrioridade                                                      │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        /// │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter o ícone FontAwesome Duotone baseado na prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    prioridade - Prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    string - Classe CSS do ícone FontAwesome.
        /// </para>
        /// </summary>
        /// <param name="prioridade">Prioridade do alerta.</param>
        /// <returns>Classe CSS do ícone FontAwesome.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetCorPrioridade                                                         │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        /// │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter a classe CSS de cor baseada na prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    prioridade - Prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    string - Classe CSS para cor do alerta.
        /// </para>
        /// </summary>
        /// <param name="prioridade">Prioridade do alerta.</param>
        /// <returns>Classe CSS para cor do alerta.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetCorHexPrioridade                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        /// │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter a cor hexadecimal baseada na prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    prioridade - Prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    string - Cor hexadecimal associada à prioridade.
        /// </para>
        /// </summary>
        /// <param name="prioridade">Prioridade do alerta.</param>
        /// <returns>Cor hexadecimal associada à prioridade.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetNomePrioridade                                                        │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : UI/Views e helpers de alertas                                       │
        /// │    ➡️ CHAMA       : (switch de PrioridadeAlerta)                                      │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Obter o nome descritivo da prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    prioridade - Prioridade do alerta.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    string - Nome descritivo da prioridade.
        /// </para>
        /// </summary>
        /// <param name="prioridade">Prioridade do alerta.</param>
        /// <returns>Nome descritivo da prioridade.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TempDataSet                                                                │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : SetAlert(), SetErrorUnexpectedAlert()                               │
        /// │    ➡️ CHAMA       : TempFactory.GetTempData()                                          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Gravar uma entrada em TempData para exibição de alertas no cliente.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    key - Chave do TempData<br/>
        ///    value - Valor a ser armazenado
        /// </para>
        /// </summary>
        /// <param name="key">Chave do TempData.</param>
        /// <param name="value">Valor a ser armazenado.</param>
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
