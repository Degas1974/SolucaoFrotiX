// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewViagens.cs                                                     ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model principal para listagem e gestão de viagens da frota.            ║
// ║ Inclui dados completos da viagem, ocorrências, custos e relacionamentos.    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificadores:                                                             ║
// ║ • ViagemId - Identificador único da viagem                                  ║
// ║ • RequisitanteId, SetorSolicitanteId - Solicitante e setor                  ║
// ║ • VeiculoId, MotoristaId, UnidadeId - Recursos alocados                     ║
// ║ • ItemManutencaoId - Item de manutenção vinculado (se houver)               ║
// ║ • EventoId - Evento associado (se houver)                                   ║
// ║                                                                              ║
// ║ Dados da Viagem:                                                             ║
// ║ • Descricao - Descrição/destino da viagem                                   ║
// ║ • NoFichaVistoria - Número da ficha de vistoria                             ║
// ║ • DataInicial/Final, HoraInicio/Fim - Período da viagem                     ║
// ║ • KmInicial/Final - Quilometragem de saída/chegada                          ║
// ║ • CombustivelInicial/Final - Nível de combustível                           ║
// ║ • Status - Status da viagem (Agendada, Em Andamento, Concluída)             ║
// ║ • Finalidade - Finalidade da viagem                                         ║
// ║ • StatusAgendamento - Flag se é agendamento                                 ║
// ║ • CustoViagem - Custo calculado da viagem                                   ║
// ║                                                                              ║
// ║ Ocorrência:                                                                   ║
// ║ • ResumoOcorrencia, DescricaoOcorrencia - Detalhes do problema              ║
// ║ • DescricaoSolucaoOcorrencia - Solução aplicada                             ║
// ║ • StatusOcorrencia - Status da ocorrência                                   ║
// ║ • ImagemOcorrencia - URL da foto                                            ║
// ║                                                                              ║
// ║ Dados para Exibição:                                                         ║
// ║ • NomeRequisitante, NomeSetor, NomeMotorista - Nomes denormalizados         ║
// ║ • DescricaoVeiculo, Placa - Dados do veículo                                ║
// ║ • StatusDocumento, StatusCartaoAbastecimento - Estados de documentos        ║
// ║                                                                              ║
// ║ Upload:                                                                       ║
// ║ • FotoUpload [NotMapped] - Campo para upload de foto de ocorrência          ║
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

        public string? StatusDocumento { get; set; }

        public string? StatusCartaoAbastecimento { get; set; }

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
