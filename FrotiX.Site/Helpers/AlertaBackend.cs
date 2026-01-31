/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AlertaBackend.cs                                                                      ║
   ║ 📂 CAMINHO: Helpers/                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Helper backend-only para logging de erros (sem JSInterop). Usa CallerMemberName/FilePath/Line  ║
   ║    automáticos e correlation ID via Activity/Guid, com versões estáticas e por instância.         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS DISPONÍVEIS:                                                                            ║
   ║    • ConfigureLogger(ILogger logger)                                                              ║
   ║    • GetCorrelationId()                                                                           ║
   ║    • TratamentoErroComLinha(object? ctx, Exception ex, string? userMessage = null, ...)           ║
   ║    • TratamentoErroComLinhaStatic<T>(Exception ex, string? userMessage = null, ...)                ║
   ║    • SendUnexpected(string source, string? userMessage, Exception ex, ...)                         ║
   ║    • TryExtractFileLine(Exception ex)                                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: ILogger, System.Diagnostics.Activity, CallerMemberName/FilePath/LineNumber        ║
   ║ 📅 ATUALIZAÇÃO: 30/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FrotiX.Helpers
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: AlertaBackend                                                                      │
    // │ 📦 TIPO: Estática                                                                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    
    
    // 🎯 OBJETIVO:
    // Logar erros inesperados no backend de forma consistente, sem dependência de JSInterop.
    
    
    
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Services, Filters, Controllers e Helpers internos
    // ➡️ CHAMA       : ILogger.LogError(), Console.Error, Activity.Current
    
    
    public static class AlertaBackend
    {
        private static ILogger? _logger;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: ConfigureLogger                                                             │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Program.cs / Startup / composição de serviços                        │
        // │    ➡️ CHAMA       : (atribuição direta de logger)                                        │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Injetar um ILogger opcional para uso interno do helper.
        
        
        
        // 📥 PARÂMETROS:
        // logger - Logger a ser utilizado internamente (opcional).
        
        
        // Param logger: Logger a ser utilizado internamente (opcional).
        public static void ConfigureLogger(ILogger logger) => _logger = logger;

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: GetCorrelationId                                                            │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : TratamentoErroComLinha*, SendUnexpected                              │
        // │    ➡️ CHAMA       : Activity.Current, Guid.NewGuid()                                    │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Gerar um identificador de correlação usando Activity.Current ou GUID.
        
        
        
        // 📤 RETORNO:
        // string - Identificador de correlação para rastreabilidade.
        
        
        // Returns: Identificador de correlação para rastreabilidade.
        public static string GetCorrelationId() =>
            Activity.Current?.Id ?? Guid.NewGuid().ToString("N");

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TratamentoErroComLinha                                                     │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Código de domínio (instance)                                         │
        // │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Logar um erro inesperado com contexto da instância, arquivo e linha.
        
        
        
        // 📥 PARÂMETROS:
        // ctx - Contexto de instância (this) para identificação
        // ex - Exceção capturada
        // userMessage - Mensagem amigável opcional
        // tag - Tag de categorização opcional
        // severity - Severidade numérica para compatibilidade
        // member - Nome do membro chamador (CallerMemberName)
        // file - Caminho do arquivo chamador (CallerFilePath)
        // line - Linha do arquivo chamador (CallerLineNumber)
        
        
        
        // 📤 RETORNO:
        // ValueTask concluída após registrar o log.
        
        
        // Param ctx: Contexto de instância (this) para identificação.
        // Param ex: Exceção capturada.
        // Param userMessage: Mensagem amigável opcional.
        // Param tag: Tag de categorização opcional.
        // Param severity: Severidade numérica para compatibilidade.
        // Param member: Nome do membro chamador (CallerMemberName).
        // Param file: Caminho do arquivo chamador (CallerFilePath).
        // Param line: Linha do arquivo chamador (CallerLineNumber).
        // Returns: ValueTask concluída após registrar o log.
        public static ValueTask TratamentoErroComLinha(
            object? ctx,
            Exception ex,
            string? userMessage = null,
            string? tag = null,
            int severity = 0, // compat
            [CallerMemberName] string? member = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0
        )
        {
            try
            {
                var logger = _logger;
                var (srcFile, srcLine) = TryExtractFileLine(ex);
                var correlationId = GetCorrelationId();

                string typeName = ctx?.GetType().FullName ?? "UnknownContext";
                string msg = userMessage ?? ex.Message;

                if (logger != null)
                {
                    logger.LogError(
                        ex,
                        "Unexpected error | ctx={Context} | member={Member} | file={File}:{Line} | exFile={ExFile}:{ExLine} | tag={Tag} | correlationId={CorrelationId} | msg={Message}",
                        typeName,
                        member,
                        file,
                        line,
                        srcFile,
                        srcLine,
                        tag,
                        correlationId,
                        msg
                    );
                }
                else
                {
                    Console.Error.WriteLine(
                        $"[ERROR] {DateTime.Now:o} {typeName}.{member} {file}:{line} tag={tag} corr={correlationId} msg={msg} ex={ex}"
                    );
                }

                return ValueTask.CompletedTask;
            }
            catch (Exception ex2)
            {
                Console.Error.WriteLine($"[ERROR][logging-failed] {ex2}");
                return ValueTask.CompletedTask;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TratamentoErroComLinhaStatic                                                │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Código estático (sem instância)                                      │
        // │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Logar um erro inesperado em contexto estático, com arquivo e linha.
        
        
        
        // 📥 PARÂMETROS:
        // ex - Exceção capturada
        // userMessage - Mensagem amigável opcional
        // tag - Tag de categorização opcional
        // severity - Severidade numérica para compatibilidade
        // member - Nome do membro chamador (CallerMemberName)
        // file - Caminho do arquivo chamador (CallerFilePath)
        // line - Linha do arquivo chamador (CallerLineNumber)
        
        
        
        // 📤 RETORNO:
        // ValueTask concluída após registrar o log.
        
        
        // Param ex: Exceção capturada.
        // Param userMessage: Mensagem amigável opcional.
        // Param tag: Tag de categorização opcional.
        // Param severity: Severidade numérica para compatibilidade.
        // Param member: Nome do membro chamador (CallerMemberName).
        // Param file: Caminho do arquivo chamador (CallerFilePath).
        // Param line: Linha do arquivo chamador (CallerLineNumber).
        // Returns: ValueTask concluída após registrar o log.
        public static ValueTask TratamentoErroComLinhaStatic<T>(
            Exception ex,
            string? userMessage = null,
            string? tag = null,
            int severity = 0,
            [CallerMemberName] string? member = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0
        )
        {
            try
            {
                var logger = _logger;
                var (srcFile, srcLine) = TryExtractFileLine(ex);
                var correlationId = GetCorrelationId();

                string typeName = typeof(T).FullName ?? typeof(T).Name;
                string msg = userMessage ?? ex.Message;

                if (logger != null)
                {
                    logger.LogError(
                        ex,
                        "Unexpected error [static] | ctx={Context} | member={Member} | file={File}:{Line} | exFile={ExFile}:{ExLine} | tag={Tag} | correlationId={CorrelationId} | msg={Message}",
                        typeName,
                        member,
                        file,
                        line,
                        srcFile,
                        srcLine,
                        tag,
                        correlationId,
                        msg
                    );
                }
                else
                {
                    Console.Error.WriteLine(
                        $"[ERROR][static] {DateTime.Now:o} {typeName}.{member} {file}:{line} tag={tag} corr={correlationId} msg={msg} ex={ex}"
                    );
                }

                return ValueTask.CompletedTask;
            }
            catch (Exception ex2)
            {
                Console.Error.WriteLine($"[ERROR][logging-failed] {ex2}");
                return ValueTask.CompletedTask;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: SendUnexpected                                                              │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : Helpers puros / chamadas utilitárias                                │
        // │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Registrar erro inesperado sem contexto de instância/classe.
        
        
        
        // 📥 PARÂMETROS:
        // source - Identificador de origem do log
        // userMessage - Mensagem amigável opcional
        // ex - Exceção capturada
        // tag - Tag de categorização opcional
        // severity - Severidade numérica para compatibilidade
        // member - Nome do membro chamador (CallerMemberName)
        // file - Caminho do arquivo chamador (CallerFilePath)
        // line - Linha do arquivo chamador (CallerLineNumber)
        
        
        
        // 📤 RETORNO:
        // ValueTask concluída após registrar o log.
        
        
        // Param source: Identificador de origem do log.
        // Param userMessage: Mensagem amigável opcional.
        // Param ex: Exceção capturada.
        // Param tag: Tag de categorização opcional.
        // Param severity: Severidade numérica para compatibilidade.
        // Param member: Nome do membro chamador (CallerMemberName).
        // Param file: Caminho do arquivo chamador (CallerFilePath).
        // Param line: Linha do arquivo chamador (CallerLineNumber).
        // Returns: ValueTask concluída após registrar o log.
        public static ValueTask SendUnexpected(
            string source,
            string? userMessage,
            Exception ex,
            string? tag = null,
            int severity = 0,
            [CallerMemberName] string? member = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0
        )
        {
            try
            {
                var logger = _logger;
                var (srcFile, srcLine) = TryExtractFileLine(ex);
                var correlationId = GetCorrelationId();
                string msg = userMessage ?? ex.Message;

                if (logger != null)
                {
                    logger.LogError(
                        ex,
                        "Unexpected error [send] | src={Source} | member={Member} | file={File}:{Line} | exFile={ExFile}:{ExLine} | tag={Tag} | correlationId={CorrelationId} | msg={Message}",
                        source,
                        member,
                        file,
                        line,
                        srcFile,
                        srcLine,
                        tag,
                        correlationId,
                        msg
                    );
                }
                else
                {
                    Console.Error.WriteLine(
                        $"[ERROR][send] {DateTime.Now:o} {source}.{member} {file}:{line} tag={tag} corr={correlationId} msg={msg} ex={ex}"
                    );
                }

                return ValueTask.CompletedTask;
            }
            catch (Exception ex2)
            {
                Console.Error.WriteLine($"[ERROR][logging-failed] {ex2}");
                return ValueTask.CompletedTask;
            }
        }

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ MÉTODO: TryExtractFileLine                                                          │
        // │ 🔗 RASTREABILIDADE:                                                                      │
        // │    ⬅️ CHAMADO POR : TratamentoErroComLinha*, SendUnexpected                              │
        // │    ➡️ CHAMA       : Exception.StackTrace                                                │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        
        // 🎯 OBJETIVO:
        // Extrair o arquivo e a linha do topo do stack trace da exceção.
        
        
        
        // 📥 PARÂMETROS:
        // ex - Exceção capturada.
        
        
        
        // 📤 RETORNO:
        // (string? file, int? line) com o arquivo e a linha encontrados.
        
        
        // Param ex: Exceção capturada.
        // Returns: Tupla (arquivo, linha) do stack trace.
        public static (string? file, int? line) TryExtractFileLine(Exception ex)
        {
            try
            {
                var st = ex.StackTrace;
                if (string.IsNullOrWhiteSpace(st))
                    return (null, null);

                // padrío: " in C:\path\file.cs:line 123"
                const string token = ":line ";
                int lineIdx = st.IndexOf(token, StringComparison.OrdinalIgnoreCase);
                if (lineIdx < 0)
                    return (null, null);

                int inIdx = st.LastIndexOf(" in ", lineIdx, StringComparison.OrdinalIgnoreCase);
                if (inIdx < 0)
                    return (null, null);

                int pathStart = inIdx + 4;
                int pathEnd = st.LastIndexOf(':', lineIdx - 1);
                if (pathEnd < 0 || pathEnd <= pathStart)
                    pathEnd = lineIdx;

                var path = st.Substring(pathStart, pathEnd - pathStart).Trim();
                int numStart = lineIdx + token.Length;

                if (
                    int.TryParse(
                        st.Substring(numStart)
                            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0],
                        out var ln
                    )
                )
                    return (path, ln);

                return (path, null);
            }
            catch (Exception ex2)
            {
                Console.Error.WriteLine($"[ERROR][extract-fileline-failed] {ex2}");
                return (null, null);
            }
        }
    }
}
