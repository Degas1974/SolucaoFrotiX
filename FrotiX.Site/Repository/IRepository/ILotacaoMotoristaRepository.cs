// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : ILotacaoMotoristaRepository.cs                                  ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de LotacaoMotorista, gerenciando lotações de        ║
// ║ motoristas em setores/unidades organizacionais.                              ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetLotacaoMotoristaListForDropDown() → DropDown de lotações                ║
// ║ • Update() → Atualização de lotação motorista                                ║
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
    /// Interface do repositório de LotacaoMotorista. Estende IRepository&lt;LotacaoMotorista&gt;.
    /// </summary>
    public interface ILotacaoMotoristaRepository : IRepository<LotacaoMotorista>
        {

        IEnumerable<SelectListItem> GetLotacaoMotoristaListForDropDown();

        void Update(LotacaoMotorista lotacaoMotorista);

        }
    }


