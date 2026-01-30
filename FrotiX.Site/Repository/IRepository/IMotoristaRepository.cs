/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IMotoristaRepository.cs                                                                ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de Motorista. Define contrato para operações CRUD e listas de         ║
   ║    seleção de motoristas da frota (próprios e terceirizados).                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetMotoristaListForDropDown] : Lista motoristas para DropDown.... () -> IEnumerable           ║
   ║ 2. [Update] : Atualiza cadastro de motorista...................... (Motorista) -> void            ║
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
    /// │ ⚡ INTERFACE: IMotoristaRepository                                                    │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    /// │    Interface do repositório de Motorista. Gerencia cadastro completo de motoristas   │
    /// │    da frota (próprios e terceirizados) com dados pessoais, CNH e histórico.          │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : Controllers de Motorista/Viagem, UnitOfWork                      │
    /// │    ➡️ CHAMA       : IRepository&lt;Motorista&gt; (métodos base)                       │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public interface IMotoristaRepository : IRepository<Motorista>
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: GetMotoristaListForDropDown                                        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Retorna lista de motoristas ativos ordenada por nome para uso em DropDown.        │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • Nenhum parâmetro                                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem                    │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : Controllers de Viagem/Motorista, ViewComponents                 │
        /// │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IEnumerable<SelectListItem> GetMotoristaListForDropDown();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Update                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Atualiza um registro de Motorista no banco de dados.                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • motorista [Motorista]: Entidade com dados atualizados                           │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Motorista                 │
        /// │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        void Update(Motorista motorista);
    }
}
