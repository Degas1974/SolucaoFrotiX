/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Lavador.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Definir entidade e view model para cadastro de lavadores e seleção de contrato.       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: LavadorViewModel, Lavador                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, EF Core, SelectListItem, Validations                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: reunir dados do lavador e lista de contratos para telas de cadastro/edição.
    // Observações:
    // - ContratoList é preenchida na camada de apresentação para seleção em combo.
    // - Lavador concentra os dados persistidos no banco.
    // ==================================================================================================
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

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa um lavador (funcionário) vinculado a contrato.
    // ==================================================================================================
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
