/* ****************************************************************************************
 * ⚡ ARQUIVO: MovimentacaoEmpenho.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar movimentações de empenho (tipo, valor e data).
 *
 * 📥 ENTRADAS     : Dados financeiros e vínculo com empenho.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Módulos financeiros e relatórios.
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
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: MovimentacaoEmpenhoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar a movimentação e a lista de empenhos para seleção.
     *
     * 📥 ENTRADAS     : MovimentacaoEmpenho e EmpenhoList.
     *
     * 📤 SAÍDAS       : ViewModel para telas de movimentação.
     *
     * 🔗 CHAMADA POR  : Controllers/Views financeiras.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class MovimentacaoEmpenhoViewModel
    {
        // Identificador da movimentação.
        public Guid MovimentacaoId { get; set; }

        // Entidade principal do formulário.
        public MovimentacaoEmpenho? MovimentacaoEmpenho { get; set; }

        // Lista de empenhos disponível para seleção.
        public IEnumerable<SelectListItem>? EmpenhoList { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: MovimentacaoEmpenho
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar movimentação financeira associada a um empenho.
     *
     * 📥 ENTRADAS     : Descrição, tipo, valor e data.
     *
     * 📤 SAÍDAS       : Registro persistido da movimentação.
     *
     * 🔗 CHAMADA POR  : Fluxos financeiros.
     *
     * 🔄 CHAMA        : ForeignKey.
     ****************************************************************************************/
    public class MovimentacaoEmpenho
    {
        // Identificador único da movimentação.
        [Key]
        public Guid MovimentacaoId { get; set; }

        // Descrição da movimentação.
        public string? Descricao { get; set; }

        // Tipo de movimentação (débito/crédito).
        public string? TipoMovimentacao { get; set; }

        // Valor movimentado.
        public double? Valor { get; set; }

        // Data da movimentação.
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de emissão é obrigatória)")]
        [Display(Name = "Data de Emissão")]
        public DateTime? DataMovimentacao { get; set; }

        // Empenho associado.
        [Display(Name = "Empenho")]
        public Guid EmpenhoId { get; set; }

        // Navegação para empenho.
        [ForeignKey("EmpenhoId")]
        public virtual Empenho? Empenho { get; set; }
    }
}
