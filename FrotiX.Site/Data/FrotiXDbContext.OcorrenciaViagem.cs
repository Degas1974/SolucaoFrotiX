// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: FrotiXDbContext.OcorrenciaViagem.cs (Partial Class)                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Extensão parcial do FrotiXDbContext para módulo de Ocorrências de Viagem.    ║
// ║ Separa DbSets relacionados a ocorrências para melhor organização do código.  ║
// ║                                                                              ║
// ║ DbSets INCLUÍDOS:                                                            ║
// ║ - OcorrenciaViagem: Tabela principal de ocorrências (avarias, danos, etc.)   ║
// ║ - ViewOcorrenciasViagem: View consolidada de ocorrências por viagem          ║
// ║ - ViewOcorrenciasAbertasVeiculo: View de ocorrências pendentes por veículo   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using FrotiX.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /// <summary>
    /// Partial class - DbSets do módulo de Ocorrências de Viagem.
    /// </summary>
    public partial class FrotiXDbContext
    {
        public DbSet<OcorrenciaViagem> OcorrenciaViagem { get; set; }
        public DbSet<ViewOcorrenciasViagem> ViewOcorrenciasViagem { get; set; }
        public DbSet<ViewOcorrenciasAbertasVeiculo> ViewOcorrenciasAbertasVeiculo { get; set; }
    }
}
