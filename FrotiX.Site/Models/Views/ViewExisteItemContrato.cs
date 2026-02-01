/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewExisteItemContrato.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL para verificação de itens de contrato.
 *
 * 📥 ENTRADAS     : Identificadores e valores de itens.
 *
 * 📤 SAÍDAS       : DTO de leitura para validações.
 *
 * 🔗 CHAMADA POR  : Telas de itens de contrato.
 *
 * 🔄 CHAMA        : Não se aplica.
 *
 * 📦 DEPENDÊNCIAS : FrotiX.Services, FrotiX.Validations.
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
using Microsoft.AspNetCore.Http;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ MODEL: ViewExisteItemContrato
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de itens de contrato.
     *
     * 📥 ENTRADAS     : Item, quantidade e valores.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e validações.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewExisteItemContrato
        {

        // Identificador do item de veículo.
        public Guid ItemVeiculoId { get; set; }

        // Indicador de existência do veículo (GUID sentinel).
        public Guid ExisteVeiculo { get; set; }

        // Identificador da repactuação do contrato.
        public Guid RepactuacaoContratoId { get; set; }

        // Número do item.
        public int? NumItem { get; set; }

        // Descrição do item.
        public string? Descricao { get; set; }

        // Quantidade do item.
        public int? Quantidade { get; set; }

        // Valor unitário do item.
        public double? ValUnitario { get; set; }

        }
    }

