/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewEmpenhos.cs                                   ║
 * ║  Descrição: Modelo mapeado da View de empenhos de contratos              ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class ViewEmpenhos
    {
        [Key]
        public Guid EmpenhoId { get; set; }

        public string? NotaEmpenho { get; set; }

        public DateTime? DataEmissao { get; set; }

        public int? AnoVigencia { get; set; }

        public DateTime? VigenciaInicial { get; set; }

        public DateTime? VigenciaFinal { get; set; }

        public double? SaldoInicial { get; set; }

        public double? SaldoFinal { get; set; }

        public double? SaldoMovimentacao { get; set; }

        public double? SaldoNotas { get; set; }

        public int? Movimentacoes { get; set; }

        // View SQL usa ISNULL para garantir GUID vazio em vez de NULL
        public Guid ContratoId { get; set; }

        public Guid AtaId { get; set; }
    }
}
