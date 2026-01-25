using System.Text;
using System.Text.Json;

namespace FrotiX.Mobile.Shared.Services
{
    /// <summary>
    /// Serviço para comunicação com a API FrotiX via Azure Relay
    /// </summary>
    public class RelayApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        // =====================================================
        // LOGGER ESTÁTICO - Configurado pelo MauiProgram
        // =====================================================
        private static Action<string>? _logInfo;
        private static Action<string, Exception?>? _logError;

        /// <summary>
        /// Configura o logger estático (chamar no MauiProgram)
        /// </summary>
        public static void ConfigurarLogger(Action<string> logInfo, Action<string, Exception?> logError)
        {
            _logInfo = logInfo;
            _logError = logError;
        }

        private static void LogInfo(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            _logInfo?.Invoke(message);
        }

        private static void LogError(string message, Exception? ex = null)
        {
            System.Diagnostics.Debug.WriteLine($"ERRO: {message} - {ex?.Message}");
            _logError?.Invoke(message, ex);
        }

        // =====================================================
        // CONFIGURAÇÃO - TOKEN SAS FIXO (VÁLIDO ATÉ 2075)
        // =====================================================
        
        private const string RelayNamespace = "frotix-relay.servicebus.windows.net";
        private const string HybridConnectionName = "frotix-bridge";
        
        // Token SAS com validade de 50 anos (expira em 07/12/2075)
        private const string SasToken = "SharedAccessSignature sr=http%3A%2F%2Ffrotix-relay.servicebus.windows.net%2Ffrotix-bridge%2F&sig=ETyV%2FW%2BVLIn%2B01vlcUpbDqnorM6u%2FUB1HQ6qtBgWZVc%3D&se=3341905373&skn=ListenSend";

        // =====================================================
        // Armazena último erro para diagnóstico
        // =====================================================
        public static string? UltimoErro { get; private set; }
        public static string? UltimaRespostaErro { get; private set; }

        // =====================================================

        public RelayApiService()
        {
            // Configurar handler para descompressão automática de GZIP/Deflate
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            };
            
            _httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(60) };
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        /// <summary>
        /// GET genérico
        /// </summary>
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Get, endpoint);
                var response = await _httpClient.SendAsync(request);
                
                responseBody = await response.Content.ReadAsStringAsync();
                LogInfo($"[API GET {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"GET {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API GET {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                }
                
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                UltimoErro = $"GET {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API GET {endpoint}] Exceção: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// POST genérico
        /// </summary>
        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Post, endpoint);
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                LogInfo($"[API POST {endpoint}] Enviando {jsonData.Length} bytes");
                
                var response = await _httpClient.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                
                LogInfo($"[API POST {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"POST {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API POST {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                }
                
                response.EnsureSuccessStatusCode();
                
                if (string.IsNullOrWhiteSpace(responseBody))
                    return default;
                    
                return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                UltimoErro = $"POST {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API POST {endpoint}] Exceção: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// POST que retorna apenas sucesso/falha
        /// </summary>
        public async Task<bool> PostAsync(string endpoint, object data)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Post, endpoint);
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                LogInfo($"[API POST {endpoint}] Enviando {jsonData.Length} bytes");
                
                var response = await _httpClient.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                
                LogInfo($"[API POST {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"POST {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API POST {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                UltimoErro = $"POST {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API POST {endpoint}] Exceção: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// PUT genérico
        /// </summary>
        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Put, endpoint);
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                LogInfo($"[API PUT {endpoint}] Enviando {jsonData.Length} bytes");
                
                var response = await _httpClient.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                
                LogInfo($"[API PUT {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"PUT {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API PUT {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                }
                
                response.EnsureSuccessStatusCode();
                
                if (string.IsNullOrWhiteSpace(responseBody))
                    return default;
                    
                return JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            }
            catch (Exception ex)
            {
                UltimoErro = $"PUT {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API PUT {endpoint}] Exceção: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// PUT que retorna apenas sucesso/falha
        /// </summary>
        public async Task<bool> PutAsync(string endpoint, object data)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Put, endpoint);
                var jsonData = JsonSerializer.Serialize(data, _jsonOptions);
                request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                
                LogInfo($"[API PUT {endpoint}] Enviando {jsonData.Length} bytes");
                
                var response = await _httpClient.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                
                LogInfo($"[API PUT {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"PUT {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API PUT {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                UltimoErro = $"PUT {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API PUT {endpoint}] Exceção: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// DELETE genérico
        /// </summary>
        public async Task<bool> DeleteAsync(string endpoint)
        {
            string? responseBody = null;
            try
            {
                var request = CriarRequest(HttpMethod.Delete, endpoint);
                var response = await _httpClient.SendAsync(request);
                responseBody = await response.Content.ReadAsStringAsync();
                
                LogInfo($"[API DELETE {endpoint}] Status: {(int)response.StatusCode} {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    UltimoErro = $"DELETE {endpoint} retornou {(int)response.StatusCode}";
                    UltimaRespostaErro = responseBody;
                    LogError($"[API DELETE {endpoint}] Erro HTTP {(int)response.StatusCode}: {responseBody}");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                UltimoErro = $"DELETE {endpoint}: {ex.Message}";
                UltimaRespostaErro = responseBody;
                LogError($"[API DELETE {endpoint}] Exceção: {ex.Message}", ex);
                return false;
            }
        }

        private HttpRequestMessage CriarRequest(HttpMethod method, string endpoint)
        {
            var uri = new Uri($"https://{RelayNamespace}/{HybridConnectionName}/{endpoint.TrimStart('/')}");
            
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Add("ServiceBusAuthorization", SasToken);
            
            return request;
        }
    }
}
