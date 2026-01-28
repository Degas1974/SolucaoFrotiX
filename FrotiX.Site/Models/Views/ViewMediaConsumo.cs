// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewMediaConsumo.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição da média de consumo de combustível por veículo.    ║
// ║ Usado em dashboards e relatórios de eficiência de combustível.              ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • VeiculoId - Identificador único do veículo (Guid)                         ║
// ║ • ConsumoGeral - Média de consumo geral em Km/L (decimal nullable)          ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ • Consumo calculado a partir de abastecimentos validados                    ║
// ║ • Usado para comparação entre veículos e identificação de anomalias         ║
// ║ • Integra com sistema de alertas de consumo anormal                         ║
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
    public class ViewMediaConsumo
        {

        public Guid VeiculoId { get; set; }

        public decimal? ConsumoGeral { get; set; }

        }
    }


