/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IModeloVeiculoRepository.cs                                                            ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de ModeloVeiculo, gerenciando modelos de veículos por marca           ║
   ║    (Uno, Ka, Onix, Gol, Logan, Corolla, HB20, etc.).                                              ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetModeloVeiculoListForDropDown] : Lista modelos para DropDown.. () -> IEnumerable            ║
   ║ 2. [Update] : Atualiza modelo de veículo..................... (ModeloVeiculo) -> void             ║
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
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ INTERFACE: IModeloVeiculoRepository                                                │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ModeloVeiculo. Gerencia modelos de veículos por marca │
    // │    (Uno, Ka, Onix, etc.), vinculados a MarcaVeiculo para classificação da frota.     │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de Veículo, UnitOfWork                               │
    // │    ➡️ CHAMA       : IRepository&lt;ModeloVeiculo&gt; (métodos base)                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetModeloVeiculoListForDropDown                                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna lista de modelos de veículos para uso em DropDown.                        │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de Veículo, ViewComponents                           │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza um registro de ModeloVeiculo no banco de dados.                          │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • modeloVeiculo [ModeloVeiculo]: Entidade com dados atualizados                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Veículo                   │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        void Update(ModeloVeiculo modeloVeiculo);
    }
}
