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
    public interface IAbastecimentoRepository : IRepository<Abastecimento>
        {

        IEnumerable<SelectListItem> GetAbastecimentoListForDropDown();

        void Update(Abastecimento abastecimento);

        }
    }


