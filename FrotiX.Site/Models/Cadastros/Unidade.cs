/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 📌 ARQUIVO: Unidade.cs                                                                              ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🧭 OBJETIVO: Cadastrar unidades do órgão e contatos operacionais.                                   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🗂️  CONTÉM: UnidadeViewModel, Unidade                                                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: DataAnnotations, Validations                                                       ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using FrotiX.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FrotiX.Models
{
    // ==================================================================================================
    // VIEW MODEL
    // ==================================================================================================
    // Finalidade: transportar a chave da unidade em operações simples.
    // ==================================================================================================
    public class UnidadeViewModel
    {
        // Identificador da unidade.
        public Guid UnidadeId
        {
            get; set;
        }
    }

    // ==================================================================================================
    // ENTIDADE
    // ==================================================================================================
    // Representa uma unidade do órgão (base operacional).
    // ==================================================================================================
    public class Unidade
    {
        // Identificador único da unidade.
        [Key]
        public Guid UnidadeId
        {
            get; set;
        }

        // Sigla da unidade.
        [StringLength(50 , ErrorMessage = "A sigla não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A sigla da Unidade é obrigatória)")]
        [Display(Name = "Sigla da Unidade")]
        public string? Sigla
        {
            get; set;
        }

        // Descrição/nome da unidade.
        [StringLength(100 , ErrorMessage = "A descrição não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(A descrição da Unidade é obrigatória)")]
        [Display(Name = "Nome da Unidade")]
        public string? Descricao
        {
            get; set;
        }

        // Ponto do primeiro contato.
        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O ponto do contato é obrigatório)")]
        [Display(Name = "Ponto (1º)")]
        public string? PontoPrimeiroContato
        {
            get; set;
        }

        // Nome do primeiro contato.
        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O contato é obrigatório)")]
        [Display(Name = "Contato (1º)")]
        public string? PrimeiroContato
        {
            get; set;
        }

        // Ramal/celular do primeiro contato.
        [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal/Celular (1º)")]
        public long? PrimeiroRamal
        {
            get; set;
        }

        // Ponto do segundo contato.
        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Display(Name = "Ponto (2º)")]
        public string? PontoSegundoContato
        {
            get; set;
        }

        // Nome do segundo contato.
        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Display(Name = "Contato (2º)")]
        public string? SegundoContato
        {
            get; set;
        }

        // Ramal/celular do segundo contato.
        [Display(Name = "Ramal/Celular (2º)")]
        public long? SegundoRamal
        {
            get; set;
        }

        // Ponto do terceiro contato.
        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Display(Name = "Ponto (3º)")]
        public string? PontoTerceiroContato
        {
            get; set;
        }

        // Nome do terceiro contato.
        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Display(Name = "Contato (3º)")]
        public string? TerceiroContato
        {
            get; set;
        }

        // Ramal/celular do terceiro contato.
        [Display(Name = "Ramal/Celular (3º)")]
        public long? TerceiroRamal
        {
            get; set;
        }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        // Categoria da unidade.
        [Display(Name = "Categoria")]
        public string? Categoria
        {
            get; set;
        }

        // Quantidade de motoristas vinculados.
        [Display(Name = "Qtd Motoristas")]
        public int? QtdMotoristas
        {
            get; set;
        }
    }
}
