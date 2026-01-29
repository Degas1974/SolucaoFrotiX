// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IVeiculoAtaRepository.cs                                        ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de VeiculoAta, gerenciando veículos vinculados      ║
// ║ a Atas de Registro de Preços (associação MxN).                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetVeiculoAtaListForDropDown() → DropDown de veículos em atas              ║
// ║ • Update() → Atualização de veículo-ata                                      ║
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
    /// Interface do repositório de VeiculoAta. Estende IRepository&lt;VeiculoAta&gt;.
    /// </summary>
    public interface IVeiculoAtaRepository : IRepository<VeiculoAta>
        {

        IEnumerable<SelectListItem> GetVeiculoAtaListForDropDown();

        void Update(VeiculoAta VeiculoAta);

        }
    }


