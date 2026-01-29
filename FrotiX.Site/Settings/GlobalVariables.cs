/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Settings/GlobalVariables.cs                                    ║
 * ║  Descrição: Variáveis globais estáticas da aplicação. Inclui             ║
 * ║             VeiculoID (Guid), gPontoUsuario (string), ConnectionString.  ║
 * ║             ATENÇÃO: Uso de variáveis globais estáticas - considerar     ║
 * ║             migração para IOptions<T> ou scoped services.                ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
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


