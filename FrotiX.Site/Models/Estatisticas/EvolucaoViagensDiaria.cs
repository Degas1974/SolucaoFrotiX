/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: EvolucaoViagensDiaria.cs                                                                ║
    ║ 📂 CAMINHO: /Models/Estatisticas                                                                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: Evolução diária de viagens para gráficos e dashboards (por motorista ou geral).      ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 ENTIDADE: EvolucaoViagensDiaria (Data, MotoristaId, TotalViagens, KmTotal, MinutosTotais)       ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("EvolucaoViagensDiaria")]
    public class EvolucaoViagensDiaria
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public Guid? MotoristaId { get; set; } // NULL = todos os motoristas

        public int TotalViagens { get; set; }

        public decimal KmTotal { get; set; }

        public int MinutosTotais { get; set; }

        // Controle
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
