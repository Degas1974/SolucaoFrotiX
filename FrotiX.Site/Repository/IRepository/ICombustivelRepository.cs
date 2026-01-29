// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ICombustivelRepository.cs                                       ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Combustível, gerenciando tipos de combustível    ║
// ║ cadastrados (gasolina, diesel, etanol, GNV, elétrico).                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetCombustivelListForDropDown() → DropDown de combustíveis                 ║
// ║ • Update() → Atualização de combustível                                      ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de Combustível. Estende IRepository&lt;Combustivel&gt;.
    /// </summary>
    public interface ICombustivelRepository : IRepository<Combustivel>
        {

        IEnumerable<SelectListItem> GetCombustivelListForDropDown();

        void Update(Combustivel combustivel);

        }
    }


