/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewMultas.cs                                                                         ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de multas de trânsito (motorista, veículo, valores e status).               ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: MultaId, MotoristaId, VeiculoId, OrgaoAutuanteId, Valor, Status, etc.                    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                        ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewMultas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de multas de trânsito
     *
     * 📥 ENTRADAS     : Motorista, veículo, órgão autuante, valores, PDFs
     *
     * 📤 SAÍDAS       : Registro somente leitura para gestão de multas
     *
     * 🔗 CHAMADA POR  : Telas de multas e dashboards financeiros
     *
     * 🔄 CHAMA        : Não se aplica
     ****************************************************************************************/
    public class ViewMultas
    {
        // [DADOS] Identificador único da multa
        public Guid MultaId
        {
            get; set;
        }

        // [DADOS] Identificador do motorista (FK nullable)
        public Guid? MotoristaId
        {
            get; set;
        }

        // [DADOS] Identificador do veículo (FK nullable)
        public Guid? VeiculoId
        {
            get; set;
        }

        // [DADOS] Identificador do órgão autuante (FK nullable)
        public Guid? OrgaoAutuanteId
        {
            get; set;
        }

        // [DADOS] Identificador do tipo de multa (FK nullable)
        public Guid? TipoMultaId
        {
            get; set;
        }

        // [DADOS] URL ou caminho do PDF de autuação
        public string? AutuacaoPDF
        {
            get; set;
        }

        // [DADOS] URL ou caminho do PDF de penalidade
        public string? PenalidadePDF
        {
            get; set;
        }

        // [DADOS] URL ou caminho do PDF de comprovante de pagamento
        public string? ComprovantePDF
        {
            get; set;
        }

        // [DADOS] Número da infração/auto de infração
        public string? NumInfracao
        {
            get; set;
        }

        // [DADOS] Data da infração (formatada)
        public string? Data
        {
            get; set;
        }

        // [DADOS] Hora da infração
        public string? Hora
        {
            get; set;
        }

        // [DADOS] Nome do motorista ou responsável
        public string? Nome
        {
            get; set;
        }

        // [DADOS] Placa do veículo
        public string? Placa
        {
            get; set;
        }

        // [DADOS] Telefone do órgão autuante
        public string? Telefone
        {
            get; set;
        }

        // [DADOS] Sigla do órgão/secretaria
        public string? Sigla
        {
            get; set;
        }

        // [DADOS] Localização onde ocorreu a infração
        public string? Localizacao
        {
            get; set;
        }

        // [DADOS] Artigo/lei infringida
        public string? Artigo
        {
            get; set;
        }

        // [DADOS] Data de vencimento da multa
        public string? Vencimento
        {
            get; set;
        }

        // [DADOS] Valor até data de vencimento
        public double? ValorAteVencimento
        {
            get; set;
        }

        // [DADOS] Valor pós vencimento (com multa adicional)
        public double? ValorPosVencimento
        {
            get; set;
        }

        // [DADOS] Número de processo e e-doc
        public string? ProcessoEDoc
        {
            get; set;
        }

        // [DADOS] Status da multa (aberta/paga/contestada/etc)
        public string? Status
        {
            get; set;
        }

        // [DADOS] Fase do procedimento (autuação/notificação/etc)
        public string? Fase
        {
            get; set;
        }

        // [DADOS] Descrição da infração
        public string? Descricao
        {
            get; set;
        }

        // [DADOS] Observações adicionais
        public string? Observacao
        {
            get; set;
        }

        // [DADOS] Flag indicando se multa foi paga
        public bool Paga
        {
            get; set;
        }

        // [DADOS] Data do pagamento (se aplicável)
        public string? DataPagamento
        {
            get; set;
        }

        // [DADOS] Valor pago
        public double? ValorPago
        {
            get; set;
        }
    }
}
