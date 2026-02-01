/* ****************************************************************************************
 * ⚡ ARQUIVO: Lavador.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Definir entidade e ViewModel para cadastro de lavadores.
 *
 * 📥 ENTRADAS     : Dados pessoais, contrato e arquivo de foto.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de lavadores e gestão de contratos.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaLista, ForeignKey, IFormFile.
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.AspNetCore.Http.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: LavadorViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Reunir dados do lavador e lista de contratos para UI.
     *
     * 📥 ENTRADAS     : Lavador, ContratoId e lista de contratos.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de lavadores.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class LavadorViewModel
    {
        // Identificador do lavador exibido/alterado na tela.
        public Guid LavadorId { get; set; }

        // Contrato selecionado na interface.
        public Guid ContratoId { get; set; }

        // Entidade principal associada ao formulário.
        public Lavador Lavador { get; set; }

        // Nome do usuário responsável pela última alteração (uso exibicional).
        public string NomeUsuarioAlteracao { get; set; }

        // Lista de contratos para seleção.
        public IEnumerable<SelectListItem> ContratoList { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: Lavador
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar lavador vinculado a contrato.
     *
     * 📥 ENTRADAS     : Dados pessoais, contato, contrato e foto.
     *
     * 📤 SAÍDAS       : Registro persistido para gestão de lavadores.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ValidaLista, ForeignKey, NotMapped.
     ****************************************************************************************/
    public class Lavador
    {
        // Identificador único do lavador.
        [Key]
        public Guid LavadorId { get; set; }

        // Nome completo do lavador.
        [StringLength(100 , ErrorMessage = "o Nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Lavador")]
        public string? Nome { get; set; }

        // Ponto / matrícula do lavador.
        [StringLength(20 , ErrorMessage = "o Ponto não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O Ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto { get; set; }

        // Data de nascimento.
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de nascimento é obrigatória)")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento { get; set; }

        // CPF do lavador.
        [StringLength(20 , ErrorMessage = "O CPF não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O CPF é obrigatório)")]
        [Display(Name = "CPF")]
        public string? CPF { get; set; }

        // Primeiro celular.
        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O celular é obrigatório)")]
        [Display(Name = "Primeiro Celular")]
        public string? Celular01 { get; set; }

        // Segundo celular (opcional).
        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Display(Name = "Segundo Celular")]
        public string? Celular02 { get; set; }

        // Data de ingresso na empresa/contrato.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Ingresso")]
        public DateTime? DataIngresso { get; set; }

        // Foto armazenada em bytes.
        public byte[]? Foto { get; set; }

        // Flag de status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        // Data da última alteração.
        public DateTime? DataAlteracao { get; set; }

        // Usuário que realizou a última alteração.
        [Required]
        public string? UsuarioIdAlteracao { get; set; }

        // Contrato associado ao lavador.
        [ValidaLista(ErrorMessage = "(O contrato é obrigatório)")]
        [Display(Name = "Contrato")]
        public Guid? ContratoId { get; set; }

        // Navegação para contrato.
        [ForeignKey("ContratoId")]
        public virtual Contrato Contrato { get; set; }

        // Arquivo de foto enviado na UI (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto { get; set; }
    }
}
