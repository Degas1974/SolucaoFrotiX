// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMultas.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model completo para gestão de multas de trânsito da frota.             ║
// ║ Inclui dados da infração, valores, documentos e status de pagamento.        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • MultaId - Identificador único da multa                                    ║
// ║ • MotoristaId - Motorista infrator                                          ║
// ║ • VeiculoId - Veículo autuado                                               ║
// ║ • OrgaoAutuanteId - Órgão que aplicou a multa                               ║
// ║ • TipoMultaId - Tipo/categoria da multa                                     ║
// ║                                                                              ║
// ║ Dados da Infração:                                                           ║
// ║ • NumInfracao - Número do auto de infração                                  ║
// ║ • Data, Hora - Data e hora da infração                                      ║
// ║ • Localizacao - Local da infração                                           ║
// ║ • Artigo - Artigo do CTB infringido                                         ║
// ║ • Descricao, Observacao - Detalhes da infração                              ║
// ║                                                                              ║
// ║ Valores:                                                                      ║
// ║ • ValorAteVencimento - Valor com desconto                                   ║
// ║ • ValorPosVencimento - Valor após vencimento                                ║
// ║ • Vencimento - Data de vencimento                                           ║
// ║ • ValorPago, DataPagamento, Paga - Dados de pagamento                       ║
// ║                                                                              ║
// ║ Documentos:                                                                   ║
// ║ • AutuacaoPDF - PDF do auto de infração                                     ║
// ║ • PenalidadePDF - PDF da penalidade                                         ║
// ║ • ComprovantePDF - Comprovante de pagamento                                 ║
// ║                                                                              ║
// ║ Dados para Exibição:                                                         ║
// ║ • Nome - Nome do motorista, Placa - Placa do veículo                        ║
// ║ • Sigla - Unidade, Telefone - Contato                                       ║
// ║ • Status, Fase - Status processual                                          ║
// ║ • ProcessoEDoc - Número do processo e-Doc                                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
    public class ViewMultas
    {
        public Guid MultaId
        {
            get; set;
        }

        public Guid? MotoristaId
        {
            get; set;
        }

        public Guid? VeiculoId
        {
            get; set;
        }

        public Guid? OrgaoAutuanteId
        {
            get; set;
        }

        public Guid? TipoMultaId
        {
            get; set;
        }

        public string? AutuacaoPDF
        {
            get; set;
        }

        public string? PenalidadePDF
        {
            get; set;
        }

        public string? ComprovantePDF
        {
            get; set;
        }

        public string? NumInfracao
        {
            get; set;
        }

        public string? Data
        {
            get; set;
        }

        public string? Hora
        {
            get; set;
        }

        public string? Nome
        {
            get; set;
        }

        public string? Placa
        {
            get; set;
        }

        public string? Telefone
        {
            get; set;
        }

        public string? Sigla
        {
            get; set;
        }

        public string? Localizacao
        {
            get; set;
        }

        public string? Artigo
        {
            get; set;
        }

        public string? Vencimento
        {
            get; set;
        }

        public double? ValorAteVencimento
        {
            get; set;
        }

        public double? ValorPosVencimento
        {
            get; set;
        }

        public string? ProcessoEDoc
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public string? Fase
        {
            get; set;
        }

        public string? Descricao
        {
            get; set;
        }

        public string? Observacao
        {
            get; set;
        }

        public bool Paga
        {
            get; set;
        }

        public string? DataPagamento
        {
            get; set;
        }

        public double? ValorPago
        {
            get; set;
        }
    }
}
