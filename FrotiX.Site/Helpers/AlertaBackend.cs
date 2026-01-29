/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: AlertaBackend.cs                                                                        ║
   ║ 📂 CAMINHO: /Helpers                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Helper backend-only para logging de erros (sem JSInterop). TratamentoErroComLinha com            ║
   ║    CallerMemberName/FilePath/Line automáticos. Versões estáticas e instância. Extrai file/line     ║
   ║    do stack. Correlation ID via Activity.Current ou GUID para rastreabilidade.                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE FUNÇÕES (Entradas -> Saídas):                                                         ║
   ║ 1. [ConfigureLogger]           : Injeta ILogger opcional............. (logger) -> void             ║
   ║ 2. [GetCorrelationId]          : Retorna Activity.Id ou GUID novo.... () -> string                 ║
   ║ 3. [TratamentoErroComLinha]    : Log erro com contexto (instância)... (ctx,ex,...) -> ValueTask    ║
   ║ 4. [TratamentoErroComLinhaStatic]: Log erro contexto estático........ <T>(ex,...) -> ValueTask     ║
   ║ 5. [SendUnexpected]            : Log direto sem contexto de classe... (src,msg,ex) -> ValueTask    ║
   ║ 6. [TryExtractFileLine]        : Extrai (arquivo,linha) do stack..... (Exception) -> tuple        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: ILogger, System.Diagnostics.Activity, CallerMemberName/FilePath/LineNumber        ║
   ║ 📅 ATUALIZAÇÃO: 29/01/2026 | 👤 AUTOR: Copilot | 📝 VERSÃO: 2.0                                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FrotiX.Helpers
{
    /// <summary>
    /// Backend-only alert/log helper. Não depende de JSInterop.
    /// Único objetivo: logar de forma consistente os erros inesperados
    /// e padronizar mensagem/correlação para o ExceptionHandler global.
    /// </summary>
    public static class AlertaBackend
    {
        private static ILogger? _logger;

        /// <summary>Injeta um logger opcional (ex.: em Startup).</summary>
        public static void ConfigureLogger(ILogger logger) => _logger = logger;

        /// <summary>Id de correlação (Activity.Current.Id, se houver).</summary>
        public static string GetCorrelationId() =>
            Activity.Current?.Id ?? Guid.NewGuid().ToString("N");

        /// <summary>
        /// Loga um erro inesperado a partir de uma instância (this).
        /// </summary>
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

        /// <summary>
        /// Versão para chamadas em contextos estáticos (sem 'this').
        /// </summary>
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

        /// <summary>
        /// Versão direta para enviar/logar sem contexto (helpers puros).
        /// </summary>
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

        /// <summary>
        /// Tenta extrair (arquivo,linha) do topo do stacktrace.
        /// </summary>
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
