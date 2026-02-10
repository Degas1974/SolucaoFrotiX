/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaAbastecimentoCategoria.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas de abastecimento por categoria.
 *
 * 📥 ENTRADAS     : Ano, mês, categoria, totais e valores.
 *
 * 📤 SAÍDAS       : Registro para dashboards e análises.
 *
 * 🔗 CHAMADA POR  : Relatórios e filtros de abastecimento.
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
     * ⚡ MODEL: EstatisticaAbastecimentoCategoria
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas por categoria de abastecimento.
     *
     * 📥 ENTRADAS     : Ano, mês, categoria, totais e litros.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, StringLength.
     ****************************************************************************************/
    [Table("EstatisticaAbastecimentoCategoria")]
    public class EstatisticaAbastecimentoCategoria
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Categoria do abastecimento.
        [StringLength(100)]
        public string Categoria { get; set; } = string.Empty;

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
