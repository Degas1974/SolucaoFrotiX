using System;
using System.Collections.Generic;
using System.Text;
using FrotiXApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FrotiXApi.Data
{
    public class ControleAcessoDbContext : DbContext
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
            modelBuilder.Entity<ControleAcesso>().HasKey(ca => new { ca.UsuarioId, ca.RecursoId });
        }

        public DbSet<Recurso> Recurso { get; set; }
        public DbSet<ControleAcesso> ControleAcesso { get; set; }
    }
}
