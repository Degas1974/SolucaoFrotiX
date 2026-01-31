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
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    /// │ 🎯 CLASSE: AlertaBackend                                                                      │
    /// │ 📦 TIPO: Estática                                                                             │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    ///
    /// <para>
    /// 🎯 <b>OBJETIVO:</b><br/>
    ///    Logar erros inesperados no backend de forma consistente, sem dependência de JSInterop.
    /// </para>
    ///
    /// <para>
    /// 🔗 <b>RASTREABILIDADE:</b><br/>
    ///    ⬅️ CHAMADO POR : Services, Filters, Controllers e Helpers internos<br/>
    ///    ➡️ CHAMA       : ILogger.LogError(), Console.Error, Activity.Current
    /// </para>
    /// </summary>
    public static class AlertaBackend
    {
        private static ILogger? _logger;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: ConfigureLogger                                                             │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Program.cs / Startup / composição de serviços                        │
        /// │    ➡️ CHAMA       : (atribuição direta de logger)                                        │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Injetar um ILogger opcional para uso interno do helper.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    logger - Logger a ser utilizado internamente (opcional).
        /// </para>
        /// </summary>
        /// <param name="logger">Logger a ser utilizado internamente (opcional).</param>
        public static void ConfigureLogger(ILogger logger) => _logger = logger;

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetCorrelationId                                                            │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : TratamentoErroComLinha*, SendUnexpected                              │
        /// │    ➡️ CHAMA       : Activity.Current, Guid.NewGuid()                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Gerar um identificador de correlação usando Activity.Current ou GUID.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    string - Identificador de correlação para rastreabilidade.
        /// </para>
        /// </summary>
        /// <returns>Identificador de correlação para rastreabilidade.</returns>
        public static string GetCorrelationId() =>
            Activity.Current?.Id ?? Guid.NewGuid().ToString("N");

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TratamentoErroComLinha                                                     │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Código de domínio (instance)                                         │
        /// │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Logar um erro inesperado com contexto da instância, arquivo e linha.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    ctx - Contexto de instância (this) para identificação<br/>
        ///    ex - Exceção capturada<br/>
        ///    userMessage - Mensagem amigável opcional<br/>
        ///    tag - Tag de categorização opcional<br/>
        ///    severity - Severidade numérica para compatibilidade<br/>
        ///    member - Nome do membro chamador (CallerMemberName)<br/>
        ///    file - Caminho do arquivo chamador (CallerFilePath)<br/>
        ///    line - Linha do arquivo chamador (CallerLineNumber)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    ValueTask concluída após registrar o log.
        /// </para>
        /// </summary>
        /// <param name="ctx">Contexto de instância (this) para identificação.</param>
        /// <param name="ex">Exceção capturada.</param>
        /// <param name="userMessage">Mensagem amigável opcional.</param>
        /// <param name="tag">Tag de categorização opcional.</param>
        /// <param name="severity">Severidade numérica para compatibilidade.</param>
        /// <param name="member">Nome do membro chamador (CallerMemberName).</param>
        /// <param name="file">Caminho do arquivo chamador (CallerFilePath).</param>
        /// <param name="line">Linha do arquivo chamador (CallerLineNumber).</param>
        /// <returns>ValueTask concluída após registrar o log.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TratamentoErroComLinhaStatic                                                │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Código estático (sem instância)                                      │
        /// │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Logar um erro inesperado em contexto estático, com arquivo e linha.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    ex - Exceção capturada<br/>
        ///    userMessage - Mensagem amigável opcional<br/>
        ///    tag - Tag de categorização opcional<br/>
        ///    severity - Severidade numérica para compatibilidade<br/>
        ///    member - Nome do membro chamador (CallerMemberName)<br/>
        ///    file - Caminho do arquivo chamador (CallerFilePath)<br/>
        ///    line - Linha do arquivo chamador (CallerLineNumber)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    ValueTask concluída após registrar o log.
        /// </para>
        /// </summary>
        /// <param name="ex">Exceção capturada.</param>
        /// <param name="userMessage">Mensagem amigável opcional.</param>
        /// <param name="tag">Tag de categorização opcional.</param>
        /// <param name="severity">Severidade numérica para compatibilidade.</param>
        /// <param name="member">Nome do membro chamador (CallerMemberName).</param>
        /// <param name="file">Caminho do arquivo chamador (CallerFilePath).</param>
        /// <param name="line">Linha do arquivo chamador (CallerLineNumber).</param>
        /// <returns>ValueTask concluída após registrar o log.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: SendUnexpected                                                              │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : Helpers puros / chamadas utilitárias                                │
        /// │    ➡️ CHAMA       : TryExtractFileLine(), GetCorrelationId(), ILogger.LogError()          │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Registrar erro inesperado sem contexto de instância/classe.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    source - Identificador de origem do log<br/>
        ///    userMessage - Mensagem amigável opcional<br/>
        ///    ex - Exceção capturada<br/>
        ///    tag - Tag de categorização opcional<br/>
        ///    severity - Severidade numérica para compatibilidade<br/>
        ///    member - Nome do membro chamador (CallerMemberName)<br/>
        ///    file - Caminho do arquivo chamador (CallerFilePath)<br/>
        ///    line - Linha do arquivo chamador (CallerLineNumber)
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    ValueTask concluída após registrar o log.
        /// </para>
        /// </summary>
        /// <param name="source">Identificador de origem do log.</param>
        /// <param name="userMessage">Mensagem amigável opcional.</param>
        /// <param name="ex">Exceção capturada.</param>
        /// <param name="tag">Tag de categorização opcional.</param>
        /// <param name="severity">Severidade numérica para compatibilidade.</param>
        /// <param name="member">Nome do membro chamador (CallerMemberName).</param>
        /// <param name="file">Caminho do arquivo chamador (CallerFilePath).</param>
        /// <param name="line">Linha do arquivo chamador (CallerLineNumber).</param>
        /// <returns>ValueTask concluída após registrar o log.</returns>
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
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: TryExtractFileLine                                                          │
        /// │ 🔗 RASTREABILIDADE:                                                                      │
        /// │    ⬅️ CHAMADO POR : TratamentoErroComLinha*, SendUnexpected                              │
        /// │    ➡️ CHAMA       : Exception.StackTrace                                                │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        ///
        /// <para>
        /// 🎯 <b>OBJETIVO:</b><br/>
        ///    Extrair o arquivo e a linha do topo do stack trace da exceção.
        /// </para>
        ///
        /// <para>
        /// 📥 <b>PARÂMETROS:</b><br/>
        ///    ex - Exceção capturada.
        /// </para>
        ///
        /// <para>
        /// 📤 <b>RETORNO:</b><br/>
        ///    (string? file, int? line) com o arquivo e a linha encontrados.
        /// </para>
        /// </summary>
        /// <param name="ex">Exceção capturada.</param>
        /// <returns>Tupla (arquivo, linha) do stack trace.</returns>
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
