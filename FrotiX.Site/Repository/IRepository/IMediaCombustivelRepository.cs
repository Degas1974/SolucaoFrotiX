// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IMediaCombustivelRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de MediaCombustivel, gerenciando médias de consumo  ║
// ║ de combustível por veículo/modelo (km/l esperado).                           ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetMediaCombustivelListForDropDown() → DropDown de médias                  ║
// ║ • Update() → Atualização de média combustível                                ║
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
    /// Interface do repositório de MediaCombustivel. Estende IRepository&lt;MediaCombustivel&gt;.
    /// </summary>
    public interface IMediaCombustivelRepository : IRepository<MediaCombustivel>
        {

        IEnumerable<SelectListItem> GetMediaCombustivelListForDropDown();

        void Update(MediaCombustivel mediacombustivel);

        }
    }


