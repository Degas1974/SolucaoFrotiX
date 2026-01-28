// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewLotacaoMotorista.cs                                            ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model detalhado para lotação de motorista com período e cobertura.     ║
// ║ Usado em telas de gestão de alocação de motoristas por unidade.             ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • UnidadeId - Identificador da unidade de lotação                           ║
// ║ • LotacaoMotoristaId - Identificador único da lotação                       ║
// ║ • MotoristaId - Identificador do motorista                                  ║
// ║ • Lotado - Flag indicando status atual de lotação                           ║
// ║ • Motivo - Motivo da lotação/deslotação                                     ║
// ║ • Unidade - Nome da unidade para exibição                                   ║
// ║ • DataInicial - Data de início da lotação                                   ║
// ║ • DataFim - Data de término da lotação (se houver)                          ║
// ║ • MotoristaCobertura - Motorista que cobre durante ausência                 ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ • Motorista pode ter múltiplas lotações históricas                          ║
// ║ • Cobertura é obrigatória quando motorista titular se ausenta               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    public class ViewLotacaoMotorista
        {

        public Guid UnidadeId { get; set; }

        public Guid LotacaoMotoristaId { get; set; }

        public Guid MotoristaId { get; set; }

        public bool Lotado { get; set; }

        public string? Motivo { get; set; }

        public string? Unidade { get; set; }

        public string? DataInicial { get; set; }

        public string? DataFim { get; set; }

        public string? MotoristaCobertura { get; set; }
        }
    }


