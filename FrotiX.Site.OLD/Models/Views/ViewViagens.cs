/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewViagens.cs                                                                        ║
    ║ 📂 CAMINHO: /Models/Views                                                                          ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL principal de viagens (dados completos para listagens).                      ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: ViagemId, Descricao, NoFichaVistoria, DataInicial/Final, HoraInicio/Fim, KmInicial/Final ║
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
    public class ViewViagens
    {
        public Guid ViagemId { get; set; }

        public string? Descricao { get; set; }

        public int? NoFichaVistoria { get; set; }

        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public DateTime? HoraInicio { get; set; }

        public DateTime? HoraFim { get; set; }

        public int? KmInicial { get; set; }

        public int? KmFinal { get; set; }

        public string? CombustivelInicial { get; set; }

        public string? CombustivelFinal { get; set; }

        public string? ResumoOcorrencia { get; set; }

        public string? DescricaoOcorrencia { get; set; }

        public string? DescricaoSolucaoOcorrencia { get; set; }

        public string? StatusOcorrencia { get; set; }

        public string? Status { get; set; }

        public string? NomeRequisitante { get; set; }

        public string? NomeSetor { get; set; }

        public string? NomeMotorista { get; set; }

        public string? DescricaoVeiculo { get; set; }

        public bool? DocumentoEntregue { get; set; }

        public bool? DocumentoDevolvido { get; set; }

        public bool? CartaoAbastecimentoEntregue { get; set; }

        public bool? CartaoAbastecimentoDevolvido { get; set; }

        public string? Finalidade { get; set; }

        public string? Placa { get; set; }

        public string? ImagemOcorrencia { get; set; }

        public bool StatusAgendamento { get; set; }

        public double? CustoViagem { get; set; }

        public Guid? RequisitanteId { get; set; }

        public Guid? SetorSolicitanteId { get; set; }

        public Guid? VeiculoId { get; set; }

        public Guid? MotoristaId { get; set; }

        public Guid? UnidadeId { get; set; }

        public Guid? ItemManutencaoId { get; set; }

        public Guid? EventoId { get; set; }

        [NotMapped]
        public IFormFile FotoUpload { get; set; }
    }
}
