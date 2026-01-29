// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRepactuacaoAtaRepository.cs                                    ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RepactuacaoAta, gerenciando repactuações de      ║
// ║ valores em Atas de Registro de Preços (atualizações monetárias).            ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRepactuacaoAtaListForDropDown() → DropDown de repactuações              ║
// ║ • Update() → Atualização de repactuação de ata                               ║
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
    /// Interface do repositório de RepactuacaoAta. Estende IRepository&lt;RepactuacaoAta&gt;.
    /// </summary>
    public interface IRepactuacaoAtaRepository : IRepository<RepactuacaoAta>
        {

        IEnumerable<SelectListItem> GetRepactuacaoAtaListForDropDown();

        void Update(RepactuacaoAta repactuacaoitemveiculoata);

        }
    }


