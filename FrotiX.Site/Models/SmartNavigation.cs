/* ****************************************************************************************
 * ⚡ ARQUIVO: SmartNavigation.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir modelos e utilitários de navegação baseados em JSON (nav.json).
 *
 * 📥 ENTRADAS     : JSON de navegação e configurações de menu.
 *
 * 📤 SAÍDAS       : Estruturas de navegação para menus dinâmicos.
 *
 * 🔗 CHAMADA POR  : NavigationModel e componentes de menu.
 *
 * 🔄 CHAMA        : System.Text.Json.
 *
 * 📦 DEPENDÊNCIAS : System.Text.Json.
 **************************************************************************************** */

using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ BUILDER: NavigationBuilder
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fornecer utilitários para construir navegação a partir de JSON.
     *
     * 📥 ENTRADAS     : JSON de navegação.
     *
     * 📤 SAÍDAS       : Instâncias de SmartNavigation.
     *
     * 🔗 CHAMADA POR  : NavigationModel.
     *
     * 🔄 CHAMA        : JsonSerializer.
     ****************************************************************************************/
    internal static class NavigationBuilder
        {
        private static JsonSerializerOptions DefaultSettings => SerializerSettings();

        /****************************************************************************************
         * ⚡ MÉTODO: SerializerSettings
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Configurar opções padrão de serialização JSON.
         *
         * 📥 ENTRADAS     : indented (true para JSON formatado).
         *
         * 📤 SAÍDAS       : JsonSerializerOptions configurado.
         *
         * 🔗 CHAMADA POR  : DefaultSettings.
         *
         * 🔄 CHAMA        : JsonStringEnumConverter.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: FromJson
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Desserializar o JSON de navegação.
         *
         * 📥 ENTRADAS     : json (texto do nav.json).
         *
         * 📤 SAÍDAS       : SmartNavigation desserializado.
         *
         * 🔗 CHAMADA POR  : NavigationModel.
         *
         * 🔄 CHAMA        : JsonSerializer.Deserialize.
         ****************************************************************************************/
        public static SmartNavigation FromJson(string json) => JsonSerializer.Deserialize<SmartNavigation>(json, DefaultSettings);
        }

    /****************************************************************************************
     * ⚡ MODEL: SmartNavigation
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar a navegação principal da aplicação.
     *
     * 📥 ENTRADAS     : Lista de itens de menu.
     *
     * 📤 SAÍDAS       : Estrutura de navegação pronta para UI.
     *
     * 🔗 CHAMADA POR  : NavigationModel, layouts e componentes de menu.
     *
     * 🔄 CHAMA        : ListItem.
     ****************************************************************************************/
    public sealed class SmartNavigation
        {
        /****************************************************************************************
         * ⚡ CONSTRUTOR: SmartNavigation
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar navegação vazia.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : Instância com listas padrão.
         *
         * 🔗 CHAMADA POR  : Desserialização JSON.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
        public SmartNavigation()
            {
            }

        /****************************************************************************************
         * ⚡ CONSTRUTOR: SmartNavigation
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Inicializar navegação com lista de itens.
         *
         * 📥 ENTRADAS     : items.
         *
         * 📤 SAÍDAS       : Instância com listas preenchidas.
         *
         * 🔗 CHAMADA POR  : NavigationModel.
         *
         * 🔄 CHAMA        : List.
         ****************************************************************************************/
        public SmartNavigation(IEnumerable<ListItem> items)
            {
            Lists = new List<ListItem>(items);
            }

        // Versão do modelo.
        public string Version { get; set; }
        // Lista principal de itens.
        public List<ListItem> Lists { get; set; } = new List<ListItem>();
        }

    /****************************************************************************************
     * ⚡ MODEL: ListItem
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar item individual de menu.
     *
     * 📥 ENTRADAS     : Propriedades do item (texto, ícone, rota).
     *
     * 📤 SAÍDAS       : Item utilizado na renderização de menus.
     *
     * 🔗 CHAMADA POR  : SmartNavigation e NavigationModel.
     *
     * 🔄 CHAMA        : ItemType, Span.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: Span
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar informações auxiliares de UI para itens do menu.
     *
     * 📥 ENTRADAS     : Position, Class e Text.
     *
     * 📤 SAÍDAS       : Metadados para renderização de tags auxiliares.
     *
     * 🔗 CHAMADA POR  : ListItem.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public sealed class Span
        {
        // Posição do span.
        public string Position { get; set; }
        // Classe CSS do span.
        public string Class { get; set; }
        // Texto do span.
        public string Text { get; set; }

        /****************************************************************************************
         * ⚡ MÉTODO: HasValue
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Indicar se o span possui conteúdo definido.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : true quando Position/Class/Text possuem conteúdo.
         *
         * 🔗 CHAMADA POR  : Renderização de UI.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
        public bool HasValue() => (Position?.Length ?? 0) + (Class?.Length ?? 0) + (Text?.Length ?? 0) > 0;
        }

    /****************************************************************************************
     * ⚡ ENUM: ItemType
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Tipificar itens de navegação (categoria, pai, filho, etc.).
     *
     * 📥 ENTRADAS     : Definidas no JSON de navegação.
     *
     * 📤 SAÍDAS       : Enum de tipos de item.
     *
     * 🔗 CHAMADA POR  : ListItem.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public enum ItemType
        {
        Category = 0,
        Single,
        Parent,
        Sibling,
        Child
        }
    }
