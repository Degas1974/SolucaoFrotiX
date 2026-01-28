// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Patrimonio.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para controle de bens patrimoniais da organização.                 ║
// ║ Gerencia ativos físicos como computadores, móveis, equipamentos.            ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • PatrimonioViewModel - ViewModel com dropdowns de Marca/Setor/Secao        ║
// ║ • Patrimonio - Entidade principal                                           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificação:                                                               ║
// ║ • PatrimonioId [Key] - Identificador único                                  ║
// ║ • NPR - Número Patrimonial (regex: números.ponto.números)                   ║
// ║ • Marca, Modelo - Identificação do bem (max 30 chars)                       ║
// ║ • Descricao - Descrição detalhada (max 100 chars)                           ║
// ║ • NumeroSerie - Número de série (max 80 chars)                              ║
// ║                                                                              ║
// ║ Localização:                                                                  ║
// ║ • LocalizacaoAtual - Descrição da localização atual (max 150 chars)         ║
// ║ • SetorId → SetorPatrimonial - Setor onde está o bem                        ║
// ║ • SecaoId → SecaoPatrimonial - Seção específica                             ║
// ║ • SetorConferenciaId, SecaoConferenciaId - Local na conferência             ║
// ║ • LocalizacaoConferencia - Localização encontrada na conferência            ║
// ║                                                                              ║
// ║ Controle:                                                                     ║
// ║ • DataEntrada, DataSaida - Datas de movimentação                            ║
// ║ • Situacao - Situação atual do bem                                          ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • StatusConferencia - Status na última conferência                          ║
// ║ • Imagem, ImageUrl - Foto do patrimônio                                     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Models.Cadastros;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{ //Essa PatrimonioViewModel não faz sentido, ele só salva o objeto patrimonio dela, mais nada
    public class PatrimonioViewModel
    {
        public Guid PatrimonioId { get; set; }
        public Patrimonio? Patrimonio { get; set; }

        // Já estão nullable, mas poderiam ser inicializados:
        public IEnumerable<SelectListItem>? MarcaList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? SetorList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? SecaoList { get; set; } = new List<SelectListItem>();
    }

    public class Patrimonio
    {
        [Key]
        public Guid PatrimonioId { get; set; }

        [StringLength(10, ErrorMessage = "O número do patrimônio não pode exceder 10 caracteres")]
        [Required(ErrorMessage = "O Número do Patrimônio é Obrigatório")]
        [RegularExpression(
            @"^\d+(\.\d+)?$",
            ErrorMessage = "O formato do número deve ser: números.ponto.números"
        )] //Um regex para validar queo formato é número, ponto, número, sendo os dois últimos opcionais
        [Display(Name = "NPR")]
        public string? NPR { get; set; }

        [StringLength(30, ErrorMessage = "A marca não pode ter mais de 30 caracteres")]
        [Display(Name = "Marca")]
        public string? Marca { get; set; }

        [StringLength(30, ErrorMessage = "O Modelo não pode ter mais de 30 caracteres")]
        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }

        [StringLength(100, ErrorMessage = "A descrição não pode passar de 50 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [StringLength(80, ErrorMessage = "O NumeroSerie não pode passar de 80 caracteres")]
        [Display(Name = "Número de Série")]
        public string? NumeroSerie { get; set; }

        [StringLength(150, ErrorMessage = "A Localização Atual não pode passar de 150 caracteres")]
        [Display(Name = "Localização Atual")]
        [Required(ErrorMessage = "A Localização Atual é Obrigatória")]
        public string? LocalizacaoAtual { get; set; }

        public DateTime? DataEntrada { get; set; }

        public DateTime? DataSaida { get; set; }

        public string? Situacao { get; set; }

        public bool Status { get; set; }

        public int? StatusConferencia { get; set; }

        public string? ImageUrl { get; set; }

        public byte[]? Imagem { get; set; }

        public Guid SetorId { get; set; }

        [ForeignKey("SetorId")]
        public virtual SetorPatrimonial? SetorPatrimonial { get; set; }

        public Guid SecaoId { get; set; }

        [ForeignKey("SecaoId")]
        public virtual SecaoPatrimonial? SecaoPatrimonial { get; set; }

        public string? LocalizacaoConferencia { get; set; }

        public Guid? SetorConferenciaId { get; set; }

        public Guid? SecaoConferenciaId { get; set; }
    }
}
