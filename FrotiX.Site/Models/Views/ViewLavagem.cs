/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewLavagem.cs                                                                         ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de lavagens de veículos (horários, duração, lavadores).                      ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: LavagemId, MotoristaId, VeiculoId, Data, Horario, DuracaoMinutos                         ║
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

        public string? Horario { get; set; }

        public int? DuracaoMinutos { get; set; }

        public string? Lavadores { get; set; }

        public string? DescricaoVeiculo { get; set; }

        public string? Nome { get; set; }

        }
    }

