// ╔══════════════════════════════════════════════════════════════════════════════╗
// ║ ARQUIVO: ControleAcessoDbContext.cs                                          ║
// ║ PROJETO: FrotiX - Sistema de Gestão de Frotas                                ║
// ╠══════════════════════════════════════════════════════════════════════════════╣
// ║ DESCRIÇÃO:                                                                   ║
// ║ Contexto dedicado ao sistema de controle de acesso granular por recursos.    ║
// ║ Gerencia permissões de usuários em menus/funcionalidades específicas.        ║
// ║                                                                              ║
// ║ TABELAS GERENCIADAS:                                                         ║
// ║ - Recurso: Árvore hierárquica de menus/funcionalidades (auto-relacionamento) ║
// ║ - ControleAcesso: Relação N:N entre usuários e recursos (chave composta)     ║
// ║                                                                              ║
// ║ REGRAS DE NEGÓCIO:                                                           ║
// ║ - Chave composta: ControleAcesso(UsuarioId, RecursoId)                       ║
// ║ - Hierarquia de recursos com ParentId (auto-relacionamento)                  ║
// ║ - DeleteBehavior.Restrict evita exclusão em cascata de recursos pai          ║
// ║ - Command timeout: 9000ms para operações complexas de permissão              ║
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
    /// Contexto de controle de acesso granular.
    /// Gerencia permissões de usuários por recurso/funcionalidade.
    /// </summary>
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


