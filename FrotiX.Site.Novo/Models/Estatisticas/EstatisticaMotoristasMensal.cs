/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaMotoristasMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas mensais por motorista.
 *
 * 📥 ENTRADAS     : MotoristaId, ano, mês e totais de viagens/multas/abastecimentos.
 *
 * 📤 SAÍDAS       : Registro para relatórios e dashboards.
 *
 * 🔗 CHAMADA POR  : Relatórios de desempenho de motoristas.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    /****************************************************************************************
     * ⚡ MODEL: EstatisticaMotoristasMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas mensais por motorista.
     *
     * 📥 ENTRADAS     : MotoristaId, ano, mês e totais agregados.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, ForeignKey.
     ****************************************************************************************/
    [Table("EstatisticaMotoristasMensal")]
    public class EstatisticaMotoristasMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Identificador do motorista.
        public Guid MotoristaId { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Viagens
        // Total de viagens no período.
        public int TotalViagens { get; set; }

        // Quilometragem total.
        public decimal KmTotal { get; set; }

        // Minutos totais de viagem.
        public int MinutosTotais { get; set; }

        // Multas
        // Total de multas registradas.
        public int TotalMultas { get; set; }

        // Valor total de multas.
        public decimal ValorTotalMultas { get; set; }

        // Abastecimentos
        // Total de abastecimentos no período.
        public int TotalAbastecimentos { get; set; }

        // Total de litros abastecidos.
        public decimal LitrosTotais { get; set; }

        // Valor total abastecido.
        public decimal ValorTotalAbastecimentos { get; set; }

        // Controle
        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        // Entidade de motorista vinculada.
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
