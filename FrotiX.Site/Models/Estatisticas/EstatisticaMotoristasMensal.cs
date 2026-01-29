/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: EstatisticaMotoristasMensal.cs                                                          ║
    ║ 📂 CAMINHO: /Models/Estatisticas                                                                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Estatísticas mensais por motorista (viagens, km, multas, abastecimentos).            ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 ENTIDADE: EstatisticaMotoristasMensal (MotoristaId, Ano, Mes, TotalViagens, KmTotal, Multas)    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EstatisticaMotoristasMensal")]
    public class EstatisticaMotoristasMensal
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MotoristaId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        // Viagens
        public int TotalViagens { get; set; }

        public decimal KmTotal { get; set; }

        public int MinutosTotais { get; set; }

        // Multas
        public int TotalMultas { get; set; }

        public decimal ValorTotalMultas { get; set; }

        // Abastecimentos
        public int TotalAbastecimentos { get; set; }

        public decimal LitrosTotais { get; set; }

        public decimal ValorTotalAbastecimentos { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
