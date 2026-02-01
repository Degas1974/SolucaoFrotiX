/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewAtaFornecedor.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de atas com dados de fornecedor.
 *
 * 📥 ENTRADAS     : AtaId e descrição da ata.
 *
 * 📤 SAÍDAS       : DTO de leitura para seleção e relatórios.
 *
 * 🔗 CHAMADA POR  : Listagens de atas e fornecedores.
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
     * ⚡ MODEL: ViewAtaFornecedor
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de atas com fornecedor.
     *
     * 📥 ENTRADAS     : Identificador e descrição da ata.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewAtaFornecedor
        {

        // Identificador da ata.
        public Guid AtaId { get; set; }

        // Descrição da ata/veículo.
        public string? AtaVeiculo { get; set; }

        }
    }

