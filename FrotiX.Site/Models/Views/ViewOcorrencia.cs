/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewOcorrencia.cs                                                                     ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de ocorrências do sistema (resumo, status, veículo e motorista).            ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: VeiculoId, ViagemId, NoFichaVistoria, DataInicial, ResumoOcorrencia, StatusOcorrencia    ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: FrotiX.Services, FrotiX.Validations                                                       ║
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
    public class ViewOcorrencia
    {
        public Guid VeiculoId { get; set; }

        public Guid ViagemId { get; set; }

        public int? NoFichaVistoria { get; set; }

        public string? DataInicial { get; set; }

        public string? NomeMotorista { get; set; }

        public string? DescricaoVeiculo { get; set; }

        public string? ResumoOcorrencia { get; set; }

        public string? StatusOcorrencia { get; set; }

        public Guid? MotoristaId { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public Guid? ItemManutencaoId { get; set; }

        public string? DescricaoOcorrencia { get; set; }

        public string? DescricaoSolucaoOcorrencia { get; set; }
    }
}
