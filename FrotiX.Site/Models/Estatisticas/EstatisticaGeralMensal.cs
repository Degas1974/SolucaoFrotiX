// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: EstatisticaGeralMensal.cs                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Consolidado geral mensal com todas as principais métricas do sistema.       ║
// ║ Usado para dashboard executivo e KPIs gerenciais.                           ║
// ║                                                                              ║
// ║ MOTORISTAS:                                                                  ║
// ║ - TotalMotoristas, MotoristasAtivos, MotoristasInativos                     ║
// ║ - Efetivos, Feristas, Cobertura                                             ║
// ║                                                                              ║
// ║ VIAGENS: TotalViagens, KmTotal, HorasTotais                                 ║
// ║ MULTAS: TotalMultas, ValorTotalMultas                                       ║
// ║ ABASTECIMENTOS: TotalAbastecimentos                                         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaGeralMensal")]
    public class EstatisticaGeralMensal
    {
        [Key]
        public Guid Id { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        // Motoristas
        public int TotalMotoristas { get; set; }

        public int MotoristasAtivos { get; set; }

        public int MotoristasInativos { get; set; }

        public int Efetivos { get; set; }

        public int Feristas { get; set; }

        public int Cobertura { get; set; }

        // Viagens
        public int TotalViagens { get; set; }

        public decimal KmTotal { get; set; }

        public decimal HorasTotais { get; set; }

        // Multas
        public int TotalMultas { get; set; }

        public decimal ValorTotalMultas { get; set; }

        // Abastecimentos
        public int TotalAbastecimentos { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }
    }
}
