// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRepactuacaoVeiculoRepository.cs                                ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RepactuacaoVeiculo, gerenciando repactuações de  ║
// ║ valores específicos de veículos em contratos.                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRepactuacaoVeiculoListForDropDown() → DropDown de repactuações          ║
// ║ • Update() → Atualização de repactuação de veículo                           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
{
    /// <summary>
    /// Interface do repositório de RepactuacaoVeiculo. Estende IRepository&lt;RepactuacaoVeiculo&gt;.
    /// </summary>
    public interface IRepactuacaoVeiculoRepository : IRepository<RepactuacaoVeiculo>
    {
        IEnumerable<SelectListItem> GetRepactuacaoVeiculoListForDropDown();

        void Update(RepactuacaoVeiculo repactuacaoVeiculo);
    }
}
