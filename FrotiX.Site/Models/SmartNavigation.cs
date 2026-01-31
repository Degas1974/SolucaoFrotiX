/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: SmartNavigation.cs                                                                      ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Construir navegação dinâmica a partir de JSON (nav.json).                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: NavigationBuilder, SmartNavigation, ListItem, Span, ItemType                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.Text.Json                                                                   ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace FrotiX.Models
    {
    // ==================================================================================================
    // BUILDER
    // ==================================================================================================
    // Fornece utilitários para construir navegação a partir de JSON.
    // ==================================================================================================
    internal static class NavigationBuilder
        {
        private static JsonSerializerOptions DefaultSettings => SerializerSettings();

        private static JsonSerializerOptions SerializerSettings(bool indented = true)
            {
            var options = new JsonSerializerOptions
                {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = indented,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return options;
            }

        // Desserializa o JSON de navegação.
        public static SmartNavigation FromJson(string json) => JsonSerializer.Deserialize<SmartNavigation>(json, DefaultSettings);
        }

    // ==================================================================================================
    // MODELO DE NAVEGAÇÃO
    // ==================================================================================================
    public sealed class SmartNavigation
        {
        public SmartNavigation()
            {
            }

        public SmartNavigation(IEnumerable<ListItem> items)
            {
            Lists = new List<ListItem>(items);
            }

        // Versão do modelo.
        public string Version { get; set; }
        // Lista principal de itens.
        public List<ListItem> Lists { get; set; } = new List<ListItem>();
        }

    // ==================================================================================================
    // ITEM DE MENU
    // ==================================================================================================
    public class ListItem
        {
        // Ícone principal.
        public string Icon { get; set; }
        // Indica se aparece na navegação inicial.
        public bool ShowOnSeed { get; set; } = true;
        // Nome do item pai.
        public string Parent { get; set; }
        // Título do item.
        public string Title { get; set; }
        // Texto exibido.
        public string Text { get; set; }
        // Nome do menu.
        public string NomeMenu { get; set; }
        // URL do item.
        public string Href { get; set; }
        // Tipo do item (categoria/filho/etc.).
        public ItemType Type { get; set; } = ItemType.Single;
        // Rota calculada.
        public string Route { get; set; }
        // Tags para busca.
        public string Tags { get; set; }
        // Chave de i18n.
        public string I18n { get; set; }
        // Indica se está desabilitado.
        public bool Disabled { get; set; }
        // Indica se possui filhos.
        public bool HasChild { get; set; }
        // Lista de filhos.
        public List<ListItem> Items { set; get; } = new List<ListItem>();
        // Span de apoio para UI.
        public Span Span { get; set; } = new Span();
        // Perfis/roles autorizados.
        public string[] Roles { get; set; }
        }

    // ==================================================================================================
    // SPAN
    // ==================================================================================================
    public sealed class Span
        {
        // Posição do span.
        public string Position { get; set; }
        // Classe CSS do span.
        public string Class { get; set; }
        // Texto do span.
        public string Text { get; set; }

        // Indica se algum valor foi preenchido.
        public bool HasValue() => (Position?.Length ?? 0) + (Class?.Length ?? 0) + (Text?.Length ?? 0) > 0;
        }

    // Tipos de itens de navegação.
    public enum ItemType
        {
        Category = 0,
        Single,
        Parent,
        Sibling,
        Child
        }
    }

