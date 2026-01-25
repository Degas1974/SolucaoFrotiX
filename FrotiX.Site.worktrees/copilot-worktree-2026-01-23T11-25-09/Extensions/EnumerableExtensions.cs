/*
 * =========================================================================================
 * SISTEMA FROTIX 2026 - SOLUÇÃO DE GESTÃO DE FROTAS
 * =========================================================================================
 * Desenvolvido por: Agente IA (GitHub Copilot)
 * Data de Criação/Atualização: 2026
 * Tecnologias: .NET 10 (Preview), C#, Entity Framework Core
 * 
 * Descrição do Arquivo:
 * Métodos de extensão para tipos enumerable e serialização JSON.
 * =========================================================================================
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using FrotiX.Helpers;

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
        public static IEnumerable<TTarget> MapTo<TSource, TTarget>(this IEnumerable<TSource> source) 
        {
            try
            {
                if (source == null) return new List<TTarget>();
                return source.Select(element => element.MapTo<TTarget>());
            }
            catch (Exception ex)
            {
                // Em extensões genéricas, o tratamento de erro com Alerta pode ser problemático se não houver contexto web.
                // Mas seguindo a regra Zero Tolerance, vamos pelo menos encapsular.
                // Se disparar erro aqui, provavelmente é erro de serialização.
                 Alerta.TratamentoErroComLinha("EnumerableExtensions.cs", "MapTo", ex);
                 throw;
            }
        }
    }
}


