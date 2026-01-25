using FrotiX.Services.WhatsApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using FrotiX.Helpers;
using FrotiX.Services;

namespace FrotiX.Controllers.Api
{
    /*
    *  #################################################################################################
    *  #                                                                                               #
    *  #   ███████╗██████╗  ██████╗ ████████╗██╗██╗  ██╗    ██████╗  ██████╗ ██████╗  ██████╗          #
    *  #   ██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝██║╚██╗██╔╝    ╚════██╗██╔═████╗╚════██╗██╔════╝          #
    *  #   █████╗  ██████╔╝██║   ██║   ██║   ██║ ╚███╔╝      █████╔╝██║██╔██║ █████╔╝███████╗          #
    *  #   ██╔══╝  ██╔══██╗██║   ██║   ██║   ██║ ██╔██╗     ██╔═══╝ ████╔╝██║██╔═══╝ ██╔═══██╗          #
    *  #   ██║     ██║  ██║╚██████╔╝   ██║   ██║██╔╝ ██╗    ███████╗╚██████╔╝███████╗╚██████╔╝          #
    *  #   ╚═╝     ╚═╝  ╚═╝ ╚═════╝    ╚═╝   ╚═╝╚═╝  ╚═╝    ╚══════╝ ╚═════╝ ╚══════╝ ╚═════╝           #
    *  #                                                                                               #
    *  #   PROJETO: FROTIX - SOLUÇÃO INTEGRADA DE GESTÃO DE FROTAS                                     #
    *  #   MODULO:  WHATSAPP API (NOTIFICAÇÕES & INTEGRAÇÃO)                                           #
    *  #   DATA:    2026 (Modernização FrotiX 2026)                                                   #
    *  #                                                                                               #
    *  #################################################################################################
    */

    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    /// <para>CLASSE: <c>WhatsAppController</c></para>
    /// <para>DESCRIÇÃO: Interface API para orquestração de mensagens via WhatsApp (Baileys/Evolution).</para>
    /// <para>PADRÃO: FrotiX 2026 - (IA) Documented & Modernized </para>
    /// <para>────────────────────────────────────────────────────────────────────────────────────────────</para>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppService _wa;
        private readonly ILogger<WhatsAppController> _logger;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: WhatsAppController (Constructor)                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de mensageria com serviço de WhatsApp e logger. ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Habilita integrações de notificação e comunicação externa.                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • wa (IWhatsAppService): serviço de integração WhatsApp.                  ║
        /// ║    • logger (ILogger<WhatsAppController>): logger local.                     ║
        /// ║    • log (ILogService): log centralizado.                                    ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • Tipo: N/A                                                               ║
        /// ║    • Significado: N/A                                                        ║
        /// ║    • Consumidor: runtime do ASP.NET Core.                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • Injeção de dependência ao instanciar o controller.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: INTERNA ao módulo                                                 ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public WhatsAppController(IWhatsAppService wa, ILogger<WhatsAppController> logger, ILogService log)
        {
            try
            {
                _wa = wa;
                _logger = logger;
                _log = log;
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Constructor", ex);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Start                                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicia uma sessão do WhatsApp para integração de mensagens.               ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite autenticação e ativação do canal de comunicação.                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • req (StartSessionRequest): dados da sessão.                             ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status da sessão.                               ║
        /// ║    • Consumidor: UI de integrações.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _wa.StartSessionAsync() → inicia sessão.                                ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/WhatsApp/start                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Integração WhatsApp                                     ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartSessionRequest req, CancellationToken ct)
        {
            try
            {
                // [DADOS] Normaliza sessão
                var session = string.IsNullOrWhiteSpace(req?.Session) ? null : req.Session.Trim();
                // [AJAX] Chamada ao serviço de integração
                var r = await _wa.StartSessionAsync(session, ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Start", ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Status                                                              ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Verifica o status de uma sessão WhatsApp.                                 ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite monitorar conectividade da integração.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • session (string): nome da sessão.                                       ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status (CONNECTED, DISCONNECTED etc).           ║
        /// ║    • Consumidor: UI de integrações.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _wa.GetStatusAsync() → status da sessão.                                ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/WhatsApp/status                                                ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Integração WhatsApp                                     ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("status")]
        public async Task<IActionResult> Status([FromQuery] string session, CancellationToken ct)
        {
            try
            {
                // [AJAX] Consulta status da sessão
                var r = await _wa.GetStatusAsync(session, ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Status", ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Qr                                                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Obtém o QR Code para autenticação da sessão WhatsApp.                     ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Necessário para autenticar novas sessões.                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • session (string): nome da sessão.                                       ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com QR Code em Base64.                              ║
        /// ║    • Consumidor: UI de integrações.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _wa.GetQrBase64Async() → QR code da sessão.                             ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • GET /api/WhatsApp/qr                                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Integração WhatsApp                                     ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("qr")]
        public async Task<IActionResult> Qr([FromQuery] string session, CancellationToken ct)
        {
            try
            {
                // [AJAX] Solicita QR Code
                var b64 = await _wa.GetQrBase64Async(session, ct);
                if (string.IsNullOrWhiteSpace(b64))
                    return NotFound(new { success = false, message = "QR não disponível." });

                // [REGRA] Garante prefixo data URI se necessário
                if (!b64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                    b64 = "data:image/png;base64," + b64;

                return Ok(new { success = true, qrcode = b64 });
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "Qr", ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SendText                                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Envia mensagem de texto para um destinatário via WhatsApp.                ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Permite comunicação automatizada com usuários.                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • req (SendTextRequest): dados da mensagem.                               ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com sucesso ou falha.                               ║
        /// ║    • Consumidor: UI de integrações.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _wa.SendTextAsync() → envio de texto.                                   ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/WhatsApp/send-text                                            ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Integração WhatsApp                                     ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("send-text")]
        public async Task<IActionResult> SendText([FromBody] SendTextRequest req, CancellationToken ct)
        {
            try
            {
                // [AJAX] Envio de mensagem de texto
                var r = await _wa.SendTextAsync(req.Session, req.PhoneE164, req.Message, ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendText", ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: SendMedia                                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Envia mídia (imagem, PDF, etc.) via WhatsApp.                             ║
        /// ║                                                                              ║
        /// ║ 🎯 IMPORTÂNCIA PARA A SOLUÇÃO:                                              ║
        /// ║    Suporta envio de documentos e anexos automatizados.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • req (SendMediaRequest): dados da mídia.                                 ║
        /// ║    • ct (CancellationToken): cancelamento.                                  ║
        /// ║                                                                              ║
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com sucesso ou falha.                               ║
        /// ║    • Consumidor: UI de integrações.                                          ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📞 FUNÇÕES QUE CHAMA:                                                        ║
        /// ║    • _wa.SendMediaAsync() → envio de mídia.                                  ║
        /// ║    • Alerta.TratamentoErroComLinha() → tratamento de erro.                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📲 CHAMADA POR:                                                              ║
        /// ║    • POST /api/WhatsApp/send-media                                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 🔗 ESCOPO: EXTERNA - Integração WhatsApp                                     ║
        /// ║    • Arquivos relacionados: Services/WhatsApp                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("send-media")]
        public async Task<IActionResult> SendMedia([FromBody] SendMediaRequest req, CancellationToken ct)
        {
            try
            {
                // [AJAX] Envio de mídia
                var r = await _wa.SendMediaAsync(req.Session, req.PhoneE164, req.FileName, req.Base64Data, req.Caption, ct);
                return Ok(r);
            }
            catch (Exception ex)
            {
                Alerta.TratamentoErroComLinha("WhatsAppController.cs", "SendMedia", ex);
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
