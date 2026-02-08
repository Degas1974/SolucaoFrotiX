/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculoContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Veículo e Contrato via chave composta.
 *
 * 📥 ENTRADAS     : Identificadores de veículo e contrato.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação veículo-contrato.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 *
 * ⚠️ ATENÇÃO      : Chave composta (VeiculoId + ContratoId).
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
     * ⚡ VIEWMODEL: VeiculoContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar vínculo veículo-contrato nas telas de edição.
     *
     * 📥 ENTRADAS     : VeiculoId, ContratoId e entidade de vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de vínculo.
     ****************************************************************************************/
    public class VeiculoContratoViewModel
    {
        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Entidade do vínculo.
        public VeiculoContrato? VeiculoContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: VeiculoContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Veículo e Contrato.
     *
     * 📥 ENTRADAS     : VeiculoId e ContratoId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação veículo-contrato.
     *
     * 🔄 CHAMA        : Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta (VeiculoId + ContratoId).
     ****************************************************************************************/
    public class VeiculoContrato
    {
        // Chave composta - FK para Veículo.
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
