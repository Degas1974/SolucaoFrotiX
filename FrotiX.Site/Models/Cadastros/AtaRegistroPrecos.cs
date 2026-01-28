// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: AtaRegistroPrecos.cs                                               ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidades para gestão de Atas de Registro de Preços e seus itens.           ║
// ║ Usado para controle de aquisição de veículos via licitação.                 ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • AtaRegistroPrecosViewModel - ViewModel com dropdown de fornecedores       ║
// ║ • AtaRegistroPrecos - Entidade principal da ata                             ║
// ║ • RepactuacaoAta - Repactuações/aditivos da ata                             ║
// ║ • ItemVeiculoAta - Itens de veículos da ata                                 ║
// ║                                                                              ║
// ║ PROPRIEDADES ATAREGISTROPRECOS:                                             ║
// ║ • AtaId [Key] - Identificador único                                         ║
// ║ • NumeroAta, AnoAta - Número e ano da ata                                   ║
// ║ • AnoProcesso, NumeroProcesso - Dados do processo licitatório               ║
// ║ • Objeto - Objeto da ata                                                    ║
// ║ • DataInicio, DataFim - Vigência da ata                                     ║
// ║ • Valor - Valor total da ata                                                ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • FornecedorId → Fornecedor (FK)                                            ║
// ║                                                                              ║
// ║ VALIDAÇÕES:                                                                   ║
// ║ • [ValidaZero] - Validação customizada para campos obrigatórios             ║
// ║ • [Required] - Campos obrigatórios                                          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {
    public class AtaRegistroPrecosViewModel
        {
        public Guid AtaId { get; set; }
        public AtaRegistroPrecos AtaRegistroPrecos { get; set; }
        public IEnumerable<SelectListItem> FornecedorList { get; set; }
        }

    public class AtaRegistroPrecos
        {

        [Key]
        public Guid AtaId { get; set; }

        [ValidaZero(ErrorMessage = "(O número da Ata é obrigatório)")]
        [Required(ErrorMessage = "(O número do Ata é obrigatório)")]
        [Display(Name = "Número Ata")]
        public string? NumeroAta { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Ata")]
        public string? AnoAta { get; set; }

        [ValidaZero(ErrorMessage = "(O ano é obrigatório)")]
        [Required(ErrorMessage = "(O ano é obrigatório)")]
        [Display(Name = "Ano Processo")]
        public int? AnoProcesso { get; set; }

        [Required(ErrorMessage = "(O processo é obrigatório)")]
        [Display(Name = "Número Processo")]
        public string NumeroProcesso { get; set; }

        [Required(ErrorMessage = "(O objeto é obrigatório)")]
        [Display(Name = "Objeto da Ata")]
        public string Objeto { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(O início é obrigatório)")]
        [Display(Name = "Início da Ata")]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(O final é obrigatório)")]
        [Display(Name = "Final da Ata")]
        public DateTime? DataFim { get; set; }

        [ValidaZero(ErrorMessage = "(O valor é obrigatório)")]
        [Required(ErrorMessage = "(O valor é obrigatório)")]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor (R$)")]
        public double? Valor { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        [Display(Name = "Fornecedor")]
        public Guid FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public virtual Fornecedor Fornecedor { get; set; }

        }

    public class RepactuacaoAta
        {
        [Key]
        public Guid RepactuacaoAtaId { get; set; }

        public DateTime? DataRepactuacao { get; set; }

        public string? Descricao { get; set; }

        [Display(Name = "Ata")]
        public Guid AtaId { get; set; }

        [ForeignKey("AtaId")]
        public virtual AtaRegistroPrecos AtaRegistroPrecos { get; set; }

        }


    public class ItemVeiculoAta
        {
        [Key]
        public Guid ItemVeiculoAtaId { get; set; }

        public int? NumItem { get; set; }

        public string? Descricao { get; set; }

        public int? Quantidade { get; set; }

        public double? ValorUnitario { get; set; }

        [Display(Name = "RepactuacaoAta")]
        public Guid RepactuacaoAtaId { get; set; }

        [ForeignKey("RepactuacaoAtaId")]
        public virtual RepactuacaoAta RepactuacaoAta { get; set; }
        }



    }


