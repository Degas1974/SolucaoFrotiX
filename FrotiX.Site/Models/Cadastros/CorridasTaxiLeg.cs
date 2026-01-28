// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: CorridasTaxiLeg.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de corridas do serviço TaxiLeg (táxi legislativo).   ║
// ║ Armazena dados importados do sistema externo de táxi.                       ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • CorridasTaxiLegViewModel - ViewModel para filtros                         ║
// ║ • CorridasTaxiLeg - Entidade principal de corridas                          ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • CorridaId [Key] - Identificador único                                     ║
// ║ • QRU - Código QRU da corrida                                               ║
// ║ • Origem - Origem da solicitação                                            ║
// ║ • Setor, DescSetor - Setor solicitante                                      ║
// ║ • Unidade, DescUnidade - Unidade solicitante                                ║
// ║ • QtdPassageiros - Quantidade de passageiros                                ║
// ║ • MotivoUso - Motivo da corrida                                             ║
// ║ • DataAgenda, DataFinal - Datas da corrida                                  ║
// ║ • HoraAgenda/Aceite/Local/Inicio/Final - Horários detalhados                ║
// ║ • KmReal - Quilometragem real percorrida                                    ║
// ║ • QtdEstrelas, Avaliacao - Avaliação do serviço                             ║
// ║ • Duracao, Espera - Tempos em minutos                                       ║
// ║ • OrigemCorrida, DestinoCorrida - Endereços                                 ║
// ║ • Glosa, ValorGlosa - Dados de glosa se aplicável                           ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
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
    public class CorridasTaxiLegViewModel
        {
        public Guid AbastecimentoId { get; set; }
        public Guid VeiculoId { get; set; }
        public Guid MotoristaId { get; set; }
        public Guid CombustivelId { get; set; }
        }

    public class CorridasTaxiLeg
        {
        [Key]
        public Guid CorridaId { get; set; }

        public string? QRU { get; set; }

        public string? Origem { get; set; }

        public string? Setor { get; set; }

        public string? DescSetor { get; set; }

        public string? Unidade { get; set; }

        public string? DescUnidade { get; set; }

        public int? QtdPassageiros { get; set; }

        public string? MotivoUso { get; set; }

        public DateTime? DataAgenda { get; set; }

        public DateTime? DataFinal { get; set; }

        public string? HoraAgenda { get; set; }

        public string? HoraAceite { get; set; }

        public string? HoraLocal { get; set; }

        public string? HoraInicio { get; set; }

        public string? HoraFinal { get; set; }

        public double? KmReal { get; set; }

        public int? QtdEstrelas { get; set; }

        public string? Avaliacao { get; set; }

        public int? Duracao { get; set; }

        public int? Espera { get; set; }

        public string? OrigemCorrida { get; set; }

        public string? DestinoCorrida { get; set; }

        public bool Glosa { get; set; }

        public double? ValorGlosa { get; set; }
        }
    }


