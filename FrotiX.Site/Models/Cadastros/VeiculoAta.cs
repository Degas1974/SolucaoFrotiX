// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: VeiculoAta.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Veiculo e AtaRegistroPrecos.           ║
// ║ Permite histórico de atas por veículo ao longo do tempo.                    ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • VeiculoAtaViewModel - ViewModel simples                                   ║
// ║ • VeiculoAta - Entidade de relacionamento                                   ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • VeiculoId [Key, Order=0] - ID do veículo                                  ║
// ║ • AtaId [Key, Order=1] - ID da ata de registro de preços                    ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Histórico de aquisições via ata                                           ║
// ║ • Rastreabilidade de compras públicas                                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class VeiculoAtaViewModel
    {
        public Guid VeiculoId { get; set; }
        public Guid AtaId { get; set; }
        public VeiculoAta? VeiculoAta { get; set; }
    }

    public class VeiculoAta
    {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        [Key, Column(Order = 1)]
        public Guid AtaId { get; set; }
    }
}
