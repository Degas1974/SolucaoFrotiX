// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ControleAcesso.cs                                                  ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                               ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Entidade de relacionamento N:N entre Usuário e Recurso para permissões.     ║
// ║ Define quais menus/funcionalidades cada usuário pode acessar.               ║
// ║                                                                              ║
// ║ PROPRIEDADES:                                                                ║
// ║ • UsuarioId [Key, Order=0] - ID do usuário (string, AspNetUsers.Id)         ║
// ║ • RecursoId [Key, Order=1] - ID do recurso/menu                             ║
// ║ • Acesso - Flag indicando se o acesso está liberado                         ║
// ║                                                                              ║
// ║ CHAVE COMPOSTA:                                                              ║
// ║ • (UsuarioId, RecursoId) - Chave primária composta                          ║
// ║                                                                              ║
// ║ RELACIONAMENTOS:                                                             ║
// ║ • Usuário → AspNetUsers                                                     ║
// ║ • Recurso → Recurso (menus/funcionalidades)                                 ║
// ║                                                                              ║
// ║ USO:                                                                          ║
// ║ • Tela de gestão de permissões de usuários                                  ║
// ║ • Verificação de acesso a menus no NavigationModel                          ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 18                                       ║
// ╚══════════════════════════════════════════════════════════════════════════════╝
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


