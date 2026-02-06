/* ****************************************************************************************
 * ⚡ ARQUIVO: RankingMotoristasMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar ranking mensal de motoristas.
 *
 * 📥 ENTRADAS     : Tipo de ranking, posição e métricas.
 *
 * 📤 SAÍDAS       : Registro para rankings e dashboards.
 *
 * 🔗 CHAMADA POR  : Relatórios de desempenho.
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
     * ⚡ MODEL: RankingMotoristasMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar ranking mensal de motoristas.
     *
     * 📥 ENTRADAS     : Tipo de ranking, posição e valores agregados.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength, ForeignKey.
     ****************************************************************************************/
    [Table("RankingMotoristasMensal")]
    public class RankingMotoristasMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano de referência.
        public int Ano { get; set; }

        // Mês de referência.
        public int Mes { get; set; }

        // Tipo de ranking (VIAGENS, KM, HORAS, ABASTECIMENTOS, MULTAS, PERFORMANCE).
        [StringLength(50)]
        public string TipoRanking { get; set; }

        // Posição no ranking.
        public int Posicao { get; set; }

        // Identificador do motorista.
        public Guid MotoristaId { get; set; }

        // Nome do motorista.
        [StringLength(200)]
        public string NomeMotorista { get; set; }

        // Tipo do motorista (Efetivo/Ferista/Cobertura).
        [StringLength(50)]
        public string TipoMotorista { get; set; }

        // Valores conforme o tipo de ranking
        // Valor principal (viagens/km/horas/etc).
        public decimal ValorPrincipal { get; set; }

        // Valor secundário (KM para performance, valor para multas).
        public decimal ValorSecundario { get; set; }

        // Valor terciário (horas para performance).
        public decimal ValorTerciario { get; set; }

        // Valor quaternário (multas para performance).
        public int ValorQuaternario { get; set; }

        // Controle
        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        // Motorista associado.
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
