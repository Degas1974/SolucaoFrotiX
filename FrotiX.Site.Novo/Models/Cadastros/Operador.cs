/* ****************************************************************************************
 * ⚡ ARQUIVO: Operador.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cadastro de operadores de frota com dados pessoais e vínculo de contrato.
 *
 * 📥 ENTRADAS     : Dados pessoais, contrato e arquivo de foto.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de operadores e gestão de contratos.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaLista, ForeignKey, IFormFile.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Http.
 **************************************************************************************** */

using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: OperadorViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agregar dados do operador e lista de contratos na UI.
     *
     * 📥 ENTRADAS     : Operador, ContratoId e lista de contratos.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de operadores.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class OperadorViewModel
    {
        // Identificador do operador.
        public Guid OperadorId
        {
            get; set;
        }

        // Contrato selecionado no formulário.
        public Guid ContratoId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Operador? Operador
        {
            get; set;
        }

        // Nome do usuário que realizou a última alteração.
        public string? NomeUsuarioAlteracao
        {
            get; set;
        }

        // Lista de contratos para seleção.
        public IEnumerable<SelectListItem>? ContratoList
        {
            get; set;
        }
    }

    /****************************************************************************************
     * ⚡ MODEL: Operador
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um operador de frota.
     *
     * 📥 ENTRADAS     : Dados pessoais, contrato e foto.
     *
     * 📤 SAÍDAS       : Registro persistido para controle de operadores.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ValidaLista, ForeignKey, NotMapped.
     ****************************************************************************************/
    public class Operador
    {
        // Identificador único do operador.
        [Key]
        public Guid OperadorId
        {
            get; set;
        }

        // Nome do operador.
        [StringLength(100 , ErrorMessage = "o Nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Operador")]
        public string? Nome
        {
            get; set;
        }

        // Ponto/matrícula do operador.
        [StringLength(20 , ErrorMessage = "o Ponto não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O Ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto
        {
            get; set;
        }

        // Data de nascimento.
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de nascimento é obrigatória)")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento
        {
            get; set;
        }

        // CPF do operador.
        [StringLength(20 , ErrorMessage = "O CPF não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O CPF é obrigatório)")]
        [Display(Name = "CPF")]
        public string? CPF
        {
            get; set;
        }

        // Primeiro celular.
        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O celular é obrigatório)")]
        [Display(Name = "Primeiro Celular")]
        public string? Celular01
        {
            get; set;
        }

        // Segundo celular (opcional).
        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Display(Name = "Segundo Celular")]
        public string? Celular02
        {
            get; set;
        }

        // Data de ingresso.
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Ingresso")]
        public DateTime? DataIngresso
        {
            get; set;
        }

        // Foto armazenada em bytes.
        public byte[]? Foto
        {
            get; set;
        }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        // Data da última alteração.
        public DateTime? DataAlteracao
        {
            get; set;
        }

        // Usuário responsável pela última alteração.
        public string? UsuarioIdAlteracao
        {
            get; set;
        }

        // Contrato associado.
        [ValidaLista(ErrorMessage = "(O contrato é obrigatório)")]
        [Display(Name = "Contrato")]
        public Guid ContratoId
        {
            get; set;
        }

        // Navegação para contrato.
        [ForeignKey("ContratoId")]
        public virtual Contrato? Contrato
        {
            get; set;
        }

        // Arquivo de foto enviado na UI (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto
        {
            get; set;
        }
    }
}
