// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRepactuacaoContratoRepository.cs                               ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RepactuacaoContrato, gerenciando repactuações de ║
// ║ valores em contratos de terceirização (reajustes anuais).                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRepactuacaoContratoListForDropDown() → DropDown de repactuações         ║
// ║ • Update() → Atualização de repactuação de contrato                          ║
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
    /// Interface do repositório de RepactuacaoContrato. Estende IRepository&lt;RepactuacaoContrato&gt;.
    /// </summary>
    public interface IRepactuacaoContratoRepository : IRepository<RepactuacaoContrato>
        {

        IEnumerable<SelectListItem> GetRepactuacaoContratoListForDropDown();

        void Update(RepactuacaoContrato RepactuacaoContrato);

        }
    }


