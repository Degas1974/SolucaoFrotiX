/* ****************************************************************************************
 * ⚡ ARQUIVO: ViewControleAcesso.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Mapear view SQL de controles de acesso (usuário x recurso).
 *
 * 📥 ENTRADAS     : Usuário, recurso e flags de acesso.
 *
 * 📤 SAÍDAS       : DTO de leitura para administração de acessos.
 *
 * 🔗 CHAMADA POR  : Consultas e telas de controle de acesso.
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
     * ⚡ MODEL: ViewControleAcesso
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar view SQL de controle de acesso.
     *
     * 📥 ENTRADAS     : Usuário, recurso e descrição.
     *
     * 📤 SAÍDAS       : Registro somente leitura.
     *
     * 🔗 CHAMADA POR  : Consultas e relatórios.
     *
     * 🔄 CHAMA        : Não se aplica.
     ****************************************************************************************/
    public class ViewControleAcesso
        {

        // Identificador do usuário.
        public string? UsuarioId { get; set; }

        // Identificador do recurso.
        public Guid RecursoId { get; set; }

        // Flag de acesso.
        public bool Acesso { get; set; }

        // Nome do recurso.
        public string? Nome { get; set; }

        // Descrição do recurso.
        public string? Descricao { get; set; }

        // Ordem de exibição.
        public double? Ordem { get; set; }

        // Nome completo do recurso.
        public string? NomeCompleto { get; set; }

        // Identificadores concatenados (uso interno).
        public string? IDS { get; set; }

        }
    }

