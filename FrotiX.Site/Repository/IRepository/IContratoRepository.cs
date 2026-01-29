// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ 📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                        ║
// ║ ARQUIVO    : IContratoRepository.cs                                          ║
// ║ LOCALIZAÇÃO: Repository/IRepository/                                         ║
// ║ LOTE       : 24 — Repository/IRepository                                     ║
// ║ DATA       : 29/01/2026                                                      ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ FINALIDADE                                                                   ║
// ║ Interface do repositório de Contrato. Define contrato para operações com     ║
// ║ contratos administrativos, filtrados por tipo e status ativo.                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ MÉTODOS DEFINIDOS                                                            ║
// ║ • GetDropDown(tipoContrato?) → IQueryable<SelectListItem> filtrado           ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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


