// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EnumerableExtensions.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Extension methods para IEnumerable e serialização JSON.                      ║
// ║ Utilitários genéricos usados em todo o sistema.                              ║
// ║                                                                              ║
// ║ MÉTODOS DISPONÍVEIS:                                                         ║
// ║ - HasItems<T>()     → Verifica se coleção não é nula e tem elementos         ║
// ║ - IsNullOrEmpty<T>()→ Verifica se coleção é nula ou vazia                    ║
// ║ - ToSafeList<T>()   → Converte para List<T> de forma segura                  ║
// ║ - MapTo<T>()        → Mapeia objeto para outro tipo via JSON (deep clone)    ║
// ║ - MapTo<S,T>()      → Mapeia objeto tipado para outro tipo                   ║
// ║ - MapTo<S,T>(IEnum) → Mapeia coleção para outra coleção de tipo diferente    ║
// ║                                                                              ║
// ║ CONFIGURAÇÃO JSON:                                                           ║
// ║ - Ignora nulos (JsonIgnoreCondition.WhenWritingNull)                         ║
// ║ - Formatação indentada para legibilidade                                     ║
// ║ - CamelCase para propriedades                                                ║
// ║ - Enums como strings                                                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FrotiX.Extensions
    {
    /// <summary>
    /// Extensions para IEnumerable e serialização JSON.
    /// Inclui HasItems, IsNullOrEmpty, ToSafeList e MapTo.
    /// </summary>
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


