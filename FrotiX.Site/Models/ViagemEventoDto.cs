/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ViagemEventoDto.cs                                                                      ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: DTO para relacionamento entre Viagem e Evento (transporte de dados simplificado).     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 PROPS: EventoId, ViagemId, NoFichaVistoria, NomeRequisitante, NomeSetor, NomeMotorista, etc.     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Models                                                                              ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
