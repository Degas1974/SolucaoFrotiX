/* ****************************************************************************************
 * ⚡ ARQUIVO: LavadorContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear vínculo N:N entre Lavador e Contrato.
 *
 * 📥 ENTRADAS     : Identificadores de lavador e contrato.
 *
 * 📤 SAÍDAS       : Entidade de relacionamento e ViewModel para UI.
 *
 * 🔗 CHAMADA POR  : Fluxos de associação lavador-contrato.
 *
 * 🔄 CHAMA        : DataAnnotations, Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : Chave composta (LavadorId + ContratoId).
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
     * ⚡ VIEWMODEL: LavadorContratoViewModel
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Transportar dados do vínculo Lavador-Contrato na UI.
     *
     * 📥 ENTRADAS     : LavadorId, ContratoId e entidade de vínculo.
     *
     * 📤 SAÍDAS       : ViewModel para telas de associação.
     *
     * 🔗 CHAMADA POR  : Controllers/Views de vínculo.
     ****************************************************************************************/
    public class LavadorContratoViewModel
    {
        // Identificador do lavador selecionado.
        public Guid LavadorId { get; set; }

        // Identificador do contrato selecionado.
        public Guid ContratoId { get; set; }

        // Entidade que representa o vínculo persistido.
        public LavadorContrato LavadorContrato { get; set; }
    }

    /****************************************************************************************
     * ⚡ MODEL: LavadorContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar o relacionamento N:N entre Lavador e Contrato.
     *
     * 📥 ENTRADAS     : LavadorId e ContratoId.
     *
     * 📤 SAÍDAS       : Registro de vínculo persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de associação lavador-contrato.
     *
     * 🔄 CHAMA        : Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta (LavadorId + ContratoId).
     ****************************************************************************************/
    public class LavadorContrato
    {
        // Chave composta - FK para Lavador.
        [Key, Column(Order = 0)]
        public Guid LavadorId { get; set; }

        // Chave composta - FK para Contrato.
        [Key, Column(Order = 1)]
        public Guid ContratoId { get; set; }
    }
}
