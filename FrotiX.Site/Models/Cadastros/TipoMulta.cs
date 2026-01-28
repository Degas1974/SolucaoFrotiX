// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: TipoMulta.cs                                                       ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de tipos de multas de trânsito.                      ║
// ║ Catálogo de infrações baseado no Código de Trânsito Brasileiro.             ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • TipoMulta - Entidade única (sem ViewModel separada)                       ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • TipoMultaId [Key] - Identificador único                                   ║
// ║ • Artigo - Artigo/Parágrafo/Inciso do CTB (max 100 chars)                   ║
// ║ • Descricao - Descrição da infração                                         ║
// ║ • Infracao - Tipo da infração (Leve, Média, Grave, Gravíssima)              ║
// ║ • CodigoDenatran - Código oficial DENATRAN                                  ║
// ║ • Desdobramento - Desdobramento DENATRAN                                    ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Vinculação em multas para classificação                                   ║
// ║ • Relatórios por tipo de infração                                           ║
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
    public class TipoMulta
    {
        [Key]
        public Guid TipoMultaId { get; set; }

        [StringLength(100, ErrorMessage = "O artigo não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O artigo/parágrafo/inciso da multa é obrigatório)")]
        [Display(Name = "Artigo/Parágrafo/Inciso")]
        public string? Artigo { get; set; }

        [Required(ErrorMessage = "(A descrição da multa é obrigatório)")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "(A infração da multa é obrigatória)")]
        [Display(Name = "Infração")]
        public string? Infracao { get; set; }

        [Display(Name = "Código Denatran")]
        public string? CodigoDenatran { get; set; }

        [Display(Name = "Desdobramento Denatran")]
        public string? Desdobramento { get; set; }
    }
}
