#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    /****************************************************************************************
     * ⚡ MODEL: HeatmapAbastecimentoMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar dados de heatmap de abastecimentos.
     *
     * 📥 ENTRADAS     : Ano, mês, veículo/tipo, dia da semana e hora.
     *
     * 📤 SAÍDAS       : Entidade consultável para gráficos.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength.
     ****************************************************************************************/
    [Table("HeatmapAbastecimentoMensal")]
    public class HeatmapAbastecimentoMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano de referência.
        public int Ano { get; set; }

        // Mês de referência.
        public int Mes { get; set; }

        // Veículo associado (null para todos).
        public Guid? VeiculoId { get; set; }

        // Tipo de veículo (null para todos).
        [StringLength(100)]
        public string? TipoVeiculo { get; set; }

        // Dia da semana (0=Domingo, 6=Sábado).
        public int DiaSemana { get; set; }

        // Hora do dia (0-23).
        public int Hora { get; set; }

        // Total de abastecimentos no recorte.
        public int TotalAbastecimentos { get; set; }

        // Valor total abastecido.
        public decimal? ValorTotal { get; set; }

        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }
    }
}
