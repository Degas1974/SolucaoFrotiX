/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IVeiculoRepository.cs                                                                 ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de Veiculo, definindo contrato para operações CRUD                     ║
║    e listas de seleção de veículos da frota.                                                       ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetVeiculoListForDropDown] : Lista veículos (placas).. () -> IEnumerable                        ║
║ 2. [Update] : Atualiza registro de veículo........... (Veiculo) -> void                             ║
║ 3. [GetVeiculoCompletoListForDropDown] : Lista veículos completos.. () -> IEnumerable              ║
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
    /// │ ⚡ INTERFACE: IVeiculoRepository                                                     │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    /// │    Interface do repositório de Veiculo. Centraliza operações de consulta,               │
    /// │    atualização e listagens para seleção de veículos da frota.                           │
    /// │───────────────────────────────────────────────────────────────────────────────────────│
    /// │ 🔗 RASTREABILIDADE:                                                                   │
    /// │    ⬅️ CHAMADO POR : Controllers de Veiculo, UnitOfWork                                │
    /// │    ➡️ CHAMA       : IRepository<Veiculo> (métodos base)                               │
    /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
    /// </summary>
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: GetVeiculoListForDropDown                                         │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Retorna a lista de veículos (ex.: placas) formatada para uso em DropDown (Select).│
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • Nenhum parâmetro                                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [IEnumerable<SelectListItem>]: Lista de itens para seleção                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : Controllers de Veiculo, ViewComponents                           │
        /// │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IEnumerable<SelectListItem> GetVeiculoListForDropDown();

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: Update                                                             │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Atualiza um registro de Veiculo no banco de dados, ajustando dados cadastrais,     │
        /// │    status e demais informações operacionais.                                          │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • veiculo [Veiculo]: Entidade com dados atualizados                                │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [void]: Método void - alterações aplicadas ao contexto EF Core                  │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Veiculo                   │
        /// │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        void Update(Veiculo veiculo);

        /// <summary>
        /// ╭───────────────────────────────────────────────────────────────────────────────────────╮
        /// │ ⚡ FUNCIONALIDADE: GetVeiculoCompletoListForDropDown                                  │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        /// │    Retorna a lista de veículos com dados completos formatada para uso em DropDown     │
        /// │    (Select), incluindo informações adicionais de identificação.                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📥 INPUTS (Entradas):                                                                 │
        /// │    • Nenhum parâmetro                                                                 │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 📤 OUTPUTS (Saídas):                                                                  │
        /// │    • [IEnumerable<SelectListItem>]: Lista de itens para seleção                       │
        /// │───────────────────────────────────────────────────────────────────────────────────────│
        /// │ 🔗 RASTREABILIDADE:                                                                   │
        /// │    ⬅️ CHAMADO POR : Controllers de Veiculo, ViewComponents                           │
        /// │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        /// ╰───────────────────────────────────────────────────────────────────────────────────────╯
        /// </summary>
        IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown();
    }
}
