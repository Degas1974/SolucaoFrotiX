// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: LavadorContrato.cs                                                 ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Lavador e Contrato.                    ║
// ║ Permite que um lavador trabalhe em múltiplos contratos ao longo do tempo.   ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • LavadorContratoViewModel - ViewModel simples                              ║
// ║ • LavadorContrato - Entidade de relacionamento                              ║
// ║                                                                              ║
// ║ PROPRIEDADES (Chave Composta):                                              ║
// ║ • LavadorId [Key, Order=0] - ID do lavador                                  ║
// ║ • ContratoId [Key, Order=1] - ID do contrato                                ║
// ║                                                                              ║
// ║ NOTA:                                                                         ║
// ║ • Chave primária composta (LavadorId, ContratoId)                           ║
// ║ • Histórico de contratos por lavador                                        ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    public class LavadorContratoViewModel
        {
        public Guid LavadorId { get; set; }
        public Guid ContratoId { get; set; }
        public LavadorContrato LavadorContrato { get; set; }
        }

    public class LavadorContrato
        {
        //2 Foreign Keys as Primary Key
        //=============================
        [Key, Column(Order = 0)]
        public Guid LavadorId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }

        }
    }


