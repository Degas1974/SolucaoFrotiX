/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: TempDataExtensions.cs                                                                   ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Extensões para serialização/deserialização de objetos em TempData (JSON).             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 MÉTODOS: Put<T>(tempData, key, value), Get<T>(tempData, key)                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: Newtonsoft.Json, ITempDataDictionary                                                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;


namespace FrotiX.Models
    {

    public static class TempDataExtensions
        {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            {
            tempData[key] = JsonConvert.SerializeObject(value);
            }

        public static T Get<T>(this ITempDataDictionary tempData, string key)
            {
            if (tempData.TryGetValue(key, out object o))
                {
                return o == null ? default : JsonConvert.DeserializeObject<T>((string)o);
                }
            return default;
            }
        }
    }


