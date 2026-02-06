/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewManutencaoRepository.cs                                                         ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewManutencao, consultando SQL View consolidada                    ║
║    de manutenções com dados de veículo, fornecedor e empenho.                                      ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetViewManutencaoListForDropDown] : Lista manutenções.. () -> IEnumerable                       ║
║ 2. [Update] : Atualização não aplicável (view)... (ViewManutencao) -> void                         ║
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
    // │ ⚡ INTERFACE: IViewManutencaoRepository                                              │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewManutencao. Centraliza consultas consolidadas de     │
    // │    manutenções para análises e relatórios.                                              │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Manutencao                             │
    // │    ➡️ CHAMA       : IRepository (métodos base)                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
    {

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetViewManutencaoListForDropDown                                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna a lista de manutenções formatada para uso em DropDown (Select).           │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable]: Lista de itens para seleção                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers e relatórios de Manutencao                           │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        IEnumerable<SelectListItem> GetViewManutencaoListForDropDown();


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Método presente para compatibilidade com o contrato. Como se trata de view,       │
        // │    não há atualização persistente em banco.                                           │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • viewManutencao [ViewManutencao]: Entidade de view                                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - sem persistência (view somente leitura)                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Services e compatibilidade de interface                           │
        // │    ➡️ CHAMA       : Não aplicável (view)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        void Update(ViewManutencao viewManutencao);
    }
}


