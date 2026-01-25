using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services;

/// <summary>
/// Implementação do serviço de logging com notificações em tempo real
/// </summary>
public class LogService : ILogService
{
    private static string LogPath => Path.Combine(FileSystem.AppDataDirectory, "app_logs.txt");
    
    // Evento disparado quando um novo erro ocorre
    public event Action<string>? OnErrorOccurred;
    
    public void Info(string message)
    {
        try
        {
            WriteLog($"[INFO] {message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Info: {ex.Message}");
        }
    }

    public void Warning(string message)
    {
        try
        {
            WriteLog($"[WARN] ⚠️ {message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Warning: {ex.Message}");
        }
    }

    public void Error(string message, Exception? exception = null)
    {
        try
        {
            WriteLogError(message, exception);
            
            // Notifica ouvintes sobre o novo erro
            OnErrorOccurred?.Invoke(message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Error: {ex.Message}");
        }
    }

    public void Debug(string message)
    {
#if DEBUG
        try
        {
            WriteLog($"[DEBUG] 🐛 {message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar log Debug: {ex.Message}");
        }
#endif
    }

    public void OperationStart(string operationName)
    {
        try
        {
            WriteLog($"[OPERATION] ▶️ Iniciando: {operationName}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar início de operação: {ex.Message}");
        }
    }

    public void OperationSuccess(string operationName, string? details = null)
    {
        try
        {
            var message = $"[OPERATION] ✅ Sucesso: {operationName}";
            if (!string.IsNullOrEmpty(details))
                message += $" - {details}";
            
            WriteLog(message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar sucesso de operação: {ex.Message}");
        }
    }

    public void OperationFailed(string operationName, Exception exception)
    {
        try
        {
            WriteLogError($"[OPERATION] ❌ Falha: {operationName}", exception);
            
            // Notifica ouvintes sobre o erro na operação
            OnErrorOccurred?.Invoke($"Falha: {operationName}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar falha de operação: {ex.Message}");
        }
    }

    public void UserAction(string action, string? details = null)
    {
        try
        {
            var message = $"[USER] 👤 {action}";
            if (!string.IsNullOrEmpty(details))
                message += $" - {details}";
            
            WriteLog(message);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao registrar ação do usuário: {ex.Message}");
        }
    }

    public string GetAllLogs()
    {
        try
        {
            if (File.Exists(LogPath))
                return File.ReadAllText(LogPath);
            return "Nenhum log disponível.";
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao obter logs: {ex.Message}");
            return $"Erro ao obter logs: {ex.Message}";
        }
    }

    public void ClearLogs()
    {
        try
        {
            if (File.Exists(LogPath))
                File.Delete(LogPath);
            WriteLog("========== LOGS LIMPOS ==========");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao limpar logs: {ex.Message}");
        }
    }

    public int GetErrorCount()
    {
        try
        {
            var logs = GetAllLogs();
            if (string.IsNullOrEmpty(logs))
                return 0;

            // Conta quantas linhas começam com [ERROR
            return logs.Split('\n')
                       .Count(line => line.TrimStart().StartsWith("[ERROR"));
        }
        catch
        {
            return 0;
        }
    }

    // ========== MÉTODOS PRIVADOS DE LOG ==========

    private static void WriteLog(string message)
    {
        try
        {
            var logMessage = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
            File.AppendAllText(LogPath, logMessage + Environment.NewLine);
            System.Diagnostics.Debug.WriteLine(logMessage);
        }
        catch { }
    }

    private static void WriteLogError(string message, Exception? exception = null)
    {
        try
        {
            var logMessage = $"[ERROR {DateTime.Now:HH:mm:ss.fff}] {message}";
            if (exception != null)
            {
                logMessage += $"\n  Exception: {exception.GetType().Name}";
                logMessage += $"\n  Message: {exception.Message}";
                logMessage += $"\n  StackTrace: {exception.StackTrace}";
                if (exception.InnerException != null)
                {
                    logMessage += $"\n  InnerException: {exception.InnerException.Message}";
                }
            }
            File.AppendAllText(LogPath, logMessage + Environment.NewLine);
            System.Diagnostics.Debug.WriteLine(logMessage);
        }
        catch { }
    }
}
