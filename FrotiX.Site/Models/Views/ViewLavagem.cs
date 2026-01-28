// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewLavagem.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição de registros de lavagem de veículos.               ║
// ║ Inclui dados do veículo, motorista responsável e equipe de lavadores.       ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • LavagemId - Identificador único do registro de lavagem                    ║
// ║ • MotoristaId - Motorista responsável pelo veículo                          ║
// ║ • VeiculoId - Veículo que foi lavado                                        ║
// ║ • LavadoresId - IDs dos lavadores (separados por vírgula)                   ║
// ║                                                                              ║
// ║ Dados da Lavagem:                                                            ║
// ║ • Data - Data da lavagem                                                    ║
// ║ • HorarioInicio/HorarioFim - Horários de início e término                   ║
// ║ • DuracaoMinutos - Duração total em minutos                                 ║
// ║                                                                              ║
// ║ Dados para Exibição:                                                         ║
// ║ • Lavadores - Nomes dos lavadores (concatenados)                            ║
// ║ • DescricaoVeiculo - Descrição do veículo lavado                            ║
// ║ • Nome - Nome do motorista responsável                                      ║
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
    public class ViewLavagem
        {

        public Guid LavagemId { get; set; }

        public Guid MotoristaId { get; set; }

        public Guid VeiculoId { get; set; }

        public string? LavadoresId { get; set; }

        public string? Data { get; set; }

        public string? HorarioInicio { get; set; }

        public string? HorarioFim { get; set; }

        public int? DuracaoMinutos { get; set; }

        public string? Lavadores { get; set; }

        public string? DescricaoVeiculo { get; set; }

        public string? Nome { get; set; }

        }
    }


