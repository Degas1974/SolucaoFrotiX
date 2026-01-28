// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: SetorPatrimonial.cs                                                ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de setores patrimoniais.                             ║
// ║ Representa departamentos/áreas que detêm patrimônios.                       ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • SetorPatrimonial - Entidade única (sem ViewModel separada)                ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • SetorId [Key] - Identificador único                                       ║
// ║ • NomeSetor - Nome do setor (max 50 chars)                                  ║
// ║ • DetentorId - ID do usuário responsável pelo setor                         ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • SetorBaixa - Indica se é setor de baixa patrimonial                       ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Raiz da hierarquia patrimonial                                            ║
// ║ • SetorBaixa=true: setor onde bens são baixados/descartados                 ║
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

namespace FrotiX.Models.Cadastros
{
    public class SetorPatrimonial
    {
        [Key]
        public Guid SetorId { get; set; }

        [StringLength(50, ErrorMessage = "O Nome do Setor não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(Obrigatória)")]
        [Display(Name = "Nome do Setor")]
        public string? NomeSetor { get; set; }

        public string? DetentorId { get; set; }

        public bool Status { get; set; }

        public bool SetorBaixa { get; set; }
    }
}
