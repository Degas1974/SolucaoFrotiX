// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: TempDataExtensions.cs                                              ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Métodos de extensão para ITempDataDictionary.                               ║
// ║ Permite armazenar e recuperar objetos complexos em TempData.                ║
// ║                                                                              ║
// ║ MÉTODOS:                                                                     ║
// ║ - Put<T>(key, value): Serializa objeto para JSON e armazena                 ║
// ║ - Get<T>(key): Recupera e deserializa objeto do TempData                    ║
// ║                                                                              ║
// ║ SERIALIZAÇÃO: Newtonsoft.Json (JsonConvert)                                 ║
// ║                                                                              ║
// ║ USO: TempData.Put("Toast", toastObj); var t = TempData.Get<Toast>("Toast"); ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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


