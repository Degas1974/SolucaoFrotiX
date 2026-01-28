// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MotoristaContrato.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Motorista e Contrato.                  ║
// ║ Permite histórico de contratos por motorista ao longo do tempo.             ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • MotoristaoContratoViewModel - ViewModel simples (note o typo histórico)   ║
// ║ • MotoristaContrato - Entidade de relacionamento                            ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • MotoristaId [Key, Order=0] - ID do motorista                              ║
// ║ • ContratoId [Key, Order=1] - ID do contrato                                ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Histórico de vínculos contratuais do motorista                            ║
// ║ • Permite motorista mudar de contrato mantendo histórico                    ║
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
    public class MotoristaoContratoViewModel
    {
        public Guid MotoristaId { get; set; }
        public Guid ContratoId { get; set; }
        public MotoristaContrato? MotoristaContrato { get; set; }
    }

    public class MotoristaContrato
    {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid MotoristaId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
