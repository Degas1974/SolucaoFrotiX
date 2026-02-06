/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas mensais de abastecimento.
 *
 * 📥 ENTRADAS     : Ano, mês, totais e litros.
 *
 * 📤 SAÍDAS       : Registro para análises e relatórios.
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
     * ⚡ MODEL: EstatisticaAbastecimentoMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas mensais de abastecimento.
     *
     * 📥 ENTRADAS     : Ano, mês, totais e litros.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoMensal")]
    public class EstatisticaAbastecimentoMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

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
