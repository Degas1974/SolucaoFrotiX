// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IControleAcessoRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ControleAcesso, gerenciando registros de acesso  ║
// ║ de veículos em portarias e áreas controladas.                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetControleAcessoListForDropDown() → DropDown de registros de acesso       ║
// ║ • Update() → Atualização de controle de acesso                               ║
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
    /// Interface do repositório de ControleAcesso. Estende IRepository&lt;ControleAcesso&gt;.
    /// </summary>
    public interface IControleAcessoRepository : IRepository<ControleAcesso>
        {

        IEnumerable<SelectListItem> GetControleAcessoListForDropDown();

        void Update(ControleAcesso controleacesso);

        }
    }


