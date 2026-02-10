/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoVeiculoMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas mensais de abastecimento por veículo.
 *
 * 📥 ENTRADAS     : VeiculoId, ano, mês, totais e valores.
 *
 * 📤 SAÍDAS       : Registro para relatórios mensais.
 *
 * 🔗 CHAMADA POR  : Dashboards e relatórios de abastecimento.
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
     * ⚡ MODEL: EstatisticaAbastecimentoVeiculoMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas mensais por veículo.
     *
     * 📥 ENTRADAS     : VeiculoId, ano, mês e totais.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoVeiculoMensal")]
    public class EstatisticaAbastecimentoVeiculoMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

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
