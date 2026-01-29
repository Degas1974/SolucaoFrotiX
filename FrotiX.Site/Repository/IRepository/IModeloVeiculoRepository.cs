// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IModeloVeiculoRepository.cs                                     ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ModeloVeiculo, gerenciando modelos de veículos   ║
// ║ por marca (Uno, Ka, Onix, Gol, Logan, etc.).                                 ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetModeloVeiculoListForDropDown() → DropDown de modelos                    ║
// ║ • Update() → Atualização de modelo                                           ║
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
    /// Interface do repositório de ModeloVeiculo. Estende IRepository&lt;ModeloVeiculo&gt;.
    /// </summary>
    public interface IModeloVeiculoRepository : IRepository<ModeloVeiculo>
        {

        IEnumerable<SelectListItem> GetModeloVeiculoListForDropDown();

        void Update(ModeloVeiculo modeloVeiculo);

        }
    }


