// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ILavadorRepository.cs                                           ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Lavador, gerenciando profissionais que           ║
// ║ executam serviços de lavagem de veículos.                                     ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetLavadorListForDropDown() → DropDown de lavadores                        ║
// ║ • Update() → Atualização de lavador                                          ║
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
    /// Interface do repositório de Lavador. Estende IRepository&lt;Lavador&gt;.
    /// </summary>
    public interface ILavadorRepository : IRepository<Lavador>
        {

        IEnumerable<SelectListItem> GetLavadorListForDropDown();

        void Update(Lavador lavador);

        }
    }


