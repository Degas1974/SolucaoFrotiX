/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewLotacoesRepository.cs                                                            ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewLotacoes, consultando SQL View consolidada                      ║
║    de lotações de motoristas com dados de setor e período.                                         ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [Nenhum método adicional] : Usa apenas IRepository<ViewLotacoes>                                ║
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
    // │ ⚡ INTERFACE: IViewLotacoesRepository                                                │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewLotacoes. Centraliza consultas consolidadas de       │
    // │    lotações de motoristas, utilizando apenas operações genéricas de repositório.        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Lotações                              │
    // │    ➡️ CHAMA       : IRepository (métodos base)                          │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IViewLotacoesRepository : IRepository<ViewLotacoes>
    {
        // Sem métodos adicionais: utiliza apenas as operações genéricas do IRepository.
    }
}


