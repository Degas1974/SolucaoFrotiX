/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
    ║ 🚀 ARQUIVO: ViewEscalasCompletas.cs                                                                ║
    ║ 📂 CAMINHO: /Models/Views                                                                           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🎯 OBJETIVO: View SQL de escalas completas de motoristas (turnos, veículo, lotação).               ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 📋 PROPS: EscalaDiaId, DataEscala, HoraInicio/Fim, MotoristaId, VeiculoId, Lotacao, etc.           ║
    ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
    ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
    ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
    ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    // ViewModel para listagem de escalas
    public class ViewEscalasCompletas
    {
        public Guid? EscalaDiaId { get; set; }
        public DateTime DataEscala { get; set; }
        public string? HoraInicio { get; set; }
        public string? HoraFim { get; set; }
        public string? HoraIntervaloInicio { get; set; }
        public string? HoraIntervaloFim { get; set; }
        public int NumeroSaidas { get; set; }
        public string? StatusMotorista { get; set; }
        public string? Lotacao { get; set; }
        public string? Observacoes { get; set; }
        
        // Motorista
        public Guid? MotoristaId { get; set; }
        public string? NomeMotorista { get; set; }
        public string? Ponto { get; set; }
        public string? CPF { get; set; }
        public string? CNH { get; set; }
        public string? Celular01 { get; set; }
        public byte[]? Foto { get; set; }
        
        // VeÃ­culo
        public Guid? VeiculoId { get; set; }
        public string? Placa { get; set; }
        public string? Modelo { get; set; }
        public string? VeiculoDescricao { get; set; }

        // Tipo ServiÃ§o
        public Guid? TipoServicoId { get; set; }
        public string? NomeServico { get; set; }
        
        // Turno
        public Guid? TurnoId { get; set; }
        public string? NomeTurno { get; set; }
        
        // Requisitante
        public Guid? RequisitanteId { get; set; }
        public string? NomeRequisitante { get; set; }
        
        // Cobertura
        public Guid? CoberturaId { get; set; }
        public Guid? MotoristaCoberturaId { get; set; }
        public Guid? MotoristaFolgaId { get; set; }
        public DateTime? DataInicio{ get; set; }
        public DateTime? DataFim { get; set; }
        public string? MotivoCobertura { get; set; }
        public string? NomeMotoristaCobertor { get; set; }
        public string? NomeMotoristaTitular { get; set; }

        // Helpers para status visual
        public string GetStatusClass()
        {
            return StatusMotorista?.ToLower() switch
            {
                "disponível" or "disponivel" => "badge bg-success",
                "em viagem" => "badge bg-primary",
                "indisponível" or "indisponivel" => "badge bg-danger",
                "em serviço" or "em servico" => "badge bg-warning",
                "economildo" => "badge bg-info",
                _ => "badge bg-secondary"
            };
        }

        public string GetStatusText()
        {
            return StatusMotorista?.ToLower() switch
            {
                "disponível" or "disponivel" => "Disponí­vel",
                "em viagem" => "Em Viagem",
                "indisponí­vel" or "indisponivel" => "Indisponí­vel",
                "em serviço" or "em servico" => "Em Serviço",
                "economildo" => "Economildo",
                _ => StatusMotorista ?? "Sem Status"
            };
        }
    }
}
