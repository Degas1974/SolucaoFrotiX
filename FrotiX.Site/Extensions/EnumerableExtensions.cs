/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Extensions/EnumerableExtensions.cs                             ║
 * ║  Descrição: Métodos de extensão para IEnumerable<T>, incluindo           ║
 * ║             verificações de nulidade (HasItems, IsNullOrEmpty),          ║
 * ║             conversão segura para lista e mapeamento JSON via            ║
 * ║             serialização/desserialização para transferência de dados.    ║
 * ║  Data: 28/01/2026 | LOTE: 21                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrotiX.Extensions
    {
    public static class EnumerableExtensions
        {
        [DebuggerStepThrough]
        public static bool HasItems<T>(this IEnumerable<T> source) => source != null && source.Any();

        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || source.Any() == false;

        [DebuggerStepThrough]
        public static List<T> ToSafeList<T>(this IEnumerable<T> source) => new List<T>(source);

        private static readonly JsonSerializerOptions DefaultSettings = SerializerSettings();

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

        [DebuggerStepThrough]
        private static string Serialize<TTarget>(this TTarget source) => JsonSerializer.Serialize(source, DefaultSettings);

        [DebuggerStepThrough]
        private static TTarget Deserialize<TTarget>(this string value) => JsonSerializer.Deserialize<TTarget>(value, DefaultSettings);

        [DebuggerStepThrough]
        public static TTarget MapTo<TTarget>(this object source) => source.MapTo<object, TTarget>();

        [DebuggerStepThrough]
        public static TTarget MapTo<TSource, TTarget>(this TSource source) => source.Serialize().Deserialize<TTarget>();

        [DebuggerStepThrough]
        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source) => source.Select(element => element.MapTo<TTarget>());
        }
    }


