// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ILavagemRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Lavagem, controlando registros de lavagens dos   ║
// ║ veículos da frota com data, tipo e lavador responsável.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetLavagemListForDropDown() → DropDown de lavagens                         ║
// ║ • Update() → Atualização de registro de lavagem                              ║
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
    /// Interface do repositório de Lavagem. Estende IRepository&lt;Lavagem&gt;.
    /// </summary>
    public interface ILavagemRepository : IRepository<Lavagem>
        {

        IEnumerable<SelectListItem> GetLavagemListForDropDown();

        void Update(Lavagem lavagem);

        }
    }


