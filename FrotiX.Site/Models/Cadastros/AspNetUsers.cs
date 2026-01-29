/*
 * ╔══════════════════════════════════════════════════════════════════════════╗
 * ║  📚 DOCUMENTAÇÃO INTRA-CÓDIGO — FrotiX                                   ║
 * ║  Arquivo: Models/Cadastros/AspNetUsers.cs                                ║
 * ║  Descrição: Extensão do Identity e ViewModels para usuários do sistema  ║
 * ║  Data: 29/01/2026 | LOTE: 22                                             ║
 * ╚══════════════════════════════════════════════════════════════════════════╝
 */

using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FrotiX.Models
{
    public class UsuarioViewModel
    {
        public string? Id
        {
            get; set;
        }
        public AspNetUsers? AspNetUsers
        {
            get; set;
        }
    }

    public class AspNetUsers :IdentityUser
    {
        [Key]
        public new string? Id
        {
            get; set;
        }

        public new string? UserName
        {
            get; set;
        }
        public new string? NormalizedUserName
        {
            get; set;
        }
        [StringLength(256, ErrorMessage = "(O email deve ter no máximo 256 caracteres)")]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@camara\.leg\.br$", ErrorMessage = "(O email deve terminar em @camara.leg.br e conter apenas letras, números, ponto, hífen ou underscore)")]
        public new string? Email
        {
            get; set;
        }
        public new string? NormalizedEmail
        {
            get; set;
        }

        // MUDANÇAS AQUI: bool → bool?
        public new bool? EmailConfirmed
        {
            get; set;
        }

        public new string? PasswordHash
        {
            get; set;
        }
        public new string? SecurityStamp
        {
            get; set;
        }
        public new string? ConcurrencyStamp
        {
            get; set;
        }
        public new string? PhoneNumber
        {
            get; set;
        }

        // MUDANÇAS AQUI: bool → bool?
        public new bool? PhoneNumberConfirmed
        {
            get; set;
        }
        public new bool? TwoFactorEnabled
        {
            get; set;
        }
        public new bool? LockoutEnabled
        {
            get; set;
        }

        public new int? AccessFailedCount
        {
            get; set;
        }
        public string? Discriminator
        {
            get; set;
        }

        [Required(ErrorMessage = "(O nome completo é obrigatório)")]
        [StringLength(80, ErrorMessage = "(O nome completo deve ter no maximo 80 caracteres)")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "(O nome completo deve conter apenas letras e espacos)")]
        public string? NomeCompleto
        {
            get; set;
        }

        [Required(ErrorMessage = "(O ponto é obrigatório)")]
        [StringLength(12, ErrorMessage = "(O ponto deve ter no maximo 12 caracteres)")]
        [RegularExpression(@"^p_\d{1,10}$", ErrorMessage = "(O ponto deve ser no formato p_########## com ate 10 numeros)")]
        public string? Ponto
        {
            get; set;
        }

        // MUDANÇAS AQUI: bool → bool?
        public bool? PrecisaMudarSenha
        {
            get; set;
        }

        [Range(10000000, 99999999, ErrorMessage = "(O ramal deve ter 8 digitos e nao pode comecar com zero)")]
        public int? Ramal
        {
            get; set;
        }

        // MUDANÇAS AQUI: bool → bool?
        public bool? Status
        {
            get; set;
        }

        public byte[]? Foto
        {
            get; set;
        }
        public DateTime? Criacao
        {
            get; set;
        }
        public DateTime? UltimoLogin
        {
            get; set;
        }

        // MUDANÇAS AQUI: bool → bool?
        public bool? DetentorCargaPatrimonial
        {
            get; set;
        }

        public string? UsuarioIdAlteracao
        {
            get; set;
        }
    }
}
