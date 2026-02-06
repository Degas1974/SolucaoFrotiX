/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewEscalasCompletas.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de escalas completas de motoristas.
 *
 * 📥 ENTRADAS     : Dados de escala, motorista, veículo e cobertura.
 *
 * 📤 SAÍDAS       : DTO de leitura para listagens.
 *
 * 🔗 CHAMADA POR  : Telas de escalas e relatórios.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ MODEL: ViewEscalasCompletas
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de escalas completas.
     *
     * 📥 ENTRADAS     : Turnos, motorista, veículo, lotação e cobertura.
     *
     * 📤 SAÍDAS       : Registro somente leitura para UI.
     *
     * 🔗 CHAMADA POR  : Consultas e listagens de escalas.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewEscalasCompletas
    {
        // Identificador da escala do dia.
        public Guid? EscalaDiaId { get; set; }
        // Data da escala.
        public DateTime DataEscala { get; set; }
        // Hora de início.
        public string? HoraInicio { get; set; }
        // Hora de fim.
        public string? HoraFim { get; set; }
        // Início do intervalo.
        public string? HoraIntervaloInicio { get; set; }
        // Fim do intervalo.
        public string? HoraIntervaloFim { get; set; }
        // Número de saídas.
        public int NumeroSaidas { get; set; }
        // Status do motorista.
        public string? StatusMotorista { get; set; }
        // Lotação/lotação atual.
        public string? Lotacao { get; set; }
        // Observações da escala.
        public string? Observacoes { get; set; }

        // Motorista
        // Identificador do motorista.
        public Guid? MotoristaId { get; set; }
        // Nome do motorista.
        public string? NomeMotorista { get; set; }
        // Ponto/matrícula do motorista.
        public string? Ponto { get; set; }
        // CPF do motorista.
        public string? CPF { get; set; }
        // CNH do motorista.
        public string? CNH { get; set; }
        // Celular principal.
        public string? Celular01 { get; set; }
        // Foto do motorista.
        public byte[]? Foto { get; set; }

        // VeÃ­culo
        // Identificador do veículo.
        public Guid? VeiculoId { get; set; }
        // Placa do veículo.
        public string? Placa { get; set; }
        // Modelo do veículo.
        public string? Modelo { get; set; }
        // Descrição completa do veículo.
        public string? VeiculoDescricao { get; set; }

        // Tipo ServiÃ§o
        // Identificador do tipo de serviço.
        public Guid? TipoServicoId { get; set; }
        // Nome do serviço.
        public string? NomeServico { get; set; }

        // Turno
        // Identificador do turno.
        public Guid? TurnoId { get; set; }
        // Nome do turno.
        public string? NomeTurno { get; set; }

        // Requisitante
        // Identificador do requisitante.
        public Guid? RequisitanteId { get; set; }
        // Nome do requisitante.
        public string? NomeRequisitante { get; set; }

        // Cobertura
        // Identificador da cobertura.
        public Guid? CoberturaId { get; set; }
        // Motorista de cobertura.
        public Guid? MotoristaCoberturaId { get; set; }
        // Motorista em folga.
        public Guid? MotoristaFolgaId { get; set; }
        // Data de início da cobertura.
        public DateTime? DataInicio{ get; set; }
        // Data de fim da cobertura.
        public DateTime? DataFim { get; set; }
        // Motivo da cobertura.
        public string? MotivoCobertura { get; set; }
        // Nome do motorista cobertor.
        public string? NomeMotoristaCobertor { get; set; }
        // Nome do motorista titular.
        public string? NomeMotoristaTitular { get; set; }

        // Helpers para status visual
        /****************************************************************************************
         * ⚡ MÉTODO: GetStatusClass
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter classe CSS com base no status do motorista.
         *
         * 📥 ENTRADAS     : StatusMotorista.
         *
         * 📤 SAÍDAS       : Classe CSS para badge.
         *
         * 🔗 CHAMADA POR  : Views de escalas.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ MÉTODO: GetStatusText
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Obter texto formatado com base no status do motorista.
         *
         * 📥 ENTRADAS     : StatusMotorista.
         *
         * 📤 SAÍDAS       : Texto amigável para UI.
         *
         * 🔗 CHAMADA POR  : Views de escalas.
         *
         * 🔄 CHAMA        : Não se aplica.
         ****************************************************************************************/
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
