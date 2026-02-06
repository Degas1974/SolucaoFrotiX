/* ****************************************************************************************
 * ⚡ ARQUIVO: Fornecedor.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar fornecedores (empresas contratadas) e ViewModel de apoio.
 *
 * 📥 ENTRADAS     : Dados cadastrais, contatos e status.
 *
 * 📤 SAÍDAS       : Entidade persistida e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Cadastros de fornecedores e módulos de contratos/atas.
 *
 * 🔄 CHAMA        : DataAnnotations e ValidaZero.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Validations, System.ComponentModel.DataAnnotations.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ VIEWMODEL: FornecedorViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Fornecer dados básicos para telas de fornecedores.
     *
     * 📥 ENTRADAS     : Identificador do fornecedor.
     *
     * 📤 SAÍDAS       : ViewModel simplificado para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de fornecedores.
     ****************************************************************************************/
    public class FornecedorViewModel
        {
        public Guid FornecedorId { get; set; }
        }

    /****************************************************************************************
     * ⚡ MODEL: Fornecedor
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar fornecedor contratado com dados de contato.
     *
     * 📥 ENTRADAS     : Razão social, CNPJ e contatos.
     *
     * 📤 SAÍDAS       : Registro persistido para contratos/atas.
     *
     * 🔗 CHAMADA POR  : Repositórios e controllers.
     *
     * 🔄 CHAMA        : DataAnnotations, ValidaZero.
     ****************************************************************************************/
    public class Fornecedor
        {

        [Key]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "(O nome do fornecedor é obrigatório)")]
        [Display(Name = "Nome do Fornecedor")]
        public string DescricaoFornecedor { get; set; }

        [Required(ErrorMessage = "(O CNPJ do fornecedor é obrigatório)")]
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Required(ErrorMessage = "(O contato é obrigatório)")]
        [Display(Name = "Contato (1º)")]
        public string Contato01 { get; set; }

        [ValidaZero(ErrorMessage = "(O telefone é obrigatório)")]
        [Required(ErrorMessage = "(O telefone é obrigatório)")]
        [Display(Name = "Telefone/Celular (1º)")]
        public string Telefone01 { get; set; }

        [Display(Name = "Contato (2º)")]
        public string? Contato02 { get; set; }

        [Display(Name = "Telefone/Celular (2º)")]
        public string? Telefone02 { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        }
    }

