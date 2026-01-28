// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ModeloVeiculo.cs                                                   ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de modelos de veículos vinculados a marcas.          ║
// ║ Hierarquia: Marca → Modelo → Veículo.                                       ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • ModeloVeiculoViewModel - ViewModel com dropdown de marcas                 ║
// ║ • ModeloVeiculo - Entidade principal                                        ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • ModeloId [Key] - Identificador único                                      ║
// ║ • DescricaoModelo - Nome do modelo (max 50 chars)                           ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • MarcaId → MarcaVeiculo (FK) - Marca do modelo                             ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • Veiculo.ModeloId - Veículos deste modelo                                  ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
