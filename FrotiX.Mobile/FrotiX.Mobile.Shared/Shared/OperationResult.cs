namespace FrotiX.Mobile.Shared.Shared
{
    /// <summary>
    /// Resultado de operação simples (sem dados)
    /// </summary>
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Fail(string message) => new() { Success = false, Message = message };
    }

    /// <summary>
    /// Resultado de operação com dados genéricos
    /// </summary>
    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static OperationResult<T> Ok(T data) => new() 
        { 
            Success = true, 
            Data = data 
        };

        public static OperationResult<T> Fail(string message) => new() 
        { 
            Success = false, 
            Message = message 
        };
    }
}
