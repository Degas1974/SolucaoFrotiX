// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : EvolutionApiWhatsAppService.cs                                  ║
// ║ LOCALIZAÇÃO: Services/WhatsApp/                                              ║
// ║ FINALIDADE : Implementação do IWhatsAppService usando Evolution API.         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Serviço HTTP para comunicação com WhatsApp via Evolution API:                ║
// ║ • StartSessionAsync: POST para iniciar/criar sessão (retorna "aguarde QR")   ║
// ║ • GetStatusAsync: GET para obter status da sessão, parseia JSON para         ║
// ║   extrair status (CONNECTED/QRCODE/DISCONNECTED) e QR code se disponível     ║
// ║ • GetQrBase64Async: GET para obter QR code em Base64 para pareamento         ║
// ║ • SendTextAsync: POST para enviar mensagem de texto ao número E.164          ║
// ║ • SendMediaAsync: POST para enviar mídia (imagem/arquivo) em Base64          ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DETALHES TÉCNICOS                                                            ║
// ║ • Usa HttpClient tipado injetado via DI                                      ║
// ║ • Configuração via IOptions<EvolutionApiOptions>                             ║
// ║ • Usa JsonContent.Create() ao invés de PostAsJsonAsync (evita ambiguidade)   ║
// ║ • Parsing de JSON com JsonDocument para extrair campos de resposta           ║
// ║ • Tratamento de erros via Alerta.TratamentoErroComLinha                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ REGISTRO NO DI (Program.cs ou Startup.cs)                                    ║
// ║ services.AddHttpClient<IWhatsAppService, EvolutionApiWhatsAppService>(c =>   ║
// ║   c.BaseAddress = new Uri(config["EvolutionApi:BaseUrl"]));                  ║
// ║ services.Configure<EvolutionApiOptions>(config.GetSection("EvolutionApi"));  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 22 — Services                                                  ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Services.WhatsApp;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Json; // JsonContent.Create(...)
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FrotiX.Services.WhatsApp
{
    public sealed class EvolutionApiWhatsAppService : IWhatsAppService
    {
        private readonly HttpClient _http;
        private readonly EvolutionApiOptions _opts;

        public EvolutionApiWhatsAppService(HttpClient http , IOptions<EvolutionApiOptions> opts)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _opts = opts?.Value ?? throw new ArgumentNullException(nameof(opts));
        }

        public async Task<ApiResult> StartSessionAsync(string session , CancellationToken ct = default)
        {
            try
            {
                var url = _opts.Resolve("StartSession");
                var payload = new { session = session ?? _opts.DefaultSession };

                // RESOLVE AMBIGUIDADE: NÃO usar PostAsJsonAsync
                var rsp = await _http.PostAsync(url , JsonContent.Create(payload) , ct);
                if (!rsp.IsSuccessStatusCode)
                    return ApiResult.Fail($"StartSession falhou: {(int)rsp.StatusCode} {rsp.ReasonPhrase}");

                return ApiResult.Ok("Sessão iniciada (aguarde QR).");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EvolutionApiWhatsAppService.cs" , "StartSessionAsync" , ex);
                return ApiResult.Fail(ex.Message);
            }
        }

        public async Task<SessionStatusDto> GetStatusAsync(string session , CancellationToken ct = default)
        {
            try
            {
                var url = _opts.Resolve("GetStatus" , session ?? _opts.DefaultSession);
                var rsp = await _http.GetAsync(url , ct);
                var txt = await rsp.Content.ReadAsStringAsync(ct);

                if (!rsp.IsSuccessStatusCode)
                    throw new InvalidOperationException($"Status falhou: {(int)rsp.StatusCode} {rsp.ReasonPhrase} - {txt}");

                using var doc = JsonDocument.Parse(txt);
                var root = doc.RootElement;

                var status = root.TryGetProperty("status" , out var s) ? s.GetString() : "UNKNOWN";
                var dto = new SessionStatusDto
                {
                    Session = session ?? _opts.DefaultSession ,
                    Status = status ,
                    UpdatedAt = DateTime.UtcNow
                };

                if (status?.Equals("QRCODE" , StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (root.TryGetProperty("qrcode" , out var qr))
                        dto.QrCodeBase64 = qr.GetString();
                    else if (root.TryGetProperty("base64" , out var b64))
                        dto.QrCodeBase64 = b64.GetString();
                }
                return dto;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EvolutionApiWhatsAppService.cs" , "GetStatusAsync" , ex);
                return new SessionStatusDto { Session = session ?? _opts.DefaultSession , Status = "ERROR" };
            }
        }

        public async Task<string> GetQrBase64Async(string session , CancellationToken ct = default)
        {
            try
            {
                var url = _opts.Resolve("GetQr" , session ?? _opts.DefaultSession);
                var rsp = await _http.GetAsync(url , ct);
                var txt = await rsp.Content.ReadAsStringAsync(ct);

                if (!rsp.IsSuccessStatusCode)
                    throw new InvalidOperationException($"GetQr falhou: {(int)rsp.StatusCode} {rsp.ReasonPhrase} - {txt}");

                using var doc = JsonDocument.Parse(txt);
                var root = doc.RootElement;
                if (root.TryGetProperty("qrcode" , out var qr))
                    return qr.GetString();
                if (root.TryGetProperty("base64" , out var b64))
                    return b64.GetString();

                // fallback: alguns retornam data:image/png;base64,xxx
                return txt;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EvolutionApiWhatsAppService.cs" , "GetQrBase64Async" , ex);
                return null;
            }
        }

        public async Task<ApiResult> SendTextAsync(string session , string phoneE164 , string message , CancellationToken ct = default)
        {
            try
            {
                var url = _opts.Resolve("SendText");
                var payload = new
                {
                    session = session ?? _opts.DefaultSession ,
                    number = phoneE164 ,
                    text = message
                };

                // RESOLVE AMBIGUIDADE: NÃO usar PostAsJsonAsync
                var rsp = await _http.PostAsync(url , JsonContent.Create(payload) , ct);
                var txt = await rsp.Content.ReadAsStringAsync(ct);
                if (!rsp.IsSuccessStatusCode)
                    return ApiResult.Fail($"SendText falhou: {(int)rsp.StatusCode} {rsp.ReasonPhrase} - {txt}");
                return ApiResult.Ok("Mensagem enviada.");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EvolutionApiWhatsAppService.cs" , "SendTextAsync" , ex);
                return ApiResult.Fail(ex.Message);
            }
        }

        public async Task<ApiResult> SendMediaAsync(string session , string phoneE164 , string fileName , string base64Data , string caption = null , CancellationToken ct = default)
        {
            try
            {
                var url = _opts.Resolve("SendMedia");
                var payload = new
                {
                    session = session ?? _opts.DefaultSession ,
                    number = phoneE164 ,
                    filename = fileName ,
                    caption = caption ,
                    base64 = base64Data // pode vir "data:*;base64,..." ou só base64
                };

                // RESOLVE AMBIGUIDADE: NÃO usar PostAsJsonAsync
                var rsp = await _http.PostAsync(url , JsonContent.Create(payload) , ct);
                var txt = await rsp.Content.ReadAsStringAsync(ct);
                if (!rsp.IsSuccessStatusCode)
                    return ApiResult.Fail($"SendMedia falhou: {(int)rsp.StatusCode} {rsp.ReasonPhrase} - {txt}");
                return ApiResult.Ok("Mídia enviada.");
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("EvolutionApiWhatsAppService.cs" , "SendMediaAsync" , ex);
                return ApiResult.Fail(ex.Message);
            }
        }
    }
}
