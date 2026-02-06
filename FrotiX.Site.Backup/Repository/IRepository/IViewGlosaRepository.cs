/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IViewGlosaRepository.cs                                                               ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de ViewGlosa, consultando SQL View de glosas (descontos)               ║
║    aplicadas em contratos de terceirização.                                                        ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [Nenhum método adicional] : Usa apenas métodos de IRepository<ViewGlosa>                        ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ ⚠️ MANUTENÇÃO:                                                                                     ║
║    Qualquer alteração neste código exige atualização imediata deste Card e do Header do Método.   ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/
using System;
using FrotiX.Models;
using FrotiX.Repository.IRepository;

namespace FrotiX.Repository.IRepository
{
    
    // ╭───────────────────────────────────────────────────────────────────────────────────────╮
    // │ ⚡ INTERFACE: IViewGlosaRepository                                                   │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ViewGlosa (view/keyless). Mantém apenas operações        │
    // │    genéricas herdadas para consultas consolidadas de glosas.                            │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers e relatórios de Glosa                                 │
    // │    ➡️ CHAMA       : IRepository (métodos base)                             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public interface IViewGlosaRepository : IRepository<ViewGlosa>
    {
        // Sem métodos adicionais: utiliza apenas as operações genéricas do IRepository.
    }
}


