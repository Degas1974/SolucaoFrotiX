/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoCombustivel.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas de abastecimento por combustível.
 *
 * 📥 ENTRADAS     : Ano, mês, tipo de combustível e totais.
 *
 * 📤 SAÍDAS       : Registro para relatórios e dashboards.
 *
 * 🔗 CHAMADA POR  : Relatórios de consumo e custos.
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
     * ⚡ MODEL: EstatisticaAbastecimentoCombustivel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas por tipo de combustível.
     *
     * 📥 ENTRADAS     : Ano, mês, tipo, totais e média por litro.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoCombustivel")]
    public class EstatisticaAbastecimentoCombustivel
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Tipo de combustível.
        [StringLength(100)]
        public string TipoCombustivel { get; set; } = string.Empty;

        // Total de abastecimentos no período.
        public int TotalAbastecimentos { get; set; }

        // Valor total abastecido.
        public decimal? ValorTotal { get; set; }

        // Total de litros abastecidos.
        public decimal? LitrosTotal { get; set; }

        // Média de valor por litro.
        public decimal? MediaValorLitro { get; set; }

        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }
    }
}
