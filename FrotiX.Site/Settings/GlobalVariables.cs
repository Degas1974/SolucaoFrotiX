// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: GlobalVariables.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Variáveis globais estáticas do sistema (uso limitado, preferir DI).          ║
// ║ Mantido por compatibilidade com código legado.                               ║
// ║                                                                              ║
// ║ VARIÁVEIS DISPONÍVEIS:                                                       ║
// ║ - GlobalString: Constante de texto importante                                ║
// ║ - VeiculoID / _veiculoId: ID do veículo atual (contexto de sessão)           ║
// ║ - gPontoUsuario: Ponto do usuário logado                                     ║
// ║ - GlobalBoolean: Flag global genérica                                        ║
// ║ - ConnectionString: String de conexão do banco de dados                      ║
// ║                                                                              ║
// ║ OBSERVAÇÃO:                                                                  ║
// ║ Variáveis estáticas não são thread-safe. Para novos desenvolvimentos,        ║
// ║ preferir injeção de dependência com IOptions<T> ou scoped services.          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 13                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Settings
    {
    /// <summary>
    /// Variáveis globais estáticas (legado - preferir DI).
    /// </summary>
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


