// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewControleAcessoRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewControleAcesso, consultando SQL View         ║
// ║ consolidada de registros de controle de acesso em portarias.                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewControleAcessoListForDropDown() → DropDown de registros de acesso   ║
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
    /// Interface do repositório de ViewControleAcesso. Estende IRepository&lt;ViewControleAcesso&gt;.
    /// </summary>
    public interface IViewControleAcessoRepository : IRepository<ViewControleAcesso>
        {

        IEnumerable<SelectListItem> GetViewControleAcessoListForDropDown();

        void Update(ViewControleAcesso viewControleAcesso);

        }
    }


