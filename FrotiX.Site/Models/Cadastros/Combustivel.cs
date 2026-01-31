/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: Combustivel.cs                                                                        ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para tipos de combustível e médias de preço.                                           ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • CombustivelViewModel                                                                         ║
   ║    • Combustivel                                                                                  ║
   ║    • MediaCombustivel                                                                             ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: System.ComponentModel.DataAnnotations                                              ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FrotiX.Models
    {
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: CombustivelViewModel                                                               │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Fornecer dados básicos para telas de cadastro de combustível.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Views e Controllers de cadastro
    // ➡️ CHAMA       : (sem chamadas internas)
    //
    public class CombustivelViewModel
        {
        public Guid CombustivelId { get; set; }
        }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: Combustivel                                                                        │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar um tipo de combustível cadastrado no sistema.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Repositórios e Controllers
    // ➡️ CHAMA       : DataAnnotations
    //
    public class Combustivel
        {

        [Key]
        public Guid CombustivelId { get; set; }

        [StringLength(50, ErrorMessage = "O combustível não pode exceder 50 caracteres")]
        [Required(ErrorMessage = "(O tipo de combustível é obrigatório)")]
        [Display(Name = "Tipo de Combustível")]
        public string Descricao { get; set; }

        [Display(Name = "Ativo/Inativo")]
        public bool Status { get; set; }

        }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: MediaCombustivel                                                                   │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar médias de preço por combustível, nota fiscal e período.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Relatórios/Estatísticas
    // ➡️ CHAMA       : DataAnnotations, Column(Order)
    //
    // ⚠️ ATENÇÃO:
    // Chave composta: NotaFiscalId + CombustivelId + Ano + Mes.
    //
    public class MediaCombustivel
        {

        [Key, Column(Order = 0)]
        public Guid NotaFiscalId { get; set; }

        [Key, Column(Order = 1)]
        public Guid CombustivelId { get; set; }

        [Key, Column(Order = 2)]
        public int Ano { get; set; }

        [Key, Column(Order = 3)]
        public int Mes { get; set; }

        public double PrecoMedio { get; set; }

        }

    }

