// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IUnidadeRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Unidade, gerenciando unidades de medida como     ║
// ║ litros, quilômetros, horas e outras usadas no sistema.                       ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetUnidadeListForDropDown() → DropDown de unidades                         ║
// ║ • Update() → Atualização de unidade                                          ║
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
    /// Interface do repositório de Unidade. Estende IRepository&lt;Unidade&gt;.
    /// </summary>
    public interface IUnidadeRepository : IRepository<Unidade>
        {

        IEnumerable<SelectListItem> GetUnidadeListForDropDown();

        void Update(Unidade unidade);

        }
    }


