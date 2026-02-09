/* ****************************************************************************************
 * ⚡ ARQUIVO: Patrimonio.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar bens patrimoniais com identificação, localização e status.
 *
 * 📥 ENTRADAS     : Dados do bem, localização e vínculos com setor/seção.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Gestão patrimonial e inventário.
 *
 * 🔄 CHAMA        : DataAnnotations, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

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
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: PatrimonioViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar patrimônio e listas de seleção para a UI.
     *
     * 📥 ENTRADAS     : Patrimonio e listas de marca/setor/seção.
     *
     * 📤 SAÍDAS       : ViewModel para telas patrimoniais.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de patrimônio.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class PatrimonioViewModel
    {
        // Identificador do patrimônio.
        public Guid PatrimonioId { get; set; }

        // Entidade principal do formulário.
        public Patrimonio? Patrimonio { get; set; }

        // Já estão nullable, mas poderiam ser inicializados:
        // Listas para seleção na UI.
        public IEnumerable<SelectListItem>? MarcaList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? SetorList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem>? SecaoList { get; set; } = new List<SelectListItem>();
    }

    /****************************************************************************************
     * ⚡ MODEL: Patrimonio
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um bem patrimonial do órgão.
     *
     * 📥 ENTRADAS     : Identificação, localização e status de conferência.
     *
     * 📤 SAÍDAS       : Registro persistido de patrimônio.
     *
     * 🔗 CHAMADA POR  : Gestão patrimonial, inventário e conferência.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class Patrimonio
    {
        // Identificador único do patrimônio.
        [Key]
        public Guid PatrimonioId { get; set; }

        // Número patrimonial (NPR).
        [StringLength(10, ErrorMessage = "O número do patrimônio não pode exceder 10 caracteres")]
        [Required(ErrorMessage = "O Número do Patrimônio é Obrigatório")]
        [RegularExpression(
            @"^\d+(\.\d+)?$",
            ErrorMessage = "O formato do número deve ser: números.ponto.números"
        )] //Um regex para validar queo formato é número, ponto, número, sendo os dois últimos opcionais
        [Display(Name = "NPR")]
        public string? NPR { get; set; }

        // Marca do patrimônio.
        [StringLength(30, ErrorMessage = "A marca não pode ter mais de 30 caracteres")]
        [Display(Name = "Marca")]
        public string? Marca { get; set; }

        // Modelo do patrimônio.
        [StringLength(30, ErrorMessage = "O Modelo não pode ter mais de 30 caracteres")]
        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }

        // Descrição do bem.
        [StringLength(100, ErrorMessage = "A descrição não pode passar de 50 caracteres")]
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        // Número de série.
        [StringLength(80, ErrorMessage = "O NumeroSerie não pode passar de 80 caracteres")]
        [Display(Name = "Número de Série")]
        public string? NumeroSerie { get; set; }

        // Localização atual.
        [StringLength(150, ErrorMessage = "A Localização Atual não pode passar de 150 caracteres")]
        [Display(Name = "Localização Atual")]
        [Required(ErrorMessage = "A Localização Atual é Obrigatória")]
        public string? LocalizacaoAtual { get; set; }

        // Data de entrada no patrimônio.
        public DateTime? DataEntrada { get; set; }

        // Data de saída/baixa.
        public DateTime? DataSaida { get; set; }

        // Situação do patrimônio.
        public string? Situacao { get; set; }

        // Status ativo/inativo.
        public bool Status { get; set; }

        // Status de conferência.
        public int? StatusConferencia { get; set; }

        // URL da imagem (quando armazenada externamente).
        public string? ImageUrl { get; set; }

        // Imagem armazenada em bytes.
        public byte[]? Imagem { get; set; }

        // Setor patrimonial vinculado.
        public Guid SetorId { get; set; }

        // Navegação para setor patrimonial.
        [ForeignKey("SetorId")]
        public virtual SetorPatrimonial? SetorPatrimonial { get; set; }

        // Seção patrimonial vinculada.
        public Guid SecaoId { get; set; }

        // Navegação para seção patrimonial.
        [ForeignKey("SecaoId")]
        public virtual SecaoPatrimonial? SecaoPatrimonial { get; set; }

        // Localização informada na conferência.
        public string? LocalizacaoConferencia { get; set; }

        // Setor informado na conferência.
        public Guid? SetorConferenciaId { get; set; }

        // Seção informada na conferência.
        public Guid? SecaoConferenciaId { get; set; }
    }
}
