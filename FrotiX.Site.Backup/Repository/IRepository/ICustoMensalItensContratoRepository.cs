/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: ICustoMensalItensContratoRepository.cs                                                              ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de CustoMensalItensContrato, gerenciando custos mensais de itens de       ║
║              contratos de terceirização.                                                                          ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetCustoMensalItensContratoListForDropDown() → DropDown de custos                                         ║
║     • Update() → Atualização de custo mensal                                                                    ║
║  🔗 DEPENDÊNCIAS: IRepository<CustoMensalItensContrato>, SelectListItem                                        ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {

    // Interface do repositório de CustoMensalItensContrato. Estende IRepository&lt;CustoMensalItensContrato&gt;.

    public interface ICustoMensalItensContratoRepository : IRepository<CustoMensalItensContrato>
        {

        IEnumerable<SelectListItem> GetCustoMensalItensContratoListForDropDown();

        void Update(CustoMensalItensContrato customensalitenscontrato);

        }
    }


