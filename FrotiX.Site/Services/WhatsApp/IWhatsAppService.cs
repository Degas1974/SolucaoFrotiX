// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : IWhatsAppService.cs                                             ║
// ║ LOCALIZAÇÃO: Services/WhatsApp/                                              ║
// ║ FINALIDADE : Interface do serviço de integração WhatsApp.                    ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • StartSessionAsync(session): Inicia/cria sessão WhatsApp                    ║
// ║ • GetStatusAsync(session): Obtém status (CONNECTED/QRCODE/DISCONNECTED)      ║
// ║ • GetQrBase64Async(session): Retorna QR Code em Base64 para pareamento       ║
// ║ • SendTextAsync(session, phone, message): Envia mensagem de texto            ║
// ║ • SendMediaAsync(session, phone, fileName, base64, caption): Envia mídia     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ IMPLEMENTAÇÃO                                                                ║
// ║ EvolutionApiWhatsAppService — implementa via Evolution API                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 22 — Services                                                  ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Threading;
using System.Threading.Tasks;

namespace FrotiX.Services.WhatsApp
{
    public interface IWhatsAppService
    {
        Task<ApiResult> StartSessionAsync(string session , CancellationToken ct = default);

        Task<SessionStatusDto> GetStatusAsync(string session , CancellationToken ct = default);

        Task<string> GetQrBase64Async(string session , CancellationToken ct = default);

        Task<ApiResult> SendTextAsync(string session , string phoneE164 , string message , CancellationToken ct = default);

        Task<ApiResult> SendMediaAsync(string session , string phoneE164 , string fileName , string base64Data , string caption = null , CancellationToken ct = default);
    }
}
