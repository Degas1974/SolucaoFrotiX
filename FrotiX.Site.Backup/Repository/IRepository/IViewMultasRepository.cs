/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewMultasRepository.cs                                                              ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewMultas, consultando SQL View consolidada                        ║
║    de multas com dados de veículo, motorista e empenho.                                            ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetviewMultasListForDropDown] : Lista multas.. () -> IEnumerable                                 ║
║ 2. [Update] : Atualização não aplicável (view)... (ViewMultas) -> void                             ║
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
    // │ ⚡ INTERFACE: IviewMultasRepository                                                   │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewMultas. Centraliza consultas consolidadas de         │
    // │    multas para análises e relatórios.                                                  │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Multas                               │
    // │    ➡️ CHAMA       : IRepository (métodos base)                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IviewMultasRepository : IRepository<ViewMultas>
    {

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetviewMultasListForDropDown                                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna a lista de multas formatada para uso em DropDown (Select).                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable]: Lista de itens para seleção                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers e relatórios de Multas                              │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        IEnumerable<SelectListItem> GetviewMultasListForDropDown();


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Método presente para compatibilidade com o contrato. Como se trata de view,       │
        // │    não há atualização persistente em banco.                                           │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • viewMultas [ViewMultas]: Entidade de view                                        │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - sem persistência (view somente leitura)                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Services e compatibilidade de interface                           │
        // │    ➡️ CHAMA       : Não aplicável (view)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        void Update(ViewMultas viewMultas);
    }
}


