/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: ViagemEventoDto.cs                                                                      ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: DTO para relacionamento entre Viagem e Evento (dados simplificados).                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: ViagemEventoDto                                                                         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Models                                                                      ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Models;
using System;

namespace FrotiX.Models
{
    // ==================================================================================================
    // DTO
    // ==================================================================================================
    public class ViagemEventoDto
    {
        // Evento associado.
        public Guid EventoId
        {
            get; set;
        }

        // Viagem associada.
        public Guid ViagemId
        {
            get; set;
        }

        // Número da ficha de vistoria.
        public int NoFichaVistoria
        {
            get; set;
        }
        // Nome do requisitante.
        public string NomeRequisitante
        {
            get; set;
        }
        // Nome do setor solicitante.
        public string NomeSetor
        {
            get; set;
        }
        // Nome do motorista.
        public string NomeMotorista
        {
            get; set;
        }
        // Descrição do veículo.
        public string DescricaoVeiculo
        {
            get; set;
        }
        // Custo da viagem.
        public decimal CustoViagem
        {
            get; set;
        }
        // Data inicial.
        public DateTime DataInicial
        {
            get; set;
        }
        // Hora de início.
        public DateTime? HoraInicio
        {
            get; set;
        } // ← DEVE SER DateTime? (não TimeSpan?)
        // Placa do veículo.
        public string Placa
        {
            get; set;
        }
    }
}
