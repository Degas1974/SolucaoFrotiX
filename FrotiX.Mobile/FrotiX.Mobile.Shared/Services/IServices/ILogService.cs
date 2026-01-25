namespace FrotiX.Mobile.Shared.Services.IServices;

/// <summary>
/// Serviço de logging para toda a aplicação FrotiX
/// </summary>
public interface ILogService
{
    /// <summary>
    /// Registra uma mensagem informativa
    /// </summary>
    void Info(string message);
    
    /// <summary>
    /// Registra um aviso
    /// </summary>
    void Warning(string message);
    
    /// <summary>
    /// Registra um erro com exceção opcional
    /// </summary>
    void Error(string message, Exception exception = null);
    
    /// <summary>
    /// Registra uma mensagem de debug (apenas em modo DEBUG)
    /// </summary>
    void Debug(string message);
    
    /// <summary>
    /// Registra o início de uma operação
    /// </summary>
    void OperationStart(string operationName);
    
    /// <summary>
    /// Registra o sucesso de uma operação
    /// </summary>
    void OperationSuccess(string operationName, string details = null);
    
    /// <summary>
    /// Registra a falha de uma operação
    /// </summary>
    void OperationFailed(string operationName, Exception exception);
    
    /// <summary>
    /// Registra uma ação do usuário
    /// </summary>
    void UserAction(string action, string details = null);
    
    /// <summary>
    /// Obtém todos os logs
    /// </summary>
    string GetAllLogs();
    
    /// <summary>
    /// Limpa todos os logs
    /// </summary>
    void ClearLogs();
    
    /// <summary>
    /// Obtém a contagem atual de erros
    /// </summary>
    int GetErrorCount();
}
