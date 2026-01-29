/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Views/ViewVeiculosManutencaoReserva.cs                  ║
 * ║  Descrição: Modelo mapeado da View de veículos reserva em manutenção     ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using System;

namespace FrotiX.Models
    {
    public class ViewVeiculosManutencaoReserva
        {
        public String? Descricao { get; set; }
        public Guid VeiculoId { get; set; }
        }
    }

