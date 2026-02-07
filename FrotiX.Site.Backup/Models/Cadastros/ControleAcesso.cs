/* ****************************************************************************************
 * ⚡ ARQUIVO: ControleAcesso.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Modelar permissões de usuários para recursos do sistema.
 *
 * 📥 ENTRADAS     : Identificadores de usuário e recurso.
 *
 * 📤 SAÍDAS       : Registro de acesso (permitido/negado).
 *
 * 🔗 CHAMADA POR  : Fluxos de autorização e gestão de acesso.
 *
 * 🔄 CHAMA        : DataAnnotations e Column(Order).
 *
 * 📦 DEPENDÊNCIAS : System.ComponentModel.DataAnnotations, Microsoft.EntityFrameworkCore.
 *
 * ⚠️ ATENÇÃO      : Chave composta: UsuarioId + RecursoId.
 **************************************************************************************** */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrotiX.Models
    {
    /****************************************************************************************
     * ⚡ MODEL: ControleAcesso
     * --------------------------------------------------------------------------------------
     * 🎯 OBJETIVO     : Representar permissões de acesso de usuários a recursos.
     *
     * 📥 ENTRADAS     : UsuarioId, RecursoId e flag de acesso.
     *
     * 📤 SAÍDAS       : Registro de permissão persistido.
     *
     * 🔗 CHAMADA POR  : Fluxos de autorização.
     *
     * 🔄 CHAMA        : DataAnnotations, Column(Order).
     *
     * ⚠️ ATENÇÃO      : Chave composta: UsuarioId + RecursoId.
     ****************************************************************************************/
    public class ControleAcesso
        {
        [Key, Column(Order = 0)]
        public String UsuarioId { get; set; }

        [Key, Column(Order = 1)]
        public Guid RecursoId { get; set; }

        public bool Acesso { get; set; }

        }
    }
