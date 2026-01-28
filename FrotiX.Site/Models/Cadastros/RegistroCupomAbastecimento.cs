// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: RegistroCupomAbastecimento.cs                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para registro de lotes de cupons fiscais de abastecimento.         ║
// ║ Organiza a digitalização/arquivamento de comprovantes.                      ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • RegistroCupomAbastecimentoViewModel - ViewModel simples                   ║
// ║ • RegistroCupomAbastecimento - Entidade principal                           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • RegistroCupomId [Key] - Identificador único                               ║
// ║ • DataRegistro - Data do registro dos cupons                                ║
// ║ • Observacoes - Observações sobre o lote                                    ║
// ║ • RegistroPDF - Caminho/nome do PDF digitalizado                            ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Controle de digitalização de comprovantes fiscais                         ║
// ║ • Auditoria de abastecimentos                                               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    public class RegistroCupomAbastecimentoViewModel
    {
        public Guid RegistroCupomId { get; set; }
        public RegistroCupomAbastecimento? RegistroCupomAbastecimento { get; set; }
    }

    public class RegistroCupomAbastecimento
    {
        [Key]
        public Guid RegistroCupomId { get; set; }

        [Display(Name = "Data do Registro dos Cupons")]
        public DateTime? DataRegistro { get; set; }

        public string? Observacoes { get; set; }

        public string? RegistroPDF { get; set; }
    }
}
