/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: EvolutionApiOptions.cs                                                                  ║
   ║ 📂 CAMINHO: /Services/WhatsApp                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Configuração de conexão com Evolution API para WhatsApp. Bindável via appsettings.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Provider, BaseUrl, ApiKey, DefaultSession, Endpoints (dicionário), Resolve()             ║
   ║ 🔗 DEPS: appsettings.json seção "EvolutionApi" | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

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
