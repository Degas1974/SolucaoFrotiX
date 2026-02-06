/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
║ 🚀 ARQUIVO: IMovimentacaoEmpenhoRepository.cs                                                      ║
║ 📂 CAMINHO: /Repository/IRepository                                                                ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
║    Interface do repositório de MovimentacaoEmpenho, controlando movimentações                      ║
║    financeiras vinculadas a empenhos (créditos, débitos, anulações e ajustes).                     ║
╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
║ 1. [GetMovimentacaoEmpenhoListForDropDown] : Lista para DropDown........ () -> IEnumerable          ║
║ 2. [Update] : Atualiza movimentação de empenho................. (MovimentacaoEmpenho) -> void      ║
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
    // │ ⚡ INTERFACE: IMovimentacaoEmpenhoRepository                                           │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de MovimentacaoEmpenho. Centraliza operações de           │
    // │    consulta e atualização de movimentações financeiras vinculadas a empenhos,        │
    // │    assegurando controle de lançamentos, ajustes e histórico de movimentação.          │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de MovimentacaoEmpenho, UnitOfWork                     │
    // │    ➡️ CHAMA       : IRepository (métodos base)                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯
    
    public interface IMovimentacaoEmpenhoRepository : IRepository<MovimentacaoEmpenho>
    {
        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetMovimentacaoEmpenhoListForDropDown                               │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna a lista de movimentações de empenho formatada para uso em DropDown        │
        // │    (Select), facilitando seleção em telas e filtros de consulta.                     │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable]: Lista de itens para seleção                       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de MovimentacaoEmpenho, ViewComponents               │
        // │    ➡️ CHAMA       : DbContext, LINQ queries                                           │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        IEnumerable<SelectListItem> GetMovimentacaoEmpenhoListForDropDown();

        
        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza uma movimentação de empenho no banco de dados, alterando valores,        │
        // │    datas, tipo de lançamento ou status conforme regras de negócio.                   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • movimentacaoempenho [MovimentacaoEmpenho]: Entidade com dados atualizados       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - alterações aplicadas ao contexto EF Core                  │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de MovimentacaoEmpenho       │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯
        
        void Update(MovimentacaoEmpenho movimentacaoempenho);
    }
}


