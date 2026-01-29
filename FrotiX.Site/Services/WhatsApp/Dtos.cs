// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : Dtos.cs                                                         ║
// ║ LOCALIZAÇÃO: Services/WhatsApp/                                              ║
// ║ FINALIDADE : DTOs para integração com WhatsApp via Evolution API.            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Define as classes de transferência de dados para comunicação WhatsApp:       ║
// ║ • StartSessionRequest: Identificador da sessão a iniciar                     ║
// ║ • SendTextRequest: Mensagem de texto (Session, PhoneE164, Message)           ║
// ║ • SendMediaRequest: Envio de mídia (FileName, Caption, Base64Data)           ║
// ║ • ApiResult: Resultado genérico com Success/Message e métodos Ok/Fail        ║
// ║ • SessionStatusDto: Status da sessão (CONNECTED, QRCODE, DISCONNECTED)       ║
// ║   com QrCodeBase64 para exibir QR de pareamento                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FORMATO DO TELEFONE                                                          ║
// ║ PhoneE164 no formato internacional: 5591988887777 (sem +, sem espaços)       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 22 — Services                                                  ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;

namespace FrotiX.Services.WhatsApp
{
    public sealed class StartSessionRequest
    {
        public string Session { get; set; }
    }

    public sealed class SendTextRequest
    {
        public string Session { get; set; }
        public string PhoneE164 { get; set; } // ex.: 5591988887777
        public string Message { get; set; }
    }

    public sealed class SendMediaRequest
    {
        public string Session { get; set; }
        public string PhoneE164 { get; set; }
        public string FileName { get; set; }  // ex.: foto.jpg
        public string Caption { get; set; }
        public string Base64Data { get; set; } // "data:image/jpeg;base64,...." ou apenas base64
    }

    public sealed class ApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ApiResult Ok(string msg = null) => new() { Success = true , Message = msg };

        public static ApiResult Fail(string msg) => new() { Success = false , Message = msg };
    }

    public sealed class SessionStatusDto
    {
        public string Session { get; set; }
        public string Status { get; set; } // ex.: "CONNECTED", "QRCODE", "DISCONNECTED"
        public DateTime? UpdatedAt { get; set; }
        public string QrCodeBase64 { get; set; } // se status == QRCODE
    }
}
