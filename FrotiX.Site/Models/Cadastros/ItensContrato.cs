// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ItensContrato.cs                                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Modelo auxiliar para navegação de itens de contrato.                        ║
// ║ Usado como wrapper para acesso à tela de itens de contrato.                 ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • ItensContratoViewModel - ViewModel com dropdown de contratos              ║
// ║ • ItensContrato - Modelo auxiliar (ContratoId NotMapped)                    ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ContratoId [NotMapped] - ID do contrato para navegação                    ║
// ║                                                                              ║
// ║ NOTA:                                                                         ║
// ║ • Os itens reais do contrato estão em ItemVeiculoContrato                   ║
// ║ • Esta classe é apenas para suporte à navegação na UI                       ║
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
    public class ItensContratoViewModel
        {
        public Guid ContratoId { get; set; }
        public ItensContrato ItensContrato { get; set; }

        public IEnumerable<SelectListItem> ContratoList { get; set; }


        }

    public class ItensContrato
        {

        [NotMapped]
        public Guid ContratoId { get; set; }

        }
    }


