/* ****************************************************************************************
 * ⚡ ARQUIVO: EvolucaoViagensDiaria.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar evolução diária de viagens para gráficos e dashboards.
 *
 * 📥 ENTRADAS     : Data, motorista (opcional), totais e métricas.
 *
 * 📤 SAÍDAS       : Registro para análises diárias.
 *
 * 🔗 CHAMADA POR  : Dashboards e relatórios de evolução.
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
     * ⚡ MODEL: EvolucaoViagensDiaria
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar evolução diária de viagens (geral ou por motorista).
     *
     * 📥 ENTRADAS     : Data, motorista, total de viagens e métricas.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key, Column, ForeignKey.
     ****************************************************************************************/
    [Table("EvolucaoViagensDiaria")]
    public class EvolucaoViagensDiaria
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Data de referência (apenas data).
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        // Motorista associado (null para total geral).
        public Guid? MotoristaId { get; set; }

        // Total de viagens no dia.
        public int TotalViagens { get; set; }

        // Quilometragem total no dia.
        public decimal KmTotal { get; set; }

        // Minutos totais no dia.
        public int MinutosTotais { get; set; }

        // Controle
        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }

        // Navegação
        // Motorista associado (quando aplicável).
        [ForeignKey("MotoristaId")]
        public virtual FrotiX.Models.Motorista Motorista { get; set; }
    }
}
