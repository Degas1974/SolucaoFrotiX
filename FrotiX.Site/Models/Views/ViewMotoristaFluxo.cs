// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMotoristaFluxo.cs                                              ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição de motoristas no fluxo Economildo.                 ║
// ║ Estrutura simplificada para seleção rápida em dropdowns de fluxo.           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MotoristaId - Identificador único do motorista                            ║
// ║ • NomeMotorista - Nome completo para exibição                               ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • Usado em telas de fluxo Economildo para seleção de motoristas             ║
// ║ • Integra com ViewFluxoEconomildo e ViewFluxoEconomildoData                 ║
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
    public class ViewMotoristaFluxo
        {

        public string? MotoristaId { get; set; }

        public string? NomeMotorista { get; set; }

        }
    }


