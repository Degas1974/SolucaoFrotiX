/* ****************************************************************************************
 * ⚡ ARQUIVO: AspNetUsers.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Estender o IdentityUser e fornecer ViewModel para telas de usuários.
 *
 * 📥 ENTRADAS     : Dados de cadastro/autenticação e validações específicas do FrotiX.
 *
 * 📤 SAÍDAS       : Entidade Identity estendida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Identity, controllers e telas administrativas.
 *
 * 🔄 CHAMA        : DataAnnotations para validações.
 *
 * 📦 DEPENDÊNCIAS : Microsoft.AspNetCore.Identity, System.ComponentModel.DataAnnotations.
 *
 * ⚠️ ATENÇÃO      : Vários campos são redefinidos como nullable por compatibilidade.
 **************************************************************************************** */

using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: UsuarioViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar dados de usuário para uso em telas administrativas.
     *
     * 📥 ENTRADAS     : Id e entidade AspNetUsers.
     *
     * 📤 SAÍDAS       : ViewModel utilizado em views/relatórios.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de usuários.
     *
     * 🔄 CHAMA        : AspNetUsers.
     ****************************************************************************************/
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

    /****************************************************************************************
     * ⚡ MODEL: AspNetUsers
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Estender IdentityUser com campos adicionais do FrotiX.
     *
     * 📥 ENTRADAS     : Dados de autenticação, perfil e validações customizadas.
     *
     * 📤 SAÍDAS       : Entidade Identity persistida.
     *
     * 🔗 CHAMADA POR  : Identity, controllers e repositórios.
     *
     * 🔄 CHAMA        : DataAnnotations.
     *
     * ⚠️ ATENÇÃO      : Campos redefinidos como nullable para dados legados.
     ****************************************************************************************/
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
