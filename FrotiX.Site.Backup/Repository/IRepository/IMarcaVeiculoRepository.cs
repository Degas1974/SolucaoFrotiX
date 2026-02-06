/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IMarcaVeiculoRepository.cs                                                             ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de MarcaVeiculo, gerenciando cadastro de marcas de veículos           ║
   ║    (Fiat, Ford, Chevrolet, Volkswagen, Renault, Toyota, Honda, etc.).                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetMarcaVeiculoListForDropDown] : Lista marcas para DropDown....... () -> IEnumerable         ║
   ║ 2. [Update] : Atualiza marca de veículo........................ (MarcaVeiculo) -> void            ║
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
    // │ ⚡ INTERFACE: IMarcaVeiculoRepository                                                 │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de MarcaVeiculo. Gerencia o cadastro de marcas de        │
    // │    veículos (fabricantes) utilizadas pela frota, como Fiat, Ford, Chevrolet, VW,     │
    // │    Renault, Toyota, Honda, Nissan, entre outras.                                     │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de Veículo, UnitOfWork                               │
    // │    ➡️ CHAMA       : IRepository&lt;MarcaVeiculo&gt; (métodos base)                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetMarcaVeiculoListForDropDown                                     │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna lista de marcas de veículos formatadas para uso em DropDown (Select).     │
        // │    Útil para formulários de cadastro de veículos onde se seleciona a marca.          │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem com Text e Value   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de Veículo, ViewComponents                           │
        // │    ➡️ CHAMA       : DbContext, LINQ queries (Where ativa = true)                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza um registro de MarcaVeiculo no banco de dados. Modifica nome da marca,   │
        // │    status de ativação ou outras propriedades do cadastro de marca.                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • marcaVeiculo [MarcaVeiculo]: Entidade com dados atualizados                     │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Veículo                   │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        void Update(MarcaVeiculo marcaVeiculo);
    }
}


