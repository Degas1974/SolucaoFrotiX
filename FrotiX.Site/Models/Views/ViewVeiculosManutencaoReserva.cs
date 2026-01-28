// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewVeiculosManutencaoReserva.cs                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model para seleção de veículos reserva disponíveis para manutenção.    ║
// ║ Usado quando veículo principal vai para manutenção e precisa de reserva.    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • VeiculoId - Identificador único do veículo reserva                        ║
// ║ • Descricao - Descrição formatada (Placa + Modelo)                          ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Dropdown de veículos reserva no cadastro de manutenção                    ║
// ║ • Lista apenas veículos marcados como reserva e disponíveis                 ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;

namespace FrotiX.Models
    {
    public class ViewVeiculosManutencaoReserva
        {
        public String? Descricao { get; set; }
        public Guid VeiculoId { get; set; }
        }
    }

