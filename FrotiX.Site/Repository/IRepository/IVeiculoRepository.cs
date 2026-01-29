// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IVeiculoRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Veículo. Define contrato para operações CRUD e   ║
// ║ listas de seleção de veículos da frota.                                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • GetVeiculoListForDropDown() → SelectList simples com placas                ║
// ║ • GetVeiculoCompletoListForDropDown() → SelectList com dados completos       ║
// ║ • Update() → Atualização de veículo                                           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    public interface IVeiculoRepository : IRepository<Veiculo>
        {

        IEnumerable<SelectListItem> GetVeiculoListForDropDown();

        void Update(Veiculo veiculo);

        IEnumerable<SelectListItem> GetVeiculoCompletoListForDropDown();

        }
    }


