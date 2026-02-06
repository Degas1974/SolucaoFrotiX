/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: IItemVeiculoContratoRepository.cs                                                      ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de ItemVeiculoContrato, gerenciando itens de veículos dentro de       ║
   ║    contratos de terceirização (detalhamento por modelo e especificação contratual).               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetItemVeiculoContratoListForDropDown] : Lista para DropDown...... () -> IEnumerable          ║
   ║ 2. [Update] : Atualiza item veículo-contrato................ (ItemVeiculoContrato) -> void        ║
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
    // │ ⚡ INTERFACE: IItemVeiculoContratoRepository                                          │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de ItemVeiculoContrato. Gerencia os itens (detalhamentos)│
    // │    de veículos especificados em contratos de terceirização, permitindo consulta e    │
    // │    atualização de especificações técnicas, valores mensais e condições contratuais.  │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de Contrato, UnitOfWork                              │
    // │    ➡️ CHAMA       : IRepository&lt;ItemVeiculoContrato&gt; (métodos base)             │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
    {

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetItemVeiculoContratoListForDropDown                              │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna lista de itens de veículos de contrato formatados para uso em DropDown    │
        // │    (Select). Útil para formulários de seleção de itens contratuais em telas.         │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem com Text e Value   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de Contrato, ViewComponents                          │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        IEnumerable<SelectListItem> GetItemVeiculoContratoListForDropDown();


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza um registro de ItemVeiculoContrato no banco de dados. Modifica           │
        // │    especificações técnicas, valores mensais ou outras propriedades contratuais.      │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • itemveiculocontrato [ItemVeiculoContrato]: Entidade com dados atualizados       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Contrato                  │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        void Update(ItemVeiculoContrato itemveiculocontrato);
    }
}


