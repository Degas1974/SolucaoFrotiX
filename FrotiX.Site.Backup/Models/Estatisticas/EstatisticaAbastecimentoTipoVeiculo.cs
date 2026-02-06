/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoTipoVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas de abastecimento por tipo de veículo.
 *
 * 📥 ENTRADAS     : Ano, mês, tipo de veículo, totais e litros.
 *
 * 📤 SAÍDAS       : Registro para relatórios e dashboards.
 *
 * 🔗 CHAMADA POR  : Relatórios de abastecimento.
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
     * ⚡ MODEL: EstatisticaAbastecimentoTipoVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas por tipo de veículo.
     *
     * 📥 ENTRADAS     : Ano, mês, tipo de veículo e totais.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoTipoVeiculo")]
    public class EstatisticaAbastecimentoTipoVeiculo
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Tipo de veículo.
        [StringLength(100)]
        public string TipoVeiculo { get; set; } = string.Empty;

        // Total de abastecimentos no período.
        public int TotalAbastecimentos { get; set; }

        // Valor total abastecido.
        public decimal? ValorTotal { get; set; }

        // Total de litros abastecidos.
        public decimal? LitrosTotal { get; set; }

        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }
    }
}
