/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoVeiculo.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas de abastecimento por veículo.
 *
 * 📥 ENTRADAS     : Ano, veículo, placa, categoria e totais.
 *
 * 📤 SAÍDAS       : Registro para relatórios e dashboards.
 *
 * 🔗 CHAMADA POR  : Consultas de abastecimento por veículo.
 *
 * 🔄 CHAMA        : DataAnnotations, EF Core (Table).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations,
 *                   System.ComponentModel.DataAnnotations.Schema.
 **************************************************************************************** */

#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    /****************************************************************************************
     * ⚡ MODEL: EstatisticaAbastecimentoVeiculo
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas por veículo.
     *
     * 📥 ENTRADAS     : VeiculoId, placa, tipo, categoria e totais.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoVeiculo")]
    public class EstatisticaAbastecimentoVeiculo
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Placa do veículo.
        [StringLength(20)]
        public string? Placa { get; set; }

        // Tipo do veículo.
        [StringLength(100)]
        public string? TipoVeiculo { get; set; }

        // Categoria do veículo.
        [StringLength(100)]
        public string? Categoria { get; set; }

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
