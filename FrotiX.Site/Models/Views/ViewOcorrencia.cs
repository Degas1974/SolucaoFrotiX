// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewOcorrencia.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para exibição de ocorrências registradas em viagens.             ║
// ║ Inclui dados da viagem, veículo, motorista e detalhes da ocorrência.        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • VeiculoId - Veículo da ocorrência                                         ║
// ║ • ViagemId - Viagem em que ocorreu                                          ║
// ║ • MotoristaId - Motorista responsável                                       ║
// ║ • ItemManutencaoId - Item de manutenção vinculado (se houver)               ║
// ║                                                                              ║
// ║ Dados da Viagem:                                                             ║
// ║ • NoFichaVistoria - Número da ficha de vistoria                             ║
// ║ • DataInicial - Data da viagem/ocorrência                                   ║
// ║ • NomeMotorista - Nome do motorista                                         ║
// ║ • DescricaoVeiculo - Descrição do veículo                                   ║
// ║                                                                              ║
// ║ Dados da Ocorrência:                                                         ║
// ║ • ResumoOcorrencia - Resumo curto                                           ║
// ║ • DescricaoOcorrencia - Descrição detalhada do problema                     ║
// ║ • DescricaoSolucaoOcorrencia - Solução aplicada                             ║
// ║ • StatusOcorrencia - Status (Pendente, Resolvida, Em Manutenção)            ║
// ║ • ImagemOcorrencia - URL da imagem/foto da ocorrência                       ║
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
