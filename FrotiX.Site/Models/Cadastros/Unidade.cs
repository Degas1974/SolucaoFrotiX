// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Unidade.cs                                                         ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de unidades organizacionais.                         ║
// ║ Representa filiais, departamentos ou bases da organização.                  ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • UnidadeViewModel - ViewModel simples (apenas ID)                          ║
// ║ • Unidade - Entidade principal                                              ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ Identificação:                                                               ║
// ║ • UnidadeId [Key] - Identificador único                                     ║
// ║ • Sigla - Sigla da unidade (max 50 chars)                                   ║
// ║ • Descricao - Nome completo (max 100 chars)                                 ║
// ║ • Categoria - Categoria da unidade                                          ║
// ║                                                                              ║
// ║ Contatos (até 3):                                                            ║
// ║ • PontoPrimeiroContato, PrimeiroContato, PrimeiroRamal                      ║
// ║ • PontoSegundoContato, SegundoContato, SegundoRamal                         ║
// ║ • PontoTerceiroContato, TerceiroContato, TerceiroRamal                      ║
// ║                                                                              ║
// ║ Controle:                                                                     ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║ • QtdMotoristas - Quantidade de motoristas na unidade                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using FrotiX.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace FrotiX.Models
{
    public class UnidadeViewModel
    {
        public Guid UnidadeId
        {
            get; set;
        }
    }

    public class Unidade
    {
        [Key]
        public Guid UnidadeId
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "A sigla não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(A sigla da Unidade é obrigatória)")]
        [Display(Name = "Sigla da Unidade")]
        public string? Sigla
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "A descrição não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(A descrição da Unidade é obrigatória)")]
        [Display(Name = "Nome da Unidade")]
        public string? Descricao
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O ponto do contato é obrigatório)")]
        [Display(Name = "Ponto (1º)")]
        public string? PontoPrimeiroContato
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Required(ErrorMessage = "(O contato é obrigatório)")]
        [Display(Name = "Contato (1º)")]
        public string? PrimeiroContato
        {
            get; set;
        }

        [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal/Celular (1º)")]
        public long? PrimeiroRamal
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Display(Name = "Ponto (2º)")]
        public string? PontoSegundoContato
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Display(Name = "Contato (2º)")]
        public string? SegundoContato
        {
            get; set;
        }

        [Display(Name = "Ramal/Celular (2º)")]
        public long? SegundoRamal
        {
            get; set;
        }

        [StringLength(50 , ErrorMessage = "O ponto não pode exceder 50 caracteres")]
        [Display(Name = "Ponto (3º)")]
        public string? PontoTerceiroContato
        {
            get; set;
        }

        [StringLength(100 , ErrorMessage = "O nome não pode exceder 100 caracteres")]
        [Display(Name = "Contato (3º)")]
        public string? TerceiroContato
        {
            get; set;
        }

        [Display(Name = "Ramal/Celular (3º)")]
        public long? TerceiroRamal
        {
            get; set;
        }

        [Display(Name = "Ativo/Inativo")]
        public bool Status
        {
            get; set;
        }

        [Display(Name = "Categoria")]
        public string? Categoria
        {
            get; set;
        }

        [Display(Name = "Qtd Motoristas")]
        public int? QtdMotoristas
        {
            get; set;
        }
    }
}
