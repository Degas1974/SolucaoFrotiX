/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewEmpenhoMulta.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de empenhos de multas.
 *
 * 📥 ENTRADAS     : Identificadores, saldos e movimentações.
 *
 * 📤 SAÍDAS       : DTO de leitura para relatórios financeiros.
 *
 * 🔗 CHAMADA POR  : Relatórios de multas e empenhos.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ MODEL: ViewEmpenhoMulta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de empenhos de multas.
     *
     * 📥 ENTRADAS     : Saldos e dados financeiros.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios financeiros.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewEmpenhoMulta
        {

        // Identificador do empenho de multa.
        public Guid EmpenhoMultaId { get; set; }

        // Identificador do órgão autuante.
        public Guid OrgaoAutuanteId { get; set; }

        // Nota de empenho.
        public string? NotaEmpenho { get; set; }

        // Ano de vigência.
        public int? AnoVigencia { get; set; }

        // Saldo inicial.
        public double? SaldoInicial { get; set; }

        // Saldo atual.
        public double? SaldoAtual { get; set; }

        // Saldo de movimentação.
        public double? SaldoMovimentacao { get; set; }

        // Saldo destinado a multas.
        public double? SaldoMultas { get; set; }

        // Total de movimentações.
        public int? Movimentacoes { get; set; }

        }
    }
