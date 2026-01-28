// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ViewVeiculosManutencao.cs                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ View model simplificado para seleção de veículos em manutenção.             ║
// ║ Usado em dropdowns de cadastro de ordens de serviço.                        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • VeiculoId - Identificador único do veículo                                ║
// ║ • Descricao - Descrição formatada para exibição (Placa + Modelo)            ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Dropdown de veículos no cadastro de manutenção                            ║
// ║ • Lista apenas veículos elegíveis para manutenção (efetivos ativos)         ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 17                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;

namespace FrotiX.Models
    {
    public class ViewVeiculosManutencao
        {
        public String? Descricao { get; set; }
        public Guid VeiculoId { get; set; }
        }
    }

