// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: MarcaVeiculo.cs                                                    ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de marcas de veículos (Fiat, VW, Chevrolet, etc).    ║
// ║ Cadastro básico hierárquico: Marca → Modelo → Veículo.                      ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • MarcaVeiculoViewModel - ViewModel simples                                 ║
// ║ • MarcaVeiculo - Entidade principal                                         ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • MarcaId [Key] - Identificador único                                       ║
// ║ • DescricaoMarca - Nome da marca (max 50 chars)                             ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • ModeloVeiculo.MarcaId - Modelos desta marca                               ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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
