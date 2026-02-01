/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewEmpenhos.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de empenhos de contratos.
 *
 * 📥 ENTRADAS     : Identificadores, saldos, vigência e dados financeiros.
 *
 * 📤 SAÍDAS       : DTO de leitura para relatórios.
 *
 * 🔗 CHAMADA POR  : Relatórios de contratos e empenhos.
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
     * ⚡ MODEL: ViewEmpenhos
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de empenhos de contratos.
     *
     * 📥 ENTRADAS     : Saldos e dados de vigência.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios financeiros.
     *
     * 🔄 CHAMA        : Key.
     ****************************************************************************************/
    public class ViewEmpenhos
    {
        // Identificador do empenho.
        [Key]
        public Guid EmpenhoId { get; set; }

        // Nota de empenho.
        public string? NotaEmpenho { get; set; }

        // Data de emissão.
        public DateTime? DataEmissao { get; set; }

        // Ano de vigência.
        public int? AnoVigencia { get; set; }

        // Início da vigência.
        public DateTime? VigenciaInicial { get; set; }

        // Fim da vigência.
        public DateTime? VigenciaFinal { get; set; }

        // Saldo inicial.
        public double? SaldoInicial { get; set; }

        // Saldo final.
        public double? SaldoFinal { get; set; }

        // Saldo de movimentação.
        public double? SaldoMovimentacao { get; set; }

        // Saldo de notas.
        public double? SaldoNotas { get; set; }

        // Total de movimentações.
        public int? Movimentacoes { get; set; }

        // Identificador do contrato (ISNULL na view).
        public Guid ContratoId { get; set; }

        // Identificador da ata.
        public Guid AtaId { get; set; }
    }
}
