/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IViewVeiculosRepository.cs                                                             ║
   ║ 📂 CAMINHO: Repository/IRepository/                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de ViewVeiculos.                                                       ║
   ║    Consulta SQL View consolidada de veículos com dados de contrato, marca, modelo e situação.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetViewVeiculosListForDropDown] : DropDown de veículos consolidados                           ║
   ║    () -> IEnumerable<SelectListItem>                                                               ║
   ║ 2. [Update] : Atualização (não aplicável - view somente leitura)                                  ║
   ║    (ViewVeiculos) -> void                                                                          ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ ⚠️ MANUTENÇÃO:                                                                                     ║
   ║    Qualquer alteração neste código exige atualização imediata deste Card e do Header dos Métodos. ║
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
    /// │ 🎯 INTERFACE: IViewVeiculosRepository                                                │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ DESCRIÇÃO:                                                                             │
    /// │    Interface do repositório de ViewVeiculos.                                           │
    /// │    Estende IRepository&lt;ViewVeiculos&gt; com métodos especializados.                 │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public interface IViewVeiculosRepository : IRepository<ViewVeiculos>
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: GetViewVeiculosListForDropDown                                            │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO:                                                                          │
        /// │    Retorna lista consolidada de veículos formatada para DropDown.                      │
        /// │    Inclui dados de contrato, marca, modelo e situação.                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS: Nenhum                                                                      │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS:                                                                            │
        /// │    • IEnumerable&lt;SelectListItem&gt; - Lista para DropDown de veículos              │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Controllers diversos (seleção de veículos)                         │
        /// │    ➡️ CHAMA       : Context.ViewVeiculos (SQL View)                                    │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IEnumerable<SelectListItem> GetViewVeiculosListForDropDown();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ MÉTODO: Update                                                                     │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO:                                                                          │
        /// │    Método de atualização. NÃO APLICÁVEL para views (somente leitura).                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS:                                                                             │
        /// │    • viewVeiculos [ViewVeiculos]                                                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS: void                                                                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                    │
        /// │    ⬅️ CHAMADO POR : Não aplicável (views são somente leitura)                         │
        /// │    ➡️ CHAMA       : Nenhum                                                             │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        void Update(ViewVeiculos viewVeiculos);
    }
}


