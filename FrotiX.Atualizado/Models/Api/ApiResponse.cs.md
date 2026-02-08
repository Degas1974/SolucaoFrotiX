# Models/Api/ApiResponse.cs

**ARQUIVO NOVO** | 93 linhas de codigo

> Copiar integralmente para o Janeiro.

---

```csharp
using System;

namespace FrotiX.Models.Api
{

    public class ApiResponse<T>
    {

        public bool Success { get; set; }

        public T? Data { get; set; }

        public string Message { get; set; } = string.Empty;

        public string? RequestId { get; set; }

        public ApiErrorDetails? ErrorDetails { get; set; }

        public static ApiResponse<T> Ok(T data, string message = "Operação realizada com sucesso")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message,
                RequestId = GenerateRequestId()
            };
        }

        public static ApiResponse<T> Error(string message, ApiErrorDetails? details = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = message,
                RequestId = GenerateRequestId(),
                ErrorDetails = details
            };
        }

        public static ApiResponse<T> FromException(Exception ex, bool includeDetails = false)
        {
            var response = new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Message = "Ocorreu um erro ao processar a requisição",
                RequestId = GenerateRequestId()
            };

            if (includeDetails)
            {
                response.ErrorDetails = new ApiErrorDetails
                {
                    ExceptionType = ex.GetType().Name,
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace?.Split('\n', StringSplitOptions.RemoveEmptyEntries),
                    Timestamp = DateTime.Now
                };
            }

            return response;
        }

        private static string GenerateRequestId()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }
    }

    public class ApiResponse : ApiResponse<object>
    {

        public static ApiResponse CreateSuccess(string message = "Operação realizada com sucesso")
        {
            return new ApiResponse
            {
                Success = true,
                Data = null,
                Message = message,
                RequestId = Guid.NewGuid().ToString("N")[..8]
            };
        }

        public static ApiResponse CreateError(string message, ApiErrorDetails? details = null)
        {
            return new ApiResponse
            {
                Success = false,
                Data = null,
                Message = message,
                RequestId = Guid.NewGuid().ToString("N")[..8],
                ErrorDetails = details
            };
        }
    }

    public class ApiErrorDetails
    {

        public string? RequestId { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string? ExceptionType { get; set; }

        public string? ExceptionMessage { get; set; }

        public string[]? StackTrace { get; set; }

        public string? Arquivo { get; set; }

        public string? Metodo { get; set; }

        public int? Linha { get; set; }
    }
}
```
