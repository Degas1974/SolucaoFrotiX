// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IRegistroCupomAbastecimentoRepository.cs                        ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de RegistroCupomAbastecimento, gerenciando cupons   ║
// ║ fiscais de abastecimentos digitalizados/armazenados.                         ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetRegistroCupomAbastecimentoListForDropDown() → DropDown de cupons        ║
// ║ • Update() → Atualização de registro de cupom                                ║
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
    /// Interface do repositório de RegistroCupomAbastecimento. Estende IRepository&lt;RegistroCupomAbastecimento&gt;.
    /// </summary>
    public interface IRegistroCupomAbastecimentoRepository : IRepository<RegistroCupomAbastecimento>
        {

        IEnumerable<SelectListItem> GetRegistroCupomAbastecimentoListForDropDown();

        void Update(RegistroCupomAbastecimento registroCupomAbastecimento);

        }
    }


