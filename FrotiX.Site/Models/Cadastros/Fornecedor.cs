// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: Fornecedor.cs                                                      ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade para cadastro de fornecedores de contratos e atas.                 ║
// ║ Empresas que fornecem veículos, motoristas ou serviços terceirizados.       ║
// ║                                                                              ║
// ║ CLASSES:                                                                      ║
// ║ • FornecedorViewModel - ViewModel simples                                   ║
// ║ • Fornecedor - Entidade principal                                           ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • FornecedorId [Key] - Identificador único                                  ║
// ║ • DescricaoFornecedor - Nome/razão social do fornecedor                     ║
// ║ • CNPJ - CNPJ do fornecedor                                                 ║
// ║ • Endereco - Endereço comercial                                             ║
// ║ • Contato01/02 - Nomes dos contatos                                         ║
// ║ • Telefone01/02 - Telefones dos contatos                                    ║
// ║ • Status - Ativo/Inativo                                                    ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • Contrato.FornecedorId - Contratos do fornecedor                           ║
// ║ • AtaRegistroPrecos.FornecedorId - Atas do fornecedor                       ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Validations;

namespace FrotiX.Models
    {
    public class FornecedorViewModel
        {
        public Guid FornecedorId { get; set; }
        }

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


