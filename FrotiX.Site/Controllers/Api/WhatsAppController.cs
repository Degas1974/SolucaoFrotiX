/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: WhatsAppController.cs                                                                   ║
   ║ 📂 CAMINHO: /Controllers/Api                                                                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Gerenciar integração com WhatsApp Web para envio de mensagens. Controla sessões,      ║
   ║    exibe QR Code para autenticação e envia mensagens para destinatários.                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENDPOINTS: [POST] /start → Iniciar sessão | [GET] /status → Status sessão                       ║
   ║    [POST] /send → Enviar mensagem | ROTA BASE: api/WhatsApp                                        ║
   ║    ATRIBUTO: [Authorize] - Requer autenticação                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: IWhatsAppService, CancellationToken (async)                                                ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Services.WhatsApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FrotiX.Controllers.Api
{
    /****************************************************************************************
     * ⚡ CONTROLLER: WhatsAppController
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Gerenciar integração com WhatsApp Web para envio de mensagens
     * 📥 ENTRADAS     : StartSessionRequest (sessão), Mensagens (destinatário, texto)
     * 📤 SAÍDAS       : JSON com status da sessão, QR Code, confirmação de envio
     * 🔗 CHAMADA POR  : Frontend de notificações e comunicações
     * 🔄 CHAMA        : IWhatsAppService (serviço de integração WhatsApp)
     * 📦 DEPENDÊNCIAS : IWhatsAppService, CancellationToken (async)
     * --------------------------------------------------------------------------------------
     * [DOC] API REST para controle de sessões WhatsApp Web
     * [DOC] Endpoints: Start (iniciar sessão), Status (verificar status), Send (enviar msg)
     * [DOC] Usa CancellationToken para operações assíncronas que podem ser canceladas
     * [DOC] Requer autorização para todos os endpoints
     ****************************************************************************************/
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _wa;

        public WhatsAppController(IWhatsAppService wa)
        {
            _wa = wa;
        }

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartSessionRequest req , CancellationToken ct)
        {
            try
            {
                var session = string.IsNullOrWhiteSpace(req?.Session) ? null : req.Session.Trim();
                var r = await _wa.StartSessionAsync(session , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Start" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> Status([FromQuery] string session , CancellationToken ct)
        {
            try
            {
                var r = await _wa.GetStatusAsync(session , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Status" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }

        [HttpGet("qr")]
        public async Task<IActionResult> Qr([FromQuery] string session , CancellationToken ct)
        {
            try
            {
                var b64 = await _wa.GetQrBase64Async(session , ct);
                if (string.IsNullOrWhiteSpace(b64))
                    return NotFound(new { success = false , message = "QR não disponível." });

                // Se vier só o base64, garanta o prefixo data URI
                if (!b64.StartsWith("data:" , StringComparison.OrdinalIgnoreCase))
                    b64 = "data:image/png;base64," + b64;

                return Ok(new { success = true , qrcode = b64 });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "Qr" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }

        [HttpPost("send-text")]
        public async Task<IActionResult> SendText([FromBody] SendTextRequest req , CancellationToken ct)
        {
            try
            {
                var r = await _wa.SendTextAsync(req.Session , req.PhoneE164 , req.Message , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendText" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }

        [HttpPost("send-media")]
        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req , CancellationToken ct)
        {
            try
            {
                var r = await _wa.SendMediaAsync(req.Session , req.PhoneE164 , req.FileName , req.Base64Data , req.Caption , ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs" , "SendMedia" , ex);
                return BadRequest(new { success = false , message = ex.Message });
            }
        }
    }
}
