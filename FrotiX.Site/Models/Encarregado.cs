/* ****************************************************************************************
 * ⚡ ARQUIVO: Encarregado.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar encarregados vinculados a contratos.
 *
 * 📥 ENTRADAS     : Dados pessoais, contrato e arquivos de foto.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Telas de cadastro e manutenção de encarregados.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaLista, IFormFile.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering,
 *                   Microsoft.AspNetCore.Http.
 **************************************************************************************** */

#nullable enable
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
     * ⚡ VIEWMODEL: EncarregadoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agregar dados do encarregado e lista de contratos para a UI.
     *
     * 📥 ENTRADAS     : Encarregado, ContratoId e listas de seleção.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de encarregados.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class EncarregadoViewModel
    {
        // Identificador do encarregado.
        public Guid EncarregadoId
        {
            get; set;
        }

        // Contrato selecionado.
        public Guid ContratoId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Encarregado? Encarregado
        {
            get; set;
        }

        // Nome do usuário responsável pela última alteração.
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
     * ⚡ MODEL: Encarregado
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar encarregado vinculado a contrato.
     *
     * 📥 ENTRADAS     : Dados pessoais, contrato e foto.
     *
     * 📤 SAÍDAS       : Registro persistido para gestão de contratos.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ValidaLista, ForeignKey, IFormFile.
     ****************************************************************************************/
    public class Encarregado
    {
        // Identificador único do encarregado.
        [Key]
        public Guid EncarregadoId
        {
            get; set;
        }

        // Nome do encarregado.
        [StringLength(100 , ErrorMessage = "O Nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Encarregado")]
        public string? Nome
        {
            get; set;
        }

        // Ponto/matrícula.
        [StringLength(20 , ErrorMessage = "O Ponto não pode exceder 20 caracteres")]
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

        // CPF do encarregado.
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

        // Usuário responsável pela alteração.
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
