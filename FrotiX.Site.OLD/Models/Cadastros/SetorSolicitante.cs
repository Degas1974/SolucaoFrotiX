/* ****************************************************************************************
 * ⚡ ARQUIVO: SetorSolicitante.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Cadastrar setores solicitantes de viagens (departamentos clientes).
 *
 * 📥 ENTRADAS     : Dados do setor, ramal e hierarquia.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de viagens e gestão de solicitantes.
 *
 * 🔄 CHAMA        : DataAnnotations, Validations.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: SetorSolicitanteViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar setor solicitante e metadados de alteração.
     *
     * 📥 ENTRADAS     : SetorSolicitante e NomeUsuarioAlteracao.
     *
     * 📤 SAÍDAS       : ViewModel para telas de edição.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de setores solicitantes.
     ****************************************************************************************/
    public class SetorSolicitanteViewModel
    {
        // Identificador do setor solicitante.
        public Guid SetorSolicitanteId { get; set; }

        // Entidade principal do formulário.
        public SetorSolicitante? SetorSolicitante { get; set; }

        // Nome do usuário responsável pela última alteração.
        public string? NomeUsuarioAlteracao { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: SetorSolicitante
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar um setor solicitante.
     *
     * 📥 ENTRADAS     : Nome, sigla, ramal e status.
     *
     * 📤 SAÍDAS       : Registro persistido do setor.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers de viagens.
     ****************************************************************************************/
    public class SetorSolicitante
    {
        // Identificador único do setor.
        [Key]
        public Guid SetorSolicitanteId { get; set; }

        // Nome do setor solicitante.
        [StringLength(200, ErrorMessage = "o Nome não pode exceder 200 caracteres")]
        [Required(ErrorMessage = "(O Nome é obrigatório)")]
        [Display(Name = "Nome do Setor")]
        public string? Nome { get; set; }

        // Sigla do setor.
        [StringLength(50, ErrorMessage = "A Sigla não pode exceder 50 caracteres")]
        [Display(Name = "Sigla")]
        public string? Sigla { get; set; }

        // Setor pai (hierarquia).
        [Display(Name = "CNH")]
        public Guid? SetorPaiId { get; set; }

        // Ramal do setor.
        [Required(ErrorMessage = "(O ramal é obrigatório)")]
        [Display(Name = "Ramal")]
        public int? Ramal { get; set; }

        // Status ativo/inativo.
        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        // Data da última alteração.
        public DateTime? DataAlteracao { get; set; }

        // Usuário responsável pela alteração.
        public string? UsuarioIdAlteracao { get; set; }
    }
}
