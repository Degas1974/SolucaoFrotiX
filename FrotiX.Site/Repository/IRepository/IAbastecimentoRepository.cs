// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IAbastecimentoRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Abastecimento. Define contrato para operações    ║
// ║ CRUD de registros de abastecimento de combustível.                            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • GetAbastecimentoListForDropDown() → SelectList para dropdowns              ║
// ║ • Update() → Atualização de abastecimento                                    ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    public interface IAbastecimentoRepository : IRepository<Abastecimento>
        {

        IEnumerable<SelectListItem> GetAbastecimentoListForDropDown();

        void Update(Abastecimento abastecimento);

        }
    }


