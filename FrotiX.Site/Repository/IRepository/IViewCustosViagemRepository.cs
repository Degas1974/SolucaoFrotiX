// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewCustosViagemRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewCustosViagem, consultando SQL View com       ║
// ║ custos consolidados de viagens (combustível, locacao, terceirizacao).        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewCustosViagemListForDropDown() → DropDown de custos de viagem        ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
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
    /// Interface do repositório de ViewCustosViagem. Estende IRepository&lt;ViewCustosViagem&gt;.
    /// </summary>
    public interface IViewCustosViagemRepository : IRepository<ViewCustosViagem>
        {

        IEnumerable<SelectListItem> GetViewCustosViagemListForDropDown();

        void Update(ViewCustosViagem ViewCustosViagem);

        }
    }


