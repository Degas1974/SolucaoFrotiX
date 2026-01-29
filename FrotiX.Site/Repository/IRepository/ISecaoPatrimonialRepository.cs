// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ISecaoPatrimonialRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de SecaoPatrimonial, gerenciando seções dentro de   ║
// ║ setores patrimoniais para controle hierárquico de bens.                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetSecaoListForDropDown() → DropDown de seções                             ║
// ║ • Update() → Atualização de seção patrimonial                                ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Data;
using FrotiX.Models;
using FrotiX.Models.Cadastros;
using FrotiX.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    /// <summary>
    /// Interface do repositório de SecaoPatrimonial. Estende IRepository&lt;SecaoPatrimonial&gt;.
    /// </summary>
    public interface ISecaoPatrimonialRepository : IRepository<SecaoPatrimonial>
        {

        IEnumerable<SelectListItem> GetSecaoListForDropDown();

        void Update(SecaoPatrimonial secao);

        }
    }


