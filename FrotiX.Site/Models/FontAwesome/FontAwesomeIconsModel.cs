/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: FontAwesomeIconsModel.cs                                                                ║
    ║ 📂 CAMINHO: /Models/FontAwesome                                                                     ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Modelos para manipulação de ícones FontAwesome traduzidos (PT-BR).                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 CLASSES: FontAwesomeCategoryPT, FontAwesomeIconPT                                                 ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.Text.Json, System.Collections.Generic                                               ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrotiX.Models.FontAwesome
{
    /// <summary>
    /// Representa uma categoria de ícones FontAwesome traduzida para PT-BR
    /// </summary>
    public class FontAwesomeCategoryPT
    {
        [JsonPropertyName("categoria")]
        public string Categoria { get; set; }

        [JsonPropertyName("categoriaOriginal")]
        public string CategoriaOriginal { get; set; }

        [JsonPropertyName("icones")]
        public List<FontAwesomeIconPT> Icones { get; set; } = new();
    }

    /// <summary>
    /// Representa um ícone FontAwesome individual com tradução e keywords
    /// </summary>
    public class FontAwesomeIconPT
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; } = new();
    }

    /// <summary>
    /// Helper para carregar e desserializar fontawesome-icons.json (estrutura traduzida)
    /// </summary>
    internal static class FontAwesomeIconsLoader
    {
        private static JsonSerializerOptions SerializerSettings()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public static List<FontAwesomeCategoryPT> FromJson(string json)
        {
            return JsonSerializer.Deserialize<List<FontAwesomeCategoryPT>>(json, SerializerSettings())
                   ?? new List<FontAwesomeCategoryPT>();
        }
    }
}
