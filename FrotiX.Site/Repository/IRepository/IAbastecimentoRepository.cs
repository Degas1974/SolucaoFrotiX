/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IAbastecimentoRepository.cs                                                                         ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de Abastecimento. Define contrato para operações CRUD de registros de    ║
║              abastecimento de combustível.                                                                       ║
║  📋 MÉTODOS DEFINIDOS:                                                                                           ║
║     • GetAbastecimentoListForDropDown() → SelectList para dropdowns                                             ║
║     • Update() → Atualização de abastecimento                                                                   ║
║  🔗 DEPENDÊNCIAS: IRepository<Abastecimento>, SelectListItem                                                    ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /********************************************************************************************
     * ⚡ INTERFACE: IAbastecimentoRepository
     * ─────────────────────────────────────────────────────────────────────────────────────
     * 🎯 OBJETIVO     : Contrato para repositório de Abastecimento (CRUD + customizações)
     *
     * 📥 ENTRADAS     : Instâncias de Abastecimento injetadas via DI
     *
     * 📤 SAÍDAS       : SelectListItem para dropdowns, resultados de CRUD
     *
     * 🔗 CHAMADA POR  : UnitOfWork, Services, Controllers
     *
     * 🔄 CHAMA        : IRepository<Abastecimento> (métodos base CRUD)
     *
     * 📦 DEPENDÊNCIAS : IRepository<Abastecimento>, SelectListItem
     *
     * 📝 OBSERVAÇÕES  : Define especialização de repositório para domínio Abastecimento
     *********************************************************************************************/
    public interface IAbastecimentoRepository : IRepository<Abastecimento>
        {
        /********************************************************************************************
         * ⚡ MÉTODO: GetAbastecimentoListForDropDown
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Retornar lista de abastecimentos formatados para dropdown UI
         *
         * 📤 SAÍDAS       : IEnumerable<SelectListItem> - Abastecimentos prontos para select
         *
         * ⬅️ CHAMADO POR  : Formulários que necessitam listar abastecimentos
         *
         * ➡️ CHAMA        : Implementação no repositório concreto
         *********************************************************************************************/
        IEnumerable<SelectListItem> GetAbastecimentoListForDropDown();

        /********************************************************************************************
         * ⚡ MÉTODO: Update
         * ─────────────────────────────────────────────────────────────────────────────────────
         * 🎯 OBJETIVO     : Atualizar registro de abastecimento no banco
         *
         * 📥 ENTRADAS     : abastecimento [Abastecimento] - Entidade a atualizar
         *
         * 📤 SAÍDAS       : void - Alterações persistidas
         *
         * ⬅️ CHAMADO POR  : UnitOfWork, Controllers
         *
         * ➡️ CHAMA        : Implementação no repositório concreto
         *********************************************************************************************/
        void Update(Abastecimento abastecimento);

        }
    }


