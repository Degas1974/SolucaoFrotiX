// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IItemVeiculoAtaRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ItemVeiculoAta, gerenciando itens de veículos    ║
// ║ dentro de Atas de Registro de Preços (detalhamento).                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetItemVeiculoAtaListForDropDown() → DropDown de itens                     ║
// ║ • Update() → Atualização de item veículo-ata                                 ║
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
    /// Interface do repositório de ItemVeiculoAta. Estende IRepository&lt;ItemVeiculoAta&gt;.
    /// </summary>
    public interface IItemVeiculoAtaRepository : IRepository<ItemVeiculoAta>
        {

        IEnumerable<SelectListItem> GetItemVeiculoAtaListForDropDown();

        void Update(ItemVeiculoAta itemveiculoata);

        }
    }


