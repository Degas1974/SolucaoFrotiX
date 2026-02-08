/* ****************************************************************************************
 * ⚡ ARQUIVO: MotoristaContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Motorista e Contrato.
 *
 * 📥 ENTRADAS     : Identificadores de motorista e contrato.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação motorista-contrato.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : Chave composta (MotoristaId + ContratoId).
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
     * ⚡ VIEWMODEL: MotoristaoContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar o vínculo Motorista-Contrato nas telas de edição.
     *
     * 📥 ENTRADAS     : MotoristaId, ContratoId e entidade de vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de vínculo.
     ****************************************************************************************/
    public class MotoristaoContratoViewModel
    {
        // Identificador do motorista.
        public Guid MotoristaId { get; set; }

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Entidade do vínculo.
        public MotoristaContrato? MotoristaContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: MotoristaContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Motorista e Contrato.
     *
     * 📥 ENTRADAS     : MotoristaId e ContratoId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação motorista-contrato.
     *
     * 🔄 CHAMA        : Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta (MotoristaId + ContratoId).
     ****************************************************************************************/
    public class MotoristaContrato
    {
        // Chave composta - FK para Motorista.
        [Key, Column(Order = 0)]
        public Guid MotoristaId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
