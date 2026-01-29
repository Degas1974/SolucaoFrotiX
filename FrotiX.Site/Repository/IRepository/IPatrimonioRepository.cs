// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IPatrimonioRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Patrimônio, gerenciando bens patrimoniais da     ║
// ║ frota como veículos próprios com número de tombamento.                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetPatrimonioListForDropDown() → DropDown de patrimônios                   ║
// ║ • Update() → Atualização de patrimônio                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de Patrimônio. Estende IRepository&lt;Patrimonio&gt;.
    /// </summary>
    public interface IPatrimonioRepository : IRepository<Patrimonio>
        {

        IEnumerable<SelectListItem> GetPatrimonioListForDropDown();

        void Update(Patrimonio patrimonio);

        }
    }


