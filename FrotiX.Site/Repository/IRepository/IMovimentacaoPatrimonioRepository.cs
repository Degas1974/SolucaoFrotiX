// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMovimentacaoPatrimonioRepository.cs                            ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MovimentacaoPatrimonio, gerenciando movimentações║
// ║ de bens patrimoniais (transferências, baixas, devoluções).                  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMovimentacaoPatrimonioListForDropDown() → DropDown de movimentações     ║
// ║ • Update() → Atualização de movimentação patrimonial                          ║
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
    /// Interface do repositório de MovimentacaoPatrimonio. Estende IRepository&lt;MovimentacaoPatrimonio&gt;.
    /// </summary>
    public interface IMovimentacaoPatrimonioRepository : IRepository<MovimentacaoPatrimonio>
        {

        IEnumerable<SelectListItem> GetMovimentacaoPatrimonioListForDropDown();

        void Update(MovimentacaoPatrimonio movimentacaoPatrimonio);

        }
    }


