// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IEmpenhoRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Empenho, gerenciando notas de empenho para       ║
// ║ controle orçamentário de despesas da frota (DESPFIN, DESPMA).                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetEmpenhoListForDropDown() → DropDown de empenhos disponíveis             ║
// ║ • Update() → Atualização de empenho                                          ║
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
    /// Interface do repositório de Empenho. Estende IRepository&lt;Empenho&gt;.
    /// </summary>
    public interface IEmpenhoRepository : IRepository<Empenho>
        {

        IEnumerable<SelectListItem> GetEmpenhoListForDropDown();

        void Update(Empenho empenho);

        }
    }


