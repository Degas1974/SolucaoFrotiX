/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Estatisticas/AnosDisponiveisAbastecimento.cs            ║
 * ║  Descrição: Modelo para anos disponíveis de abastecimentos (filtros)     ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models.Estatisticas
{
    [Table("AnosDisponiveisAbastecimento")]
    public class AnosDisponiveisAbastecimento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ano { get; set; }

        public int TotalAbastecimentos { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
