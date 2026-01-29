// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IVeiculoContratoRepository.cs                                   ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de VeiculoContrato, gerenciando veículos vinculados ║
// ║ a contratos de terceirização (associação MxN).                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetVeiculoContratoListForDropDown() → DropDown de veículos em contratos    ║
// ║ • Update() → Atualização de veículo-contrato                                 ║
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
    /// Interface do repositório de VeiculoContrato. Estende IRepository&lt;VeiculoContrato&gt;.
    /// </summary>
    public interface IVeiculoContratoRepository : IRepository<VeiculoContrato>
        {

        IEnumerable<SelectListItem> GetVeiculoContratoListForDropDown();

        void Update(VeiculoContrato VeiculoContrato);

        }
    }


