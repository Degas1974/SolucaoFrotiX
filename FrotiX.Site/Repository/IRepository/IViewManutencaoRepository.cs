// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewManutencaoRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewManutencao, consultando SQL View consolidada ║
// ║ de manutenções com dados de veículo, fornecedor e empenho.                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewManutencaoListForDropDown() → DropDown de manutenções               ║
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
    /// Interface do repositório de ViewManutencao. Estende IRepository&lt;ViewManutencao&gt;.
    /// </summary>
    public interface IViewManutencaoRepository : IRepository<ViewManutencao>
        {

        IEnumerable<SelectListItem> GetViewManutencaoListForDropDown();

        void Update(ViewManutencao viewManutencao);

        }
    }


