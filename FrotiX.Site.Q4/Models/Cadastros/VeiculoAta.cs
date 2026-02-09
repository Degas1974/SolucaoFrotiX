/* ****************************************************************************************
 * ⚡ ARQUIVO: VeiculoAta.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Veículo e Ata de Preços via chave composta.
 *
 * 📥 ENTRADAS     : Identificadores de veículo e ata.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação veículo-ata.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations.
 *
 * ⚠️ ATENÇÃO      : Chave composta (VeiculoId + AtaId).
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
     * ⚡ VIEWMODEL: VeiculoAtaViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar vínculo veículo-ata nas telas de edição.
     *
     * 📥 ENTRADAS     : VeiculoId, AtaId e entidade de vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para UI.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de vínculo.
     ****************************************************************************************/
    public class VeiculoAtaViewModel
    {
        // Identificador do veículo.
        public Guid VeiculoId { get; set; }

        // Identificador da ata.
        public Guid AtaId { get; set; }

        // Entidade do vínculo.
        public VeiculoAta? VeiculoAta { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: VeiculoAta
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Veículo e Ata de Preços.
     *
     * 📥 ENTRADAS     : VeiculoId e AtaId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação veículo-ata.
     *
     * 🔄 CHAMA        : Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta (VeiculoId + AtaId).
     ****************************************************************************************/
    public class VeiculoAta
    {
        // Chave composta - FK para Veículo.
        [Key, Column(Order = 0)]
        public Guid VeiculoId { get; set; }

        // Chave composta - FK para Ata.
        [Key, Column(Order = 1)]
        public Guid AtaId { get; set; }
    }
}
