// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IItemVeiculoContratoRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ItemVeiculoContrato, gerenciando itens de        ║
// ║ veículos dentro de contratos de terceirização (detalhamento).                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetItemVeiculoContratoListForDropDown() → DropDown de itens contrato       ║
// ║ • Update() → Atualização de item veículo-contrato                            ║
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
    /// Interface do repositório de ItemVeiculoContrato. Estende IRepository&lt;ItemVeiculoContrato&gt;.
    /// </summary>
    public interface IItemVeiculoContratoRepository : IRepository<ItemVeiculoContrato>
        {

        IEnumerable<SelectListItem> GetItemVeiculoContratoListForDropDown();

        void Update(ItemVeiculoContrato itemveiculocontrato);

        }
    }


