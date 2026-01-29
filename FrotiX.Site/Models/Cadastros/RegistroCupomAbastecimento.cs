/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: RegistroCupomAbastecimento.cs                                                           ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade para registro de cupons de abastecimento (comprovantes físicos).             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: RegistroCupomAbastecimento (RegistroCupomId, DataRegistro, Observacoes, RegistroPDF)   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
