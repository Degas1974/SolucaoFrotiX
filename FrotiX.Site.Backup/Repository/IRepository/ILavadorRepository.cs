/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ILavadorRepository.cs                                                                  ║
   ║ 📂 CAMINHO: /Repository/IRepository                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Interface do repositório de Lavador, gerenciando cadastro de profissionais que executam        ║
   ║    serviços de lavagem de veículos da frota (lavadores terceirizados ou próprios).                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE DE MÉTODOS (Entradas -> Saídas):                                                         ║
   ║ 1. [GetLavadorListForDropDown] : Lista lavadores para DropDown......... () -> IEnumerable         ║
   ║ 2. [Update] : Atualiza cadastro de lavador....................... (Lavador) -> void               ║
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
    // │ ⚡ INTERFACE: ILavadorRepository                                                      │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
    // │    Interface do repositório de Lavador. Gerencia o cadastro de profissionais         │
    // │    (lavadores) que executam serviços de lavagem de veículos da frota, sejam eles     │
    // │    próprios ou terceirizados vinculados a contratos de lavagem.                      │
    // │───────────────────────────────────────────────────────────────────────────────────────│
    // │ 🔗 RASTREABILIDADE:                                                                   │
    // │    ⬅️ CHAMADO POR : Controllers de Lavagem, UnitOfWork                               │
    // │    ➡️ CHAMA       : IRepository&lt;Lavador&gt; (métodos base)                         │
    // ╰───────────────────────────────────────────────────────────────────────────────────────╯

    public interface ILavadorRepository : IRepository<Lavador>
    {

        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: GetLavadorListForDropDown                                          │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Retorna lista de lavadores formatados para uso em DropDown (Select). Útil para    │
        // │    formulários onde o usuário seleciona o lavador responsável por uma lavagem.       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • Nenhum parâmetro                                                                 │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [IEnumerable&lt;SelectListItem&gt;]: Lista de SelectListItem com Text e Value   │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : Controllers de Lavagem, ViewComponents                           │
        // │    ➡️ CHAMA       : DbContext, LINQ queries (Where ativo = true)                     │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        IEnumerable<SelectListItem> GetLavadorListForDropDown();


        // ╭───────────────────────────────────────────────────────────────────────────────────────╮
        // │ ⚡ FUNCIONALIDADE: Update                                                             │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🎯 DESCRIÇÃO DETALHADA:                                                               │
        // │    Atualiza um registro de Lavador no banco de dados. Modifica nome, CPF, telefone,  │
        // │    endereço, status de ativação ou outras propriedades do cadastro de lavador.       │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📥 INPUTS (Entradas):                                                                 │
        // │    • lavador [Lavador]: Entidade com dados atualizados                               │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 📤 OUTPUTS (Saídas):                                                                  │
        // │    • [void]: Método void - modificações aplicadas ao contexto EF Core                │
        // │───────────────────────────────────────────────────────────────────────────────────────│
        // │ 🔗 RASTREABILIDADE:                                                                   │
        // │    ⬅️ CHAMADO POR : UnitOfWork.SaveAsync(), Controllers de Lavagem                   │
        // │    ➡️ CHAMA       : DbContext.Update(), Entity State tracking                         │
        // ╰───────────────────────────────────────────────────────────────────────────────────────╯

        void Update(Lavador lavador);
    }
}


