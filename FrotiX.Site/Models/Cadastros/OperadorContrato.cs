// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: OperadorContrato.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Operador e Contrato.                   ║
// ║ Permite histórico de contratos por operador ao longo do tempo.              ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • OperadorContratoViewModel - ViewModel simples                             ║
// ║ • OperadorContrato - Entidade de relacionamento                             ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • OperadorId [Key, Order=0] - ID do operador                                ║
// ║ • ContratoId [Key, Order=1] - ID do contrato                                ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Histórico de vínculos contratuais do operador                             ║
// ║ • Similar ao MotoristaContrato                                              ║
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
    public class OperadorContratoViewModel
    {
        public Guid OperadorId { get; set; }
        public Guid ContratoId { get; set; }
        public OperadorContrato? OperadorContrato { get; set; }
    }

    public class OperadorContrato
    {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid OperadorId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
