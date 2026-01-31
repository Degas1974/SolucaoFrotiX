/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Motorista.cs                                                                            ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastro de motoristas com dados pessoais, CNH, contrato e vínculo de unidade.        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: MotoristaViewModel, Motorista                                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, SelectListItem, IFormFile                                ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: agregar dados do motorista e listas de seleção usadas na UI.
    // ==================================================================================================
    public class MotoristaViewModel
    {
        // Identificador do motorista.
        public Guid MotoristaId
        {
            get; set;
        }

        // Contrato selecionado na tela (quando aplicável).
        public Guid? ContratoId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Motorista? Motorista
        {
            get; set;
        }

        // Nome do usuário que realizou a última alteração.
        public string? NomeUsuarioAlteracao
        {
            get; set;
        }

        // Lista de unidades para seleção.
        public IEnumerable<SelectListItem>? UnidadeList
        {
            get; set;
        }

        // Lista de contratos para seleção.
        public IEnumerable<SelectListItem>? ContratoList
        {
            get; set;
        }

        // Lista de condutores (quando aplicável).
        public IEnumerable<SelectListItem>? CondutorList
        {
            get; set;
        }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa o motorista com dados pessoais, documentos e vínculos.
    // ==================================================================================================
    public class Motorista
    {
        // Identificador único do motorista.
        [Key]
        public Guid MotoristaId
        {
            get; set;
        }

        // Nome do motorista.
        [StringLength(100 , ErrorMessage = "o Nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Motorista")]
        public string? Nome
        {
            get; set;
        }

        // Ponto/matrícula do motorista.
        [StringLength(20 , ErrorMessage = "o Ponto não pode exceder 20 caracteres")]
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

        // CPF do motorista.
        [StringLength(20 , ErrorMessage = "O CPF não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(O CPF é obrigatório)")]
        [Display(Name = "CPF")]
        public string? CPF
        {
            get; set;
        }

        // CNH do motorista.
        [StringLength(20 , ErrorMessage = "A CNH não pode exceder 20 caracteres")]
        [Required(ErrorMessage = "(A CNH é obrigatória)")]
        [Display(Name = "CNH")]
        public string? CNH
        {
            get; set;
        }

        // Categoria da CNH.
        [StringLength(10 , ErrorMessage = "A Categoria da CNH não pode exceder 10 caracteres")]
        [Required(ErrorMessage = "(A categoria da CNH é obrigatória)")]
        [Display(Name = "Categoria da Habilitação")]
        public string? CategoriaCNH
        {
            get; set;
        }

        // Data de vencimento da CNH.
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "(A data de vencimento da CNH é obrigatória)")]
        [Display(Name = "Data de Vencimento CNH")]
        public DateTime? DataVencimentoCNH
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
        [Required(ErrorMessage = "(A data de ingresso é obrigatória)")]
        [Display(Name = "Data de Ingresso")]
        public DateTime? DataIngresso
        {
            get; set;
        }

        // Origem da indicação do motorista.
        [Display(Name = "Origem da Indicação")]
        public string? OrigemIndicacao
        {
            get; set;
        }

        // Tipo de condutor.
        [Display(Name = "Tipo de Condutor")]
        public string? TipoCondutor
        {
            get; set;
        }

        // Foto do motorista.
        public byte[]? Foto
        {
            get; set;
        }

        // CNH digital armazenada.
        public byte[]? CNHDigital
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

        // Usuário que realizou a alteração.
        public string? UsuarioIdAlteracao
        {
            get; set;
        }

        // Código QCard (quando aplicável).
        [Display(Name = "Código QCard")]
        public int? CodMotoristaQCard
        {
            get; set;
        }

        // Unidade vinculada ao motorista.
        [Display(Name = "Unidade Vinculada")]
        public Guid? UnidadeId
        {
            get; set;
        }

        // Navegação para unidade.
        [ForeignKey("UnidadeId")]
        public virtual Unidade? Unidade
        {
            get; set;
        }

        //[ValidaLista(ErrorMessage = "(O contrato é obrigatório)")]
        //[Required(ErrorMessage = "(O contrato é obrigatório)")]
        // Contrato associado (quando aplicável).
        [Display(Name = "Contrato")]
        public Guid? ContratoId
        {
            get; set;
        }

        // Navegação para contrato.
        [ForeignKey("ContratoId")]
        public virtual Contrato? Contrato
        {
            get; set;
        }

        // Arquivo de foto enviado pela UI (não mapeado).
        [NotMapped]
        public IFormFile? ArquivoFoto
        {
            get; set;
        }

        // Indica se o motorista é efetivo ou ferista.
        [Required(ErrorMessage = "(Você deve indicar se o Motorista é Efetivo ou Ferista)")]
        [Display(Name = "Efetivo / Ferista")]
        public string? EfetivoFerista
        {
            get; set;
        }
    }
}
