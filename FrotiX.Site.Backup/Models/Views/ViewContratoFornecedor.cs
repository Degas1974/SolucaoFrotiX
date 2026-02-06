/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewContratoFornecedor.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de contratos com dados de fornecedor.
 *
 * 📥 ENTRADAS     : ContratoId, descrição e tipo de contrato.
 *
 * 📤 SAÍDAS       : DTO de leitura para relatórios e seleção.
 *
 * 🔗 CHAMADA POR  : Listagens de contratos e fornecedores.
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
     * ⚡ MODEL: ViewContratoFornecedor
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de contratos com fornecedor.
     *
     * 📥 ENTRADAS     : Identificador, descrição e tipo.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewContratoFornecedor
        {

        // Identificador do contrato.
        public Guid ContratoId { get; set; }

        // Descrição do contrato.
        public string? Descricao { get; set; }

        // Tipo de contrato.
        public string? TipoContrato { get; set; }

        }
    }
