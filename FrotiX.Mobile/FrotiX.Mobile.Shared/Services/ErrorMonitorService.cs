using FrotiX.Mobile.Shared.Services.IServices;

namespace FrotiX.Mobile.Shared.Services;

/// <summary>
/// Serviço de monitoramento de erros em tempo real
/// Mantém contador de erros e notifica componentes sobre mudanças
/// </summary>
public class ErrorMonitorService
{
    private int _errorCount = 0;
    private readonly ILogService _logService;

    // Evento disparado quando o contador de erros muda
    public event Action? OnErrorCountChanged;

    public int ErrorCount => _errorCount;

    public ErrorMonitorService(ILogService logService)
    {
        _logService = logService;
        
        // Carrega contagem inicial de erros
        RefreshErrorCount();
        
        // Se o LogService suportar eventos, inscreve-se
        if (_logService is LogService logServiceImpl)
        {
            logServiceImpl.OnErrorOccurred += HandleNewError;
        }
    }

    private void HandleNewError(string errorMessage)
    {
        _errorCount++;
        NotifyErrorCountChanged();
    }

    public void RefreshErrorCount()
    {
        try
        {
            var logs = _logService.GetAllLogs();
            if (string.IsNullOrEmpty(logs))
            {
                _errorCount = 0;
            }
            else
            {
                // Conta quantas linhas começam com [ERROR
                _errorCount = logs.Split('\n')
                                 .Count(line => line.TrimStart().StartsWith("[ERROR"));
            }
            
            NotifyErrorCountChanged();
        }
        catch
        {
            _errorCount = 0;
        }
    }

    public void ClearErrors()
    {
        _errorCount = 0;
        NotifyErrorCountChanged();
    }

    private void NotifyErrorCountChanged()
    {
        OnErrorCountChanged?.Invoke();
    }
}
