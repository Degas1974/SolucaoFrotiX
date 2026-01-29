// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ICustoMensalItensContratoRepository.cs                          ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de CustoMensalItensContrato, gerenciando custos     ║
// ║ mensais de itens de contratos de terceirização.                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetCustoMensalItensContratoListForDropDown() → DropDown de custos          ║
// ║ • Update() → Atualização de custo mensal                                     ║
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
    /// Interface do repositório de CustoMensalItensContrato. Estende IRepository&lt;CustoMensalItensContrato&gt;.
    /// </summary>
    public interface ICustoMensalItensContratoRepository : IRepository<CustoMensalItensContrato>
        {

        IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown();

        void Update(CustoMensalItensContrato customensalitenscontrato);

        }
    }


