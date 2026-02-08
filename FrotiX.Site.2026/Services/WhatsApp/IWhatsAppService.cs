/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IWhatsAppService.cs                                                                     ║
   ║ 📂 CAMINHO: /Services/WhatsApp                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Interface do serviço de integração WhatsApp. Sessões, QR code, texto, mídia.           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: StartSessionAsync(), GetStatusAsync(), GetQrBase64Async(), SendTextAsync(), SendMedia()  ║
   ║ 🔗 DEPS: Impl: EvolutionApiWhatsAppService | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                   ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
