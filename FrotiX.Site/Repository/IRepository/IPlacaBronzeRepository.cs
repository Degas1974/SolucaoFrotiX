/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IPlacaBronzeRepository.cs                                                             ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de PlacaBronze, gerenciando dados de placas de veículos                ║
║    no sistema bronze (importação, sincronização e consulta).                                       ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetPlacaBronzeListForDropDown] : Lista placas para DropDown.. () -> IEnumerable                ║
║ 2. [Update] : Atualiza registro de placa bronze....... (PlacaBronze) -> void                        ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ ⚠️ MANUTENÇÃO:                                                                                     ║
║    Qualquer alteração neste código exige atualização imediata deste Card e do Header do Método.   ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
    /// │ ⚡ INTERFACE: IPlacaBronzeRepository                                                  │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    /// │    Interface do repositório de PlacaBronze. Centraliza operações de consulta e         │
    /// │    atualização de placas no sistema bronze, assegurando integração e histórico.       │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : Controllers de PlacaBronze, UnitOfWork                             │
    /// │    ➡️ CHAMA       : IRepository<PlacaBronze> (métodos base)                           │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public interface IPlacaBronzeRepository : IRepository<PlacaBronze>
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: GetPlacaBronzeListForDropDown                                      │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Retorna a lista de placas bronze formatada para uso em DropDown (Select),         │
        /// │    facilitando seleção em telas de consulta e sincronização.                         │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • Nenhum parâmetro                                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [IEnumerable<SelectListItem>]: Lista de itens para seleção                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : Controllers de PlacaBronze, ViewComponents                        │
        /// │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IEnumerable<SelectListItem> GetPlacaBronzeListForDropDown();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Update                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Atualiza um registro de PlacaBronze no banco de dados, alterando dados             │
        /// │    de importação, status de sincronização e informações da placa.                    │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • placaBronze [PlacaBronze]: Entidade com dados atualizados                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [void]: Método void - alterações aplicadas ao contexto EF Core                  │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de PlacaBronze               │
        /// │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        void Update(PlacaBronze placaBronze);
    }
}
