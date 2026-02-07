/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════════════════╗
║  🚀 ARQUIVO: IContratoRepository.cs                                                                              ║
║  📂 CAMINHO: Repository/IRepository/                                                                             ║
║  🎯 OBJETIVO: Interface do repositório de Contrato. Define contrato para operações com contratos                ║
║              administrativos, filtrados por tipo e status ativo.                                                   ║
║  📋 MÉTODOS DEFINIDOS:                                                                                           ║
║     • GetDropDown(tipoContrato?) → IQueryable<SelectListItem> filtrado                                          ║
║  🔗 DEPENDÊNCIAS: IRepository<Contrato>, SelectListItem                                                        ║
║  📅 Atualizado: 29/01/2026    👤 Team: FrotiX    📝 Versão: 2.0                                                 ║
╚════════════════════════════════════════════════════════════════════════════════════════════════════════════════╝ */
using System.Linq;
using FrotiX.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Repository.IRepository
    {
    public interface IContratoRepository : IRepository<Contrato>
        {
        // Status é sempre TRUE, sem parâmetro "status"
        IQueryable<SelectListItem> GetDropDown(string? tipoContrato = null);
        }
    }


