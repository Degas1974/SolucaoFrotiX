// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMotoristaRepository.cs                                         ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Motorista. Define contrato para operações CRUD   ║
// ║ e listas de seleção de motoristas da frota.                                   ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • GetMotoristaListForDropDown() → SelectList ordenada por nome               ║
// ║ • Update() → Atualização de motorista                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    public interface IMotoristaRepository : IRepository<Motorista>
        {

        IEnumerable<SelectListItem> GetMotoristaListForDropDown();

        void Update(Motorista motorista);

        }
    }


