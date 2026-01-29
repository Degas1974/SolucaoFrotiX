/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MarcaVeiculo.cs                                                                         ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para cadastro de marcas de veículos (Ford, Fiat, etc.).         ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: MarcaVeiculo (MarcaId, DescricaoMarca, Status), MarcaVeiculoViewModel                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations                                                      ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
{
    public class MarcaVeiculoViewModel
    {
        public Guid MarcaId { get; set; }
    }

    public class MarcaVeiculo
    {
        [Key]
        public Guid MarcaId { get; set; }

        [StringLength(50, ErrorMessage = "A descrição não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A descrição da marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public string? DescricaoMarca { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }
    }
}
