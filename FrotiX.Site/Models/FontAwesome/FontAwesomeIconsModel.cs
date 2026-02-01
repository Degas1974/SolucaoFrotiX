/* ****************************************************************************************
 * ⚡ ARQUIVO: FontAwesomeIconsModel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir modelos de ícones FontAwesome traduzidos (PT-BR).
 *
 * 📥 ENTRADAS     : JSON de categorias e ícones traduzidos.
 *
 * 📤 SAÍDAS       : Estruturas para pesquisa e exibição de ícones.
 *
 * 🔗 CHAMADA POR  : Rotinas de carregamento de ícones.
 *
 * 🔄 CHAMA        : System.Text.Json.
 *
 * 📦 DEPENDÊNCIAS : System.Text.Json, System.Collections.Generic.
 **************************************************************************************** */

using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrotiX.Models.FontAwesome
{
    /****************************************************************************************
     * ⚡ MODEL: FontAwesomeCategoryPT
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar categoria de ícones traduzida para PT-BR.
     *
     * 📥 ENTRADAS     : Categoria original e lista de ícones.
     *
     * 📤 SAÍDAS       : Categoria pronta para uso na UI.
     *
     * 🔗 CHAMADA POR  : FontAwesomeIconsLoader.
     *
     * 🔄 CHAMA        : FontAwesomeIconPT.
     ****************************************************************************************/
    public class FontAwesomeCategoryPT
    {
        // Nome traduzido da categoria.
        [JsonPropertyName("categoria")]
        public string Categoria { get; set; }

        // Nome original da categoria.
        [JsonPropertyName("categoriaOriginal")]
        public string CategoriaOriginal { get; set; }

        // Lista de ícones associados.
        [JsonPropertyName("icones")]
        public List<FontAwesomeIconPT> Icones { get; set; } = new();
    }

    /****************************************************************************************
     * ⚡ MODEL: FontAwesomeIconPT
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar ícone individual com tradução e keywords.
     *
     * 📥 ENTRADAS     : Id, nome, label e palavras-chave.
     *
     * 📤 SAÍDAS       : Ícone traduzido para busca e exibição.
     *
     * 🔗 CHAMADA POR  : FontAwesomeCategoryPT.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class FontAwesomeIconPT
    {
        // Identificador do ícone.
        [JsonPropertyName("id")]
        public string Id { get; set; }

        // Nome interno do ícone.
        [JsonPropertyName("name")]
        public string Name { get; set; }

        // Rótulo traduzido do ícone.
        [JsonPropertyName("label")]
        public string Label { get; set; }

        // Palavras-chave associadas.
        [JsonPropertyName("keywords")]
        public List<string> Keywords { get; set; } = new();
    }

    /****************************************************************************************
     * ⚡ HELPER: FontAwesomeIconsLoader
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Carregar e desserializar fontawesome-icons.json (PT-BR).
     *
     * 📥 ENTRADAS     : JSON com categorias e ícones.
     *
     * 📤 SAÍDAS       : Lista de categorias traduzidas.
     *
     * 🔗 CHAMADA POR  : Serviços de lookup de ícones.
     *
     * 🔄 CHAMA        : JsonSerializer.
     ****************************************************************************************/
    internal static class FontAwesomeIconsLoader
    {
        /****************************************************************************************
         * ⚡ MÉTODO: SerializerSettings
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Configurar opções de desserialização JSON.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JsonSerializerOptions configurado.
         *
         * 🔗 CHAMADA POR  : FromJson.
         *
         * 🔄 CHAMA        : JsonSerializerOptions.
         ****************************************************************************************/
        private static JsonSerializerOptions SerializerSettings()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        /****************************************************************************************
         * ⚡ MÉTODO: FromJson
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Desserializar JSON de ícones traduzidos.
         *
         * 📥 ENTRADAS     : json.
         *
         * 📤 SAÍDAS       : Lista de categorias traduzidas.
         *
         * 🔗 CHAMADA POR  : Serviços de carregamento.
         *
         * 🔄 CHAMA        : JsonSerializer.Deserialize.
         ****************************************************************************************/
        public static List<FontAwesomeCategoryPT> FromJson(string json)
        {
            return JsonSerializer.Deserialize<List<FontAwesomeCategoryPT>>(json, SerializerSettings())
                   ?? new List<FontAwesomeCategoryPT>();
        }
    }
}
