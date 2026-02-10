/* ****************************************************************************************
 * ⚡ ARQUIVO: OperadorContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Operador e Contrato.
 *
 * 📥 ENTRADAS     : Identificadores de operador e contrato.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação operador-contrato.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : Chave composta (OperadorId + ContratoId).
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FrotiX.Services;
using FrotiX.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
{
    /****************************************************************************************
     * ⚡ VIEWMODEL: OperadorContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar vínculo Operador-Contrato nas telas de edição.
     *
     * 📥 ENTRADAS     : OperadorId, ContratoId e entidade de vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de vínculo.
     ****************************************************************************************/
    public class OperadorContratoViewModel
    {
        // Identificador do operador.
        public Guid OperadorId { get; set; }

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Entidade do vínculo.
        public OperadorContrato? OperadorContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: OperadorContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Operador e Contrato.
     *
     * 📥 ENTRADAS     : OperadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação operador-contrato.
     *
     * 🔄 CHAMA        : Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta (OperadorId + ContratoId).
     ****************************************************************************************/
    public class OperadorContrato
    {
        // Chave composta - FK para Operador.
        [Key, Column(Order = 0)]
        public Guid OperadorId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
