/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: GlobalVariables.cs                                                                      ║
   ║ 📂 CAMINHO: /Settings                                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Variáveis globais estáticas. VeiculoID, gPontoUsuario, ConnectionString.               ║
   ║ ⚠️ ATENÇÃO: Considerar migrar para IOptions<T> ou scoped services.                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: VeiculoID (Guid), gPontoUsuario (string), ConnectionString, GlobalBoolean                ║
   ║ 🔗 DEPS: Nenhuma | 📅 29/01/2026 | 👤 Copilot | 📝 v2.0                                             ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Settings
    {
    public static class GlobalVariables
        {
        /// <summary>
        /// Global variable that is constant.
        /// </summary>
        public const string GlobalString = "Important Text";

        /// <summary>
        /// Static value protected by access routine.
        /// </summary>
        public static Guid _veiculoId;

        public static string gPontoUsuario;

        /// <summary>
        /// Access routine for global variable.
        /// </summary>
        public static Guid VeiculoID
            {
            get
                {
                return _veiculoId;
                }
            set
                {
                _veiculoId = value;
                }
            }

        /// <summary>
        /// Global static field.
        /// </summary>
        public static bool GlobalBoolean;

        public static string ConnectionString;
        }
    }


