/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IAtaRegistroPrecosRepository.cs                                                                     ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de AtaRegistroPrecos, gerenciando Atas de Registro de Preços para         ║
║              contratação de veículos terceirizados.                                                               ║
║  📋 MÉTODOS ADICIONAIS:                                                                                          ║
║     • GetAtaListForDropDown(status) → DropDown filtrado por status da ata                                      ║
║     • Update() → Atualização de ata                                                                             ║
║  🔗 DEPENDÊNCIAS: IRepository<AtaRegistroPrecos>, SelectListItem                                                ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {

    // Interface do repositório de AtaRegistroPrecos. Estende IRepository&lt;AtaRegistroPrecos&gt;.

    public interface IAtaRegistroPrecosRepository : IRepository<AtaRegistroPrecos>
        {

        IEnumerable<SelectListItem> GetAtaListForDropDown(int status);

        void Update(AtaRegistroPrecos ataRegistroPrecos);

        }
    }


