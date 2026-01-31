/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ItensContrato.cs                                                                      ║
   ║ 📂 CAMINHO: Models/Cadastros/                                                                     ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO DO ARQUIVO:                                                                            ║
   ║    Modelos para itens vinculados a contratos (veículos associados).                               ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 CLASSES DISPONÍVEIS:                                                                           ║
   ║    • ItensContratoViewModel                                                                       ║
   ║    • ItensContrato                                                                                ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPENDÊNCIAS: FrotiX.Services, FrotiX.Validations, SelectListItem                                ║
   ║ 📅 ATUALIZAÇÃO: 31/01/2026 | 👤 AUTOR: FrotiX Team | 📝 VERSÃO: 2.0                                 ║
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

namespace FrotiX.Models
    {
    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ItensContratoViewModel                                                            │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Agrupar item de contrato e lista de contratos para UI.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Controllers/Views de contratos
    // ➡️ CHAMA       : SelectListItem
    //
    public class ItensContratoViewModel
        {
        public Guid ContratoId { get; set; }
        public ItensContrato ItensContrato { get; set; }

        public IEnumerable<SelectListItem> ContratoList { get; set; }


    }

    // ╭───────────────────────────────────────────────────────────────────────────────────────────────╮
    // │ 🎯 CLASSE: ItensContrato                                                                     │
    // ╰───────────────────────────────────────────────────────────────────────────────────────────────╯
    //
    // 🎯 OBJETIVO:
    // Representar item de contrato com vínculo de contrato em operações de UI.
    //
    // 🔗 RASTREABILIDADE:
    // ⬅️ CHAMADO POR : Fluxos de associação de veículos a contratos
    // ➡️ CHAMA       : NotMapped
    //
    // ⚠️ ATENÇÃO:
    // ContratoId é NotMapped (usado apenas em camada de apresentação).
    //
    public class ItensContrato
        {

        [NotMapped]
        public Guid ContratoId { get; set; }

        }
    }

