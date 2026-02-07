/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewAbastecimentosRepository.cs                                                      ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewAbastecimentos, consultando SQL View consolidada                ║
║    de abastecimentos para listagens e relatórios.                                                  ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetViewAbastecimentosListForDropDown] : Lista abastecimentos.. () -> IEnumerable               ║
║ 2. [Update] : Atualização não aplicável (view)...... (ViewAbastecimentos) -> void                 ║
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
    // │ ⚡ INTERFACE: IViewAbastecimentosRepository                                           │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewAbastecimentos. Centraliza consultas de              │
    // │    abastecimentos consolidados em uma view SQL para análises e relatórios.             │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Abastecimento                          │
    // │    ➡️ CHAMA       : IRepository (métodos base)                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IViewAbastecimentosRepository : IRepository<ViewAbastecimentos>
        {

        IEnumerable<SelectListItem> GetViewAbastecimentosListForDropDown();

        void Update(ViewAbastecimentos viewAbastecimentos);

        }
    }


