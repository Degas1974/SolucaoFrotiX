/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewLotacaoMotorista.cs                                                              ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewLotacaoMotorista, consultando lotações consolidadas            ║
║    de motoristas para acompanhamento e relatórios.                                                 ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [Nenhum método adicional] : Usa apenas IRepository<ViewLotacaoMotorista>                        ║
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
    // │ ⚡ INTERFACE: IViewLotacaoMotoristaRepository                                        │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewLotacaoMotorista. Centraliza consultas de lotações  │
    // │    de motoristas por setor e período, usando apenas operações genéricas.               │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Lotação de Motorista                   │
    // │    ➡️ CHAMA       : IRepository (métodos base)                  │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IViewLotacaoMotoristaRepository : IRepository<ViewLotacaoMotorista>
    {
        // Sem métodos adicionais: utiliza apenas as operações genéricas do IRepository.
    }
}


