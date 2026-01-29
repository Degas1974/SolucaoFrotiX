// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IViewOcorrencia.cs                                              ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de ViewOcorrencia, consultando SQL View consolidada ║
// ║ de ocorrências de veículos com dados de status e resolucao.                  ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetViewOcorrenciaListForDropDown() → DropDown de ocorrências               ║
// ║ • Update() → Não aplicável (view somente leitura)                            ║
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
    /// Interface do repositório de ViewOcorrencia. Estende IRepository&lt;ViewOcorrencia&gt;.
    /// </summary>
    public interface IViewOcorrenciaRepository : IRepository<ViewOcorrencia>
        {

        IEnumerable<SelectListItem> GetViewOcorrenciaListForDropDown();

        void Update(ViewOcorrencia viewOcorrencia);

        }
    }


