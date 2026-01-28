// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViagemEventoDto.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ DTO para listagem de viagens associadas a eventos.                          ║
// ║ Usado em grids e DataTables na tela de eventos.                             ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ - EventoId, ViagemId: Identificadores                                       ║
// ║ - NoFichaVistoria: Número da ficha de vistoria                              ║
// ║ - NomeRequisitante, NomeSetor, NomeMotorista                                ║
// ║ - DescricaoVeiculo, Placa                                                   ║
// ║ - CustoViagem: Custo calculado da viagem                                    ║
// ║ - DataInicial, HoraInicio                                                   ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 16                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

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
