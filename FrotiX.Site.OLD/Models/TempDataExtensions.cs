/* ****************************************************************************************
 * ⚡ ARQUIVO: TempDataExtensions.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Serializar e desserializar objetos em TempData via JSON.
 *
 * 📥 ENTRADAS     : Objetos e chaves de armazenamento.
 *
 * 📤 SAÍDAS       : Objetos recuperados do TempData.
 *
 * 🔗 CHAMADA POR  : Controllers e Pages com TempData.
 *
 * 🔄 CHAMA        : JsonConvert, ITempDataDictionary.
 *
 * 📦 DEPENDÊNCIAS : Newtonsoft.Json, Microsoft.AspNetCore.Mvc.ViewFeatures.
 **************************************************************************************** */

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;


namespace FrotiX.Models
    {

    /****************************************************************************************
     * ⚡ EXTENSIONS: TempDataExtensions
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Estender TempData com serialização JSON.
     *
     * 📥 ENTRADAS     : Chave e valor a armazenar.
     *
     * 📤 SAÍDAS       : Objetos recuperados por chave.
     *
     * 🔗 CHAMADA POR  : Controllers/Pages.
     *
     * 🔄 CHAMA        : JsonConvert.
     ****************************************************************************************/
    public static class TempDataExtensions
        {
        /****************************************************************************************
         * ⚡ MÉTODO: Put
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Serializar e salvar um objeto no TempData.
         *
         * 📥 ENTRADAS     : key e value.
         *
         * 📤 SAÍDAS       : TempData preenchido.
         *
         * 🔗 CHAMADA POR  : Controllers/Pages.
         *
         * 🔄 CHAMA        : JsonConvert.SerializeObject.
         ****************************************************************************************/
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            {
            tempData[key] = JsonConvert.SerializeObject(value);
            }

        /****************************************************************************************
         * ⚡ MÉTODO: Get
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Recuperar e desserializar um objeto do TempData.
         *
         * 📥 ENTRADAS     : key.
         *
         * 📤 SAÍDAS       : Objeto desserializado ou default.
         *
         * 🔗 CHAMADA POR  : Controllers/Pages.
         *
         * 🔄 CHAMA        : JsonConvert.DeserializeObject.
         ****************************************************************************************/
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
