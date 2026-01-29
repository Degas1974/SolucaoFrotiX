/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: ControleAcesso.cs                                                                       ║
   ║ 📂 CAMINHO: /Models/Cadastros                                                                       ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Entidade para controle de acesso/permissões de usuários a recursos do sistema.        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ENTIDADE: ControleAcesso (UsuarioId, RecursoId, Acesso) - chave composta                        ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🔗 DEPS: Microsoft.AspNetCore.Identity, System.ComponentModel.DataAnnotations                       ║
   ║ 📅 Atualizado: 2026 | 👤 FrotiX Team | 📝 Versão: 2.0                                              ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝ */

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
    public class ControleAcesso
        {
        [Key, Column(Order = 0)]
        public String UsuarioId { get; set; }

        [Key, Column(Order = 1)]
        public Guid RecursoId { get; set; }

        public bool Acesso { get; set; }

        }
    }


