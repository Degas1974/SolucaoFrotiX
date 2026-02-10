/* ****************************************************************************************
 * ⚡ ARQUIVO: EstatisticaGeralMensal.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Consolidar estatísticas gerais mensais da frota.
 *
 * 📥 ENTRADAS     : Totais de motoristas, viagens, multas e abastecimentos.
 *
 * 📤 SAÍDAS       : Registro para relatórios gerenciais.
 *
 * 🔗 CHAMADA POR  : Dashboards e indicadores de frota.
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
     * ⚡ MODEL: EstatisticaGeralMensal
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar estatísticas gerais da frota por mês.
     *
     * 📥 ENTRADAS     : Ano, mês e totais agregados.
     *
     * 📤 SAÍDAS       : Entidade consultável para análises.
     *
     * 🔗 CHAMADA POR  : Serviços de estatísticas.
     *
     * 🔄 CHAMA        : Key.
     ****************************************************************************************/
    [Table("EstatisticaGeralMensal")]
    public class EstatisticaGeralMensal
    {
        // Identificador do registro.
        [Key]
        public Guid Id { get; set; }

        // Ano da estatística.
        public int Ano { get; set; }

        // Mês da estatística.
        public int Mes { get; set; }

        // Motoristas
        // Total de motoristas.
        public int TotalMotoristas { get; set; }

        // Total de motoristas ativos.
        public int MotoristasAtivos { get; set; }

        // Total de motoristas inativos.
        public int MotoristasInativos { get; set; }

        // Total de motoristas efetivos.
        public int Efetivos { get; set; }

        // Total de motoristas feristas.
        public int Feristas { get; set; }

        // Total de motoristas em cobertura.
        public int Cobertura { get; set; }

        // Viagens
        // Total de viagens no período.
        public int TotalViagens { get; set; }

        // Quilometragem total.
        public decimal KmTotal { get; set; }

        // Horas totais de viagem.
        public decimal HorasTotais { get; set; }

        // Multas
        // Total de multas registradas.
        public int TotalMultas { get; set; }

        // Valor total de multas.
        public decimal ValorTotalMultas { get; set; }

        // Abastecimentos
        // Total de abastecimentos no período.
        public int TotalAbastecimentos { get; set; }

        // Controle
        // Data da última atualização do agregado.
        public DateTime DataAtualizacao { get; set; }
    }
}
