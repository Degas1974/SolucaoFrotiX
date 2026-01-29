/*
╔══════════════════════════════════════════════════════════════════════════════╗
║                    DOCUMENTACAO INTRA-CODIGO - FROTIX                        ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Arquivo    : ControleAcessoDbContext.cs                                      ║
║ Projeto    : FrotiX.Site                                                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DESCRICAO                                                                    ║
║ DbContext para controle de acesso (recursos e permissoes). Gerencia          ║
║ entidades Recurso e ControleAcesso com chaves compostas.                     ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ DBSETS                                                                       ║
║ - Recurso        : Recursos do sistema (menus, paginas, actions)             ║
║ - ControleAcesso : Permissoes usuario x recurso                              ║
╠══════════════════════════════════════════════════════════════════════════════╣
║ Data Documentacao: 28/01/2026                              LOTE: 21          ║
╚══════════════════════════════════════════════════════════════════════════════╝
*/

using System;
using System.Collections.Generic;
using System.Text;
using FrotiX.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FrotiX.Data
{
    public class ControleAcessoDbContext  : DbContext
    {
        public ControleAcessoDbContext(DbContextOptions<ControleAcessoDbContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(9000);
        }

        // Recurso para tabelas com múltiplas chaves primárias
        //====================================================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControleAcesso>().HasKey(ca => new { ca.UsuarioId, ca.RecursoId});

            // Configuração da hierarquia de Recursos (auto-relacionamento)
            modelBuilder.Entity<Recurso>()
                .HasOne(r => r.Parent)
                .WithMany(r => r.Children)
                .HasForeignKey(r => r.ParentId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata
        }


        public DbSet<Recurso> Recurso { get; set; }
        public DbSet<ControleAcesso> ControleAcesso { get; set; }

    }
}


