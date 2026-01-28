// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: VeiculoContrato.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Veiculo e Contrato.                    ║
// ║ Permite histórico de contratos por veículo (trocas de locadora).            ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • VeiculoContratoViewModel - ViewModel simples                              ║
// ║ • VeiculoContrato - Entidade de relacionamento                              ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • VeiculoId [Key, Order=0] - ID do veículo                                  ║
// ║ • ContratoId [Key, Order=1] - ID do contrato                                ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Histórico de vínculos contratuais do veículo                              ║
// ║ • Auditoria de locações                                                     ║
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
    public class VeiculoContratoViewModel
    {
        public Guid VeiculoId { get; set; }
        public Guid ContratoId { get; set; }
        public VeiculoContrato? VeiculoContrato { get; set; }
    }

    public class VeiculoContrato
    {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
