/*
*  #################################################################################################
*  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                    #
*  #   MODULO:  HELPERS - SISTEMA DE ALERTAS E LOGGING DE ERROS                                    #
*  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
*  #################################################################################################
*/

using FrotiX.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace FrotiX.Helpers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: Alerta                                                              ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Sistema centralizado de alertas visuais (SweetAlert) e logging de erros  ║
    /// ║    com rastreamento automático de arquivo e linha. Usa TempData para        ║
    /// ║    persistência entre requisições e logs estruturados.                       ║
    /// ║                                                                              ║
    /// ║ 🎯 IMPORTÂNCIA CRÍTICA:                                                      ║
    /// ║    NÚCLEO DO SISTEMA DE FEEDBACK. Todo erro/sucesso/aviso do FrotiX passa  ║
    /// ║    por aqui. Garante UX consistente e auditoria completa de erros.          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📞 FUNÇÕES PRINCIPAIS:                                                       ║
    /// ║    • Erro/Sucesso/Warning() → Alertas visuais SweetAlert                    ║
    /// ║    • TratamentoErroComLinha() → Log de exceção com stack trace              ║
    /// ║    • Confirmar() → Diálogo de confirmação assíncrono                         ║
    /// ║                                                                              ║
    /// ║ 🔧 DEPENDÊNCIAS INJETADAS (via Startup.cs):                                  ║
    /// ║    • HttpCtx → Acesso ao contexto HTTP                                       ║
    /// ║    • TempFactory → Persistência de alertas entre requests                    ║
    /// ║    • LoggerFactory → Sistema de logging estruturado                          ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: GLOBAL - Usado em TODA a aplicação                                ║
    /// ║    • Arquivos relacionados: sweetalert_interop.js, _Layout.cshtml           ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    public static class Alerta
    {
        // ═══════════════════════════════════════════════════════════════════════════
        // BRIDGES DE DEPENDÊNCIA (Injetados no Startup/Program.cs)
        // ═══════════════════════════════════════════════════════════════════════════
        
        /// <summary>[INFRA] Acesso ao HttpContext para TempData e informações da requisição</summary>
        public static IHttpContextAccessor HttpCtx { get; set; }
        
        /// <summary>[INFRA] Factory para criar TempData (persistência de alertas entre redirects)</summary>
        public static ITempDataDictionaryFactory TempFactory { get; set; }
        
        /// <summary>[INFRA] Factory de Loggers para registro estruturado de erros</summary>
        public static ILoggerFactory LoggerFactory { get; set; }

        #region ═══════════════ MÉTODOS DE ALERTA VISUAL ═══════════════

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Erro                                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe alerta visual de ERRO usando SweetAlert com ícone vermelho.         ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • titulo: Título do alerta (ex: "Erro ao Salvar")                         ║
        /// ║    • texto: Mensagem detalhada do erro                                       ║
        /// ║    • confirmButtonText: Texto do botão (padrão: "OK")                        ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Erro(string titulo, string texto, string confirmButtonText = "OK")
        {
            // [DADOS] Armazena alerta tipo "error" em TempData para exibição no frontend
            SetAlert("error", titulo, texto, confirmButtonText);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Sucesso                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe alerta visual de SUCESSO com ícone verde (checkmark).               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Sucesso(string titulo, string texto, string confirmButtonText = "OK")
        {
            // [DADOS] Armazena alerta tipo "success" para feedback positivo
            SetAlert("success", titulo, texto, confirmButtonText);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Info                                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Exibe alerta visual de INFORMAÇÃO com ícone azul.              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Info(string titulo, string texto, string confirmButtonText = "OK")
        {
            // [DADOS] Alerta informativo neutro
            SetAlert("info", titulo, texto, confirmButtonText);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Warning                                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO: Exibe alerta de AVISO com ícone amarelo (warning).             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Warning(string titulo, string texto, string confirmButtonText = "OK")
        {
            // [DADOS] Alerta de atenção/cuidado
            SetAlert("warning", titulo, texto, confirmButtonText);
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Confirmar                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Exibe diálogo de CONFIRMAÇÃO com dois botões (Sim/Cancelar).              ║
        /// ║    Retorna Promise em JavaScript que resolve true/false.                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void Confirmar(
            string titulo,
            string texto,
            string confirmButtonText = "Sim",
            string cancelButtonText = "Cancelar"
        )
        {
            // [AJAX] Diálogo assíncrono de confirmação
            SetAlert("confirm", titulo, texto, confirmButtonText, cancelButtonText);
        }

        #endregion

        #region ═══════════════ TRATAMENTO DE ERRO COM RASTREAMENTO ═══════════════

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: TratamentoErroComLinha                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Método CRÍTICO de tratamento de exceções. Faz:                            ║
        /// ║    1. Log estruturado no servidor (arquivo Logs/)                            ║
        /// ║    2. Extração automática de arquivo e linha do stack trace                  ║
        /// ║    3. Exibição de alerta visual SweetAlert no frontend                       ║
        /// ║    4. Registro em banco de dados (tabela LogErros)                           ║
        /// ║                                                                              ║
        /// ║ 🎯 USO OBRIGATÓRIO:                                                          ║
        /// ║    TODO catch (Exception ex) deve chamar este método.                        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • arquivo: Nome do arquivo .cs (ex: "MotoristaController.cs")             ║
        /// ║    • funcao: Nome do método (ex: "Salvar")                                   ║
        /// ║    • error: Exception capturada                                              ║
        /// ║    • logger: Logger opcional (se null, cria um genérico)                     ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public static void TratamentoErroComLinha(
            string arquivo,
            string funcao,
            Exception error,
            ILogger logger = null
        )
        {
            // [REGRA] Validação obrigatória
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            // [LOGICA] Extrai informações do stack trace (arquivo e linha)
            var info = TentarObterLinha(error);
            
            // [DADOS] Monta nome do arquivo (prioriza parâmetro, fallback para stack)
            string fileName = !string.IsNullOrWhiteSpace(arquivo)
                ? Path.GetFileName(arquivo)
                : (info.file != null ? Path.GetFileName(info.file) : "arquivo desconhecido");

            // [DADOS] Monta nome da função
            string member = !string.IsNullOrWhiteSpace(funcao)
                ? funcao
                : (info.member ?? "função desconhecida");

            // [DADOS] Formata linha se disponível
            string linhaText = info.line.HasValue ? $" (linha {info.line.Value})" : string.Empty;
            
            // [DADOS] Monta mensagem de log estruturada
            string msg =
                $"{fileName}::{member}{linhaText}: {error.GetType().Name} - {error.Message}";

            // Tenta usar o logger, com proteção contra ObjectDisposedException
            ILogger useLogger = logger;
            if (useLogger == null)
            {
                try
                {
                    useLogger = LoggerFactory?.CreateLogger("Alerta");
                }
                catch (ObjectDisposedException)
                {
                    // LoggerFactory foi descartado (aplicação encerrando), usar fallback
                    useLogger = null;
                }
            }

            if (useLogger != null)
                useLogger.LogError(error , msg);
            else
                Debug.WriteLine(msg);

            // =========================================================================
            // 📝 LOG DE ERROS CENTRALIZADO (Integração com LogService)
            // =========================================================================
            try
            {
                // Tenta resolver o ILogService via Service Locator (HttpContext)
                // Isso evita ter que injetar ILogService em todas as classes que usam Alerta
                var logService = HttpCtx?.HttpContext?.RequestServices.GetService(typeof(FrotiX.Services.ILogService)) as FrotiX.Services.ILogService;
                
                if (logService != null)
                {
                    // Registra o erro no sistema de arquivos
                    logService.Error(
                        message: error.Message, 
                        exception: error, 
                        arquivo: fileName, 
                        metodo: member, 
                        linha: info.line
                    );
                }
            }
            catch (Exception logEx)
            {
                // Falha silenciosa no log para não interromper o fluxo de erro visual
                Debug.WriteLine($"[Alerta] Falha ao registrar log: {logEx.Message}");
            }
            // =========================================================================

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
