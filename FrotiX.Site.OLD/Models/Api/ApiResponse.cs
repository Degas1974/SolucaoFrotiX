/* ****************************************************************************************
 * ⚡ ARQUIVO: ApiResponse.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Padronizar respostas da API e facilitar tratamento de erros no frontend.
 *
 * 📥 ENTRADAS     : Dados, mensagens, detalhes de erro e requestId.
 *
 * 📤 SAÍDAS       : JSON com { success, data, message, requestId, errorDetails }.
 *
 * 🔗 CHAMADA POR  : Controllers API.
 *
 * 🔄 CHAMA        : Guid, Exception.
 *
 * 📦 DEPENDÊNCIAS : System.
 **************************************************************************************** */

using System;

namespace FrotiX.Models.Api
{
    /****************************************************************************************
     * ⚡ MODEL: ApiResponse<T>
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Padronizar respostas da API com payload tipado.
     *
     * 📥 ENTRADAS     : Dados, mensagem, requestId e detalhes de erro.
     *
     * 📤 SAÍDAS       : Resposta consistente para consumo pelo frontend.
     *
     * 🔗 CHAMADA POR  : Controllers e serviços de API.
     *
     * 🔄 CHAMA        : ApiErrorDetails.
     ****************************************************************************************/
    public class ApiResponse<T>
    {
        // Indica se a operação foi bem sucedida.
        public bool Success { get; set; }

        // Dados retornados pela operação.
        public T? Data { get; set; }

        // Mensagem descritiva (sucesso ou erro).
        public string Message { get; set; } = string.Empty;

        // ID único da requisição para rastreamento.
        public string? RequestId { get; set; }

        // Detalhes do erro (apenas em ambiente de desenvolvimento).
        public ApiErrorDetails? ErrorDetails { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: Ok
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar resposta de sucesso.
         *
         * 📥 ENTRADAS     : data e message (opcional).
         *
         * 📤 SAÍDAS       : ApiResponse<T> com Success=true.
         *
         * 🔗 CHAMADA POR  : Controllers API.
         *
         * 🔄 CHAMA        : GenerateRequestId.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: Error
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar resposta de erro.
         *
         * 📥 ENTRADAS     : message e details (opcional).
         *
         * 📤 SAÍDAS       : ApiResponse<T> com Success=false.
         *
         * 🔗 CHAMADA POR  : Controllers API.
         *
         * 🔄 CHAMA        : GenerateRequestId.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: FromException
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar resposta de erro a partir de uma exceção.
         *
         * 📥 ENTRADAS     : ex e includeDetails.
         *
         * 📤 SAÍDAS       : ApiResponse<T> com mensagem padrão e detalhes opcionais.
         *
         * 🔗 CHAMADA POR  : Camadas de tratamento de exceções.
         *
         * 🔄 CHAMA        : GenerateRequestId.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: GenerateRequestId
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Gerar identificador curto para rastreamento.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : String com 8 caracteres.
         *
         * 🔗 CHAMADA POR  : Métodos Ok, Error e FromException.
         *
         * 🔄 CHAMA        : Guid.
         ****************************************************************************************/
        private static string GenerateRequestId()
        {
            return Guid.NewGuid().ToString("N")[..8];
        }
    }

    /****************************************************************************************
     * ⚡ MODEL: ApiResponse
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Padronizar respostas da API sem payload.
     *
     * 📥 ENTRADAS     : message e detalhes de erro.
     *
     * 📤 SAÍDAS       : ApiResponse sem dados.
     *
     * 🔗 CHAMADA POR  : Controllers API.
     *
     * 🔄 CHAMA        : ApiErrorDetails.
     ****************************************************************************************/
    public class ApiResponse : ApiResponse<object>
    {
        /****************************************************************************************
         * ⚡ MÉTODO: CreateSuccess
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar resposta de sucesso sem dados.
         *
         * 📥 ENTRADAS     : message (opcional).
         *
         * 📤 SAÍDAS       : ApiResponse com Success=true.
         *
         * 🔗 CHAMADA POR  : Controllers API.
         *
         * 🔄 CHAMA        : Guid.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: CreateError
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Criar resposta de erro sem dados.
         *
         * 📥 ENTRADAS     : message e details (opcional).
         *
         * 📤 SAÍDAS       : ApiResponse com Success=false.
         *
         * 🔗 CHAMADA POR  : Controllers API.
         *
         * 🔄 CHAMA        : Guid.
         ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: ApiErrorDetails
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Detalhar exceções em respostas de erro (debug/desenvolvimento).
     *
     * 📥 ENTRADAS     : Dados técnicos da exceção.
     *
     * 📤 SAÍDAS       : Payload de diagnóstico.
     *
     * 🔗 CHAMADA POR  : ApiResponse.FromException.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ApiErrorDetails
    {
        // ID da requisição.
        public string? RequestId { get; set; }

        // Data/hora do erro.
        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Tipo da exceção.
        public string? ExceptionType { get; set; }

        // Mensagem da exceção.
        public string? ExceptionMessage { get; set; }

        // Stack trace (primeiras linhas).
        public string[]? StackTrace { get; set; }

        // Arquivo onde ocorreu o erro.
        public string? Arquivo { get; set; }

        // Método onde ocorreu o erro.
        public string? Metodo { get; set; }

        // Linha do erro.
        public int? Linha { get; set; }
    }
}
