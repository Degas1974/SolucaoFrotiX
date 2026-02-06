/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ILavadoresLavagemRepository.cs                                                         ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de LavadoresLavagem, gerenciando associação MxN (muitos-para-muitos)  ║
   ║    entre lavadores e registros de lavagem de veículos (quem lavou cada veículo).                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetLavadoresLavagemListForDropDown] : Lista para DropDown...... () -> IEnumerable             ║
   ║ 2. [Update] : Atualiza lavador-lavagem..................... (LavadoresLavagem) -> void            ║
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
    // │ ⚡ INTERFACE: ILavadoresLavagemRepository                                             │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de LavadoresLavagem. Gerencia a tabela associativa MxN   │
    // │    que vincula lavadores a registros de lavagem, permitindo rastrear quais           │
    // │    profissionais executaram cada lavagem de veículo.                                 │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de Lavagem, UnitOfWork                               │
    // │    ➡️ CHAMA       : IRepository&lt;LavadoresLavagem&gt; (métodos base)                │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface ILavadoresLavagemRepository : IRepository<LavadoresLavagem>
    {

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetLavadoresLavagemListForDropDown                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna lista de associações lavador-lavagem formatadas para uso em DropDown.     │
        // │    Útil para formulários onde se registra quem executou uma lavagem.                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem com Text e Value   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de Lavagem, ViewComponents                           │
        // │    ➡️ CHAMA       : DbContext, LINQ queries com Include de Lavador e Lavagem         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        IEnumerable<SelectListItem> GetLavadoresLavagemListForDropDown();


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza um registro de LavadoresLavagem no banco de dados. Modifica              │
        // │    associações entre lavadores e lavagens de veículos.                               │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • lavadoresLavagem [LavadoresLavagem]: Entidade com dados atualizados             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Lavagem                   │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        void Update(LavadoresLavagem lavadoresLavagem);
    }
}


