/* ****************************************************************************************
 * ⚡ ARQUIVO: AnosDisponiveisAbastecimento.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Disponibilizar anos com dados de abastecimento para filtros.
 *
 * 📥 ENTRADAS     : Ano, total de abastecimentos e data de atualização.
 *
 * 📤 SAÍDAS       : Registro consultável para UI e relatórios.
 *
 * 🔗 CHAMADA POR  : Dashboards e filtros de estatísticas.
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
     * ⚡ MODEL: AnosDisponiveisAbastecimento
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar anos disponíveis para análise de abastecimentos.
     *
     * 📥 ENTRADAS     : Ano, total de registros e data de atualização.
     *
     * 📤 SAÍDAS       : Entidade usada em filtros e relatórios.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, DatabaseGenerated.
     ****************************************************************************************/
    [Table("AnosDisponiveisAbastecimento")]
    public class AnosDisponiveisAbastecimento
    {
        // Ano disponível para consulta.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        // Total de abastecimentos no ano.
        public int TotalAbastecimentos { get; set; }

        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }
    }
}
