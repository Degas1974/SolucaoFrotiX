// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IAtaRegistroPrecosRepository.cs                                 ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de AtaRegistroPrecos, gerenciando Atas de Registro  ║
// ║ de Preços para contratação de veículos terceirizados.                        ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS ADICIONAIS                                                           ║
// ║ • GetAtaListForDropDown(status) → DropDown filtrado por status da ata       ║
// ║ • Update() → Atualização de ata                                              ║
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
    /// Interface do repositório de AtaRegistroPrecos. Estende IRepository&lt;AtaRegistroPrecos&gt;.
    /// </summary>
    public interface IAtaRegistroPrecosRepository : IRepository<AtaRegistroPrecos>
        {

        IEnumerable<SelectListItem> GetAtaListForDropDown(int status);

        void Update(AtaRegistroPrecos ataRegistroPrecos);

        }
    }


