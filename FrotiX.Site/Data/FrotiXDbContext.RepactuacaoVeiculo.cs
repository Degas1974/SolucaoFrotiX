// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: FrotiXDbContext.RepactuacaoVeiculo.cs (Partial Class)               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Extensão parcial do FrotiXDbContext para módulo de Repactuação de Veículos.  ║
// ║ Gerencia histórico de reajustes contratuais de veículos da frota.            ║
// ║                                                                              ║
// ║ DbSets INCLUÍDOS:                                                            ║
// ║ - RepactuacaoVeiculo: Histórico de repactuações/reajustes de veículos        ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ - Repactuação = reajuste de valores contratuais de veículos                  ║
// ║ - Mantém histórico para auditoria e cálculos de custos                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /// <summary>
    /// Partial class - DbSet do módulo de Repactuação de Veículos.
    /// </summary>
    public partial class FrotiXDbContext
    {
        public DbSet<RepactuacaoVeiculo> RepactuacaoVeiculo { get; set; }
    }
}
