/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewEmpenhoMulta.cs                                                                    ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de empenhos de multas (saldos e movimentações).                              ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: EmpenhoMultaId, OrgaoAutuanteId, NotaEmpenho, SaldoInicial/Atual, Movimentacoes          ║
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
    public class ViewEmpenhoMulta
        {

        public Guid EmpenhoMultaId { get; set; }

        public Guid OrgaoAutuanteId { get; set; }

        public string? NotaEmpenho { get; set; }

        public int? AnoVigencia { get; set; }

        public double? SaldoInicial { get; set; }

        public double? SaldoAtual { get; set; }

        public double? SaldoMovimentacao { get; set; }

        public double? SaldoMultas { get; set; }

        public int? Movimentacoes { get; set; }

        }
    }


