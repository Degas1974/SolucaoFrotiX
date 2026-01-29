/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ModeloVeiculo.cs                                                                        ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModels para cadastro de modelos de veículos (Ka, Uno, Onix, etc.).     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: ModeloVeiculo (ModeloId, DescricaoModelo, MarcaId), ModeloVeiculoViewModel (MarcaList)  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: FrotiX.Validations, SelectListItem                                                         ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    public class ModeloVeiculoViewModel
    {
        public Guid ModeloId
        {
            get; set;
        }
        public ModeloVeiculo? ModeloVeiculo
        {
            get; set;
        }
        public IEnumerable<SelectListItem>? MarcaList
        {
            get; set;
        }
    }

    public class ModeloVeiculo
    {
        [Key]
        public Guid ModeloId
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "A descrição não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A descrição do modelo é obrigatória)")]
        [Display(Name = "Modelo do Veículo")]
        public string? DescricaoModelo
        {
            get; set;
        }

        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        [ValidaLista(ErrorMessage = "(A Marca é obrigatória)")]
        [Display(Name = "Marca do Veículo")]
        public Guid MarcaId
        {
            get; set;
        }

        [ForeignKey("MarcaId")]
        public virtual MarcaVeiculo? MarcaVeiculo
        {
            get; set;
        }
    }
}
