// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMarcaVeiculoRepository.cs                                      ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MarcaVeiculo, gerenciando marcas de veículos     ║
// ║ cadastradas (Fiat, Ford, Chevrolet, VW, Renault, etc.).                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMarcaVeiculoListForDropDown() → DropDown de marcas                      ║
// ║ • Update() → Atualização de marca                                            ║
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
    /// Interface do repositório de MarcaVeiculo. Estende IRepository&lt;MarcaVeiculo&gt;.
    /// </summary>
    public interface IMarcaVeiculoRepository : IRepository<MarcaVeiculo>
        {

        IEnumerable<SelectListItem> GetMarcaVeiculoListForDropDown();

        void Update(MarcaVeiculo marcaVeiculo);

        }
    }


