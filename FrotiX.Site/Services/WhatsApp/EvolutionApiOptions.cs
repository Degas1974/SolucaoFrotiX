// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ ARQUIVO    : EvolutionApiOptions.cs                                          ║
// ║ LOCALIZAÇÃO: Services/WhatsApp/                                              ║
// ║ FINALIDADE : Configuração de conexão com Evolution API para WhatsApp.        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO FUNCIONAL                                                          ║
// ║ Classe de configuração bindável via appsettings.json seção "EvolutionApi":   ║
// ║ • Provider: Identificador do provedor ("EvolutionApi")                       ║
// ║ • BaseUrl: URL base do servidor Evolution (ex: https://api.evolution.io)     ║
// ║ • ApiKey: Chave de autenticação da API                                       ║
// ║ • DefaultSession: Nome da sessão padrão ("FrotiX")                           ║
// ║ • Endpoints: Dicionário com rotas da API (StartSession, GetQr, GetStatus,    ║
// ║   SendText, SendMedia) com placeholder {session}                             ║
// ║ • Resolve(key, session): Substitui {session} e retorna endpoint completo     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ EXEMPLO APPSETTINGS.JSON                                                     ║
// ║ "EvolutionApi": {                                                            ║
// ║   "BaseUrl": "https://sua-evolution-api.com",                                ║
// ║   "ApiKey": "sua-chave-aqui",                                                ║
// ║   "DefaultSession": "FrotiX"                                                 ║
// ║ }                                                                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ LOTE        : 22 — Services                                                  ║
// ║ DATA        : 29/01/2026                                                     ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Collections.Generic;

namespace FrotiX.Services.WhatsApp
{
    public sealed class EvolutionApiOptions
    {
        public string Provider { get; set; } = "EvolutionApi";
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string DefaultSession { get; set; } = "FrotiX";

        public Dictionary<string , string> Endpoints { get; set; } = new()
        {
            ["StartSession"] = "/session/start" ,
            ["GetQr"] = "/session/qr/{session}" ,
            ["GetStatus"] = "/session/status/{session}" ,
            ["SendText"] = "/message/sendText" ,
            ["SendMedia"] = "/message/sendMedia"
        };

        public string Resolve(string key , string session = null)
        {
            if (!Endpoints.TryGetValue(key , out var path) || string.IsNullOrWhiteSpace(path))
                throw new System.InvalidOperationException($"Endpoint '{key}' não configurado.");
            return path.Replace("{session}" , session ?? DefaultSession);
        }
    }
}
