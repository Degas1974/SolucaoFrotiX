/* ****************************************************************************************
 * ⚡ ARQUIVO: RegistroCupomAbastecimento.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Registrar cupons de abastecimento e seus comprovantes anexados.
 *
 * 📥 ENTRADAS     : Datas, observações e arquivo do comprovante.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Rotinas de abastecimento e auditoria.
 *
 * 🔄 CHAMA        : DataAnnotations.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ VIEWMODEL: RegistroCupomAbastecimentoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar o registro de cupom nas telas de cadastro.
     *
     * 📥 ENTRADAS     : RegistroCupomAbastecimento.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de abastecimento.
     ****************************************************************************************/
    public class RegistroCupomAbastecimentoViewModel
    {
        // Identificador do registro.
        public Guid RegistroCupomId { get; set; }

        // Entidade principal do formulário.
        public RegistroCupomAbastecimento? RegistroCupomAbastecimento { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: RegistroCupomAbastecimento
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o registro de cupons de abastecimento.
     *
     * 📥 ENTRADAS     : Data de registro, observações e PDF.
     *
     * 📤 SAÍDAS       : Registro persistido para controle.
     *
     * 🔗 CHAMADA POR  : Processos de abastecimento.
     ****************************************************************************************/
    public class RegistroCupomAbastecimento
    {
        // Identificador único do registro.
        [Key]
        public Guid RegistroCupomId { get; set; }

        // Data do registro dos cupons.
        [Display(Name = "Data do Registro dos Cupons")]
        public DateTime? DataRegistro { get; set; }

        // Observações do registro.
        public string? Observacoes { get; set; }

        // Caminho/identificador do PDF anexado.
        public string? RegistroPDF { get; set; }
    }
}
