/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: ICombustivelRepository.cs                                                                           ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de Combustível, gerenciando tipos de combustível cadastrados            ║
║              (gasolina, diesel, etanol, GNV, elétrico).                                                          ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetCombustivelListForDropDown() → DropDown de combustíveis                                                ║
║     • Update() → Atualização de combustível                                                                     ║
║  🔗 DEPENDÊNCIAS: IRepository<Combustivel>, SelectListItem                                                     ║
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

    // Interface do repositório de Combustível. Estende IRepository&lt;Combustivel&gt;.

    public interface ICombustivelRepository : IRepository<Combustivel>
        {

        IEnumerable<SelectListItem> GetCombustivelListForDropDown();

        void Update(Combustivel combustivel);

        }
    }


