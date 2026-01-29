/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/ViagemEventoDto.cs                                      ║
 * ║  Descrição: DTO para relacionamento entre Viagem e Evento                ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using FrotiX.Models;
using System;

namespace FrotiX.Models
{
    public class ViagemEventoDto
    {
        public Guid EventoId
        {
            get; set;
        }

        public Guid ViagemId
        {
            get; set;
        }

        public int NoFichaVistoria
        {
            get; set;
        }
        public string NomeRequisitante
        {
            get; set;
        }
        public string NomeSetor
        {
            get; set;
        }
        public string NomeMotorista
        {
            get; set;
        }
        public string DescricaoVeiculo
        {
            get; set;
        }
        public decimal CustoViagem
        {
            get; set;
        }
        public DateTime DataInicial
        {
            get; set;
        }
        public DateTime? HoraInicio
        {
            get; set;
        } // ← DEVE SER DateTime? (não TimeSpan?)
        public string Placa
        {
            get; set;
        }
    }
}
