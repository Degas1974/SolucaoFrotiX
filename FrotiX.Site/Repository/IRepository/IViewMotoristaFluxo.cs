/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewMotoristaFluxo.cs                                                               ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewMotoristaFluxo, consultando fluxo consolidado                  ║
║    de motoristas para análises operacionais.                                                       ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetViewMotoristaFluxoListForDropDown] : Lista itens.. () -> IEnumerable                         ║
║ 2. [Update] : Atualização não aplicável (view)... (ViewMotoristaFluxo) -> void                     ║
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
    // │ ⚡ INTERFACE: IViewMotoristaFluxoRepository                                         │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewMotoristaFluxo. Centraliza consultas do fluxo        │
    // │    de motoristas para análises e relatórios.                                           │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Motorista                             │
    // │    ➡️ CHAMA       : IRepository (métodos base)                    │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public interface IViewMotoristaFluxoRepository : IRepository<ViewMotoristaFluxo>
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetViewMotoristaFluxoListForDropDown                                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna a lista de itens do fluxo de motoristas formatada para uso em DropDown.   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable]: Lista de itens para seleção                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers e relatórios de Motorista                            │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        IEnumerable<SelectListItem> GetViewMotoristaFluxoListForDropDown();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Método presente para compatibilidade com o contrato. Como se trata de view,       │
        // │    não há atualização persistente em banco.                                           │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • viewMotoristaFluxo [ViewMotoristaFluxo]: Entidade de view                        │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - sem persistência (view somente leitura)                    │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Services e compatibilidade de interface                           │
        // │    ➡️ CHAMA       : Não aplicável (view)                                              │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        void Update(ViewMotoristaFluxo viewMotoristaFluxo);
    }
}
