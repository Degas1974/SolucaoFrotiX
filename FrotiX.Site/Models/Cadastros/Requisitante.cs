/* ****************************************************************************************
 * ⚡ ARQUIVO: Requisitante.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cadastrar requisitantes de viagens (pessoas autorizadas a solicitar).
 *
 * 📥 ENTRADAS     : Dados pessoais, contato e setor solicitante.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de viagens e módulos de requisição.
 *
 * 🔄 CHAMA        : DataAnnotations, ValidaZero, ForeignKey, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: RequisitanteViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar requisitante e lista de setores solicitantes.
     *
     * 📥 ENTRADAS     : Requisitante e SetorSolicitanteList.
     *
     * 📤 SAÍDAS       : ViewModel para telas de cadastro/edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de requisitantes.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class RequisitanteViewModel
    {
        // Identificador do requisitante.
        public Guid RequisitanteId
        {
            get; set;
        }

        // Entidade principal do formulário.
        public Requisitante? Requisitante
        {
            get; set;
        }

        // Lista de setores solicitantes para seleção.
        public IEnumerable<SelectListItem>? SetorSolicitanteList
        {
            get; set;
        }
    }

    /****************************************************************************************
     * ⚡ MODEL: Requisitante
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um requisitante de viagens.
     *
     * 📥 ENTRADAS     : Nome, ponto, ramal e setor solicitante.
     *
     * 📤 SAÍDAS       : Registro persistido para solicitações de viagem.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers de viagens.
     *
     * 🔄 CHAMA        : ValidaZero, ForeignKey.
     ****************************************************************************************/
    public class Requisitante
    {
        // Identificador único do requisitante.
        [Key]
        public Guid RequisitanteId
        {
            get; set;
        }

        // Nome do requisitante.
        [Required(ErrorMessage = "(O nome do requisitante é obrigatório)")]
        [Display(Name = "Requisitante")]
        public string? Nome
        {
            get; set;
        }

        // Ponto/matrícula.
        [Required(ErrorMessage = "(O ponto é obrigatório)")]
        [Display(Name = "Ponto")]
        public string? Ponto
        {
            get; set;
        }

        // Ramal de contato.
        [ValidaZero(ErrorMessage = "(O ramal é obrigatório)")]
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal
        {
            get; set;
        }

        // Email de contato.
        [Display(Name = "Email")]
        public string? Email
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

        // Setor solicitante vinculado.
        [Display(Name = "Setor Solicitante")]
        public Guid SetorSolicitanteId
        {
            get; set;
        }

        // Navegação para setor solicitante.
        [ForeignKey("SetorSolicitanteId")]
        public virtual SetorSolicitante? SetorSolicitante
        {
            get; set;
        }
    }
}
