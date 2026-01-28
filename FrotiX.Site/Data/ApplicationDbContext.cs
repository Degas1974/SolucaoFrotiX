// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ApplicationDbContext.cs                                             ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Contexto de banco de dados para autenticação ASP.NET Identity.               ║
// ║ Herda de IdentityDbContext para gerenciar usuários, roles, claims, tokens.   ║
// ║                                                                              ║
// ║ RESPONSABILIDADE:                                                            ║
// ║ - Autenticação e autorização via ASP.NET Identity                            ║
// ║ - Tabelas: AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, etc. ║
// ║                                                                              ║
// ║ RELACIONAMENTO:                                                              ║
// ║ - Usado em conjunto com FrotiXDbContext (dados de negócio)                   ║
// ║ - Usado em conjunto com ControleAcessoDbContext (permissões por recurso)     ║
// ║                                                                              ║
// ║ DOCUMENTADO EM: 2026-01-28 | LOTE: 11                                        ║
// ╚══════════════════════════════════════════════════════════════════════════════╝

using System;
using System.Collections.Generic;
using System.Text;
using FrotiX.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    /// <summary>
    /// Contexto de autenticação ASP.NET Identity.
    /// Gerencia usuários, roles e claims do sistema.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
