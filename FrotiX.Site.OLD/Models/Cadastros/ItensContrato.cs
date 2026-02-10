/* ****************************************************************************************
 * ⚡ ARQUIVO: ItensContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar itens vinculados a contratos e ViewModel de apoio.
 *
 * 📥 ENTRADAS     : Itens e vínculos com contrato.
 *
 * 📤 SAÍDAS       : Entidade auxiliar e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação de veículos a contratos.
 *
 * 🔄 CHAMA        : NotMapped, SelectListItem.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations, Microsoft.AspNetCore.Mvc.Rendering.
 **************************************************************************************** */

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
    /****************************************************************************************
     * ⚡ VIEWMODEL: ItensContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Agrupar item de contrato e lista de contratos para UI.
     *
     * 📥 ENTRADAS     : ItensContrato e lista de contratos.
     *
     * 📤 SAÍDAS       : ViewModel para telas de associação.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de contratos.
     *
     * 🔄 CHAMA        : SelectListItem.
     ****************************************************************************************/
    public class ItensContratoViewModel
        {
        public Guid ContratoId { get; set; }
        public ItensContrato ItensContrato { get; set; }

        public IEnumerable<SelectListItem> ContratoList { get; set; }


    }

    /****************************************************************************************
     * ⚡ MODEL: ItensContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar item de contrato usado em operações de UI.
     *
     * 📥 ENTRADAS     : ContratoId (apenas na camada de apresentação).
     *
     * 📤 SAÍDAS       : Estrutura auxiliar para associação de contratos.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação de veículos a contratos.
     *
     * 🔄 CHAMA        : NotMapped.
     *
     * ⚠️ ATENÇÃO      : ContratoId é NotMapped (uso apenas na UI).
     ****************************************************************************************/
    public class ItensContrato
        {

        [NotMapped]
        public Guid ContratoId { get; set; }

        }
    }
