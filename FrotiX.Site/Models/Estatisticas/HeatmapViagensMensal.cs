/* ****************************************************************************************
 * ⚡ ARQUIVO: HeatmapViagensMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar dados para heatmap de viagens mensais.
 *
 * 📥 ENTRADAS     : Ano, mês, motorista, dia da semana e hora.
 *
 * 📤 SAÍDAS       : Registro para visualização em heatmap.
 *
 * 🔗 CHAMADA POR  : Dashboards de viagens.
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
     * ⚡ MODEL: HeatmapViagensMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar dados de heatmap de viagens.
     *
     * 📥 ENTRADAS     : Ano, mês, motorista, dia da semana e hora.
     *
     * 📤 SAÍDAS       : Entidade consultável para gráficos.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, ForeignKey.
     ****************************************************************************************/
    [Table("HeatmapViagensMensal")]
    public class HeatmapViagensMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano de referência.
        public int Ano { get; set; }

        // Mês de referência.
        public int Mes { get; set; }

        // Motorista associado (null para todos).
        public Guid? MotoristaId { get; set; }

        // Dia da semana (0=Domingo, 6=Sábado).
        public int DiaSemana { get; set; }

        // Hora do dia (0-23).
        public int Hora { get; set; }

        // Total de viagens no recorte.
        public int TotalViagens { get; set; }

        // Controle
        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        // Motorista associado (quando aplicável).
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
