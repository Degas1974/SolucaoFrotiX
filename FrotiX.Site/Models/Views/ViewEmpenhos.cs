/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewEmpenhos.cs                                                                        ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de empenhos de contratos (saldos, vigência, dados financeiros).             ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: EmpenhoId, NotaEmpenho, DataEmissao, AnoVigencia, SaldoInicial/Final                     ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Validations                                                                         ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
