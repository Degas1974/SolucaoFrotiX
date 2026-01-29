/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Encarregado.cs                                                                          ║
   ║ 📂 CAMINHO: /Models                                                                                 ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade e ViewModel para gerenciamento de encarregados de contrato, com dados        ║
   ║    pessoais, vínculo com contrato e funcionalidades de gerenciamento.                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES: EncarregadoViewModel, Encarregado                                                       ║
   ║    PROPS: EncarregadoId, ContratoId, Nome, Email, Telefone, Status                                  ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: System.ComponentModel.DataAnnotations, FrotiX.Validations                                  ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    public class EncarregadoViewModel
    {
        public Guid EncarregadoId
        {
            get; set;
        }

        public Guid ContratoId
        {
            get; set;
        }

        public Encarregado? Encarregado
        {
            get; set;
        }

        public string? NomeUsuarioAlteracao
        {
            get; set;
        }

        public IEnumerable<SelectListItem>? ContratoList
        {
            get; set;
        }
    }

    public class Encarregado
    {
        [Key]
        public Guid EncarregadoId
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "O Nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Encarregado")]
        public string? Nome
        {
            get; set;
        }

        [StringLength(20 , ErrorMessage = "O Ponto não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O Ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto
        {
            get; set;
        }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de nascimento é obrigatória)")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? DataNascimento
        {
            get; set;
        }

        [StringLength(20 , ErrorMessage = "O CPF não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O CPF é obrigatório)")]
        [Display(Name = "CPF")]
        public string? CPF
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O celular é obrigatório)")]
        [Display(Name = "Primeiro Celular")]
        public string? Celular01
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "O celular não pode exceder 50 caracteres")]
        [Display(Name = "Segundo Celular")]
        public string? Celular02
        {
            get; set;
        }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Ingresso")]
        public DateTime? DataIngresso
        {
            get; set;
        }

        public byte[]? Foto
        {
            get; set;
        }

        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        public DateTime? DataAlteracao
        {
            get; set;
        }

        public string? UsuarioIdAlteracao
        {
            get; set;
        }

        [ValidaLista(ErrorMessage = "(O contrato é obrigatório)")]
        [Display(Name = "Contrato")]
        public Guid ContratoId
        {
            get; set;
        }

        [ForeignKey("ContratoId")]
        public virtual Contrato? Contrato
        {
            get; set;
        }

        [NotMapped]
        public IFormFile? ArquivoFoto
        {
            get; set;
        }
    }
}
