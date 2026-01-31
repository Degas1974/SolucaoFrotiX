/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: TempDataExtensions.cs                                                                   ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Serializar/deserializar objetos em TempData via JSON.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: TempDataExtensions                                                                      ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: Newtonsoft.Json, ITempDataDictionary                                               ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;


namespace FrotiX.Models
    {

    // ==================================================================================================
    // EXTENSIONS
    // ==================================================================================================
    // Extensões para armazenar objetos em TempData.
    // ==================================================================================================
    public static class TempDataExtensions
        {
        // Serializa e salva um objeto no TempData.
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            {
            tempData[key] = JsonConvert.SerializeObject(value);
            }

        // Recupera e desserializa um objeto do TempData.
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

